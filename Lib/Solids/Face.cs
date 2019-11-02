using System;
using System.Collections.Generic;
using System.Drawing;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// is used from <see cref="Face.Bounds"/>.
    /// </summary>
    [Serializable]
    public class Bounds : List<EdgeLoop>
    {
        /// <summary>
        /// copy the <see cref="Bounds"/> to the target <see cref="Solid"/>.
        /// </summary>
        /// <param name="TargetSolid">the target <see cref="Solid"/>.</param>
        /// <returns></returns>
        public virtual Bounds Copy(Solid TargetSolid)
        {
            Bounds result = new Bounds();
            for (int i = 0; i < Count; i++)
            {
                result.Add(this[i].Copy(TargetSolid));
            }
            return result;
        }
   }

    /// <summary>
    /// is a part of a <see cref="Solid"/>. A <see cref="Face"/> is bounded by <see cref="Face.Bounds"/>, which consists of <see cref="Edge"/>s.
    /// Bounds[0] is the outer contur. The edges are clockwise. Contains a <see cref="Face"/> Holes, so are the bounds of these counter clockwise.
    /// The face will be drawn by the containing <see cref="Face.Surface"/>
    /// </summary>
    [Serializable]
    public class Face
    {
        /// <summary>
        /// is the owner of the <see cref="Face"/>.
        /// </summary>
        [NonSerialized]
        public Solid Parent;
      
        internal bool DrawRelativToSurfaceBase = true;
        void RefreshNormals()
        {

            Normals = new Loxyz();
            Normals.Count = Bounds.Count;
            DrawPoints = new Loxyz();
            DrawPoints.Count = ParamCurves.Count;
            if (!DrawRelativToSurfaceBase)
            {
                for (int i = 0; i < Bounds.Count; i++)
                {
                    EdgeLoop EL = Bounds[i];
                    for (int k = 0; k < EL.Count; k++)
                    {
                        Edge E = EL[k];
                        DrawPoints[i].Add(E.EdgeStart.Value);
                    }
                }

            }

            for (int i = 0; i < ParamCurves.Count; i++)
            {

                xyArray A = ParamCurves[i].getxyArrayClosed(false);
                if (DrawRelativToSurfaceBase)
                    DrawPoints[i] = A.ToxyzArray();

                int ct = 0;
                for (int t = 0; t < ParamCurves[i].Count; t++)
                    ct = ct + ParamCurves[i][t].Resolution;
                xyzArray NormalsLoop = new xyzArray(ct);
                Normals[i] = NormalsLoop;
                int id = 0;

                for (int j = 0; j < ParamCurves[i].Count; j++)
                {

                    List<Face> L = GetFaces(this, i, j);
                    Edge E = Bounds[i][j];

                    if (L == null)
                    {

                        Normals = null;

                        DrawPoints = null;
                        return;
                    }
                    double SmoothAngle = Parent.SmoothAngle;
                    xyz N = Surface.Normal(0, 0).normalized();
                    xyz N1 = Surface.Normal(0, 0).normalized();
                    for (int k = 0; k < L.Count; k++)
                    {

                        if (System.Math.Abs((N1 * L[k].Surface.Normal(0, 0).normalized())) > System.Math.Cos(SmoothAngle))
                            N = N + L[k].Surface.Normal(0, 0).normalized();
                   }
                    N = N.normalized();
                    if (DrawRelativToSurfaceBase)
                    {
                        Matrix M = Surface.Base.ToMatrix().invert();
                        N = M * N - M * new xyz(0, 0, 0);
                    }

                    NormalsLoop[id] = N;
                    id++;

                }
             
            }
        }
        /// <summary>
        /// gets neighbor faces of the <b>Face</b>, which has the common the <see cref="Face.Bounds"/>[Bound][Edge] <see cref="Edge"/>
        /// </summary>
        /// <param name="Face">for which the neighbors wil be returned./></param>
        /// <param name="Bound">index in <see cref="Face.Bounds"/></param>
        /// <param name="Edge">index in Face.Bounds[Bound] </param>
        /// <returns>the neighbor faces</returns>
        public static List<Face> GetFaces(Face Face, int Bound, int Edge)
        {
            int DualBound = -1;
            Face DualFace = null;
            Face ThisFace = Face;
            List<Face> Result = new List<Face>();
            double DEdge = Edge - 0.5;
            if (DEdge < 0)
                DEdge = Face.Bounds[Bound].Count - 0.5;
            int CT = 0;
            while (true)
            {
                DEdge = GetDualEdge(Face, Bound, DEdge, ref DualBound, ref DualFace);
                if (DualFace == ThisFace)
                    return Result;
                if (DualFace == null)
                    return null;

                CT++;
                if (CT > 1000) // Error
                    return null;
                DEdge = (int)DEdge;
                DEdge--;
                if (DEdge < 0)
                    DEdge = DualFace.Bounds[DualBound].Count - 0.5;
                Result.Add(DualFace);
                Bound = DualBound;
                Face = DualFace;
            }
        }
       
        static Edge GetDualEdge(Face Face, int Bound, int Edge, ref int OutLoop, ref Face Neighbor)
        {
            double d = GetDualEdge(Face, Bound, Edge + 0.5, ref OutLoop, ref Neighbor);

            if (d < 0) return null;
            int id = (int)d;
            return Neighbor.Bounds[OutLoop][id];

        }
        /// <summary>
        /// gets the dual <see cref="Edge"/> in a <see cref="Face"/> which is given by <see cref="Face.Bounds"/>[Bound][(int)Param] In <b>OutBound</b> is the index of the dual <see cref="Edge"/> relativ to <b>Neighbor.Bounds</b> 
        /// and the result value gives the parameter for the <see cref="Edge.ParamCurve"/> of this <see cref="Edge"/>. <b>Neighbor</b> is the neighbor <see cref="Face"/>.
        /// If no dual edge is found -1 will be returned.
        /// </summary>
        /// <param name="Face"><see cref="Face"/> in which <b>Bound</b> and <b>Param</b> are taken.</param>
        /// <param name="Bound">gives together with <b>Param</b> the <see cref="Edge"/> by <see cref="Face.Bounds"/>[Bound][(int)Param].</param>
        /// <param name="Param">gives together with <b>Bound</b> the <see cref="Edge"/> by <see cref="Face.Bounds"/>[Bound][(int)Param].</param>
        /// <param name="OutBound">is the bound index of the dual edge.</param>
        /// <param name="Neighbor">is the neighbor of <b>Face</b>.</param>
        /// <returns>a parameter, which gives togethe with <b>Outbound</b> the same point in the <b>dualedge</b> with dualedge.Paramcurve.value(parameter-(int)parameter.</returns>
        public static double GetDualEdge(Face Face, int Bound, double Param, ref int OutBound, ref Face Neighbor)
        {   
            Edge E = Face.Bounds[Bound][(int)Param];
          
            Vertex3d A = E.EdgeStart;
            Vertex3d B = E.EdgeEnd;
            Neighbor = Face.Neighbor(Bound, (int)Param) as Face;
            if (Neighbor == null)
                return -1;
            OutBound = -1;
            for (int i1 = 0; i1 < Neighbor.Bounds.Count; i1++)
            {
                OutBound = i1;

                for (int i = 0; i < Neighbor.Bounds[i1].Count; i++)
                {
                    if ((Neighbor.Bounds[i1][i].EdgeStart == B) &&
                        (Neighbor.Bounds[i1][i].EdgeEnd == A))
                    {
                        if (Param != (int)Param)
                            return i + (1 - (Param - (int)Param));

                        return i;

                    }
                    if ((Neighbor.Bounds[i1][i].EdgeStart == A) &&
                        (Neighbor.Bounds[i1][i].EdgeEnd == B))
                    {
                        if (Param != (int)Param)
                            return i + (1 - (Param - (int)Param));
                        return i;

                    }
                }

            }

            return -1;

        }


        /// <summary>
        /// a virtual method to refreh the <see cref="Face"/>. The method <see cref="Surface.To2dCurve"/> will be called for the <see cref="Edge.EdgeCurve"/>. It mus be implemented.
        /// If the <see cref="Face.Surface"/> is a <see cref="PlaneSurface"/> is this implemented.
        /// She is called from <see cref="Solid.Refresh"/>.
        /// </summary>
        public virtual void Refresh()
        {
            return;
        }
        [NonSerialized]
         Loxyz DrawPoints = null;
        
        /// <summary>
        /// refreshes the <see cref="Edge.ParamCurve"/> of the <see cref="EdgeList"/>. 
        /// <see cref="ParamCurves"/>-liste. Diese doppelte Verwaltung der ParamCurves wird lediglich aus Performancegründen gemacht.
        /// </summary>
        public virtual void RefreshParamCurves()
        {
            Solid P = Parent;
            DrawPoints = new Loxyz();
            DrawPoints.Count = Bounds.Count;

            if (ParamCurves == null)
                Surface.BoundedCurves = new Loca();
            ParamCurves.Clear();
            for (int i = 0; i < Bounds.Count; i++)
            {

                EdgeLoop EL = Bounds[i];
                CurveArray CA = new CurveArray();
                ParamCurves.Add(CA);
                for (int j = 0; j < EL.Count; j++)
                {
                    Edge E = EL[j];
                    if (!DrawRelativToSurfaceBase)
                    {
                        DrawPoints[i].Add(E.EdgeStart.Value);
                    }
                    Curve c = Surface.To2dCurve(EL[j].EdgeCurve);
                    if (!EL[j].SameSense) c.Invert();
                    EL[j].ParamCurve = c;

                    CA.Add(c);
                }
                xyArray A = CA.getxyArrayClosed(false);
           
                    DrawPoints[i] = A.ToxyzArray();
            }
        }
        [NonSerialized]

        private Loxyz Normals = new Loxyz();

        /// <summary>
        /// gets the neighbor which is has the <see cref="Edge"/> Bounds[EdgeLoop][Edge] common.
        /// </summary>
        /// <param name="EdgeLoop">is the first parameter from Bounds[EdgeLoop][Edge].</param>
        /// <param name="Edge">is the second parameter from Bounds[EdgeLoop][Edge].</param>
        /// <returns>a neighbor <b>Face</b></returns>
        public Face Neighbor(int EdgeLoop, int Edge)
        {
           EdgeLoop EL = Bounds[EdgeLoop];
            if (EL.Count <= Edge) return null;

            if (EL[Edge].EdgeCurve.Neighbors[0] == this)
            {
       
                return EL[Edge].EdgeCurve.Neighbors[1] as Face;
            }

            else
            {
                if (EL[Edge].EdgeCurve.Neighbors[1] == this)
                {
 
                    return EL[Edge].EdgeCurve.Neighbors[0] as Face;
                }
            }
            return null;
        }

      
        /// <summary>
        /// copies the <b>Face</b> to the <b>TargetSolid</b>.
        /// </summary>
        /// <param name="TargetSolid">is the target solid, in which the <see cref="Face"/> will be copied.
        /// If <b>TargetSolid</b> is null a simply copy of the face will be returned.</param>
        /// <returns>a copy of <b>Face</b>.</returns>
        public virtual Face Copy(Solid TargetSolid)
        {
            Face Result = Activator.CreateInstance(this.GetType()) as Face;
            Tag = Result;
            Result.Bounds = this.Bounds.Copy(TargetSolid);
            Result._ParamCurves = this.ParamCurves.Copy();
            Result.Surface = this.Surface.Copy();
            for (int i = 0; i < Result.Bounds.Count; i++)
            {
                EdgeLoop EL = Result.Bounds[i];
                for (int j = 0; j < EL.Count; j++)
                {
                    Edge E = EL[j];
                    Curve3D C = E.EdgeCurve;
                    // if (Result is Face)
                    {
                        if (C.Neighbors == null)
                        {
                            C.Neighbors = new Face[2];
                            C.Neighbors[0] = Result as Face;
                        }
                        else
                            C.Neighbors[1] = Result as Face;
                    }

                }

            }

            if (TargetSolid != null)
                TargetSolid.FaceList.Add(Result);
            Result.RefreshParamCurves();
            return Result;
        }
        /// <summary>
        /// calls the method <see cref="Copy(Solid)"/> with <b>TargetSolid</b> = null.
        /// </summary>
        /// <returns>a copy of <b>Face</b>.</returns>
        public virtual Face Copy()
        {
            return Copy(null);
        }


        /// <summary>
        /// is a constructor of a <see cref="Face"/> in a <see cref="Solid"/>.
        /// </summary>
        /// <param name="Solid">a <see cref="Solid"/>, to which <b>FaceList</b> the <b>Face</b> will be added.</param>
        public Face(Solid Solid)
        {
            Solid.FaceList.Add(this);
        }
        /// <summary>
        /// empty constructor.
        /// </summary>
        public Face()
        {

        }
       
        [NonSerialized]
        object _Tag = null;
        /// <summary>
        /// this field is free for use.
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set
            {
                _Tag = value;
            }
        }

        /// <summary>
        /// creates a <see cref="Face"/> for a <see cref="Solid"/> with <see cref="Model.Solid"/>.
        /// <b>Curves</b>
        /// 
        /// 
        /// 
        /// 
        /// Erstell ein massives <see cref="Face"/>. Wenn <b>Solid</b> nicht null ist, wird dieses Face eingebaut in
        /// <b>Solid</b>. <b>Curves</b> spezifizieren einee Liste von Curve3d-Lists. Sie werden als <see cref="Edge.EdgeCurve"/>
        /// in jede <see cref="Edge"/> gesetzt. <b>Curves</b> müssen von der Struktur identisch sein wie Bounds,
        /// d.h. die selbe Länge und jeder Eintrag in Bounds ( Bounds[i] ) muss die selbe Länge haben wie
        /// der entsprechende Eintrag in <b>Curves</b> (Curves[i]).
        /// gesetzt 
        /// </summary>
        /// <param name="Solid"><see cref="Solid"/> in which the result<see cref="Face"/> will be integrated.</param>
        /// <param name="Surface"><see cref="Surface"/> belonging to the new <see cref="Face"/>.</param>
        /// <param name="Bounds">list of <see cref="Vertex3dArray"/>, who contains the start and end points of the <see cref="Edge"/>s.</param>
        /// <param name="Curves"><see cref="Curve3D"/>, which are the new <see cref="Edge.EdgeCurve"/>s. Their start points and their end point must correspond to
        /// the parameter Bounds start and end points.</param>
        /// <returns>a <see cref="Face"/>, which will be added to the <b>Solid</b>.</returns>
        public static Face SolidFace(Solid Solid, Surface Surface, Vertex3dArray_2 Bounds, Loca3D Curves)
        {
            if (Bounds.Count == 0) return null;

            Face Result = new Face();
            Result.Surface = Surface;
            for (int i = 0; i < Bounds.Count; i++)
            {
                EdgeLoop Edgeloop = new EdgeLoop();
                Result.Bounds.Add(Edgeloop);
                for (int j = 0; j < Bounds[i].Count; j++)
                {
                    Vertex3d A = Bounds[i][j];
                    Vertex3d B = null;
                    if (j == Bounds[i].Count - 1) B = Bounds[i][0];
                    else
                        B = Bounds[i][j + 1];

                    Edge E = Edge.SolidEdge(Solid, Result, A, B, Curves[j][i]);
              
                    Edgeloop.Add(E);
                }
            }
            return Result;
       }
        /// <summary>
        /// creates a <see cref="Face"/> for a <see cref="Solid"/> in the <see cref="Model.Solid"/>.
        /// The Curves are all <see cref="Line3D"/>. The <see cref="Face"/> is plane and has as <see cref="Face.Surface"/> a <see cref="PlaneSurface"/>.
        /// </summary>
        /// <param name="Solid">is the target in which the <see cref="Face"/> will be posed.</param>
        /// <param name="Bounds">contains the <see cref="Vertex3d"/> for the <see cref="Line3D"/>.</param>
        /// <returns>a <see cref="Face"/></returns>
        public static Face SolidPlane(Solid Solid, Vertex3dArray_2 Bounds)
        {
            if (Bounds.Count == 0) return null;
            xyz N = new xyz(0, 0, 0);
            xyz P = Bounds[0][0].Value;
            for (int i = 1; i < Bounds[0].Count - 1; i++)
            {
                xyz A = Bounds[0][i].Value;
                xyz B = Bounds[0][i + 1].Value;
                xyz M = N;
                N = N + ((A - P) & (B - P));

            }
            N = N.normalized()*(-1);
            Base Base = Base.UnitBase;
            Base.BaseO = P;
            Base.BaseZ = N;
            if ((Base.BaseZ & new xyz(1, 0, 0)).dist(xyz.Null) > 0.01)
            {
                Base.BaseY = (Base.BaseZ & (new xyz(1, 0, 0))).normalized();
                Base.BaseX = Base.BaseY & Base.BaseZ;
            }
            else
            {
                Base.BaseY = (Base.BaseZ & (new xyz(0, 1, 0))).normalized();
                Base.BaseX = Base.BaseY & Base.BaseZ;
            }

            PlaneSurface Surface = new PlaneSurface();
            Surface.Base = Base;
            //-------------------------------------
            //-------- Create the Face
            Face Result = new Face();
            // ---- With Plane Surface
            Result.Surface = Surface;
            if (Solid != null)
            {
                Solid.FaceList.Add(Result);
            }
            //----- Set the Edges
            for (int i = 0; i < Bounds.Count; i++)
            {
                EdgeLoop Edgeloop = new EdgeLoop();
                Result.Bounds.Add(Edgeloop);
                for (int j = 0; j < Bounds[i].Count; j++)
                {
                    Vertex3d A = Bounds[i][j];
                    Vertex3d B = null;
                    if (j == Bounds[i].Count - 1) B = Bounds[i][0];
                    else
                        B = Bounds[i][j + 1];
                    Edge E = Edge.SolidEdge(Solid, Result, A, B, new Line3D(A.Value, B.Value));

                    Edgeloop.Add(E);
                }
            }

            Result.RefreshParamCurves();
            return Result;

        }
        /// <summary>
        /// creates a <see cref="Face"/> in the <see cref="Model.Shell"/> with the points <b>Pts</b>.
        /// </summary>
        /// <param name="Pts"><see cref="xyzArray"/> which gives the points of the plane <see cref="Face"/>.</param>
        /// <returns>plane Face.</returns>
        public static Face ShellPlane(xyzArray Pts)
        {
            Loxyz L = new Loxyz();
            L.Count = 1;
            L[0] = Pts;
            return ShellPlane(L);
        }
        /// <summary>
        /// creates a <see cref="Face"/> in the <see cref="Model.Shell"/> with the points <b>Pts</b>.
        /// </summary>
        /// <param name="Pts"><see cref="Loxyz"/> which gives the points of the plane <see cref="Face"/>.</param>
        /// <returns>plane Face.</returns>
        public static Face ShellPlane(Loxyz Pts)
        {
            if (Pts.Count == 0) return null;
            Plane _Plane = Pts[0].GetPlane();
            Base Base = Base.UnitBase;
            Base.BaseO = _Plane.P;
            Base.BaseZ = _Plane.NormalUnit.normalized() * (-1);
            if ((Base.BaseZ & new xyz(1, 0, 0)).dist(xyz.Null) > 0.01)
            {
                Base.BaseY = (Base.BaseZ & (new xyz(1, 0, 0))).normalized();
                Base.BaseX = Base.BaseY & Base.BaseZ;
            }
            else
            {
                Base.BaseY = (Base.BaseZ & (new xyz(0, 1, 0))).normalized();
                Base.BaseX = Base.BaseY & Base.BaseZ;
            }

            PlaneSurface Surface = new PlaneSurface();
            Surface.Base = Base;

            return ShellFace(Pts, Surface);
        }
        /// <summary>
        /// creates a <see cref="Face"/> in the <see cref="Model.Shell"/> with the points <b>Pts</b>.
        /// </summary>
        /// <param name="Pts"><see cref="Loxyz"/> which gives the points of the plane <see cref="Face"/>.</param>
        /// <param name="Surface"><see cref="Loxyz"/> which gives the points of the plane <see cref="Face"/>.</param>
        /// <returns>plane Face.</returns>
        static Face ShellFace(Loxyz Pts, Surface Surface)
        {
            if (Pts.Count == 0) return null;

            Face Result = new Face();
            for (int i = 0; i < Pts.Count; i++)
            {
                EdgeLoop Edgeloop = new EdgeLoop();
                Result.Bounds.Add(Edgeloop);
                for (int j = 0; j < Pts[i].Count; j++)
                {
                    xyz A = Pts[i][j];
                    xyz B = xyz.Null;
                    if (j == Pts[i].Count - 1) B = Pts[i][0];
                    else
                        B = Pts[i][j + 1];
                    Edge E = Edge.ShellLineEdge(A, B);
                    E.SameSense = true;
                    Edgeloop.Add(E);
                }
            }
            Result.Surface = Surface;
        
            return Result;
        }

        private Surface _Surface;
        /// <summary>
        /// is the geometry of a <b>Face</b>. The surface will be bounded by <see cref="Surface.BoundedCurves"/>, which corresponds to <see cref="Face.Bounds"/>.
        /// </summary>
        public Surface Surface
        {
            get { return _Surface; }
            set
            {
                _Surface = value;
            }
        }
       Bounds _Bounds = new Bounds();

        /// <summary>
        /// is the border of a <see cref="Face"/>. Its a list of <see cref="EdgeLoop"/>s. <see cref="Bounds"/>[0] is the outer contour of the <see cref="Face"/>.
        /// She is in the clockwise sense. Holes are counterclockwise.
        /// </summary>
        public Bounds Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; }
        }


        [NonSerialized]
        Loca _ParamCurves = new Loca();
        /// <summary>
        /// are the <see cref="Surface.BoundedCurves"/>.
        /// </summary>
        public Loca ParamCurves
        {
            get
            {
                if (Surface != null) return Surface.BoundedCurves;
                else
                    return null;
           }
        }
   }
}