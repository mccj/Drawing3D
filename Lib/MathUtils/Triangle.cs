
using System;
using System.Collections.Generic;
using System.Drawing;


namespace Drawing3d
{ /// <summary>
  /// Interface, which holds three points
  /// </summary>
    public interface ITriangle
    {/// <summary>
     /// gets the Point A
     /// </summary>
     /// <returns>A</returns>
        xyzf GetA();
        /// <summary>
        /// sets the Point A
        /// </summary>
        void SetA(xyzf value);
        /// <summary>
        /// gets the Point B
        /// </summary>
        /// <returns>B</returns>
        xyzf GetB();
        /// <summary>
        /// sets the Point B
        /// </summary>
        void SetB(xyzf value);
        /// <summary>
        /// gets the Point C
        /// </summary>
        /// <returns>C</returns>
        xyzf GetC();
        /// <summary>
        /// sets the Point C
        /// </summary>
        void SetC(xyzf value);
    }
    /// <summary>
    /// This class represents a container for three points A, B, C
    /// </summary>
    [Serializable]
    public class Triangle
    : ITriangle, INeighbors
    {/// <summary>
     /// returns the contur of a list of triangles
     /// </summary>
     /// <param name="L">List of triangles</param>
     /// <returns></returns>
        public static Loxyz GetContour(List<Triangle> L)
        {

            Dictionary<xyz, xyz> D = new Dictionary<xyz, xyz>();
            for (int i = 0; i < L.Count; i++)
            {
                INeighbors N = L[i] as INeighbors;
                ITriangle T = L[i];
                if ((T.GetB() - T.GetA() & T.GetC() - T.GetA()).length() < 0.0000001) continue;
                if (N.GetNeighbor(0) == -1) D.Add(T.GetA().Toxyz(), T.GetB().Toxyz());
                if (N.GetNeighbor(1) == -1) D.Add(T.GetB().Toxyz(), T.GetC().Toxyz());
                if (N.GetNeighbor(2) == -1) D.Add(T.GetC().Toxyz(), T.GetA().Toxyz());

            }
            Loxyz Result = new Loxyz();
            while (true)
            {
                if (D.Count == 0) break;
                xyz First = new xyz(0, 0, 0);
                foreach (KeyValuePair<xyz, xyz> Pt in D)
                {
                    First = Pt.Key;
                    break;
                }
                xyz Current = First;
                xyzArray A = new xyzArray();
                Result.Add(A);
                while (true)
                {
                    if (D.ContainsKey(Current))
                    {
                        xyz C = D[Current];
                        A.Add(C);
                        D.Remove(Current);
                        Current = C;
                    }
                    else
                        break;
                }
            }

            return Result;
        }
        /// <summary>
        /// checks, if a point _P is inside the <see cref="Triangle"/>.
        /// </summary>
        /// <param name="Pkt">point, which will be checked.</param>
        /// <returns><b>true</b> if it is inside.</returns>
        public bool Inside(xyz Pkt)
        {

            xyz N = ((B - A) & (C - A)).normalized();
            return (((((A - Pkt) & (B - Pkt)) * N > -0.01)
                  && (((B - Pkt) & (C - Pkt)) * N > -0.01)
                && (((C - Pkt) & (A - Pkt)) * N > -0.01)
                )
                ||
                ((((A - Pkt) & (B - Pkt)) * N < 0.01)
                  && (((B - Pkt) & (C - Pkt)) * N < 0.01)
                && (((C - Pkt) & (A - Pkt)) * N < 0.01)
                ))




                ;
        }
        /// <summary>
        /// calculates the neighbors for each triangle.
        /// Rem: Triangle contans a <see cref="INeighbors"/>
        /// </summary>
        /// <param name="StlTriangles">List of triangles</param>
        public static void SetNeighbors(List<Triangle> StlTriangles)
        {


            int i = -1;
            Dictionary<EdgeCoordsf, Point> D = new Dictionary<EdgeCoordsf, Point>();
            foreach (ITriangle T in StlTriangles)
            {
                i++;
                if (((T.GetB() - T.GetA()) & (T.GetC() - T.GetA())).length() <= 0.00001)
                    continue;


                D.Add(new EdgeCoordsf(T.GetA(), T.GetB()), new Point(i, 0));
                D.Add(new EdgeCoordsf(T.GetB(), T.GetC()), new Point(i, 1));
                D.Add(new EdgeCoordsf(T.GetC(), T.GetA()), new Point(i, 2));

            }

            foreach (ITriangle T in StlTriangles)
            {
                {
                    (T as INeighbors).SetNeighbor(0, -1);
                    (T as INeighbors).SetNeighbor(1, -1);
                    (T as INeighbors).SetNeighbor(2, -1);
                    Point P = new Point(0, 0);
                    if (D.ContainsKey(new EdgeCoordsf(T.GetA(), T.GetC())))
                    {
                        P = D[new EdgeCoordsf(T.GetA(), T.GetC())];
                        (T as INeighbors).SetNeighbor(2, P.X);
                        (T as INeighbors).SetNeighborEdge(2, P.Y);
                    }
                    if (D.ContainsKey(new EdgeCoordsf(T.GetC(), T.GetB())))
                    {
                        P = D[new EdgeCoordsf(T.GetC(), T.GetB())];
                        (T as INeighbors).SetNeighbor(1, P.X);
                        (T as INeighbors).SetNeighborEdge(1, P.Y);
                    }
                    if (D.ContainsKey(new EdgeCoordsf(T.GetB(), T.GetA())))
                    {
                        P = D[new EdgeCoordsf(T.GetB(), T.GetA())];
                        (T as INeighbors).SetNeighbor(0, P.X);
                        (T as INeighbors).SetNeighborEdge(0, P.Y);
                    }

                }
            }

        }
       
        MeshEdge[] Edges = new MeshEdge[3] { new MeshEdge(-1, 0), new MeshEdge(-1, 0), new MeshEdge(-1, 0) };

        /// <summary>
        /// Point of the Triangle
        /// </summary>
        public xyz A;
        /// <summary>
        /// Point of the Triangle
        /// </summary>

        public xyz B;
        /// <summary>
        ///  Point of the Triangle
        /// </summary>

        public xyz C;

        /// <summary>
        /// The constructor initializes the class with the values A, B, C
        /// </summary>
        /// <param name="A">1. point</param>
        /// <param name="B">2. point</param>
        /// <param name="C">3. point</param>
        public Triangle(xyz A, xyz B, xyz C)
        {
            this.A = A;
            this.B = B;
            this.C = C;

        }
        /// <summary>
        /// Calculates a normal vector to the triangle and returns it.
        /// </summary>
        /// <returns>returns a normalvector to the triangle </returns>

        public xyz Normal() { return ((B - A) & (C - B)); }
        /// <summary>
        /// Clockwise checks orientation from <b>this</b> respecting to direction.
        /// Revises the orientation of the points A, B, C clockwise, relativly to their direction.
        /// If we head from A to B and then to C, we can apply the method of the screwdriver. If this
        /// direction points at the same side as the param direction, we obtain true as a result.
        /// </summary>
        /// <param name="direction">The direction for the check of clockwise</param>
        /// <returns>Returns true, if the three points are clockwise oriented</returns>
        /// <example>
        /// 	Triangle t = new Triangle(new xyz(2,0,0), new xyz(0,2,0), new xyz(-2,0,0));
        ///		if (t.ClockWise(new xyz(0,0,1)))
        ///		MessageBox.Show("ClockWise");
        /// </example>
        /// <remarks>
        /// Unfortunately we obtain the normal clockwise in 2D, if we take <b>Direction=( 0 , 0, -1)</b> as
        /// reference
        /// </remarks>

        public bool ClockWise(xyz direction)
        {
            return Utils.Less(Utils.Spat(B - A, C - A, direction), 0);
        }
        /// <summary>
        /// Creates a Planeobject, which contains the three Points A, B, C
        /// </summary>
        /// <returns></returns>
        public Plane GetPlane()
        {
            return new Plane(A, B, C);
        }
        /// <summary>
        /// This method checks, whether a LineType L intersects the triangle or not. The Line is regarded as a
        /// bounded Line with Endpoints A and B. The cross point - if exists - has to lie between A and B.
        /// 	
        /// </summary>
        /// <param name="L">A line to check</param>
        /// <returns>Returns true, if the Line L is disjoint from the Triangle and false if the Line L 
        /// intersects the Triangle
        /// </returns>
        public bool disjoint(LineType L)
        {
            xyz N = Normal();
            xyz pt;
            double lam;
            if (!Utils.Equals((N * L.Direction), 0))
            {
                if (Cross(L, out lam, out pt))
                {
                    if (Utils.Less(0, lam) && Utils.Less(lam, 1)) return false;
                    else return true;
                }
                return true;

            }
            xyz PA = L.P - A;
            if (!Utils.Equals((N * PA), 0)) return true;

            if (this.Intersect(new LineType(L.P, N))) return false;
            if (L.CrossBounded(new LineType(A, B - A))) return false;
            if (L.CrossBounded(new LineType(B, C - B))) return false;
            return true;
        }
        /// <summary>
        /// "Cross" calculates more then the Method <see cref="disjoint"/>. In case of crossing, 
        /// a value lam and the cross point is returned. This value ist between 0 and 1.
        /// The line is regarded as a bounded line.
        /// For the use of lam watch <see cref="Plane"/>
        /// 
        /// </summary>
        /// <param name="L">A bounded line which will be checked</param>
        /// <param name="lam">Param which defines the cross point in the sense pt = L.P + L,Direction*lam</param>
        /// <param name="pt">Crosspoint</param>
        /// <returns>returns true, if the Line crosses the Triangle, otherwise it returns false</returns>
        public bool Cross(LineType L, out double lam, out xyz pt)
        {
            lam = -1;
            pt = new xyz(-1, -1, -1);
            if (Intersect(L))
            {
                this.GetPlane().Cross(L, out lam, out pt);
                return true;
            }
            return false;

        }
        /// <summary>
        /// Checks intersecting of a not bounded Line with the Triangle
        /// </summary>
        /// <param name="L">Not bounded Line</param>
        /// <returns></returns>
        public bool Intersect(LineType L)
        { double Lam = -1;
           
            xyz Pkt = new Drawing3d.xyz(0, 0, 0);
            Plane P = new Plane(A, B, C);
            P.Cross(L, out Lam,  out Pkt);
            xyz Dummy = new xyz(0, 0, 0);
          
            xyz SA = A.sub(Pkt);
            xyz SB = B.sub(Pkt);
            xyz SC = C.sub(Pkt);
            LineType LL = new LineType(A, (B - A).normalized());
          double bb=  LL.Distance(Pkt, out Lam, out Dummy);
            if (bb <0.1)
            { }
            LL = new LineType(B, (C - B).normalized());
            bb = LL.Distance(Pkt, out Lam, out Dummy);
            if (bb < 0.1)
            { }
            LL = new LineType(C, (C - A).normalized());
            bb = LL.Distance(Pkt, out Lam, out Dummy);
            if (bb < 0.1)
            { }
            //double D1 = Utils.Spat(SA, SB, L.Direction);
            //double D2 = Utils.Spat(SB, SC, L.Direction);
            //double D3 = Utils.Spat(SC, SA, L.Direction);
            double D1 = Utils.Spat(SA, SB, SC);

            return true;
            //if (Utils.Less(0, D1))
            //{
            //    return (Utils.Less(0, D2) && Utils.Less(0, D3));
            //}
            //else
            //    return (Utils.Less(D2, 0)) && (Utils.Less(D3, 0));
        }
        /// <summary>
        /// Calculates the affine coordinates of a cross point of a infinitly line.
        /// To calculate the cross point use: pt = A + (B - A) * Lam + (C - A) * mue
        /// </summary>
        /// <param name="L">Infinitly line</param>
        /// <param name="Lam">affine coordinate relatively to (B - A)</param>
        /// <param name="Mue">affine coordinate relatively to (C - A)</param>
        /// <returns></returns>
        public bool AffinCoordOfCross(LineType L, ref double Lam, ref double Mue)
        {
            Lam = 0;
            Mue = 0;
            Plane plane = this.GetPlane();
            double dummy, a, b, c, d;
            xyz P, AB, AC, AP;
            plane.Cross(L, out dummy, out P);

            AB = B - A;
            AC = C - A;
            AP = P - A;
            a = AB.x; b = AC.x; c = AB.y; d = AC.y;

            xyz Det = AB & AC;
            xyz Det1 = AP & AB;
            xyz Det2 = AP & AC;
            if ((Det * plane.NormalUnit) < 0.0000000001) return false;
            Lam = (Det2 * plane.NormalUnit / (Det * plane.NormalUnit));
            Mue = -(Det1 * plane.NormalUnit / (Det * plane.NormalUnit));
            xyz T = AB * Lam + AC * Mue;
            if (AP.dist(T) > 0.0001)
            {
            }
            return true;



        }
        /// <summary>
        /// returns the neighbor of Id (Id 0 edge AB,Id 1 edge BC,Id 2 edge CA)
        /// </summary>
        /// <param name="Id">Id 0 edge AB,Id 1 edge BC,Id 2 edge CA</param>
        /// <returns></returns>
        public int GetNeighbor(int Id)
        {
            return Edges[Id].Neighbor;
        }
        /// <summary>
        /// Set the neighbor
        /// </summary>
        /// <param name="Id">Id 0 edge AB,Id 1 edge BC,Id 2 edge CA</param>
        /// <param name="value">Neighbor</param>
        public void SetNeighbor(int Id, int value)
        {
            Edges[Id].Neighbor = value;
        }
        /// <summary>
        /// gets the Neighboredge 
        /// </summary>
        /// <param name="Id">Id 0 edge AB,Id 1 edge BC,Id 2 edge CA</param>
        /// <returns>Neighboredge</returns>
        public int GetNeighborEdge(int Id)
        {
            return Edges[Id].Edge;
        }
        /// <summary>
        /// Set the neighboredge
        /// </summary>
        /// <param name="Id">Id 0 edge AB,Id 1 edge BC,Id 2 edge CA</param>
        /// <param name="value">Neighboredge</param>
        public void SetNeighborEdge(int Id, int value)
        {
            Edges[Id].Edge = value;
        }

        xyzf ITriangle.GetA()
        {
            return A.toXYZF();
        }

        void ITriangle.SetA(xyzf value)
        {
            A = value.Toxyz();
        }

        xyzf ITriangle.GetB()
        {
            return B.toXYZF();
        }

        void ITriangle.SetB(xyzf value)
        {
            B = value.Toxyz();
        }

        xyzf ITriangle.GetC()
        {
            return C.toXYZF();
        }

        void ITriangle.SetC(xyzf value)
        {
            C = value.Toxyz();
        }
    }
}
