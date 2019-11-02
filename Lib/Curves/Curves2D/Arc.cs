using System;
using System.ComponentModel;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
namespace Drawing3d
{

 
    /// <summary>
    /// The class a encapsulate properties and method about an arc, i.e a part of
    /// a ellipse. It has main properties <see cref="Center"/>, <see cref="aRadius"/>, <see cref="bRadius"/>
    /// So you can define an ellipse. Additionally it has an start angle
    /// <see cref="Alfa"/> and an end angle <see cref="Beta"/>. See also <see cref="ClockWise"/>.
    /// </summary>
    [Serializable]
    public class Arc : Curve
    {


        private xy FTang = new xy(0, 0);
        private Matrix3x3 FTransformation = new Matrix3x3(1);

        /// <summary>
        /// Gets and sets a transformation matrix for the arc
        /// </summary>
        public Matrix3x3 Transformation
        {
            get
            {
                return FTransformation;
            }
            set
            {
                FTransformation = value;

            }
        }

        /// <summary>
        /// This constructor initializes <see cref="Alfa"/>=<see cref="Beta"/>=
        /// <see cref="bRadius"/>=<see cref="aRadius"/>= 0 and
        /// <see cref="Center"/> with (0/0);
        /// 
        /// </summary>
        public Arc()
        {
            Alfa = 0;
            Beta = 0;
            Center = new xy(0, 0);
            bRadius = 0;
            aRadius = 0;
            Resolution = 32;
        }
        /// <summary>
        /// Initializes <see cref="Alfa"/>=0
        /// <see cref="Beta"/>=2PI
        /// <see cref="bRadius"/>=<see cref="aRadius"/>= Radius and
        /// so you get a full circle.
        /// </summary>

        public Arc(xy Center, double Radius) : this()
        {
            this.Center = Center;
            this.bRadius = Radius;
            this.aRadius = Radius;
            Alfa = 0;
            //   this.CircleBeta = 2 * PI + 0.00001;
            Beta = 2 * Math.PI;
            ClockWise = false;
        }
        /// <summary>
        /// a constructor, which creates an ellipse, which is rotated by the angle <b>RotationAngle</b> in the clockwise sense.
        /// </summary>
        /// <param name="aRadius"></param>
        /// <param name="bRadius"></param>
        /// <param name="RotationAngle"></param>
        public Arc(double aRadius, double bRadius,double RotationAngle) : this()
        {
          
            this.bRadius = aRadius;
            this.aRadius = bRadius;
            Transform(Matrix3x3.Rotation(RotationAngle));
            Alfa = 0;
            Beta = 2 * Math.PI;
            ClockWise = false;
        }
        /// <summary>
        ///  Initializes an arc by Center, startangle Alfa, endangle Beta, Orientation Clockwise
        ///  and the radius Radius.
        /// </summary>
        /// <param name="Center">Center of the circle</param>
        /// <param name="Radius">Radius of the circle</param>
        /// <param name="Alfa">Startangle</param>
        /// <param name="Beta">Endangle</param>
        /// <param name="ClockWise">Orientation</param>
        public Arc(xy Center, double Radius, double Alfa, double Beta, bool ClockWise) : this()
        {

            this.Center = Center;
            this.bRadius = Radius;
            this.aRadius = Radius;
            this.Alfa = Alfa;
            this.Beta = Beta;
            this.ClockWise = ClockWise;

        }
        /// <summary>
        /// a constructor with: Center, aRadius, bRadius, Alfa, Beta and ClockWise;
        /// </summary>
        /// <param name="Center"><see cref="Center"/></param>
        /// <param name="ARadius"><see cref="aRadius"/></param>
        /// <param name="BRadius"><see cref="bRadius"/></param>
        /// <param name="Alfa"><see cref="Alfa"/></param>
        /// <param name="Beta"><see cref="Beta"/></param>
        /// <param name="ClockWise"><see cref="ClockWise"/></param>
        public Arc(xy Center, double ARadius, double BRadius, double Alfa, double Beta, bool ClockWise) : this()
        {

            this.Center = Center;
            this.bRadius = ARadius;
            this.aRadius = BRadius;
            this.Alfa = Alfa;
            this.Beta = Beta;
            this.ClockWise = ClockWise;

        }

