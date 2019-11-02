using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.





namespace Drawing3d
{ /// <summary>
  /// is an interface, which holds the nurbs elements.
  /// </summary>
    public interface INurbs2d
    {
        /// <summary>
        /// get the degree of the nurbs.
        /// </summary>
        /// <returns></returns>
        int GetDegree();
        /// <summary>
        /// array, which holds the control points.
        /// </summary>
        /// <returns>gets the control points</returns>
        xy[] getCtrlPoints();
        /// <summary>
        /// is a vector with the knots.
        /// </summary>
        /// <returns>get the knot vector</returns>
        double[] getKnots();
        /// <summary>
        /// holds the weights.
        /// </summary>
        /// <returns>the weights of the control points</returns>
        double[] getWeights();
    }
    /// <summary>
    /// The class BSpline implements a BSpline curve. This Class contains <see cref="ControlPoints"/> and
    /// the property Degree. The curve is trimmed to his endpoints. The controlPointsarray is initialized with four zero values.
    /// The <see cref="Degree"/> is initialized with 3.
    /// </summary>
    [Serializable]
    public class Nurbs : Curve
    {
        /// <summary>
        /// Implements the <see cref="INurbs2d"/>-method
        /// </summary>
        /// <returns></returns>
        public virtual int GetDegree()
        {
            return Degree;
        }
        
