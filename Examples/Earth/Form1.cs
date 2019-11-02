using System.Windows.Forms;
using Drawing3d;
using System;

namespace Sample
{
    public partial class Form1 : Form
    {
        public static Form1 CurrentForm = null;
        MyDevice Device = new MyDevice();
        xyzAnimator Animator = new xyzAnimator();
        public Form1()
        {
            InitializeComponent();
            CurrentForm = this;
            Device.WinControl = panel1;
         
            // Animator settings
            Animator.Device = Device;
            Animator.Animate += Animator_Animate;

        }
        internal void toFields(xyz value)
        {
            if ((Latitude.Focused) || (Longitude.Focused) || (Refresh.Focused)) return;
            Latitude.Text = System.Math.Round(value.y * 180 / System.Math.PI, 0).ToString();
            if (!Latitude.Focused)
                Longitude.Text = System.Math.Round(value.x * 180 / System.Math.PI, 0).ToString();

        }
        void Animator_Animate(object sender, EventArgs e)
        {
            Device.Camera.Angles = Animator.Value;
        }
        xyz fromFields()
        {
            double x = 0;
            double y = 0;
            try
            {
                x = (System.Math.PI * Convert.ToDouble(Longitude.Text)) / 180.0;
                y = (System.Math.PI * Convert.ToDouble(Latitude.Text)) / 180.0;

            }
            catch (Exception)
            {
                return new xyz(0, 0, 0);
            }

            return new xyz(x, y, 0);
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            xyz B = Device.Camera.Angles;
            Animator.From = new xyz(B.x, B.y, B.z);
            Animator.To = fromFields();
            Animator.Duration = 1000;
            Animator.Start();
            Device.Camera.Angles = fromFields();
        }
    }
    public class MyDevice : OpenGlDevice
    {
        Texture EarthTexture = null;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Form1.CurrentForm.toFields(Camera.Angles);
        }
        protected override void OnCreated()
        {
            base.OnCreated();
            EarthTexture = new Texture();

            Ambient = System.Drawing.Color.LightGray;

            this.NavigateKind = NavigationKind.ZoomRotate;
            Matrix M = Matrix.Rotation(new LineType(new xyz(0, 0, 0), Camera.Base.BaseX), System.Math.PI / 2);
            Camera.Base = M * Camera.Base;
            M = Matrix.Rotation(new LineType(new xyz(0, 0, 0), Camera.Base.BaseY), System.Math.PI);
            Camera.Base = M * Camera.Base;
            FieldOfView = 0;
            Camera.SetRelativeSystem();

            // zuerst x-Drehung um PI/2
            // 1 / 0 / 0;             0 / 0 / 1;          0 / -1 / 0
            // dann y-Drehung um PI ( wegen texture,die bei 180° beginnt )
            // -1 / 0 / 0;            0 / 0 / 1;          0 /  1 / 0
            EarthTexture.LoadFromFile("earthmap1k.jpg");

        }

        public override void OnPaint()
        {
            texture = EarthTexture;
            drawSphere(new xyz(0, 0, 0), 8);
           
        }
    }
}

