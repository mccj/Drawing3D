using System;

using System.Collections;
using Drawing3d;
using System.Collections.Generic;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


//#if LONGDEF
using IndexType = System.Int32;
//#else
//using IndexType = System.UInt16;
//#endif
namespace _CtrlRectMrkered
{
    /// <summary>
    /// A Hititem is an Entry of a <see cref="HitItemList"/> and is generated, when a drawing object is snapped.
    /// Every <see cref="MNDevice"/> contains a Hititemlist.
    /// </summary>
    [Serializable]
    public class TriangleInfo
    {
        public IndexType[] Indices;
        public xyzf[] Points;
        public xyzf[] Normals;
        public xyf[] Texture;
        public TriangleInfo()
        { }
        public TriangleInfo TransformAnimator(Matrix Matrix)
        {
            TriangleInfo Result = new TriangleInfo();
            Result.Indices = Indices;
            Result.Points = new xyzf[Points.Length];
            Result.Normals = new xyzf[Normals.Length];
            Result.Texture = new xyf[Texture.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = Matrix * Points[i];
            }
            xyzf Affin = Matrix * new xyzf(0, 0, 0);
            for (int i = 0; i < Points.Length; i++)
            {
                Normals[i] = Matrix * Points[i] - Affin;
            }
            for (int i = 0; i < Texture.Length; i++)
            {
                xyz A = Matrix * new xyz(Texture[i].x, Texture[i].y, 0);
                Texture[i] = new xyf((float)A.x, (float)A.y);
            }
            return Result;
        }
        public TriangleInfo(IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture)
        {
            this.Indices = Indices;
            this.Points = Points;
            this.Normals = Normals;
            this.Texture = Texture;



        }
        public bool Cross(LineType L, ref xyzf Pt, ref int NearestTriangle)
        {
            xyzf P = new xyzf(0, 0, 0);
            bool Res = false;
            double di = 1e10;
            NearestTriangle = -1;
            double Lam = -1;
            double Depth = 1e10;
            xyzf Result = new xyzf(0, 0, 0);
            for (int i = 0; i < Indices.Length; i = i + 3)
            {
                xyzf A = Points[Indices[i]];
                xyzf B = Points[Indices[i + 1]];
                xyzf C = Points[Indices[i + 2]];
                //  if (((B - A) & (C - A)) * L.Direction.toXYZF() < 0) continue;
                TriangleF T = new TriangleF(A, B, C);
                if ((T.Cross(L, ref P, ref Lam)) && (Lam < Depth))
                {
                    Res = true;
                    Depth = Lam;
                    Pt = P;
                    // return true;
                }
                else
                {
                    xyzf t = (T.A + T.B + T.C) * (1f / 3f);
                    double d = t.DistQ(P);
                    if (d < di)
                    {
                        NearestTriangle = i;
                        di = d;
                    }
                    //  if ((T.A + T.B + T.C) * (1f / 3f))}
                }
            }
            return Res;
        }
    }
    [Serializable]
    public class SnapItem
    {
        public bool PtCrossed = false;
        internal bool doExchange = false;
        internal bool Udated = false;
        internal static double Snapdist = 0;
        public int IdInList = -1;
        public int Start = -1;
        xyz Nearest(LineType L, xyzf A, xyzf B, xyzf C)
        {
            xyzArray M = new xyzArray(4);
            M[0] = A.Toxyz();
            M[1] = B.Toxyz();
            M[2] = C.Toxyz();
            M[3] = A.Toxyz();
            double Param = -1;
            double LineLam = -1;
            double n = M.Distance(L, 1e10, out Param, out LineLam);
            return M.Value(Param);
        }
        public bool Crossed = false;

