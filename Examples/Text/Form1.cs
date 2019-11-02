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
        Drawing3d.Font Arial = new Drawing3d.Font("Arial");
        protected override void OnCreated()
        {
            base.OnCreated();
            BackColor = Color.DarkKhaki;
            Camera.LookAt(new xyz(0, 0, 20), new xyz(0, 0, 0), new xyz(0, 1, 0));
            Arial.FontSize = 3;
        }
        public override void OnPaint()
        {
            base.OnPaint();
          
            double FirstLine = 2;
            xy Size = getEnvText(Arial, "Drawing 3d") ;
            drawText(Arial, new xyz(- Size.x / 2, FirstLine, 0), "Drawing 3d", 1);
            Size = getEnvText(Arial, "for") * 1;
            drawText(Arial,new xyz(- Size.x / 2, FirstLine - Size.y, 0), "for", 1);
            Size = getEnvText(Arial, "Windows") * 1;
            drawText(Arial, new xyz(-Size.x/2, FirstLine - 2 * Size.y, 0), "Windows", 1);

        }
    }
}
