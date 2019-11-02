using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;

namespace SurfaceDocu
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
            comboBox1.SelectedIndex = 4;
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Device.ActiveSurface = (MyDevice.SurfaceKind)comboBox1.SelectedIndex;
       }
    }
    public class MyDevice : OpenGlDevice
    {
       public enum SurfaceKind
        {
                ArrayExtruder,
                BezierSurface,
                BsplineSurface,
                Cone,
                Curve2DRotator,
                Curve3dRotator,
                CurveExtruder,
                CustomSurface,
                Cylinder,
                NurbsSurface,
                OffsetSurface,
                PlaneSurface,
                SmoothPlane,
                Sphere,
                Torus,
           

        };
        CustomSurface CustomSurface = new CustomSurface();
       public SurfaceKind ActiveSurface = SurfaceKind.Torus;
        protected override void OnCreated()
        {
            base.OnCreated();
            DefaultLightPos = Lights[0].Position;
            BackColor = Color.White;
            CustomSurface.OnGetUDerivation += CustomSurface_OnGetUDerivation;
            CustomSurface.OnGetVDerivation += CustomSurface_OnGetVDerivation;
            CustomSurface.OnGetValue += CustomSurface_OnGetValue;
        }

        private xyz CustomSurface_OnGetValue(object sender, double u, double v)
        {
            double x = 5 * System.Math.Cos(u * 2*Math.PI);
            double y = 5 * System.Math.Sin(u * 2 * Math.PI);
            double z = 4 * v;
            return new xyz(x, y, z);
        }

        private xyz CustomSurface_OnGetUDerivation(object sender, double u, double v)
        {
            double x = -5 * System.Math.Sin(u * 2*Math.PI) ;
            double y = 5 * System.Math.Cos(u * 2 * Math.PI) ;
            double z = 4;
            return new xyz(x, y, z);
        }

        private xyz CustomSurface_OnGetVDerivation(object sender, double u, double v)
        {
            return (new xyz(0, 0, 1));
        }
        xyzwf DefaultLightPos = new xyzwf(0, 0, 0, 0);
        public override void OnPaint()
        {
            switch (ActiveSurface)
            {
                case SurfaceKind.ArrayExtruder:
                    xyArray A = new xyArray();
                    A.data = new xy[] { new xy(0, 0), new xy(0, 4), new xy(4, 4), new xy(4, 0),new xy(0,0) };
                    ArrayExtruder AE = new ArrayExtruder();
                    AE.Array = A;
                    AE.Height = 5;
                    AE.Paint(this);
                    break;
                case SurfaceKind.BezierSurface:
                    Lights[0].Position = DefaultLightPos;
                    BezierSurface Bezs = new BezierSurface();
                    Bezs.ControlPoints = new xyz[,] { { new xyz(0,0,0), new xyz(2, 0, 0), new xyz(4, 0, 0), new xyz(6, 0, 0) },
                                                   { new xyz(0,3,1), new xyz(2, 3, 3), new xyz(4, 3, 0), new xyz(6,3, 0) },
                                                   { new xyz(0,6,1), new xyz(2,6, 3), new xyz(4,6, 0), new xyz(6, 6, 0) },
                                                    { new xyz(0,9,0), new xyz(2,9, 0), new xyz(4, 9, 0), new xyz(6, 9, 0) },
                                                    };


                    Bezs.Paint(this);
                    break;
                case SurfaceKind.BsplineSurface:
                    BSplineSurface BS = new BSplineSurface();

                    Lights[0].Position = DefaultLightPos;
                    BS.ControlPoints = new xyz[,] { { new xyz(0,0,0), new xyz(0,3,0), new xyz(0,6,0), new xyz(0,9,0), },
                                                   { new xyz(2, 0, 1), new xyz(2, 3, 3), new xyz(2,6, 3), new xyz(2,9, 0), },
                                                   { new xyz(4, 0, 1), new xyz(4, 3, 3), new xyz(4,6,3),  new xyz(4, 9, 0),  },
                                                 { new xyz(6, 0, 0), new xyz(6,3, 0),  new xyz(6, 6, 0),  new xyz(6, 9, 0) },
                                              };

                    BS.UDegree = 3;
                    BS.VDegree = 2;
                    BS.SetDefaultKnots();

                  

                    BS.Paint(this);
                    break;
                case SurfaceKind.Cone:
                    Lights[0].Position = new xyzwf(5, 0, (float)2.5,1);
                    Cone Cone = new Cone(new xyz(0,0,0),5,2,System.Math.PI/4);
                    Cone.Paint(this);
                    break;
                case SurfaceKind.Curve2DRotator:
                       Lights[0].Position = DefaultLightPos;
                       Curve2dRotator C2d = new Curve2dRotator();
                       C2d.Curve = new Bezier(new xy(1, 0), new xy(2, 3), new xy(4, 3), new xy(6, 0)); ;
                       C2d.FromAngle = 0;
                       C2d.ToAngle = Math.PI / 2;
                 
                    C2d.Paint(this);
                    break;
                case SurfaceKind.Curve3dRotator:
                    Curve3dRotator C3d = new Curve3dRotator();
                    C3d.Curve = new Bezier3D(new xyz(1, 2,0), new xyz(2, 1,3), new xyz(4, 2,3), new xyz(5, 2,0));
                    C3d.Paint(this);
                    break;
                case SurfaceKind.CurveExtruder:
                    Lights[0].Position = new xyzwf(3,6,3,1);
                    CurveExtruder CE = new CurveExtruder();
                    CE.Curve = new Bezier(new xy(1, 0), new xy(2, 3), new xy(4, 3), new xy(6, 0));
                    CE.UpPlane = new Plane(new xyz(0, 0, 4), new xyz(0, 0.3, 1));
                    CE.DownPlane = new Plane(new xyz(0, 0, 0), new xyz(0, -0.3, 1));
                    CE.Height = -5;
                    CE.Paint(this);
                    break;
                case SurfaceKind.CustomSurface:
                    Lights[0].Position = new xyzwf(8, 1, 2, 1);
                    CustomSurface.Paint(this);
                    break;
                case SurfaceKind.Cylinder:
                    Lights[0].Position = new xyzwf(8, 1, 2, 1);
                    Cylinder Cylinder = new Cylinder(3, 5);
                    drawSurface(Cylinder);
                    break;
                case SurfaceKind.NurbsSurface:
                    NurbsSurface NS = new NurbsSurface();
                    Lights[0].Position = DefaultLightPos;
                    NS.ControlPoints = new xyz[,] {
                                          { new xyz(0,0,0), new xyz(0,3,0), new xyz(0,6,0), new xyz(0,9,0), },
                                                   { new xyz(2, 0, 1), new xyz(2, 3, 3), new xyz(2,6, 3), new xyz(2,9, 0), },
                                                   { new xyz(4, 0, 1), new xyz(4, 3, 3), new xyz(4,6,3),  new xyz(4, 9, 0),  },
                                                 { new xyz(6, 0, 0), new xyz(6,3, 0),  new xyz(6, 6, 0),  new xyz(6, 9, 0) },
                                              };

                    NS.UDegree = 3;
                    NS.VDegree = 2;
                    NS.Weights = new double[,] { { 1, 1, 1, 1 },
                                                 { 1, 3,3, 1 },
                                                 { 1, 3, 3, 1 },
                                                 { 1, 1, 1, 1 }
                                               };
                     NS.SetDefaultKnots();
                     NS.Paint(this);
                    break;
                case SurfaceKind.OffsetSurface:
                    Lights[0].Position = DefaultLightPos;
                    BSplineSurface _BS = new BSplineSurface();
                    _BS.ControlPoints = new xyz[,] { { new xyz(0,0,0), new xyz(0,3,0), new xyz(0,6,0), new xyz(0,9,0), },
                                                   { new xyz(2, 0, 1), new xyz(2, 3, 3), new xyz(2,6, 3), new xyz(2,9, 0), },
                                                   { new xyz(4, 0, 1), new xyz(4, 3, 3), new xyz(4,6,3),  new xyz(4, 9, 0),  },
                                                 { new xyz(6, 0, 0), new xyz(6,3, 0),  new xyz(6, 6, 0),  new xyz(6, 9, 0) },
                                              };

                    _BS.UDegree = 3;
                    _BS.VDegree = 2;
                    _BS.SetDefaultKnots();

                    OffsetSurface OS = new OffsetSurface();
                    OS.BasisSurface = _BS;
                    OS.Distance = 0.5;
                    OS.Paint(this);

                    
                    break;
                case SurfaceKind.PlaneSurface:
                    Lights[0].Position = new xyzwf(2,2,4,1);
                    PlaneSurface PS = new PlaneSurface(new xyz(0, 0, 0), new xyz(4, 2, 2), new xyz(1, 5, 7));
                    PS.Width = 4;
                    PS.Length  = 5;
                    PS.Paint(this);
                    break;
              
                case SurfaceKind.SmoothPlane:
                    Lights[0].Position = new xyzwf(2, 2, 4, 1);
                    SmoothPlane SP = new SmoothPlane(new xyz(0, 0, 0), new xyz(0, 5, 0), new xyz(5, 5, 0), new xyz(5, 0, 0), new xyz(-1, -1, 1), new xyz(-1, 1, 1), new xyz(1, 1, 1), new xyz(1, 1, 1));
                    SP.Width = 5;
                    SP.Length = 6;
                    SP.Paint(this);
                    break;
                case SurfaceKind.Sphere:
                    Lights[0].Position = DefaultLightPos;
                    Sphere Sphere = new Sphere(new xyz(2, 1, 1), 4);
                    drawSurface(Sphere);
                    
                    break;
              
                case SurfaceKind.Torus:
                    Lights[0].Position = DefaultLightPos;
                    Torus Torus = new Torus(1, 4);
                    drawSurface(Torus);
                    break;
                default:
                    break;
            }
            base.OnPaint();
        }
    }
   
}
