using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    
   /// <summary>
   /// definition of an interface for Nurbs.
   /// </summary>
    public interface INurbs3d  
    {
        /// <summary>
        /// returns the Udegree.
        /// </summary>
        /// <returns>the Udegree.</returns>
        int getUDegree();
        /// <summary>
        /// returns the Vdegree.
        /// </summary>
        /// <returns>the Vdegree.</returns>
        int getVDegree();
        /// <summary>
        /// returns the control points.
        /// </summary>
        /// <returns>the control points.</returns>
        xyz[,] getCtrlPoints();
        /// <summary>
        /// returns the UKnots.
        /// </summary>
        /// <returns>the UKnots.</returns>
        double[] getUKnots();
        /// <summary>
        /// returns the VKnots.
        /// </summary>
        /// <returns>the VKnots.</returns>
        double[] getVKnots();
        /// <summary>
        /// returns the Weights.
        /// </summary>
        /// <returns>the Weights.</returns>
        double[,] getWeights();
    }
    /// <summary>
    /// describes a pol of a <see cref="Surface"/>.
    /// </summary>
    [Serializable]
    public class Pol
    {   
        /// <summary>
        /// empty constructor.
        /// </summary>
        public Pol()
        {
        }
        /// <summary>
        /// is a constructor with <b>enable</b> and a <b>Param</b>.
        /// </summary>
        /// <param name="Enable">is<b>true</b> if the pol exists.</param>
        /// <param name="Param">is a value, which belongs to the <b>pol</b></param>
        public Pol(bool Enable, double Param)
        {
            this.Enable = Enable;
            this.Param = Param;
        }
        /// <summary>
        /// is<b>true</b> if the pol exists.
        /// </summary>
        public bool Enable = false;
        /// <summary>
        /// a value, which belongs to the <b>pol</b>.
        /// </summary>
        public double Param = -1;
    }
   
    /// <summary>
    /// A surface is a area, which is given by a parameterized function (<see cref="Value(double, double)"/>). She has two parameters u and v and has
    /// a xyz result. The parameters u and v are taken in the interval [0,1]. 
    /// You define on this way a 3-dimsonal surface.
    /// it inherits from <see cref="Entity"/>. You can draw it by <see cref="Entity.Paint(OpenGlDevice)"/>.
    /// If you want to draw only a part of the surface, you have to define <see cref="BoundedCurves"/>. Now you get only the part of
    /// surface, which enclosed by the <see cref="BoundedCurves"/>.
    /// Additional the partial derivations <see cref="uDerivation"/> and <see cref="vDerivation"/>
    /// must bee given to calculate the <see cref="Normal"/>.
    /// Surface by himself is an abstract class. <see cref="Value(double, double)"/>, <see cref="uDerivation"/> and <see cref="vDerivation"/> have to been overridden.
    /// <seealso cref="Sphere"/> <seealso cref="BezierSurface"/> <seealso cref="PlaneSurface"/>
    /// <seealso cref="Cone"/>  <seealso cref="Cylinder"/> <seealso cref="NurbsSurface"/>
    /// </summary>
    /// 
    [Serializable]
    public abstract class Surface
    {



        /// <summary>
        /// hods the values of a heighmap. See also <see cref="HightScale"/>
        /// </summary>
        double[,] HeightMap = null;
        /// <summary>
        /// factor, wich scales the value of the bitmap, setted with <see cref="SetHeighMap(Bitmap)"/>. See also <see cref="HightScale"/>.
        /// </summary>
        public double HightScale = 200;
        /// <summary>
        /// activates a <b>high map</b>. The red channel of each texel will be divided by <see cref="HightScale"/> and setted as z-coordinate if the <see cref="Terrain"/>.
        /// </summary>
        /// <param name="HighBitmap">is the Bitmap, which holds the inormatios about the height.</param>
        public void SetHeighMap(Bitmap HighBitmap)
        {
            int US = USnapResolution;
            int VS = VSnapResolution;
            UResolution = HighBitmap.Width;
            VResolution = HighBitmap.Height;
            USnapResolution = US;
            VSnapResolution = VS;
            BitmapData SourceData = HighBitmap.LockBits(new Rectangle(0, 0, HighBitmap.Width, HighBitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            int SourceStride = SourceData.Stride;

            int H = HighBitmap.Height;
            int W = HighBitmap.Width;
            unsafe
            {

                byte* SourcePtr = (byte*)(void*)SourceData.Scan0;
                int Ydisp = SourceStride * (H - 1);
                HeightMap = new double[W+1, H+1];
                for (int _y = 0; _y < H; _y++)
                {
                    for (int _x = 0; _x < W; _x++)
                    { 
                        HeightMap[_x, _y] = (float)SourcePtr[_x * 4 + 2 + Ydisp] / HightScale;// red
                       
                     
                    }
                    HeightMap[W, _y] = (float)SourcePtr[(W-1) * 4 + 2 + Ydisp] / HightScale;
                    Ydisp = Ydisp - SourceStride;
                }
                Ydisp = 0;
                for (int _x = 0; _x <= W; _x++)
                {
                    if (_x == W)
                        HeightMap[W, H] = (float)SourcePtr[(W-1) * 4 + 2 + Ydisp] / HightScale;// red
                    HeightMap[_x, H] = (float)SourcePtr[_x * 4 + 2 + Ydisp] / HightScale;// red
                }
            }

            HighBitmap.UnlockBits(SourceData);
        
    }
        /// <summary>
        /// describes the "down"<see cref="Pol"/>.
        /// </summary>
        public Pol DownPol = new Pol();
        /// <summary>
        /// describes the "up"<see cref="Pol"/>.
        /// </summary>
        public Pol UpPol = new Pol();
        /// <summary>
        /// describes the "left"<see cref="Pol"/>.
        /// </summary>
        public Pol LeftPol = new Pol();
        /// <summary>
        /// describes the "right"<see cref="Pol"/>.
        /// </summary>
        public Pol RightPol = new Pol();
        /// <summary>
        /// noch machen
        /// </summary>
        protected virtual void CheckPeriodic()
        {

        }
        xy ProjectIntheNearOf = new xy(0, 0);
        /// <summary>
        /// calculates a 2-dimensional <see cref="Curve"/> on the surface, whose points are parameter of the surface.
        /// This curve is near to the 3D-Curve.
        /// </summary>
        /// <param name="C">is the 3D-Curve</param>
        /// <returns>curve</returns>
        public virtual Curve GetPCurve(Curve3D C)
        {
            if (C == null) return
                 null;
            int saveUresolution = UResolution;
            int saveVresolution = VResolution;
            UResolution = C.Resolution / 2;
            VResolution = C.Resolution / 2;

            if (UResolution == 0) UResolution = 1;
            if (VResolution == 0) VResolution = 1;
            int SaveR = C.Resolution;

         
            CubicInterpolation InterP = new CubicInterpolation();
            
            List<xy> _Pts = new List<xy>();
            xy[] Pts = new xy[C.Resolution + 1];
            xyzArray Array = new xyzArray(C.Resolution + 1);
            
            C.ToArray(Array, 0);
            ProjectIntheNearOf = new xy(0, 0);
            xy delta = new xy(0, 0);
            for (int i = 0; i <= C.Resolution; i++)
            {
                try
                {

                    xyz A = Array[i];
                    if (i < Pts.Length - 1) ProjectIntheNearOf = Pts[i + 1];
                    xy OldNear = ProjectIntheNearOf;
                    System.Diagnostics.Stopwatch W = new System.Diagnostics.Stopwatch();
                    W.Start();
                    xy P2d = ProjectPoint(A);

                    W.Stop();
                    delta = P2d - OldNear;
                    OldNear = P2d;

                    Pts[i] = P2d;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }


            }

            double fac = 0.3;
            {
               
                if (UPeriodicity < 10000)
                {
                    //  if (Pts[0].x < 0) Pts[0].x += UPeriodicity;
                    for (int i = 0; i < Pts.Length - 1; i++)
                    {
                        if (i == 30)
                        {
                        }
                        if (UPeriodicity > 0)
                            while (Pts[i + 1].x - Pts[i].x > UPeriodicity * fac)
                                Pts[i + 1].x -= UPeriodicity;
                        if (UPeriodicity > 0)
                            while (Pts[i].x - Pts[i + 1].x > UPeriodicity * fac)
                                Pts[i + 1].x += UPeriodicity;
                    }

                }


                if (VPeriodicity < 10000)
                {
                    // if (Pts[0].y < 0) Pts[0].y += VPeriodicity;
                    for (int i = 0; i < Pts.Length - 1; i++)
                    {
                        if (VPeriodicity > 0)
                            while (Pts[i + 1].y - Pts[i].y > VPeriodicity * fac)
                                Pts[i + 1].y -= VPeriodicity;


                        if (VPeriodicity > 0)
                            while (Pts[i].y - Pts[i + 1].y > VPeriodicity * fac)
                                Pts[i + 1].y += VPeriodicity;
                    }
                }
            }
            C.Resolution = SaveR;
            InterP.InterpolationPoints=Pts;
            UResolution = saveUresolution;
            VResolution = saveVresolution;
            return InterP;
        }
        /// <summary>
        /// calculates a 2-dimensional <see cref="Curve"/> on the surface, whose points are parameter of the surface.
        /// This curve is near to a list of <see cref="xyz"/> <b>C</b>.
        /// </summary>
        /// <param name="C">is the List of <see cref="xyz"/></param>
        /// <returns>curve</returns>
        public virtual Curve GetPCurve(List<xyz> C)
        {
            if (C == null) return
                 null;
            CubicInterpolation InterP = new CubicInterpolation();
            List<xy> _Pts = new List<xy>();
            xy[] Pts = new xy[C.Count];
            xyzArray Array = new xyzArray(C.Count);
            for (int i = 0; i < C.Count; i++)
            {
                Array[i] = C[i];
            }
         
            ProjectIntheNearOf = new xy(0, 0);
            xy delta = new xy(0, 0);
            for (int i = 0; i < Array.Count; i++)
            {
                try
                {

                    xyz A = Array[i];
                    if (i < Pts.Length - 1) ProjectIntheNearOf = Pts[i + 1];
                    xy OldNear = ProjectIntheNearOf;
                    System.Diagnostics.Stopwatch W = new System.Diagnostics.Stopwatch();
                    W.Start();
                    xy P2d = ProjectPoint(A);

                    W.Stop();
                    delta = P2d - OldNear;
                    OldNear = P2d;

                    Pts[i] = P2d;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }


            }

            double fac = 0.3;// Statt 0.8 26.10

            {

                if (UPeriodicity < 10000)
                {

                    for (int i = 0; i < Pts.Length - 1; i++)
                    {

                        if (UPeriodicity > 0)
                            while (Pts[i + 1].x - Pts[i].x > UPeriodicity * fac)
                                Pts[i + 1].x -= UPeriodicity;
                        if (UPeriodicity > 0)
                            while (Pts[i].x - Pts[i + 1].x > UPeriodicity * fac)
                                Pts[i + 1].x += UPeriodicity;
                    }

                }


                if (VPeriodicity < 10000)
                {

                    for (int i = 0; i < Pts.Length - 1; i++)
                    {
                        if (VPeriodicity > 0)
                            while (Pts[i + 1].y - Pts[i].y > VPeriodicity * fac)
                                Pts[i + 1].y -= VPeriodicity;


                        if (VPeriodicity > 0)
                            while (Pts[i].y - Pts[i + 1].y > VPeriodicity * fac)
                                Pts[i + 1].y += VPeriodicity;

                    }

                }


            }
            InterP.Resolution = Pts.Length / 2;
            InterP.InterpolationPoints=Pts;
           return InterP;
        }
        /// <summary>
        /// in <see cref="GetTrianglesBordered(ref IndexType[], ref xyzf[], ref xyzf[], ref xyf[])"/> a refinement of the triangles will be recursively made.
        /// This field holds the depth of the stack of this refinemnt. The default is 1.
         /// </summary>
        public static int TriangleRefinement = 1;
        void RefineTriangle(IndexType A, IndexType B, IndexType C, List<xyf> _Points, List<IndexType> _Indices, int depth)
        {
            Stack++;

            if (Stack >= TriangleRefinement)
            {

                _Indices.Add(A);
                _Indices.Add(B);
                _Indices.Add(C);
                Stack--;
            }
            else
            {


                xyf M1 = (_Points[(int)A] + _Points[(int)B]) * 0.5f;
                xyf M2 = (_Points[(int)B] + _Points[(int)C]) * 0.5f;
                xyf M3 = (_Points[(int)C] + _Points[(int)A]) * 0.5f;


                {
                    _Points.Add(M1);
                    IndexType D1 = (IndexType)(_Points.Count - 1);
                    _Points.Add(M2);
                    IndexType D2 = (IndexType)(_Points.Count - 1);
                    _Points.Add(M3);
                    IndexType D3 = (IndexType)(_Points.Count - 1);

                    RefineTriangle(A, D1, D3, _Points, _Indices, depth);
                    RefineTriangle(D1, B, D2, _Points, _Indices, depth);
                    RefineTriangle(D2, C, D3, _Points, _Indices, depth);
                    RefineTriangle(D1, D2, D3, _Points, _Indices, depth);
                    Stack--;
                }



            }

        }
        Hashtable TmpPointArray = new Hashtable();
        List<xyf> _TmpPoints = new List<xyf>();
        List<IndexType> OutIndices = null;
        int Stack = 0;
     
        void Refinement(List<xyf> Points, List<IndexType> Indices)
        {

            int StartId = Points.Count;
            int Startindex = Indices.Count;
            TriAngulation(Indices, Points);
            Stack = 0;
            TmpPointArray.Clear();
            _TmpPoints = Points;

            int CT = Indices.Count;
            int CTPT = Points.Count;

            try
            {
                Rectangled DefinitionDomain = new Rectangled(0, 0, 1, 1);
                xyz A = Value(DefinitionDomain.Left, DefinitionDomain.Bottom);
                xyz B = Value(DefinitionDomain.Right, DefinitionDomain.Top);

               
                OutIndices = new List<IndexType>();
                for (int i = Startindex; i < CT - 2; i += 3)
                {
                    xyz _A = Value(Points[Indices[i]]);
                    xyz _B = Value(Points[Indices[i + 1]]);
                    xyz _C = Value(Points[Indices[i + 2]]);
                    RefineTriangle(Indices[i], Indices[i + 1], Indices[i + 2], Points, Indices, 1);
                }
            }
            catch (Exception)
            {


            }
           Indices.RemoveRange(Startindex, CT - Startindex);
        }
        bool FindNearer(xyz Pt, ref xy StartPoint, int Ucount, int Vcount, double UWidth, double VWidth)
        {
            xy Result = new xy(0, 0);
            bool Found = false;
            double Dist = Pt.dist(Value(StartPoint.x, StartPoint.y));
            if (Dist < 0.1)
                return true;
            for (int i = -Ucount; i < Ucount; i++)
            {
                for (int j = -Vcount; j < Vcount; j++)
                {
                    xy P = StartPoint + new xy(UWidth * i, VWidth * j);
                    P = clampToDefArea(new xy(P.x, P.y));
                    double _dist = Pt.dist(Value(P.x, P.y));
                    if (_dist < Dist)
                    {
                        Dist = _dist;
                        Result = P;
                        Found = true;
                        if (_dist < 0.0001)
                            return true;
                    }

                }
            }
            if (Found)
            {
                StartPoint = Result;
                if (Pt.dist(Value(Result.x, Result.y)) > 0.0001)
                {
                    if (FindNearer(Pt, ref StartPoint, Ucount, Vcount, UWidth / 2f, VWidth / 2f))
                    {
                    }
                }
            }
            double hh = Pt.dist(Value(StartPoint.x, StartPoint.y));
            return Found;
        }
 
        xyz[,] FindBestArray = null;

        // in projectpoint
        xy FindBestIndices(xyz Pt)
        {
            if (CurrentGetpCurveSurface != this)
            {

                {
                    FindCTx = 10;
                    FindCTy = 10;
                }
 
                fillFindBestArray();

                CurrentGetpCurveSurface = this;
            }

            double Dist = 1e10;
            xy Result = new xy(0, 0);
            Rectangled Rect = new Rectangled(0, 0, 1, 1);

            int SicVResolution = VResolution;
            int SicUResolution = UResolution;
            VResolution = FindCTx;
            UResolution = FindCTy;
            double deltax = Rect.Width / (float)FindCTx;
            double deltay = Rect.Height / (float)FindCTy;
            int i = 0;
            int j = 0;
            xy Old = new xy(0, 0);
            for (i = 0; i < FindCTx; i++)
            {
                try
                {


                    for (j = 0; j < FindCTy; j++)
                    {
                        double x = Rect.Left + deltax * i;
                        double y = Rect.Bottom + deltay * j;
                        xyz A = FindBestArray[i, j];
                        xyz B = FindBestArray[i + 1, j];
                        xyz C = FindBestArray[i, j + 1];

                        Base Base = new Base();
                        Base.BaseO = A;
                        Base.BaseX = (B - A);
                        Base.BaseY = (C - A);
                        Base.BaseZ = Base.BaseX & Base.BaseY;
                        if (Base.BaseZ.length() < 0.0000000000001)
                        {
                            continue;

                        }
                        xyz Rel = Base.Relativ(Pt);

                        xy P = clampToDefArea(new xy(x + Rel.x * deltax, y + Rel.Y * deltay));
                        if (P.dist(Old) < 0.0001)
                        {

                            continue;
                        }
                        Old = P;
 
                        double d = Value(P.x, P.y).dist(Pt);


                        if (d < Dist)
                        {
                            Dist = d;
                            Result = P;
                            if (Dist < 0.001)
                            {
                                ProjectIntheNearOf = clampToDefArea(new xy(Result.x, Result.y));
                                UResolution = SicUResolution;
                                VResolution = SicVResolution;
                                return ProjectIntheNearOf;
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message);


                }
            }

            UResolution = SicUResolution;
            VResolution = SicVResolution;
            ProjectIntheNearOf = clampToDefArea(new xy(Result.x, Result.y));
            return ProjectIntheNearOf;
        }
        //-------------------------------- 


        Rectangled GetDefinitionDomain()
        {
            return new Rectangled(0, 0, 1, 1);
        }
        void fillFindBestArray()
        {

            FindBestArray = new xyz[FindCTx + 1, FindCTy + 1];
            Rectangled Rect = new Rectangled(0,0,1,1);
            double deltax = Rect.Width / FindCTx;
            double deltay = Rect.Height / FindCTy;
            int i = 0;
            int j = 0;
            for (i = 0; i < FindCTx + 1; i++)
            {
                for (j = 0; j < FindCTy + 1; j++)
                {
                    double x = Rect.Left + deltax * i;
                    double y = Rect.Bottom + deltay * j; ;


                    FindBestArray[i, j] = Value(x, y);
                }
            }

        }
        [NonSerialized]
        Surface CurrentGetpCurveSurface = null;
       


       
        /// <summary>
        /// every surface hase a <see cref="Drawing3d.Base"/>, which is settable and readable.
        /// </summary>
        public Base Base
        {
            get { return _Base; }
            set
            {
               _Base= value;
            }
        }
        /// <summary>
        /// the <b>basepoint</b> is the origin of the <see cref="Base"/>.
        /// </summary>
        public xyz BasePoint
        {
            get { return Base.BaseO; }
            set
            {
                Base B = Base;
                B.BaseO = value;
                Base = B;
                RefreshEnvBox();
            }
         }
        private Loca _BoundedCurves = null;
        /// <summary>
        /// Defines a enclosing curve list. If this value is different from null, only the part inside the curves will be drawn
        /// will be drawn.
        /// </summary>
        public Loca BoundedCurves
        {
            get { return GetBoundedCurves(); }
            set
            {
                SetBoundedCurves(value);
            }
        }
        /// <summary>
        /// is the virtual setter of <see cref="BoundedCurves"/>.
        /// </summary>
        /// <param name="value"><see cref="Loca"/></param>
        protected virtual void SetBoundedCurves(Loca value)
        {
            _BoundedCurves = value;
            Invalid = true;
        }
        /// <summary>
        /// is the virtual getter of <see cref="BoundedCurves"/>.
        /// </summary>
        /// <returns><see cref="Loca"/></returns>
        protected virtual Loca GetBoundedCurves()
        {
            return _BoundedCurves;
        }
        /// <summary>
        /// overrides <see cref="Entity.GetMaxBox"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Box GetMaxBox()
        {  
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            Box B= Box.ResetBox(); ;
            if ((BoundedCurves != null) && (BoundedCurves.Count > 0))
            {

                GetTrianglesBordered(ref Indices, ref Points, ref Normals, ref Texture);
            }
            else
            {
                GetTrianglesFull(ref Indices, ref Points, ref Normals, ref Texture);
            }
           return B.GetMaxBox(Base.UnitBase, Points);

        }
        void TriAngulation(List<IndexType> Indices, List<xyf> Points)
        {
            if (BoundedCurves == null) return;
         Loxy L=   BoundedCurves.ToLoxy();
            L.TriAngulation(Indices, Points);
         }
        /// <summary>
        /// is the improtant abstract method, which must be overriden. She gets the geometry of the surface.
        /// Its a function from [0,1]x[0,1] to <b>R³</b>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns><see cref="xyz"/> point.</returns>
        public abstract xyz Value(double u, double v);
        /// <summary>
        /// is the same as <see cref="Value(double, double)"/>, but the parameters are <see cref="xyf"/> values.
        /// </summary>
        /// <param name="Param">parameter.</param>
        /// <returns><see cref="xyz"/> point.</returns>
        public xyz Value(xyf Param)
        { return Value(Param.x, Param.y); }

        /// <summary>
        /// is the same as <see cref="Value(double, double)"/>, but the return point is are <see cref="xyzf"/>.
        /// </summary>
        /// <param name="x">first parameter.</param>
        /// <param name="y">second parameter.</param>
        /// <returns><see cref="xyzf"/> point.</returns>
        public xyzf Valuef(double x, double y)
        {
            xyz A = Value(x, y);
            return new xyzf((float)A.x, (float)A.y, (float)A.z);
        }
        /// <summary>
        /// is the same as <see cref="Normal(double, double)"/>, but the return point is are <see cref="xyzf"/>.
        /// </summary>
        /// <param name="x">first parameter.</param>
        /// <param name="y">second parameter.</param>
        /// <returns><see cref="xyzf"/> point.</returns>
        public xyzf Normalf(double x, double y)
        {
            xyz A = Normal(x, y);
            return new xyzf((float)A.x, (float)A.y, (float)A.z);
        }
        /// <summary>
        /// Returns the normal at point u, v, which is calculated as th crossproduct of <see cref="uDerivation"/> and <see cref="vDerivation"/>.
        /// Is <see cref="SameSense"/> true the result vector looks in the right oriented crossproduct else in the left oriented.
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>Normal</returns>
        public virtual xyz Normal(double u, double v)
        {

            if (SameSense)
                return ((uDerivation(u, v) & vDerivation(u, v))).normalized();
            else
                return (uDerivation(u, v) & vDerivation(u, v)).normalized() * (-1);
        }
        /// <summary>
        /// it calculates a point <b>and</b> a <b>normal</b> for the values x and y. It is for some surfaces faster <seealso cref="Sphere"/>.
        /// </summary>
        /// <param name="x">the first parameter.</param>
        /// <param name="y">the second parameter.</param>
        /// <param name="Point">the calculated point.</param>
        /// <param name="Normal">is the normal.</param>
        public virtual void GetPointAndNormal(double x, double y, ref xyzf Point, ref xyzf Normal)
        {
            Point = Valuef(x, y);
            Normal = Normalf(x, y);
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public Surface()
        {


        }
        /// <summary>
        /// a tag, which you can use to store something.
        /// </summary>
        public object Tag = 0;
        bool _SameSense = true;
        /// <summary>
        /// is this <b>true</b> the normal vector looks in the right oriented crossproduct else in the left oriented.
        /// </summary>
        public virtual bool SameSense
        {
            get { return _SameSense; }
            set
            {
                Invalid = true;
                _SameSense = value;
             }
        }
        
        /// <summary>
        /// Makes a flat copy of the surface and returns it. (you can use also <see cref="Entity.Clone()"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Surface Copy()
        {
            //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream(10000);
            //formatter.Serialize(stream, this);
            //stream.Position = 0;
            //Surface result = formatter.Deserialize(stream) as Surface;
            //stream.Close();
            //return result;
            Surface Result = Activator.CreateInstance(this.GetType()) as Surface;

            Result.UFactor = this.UFactor;
            Result.UPeriodicity = this.UPeriodicity;

            Result.VFactor = this.VFactor;
            Result.VPeriodicity = this.VPeriodicity;
            Result.UResolution = this.UResolution;
            Result.VResolution = this.VResolution;
            Result.SameSense = this.SameSense;
            Result.EnvBox = this.EnvBox;
            Result.Base = Base;
            return Result;
        }
        /// <summary>
        /// is a definition of an event for drawing. See e.g <see cref="CustomEntity.Draw"/>.
        /// </summary>
        /// <param name="Sender">is a <see cref="Entity"/></param>
        /// <param name="Device">Device in which it will be drawn.</param>
        public delegate void DrawEvent(Surface Sender, OpenGlDevice Device);
        /// <summary>
        /// an <see cref="DrawEvent"/>, which is called by the <see cref="OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        [field: NonSerialized]
        public event DrawEvent Draw;
        /// <summary>
        /// public method, who calls the virtual <see cref="OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        public void Paint(OpenGlDevice Device)
        {
            OnDraw(Device);
        }
        /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected virtual void OnDraw(OpenGlDevice Device)
        {
            if (Draw != null) Draw(this, Device);
            object Handle = null;
            int SaveUResolution = -1;
            int SaveVResolution = -1;
            double[,] SaveHMap = null;
            int[] SaveIndices = null;
            xyzf[] SaveVerticesBuffer = null;
            xyzf[] SaveNormalBuffer = null;
            xyf[] SaveTextureBuffer = null;
            if ((Device.RenderKind == RenderKind.SnapBuffer) )
            {
                SurfaceSnappItem S = new SurfaceSnappItem();
                if (HeightMap != null)
                {
                    SaveUResolution = UResolution;
                    SaveVResolution = VResolution;
                    SaveHMap = HeightMap;
                    UResolution = USnapResolution;
                    VResolution = VSnapResolution;

                    HeightMap = null;
                    SaveVerticesBuffer = VerticesBuffer;
                    SaveNormalBuffer = NormalBuffer;
                    SaveTextureBuffer = TextureBuffer;
                    SaveIndices = IndicesBuffer;
                    this.IndicesBuffer = null;
                    S.Surface = this.Copy();
                }
                else
                    S.Surface = this;
                
                Handle = Device.Selector.RegisterSnapItem(S);
                
            }
             IndexType[] Indices = null;
             xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            if ((BoundedCurves != null) && (BoundedCurves.Count > 0))
            {
                GetTrianglesBordered(ref Indices, ref Points, ref Normals, ref Texture);
            }
            else
                if (Device.PolygonMode == PolygonMode.Fill)
            { 
               
             

               
                
                GetTrianglesFull(ref Indices, ref Points, ref Normals, ref Texture);
                if (Device.RenderKind == RenderKind.SnapBuffer)
                {
                 
                    
                   
                }
            }
            else
                getQuads(ref Points);
          
            if ((Device.PolygonMode== PolygonMode.Fill))
            Primitives3d.drawTriangles(Device, Indices, Points, Normals, Texture,null);
           else
                Primitives3d.drawFouranglesLined(Device, Indices, Points, Normals, Texture);


            if ((Device.RenderKind == RenderKind.SnapBuffer))
            {


                if (SaveUResolution>0)
                {
                    _UResolution = SaveUResolution;
                    _VResolution = SaveVResolution;
                    IndicesBuffer = SaveIndices;
                    VerticesBuffer = SaveVerticesBuffer;
                    NormalBuffer = SaveNormalBuffer;
                    TextureBuffer = SaveTextureBuffer;
                    HeightMap = SaveHMap;
                }
                Device.Selector.UnRegisterSnapItem(Handle);
            }

        }
        /// <summary>
        /// gets the triangle of the area bouded by <see cref="BoundedCurves"/>. These triangles will
        /// bee  recursive refined in thre triangles.
        /// </summary>
        /// <param name="Indices">indices.</param>
        /// <param name="Points">points</param>
        /// <param name="Normals">normals</param>
        /// <param name="Texture">texture coordinates.</param>
        void GetTrianglesBordered(ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {

            Loxy LB = BoundedCurves.ToLoxy();
            List<IndexType> _Indices = new List<IndexType>();
            List<xyf> Params = new List<xyf>();
            Refinement(Params, _Indices);

            Indices = _Indices.ToArray();
            Points = new xyzf[Params.Count];
            Normals = new xyzf[Params.Count];

            for (int j = 0; j < Params.Count; j++)
            {
                try
                {
                    xyf P = Params[j];
                    GetPointAndNormal(P.x, P.y, ref Points[j], ref Normals[j]);

                }
                catch (System.Exception)
                {


                }
            }
           Texture = new xyf[Params.Count];
            for (int i = 0; i < Params.Count; i++)
                Texture[i] = new xyf((float)Params[i].x, (float)Params[i].y);
        }
        void getQuads(ref xyzf[] Vertices)
        {
            double xStep = (float)1 / (float)(UResolution);
            double yStep = (float)1 / (float)(VResolution);
            Vertices = new xyzf[(UResolution + 1) * (VResolution + 1)];
            xyzf _Point = new xyzf(0, 0, 0);
            xyzf _Normal = new xyzf(0, 0, 0);
            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                for (int v = 0; v < VResolution + 1; v++)
                {
                    Vertices[URow + v] = Value(xStep * u, yStep * v).toXYZF();
                    Vertices[URow + v+1] = Value(xStep * u, yStep * (v+1)).toXYZF();
                    Vertices[URow + v + 2] = Value(xStep * (u+1), yStep * (v + 1)).toXYZF();
                    Vertices[URow + v + 3] = Value(xStep * (u + 1), yStep * v).toXYZF();
                }

            }
        }
        void GetFourAngles(ref IndexType[] Indices, ref xyzf[] Vertices)
        {
            Vertices = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Indices = new IndexType[(UResolution) * (VResolution) * 4];
          
            double xStep = (float)1 / (float)(UResolution);
            double yStep = (float)1 / (float)(VResolution);
            xyzf _Point = new xyzf(0, 0, 0);
            xyzf _Normal = new xyzf(0, 0, 0);
            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                for (int v = 0; v < VResolution + 1; v++)
                {
                    Vertices[URow + v] = Value(xStep * u, yStep * v).toXYZF();
                   
                }

            }

            for (int u = 0; u < UResolution; u++)
            {
                for (int v = 0; v < VResolution; v++)
                {
                    Indices[4 * u * (VResolution) + 4 * v] =      (IndexType)(u * (VResolution + 1) + v);
                    Indices[4 * u * (VResolution) + 4 * v + 1] = (IndexType)((u + 1) * (VResolution + 1) + v);
                    Indices[4 * u * (VResolution) + 4 * v + 2] = (IndexType)((u + 1) * (VResolution + 1) + v + 1); 
                    Indices[4 * u * (VResolution) + 4 * v + 3] = (IndexType)(u * (VResolution + 1) + v + 1); ;

                }

            }

        }
        xyzf[] VerticesBuffer = null;
        xyzf[] NormalBuffer = null;
        xyf[] TextureBuffer = null;
        IndexType[] IndicesBuffer = null;
        /// <summary>
        /// gets the triangles of the whole surface.
        /// </summary>
        /// <param name="Indices">are the indices.</param>
        /// <param name="Vertices">are the vertices.</param>
        /// <param name="Normals">are the normals</param>
        /// <param name="Texture">are the texture coordinates.</param>
        void GetTrianglesFull(ref IndexType[] Indices, ref xyzf[] Vertices, ref xyzf[] Normals, ref xyf[] Texture)
        {
            
           
            if (IndicesBuffer == null)
            {
            Vertices = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Normals = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Texture = new xyf[(UResolution + 1) * (VResolution + 1)];
            Indices = new IndexType[(UResolution) * (VResolution) * 6];
            Rectangled R = new Rectangled(0, 0, 1, 1);
            double xStep = (float)1 / (float)(UResolution);
            double yStep = (float)1 / (float)(VResolution);
            xyzf _Point = new xyzf(0, 0, 0);
            xyzf _Normal = new xyzf(0, 0, 0);
                for (int u = 0; u < UResolution + 1; u++)
                {
                    int URow = u * (VResolution + 1);
                    for (int v = 0; v < VResolution + 1; v++)
                    {

                        GetPointAndNormal(xStep * u, yStep * v, ref Vertices[URow + v], ref Normals[URow + v]);
                        Texture[URow + v] = new xyf((float)(R.Left + xStep * u), (float)(R.Bottom + yStep * v));

                    }

                }
     
                for (int u = 0; u < UResolution; u++)
                {
                    for (int v = 0; v < VResolution; v++)
                    {
                        Indices[6 * u * (VResolution) + 6 * v] = (IndexType)(u * (VResolution + 1) + v);
                        Indices[6 * u * (VResolution) + 6 * v + 1] = (IndexType)(u * (VResolution + 1) + v + 1);
                        Indices[6 * u * (VResolution) + 6 * v + 2] = (IndexType)((u + 1) * (VResolution + 1) + v);

                        Indices[6 * u * (VResolution) + 6 * v + 3] = (IndexType)((u + 1) * (VResolution + 1) + v);
                        Indices[6 * u * (VResolution) + 6 * v + 4] = (IndexType)((u) * (VResolution + 1) + v + 1);
                        Indices[6 * u * (VResolution) + 6 * v + 5] = (IndexType)((u + 1) * (VResolution + 1) + v + 1);

                    }

                }
                VerticesBuffer = Vertices;
                NormalBuffer = Normals;
                TextureBuffer = Texture;
                IndicesBuffer = Indices;
            }
            else
            {
                Vertices = VerticesBuffer;
                Indices = IndicesBuffer;
                Normals = NormalBuffer;
                Texture = TextureBuffer;
           }

        }
         private double _UFactor = 1;
        /// <summary>
        /// The Ufactor normalizes the u parameter to [0,1].
        /// This must be respected, when you define a <see cref="Value(double, double)"/>, <see cref="uDerivation"/> or <see cref="vDerivation"/>.
        /// E.g. the you set the UFactor = 2*PI you can use it in the <see cref="Value(double, double)"/> function with Cos(u * UFactor). <seealso cref="VFactor"/>.
        /// The default is 1.
        /// </summary>
        public double UFactor
       {
        get { return _UFactor; }
        set
        {
             Invalid = true;
            _UFactor = value;
       }
    }
    private double _VFactor = 1;
     
        /// <summary>
        /// The Vfactor normalizes the u parameter to [0,1].
        /// This must be respected, when you define a <see cref="Value(double, double)"/>, <see cref="uDerivation"/> or <see cref="vDerivation"/>.
        /// E.g. the you set the VFactor = 2*PI you can use it in the <see cref="Value(double, double)"/> function with Cos(v * VFactor). <seealso cref="UFactor"/>.
        /// The default is 1.
        /// </summary>
        public double VFactor
    {
        get { return _VFactor; }
        set
            {
                Invalid = true;
                _VFactor = value;
       }
    }
   
    Box _EnvBox = new Box(new xyz(0, 0, 0), new xyz(0, 0, 0));
    /// <summary>
    /// gets and sets the enclosing box.
    /// </summary>
    public Box EnvBox
    {
        get { return _EnvBox; }
        set { _EnvBox = value; }
   }
        /// <summary>
        /// is a virtual method, where you can set the enclosing <see cref="EnvBox"/>
        /// </summary>
    protected virtual void RefreshEnvBox()
    {
        xyzArray A = new xyzArray((UResolution + 1) * (VResolution + 1));
        Rectangled R = new Rectangled(0, 0, 1, 1);
            double Deltax = R.Width / UResolution;
        double Deltay = R.Height / VResolution;
        for (int i = 0; i <= UResolution; i++)
            for (int j = 0; j <= VResolution; j++)
            {
                double x = R.X + i * Deltax;
                double y = R.Y + j * Deltay;

                A[i * VResolution + j] = Value(x, y);

            }

        _EnvBox = A.GetMaxBox(  Base.UnitBase);

    }
    /// <summary>
    /// Creates a 3D-Curve which belongs to the curve which is given by the curve consisting of parameter values of the surface.
    /// </summary>
    /// <param name="ParamCurve">A 2-dimensional curve, which is taken in the parameter room.</param>
    /// <returns></returns>
    public virtual Curve3D To3dCurve(Curve ParamCurve)
    {
        return null;
    }
    /// <summary>
    /// produces for a 3D-Curve a 2D-Curve, which is a curve in the room of parameters of the surface. When you apply <see cref="Value(double, double)"/> you get back  the 3D-curve.
    /// mit <see cref="Value(double, double)"/> wieder die 3D-Kurve ergibt.
    /// </summary>
    /// <param name="Curve3d">the 3D-curve.</param>
    /// <returns></returns>
    public virtual Curve To2dCurve(Curve3D Curve3d)
    {

        if (Curve3d is Line3D)
        {

            xyz _A = Base.Relativ(Curve3d.A);
            xyz _B = Base.Relativ(Curve3d.B);
            if (System.Math.Abs(_A.z) > 0.001)
            {
            }
          
            return new Line(new xy(_A.x, _A.y), new xy(_B.x, _B.y));
        }

        return null;
    }
        /// <summary>
        /// Some surfaces are periodically in the parameters. For example a <see cref="Sphere"/> has a periode of 2*PI for the u-parameter.
        /// The UPeriodicity gets and sets this priodicity. Default is 1e10;
        /// </summary> 
        [NonSerialized]
    public double UPeriodicity = 1e10;
        /// <summary>
        /// Some surfaces are periodically in the parameters. For example a <see cref="Cone"/> has a periode of 2*PI for the u-parameter.
        /// The VPeriodicity gets and sets this priodicity. Default is 1e10;
        /// </summary>
        [NonSerialized]
    public double VPeriodicity = 1e10;
    private Base _Base = Base.UnitBase;
    private int _UResolution = 20;
    private int _VResolution = 20;
    private xy _TextureOffset = new xy(0, 0);
 
    /// <summary>
    /// Sets or gets an offset for a texture
    /// </summary>
   public xy TextureOffset
    {
        get { return _TextureOffset; }
        set { _TextureOffset = value; }
    }
        /// <summary>
        /// you have to call <b>Invalidate</b> when you have changed the surface. Then
        /// new values will be calculated.
        /// </summary>
        public bool Invalid
        {
            set
            {
                IndicesBuffer = null;
            }
            get { return IndicesBuffer == null; }
        }
        /// <summary>
        /// when you implement a new surface you have to add <b>ZHeight</b> to the result
        /// of the <see cref="Value(double, double)"/> method. So you get the <b>HeighMap</b>, which is
        /// created by <see cref="SetHeighMap(Bitmap)"/>.
        /// E.g.: the <see cref="PlaneSurface"/> has the <see cref="Value(double, double)"/> method:return Base.Absolut(new xyz(u * UFactor, v * VFactor,ZHeight(u,v)));
        /// </summary>
        /// <returns></returns>
        public double ZHeight(double u, double v)
        {

           if (HeightMap != null)
            {
                double _u = u;
                if (_u < 0) _u = 0;
                if (_u > 1) _u = 1;
                double _v = v;
                if (_v < 0) _v = 0;
                if (_v > 1) _v = 1;
                int UId = (int)(_u * (UResolution));
                int VId = (int)(_v * (VResolution));
                return HeightMap[UId, VId];
           }
            return 0;
        }
        int USnapResolution;
        int VSnapResolution;
    /// <summary>
    /// Defines a Resolution for the u parameter
    /// Default is 20;
    /// </summary>
        public int UResolution
    {
        get
        {
            return _UResolution;
        }
        set
        {
                USnapResolution = value;
            _UResolution= value;
        }

    }
    /// <summary>
    /// Defines a Resolution for the v parameter
    /// Default is 20;
    /// </summary>
   public int VResolution
    {
        get
        {
            return _VResolution;
        }
        set
        {
                VSnapResolution = value;
            _VResolution=value;
        }

    }
   int FindCTx = 10;
   int FindCTy = 10;
   xy LastProject = new xy(0, 0);

    /// <summary>
    /// projects a 3D-point to the <see cref="Surface"/> and returns the cooordinates UV. so you get
    /// the <b>Point</b> by <see cref="Value(double, double)"/> with the parameter UV.x and Uv.y.
    /// </summary>
    /// <param name="Point"></param>
    /// <returns>the parameter u and v.</returns>
    public virtual xy ProjectPoint(xyz Point)
    {
        xyz P = Point;
        xy Result = ProjectIntheNearOf;
        double x = Result.x;
        double y = Result.y;
       

        bool Weiter = false;
        double dd = Value(x, y).dist(Point);
        if ((dd > 0.01) || double.IsNaN(dd))
        {
            FindBestIndices(Point);
            x = ProjectIntheNearOf.x;
            y = ProjectIntheNearOf.y;
        }

        int Anz = 0;
        Base Ba = new Base(true);

        Ba.BaseO = Point;
        do
        {

            Anz++;

            Ba.BaseX = uDerivation(x, y);
            Ba.BaseY = vDerivation(x, y);
            Ba.BaseZ = (Ba.BaseX & Ba.BaseY);
            if (Ba.BaseZ.dist(xyz.Null) < 0.00000000001)
                Weiter = false;
            else
            {
                xyz Pkt = Value(x, y);
                xyz K = Ba.Relativ(Pkt);
                xy NP = clampToDefArea(new xy(x - K.x, y - K.y));
                x = NP.x;
                y = NP.y;
             }

            double dist = Value(x, y).dist(Point);

            if (dist < 0.00001)
            {
                return clampToDefArea(new xy(x, y));
            }
  
            if (dist >= dd - 0.0001)
            {
                if (Anz <= 2)
                {
                    FindBestIndices(P);

                    dist = Value(ProjectIntheNearOf.x, ProjectIntheNearOf.y).dist(Point);

                    x = ProjectIntheNearOf.x;
                    y = ProjectIntheNearOf.y;
                    dd = dist;
                    Weiter = true;
                }
                else
                {
                    Weiter = false;
                    break;
                }

            }
            else
            {
                dd = dist;
                Weiter = true;
            }

        }
        while (Weiter);
        ProjectIntheNearOf = clampToDefArea(new xy(x, y));
        return clampToDefArea(new xy(x, y));
    }
    xy clampToDefArea(xy value)
    {
        Rectangled rect = GetDefinitionDomain();
        if (value.x < rect.Left)
            value.x = rect.Left;
        if (value.y < rect.Bottom)
            value.y = rect.Bottom;
        if (value.x > rect.Left + rect.Width)
            value.x = rect.Left + rect.Width;
        if (value.y > rect.Bottom + rect.Height)
            value.y = rect.Bottom + rect.Height;
      return value;
    }
    /// <summary>
    /// uDerivation returns the partial uderivation.
    /// </summary>
    /// <param name="u">first parameter</param>
    /// <param name="v">second parameter</param>
    /// <returns>partial uderivation</returns>
    public abstract xyz uDerivation(double u, double v);
        /// <summary>
        /// vDerivation returns the partial vderivation.
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>partial vderivation</returns>
    public abstract xyz vDerivation(double u, double v);
        /// <summary>
        /// This method calculates a crosspoint of a <see cref="Drawing3d.LineType"/> with the surface, 
        /// if this exists.
        /// In that case the crosspoint is given by the values u and v and can be calculated with
        /// the <see cref="Value(double, double)"/>-function
        /// </summary>
        /// <param name="L">A <see cref="Drawing3d.LineType"/></param>
        /// <param name="u">is the u parameter of the crosspoint.</param>
        /// <param name="v">is the v parameter of the crosspoint.</param>
        /// <returns>if the line crosses the surface then the result is true else it is false</returns>
        public virtual bool getCross(LineType L, ref double u, ref double v)
    {
        double _u, _v;
        double deltaU = 1 / (float)UResolution;
        double deltaV = 1 / (float)VResolution;
        Triangle T;
        xyz A, B, C,D;
        double Dist = 1e10; // von PolyCurveex
        for (_u = 0; _u < 1; _u = _u + deltaU)
            for (_v = 0; _v < 1; _v = _v + deltaV)
            {   
                A = Value(_u, _v);
                B = Value(_u + deltaU, _v);
                C= Value( _u + deltaU, _v+deltaV);
                D = Value(_u , _v + deltaV);
           xyz _A = new xyz(0, 0, 0);
           xyz _B = new xyz(0, 0, 0);
           xyz _C = new xyz(0, 0, 0);
           xyz _D = new xyz(0, 0, 0);

                    Plane P = new Plane(A, B, D);
                    xyz PT = new xyz(0, 0, 0);
                    double Lam = -1;
                    P.Cross(L, out Lam, out PT);
                    T = new Triangle(A, B, D);
                    if (T.Inside(PT))
                    {
                        L.P = A;
                        P.Cross(L, out Lam, out _A);
                        L.P = B;
                        P.Cross(L, out Lam, out _B);
                        L.P = D;
                        P.Cross(L, out Lam, out _D);
                        xy LamMue = PT.getAffin(_A, _B - _A, _D - _A);
                        Lam = _u + LamMue.x;
                        double  Mue = _v + LamMue.y;
                         xyz hh = Value(Lam,Mue);
                        u = Lam;
                        v = Mue;
                        return true;
                    }
                    else
                    {
                        P = new Plane(B, C, D);
                        Lam = -1;
                        P.Cross(L, out Lam, out PT);
                        T = new Triangle( B, C,D);
                    if (T.Inside(PT))
                    {
                            L.P = C;
                            P.Cross(L, out Lam, out _C);
                            L.P = B;
                            P.Cross(L, out Lam, out _B);
                            L.P = D;
                            P.Cross(L, out Lam, out _D);
                            xy LamMue = PT.getAffin(_C, _D - _C, _B - _C);
                            Lam = _u + (deltaU - LamMue.x);
                            double  Mue = _v + (deltaV - LamMue.y);
                            u = Lam;
                            v = Mue;
                            return true;
                    } 
                }
            }
       
        return (Dist < 1e10);
    }
}
  
}