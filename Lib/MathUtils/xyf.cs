
using System;
using System.Collections.Generic;

namespace Drawing3d
{
    /// <summary>
    /// a point, which holds x,y coordinates as float.
    /// We call such a point "Float Point"
    /// See <see cref="xy"/>
    /// </summary>
    [Serializable]

    public struct xyf
    {/// <summary>
     /// x coordinate as float 
     /// </summary>
        public float x;
        /// <summary>
        /// y coordinate as float 
        /// </summary>
        public float y;
        /// <summary>
        /// Constructor with x and y coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public xyf(float x, float y)
        {
            this.x = x;
            this.y = y;

        }
        /// <summary>
        /// shifts <see cref="xyf"/> to a <see cref="xyzf"/> by adding 0.
        /// </summary>
        /// <returns></returns>
        public xyzf Toxyzf()
        { return new Drawing3d.xyzf(x, y, 0); }
        /// <summary>
        /// Operator for addition of xyf points
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <returns>the addition of a and b</returns>
        public static xyf operator +(xyf a, xyf b)
        { return a.add(b); }
        /// <summary>
        /// Operator for scalar product of two xyf points
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <returns>the scalarproduct of a and b</returns>
        public static xyf operator *(xyf a, float b)
        { return new xyf(a.x * b, a.y * b); }
        /// <summary>
        /// Converts a xyArray to an array of float points
        /// </summary>
        /// <param name="Array">the xyArray, which will be converted</param>
        /// <returns>Array of <see cref="xyf"/></returns>
        public static xyf[] ToxyfArray(xyArray Array)
        {
            xyf[] Result = new xyf[Array.Count];
            for (int i = 0; i < Array.Count; i++)
            {
                Result[i] = new xyf((float)Array[i].x, (float)Array[i].y);
            }
            return Result;
        }
        /// <summary>
        /// Converts a List of xy to an array of float points
        /// </summary>
        /// <param name="Array">List of xy, which will be converted</param>
        /// <returns>Array of <see cref="xyf"/></returns>
        public static xyf[] ToxyfArray(List<xy> Array)
        {
            xyf[] Result = new xyf[Array.Count];
            for (int i = 0; i < Array.Count; i++)
            {
                Result[i] = new xyf((float)Array[i].x, (float)Array[i].y);
            }
            return Result;
        }
        /// <summary>
        /// Converts an array of xy to an array of float points
        /// </summary>
        /// <param name="Array">Array of xy, which will be converted</param>
        /// <returns>Array of <see cref="xyf"/></returns>
        public static xyf[] ToxyfArray(xy[] Array)
        {
            xyf[] Result = new xyf[Array.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                Result[i] = new xyf((float)Array[i].x, (float)Array[i].y);
            }
            return Result;
        }
        /// <summary>
        /// adds the float point a
        /// </summary>
        /// <param name="a"></param>
        /// <returns>returns this + a</returns>
        public xyf add(xyf a)
        {
            return new xyf(a.x + x, a.y + y);
        }
    }
}