        private void InsertKnot(double newKnot)
        {
          // Find KnotIndex
            int KnotIndex = -1;
            for (int i = 0; i < Knots.Length - 1; i++)
            {
                if ((newKnot >= Knots[i]) && (newKnot < Knots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }
                if ((newKnot > Knots[i]) && (newKnot <= Knots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }

            }
            if ((KnotIndex < 0) || (KnotIndex - Degree + 1 < 0) || (KnotIndex >= Knots.Length))
                throw new System.Exception("No Knot found");

            xy[] q = new xy[Degree];
            double[] w = new double[Degree];


            int j = 0;
            for (int i = KnotIndex - Degree + 1; i <= KnotIndex; i++)
            {
                double ai = (newKnot - Knots[i]) / (Knots[i + Degree] - Knots[i]);

                q[j] = ControlPoints[i - 1] * (1 - ai) /* *Weights[ i - 1]*/ + ControlPoints[i] * ai/* * Weights[index, i]*/;
               
                j++;



            }


            
            xy[] NewCtrlPoints = new xy[ControlPoints.Length + 1];
            double[] NewWeights = new double[ControlPoints.Length + 1];

            for (int i = 0; i < ControlPoints.Length; i++)
            {
                if (i <= KnotIndex - Degree)
                {
                    NewCtrlPoints[i] = ControlPoints[i];
                      }
                else
                {
                    NewCtrlPoints[i + 1] = ControlPoints[i];
                }
            }

            // Einfügen neue

            for (int i = KnotIndex - Degree + 1; i < KnotIndex + 1; i++)
            {

                NewCtrlPoints[i] = q[i - KnotIndex + Degree - 1];
            
            }

            double[] NewKnots = new double[Knots.Length + 1];
            for (int i = 0; i < Knots.Length; i++)
            {
                if (i < KnotIndex + 1)
                    NewKnots[i] = Knots[i];
                if (i == KnotIndex + 1)
                {
                    NewKnots[i] = newKnot;
                    NewKnots[i + 1] = Knots[i];
                }
                if (i > KnotIndex + 1)
                    NewKnots[i + 1] = Knots[i];
            }
            ControlPoints = NewCtrlPoints;
            Knots = NewKnots;
            // Weights = NewWeights;
        }
        /// <summary>
        /// Implements the <see cref="INurbs2d"/>-method
        /// </summary>
        /// <returns></returns>
        public virtual xy[] getCtrlPoints()
        {
            return ControlPoints;
        }
        /// <summary>
        /// Implements the <see cref="INurbs2d"/>-method
        /// </summary>
        /// <returns></returns>
        public virtual double[] getKnots()
        {
            return Knots;
        }
     
        private double[] _Weights = null;
        /// <summary>
        /// gets and sets the weights. This are the attractions for every control point
        /// </summary>
        public double[] Weights {
            get { return getWeights(); }
            set { _Weights = value; }

        }
        /// <summary>
        /// Implements the <see cref="INurbs2d"/>-method
        /// </summary>
        /// <returns></returns>
        public virtual double[] getWeights()
        {
            if (_Weights == null)
            {
                _Weights = new double[ControlPoints.Length];
                for (int i = 0; i < _Weights.Length; i++)
                {
                    Weights[i] = 1;
                }
            }
                
            return _Weights;
        }
     
        /// <summary>
        /// Overrides the Invertmethod
        /// </summary>
        public override void Invert()
        {
            //Inverted = !Inverted;
            xy _A = A;
            xy _B = B;

            //double _dummy = fromParam;
            //double _dummy2 = toParam;
            //fromParam = 1 - toParam;
            //toParam = 1 - _dummy;

            for (int i = 0; i < ControlPoints.Length / 2; i++)
            {
                xy dummy = ControlPoints[i];
                ControlPoints[i] = ControlPoints[ControlPoints.Length - i - 1];
                ControlPoints[ControlPoints.Length - i - 1] = dummy;
            }


            for (int i = 0; i < Knots.Length / 2; i++)
            {
                double dummy = Knots[i];
                Knots[i] = -Knots[Knots.Length - i - 1];
                Knots[Knots.Length - i - 1] = -dummy;
            }
            if (Knots.Length > 0)
                if (((Knots.Length / 2) * 2) != Knots.Length)
                    Knots[Knots.Length / 2] = -Knots[Knots.Length / 2];

            Dirty = true;
        }
        xy[] _ControlPoints = new xy[0];
        /// <summary>
        /// This array contains the control points of a bezier curve. The array is initialized by four points, which indicate 
        /// the curve
        /// as cubic bezier.<br/>
        /// You can set any array of control points. 
        /// </summary>
        public xy[] ControlPoints
        {
            get { return _ControlPoints; }
            set
            {
                SetControlPoints(value);
                Dirty = true;

            }
        }
        /// <summary>
        /// The virtual Setmethod of the property <see cref="ControlPoints"/>
        /// </summary>
        /// <param name="value">Parameter</param>
        virtual protected void SetControlPoints(xy[] value)
        {
            _ControlPoints = value;

            Dirty = true;
        }
        private int _Degree = 2;
        /// <summary>
        /// The degree the BSpline.
        /// The default is 3.
        /// <remarks>Usually: the Number of Controlpoints plus the degree plus 1 gives the number of Knots
        /// If this is not valid then the Knots will be setted new by <see cref="DefaultKnots"/>
        /// 
        /// </remarks>
        /// </summary>
        public int Degree
        {
            get { return _Degree; }
            set
            {
                _Degree = value;
                //if ((Knots == null) || (Knots.Length != (_ControlPoints.Length + Degree + 1)))
                //    Knots = DefaultKnots();
                Dirty = true;
            }
        }
        private double[] _Knots;
        /// <summary>
        /// Sets and gets the Knots of the BSpline
        /// </summary>
        public double[] Knots
        {
            get
            {
                if (_Knots == null) _Knots = DefaultKnots();
                return
                _Knots;
            }
            set
            {
                _Knots = value;
                Dirty = true;
            }
        }

        /// <summary>
        /// Gets nonuniform knots
        /// </summary>
        /// <returns>nonuniform knots</returns>
        public double[] DefaultKnots()
        {

            return Utils.StandardKnots(ControlPoints.Length, Degree);

        }
      
        /// <summary>
        /// Closes an open BSpline.
        /// </summary>
        public virtual void Close()
        {

            int Count = ControlPoints.Length + Degree;
            bool Closed = true;
            double[] k = Utils.UniformKnots(ControlPoints.Length, Degree);
            for (int i = 0; i < k.Length; i++)
            {
                if (k[i] != Knots[i])
                {
                    Closed = false;
                    break;
                }
            }


            for (int i = 0; i < Degree; i++)
            {
                if (!ControlPoints[i].Equals(ControlPoints[ControlPoints.Length + i - Degree]))
                {
                    Closed = false;
                    break;
                }

            }
            if (Closed) return;
            // Move ControlPoints to new
            xy[] ActualControlPoints = new xy[Count];
            for (int i = 0; i < ControlPoints.Length; i++)
                ActualControlPoints[i] = ControlPoints[i];
            // Move weigths
            // Add Points Identic to Point[0], Point[1],.. Point[UDegree-1]

            for (int i = 0; i < Degree; i++)

                ActualControlPoints[ControlPoints.Length + i] = ActualControlPoints[i];


            ControlPoints = ActualControlPoints;
            Knots = Utils.UniformKnots(ActualControlPoints.Length, Degree);
            Dirty = true;
        }
        double[][] Coeffs = null;
        void RefreshCoeffs()
        {
            Coeffs = new double[100][];
            for (int i = 0; i < 100; i++)
            {
                Coeffs[i] = Utils.UniformCoeffSpline(Knots, ControlPoints.Length, Degree, i / 99f);
            }
        }
        /// <summary>
        /// Overrides the abstract Value function <see cref="Curve.Value"/>of the curve class and retrieves the Bezierfunction of t depending on the
        /// control points <see cref="ControlPoints"/>.
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>returns the bezierfunction</returns>
        public override xy Value(double t)
        {
          
            return Utils.funNurbs2d(Knots, Weights, ControlPoints, Degree, t);
          

        }
        /// <summary>
        /// Overrides the abstract Derivationfunction <see cref="Curve.Derivation"/> of the curve class and retrieves the 
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>Derivation of the BSplinefunction</returns>
        public override xy Derivation(double t)
        {
           
           return  Utils.funNurbsDerive2d(Knots, Weights, ControlPoints, Degree, t);
       
        }
        /// <summary>
        /// Overrides the method <see cref="Curve.setA"/> by setting the value of the Point[0];
        /// </summary>
        /// <returns>Value of A</returns>
        protected override void setA(xy value)
        {
           ControlPoints[0] = value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-method.
        /// </summary>

        public override void Slice(double from, double to)
        {
            xy _A = Value(from);
            xy _B = Value(to);
            xy _At = Derivation(from) * ((to - from) / (float)3);
            xy _Bt = Derivation(to) * ((to - from) / (float)3);
            A = _A;
            B = _B;
            Atang = _At;
            Btang = _Bt;
            Dirty = true;
        }
       
        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected override void setAtang(xy value)
        {
            ControlPoints[1] = ControlPoints[0] + value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, whitch transforms the Control<see cref="ControlPoints"/>.
        /// </summary>
        /// <param name="TransformMatrix"></param>
        public override void Transform(Matrix3x3 TransformMatrix)
        {
            for (int i = 0; i < ControlPoints.Length; i++)
                ControlPoints[i] = ControlPoints[i].mul(TransformMatrix);
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.setA"/>-method by setting the endpoint to Points[3].
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xy value)
        {
          
            ControlPoints[ControlPoints.Length - 1] = value;
           
            Dirty = true;
        }
     
        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/> by setting Points[2] to Points[3] - value;
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected override void setBtang(xy value)
        {
            ControlPoints[ControlPoints.Length - 2] = B - value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[1] - Points[0].
        /// </summary>
        /// <returns>Starttangent</returns>
        protected override xy getAtang()
        {
            return ControlPoints[1] - A;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[3] - Points[2].
        /// </summary>
        /// <returns>Endtangent</returns>
        protected override xy getBtang()
        {
            return B - ControlPoints[ControlPoints.Length - 2];
        }
    }

}
