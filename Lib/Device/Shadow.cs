using System;
using OpenTK.Graphics.OpenGL;
namespace Drawing3d
{

    public partial class OpenGlDevice
    {
        /// <summary>
        /// describes the settings of the use of shadow.
        /// </summary>
        [Serializable]

        public class ShadowSettings
        {
            OpenGlDevice Device = null;
            /// <summary>
            /// constructor of <see cref="ShadowSettings"/> which has a <see cref="OpenGlDevice"/>.
            /// </summary>
            /// <param name="Device">the <see cref="OpenGlDevice"/> in which will be drawn.</param>
            public ShadowSettings(OpenGlDevice Device)
            { this.Device = Device; }
            /// <summary>
            /// a vector from the light to the scene.
            /// </summary>
            public xyz LookFromLightTo = new xyz(0, 0, 0);
            /// <summary>
            /// near clipping. See also <see cref="OpenGlDevice.NearClipping"/>.
            /// </summary>
            public double NearClipping = 1;
            /// <summary>
            /// fae clipping. See also <see cref="OpenGlDevice.FarClipping"/>.
            /// </summary>
            public double FarClipping = 10000;
          
            /// <summary>
            /// gets and sets the darkness of the shadows. the value is between 0 and 1.
            /// 0.. no shadow, 1 .. black shadow. Default is 0.3;
            /// </summary>
            public double DarknessPercentage
            {
                get { return _DarknessPercentage; }
                set
                {
                    _DarknessPercentage = value;
                   
                    if (Device.RenderKind == RenderKind.Render)
                    {Field I= Device.Shader.getvar("ShadowIntensity");
                        if (I != null) I.Update();


                    }

                }
            }
            int _Samplingcount = 1;
            /// <summary>
            /// to get a smooth border of the shadow the device plays the shadow samplingount times.
            /// The default is 1.
            /// </summary>
            public int Samplingcount
            {
                get { return _Samplingcount; }
                set
                {
                    _Samplingcount = value;
                    if (Device.Shader != null)
                    {
                        Field C = Device.Shader.getvar("SamplingCount");
                        if (C != null) C.Update();
                    }
                }
            }
            float _Smoothwith = 1f;
            /// <summary>
            /// gets and sets the width of the smoothing border. The default is 1.
            /// </summary>
            public float Smoothwidth
            {
              get { return _Smoothwith; }
              set { _Smoothwith = value;

                    Field F = Device.Shader.getvar("Smoothwidth");
                    if (F != null)
                        F.SetValue(Smoothwidth);
                  }
            }
            double _DarknessPercentage = 0.3;
            /// <summary>
            /// gets and sets width for the lightprojectionmatrix.
            /// </summary>
            public int Width
            {
                get { return _Width; }
                set
                {
                    _Width = value;

                }
            }

            int _Width = 1024;
            /// <summary>
            /// gets and sets height for the lightprojectionmatrix.
            /// </summary>
            public int Height
            {
                get { return _Height; }
                set
                {
                    _Height = value;
                }
            }

            int _Height = 1024;
            xyz _Upvector = new xyz(0, 1, 0);
            /// <summary>
            /// gets and sets the Upvector for the lightprojectionmatrix.
            /// </summary>
            public xyz Upvector { get { return _Upvector; } set { _Upvector = value; } }
        }
        bool _Shadow = false;
        /// <summary>
        /// If you set this property <b>true</b> the shadow will be drawn.
        /// </summary>
        public bool Shadow
        {
            get { return _Shadow; }
            set
            {
                _Shadow = value;
                if (Shader != null)
                {
                    Field SE = Shader.getvar("ShadowEnable");
                    if (SE != null) SE.Update();
                }
                ShadowDirty = true;
            }
        }
        bool _ShadowDirty = false;
        /// <summary>
        /// set this property true, if the shadow has been changed.
        /// </summary>
        public bool ShadowDirty
        {
            get { return _ShadowDirty; }
            set
            {
                _ShadowDirty = value;

            }
        }
        /// <summary>
        /// with these settings the shadow will be calculated. See <see cref="ShadowSettings"/>
        /// </summary>
        public ShadowSettings ShadowSetting = null;
        FBO ShadowFBO = new FBO();
        /// <summary>
        /// gives the projection matrix for a look from the light to the scene (<see cref="ShadowSettings.LookFromLightTo"/>
        /// </summary>
        /// <returns>the projection matrix, which is used to calculate the shadow.</returns>
        public Matrix GetLightProjectionMatrix()
        {
            Matrix LightProj = Matrix.identity;
            double left = -(ShadowSetting.Width) / 2 / PixelsPerUnit;
            double right = (ShadowSetting.Width) / 2 / PixelsPerUnit;
            double top = ShadowSetting.Width / 2 / PixelsPerUnit;
            double bottom = -ShadowSetting.Width / 2 / PixelsPerUnit;
            double near = ShadowSetting.NearClipping;
            double far = ShadowSetting.FarClipping;
            LightProj = Matrix.Orthogonal(left, right, bottom, top, near, far);
            xyz LP = Lights[0].Position.ToXYZ();
            xyz LD = ShadowSetting.LookFromLightTo - Lights[0].Position.ToXYZ();
            return LightProj * Matrix.LookAt(LP, LP + LD, ShadowSetting.Upvector);
        }

        const int GL_CLAMP = 0x00002900;
        bool FShadow = false;
        /// <summary>
        /// The private Settermethod for the <see cref="Shadow"/>-property.
        /// </summary>
        /// <param name="value"></param>
        private void setShadow(bool value)
        {
            if (value)
            {
                if (this is OpenGlDevice)
                {
                    if (Shader != null)
                    {
                        Field SE = Shader.getvar("ShadowEnable");
                        if (SE != null) SE.Update();
                    }
                   
                }
                FShadow = value;
                ResetShadow();
            }
            else
                if (this is OpenGlDevice)
            {
                Shader = null;
            }

            FShadow = value;

     

        }
        /// <summary>
        /// is the number of the shadow sampler
        /// </summary>
        internal int ShadowMapSampler =2;
        /// <summary>
        /// internal.
        /// </summary>
        internal int Texture0Sampler = 0;
       
        /// <summary>
        /// calculates a new shadow. 
        /// </summary>
      public void ResetShadow()
        {
           
            ShadowDirty = false;
            if (!Shadow) return;
            GLShader SaveShader = this.Shader;
           
            RenderKind = RenderKind.Render;
            Matrix SaveP = ProjectionMatrix;
            Texture SaveTexture = texture;
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(1, 0);
            Shader = ShadowDepthShader;
            ProjectionMatrix = GetLightProjectionMatrix();
            ShadowFBO.EnableWriting();
            OnPaint();
            ShadowFBO.DisableWriting();
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, ShadowFBO.FboTexture);
            ProjectionMatrix = SaveP;
            Shader = SaveShader;
            GL.ActiveTexture(TextureUnit.Texture0);
        

        }
    }
}

