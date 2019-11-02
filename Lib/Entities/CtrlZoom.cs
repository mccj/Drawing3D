using System.Drawing;
using System;
using System.Windows.Forms;

namespace Drawing3d
{
    /// <summary>
    /// is a ver helpful class. It will be created a rectangle and than the backgroud zoomed.
    /// </summary>
    [Serializable]
    public class RectCtrlZoom : CtrlEntity
    {
        enum State
        {
            FirstPoint,
            SecondPoint
        }
        State CurrentState = State.FirstPoint;
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public RectCtrlZoom() : base()
        { CurrentState = State.FirstPoint; }
       /// <summary>
       /// is a constructor with a base and a device
       /// </summary>
       /// <param name="Base">base, which you can set.</param>
       /// <param name="Device">the current device.</param>
        public RectCtrlZoom(Base Base, OpenGlDevice Device) : base(Base, Device)
        {
            CurrentState = State.FirstPoint;
        }
        /// <summary>
        /// is a constructor with  a device
        /// </summary>
        /// <param name="Device">the current device.</param>
        public RectCtrlZoom(OpenGlDevice Device) : base(Device)
        {
            CurrentState = State.FirstPoint;
        }
       
        xy A = new xy(0, 0);
        xy B = new xy(0, 0);
        /// <summary>
        /// gets and sets the width of the pen. See also <see cref="OpenGlDevice.PenWidth"/>
        /// </summary>
        public float PenWidth = 1;
        Color _Color = Color.White;
        /// <summary>
        /// gets and sets the color of the pen.
        /// </summary>
        public Color Color
        {
            get { return _Color; }
            set { _Color = value;
 
               }
        }
        double _ZoomFactor = 1.05;
        /// <summary>
        /// gets the zoom factor.
        /// </summary>
        public double ZoomFactor
        {
            get { return _ZoomFactor; }
           
        }
        Matrix ForegroundMatrix()
        {  
            if (Device == null) return Matrix.identity;
            if (Device.ForegroundDrawEnable)
                return Matrix.Translation(-1, 1, 0) * Matrix.identity * Matrix.Scale(1f / (Device.ViewPort.Width / 2), -(1f / (Device.ViewPort.Height / 2)), -1f / (Device.ViewPort.Width / 2));
            else
                return Matrix.scale(new xyz(1, -1, 1)) * Matrix.Translation(-1, 1, 0) * Matrix.identity * Matrix.Scale(1f / (Device.ViewPort.Width / 2), -(1f / (Device.ViewPort.Height / 2)), -1f / (Device.ViewPort.Width / 2));
        }
        double Aspect
        { get { return (float)Device.ViewPort.Height / (float)Device.ViewPort.Width; } }
        /// <summary>
        /// when is <b>true</b> the aspectratio of the rectangle is given by the viewport width and height.
        /// </summary>
        public bool UseViewPortAspect = false;

       /// <summary>
       /// overrides the <see cref="CtrlEntity.OnMouseUp(HandledMouseEventArgs)"/> method.
       /// </summary>
       /// <param name="e"></param>
       /// <returns></returns>
        public override void OnMouseUp(HandledMouseEventArgs e)
        {
          
            if (escaped)
            { escaped = false;

                { e.Handled = true; return; }
            }
            if (A.dist(B) < 20)
            { e.Handled = true; return; }
            RectangleF R = Utils.ToRectangle(A, B);
            double Factor = (float)Device.WinControl.ClientSize.Width / R.Width;

            Device.Camera.ZoomTransform(new Point((int)(R.X + R.Width / 2), (int)(R.Y + R.Height / 2)), new Point((int)Device.ViewPort.Width / 2, (int)Device.ViewPort.Height / 2), Factor);
            Device.Selector.RefreshSnapBuffer();
            CurrentState = State.FirstPoint;
            A = B;
            Device.ForegroundDrawEnable = false;
           
            Device.OutFitChanged = true;
            Device = null;
            base.OnMouseUp(e);
            return;


        }

        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnMouseMove(HandledMouseEventArgs)"/> method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnMouseMove(HandledMouseEventArgs e)
        {
            if (escaped)
            { e.Handled = true; return; }
            if (CurrentState == State.SecondPoint)
            {
                xy _B = new xy(Device.MousePos.X, Device.MousePos.Y);
                double sgn = 1;
                if (_B.Y - A.Y < 0) sgn = -1;
             if (UseViewPortAspect)
                B = new xy(_B.X, A.Y + sgn * System.Math.Abs(_B.X - A.X) * Aspect);
                else
                B= new xy(Device.MousePos.X, Device.MousePos.Y);
                { e.Handled = true; return; }
              
            }
            if (CurrentState == State.FirstPoint)
            {
                A = B = new xy(Device.MousePos.X, Device.MousePos.Y);

                { e.Handled = true; return; }
            }
          
        }
      
        /// <summary>
        /// overrides the <see cref="Entity.OnForegroundDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        public override void OnForegroundDraw(OpenGlDevice Device)
        {

            Matrix P = Device.ProjectionMatrix;
            Device.ProjectionMatrix = ForegroundMatrix();
            float pw = Device.PenWidth;
            Device.PenWidth = PenWidth;
            Color C = Device.Emission;
            Device.Emission = Color;

            Device.drawLine(new xyz(A.x, A.Y, Device.ViewPort.Width / 2),
                              new xyz(A.x, B.Y, Device.ViewPort.Width / 2));
            Device.drawLine(new xyz(A.x, B.Y, Device.ViewPort.Width / 2),
                                         new xyz(B.x, B.Y, Device.ViewPort.Width / 2));
            Device.drawLine(new xyz(B.x, B.Y, Device.ViewPort.Width / 2),
                                         new xyz(B.x, A.Y, Device.ViewPort.Width / 2));

            Device.drawLine(new xyz(B.x, A.Y, Device.ViewPort.Width / 2),
                                       new xyz(A.x, A.Y, Device.ViewPort.Width / 2));
            Device.PenWidth = pw;
            Device.Emission = C;
            
            Device.ProjectionMatrix = P;
        }
        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnLogout(bool)"/> method.
        /// </summary>
        /// <param name="KeepThedevice"><b>true</b> meens, that the field <b>Device</b> not null will be setted.</param>
        public override void OnLogout(bool KeepThedevice)
        {
            A = B;
            CurrentState = State.FirstPoint;
            base.OnLogout(KeepThedevice);
        }
        bool escaped = false;
        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnKeyDown(KeyEventArgs)"/> method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnKeyDown(KeyEventArgs e)
        {  if (e.KeyCode == Keys.Escape)
            {
                A = B;
                CurrentState = State.FirstPoint;
                escaped = true;
                e.Handled = true; return;
            }
        
            base.OnKeyDown(e);
            return;
        }
        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnMouseDown(HandledMouseEventArgs)"/> method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnMouseDown(HandledMouseEventArgs e)
        {
            base.OnMouseDown(e);
          
            if (CurrentState == State.FirstPoint)
            {
                A = B = new xy(Device.MousePos.X, Device.MousePos.Y);
            }
            Device.ForegroundDrawEnable = true;
            CurrentState = State.SecondPoint;
            e.Handled = true; return; 
        }
    }
}
