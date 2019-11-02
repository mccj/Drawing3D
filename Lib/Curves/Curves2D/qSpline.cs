using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.



namespace Drawing3d
{

    /// <summary>
    /// QSpline represents a quadratic spline which has the three control points <see cref="Curve.A"/>, <see cref="Curve.B"/>
    /// and <see cref="ControlPoint"/>. Additionally it holds a <see cref="weight"/>, which represents the
    /// attractivity of the <see cref="ControlPoint"/>. By default, the weight is set to 1.
    /// 
    /// </summary>
    [Serializable]
    public class QSpline : Nurbs
    {
        /// <summary>
        /// overrides this method and returns 2.
        /// </summary>
        /// <returns>2</returns>
        public override int GetDegree()
        {
            return 2;
        }

        /// <summary>
        /// overrides this method and returns  new xy[] { A, ControlPoint, B }.
        /// </summary>
        /// <returns> new xy[] { A, ControlPoint, B }</returns>
        public override xy[] getCtrlPoints()
        {
            return new xy[] { A, ControlPoint, B };
        }
        /// <summary>
        /// overrides this method and returns { 0, 0, 0, 1, 1, 1 }
        /// </summary>
        /// <returns>{ 0, 0, 0, 1, 1, 1 }</returns>
        public override double[] getKnots()
        {
            return new double[] { 0, 0, 0, 1, 1, 1 };
        }
        /// <summary>
        /// overrides this method and returns { 1, Weight, 1 }
        /// </summary>
        /// <returns>{ 1, Weight, 1 }</returns>
        public override double[] getWeights()
        {
            return new double[] { 1, Weight, 1 };
        }
       
        private xy a, b;
       
       
        