        /// <summary>
        /// Creates an arc by tree points P,Q and R.
        /// </summary>
        /// <param name="P">First point</param>
        /// <param name="Q">Second point</param>
        /// <param name="R">Third point</param>
        public Arc(xy P, xy Q, xy R)
        {
            LineType2d L1 = new LineType2d((P + Q) * 0.5, (Q - P).normal());
            LineType2d L2 = new LineType2d((Q + R) * 0.5, (R - Q).normal());
            double Lam, Mue;
            if (L1.Cross(L2, out Lam, out Mue))
            {
                Center = L1.Value(Lam);
                aRadius = bRadius = Center.dist(P);
                Alfa = getAngle(P);
                Beta = getAngle(R);
                ClockWise = ((R - Q) & (P - Q)) < 0;

            }

        }
        xy _Center = new xy(0, 0);
        /// <summary>
        /// Center of the arc
        /// </summary>
        [BrowsableAttribute(false)]
        public xy Center
        {
            get { return Transformation * new xy(0, 0); }
            set
            {
                xy M = Center;
                Matrix3x3 T = Matrix3x3.Translation(new xy(value.x - M.x, value.y - M.y));
                Transformation = T * Transformation;
            }
        }

        private xy FA = new xy(0, 0);
        private xy FB = new xy(0, 0);


        /// <summary>
        /// Returns the arc value for a parameter t between 0 and 1.
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>Arcvalue</returns>
        public override xy Value(double t)
        {
            double d = 0;
            if (ClockWise)
            {
                if (Alfa >= Beta) d = Beta - Alfa;
                else
                    d = (Beta - Alfa) - 2 * Math.PI;// (2 * Math.PI - Alfa);

            }
            else
            {
                if (Beta >= Alfa) d = (Beta - Alfa);

                else
                    d = (Beta - Alfa) + 2 * Math.PI;

            }
            return (Transformation * new xy(System.Math.Cos(Alfa + d * t), System.Math.Sin(Alfa + d * t)));
        }
        /// <summary>
        /// gets the angle of a point.
        /// </summary>
        /// <param name="Pt">a point</param>
        /// <returns>the angle</returns>
        public double getAngle(xy Pt)
        {
            Matrix3x3 M = Transformation.invert();
            xy P = M * Pt;
            return Math.Atan2(P.y, P.x);

        }
        /// <summary>
        /// Overrides the <see cref="Curve.setB"/>-method. It keeps the center.
        /// </summary>
        /// <param name="value"></param>
        protected override void setB(xy value)
        {

            SetRadiusbyPoint(value);
            Beta = getAngle(value);
            Dirty = true;

        }
        /// <summary>
        /// Overrides the <see cref="Curve.setA"/>-method. It keeps the center.
        /// </summary>
        /// <param name="value"></param>
        protected override void setA(xy value)
        {

            SetRadiusbyPoint(value);
            Alfa = getAngle(value);
            Dirty = true;

        }
        /// <summary>
        /// defines <see cref="aRadius"/> and <see cref="bRadius"/> by keeping the ratio of aRadius and bRadius.
        /// The arc contains the point <b>Pt</b>.
        /// </summary>
        /// <param name="Pt">Point, which lies on the new arc.</param>
        public void SetRadiusbyPoint(xy Pt)
        {
            xy _PT = Transformation.invert() * Pt;
            double A = _PT.length();
            Transformation = Matrix3x3.Scale(A, A) * Transformation;
            Dirty = true;

        }
        private double _Alfa = 0;
        /// <summary>
        /// Sets and gets the startangle.<remarks>2PI and 0 are the same</remarks> .
        /// </summary>
        public double Alfa
        {
            get
            {
                return _Alfa;

            }
            set
            {
                _Alfa = value;
                Dirty = true;

            }
        }
        private double _Beta = 0;
        /// <summary>
        /// Ses and gets the endangle. 2PI and 0 are the same.
        /// </summary>

        public double Beta
        {
            get
            {
                return _Beta;

            }
            set
            {
                _Beta = value;
                Dirty = true;

            }
        }

