using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace Shaders
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        FrmShader ShaderForm = new FrmShader();
      
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
            ShaderForm.Device = Device;
            ShaderForm.Show();
        }
    }
    public class MyDevice:OpenGlDevice
    {
        Texture EarthTexture = new Texture();
        GLShader SaveShader = null;
        Sphere S = new Sphere(3);
        protected override void OnCreated()
        {
            base.OnCreated();
         
            SaveShader = Shader;
            EarthTexture.LoadFromFile("earthmap1k.jpg");
            Shadow = true;
        
         
        }
        GLShader ExternShader = null;
        internal void ActivateShader(string Fragment, string Vertex)
        {
            if ((ExternShader != null) && (ExternShader.Handle > 0))
                ExternShader.Dispose();
            
            try
            {
                ExternShader = new GLShader(Fragment, Vertex,this);
              
                Shader = ExternShader;
              
            }
            catch (Exception E)
            {

              
                Shader = SaveShader;
              
            }
            Shader.UpDateAllVars();
            Refresh();
           


        }
        public override void OnPaint()
        {
           base.OnPaint();
            Color SaveAmient = Ambient;
            Ambient = Color.FromArgb(255,50,50,50);
            drawBox(new xyz(-10, -10, -4), new xyz( 20, 20, 1));
            Ambient = SaveAmient;
            texture = EarthTexture;
            S.Paint(this);
            
             texture = null;
        }
       
        
    }
}
