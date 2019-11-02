using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;
using System.Collections.Generic;

using LibTessDotNet;
//Copyright (C) 2016 Wolfgang Nagl
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// describes the join type used in <see cref="Loxy.GetOffset(JoinType, EndType, double)"/>
    /// </summary>
    public enum JoinType {
        /// <summary>
        /// the offset lines will be joint by a line.
        /// </summary>
        jtSquare,
        /// <summary>
        /// the offset lines will be joined by a part of circle.
        /// </summary>
        jtRound,
        /// <summary>
        /// the offset lines will be line enlargement of the parts.
        /// </summary>
        jtMiter
    };
    /// <summary>
    /// describes the end type used in <see cref="Loxy.GetOffset(JoinType, EndType, double)"/>
    /// </summary>
    public enum EndType
    {
        /// <summary>
        /// closes the outer open polyline
        /// </summary>
        etClosedPolygon,
        /// <summary>
        /// closes the inner and the outer open polyline
        /// </summary>
        etClosedLine,
        /// <summary>
        /// at open lines a normal will be geberated.
        /// </summary>
        etOpenButt,
        /// <summary>
        /// at open lines a square will be created with center the endpoints.
        /// </summary>
        etOpenSquare,
        /// <summary>
        /// at open part of a circle will be geberated.
        /// </summary>
        etOpenRound
    };
    /// <summary>
    /// Loxy is a <b>L</b>ist <b>o</b>f <b>xyz</b>Arrays. This is sometimes needed in geometry, when a polygon
    /// contains holes.
    /// </summary>

    [Serializable]
    public partial class Loxy : List<xyArray>, ITransform2d
    {
        /// <summary>
        /// Gets a parallel <see cref="Loxy"/> with offset
        /// </summary>
        /// <param name="JT">Jointype</param>
        /// <param name="ET">Endtype</param>
        /// <param name="Offset">Offset</param>
        /// <returns></returns>
        public Loxy GetOffset(JoinType JT, EndType ET, double Offset)
        {
            ClipperOffset co = new ClipperOffset();
            List<List<IntPoint>> Solution = new List<List<IntPoint>>();
            co.AddPaths(ToClipperPoly(this), (ClipperLib.JoinType)JT, (ClipperLib.EndType)ET);
            co.Execute(ref Solution, (long)(ToInt * Offset));
            return FromClipperLoxy(Solution);
        }
        private static double ToInt = 100;
     
        private IntPoint ToClipperPt(xy Point)
        {
            return new IntPoint(ToInt * Point.x, ToInt * Point.y);
        }
        private xy FromClipperPt(IntPoint Point)
        {
            return new xy((double)Point.X / ToInt, (double)Point.Y / ToInt);
        }
        private List<IntPoint> ToClipperPoly(xyArray Src)
        {
            List<IntPoint> Result = new List<IntPoint>();
            for (int i = 0; i < Src.Count; i++)
                Result.Add(ToClipperPt(Src[i]));
            return Result;
        }

        private xyArray FromClipperPoly(List<IntPoint> Src)
        {
            xyArray Result = new xyArray(Src.Count + 1);
            for (int i = 0; i < Src.Count; i++)
                Result[i] = FromClipperPt(Src[i]);
            Result[Src.Count] = Result[0];
            return Result;
        }
        private Loxy FromClipperLoxy(List<List<IntPoint>> Src)
        {
            Loxy Result = new Loxy();
            for (int i = 0; i < Src.Count; i++)
                Result.Add(FromClipperPoly(Src[i]));
            return Result;
        }




        private List<List<IntPoint>> ToClipperPoly(Loxy Src)
        {
            List<List<IntPoint>> Result = new List<List<IntPoint>>();
            for (int i = 0; i < Src.Count; i++)
                Result.Add(ToClipperPoly(Src[i]));
            return Result;
        }




        /// <summary>
        /// Set operations are applied <see cref="SetOperations"/>
        /// </summary>
        /// <param name="Operation">Setoperation</param>
        /// <param name="Clip">the other Loxy</param>
        /// <returns>the result of the setoperation</returns>
        public Loxy SetOperation(SetOperations Operation, Loxy Clip)
        {

            List<List<IntPoint>> Me = ToClipperPoly(this);
            List<List<IntPoint>> Second = ToClipperPoly(Clip);
            Clipper c = new Clipper();
            c.AddPaths(Me, PolyType.ptSubject, true);
            c.AddPaths(Second, PolyType.ptClip, true);
            List<List<IntPoint>> Solution = new List<List<IntPoint>>();
            c.Execute((ClipType)Operation, Solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return FromClipperLoxy(Solution);



        }

     

        private Hashtable TmpPointArray = new Hashtable();
        private List<xyf> _TmpPoints = new List<xyf>();
    
        /// <summary>
        /// Gets a triangulation, where the Indices are related to the Points
        /// </summary>
        /// <param name="Indices">Indices</param>
        /// <param name="Points">Array of float Points</param>
        public void TriAngulation(List<IndexType> Indices, ref xyf[] Points)
        {
            List<xyf> Pts = new List<xyf>();
            TriAngulation(Indices, Pts);
            Points = Pts.ToArray();

        }
        /// <summary>
        /// Gets a triangulation, where the Indices are related to the Points
        /// </summary>
        /// <param name="Indices">Indices</param>
        /// <param name="Points">List ofPoints</param>
        public void TriAngulation(List<IndexType> Indices, List<xyf> Points)
        {

            Tess Tess = new Tess();
            Tess.UsePooling = true;

            for (int i = 0; i < Count; i++)
            {
                ContourVertex[] CV = new ContourVertex[this[i].Count];
                for (int j = 0; j < this[i].Count; j++)
                {
                    CV[j] = new ContourVertex();

                    CV[j].Position.X = (float)this[i][j].X;
                    CV[j].Position.Y = (float)this[i][j].Y;
                }
                Tess.AddContour(CV);
            }
            Tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);
            Indices.Clear();
            for (int i = 0; i < Tess.Elements.Length; i++)
            {
                Indices.Add((IndexType)(Tess.Elements[i]));
            }

            for (int i = 0; i < Tess.Vertices.Length; i++)
            {
                Points.Add(new xyf(Tess.Vertices[i].Position.X, Tess.Vertices[i].Position.Y));
            }







        }
        /// <summary>
        /// shifted a <see cref="Loxy"/> to a <see cref="Loxyz"/> by settimg the z-value to zero.
        /// </summary>
        /// <returns></returns>
        public Loxyz ToLoxyz()
        {

            Loxyz Result = new Loxyz();
            for (int i = 0; i < Count; i++)
                Result.Add(this[i].ToxyzArray());

            return Result;
        }
        

        #region ITransform Member

        void ITransform2d.Transform(Matrix3x3 T)
        {
            for (int i = 0; i < Count; i++) this[i] = T * this[i];

        }

        #endregion

        ////  public ArrayList ListOfxyArrays = new ArrayList();
        //  /// <summary>
        //  /// The default indexer is overridden
        //  /// </summary>
        //  public xyArray this[int i]   // Indexer declaration
        //  {
        //      get { return (xyArray)base[i]; }
        //      set { base[i] = value; }
        //  }

        /// <summary>
        /// Adds an <see cref="xyArray"/> to the Loxy
        /// </summary>
        /// <returns>A xyArray</returns>
        public xyArray AddArray()
        {
            xyArray result = new xyArray(0);
            Add(result);
            return result;

        }

        ///// <summary>
        ///// Converts the Loxy to a <see cref="Loxyz"/> by setting the z-coordinates to 0.
        ///// </summary>
        ///// <returns></returns>
        //public Loxyz toLoxyz()
        //{
        //    return toLoxyz(0);
        //}
        short getPointId(xyf Point)
        {
            //_TmpPoints.Add(Point);
            //return (uint)(_TmpPoints.Count - 1);

            if (TmpPointArray.ContainsKey(Point))
            {
                object Ob = TmpPointArray[Point];

                return (short)Ob;
            }
            else
            {

                TmpPointArray.Add(Point, (uint)_TmpPoints.Count);
                _TmpPoints.Add(Point);
                return (short)(_TmpPoints.Count - 1);
            }
        }
        /*short getPointId(xyf Point)
        {
            //_TmpPoints.Add(Point);
            //return (uint)(_TmpPoints.Count - 1);

            if (TmpPointArray.ContainsKey(Point))
            {
                object Ob = TmpPointArray[Point];

                return (short)Ob;
            }
            else
            {

                TmpPointArray.Add(Point, (uint)_TmpPoints.Count);
                _TmpPoints.Add(Point);
                return (short)(_TmpPoints.Count - 1);
            }
        }
        */
        bool xCross(xy A, xy B, double x, ref xy Result)
        {
            double dx = B.x - A.x;
            if (System.Math.Abs(dx) < 0.0000000000000001)
            {
                if (System.Math.Abs(x - A.X) < 0.0001)
                {
                }
                return false;
            }
            double dy = B.y - A.y;
            double _x = (x - A.x);
            if (_x * dx < 0.0000000001) return false;
            if (_x / dx > 1) return false;
            Result = new xy(x, _x * dy / dx + A.y);
            return true;

        }
        bool yCross(xy A, xy B, double y, ref xy Result)
        {
            double dy = B.y - A.y;
            if (System.Math.Abs(dy) < 0.0000000000000001) return false;

            double dx = B.x - A.x;
            double _y = (y - A.y);
            if (_y * dy < 0.0000000001) return false;
            if (_y / dy > 1) return false;
            Result = new xy(_y * dx / dy + A.x, y);
            return true;

        }
        //public Loxyz toLoxyz(double z)
        //{
        //    Loxyz result = new Loxyz();
        //    result.Count = Count;

        //    for (int i = 0; i < Count; i++)
        //    {
        //        result[i] = this[i].ToxyzArray(z);
        //    }
        //    return result;
        //}
        /// <summary>
        /// The number of xyArrays, contained in the array.
        /// This property is also settable.
        /// 
        /// </summary>
        public new int Count
        {
            get
            {
                return base.Count;
            }

            set
            {

                while (value < Count) RemoveAt(Count - 1);
                while (value > Count) Insert(Count, new xyArray(0));



            }
        }

        /// <summary>
        /// Calculates the cross product over all xyArray by adding the cross products.
        /// If the value is positive, the orientation is counterclockwise.
        /// </summary>
        /// <returns>Return the cross product over all xyArrays contained in it</returns>
        public double cross()
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
                result = result + this[i].cross();
            return result;

        }
    }

}
