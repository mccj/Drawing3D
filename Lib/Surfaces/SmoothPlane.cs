using System;
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// is quasi the same as the <see cref="PlaneSurface"/>. The smoothplane can moreover seems smoothy. You can set normalvectors, so you get a "smoothy" light on the plane.
    /// </summary>
    [Serializable]
    public class SmoothPlane : PlaneSurface
    {
        xyz BaryCentric(xy A, xy B, xy C, xy Pt)
        {
            double F = (B - A) & (C - A);
            double z = ((Pt - A) & (Pt - B));
            double x = ((Pt - B) & (Pt - C));
            double y = ((Pt - C) & (Pt - A));
            return new xyz(x / F, y / F, z / F);
        }
        bool _Smooth = true;
        /// <summary>
        /// tis property activates the smoothness. The default is <b>true</b>.
        /// </summary>
        public bool Smooth
        {
            get { return _Smooth; }
            set { _Smooth = value;
                   Invalid = true;
                }
        }
        int Inside(xy value)
        {
            if ((((value - A01) & (value - A11)) > -0.0001) &&
               (((value - A11) & (value - A00)) > -0.0001) &&
               (((value - A00) & (value - A01)) > -0.0001))
                return 2;
            else
                return 1;


        }
        xyzf A;
        xyzf B;
        xyzf C;
        xyzf D;
        /// <summary>
        /// overrides the <see cref="PlaneSurface.Normal(double, double)"/> method.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>the normal vector.</returns>
        public override xyz Normal(double u, double v)
        {
           
            xy value = new xy(u, v);
            double dir = 1;
            if (!SameSense) dir = -1;
             u = value.x;
            v = value.y;
            if (!Smooth)
                return base.Normal(u, v);
            if (value.dist(A11) < 0.001)
                return N11.normalized() * dir;
            if (value.dist(A01) < 0.001)
                return N01.normalized() * dir;
            if (value.dist(A10) < 0.001)
                return N10.normalized() * dir;
            if (value.dist(A00) < 0.001)
                return N00.normalized() * dir;

            if (A10.dist(A01)<0.00001)
            {
                xyz B = BaryCentric(A00, A10, A11, value);
                return ((N00) * B.x + (N10) * B.y + (N11) * B.z) * dir;
            }
            if (((A10 - value) & (A01 - value))== 0)
            { }
                if (((A00 - value) & (A01 - value)) > 0)
            {
             
                xyz B = BaryCentric(A11, A00, A01, value);
                return ((N11) * B.x + (N00) * B.y + (N01) * B.z) * dir;
            }
            else
            {
                xyz B = BaryCentric(A00, A11, A10, value);
               
                return ((N00) * B.x + (N11) * B.y + (N10) * B.z) * dir;
            }
           

        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public SmoothPlane()
            : base()
        {
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
          SmoothPlane Result = base.Copy() as SmoothPlane;
            Result.A00 = A00;
            Result.A10 = A10;
            Result.A01 = A01;
            Result.A11 = A11;
            Result.N00 = N00;
            Result.N10 = N10;
            Result.N01 = N01;
            Result.N11 = N11;
           
            return Result;
        }
        /// <summary>
        /// overrides the <see cref="Surface.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which will be painted.</param>

        protected override void OnDraw(OpenGlDevice Device)
        {
            if (!plane)
            { base.OnDraw(Device); return; }
            object Handle = null;
            if ((Device.RenderKind == RenderKind.SnapBuffer) || (Entity.Compiling))
            {
                SurfaceSnappItem S = new SurfaceSnappItem();
                Handle = Device.Selector.RegisterSnapItem(S);
                S.Surface = this;
            }
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            //         if ((BoundedCurves != null) && (BoundedCurves.Count > 0))
            {
                Indices = new IndexType[] { 0, 1, 2, 0, 2, 3 };
                Points = new xyzf[] { A, B, C, D };
                Normals = new xyzf[] { N00.toXYZF(), N00.toXYZF(), N00.toXYZF(), N00.toXYZF() };
                Texture = new xyf[] { new xyf(0, 0), new xyf(0, 0), new xyf(0, 0), new xyf(0, 0) };

            }
            //else
            //    GetTrianglesFull(ref Indices, ref Points, ref Normals, ref Texture);
            if (Device.PolygonMode == PolygonMode.Fill)
                Primitives3d.drawTriangles(Device, Indices, Points, Normals, Texture,null);
            else
                Primitives3d.drawFouranglesLined(Device, Indices, Points, Normals, Texture);

            Entity.CheckCompiling();
            if ((Device.RenderKind == RenderKind.SnapBuffer) || (Entity.Compiling))
            {

                Device.Selector.UnRegisterSnapItem(Handle);

            }

        }
        /// <summary>
        /// is a constructor wich has four points A, B, C, D on the plane and furthermore normal vectors N00, N10, N01, and N11 in that points
        /// </summary>
        /// <param name="A">Point in the plane.</param>
        /// <param name="B">Point in the plane.</param>
        /// <param name="C">Point in the plane.</param>
        /// <param name="D">Point in the plane.</param>
        /// <param name="N00">Normal in A.</param>
        /// <param name="N10">Normal in B.</param>
        /// <param name="N01">Normal in C.</param>
        /// <param name="N11">Normal in D.</param>
        public SmoothPlane(xyz A, xyz B, xyz C, xyz D, xyz N00, xyz N10, xyz N01, xyz N11) : this()
        {
            Base = Base.From4Points(A, B, C, D);
            this.N00 = N00.normalized();
            this.N10 = N10.normalized(); 
            this.N11 = N11.normalized(); 
            this.N01 = N01.normalized(); 


            this.A00 = Base.Relativ(A).toXY();
            this.A10 = Base.Relativ(B).toXY();
            this.A01 = Base.Relativ(C).toXY();
            this.A11 = Base.Relativ(D).toXY();
            this.A = A.toXYZF();
            this.B = B.toXYZF();
            this.C = C.toXYZF();
            this.D = D.toXYZF();

        }
        bool plane = false;
        /// <summary>
        /// is a constructor wich has four points A, B, C, D on the plane and a normalvector N00
        /// </summary>
        /// <param name="A">Point in the plane.</param>
        /// <param name="B">Point in the plane.</param>
        /// <param name="C">Point in the plane.</param>
        /// <param name="D">Point in the plane.</param>
        /// <param name="N00">Normalvector of the plane.</param>
        public SmoothPlane(xyz A, xyz B, xyz C, xyz D, xyz N00) : this()
        {
            plane = true;
            Base = Base.From4Points(A, B, C, D);
            this.N00 = N00.normalized();
           


            this.A00 = Base.Relativ(A).toXY();
            this.A10 = Base.Relativ(B).toXY();
            this.A01 = Base.Relativ(C).toXY();
            this.A11 = Base.Relativ(D).toXY();
            this.A = A.toXYZF();
            this.B = B.toXYZF();
            this.C = C.toXYZF();
            this.D = D.toXYZF();

        }
        xy A00 = new xy(0, 0);
        xy A10 = new xy(0, 0);
        xy A11 = new xy(0, 0);
        xy A01 = new xy(0, 0);
        xyz N00 = new xyz(0, 0, -1);
        xyz N10 = new xyz(0, 0, 1);
        xyz N11 = new xyz(0, 0, -1);
        xyz N01 = new xyz(0, 0, 1);

    }





}
