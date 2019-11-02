using System;


//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// Line, which contains the two points A and B and describes a line between A and B.
    /// </summary>
    [Serializable]
    public class Line3D : Curve3D
    {
        /// <summary>
        /// Defines the Atang property new.
        /// </summary>

        new public xyz Atang
        {
            get { return this.getAtang(); }
            set { this.setAtang(value); }
        }
        /// <summary>
        /// Defines the Btang property new.
        /// </summary>

        new public xyz Btang
        {
            get { return this.getBtang(); }
            set { this.setBtang(value); }
        }
        //private xyz FA, FB;
        ///// <summary>
        ///// Overrides the <see cref="setA"/>-method and saves the value in a local variable.
        ///// </summary>
        ///// <param name="value">Startpoint</param>
        //protected override void setA(xyz value)
        //{
        //    FA = value;
        //}
     
        /// <summary>
        /// Overrides the <see cref="Curve3D.setAtang"/>-method and ignores the setting.
        /// </summary>
        /// <param name="value">starttangent</param>
        protected override void setAtang(xyz value)
        {

        }
        /// <summary>
        /// Overrides the <see cref="Curve3D.SetBorder"/>-method and set A to _A and B to _B.
        /// </summary>
        /// <param name="_A">Start point of the line</param>
        /// <param name="_B">End point of the line</param>
        public override void SetBorder(xyz _A, xyz _B)
        {


            A = _A;
            B = _B;
            //base.SetBorder(_A, _B);
            //fromParam = 0;
            //toParam = 1;

        }
        ///// <summary>
        ///// Overrides the <see cref="getB"/>-method and returns the value of a local variable.
        ///// </summary>
        ///// <returns>Endpoint</returns>

        //protected override xyz getB()
        //{
        //    return FB;
        //}
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-Method.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public override void Slice(double from, double to)
        {
            xyz _A = Value(from);
            xyz _B = Value(to);
            A = _A;
            B = _B;

        }
        /// <summary>
        /// Projects a point normal to the Line.
        /// </summary>
        /// <param name="Point">A Point</param>
        /// <param name="Lambda">Parameter to calculate the normal projected Point on the line</param>
        /// <returns></returns>
        public override bool NormalCross(xyz Point, ref double Lambda)
        {

            xyz Dummy;
            Plane P = new Plane(Point, B - A);
            return P.Cross(new LineType(A, B - A), out Lambda, out Dummy);

        }
        ///// <summary>
        ///// Overrides the <see cref="setB"/>-method and saves the value in a local variable.
        ///// </summary>
        ///// <param name="value">Endpoint</param>

        //protected override void setB(xyz value)
        //{
        //    FB = value;
        //}
        /// <summary>
        /// Overrides the <see cref="Curve3D.setBtang"/>-method and ignores the setting.
        /// </summary>
        /// <param name="value">Endtangent</param>

        protected override void setBtang(xyz value)
        {

        }
        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method and transforms the points A and B.
        /// </summary>
        /// <param name="m">Transformationmatrix</param>
        public override void Transform(Matrix m)
        {
            A = A.mul(m);
            B = B.mul(m);

        }
        /// <summary>
        /// The default constructor set the <see cref="Curve.Resolution"/> to 1.
        /// </summary>
        public Line3D():base()
        {
            Resolution = 1;
        }

        /// <summary>
        /// Constructor, which initializes the line by the starting point and the endpoint.
        /// </summary>
        /// 
        /// <param name="A">Startpoint</param>
        /// <param name="B">Endpoint</param>

        public Line3D(xyz A, xyz B):base()
        {
            Resolution = 1;
            this.A = A;
            this.B = B;
        }

        /// <summary>
        /// Overrides the abstract value function <see cref="Curve.Value"/>of the curve class 
        /// and returns the linear function through <see cref="Curve3D.A"/> and <see cref="Curve3D.B"/>
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>values of a line</returns>
        public override xyz Value(double t)
        {
            return A + (B - A) * t;
        }
        /// <summary>
        /// Overrides the abstract value function <see cref="Curve.Derivation"/>of the curve class 
        /// and returns direction B - A
        /// 
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>B - A</returns>
        public override xyz Derivation(double t)
        {
            return B - A;
        }
    }
}