        bool Inside(LineType L, xyzf A, xyzf B, xyzf C)
        {
            TriangleF T = new TriangleF(A, B, C);
            Plane Triangle = new Plane(A, B, C);
            xyzf N = Triangle.NormalUnit.toXYZF();
            double Depth = -1;
            xyz pt = new xyz(0, 0, 0);
            Triangle.Cross(L, out Depth, out pt);
            bool tttt = T.Inside(pt.toXYZF());
            xyzf _P = pt.toXYZF();
            bool result = ((((A - _P) & (B - _P)) * N >= -0.00001)
                   && (((B - _P) & (C - _P)) * N >= -0.00001)
                 && (((C - _P) & (A - _P)) * N >= -0.00001));
            if (tttt != result)
            { }
            return tttt;
        }
        public OpenGlDevice Device = null;
        /// <summary>
        /// 
        /// The value in the info classes are all in local coordinates. If you want to transform it into global coordinates you have to transform
        /// with the Modelmatrix.
        /// </summary>
        public object OfObject = null;
        PolygonMode _PolygonMode = PolygonMode.Fill;
        public PolygonMode PolygonMode
        {
            get { return _PolygonMode; }
            set { _PolygonMode = value; }

        }

        public xyz _Point = new xyz(0, 0, 0);
        public xyz Point
        {
            get { return _Point; }
            set { _Point = value; }
        }
        virtual internal TriangleF GetTriangle()
        {
            if ((TriangleInfo != null))

                return new TriangleF(TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]);
            return null;

        }

