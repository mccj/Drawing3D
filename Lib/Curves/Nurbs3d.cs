using System;


//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// The class Nurbs implements a Nurbs curve. This Class contains <see cref="ControlPoints"/> and
    /// the property Degree. The curve is trimmed to his endpoints. The controlPointsarray is initialized with four zero values.
    /// The <see cref="Degree"/> is initialized with 3.
    /// </summary>
    [Serializable]
    public class Nurbs3d : Curve3D
    {/// <summary>
     /// empty constructor.
     /// </summary>
        public Nurbs3d()
        { }
        /// <summary>
        /// constructor, which holds ControlPoints and Degree
        /// </summary>
        /// <param name="ControlPoints"><see cref="ControlPoints"/></param>
        /// <param name="Degree"><see cref="Degree"/></param>
        public Nurbs3d(xyz[] ControlPoints, int Degree) :this()
            {
            this.Degree = Degree;
            this.ControlPoints = ControlPoints;
            
        }
       

        private void KnotsNormal(double Start, double End)
        {
            double w = (Knots[Knots.Length - 1] - Knots[0]);
            double np = Knots[0];
            for (int d = 0; d < Knots.Length; d++)
            {
                Knots[d] = (Knots[d] - np) / w * (End - Start) + Start;
            }



        }
        double[] _Weights = null;
        /// <summary>
        /// Virtual Set method of <see cref="Weights"/>.
        /// </summary>
        /// <param name="Weights"></param>
        virtual protected void SetWeights(double[] Weights)
        {
            this._Weights = Weights;
        }
        /// <summary>
        /// Defines the Weights of the Nurbs.
        /// </summary>
        public double[] Weights
        {
            get { return _Weights; }
            set { SetWeights(value); }
        }
        /// <summary>
        /// This array contains the control points of a Nurbs curve. The array is initialized by four points, which indicate 
        /// the curve
        /// as cubic bezier.<br/>
        /// You can set any array of control points. 
        /// </summary>

        public xyz[] ControlPoints
        {
            get { return _ControlPoints; }
            set { SetControlPoints(value); }
        }
        xyz[] _ControlPoints = new xyz[4];
        /// <summary>
        /// The virtual setmethod of the property <see cref="ControlPoints"/>.
        /// If <see cref="Knots"/> are null then Knots ar initialized by <see cref="DefaultKnots"/>.
        /// </summary>
        /// <param name="value"></param>
        virtual protected void SetControlPoints(xyz[] value)
        {
            _ControlPoints = value;
            if ((Weights == null) || (Weights.Length != ControlPoints.Length))
            {
                Weights = new double[ControlPoints.Length];
                for (int i = 0; i < Weights.Length; i++)
                    Weights[i] = 1;

            }

        

       
            if ((Knots == null) || (Knots.Length != (_ControlPoints.Length + Degree + 1)))
                Knots = DefaultKnots();
           
        }
        private int _Degree = 4;
        /// <summary>
        /// The Degree of the Nurbs.
        /// </summary>
        public int Degree
        {
            get { return _Degree; }
            set
            {
                _Degree = value;

                if ((Knots == null) || (Knots.Length != (_ControlPoints.Length + Degree + 1)))
                    Knots = this.DefaultKnots();
            }
        }
        private double[] _Knots;
        /// <summary>
        /// Gets and sets the Knots of the Nurbs.
        /// </summary>
        public double[] Knots
        {
            get { return _Knots; }
            set
            {
                _Knots = value;
            }
        }
        /// <summary>
        /// Gets the <see cref="Utils.DefaultKnots"/>.
        /// </summary>
        /// <returns></returns>
        public double[] DefaultKnots()
        {

            return Utils.StandardKnots(ControlPoints.Length, Degree);

        }
        /// <summary>
        /// Gets the <see cref="Utils.UniformKnots"/>
        /// </summary>
        /// <returns></returns>
        public double[] UniformKnots()
        {

            return Utils.UniformKnots(ControlPoints.Length, Degree);

        }
        /// <summary>
        /// Overrides the abstract Value function <see cref="Curve.Value"/>of the curve class and retrieves the Bezierfunction of t depending on the
        ///  <see cref="ControlPoints"/>. It call the <see cref="Utils.funNurbs3d"/>-method.
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>returns the bezierfunction</returns>
        public override xyz Value(double t)
        {
                return Utils.funNurbs3d(Knots, Weights, ControlPoints, Degree, t);

        }
     



       /// <summary>
       /// overrides <see cref="Curve3D.Invert"/>
       /// </summary>
        public override void Invert()
        {
           

            for (int i = 0; i < ControlPoints.Length / 2; i++)
            {
                xyz Pivot = ControlPoints[i];
                ControlPoints[i] = ControlPoints[ControlPoints.Length - 1 - i];
                ControlPoints[ControlPoints.Length - 1 - i] = Pivot;
            }
            double MaxK = Knots[Knots.Length - 1];
            for (int i = 0; i < Knots.Length / 2; i++)
            {

                double P = Knots[i];
                Knots[i] = MaxK - Knots[Knots.Length - 1 - i];
                Knots[Knots.Length - 1 - i] = MaxK - P;
            }
        }
        /// <summary>
        /// Closes the Nurbs by changing some Controlpoints and Knots.
        /// </summary>
        /// <summary>
        /// Closes a Nurbs by changing some Controlpoints and some Weights.
        /// </summary>
        public  void Close()
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
            xyzArray ActualControlPoints = new xyzArray(Count);
            for (int i = 0; i < ControlPoints.Length; i++)
                ActualControlPoints[i] = ControlPoints[i];
            // Move weigths
            // Add Points Identic to Point[0], Point[1],.. Point[UDegree-1]

            for (int i = 0; i < Degree; i++)

                ActualControlPoints[ControlPoints.Length + i] = ActualControlPoints[i];


            ControlPoints = ActualControlPoints.ToArray();
            Knots = Utils.UniformKnots(ActualControlPoints.Count, Degree);

        }

        /// <summary>
        /// Overrides the Derivationfunction <see cref="Curve3D.Derivation"/> of the curve class and retrieves the 
        /// derivation of the Bezierfunction of t, 
        /// depending on the controlpoints <see cref="ControlPoints"/>
        /// 
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>Derivation of the bezierfunction</returns>
        public override xyz Derivation(double t)
        {
        return Utils.funNurbsDerive3d(Knots, Weights, ControlPoints, Degree, t);
    }
        /// <summary>
        /// Overrides the method <see cref="Curve3D.setA"/> by setting the value of the Point[0];
        /// </summary>
        /// <returns>Value of A</returns>
        protected override void setA(xyz value)
        {
            xyz save = Atang;
            ControlPoints[0] = value;
            Atang = save;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-method.
        /// </summary>

        public override void Slice(double from, double to)
        {
            xyz _A = Value(from);
            xyz _B = Value(to);
            xyz _At = Derivation(from) * ((to - from) / (float)3);
            xyz _Bt = Derivation(to) * ((to - from) / (float)3);
            A = _A;
            B = _B;
            Atang = _At;
            Btang = _Bt;
        }
        /// <summary>
        /// Overrides the abstract <see cref="Curve.getA"/>-method and returns the point[0].
        /// </summary>
        /// <summary>
        /// Overrides the <see cref="Curve3D.setAtang"/>-method
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected override void setAtang(xyz value)
        {
            ControlPoints[1] = ControlPoints[0] + value;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, whitch transforms the Control<see cref="ControlPoints"/>.
        /// </summary>
        /// <param name="m"></param>
        public override void Transform(Matrix m)
        {
            for (int i = 0; i < ControlPoints.Length; i++)
                ControlPoints[i] = ControlPoints[i].mul(m);
        }

        /// <summary>
        /// Overrides the <see cref="Curve3D.setA"/>-method by setting the endpoint to Points[3].
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xyz value)
        {
            xyz save = Btang;
            ControlPoints[ControlPoints.Length - 1] = value;
            Btang = save;
        }

        /// <summary>
        /// Overrides the <see cref="Curve3D.setBtang"/> by setting Points[2] to Points[3] - value;
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected override void setBtang(xyz value)
        {
            ControlPoints[ControlPoints.Length - 2] = B - value;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[1] - Points[0].
        /// </summary>
        /// <returns>Starttangent</returns>
        protected override xyz getAtang()
        {
            return ControlPoints[1] - A;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[3] - Points[2].
        /// </summary>
        /// <returns>Endtangent</returns>
        protected override xyz getBtang()
        {
            return B - ControlPoints[ControlPoints.Length - 2];
        }
    }

}