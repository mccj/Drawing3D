using System;
using System.Windows.Forms;
using System.Drawing;

namespace Drawing3d
{
    public partial class OpenGlDevice
    { 
        private delegate void NCMouseMoveHandler(object sender,Point Point);
   
        [field:NonSerialized]
        private System.Windows.Forms.MouseEventHandler MouseDownHandler;
        [field: NonSerialized]
        private System.Windows.Forms.MouseEventHandler MouseWheelHandler;
        [field: NonSerialized]
        private System.Windows.Forms. MouseEventHandler MouseDoubleClickHandler;
        [field: NonSerialized]
        private EventHandler SizeChangedHandler;
        [field: NonSerialized]
        private System.Windows.Forms.MouseEventHandler MouseUpHandler;
        [field: NonSerialized]
        //private EventHandler StyleChangedHandler;
        //[field: NonSerialized]
        //private EventHandler VisibleChangedHandler;
        //[field: NonSerialized]
        private PaintEventHandler PaintHandler;
        [field: NonSerialized]
        private System.Windows.Forms.MouseEventHandler MouseMoveHandler;
        [field: NonSerialized]
        private EventHandler HandleDestroyedHandler;
        [field: NonSerialized]
        private System.Windows.Forms.KeyPressEventHandler KeyPressHandler;
        [field: NonSerialized]
        private System.Windows.Forms.KeyEventHandler KeyUpHandler;
        [field: NonSerialized]
        private System.Windows.Forms.KeyEventHandler KeyDownHandler;
        //[field: NonSerialized]
        //private InvalidateEventHandler InvalidateEventHandler;
        internal bool SizeChangeBegin = false;
        int sizeW = 0;
        int sizeH = 0;
        private void _SizeChanged(object sender, System.EventArgs e)
        {
          
            sizeW = WinControl.Width;
            sizeH = WinControl.Height;
           
            if (restore)
            { restore = false;
                SizeChanged();
                


            }
            if (WinControl is Form)
            { 
              if ((WinControl as Form).WindowState == FormWindowState.Maximized)
                {
                    SizeChanged();
                }


            }
            
            OnViewPortChanged(WinControl.ClientSize.Width, WinControl.ClientSize.Height);
            
        }

      
        private void _Destroy(object sender, System.EventArgs e)
        {
            if (RenderingContext != null)
            {
                RenderingContext.Dispose();
                RenderingContext = null;
                WinControl = null;
               
            }
    
        }
        Point BeginTransForm = new Point(0, 0);
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        virtual protected void onMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
        }
       
        private void _MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MousePos = new Point(e.X, e.Y);

