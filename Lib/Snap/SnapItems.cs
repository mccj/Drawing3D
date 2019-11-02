using System;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//Copyright (C) 2018 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// describes the geometry of the filled snapped <see cref="SnappItem"/> by <see cref="Indices"/>, <see cref="Points"/>, <see cref="Normals"/> and <see cref="Texture"/>. See also <see cref="PolygonMode"/>
    /// Three indices give a triangle of points. E.g. Points[Indices[3*i]], Points[Indices[3*i+1]], Points[Indices[3*i+2]].
    /// Every normal belongs to a point. Texture are 2-dimensional coordinates for a <see cref="Drawing3d.Texture"/>
    /// </summary>
    [Serializable]
    public class TriangleInfo
    {
        /// <summary>
        /// tree sequential indices describe a triangles with the <see cref="Points"/>. They are from type IndexType, this means an integer when the compilation symbol LONGDEF is setted else it is a UInt16
        /// </summary>
        public IndexType[] Indices;
        /// <summary>
        /// are the points of triangles, which are indexed by <see cref="Indices"/>
        /// </summary>
        public xyzf[] Points;
        /// <summary>
        /// belongs to every <see cref="Points"/>.
        /// </summary>
        public xyzf[] Normals;
        /// <summary>
        /// are the <see cref="Drawing3d.Texture"/> coordinates.
        /// </summary>
        public xyf[] Texture;

        /// <summary>
        /// is the constructor TriangleInfo.
        /// </summary>
        /// <param name="Indices">are the <see cref="Indices"/></param>
        /// <param name="Points">are the <see cref="Points"/></param>
        /// <param name="Normals">ares the <see cref="Normals"/></param>
        /// <param name="Texture">are the <see cref="Drawing3d.Texture"/> coordinates.</param>
        public TriangleInfo(IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture)
        {
            this.Indices = Indices;
            this.Points = Points;
            this.Normals = Normals;
            this.Texture = Texture;
        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Pt"></param>
        /// <param name="NearestTriangle"></param>
        /// <returns></returns>
        internal bool Cross(LineType L, ref xyzf Pt, ref int NearestTriangle)
        {
            //Überschaffen
            xyzf P = new xyzf(0, 0, 0);
            bool Res = false;
            double di = 1e10;
            NearestTriangle = -1;
            double Lam = -1;
            double Depth = 1e10;
            xyzf Result = new xyzf(0, 0, 0);
            for (int i = 0; i < Indices.Length - 2; i = i + 3)
            {
                xyzf A = Points[Indices[i]];
                xyzf B = Points[Indices[i + 1]];
                xyzf C = Points[Indices[i + 2]];
                //xyz BC= xyz.BaryCentric(A.Toxyz(), B.Toxyz(), C.Toxyz(), P.Toxyz());
                //if ((BC.x>=-0.1) && (BC.x <= 1.1) && (BC.y >= -0.1) && (BC.y <= 1.1) && (BC.z >= -0.1) && (BC.z <= 1.1))
                //{ }
                //  if (((B - A) & (C - A)) * L.Direction.toXYZF() < 0) continue;
                // Überschaffen
                //  TriangleF T = new TriangleF(A, B, C);
                xyz PD = new xyz(0, 0, 0);
                new Plane(A.Toxyz(), B.Toxyz(), C.Toxyz()).Cross(L, out Lam, out PD);
                //  if (( T.Cross(L, ref P, ref Lam))&& (Lam < Depth))
                if (new TriangleF(A, B, C).Inside(PD.toXYZF()))
                {
                    if (Lam < Depth)
                    {
                        Pt = PD.toXYZF();
                        Res = true;
                        Depth = Lam;
                        //   Pt = P;
                        // return true;
                    }
                }
                else
                {
                    TriangleF T = new TriangleF(A, B, C);
                    xyzf t = (T.A + T.B + T.C) * (1f / 3f);
                    double d = t.Dist(P);
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
 
    /// <summary>
    /// is an entry of <see cref="OpenGlDevice.SnappItems"/>. It holds informations about the object in the near of the mouse position. See also <see cref="OpenGlDevice.SnapMagnetism"/>
    /// If there are more than one the topmost is the first in the <see cref="OpenGlDevice.SnappItems"/>list.
    /// </summary>
    [Serializable]
    public class SnappItem
    {  
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<short> PrimIds = null;
        /// <summary>
        /// draws the <see cref="Triangle"/>s of the <see cref="TriangleInfo"/>.
        /// </summary>
        public void DrawTriangleInfo()
        {
            if (TriangleInfo == null) return;
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Primitives3d.drawTriangles(Device, TriangleInfo.Indices, TriangleInfo.Points, TriangleInfo.Normals, TriangleInfo.Texture,null);
         



            Device.PopMatrix();
        }
        
        /// <summary>
        /// internal
        /// </summary>
        /// <returns></returns>
        internal xyz Standard()
        {
            LineType ViewLine = Device.FromScr(Device.MousePos);
            Plane P = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
            double Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            P.Cross(ViewLine, out Lam, out Result);
            return Result;
        }

        /// <summary>
        /// internal.
        /// </summary>
        internal bool doExchange = false;
        bool _Udated = false;
        /// <summary>
        /// internal.
        /// </summary>
        internal bool Udated
        {
            get { return _Udated; }
            set { _Udated = value; }
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static double Snappdist = 0;
        xyz Nearest(LineType L, xyzf A, xyzf B, xyzf C)
        {
            xyzArray M = new xyzArray(4);
            M[0] = A.Toxyz();
            M[1] = B.Toxyz();
            M[2] = C.Toxyz();
            M[3] = A.Toxyz();
            double Param = -1;
            double LineLam = -1;
            double _LineLam = -1;
            double n = M.Distance(L, 1e10, out Param, out LineLam);
            double DI = 1e10;
            double _Param = -1;
            LineType _LT = new LineType(new xyz(0, 0, 0), new xyz(0, 0, 0));
            LineType LT = new LineType(M[0], M[1] - M[0]);

            double d1 = LT.Distance(L, 1e10, out Param, out LineLam);
            if ((d1 < DI) && (0 <= Param) && (Param <= 1))
            {
                DI = d1;
                _Param = Param;
                _LT = LT;
                _LineLam = LineLam;
            }
            LT = new LineType(M[1], M[2] - M[1]);
            d1 = LT.Distance(L, 1e10, out Param, out LineLam);
            if ((d1 < DI) && (0 <= Param) && (Param <= 1))
            {
                DI = d1;
                _Param = Param;
                _LT = LT;
                _LineLam = LineLam;
            }

            LT = new LineType(M[2], M[3] - M[2]);
            d1 = LT.Distance(L, 1e10, out Param, out LineLam);
            if ((d1 < DI) && (0 <= Param) && (Param <= 1))
            {
                DI = d1;
                _Param = Param;
                _LT = LT;
                _LineLam = LineLam;
            }
            if (d1== 1e10)
            { }
            return L.Value(LineLam);
       }
        /// <summary>
        /// indicates two object, which are in the <see cref="OpenGlDevice.SnapMagnetism"/> distance crosses each other.
        /// </summary>
        public bool Crossed = false;

        bool Inside(LineType L, xyzf A, xyzf B, xyzf C)
        {
            TriangleF T = new TriangleF(A, B, C);
            Plane Triangle = new Plane(A, B, C);
            xyzf N = Triangle.NormalUnit.toXYZF()*(-1);
            double Depth = -1;
            xyz pt = new xyz(0, 0, 0);
            Triangle.Cross(L, out Depth, out pt);
            bool ok = T.Inside(pt.toXYZF());
            xyzf _P = pt.toXYZF();
            bool result = (((((A - _P) & (B - _P)) * N >= -0.00001)
                   && (((B - _P) & (C - _P)) * N >= -0.00001)
                 && (((C - _P) & (A - _P)) * N >= -0.00001))
                 ||
                  (((((A - _P) & (B - _P)) * N*(-1) >= -0.00001)
                   && (((B - _P) & (C - _P)) * N * (-1) >= -0.00001)
                 && (((C - _P) & (A - _P)) * N*(-1) >= -0.00001)))




                 );
            if (ok != result)
            { }
            return ok;
        }
        OpenGlDevice _Device = null;
        /// <summary>
        /// is the device in which the object is drawn.
        /// </summary>
        public OpenGlDevice Device
        {
            get { return _Device; }
            set { SetDevice(value); }
        }
       
        void SetDevice(OpenGlDevice value)
        {
            _Device = value;

        }
        /// <summary>
        /// holds the object to which the snaped object belongs.
        /// </summary>
        public object OfObject = null;

        PolygonMode _PolygonMode = PolygonMode.Fill;
        /// <summary>
        /// is the <see cref="Drawing3d.PolygonMode"/> which is used to paint the object.
        /// </summary>
        public PolygonMode PolygonMode
        {
            get { return _PolygonMode; }
            set { _PolygonMode = value; }

        }
        xyz _Point = new xyz(0, 0, 0);
        /// <summary>
        /// is the point on the snapped object on the mouse posistion.
        /// </summary>
        public xyz Point
        {
            get { return _Point; }
            set { _Point = value; }
        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <returns></returns>
        virtual internal TriangleF GetTriangle()
        {
            if ((TriangleInfo != null))

                return new TriangleF(TriangleInfo.Points[TriangleInfo.Indices[PrimId]], TriangleInfo.Points[TriangleInfo.Indices[PrimId + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId + 2]]);
            return null;

        }
        short _PrimId = -1;
        /// <summary>
        /// gets the index in <see cref="TriangleInfo"/> for the triangle containing the mouse position.
        /// The triangle is TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId * 3 + 2]]
        /// </summary>
        public short PrimId
        {
            get { return _PrimId; }
            set
            {
                _PrimId = value;
                if (value == 0)
                { }
            }
        }
        /// <summary>
        /// returns the border for filled objects and the <see cref="xyzArray"/> of a lined object. See also <see cref="PolygonMode"/>.
        /// </summary>
        /// <returns></returns>
        public virtual xyzArray getxyzArray()
        {
            return null;
        }
        TriangleInfo _TriangleInfo = null;
        /// <summary>
        /// holds the <see cref="TriangleInfo"/> of the snappitem.
        /// </summary>
        public TriangleInfo TriangleInfo
        {
            get { return _TriangleInfo; }
            set
            {
                _TriangleInfo = value;
            }
        }
        Matrix _ModelMatrix = Matrix.identity;
   
        /// <summary>
        /// gets the <see cref="OpenGlDevice.ModelMatrix"/> which is setted for drawing the object.
        /// </summary>
        public Matrix ModelMatrix
        {
            get { return _ModelMatrix; }
            set
            {
                _ModelMatrix = value;
            }
        }
        Entity _Object = null;
        /// <summary>
        /// is the <see cref="Entity"/>, which has drawn the object.
        /// </summary>
        public Entity Object
        {
            get { return _Object; }
            set { _Object = value; }
        }
       
        [NonSerialized]
        object _Tag = null;
        /// <summary>
        /// holds the object which is setted by <see cref="OpenGlDevice.PushTag(object)"/>.
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }

        }

        /// <summary>
        /// gives the depth, which is used for ordering the snappitems. Is the depth greater the object is more far from the eye.
        /// </summary>
        public double Depth = -1;
        /// <summary>
        /// is called when the mouse pointer is moved over the snappitem. It calculates the coordinates of the object.
        /// </summary>
        /// <param name="ViewLine">the line from the eye to the obhect.</param>
        /// <returns></returns>
        internal virtual protected xyz Cross(LineType ViewLine)
        {
            xyz pt = new xyz(0, 0, 0);
            if ((Device.Selector.IntersectionUsed) && (TriangleInfo != null))
           {
                bool OK = false;
                for (int i = 0; i < PrimIds.Count; i++)
                {
                 

                    Plane Triangle = new Plane(TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i]]], TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i] + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i] + 2]]);
                  
                    {
                        if (Inside(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i]]], TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i] + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimIds[i] + 2]]))
                        {
                            if (Triangle.Cross(ViewLine, out Depth, out pt))
                            {
                                
                                OK = true;
                                break;
                            }
                        }
                    }
                  
                }
              
                if (!OK)
                        pt = Nearest(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[PrimId]], TriangleInfo.Points[TriangleInfo.Indices[PrimId + 1]], TriangleInfo.Points[TriangleInfo.Indices[PrimId + 2]]);
              return pt;
            }
            else
            {
                int N = -1;
                xyzf Pk = new xyzf(0, 0, 0);
                if (TriangleInfo.Cross(ViewLine, ref Pk, ref N))
                    return Pk.Toxyz();
                else
                   return Nearest(ViewLine, TriangleInfo.Points[TriangleInfo.Indices[N]], TriangleInfo.Points[TriangleInfo.Indices[N + 1]], TriangleInfo.Points[TriangleInfo.Indices[N + 2]]);
            }
        }
        /// <summary>
        /// returns a <see cref="Base"/> with origin <see cref="Point"/> and the x-axis is thangential for a border of the object.
        /// </summary>
        /// <returns></returns>
        public virtual Base GetBase()        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (PrimId >= 0)
                if ((TriangleInfo != null))
                {
                    Plane Triangle = new Plane(ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId]], ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId+ 1]], ModelMatrix * TriangleInfo.Points[TriangleInfo.Indices[PrimId + 2]]);
                    return Base.DoComplete(Point, Triangle.NormalUnit * (-1));

                }
            return B;
        }
        /// <summary>
        /// draws a snappitem. It is primarily for test a new snappitem.
        /// </summary>
        /// <param name="Device"></param>
        public virtual void draw(OpenGlDevice Device)
        {
        }
   }
    /// <summary>
    /// a <see cref="SnappItem"/>, which belongs to a <see cref="Drawing3d.Surface"/>. It contains the u and v parameter of the surface
    /// and overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class SurfaceSnappItem : SnappItem
    {
        /// <summary>
        /// contains the u and v parameter of the mouse position on the <see cref="Surface"/>.
        /// </summary>
        public xy UV = new xy(-1, -1);
        /// <summary>
        /// the <see cref="Drawing3d.Surface"/>, which is snapped.
        /// </summary>
        public Surface Surface;
        xy GetUV()
        {  xy R= Surface.ProjectPoint(Point);
          
            return R;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and returns the border of the <see cref="Drawing3d.Surface"/>
        /// </summary>
        /// <returns></returns>
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
           }
            Result.data[Surface.UResolution] = Surface.Value(1, 0);
            return Result;
        }
       /// <summary>
       /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates the surface parameter u and v.
       /// </summary>
       /// <param name="ViewLine">line to the eye.</param>
       /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {   
            UV = new xy(-1, -1);
            Point = base.Cross(ViewLine);

            UV = GetUV();

            if ((UV.X > -0.2) && (UV.X < 1.2) && (UV.X > -0.2) && (UV.Y < 1.2))
            {
                xyz PT = ModelMatrix * Surface.Value(UV.x, UV.y);
                if (PT.dist(ModelMatrix * Point)> 2 * Snappdist)
                {
                    xyzArray A = getxyzArray();
                    double param, Lam;
                    A.Distance(ViewLine, 1e10, out param, out Lam);
                    if (param <=Surface.UResolution)
                    {
                        UV.Y = 0;
                        UV.x = param / Surface.UResolution;

                    }
                    else
                    {
                        param -= (Surface.UResolution+1);
                        if (param <= Surface.VResolution)
                        {
                            UV.x = 1;
                            UV.y = param / Surface.VResolution;

                        }
                        else
                        {
                            param -= (Surface.VResolution+1);
                            if (param <= Surface.UResolution)
                            {
                                UV.y = 1;
                                UV.x =1- param / Surface.UResolution;


                            }
                            else
                            {
                                param -= (Surface.UResolution + 1);
                                if (param <= Surface.VResolution)
                                {
                                    UV.x = 0;
                                    UV.y =1- param / Surface.VResolution;
                                }
                            }
                        }
                    }

                }
                return ModelMatrix * Surface.Value(UV.x, UV.y);
            }
            
            else
                return ModelMatrix * Point;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the surface will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Surface.Paint(Device);
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/> by calling the <see cref="Drawing3d.Surface"/> value with <see cref="UV"/>.
        /// </summary>
        public override Base GetBase()
        {
            Base B = Base.UnitBase;
            if (UV.Y <0.01)
            { }
            if ((UV.X > -0.2) && (UV.X < 1.2) && (UV.Y > -0.2) && (UV.Y < 1.2))
            {
                B.BaseO = Surface.Value(UV.x, UV.y);
                B.BaseZ = Surface.Normal(UV.x, UV.y).normalized(); ;
                B.BaseX = Surface.uDerivation(UV.X, UV.Y).normalized();
                B.BaseY = B.BaseZ & B.BaseX;
                B = this.ModelMatrix * B;
            }
            else
            { B = base.GetBase(); }
            return B;
        }
    }

    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the method <see cref="OpenGlDevice.drawLine(xyz, xyz)"/>.
    /// It contains the starting point <see cref="A"/> and the endpoint <see cref="B"/> and
    /// overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class LineSnappItem : SnappItem
    {
        /// <summary>
        /// gets the part of the line in the interval [0,1] where 0 gets A and 1 gets B.
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// the starting point.
        /// </summary>
        public xyz A = new xyz(0, 0, 0);
        /// <summary>
        /// the endpoint.
        /// </summary>
        public xyz B = new xyz(0, 0, 0);
        /// <summary>
        /// is the constructor the starting point <see cref="A"/> and the endpoint <see cref="B"/>
        /// </summary>
        /// <param name="A">the starting point</param>
        /// <param name="B">the endpoint</param>
        public LineSnappItem(xyz A, xyz B)
        {
            this.A = A;
            this.B = B;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and returns an array of length 2 with the elements A and B.
        /// </summary>
        /// <returns></returns>
        public override xyzArray getxyzArray()
        {
            xyzArray M = new xyzArray(2);
            M[0] = A;
            M[1] = B;
            return this.ModelMatrix * M;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point and calculates <see cref="Lam"/>.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            LineType L = new LineType(A, B - A);
            Lam = -1;
            double Mue = -1;
            xyz Pkt = new xyz(0, 0, 0);
            double da = ViewLine.Distance(A, out Lam, out Pkt);
            if (da <= 2 * Snappdist)
            {
                Lam = 0;
                return ModelMatrix* A;
            }
            double db = ViewLine.Distance(B, out Lam, out Pkt);
            if (db <= 2 * Snappdist)
            {
                Lam = 1;
                return ModelMatrix*B;
            }
            L.Distance(ViewLine, 1e10, out Lam, out Mue);
            return L.Value(Lam);
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>.
        /// </summary>
        public override Base GetBase()
        {
            return ModelMatrix * Base.DoComplete(new LineType(A, B - A).Value(Lam), B - A);
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the line will be drawn.</param>
       public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Device.drawLine(A, B);
            Device.PopMatrix();
        }
    }

    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPoint(xyz)"/>.
    /// It contains the point <see cref="A"/> , <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PointSnappItem : SnappItem
    {
        /// <summary>
        /// the point, which is used from to the call <see cref="OpenGlDevice.drawPoint(xyz)"/>.
        /// </summary>
        public xyz A = new xyz(0, 0, 0);
        /// <summary>
        /// is the constructor with the associated point A.
        /// </summary>
        /// <param name="A">the associated point A.</param>
        public PointSnappItem(xyz A)
        {
            this.A = A;
            PolygonMode = PolygonMode.Point;
       }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>.
        /// </summary>
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            return B;
      }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>associated point.</returns>
       protected internal override xyz Cross(LineType ViewLine)
        {

            xyz Pkt = new xyz(0, 0, 0);
            return ModelMatrix * A;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the point will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Device.drawPoint(A, 0.4);
            Device.PopMatrix();
        }
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawCurve(Curve)"/>.
    /// It contains the <see cref="Drawing3d.Curve"/> and a parameter Lam. It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class CurveSnappItem : SnappItem
    {
        /// <summary>
        /// is the <see cref="Drawing3d.Curve"/>, which is drawn.
        /// </summary>
        public Curve Curve = null;
        /// <summary>
        /// is the parameter, which gives the <see cref="SnappItem.Point"/> by curve.value(Lam).
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/>, which contains the points of the curve
        /// </summary>
        /// <returns>xyzArray belonging to the curve</returns>
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * this.Curve.ToXYArray().ToxyzArray();

        }
        /// <summary>
        /// is the constructor with the <b>curve</b>.
        /// </summary>
        /// <param name="Curve">is the curve, which will be drawn.</param>
        public CurveSnappItem(Curve Curve)
        {
            this.Curve = Curve;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangent of the curve.
        /// </summary>
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
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            xyzArray A = Curve.ToXYArray().ToxyzArray();
            double LineLam = -1;
            double di = A.Distance(ViewLine, 2 * Snappdist, out Lam, out LineLam);
            if (di <= 2 * Snappdist)
            {
                doExchange = true;
                Lam = Lam / Curve.Resolution;
                return ModelMatrix * Curve.Value(Lam).toXYZ();
            }
            else
                Lam = -1;        
            xyz Result = new xyz(0, 0, 0);
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the curve will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawCurve(this.Curve);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyCurve(CurveArray)"/>.
    /// It contains the <see cref="Drawing3d.CurveArray"/> and a parameter Lam. You get the <see cref="SnappItem.Point"/> by ModelMatrix *Curvearray.Value(Lam).It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyCurveSnappItem : SnappItem
    {
        /// <summary>
        /// the <see cref="Drawing3d.CurveArray"/>, which will be drawn.
        /// </summary>
        public CurveArray CurveArray = null;
        /// <summary>
        /// the parameter, which gives tthe <see cref="SnappItem.Point"/> by ModelMatrix*CurveArray.value(Lam).
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// is the constructor with the underlyinging <see cref="Drawing3d.CurveArray"/>.
        /// </summary>
        /// <param name="CurveArray">the underlyinging <see cref="Drawing3d.CurveArray"/></param>
        public PolyCurveSnappItem(CurveArray CurveArray)
        {
            this.CurveArray = CurveArray;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the curvearray will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyCurve(CurveArray);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/>, which contains the points of the <see cref="Drawing3d.CurveArray"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.CurveArray"/>.</returns>
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * CurveArray.getxyArray().ToxyzArray();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
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
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            xyArray A = this.CurveArray.getxyArray();
            double Dummy = -1;
            double _Lam = -1;
            double di = A.Distance(ViewLine, 2 * Snappdist, out _Lam, out Dummy);
            if (di <= 2 * Snappdist)
            {
                Lam = this.CurveArray.xyArrayIndexToCurveArrayIndex(_Lam);
                doExchange = true;
                return ModelMatrix*this.CurveArray.Value(Lam).toXYZ();
            }
            else
             Lam = -1; 
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            if (PolygonMode == PolygonMode.Fill)
            {
                new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
                return ModelMatrix*Result;
            }
            else
                return Standard();
        }
    }
  
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawText(Font, Matrix, string, double)"/>.
    /// It contains the <see cref="Text"/>, the <see cref="StartPos"/>,the <see cref="Font"/>, the <see cref="CharId"/> and the <see cref="CharPos"/>.
    /// </summary>
    [Serializable]
    public class TextSnappItem : SnappItem
    {
        /// <summary>
        /// is the position, where the text is drawn.
        /// </summary>
        public xyz StartPos = new xyz(0, 0, 0);
        /// <summary>
        /// is the <see cref="Drawing3d.Font"/>, which is used in <see cref="OpenGlDevice.drawText(Font, Matrix, string, double)"/>.
        /// </summary>
        public Font Font = null;
        string _Text = "";
        /// <summary>
        /// is the text, which is used in <see cref="OpenGlDevice.drawText(Font, Matrix, string, double)"/>.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        /// <summary>
        /// is the char, where the mouse pointer is.
        /// </summary>
        public int CharId = -1;
        /// <summary>
        /// is the position of the char, where the mouse pointer is.
        /// </summary>
        public xyz CharPos = new xyz(0, 0, 0);
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the text will be drawn.</param>
       public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.ModelMatrix = ModelMatrix;
            // Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawText(Font, Text, 1);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// is the constructor.
        /// </summary>
        /// <param name="Font">the<see cref="Drawing3d.Font"/>, which is used in <see cref="OpenGlDevice.drawText(Font, Matrix, string, double)"/>.</param>
        /// <param name="Text">the text, which is used in <see cref="OpenGlDevice.drawText(Font, Matrix, string, double)"/>.</param>
        /// <param name="CharId">the char</param>
        /// <param name="StartPos">the position where the text will be drawn.</param>
        /// <param name="CharPos">the position of the char</param>
        public TextSnappItem(Font Font, string Text, int CharId,xyz StartPos, xyz CharPos)
        {
            this.Text = Text;
            this.CharId = CharId;
            this.Font = Font;
            this.CharPos = CharPos;
            this.StartPos = StartPos;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the position of the text.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            return CharPos;           
        }
   }
    /// <summary>
    /// is a <see cref="SnappItem"/>, which belongs to a <see cref="Mesh"/>.
    /// </summary>
    [Serializable]
    public class MeshSnappItem : SnappItem
    {
        /// <summary>
        /// is the <see cref="Mesh"/> belonging to the <see cref="MeshSnappItem"/>,
        /// </summary>
        public Mesh Mesh = null;
        /// <summary>
        /// is the constructor with a <see cref="Mesh"/>.
        /// </summary>
        public MeshSnappItem(Mesh Mesh)
        {   
            this.Mesh = Mesh;
            if ((Mesh.Position!=null))
            {

                xyz[] Data = new xyz[Mesh.Position.Length+1];
                for (int i = 0; i < Mesh.Position.Length; i++)
                {
                    xyzf PT = Mesh.Position[i];
                    Data[i] = new xyz(PT.x, PT.y, PT.z);
                }
                Data[Mesh.Position.Length] = Data[0];
                PolyLine = new xyzArray();
                PolyLine.data = Data;
            }
        }
        xyzArray PolyLine = new xyzArray();
        double Lam = -1;
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">the line from the eye.</param>
        /// <returns></returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            if (TriangleInfo != null)
                return base.Cross(ViewLine);
            double Dummy = -1;
            Lam = -1;
            double di = PolyLine.Distance(ViewLine, 1e10, out Lam, out Dummy);
            if (di <= 2 * Snappdist)
                return ModelMatrix * (PolyLine.Value(Lam)); 
            else
                Lam = -1;
         
            return new xyz(0, 0, 0);
            
        }
        
        /// <summary>
        /// overrides the <see cref="SnappItem.draw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Mesh.Paint(Device);
            Device.PopMatrix();
        }
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyPolyCurve(Loca)"/>.
    /// It contains the <see cref="Drawing3d.Loca"/> a <b>Path</b> and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loca[Path].Value(Lam).It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyPolyCurveSnappItem : SnappItem
    {
        /// <summary>
        /// the <see cref="Drawing3d.Loca"/>, which will be drawn.
        /// </summary>
        public Loca Loca = null;
        int _Path = -1;
        /// <summary>
        /// is the id of the <see cref="Drawing3d.CurveArray"/> in <see cref="Drawing3d.Loca"/>, where the mouse pointer is. 
        /// </summary>
        public int Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        /// <summary>
        /// is the parameter, which gives tthe <see cref="SnappItem.Point"/> by ModelMatrix * Loca[Path].getxyArray().
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the Loca will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyPolyCurve(Loca);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// is the constructor with the underlyinging <see cref="Drawing3d.Loca"/>.
        /// </summary>
        /// <param name="Loca"> is the underlyinging <see cref="Drawing3d.Loca"/></param>
        public PolyPolyCurveSnappItem(Loca Loca)
        {
            this.Loca = Loca;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/>, which contains the points of the <see cref="Drawing3d.Loca"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.Loca"/>.</returns>
        public override xyzArray getxyzArray()
        {
            if (Path >= 0)
                return this.ModelMatrix * Loca[Path].getxyArray().ToxyzArray();
            return this.ModelMatrix * Loca[0].getxyArray().ToxyzArray();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Path</b> and <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            Path = -1;
            Lam = -1;
            xyArray A = null;
            {
                double Distance = 1e10;
                double _Lam = -1;
                for (int i = 0; i < Loca.Count; i++)
                {
                    A = Loca[i].getxyArray();
                    double Dummy = -1;
                    double di = A.Distance(ViewLine, 2 * Snappdist, out _Lam, out Dummy);
                    if (di <= 2 * Distance)
                    {
                        Distance = di;
                        Lam = Loca[i].xyArrayIndexToCurveArrayIndex(_Lam);
                        Path = i;
                        if ((Path >= 0) && (Lam >= 0))
                        {
                            xyz Pkt = ModelMatrix * Loca[Path].Value(Lam).toXYZ();
                            return ModelMatrix * Loca[Path].Value(Lam).toXYZ();
                        }
                        continue;
                   }
                    else
                        Lam = -1;
                }
            }
            if (PolygonMode == PolygonMode.Line)
                return Standard();
            double Lam1;
            xyz Result = new xyz(0, 0, 0);
            Plane P = new Plane(Loca[0][0].A.toXYZ(), new xyz(0, 0, 1));
            P.Cross(ViewLine, out Lam1, out Result);
            return ModelMatrix * Result;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if ((Path >= 0) && (Lam >= 0))
            {
                xyz D = ModelMatrix * Loca[Path].Direction(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }
            return B;
        }
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyLine(xyArray)"/>.
    /// It contains the <see cref="Drawing3d.xyArray"/>and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Poly[Path].Value(Lam).It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyLineSnappItem : SnappItem
    {
        /// <summary>
        /// is the <see cref="Drawing3d.xyArray"/>, which is used from <see cref="OpenGlDevice.drawPolyLine(xyArray)"/>.
        /// </summary>
        public xyArray Poly = null;
        /// <summary>
        /// is the parameter, which gives the <see cref="Point"/> by ModelMatrix * Poly.Value(Lam).
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// is the constructor with the underlying <b>Poly</b>
        /// </summary>
        /// <param name="Poly">is the <see cref="Drawing3d.xyArray"/>, which is used from <see cref="OpenGlDevice.drawPolyLine(xyArray)"/></param>
        public PolyLineSnappItem(xyArray Poly)
        {
            this.Poly = Poly;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/>, which contains the points of the <see cref="Drawing3d.xyArray"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.xyArray"/>.</returns>
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * Poly.ToxyzArray();// Mit Model Matrix ??
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the poly will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyLine(Poly);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            double Dummy = -1;
            Lam = -1;
            double di = Poly.Distance(ViewLine, 2 * Snappdist, out Lam, out Dummy);
            if (di <= 2 * Snappdist)     
                return ModelMatrix * (Poly.Value(Lam)).toXYZ(); // Distance checken
             else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
           return Result;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
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
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawCurve(Curve3D)"/>.
    /// It contains the <see cref="Drawing3d.Curve3D"/>and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by Curve.Value(Lam).It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class CurveSnappItem3D : SnappItem
    {
        /// <summary>
        /// the curve which is used from <see cref="OpenGlDevice.drawCurve(Curve3D)"/>.
        /// </summary>
        public Curve3D Curve = null;
        /// <summary>
        /// is a parameter, which can used to calculate the <see cref="Point"/> with Curve.Value(Lam).
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// is the constructor with the belonging curve.
        /// </summary>
        /// <param name="Curve">the curve, which is used from <see cref="OpenGlDevice.drawCurve(Curve3D)"/>.</param>
        public CurveSnappItem3D(Curve3D Curve)
        {
            this.Curve = Curve;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and contains the points of the <see cref="Drawing3d.Curve3D"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.Curve3D"/>.</returns>
        public override xyzArray getxyzArray()
        {
            return ModelMatrix * Curve.ToxyzArray();
       }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the curve will be drawn.</param>
       public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawCurve(Curve);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the cross point.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            xyzArray A = Curve.ToxyzArray();
            double LineLam = -1;
            double di = A.Distance(ViewLine, 2 * Snappdist, out Lam, out LineLam);
           if (di <= 2 * Snappdist)
            {
                Lam /= Curve.Resolution;
                return ModelMatrix*Curve.Value(Lam);
            }
            else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return Result;
       }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
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
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyLine(xyzArray)"/>.
    /// It contains the <see cref="Drawing3d.xyzArray"/> (Poly) and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by Poly.Value(Lam).
    /// It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyLineSnappItem3D : SnappItem
    {
        /// <summary>
        /// is the <see cref="xyzArray"/> belonging to the <see cref="PolyLineSnappItem3D"/>.
        /// </summary>
        public xyzArray Poly = null;
        /// <summary>
        /// is the parameter, which is set from <see cref="PolyLineSnappItem3D.Cross(LineType)"/> to a value not -1, if the mouseposition is in
        /// the near of the <b>Poly</b>. You get the point by <b>Poly.Value(Lam)</b>.
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// is the constructor with the belonging Poly.
        /// </summary>
        /// <param name="Poly"> is the <see cref="Drawing3d.xyzArray"/> , which is used from <see cref="OpenGlDevice.drawPolyLine(xyzArray)"/>.</param>
        public PolyLineSnappItem3D(xyzArray Poly)
        {
            this.Poly = Poly;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the poly will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyLine(Poly);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and contains the points of the <see cref="Drawing3d.xyzArray"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.xyzArray"/>.</returns>
        public override xyzArray getxyzArray()
        {
            return this.ModelMatrix * Poly;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the nearest point to the <see cref="Drawing3d.xyzArray"/> or the cross point with the plane of the <see cref="Drawing3d.xyzArray"/>.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            double Dummy = -1;
            double di = Poly.Distance(ViewLine, 2 * Snappdist, out Lam, out Dummy);
            if (di <= 2 * Snappdist)
         
                return ModelMatrix * Poly.Value(Lam);
            else
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            LineType LT = Device.FromScr(Device.MousePos);
            if (PolygonMode == PolygonMode.Fill)
            {
                new Plane(ModelMatrix * Poly.Base.BaseO, ModelMatrix.multaffin(Poly.Base.BaseZ)).Cross(LT, out LineLam, out Result);
                return Result;
            }
            return Device.Currentxyz;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
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
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyPolyLine(Loxyz)"/>.
    /// It contains the <see cref="Drawing3d.Loxyz"/> an id and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxyz[id].Value(Lam) if id >=0.
    /// It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyPolyLineSnappItem3D : SnappItem
    {
        /// <summary>
        /// the <see cref="Drawing3d.Loxyz"/> which belongs to the call <see cref="OpenGlDevice.drawPolyPolyLine(Loxyz)"/>.
        /// </summary>
        public Loxyz Loxyz = null;
        /// <summary>
        /// is a parameter.  You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxyz[id].Value(Lam) if id >=0. 
        /// </summary>
        public double Lam = -1;
        /// <summary>
        /// is the constructor with the belonging <see cref="Drawing3d.Loxyz"/>. 
        /// </summary>
        /// <param name="Loxyz"></param>
        public PolyPolyLineSnappItem3D(Loxyz Loxyz)
        {
            this.Loxyz = Loxyz;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the Loxyz will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyPolyLine(Loxyz);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and contains the points of the <see cref="Drawing3d.Loxyz"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.Loxyz"/>.</returns>
       public override xyzArray getxyzArray()
        {
            if (Loxyz.Count > 0)
                return this.ModelMatrix * Loxyz[0];
            return new xyzArray();
        }
        /// <summary>
        /// indicates the member of the Loxyz. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxyz[id].Value(Lam) if id >=0. 
        /// </summary>
        int id = -1;
        /// <summary>
        /// is the parameter in the <see cref="Drawing3d.xyzArray"/> indexed by <see cref="id"/>. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxyz[id].Value(Lam) if id >=0. 
        /// </summary>
        double _Lam = -1;
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>id</b> and <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the nearest point to the <see cref="Drawing3d.Loxyz"/> or the cross point with the plane of the <see cref="Drawing3d.Loxyz"/>.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            double Dummy = -1;
            double _di = 0;
            double di = 1e10;
            for (int i = 0; i < Loxyz.Count; i++)
            {
                _di = Loxyz[i].Distance(ViewLine, 2 * Snappdist, out _Lam, out Dummy);
                if (_di < di)
                {
                    id = i;
                    Lam = _Lam;
                    di = _di;

                }
                if (di <= 2 * Snappdist)
                {
                    return ModelMatrix * Loxyz[id].Value(Lam);
                }
            }
           
          
                Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            LineType LL = Device.FromScr(Device.MousePos);
            if (PolygonMode == PolygonMode.Fill)
            {
                new Plane(ModelMatrix * Loxyz.Base.BaseO, ModelMatrix.multaffin(Loxyz.Base.BaseZ)).Cross(LL, out LineLam, out Result);
                return Result;
            }
            return Device.Currentxyz;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
        public override Base GetBase()
        {  
            Base B = ModelMatrix.toBase();
            B.BaseO = Point;
            if (Lam >= 0)
            {
                xyz D = ModelMatrix * Loxyz[id].Direction(Lam) - ModelMatrix * new xyz(0, 0, 0);
                B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
            }

            return B;
        }
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawPolyPolyLine(Loxy)"/>.
    /// See also <see cref="OpenGlDevice.drawPolyPolyLine(Loxyz)"/>
    /// It contains the <see cref="Drawing3d.Loxy"/> a path and a parameter <b>Lam</b>. You get the <see cref="SnappItem.Point"/> by Loxy[path].Value(Lam) if path >=0.
    /// It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class PolyPolyLineSnappItem : SnappItem
    {
        /// <summary>
        /// the <see cref="Drawing3d.Loxy"/>
        /// </summary>
        public Loxy PolyPoly = null;
        /// <summary>
        /// indicates the member of the Loxyz. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxyz[Path].Value(Lam) if Path >=0. 
        /// </summary>
        public int Path = -1;
        /// <summary>
        /// is the parameter in the <see cref="Drawing3d.xyArray"/> indexed by <see cref="Path"/>. You get the <see cref="SnappItem.Point"/> by ModelMatrix * Loxy[Path].Value(Lam) if Path >=0. 
        /// </summary>
        public Double Lam;
        /// <summary>
        /// is the constructor with the belonging <see cref="Drawing3d.Loxy"/>. 
        /// </summary>
        /// <param name="Poly"></param>
        public PolyPolyLineSnappItem(Loxy Poly)
        {
            this.PolyPoly = Poly;

        }
        /// <summary>
        /// overrides <see cref="SnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the Loxy will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            Device.drawPolyPolyLine(PolyPoly);
            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and contains the points of the <see cref="Drawing3d.Loxy"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.Loxy"/>.</returns>
        public override xyzArray getxyzArray()
        {
            if (Path >= 0) return ModelMatrix * PolyPoly[Path].ToxyzArray();
            return ModelMatrix * PolyPoly[0].ToxyzArray();
        }
        /// <summary>
        /// overrides <see cref="SnappItem.Cross(LineType)"/> and calculates <b>Path</b> and <b>Lam</b>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the nearest point to the <see cref="Drawing3d.Loxy"/>.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
            Path = -1;
            Lam = -1;
            xyArray A = null;
                for (int i = 0; i < PolyPoly.Count; i++)
                {
                    A = PolyPoly[i];
                    double Dummy = -1;
                    double di = A.Distance(ViewLine, 2 * Snappdist, out Lam, out Dummy);
                    if (di <= 2 * Snappdist)
                    {
                        Path = i;
                        return ModelMatrix * PolyPoly[i].Value(Lam).toXYZ();
                    }
                }
            if (PolygonMode== PolygonMode.Line)
            { return Standard(); }
            xyz Result = new xyz(0, 0, 0);
            double LineLam = -1;
            new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1)).Cross(ViewLine, out LineLam, out Result);
            return ModelMatrix*Result;
        }
        /// <summary>
        /// overrides <see cref="SnappItem.GetBase"/>, where the x-axis is the tangential.
        /// </summary>
        public override Base GetBase()
        {
            Base B = ModelMatrix.toBase(); ;
            B.BaseO = Point;
            if (Path >=0)
            {
               
                    xyzArray A = ModelMatrix * PolyPoly[Path].ToxyzArray();
                    xyz D = ModelMatrix * PolyPoly[Path].Direction(Lam).toXYZ() - ModelMatrix * new xyz(0, 0, 0);
                    B = Base.DoComplete(Point, D, new xyz(0, 0, 1) & D);
                   

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
    /// <summary>
    /// is a <see cref="SurfaceSnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawSphere(xyz, double)"/>.
    /// It contains the center and the radius. 
    /// It overrides <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class SphereSnappItem : SurfaceSnappItem
    {
        /// <summary>
        /// the center of the sphere.
        /// </summary>
        public xyz Center
        { get { return ModelMatrix * new xyz(0, 0, 0); } }
        /// <summary>
        /// constuctor with the radius.
        /// </summary>
        /// <param name="Radius"> radius of the sphere.</param>
        public SphereSnappItem(double Radius)
        {
            this.Radius = Radius;
        }
        /// <summary>
        /// overrides <see cref="SurfaceSnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the Loxy will be drawn.</param>
        public override void draw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            Device.drawSphere(Radius);
            Device.PopMatrix();
        }
        xyz _Cross(LineType ViewLine)
        {
            Plane P = new Plane(new xyz(0, 0, 0), new xyz(1, 0, 0), new xyz(0, 1, 0));
            double Lam = -1;
            xyz PP = new xyz(0, 0, 0);
            P.Cross(ViewLine, out Lam, out PP);
            LineType VL = new LineType(PP, ViewLine.Direction);
            xyz Normal = VL.Direction & VL.P;
            if (Normal.length() < 0.000001)
                return ViewLine.Value(VL.P.length() - Radius);
            Normal = (Normal & VL.Direction).normalized();
            double d = VL.P * Normal;
            if (System.Math.Abs(d) > Radius) d = Radius;
            xyz H = VL.Value((-1) * (VL.P * VL.Direction) - System.Math.Sqrt(Radius * Radius - d * d));
            double dd = ViewLine.Distance(new xyz(0, 0, 0), out Lam, out PP);
            if (dd > Radius)
            { return PP.normalized() * Radius; }
            return
                ViewLine.Value((-1) * (ViewLine.P * ViewLine.Direction) - System.Math.Sqrt(Radius * Radius - d * d));
        }
        /// <summary>
        /// overrides <see cref="SurfaceSnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the nearest point to the eye.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {

            xyz Result = _Cross(ViewLine);
            //double dd = Result.length();
            UV = new xy(-1, -1);
            UV = Surface.ProjectPoint(Result);
           
            if ((UV.X > -0.2) && (UV.X < 1.2) && (UV.X > -0.2) && (UV.Y < 1.2))
                return ModelMatrix * Surface.Value(UV.x, UV.Y);
            return ModelMatrix * Result;
        }
     /// <summary>
     /// radius of the sphere.
     /// </summary>
      public double Radius;
    }
    /// <summary>
    /// is a <see cref="SnappItem"/> which belongs to the call <see cref="OpenGlDevice.drawBox(xyz, xyz)"/>.
    /// It contains an <b>id</b> the <b>position</b> and the <b>size</b>. 
    /// It overrides <see cref="SnappItem.getxyzArray"/>, <see cref="SnappItem.Cross(LineType)"/>, <see cref="SnappItem.draw(OpenGlDevice)"/> and <see cref="SnappItem.GetBase"/>
    /// </summary>
    [Serializable]
    public class BoxSnappItem : SnappItem
    {
        /// <summary>
        /// the position of the box.
        /// </summary>
        public xyz Position;
        /// <summary>
        /// the size of the box.
        /// </summary>
        public xyz Size;
        int Id = -1;
        /// <summary>
        /// overrides <see cref="SnappItem.getxyzArray"/> and contains the points of the <see cref="Drawing3d.Loxy"/>.
        /// </summary>
        /// <returns>the points of the <see cref="Drawing3d.Loxy"/>.</returns>
       public override xyzArray getxyzArray()
        {   if (Id>=0)
            return ModelMatrix * Planes[Id];
            return null;
        }
        /// <summary>
        /// is the constructor of the BoxSnappItem with <b>position</b> and <b>size</b>.
        /// </summary>
        /// <param name="Position">the position.</param>
        /// <param name="Size">the size.</param>
        public BoxSnappItem(xyz Position, xyz Size)
        {
            this.Position = Position;
            this.Size = Size;
       
            Planes.Clear();
            xyz[] P3 = new[]
           {
            new xyz(Position.x, Position.y, Position.z),
            new xyz(Position.x + Size.x, Position.y, Position.z),
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z),
            new xyz(Position.x, Position.y + Size.y, Position.z),
            new xyz(Position.x, Position.y, Position.z)
           };

            xyzArray A = new xyzArray();
            A.data = P3;
            Planes.Add(A);

            xyz[] P4 = new[]
          { 
            new xyz(Position.x, Position.y, Position.z),
            new xyz(Position.x , Position.y, Position.z+Size.z),
            new xyz(Position.x , Position.y + Size.y, Position.z+Size.z),
            new xyz(Position.x, Position.y + Size.y, Position.z),
            new xyz(Position.x, Position.y, Position.z)
           };


            A = new xyzArray();
            A.data = P4;
            Planes.Add(A);
            xyz[] P5 = new[]
          {
            new xyz(Position.x, Position.y, Position.z),
            new xyz(Position.x + Size.x, Position.y, Position.z),
            new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y, Position.z)
           };

            A = new xyzArray();
            A.data = P5;
            Planes.Add(A);
            xyz[] P1 = new[] {
         
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z),
            new xyz(Position.x+ Size.x, Position.y + Size.y, Position.z),
            new xyz(Position.x+ Size.x, Position.y,  Position.z),
             new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z),
             new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z)
            };
            A = new xyzArray();
            A.data = P1;
            Planes.Add(A);
            xyz[] P2 = new[]
            {
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y, Position.z + Size.Z),
            new xyz(Position.x + Size.x, Position.y, Position.z + Size.Z),
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z)
            };

            A = new xyzArray();
            A.data = P2;
            Planes.Add(A);
            xyz[] P6 = new[]
          {
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y + Size.y, Position.z + Size.Z),
            new xyz(Position.x, Position.y+ Size.y, Position.z ),
            new xyz(Position.x + Size.x, Position.y+ Size.y, Position.z ),
            new xyz(Position.x + Size.x, Position.y + Size.y, Position.z + Size.Z)
           };
            A = new xyzArray();
            A.data = P6;
            Planes.Add(A);
        }
        /// <summary>
        /// overrides <see cref="SurfaceSnappItem.draw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the box will be drawn.</param>
       public override void draw(OpenGlDevice Device)

        {
            Device.PushMatrix();
            Device.MulMatrix(ModelMatrix);
            PolygonMode Save = PolygonMode;
            Device.PolygonMode = PolygonMode;
            for (int i = 0; i < Planes.Count; i++)
            {
                Device.drawPolyLine(Planes[i]);
            }

            Device.PolygonMode = Save;
            Device.PopMatrix();
        }
        Loxyz Planes = new Loxyz();
        bool InsidePlane(xyz P, xyzArray Plane)
        {
            Triangle T1 = new Triangle(Plane[0], Plane[1], Plane[2]);
            Triangle T2 = new Triangle(Plane[0], Plane[2], Plane[3]);
            return ((T1.Inside(P)) || (T1.Inside(P)));

        }
        /// <summary>
        /// overrides <see cref="SurfaceSnappItem.Cross(LineType)"/>.
        /// </summary>
        /// <param name="ViewLine">line to the eye </param>
        /// <returns>the nearest point to the eye.</returns>
        protected internal override xyz Cross(LineType ViewLine)
        {
          ViewLine = Device.FromScr(Device.MousePos);
            if (this.PolygonMode == PolygonMode.Line)
            {
                double _Param = -1;
                double _LineLam = -1;

                double _di = 1e10;
              
                for (int i = 0; i < Planes.Count; i++)
                {
                    _di = Planes[i].Distance(ViewLine, 1e10, out _Param, out _LineLam);
                    if (_di <= 2 * Snappdist)
                    {
                        return Planes[i].Value(_Param);

                    }
                }

                return Standard();
            }
            Id = -1;
            double Dist = 1e10;
            double Lam = -1;
            xyz Pt = new xyz(0, 0, 0);
            xyz Pt1 = new xyz(0, 0, 0);
            xyz Pkt = new xyz(0, 0, 0);
            for (int i = 0; i < Planes.Count; i++)
            {
                Plane P = new Plane(Planes[i][0], Planes[i][1], Planes[i][2]);
                P.Cross(ViewLine, out Lam, out Pt1);

                Triangle T1 = new Triangle(Planes[i][0], Planes[i][1], Planes[i][2]);
                Triangle T2 = new Triangle(Planes[i][0], Planes[i][2], Planes[i][3]);
                if ((T1.Inside(Pt1)) || (T2.Inside(Pt1)))
                    if (Lam < Dist)
                    {
                        if (Lam<0)
                        { }
                        Pt = Pt1;
                        Id = i;
                        Dist = Lam;

                    }


            }

            double Param = -1;
            double LineLam = -1;
            double di = 1e10;
            if (Id >= 0)
            {
                di = Planes[Id].Distance(ViewLine, 1e10, out Param, out LineLam);
                if (di <= 2 * Snappdist)
                {
                    Pkt = Planes[Id].Value(Param);
                    return ModelMatrix* Pkt;
                }
            }
            else
                for (int i = 0; i < Planes.Count; i++)
                {
                    di = Planes[i].Distance(ViewLine, 1e10, out Param, out LineLam);
                    if (di <= 2 * Snappdist)
                    {
                        Pkt = Planes[i].Value(Param);
                        return ModelMatrix* Pkt;
                    }
                }
            if (Id >= 0)
                return ModelMatrix*Pt;
            else
                return Standard();
        }
     }
}