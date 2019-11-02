using System.Windows.Forms;
using Drawing3d;
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
        VisualBase VB = new VisualBase();
        protected override void OnCreated()
        {
            base.OnCreated();
            VB.Size = 4;
            VB.Thickness = 0.3;
            BackColor = System.Drawing.Color.White;
         }

        protected override void onMouseDown(MouseEventArgs e)
        {
            ForegroundDrawEnable = true;
            base.onMouseDown(e);
        }
        protected override void onMouseUp(MouseEventArgs e)
        {
            ForegroundDrawEnable = false;
            OutFitChanged = true;
            base.onMouseUp(e);
        }
        protected override void OnForegroundPaint()
        {
           
            VB.Paint(this);
          
        }
        public override void OnPaint()
        {
            base.OnPaint();
            drawSphere(new xyz(-6, 0, 0), 3);
            drawSphere(new xyz(0, 6, 0), 3);
            drawSphere(new xyz(6, 0, 0), 3);
            drawSphere(new xyz(0, -6, 0), 3);

        }
    }
}
