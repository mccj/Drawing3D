using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;
using Drawing3d;
namespace Sample
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        Drawing3d.Camera Camera
        {
            get { return MyDevice.CurrentDevice.Camera; }
        }
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
        }
        float PI = (float)System.Math.PI;
        private void Left_Click(object sender, EventArgs e)
        {
            Camera.Animated.LookRight(-PI / 30, 300, false);
  
        }

        private void Right_Click(object sender, EventArgs e)
        {


            Camera.Animated.LookRight(PI / 30, 300, false);
          
        }

        private void Up_Click(object sender, EventArgs e)
        {
            Camera.Animated.LookDown(-PI / 30, 300, false);
         
        }

        private void Down_Click(object sender, EventArgs e)
        {
            Camera.Animated.LookDown(PI / 30, 300, false);
         
        }

        private void Forward_Click(object sender, EventArgs e)
        {
            Camera.Animated.WalkForward(0.005);
       
        }

        private void Back_Click(object sender, EventArgs e)
        {
           
            Camera.Animated.WalkForward(-0.005);
         
        }

        private void RollUp_Click(object sender, EventArgs e)
        {
            Camera.Animated.RollRight(PI / 20, 100, false);
        
        }

        private void RollDown_Click(object sender, EventArgs e)
        {
            Camera.Animated.RollRight(-PI / 20, 100, false);
        
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Camera.Animated.StopAllAnimations();
        }

        

        
    }
    public class MyDevice:OpenGlDevice
    {
        Texture Floor = new Texture();
        public static MyDevice CurrentDevice = null;
        public override void OnPaint()
        {

            texture = Floor;
            Color SaveAmbient = Ambient;
            Ambient = Color.LightGray;
            drawBox(new xyz(-20, -20, -1), new xyz(40, 40, 1));
            Ambient = SaveAmbient;
            texture = null;
            Material = Materials.Copper;
            drawBox(new xyz(-8, -5, 0), new xyz(4, 3, 5));
            Material = Materials.Emerald;
            drawBox(new xyz(-6, 0, 0), new xyz(3, 5, 2));
            Material = Materials.Gold;
            drawBox(new xyz(4, -3, 0), new xyz(4, 3, 5));
            Material = Materials.Chrome;
            drawBox(new xyz(3, 5, 0), new xyz(3, 4, 6));
        

        }
        protected override void OnCreated()
        {
            base.OnCreated(); CurrentDevice = this;
            base.OnPaint();
            FieldOfView = Math.PI/6;
            Floor.LoadFromFile("bricks06.png");
            Floor.Mirrored = true;
            Camera.LookAt(new xyz(0, -20, 8), new xyz(0, 10, 4), new xyz(0, 0, 1));
        }
    }
}