        short _PrimId = -1;
        public short PrimId
        {
            get { return _PrimId; }
            set { _PrimId = value; }
        }
        public virtual xyzArray getxyzArray()
        {
            return null;
        }
        TriangleInfo _TriangleInfo = null;
        public TriangleInfo TriangleInfo
        {
            get { return _TriangleInfo; }
            set
            {
                _TriangleInfo = value;



            }
        }
        Matrix _ModelMatrix = Matrix.identity;
        public Matrix ModelMatrix
        {
            get { return _ModelMatrix; }
            set { _ModelMatrix = value; }
        }
        Entity _Object = null;
        public Entity Object
        {
            get { return _Object; }
            set { _Object = value; }
        }
        [NonSerialized]
        object _Tag = null;
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }

        }
        public double Depth = -1;
        /// <summary>
        /// Constructor for HitInfo
        /// </summary>
        public SnapItem()
        {

        }

        internal virtual protected xyz Cross(LineType ViewLine)
        {
            xyz pt = new xyz(0, 0, 0);
            if ((Device.Selector.IntersectionUsed) && (TriangleInfo != null))
            //  if (TriangleInfo != null)
            {
                Plane Triangle = new Plane(TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]);


                if (Inside(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]))
                    Triangle.Cross(ViewLine, out Depth, out pt);
                else
                    pt = Nearest(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]);

                return pt;
            }
            else
            {
                int N = -1;
                xyzf Pk = new xyzf(0, 0, 0);
                if (TriangleInfo.Cross(ViewLine, ref Pk, ref N))
                {
                    return Pk.Toxyz();
                }
                else
                {

                    return Nearest(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[N]], TriangleInfo.Points[TriangleInfo.Indices[N + 1]], TriangleInfo.Points[TriangleInfo.Indices[N + 2]]);

                    //Plane P =  new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
                    //double Lam = 0;
                    //xyz R = new xyz(0, 0, 0);
                    //P.Cross(ViewLine, out Lam, out R);
                    //return R;     

                }
            }
            return new xyz(0, 0, 0);

        }
        public virtual Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if ((TriangleInfo != null))
            {
                Plane Triangle = new Plane(ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]);

                return Base.DoComplete(Point, Triangle.NormalUnit * (-1));

            }
            return B;

        }
        public virtual void draw(OpenGlDevice Device)
        {
        }


    }
    [Serializable]
    public class SurfaceSnapItem : SnapItem
    {
        public Surface Surface;
        public SurfaceSnapItem()
        { }
        public xy Params = new xy(0, 0);
        xy GetUV()
        {
            return Surface.ProjectPoint(ModelMatrix.invert() * Point);

        }
        public override xyzArray getxyzArray()
        {
            xyzArray Result = new xyzArray();
            int id = 0;
            int ct = 2 * (Surface.UResolution + 1) + 2 * (Surface.VResolution + 1);
            Result.data = new xyz[ct];
            int k = Result.Count;
            for (int i = 0; i <= Surface.UResolution; i++)
            {
                double u = (double)i / (double)Surface.UResolution;
                double v = 0;
                Result.data[id] = Surface.Value(u, v);
                id++;
            }


            for (int i = 0; i <= Surface.VResolution; i++)
            {
                double v = (double)i / (double)Surface.VResolution;
                double u = 1;
                Result.data[id] = Surface.Value(u, v);
                id++;
            }

            for (int i = Surface.UResolution; i >= 0; i--)
            {
                double u = (double)i / (double)Surface.UResolution;
                double v = 1;
                Result.data[id] = Surface.Value(u, v);
                id++;
            }
            for (int i = Surface.VResolution; i >= 0; i--)
            {
                double v = (double)i / (double)Surface.VResolution;
                double u = 0;
                xyz U = Surface.Value(u, v);
                Result.data[id] = U;
                id++;
                xyz II = Result.data[i];
            }
            Result.data[Surface.UResolution] = Surface.Value(1, 0);



            return Result;
        }
        protected internal override xyz Cross(LineType ViewLine)
        {
            Point = ModelMatrix * base.Cross(ViewLine);
            Params = GetUV();
            if (Params.y >= 1) Params.y = 1;


            return Surface.Value(Params.x, Params.y);
        }
        public override Base GetBase()
        {

            xy UV = GetUV();
            if ((UV.x < 0) || (UV.x < 0) || (UV.y < 0) || (UV.y < 0))
            { }
            Base B = Base.UnitBase;
            B.BaseO = Surface.Value(UV.x, UV.y);
            B.BaseZ = Surface.Normal(UV.x, UV.y).normalized(); ;
            B.BaseX = Surface.uDerivation(UV.X, UV.Y).normalized();
            B.BaseY = B.BaseZ & B.BaseX;
            B = this.ModelMatrix * B;
            return B;
        }
    }
    [Serializable]
    public class LineSnapItem : SnapItem
    {
        public double Lam = -1;
        public xyz A = new xyz(0, 0, 0);
        public xyz B = new xyz(0, 0, 0);
        public LineSnapItem(xyz A, xyz B)
        {
            this.A = A;
            this.B = B;
        }
        public override xyzArray getxyzArray()
        {
            xyzArray M = new xyzArray(2);
            M[0] = A;
            M[1] = B;
            return this.ModelMatrix * M;
        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            LineType L = new LineType(A, B - A);

            if (ViewLine.Distance(L, Snapdist, false, false, out Lam) <= Snapdist)
                return L.Value(Lam);
            return A;
        }
    }
    [Serializable]
    public class PointSnapItem : SnapItem
    {
        public xyz A = new xyz(0, 0, 0);

        public PointSnapItem(xyz A)
        {
            this.A = A;
            this.PolygonMode = PolygonMode.Point;

        }
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;

            return B;

        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            return A;
        }
    }
    [Serializable]
    public class CurveSnapItem : SnapItem
    {
        public Curve Curve = null;
        public double Lam = -1;
        public override xyzArray getxyzArray()
        {

            return this.Curve.ToXYArray().Toxyz(this.ModelMatrix);
        }
        public CurveSnapItem(Curve Curve)
        {
            this.Curve = Curve;
        }
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * Curve.Derivation(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }


            return B;

        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            xyzArray A = Curve.ToXYArray().ToxyzArray();

            double LineLam = -1;
            double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out LineLam);

            if (di <= 2 * Snapdist)
            {
                doExchange = true;
                Lam = Lam / Curve.Resolution;
                return Curve.Value(Lam).toXYZ();
            }
            else
            {
                Lam = -1;
            }
            xyz Result = new xyz(0, 0, 0);

            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
        }
    }
    [Serializable]
    public class PolyCurveSnapItem : SnapItem
    {
        public CurveArray CurveArray = null;
        public double Lam = -1;
        public PolyCurveSnapItem(CurveArray CurveArray)
        {
            this.CurveArray = CurveArray;
        }
        public override xyzArray getxyzArray()
        {
            return CurveArray.getxyArray().Toxyz(this.ModelMatrix);
        }
        public override Base GetBase()
        {
            xyzArray A = ModelMatrix * CurveArray.getxyArray().ToxyzArray();

            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * CurveArray.Direction(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }
            return B;
        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            xyArray A = this.CurveArray.getxyArray();

            double Dummy = -1;
            double _Lam = -1;
            double di = A.Distance(ViewLine, 2 * Snapdist, out _Lam, out Dummy);
            if (di <= 2 * Snapdist)
            {
                Lam = this.CurveArray.xyArrayIndexToCurveArrayIndex(_Lam);
                doExchange = true;
                return this.CurveArray.Value(Lam).toXYZ();
            }
            else
            { Lam = -1; }
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
        }
    }
    [Serializable]
    public class PolyPolyCurveSnapItem : SnapItem
    {
        public Loca Loca = null;
        public int _Path = -1;
        public int Path
        {
            get { return _Path; }
            set { _Path = value; }

        }
        public double Lam = -1;

        public PolyPolyCurveSnapItem(Loca Loca)
        {
            this.Loca = Loca;

        }
        public override xyzArray getxyzArray()
        {
            if (Path >= 0)
                return Loca[Path].getxyArray().Toxyz(this.ModelMatrix);
            return Loca[0].getxyArray().Toxyz(this.ModelMatrix);

        }
        protected internal override xyz Cross(LineType ViewLine)
        {
            //if (PolygonMode == PolygonMode.Line)
            //    Path = PrimId;
            //else
            //    Path = -1;
            xyArray A = null;
            if (Path >= 0)
            {
                A = Loca[Path].getxyArray();

                double Dummy = -1;
                double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
                if (di <= 2 * Snapdist)
                {
                    Lam = Loca[Path].xyArrayIndexToCurveArrayIndex(Lam);
                    return Loca[Path].Value(Lam).toXYZ();
                }
                else
                    Lam = -1;
            }
            else
            {


                double Distance = 1e10;
                double _Lam = -1;

                for (int i = 0; i < Loca.Count; i++)
                {
                    A = Loca[i].getxyArray();

                    double Dummy = -1;
                    double di = A.Distance(ViewLine, 2 * Snapdist, out _Lam, out Dummy);
                    if (di <= 2 * Distance)
                    {
                        Distance = di;
                        Lam = Loca[i].xyArrayIndexToCurveArrayIndex(_Lam);
                        //Path = i;
                        if ((Path >= 0) && (Lam >= 0))
                            return Loca[Path].Value(Lam).toXYZ();
                        continue;

                    }
                    else
                        Lam = -1;
                }
                if (Distance <= Snapdist)
                {

                    if ((Path >= 0) && (Lam >= 0))
                        return Loca[Path].Value(Lam).toXYZ();
                }
            }
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;


        }
        public override Base GetBase()
        {
            Base B = Base.UnitBase;
            B.BaseO = Point;
            int id = -1;
            xyArray A = null;
            LineType ViewLine = Device.FromScr(Device.MousePos);
            xyz P = ModelMatrix * ViewLine.P;
            xyz Q = ModelMatrix * ViewLine.Q;
            ViewLine = new LineType(P, Q);
            for (int i = 1; i < Loca.Count; i++)
            {
                A = Loca[i].getxyArray();
                A = ModelMatrix * A;


                double Dummy = -1;
                double di = A.Distance(ViewLine, Snapdist * 8, out Lam, out Dummy);
                if (di <= Snapdist)
                {

                    id = i;
                    // break;

                }

            }
            if (id >= 0)

            {
                if (id == 1)
                { }
                if (Lam >= 0)
                {
                    double L = Lam;
                    Lam = Loca[id].xyArrayIndexToCurveArrayIndex(Lam);
                    xyz D = Loca[id].Direction(Lam).toXYZ();
                    B = Base.DoComplete(A.Value(L).toXYZ(), D, new xyz(0, 0, 1) & D);
                }

            }
            else
            {


                B = ModelMatrix.toBase();
                B.BaseO = Point;
            }
            return B;

        }
    }
    [Serializable]
    public class TextSnapItem : SnapItem
    {
        public Font Font = null;
        public string Text = "";
        public int CharId = -1;
        public TextSnapItem(Font Font, string Text, int CharId)
        {
            this.Text = Text;
            this.CharId = CharId;
            this.Font = Font;
        }
    }
    [Serializable]
    public class PolyLineSnapItem : SnapItem
    {
        public xyArray Poly = null;
        public double Lam = -1;
        public PolyLineSnapItem(xyArray Poly)
        {
            this.Poly = Poly;
        }
        public override xyzArray getxyzArray()
        {
            return Poly.Toxyz(ModelMatrix);


        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            double Dummy = -1;

            double di = Poly.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
            if (di <= 2 * Snapdist)
            {

                return Poly.Value(Lam).toXYZ();
            }
            else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);




            return Result;
        }
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * Poly.Direction(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }
            return B;
        }
    }
    [Serializable]
    public class CurveSnapItem3D : SnapItem
    {
        public Curve3D Curve = null;
        public double Lam = -1;
        public CurveSnapItem3D(Curve3D Curve)
        {
            this.Curve = Curve;
        }
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * Curve.ToxyzArray();


        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            double vz = ViewLine.Direction.z;
            if (ViewLine.Direction.z == 0) return new xyz(0, 0, 0);
            xyzArray A = Curve.ToxyzArray();

            double LineLam = -1;
            double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out LineLam);

            if (di <= 2 * Snapdist)
            {
                Lam /= Curve.Resolution;
                return Curve.Value(Lam);
            }
            else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;

        }
        public override Base GetBase()
        {

            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * Curve.Derivation(Lam) - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }
            return B;
        }
    }
    [Serializable]
    public class PolyLineSnapItem3D : SnapItem
    {
        public xyzArray Poly = null;
        public double Lam = -1;
        public PolyLineSnapItem3D(xyzArray Poly)
        {
            this.Poly = Poly;
        }
        public override xyzArray getxyzArray()
        {
            return this.ModelMatrix * Poly;
        }
        protected internal override xyz Cross(LineType ViewLine)
        {
            double Dummy = -1;
            double di = Poly.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
            if (di <= 2 * Snapdist)
            {

                return Poly.Value(Lam);
            }
            else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
        }


        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * Poly.Direction(Lam) - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }
            return B;
        }
    }
    [Serializable]
    public class PolyPolyLineSnapItem : SnapItem
    {
        public Loxy PolyPoly = null;
        public int Path = -1;
        public Double Lam;
        public PolyPolyLineSnapItem(Loxy Poly)
        {
            this.PolyPoly = Poly;

        }
        public override xyzArray getxyzArray()
        {
            if (Path >= 0) return PolyPoly[Path].Toxyz(ModelMatrix);
            return PolyPoly[0].Toxyz(ModelMatrix);
        }
        protected internal override xyz Cross(LineType ViewLine)
        {
            //if (PolygonMode == PolygonMode.Line)
            //    Path = PrimId;
            //else
            //    Path = -1;
            xyArray A = null;
            if (Path >= 0)
            {
                A = PolyPoly[Path];

                double Dummy = -1;
                double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
                if (di < 2 * Snapdist)
                {

                    return PolyPoly[Path].Value(Lam).toXYZ();
                }
                else
                    Lam = -1;
            }
            else
                for (int i = 0; i < PolyPoly.Count; i++)
                {
                    A = PolyPoly[i];

                    double Dummy = -1;
                    double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
                    if (di <= 2 * Snapdist)
                    {
                        //    Path = i;
                        return PolyPoly[i].Value(Lam).toXYZ();
                    }
                    //else
                    //    Path = -1;
                }
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
        }
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase(); ;
            B.BaseO = Point;
            int id = -1;
            for (int i = 0; i < PolyPoly.Count; i++)
            {
                xyArray A = PolyPoly[i];
                LineType ViewLine = Device.FromScr(Device.MousePos);
                double Dummy = -1;
                double di = A.Distance(ViewLine, 2 * Snapdist, out Lam, out Dummy);
                if (di <= 2 * Snapdist)
                {
                    id = i;
                    break;

                }

            }
            if (id >= 0)
            {
                xyzArray A = ModelMatrix * PolyPoly[id].ToxyzArray();
                if (Lam >= 0)
                {
                    xyz D = ModelMatrix * PolyPoly[id].Direction(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                    B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
                }

            }
            else
            {
                xyzArray A = ModelMatrix * PolyPoly[0].ToxyzArray();
                xyz D = A.cross();
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);

            }
            return B;
        }
    }
    [Serializable]
    public class SphereSnapItem : SurfaceSnapItem
    {
        public SphereSnapItem(double Radius)
        {
            this.Radius = Radius;

        }
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Device.drawSphere(Radius);
            Device.PopMatrix();
        }
        protected internal override xyz Cross(LineType ViewLine)
        {

            xyz Normal = ViewLine.Direction & ViewLine.P;
            if (Normal.length() < 0.000001)
                return ViewLine.Value(ViewLine.P.length() - Radius);
            Normal = (Normal & ViewLine.Direction).normalized();
            double d = ViewLine.P * Normal;
            if (System.Math.Abs(d) > Radius) d = Radius;
            return
                ViewLine.Value((-1) * (ViewLine.P * ViewLine.Direction) - System.Math.Sqrt(Radius * Radius - d * d));
        }
        public double Radius;

    }
    [Serializable]
    public class BoxSnapItem : SnapItem
    {
        public xyz Position;
        public xyz Size;
        public xyzArray Polygon = new xyzArray();
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * Polygon;
        }
        public BoxSnapItem(xyz Position, xyz Size)
        {
            this.Position = Position;
            this.Size = Size;

        }

        public override void draw(OpenGlDevice Device)
        {
            Device.drawBox(Position, Size); //<-------------------
        }
        protected internal override xyz Cross(LineType ViewLine)
        {
            if (this.PolygonMode == PolygonMode.Line)
            {
                Polygon = Primitives3d.GetBoxPoints(Position, Size);
                double Param = -1;
                double LineLam1 = -1;
                if (Polygon.Distance(ViewLine, 1e10, out Param, out LineLam1) < 1)
                    return Polygon.Value(Param);
                return new xyz(0, 0, 0);
            }
            xyz[] Result = new xyz[5];
            Plane Triangle = new Plane(TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]);
            //Test
            switch (PrimId)
            {// Oben
                case 0:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[0].Toxyz(),
                    //                TriangleInfo.Points[2].Toxyz(),
                    //                TriangleInfo.Points[1].Toxyz(),
                    //                TriangleInfo.Points[3].Toxyz(),
                    //                TriangleInfo.Points[0].Toxyz()};
                    //Point = Position;
                    //Result[0] = new xyz(Point.x, Point.y, Point.z + Size.z);
                    //Result[1] = new xyz(Point.x, Point.y + Size.y, Point.z + Size.z);
                    //Result[2] = new xyz(Point.x, Point.y + Size.y, Point.z);
                    //Result[3] = new xyz(Point.x, Point.y, Point.z);
                    //Result[4] = Result[0];
                    Result[0] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 1:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[0].Toxyz(),
                    //                TriangleInfo.Points[2].Toxyz(),
                    //                TriangleInfo.Points[1].Toxyz(),
                    //                TriangleInfo.Points[3].Toxyz(),
                    //                 TriangleInfo.Points[0].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 2:// unten
                    //Polygon.data = new xyz[] {TriangleInfo.Points[4].Toxyz(),
                    //                TriangleInfo.Points[6].Toxyz(),
                    //                TriangleInfo.Points[5].Toxyz(),
                    //                TriangleInfo.Points[7].Toxyz(),
                    //                TriangleInfo.Points[4].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z);
                    Result[3] = new xyz(Position.x, Position.y + Size.y, Position.z);
                    Result[4] = Result[0];
                    break;
                case 3:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[4].Toxyz(),
                    //                TriangleInfo.Points[6].Toxyz(),
                    //                TriangleInfo.Points[5].Toxyz(),
                    //                TriangleInfo.Points[7].Toxyz(),
                    //                TriangleInfo.Points[4].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z);
                    Result[3] = new xyz(Position.x, Position.y + Size.y, Position.z);
                    Result[4] = Result[0];
                    break;
                case 4:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[8].Toxyz(),
                    //                TriangleInfo.Points[10].Toxyz(),
                    //                TriangleInfo.Points[9].Toxyz(),
                    //                TriangleInfo.Points[11].Toxyz(),
                    //                TriangleInfo.Points[8].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 5:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[8].Toxyz(),
                    //                TriangleInfo.Points[10].Toxyz(),
                    //                TriangleInfo.Points[9].Toxyz(),
                    //                TriangleInfo.Points[11].Toxyz(),
                    //                TriangleInfo.Points[8].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[4] = Result[0];

                    break;
                case 6:// Hinten
                    //Polygon.data = new xyz[] {TriangleInfo.Points[12].Toxyz(),
                    //                TriangleInfo.Points[14].Toxyz(),
                    //                TriangleInfo.Points[13].Toxyz(),
                    //                TriangleInfo.Points[15].Toxyz(),
                    //                TriangleInfo.Points[12].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x + Size.x, Position.y + Size.Y, Position.z);
                    Result[1] = new xyz(Position.x, Position.y + Size.Y, Position.z);
                    Result[2] = new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 7:// Hinten
                    //Polygon.data = new xyz[] {TriangleInfo.Points[12].Toxyz(),
                    //                TriangleInfo.Points[14].Toxyz(),
                    //                TriangleInfo.Points[13].Toxyz(),
                    //                TriangleInfo.Points[15].Toxyz(),
                    //                TriangleInfo.Points[12].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x + Size.x, Position.y + Size.Y, Position.z);
                    Result[1] = new xyz(Position.x, Position.y + Size.Y, Position.z);
                    Result[2] = new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 8: // Links))
                    //Polygon.data = new xyz[] {TriangleInfo.Points[16].Toxyz(),
                    //                TriangleInfo.Points[18].Toxyz(),
                    //                TriangleInfo.Points[17].Toxyz(),
                    //                TriangleInfo.Points[19].Toxyz(),
                    //                TriangleInfo.Points[16].Toxyz()
                    //};
                    Result[0] = new xyz(Position.x, Position.y + Size.Y, Position.z);
                    Result[1] = new xyz(Position.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y + Size.Y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 9:
                    //Polygon.data = new xyz[] {TriangleInfo.Points[16].Toxyz(),
                    //                TriangleInfo.Points[18].Toxyz(),
                    //                TriangleInfo.Points[17].Toxyz(),
                    //                TriangleInfo.Points[19].Toxyz(),
                    //                TriangleInfo.Points[16].Toxyz()

                    //                  };
                    Result[0] = new xyz(Position.x, Position.y + Size.Y, Position.z);
                    Result[1] = new xyz(Position.x, Position.y, Position.z);
                    Result[2] = new xyz(Position.x, Position.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x, Position.y + Size.Y, Position.z + Size.Z);
                    Result[4] = Result[0];
                    break;
                case 10://Rechts
                    //Polygon.data = new xyz[] {TriangleInfo.Points[20].Toxyz(),
                    //                TriangleInfo.Points[22].Toxyz(),
                    //                TriangleInfo.Points[21].Toxyz(),
                    //                TriangleInfo.Points[23].Toxyz(),
                    //                TriangleInfo.Points[20].Toxyz()
                    //                   };
                    Result[0] = new xyz(Position.x + Size.X, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y + Size.Y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.z);
                    Result[4] = Result[0];
                    break;
                case 11://Rechts
                    //Polygon.data = new xyz[] {TriangleInfo.Points[20].Toxyz(),
                    //                TriangleInfo.Points[22].Toxyz(),
                    //                TriangleInfo.Points[21].Toxyz(),
                    //                TriangleInfo.Points[23].Toxyz(),
                    //                TriangleInfo.Points[20].Toxyz()
                    //                 };
                    Result[0] = new xyz(Position.x + Size.X, Position.y, Position.z);
                    Result[1] = new xyz(Position.x + Size.x, Position.y + Size.Y, Position.z);
                    Result[2] = new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z);
                    Result[3] = new xyz(Position.x + Size.x, Position.y, Position.z + Size.z);
                    Result[4] = Result[0];
                    break;
                default:
                    break;
            }
            Polygon.data = Result;
            double Lam = -1;
            double LineLam = -1;
            double di = Polygon.Distance(ViewLine, Snapdist, out Lam, out LineLam);

            if (di <= Snapdist)
            {
                xyz P = Polygon.Value(Lam);
                doExchange = true;

                //xyz W = ViewLine.Value(LineLam);
                //double mm = P.dist(W);
                return P;

            }
            return base.Cross(ViewLine);
        }
        static bool Cross(LineType L1, LineType L2, ref double Lam1, ref double Lam2)
        {
            xyz PQ = L2.P - L1.P;
            double PQV = PQ * L1.Direction;
            double PQW = PQ * L2.Direction;
            double vv = L1.Direction * L1.Direction;
            double ww = L2.Direction * L2.Direction;
            double vw = L2.Direction * L1.Direction;
            double Det = vv * ww - vw * vw;
            if (Utils.Equals(Det, 0))// parallele
            {
                return false;
            }
            else
            {
                Lam1 = (PQV * ww - PQW * vw) / Det;
                Lam2 = -(PQW * vv - PQV * vw) / Det;

                double d = PQ * (L2.Direction & L1.Direction).normalized();

                return (Math.Abs(d) < 0.0000001) && (0 <= Lam1) && (Lam1 <= 1) && (0 <= Lam2) && (Lam2 <= 1);
            }

        }
        static List<CrossTag1> GetCrossList(xyzArray A, xyzArray B)
        {
            List<CrossTag1> Result = new List<CrossTag1>();
            for (int i = 0; i < A.Count - 1; i++)
            {
                LineType L1 = new LineType(A[i], A[i + 1] - A[i]);
                for (int j = 0; j < B.Count - 1; j++)
                {
                    LineType L2 = new LineType(B[j], B[j + 1] - B[j]);
                    double Lam1 = -1;
                    double Lam2 = -1;
                    if (Cross(L1, L2, ref Lam1, ref Lam2))
                    {
                        Result.Add(new CrossTag1(i + Lam1, j + Lam2));
                    }
                }
            }
            return Result;
        }
    }

}