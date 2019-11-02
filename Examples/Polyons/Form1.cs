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

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Device.Offset = (double)trackBar1.Value / 50f;
        }
    }

    public class MyDevice:OpenGlDevice
    {

        internal double Offset = 0.1;
        Loxy G = null;
       
        Drawing3d.Font F = new Drawing3d.Font("Arial");
        protected override void OnCreated()
        {
            base.OnCreated();
            Loca L = F.GlyphInfo[(byte)'G'].Curves;
            G = L.ToLoxy();
            
            (G as ITransform2d).Transform(Matrix3x3.Scale(15, 15));
 
        }
       
        public override void OnPaint()
        {
            base.OnPaint();
            PushMatrix();
            MulMatrix(Matrix.Translation(-4, -4, 0));
        
         
            drawPolyPolyLine(G);
            //      PolygonMode = Drawing3d.PolygonMode.Line;
            Loxy GLo = G.GetOffset(Drawing3d.JoinType.jtRound, Drawing3d.EndType.etOpenRound, Offset + 0.1);
           // drawPolyPolyLine(G);
            Emission = Color.White;
            PolygonMode = Drawing3d.PolygonMode.Line;
            drawPolyPolyLine(GLo);
            Emission = Color.Black;
            PolygonMode = Drawing3d.PolygonMode.Fill;
            PopMatrix();
       }
       
        
    }
}
