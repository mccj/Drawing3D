using System;
using System.ComponentModel;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace Drawing3d
{
    /// <summary>
    /// Kind of the shader. On several computers the large shader does'nt works.
    /// </summary>
    public  enum Shaders
    {
        /// <summary>
        /// no shader
        /// </summary>
        None,
        /// <summary>
        /// Small shader
        /// </summary>
        SmallShader,
        /// <summary>
        /// Large shader
        /// </summary>
        LargeShader
    }
    public partial class OpenGlDevice
    {
       

       // Shaders _ShaderKind = Shaders.None;


   
        [BrowsableAttribute(false)]
        [NonSerialized]
        private OpenTK.Graphics.GraphicsContext RenderingContext = null;
        [BrowsableAttribute(false)]
        [NonSerialized]
        private OpenTK.Platform.IWindowInfo WindowsInfo = null;
        private Color _BackColor = Color.Black;
        /// <summary>
        /// sets and gets the back color of the <see cref="WinControl"/>
        /// </summary>
        public Color BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; }
        }

        private void MakeCurrent()
        {
            if (RenderingContext != null)
            {
                if (!RenderingContext.IsCurrent)
                    try
                    {
                        RenderingContext.MakeCurrent(WindowsInfo);
                        //if (Shader != null)
                        //    Shader.UpdateUniforms(this);
                    }
                    catch (Exception)
                    {
                    }
            }
        }

        private void ClearScreen()
        {
            GL.ClearColor(BackColor);
            GL.Clear((ClearBufferMask)((int)(ClearBufferMask.ColorBufferBit) + (int)(ClearBufferMask.DepthBufferBit)));
        }

        private void SwapBuffers()
        {
            if (RenderingContext != null)
                RenderingContext.SwapBuffers();
        }

        private void DisposeOglObjects()
        {
            BackGroundShader.Dispose();
            if (Selector.PickingShader != null)
            {
                Selector.PickingShader.Dispose();
                Selector.PickingShader = null;

            }
            
            Selector.PickingShader = null;
         
            RenderingContext.Dispose();
            RenderingContext = null;
        }

        private void InitializeOgl(Control WinControl)
        {

            if (RenderingContext != null)

                if (!RenderingContext.IsDisposed)
                {
                    DisposeOglObjects();
                }


            /* private OpenTK.Platform.IWindowInfo*/
            WindowsInfo = OpenTK.Platform.Utilities.CreateWindowsWindowInfo(WinControl.Handle);

            OpenTK.Graphics.GraphicsMode GraphicMode = new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(PfdColorBits), PfdDepthBits, PfdStencilBits, PfdAuxBits, new OpenTK.Graphics.ColorFormat(PfdAccumBits), PfdBuffers, PfdStereo);

            OpenTK.Graphics.GraphicsMode GraphicMode2 = OpenTK.Graphics.GraphicsMode.Default;
             RenderingContext = new OpenTK.Graphics.GraphicsContext(GraphicMode2, WindowsInfo);
            RenderingContext.LoadAll();
            FBO.LoadMethods();
            RenderingContext.SwapInterval = 1;
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
           
            ShadowMapSampler = GL.GenSampler();
          //  Texture.Sampler= GL.GenSampler();
            ActivateShader();
            Lights[0].Position = new xyzwf(3, 4, 10, 0);
            FieldOfView = 0;
            Selector.PickingShader = PickingShader;
            //System.Diagnostics.ConsoleTraceListener myWriter = new
            //System.Diagnostics.ConsoleTraceListener();
            
            //System.Diagnostics.Debug.Listeners.Clear();
            //System.Diagnostics.Debug.Listeners.Add(myWriter);
            CheckError();
            
         
            ClockWise = true;
            Culling = false;
       
            SwapBuffers();
            CheckError();
        }
    }
    }
