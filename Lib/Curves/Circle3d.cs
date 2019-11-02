using System;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
namespace Drawing3d
{  
    /// <summary>
    /// Implements a Circle, which is given by an <see cref="Arc"/> in a the <see cref="Base"/>. The range for the parameter goes from 0 to 1 and calculates
    /// the values in a the sense, which is given by the property <see cref="Clockwise"/>. It is false by default. If you will produce a an arc from 0° to 90° you have set <see cref="Alfa"/>  to 0 and
    /// <see cref="Beta"/> to PI/2.
    /// </summary>
    [Serializable]
    public class Circle3D : Curve3D
    {


        private Arc Arc = new Arc();
        /// <summary>
        /// An empty contructor. It sets the resolution to 40
        /// </summary>
        public Circle3D()
        {
            Arc = new Arc();
            Resolution = 40;  
        }
        /// <summary>
        /// Initializes <see cref="Alfa"/>=0
        /// <see cref="Beta"/>=2PI
        /// <see cref="bRadius"/>=<see cref="aRadius"/>= Radius and
        /// so you get a full circle.
        /// </summary>

        public Circle3D(Base Base,xyz Center, double Radius) : this()
        {
            this.Base = Base;
            Arc = new Arc(Center.toXY(), Radius);
           
        }
        /// <summary>
        /// a constructor, which creates an ellipse, which is rotated by the angle <b>RotationAngle</b> in the clockwise sense.
        /// </summary>
        /// <param name="aRadius"></param>
        /// <param name="bRadius"></param>
        /// <param name="RotationAngle"></param>
        public Circle3D(double aRadius, double bRadius, double RotationAngle) : this()
       {
            Arc = new Arc(aRadius, bRadius, RotationAngle);
       }
        /// <summary>
        /// Creates a Circle by tree points P,Q and R.
        /// </summary>
        /// <param name="P">First point</param>
        /// <param name="Q">Second point</param>
        /// <param name="R">Third point</param>
        /// <param name="Base">the base of the circle</param>

        public Circle3D(Base Base,xyz P, xyz Q, xyz R)
        {
            this.Base = Base;
            Arc = new Arc(P.toXY(), Q.toXY(), R.toXY());

        }
        /// <summary>
        /// a constructor with: Center, aRadius, bRadius, Alfa, Beta and ClockWise;
        /// </summary>
        /// <param name="Center"><see cref="Center"/></param>
        /// <param name="ARadius"><see cref="aRadius"/></param>
        /// <param name="BRadius"><see cref="bRadius"/></param>
        /// <param name="Alfa"><see cref="Alfa"/></param>
        /// <param name="Beta"><see cref="Beta"/></param>
        /// <param name="Base">the base of the circle3d. <see cref="Base"/></param>
        /// <param name="ClockWise"><see cref="ClockWise"/></param>
        public Circle3D(Base Base,xyz Center, double ARadius, double BRadius, double Alfa, double Beta, bool ClockWise) : this()
        {
            this.Base = Base;
            Arc = new Arc(Center.toXY(), ARadius, BRadius, Alfa, Beta, ClockWise);

        }
        /// <summary>
        /// Overrides the <see cref="setA"/>-method and saves the value in a local variable.
        /// </summary>
        /// <param name="value">Startpoint</param>
        protected override void setA(xyz value)
        {
            Arc.A = (value).toXY();

        }

        /// <summary>
        /// Gets and sets the orientation of the circle.
        /// </summary>
        public bool Clockwise
        {
            get { return Arc.ClockWise; }
            set { Arc.ClockWise = value; }

        }
        /// <summary>
        /// Overrides the <see cref="setB"/>-method and saves the value in a local variable.
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xyz value)
        {
            Arc.B = (value).toXY();
        }
        /// <summary>
        /// Gets and sets a transformation matrix for the arc
        /// </summary>
        public Matrix3x3 Transformation
        {
            get
            {
                return Arc.Transformation;
            }
            set
            {
                Arc.Transformation = value;

            }
        }
        private Base _Base = Base.UnitBase;
        /// <summary>
        /// This property holds the base for the circle. It will be drawn in the xy-plane
        /// </summary>
        public Base Base { get { return _Base; } set { _Base = value; } }

