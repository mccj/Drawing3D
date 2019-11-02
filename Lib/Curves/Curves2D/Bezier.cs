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
    /// The class Bezier implements a Bezier curve. This curve is initialized by four control points, which make the curve
    /// to a nurbs of degree 3 and a fix weight of 1.
    /// </summary>
    [Serializable]
    public class Bezier :Nurbs, INurbs2d
    {
        /// <summary>
        /// implements the <see cref="INurbs2d"/>
        /// </summary>
        /// <returns></returns>
        public override int GetDegree()
        {
            return 3;
        }
        /// <summary>
        /// implements the <see cref="INurbs2d"/>
        /// </summary>
        /// <returns></returns>
        public override xy[] getCtrlPoints()
        {
            return Points;
        }
        public override void Invert()
        {
          

            xy dummy = new Drawing3d.xy(0, 0);
            dummy = Points[0];
            Points[0] = Points[3];
            Points[3] = dummy;
            dummy = Points[1];
            Points[1] = Points[2];
            Points[2] = dummy;
            
        }
        /// <summary>
        /// implements the <see cref="INurbs2d.getKnots"/> by 0, 0, 0, 0, 1, 1, 1, 1
        /// </summary>
        /// <returns></returns>
        public override double[] getKnots()
        {
            return new double[] { 0, 0, 0, 0, 1, 1, 1, 1 };
        }
        /// <summary>
        /// implements the <see cref="INurbs2d.getWeights"/> by 1, 1, 1, 1
        /// </summary>
        /// <returns></returns>
        public override double[] getWeights()
        {
            return new double[] { 1, 1, 1, 1 };
        }
        /// <summary>
        /// Constructor, which initializes the bezier curve with startpoint A endpoint B and two controlpoints
        /// </summary>
        /// <param name="A">Startpoint</param>
        /// <param name="ControlPoint1">1. Controlpoint</param>
        /// <param name="ControlPoint2">2.Controlpoint</param>
        /// <param name="B">Endpoint</param>
        public Bezier(xy A, xy ControlPoint1, xy ControlPoint2, xy B)
        {
            Points[0] = A;
            Points[1] = ControlPoint1;
            Points[2] = ControlPoint2;
            Points[3] = B;

        }
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Bezier() : base()
        {
           
        }
        /// <summary>
        /// This array contains the control points of a bezier curve. The array is initialized by four points, which indicate 
        /// the curve
        /// as cubic bezier.<br/>
        /// You can set any array of control points. 
        /// </summary>
        [BrowsableAttribute(false)]
        private xy[] Points = new xy[4];
        /// <summary>
        /// Overrides the abstract Value function <see cref="Curve.Value"/>of the curve class and retrieves the Bezierfunction of t depending on the
        /// four control points <see cref="Points"/>.
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>returns the bezierfunction</returns>
        public override xy Value(double t)
        {
            return Utils.funBezier(Points, t);
        }
        /// <summary>
        /// Overrides the abstract Derivationfunction <see cref="Curve.Derivation"/> of the curve class and retrieves the 
        /// derivation of the Bezierfunction of t, 
        /// depending on the controlpoints <see cref="Points"/>
       /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>Derivation of the bezierfunction</returns>
        public override xy Derivation(double t)
        {
            return Utils.funBezierDerive(Points, t);
        }
      
        /// <summary>
        /// Overrides the method <see cref="Curve.setA"/> by setting the value of the Point[0];
        /// </summary>
        /// <returns>Value of A</returns>
        protected override void setA(xy value)
        {
            xy save = Atang;
            Points[0] = value;
            Atang = save;
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
        /// Overrides the abstract <see cref="Curve.getA"/>-method and returns the point[0].
        /// </summary>
        protected override xy getA()
        {
            return Points[0];
        }
        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected override void setAtang(xy value)
        {
            Points[1] = Points[0] + value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="getB"/>-Method by returning the Point[3]
        /// </summary>
        /// <returns>Value of the endpoint</returns>
        protected override xy getB()
        {
            return Points[3];
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, whitch transforms the Control<see cref="Points"/>.
        /// </summary>
        /// <param name="m"></param>
        public override void Transform(Matrix3x3 m)
        {
            for (int i = 0; i < Points.Length; i++)
                Points[i] = Points[i].mul(m);
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.setA"/>-method by setting the endpoint to Points[3].
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xy value)
        {
            xy save = Btang;
            Points[3] = value;
            Btang = save;
            Dirty = true;
        }
     
        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/> by setting Points[2] to Points[3] - value;
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected override void setBtang(xy value)
        {
            Points[2] = Points[3] - value;
            Dirty = true;
        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[1] - Points[0].
        /// </summary>
        /// <returns>Starttangent</returns>
        protected override xy getAtang()
        {
           return Points[1] - Points[0];

        }
        /// <summary>
        /// Overrides the <see cref="Curve.getAtang"/> and returns Points[3] - Points[2].
        /// </summary>
        /// <returns>Endtangent</returns>
        protected override xy getBtang()
        {
           return Points[3] - Points[2];
        }
    }
}