        /// <summary>
        /// Sets anf gets the x-radius of the ellipse
        /// </summary>
        [BrowsableAttribute(false)]

        public double aRadius
        {
            get
            {

                return (Transformation * (new xy(1, 0)) - Center).length();
            }
            set
            {
                xy C = Center;
                double factor;
                if (aRadius < 0.00001)
                {

                    factor = value;
                    Transformation =
                        Matrix3x3.Translation(new xy(C.x, C.y)) *
                        Matrix3x3.Scale(new xy(factor, 1));
                }
                else
                {
                    factor = value / aRadius;
                    Transformation = Matrix3x3.Translation(new xy(C.x, C.y)) *
                        Matrix3x3.Scale(new xy(factor, 1)) *
                        Matrix3x3.Translation(new xy(-C.x, -C.y)) *
                        Transformation;
                }
                Dirty = true;
            }
        }
        /// <summary>
        /// Sets and gets the y-radius of the ellipse.
        /// </summary>
        [BrowsableAttribute(false)]
        public double bRadius
        {
            get
            {

                return (Transformation * (new xy(0, 1)) - Center).length();
            }
            set
            {
                xy C = Center;
                double factor;
                if (bRadius < 0.00001)
                {

                    factor = value;
                    Transformation =
                        Matrix3x3.Translation(new xy(C.x, C.y)) *
                        Matrix3x3.Scale(new xy(1, factor));
                }
                else
                {
                    factor = value / bRadius;
                    Transformation = Matrix3x3.Translation(new xy(C.x, C.y)) *
                        Matrix3x3.Scale(new xy(1, factor)) *
                        Matrix3x3.Translation(new xy(-C.x, -C.y)) *
                        Transformation;
                }

                Dirty = true;
            }
        }
        private bool _ClockWise = false;
        /// <summary>
        /// Sets and gets the orientation of the arc.
        /// </summary>
        public bool ClockWise
        {
            get { return _ClockWise; }
            set { _ClockWise = value; Dirty = true; }
        }
        /// <summary>
        /// Overrides the <see cref="Curve.Derivation"/>
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns></returns>
        public override xy Derivation(double t)
        {

            xy P = Value(t);
            Matrix3x3 M = Transformation.invert();
            xy Result = (Transformation.multaffin(((M * P).normal())));
            if (!ClockWise) return Result * (-1);
            return Result;
        }
        /// <summary>
        /// Overrides the invert method and exchanges alfa and beta.
        /// </summary>
        public override void Invert()
        {
            double dummy = Alfa;
            Alfa = Beta;
            Beta = dummy;
            ClockWise = !ClockWise;
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method. It holds the point A.
        /// </summary>
        /// <param name="value"></param>
        protected override void setAtang(xy value)
        {
            xy SaveA = A;
            xy _value = Transformation.invert().multaffin(value);
            Alfa = Math.Atan2(-_value.x, _value.y);
            Center += (SaveA - A);
            Dirty = true;

        }

        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/>-method.
        /// </summary>		
        protected override void setBtang(xy value)
        {
            xy SaveB = B;
            xy _value = Transformation.invert().multaffin(value);
            Alfa = Math.Atan2(-_value.x, _value.y);
            Center += (SaveB - B);
            Dirty = true;

        }
        /// <summary>
        /// Overrides the <see cref="Curve.Slice"/>-method.
        /// </summary>
        public override void Slice(double from, double to)
        {
            double d = 0;
            if (ClockWise)
            {
                if (Alfa >= Beta) d = Beta - Alfa;
                else
                    d = (Beta - Alfa) - 2 * Math.PI;// (2 * Math.PI - Alfa);

            }
            else
            {
                if (Beta >= Alfa) d = (Beta - Alfa);

                else
                    d = (Beta - Alfa) + 2 * Math.PI;

            }

            Beta = Alfa + d * to;
            Alfa = Alfa + d * from;
        }


        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method.
        /// </summary>		
        public override void Transform(Matrix3x3 m)
        {

            Transformation = m * Transformation;

            Dirty = true;
        }

    }
}
