using System.Windows.Forms;
using Drawing3d;

namespace Normalmap
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
      
      
        public Texture Normal = null; // holds the normal information
        BoxEntity Box = new BoxEntity(new xyz(-3, -3, -3), new xyz(6, 6, 6));
        MaterialAnimator MaterialAnimator1 = new MaterialAnimator();
        MaterialAnimator MaterialAnimator2 = new MaterialAnimator();
        Mesh M = null;
        AnimatorSet AnimatorSet = new AnimatorSet();
        protected override void OnCreated()
        {
            BackColor = System.Drawing.Color.WhiteSmoke;
            
            Normal = new Texture();
            Normal.LoadFromFile("NormalMap.tif");
            Normal.NormalMap = true;

            MaterialAnimator1.FromMaterial = Materials.Gold;
            MaterialAnimator1.ToMaterial = Materials.Silver;
            MaterialAnimator1.Duration = 4000; // 4 seconds

            MaterialAnimator2.FromMaterial = Materials.Silver;
            MaterialAnimator2.ToMaterial = Materials.Gold;
            MaterialAnimator2.Duration = 4000; // 4 seconds



            AnimatorSet.Device = this;
            AnimatorSet.ChildAnimations.Add(MaterialAnimator1);
            AnimatorSet.ChildAnimations.Add(MaterialAnimator2);
            AnimatorSet.Repeating= - 1;
            AnimatorSet.Start();
          
           
            M = Box.getMeshes(this)[0];
            M.Texture = Normal; // sets the normal texture
            // Normalize the texture coordinates.
            for (int i = 0; i < M.TextureCoords.Length; i++)
            {
                if (M.TextureCoords[i].x != 0) M.TextureCoords[i].x = 1f;
                if (M.TextureCoords[i].y != 0) M.TextureCoords[i].y = 1f;
            }
            base.OnCreated();
        }

      

        public override void OnPaint()
        {
            // Animates
           if (AnimatorSet.Current==0)
            M.Material = MaterialAnimator1.Value;
            else
                M.Material = MaterialAnimator2.Value;
           // paints the box
            M.Paint(this);
            base.OnPaint();
        }
    }
}