        /// <summary>
        /// Gets or sets the Radius of the Circle
        /// </summary>
        public double Radius
        {
            get { return Arc.aRadius; }
            set { Arc.aRadius = Arc.bRadius = value; }
        }
        /// <summary>
        /// gets the first angle
        /// </summary>
        public double Alfa
        {
            get { return Arc.Alfa; }
            set { Arc.Alfa = value; }
        }
        /// <summary>
        /// gets the second angle
        /// </summary>
        public double Beta
        {
            get { return Arc.Beta; }
            set { Arc.Beta = value; }
        }
        /// <summary>
        /// Overrides the value function and implement the circle function. For a full Circle you have to take the parameter from 0 to 1.
        /// 1 is equivalent to 2*PI.
        /// </summary>
        /// <param name="t">a Parameter</param>
        /// <returns></returns>
        public override xyz Value(double t)
        {

            return Base.Absolut(Arc.Value(t).toXYZ());

        }
        /// <summary>
        /// invert the circle
        /// </summary>
        public override void Invert()
        {
            Arc.Invert();

        }
        /// <summary>
        /// gets the clockwise sense.
        /// </summary>
        public bool ClockWise
        {
            get { return Arc.ClockWise; }
            set { Arc.ClockWise = value; }

        }


        /// <summary>
        /// Center of the Circle
        /// </summary>

        public xyz Center
        {
            get { return Arc.Center.toXYZ(); }
            set
            {
                Arc.Center = value.toXY();
            }
        }
        /// <summary>
        /// gets a circle withe the two points A and B as end and startpoint with a orientation and
        /// keeps the center.
        /// </summary>
        /// <param name="_A">Start point</param>
        /// <param name="_B">End point</param>
        /// <param name="ClockWise">sense of the circle</param>
        public virtual void SetBorder(xyz _A, xyz _B, bool ClockWise)
        {

           
            double ra = aRadius;
            double rb = bRadius;
            Arc = new Arc(Center.toXY(), Radius, Arc.getAngle(Base.Relativ(_A).toXY()), Arc.getAngle(Base.Relativ(_B).toXY()), ClockWise);
            aRadius = ra;
            bRadius = rb;


        }


        /// <summary>
        /// Overrides the <see cref="Curve3D.Derivation"/>-method.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override xyz Derivation(double t)
        {

            return Base.Absolut(Arc.Derivation(t).toXYZ())-Base.BaseO;

        }
        /// <summary>
        /// overrides the <see cref="Curve3D.Transform(Matrix)"/> method.
        /// </summary>
        /// <param name="m"></param>
        public override void Transform(Matrix m)
        {
            Arc.Transform(m.toMatrix3x3());
          
         
        }
        /// <summary>
        /// Sets anf gets the x-radius of the ellipse
        /// </summary>
        public double aRadius
        {
            get
            {
                return Arc.aRadius;


            }
            set
            {
                Arc.aRadius = value;
            }
        }
        /// <summary>
        /// defines <see cref="aRadius"/> and <see cref="bRadius"/> by keeping the ratio of aRadius and bRadius.
        /// The arc contains the point <b>Pt</b>.
        /// </summary>
        /// <param name="Pt">Point, which lies on the new arc.</param>
        public void SetRadiusbyPoint(xyz Pt)
        {
            Arc.SetRadiusbyPoint(Pt.toXY());
        }
        /// <summary>
        /// Sets and gets the y-radius of the ellipse.
        /// </summary>

        public double bRadius
        {
            get
            {
                return Arc.bRadius;


            }
            set
            {
                Arc.bRadius = value;
            }
        }
    }
}