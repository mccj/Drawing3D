using System;

namespace Drawing3d
{
    /// <summary>
    /// This class implements a surface, which is simply a plane.
    /// </summary>
    [Serializable]
    public class PlaneSurface : Surface
    {
        /// <summary>
        /// is the empty constructor it sets the <see cref="Surface.UResolution"/> and <see cref="Surface.UResolution"/> to 1.
        /// </summary>
        public PlaneSurface()
        {
            UResolution = 1;
            VResolution = 1;
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            PlaneSurface Result = base.Copy() as PlaneSurface;
            Result.Width = Width;
            Result.Length = Length;
            return Result;
        }
        //public override void Compile(OpenGlDevice Device)
        //{
        //    bool Inverted = false;
        //    base.Compile(Device);
        //    if (Inverted) BoundedCurves.Invert();
        //}
        /// <summary>
        /// is a constructor, which has thre points of the plane.
        /// </summary>
        /// <param name="A">the first point.</param>
        /// <param name="B">the second point.</param>
        /// <param name="C">the third point.</param>
        public PlaneSurface(xyz A, xyz B, xyz C)
        {
            xyz BA = B - A;
            xyz CA = C - A;
            xyz BaseZ = BA & CA;
            BaseZ = BaseZ.normalized();
            if (BaseZ.length() < 0.000001)
            {
                Base BB = Base.UnitBase;
                BB.BaseO = A;
                Base = BB;
                return;
            }
            xyz BaseX = (CA & BaseZ).normalized();
            xyz BaseY = BaseZ & BaseX;

            Base __Base = new Base();
            __Base.BaseO = A;
            __Base.BaseX = BaseX;
            __Base.BaseY = BaseY;
            __Base.BaseZ = BaseZ;
            Base = __Base;
        }

        /// <summary>
        /// Sets or gets the Width of the Plane by setting or getting the <see cref="Surface.UFactor"/>.
        /// </summary>
        public double Width
        {
            get { return UFactor; }
            set { UFactor = value;
                Invalid = true;
                }
        }
        /// <summary>
        /// sets or gets the length of the plane by setting or getting the <see cref="Surface.VFactor"/>.
        /// </summary>
        public double Length
        {
            get { return VFactor; }
            set { VFactor = value;
                Invalid = true;
            }
        }
        /// <summary>
        /// Overrides the <see cref="Surface.To3dCurve"/> method and converts paramcurves to Curve3D.
        /// For instead only Arc and Line is implemented.
        /// </summary>
        /// <param name="ParamCurve">Specifies the curve in the parameter room</param>
        /// <returns>Returns a 3D-Curve.</returns>
        public override Curve3D To3dCurve(Curve ParamCurve)
        {
            if (ParamCurve is Arc)
            {
                Arc Arc = (ParamCurve as Arc);
                Circle3D C = new Circle3D();
                C.Base = Base;
                
                if (Arc.ClockWise)
                {
                    C.Alfa = Arc.Alfa;
                    C.Beta = Arc.Beta;
                }
                else
                {
                    C.Alfa= Arc.Beta;
                    C.Beta = Arc.Alfa;
                }
                return C;
            }
            if (ParamCurve is Line)
            {
                Line3D L = new Line3D(Value(ParamCurve.A.x, ParamCurve.A.y), Value(ParamCurve.B.x, ParamCurve.B.y));
                return L;
            }
            return null;
        }

        /// <summary>
        /// Overrides the<see cref="Surface.Value(double, double)"/> method.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz Value(double u, double v)
        {
            

            return Base.Absolut(new xyz(u * UFactor, v * VFactor, ZHeight(u, v)));
        }
        /// <summary>
        /// Project a 3DPoint to the plane.
        /// </summary>
        /// <param name="Point">Specifies the point, which will be projected</param>
        /// <returns>Returns the u and v parameters of the projected point</returns>
        public override xy ProjectPoint(xyz Point)
        {
            xyz p = Base.Relativ(Point);
            return new xy(p.x, p.y);
        }
        
        /// <summary>
        /// Overrides the uDerivation function and implements the plane calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz uDerivation(double u, double v)
        {
            return Base.BaseX * UFactor;
        }
        /// <summary>
        /// Overrides the vDerivation function and implements the plane calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz vDerivation(double u, double v)
        {
            return Base.BaseY * VFactor;
        }
        /// <summary>
        /// Overrides the <see cref="Normal"/> method and returns Base.BaseZ*(-1);
        /// </summary>
        /// <param name="u">Specifies the parameter u.</param>
        /// <param name="v">Specifies the parameter v.</param>
        /// <returns>The normal vektor of the plane.</returns>
        public override xyz Normal(double u, double v)
        {
           
            if (SameSense)
                return Base.BaseZ;
            else
                return Base.BaseZ * (-1);
        }
    }
}