using System;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Drawing;

#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d   
{

    /// <summary>
    /// indicates, what the device will be do.
    /// </summary>
    public enum RenderKind
    {

        /// <summary>
        /// shoud'nt appear. Its default.
        /// </summary>
        None,
        /// <summary>
        /// the device render the scene.
        /// </summary>
        Render,
        /// <summary>
        /// the device draw to the <see cref="Selector.SnappList"/>
        /// </summary>
        SnapBuffer,
        /// <summary>
        /// the device want a maximal box.
        /// </summary>
        MaxBox
    };
    /// <summary>
    /// The enum is used by <see cref="OpenGlDevice.PenStyle"/>
    /// </summary>
    [Serializable]
    public enum PenStyles
    {
        /// <summary>
        /// Full line
        /// </summary>
        Full,
        /// <summary>
        /// Short lines
        /// </summary>
        Dash,
        /// <summary>
        /// Pointed line
        /// </summary>
        Dot,
        /// <summary>
        /// One point and one short line
        /// </summary>
        DashDot,
        /// <summary>
        /// A short line and two points
        /// </summary>
        DashDotDot
    }
    /// <summary>
    /// describes the kind of drawing.
    /// </summary>
    public enum PolygonMode
    {
        /// <summary>
        /// will be drawn as point.
        /// </summary>
        Point = 6912,
        /// <summary>
        /// will be drawn as line.
        /// </summary>
        Line = 6913,
        /// <summary>
        /// draws filled.
        /// </summary>
        Fill = 6914,
    }
    /// <summary>
    /// is the most important class. All inputs and outputs are made for his.
    /// It has a lot of functions, metods, properties, fields and more
    /// </summary>
    public partial class OpenGlDevice
    {

        private D3DCursors D3DCursor = new D3DCursors();
        private static byte PfdAccumBits = 64;                                         // Accumulation buffer bits
        private static byte PfdColorBits = 64;                                        // Color buffer bits
        private static byte PfdDepthBits = 24;                                        // Depth buffer bits
        private static byte PfdStencilBits = 0;                                       // Stencil buffer bits
        private static byte PfdBuffers = 2;
        private static byte PfdAuxBits = 0;
        private static bool PfdStereo = false;

        private static List<OpenGlDevice> Devices = null;
        /// <summary>
        /// empty constructor.
        /// </summary>
        public OpenGlDevice()
        {
            InitializeCtrlHandler();
            OnTime = new EventHandler(Timer_Tick);
            Camera = new Camera(this);
            Light L = new Light();
            L.Device = this;
            Lights.Add(L);
            ShadowSetting = new ShadowSettings(this);
            Selector.Device = this;
            if (Devices == null) Devices = new List<OpenGlDevice>();
        }
        /// <summary>
        /// sets and gets the distance in pixels for snapping an object. The default is 4.
        /// </summary>
        public int SnapMagnetism
        {
            get { return Selector.SnapDistance; }
            set { Selector.SnapDistance = value; }
        }
        /// <summary>
        /// gets and sets the windows cursor. 
        /// </summary>
        public Cursor Cursor
        {

            set { WinControl.Cursor = value; }
            get { return WinControl.Cursor; }
        }
        /// <summary>
        /// gets the version of openGl.
        /// </summary>
        public string Version
        { get { return GL.GetString(StringName.Version); }
        }
        /// <summary>
        /// gets the vendor of openGl.
        /// </summary>
        public string Vendor
        { get { return GL.GetString(StringName.Vendor); }
        }
        /// <summary>
        /// gets the avaliable extensions.
        /// </summary>
        public string Extensions
        { get { return GL.GetString(StringName.Extensions); }
        }
        /// <summary>
        /// gets the renderer.
        /// </summary>
        public string Renderer
        { get { return GL.GetString(StringName.Renderer); }
        }
        PenStyles _PenStyle = PenStyles.Full;
        /// <summary>
        /// sets and gets the style of the pen. See also <see cref="PenStyles"/>
        /// </summary>
       public PenStyles PenStyle
        {
          get { return _PenStyle; }
          set {
               if (Entity.Compiling)
                {
                    if (MeshCreator.HasPenStyle) MeshCreator.Renew();
                    MeshCreator.PenStyle = value;
                        
                        
                }

                setPenStyle(value);
                _PenStyle = value; }
        }
        private void setPenStyle(PenStyles value)
        {

            short Pattern = Convert.ToInt16("FFFF", 16);
            int Factor = 0;

            switch (value)
            {
                case PenStyles.Dash:
                    Pattern = Convert.ToInt16("EEEE", 16);
                    Factor = 3; break;
                case PenStyles.Dot:
                    Pattern = Convert.ToInt16("AAAA", 16);
                    Factor = 3; break;
                case PenStyles.DashDot:
                    Pattern = Convert.ToInt16("E4E4", 16);
                    Factor = 3; break;
                case PenStyles.DashDotDot:
                    Pattern = Convert.ToInt16("E4E4", 16);
                    Factor = 3; break;
                case PenStyles.Full: break;
            }
            if (Factor > 0)
            {
                GL.LineStipple(Factor, Pattern);
                GL.Enable(EnableCap.LineStipple);
            }
            else

                GL.Disable(EnableCap.LineStipple);


        }
        bool _invalid = false;
        internal bool invalid
        { get { return _invalid; }
          set { _invalid = value;
if (value)
                { }

            }

        }
        /// <summary>
        /// This method shows a message box, if an OpenGl error ocurres.
        /// <list type="Table">
        /// <item>
        ///	InvalidEnum:An unacceptable value has been specified for an enumerated argument. The offending function has been ignored.
        ///	</item>
        /// <item>
        /// InvalidValue : A numeric argument is out of range. The offending function has been ignored.
        /// </item>
        ///<item>
        ///InvalidOperation : The specified operation is not allowed in the current state. The offending function has been ignored.
        /// </item>
        /// <item>
        ///StackOverflow : This function would cause a stack overflow. The offending function has been ignored.
        /// </item>
        /// <item>
        ///StackUnderflow : This function would cause a stack underflow. The offending function has been ignored.
        /// </item>
        /// <item>
        ///OutOfMemory : There is not enough memory left to execute the function. The state of OpenGL has been left undefined.
        /// </item>
        /// </list>
        /// </summary>
        /// 
        public static void CheckError()
        {

            OpenTK.Graphics.OpenGL.ErrorCode errorCode = ErrorCode.NoError;
            try
            {
                errorCode = GL.GetError();
            }
            catch (Exception)
            {
                   
                throw;
            }


            if (errorCode != OpenTK.Graphics.OpenGL.ErrorCode.NoError)
            {

                switch (errorCode)
                {
                    case ErrorCode.InvalidEnum:
                        MessageBox.Show("InvalidEnum - An unacceptable value has been specified for an enumerated argument.  The offending function has been ignored.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorCode.InvalidValue:
                        MessageBox.Show("InvalidValue - A numeric argument is out of range.  The offending function has been ignored.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorCode.InvalidOperation:
                        MessageBox.Show("InvalidOperation - The specified operation is not allowed in the current state.  The offending function has been ignored.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorCode.StackOverflow:
                        MessageBox.Show("StackOverflow - This function would cause a stack overflow.  The offending function has been ignored.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorCode.StackUnderflow:
                        MessageBox.Show("StackUnderflow - This function would cause a stack underflow.  The offending function has been ignored.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorCode.OutOfMemory:
                        MessageBox.Show("OutOfMemory - There is not enough memory left to execute the function.  The state of OpenGL has been left undefined.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    default:
                        MessageBox.Show("Unknown GL error.  This should never happen.", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }

        }
        private bool _Culling = true;
        /// <summary>
        /// gets and sets whether the CullFace will be drawn. the default is true.
        /// For testing you can set <see cref="Culling"/> false.
        /// aA value of true makes the drawing faster.
        /// </summary>
        public bool Culling
        {
            get { return _Culling; }
            set
            { setCulling(value);
            }
        }
     
        private static bool _ClockWise = false;
        private void SetClockWise(bool value)
        {
            if (value)
                GL.FrontFace(FrontFaceDirection.Cw);

            else
                GL.FrontFace(FrontFaceDirection.Ccw);
            _ClockWise = value;


        }
        /// <summary>
        /// gets and sets the clockwise of the frontface. Default is true.
        /// </summary>
        public bool ClockWise
        {
            get { return _ClockWise; }
            set { SetClockWise(value); }

        }
        void setCulling(bool value)
        {
            
            {
                if (value)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(CullFaceMode.Back);
                }
                else
                    GL.Disable(EnableCap.CullFace);


            }


        }

       
        
        private RenderKind _RenderKind = RenderKind.None;
        /// <summary>
        /// gets and sets the <see cref="RenderKind"/>.
        /// </summary>
        public RenderKind RenderKind
        {
            get { return _RenderKind; }
            set
            {
                _RenderKind = value;
                //if (RenderKind == RenderKind.Render)
                //{ Shader = DefaultShader; }


            }
        }
        [NonSerialized]
        GLShader _Shader = null;
        /// <summary>
        /// shader, which draws directly the scene.
        /// </summary>
      
       
        public GLShader Shader
        {
            get { return _Shader; }

                   
            set   
            {  
                if ((_Shader != value)) 
                {
                   
                    if (_Shader != null) _Shader.Using = false;
                    _Shader = value;
                 
                    if (_Shader != null)
                    {    
                       
                  
                        _Shader.Using = true;
                     
                    }
                }

            }
        }
        bool _LightEnabled = true;
        /// <summary>
        /// you can turn off or on the light by setting true or false.
        /// </summary>
        public bool LightEnabled
        {
            get { return _LightEnabled; }
            set
            {

                _LightEnabled = value;
                if (Shader != null)
                {
                  Field  C = Shader.getvar("Light0Enabled");
                    if (C!=null)
                    C.Update();
                }
            
            }
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal bool drawEnable = true;
        bool _SnapEnable = true;
        /// <summary>
        /// you can turn off the snap mechanism by setting this property to false.
        /// Default it is true.
        /// </summary>
        public bool SnapEnable
        {
            get { return _SnapEnable; }
            set
            {
                _SnapEnable = value;
            }
        }
        /// <summary>
        /// compiles ato an entiy. The draw method describes outfit.
        /// </summary>
        /// <param name="Draw">Method, which drsws some thing.</param>
        /// <returns></returns>
        public Entity Compile(DrawAction Draw)
        {
            bool SaveCompiling = Entity.Compiling;
            Entity.Compiling = true;
            Entity Result= MeshCreator.Compile(this, Draw);
            Entity.Compiling = SaveCompiling;
            return Result;
        }
     
        /// <summary>
        /// plays the scene in Bitmap.
        /// </summary>
        /// <returns></returns>
        public Bitmap ScreenShot()
        {
            FBO FrameBuffer = new FBO((int)ViewPort.Width, (int)ViewPort.Height);
            Texture Save = texture;
            FrameBuffer.EnableWriting();
            Refresh();
            FrameBuffer.DisableWriting();
            texture = Save;
            Bitmap Result = FrameBuffer.ReadBitmap();
            FrameBuffer.Dispose();
            return Result;
        }
        /// <summary>
        /// convrt a world point to a screen point.
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public Point ToScr(xyz P)
        {
            xyz Q = ProjectionMatrix * P;
            int x = (int)(Math.Round(Q.X * ViewPort.Width / 2 + ViewPort.Width / 2, 0));
            int y = (int)(Math.Round(ViewPort.Height / 2 - Q.Y * ViewPort.Height / 2, 0));
            return new Point(x, y);
        }
        /// <summary>
        /// copy the screen to the clipboard.
        /// </summary>
        public void ScreenToClipboard()
        {
            Bitmap B = ScreenShot();
            Clipboard.SetImage(B);
        }
        /// <summary>
        /// gets or sets the <see cref="Light"/>s.
        /// </summary>
        public List<Light> Lights = new List<Light>();
       
        /// <summary>
        /// makes a translation of the scene.
        /// </summary>
        /// <param name="Vector">translation vector.</param>
        public void Translate(xyz Vector)
        {
            MulMatrix(Matrix.Translation(Vector));
        }
        /// <summary>
        /// makes a rotation of the scene.
        /// </summary>
        /// <param name="L">the rotation axis. See also <see cref="LineType"/></param>
        /// <param name="Angle">the rotation angle.</param>
        public void Rotate(LineType L, double Angle)
        {
            MulMatrix(Matrix.Rotation(L, Angle));
        }
        /// <summary>
        /// calculates a mirrored image of the scene.
        /// </summary>
        /// <param name="MirrorPlane">the mirror plane.</param>
        public void Mirror(Plane MirrorPlane)
        {
            MulMatrix(Matrix.Mirror(MirrorPlane));
        }
        /// <summary>
        /// scales a scene by a factor.
        /// </summary>
        /// <param name="Factor">factor for the scaling.</param>
        public void Scale(xyz Factor)
        {
            MulMatrix(Matrix.Scale(Factor));
        }
        internal Drawing3d.PolygonMode _PolygonMode = Drawing3d.PolygonMode.Fill;
        /// <summary>
        /// set or gets the kind with that the objects will be drawn. See also <see cref="Drawing3d.PolygonMode"/>
        /// </summary>
        public Drawing3d.PolygonMode PolygonMode
        {
            get { return _PolygonMode; }
            set
            {
                if (Entity.Compiling)
                    if ((MeshCreator.MeshMode == PolygonMode.Line) || (MeshCreator.MeshMode != value))
                    {
                        MeshCreator.Renew();
                        MeshCreator.MeshMode = value;
                    }

                GL.PolygonMode(MaterialFace.FrontAndBack, (OpenTK.Graphics.OpenGL.PolygonMode)value);
                _PolygonMode = value;


               
                   





            }

        }
      

      
        /// <summary>
        /// gets the <see cref="SnappItem"/>s. See also <see cref="SnappItems"/>.
        /// </summary>
        public List<SnappItem> SnappItems { get { return Selector.SnappList; } }

        bool IsReady()
        {

            return ((RenderingContext != null) && (RenderingContext.IsCurrent));
        }
        /// <summary>
        /// destructor, which dispose textures and rendering context.
        /// </summary>
        ~OpenGlDevice()
        {
            DisposeTextures();
            if (RenderingContext != null)
                RenderingContext.Dispose();
        }
        /// <summary>
        /// an event which is called, when the <see cref="ProjectionMatrix"/> has been changed.
        /// </summary>
        public event SetMatrix ProjectionMatrixChanged;
        Matrix OldProjection = Matrix.identity;
        /// <summary>
        /// SetMethod of the <see cref="ProjectionMatrix"/>.
        /// </summary>
        void setProjectionMatrix(Matrix value)
        {
            if (ProjectionMatrixChanged != null)
            {
                 _ProjectionMatrix = value;
                  ProjectionMatrixChanged(ref _ProjectionMatrix);
            }
            else
                _ProjectionMatrix = value;

            if (IsReady())
                if ((Shader != null) && (Shader.Using))
                {
                   Field C = Shader.getvar("ProjectionMatrix");
                    if (C != null) C.Update();
                 

                }

            ProjectMatrixInvalid = true;


        }
        /// <summary>
        /// definition of an event which is called, when the <see cref="ProjectionMatrix"/> has been changed.
        /// </summary>
        /// <param name="CurrentMatrix">You can set a new <see cref="ProjectionMatrix"/></param>
        /// <returns></returns>
        public delegate void SetMatrix(ref Matrix CurrentMatrix);
        /// <summary>
        /// an event for additional drawings.
        /// </summary>
        public event EventHandler Draw;
        /// <summary>
        /// an event for foreground drawings.
        /// </summary>
        public event EventHandler ForeGroundDraw;
        /// <summary>
        /// you can override this method to draw your foreground. See also <see cref="ForegroundDrawEnable"/>
        /// </summary>
        protected virtual void OnForegroundPaint()
        {

            if (ForeGroundDraw != null)
                ForeGroundDraw(this, new EventArgs());
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].OnForegroundDraw(this);
            }
        }
        /// <summary>
        /// gets or sets a texture.
        /// </summary>
        public Texture texture
        {
            get
            {

                return _texture;
            }
            set
            {
                setTexture(value);
            }
        }  
           
        /// <summary>       
        /// the main painting method.
        /// </summary>   
        public virtual void OnPaint()
        {      
           
            {         
                          
                for (int i = 0; i < Controls.Count; i++)
                {
                    if (Controls[i].Visible)
                        Controls[i].Paint(this);
                }
                if (Draw != null)
                    Draw(this, new EventArgs());
            }
        }
        string ErrorMsg = null;
        internal bool _OutFitChanged = false;



        /// <summary>
        /// if you make a changing of the scene you have set outfitChanged = true.
        /// </summary>
        public bool OutFitChanged
        {
            get { return _OutFitChanged; }
            set
            {
                _OutFitChanged = value;
            }


        }
      
      
        /// <summary>
        /// internal.
        /// </summary>
        internal void Paint()
        {
            System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();
            SW.Reset();
            SW.Start();
           
            {
            
                invalid = false;
              
                if (OutFitChanged)
                    if (ForegroundDrawEnable)
                    {
                        RefreshBackGround();
                        OutFitChanged = false;
                    }

                
                if ((Shadow) && (ShadowDirty))
                {
                    ResetShadow();
                
                }
               
                BeginPaint();
                CallAnimation();

                if (ForegroundDrawEnable)
                {

                    CopyFromBackGround();
                    OnForegroundPaint();
                }
                else
                    OnPaint();
                EndPaint();
                if (invalid)
                {
                    Selector.RefreshSnapBuffer();
                    invalid = false;
                }
            }
            SW.Stop();
           
        }
      
        /// <summary>
        /// is called at the begin of <see cref="OnPaint"/>
        /// </summary>

        virtual protected void BeginPaint()
        {
            ClearScreen();
        }
        
        /// <summary>
        /// is called at the end of <see cref="OnPaint"/>
        /// </summary>
     
        virtual protected void EndPaint()
        {

            SwapBuffers();
            OutFitChanged = false;
        }

        bool _ForegroundDrawEnable = false;
        void _SetForegroundDrawEnable(OpenGlDevice Device, object data)
        {
            ForegroundDrawEnable = (bool)data;

        }
    
        /// <summary>
        /// if you want to paint only in the fore ground set <see cref="ForegroundDrawEnable"/>=true. See also <see cref="ForeGroundDraw"/> and <see cref="ForeGroundDraw"/>.
        /// </summary>
        public bool ForegroundDrawEnable
        {
            get { return _ForegroundDrawEnable; }
            set
            {
               
                _ForegroundDrawEnable = value;

             
                {
                    _ForegroundDrawEnable = value;

                    {

                        if (value)
                        {

                            RefreshBackGround();
                            CopyFromBackGround();

                        }
                        Selector.RefreshSnapBuffer();

                    }
                   OutFitChanged=true;
                }

            }

        }
      
         /// <summary>
         /// refreshes the back ground for <see cref="ForeGroundDraw"/>.
         /// </summary>
         public void RefreshBackGround()
        {
            Texture Savet = texture;
            GLShader S = Shader;
            BackBuffer.BackGround = BackColor;
            if (BackBuffer.FboHandle <= 0)
                BackBuffer.Init((int)ViewPort.Width, (int)ViewPort.Height);
          
            RenderKind K = RenderKind;
            RenderKind = RenderKind.Render;
            Matrix P = ProjectionMatrix;
            ProjectionMatrix = Matrix.scale(new xyz(1, -1, 1)) * P;
      
            //--------- Draw scene to Backbuffer
            BackBuffer.EnableWriting();
           
            OnPaint();
           
            BackBuffer.DisableWriting();
            texture=Savet;
            ProjectionMatrix = P;
            RenderKind = K;
            Shader = S;
          
        }
        Matrix Bias = Matrix.Translation(-1, -1, 0) * Matrix.Scale(2, 2, 1);
        /// <summary>
        /// copies the backcound to the foreground.
        /// </summary>
       public void CopyFromBackGround()
        {  

            // Draw the Texture from Backbuffer by a Polygon Rectangle for the full screen
           
            xyArray FP = new xyArray(); FP.data = new xy[] { new xy(0, 0), new xy(0, 1), new xy(1, 1), new xy(1, 0) };
            Matrix P = ProjectionMatrix;
            GLShader S = Shader;
            RenderKind K = RenderKind;
            RenderKind = RenderKind.Render;
            Shader = BackGroundShader;
            Texture T = texture;

            texture = BackBuffer.Texture;
            PushMatrix();
            ModelMatrix = Matrix.Translation(new xyz(0, 0, 1));
            PolygonMode PM = PolygonMode;
            PolygonMode = PolygonMode.Fill;
            ProjectionMatrix = Bias;

            drawPolyLine(FP);
            ProjectionMatrix = P;
            PopMatrix();

            // Save Back
            RenderKind = K;
            texture = T;
            this.Shader = S;
            PolygonMode = PM;
        }
       
        float _PenWidth = 1;
        /// <summary>
        /// gets or sets the width of the pen.
        /// </summary>
        public float PenWidth
        {
            get { return _PenWidth; }
            set
            {
                _PenWidth = value;
                if (Entity.Compiling)
                {
                    if (MeshCreator.HasPenWidth)
                    MeshCreator.Renew();
                   
                    MeshCreator.PenWidth = value;
                }
                GL.LineWidth(value);
            }

        }
        /// <summary>
        /// gets a list of <see cref="Entity"/>, which will be drawn automaticly.
        /// </summary>
  
        List<Entity> _List = new List<Entity>();
        /// <summary>
        /// gets a list of <see cref="Entity"/>, which will be drawn automaticly.
        /// </summary>
        public List<Entity> Controls
        {
            get { return _List; }
        }


        /// <summary>
        /// you can use it for identify an element. See also <see cref="PopTag"/>. it must be called before poptag.
        /// </summary>
        /// <param name="Tag">any object</param>
        public void PushTag(object Tag)
        {
            Selector.Tags.Push(Tag);
        }
        /// <summary>
        /// you can use it for identify an element. See also <see cref="PushTag"/>. it must be called after pushtag.
        /// </summary>
       
        public void PopTag()
        {
            Selector.Tags.Pop();
        }
        private FBO BackBuffer = new FBO();

        /// <summary>
        /// gets the extension of the screen in world coordinate. x is the width, y the height.
        /// (exact for orthogonal projection.For perpective a reference point of 0/0/0 is taken. See also <see cref="getWorldWidth(xyz)"/>, <see cref="getWorldHeight(xyz)"/> and <see cref="FieldOfView"/> .
        /// </summary>
        public xy WorldExtensions
        { get { return new xy(getWorldWidth(new xyz(0,0,0)), getWorldHeight(new xyz(0, 0, 0))); } }

        /// <summary>
        /// gets or sets the pixels per unit. It is used when the projection is orthogonal. <see cref="ProjectionMatrix"/>
        /// </summary>
         public double PixelsPerUnit = 20;
        /// <summary>
        /// gives the width of the screen in world coordinates.
        /// <param name="ReferencePoint">when the projection is perspectivly, the referencepoint gives the depth, for which the width will be calcluated.
        /// When the projection is orthogonal this point is not used. See also <see cref="FieldOfView"/>.
        /// </param>
        /// </summary>
        /// <returns>the width of the screen in world coordinates.</returns>
        public double getWorldWidth(xyz ReferencePoint)
        {
            if (FieldOfView <= 0.01)
                return ((double)this.ViewPort.Width / PixelsPerUnit);
            else
            {
                if (ProjectMatrixInvalid)
                {
                    ProjectMatrixInvert = ProjectionMatrix.invert();
                }
                ProjectMatrixInvalid = false;
                xyz P = ProjectionMatrix * ReferencePoint;
                xyz A = ProjectMatrixInvert * new xyz(-1, 0, P.z);
                xyz B = ProjectMatrixInvert * new xyz(1, 0, P.z);
                return A.dist(B);

            }
        }
        /// <summary>
        /// gives the height of the screen in world coordinates.
        /// <param name="ReferencePoint">when the projection is perspectivly, the referencepoint gives the depth, for which the height will be calcluated.
        /// When the projection is orthogonal this point is not used. See also <see cref="FieldOfView"/>.
        /// </param>
        /// </summary>
        /// <returns>the width of the screen in world coordinates.</returns>
        public double getWorldHeight(xyz ReferencePoint)
        {

            if (FieldOfView <= 0.01)
                return ((double)this.ViewPort.Height / PixelsPerUnit);
            else
            {
                if (ProjectMatrixInvalid)
                {
                    ProjectMatrixInvert = ProjectionMatrix.invert();
                }
                ProjectMatrixInvalid = false;
                xyz P = ProjectionMatrix * ReferencePoint;
                xyz A = ProjectMatrixInvert * new xyz(0, 1, P.z);
                xyz B = ProjectMatrixInvert * new xyz(0, -1, P.z);
                return A.dist(B);

            }
        }
        private double _NearClippIng = 1;
        private double _FarClipping = 1000;
        /// <summary>
        /// gets and sets the near plane, where the scene will be clipped.
        /// It must be greater than 0.
        /// </summary>
        public double NearClipping { get { return _NearClippIng; }  set {_NearClippIng=value ;} }
        /// <summary>
        /// gets and sets the far plane, where the scene will be clipped.
        /// </summary>
        public double FarClipping { get { return _FarClipping; } set { _FarClipping = value; } }




        Matrix EyeMatrix()
        {
            double left;
            double right;
            double top;
            double bottom;
            double near;
            double far;
            Matrix Result = Matrix.identity;
            double Aspect = (float)ViewPort.Width / (float)ViewPort.Height;
            if (Utils.Equals(FieldOfView, 0))
            {
                double WorldWidth = getWorldWidth(new xyz(0,0,0));
                left = -WorldWidth / 2;
                right = WorldWidth / 2;
                double WorldHeight = getWorldHeight(new xyz(0, 0, 0));
                top = WorldHeight / 2;
                bottom = -WorldHeight / 2;
                near = NearClipping;
                far = FarClipping;
                Result = Matrix.Orthogonal(left, right, bottom, top, near, far);


            }
            else  //perspective
            {
                near = NearClipping;
                far = FarClipping;
                left = -near * Math.Tan(FieldOfView);
                right = -left;
                top = right / Aspect;
                bottom = -right / Aspect;
                Result = Matrix.Frustum(left * PerspektiveFactor, right * PerspektiveFactor, bottom * PerspektiveFactor, top * PerspektiveFactor, near, far);
            }
            return Result;
        }
        Matrix EyeMatrix(Size S, double Angle)
        {

            double left;
            double right;
            double top;
            double bottom;
            double near;
            double far;
            Matrix Result = Matrix.identity;
            double Aspect = 1;
            if (Utils.Equals(Angle, 0))
            {
                Aspect = (float)S.Width / (float)S.Height;
                double WorldWidth = S.Width / PixelsPerUnit;
                left = -WorldWidth / 2;
                right = WorldWidth / 2;
                double WorldHeight = S.Height / PixelsPerUnit;
                top = WorldHeight / 2;
                bottom = -WorldHeight / 2;
                near = NearClipping;
                far = FarClipping;
                Result = Matrix.Orthogonal(left, right, bottom, top, near, far);


            }
            else  //perspective
            {
                near = NearClipping;
                far = FarClipping;
                left = -near * System.Math.Tan(Angle);
                right = -left;
                top = right / Aspect;
                bottom = -right / Aspect;
                Result = Matrix.Frustum(left * PerspektiveFactor, right * PerspektiveFactor, bottom * PerspektiveFactor, top * PerspektiveFactor, near, far);
            }

            return Result;
        }
        internal double PerspektiveFactor = 1;
        private double _FieldOfView = 0;
        /// <summary>
        /// gets and sets the field of view in radian. See also <see cref="Camera.FieldOfView"/>
        /// </summary>
        public double FieldOfView
        {

            get { return _FieldOfView; }
            set
            {
                if ((value > Math.PI) || (value < 0))
                    throw new System.Exception("Wrong value: FieldofView has to be between 0 and 89");
                _FieldOfView = value;

                ProjectionMatrix = (EyeMatrix() * ((Matrix.LookAt(Camera.Position, Camera.Anchor, Camera.UpVector))));//* Matrix.Scale(PerspektiveFactor, PerspektiveFactor, PerspektiveFactor));// Camera.ViewMatrix;
                                                                                                                      //if (Camera!=null)
                                                                                                                      //Camera.setDefZoom();
            }
        }
        /// <summary>
        /// overrid thsi method to implement your methods.
        /// </summary>
        virtual protected void OnCreated()
        {
           

        }
        /// <summary>
        /// Calculates a Line from a Screen coordinate to the Camera.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>A LineType</returns>
        public LineType FromScr(Point p)
        {
            Matrix T = ProjectionMatrix.invert();
            
            xyz pt = new xyz(
                  (p.X + ViewPort.Left - (float)(ViewPort.Left + ViewPort.Right) / 2.0) / ((float)(ViewPort.Right - ViewPort.Left) / 2.0),
                  -(p.Y + ViewPort.Top - (float)(ViewPort.Bottom + ViewPort.Top) / 2.0) / ((float)(ViewPort.Bottom - ViewPort.Top) / 2.0), -1);
            pt = T * pt;
            float l=ViewPort.Left;
                
            xyz pt2 = new xyz(
           (p.X + ViewPort.Left - (float)(ViewPort.Left + ViewPort.Right) / 2.0) / ((float)(ViewPort.Right - ViewPort.Left) / 2.0),
           -(p.Y + ViewPort.Top - (float)(ViewPort.Bottom + ViewPort.Top) / 2.0) / ((float)(ViewPort.Bottom - ViewPort.Top) / 2.0), 1);
            pt2 = T * pt2;
            return new LineType(pt, (pt2 - pt).normalized());
              
        }
        bool ProjectMatrixInvalid = true;
        Matrix ProjectMatrixInvert = Matrix.identity;
        /// <summary>
        /// calculate a pixels to world lenght.
        /// </summary>
        /// <param name="P">a refrence point, which is needed in case of perspective representation.</param>
        /// <param name="Pixels">Number of pixels</param>
        /// <returns></returns>
        public double PixelToWorld(Drawing3d.xyz P, int Pixels)
        {
            if (ProjectMatrixInvalid)
            {
                ProjectMatrixInvert = ProjectionMatrix.invert();
            }
            ProjectMatrixInvalid = false;
            if (FieldOfView > 0.01)
            {
                xyz q = ProjectionMatrix * P;
                xyz A = ProjectMatrixInvert * (new xyz(0, 0, q.z));
                xyz B = ProjectMatrixInvert * (new xyz(1, 0, q.z));
                double d = A.dist(B);
                return Pixels*d / (ViewPort.Width / 2);
            }
            else
                return (ProjectMatrixInvert.multaffin(new xyz((float)Pixels / (float)(ViewPort.Width / 2), 0, 0))).length();
        }
       
        /// <summary>
        /// this enumeration describes the kind for rhe automatically navigation.See also <see cref="Navigating"/>.
        /// </summary>
        public enum NavigationKind
        {
            /// <summary>
            /// no navigating
            /// </summary>
            Nothing = NavigateModus.Nothing,
            /// <summary>
            /// zooming whith the cursor.
            /// </summary>
            Zoom = NavigateModus.Zoom,
            /// <summary>
            /// moving whith the cursor.
            /// </summary>
            Trans = NavigateModus.Trans,
            /// <summary>
            /// rotate whith the cursor.
            /// </summary>

            Rotate = NavigateModus.Rotate,
            /// <summary>
            /// zoom and move whith the cursor.
            /// </summary>
            ZoomTrans = NavigateModus.Zoom | NavigateModus.Trans,
            /// <summary>
            /// zoom and rotate whith the cursor.
            /// </summary>
            ZoomRotate = NavigateModus.Zoom | NavigateModus.Rotate,
            /// <summary>
            /// rotate and move whith the cursor.
            /// </summary>
            TransRotate = NavigateModus.Zoom | NavigateModus.Trans,
            /// <summary>
            /// zoom, Rotate and trans
            /// </summary>
            ZoomRotateTrans = NavigateModus.Zoom | NavigateModus.Trans | NavigateModus.Rotate
        }
        /// <summary>
        /// Sets the kind of navigating. See also <see cref="NavigationKind"/>
        /// </summary>
        public NavigationKind NavigateKind = NavigationKind.ZoomRotateTrans;
        internal enum NavigateModus
        {
            Nothing = 0,
            Trans = 1,
            Rotate = 2,
            Zoom = 4
        };
        bool _Navigating = true;
        /// <summary>
        /// you can turn off the navigatimg by setting false.
        /// Default is true.
        /// </summary>
        public bool Navigating
        {
            get { return _Navigating; }
            set
            {
                _Navigating = value;
              
            }
        }
        /// <summary>
        /// a very imortant tool for give an outlook of the scene.
        /// </summary>
        public Camera Camera = null;
       Point _MousePos= new Point(0,0);
        /// <summary>
        /// point of the mouse position.
        /// </summary>
        public Point MousePos
        {
            get { return _MousePos; }
            set { _MousePos= value;
                

            }
        } 
        xyz _Currentxyz = new xyz(0, 0, 0);
        /// <summary>
        /// gets and sets the current xyz point.
        /// </summary>
        public xyz Currentxyz
        {


            get { return _Currentxyz; }
            set {
               
                if (CurrentNavigationsModus != NavigateModus.Nothing) return; 
   


                _Currentxyz = value;
              

            }
        }

        static System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();
        static long ElapsedTime
        {
            get { return SW.ElapsedMilliseconds; }

        }
        static void ResetTime()
        {
            SW.Reset();
            SW.Start();
        }
        bool Busy = false;
        bool MouseAction = false;
        /// <summary>
        /// you can override this method. Dhe is called authomatically.
        /// </summary>
        public virtual void Refresh()
        {
            if (Busy) return;
            Busy = true;
            if (RefreshMode == Mode.WhenMouseEvent)
            {
                if (MouseAction)
                    return;
            }
            RenderKind = Drawing3d.RenderKind.Render;
            MakeCurrent();
            Paint();
            RenderKind = Drawing3d.RenderKind.None;
            Busy = false;
        }

    }
}



