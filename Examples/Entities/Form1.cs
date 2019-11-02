using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;

namespace Entitiesdocu
{

    [Serializable]
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
            Device.SetEntityKind((MyDevice.Entitykind)comboBox1.SelectedIndex);
        }
    }


    public class MyDevice : OpenGlDevice
    {


        BoxEntity Box = null;
        CurveExtruder CurveEx = null;
        PolyCurveExtruder PolyCurveEx = null;
        CoordinateAxis Axis = null;
        TextEntity Text = null;
        RectCtrlZoom Zoom = null;
        CtrlRectangle CRect = null;
        SphereEntity Sphere = null;
        Interpolator3D Interpolator = null;
        Profiler Profil = null;
     
        Cone Cone = null;
        ActiveCursor ActiveC = null;
        Arrow Arrow = null;
     
       public enum Entitykind {
            Boxentity,
            PolyCurveExtruder,
            CoordinateAxis,
            TextEntity,
            Interpolator,
            Zoom,
            CtrlRectangle,
            Profiler,
            ActiveCursor,
            SphereEntity,
            Cone,
            Arrow,
            VisualBase
        };

        Entitykind Entities = Entitykind.Zoom;
        xyArray Triangle = null;
        Entity S = null;
       public void SetEntityKind(Entitykind Kind)
        {

            Entities = Kind;
           
            if (Entities != Entitykind.CtrlRectangle)
                if (CRect != null)
                    CRect.Device = null;
            switch (Entities)

            {
                case Entitykind.Boxentity:
                    {
                        Box = new BoxEntity(new xyz(0, 0, 0), new xyz(4, 5, 6));
                       
                        break;
                    }
                
                case Entitykind.PolyCurveExtruder:
                    {
                        PolyCurveEx = new PolyCurveExtruder();
                        Loca _Loca = new Loca();
                        CurveArray Curves0 = new CurveArray();
                        Curves0.Count = 4;
                        Curves0[0] = new Line(new xy(-3, -3), new xy(-3, 4));
                        Curves0[1] = new Line(new xy(-3, 4), new xy(4, 4));
                        Curves0[2] = new Line(new xy(4, 4), new xy(4, -3));
                        Curves0[3] = new Line(new xy(4, -3), new xy(-3, -3));

                        _Loca.Add(Curves0);
                        OpenGlDevice.CheckError();
                        CurveArray Curves1 = new CurveArray();
                        Curves1.Count = 4;
                        Curves1[0] = new Line(new xy(0, 0), new xy(1, 0));
                        Curves1[1] = new Line(new xy(1, 0), new xy(1, 1));
                        Curves1[2] = new Line(new xy(1, 1), new xy(0, 1));
                        Curves1[3] = new Line(new xy(0, 1), new xy(0, 0));
                        _Loca.Add(Curves1);

                        PolyCurveEx.DownPlane = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                        PolyCurveEx.UpPlane = new Plane(new xyz(0, 0, 4), new xyz(0, 0.5, 1));
                        PolyCurveEx.Loca = _Loca;
                        PolyCurveEx.ShowUpPlane = true;
                        PolyCurveEx.ShowDownPlane = true;
                        PolyCurveEx.Height = -1;
                        PolyCurveEx.Direction = new xyz(0, 1, 1);
                        break;
                    }
                case Entitykind.CoordinateAxis:
                    {
                        Axis = new CoordinateAxis();
                        Axis.FullSize = true;
                        Axis.ShowText = true;
                        Axis.TextHeight = 1;
                        Axis.Color = Color.Black;
                        Axis.Size = new xyz(10, 20, 30);
                        Axis.Devider = new xyz(4, 4, 4);
                        Axis.DeviderLineLength = 0.3;
                        Axis.LeftAndRight = true;
                        Axis.Dim3d = true;
                        Axis.ShowDevider = true;
                        break;
                    }
                case Entitykind.TextEntity:
                    {
                        Text = new TextEntity();
                        Text.Text = "Drawing3d";
                    
                        Text.Size = 4;
                        Text.Italic = true;
                        Text.Position = new xyz(-5, 0, 0);
                        break;
                    }
                case Entitykind.Interpolator:

                    { 
                        Interpolator = new Interpolator3D();
                        Interpolator.Points = new xyz[,]
                             { 
                               
                            
                               {new xyz(-3,-3,0),new xyz(-3,0,2),new xyz(-3,3,0),new xyz(-3,6,-2) },
                               {new xyz(0,-3,2),new xyz(0,0,3),new xyz(0,3,2),new xyz(0,6,-2) },
                               {new xyz(3,-3,3),new xyz(3,0,2),new xyz(3,3,1),new xyz(3,6,-2) },
                               {new xyz(6,-3,2),new xyz(6,0,0),new xyz(6,3,0),new xyz(6,6,-2)  }
                            };

                        break;




                    };
                       

                         
                   
                case Entitykind.Zoom:

                    {
                        Zoom = new RectCtrlZoom(this);
                        Zoom.Device = this;
                        Zoom.Color = Color.White;
                        break;
                    }
                case Entitykind.CtrlRectangle:

                    {

                        CRect = new CtrlRectangle(this);
                        CRect.Color = Color.Black;
                        CRect.MarkerColor = Color.Red;
                        CRect.Creating = false;
                        CRect.Rectangle = new RectangleF(-1, -1, 6, 5);
                        CRect.CompileEnable = false;
                        Triangle = new xyArray();
                        Triangle.data = new xy[] { new xy(0, 0), new xy(2, 2), new xy(4, 0), new xy(0, 0) };
                        CRect.TransformItems.Add(Triangle);
                       
                        // S = new SphereEntity(new xyz(0, 0, 0), 2);
                        //S = new BoxEntity(new xyz(0, 0, 0), new xyz(4, 4, 4));
                        //S.CompileEnable = false;
                        // CRect.TransformItems.Add(S);
                       // CRect.LiveTransform = true;
                        break;
                    }
                case Entitykind.Profiler:

                    {
                        xyzArray A = new xyzArray();
                        A.data = new xyz[] { new xyz(-3, 0, 0), new xyz(-3, -4, 0), new xyz(3, -4, 0), new xyz(3, 0, 0) };
                        Loca _Loca = new Loca();
                        CurveArray Curves0 = new CurveArray();
                        Curves0.Count = 4;
                        Curves0[0] = new Line(new xy(-1, -1), new xy(-1, 2));
                        Curves0[1] = new Line(new xy(-1, 2), new xy(2, 2));
                        Curves0[2] = new Line(new xy(2, 2), new xy(2, -1));
                        Curves0[3] = new Line(new xy(2, -1), new xy(-1, -1));
                        _Loca.Add(Curves0);
                        OpenGlDevice.CheckError();
                        CurveArray Curves1 = new CurveArray();
                        Curves1.Count = 4;
                        Curves1[0] = new Line(new xy(0, 0), new xy(1, 0));
                        Curves1[1] = new Line(new xy(1, 0), new xy(1, 1));
                        Curves1[2] = new Line(new xy(1, 1), new xy(0, 1));
                        Curves1[3] = new Line(new xy(0, 1), new xy(0, 0));
                        _Loca.Add(Curves1);
                        Profil = new Profiler();
                        Profil.CompileEnable = false;
                        Profil.Trace = A;
                        Profil.Transverse = _Loca;
                        Profil.CloseFirst = true;
                        Profil.CloseLast = true;
                        break;
                    }
                case Entitykind.ActiveCursor:
                    {

                        //  ActiveC.CrossColor = Color.Red;
                        break;
                    }

                case Entitykind.SphereEntity:
                    {

                        Sphere = new SphereEntity(new xyz(3, 2, 1), 4);
                        //  Sphere.CompileEnable = false;
                        //  Sphere = new SphereEntity(new xyz(0, 0, 0), 4);
                        break;
                    }
                case Entitykind.Cone:
                    {
                        Cone = new Cone(3, 5);
                        break;
                    }
               
                case Entitykind.Arrow:
                    {
                        Arrow = new Arrow();
                       Arrow.Transformation = Matrix.Translation(new xyz(5, 0, 0));
                        Arrow.SetShaftAndTop(new xyz(-10, 0, 0), new xyz(0, 0, 0));
                        Arrow.Size = 8;
                        drawSphere(new xyz(0, 0, 0), 0.5);
                        break;
                    }


                 
            }


            Selector.RefreshSnapBuffer();

        }





        OpenGlDevice Device = null;
        Drawing3d.Font FO = null;
        protected override void OnCreated()
        {

            Device = this;
            FO = new Drawing3d.Font("Arial");
            ActiveC = new ActiveCursor();
            ActiveC.xAxisColor = Color.Red;
            ActiveC.yAxisColor = Color.Red;
            ActiveC.zAxisColor = Color.Red;
            ActiveC.PenWidth = 3;
            SetEntityKind(Entitykind.Boxentity);

           

        }




        Matrix Mod = Matrix.identity;
        xyz Pt1 = new xyz(0, 0, 0);
        xyz Pt2 = new xyz(0, 0, 0);
        public override void OnPaint()


        {
            BackColor = Color.White;
            switch (Entities)

            {
                case Entitykind.Boxentity:
                    {

                        
                        Box.Paint(this);


                        break;
                    }
              
                case Entitykind.PolyCurveExtruder:
                    {

                        PolyCurveEx.Paint(this);

                        break;
                    }
                case Entitykind.CoordinateAxis:
                    {

                        Axis.Paint(this);
                        break;
                    }
                case Entitykind.TextEntity:
                    {
                        Text.Paint(this);

                        break;
                    }
                case Entitykind.Interpolator:

                    {

                        Interpolator.Paint(this);
                        break;
                    }
                case Entitykind.Zoom:

                    {

                        drawSphere(new xyz(0, 0, 0), 7, 80, 80);
                        drawBox(new xyz(-10, -10, -1), new xyz(20, 20, 1));
                        drawBox(new xyz(0, 0, 0), new xyz(2, 2, 2));

                        break;
                    }
                case Entitykind.CtrlRectangle:

                    {
                        PenWidth = 2;
                        PolygonMode = PolygonMode.Line;
                        drawPolyLine(Triangle);
                        PolygonMode = PolygonMode.Fill;
                        PenWidth = 1;
                    
                        break;
                    }
                case Entitykind.Profiler:

                    {


                        Profil.Paint(this);


                        break;
                    }
                case Entitykind.ActiveCursor:
                    {
                        PolyCurveEx = new PolyCurveExtruder();
                        Loca _Loca = new Loca();
                        CurveArray Curves0 = new CurveArray();
                        Curves0.Count = 4;
                        Curves0[0] = new Line(new xy(-3, -3), new xy(-3, 4));
                        Curves0[1] = new Line(new xy(-3, 4), new xy(4, 4));
                        Curves0[2] = new Line(new xy(4, 4), new xy(4, -3));
                        Curves0[3] = new Line(new xy(4, -3), new xy(-3, -3));

                        _Loca.Add(Curves0);
                        OpenGlDevice.CheckError();
                        CurveArray Curves1 = new CurveArray();
                        Curves1.Count = 4;
                        Curves1[0] = new Line(new xy(0, 0), new xy(1, 0));
                        Curves1[1] = new Line(new xy(1, 0), new xy(1, 1));
                        Curves1[2] = new Line(new xy(1, 1), new xy(0, 1));
                        Curves1[3] = new Line(new xy(0, 1), new xy(0, 0));
                        _Loca.Add(Curves1);

                        PolyCurveEx.DownPlane = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                        PolyCurveEx.UpPlane = new Plane(new xyz(0, 0, 4), new xyz(0, 0.5, 1));
                        PolyCurveEx.Loca = _Loca;
                        PolyCurveEx.ShowUpPlane = true;
                        PolyCurveEx.ShowDownPlane = true;
                        PolyCurveEx.Height = -1;
                        PolyCurveEx.Direction = new xyz(0, 1, 1);
                        PolyCurveEx.Paint(this);

                        ActiveC.Paint(this);
                        break;
                    }


                case Entitykind.SphereEntity:
                    {


                        Sphere.Paint(this);
                        break;
                    }
                case Entitykind.Cone:
                    {

                        Cone.Paint(this);



                        break;
                    }
                case Entitykind.Arrow:
                    {
                   
                        Arrow.Paint(this);
                     
                        break;
                    }
                case Entitykind.VisualBase:
                    {
                        VisualBase VB = new VisualBase();
                        VB.SnappEnable = false;
                        VB.xAxis.ShaftColor = Color.Blue;
                        VB.yAxis.ShaftColor = Color.Green;
                        VB.zAxis.ShaftColor = Color.Yellow;
                        VB.xAxis.TopColor = Color.Red;
                        VB.yAxis.TopColor = Color.Red;
                        VB.zAxis.TopColor = Color.Red;
                        VB.Paint(this);
                        break;
                    }


            }
            ActiveC.Paint(this);

            base.OnPaint();

        }
    }
   }


