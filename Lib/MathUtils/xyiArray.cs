using System.Drawing;
using System;
//Copyright (C) 2016 Wolfgang Nagl

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
    /// This class is a container, which holds <see cref="System.Drawing.Point"/> .
    /// <see cref="Inside"/> is an agreeable method, whith which you can check whether a point is inside a polygon or not.
    /// The property <see cref="Count"/> is read- and writeable. That makes the handling easier.
    /// With <see cref="Maxrect"/> you get a rectangle enveloping the polygon.
    /// </summary>
    [Serializable]
    public class xyiArray
    {
        /// <summary>
        /// The constructor initializes the class and sets the Count to ACount
        /// </summary>
        /// <param name="Acount">Initialvalue for <see cref="Count"/></param>

        public xyiArray(int Acount)
        {
            Count = Acount;
        }
        /// <summary>
        /// Calculates an envelopping rectangle for a xyiArray.
        /// 
        /// </summary>
        /// <param name="rect">intialvalue</param>
        /// 
        /// <returns>an envelopping rectangle</returns>
        /// <example>
        /// <code>
        ///
        /// 
        /// xyiArray A = new xyiArray(4);
        /// A[0] = new xyi(-4, 2);
        /// A[1] = new xyi(24, 12);
        /// A[2] = new xyi(4, -3);
        /// A[3] = new xyi(5, 11);
        /// 
        /// Rectangle r = Maxrect(Rectangle.Reset());
        /// // now 
        /// //r.Left = -4;
        /// //r.Top  = -3;
        /// //r.Right = 24;
        /// //r.Down  = 12;
        ///  </code>
        ///</example>
        public Rectangle Maxrect(Rectangle rect)
        {
            for (int i = 0; i < Count; i++)
                rect = Utils.MaxRect(this[i], rect);
            return rect;
        }
        /// <summary>
        /// Gets the cross product of the array;
        /// </summary>
        /// <returns></returns>
        public double Cross()
        {

            double result = 0;
            for (int i = 0; i < Count - 1; i++)
            {


                result = result + PointArray[i].X * PointArray[i + 1].Y - PointArray[i].Y * PointArray[i + 1].X;
            }


            return result;

        }
        /// <summary>
        /// Array, which contains the points
        /// </summary>
        public Point[] PointArray;
        /// <summary>
        /// default indexer which returns the i-th element from Point[]
        /// </summary>
        public Point this[int i]   // Indexer declaration
        {
            get
            {
                return PointArray[i];

            }
            set { PointArray[i] = value; }
        }
        /// <summary>
        /// Indicates how many members are contained. The property is read- and writeable.
        /// </summary>
        public int Count
        {
            get
            {
                if (PointArray != null) return PointArray.Length;
                else return 0;
            }
            set
            {
                Point[] New = new Point[value];
                if (Count > 0) PointArray.CopyTo(New, 0);
                PointArray = New;
            }

        }
        /// <summary>
        /// Checks the position of a point P, whether it lays inside the polygon or not.
        /// </summary>
        /// <param name="P">Point that will be checked</param>
        /// <returns>If P lays inside the polygon the result is true, otherwise it is false</returns>
        public bool Inside(Point P)
        {

            int i, j;
            Point p1_i, p1_j;
            bool result = false;
            j = Count - 1;
            for (i = 0; i < Count; i++)
            {
                p1_i = this[i];
                p1_j = this[j];

                if ((((p1_i.Y <= P.Y) && (P.Y < p1_j.Y)) || ((p1_j.Y <= P.Y) && (P.Y < p1_i.Y)))
                    && (P.X < (p1_j.X - p1_i.X) * (P.Y - p1_i.Y) / (p1_j.Y - p1_i.Y) + p1_i.X))
                    result = !result;
                j = i;

            }
            return result;
        }
    }

}
