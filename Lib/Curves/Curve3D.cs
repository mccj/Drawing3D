using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{

    /// <summary>
    /// Liste von <see cref="Curve3D"/>
    /// </summary>
    [Serializable]
    public class CurveList3D : List<Curve3D>
    {
    }
    /// <summary>
    /// Liste von <see cref="CurveList3D"/>
    /// </summary>
    [Serializable]
    public class Loca3D : List<CurveList3D>
    {
    }
    /// <summary>
    /// The class Curve3D represents a thre-dimensional parameterized mathematical curve. The
    /// parameter is restricted to the interval [0,1]
    /// To define your own curve you have to override the Methods <see cref="Value"/> and <see cref="Derivation"/>
    /// If this is done, a curve can be drawn in a device by the method drawCurve of <see cref="OpenGlDevice"/>.
    /// Additionally a curve has a resolution. This is used by the method <see cref="ToArray"/>, which retrieves an array with
    /// Resolution+1 points.
    /// </summary>
    [Serializable]
    public abstract class Curve3D : object, ITransform
    {
      
      
       
     
        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public Curve3D()
        {
           
        }
        /// <summary>
        /// Copy the Curve. Some instances override this method.
        /// </summary>
        /// <returns>A copy of the curve.</returns>
        public virtual Curve3D Copy()
        {


            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                System.IO.MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                Curve3D result = formatter.Deserialize(stream) as Curve3D;
                stream.Close();
                return result;
            }
            catch (Exception E)
            {

                System.Windows.Forms.MessageBox.Show(E.Message);
            }
            return null;
        }

        
       // [NonSerialized]
        object[] _Neighbours = new object[] { null, null };
        /// <summary>
        /// If a Curve3D is a contur curve of a <see cref="Face"/> this
        /// array contains both Neighbors. In generally this are two.
        /// </summary>
        public object[] Neighbors
        {
            get { return _Neighbours; }
            set { _Neighbours = value; }
        }
        
      
        
        private double _toParam = 1;
        [NonSerialized]
        private bool _Closed;
       

        double _FromParam = 0;
     
       
        /// <summary>
        /// Calculates a point of a Curve, which is nearest to "Point"
        /// </summary>
        /// <param name="Point">Point, which will be projected</param>
        /// <param name="Lambda">Evaluation parameter for <see cref="Value"/> to get the nearest point on the curve.</param>
        /// <returns></returns>
        public virtual bool NormalCross(xyz Point, ref double Lambda)
        {
            // Find first with Normal* (Point-Curve)  <0

            Lambda = -1;
            int Start = -1;
            for (int i = 0; i <= Resolution; i++)
            {
                double t = (float)i / (float)Resolution;
                xyz p = Point - Value(t);
                xyz d = Derivation(t);
                if (System.Math.Abs(d * p) < 0.001)
                {
                    Lambda = t;
                    return true;

                }
                if ((d * p) >= 0)
                {
                    Start = i;
                    break;
                }
            }
            // Find next with tangent* (Point-Curve)  >0
            if (Start == -1) return false;
            int End = -1;
            for (int i = Start; i <= Resolution; i++)
            {
                double t = (float)i / (float)Resolution;
                xyz p = Point - Value(t);
                xyz d = Derivation(t);
                if (System.Math.Abs(d * p) < 0.001)
                {
                    Lambda = t;
                    return true;

                }
                if ((d * p) <= 0)
                {
                    End = i;

                    break;
                }
            }
            if (End == -1) return false;
            double _End = (float)End / (float)Resolution;
            double _Start = _End - 1f / (float)Resolution;
            double x = Derivation(_Start) * (Point - Value(_Start));
            double y = Derivation(_End) * (Point - Value(_End));
            int IterateCount = 0;
            double Iterate = (_End + _Start) / 2;
            while (((_End - _Start) != 0) && (IterateCount < 40) && (System.Math.Abs
                ((Derivation(Iterate)) * (Point - Value(Iterate))) > 0.00001))
            {
                if (Point.dist(Value(Iterate)) > 1)
                {
                }
                IterateCount++;
                if ((Derivation(Iterate) * (Point - Value(Iterate)) > 0))
                {
                    _Start = Iterate;
                    Iterate = (_Start + _End) / 2;
                }
                else
                {
                    _End = Iterate;
                    Iterate = (_Start + _End) / 2;
                }

            }
            Lambda = Iterate;
            return true;
        }
        /// <summary>
        /// Restricts the Curve to the startpoint A and the endpoint B. The method works regular only if
        /// A and B are "close to the curve".
        /// </summary>
        /// <param name="_A"></param>
        /// <param name="_B"></param>
        public virtual void SetBorder(xyz _A, xyz _B)
        {
         
            
            double Lam = 0;
            _FromParam = 0;
            _toParam = 1;
            xyzArray A = new xyzArray(Resolution + 1);
            ToArray(A, 0);
            A.Distance(_A, 1e10, out Lam);
            double f = System.Math.Round(Lam / Resolution, 5);
         
            A.Distance(_B, 1e10, out Lam);
            double t = System.Math.Round(Lam / Resolution, 5);
            if (System.Math.Abs(f - t) < 0.00000000001)
            {
                //if (_A.dist(_B) <0.000000001)
                //{
                //    t = 1 - f;
                //}
                if (Value(0).dist(Value(1)) < 0.00000001)
                {
                    f = 0.000000000001;
                }
                if (this is Nurbs3d)
                {
                    Nurbs3d BS = this as Nurbs3d;
                    f = BS.Knots[2];
                    t = BS.Knots[BS.Knots.Length - 3];
                }
            }
           _FromParam = f;
            _toParam = t;
           
        }
        /// <summary>
        /// Gets and sets the information, that the curve is closed, ie A = B.
        /// </summary>
       
        public bool Closed
        {
            get { return _Closed; }
            set { SetClosed(value); }
        }
        /// <summary>
        /// Virtual Settermethod of <see cref="Closed"/>-property
        /// </summary>
        /// <param name="value">True or false</param>
        virtual protected void SetClosed(bool value)
        {
            _Closed = value;
        }

        private int _Resolution = 20;
        /// <summary>
        /// Retrieves and sets the resolution of the curve. The default value is 20.
        /// </summary>	
     
        public int Resolution
        {
            get {
                return _Resolution;

                        }
            set { _Resolution = value;
            
               }
        }


        /// <summary>
        /// Its implement a function from R --> R³
        /// <code>
        /// public xyz Value(double t){
        /// return new xyz(cos(t), sin(t),t);
        /// }
        /// public xyz Drivation(double t){
        /// return new xyz(-sin(t), cos(t),1);
        /// }
        /// </code>
        /// This example implements an screw curve.
        /// <seealso cref="Derivation"/>
        /// </summary>
        /// <param name="t">parameter for the function Value</param>
        /// <returns>Value of the curve defining function</returns>
        public abstract xyz Value(double t);
        /// <summary>
        /// It returns the first deriavation of the function defined in the
        /// method <see cref="Value"/>. All parameter t are taken from the interval [0,1].
        /// 
        /// </summary>
        /// 
        /// <param name="t">parameter for the first derivation</param>
        /// <returns>Value of the first derivation of the curvefunction</returns>
        public abstract xyz Derivation(double t);

        /// <summary>
        /// Converts a length to a param, which can be used in the method <see cref="Value"/>.
        /// <seealso cref="ParamToLength"/>
        /// </summary>
        /// <param name="le">A length</param>
        /// <returns>A param for using in <see cref="Value"/></returns>
        public double LengthToParam(double le)
        {
            
            xyzArray a = new xyzArray(Resolution + 1);
            this.ToArray(a, 0);
            return a.LengthToParam(le) / Resolution;
        }

     
      
        /// <summary>
        /// Tag is a free programmable property
        /// </summary>
      
        public object Tag;
      
        /// <summary>
        /// Gets the smaller rectangle, which contains the curve.
        /// </summary>
        /// <returns>A rectangle</returns>
        protected virtual Box GetMaxrect()
        {
            xyzArray a = new xyzArray(Resolution + 1);
            this.ToArray(a, 0);
            return a.MaxBox();
        }
        /// <summary>
        /// gets the smalleest <see cref="Box"/> containing the curve.
        /// </summary>
        public Box Maxrect
        {
            get { return GetMaxrect(); }
        }
        /// <summary>
        /// This is the virtual Get-Method of <see cref="A"/>. By default it calls 'Value(0)'.
        /// </summary>
        /// <returns>StartPoint</returns>
        protected virtual xyz getA()
        {
            return Value(0);
        }
        /// <summary>
        /// This is the virtual setter method of the property <see cref="A"/>
        /// </summary>
        /// <param name="value">Value of the new startpoint.</param>
        protected virtual void setA(xyz value)
        {

        }

        /// <summary>
        /// This method is the getMethod of the <see cref="Atang"/>-property, which retrieves the
        /// start tangent. By default Derivation(0) is returned.
        /// </summary>
        /// <returns>Starttangent</returns>
        protected virtual xyz getAtang()
        {
            return Derivation(0);
        }
        /// <summary>
        /// This is the virtual setter method of the property <see cref="Atang"/>
        /// </summary>
        /// <param name="value">Value of the new start tangent.</param>
        protected virtual void setAtang(xyz value)
        {

        }
        /// <summary>
        /// This method is the getMethod of the <see cref="Btang"/>-property, which retrieves the
        /// Endtangent. By default Derivaion(1) is returned.
        /// </summary>
        /// <returns>Starttangent</returns>

        protected virtual xyz getBtang()
        {
            return Derivation(1);
        }
        /// <summary>
        /// This is the virtual set method of the property <see cref="B"/>
        /// </summary>
        /// <param name="value">Value of the new endtangent.</param>
        protected virtual void setBtang(xyz value)
        {

        }

        /// <summary>
        /// This is the virtual Get-Method of <see cref="B"/>. By default, it provides 'Value(1)'.
        /// </summary>
        /// <returns>Endpoint</returns>
        protected virtual xyz getB()
        {
            return Value(1);
        }
        /// <summary>
        /// This is the virtual setter method of the property <see cref="B"/>
        /// </summary>
        /// <param name="value">Value of the new endpoint.</param>
        protected virtual void setB(xyz value)
        {

        }
        xyz _A = new xyz(0, 0, 0);
        xyz _B = new xyz(0, 0, 0);
        /// <summary>
        /// Gets the starting point of the curve by calling <see cref="getA"/>.
        /// </summary>

        public xyz A
        {

            get { return _A; }
            set { _A = value; }
        }
        /// <summary>
        ///  Gets the endpoint of the curve by calling <see cref="getB"/>.
        /// </summary>
        public xyz B
        {
            get { return _B; }
            set { _B = value; }

        }
        /// <summary>
        /// Returns and sets the start tangent. See <see cref="getAtang"/> and <see cref="Curve.Btang"/>
        /// </summary>

        public xyz Atang
        {
            get { return getAtang(); }
            set { setAtang(value); }

        }
        /// <summary>
        /// Returns and sets the end tangent. See <see cref="getBtang"/> and <see cref="Curve.Atang"/>.
        /// </summary>
        public xyz Btang
        {
            get { return getBtang(); }
            set { setBtang(value); }

        }
        /// <summary>
        /// Inserts a point at the position, which is given by Param.
        /// </summary>
        /// <param name="Param">Position, where a point will be inserted</param>
        /// <returns>The new part of the curve</returns>
        public Curve3D InsertPoint(double Param)
        {

            if (Utils.Equals(Param, 0)) return null;
            if (Utils.Equals(Param, 1)) return null;
            Curve3D Result = Clone();
            Slice(0, Param);
            Result.Slice(Param, 1);
            return Result;
        }

        /// <summary>
        ///
        /// This method calculates the distance of a LineType to a Curve only in case, when the distance is
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
            xyzArray a = new xyzArray(Resolution + 1);
            this.ToArray(a, 0);
            double di = a.Distance(L, MaxDist, out param, out lam);
            if (!Utils.Less(MaxDist, di))
            {
                param = +param / Resolution;
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
            xyzArray a = new xyzArray(Resolution + 1);
            this.ToArray(a, 0);
            double di = a.Distance(L, MaxDist, out param, out lam);
            if (!Utils.Less(MaxDist, di))
            {
                param = +param / Resolution;
                result = di;
            }
            return result;
        }


        /// <summary>
        /// Transforms a Curve with the transformation given by m.
        /// The method in the base class is empty. So you have to override this
        /// function, if you want to transform a Curve.
        /// </summary>
        /// <param name="m"></param>
        public virtual void Transform(Matrix m)
        {
        }
        /// <summary>
        /// Produces an exact copy of the curve by using the <see cref="BinaryFormatter"/>.
        /// So you have to mark a new instance of Curve with the attribute [Serializable].
        /// 
        /// </summary>
        /// <returns></returns>

        public virtual Curve3D Clone()
        {
          return  Copy();
        

        }

        /// <summary>
        /// Calculates the length of a part of the curve, which is given from 0 to the value param.
        /// <seealso cref="LengthToParam"/>
        /// </summary>
        /// <param name="Param">Param until which the length is calculated</param>
        /// <returns>Returns the length of the curve</returns>
        public double ParamToLength(double Param)
        {
            if (Param >= 1) Param = 1;
            xyzArray a = new xyzArray(Resolution + 1);
            this.ToArray(a, 0);
            return a.ParamToLength((Param) * Resolution);
        }
        /// <summary>
        /// Inverts the orientation of a curve.
        /// The base-method is empty. You should override it in a derived class.
        /// </summary>
        public virtual void Invert()
        {
            xyz C = A;
            setA(B);
            setB(C);

        }
        /// <summary>
        /// Retrieves the length of the curve.
        /// </summary>
        public double CurveLength
        {
            get { return ParamToLength(1); }
        }
        /// <summary>
        /// This method fills values, calculated by the function Value in an array, starting at index.
        /// </summary>
        /// <param name="xyzArray">Array for storing the values of the curve</param>
        /// <param name="index">startindex, from which the values are stored</param>
        /// <remarks>The array must be initialized until index + resolution + 1</remarks>

        public virtual void ToArray(xyzArray xyzArray, int index)
        {

            for (int i = 0; i <= Resolution; i++)
            {
                double t = _FromParam + (_toParam - _FromParam) * i / (float)Resolution;

               
                xyzArray[index + i] = Value(t); 
            }


        }
        /// <summary>
        /// Gets an array of Resolution+1 interpolationpoints.
        /// <seealso cref="Resolution"/> <seealso cref="ToArray"/>
        /// </summary>
        /// <returns>The interpolytion points</returns>
        public virtual xyzArray ToxyzArray()
        {
            xyzArray Result = new xyzArray(Resolution + 1);

            for (int i = 0; i <= Resolution; i++)
            {
                double t = i / (float)Resolution;
                xyz p = Value(t);
                double aaa = Math.Atan2(p.y, p.x);
                Result[i] = new xyz(p.x, p.y, p.z);
            }
            return Result;

        }


        /// <summary>
        /// This method trims the curve to the part between "from" and "to".
        /// Only this part will be drawn.
        /// </summary>
        /// <param name="from">start of the new trimmed curve</param>
        /// <param name="to">start of the new trimmed curve</param>
        public virtual void Slice(double from, double to)
        {
            _FromParam = from; _toParam = to;
        }
    }
 
  
 
   
   
  
    
}
