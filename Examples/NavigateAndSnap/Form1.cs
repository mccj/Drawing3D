using System;
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
    class MySphere : Drawing3d.Entity
    {
        public MySphere(xyz Center)
        {
            this.Center = Center;
         }
        public xyz Center = new xyz(0, 0, 0);
        protected override void OnDraw(OpenGlDevice Device)
        {
           Device.drawSphere(Center, 0.5);
        }
    }
    public class MyDevice:OpenGlDevice
    {
        Drawing3d.Entity Scene = new Drawing3d.Entity();  
        protected override void OnCreated()
        {
         base.OnCreated();
         FieldOfView = Math.PI/6;
           for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                     
                        Scene.Children.Add(new MySphere(new xyz(i - 5, j - 5, k - 5)));
                    }
                }
            }

           
        }
        public override void OnPaint()
        {
            base.OnPaint();
            Scene.Paint(this);
            Emission = Color.Yellow;

            for (int i = 0; i < SnappItems.Count; i++)
                SnappItems[i].draw(this);
           Emission = Color.Black;
        }
    }
}
