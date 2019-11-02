using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
using System.Collections.Generic;
using Drawing3d.Windows;

#if LONGDEF
#else
using IndexType = System.UInt16;
#endif
namespace Sample

{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        D3DSceneAnimator Animator = null;
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
            Device.Culling = false;
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
           
          
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            string[] _Formats = Drawing3d.Reader.GetSupportedImportFormats();

            string F = "All Graphic files (";
            for (int i = 0; i < _Formats.Length; i++)
            {
                F += "*" + _Formats[i];
                if (i + 1 < _Formats.Length)
                    F += ";";
                else F += ")|";
            }
            for (int i = 0; i < _Formats.Length; i++)
            {
                F += "*" + _Formats[i];
                if (i + 1 < _Formats.Length)
                    F += ";";
            }
            openFileDialog1.Filter = F;
            btnContinue.Left = btnStopanimation.Left= btnAnimate.Left;

        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            if (Animator == null)
                Animator = new D3DSceneAnimator(Device, Device.MSC);
            Animator.Start();
            btnAnimate.Visible = false;
            btnStopanimation.Visible = true;
        }

        private void btnStopanimation_Click(object sender, EventArgs e)
        {
            if (Animator != null) Animator.End();
            btnStopanimation.Visible = false;
            btnContinue.Left = btnStopanimation.Left;
            btnContinue.Visible = true;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
           
            btnContinue.Left = btnStopanimation.Left;
            btnStopanimation.Visible = true;
            btnContinue.Visible = false;
            if (Animator != null) Animator.Continue();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Device.MSC.Animations == null) return;
            lbAnimations.Items.Clear();
            if (Device.MSC.HasAnimations)
            {
                
                for (int i = 0; i < Device.MSC.Animations.Count; i++)
                {
                    lbAnimations.Items.Add(Device.MSC.Animations[i].Name + " : " + (Device.MSC.Animations[i].DurationInTicks / Device.MSC.Animations[i].TicksPerSecond).ToString("0.##" + " sec"));
                    
                }
            }
            lbAnimations.Height = Device.MSC.Animations.Count * 14+14; 
            lbAnimations.Show();
        }

        private void lbAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Animator == null)
                Animator = new D3DSceneAnimator(Device, Device.MSC);
            //Device.MSC.SceneAnimator.Loop = false;
            //Device.MSC.SceneAnimator.ActiveAnimation = -1;
            Animator.Start(lbAnimations.SelectedIndex);
            lbAnimations .Visible = false;
            btnAnimate.Visible = false;
            btnStopanimation.Visible = false;
            btnContinue.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Device.ResetShadow();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;// "C:\\D3d2\\Windows\\Examples\\Assimp\\bin\\Debug\\ArmyPilot\\ArmyPilot.ms3d";
                Text = FileName;
                if (Device.MSC != null) Device.MSC.Dispose();
                Device.DisposeTextures();
               
                Device.MSC = Reader.FromFile(FileName);
                //Device.Image.Children.Clear();
                //for (int i = 0; i < MS.Meshes.Count; i++)
                //{
                //    Device.Image.Children.Add(MS.Meshes[i]);
                //}
                //Box b = Device.Image.GetMaxBox();

                //Device.fitToPage(Device, Device.Image.GetMaxBox());

                //GC.Collect();
                //return;
              
                if (Device.MSC == null) return;
               Box b = Device.MSC.GetMaxBox();

                Device.fitToPage(Device, Device.MSC.GetMaxBox());
                   
                if (Animator != null)
                {
                    Animator.End();
                    Animator = null;
                }
                btnAnimate.Visible = false;
                btnStopanimation.Visible = false;
                btnContinue.Visible = false;
                if (Device.MSC.HasAnimations)
                {
                    btnAnimate.Visible = true;
                    button2.Visible = true;
                   
                }
                else
                {
                    btnAnimate.Visible = false;
                    button2.Visible = false;

                }
                Device.ResetShadow();
            }
        }     
    }

    public class MyDevice:OpenGlDevice
    {
        public Box _Box;
        public Entity Image = new Entity();
        public void fitToPage(OpenGlDevice Device, Box Box)
        {
            _Box = Box;
            double Angle = Device.FieldOfView;
            xyz Center = Box.Origin + Box.Size * (0.5);
            double diag = (Box.Size * (0.5)).length();
            double Max = System.Math.Max(System.Math.Max(Box.Size.X, Box.Size.y), Box.Size.Z);
            double Min = System.Math.Min(System.Math.Min(Box.Size.X, Box.Size.y), Box.Size.Z);
            Device.PixelsPerUnit =Device.ViewPort.Height /  Max/2;
          
            xyz Position = Center + new xyz(0, 0, diag* 3);
            Device.Camera.LookAt(Position, Center, new xyz(0, 1, 0));
            Camera.Anchor = Center;
      
            Camera.NearFar(diag, 3*diag );
            xyz F = Box.Origin + new xyz(2 * diag, diag, 4*Max);
            Device.Lights[0].Position = new xyzwf((float)(F.X), (float)F.y, (float)F.Z,1);
            Device.FieldOfView = Angle;
            Image.Invalid = true;

        }
        public  Scene MSC =null;    
        protected override void OnCreated()
        {
            base.OnCreated();
           
           
             BackColor = Color.Gray;
           
             FieldOfView = Math.PI/6;
         
        }
     
        public override void OnPaint()
        {
          //  Image.Paint(this);
            if (MSC != null)
                MSC.Paint(this);
           
           
            base.OnPaint();
        }
    }
}
