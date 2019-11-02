using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Drawing3d
{
    /// <summary>
    /// represents a two-dimensional parameterized mathematical curve. It is the base object for all twodimensionally curves.The
    /// parameter is restricted to the interval [0, 1].
    /// To define a curve you have to override the abstract Methods <see cref="Value"/> and <see cref="Derivation"/>
    /// If this is done, a curve can be drawn in a device by the method drawCurve(Curve) of a device <see cref="Drawing3d.OpenGlDevice"/>.
    /// Additionally a curve has a resolution. This is used by the method <see cref="ToArray"/>, which retrieves an array with
    /// resolution+1 points. By default she is 20 and can be setted by the static field <see cref="DefaultResolution"/>
    /// </summary>
    [Serializable]
    public abstract class Curve : ITransform2d
    {       
        /// <summary>
        /// sets or gets the default resolution for all curves.
        /// </summary>
        public static int DefaultResolution = 20;
        private int _Resolution = DefaultResolution;
        /// <summary>
        /// Produces an exact copy of the curve by using the <see cref="BinaryFormatter"/>.
        /// So you have to mark a new instance of Curve with the attribute [Serializable].
        /// </summary>
        /// <returns>the copied curve.</returns>
        public virtual Curve Clone()
        {


            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                System.IO.MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                Curve result = formatter.Deserialize(stream) as Curve;
                stream.Close();
                return result;
            }
            catch (Exception E)
            {

                System.Windows.Forms.MessageBox.Show(E.Message);
            }
            return null;
        }
        
        private double _fromParam = 0;
        private double _toparam = 1;
        /// <summary>
        /// Retrieves and sets the start parameter. All parameters are within the interval [fromParam, toParam].
        /// Initial value is 0;
        /// </summary>
       double fromParam
        {
            get { return _fromParam; }
          
        }

        /// <summary>
        /// Retrieves and sets the end parameter. All parameters are within the interval [fromParam, toParam].
        /// Initial value is 1;
        /// </summary>
        double toParam
        {
            get { return _toparam; }
       
        }
      


        /// <summary>
        /// Indicates, that the curve is changed
        /// A call to <see cref="MakeClean()"/> sets dirty to false.
        /// In this method an Array of <see cref="Resolution"/>+1 point
        /// will be created and returned, when a request <see cref="ToXYArray"/> 
        /// is given.
        /// </summary>
        [NonSerialized]
        public bool Dirty = true;
        [NonSerialized]
        private xyArray Data = new xyArray();

        /// <summary>
        /// empty constructor of Curve.
        /// </summary>
        public Curve()
        {
            Dirty = true;
        }
   
     
 
        /// <summary>
        /// Retrieves and sets the resolution of the curve. The default value is 20.
        /// </summary>	
        [BrowsableAttribute(false)]
        public int Resolution
        {
            get { return _Resolution; }
            set
            {
                Dirty = true;
                _Resolution = value;
            }
        }


        /// <summary>
        /// This abstract method must be overridden. It returns the function for a value t.
        /// <code>
        /// public xy Value(double t){
        /// return new xy(cos(t), sin(t));
        /// }
        /// public xy Drivation(double t){
        /// return new xy(-sin(t), cos(t));
        /// }
        /// </code>
        /// This example implements an unit circle.
        /// <seealso cref="Derivation"/>
        /// </summary>
        /// <param name="t">parameter for the function Value between <see cref="fromParam"/> and <see cref="toParam"/>.</param>
        /// <returns>Value of the curvedefining function</returns>
       public abstract xy Value(double t);

       
        /// <summary>
        /// This abstract method must be overridden. It returns the first deriavation of the function defined in the
        /// method <see cref="Value"/>. All parameter t are taken from the interval [<see cref="fromParam"/>,<see cref="toParam"/>].
        /// 
        /// </summary>
        /// 
        /// <param name="t">parameter for the first derivation</param>
        /// <returns>Value of the first derivation of the curvefunction</returns>
        public abstract xy Derivation(double t);
       

        /// <summary>
        /// Converts a length to a param, which can be used in the method <see cref="Value"/>.
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="le">A length.</param>
        /// <returns>A param for using in <see cref="Value"/>. Rem: if le is greater then the lenght of the curve a value -1 is returned.</returns>
        public double LengthToParam(double le)
        {
            //if ( le > CurveLength) return toParam;
            xyArray a = new xyArray(Resolution + 1);
            this.ToArray(a, 0);
            double r = a.LengthToParam(le);
            if (r < 0) return r;
            return fromParam + toParam * a.LengthToParam(le) / Resolution;
        }
        private  RectangleF RMAx = new RectangleF();
       
     
        /// <summary>
        /// Tag is a free programmable property
        /// </summary>
        [BrowsableAttribute(false)]
        [NonSerialized]
        public object Tag;
   
        /// <summary>
        /// Gets the smaller rectangle, which contains the curve.
        /// </summary>
        /// <returns>A rectangle</returns>
        public virtual RectangleF GetMaxrect()
        {
            xyArray a = new xyArray(Resolution + 1);
            this.ToArray(a, 0);
            return a.Maxrect;

        }
        /// <summary>
        /// A property, which calls <see cref="GetMaxrect()"/>
        /// </summary>
        public RectangleF Maxrect
        {
            get { return GetMaxrect(); }
        }
        /// <summary>
		/// This is the virtual Get-Method of <see cref="A"/>. By default it calls 'Value(fromParam)'.
		/// </summary>
		/// <returns>StartPoint</returns>
		protected virtual xy getA()
        {
            return Value(0);
        }
        /// <summary>
        /// This method is the getMethod of the <see cref="Atang"/>-property, which retrieves the
        /// start tangent. By default Derivation(fromParam) is returned.
        /// </summary>
        /// <returns>Starttangent</returns>
        protected virtual xy getAtang()
        {
            return Derivation(0);
        }
        /// <summary>
        /// This method is the getMethod of the <see cref="Btang"/>-property, which retrieves the
        /// Endtangent. By default Derivaion(toParam) is returned.
        /// </summary>
        /// <returns>Starttangent</returns>

        protected virtual xy getBtang()
        {
            return Derivation(1);
        }
        
        /// <summary>
        /// This is the virtual Get-Method of <see cref="B"/>. By default, it provides 'Value(toParam)'.
        /// </summary>
        /// <returns>Endpoint</returns>
        protected virtual xy getB()
        {
            return Value(1);
        }
        /// <summary>
        /// Gets an array of Resolution+1 interpolationpoints.
        /// <seealso cref="Resolution"/> <seealso cref="ToArray"/>
        /// </summary>
        /// <returns></returns>
        public xyArray ToXYArray()
        {
            xyArray Result = new xyArray(Resolution + 1);
            ToArray(Result, 0);
            return Result;
        }
        /// <summary>
        /// Crosses the Curve with an other curve by crossing the interpolating polygonarray.
        /// <seealso cref="Cross(Curve,double, double , out double , out double)"/>
        /// </summary>
        /// <param name="Curve">The other Curve</param>
        /// <param name="lam">Parameter to evaluate with the value method ( this.value(lam))</param>
        /// <param name="mue">>Parameter to evaluate with the value method of Curve ( Curve.value(mue))</param>
        /// <returns>True if there is a crosspoint else false.</returns>
        public bool Cross(Curve Curve, out double lam, out double mue)
        {
            lam = 0;
            mue = 0;

            xyArray A = new xyArray(Resolution + 1);
            ToArray(A, 0);
            xyArray B = new xyArray(Curve.Resolution + 1);
            ;
            Curve.ToArray(B, 0);
            A.RemoveZeros();
            B.RemoveZeros();
            for (int i = 0; i < A.Count - 1; i++)
            {
                LineType2d L1 = new LineType2d(A[i], A[i + 1] - A[i]);
                if (!L1.Direction.Equals(new xy(0, 0)))
                    for (int j = 0; j < B.Count - 1; j++)
                    {

                        LineType2d L2 = new LineType2d(B[j], B[j + 1] - B[j]);
                        bool solution = L1.Cross(L2, out lam, out mue);
                        if (solution)
                        {
                            if ((lam >= -0.000001) &&
                                 (lam <= 1.000001) &&
                                 (mue >= -0.00001) &&
                                 (mue <= 1.000001))
                            {
                                solution = L1.Cross(L2, out lam, out mue);
                                lam = (i + lam) / (float)Resolution;
                                mue = (j + mue) / (float)Curve.Resolution;

                                return true;
                            }
                        }
                    }
            }
            lam = -1;
            mue = -1;
            return false;
       }
        /// <summary>
        /// Gets the intersection with another curve, which is calculated by iteration.
        /// <seealso cref="Cross(Curve , out double, out double)"/>
        /// </summary>
        /// <param name="Curve">The other curve</param>
        /// <param name="InLam">A start parameter for this curve which is "near" to the solution</param>
        /// <param name="InMue">A start parameter for the other curve which is "near" to the solution</param>
        /// <param name="NearLam">Solution parameter for this curve</param>
        /// <param name="NearMue">Solution parameter for the other curve</param>
        /// <returns>true if there is a crosspoint else false.</returns>
        public bool Cross(Curve Curve, double InLam, double InMue, out double NearLam, out double NearMue)
        {
            NearLam = InLam;
            NearMue = InMue;

            double _nearLam;
            double _nearMue;
            if (Double.IsNaN(Curve.Value(0).x)) return false;

            if (Double.IsNaN(Value(0).x)) return false;
            double di1 = (float)1 / (float)Resolution;
            double di2 = (float)1 / (float)Curve.Resolution;
            xy Q1;
            xy Q2;
            bool weiter = false;
            xy _A = Value(NearLam - di1 / 2);
            xy _B = Value(NearLam + di1 / 2);
            xy C = Curve.Value(NearMue - di2 / 2);
            xy D = Curve.Value(NearMue + di2 / 2);
            LineType2d L1 = new LineType2d(_A, _B - _A);
            LineType2d L2 = new LineType2d(C, D - C);

            if (L1.Cross(L2, out _nearLam, out _nearMue))
            {
                NearLam = NearLam - di1 + _nearLam * di1;
                NearMue = NearMue - di2 + _nearLam * di2;
            }

            bool Left1 = false;
            bool Left2 = false;

            do
            {
                di1 = di1 / 2;
                di2 = di2 / 2;
                if ((NearLam / di1 - Utils.trunc(NearLam / di1)) > 0.5)
                {
                    Left1 = false;
                    _A = Value(NearLam);
                    _B = Value(NearLam + di1);
                }
                else
                {
                    Left1 = true;
                    _A = Value(NearLam - di1);
                    _B = Value(NearLam);

                }
                L1 = new LineType2d(_A, _B - _A);

                if (NearMue / di2 - Utils.trunc(NearMue / di2) > 0.5)
                {
                    Left2 = false;
                    C = Curve.Value(NearMue);
                    D = Curve.Value(NearMue + di2);
                }
                else
                {
                    Left2 = true;
                    C = Curve.Value(NearMue - di2);
                    D = Curve.Value(NearMue);
                }
                L2 = new LineType2d(C, D - C);

                weiter = false;
                if (L1.Cross(L2, out _nearLam, out _nearMue))
                {
                    if (Left1)
                        NearLam = NearLam - di1 + _nearLam * di1;
                    else
                        NearLam = NearLam + _nearLam * di1;

                    if (Left2)
                        NearMue = NearMue - di2 + _nearMue * di2;
                    else
                        NearMue = NearMue + _nearMue * di2;

                    weiter = true;
                }
                Q1 = Value(NearLam);
                Q2 = Curve.Value(NearMue);
            }
            while ((weiter) && (!Q1.Equals(Q2)));
            return (Q1.Equals(Q2) && (!Utils.Less(NearLam, 0)) && (!Utils.Less(1, NearLam)) &&
                (System.Math.Abs(InMue - NearMue) < 0.2) && (System.Math.Abs(InLam - NearLam) < 0.2) && (!Utils.Less(NearMue, 0)) && (!Utils.Less(1, NearMue)));

        }

        /// <summary>
        ///
        /// This method calculates the distance of a LineType to this Curve only in case, when the distance is
        /// smaller then MaxDist, otherwise <see cref="Utils.big"/> will be returned.
        /// You can imagine a cylinders with radius MaxDist around the Curve. If the line goes through the
        /// "curved" cylinder, then the distance will be calculated and returned.
        /// </summary>
        /// <param name="L">Line, which will be checked</param>
        /// <param name="MaxDist">Maximal distance</param>
        /// <param name="param">paramter fo the nearest point in the curve</param>
        /// <returns>distance</returns>
        public double Distance(LineType L, double MaxDist, out double param)
        {
            double lam;
            double result = Utils.big;
            xyArray a = new xyArray(Resolution + 1);
            this.ToArray(a, 0);
            double di = a.Distance(L, MaxDist, out param, out lam);
            if (!Utils.Less(MaxDist, di))
            {
                param = this.fromParam + param * toParam / Resolution;
                result = di;
            }
            return result;
        }
        /// <summary>
        /// Gets the distance to a line.
        /// </summary>
        /// <param name="L">A line</param>
        /// <param name="MaxDist">maximal distance</param>
        /// <param name="param">Parameter relative to this curve</param>
        /// <param name="lam">Prameter relative to L</param>
        /// <returns>The distance</returns>
        public double Distance(LineType L, double MaxDist, out double param, out double lam)
        {

            double result = Utils.big;
            xyArray a = new xyArray(Resolution + 1);
            this.ToArray(a, 0);
            double di = a.Distance(L, MaxDist, out param, out lam);

            xyz P = L.Value(lam);
            xyz Q = Value(param).toXYZ();
            double T = P.dist(Q);
            if (!Utils.Less(MaxDist, di))
            {
                param = this.fromParam + param * toParam / Resolution;
                result = di;
            }
            return result;
        }
        /// <summary>
        /// Returns a  leftside parallel xyArray to the curve, which has a distance of width.
        /// </summary>
        /// <param name="width">Distance of the parallel</param>
        /// <returns>a parallel <see cref="xyArray"/></returns>
        public virtual xyArray Parallel(double width)
        {
            xyArray result = new xyArray(Resolution + 1);
            for (int i = 0; i <= Resolution; i++)
            {
                double param = fromParam + i * (toParam - fromParam) / Resolution;
                result[i] = Value(param) + Derivation(param).normal().normalize() * width;
            }
            return result;

        }
        // Idee in Arc implementiert
        //public virtual Curve GetTransform(Matrix3x3 m)
        //{
        //    return null;
        //}
        /// <summary>
        /// Transforms a Curve with the transformation given by m.
        /// The method in the base class is empty. So you have to override this
        /// function, if you want to transform a Curve.
        /// </summary>
        /// <param name="m"></param>
        public virtual void Transform(Matrix3x3 m)
        { }


        /// <summary>
        /// Calculates the length of a part of the curve, which is given from 0 to the value param.
        /// <seealso cref="LengthToParam"/>
        /// </summary>
        /// <param name="Param">Param until which the length is calculated</param>
        /// <returns>Returns the length of the curve</returns>
        public double ParamToLength(double Param)
        {
            if (Param >= toParam) Param = toParam;
            xyArray a = new xyArray(Resolution + 1);
            this.ToArray(a, 0);
            return a.ParamToLength((Param - fromParam) * Resolution);
        }

        /// <summary>
        /// Retrieves the length of the curve.
        /// </summary>
        public double CurveLength
        {
            get { return ParamToLength(toParam); }
        }
        /// <summary>
        /// calculates the arc length from the start point of the curve to the normal projection of a point <b>P</b> to the curve.
        /// The result is nomalized. It means for the entire curve you get 1. If you want the real length you must it multiply by the resolution of the curve.
        /// </summary>
        /// <param name="P">the point, which will normalprojected.</param>
        /// <returns></returns>
        public double Arcus(xy P)
        {
            if (Dirty) MakeClean();

            //xyArray Points = new xyArray(Curve.Resolution + 1);
          //  Curve.ToArray(Points, 0);
            int j = 0;
            double a = 0;
            while ((true) && (j <Resolution))
            {
                a = (P - Data[j]) * (Data[j + 1] - Data[j]).normalize();
              
                if (a <= (Data[j + 1] - Data[j]).length()) break;
               

                       
                if (j == Resolution + 1) return 0;
                j++;
            }
            if (j >= Resolution)
                return 1;
               
            return (j + a / (Data[j + 1] - Data[j]).length()) / (float)Resolution;
       
        }
        private void MakeClean()
        {
            Data = new xyArray(Resolution + 1);
            for (int i = 0; i <= Resolution; i++)
            {
                double t = fromParam + (toParam - fromParam) * i / (float)Resolution;
                xy p = Value(t);

                Data[i] = p;
            }
            Dirty = false;
            RMAx = Data.GetMaxrect(Matrix.identity);
        }
        /// <summary>
        /// This method fills values, calculated by the function Value in an array, starting at index.
        /// </summary>
        /// <param name="xyArray">Array for storing the values of the curve</param>
        /// <param name="index">startindex, from which the values are stored</param>
        /// <remarks>The array must be initialized until index + resolution + 1</remarks>

        public virtual void ToArray(xyArray xyArray, int index)
        {

            if (Dirty || (Data == null))
            {

                MakeClean();
              
                Data.data.CopyTo(xyArray.data, index);
            }
            else

                if (Data.data.Length > xyArray.data.Length)
            {
                xyArray.data = new xy[Data.data.Length];
                MakeClean();
                Data.data.CopyTo(xyArray.data, index);
            }
            else
                Data.data.CopyTo(xyArray.data, index);
        }
     
        /// <summary>
        /// This method trims the curve to the part between "from" and "to".
        /// The base-method sets <see cref="fromParam"/> = from and <see cref="toParam"/> = to.
        /// In general this is not the best solution. So it is better to override this method without
        /// calling to the base method.
        /// </summary>
        /// <param name="from">start of the new trimmed curve</param>
        /// <param name="to">start of the new trimmed curve</param>
        public virtual void Slice(double from, double to)
        {
            _fromParam = from;
            _toparam = to;
        }
        /// <summary>
        /// Get a <see cref="CrossList"/>, which contains the crossing points with
        /// another curve?. The parameter param1 and param2 in <see cref="CrossItem"/> are converted to
        /// a curve parameter.
        /// </summary>
        /// <param name="Curve"></param>
        /// <returns></returns>
        public CrossList GetCrossList(Curve Curve)
        {
            xyArray xyArray1 = new xyArray(this.Resolution + 1);
            xyArray xyArray2 = new xyArray(Curve.Resolution + 1);

            this.ToArray(xyArray1, 0);
            Curve.ToArray(xyArray2, 0);
            CrossList Result = xyArray1.getCrossList(xyArray2, true);
            for (int i = 0; i < Result.Count; i++)
            {
                CrossItem C = Result[i];
                C.Param1 = C.Param1 / this.Resolution;
                C.Param2 = C.Param2 / Curve.Resolution;
            }
            return Result;
        }





        /// <summary>
        /// This method is the setMethod of the <see cref="Atang"/>-property, which retrieves the
        /// starttangent. 
        /// This method is an abstract one and has to be overridden.
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected virtual void setAtang(xy value) { }
        /// <summary>
        /// This method is the setMethod of the <see cref="Btang"/>-property, which retrieves the
        /// end tangent. This method is abstract and has to be overridden.
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected virtual void setBtang(xy value) { }
        /// <summary>
        /// Inverts the direction of the curve by exchange A and B resp. Atang and Btang
        /// </summary>
        public virtual void Invert()
        {
            xy Save;
            Save = A;
            A = B;
            B = Save;
            Save = Atang * (-1);
            Atang = Btang * (-1);
            Btang = Save;
        }

        /// <summary>
        /// This method is the setMethod of the <see cref="A"/>-property, which retrieves the
        /// StartPoint. This method is abstract and has to be overridden.
        /// </summary>
        /// <param name="value">Startpoint of the curve</param>
        protected virtual void setA(xy value) { }
        /// <summary>
        /// This method is the setMethod of the <see cref="B"/>-property, which retrieves the
        /// EndPoint. This method is abstract and must be overridden.
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void setB(xy value) { }
        /// <summary>
        /// Returns and sets the StartPoint.See <see cref="Curve.getA"/> and <see cref="setA"/>
        /// </summary>
        public xy A
        {
            get { return getA(); }
            set { setA(value); }

        }
        /// <summary>
        /// Returns and sets the EndPoint.See <see cref="Curve.getB"/> and <see cref="setB"/>
        /// </summary>

        public xy B
        {
            get { return getB(); }
            set { setB(value); }

        }
        /// <summary>
        /// Returns and sets the starttangent. See <see cref="Curve.getAtang"/>, <see cref="setAtang"/> and <see cref="Btang"/>
        /// </summary>

        public xy Atang
        {
            get { return getAtang(); }
            set { setAtang(value);
            }

        }
    
        /// <summary>
        /// Returns and sets the endtangent. See <see cref="Curve.getBtang"/>, <see cref="setBtang"/> and <see cref="Atang"/>
        /// </summary>
        public xy Btang
        {
            get { return getBtang(); }
            set { setBtang(value);  }
        }
    }
}
