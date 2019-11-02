using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;

namespace Drawing3d
{


    /// <summary>
    /// The class xyArray is our base container for points of type xy.
    /// It has some pretty properties and Methods.
    ///	So the Count is setable. With the <see cref="xyzArray.Value"/> method you get for any positive param &lt; Count
    /// a point, which lies on the polygon.
      /// The Method <seealso cref="cross"/> returns  if the xyArray is clockwise 
    /// orientated, a positive value otherwise 
    /// the value is negative. The length of cross is twice the area of the xyArray.
    /// 
    /// </summary>
    [Serializable]

    public partial class xyArray : IEnumerable, ITransform2d
    {


        /// <summary>
        /// Returns a string, which starts with "{" and ends with "}".
        /// The points are separated with ";".
        /// <seealso cref="FromString"/>
        /// </summary>
        /// <returns>
        /// Returns a string, which starts with "{" and ends with "}".
        /// The points are separated with ";".<br/>
        /// For example:<br/>
        /// {00/00;
        ///  10,00/00;
        ///  10/10,00;
        ///  00/10,00}
        /// 
        /// </returns>
        public override string ToString()
        {
            string result = "{";
            for (int i = 0; i < Count; i++)
                if (i == Count - 1)
                    result += this[i].ToString() + "}";
                else
                    result += this[i].ToString() + ";";
            return result;
        }
        /// <summary>
        /// Initializes with a xy[] array.
        /// </summary>
        /// <param name="Value">Initializing array</param>
        public void FromArray(xy[] Value)
        {
            Count = Value.Length;
            this._data = Value;
        }
        /// <summary>
        /// Check the intersect by himself
        /// </summary>
        /// <returns>returns true. if the array intersect himself</returns>
        public bool SelfIntersecting()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                xy A = this[i];
                xy B = this[i + 1];
                LineType2d L = new LineType2d(A, B - A);
                for (int j = 0; j < Count - 1; j++)
                {
                    if ((j != i))
                    {
                        xy C = this[j];
                        xy D = this[j + 1];

                        LineType2d M = new LineType2d(C, D - C);
                        double Lam = -1;
                        double Mue = -1;
                        if (L.Cross(M, out Lam, out Mue))
                        {
                            if ((0.0000001 < Lam) && (Lam < 1 - 0.00000001) && (0.00000001 < Mue) && (Mue < 1 - 0.00000001))
                                return true;
                        }

                    }
                }

            }
            return false;
        }
        /// <summary>
        /// creates a star with a <b>Center</b>, the <b>count</b> of the tips,  <b>Innerradius</b> and the <b>Outerradius</b>.
        /// </summary>
        /// <param name="Center">Cneter of the star</param>
        /// <param name="Count">number of tips of the star</param>
        /// <param name="InnerRadius">inner radius</param>
        /// <param name="OuterRadius">outer radius</param>
        /// <returns></returns>
        public static xyArray CreateStar(xy Center, int Count, double InnerRadius, double OuterRadius)
        {
            double PI = System.Math.PI;
            xyArray Result = new xyArray(2 * Count + 1);
            for (int i = 0; i < Count; i++)
            {
                int id = 2 * i;
                Result[id] = new xy(Center.x + OuterRadius * Math.Cos(id * PI / Count), Center.y + OuterRadius * System.Math.Sin(id * PI / Count));
                Result[id + 1] = new xy(Center.x + InnerRadius * Math.Cos((id + 1) * PI / Count), Center.y + InnerRadius * Math.Sin((id + 1) * PI / Count));
            }
            Result[2 * Count] = new xy(Center.x + OuterRadius, Center.y);
            Result.Invert();
            return Result;
        }
        /// <summary>
        /// Returns an xy[] array
        /// </summary>
        /// <returns>Returns an xy[] array.</returns>
        public xy[] ToArray()
        {
            return data;
        }
        /// <summary>
        /// Converts the xyarray to an array of float points.
        /// </summary>
        /// <returns>array of float points</returns>
        public xyf[] ToFloatArray()
        {
            xyf[] Result = new xyf[Count];
            for (int i = 0; i < Count; i++)
                Result[i] = new xyf((float)data[i].x, (float)data[i].y);

            return Result;
        }
       
        /// <summary>
        /// Adds a point to the xyArray.
        /// <remarks>Its faster to create an Array by A= new xyArray(10) and 
        /// A[0] = new xy(1,2);-....
        /// A[9] = new xyz(4,-2);
        /// as
        /// create an array of length 0 and add 10 times a point.
        /// </remarks>
        /// </summary>
        /// <param name="Value">Point,which will be added.</param>
        public void Add(xy Value)
        {

            Count = Count + 1;
            this[Count - 1] = Value;
        }

        /// <summary>
        /// Removes the Point with a given Index from the Array 
        /// </summary>
        /// <param name="Index">Index of the point in the array</param>
        public void RemoveAt(int Index)
        {
            int i, le;
            le = Count;
            for (i = 0; i < le - Index - 1; i++)
                this[Index + i] = this[Index + i + 1];
            Count = Count - 1;
        }
        /// <summary>
        /// Returns true if the array is convex.
        /// </summary>
        /// <returns>True if the array is convex.</returns>
        public bool IsConvex()
        {
            if (Count < 3) return true;
            xy A = this[0];

            int j = 1;
            while ((j < Count) && (A.dist(this[j]) < 0.0000001)) j++;
            if (j == Count) return true;
            xy B = this[j];
            j++;
            while ((j < Count) && ((A.dist(this[j]) < 0.0000001) || (B.dist(this[j]) < 0.0000001)))
                j++;
            if (j == Count) return true;
            xy C = this[j];


            bool Positive = (Utils.Less(((A - B) & (C - B)), 0));
            for (int i = 0; i < Count; i++)
            {
                int v = i - 1; if (v < 0) v = Count - 1;
                int n = i + 1; if (n >= Count) n = 0;
                A = this[v];
                C = this[n];
                B = this[i];
                if (Positive)
                {
                    if (((A - B) & (C - B)) > 0.00000001) return false;
                }
                else
                {
                    if (((A - B) & (C - B)) < -0.00000001) return false;
                }


            }
            return true;
        }
        /// <summary>
        /// You can initialize an xyArray with a string.<br/>
        /// For example:
        /// xyzArray A;
        /// A= fromString(
        ///  "{0/0;
        ///  10/0;
        ///  10/10;
        ///  0/10}");
        /// </summary>
        /// <param name="Source">String which sets the values of the array</param>
        /// <returns></returns>
        public static xyArray FromString(string Source)
        {
            string[] s = Source.Split(';');
            int ct = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int id = s[i].IndexOf(";");
                if (id >= 0) s[i] = s[i].Remove(id, 1);
                id = s[i].IndexOf("{");
                if (id >= 0) s[i] = s[i].Remove(id, 1);
                id = s[i].IndexOf("}");
                if (id >= 0) s[i] = s[i].Remove(id, 1);
                s[i] = s[i].Trim();
                if (s[i] != "") ct++;
            }
            xyArray Result = new xyArray(ct);
            int j = 0;
            for (int i = 0; i < ct; i++)
            {
                if (s[i] != "")
                    try
                    {
                        Result[j] = xy.FromString(s[i]);
                        j++;
                    }
                    catch (Exception)
                    {
                    }
            }

            return Result;
        }

        /// <summary>
        /// Multiplicates each point with a Matrix m
        /// </summary>
        /// <param name="m">Matrix, which multiplcates the points</param>
        /// <param name="a">a xyArray</param>
        /// <returns></returns>
        public static xyArray operator *(Matrix3x3 m, xyArray a)
        {
            xyArray Result = new xyArray(a.Count);
            for (int i = 0; i < a.Count; i++) Result[i] = m * a[i];
            return Result;

        }
        ///// <summary>
        ///// Converts the elements to xyz points by lifting with z=0 an multiply them by the Matrix m.       
        ///// </summary>
        ///// <param name="m">Multiplication matrix</param>
        ///// <returns>a xyz array</returns>
        //public xyzArray Toxyz(Matrix m)
        //{

        //    xyzArray Result = new xyzArray(Count);
        //    for (int i = 0; i < Count; i++)
        //    {
        //        Result[i] = this[i].mul3D(m);
        //    }
        //    return Result;
        //}

        /// <summary>
        /// Implements the method GetEnumerator and returns the IEnumerator of the <see cref="data"/>field.
        /// </summary>
        /// <returns>IEnumerator</returns>

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }

        /// <summary>
        /// Calculates the direction of the polygon in a point, which is described by its param
        /// </summary>
        /// <param name="param">param describes the point by <seealso cref="Value"/></param>
        /// <returns>Returns the Direction of the linear interpolated point</returns>
        public xy Direction(double param)
        {
            int ID;
            double Lam;
            if (Count <= 1) return new xy(0, 0);
            ID = Utils.trunc(param);
            Lam = param - ID;
            if (ID < 0) return this[1] - this[0];
            if (ID >= Count - 1) return this[Count - 1] - this[Count - 2];
            return this[ID + 1] - this[ID];
        }
        /// <summary>
        /// Converts a xyArray to a xyzArray by lifting each xy to a xyz with z=0;
        /// </summary>
        /// <returns>The lifted xyzArray</returns>
        public xyzArray ToxyzArray()
        {
            return ToxyzArray(0);


        }
        /// <summary>
        /// Converts the array to a xyzarray by lifting with z.
        /// </summary>
        /// <param name="z">the third coordinate of the new xyz point in the xyzarray</param>
        /// <returns>xyzarray</returns>
        public xyzArray ToxyzArray(double z)
        {
            xyzArray result = new xyzArray(Count);
            for (int i = 0; i < Count; i++) { result[i] = new xyz(this[i].x, this[i].y, z); }
            return result;

        }
        /// <summary>
        /// Rounds the coordinates of all members to the destinated number of "Decimals"/> to
        /// </summary>
        /// <param name="decimals">Number of decimals</param>

        public void Round(int decimals)
        {
            for (int i = 0; i < Count; i++)
            { this[i] = new xy(System.Math.Round(this[i].x, decimals), System.Math.Round(this[i].y, decimals)); }
        }
        /// <summary>
        /// Converts a given length to a parameter, which can be used as param for the <see cref="Value"/>-method.
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="Le">A given length</param>
        /// <returns>a parameter</returns>
        public double LengthToParam(double Le)
        {
            if (!Utils.Less(Le, ParamToLength(Count - 1))) return - 1;
            int i = 0;
            double l = 0;
            while ((i + 1 < Count) && ((l + this[i].dist(this[i + 1])) < Le))
            {
                l = l + this[i].dist(this[i + 1]);
                i++;
            }
            return i + (Le - l) / this[i].dist(this[i + 1]);

        }
        /// <summary>
        /// Remove Points, which has a distance zero to the neighbour.
        /// </summary>
        public void RemoveZeros()
        {
            if (Count == 0) return;
            for (int i = Count - 1; i >= 1; i--)
            {
                if (data[i].dist(data[i - 1]) < 0.001)
                    this.RemoveAt(i);

            }
            if (data[0].dist(data[Count - 1]) < 0.001)
                this.RemoveAt(Count - 1);
        }
        /// <summary>
        /// if the first and the last point are equal, it returns true
        /// </summary>
        /// <returns>true, if the first and the last point are equal</returns>

        public bool Closed()
        {
            if (Count == 0) return false;
            return ((this[Count - 1] - this[0]).length() < 0.00001);
        }
        /// <summary>
        /// By <see cref="Value"/> you can get a point A= value(from) and a point B= value(to).
        /// The part of the array, which is bounded by this points will be copied to
        /// the Slicearray.
        /// <remarks>If the array is <see cref="Closed()"/> and "from" exceeds "to" it copies modulo Count-1.  </remarks>
        /// </summary>
        /// <param name="from">Start parameter</param>
        /// <param name="to">End parameter</param>
        /// <param name="SliceArray">Destination for slicing</param>
        /// <returns>SliceArray.</returns>
        public xyArray Slice(double from, double to, xyArray SliceArray)
        {

            xy Ap = Value(from);
            xy Ep = Value(to);
            int i;
            if (Utils.Equals(from, to))
            {
                SliceArray.Add(Ap);
                return SliceArray;
            }
            int fromInt = Utils.trunc(from);
            int toInt = Utils.trunc(to);
            double toLam = to - toInt;
            if (Utils.Less(from, to))
            {
                SliceArray.Add(Ap);
                if (fromInt == toInt)
                {
                    SliceArray.Add(Ep);
                    return SliceArray;
                }
                i = fromInt + 1;
                while (i <= toInt) { SliceArray.Add(this[i]); i++; }
                if (Utils.Less(0, toLam))
                    SliceArray.Add(Ep);
                return SliceArray;
            }
            else
            {
                SliceArray.Add(Ap);
                i = fromInt + 1;
                while (i <= Count - 1)
                {
                    SliceArray.Add(this[i]);
                    i++;
                }
                if (Closed())
                {
                    i = 1;
                    while (i <= toInt)
                    {
                        SliceArray.Add(this[i]);
                        i++;
                    }

                }
                if (Utils.Less(0, toLam))
                {
                    SliceArray.Add(Ep);
                }

            }
            return SliceArray;
        }
        /// <summary>
        /// By <see cref="Value"/> you can get a point A= value(from) and a point B= value(to).
        /// The part of the array, which is bounded by this points will be eturned
        /// <remarks>If the array is <see cref="Closed()"/> and "from" exceeds "to" it copies modulo Count-1.  </remarks>
        /// </summary>
        /// <param name="from">Start parameter</param>
        /// <param name="to">End parameter</param>
        /// <returns>Sliced Array.</returns>
        public xyArray Slice(double from, double to)
        {
            return Slice(from, to, new xyArray(0));
        }

        /// <summary>
        /// If the rectangle R contains any point of the array true will be returned
        /// </summary>
        /// <param name="R">A rectangle</param>
        /// <returns>True, if the rectangle contains one point</returns>
        public bool ContainsPoint(RectangleF R)
        {
            for (int i = 0; i < Count; i++)
                if (R.Contains((float)this[i].x, (float)this[i].y)) return true;

            return false;

        }
        /// <summary>
        /// Calculates the length of the array form 0 until the parameter given by Param.
        /// For a Param = Count you get the full length of the array
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="Param">A given param</param>
        /// <returns>Length from 0 to param</returns>
        public double ParamToLength(double Param)
        {
            double result = 0;
            int Bis = Utils.trunc(Param);
            if (Utils.Less(Count - 1, Bis))
            {
                Bis = Count - 1;
                Param = Count;
            }
            int i = 0;
            while (i < Bis)
                for (i = 0; i < Bis; i++)
                    result = result + (this[i + 1] - this[i]).length();
            result = result + (Value(Param) - this[Bis]).length();

            return result;
        }

        /// <summary>
        /// This function implements settheoretical operations for closed xyArrays. The result is a list of xyArrays (see <see cref="Loxy"/>
        /// </summary>
        /// <param name="value">the other <see cref="xyArray"/></param>
        /// <param name="Operation">The following values are possible
        /// </param>
        /// <returns>A list of xyArrays, which are produced by the settheoretical operation</returns>
        public Loxy SetOperation1(xyArray value, SetOperations Operation)
        {
            return null;




        }
        /// <summary>
        /// Get a list of cross points with an other closed xyArray. The items of <see cref="CrossList"/>
        /// are <see cref="CrossItem"/>. The items are sorted by the <see cref="CrossItem.Param1"/>.
        /// Every Crosslist contains an even number of cross points, one for going in and one for going out.
        /// </summary>
        /// <param name="Array">an other array</param>
        /// <returns>A crosslist</returns>
        public CrossList getCrossList(xyArray Array)
        {
            return getCrossList(Array, false);
        }

        private bool OnSector(xy MP, xy A, xy B, xy Test)
        {
            A = A - MP;
            B = B - MP;
            Test = Test - MP;
            if (A.Parallel(Test) && (!Utils.Less(A * Test, 0))) return true;
            if (B.Parallel(Test) && (!Utils.Less(B * Test, 0))) return true;
            return false;
        }
        /// <summary>
        /// Overload from <see cref="getCrossList(xyArray)"/>
        /// Additionally you have the possibility to set TangentPoints, which indicate, that 
        /// if two xyArray smooth together, a cross point is set. 
        /// </summary>
        /// <param name="Array"></param>
        /// <param name="TangentPoints"></param>
        /// <returns></returns>
        public CrossList getCrossList(xyArray Array, bool TangentPoints)
        {

            if (Array == null) return null;
            LineType2d L1, L2;
            CrossList result = new CrossList();
            result.AllowMultiCross = TangentPoints; // for instant
            double lam, mue;
            int k, r;
            bool Closed1 = Closed();
            bool Closed2 = Array.Closed();
            int StartIndex1=0;
            int StartIndex2 = 0;
            int EndIndex1;
            if (Closed()) EndIndex1 = int.MaxValue;
            else EndIndex1 = Count - 2;
            int EndIndex2;
            if (Array.Closed()) EndIndex2 = int.MaxValue;
            else EndIndex2 = Array.Count - 2;
            for (int i = 0; i < Count - 1; i++)
            {
                L1 = new LineType2d(this[i], this[i + 1] - this[i]);
                if (!L1.Direction.Equals(new xy(0, 0)))
                    for (int j = 0; j < Array.Count - 1; j++)
                    {

                        L2 = new LineType2d(Array[j], Array[j + 1] - Array[j]);
                        if (!L2.Direction.Equals(new xy(0, 0)))
                        {


                            bool solution = L1.Cross(L2, out lam, out mue);
                            if (!solution)
                            {
                                if (L2.inLine(L1.Q, out mue))
                                {
                                    lam = 1;

                                    solution = true;
                                }
                            }
                            lam = System.Math.Round(lam, 6);
                            mue = System.Math.Round(mue, 6);

                          
                            if (solution)
                                solution = ((!Utils.Less(mue, 0)) && (!Utils.Less(1, mue)) &&
                                    (!Utils.Less(lam, 0)) && (!Utils.Less(1, lam)));
                            if (solution)
                            {
                                bool between1 = (Utils.Less(0, lam) && Utils.Less(lam, 1));
                                bool between2 = (Utils.Less(0, mue) && Utils.Less(mue, 1));
                                int ck;
                                double d = L1.Direction & L2.Direction;
                                if (d > 0) ck = 1; else ck = -1;					// 9 Cases

                                if (between1 && between2)
                                    //1
                                    result.Add(new CrossItem(i + lam, j + mue, ck));
                                else
                                    //2
                                    if (Utils.Equals(0, lam) && between2)
                                {
                                    if (i <= StartIndex1)
                                        result.Add(new CrossItem(i + lam, j + mue, ck));
                                    else

                                        if (TangentPoints)
                                    {
                                        r = i; do { r--; } while ((r >= 0) && (Utils.Equals(0, this[i].dist(this[r]))));
                                        if ((r < 0) || (this[i] - this[r]).Parallel(L2.Direction))
                                            result.Add(new CrossItem(i + lam, j + mue, ck));
                                    }

                                    // handled by ->(Utils.Equals(1,lam) && between2)
                                }

                                else
                                        //3
                                        if (Utils.Equals(1, lam) && between2)
                                {


                                    if (TangentPoints)
                                    {
                                        if (!(L2.Direction.Parallel(L1.Direction))
                                            || ((i + 1 == Count - 1) && (!Closed())))
                                            result.Add(new CrossItem(i + lam - 0.00000001, j + mue - 0.00000001, ck));

                                    }
                                    else
                                    {


                                        // Check with next good item
                                        // go inside
                                        k = i + 1; do { k++; if (k > Count - 1) k = 0; } while (Utils.Equals(0, this[i + 1].dist(this[k])));
                                        LineType2d L = new LineType2d(this[i + 1], this[i] - this[i + 1]);

                                        bool insidetooutside = (!L.inSector(this[k] - this[i + 1], L2.P, true) // from inside to outside
                                            && L.inSector(this[k] - this[i + 1], L2.Q, true)
                                            );
                                        if (insidetooutside) ck = -1;
                                        bool outsidetoinside = (L.inSector(this[k] - this[i + 1], L2.P, true) // from outside to inside
                                            && !L.inSector(this[k] - this[i + 1], L2.Q, true));
                                        if (outsidetoinside) ck = 1;

                                        if ((insidetooutside || outsidetoinside) || (i >= EndIndex1)
                                            || (TangentPoints &&
                                            ((L.inSector(this[k] - this[i + 1], L2.P, true) ||
                                            L.inSector(this[k] - this[i + 1], L2.Q, true)))))



                                            result.Add(new CrossItem(i + (lam - 0.00000001), j + mue, ck));
                                    }
                                }

                                else
                                            //4
                                            if (between1 && Utils.Equals(0, mue))
                                {   // go inside

                                    if (TangentPoints)
                                    {
                                        r = j; do { r--; } while ((r >= 0) && (Utils.Equals(0, Array[j].dist(Array[r]))));
                                        if ((r < 0) || (Array[j] - Array[r]).Parallel(L1.Direction))
                                            result.Add(new CrossItem(i + lam, j + mue, ck));
                                    }
                                    else
                                        if ((Utils.Less(0, d)) || (j <= StartIndex2))
                                        result.Add(new CrossItem(i + lam, j + mue, ck));
                                }
                                //5
                                else
                                                if (between1 && Utils.Equals(1, mue))
                                {
                                    if (TangentPoints)
                                    {
                                        if (!(L1.Direction.Parallel(L2.Direction))
                                            || ((j + 1 == Array.Count - 1) && (!Array.Closed())))
                                            result.Add(new CrossItem(i + lam, j + mue - 0.00000001, ck));

                                    }
                                    else

                                        if ((Utils.Less(d, 0)) || (j >= EndIndex2)) // go outside
                                        result.Add(new CrossItem(i + lam, j + mue - 0.00000001, ck));

                                }
                                else
                                                    //6
                                                    if (Utils.Equals(0, lam) && Utils.Equals(0, mue))
                                {
                                    // if ((i <= StartIndex1) && (j <= StartIndex2))
                                    result.Add(new CrossItem(i + lam, j + mue, ck));

                                }

                                else
                                                        //7
                                                        if (Utils.Equals(0, lam) && Utils.Equals(1, mue))
                                {

                                    if (i <= StartIndex1)
                                        result.Add(new CrossItem(i + lam, j + (mue - 0.00000001), ck));
                                }

                                else
                                                            //8
                                                            if (Utils.Equals(1, lam) && Utils.Equals(0, mue))
                                {

                                    if (j <= StartIndex2)
                                        result.Add(new CrossItem(i + (lam - 0.00000001), j + mue, ck));
                                }

                                else
                                                                //9
                                                                if (Utils.Equals(1, lam) && Utils.Equals(1, mue))
                                {   // go outside ?
                                    // Check with next good item
                                    k = i + 1; do { k++; if (k >= Count) k = 0; } while (Utils.Equals(0, this[i + 1].dist(this[k])));
                                    r = j + 1;
                                    do { r++; if (r >= Array.Count) r = 0; } while (Utils.Equals(0, Array[j + 1].dist(Array[r])));


                                    if (TangentPoints)
                                    {
                                        k = i + 1; do { k++; if ((Closed()) && (k >= Count)) k = 0; } while
                                         ((k < Count) && (Utils.Equals(0, this[i + 1].dist(this[k]))));
                                        r = j + 1; do { r++; if ((Array.Closed()) && (r >= Array.Count)) r = 0; } while
                                                       ((r < Array.Count) && (Utils.Equals(0, Array[j + 1].dist(Array[r]))));
                                        if ((k < Count) && (r < Array.Count))
                                        {
                                            xy P = Array[r];
                                            if (!OnSector(L1.Q, L1.P, this[k], P) || !OnSector(L1.Q, L1.P, this[k], L2.P))
                                                result.Add(new CrossItem(i + (lam - 0.00000001), j + (mue - 0.00000001), ck));


                                        }
                                   }
                                    else
                                    {
                                        LineType2d L = new LineType2d(this[i + 1], this[i] - this[i + 1]);
                                        bool InsidetoOutSide = (!L.inSector(this[k] - this[i + 1], L2.P, true) // from inside to outside
                                            && L.inSector(this[k] - this[i + 1], Array[r], true));
                                        bool OutSidetoInside = L.inSector(this[k] - this[i + 1], L2.P, true) // from outside to inside
                                            && !L.inSector(this[k] - this[i + 1], Array[r], true);
                                        if (InsidetoOutSide) ck = -1; else ck = 1;

                                        if (InsidetoOutSide || OutSidetoInside)

                                            result.Add(new CrossItem(i + (lam - 0.00000001), j + (mue + 0.00000001), ck));
                                    }
                                }
                            }
                        }
                    }
            }
            return result;
        }
        /// <summary>
        /// Get the minimal rectangle, which contains the xyArray
        /// </summary>
        /// <returns>A minimal rectangle containing the array</returns>
        protected virtual RectangleF GetMaxrect()
        {
            RectangleF R = Utils.Resetrectangle();


            for (int i = 0; i < Count; i++)
                R = Utils.GetMaxRectangle(R, this[i]);
            return R;
        }

        /// <summary>
        /// Get the minimal rectangle containing transformed xyArray.
        /// </summary>
        /// <param name="T">Transformation on xyArray</param>
        /// <returns>A minimal rectangle containing the array</returns>
        public RectangleF GetMaxrect(Matrix T)
        {
            RectangleF R = Utils.Resetrectangle();

            for (int i = 0; i < Count; i++)
            {

                xy p = (T * (this[i].toXYZ())).toXY();
                R = Utils.GetMaxRectangle(R, p);
            }
            return R;
        }
        /// <summary>
        /// Calculates a <see cref="Rectangled"/>,  which is the smallest rectangle including the xyarray. 
        /// </summary>
        /// <returns></returns>
        public Rectangled _GetMaxRect()
        {
            Rectangled R = new Rectangled(1e10, 1e10, -1e10, -1e10);
            double x1 = 1e10;
            double y1 = 1e10;
            double x2 = -1e10;
            double y2 = -1e10;
            for (int i = 0; i < Count; i++)
            {
                xy p = this[i];
                x1 = System.Math.Min(x1, p.x);
                y1 = System.Math.Min(y1, p.y);
                x2 = System.Math.Max(x2, p.x);
                y2 = System.Math.Max(y2, p.y);



            }

            R = new Rectangled(x1, y1, x2 - x1, y2 - y1);
            return R;

        }
        /// <summary>
        /// Gets a Maxrect like <see cref="GetMaxrect()"/>
        /// </summary>
        public RectangleF Maxrect
        {
            get { RectangleF R = RectangleF.Empty; return GetMaxrect(); }
        }



        /// <summary>
        /// Creates a copy of the polygon and retrieves it.
        /// </summary>
        /// <returns>A copy of the array</returns>
        public xyArray copy()
        {

            xyArray Destination = new xyArray(Count);
            this.data.CopyTo(Destination.data, 0);
            return Destination;
        }
        class WorkarrayItem
        {
            public WorkarrayItem(int aid, int aList)
            {
                id = aid;
                List = aList;
            }
            public int id;
            public int List;
        }
        class Workarray : ArrayList
        {
            public new WorkarrayItem this[int i]   // Indexer declaration
            {
                get { return (WorkarrayItem)base[i]; }
                set { base[i] = value; }
            }
        }
        
        /// <summary>
        /// You can initialize the array by the param Acount.
        /// </summary>
        /// <param name="Acount">Count of points</param>
        public xyArray(int Acount)
        {
            this.data = new xy[Acount];

        }
        /// <summary>
        /// Enpty constructor.
        /// </summary>
        public xyArray()
        {
            this.data = null;
            Count = 0;
        }
        /// <summary>
        /// Calcuates the cross product for all members, what is useful for the calculation of the orientation and the area of
        /// the array.
        /// The length of cross : 2 * area of the polygon
        /// </summary>
        /// <returns>gets the cross product for all points</returns>
        public double cross()
        {
            double result = 0;
            for (int i = 1; i < Count - 1; i++)
                result = result + ((this[i] - this[0]).cross(this[i + 1] - this[0]));
            return result / 2;
        }

        /// <summary>
        /// This is the default indexer property
        /// </summary>
        public xy this[int i]   // Indexer declaration
        {
            get { return data[i]; }
            set { data[i] = value; }
        }
        /// <summary>
        /// Reads and writes the orientation. A xyArray is ClockWise, if <see cref="cross"/> > 0.
        /// If you set the value of clockwise to a new value, the array will be inverted.
        /// </summary>
        /// <returns>If the orientation of the array is clockwise, the result is true. Otherwise, it is false.</returns>
        [BrowsableAttribute(false)]
        public bool ClockWise
        {
            get { return cross() < 0; }
            set { if (value != ClockWise) this.Invert(); }
        }

        /// <summary>
        /// The property Count holds the number of elemements. The property is also settable.
        /// </summary>

        public int Count
        {
            get
            {
                if (data == null) return 0;
                return data.Length;
            }
            set
            {
                xy[] NewArray;
                if (value < 0)
                {
                    data = new xy[0];
                    return;
                }
                NewArray = new xy[value];
                if (data != null)
                {

                    if (value < Count)
                    {
                        for (int i = 0; i < value; i++)
                            NewArray[i] = this[i];
                    }
                    else
                        data.CopyTo(NewArray, 0);



                }
                data = NewArray;
            }

        }
        /// <summary>
        /// Array, which holds the points
        /// </summary>
        public xy[] data
        {
            get { return _data; }
            set { _data = value; }
        }
        xy[] _data = null;
        /// <summary>
        /// Returns the next index. If the array is closed the next index of Count-1 is 0 else it is i+1.
        /// </summary>
        /// <param name="i">The index</param>
        /// <returns>index+1 if index +1 less than count-1.
        /// if index+1 equals count-1 and the array is closed 0 will be returned.
        /// </returns>
        public int Next(int i)
        {
            xy P = this[i];
            int Start = i;

            do
            {
                i++;
                if (i == Count) i = 0;
                if (P.dist(this[i]) > 0.0001)
                    return i;
            }
            while (i != Start);
            return -1;

        }
        /// <summary>
        /// Returns the beforeindex. This means i-1 if i > 0 otherwise
        /// count-1, when the array is closed.
        /// 
        /// </summary>
        /// <param name="i">Startindex</param>
        /// <returns>  Returns the beforeindex. This means i-1 if i > 0 otherwise
        /// count-1, when the array is closed.</returns>
        public int Before(int i)
        {
            xy P = this[i];
            int Start = i;

            do
            {
                i--;
                if (i == -1) i = Count - 1;
                if (P.dist(this[i]) > 0.0001)
                    return i;
            }
            while (i != Start);
            return -1;

        }

        /// <summary>
        /// Calculates a point on the polygon belonging to param.
        /// For param = 0 , 1, 2, 3 ... you get the points this[0], this[1], this[2], this[3],...
        /// For values between a linear interpolation will be done.
        /// <example>
        /// <code>
        /// xyzArray a = new xyzArray(4);
        ///		a[0]= new xyz(1, 1 );
        ///		a[1]= new xyz(1,-1);
        ///		a[2]= new xyz(3, 4);
        ///		a[3]= new xyz(2, 2);
        ///		//a.Value(2.5) = (2.5, 3)
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="param">Parameter to intepolate the polygon</param>
        /// <returns>Linear interpolation of the polygon</returns>
        public xy Value(double param)
        {
            int ID = Utils.trunc(param);
            double Lam = param - ID;
            if (ID < 0) return this[0];
            if (ID >= Count - 1) return this[Count - 1];
            return this[ID] + (this[ID + 1] - this[ID]) * Lam;

        }
       
        /// <summary>
        /// The method transforms every point in the array by the matrix m.
        /// </summary>
        /// <param name="m">Transformmatrix</param>

        public void Transform(Matrix3x3 m)
        {
            for (int i = 0; i < Count; i++)
            { data[i] = data[i].mul(m); }
        }

        /// <summary>
        /// Checks the position of a point P (whether it lays inside the polygon or not).
        /// </summary>
        /// <param name="P">Point, that will be checked</param>
        /// <returns>If P lays inside the polygon, the result is true. Otherwise it is false</returns>
        public bool Inside(xy P)
        {

            int i, j;
            xy p1_i, p1_j;
            bool result = false;
            j = Count - 1;
            for (i = 0; i < Count; i++)
            {
                p1_i.x = this[i].x;
                p1_i.y = this[i].y;

                p1_j.x = this[j].x;
                p1_j.y = this[j].y;



                if ((((p1_i.y < P.y) && (P.y < p1_j.y)) || ((p1_j.y <= P.y) && (P.y < p1_i.y)))
                    && (P.x < (p1_j.x - p1_i.x) * (P.y - p1_i.y) / (p1_j.y - p1_i.y) + p1_i.x))
                    result = !result;
                j = i;

            }
            return result;
        }
        /// <summary>
        /// Calculates the distance to a Point, if this distance is smaller than MaxDist. Otherwise <see cref="Utils.big"/> will be returned.
        /// </summary>
        /// <param name="P">A Point</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="Lam">A value for which the nearest point in the array can evaluated with <see cref="Value"/></param>
        /// <returns>Distance.</returns>
        public double Distance(xy P, double MaxDist, out double Lam)
        {
            double d = 1e10;


            Lam = -1;
            if (Count == 0)
                return d;

            for (int i = 0; i < Count; i++)
            {
                // nachfolger der einen Positiven Abstand hat
                int nn = i + 1;
                if ((nn == Count))
                {
                    if (Closed()) nn = 0;
                    else
                        break;
                }
                xy A = this[i];
                xy B = this[nn];
                double L = (A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y);
                if (L < 0.000000001)
                    continue;
                // abstand zu Geraden

                double _lam = ((B - A) * (P - A) / L);
                if ((_lam > 0) && ((_lam <= 1))) // Projection zwischen A und B
                {
                    double dtemp = System.Math.Abs((P - A) * (B - A).normal().normalize());

                    if (dtemp < d)
                    {
                        d = dtemp;
                        Lam = i + _lam;

                    }
                }
                // Abstand zu AnfangPunkt
                else
                {
                    L = P.dist(A);
                    if (L < d)
                    {
                        d = L;
                        Lam = i;
                    }
                }

            }
            //Abstand zum Letzten


            double DD = System.Math.Sqrt((P.x - this[Count - 1].x) * (P.x - this[Count - 1].x) + (P.y - this[Count - 1].y) * (P.y - this[Count - 1].y));
            if (DD < d)
            {
                Lam = Count - 1.0000001;
                d = DD;
            }
            if (d > MaxDist)
            {
                Lam = -1;
                d = 1e10;
            }

            return d;

        }
      
        /// <summary>
        /// Calculates the distance of a point to the given array
        /// </summary>
        /// <param name="Point">The Point, whose distance will be calculted</param>
        /// <returns>Distance</returns>
        public double Distance(xy Point)
        {
            double LineLam;
            double MaxDist = 1000000;

            return Distance(Point, MaxDist, new xy(0, 0), out LineLam);
        }

        static xy MapPeriodic(xy Point, xy Periodicity)
        {
            double u = Point.x;
            double v = Point.y;

            if (Periodicity.x > 0)
            {
                while (Utils.Less(u, 0)) u += Periodicity.x;
                while (!Utils.Less(u, Periodicity.x)) u -= Periodicity.x;
            }

            if (Periodicity.y > 0)
            {
                while (Utils.Less(v, 0)) v += Periodicity.x;
                while (!Utils.Less(v, Periodicity.x)) v -= Periodicity.x;
            }
            return new xy(u, v);
        }
        static double PeriodicNearest(double a, double b, double Period)
        {
            if (Period == 0)
                return b;
            double Result = 0;
            double di = 1e10;
            if (System.Math.Abs(b - Period - a) < di)
            {
                Result = b - Period;
                di = System.Math.Abs(b - Period - a);
            }
            if (System.Math.Abs(b - a) < di)
            {
                Result = b;
                di = System.Math.Abs(b - a);

            }
            if (System.Math.Abs(b + Period - a) < di)
            {
                Result = b + Period;
                di = System.Math.Abs(b + Period - a);

            }
            return Result;

        }
        /// <summary>
        /// Calculates the distance to a Point, if this distance is smaller than MaxDist. Otherwise <see cref="Utils.big"/> will be returned.
        /// <seealso cref="Distance(xy, double, out double)"/>. Addionally you can define a Periodicity. Points, which differences in
        /// the x- or y- coordinate by the periodicity will be identified.
        /// </summary>
        ///
        /// <param name="Point">A Point</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="Periodicity">The x and y coordinates are taken modulo Periosicity.x resp periodicity.y. A periodicity of 0 will be ignored.</param>
        /// <param name="LineLam">A value for which the nearest point in the array can evaluated with <see cref="Value"/></param>
        /// <returns>Distance.</returns>
        public double Distance(xy Point, double MaxDist, xy Periodicity, out double LineLam)
        {
            bool IsPeriodic = ((Periodicity.x != 0) || (Periodicity.y != 0));
            double result = Utils.big;


            double di, _lam;
            LineType2d L2;

            LineLam = -1;
            for (int i = 1; i < Count; i++)
            {
                L2 = new LineType2d(this[i - 1], this[i].sub(this[i - 1]));
                xy _P = new xy(PeriodicNearest(this[i - 1].x, Point.x, Periodicity.x),
                    PeriodicNearest(this[i - 1].y, Point.y, Periodicity.y));
                di = L2.DistanceBounded(_P, out _lam);

                if (!Utils.Less(MaxDist, di) && (Utils.Less(di, result)))
                {
                    result = di;
                    if (Utils.Equals(_lam, 1)) LineLam = i;
                    else
                        LineLam = i - 1 + _lam;
                }
            }

            return result;

        }
        /// <summary>
        /// Calculates the distance to an other xyArray A, if this distance is smaller than MaxDist, otherwise <see cref="Utils.big"/> will be returned
        /// </summary>
        /// <param name="A">An other xyArray</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="param">A value for which the closest point in the array can be evaluated with <see cref="Value"/>. If a point of <b>this</b> is inside in the array A, the param gets the index of the inside point. ParamA is in this case -1.</param>
        /// <param name="paramA">A value for which the closest point in the array A can be evaluated with <see cref="Value"/>.  If a point of <b>A</b> is inside in <b>ithis array</b> the paramA gets the index of the inside point. Param is in this case -1.</param>
        /// <returns>Distance.</returns>
        public double Distance(xyArray A, double MaxDist, out double param, out double paramA)
        {
            param = -1;
            paramA = -1;
            for (int i = 0; i < A.Count; i++)
                if (Inside(A[i])) { paramA = i; return 0; }
            for (int i = 0; i < Count; i++)
                if (A.Inside(this[i])) { param = i; return 0; }

            double result = MaxDist;
            double di;
            double LineLam;
            paramA = 0;


            for (int j = 0; j < Count; j++)
            {
                xy p = this[j];
                for (int i = 0; i < A.Count; i++)
                {
                    if (p.dist(A[i]) < result)
                    {
                        result = p.dist(A[i]);
                        param = j;
                        paramA = i;
                    }
                }
            }

            for (int j = 0; j < Count; j++)
            {
                xy p = this[j];
                for (int i = 1; i < A.Count; i++)
                {
                    LineType2d L = new LineType2d(A[i - 1], A[i] - A[i - 1]);
                    xy dummy = new xy(0, 0);
                    di = L.Distance(p, out LineLam, out dummy);
                    if ((di < result)
                    && (!Utils.Less(1, LineLam))
                    && (!Utils.Less(LineLam, 0)))
                    {
                        result = di;
                        param = j;

                        if (Utils.Equals(LineLam, 1)) paramA = i;
                        else
                            paramA = i - 1 + LineLam;
                    }
                }
            }
            for (int j = 0; j < A.Count; j++)
            {
                xy p = A[j];
                for (int i = 1; i < Count; i++)
                {
                    LineType2d L = new LineType2d(this[i - 1], this[i] - this[i - 1]);
                    xy dummy = new xy(0, 0);
                    di = L.Distance(p, out LineLam, out dummy);
                    if ((di < result)
                    && (!Utils.Less(1, LineLam))
                    && (!Utils.Less(LineLam, 0)))
                    {
                        result = di;
                        paramA = j;

                        if (Utils.Equals(LineLam, 1)) param = i;
                        else
                            param = i - 1 + LineLam;
                    }
                }
            }



            return result;
        }

        ///// <summary>
        ///// The method transforms every point in the array by the matrix m and returns these points in an array.
        ///// </summary>
        ///// <param name="m">Transformmatrix</param>
        ///// <returns></returns>
        //public xyArray TransformEx(Matrix m)
        //{
        //    xyArray result = new xyArray(Count);

        //    for (int i = 0; i < Count; i++)
        //    { result[i] = data[i].mul(m); }
        //    return result;
        //}


        /// <summary>
        /// Inverts the array. The first entry will be the last and so on.
        /// </summary>

        public void Invert()
        {
            if (Count == 0) return;
            xy pivot;
            for (int i = 0; i <= (Count - 1) / 2; i++)
            {
                pivot = this[i];
                this[i] = this[Count - 1 - i];
                this[Count - 1 - i] = pivot;

            }

        }

        /// <summary>
        /// This method calculates the distance between the array and a line only in case the distance is
        /// smaller than MaxDist. Otherwise Utils.big will be returned.
        /// Imagine a cylinder with radius MaxDist around the polygon. If the line goes through the
        /// one of the cylinder, then the distance will be calculated and returned.
        /// </summary>
        /// <param name="L">The line which will be tested</param>
        /// <param name="MaxDist">The maximal distance, for which a reasonable result can be returned.</param>
        /// <param name="param">A value for which the closest point in the array can evaluated with <see cref="Value"/></param>
        /// <param name="LineLam">A value for which the closest point on the line can evaluated</param>
        /// <returns>In case the distance of the line is smaller than Maxdist, otherwise the distance is returned. <see cref="Utils.big"/>
        /// </returns>
        public double Distance(LineType L, double MaxDist, out double param, out double LineLam)
        {
            double result = Utils.big;
            int i;
            double di = -1;
            double _lam = -1;
            double _mue = -1;
            param = -1;
            LineLam = -1;
            xyz P1 = new xyz(0, 0, 0);
            xyz P2 = new xyz(0, 0, 0);
            for (i = 0; i < Count-1; i++)
            {   if ((this[i + 1] - this[i]).length() < 0.0000001) continue;
                LineType _L = new LineType(this[i].toXYZ(), this[i + 1].toXYZ() - this[i].toXYZ());
             
                di = _L.Distance(L, MaxDist,  out _mue, out _lam);
        
               if (!Utils.Less(MaxDist, di) && (Utils.Less(di, result)) && (-0.001 <_mue) && (_mue <1.0001))
                {
                    result = di;
                    param = i+_mue;
                    LineLam = _lam;
                 }
            }
            return result;

        }
        /// <summary>
        /// Calculates the distance to an other xyzArray A, if this distance is smaller than MaxDist, otherwise <see cref="Utils.big"/> will be returned
        /// </summary>
        /// <param name="A">An other xyzArray</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="param">A value for which the closest point in the array can be evaluated with <see cref="Value"/></param>
        /// <param name="paramA">A value for which the closest point in the array A can be evaluated with <see cref="Value"/></param>
        /// <returns></returns>
        public double Distance(xyzArray A, double MaxDist, out double param, out double paramA)
        {
            int i;
            double result = 1e10;
            double di;


            double _param, LineLam;
            paramA = 0;
            param = 0;
            for (i = 1; i < A.Count; i++)
            {
                LineType L = new LineType(A[i - 1], A[i] - A[i - 1]);

                di = Distance(L, MaxDist, out _param, out LineLam);
                if (!Utils.Less(MaxDist, di) && (!Utils.Less(LineLam, 0))
                    && (!Utils.Less(1, LineLam))
                    && (Utils.Less(di, result)))
                {
                    result = di;
                    param = _param;

                    if (Utils.Equals(LineLam, 1)) paramA = i;
                    else
                        paramA = i - 1 + LineLam;
                }
            }
            return result;
        }
        #region ITransform Member

        void ITransform2d.Transform(Matrix3x3 T)
        {

            for (int i = 0; i < Count; i++) this[i] = T * this[i];
           
        }

        #endregion
    }

}
