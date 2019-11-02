using System;
using System.Windows.Forms;
using Drawing3d;
namespace Sample
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        void ToFields()
        {
            tbDark.Text = Device.ShadowSetting.DarknessPercentage.ToString();
            tbImageSize.Text = Device.ShadowSetting.Width.ToString();
            tbLight.Text = Device.Lights[0].Position.ToString();
            tbSmooth.Text = Device.ShadowSetting.Smoothwidth.ToString();
            tbSampling .Text = Device.ShadowSetting.Samplingcount.ToString();
          


        }
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = panel2;
            ToFields();
        }
        void fromFields()
        {
            Device.ShadowSetting.DarknessPercentage = Convert.ToDouble(tbDark.Text);
            Device.ShadowSetting.Width = Convert.ToInt32(tbImageSize.Text);
            Device.ShadowSetting.Height = Convert.ToInt32(tbImageSize.Text);
            string[] s = tbLight.Text.Split(Utils.Delimiter);
            double x = Convert.ToDouble(s[0]);
            double y = Convert.ToDouble(s[1]);
            double z = Convert.ToDouble(s[2]);
            double w = Convert.ToDouble(s[3]);
          
            Device.Lights[0].Position = new xyzwf((float)x, (float)y, (float)z, (float)w);
            Device.ShadowSetting.Smoothwidth =(float) Convert.ToDouble(tbSmooth.Text);
            Device.ShadowSetting.Samplingcount = Convert.ToInt32(tbSampling.Text);
            
        }
        private void tbOk_Click(object sender, EventArgs e)
        {
            fromFields();
            Device.ShadowDirty = true;
            ToFields();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Device.Shadow = true;
            }
            else
            {
                Device.Shadow = false;
            }
        }

        private void tbSmooth_MouseDown(object sender, MouseEventArgs e)
        {
            if (Device.Shader!= Device.LargeShader)
            { MessageBox.Show("Not implemented in SmallShader set ShaderKind to LargeShader"); }
        }

        private void tbSampling_MouseDown(object sender, MouseEventArgs e)
        {
            if (Device.Shader != Device.LargeShader)
            { MessageBox.Show("Not implemented in SmallShader set ShaderKind to LargeShader"); }

        }
    }
    public class MyDevice:OpenGlDevice
    {
       
        protected override void OnCreated()   
        {
            base.OnCreated();
            Shadow = true;   

            Shader = LargeShader;

        }
        
        public override void OnPaint()
        {
           base.OnPaint();
            Material = Drawing3d.Materials.Chrome;
            drawBox(new xyz(-10, -10, -3), new xyz(20, 20, 3));
            drawBox(new xyz(4, 1, 0), new xyz(4, 6, 6));
            Material = Drawing3d.Materials.Copper;
            drawSphere(new xyz(0, 0, 0), 7,80,80);
            Material = Drawing3d.Materials.Chrome;
        }
    }
}