        private double weight = 1;
        /// <summary>
        /// The standard constructor initializes all points to (0/0) and the weight to 1;
        /// </summary>
        public QSpline()
        {
            A = B = new xy(0, 0);
           Dirty = true;

        }
        /// <summary>
        /// This constructor initializes by two points A and B and a Controlpoint.
        /// The weight is set to 1.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// <param name="ControlPoint">ControlPoint</param>
        public QSpline(xy A, xy B, xy ControlPoint) : this()
        {

            this.A = A;
            this.B = B;
            this.ControlPoint = ControlPoint;


        }
        /// <summary>
        /// This constructor initializes by three Points A, B and Controlpoint and additionally the weight in the Controlpoint
        /// The weight is set to 1.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// /// <param name="Weight">Wight of the Controlpoint</param>
        /// <param name="ControlPoint">ControlPoint</param>
        public QSpline(xy A, xy B, xy ControlPoint, double Weight) : this()
        {
            this.A = A;
            this.B = B;
            this.ControlPoint = ControlPoint;
            this.weight = Weight;

        }

        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, which transforms the Points A, B and the ControlPoint.
        /// </summary>
        /// <param name="m">Holds the Transformmatrix</param>
        public override void Transform(Matrix3x3 m)
        {
            A = A.mul(m);
            B = B.mul(m);
            ControlPoint = ControlPoint.mul(m);
          
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Invert"/>-method.
        /// </summary>

        public override void Invert()
        {
            xy dummy = A;
            A = B;
            B = dummy;

            Dirty = true;
        }

        /// <summary>
        /// Overrides the abstract <see cref="Curve.setA"/>-method and sets A in a local value.
        /// </summary>
        /// <param name="value">Point A</param>
        protected override void setA(xy value)
        {
            a = value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the method <see cref="Curve.getA"/>.
        /// </summary>
        /// <returns>Value of A</returns>
        protected override xy getA()
        {
            return a;
        }
        /// <summary>
        /// Overrides the <see cref="Derivation"/>-method and retrieves the <see cref="Utils.funQSplineDerive"/>-function.
        /// </summary>
        /// <param name="t">Parameter for the function</param>
        /// <returns>Derivation value for the prameter t</returns>
        public override xy Derivation(double t)
        {
            return Utils.funQSplineDerive(A, B, this.ControlPoint, Weight, t);
        }
        /// <summary>
        /// Overrides the <see cref="Value"/>-method and retrieves the <see cref="Utils.funQSpline"/>-funcion.
        /// </summary>
        /// <param name="t">Parmeter</param>
        /// <returns>Function value for the parameter t</returns>
        public override xy Value(double t)
        {
            return Utils.funQSpline(A, B, this._controlpoint, weight, t);
        }
        private bool atangSetted = false;
        private xy _At = new xy(0, 0);
        private bool btangSetted = false;
        private xy _Bt = new xy(0, 0);
        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method
        /// </summary>
        /// <param name="value">starttangent</param>
        protected override void setAtang(xy value)
        {
            _At = value;
            atangSetted = true;
            if (btangSetted)
            {
                double lam, mue;
                LineType2d l1 = new LineType2d(A, value);
                LineType2d l2 = new LineType2d(B, _Bt);
                if (l1.Cross(l2, out lam, out mue))
                {

                    ControlPoint = l1.Value(lam);

                }
                Dirty = true;
            }
        }
        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/>-method
        /// </summary>
        /// <param name="value">endtangent</param>

        protected override void setBtang(xy value)
        {
            _Bt = value;
            btangSetted = true;
            double lam, mue;
            if (atangSetted)
            {
                LineType2d l1 = new LineType2d(A, _At);
                LineType2d l2 = new LineType2d(B, value);
                if (l1.Cross(l2, out lam, out mue))
                {

                    ControlPoint = l1.Value(lam);

                }

                Dirty = true;
            }
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-method.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public override void Slice(double from, double to)
        {
            xy T1 = Derivation(from).normalize();
            xy T2 = Derivation(to).normalize();
            xy _A = Value(from);
            xy _B = Value(to);
            if ((System.Math.Abs(T1 & T2) < 0.001)
                ||
               (System.Math.Abs(from - to) < 0.001))
            {

                ControlPoint = (_A + _B) * 0.5;
                A = _A;
                B = _B;
                Dirty = true;
                return;
            }
            LineType2d L1 = new LineType2d(_A, Derivation(from));
            LineType2d L2 = new LineType2d(_B, Derivation(to));
            double Lam = -1;
            double Mue = -1;
            L1.Cross(L2, out Lam, out Mue);
            xy Halbierung = (_A + _B) * 0.5;
            xy P1 = Value((from + to) * 0.5);
            xy _ControlPoint = L1.Value(Lam);
            double a = (P1 - Halbierung).length();
            double b = (_ControlPoint - Halbierung).length();

            Weight = a / (b - a);
            if (Weight < 0)
            {
            }
            A = _A;
            B = _B;
            ControlPoint = _ControlPoint;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/>-method.
        /// </summary>
        protected override xy getAtang()
        {
            return ControlPoint - A;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getBtang"/>-method.
        /// </summary>

        protected override xy getBtang()
        {
            return B - ControlPoint;
        }
   
        /// <summary>
        /// Overrides the method <see cref="Curve.getB"/>.
        /// </summary>
        /// <returns>Value of B</returns>

        protected override xy getB()
        {
            return b;
        }
        /// <summary>
        /// Overrides the method <see cref="Curve.setB"/>.
        /// </summary>
        ///
        /// <param name="value">Value of EndPoint B</param>
        protected override void setB(xy value)
        {
            b = value;
            Dirty = true;
        }
        private xy _controlpoint = new xy(0, 0);
        /// <summary>
        /// Sets and gets the control point of the QSpline.
        /// </summary>
        public xy ControlPoint
        {
            get { return _controlpoint; }
            set { _controlpoint = value; Dirty = true;
              
                }
        }
        /// <summary>
        /// Sets and gets the attraction of the control point of the QSpline.
        /// </summary>
        public double Weight
        {
            get { return weight; }
            set { weight = value;  Dirty = true; }
        }

    }
}
