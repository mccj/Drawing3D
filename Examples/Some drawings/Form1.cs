using System.Drawing;
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

        Drawing3d.Font F = new Drawing3d.Font("Courier");
        protected override void OnCreated()
        {
            base.OnCreated();
            BackColor = Color.WhiteSmoke;

        }
        public override void OnPaint()
        {
           base.OnPaint();
            Material = Materials.Bronze;

            drawSphere(new xyz(5, 0, 0), 3);
            
            Material = Materials.Copper;
            drawTorus(1, 4);
            Material = Materials.Emerald;
            drawCone(new xyz(-5, 0, 0), 3, 3, System.Math.PI / 4);
            Material = Materials.Gold;
            
            drawText(F,Matrix.Scale(4,4,4)* Matrix.Translation(-2, -1.8, 0), "Drawing3d",1);
        
        }
       
       
    }
}
