
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace HighMap
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
        }

        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButton1.Checked)
            { Device.setHeightMapAsBitMap(); }
        }

        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButton2.Checked)
            { Device.setHeightMapAsTesselation(); }
        }
    }
    public class MyDevice:OpenGlDevice
    {
        public Terrain terrain = new Terrain();
        Bitmap HighMap = null;
        Texture Height = null;
        Texture TextureForHeightMap = new Texture();
        Texture Snow = new Texture();
        public void setHeightMapAsTesselation()
        {
            terrain.VAODispose();
            terrain.Width = 40;
            terrain.Height = 40;
            terrain.UResolution = 40;
            terrain.VResolution = 40;
            terrain.Origin = new xyzf(-20, -20, 0);
            terrain.TessDisplacementMap = Height;
            terrain.Texture = Snow;
            terrain.TessDispFactor = 3;
            

        }
            public void setHeightMapAsBitMap()
        {
            terrain.TessDisplacementMap = null;
            terrain.Width = 20;
            terrain.Height = 20;
            terrain.Origin = new xyzf(-10, -10, 0);
            terrain.HightScale = 200; // scales the height of the Bitmap by dividing.
            terrain.SetHighMap(HighMap); // sets the Height 
            terrain.Texture = TextureForHeightMap;
            terrain.VAODispose();
            terrain.ActivateVao();

        }
        protected override void OnCreated()
        {
            BackColor = Color.WhiteSmoke;
            Snow.LoadFromFile("snowrocks.png");
            Height = new Texture("heightmap-1024x1024.png");
            // from https://github.com/hardware/TerrainTessellation
            HighMap = Bitmap.FromFile("Metal_Grill_006_height.png") as Bitmap;
            TextureForHeightMap.LoadFromFile("Metal_Grill_006_ambientOcclusion.jpg");
            terrain.SnappEnable = false;   // calculating the snapinfos needs a lot of time.
            base.OnCreated();
        }
        public override void OnPaint()
        {
            terrain.Paint(this);
        
        }
    }
}