            HandledMouseEventArgs E = new HandledMouseEventArgs(e);
            EventServer.MouseDoubleClick(E);
            if (!E.Handled)
                onMouseDoubleClick(e);
           
        }
        NavigateModus _CurrentNavigationsModus = NavigateModus.Nothing;
       internal NavigateModus CurrentNavigationsModus { get { return _CurrentNavigationsModus; } 
                                                       set {_CurrentNavigationsModus = value;} 
                                                      }
        Point TransStart = new Point(0, 0);
        internal Point StartNavigate = new Point();
        void BeginTrans()
        {
            CurrentNavigationsModus = NavigateModus.Trans;
            Camera.MakeConsistent();
            TransStart = MousePos;

        }
        [NonSerialized]
        EventHandler OnTime;
        [NonSerialized]
        Timer Timer = new Timer();
        /// <summary>
        /// gets and sets the last clicked positioon.
        /// </summary>
        public xyz LastClickPos = new xyz(0, 0, 0);
        bool LastClickPosIsReady = false;
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        virtual protected  void onMouseDown(System.Windows.Forms.MouseEventArgs e)
       {
            


           WinControl.Focus();
           
           if ((Navigating))
           {
               if (Control.ModifierKeys != Keys.Control)
               {
                    if (!Simulated)
                   if (((int)NavigateKind & (int)NavigateModus.Rotate) == (int)NavigateModus.Rotate)
                   {
                       CurrentNavigationsModus = NavigateModus.Rotate;
                       StartNavigate = MousePos;

                       Camera.BeginRotation();

                   }
               }
               else
                   if (((int)NavigateKind & (int)NavigateModus.Trans) == (int)NavigateModus.Trans)
                         
               {
                  if (!Simulated)
                   BeginTrans();
               }

           }
            BeginTransForm = MousePos;
            Base B = new Base();
            if (SnappItems.Count > 0)
                B = SnappItems[0].GetBase();
            if (B.BaseZ.dist(new xyz(0, 0, 1)) < 0.001)
            { }
        }
        /// <summary>
        /// this method is called at the end of wheeling. (500 milliseconds after the last movement).
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnMouseWheelEnd(System.Windows.Forms.MouseEventArgs e)
        {
            
        }
        /// <summary>
        /// the event for a long click. (Minimum 500 milliseconds)
        /// </summary>
        public event EventHandler LongClick;
        private void Timer_Tick(object sender, EventArgs e)
        { 
            Timer.Tick -= OnTime;
            Timer.Enabled = false;
            HandledMouseEventArgs E = new HandledMouseEventArgs(eSave);
            EventServer.LongClick(E);
            LongClick?.Invoke(this, eSave);
        }
        private void _LongClick(object sender, EventArgs e)
        {
            if (LongClick != null)
                LongClick(this, e);
       }
        private void _MouseWheelEnd(object sender, System.Windows.Forms.MouseEventArgs e)
        { 
            if (!LastClickPosIsReady)
            {
   
                LastClickPos = Currentxyz;

            }
            HandledMouseEventArgs E = new HandledMouseEventArgs(e);
            EventServer.MouseWheelEnd(E);
            if (!E.Handled)
                OnMouseWheelEnd(e);

        }
        [NonSerialized]
        System.Windows.Forms.MouseEventArgs eSave = null;
        private void _MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!LastClickPosIsReady)
            {   
                LastClickPos = Currentxyz;
            }
            Timer.Interval = 500;
            Timer.Tick += OnTime;
            Timer.Enabled = true;
            eSave = e;
            MousePos = new Point(e.X, e.Y);
           
            if ((!SnapEnable))
            {
                double Lam = -1;

                Plane P = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                xyz Pt = new xyz(0, 0, 0);
                P.Cross(FromScr(MousePos), out Lam, out Pt);
                Currentxyz = Pt;
            }
            else
           if ((CurrentNavigationsModus == NavigateModus.Nothing))
                Selector.CreateSnappList();
            HandledMouseEventArgs E = new HandledMouseEventArgs(e);



            EventServer.MouseDown(E);
            if (!E.Handled)
                onMouseDown(e);



            for (int i = 0; i < EventServer.Added.Count; i++)
            {
                if (EventServer.Contains(EventServer.Added[i]))
                EventServer.Added[i].OnMouseDown(E);
                
            }
            EventServer.Added.Clear();
        }
        void MoveTrans()
        {
            if (CurrentNavigationsModus == NavigateModus.Trans)
              Camera.Translate(new xy(MousePos.X - TransStart.X, TransStart.Y - MousePos.Y));
            if (RefreshMode == Mode.WhenNeeded)
            Refresh();
            TransStart = MousePos;
        }
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        virtual protected void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {if (Simulated) return;
            Timer.Tick -= OnTime;
            Timer.Enabled = false;
            if (Navigating)
            {
              
                if (CurrentNavigationsModus == NavigateModus.Trans)
                {
                    MoveTrans();

                }
                if (CurrentNavigationsModus == NavigateModus.Rotate)
                {
                    double dx = MousePos.X - StartNavigate.X;
                    double dy = MousePos.Y - StartNavigate.Y;
                    double DeltaAlfa = (float)(1 * dx / 480f * System.Math.PI); // deg 2
                    double DeltaBeta = (float)(1 * dy / 480f * System.Math.PI); // deg 2
                    Camera.Navigate(DeltaBeta, -DeltaAlfa);
                    StartNavigate = MousePos;

                }

            }
         }
       
        private void _MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
          
            if ((MousePos.X == e.X) && (MousePos.Y == e.Y)) return;
            MousePos = new Point(e.X, e.Y);
            double Lam = -1;

            Plane P = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
            xyz Pt = new xyz(0, 0, 0);
            P.Cross(FromScr(MousePos), out Lam, out Pt);
           if (CurrentNavigationsModus== NavigateModus.Nothing) // es braucht kein Currentxyz
                if (SnappItems.Count==0)
                Currentxyz = Pt;
            if ((!SnapEnable) /*&& (!ForegroundDrawEnable)*/)
            {
                //double Lam = -1;

                //Plane P = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                //xyz Pt = new xyz(0, 0, 0);
                //P.Cross(FromScr(MousePos), out Lam, out Pt);
                //Currentxyz = Pt;
            }
            else
           if ((CurrentNavigationsModus == NavigateModus.Nothing))// && (!ForegroundDrawEnable))
            {
             
               Selector.CreateSnappList();
               
            }
            HandledMouseEventArgs E = new HandledMouseEventArgs(e);
            EventServer.MouseMove(E);
            if (!E.Handled)
             OnMouseMove(e);
         

        }
        /// <summary>
        /// the <see cref="EventServer"/> of the <see cref="OpenGlDevice"/>.
        /// </summary>
        [NonSerialized]
        public EventServer EventServer = new EventServer();
        /// <summary>
        /// gwts and sets the default cursr
        /// </summary>
        public Cursor DefaultCursor = null;
        [NonSerialized]
        Control _WinControl = null;
        /// <summary>
        /// the control in which it will be drawn the scene. It is of type:System.windows.Forms.Control.
        /// </summary>
       
        public Control WinControl {  
            get { return _WinControl; }
            set { SetWinControl(  value);
                if (value!= null)
                DefaultCursor = value.Cursor;
               }
        }
        /// <summary>
        /// This property has to be set to a Control. In that the scene will be drawn.
        /// </summary>
        private Form getForm()
        {
           
            return getForm(WinControl);
        }
        private Form getForm(Control c)
        {
            Form F = null;
           

            while ((c != null) && (!(c is Form))) c = c.Parent;
            if ((c != null) && (c is Form))
                F = (Form)c;
            return F;
        }
        bool _WincursorVisible = true;
    /// <summary>
    /// gets and sets, whether the default windows cursor  visible. Default it is true.
    /// </summary>
     public bool WincursorVisible
        {
            get { return _WincursorVisible; }
            set { _WincursorVisible = value;
                if (!value)
                { NCMouseMove = NCMoveHandler;
                    WinControl.MouseMove += CtrlNCMouseMoveHandler;
                  WinControl.Cursor = D3DCursors.Empty;
                    for (int i = 0; i < WinControl.Controls.Count; i++)
                    {
                        (WinControl.Controls[i]).Cursor = Cursors.Default;
                    }
                }
                else
                {   NCMouseMove = null;
                    WinControl.Cursor = Cursors.Default;
                    WinControl.MouseMove -= CtrlNCMouseMoveHandler;
               }
            }
        }
        [field: NonSerialized]
        private System.Windows.Forms.MouseEventHandler CtrlNCMouseMoveHandler = null;
       
        private void CtrlNCMouseMove(object sender, MouseEventArgs e)
        {
            WinControl.Cursor = D3DCursors.Empty;
        }
        private NCMouseMoveHandler NCMouseMove = null;
        private NCMouseMoveHandler NCMoveHandler = null;
    
        private void NCMouse(object sender, Point P)
        {
            WinControl.Cursor = Cursors.Default;
            int w = WinControl.Width;
            int h = WinControl.Height;
           Rectangle R= WinControl.ClientRectangle;
          
            int diff = 14;
            if ((P.X < diff)&&(P.Y<diff)) WinControl.Cursor = Cursors.SizeNWSE;
            else
            if ((P.X < diff) && (Math.Abs(P.Y- WinControl.Height) < diff)) WinControl.Cursor = Cursors.SizeNESW;
            else
            //if ((Math.Abs(P.X- WinControl.Width) < diff) && (P.Y < diff)) WinControl.Cursor = Cursors.SizeNESW;
            //else
            if ((Math.Abs(P.X - WinControl.Width) < diff) && (Math.Abs(P.Y - WinControl.Height) < diff)) WinControl.Cursor = Cursors.SizeNWSE;
            else

             if (P.Y < diff) WinControl.Cursor = Cursors.SizeNS;
            else
            if (P.X<diff) WinControl.Cursor = Cursors.SizeWE;
           else
            if (Math.Abs(P.X-WinControl.Width)<=diff) WinControl.Cursor = Cursors.SizeWE;
           else
            if (Math.Abs(P.Y - WinControl.Height) <= diff) WinControl.Cursor = Cursors.SizeNS;

        }
        void _NCMouseMove(Point P)
        {
            if (NCMouseMove != null)
                NCMouseMove(this, P);
        }
       
    
     
        /// <summary>
        /// Set method of the property <see cref="WinControl"/>
        /// </summary>
        /// <param name="value"></param>
       protected virtual void SetWinControl(Control value)
        {
            if (WinControl == value) return;
         
            if (WinControl != null)
            {

                ReleaseHandle();
                Devices.Remove(this);
                UnSetTimer();
                Control.CheckForIllegalCrossThreadCalls = false;
                WinControl.MouseDoubleClick -= MouseDoubleClickHandler;
                WinControl.MouseDown -= MouseDownHandler;
                WinControl.MouseUp -= MouseUpHandler;
                WinControl.MouseMove -= MouseMoveHandler;
                WinControl.SizeChanged -= SizeChangedHandler;
                //WinControl.StyleChanged -= StyleChangedHandler;
                //WinControl.VisibleChanged -= VisibleChangedHandler;
                WinControl.HandleDestroyed -= HandleDestroyedHandler;
                WinControl.Paint -= PaintHandler;
          
                WinControl.MouseWheel -= MouseWheelHandler;
                Form F = getForm();
                if (F != null)
                {
                    
                    F.KeyDown -= KeyDownHandler;
                    F.KeyUp -= KeyUpHandler;
                    F.KeyPress -= KeyPressHandler;
                    F.HandleDestroyed -= HandleDestroyedHandler;
                   
                }
            }
            Control C = WinControl;
            _WinControl = value;
            if (WinControl != null)
            {
                WinControl.MouseDoubleClick += MouseDoubleClickHandler;
                WinControl.MouseDown += MouseDownHandler;
                WinControl.MouseUp += MouseUpHandler;
                WinControl.MouseMove += MouseMoveHandler;
                WinControl.HandleDestroyed += HandleDestroyedHandler;
                WinControl.SizeChanged += SizeChangedHandler;
             
                WinControl.Paint += PaintHandler;
                MouseWheelTimer.Tick+= _MouseWheelTick;
                Form F = getForm();
                if (F != null)
                {
                    F.MouseWheel += MouseWheelHandler;
                    F.KeyPreview = true;
                    F.KeyDown += KeyDownHandler;
                    F.KeyUp += KeyUpHandler;
                    F.KeyPress += KeyPressHandler;
                    F.ResizeEnd += F_ResizeEnd;
                }
              
                InitializeOgl(WinControl);
                CheckError();
                Devices.Add(this);
                OnViewPortChanged(WinControl.ClientRectangle.Width, WinControl.ClientRectangle.Height);
                AssignHandle();
                
               
            }
        }
        private bool Simulated = false;
        /// <summary>
        /// you can simulate a mousedown event.
        /// </summary>
        public void SimulatedMouseDown()
        {
            Simulated = true;
            _MouseDown(this, new System.Windows.Forms.MouseEventArgs(MouseButtons.Left, 1, MousePos.X, MousePos.Y, 0));
            Simulated = false;
           
          
        }
        /// <summary>
        /// you can simulate a MouseDoubleClick event.
        /// </summary>
        public void SimulatedMouseDoubleClick()
        {
            Simulated = true;
            _MouseDoubleClick(this,new System.Windows.Forms.MouseEventArgs(MouseButtons.Left, 1, MousePos.X, MousePos.Y, 0));
            Simulated = false;

        }
        /// <summary>
        /// you can simulate a MouseUp event.
        /// </summary>
        public void SimulatedMouseUp()
        {

            Simulated = true;
            _MouseUp(this, new System.Windows.Forms.MouseEventArgs(MouseButtons.Left,1,MousePos.X,MousePos.Y,0));
            Simulated = false;
        }
        /// <summary>
        /// you can simulate a MouseMove event.
        /// </summary>
        public void SimulatedMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            Simulated = true;
            int Mpx = MousePos.X;
            int Mpy = MousePos.Y;
            MousePos = new Point(MousePos.X - 1, MousePos.Y - 1);
            _MouseMove(this, new System.Windows.Forms.MouseEventArgs(MouseButtons.Left, 1, Mpx, Mpy, 0));
            
            Simulated = false;
        }
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        virtual protected void onKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
 
        private void _KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventServer.KeyDown(e);
            if (!e.Handled)
            onKeyDown(e);
           

        }
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        virtual protected void onMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (Navigating)
            {
                if (CurrentNavigationsModus != NavigateModus.Nothing)
                {
                    Camera.MakeConsistent();
                    int x = e.X;
                    int y = e.Y;
                    CurrentNavigationsModus = NavigateModus.Nothing;
                    if ((e.X != BeginTransForm.X) || (e.Y != BeginTransForm.Y))
                    {
                        Selector.RefreshSnapBuffer();
                      

                        ShadowDirty = true;
                    }
                   
                }
            }
               
             
                
            
        }

        private void _MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Timer.Tick -= OnTime;
            Timer.Enabled = false;
            MousePos = new Point(e.X, e.Y);
            HandledMouseEventArgs E = new HandledMouseEventArgs(e);

             EventServer.MouseUp(E);
            if (!E.Handled)
                onMouseUp(e);
           
        
            try
            { 


               
            }
            catch (Exception)
            {

                
            }
           
            LastClickPos = Currentxyz;
        }
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">KeyPressEventArgs</param>
        virtual protected void onKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
        }

        private void _KeyPress(object sender, KeyPressEventArgs e)
        {
            EventServer.KeyPress(e);
            if (!e.Handled)
                onKeyPress(e);
           
       }
        /// <summary>
        /// this method is called from <see cref="WinControl"/>, if the <see cref="EventServer"/> returns true.
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        virtual protected void onKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void _KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventServer.KeyUp(e);
                if (!e.Handled)
          
               onKeyUp(e);
        }
        [NonSerialized]
        Timer MouseWheelTimer = new Timer();
        void _MouseWheelTick(object sender, EventArgs e)
        {
            MouseWheelTimer.Stop();
            MouseWheelTimer.Enabled = false;
            try
            {
                Selector.RefreshSnapBuffer();
            }
            catch (Exception)
            {

               
            }
            _MouseWheelEnd(this,save);
        }
        [NonSerialized]
         MouseEventArgs save = null;
 
        void _MouseWheel(object sender, MouseEventArgs e)
        {
             save = e;
            Point P = MousePos;
            if (((int)NavigateKind & (int)NavigateModus.Zoom) == (int)NavigateModus.Zoom)
            {
           if (!(WinControl is Form))
            {
                Form F = getForm();
             
                if ((F != null) &&  (WinControl != F))
                {
                    Point Pt = new Point(Cursor.Position.X - F.Left, Cursor.Position.Y - F.Top);
                    Control C = F.GetChildAtPoint(Pt);
                    if (C != WinControl) return;
                    P = C.PointToClient(Cursor.Position);
                      

                }
                
           }
                 double Factor = 1.0 + (float)e.Delta / 1000f;
                Camera.Zoom(P, Factor);
                OutFitChanged = true;
                Camera.MakeConsistent();
                HandledMouseEventArgs E = new HandledMouseEventArgs(e);
                EventServer.MouseWheel(E);
            }
            MouseWheelTimer.Interval = 500;
            MouseWheelTimer.Stop();
            MouseWheelTimer.Enabled = true;
            MouseWheelTimer.Start();
        }
       
        private void SizeChanged()
        {
            OnViewPortChanged(WinControl.ClientSize.Width, WinControl.ClientSize.Height);
            if (SnapEnable)
            {
                try
                {    
                    
                    Selector.RefreshSnapBuffer();

                }
                catch (System.Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }
            if (Shadow) ShadowDirty = true;
        }
       
        private void F_ResizeEnd(object sender, EventArgs e)
        { 
            SizeChanged();
            ShadowDirty = true;              
         
       }

        void InitializeCtrlHandler()
        {
            PaintHandler = new System.Windows.Forms.PaintEventHandler(_Paint);
            MouseDoubleClickHandler = new System.Windows.Forms.MouseEventHandler(_MouseDoubleClick);
            MouseDownHandler = new System.Windows.Forms.MouseEventHandler(_MouseDown);
            HandleDestroyedHandler = new EventHandler(_Destroy);
            MouseWheelHandler = new System.Windows.Forms.MouseEventHandler(_MouseWheel);
            SizeChangedHandler = new System.EventHandler(_SizeChanged);
            MouseUpHandler = new System.Windows.Forms.MouseEventHandler(_MouseUp);
            PaintHandler = new System.Windows.Forms.PaintEventHandler(_Paint);
            MouseMoveHandler = new System.Windows.Forms.MouseEventHandler(_MouseMove);
            KeyPressHandler = new System.Windows.Forms.KeyPressEventHandler(_KeyPress);
            KeyUpHandler = new System.Windows.Forms.KeyEventHandler(_KeyUp);
            KeyDownHandler = new System.Windows.Forms.KeyEventHandler(_KeyDown);
            NCMoveHandler = new NCMouseMoveHandler(NCMouse);
            CtrlNCMouseMoveHandler = new System.Windows.Forms.MouseEventHandler(CtrlNCMouseMove);
          
        }
       
        private void _Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
         
            Refresh();
     
        }
        bool restore = false;
       private bool FirstViewPortChanged = true;
        /// <summary>
        /// is called when the <see cref="WinControl"/> has changed his size.
        /// </summary>
        /// <param name="width">the new width of the Wincontrol</param>
        /// <param name="height">the new height of the Wincontrol</param>
       
        protected virtual void OnViewPortChanged(int width, int height)
        {
            //if (FirstViewPortChanged)
            //{ FirstViewPortChanged = false;
            //    OnCreated();
            //    if (this._RenderMode == Mode.Allways)
            //        SetTimer();
            //}
            //    return;
            //CheckError();
            if ((width == 0) || (height == 0)) return;
            MakeCurrent();
            Matrix old = EyeMatrix(new Size((int)ViewPort.Width, (int)ViewPort.Height), FieldOfView).invert();
            double Fa = ViewPort.Width / (float)width;
             double Fb = ViewPort.Height / (float)height;
            ViewPort = new Rectangle(0, 0, width, height);
            ShadowFBO.InitForDepth(ShadowSetting.Width, ShadowSetting.Height);
            CheckError();
            Matrix New=  EyeMatrix(new Size(width, height), FieldOfView);
          Matrix M=((Matrix.LookAt(Camera.Position, Camera.Position + Camera.Direction, Camera.UpVector))) ;//* Matrix.Scale(PerspektiveFactor, PerspektiveFactor, PerspektiveFactor));

          if (FieldOfView == 0) { Fa = 1; Fb = 1; }
          ProjectionMatrix = Matrix.Scale(Fa,Fb,1)*ProjectionMatrix*M.invert()* old*New*M;
         
            Camera.MakeConsistent();
            if (FirstViewPortChanged)
            {
                Camera.setDefZoom();
                FirstViewPortChanged = false;
                Shader = SmallShader;


               
               
                OnCreated();
                SizeChanged();
                if (this._RenderMode == Mode.Allways)
                    SetTimer();
            }
          
             try
            {
                if (ForegroundDrawEnable)
                {
                    BackBuffer.Init(width, height);
                    RefreshBackGround();
                    CopyFromBackGround();
                }
            }
            catch (System.Exception)
            {

                _ForegroundDrawEnable = false;
                
            }
        
            CheckError(); 
        }
    }
}