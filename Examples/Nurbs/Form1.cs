using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace Sample
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        public static Form1 CurrentForm = null;
        public Form1()
        {
            InitializeComponent();
            CurrentForm = this;
            
            MyDevice.CurrentForm = this;
            Device.WinControl = panel1;
        }

        private void bSplineSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Device.TheNurbs = Device.BsplineSurface;
            label1.Text = "BsplineSurface";
        }

        private void sphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Device.TheNurbs= Device.Sphere;
            label1.Text = "Sphere";
        }

        private void torusToolStripMenuItem_Click(object sender, EventArgs e)
        {

          
            Device.TheNurbs = Device.Torus;
            Device.Selector.RefreshSnapBuffer();
            label1.Text = "Torus";
        }

        private void coneToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Device.TheNurbs = Device.Cone;
            Device.OutFitChanged=true;
            Device.Refresh();
            label1.Text = "Cone";
        }

        private void cylinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            Device.TheNurbs = Device.Cylinder;
            label1.Text = "Cylinder";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double Magnetism = (float)trackBar1.Value / 100.0;
          
            int i = Device.Marked.X;
            int j = Device.Marked.Y;
          xyz V=  Device.TheNurbs.ControlPoints[i, j];




          for (int l = 0; l < Device.TheNurbs.ControlPoints.GetLength(0); l++)
          {
              for (int m = 0; m < Device.TheNurbs.ControlPoints.GetLength(1); m++)
              {
                  if (Device.TheNurbs.ControlPoints[l, m].dist(V) <0.01)
                      Device.TheNurbs.Weights[l, m] = Magnetism;
              }
          }

            Device.TheNurbs.Invalid = true;
        }
    }
   

    class MyDevice : OpenGlDevice
    {
      public static Form1 CurrentForm = null;
        public System.Drawing.Point _Marked = new System.Drawing.Point(0, 0);
        public System.Drawing.Point Marked
        {
            get { return _Marked; }
            set
            {
                _Marked = value;
                if (value != new Point(-1,-1))
                setMarkToMagnetism();
            }


        }
        void setMarkToMagnetism()
        {
            Form1.CurrentForm.trackBar1.Value = (int)(TheNurbs.Weights[Marked.X, Marked.Y] * 100);
        }
        protected override void onMouseDown(MouseEventArgs e)
        {
            base.onMouseDown(e);
            if (SnappItems.Count > 0)
            {
                if (SnappItems[0].Tag is System.Drawing.Point)
                    Marked = (System.Drawing.Point)SnappItems[0].Tag;

            }
           
        }
     

        xyz[,] CtrlPoints = new xyz[,]
         {
            {new xyz(-5,-5,0),new xyz(-5,-4,1),new xyz(-4,-1,2),new xyz(-4,2,0) },
            {new xyz(-3,-5,1),new xyz(-2,-4,2),new xyz(-1,-1,2),new xyz(-2,2,1) },
            {new xyz(-1,-5,1),new xyz(0,-3,2),new xyz(1,0,2),new xyz(1,3,1) },
            {new xyz(2,-5,0),new xyz(3,-3,1),new xyz(4,-1,3),new xyz(3,4,0) }

         };
        public NurbsSurface TheNurbs = new NurbsSurface();
        public BSplineSurface _BsplineSurface = new BSplineSurface();
        public Sphere _Sphere = new Sphere();
        public Torus _Torus = new Torus();
        public Cone _Cone = new Cone();
        public Cylinder _Cylinder = new Cylinder();
        public NurbsSurface BsplineSurface=new NurbsSurface();
        public NurbsSurface Sphere=new NurbsSurface();
        public NurbsSurface Torus=new NurbsSurface();
        public NurbsSurface Cone=new NurbsSurface();
        public NurbsSurface Cylinder =new NurbsSurface();
       
         BSplineSurface CreateBsplineSurface()
        {
            BSplineSurface Result = new BSplineSurface();
            Result.ControlPoints = CtrlPoints; 
            Result.SetDefaultKnots();
          
            return Result;
        }
        protected override void OnCreated()
        {
            base.OnCreated();
            Navigating = true;
          
            TheNurbs = new NurbsSurface();
           
            _BsplineSurface = CreateBsplineSurface();
           
            BsplineSurface.AsNurb = _BsplineSurface;
            TheNurbs = BsplineSurface;
            _Sphere.Center = new xyz(4, 0, 0);
            _Sphere.Radius = 4;
          
            Sphere.AsNurb = _Sphere;
            _Torus.InnerRadius = 1;
            _Torus.OuterRadius = 5;
            Torus.AsNurb = _Torus;
           
            _Cone.HalfAngle = System.Math.PI / 6;
            _Cone.Radius = 4;
            _Cone.Height = 5;
            Cone.AsNurb = _Cone;
           
            _Cylinder.Radius = 4;
            _Cylinder.Height = 5;
            Cylinder.AsNurb = _Cylinder;
         
          
            CurrentForm.label1.Text = "BsplineSurface";
            Marked = new System.Drawing.Point(2, 2);
        }
        void DrawCtrlPoints()
        {
            int Snappedi = -1;
            int Snappedj = -1;
            if (SnappItems.Count > 0)
            {
                if (SnappItems[0].Tag is System.Drawing.Point)
                {
                    Snappedi = ((System.Drawing.Point)SnappItems[0].Tag).X;
                    Snappedj = ((System.Drawing.Point)SnappItems[0].Tag).Y;
                }
            }
            for (int i = 0; i < TheNurbs.ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < TheNurbs.ControlPoints.GetLength(1); j++)
                {
                    if ((i == Marked.X) && ((j == Marked.Y)))
                        Emission = System.Drawing.Color.Red;
                    if ((i == Snappedi) && ((j == Snappedj)))
                        Emission = System.Drawing.Color.Yellow;
                    PushTag(new System.Drawing.Point(i, j));
                    if (RenderKind == RenderKind.SnapBuffer)
                        // better to snap with a greather Radius
                        drawSphere(TheNurbs.ControlPoints[i, j], 0.4);
                    else
                        drawSphere(TheNurbs.ControlPoints[i, j], 0.2);
                    PopTag();
                    Emission = System.Drawing.Color.Black;
                }
            }

        }
        public override void OnPaint()
        {
            // TheNurbs is not snapable
            DrawCtrlPoints();
            if (RenderKind != RenderKind.SnapBuffer)
                TheNurbs.Paint(this);
          

        }
    }
}
