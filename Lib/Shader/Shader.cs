using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// is a structure of float fields, which are used from <see cref="GLShader"/>.
    /// </summary>
    [Serializable]
    public struct xyzwf
    {
        /// <summary>
        /// the x coordinate
        /// </summary>
        public float x;
        /// <summary>
        /// the y coordinate
        /// </summary>
        public float y;
        /// <summary>
        /// the z coordinate
        /// </summary>
        public float z;
        /// <summary>
        /// the w coordinate
        /// </summary>
        public float w;
        /// <summary>
        /// constructor with x,y,z,w
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public xyzwf(float x,float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        ///is a constructor with a <b>color</b>.
        /// </summary>
        /// <param name="Color"></param>
        public xyzwf(Color Color)
        { 
            this.x = (float)Color.R / 255;
            this.y = (float)Color.G / 255;
            this.z = (float)Color.B / 255;
            this.w = (float)Color.A / 255;
        }
        /// <summary>
        /// converts the struct to a <see cref="xyz"/> by omitting the w coordinate.
        /// </summary>
        /// <returns></returns>
        public xyz ToXYZ()
        {
            return new xyz(x, y, z);
        }
        /// <summary>
        /// converts the struct to a string.
        /// </summary>
        /// <returns>a string.</returns>
        public override string ToString()
        {
            return Utils.FloatToStr(x) + Utils.Delimiter + Utils.FloatToStr(y) + Utils.Delimiter + Utils.FloatToStr(z)+ Utils.Delimiter + Utils.FloatToStr(w);
        }
    }
  
    public partial class OpenGlDevice
    {
        GLShader _TesselationtShader = null;
        /// <summary>
        /// is the tesselation shader.
        /// </summary>

        public GLShader TesselationtShader
        {
            get
            {

                if (_TesselationtShader == null)
                    try
                    {
                        _TesselationtShader = new GLShader(SmallFragment, TessVertexShader, "", TessControlShader, TessEvaluationShader, this);
                    }
                    catch (Exception E)
                    {

                        MessageBox.Show(E.Message);
                        System.Environment.Exit(99);

                    }
                return _TesselationtShader;
            }
        }

        GLShader _SmallShader = null;
        /// <summary>
        /// is the small shader, which is used from <see cref="OpenGlDevice"/> for drawing.
        /// </summary>
        public GLShader SmallShader
        {
            get
            {

                if (_SmallShader == null)
                    try
                    {
                        _SmallShader = new GLShader(SmallFragment, SmallVertex, this);
                    }
                    catch (Exception E)
                    {

                        MessageBox.Show(E.Message);
                        System.Environment.Exit(99);

                    }

                return _SmallShader;
            }
            set
            {
                if (_SmallShader != null) _SmallShader.Dispose();
                _SmallShader = value;
            }
        }
        GLShader _LargeSchader = null;
        /// <summary>
        /// is the big shader, which is used from <see cref="OpenGlDevice"/> for drawing.
        /// </summary>
        public GLShader LargeShader
        {
            get
            {

                if (_LargeSchader == null)
                    try
                    {
                        _LargeSchader = new GLShader(LargeFragment, LargeVertex, this);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Graphic card to small!");
                        System.Environment.Exit(99);

                    }
              
                return _LargeSchader;
            }
            set
            {
                if (_LargeSchader != null) _LargeSchader.Dispose();
                _LargeSchader = value;
            }
        }
        /// <summary>
        /// the depth shader used to produce a <see cref="OpenGlDevice.Shadow"/>.
        /// </summary>
        public GLShader ShadowDepthShader = null;
        /// <summary>
        /// is a shader, which is used by <see cref="OpenGlDevice.OnForegroundPaint"/>.
        /// </summary>
        public GLShader BackGroundShader = null;
        /// <summary>
        /// is a shader, which writes the <see cref="PrimIdObj"/>.
        /// </summary>
        public GLShader PickingShader = null;
        void ActivateShader()
        {   
           
            ShadowDepthShader = new GLShader(OpenGlDevice.DepthFragment, OpenGlDevice.DepthVertex, this);
            BackGroundShader = new GLShader(BackGroundFragment, BackGroundVertex, this);
            PickingShader = new GLShader(Selector.PickingFragmentShader, Selector.PickingVertexShader, this);

         }

   
    }
    /// <summary>
    /// holds the handle of a <see cref="Field"/>.
    /// </summary>
    public class HandleContainer
    {
        /// <summary>
        /// is the constructor with the <b>handle</b>
        /// </summary>
        /// <param name="Handle">the handle of the <see cref="Field"/></param>
        public HandleContainer(int Handle)
        { this.handle = Handle; }
        /// <summary>
        /// is the handle of the <see cref="Field"/>.
        /// </summary>
        public int handle = -1;
    }
    /// <summary>
    /// overrides the <see cref="OGLShader"/> and has the fields <b>Position</b>, <b>Normal</b>, <b>Texture</b> and <b>Color</b>.
    /// These are used from <see cref="Primitives3d.drawTriangles(OpenGlDevice, int[], xyzf[], xyzf[], xyf[], xyzf[])"/>.
    /// </summary>
    [Serializable]
    public partial class GLShader:OGLShader
    { 
        /// <summary>
        /// holds the handle of the <b>Postion</b>
        /// </summary>
        public HandleContainer Position = new HandleContainer(-1);
        /// <summary>
        /// holds the handle of the <b>Normals</b>
        /// </summary>
        public HandleContainer Normal = new HandleContainer(-1);
        /// <summary>
        /// holds the handle of the <b>Texture</b>
        /// </summary>
        public HandleContainer Texture = new HandleContainer(-1);
        /// <summary>
        /// holds the handle of the <b>Color</b>
        /// </summary>
        public HandleContainer Color = new HandleContainer(-1);

        /// <summary>
        /// is the constructor with a <b>fragmentshader</b> and a <b>vertexshader</b>.
        /// </summary>
        /// <param name="FragmentShader">is the fragmentshader.</param>
        /// <param name="VertexShader">is the vertexshader.</param>
        /// <param name="UpdateData">holds the value for updating the fields of the shader.</param>
        public GLShader(string FragmentShader,string VertexShader,object UpdateData) :this(FragmentShader, VertexShader, "", "", "", UpdateData) 
        {
            
        }

        /// <summary>
        /// is the constructor with a <b>fragmentshader</b> a <b>vertexshader</b> and the <b>Updatedata</b>
        /// It sets the update events for the <see cref="OGLShader"/>.
        /// </summary>
        /// <param name="FragmentShader">is the fragment shader.</param>
        /// <param name="VertexShader">is the vertex shader.</param>
        /// <param name="GeometryShader">is the geometry shader.</param>
        /// <param name="TessControlShader">is the tesscontrol shader.</param>
        /// <param name="TessEvaluationShader">is the tessevaluation shader.</param>
        /// <param name="UpdateData">are the updatedata. It is the <see cref="OpenGlDevice"/></param>
        public GLShader(string FragmentShader, string VertexShader,string GeometryShader, string TessControlShader, string TessEvaluationShader, Object UpdateData) : base(FragmentShader, VertexShader, GeometryShader,  TessControlShader, TessEvaluationShader, UpdateData)
        {
           

            OpenGlDevice Device = UpdateData as OpenGlDevice;
            if (Device == null) return;
            while (Device.Lights.Count < 16)
                Device.Lights.Add(new Light());
            Device.Lights[0].Enable = 1;
            MakeLightEvents(Device);
            Field P = getvar("Position");
            // sets the sets the handle to the handle container.
            if (P != null) Position.handle = P.handle;
            Field N = getvar("Normal");
            if (N != null) Normal.handle = N.handle;
            Field T = getvar("Texture");
            if (T != null) Texture.handle = T.handle;
            Field C = getvar("Color");
            if (C != null) Color.handle = C.handle;
            C = getvar("TheObject");
            C = getvar("LightCount");
            if (C != null) C.DoUpdate += LighCountUpdate;
            C = getvar("ConstantAttenuation");
            if (C != null) C.DoUpdate += ConstantUpdate;
            C = getvar("LinearAttenuation");
            if (C != null) C.DoUpdate += LinearUpdate;
            C = getvar("QuadraticAttenuation");
            if (C != null) C.DoUpdate += QuadratricUpdate;
            C = getvar("Smoothwidth");
           if (C != null) C.DoUpdate += SmoothWidthUpdate;
            C = getvar("SamplingCount");
            if (C != null) C.DoUpdate += SamplingCountUpdate;
            C = getvar("Darkness");
            if (C != null) C.DoUpdate += DarknessUpdate;
            C = getvar("ShadowEnable");
            if (C != null) C.DoUpdate += ShadowEnablepdate;
            C = getvar("ShadowIntensity");
            if (C != null) C.DoUpdate += ShadowIntensityUpdate;
            C = getvar("ShadowFrameBufferSize");
            if (C != null) C.DoUpdate += ShadowFrameBufferSizeUpdate;
            C = getvar("GradientEnable");
            if (C != null) C.DoUpdate += GradientEnableUpdate;
            C = getvar("GradA");
            if (C != null) C.DoUpdate += GradAUpdate;
            C = getvar("GradB");
            if (C != null) C.DoUpdate += GradBUpdate;
            C = getvar("ColorA");
            if (C != null) C.DoUpdate += ColorAUpdate;
            C = getvar("ColorB");
            if (C != null) C.DoUpdate += ColorBUpdate;
            C = getvar("MpColor");
            if (C != null) C.DoUpdate += MpColorUpdate;
            C = getvar("GradSteps");
            if (C != null) C.DoUpdate += GradStepsUpdate;
            C = getvar("ClippingPlane[0]");
            if (C != null) C.DoUpdate += ClippingPlaneUpdate;
            C = getvar("ClippingPlaneEnabled[0]");
            if (C != null) C.DoUpdate += ClippingPlaneEnabledUpdate; ;
            C = getvar("ModelMatrix");
            if (C != null) C.DoUpdate += ModelMatrixUpdate; ;
            C = getvar("ProjectionMatrix");
            if (C != null) C.DoUpdate += ProjectionMatrixUpdate;
            C = getvar("Texture0Matrix");
            if (C != null) C.DoUpdate += TextureMatrixUpdate;
            C = getvar("FromLight");
            if (C != null) C.DoUpdate += FromLightUpdate; ;
            C = getvar("Filter");
            if (C != null) C.DoUpdate += FilterUpdate;
            C = getvar("ShadowMap");
            if (C != null) C.DoUpdate += ShadowMapUpdate; ;
            C = getvar("Ambient");
            if (C != null) C.DoUpdate += AmbientUpdate; ;
            C = getvar("Emission");
            if (C != null) C.DoUpdate += EmissionUpdate; ;
            C = getvar("Specular");
            if (C != null) C.DoUpdate += SpecularUpdate; ;
            C = getvar("Diffuse");
            if (C != null) C.DoUpdate += DiffuseUpdate;
            C = getvar("Translucent");
            if (C != null) C.DoUpdate += TranslucentUpdate; ;
            C = getvar("Shininess");
            if (C != null) C.DoUpdate += ShininessUpdate; ;
            C = getvar("LightEnabled");
            if (C != null) C.DoUpdate += LightEnabledUpdate; ;
            C = getvar("Light0Enabled");
            if (C != null) C.DoUpdate += Light0EnabledUpdate; 
            C = getvar("LightCount");
            if (C != null) C.DoUpdate += LightCountUpdate;
            C = getvar("Light0Diffuse");
            if (C != null) C.DoUpdate += Light0DiffuseUpdate;
            C = getvar("Light0Ambient");
            if (C != null) C.DoUpdate += Light0AmbientUpdate; ;
            C = getvar("Light0Specular");
            if (C != null) C.DoUpdate += Light0SpecularUpdate;
            C = getvar("Light0Position");
            if (C != null) C.DoUpdate += Light0PositionUpdate;
            C = getvar("Light0Direction");
            if (C != null) C.DoUpdate += Light0DirectionUpdate;
            C = getvar("Texture0_");
            if (C != null) C.DoUpdate += Texture0Update; ;
            C = getvar("Texture0Enabled");
            if (C != null) C.DoUpdate += Texture0EnabledUpdate; ;
            C = getvar("Texture0Matrix");
            if (C != null) C.DoUpdate += Texture0MatrixUpdate;
            C = getvar("Lined");
            if (C != null) C.DoUpdate += LinedUpdate; ;
            C = getvar("Color");
            if (C != null) C.DoUpdate += ColorUpdate; ;
            C = getvar("BIAS");
            if (C != null) C.DoUpdate += BIASUpdate;
            C = getvar("FilterEnable");
            if (C != null) C.DoUpdate += FilterEnableUpdate; ;
            C = getvar("WindowWidth");
            if (C != null) C.DoUpdate += WindowWidhtUpdate;
            C = getvar("WindowHeight");
            if (C != null) C.DoUpdate += WindowHeightUpdate;
        }

        private void AmbientField_DoUpdate(Field sender, object UpdateObject)
        {
            throw new NotImplementedException();
        }

        private void LighCountUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
          //  sender.SetValue(Device.Lights.Count);
            sender.SetValue(1);
        }

        private void ConstantUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
             sender.SetValue((float)Device.Lights[0].ConstantAttenuation);
            
        }

        private void LinearUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.Lights[0].LinearAttenuation);
        }

        private void QuadratricUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.Lights[0].QuadraticAttenuation);
        }

        private void WindowHeightUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ViewPort.Width);
        }

        private void WindowWidhtUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ViewPort.Width);
        }

        private void SmoothWithUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
           
            sender.SetValue(Device.ShadowSetting.Smoothwidth);
        }

        private void FilterEnableUpdate(Field sender, object UpdateObject)
        {
        }

        private void BIASUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            float b = (float)(3 / Device.FarClipping / 10);
            sender.SetValue(b);
           
        }

       

        private void ColorUpdate(Field sender, object UpdateObject)
        {
            
        }

        private void LinedUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            if (Device.PolygonMode == PolygonMode.Line)
                sender.SetValue((int)1);
            else
                sender.SetValue((int)0);
        }

        private void Texture0MatrixUpdate(Field sender, object UpdateObject)
        {
            //OpenGlDevice Device = UpdateObject as OpenGlDevice;
            //sender.SetMatrix(Device.TextureMatrix);
            
        }

        private void Texture0EnabledUpdate(Field sender, object UpdateObject)
        {
            //OpenGlDevice Device = UpdateObject as OpenGlDevice;
            //if (Device.texture == null)
            //    sender.SetValue((int)0);
            //else
            //    sender.SetValue((int)1);
        }

        private void Texture0Update(Field sender, object UpdateObject)
        {
            //OpenGlDevice Device = UpdateObject as OpenGlDevice;
            //sender.SetValue(Device.Texture0Sampler);
           
        }

        private void Light0DirectionUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            xyz R = Device.Lights[0].Direction;
            float[] P = new float[]

            {
                    (float)R.x,
                    (float)R.y,
                    (float)R.z,
                    1f

            };
            sender.SetValue(P);
          
        }

        private void Light0PositionUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            xyz R = Device.Lights[0].Position.ToXYZ();
            float[] P = new float[]

            {
                    (float)R.x,
                    (float)R.y,
                    (float)R.z,
                    1f

            };
            sender.SetValue(P);

           
        }

        private void Light0SpecularUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Specular);
            
        }

        private void Light0AmbientUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Ambient);

        }

        private void Light0DiffuseUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Diffuse);
            
        }

        private void LightCountUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights.Count);
           
        }

        private void LightEnabledUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            if (Device.LightEnabled)
                sender.SetValue((int)1);
            else
                sender.SetValue((int)0);
            
        }
        private void Light0EnabledUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            if (Device.LightEnabled)
                sender.SetValue((int)1);
            else
                sender.SetValue((int)0);

        }
        private void ShininessUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.Shininess);
           
        }

        private void TranslucentUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.Translucent);
        }

        private void DiffuseUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Diffuse);
            
        }

        private void SpecularUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Specular);
            
        }

        private void EmissionUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Emission);
            
        }

        private void AmbientUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Ambient);
          
        }

        private void ShadowMapUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ShadowMapSampler);
             
        }

        private void FilterUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(3);
               
        }

        private void FromLightUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.GetLightProjectionMatrix());

        }

        private void TextureMatrixUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.TextureMatrix);
           
        }

        private void ProjectionMatrixUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ProjectionMatrix);
            
        }

        private void ModelMatrixUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ModelMatrix);
            
        }

        private void ClippingPlaneEnabledUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(6, Device.ClippEnabled);
           
        }

        private void ClippingPlaneUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            float[] CP = Device.getClippingPlanes();
            sender.SetValue(CP.Length, CP);
            Field E = getvar("ClippingPlaneEnabled[0]");
            if (E!=null)
            E.Update();

        }
                   
    

        private void MpColorUpdate(Field sender, object UpdateObject)
        {
           
        }

        private void GradStepsUpdate(Field sender, object UpdateObject)
        {
           
        }

        private void GradStepCounUpdate(Field sender, object UpdateObject)
        {
            
        }

        private void ColorBUpdate(Field sender, object UpdateObject)
        {
           
        }

        private void ColorAUpdate(Field sender, object UpdateObject)
        {
            
        }

        private void GradBUpdate(Field sender, object UpdateObject)
        {
            
        }

        private void GradAUpdate(Field sender, object UpdateObject)
        {
            
        }

        private void GradientEnableUpdate(Field sender, object UpdateObject)
        {
          
        }

        private void ShadowFrameBufferSizeUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ShadowSetting.Width);
           
        }

        private void ShadowIntensityUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.ShadowSetting.DarknessPercentage);
           

        }

        private void ShadowEnablepdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            if (Device.Shadow)
                sender.SetValue((int)1);
            else
                sender.SetValue((int)0);
           
        }

        private void DarknessUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue((float)Device.ShadowSetting.DarknessPercentage);
           

        }

        private void SamplingCountUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ShadowSetting.Samplingcount);
           
        }

        private void SmoothWidthUpdate(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.ShadowSetting.Smoothwidth);
            
        }

       
      
      
      
        
        
    }
  
}