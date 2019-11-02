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
{ 
    /// <summary>
    /// Line, which contains the two points A and B and describes a line between A and B.
    /// </summary>
    [Serializable]
    public class Line : Curve
    {
        private xy FA, FB;
        /// <summary>
        /// Overrides the <see cref="setA"/>-method and saves the value in a local variable.
        /// </summary>
        /// <param name="value">Startpoint</param>
        protected override void setA(xy value)
        {
            FA = value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="getA"/>-method and returns the value of a local variable.
        /// </summary>
        /// <returns>Startpoint</returns>
        protected override xy getA()
        {
            return FA;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method and ignores the setting.
        /// </summary>
        /// <param name="value">starttangent</param>
        protected override void setAtang(xy value)
        {

        }
        /// <summary>
        /// Overrides the <see cref="getB"/>-method and returns the value of a local variable.
        /// </summary>
        /// <returns>Endpoint</returns>

        protected override xy getB()
        {
            return FB;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-Method.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public override void Slice(double from, double to)
        {
            xy _A = Value(from);
            xy _B = Value(to);
            A = _A;
            B = _B;

            Dirty = true;

        }

        /// <summary>
        /// Overrides the <see cref="setB"/>-method and saves the value in a local variable.
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xy value)
        {
            FB = value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/>-method and ignores the setting.
        /// </summary>
        /// <param name="value">Endtangent</param>

        protected override void setBtang(xy value)
        {

        }
        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method and transforms the points A and B.
        /// </summary>
        /// <param name="m">Transformationmatrix</param>
        public override void Transform(Matrix3x3 m)
        {
            A = A.mul(m);
            B = B.mul(m);
            Dirty = true;
       }
        /// <summary>
        /// The default constructor sets the <see cref="Curve.Resolution"/> to 1.
        /// </summary>
        public Line()
        {
            Resolution = 1;
        }
        /// <summary>
        /// Constructor, which initializes the line by the starting point and the endpoint.
        /// </summary>
        /// 
        /// <param name="A">Startpoint</param>
        /// <param name="B">Endpoint</param>

        public Line(xy A, xy B)
        {
            Resolution = 1;
            this.A = A;
            this.B = B;
            Dirty = true;
        }
        
        /// <summary>
        /// Overrides the abstract value function <see cref="Curve.Value"/>of the curve class 
        /// and returns the linear function through <see cref="Curve.A"/> and <see cref="Curve.B"/>
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>values of a line</returns>
        public override xy Value(double t)
        {

            return A + (B - A) * t;
        }
        /// <summary>
        /// Overrides the abstract value function <see cref="Curve.Derivation"/>of the curve class 
        /// and returns direction B - A
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>B - A</returns>
        public override xy Derivation(double t)
        {
            return (B - A);
        }
    }
}
