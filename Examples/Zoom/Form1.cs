using Drawing3d;
using System.Windows.Forms;
namespace Sample
{
    public partial class Form1 : Form
    {
       
        MyDevice Device = new MyDevice();
        public Form1()
        {

            InitializeComponent();
            Device.WinControl = this;
        }
    }
    public class MyDevice:OpenGlDevice
    {
        RectCtrlZoom ZoomRect = new RectCtrlZoom();
      
             
        public override void OnPaint()
        {
           
            base.OnPaint();
          
            drawBox(new xyz(-6, 0, 0), new xyz(4, 4, 7));
            drawBox(new xyz(1, 0, 0), new xyz(4, 4, 5));
        }
        protected override void OnCreated()
        {
            base.OnCreated();
            ZoomRect = new RectCtrlZoom();
            ZoomRect.Color = System.Drawing.Color.White;
         
        }
        protected override void onMouseDown(MouseEventArgs e)
        {
           if ((WinControl as Form1).checkBox1.Checked)
            ZoomRect.OnLogin(this,0);
            base.onMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        protected override void onMouseUp(MouseEventArgs e)
        {
           
            ZoomRect.OnLogout(false);
            base.onMouseUp(e);
        }
    }
}
