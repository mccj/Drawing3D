using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace Catmulldemo
{
    public partial class Form1 : Form
    {
       
        MyDevice Device = new MyDevice();
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {   if (Device.CatMull.VertexList.Count > 50000) return;
            Device.CatMull.CatMull();
            Device.CatMull.Invalid = true;
       }
       
       
    }
    public class MyDevice:OpenGlDevice
    {
    
       public DiscreteSolid CatMull = new DiscreteSolid();
        protected override void OnCreated()
        {
            Lights[0].Position = new xyzwf(4, 5, 12, 0);
            CatMull = new SolidBox(new xyz(-5,-5,-5),new xyz(10, 10, 10));
            BackColor = Color.White;
     
   
        }
        public override void OnPaint()
        {
           
                  CatMull.Paint(this);
        }


    }
}
