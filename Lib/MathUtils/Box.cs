
using System;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// The object Box encapsulates the essential properties and datas that are needed to work with it.
    /// So it holds an Origin and a Size.
    /// 
    /// </summary>
    [Serializable]
    public struct Box
    {

        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Down"- Array of the Box
        /// </summary>
        public const int Down = 0;
        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Up"- Array of the Box
        /// </summary>

        public const int Up = 1;
        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Front"- Array of the Box
        /// </summary>

        public const int Front = 2;
        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Back"- Array of the Box
        /// </summary>

        public const int Back = 3;
        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Left"- Array of the Box
        /// </summary>

        public const int Left = 4;
        /// <summary>
        /// This const is used from GetSide as param. GetSide(Down) returns the "Right"- Array of the Box
        /// </summary>
        public const int Right = 5;
        /// <summary>
        /// Point, where the Box is placed
        /// </summary>
        public xyz Origin;

        /// <summary>
        /// Size of the Box
        /// </summary>
        /// 
        public xyz Size;
        /// <summary>
        /// Constructor, 
        /// creates the box and appoints her origin and her size.
        /// </summary>
        /// <param name="Origin">Point, where the Box is placed</param>
        /// <param name="Size">Size of the box</param>
        public Box(xyz Origin, xyz Size)
        {
            this.Origin = Origin;
            this.Size = Size;
        }
        /// <summary>
        /// Constructor, who initializes the Box by its Size. The Origin is set to (0, 0, 0);
        /// </summary>
        /// <param name="Size">Size of the box</param>

        public Box(xyz Size)
        {
            this.Size = Size;
            Origin = new xyz(0, 0, 0);
        }
        /// <summary>
        /// Calculates an enveloping box of an array of points
        /// </summary>
        /// <param name="Points">array of points</param>
        /// <returns>enveloping box</returns>
        public static Box GetEnvBox(xyz[] Points)
        {
            Box B = new Box(new xyz(double.MaxValue / 2, double.MaxValue / 2, double.MaxValue / 2),
                new xyz(double.MinValue, double.MinValue, double.MinValue));
            return GetEnvBox(Points, B);
        }
        /// <summary>
        /// Calculates an enveloping box of an array of points and a startbox
        /// </summary>
        /// <param name="Points">Array of Points</param>
        /// <param name="Minimum">minimum box</param>
        /// <returns>enveloping box</returns>
        public static Box GetEnvBox(xyz[] Points, Box Minimum)
        {


            xyz untenLinks = Minimum.Origin;
            xyz obenRechts = Minimum.Origin + Minimum.Size;
            Box Result = new Box();
            foreach (xyz item in Points)
            {
                if (item.X < untenLinks.X)
                    untenLinks.X = item.X;
                if (item.Y < untenLinks.Y)
                    untenLinks.Y = item.Y;
                if (item.Z < untenLinks.Z)
                    untenLinks.Z = item.Z;


                if (item.X > obenRechts.X)
                    obenRechts.X = item.X;
                if (item.Y > obenRechts.Y)
                    obenRechts.Y = item.Y;
                if (item.Z > obenRechts.Z)
                    obenRechts.Z = item.Z;


            }
            Result.Origin = untenLinks;
            Result.Size = obenRechts - untenLinks;
            return Result;
        }
        /// <summary>
        /// Calculates an enveloping box of an array of float points and a startbox
        /// </summary>
        /// <param name="Points">Array of Points</param>
        /// <param name="Minimum">minimum box</param>
        /// <returns>enveloping box</returns>
        public static Box GetEnvBox(xyzf[] Points, Box Minimum)
        {


            xyz untenLinks = Minimum.Origin;
            xyz obenRechts = Minimum.Origin + Minimum.Size;
            Box Result = new Box();
            foreach (xyzf item in Points)
            {
                if (item.X < untenLinks.X)
                    untenLinks.X = item.X;
                if (item.Y < untenLinks.Y)
                    untenLinks.Y = item.Y;
                if (item.Z < untenLinks.Z)
                    untenLinks.Z = item.Z;


                if (item.X > obenRechts.X)
                    obenRechts.X = item.X;
                if (item.Y > obenRechts.Y)
                    obenRechts.Y = item.Y;
                if (item.Z > obenRechts.Z)
                    obenRechts.Z = item.Z;


            }
            Result.Origin = untenLinks;
            Result.Size = obenRechts - untenLinks;
            return Result;
        }



        /// <summary>
        /// Reset the Box at the begin, if you want to calculate an environment box by using the
        /// method GetMaxBox
        /// 
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// xyz P1 = new xyz(3, 2, 1);
        /// xyz P2 = new xyz(-4,1, 5);
        /// 
        /// Box B = ResetBox();
        /// B = B.GetMaxBox(P1);
        /// B = B.GetMaxBox(P2);
        /// // now B encapsulates the Points P1 and P2.
        /// // his Origin is at (-4, 1, 1)
        /// // his Size is ( 7, 1, 4 )
        /// 
        /// 
        /// </code>
        /// </example>

        static public Box ResetBox()
        {
            Box result = new Box(new xyz(Utils.small, Utils.small, Utils.small));
            result.Origin.x = Utils.big;
            result.Origin.y = Utils.big;
            result.Origin.z = Utils.big;
            return result;
        }
        /// <summary>
        /// gets true if the box is empty
        /// </summary>
        /// <returns>true if the box is empty</returns>
        public bool IsEmpty()
        {
            return ((Size.x < 0) || (Size.Y < 0) || (Size.Z < 0));
        }
        /// <summary>
        /// With this method you can easily calculate a small Box which contains the oldbox ( this) and
        /// the point pt
        /// </summary>
        /// <param name="pt">Point, that will be checked</param>
        /// <returns>Returns the small box, containing the point pt</returns>

        public Box GetMaxBox(xyz pt)
        {
            Box result;
            if (IsEmpty())
            {
                result.Origin = pt;
                result.Size = xyz.Null;
                return result;
            }
            result.Origin = Origin;
            result.Size = Size;
            if (pt.x < Origin.x) { result.Origin.x = pt.x; result.Size.x = Size.x + (Origin.x - pt.x); }
            if (pt.x > Origin.x + Size.x) { result.Size.x = pt.x - Origin.x; }
            if (pt.y < Origin.y) { result.Origin.y = pt.y; result.Size.y = Size.y + (Origin.y - pt.y); }
            if (pt.y > Origin.y + Size.y) { result.Size.y = pt.y - Origin.y; }

            if (pt.z < Origin.z) { result.Origin.z = pt.z; result.Size.z = Size.z + (Origin.z - pt.z); }
            if (pt.z > Origin.z + Size.z) { result.Size.z = pt.z - Origin.z; }
            return result;







        }
        /// <summary>
        /// With this method you can easily calculate a small Box which contains the oldbox ( this) and
        /// the float point pt
        /// </summary>
        /// <param name="pt">Float point, that will be checked</param>
        /// <returns>Returns the small box, containing the point pt</returns>
        public Box GetMaxBox(xyzf pt)
        {
            Box result;
            if (IsEmpty())
            {
                result.Origin = pt.Toxyz();
                result.Size = xyz.Null;
                return result;
            }
            result.Origin = Origin;
            result.Size = Size;
            if (pt.x < Origin.x) { result.Origin.x = pt.x; result.Size.x = Size.x + (Origin.x - pt.x); }
            if (pt.x > Origin.x + Size.x) { result.Size.x = pt.x - Origin.x; }
            if (pt.y < Origin.y) { result.Origin.y = pt.y; result.Size.y = Size.y + (Origin.y - pt.y); }
            if (pt.y > Origin.y + Size.y) { result.Size.y = pt.y - Origin.y; }

            if (pt.z < Origin.z) { result.Origin.z = pt.z; result.Size.z = Size.z + (Origin.z - pt.z); }
            if (pt.z > Origin.z + Size.z) { result.Size.z = pt.z - Origin.z; }
            return result;



        }
        /// <summary>
        /// calculates like <see cref="GetMaxBox(xyz)"/> an enveloping box. The Point pt is taken relative to the Base.
        /// </summary>
        /// <param name="RelativeBase">Base</param>
        /// <param name="pt">Point</param>
        /// <returns>enveloping box</returns>
        internal Box GetMaxBox(Base RelativeBase, xyz pt)
        {

            return GetMaxBox(RelativeBase.Relativ(pt));
        }
        private Box GetMaxBox(Base RelativeBase, xyzf pt)
        {

            return GetMaxBox(RelativeBase.Relativ(new xyz(pt.X, pt.Y, pt.Z)));
        }
        /// <summary>
        /// Returns a enveloping <see cref="Box"/> of a <see cref="xyzArray"/> , relative to a base.
        /// </summary>
        /// <param name="RelativeBase">Relative to this Base the enveloping Box will be calculated</param>
        /// <param name="A">the enveloped xyzArray</param>
        /// <returns>The enveloping Box</returns>
        internal Box GetMaxBox(Base RelativeBase, xyzArray A)
        {

            if (A.Count == 0) return Box.ResetBox();
            Box B = GetMaxBox(RelativeBase, A[0]);

            for (int i = 1; i < A.Count; i++)
                B = B.GetMaxBox(RelativeBase, A[i]);

            return B;
        }
        /// <summary>
        /// Returns a enveloping <see cref="Box"/> of a float xyzf array , relative to a base.
        /// </summary>
        /// <param name="RelativeBase">Relative to this Base the enveloping Box will be calculated</param>
        /// <param name="A">float array of points</param>
        /// <returns>The enveloping Box</returns>
        public Box GetMaxBox(Base RelativeBase, xyzf[] A)
        {

            if (A.Length == 0) return Box.ResetBox();
            Box B = GetMaxBox(A[0]);

            for (int i = 1; i < A.Length; i++)
                B = B.GetMaxBox(RelativeBase, A[i]);

            return B;
        }
       
        /// <summary>
        /// Returns a enveloping <see cref="Box"/> of this.Box and a Box, relative to a base.
        /// </summary>
        /// <param name="RelativeBase">Relative to this Base the enveloping Box will be calculated</param>
        /// <param name="Box">Is also included in the result box</param>
        /// <returns>Returns a enveloping <see cref="Box"/> of this array and a Box, relative to a base.</returns>
        public Box GetMaxBox(Base RelativeBase, Box Box)
        {

            Box B = GetMaxBox(RelativeBase, Box.Origin);
            return B.GetMaxBox(RelativeBase, Box.Origin + Box.Size);


        }
        /// <summary>
        /// Returns a enveloping <see cref="Box"/> of this array and a Box.
        /// </summary>
        /// <param name="Box">Is also included in the result box</param>
        /// <returns>Returns a enveloping <see cref="Box"/> of this array and a Box, relative to a base.</returns>
        public Box GetMaxBox(Box Box)
        {
            return GetMaxBox(Base.UnitBase, Box);
        }
        private bool IntersectParalellogram(LineType L, xyz A, xyz B, xyz C, xyz D)
        {
            xyz U = A - L.P;
            xyz V = B - L.P;
            double d = (U & V) * L.Direction;
            U = B - L.P;
            V = C - L.P;
            if ((U & V) * L.Direction * d < 0)
                return false;

            U = C - L.P;
            V = D - L.P;
            if ((U & V) * L.Direction * d < 0)
                return false;

            U = D - L.P;
            V = A - L.P;
            if ((U & V) * L.Direction * d < 0)
                return false;
            return true;
        }
        /// <summary>
        /// Gets true if the Line the box crosses.
        /// </summary>
        /// <param name="L">A Line</param>
        /// <returns>Gets true if the Line the box crosses.</returns>
        public bool InterSect(LineType L)
        {
            for (int i = 0; i < 6; i++)
            {
                xyzArray A = GetSide(i);
                if (IntersectParalellogram(L, A[0], A[1], A[2], A[3])) return true;
            }
            return false;
        }
        /// <summary>
        /// GetSide returns for the constants Up, Down, Front, Back, Left, Right a xyzArray, which holds the 
        /// points responding to its side.
        /// In order to that you can iterate over the values 0 to 5 to get all sides of a box.
        /// The Sides are clockwise orientated relative to a "screwvector", which heads for the inside of the box.
        /// </summary>
        /// <example>
        /// <code>
        /// xyzArray a;
        /// for ( int i = 0; i &lt; 6; i++)
        /// {
        ///  a = GetSide(i);
        /// .. do something with a for example: draw it
        /// }
        /// 
        /// </code>
        /// </example>
        /// <param name="value">The side you prefer, for example
        /// <see cref="Down">Down,</see>
        /// <see cref="Up">Up,</see>
        /// <see cref="Left">Left,</see>
        /// <see cref="Right">Right,</see>
        /// <see cref="Front">Front,</see>
        /// <see cref="Back">Back</see>
        /// </param>
        /// 
        /// <returns></returns>
        public xyzArray GetSide(int value)
        {
            xyzArray result = new xyzArray(5);
            switch (value)
            {
                case Down:

                    result[0] = new xyz(0, 0, 0) + Origin;
                    result[1] = new xyz(Size.x, 0, 0) + Origin;
                    result[2] = new xyz(Size.x, Size.y, 0) + Origin;
                    result[3] = new xyz(0, Size.y, 0) + Origin;
                    result[4] = new xyz(0, 0, 0) + Origin;
                    return result;

                case Up:
                    result[0] = new xyz(0, 0, Size.z) + Origin;
                    result[1] = new xyz(0, Size.y, Size.z) + Origin;
                    result[2] = new xyz(Size.x, Size.y, Size.z) + Origin;
                    result[3] = new xyz(Size.x, 0, Size.z) + Origin;
                    result[4] = new xyz(0, 0, Size.z) + Origin;
                    return result;

                case Front:
                    result[0] = new xyz(0, 0, 0) + Origin;
                    result[1] = new xyz(0, 0, Size.z) + Origin;
                    result[2] = new xyz(Size.x, 0, Size.z) + Origin;
                    result[3] = new xyz(Size.x, 0, 0) + Origin;
                    result[4] = new xyz(0, 0, 0) + Origin;
                    return result;

                case Back:
                    result[0] = new xyz(0, Size.y, 0) + Origin;
                    result[1] = new xyz(Size.x, Size.y, 0) + Origin;
                    result[2] = new xyz(Size.x, Size.y, Size.z) + Origin;
                    result[3] = new xyz(0, Size.y, Size.z) + Origin;
                    result[4] = new xyz(0, Size.y, 0) + Origin;
                    return result;

                case Right:
                    result[0] = new xyz(Size.x, 0, 0) + Origin;
                    result[1] = new xyz(Size.x, 0, Size.z) + Origin;
                    result[2] = new xyz(Size.x, Size.y, Size.z) + Origin;
                    result[3] = new xyz(Size.x, Size.y, 0) + Origin;
                    result[4] = new xyz(Size.x, 0, 0) + Origin;
                    return result;

                case Left:
                    result[0] = new xyz(0, 0, 0) + Origin;
                    result[1] = new xyz(0, Size.y, 0) + Origin;
                    result[2] = new xyz(0, Size.y, Size.z) + Origin;
                    result[3] = new xyz(0, 0, Size.z) + Origin;
                    result[4] = new xyz(0, 0, 0) + Origin;
                    return result;

            }

            return result;
        }
    }
}
