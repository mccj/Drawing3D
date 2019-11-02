
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;

namespace Drawing3d
{
    /// <summary>
    /// The class xyzArray is our base container for points of type xyz.
    /// It has some pretty properties and Methods.
    ///	So the count is setable. With the <see cref="xyzArray.Value"/> method you get for any positive param &lt; Count
    /// a point, which lies on the polygon.
    /// The Method <seealso cref="cross"/> returns if the xyzArray is plane, a normal vector.
    /// His length is 2*area of the closed polygon.
    /// </summary>
    [Serializable]
    public class xyzArray :  ITransform
    {
        #region ITransform Member

        void ITransform.Transform(Matrix T)
        {
            for (int i = 0; i < Count; i++) this[i] = T * this[i];
            {

            }
        }

        #endregion
        /// <summary>
        /// gets a Base if the array is  <see cref="planar"/>  else
        /// the Base.Basez vector is zero.
        /// 
        /// </summary>
        public  Base Base;
        /// <summary>
        /// Maultiplicates the points of the array a by a Matrix m.
        /// </summary>
        /// <param name="m">The Matrix</param>
        /// <param name="a">The array containing points, which will be transformed by the matrix m</param>
        /// <returns></returns>
        public static xyzArray operator *(Matrix m, xyzArray a)
        {
            xyzArray Result = new xyzArray(a.Count);
            for (int i = 0; i < a.Count; i++)
                Result[i]=m * a[i];
            Result.Base = m * a.Base; 
            return Result;

        }
        /// <summary>
        /// Converts the array to an array of float points
        /// </summary>
        /// <returns>array of float points</returns>
        public xyzf[] ToFArray()
        {
            xyzf[] Result = new xyzf[Count];
            for (int i = 0; i < Count; i++) Result[i] = new xyzf((float)data[i].x, (float)data[i].y, (float)data[i].z);
            return Result;


        }
        void FromArray(xyz[] Value)
        {
            Count = Value.Length;
            this._data = Value;
            CheckPlanar();
        }
        /// <summary>
        /// Converts the array to a string
        /// e.g:
        ///  xyzArray Sample = new xyzArray();
        ///   Sample.data = new xyz[] { new xyz(1, 0, 0), new xyz(1, 0, 0) };
        /// returns "1/0/0;1/0/0"
        /// <see cref="FromString(string)"/>
        /// </summary>
        /// <returns>the converted string</returns>
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Count; i++)
                if (i == Count - 1)
                    result += this[i].ToString() + "";
                else
                    result += this[i].ToString() + ";";
            return result;
        }
        /// <summary>
        /// You can initialize an xyzArray with a string.<br/>
        /// For example:
        /// xyzArray A;
        /// A= fromString(
        ///  "0/0/00;
        ///  10/0/0;
        ///  10/0/10;
        ///  0/10/0");
        /// </summary>
        /// <param name="Source">String which sets the values of the array</param>
        /// <returns></returns>
        public static xyzArray FromString(string Source)
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
            xyzArray Result = new xyzArray(ct);
            int j = 0;
            for (int i = 0; i < ct; i++)
            {
                if (s[i] != "")
                    try
                    {
                        Result[j] = xyz.FromString(s[i]);
                        j++;
                    }
                    catch (Exception)
                    {
                    }
            }
            Result.CheckPlanar();
            return Result;
        }
       void CheckPlanar()
        {   // Search Normal
            xyz Normal = new xyz(0, 0, 0);
            for (int i = 1; i < Count-2; i++)
            {
             if  (((this[i]-this[0])& (this[i+1] - this[0])).length() >0.1)
                { Normal = ((this[i] - this[0]) & (this[i + 1] - this[0])).normalized();
                    break;
                }
            }
            for (int i = 2; i < Count; i++)
            {
                if (Math.Abs(Normal*(this[i]-this[0]))>0.00001)
                    { planar = false;
                    Base.BaseZ = new xyz(0, 0, 0);
                    return;
                    }
            }
            planar = true;
            if (Count >2)
            Base = Drawing3d.Base.DoComplete(this[0],Normal);
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
            CheckPlanar();
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
        /// Returns an eveloping box of the array relative to a base.
        /// </summary>
        /// <param name="RelativeBase">relative to this base the enveloping box is calculated</param>
        /// <returns></returns>
        public Box GetMaxBox(Base RelativeBase)
        {

            Box B = Box.ResetBox();
            for (int i = 0; i < Count; i++)
                B = B.GetMaxBox(RelativeBase, this[i]); ;
            return B;

        }
        /// <summary>
        /// Implements the method GetEnumerator and returns the IEnumerator of the <see cref="data"/>field.
        /// </summary>
        /// <returns>IEnumerator</returns>

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }

        /// <summary>
        /// Calculates a point on the polygon belonging to param.
        /// For param = 0 , 1, 2, 3 ... you get the points this[0], this[1], this[2], this[3],...
        /// For values between a linear interpolation will be done.
        /// <example>
        /// <code>
        /// xyzArray a = new xyzArray(4);
        ///		a[0]= new xyz(1, 1, 1);
        ///		a[1]= new xyz(1,-1, 2);
        ///		a[2]= new xyz(3, 4, 2);
        ///		a[3]= new xyz(2, 2, 1);
        ///		//a.Value(0.5) = (1, 0, 1.5)
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="param">Parameter to interpolate the polygon</param>
        /// <returns>Linear interpolation of the polygon</returns>
        public xyz Value(double param)
        {
            int ID = Utils.trunc(param);
            double Lam = param - ID;
            if (ID < 0) return this[0];
            if (ID >= Count - 1) return this[Count - 1];
            return this[ID] + (this[ID + 1] - this[ID]) * Lam;

        }
        /// <summary>
        /// Calculates the direction of the polygon in a point, which is described by its param
        /// </summary>
        /// <param name="param">param describes the point by <seealso cref="Value"/></param>
        /// <returns>Returns the direction of the linear interpolated point</returns>
        public xyz Direction(double param)
        {
            int ID;
            double Lam;
            if (Count <= 1) return new xyz(0, 0, 0);
            ID = Utils.trunc(param);
            Lam = param - ID;
            if (ID < 0) return this[1] - this[0];
            if (ID >= Count - 1) return this[Count - 1] - this[Count - 2];
           
            return this[ID + 1] - this[ID];
        }
        /// <summary>
        /// This is the default indexer property
        /// </summary>
        public xyz this[int i]   // Indexer declaration
        {
            get { return data[i]; }
            set {

                data[i] = value;
                if (i >= Count - InizallizedBy)
                {
                    InizallizedBy--;
                    if (i == 2)
                    {

                        if (((this[2] - this[0]) & (this[1] - this[0])).length() > 0)
                        {
                            Base = Drawing3d.Base.DoComplete(this[0], ((this[2] - this[0]) & (this[1] - this[0])));
                            planar = true;

                        }
                    }
                    else
                 if (i > 2)
                    {

                        if ((planar) && (Base.BaseZ.length() > 0.9))
                        {
                            if (Math.Abs((this[i] - this[0]) * Base.BaseZ) > 0.00001) // nicht planar
                            {
                                planar = false;
                                Base.BaseZ = new xyz(0, 0, 0);
                            }

                        }
                       

                    }
                }
                 }
        }
        /// <summary>
        /// Converts a xyzArray to a xyArray relative to the base if it is planar, else the z-coord is omitted.
        /// </summary>
        /// <returns>The lifted xyzArray</returns>
        public xyArray ToxyArray()
        {


            xyArray result = new xyArray(Count);
            if (planar)
            { for (int i = 0; i < Count; i++)
                { result[i] = this.Base.Relativ(this[i]).toXY(); } }
            else
            for (int i = 0; i < Count; i++)
                { result[i] = new xy(this[i].x, this[i].y); }
            return result;

        }
        /// <summary>
        /// Returns true if the array is convex.
        /// <remarks>The Array must be flat.</remarks>
        /// </summary>
        /// <returns>True if the array is convex.</returns>
        public bool IsConvex()
        {
            if (Count < 3) return true;
            xyz A = this[0];

            int j = 1;
            while ((j < Count) && (A.dist(this[j]) < 0.0000001)) j++;
            if (j == Count) return true;
            xyz B = this[j];
            j++;
            while ((j < Count) && ((A.dist(this[j]) < 0.0000001) || (B.dist(this[j]) < 0.0000001)))
                j++;
            if (j == Count) return true;
            xyz C = this[j];

            xyz V = (A - B) & (C - B);

            for (int i = 0; i < Count; i++)
            {
                int v = i - 1; if (v < 0) v = Count - 1;
                int n = i + 1; if (n >= Count) n = 0;
                A = this[v];
                C = this[n];
                B = this[i];
                if (Utils.Less(((A - B) & (C - B)) * V, 0)) return false;

            }
            return true;
        }
        /// <summary>
        /// Returns an xy[] array
        /// </summary>
        /// <returns>Returns an xy[] array.</returns>
        public xyz[] ToArray()
        {
            return data;
        }
      /// <summary>
      /// Indicates that the array is planar. See also <see cref="Base"/>.
      /// </summary>
       public bool planar = true;
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
        public void Add(xyz Value)
        {

            Count = Count + 1;
            this[Count - 1] = Value;
            if (Count==3)
            {

                if (((this[2] - this[0]) & (this[1] - this[0])).length() > 0)
                {
                   
                    Base = Drawing3d.Base.DoComplete(this[0], ((this[2] - this[0]) & (this[1] - this[0])));
                    planar = true;

                }
            }
            else
                if (Count>3)
            {

                if ((planar)&&(Base.BaseZ.length()>0.9))
                {
                    if (  Math.Abs((this[Count - 1]- this[0])* Base.BaseZ)>0.0001) // nicht planar
                    {
                        planar = false;
                        Base.BaseZ = new xyz(0, 0, 0);
                    }

                }
            }
        }
        /// <summary>
        /// Creates a copy of the polygon and retrieves it.
        /// </summary>
        /// <returns>A copy of the array</returns>
        public xyzArray copy()
        {
            xyzArray Result = new xyzArray();
            Result.planar = planar;
            Result.Base = Base;
            this.data.CopyTo(Result.data, 0);
           
            return Result;
          
        }
        /// <summary>
        /// If the first and the last point are equal, it returns true.
        /// </summary>
        /// <returns>true, if the first and the last point are equal.</returns>
        public bool Closed()
        {
            if (Count == 0) return true;
            return ((this[Count - 1] - this[0]).length() < 0.00001);
        }
        /// <summary>
        /// Adds a point with same coordinates as the first one
        /// </summary>
        public void Close()
        {
            if (!Closed())
                this.Add(this[0]);
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
        int InizallizedBy = 0;
        /// <summary>
        /// You can initialize the array by the param Acount.
        /// </summary>
        /// <param name="Acount">Count of points</param>
        public xyzArray(int Acount)
        {
            InizallizedBy = Acount;
            this.data = new xyz[Acount];

       
        }
        /// <summary>
        /// Empty constructor
        /// </summary>
        public xyzArray()
        {
           
        }
        
        private Object _Tag;
        /// <summary>
        /// You can use this Tag
        /// </summary>
        public Object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        /// <summary>
        /// Calcuates the cross product over all members, which is usefull for calculation of the normal and the area of
        /// the array.
        /// In case, the polygon is plane cross is a normal, which has the length : 2 * area of the polygon.
        /// </summary>
        /// <returns>returns the cross product over all points</returns>
        public xyz cross()
        {

            xyz result = new xyz(0, 0, 0);
            for (int i = 1; i < Count - 1; i++)
                result = result + ((this[i] - this[0]).cross(this[i + 1] - this[0]));

            return result;
        }
        /// <summary>
        /// Calculates a normalvector of the array.
        /// <remarks>The array must be flat</remarks>
        /// </summary>
        /// <returns></returns>
        public xyz normal()
        {
            xyz result = new xyz(0, 0, 0);
            for (int i = 1; i < Count - 1; i++)
                result = result + ((this[i] - this[0]).cross(this[i + 1] - this[0]));

            return result;
        }

        xyz[] _data = null;
        /// <summary>
        /// Array, which holds the points
        /// </summary>
        public xyz[] data
        {
            get { return _data; }
            set { _data = value;
                if (!CheckedPlanar)
                CheckPlanar();
                CheckedPlanar = false;

            }
        }
        /// <summary>
        /// Indicates the orientation.
        /// If the cross vector heads to the same side of the array like the
        /// Direction then the result is true, otherwise it is false.
        /// </summary>
        /// <param name="Direction">Direction to determine the orientation</param>
        /// <returns>If the orientation of the array is clockwise then the result is true, otherwise it is false.</returns>
        [BrowsableAttribute(false)]
        public bool ClockWise(xyz Direction)
        {
            return cross() * Direction < 0;
        }
        bool CheckedPlanar = false;
        /// <summary>
        /// The property Count holds the number of elemements. The property is also settable.
        /// </summary>
        [BrowsableAttribute(false)]
        public int Count
        {
            get
            {
                if (data == null) return 0;
                return data.Length;

            }
            set
            {
                xyz[] NewArray;
                if (value < 0)
                {
                    data = new xyz[0];
                    return;
                }
                NewArray = new xyz[value];
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
                CheckedPlanar = true;
                data = NewArray;

            }

        }
        /// <summary>
        /// The method transforms every point in the array by the matrix m.
        /// </summary>
        /// <param name="m">Transformmatrix</param>
        public void Transform(Matrix m)
        {
            for (int i = 0; i < Count; i++)
            { data[i] = data[i].mul(m); }
        }
        /// <summary>
        /// Calculates a environment box with axes parallel to unitbase for the array.
        /// </summary>
        /// <returns></returns>

        public Box MaxBox()
        {
            Box result = Box.ResetBox();
            for (int i = 0; i < Count; i++)
                result = result.GetMaxBox(this[i]);
            return result;
        }
        /// <summary>
        /// Calculates a environmentbox with axes parallel to relativebase for the array.
        /// </summary>
        /// <param name="RelativeBase"></param>
        /// <returns></returns>
        public Box MaxBox(Base RelativeBase)
        {
            Box result = Box.ResetBox();

            for (int i = 0; i < Count; i++)
                result = result.GetMaxBox(RelativeBase, this[i]);
            return result;
        }
        /// <summary>
        /// The method transforms every point in the array by the matrix m and returns these points in an array.
        /// </summary>
        /// <param name="m">Transformmatrix</param>
        /// <returns></returns>
        public xyzArray TransformEx(Matrix m)
        {
            xyzArray result = new xyzArray(Count);

            for (int i = 0; i < Count; i++)
            { result[i] = this[i].mul(m); }
            return result;
        }

        /// <summary>
        /// Returns a plane, which contains the 
        /// the first three different points. If they doesn`t exist, null will be returned.
        /// </summary>
        /// <returns>Returns a plane through the first three points</returns>
        public Plane GetPlane()
        {  // ersetzen durche Base!!!!!!!!!!!!!!!!!!!!!
            if (Count < 3) return null;

            xyz A, B, C;
            int i;
            A = this[0];
            i = 1; while ((i < Count) && (this[i].Equals(A))) i++;
            if (i < Count) B = this[i]; else return null;

            i++; while ((i < Count) && ((this[i].sub(A).cross(B.sub(A))).length() < 0.0001)) i++;
            if (i < Count) C = this[i]; else return new Plane(new xyz(0, 0, 0), new xyz(0, 0, 1));
            return new Plane(A, C.sub(A).cross(B.sub(A)).normalized());

        }
        /// <summary>
        /// Inverts the array. The first entry will be the last and so on.
        /// </summary>

        public void Invert()
        {

            xyz pivot;
            for (int i = 0; i <= (Count - 1) / 2; i++)
            {
                pivot = this[i];
                this[i] = this[Count - 1 - i];
                this[Count - 1 - i] = pivot;

            }

        }

        /// <summary>
        /// This method calculates the distance of the array to a line only in case, if the distance is
        /// smaller than MaxDist., otherwise Utils.big will be returned.
        /// Imagine a cylinder with radius MaxDist around the polygon.
        /// If the line goes through the
        /// one of the cylinders, 
        /// this line will be taken and the distance will be calculated and returned.
        /// </summary>
        /// <param name="L">The line, which will be tested</param>
        /// <param name="MaxDist">The maximal distance, for which a reasonable result can be returned.</param>
        /// <param name="param">A value for which the nearset point in the array can be evaluated with <see cref="Value"/></param>
        /// <param name="LineLam">A value for which the nearset point on the line can evaluated</param>
        /// <returns>In case the distance of the line is smaller than Maxdist, the distance is returned. Otherwise <see cref="Utils.big"/>
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
            for (i = 0; i < Count - 1; i++)
            {
                if (this[i + 1].dist(this[i]) < 0.00000001)
                    continue;
                LineType _L = new LineType(this[i], this[i + 1] - this[i]);
                
                di = _L.Distance(L, MaxDist, out _mue, out _lam);

                if (!Utils.Less(MaxDist, di) && (Utils.Less(di, result)) && (-0.001 < _mue) && (_mue < 1.0001))
                {
                    result = di;
                    param = i + _mue;
                    LineLam = _lam;
                }
            }
            return result;

        }
        double MinX()
        {
            double Result = 1e10;
            for (int i = 0; i < Count; i++)
                Result = System.Math.Min(Result, this[i].x);
            return Result;
        }
        double MinY()
        {
            double Result = 1e10;
            for (int i = 0; i < Count; i++)
                Result = System.Math.Min(Result, this[i].y);
            return Result;
        }
        double MaxX()
        {
            double Result = -1e10;
            for (int i = 0; i < Count; i++)
                Result = System.Math.Min(Result, this[i].y);
            return Result;
        }
        double MaxY()
        {
            double Result = -1e10;
            for (int i = 0; i < Count; i++)
                Result = System.Math.Min(Result, this[i].x);
            return Result;
        }
      
        /// <summary>
        /// Calculates the distance to a Point, if this distance is smaller than MaxDist. Otherwise <see cref="Utils.big"/> will be returned.
        /// </summary>
        /// <param name="Point">A Point</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="LineLam">A value for which the nearest point in the array can evaluated with <see cref="Value"/></param>
        /// <returns>Distance.</returns>

        public double Distance(xyz Point, double MaxDist, out double LineLam)
        {
            double result = Utils.big;
            xyz Dummy;
            int i;
            double di, _lam;
            LineType L2;

            LineLam = -1;
            for (i = 1; i < Count; i++)
            {

                L2 = new LineType(this[i - 1], this[i].sub(this[i - 1]));

                di = L2.Distance(Point, out _lam, out Dummy);

                if (!Utils.Less(_lam, 0) && !Utils.Less(1, _lam))
                {
                    if (!Utils.Less(MaxDist, di) && (Utils.Less(di, result)))
                    {

                        result = di;

                        if (Utils.Equals(_lam, 1)) LineLam = i;
                        else
                            LineLam = i - 1 + _lam;
                    }
                }
                else
                {
                    double _di = Point.dist(this[i]);
                    if (!Utils.Less(MaxDist, _di) && (Utils.Less(_di, result)))
                    {

                        result = _di;
                        LineLam = i;
                    }
                    if (i == 1)
                    {
                        _di = Point.dist(this[0]);
                        if (!Utils.Less(MaxDist, _di) && (Utils.Less(_di, result)))
                        {

                            result = _di;
                            LineLam = 0;
                        }

                    }


                }




            }

            return result;

        }
        /// <summary>
        /// Calculates the distance to an other xyzArray A, if this distance is smaller than MaxDist. Otherwise <see cref="Utils.big"/> will be returned.
        /// </summary>
        /// <param name="A">An other xyzArray</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="param">A value for which the nearest point in the array can evaluated with <see cref="Value"/></param>
        /// <param name="paramA">A value for which the nearest point in the array A can evaluated with <see cref="Value"/></param>
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
        /// <summary>
        /// Converts a given length to a parameter, which can be used as param for the <see cref="Value"/>-method.
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="Le">A given length</param>
        /// <returns>a parameter</returns>
        public double LengthToParam(double Le)
        {
            if (!Utils.Less(Le, ParamToLength(Count))) return Count;
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
        /// Calculates the length of the array form 0 until the parameter given by Param.
        /// For a Param = Count you get the full length of the array
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="Param">A givent param</param>
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

    }

}
