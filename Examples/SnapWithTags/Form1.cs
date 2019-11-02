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
       
        public override void OnPaint()
        {
         
            object Tag = null;
            if (SnappItems.Count > 0)
                Tag = SnappItems[0].Tag;

            PushTag("Kugel");
            if ((Tag != null) && ((string)Tag == "Kugel"))
            {

                Emission = Color.Red;
            }
           
            drawSphere(new xyz(0, 0, 3), 3);
            PopTag();
            Emission = Color.Black;
            PushTag("Box");
            if ((Tag != null) && ((string)Tag == "Box"))
            {
                Emission = Color.Blue;
            }
            drawBox(new xyz(-4, -4, -1), new xyz(8, 8, 1));
            PopTag();
            Emission = Color.Black;
        } 
    }
}
