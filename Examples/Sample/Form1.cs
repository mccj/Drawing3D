using System.Windows.Forms;
using Drawing3d;
namespace Sample
{
    // don't forget to add a reference to drawing3d.dll
    public partial class Form1 : Form
    {
        // create a Device from type MyDevice
        MyDevice Device = new MyDevice();
        public Form1()
        {
            InitializeComponent();
            //assign this form as Wincontrol
            Device.WinControl = this;
           
         }
    }
    // define a new class, which inherits from Opengl
    public class MyDevice : OpenGlDevice
    {
      public override void OnPaint()
        {
           // paint a sphere
           drawSphere(4);
           
       }
    }
}

