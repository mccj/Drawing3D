using System;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{         
    /// <summary>
    /// The class Bezier implements a Bezier curve. This curve is initialized by four control points, which make the curve
    /// to a cubic Bezier. By setting the <see cref="Points"/> you can make a bezier of any degree.
    /// </summary>    
    [Serializable]
    public class Bezier3D : Curve3D
    {    
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public Bezier3D()
        { }
        /// <summary>
        /// is a constructor with the start point <b>A</b> and the endpoint <b>B</b>. 
        /// <b>CtrlPointA</b> is the first control point and <b>CtrlPointB</b> the second.
        /// </summary>
        /// <param name="A">is the start point.</param>
        /// <param name="CtrlPointA">is the first control point.</param>
        /// <param name="CtrlPointB">is the second control point.</param>
        /// <param name="B">is the endpoint.</param>
        public Bezier3D(xyz A, xyz CtrlPointA, xyz CtrlPointB,xyz B):this()
        {    
            Points[0] = A;
            Points[1] = CtrlPointA;   
            Points[2] = CtrlPointB;
            Points[3] = B;


        }
        /// <summary>
        /// This array contains the control points of a bezier curve. The array is initialized by four points, which indicate 
        /// the curve
        /// as cubic bezier.<br/>
        /// You can set any array of control points. 
        /// </summary>
        ///   [BrowsableAttribute(false)]
        public xyz[] Points = new xyz[4];
        /// <summary>
        /// Overrides the abstract Value function <see cref="Curve.Value"/>of the curve class and retrieves the Bezierfunction of t depending on the
        /// control points <see cref="Points"/>.
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>returns the bezierfunction</returns>
        public override xyz Value(double t)
        {
           return Utils.funBezier3D(Points,t);
           
          
        }
        /// <summary>
        /// Overrides the abstract Derivationfunction <see cref="Curve.Derivation"/> of the curve class and retrieves the 
        /// derivation of the Bezierfunction of t, 
        /// depending on the controlpoints <see cref="Points"/>
        /// 
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>Derivation of the bezierfunction</returns>
        public override xyz Derivation(double t)
        {
          return  Utils.funBezierDerive3D(Points, t);
            

          
        }
        /// <summary>
        /// Overrides the method <see cref="Curve3D.setA"/> by setting the value of the Point[0];
        /// </summary>
        /// <returns>Value of A</returns>
        protected override void setA(xyz value)
        {
            xyz save = Atang;
            Points[0] = value;
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


        protected override xyz getA()
        {
            return Points[0];
        }

        /// <summary>
        /// Overrides the <see cref="Curve3D.setAtang"/>-method
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected override void setAtang(xyz value)
        {
            Points[1] = Points[0] + value;
        }
        /// <summary>
        /// Overrides the <see cref="getB"/>-Method by returning the Point[3]
        /// </summary>
        /// <returns>Value of the endpoint</returns>
        protected override xyz getB()
        {
            return Points[3];
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, whitch transforms the Control<see cref="Points"/>.
        /// </summary>
        /// <param name="m"></param>
        public override void Transform(Matrix m)
        {
            for (int i = 0; i < Points.Length; i++)
                Points[i] = Points[i].mul(m);
        }

        /// <summary>
        /// Overrides the <see cref="Curve3D.setA"/>-method by setting the endpoint to Points[3].
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xyz value)
        {
            xyz save = Btang;
            Points[3] = value;
            Btang = save;
        }

        /// <summary>
        /// Overrides the <see cref="Curve3D.setBtang"/> by setting Points[2] to Points[3] - value;
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected override void setBtang(xyz value)
        {
            Points[2] = Points[3] - value;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[1] - Points[0].
        /// </summary>
        /// <returns>Starttangent</returns>
        protected override xyz getAtang()
        {
            return Points[1] - Points[0];
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[3] - Points[2].
        /// </summary>
        /// <returns>Endtangent</returns>
        protected override xyz getBtang()
        {
            return Points[3] - Points[2];
        }


    }
}