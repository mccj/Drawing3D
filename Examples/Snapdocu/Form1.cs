using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace Snapdocu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
            comboBox1.SelectedIndex = 0;
        }
        public MyOpenGl Device = new MyOpenGl();
       private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Device.ActiveSnapKind = (MyOpenGl.Snapkind)comboBox1.SelectedIndex;
            Device.Selector.RefreshSnapBuffer();
        }
    }
    public class MyOpenGl:OpenGlDevice
    {
       public enum Snapkind
        {
            SurfaceSnapItem,
            LineSnapItem,
            PointSnapItem,
            CurveSnapItem,
            PolyCurveSnapItem,
            PolyPolyCurveSnapItem,
            TextSnapItem,
            MeshSnapItem,
            PolyLineSnapItem,
            CurveSnapItem3D,
            PolyLineSnapItem3D,
            PolyPolyLineSnapItem3D,
            PolyPolyLineSnapItem,
            SphereSnapItem,
            BoxSnapItem,
           
        };
       public Snapkind ActiveSnapKind = Snapkind.MeshSnapItem;
        ActiveCursor ACursor = new ActiveCursor();
       
        Drawing3d.Font Font = new Drawing3d.Font("Arial");
       
        protected override void OnCreated()
        {
            Lights[0].Position = new xyzwf(16, 5, 10, 1);
 
           
            ACursor.xAxisColor = Color.Red;
            ACursor.zAxisColor = Color.Green;
            ACursor.zAxisColor = Color.Blue;
            ACursor.PenWidth = 3;
            base.OnCreated();
        }
        public override void OnPaint()
        {
            BackColor = Color.White;
        
            switch (ActiveSnapKind)
            {
                case Snapkind.SurfaceSnapItem: // Problem Ränder
                    //if (SnappItems.Count >0)
                    //{
                    //    Emission = Color.Red;
                    //    SnappItems[0].DrawTriangleInfo();
                    //    Emission = Color.Black;
                    //    return;
                    //}
                    CurveExtruder CurveEx = new CurveExtruder();
                    CurveEx.Direction = new xyz(0,1, 1);
                     CurveEx.Height = -1;
                     CurveEx.Curve = new Bezier(new xy(9, 0), new xy(6, 3), new xy(3, 3), new xy(0, 0));
                    CurveEx.DownPlane = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                    CurveEx.UpPlane = new Plane(new xyz(0, 0, 0) + CurveEx.Direction * 7, CurveEx.Direction);
                   
                    CurveEx.Paint(this);
                    break;
                case Snapkind.LineSnapItem:
                   
                    drawCurve(new Line(new xy(0, 0), new xy(10, 0)));
                    break;
                case Snapkind.PointSnapItem:
                    drawPoint(new xyz(4, 3, 0), 0.2);

                    break;
                case Snapkind.CurveSnapItem:
                    Curve C = new Bezier(new xy(9, 0), new xy(6, 3), new xy(3, 3), new xy(0, 0));
                    PolygonMode = PolygonMode.Line;
                    Emission = Color.Black;
                    PenWidth = 3;
                    drawCurve(C);
                    PolygonMode = PolygonMode.Fill;
                    break;
                case Snapkind.PolyCurveSnapItem:

                    Lights[0].Position = new xyzwf(-10, -10, 10, 1);
                    CurveArray _Curves0 = new CurveArray();
                    PolygonMode = PolygonMode.Line;
                    _Curves0.Count = 4;
                    _Curves0[0] = new Line(new xy(-3, -3), new xy(-3, 4));
                    _Curves0[1] = new Line(new xy(-3, 4), new xy(4, 4));
                    _Curves0[2] = new Line(new xy(4, 4), new xy(4, -3));
                    _Curves0[3] = new Bezier(new xy(4, -3), new xy(3, -3.5), new xy(1, -3.5), new xy(-3, -3));
                   
                    drawPolyCurve(_Curves0);

                    PolygonMode = PolygonMode.Fill;
                   break;
                case Snapkind.PolyPolyCurveSnapItem:
                    Loca _Loca = new Loca();
                    CurveArray Curves0 = new CurveArray();
                    Curves0.Count = 4;
                    Curves0[0] = new Line(new xy(-3, -3), new xy(-3, 4));
                    Curves0[1] = new Line(new xy(-3, 4), new xy(4, 4));
                    Curves0[2] = new Line(new xy(4, 4), new xy(4, -3));
                    Curves0[3] = new Bezier(new xy(4, -3), new xy(3, -3.5), new xy(1, -3.5), new xy(-3, -3));
                   _Loca.Add(Curves0);
                    OpenGlDevice.CheckError();
                    CurveArray Curves1 = new CurveArray();
                    Curves1.Count = 4;
                    Curves1[0] = new Line(new xy(0, 0), new xy(1, 0));
                    Curves1[1] = new Line(new xy(1, 0), new xy(1, 1));
                    Curves1[2] = new Line(new xy(1, 1), new xy(0, 1));
                    Curves1[3] = new Line(new xy(0, 1), new xy(0, 0));
                    _Loca.Add(Curves1);
                  
                    PolygonMode = PolygonMode.Fill;
                    drawPolyPolyCurve(_Loca);
                    
                    break;
                case Snapkind.TextSnapItem:
                   
                   
                    Font.FontSize = 4; 
                    xy Pos= getEnvText(Font, "Drawing 3D");
                    drawText(Font, new xyz(-Pos.X/2,0,0), "Drawing 3D", 2);
                   
                    break;
                case Snapkind.MeshSnapItem:

                    int[] Indices = new int[] { 0, 1, 2 };
                    xyzf[] Position = new xyzf[] { new xyzf(0, 0, 0), new xyzf(3, 5, 0), new xyzf(6, 0, 0) };
                    xyzf[] Normal = new xyzf[] { new xyzf(0, 0, 1), new xyzf(0, 0, 1), new xyzf(0, 0, 1) };
                    xyf[] Texture = new xyf[] { new xyf(0, 0), new xyf(3, 5), new xyf(6, 0) };
                    xyzf[] Colors = new xyzf[] { new xyzf(1, 0, 0), new xyzf(0, 1, 0), new xyzf(0, 0, 1) };
                   
                   drawMesh(Indices, Position, Normal, null, Colors);
                    break;
                case Snapkind.PolyLineSnapItem:

                    xyArray Poly = new xyArray();
                  
                    Poly.data = new xy[] { new xy(1, 1), new xy(3, 4), new xy(5, 4), new xy(6, 2), new xy(1, 1) };
                    PolygonMode = PolygonMode.Fill;
                    PenWidth = 3;
                  
                     drawPolyLine(Poly);
                    PolygonMode = PolygonMode.Fill;
                   
                   
                    break; 
                case Snapkind.CurveSnapItem3D:
 
                    Bezier3D Bezier = new Bezier3D(new xyz(1, 2, 1), new xyz(3, -2, 3), new xyz(5, 3, 2), new xyz(7,1,1));

                    drawCurve(Bezier);
                  

                    break;
                case Snapkind.PolyLineSnapItem3D:
                    xyzArray Poly3d = new xyzArray();
                    Poly3d.data = new xyz[] { new xyz(0, 0, 1), new xyz(0, 3, 1), new xyz(3, 3, 1), new xyz(4, 0 ,1), new xyz(0, 0, 1) };
                    drawPolyLine(Poly3d);
                   break;
                case Snapkind.PolyPolyLineSnapItem3D:
                   
                    xyzArray AA = new xyzArray();
                    AA.Add(new xyz(1, 1, 1));
                    AA.Add(new xyz(0, 1, 1));
                    AA.Add(new xyz(0, 0, 1));
                    AA.Add(new xyz(1, 0, 1));
                    AA.Add(new xyz(1, 1, 1));
                    xyzArray BB = new xyzArray();
                   
                    BB.Add(new xyz(3, 3, 1));
                    BB.Add(new xyz(3, -2, 1));
                    BB.Add(new xyz(-1, -2, 1));
                    BB.Add(new xyz(-1, 3, 1));
                    BB.Add(new xyz(3, 3, 1));



                    Loxyz L = new Loxyz();
                    L.Add(AA);
                    L.Add(BB);

                   
                    drawPolyPolyLine(L);
                 
                    break;
                case Snapkind.PolyPolyLineSnapItem:

                    xyArray _AA = new xyArray();
                    _AA.Add(new xy(2, 0.5));
                    _AA.Add(new xy(0, 0.5));
                    _AA.Add(new xy(0, 0));
                    _AA.Add(new xy(2, 0));
                    _AA.Add(new xy(2, 0.5));
                    xyArray _BB = new xyArray();

                    _BB.Add(new xy(4, 4));
                    _BB.Add(new xy(4, -2));
                    _BB.Add(new xy(-1, -2));
                    _BB.Add(new xy(-1, 4));
              
                    _BB.Add(new xy(4, 4));
                    Loxy _L = new Loxy();
                    _L.Add(_AA);
                    _L.Add(_BB);


                    drawPolyPolyLine(_L);

                    break;
                    
                case Snapkind.SphereSnapItem:
                   drawSphere(new xyz(2,1,1),3);


                    break;
                case Snapkind.BoxSnapItem:
 
                    drawBox(new xyz(0, 0, 0), new xyz(4, 3, 4));
                    break;
                default:
                    break;
            
            }
            ACursor.Paint(this);
        }

    }
  
}
