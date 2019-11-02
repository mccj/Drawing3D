using System;


namespace Drawing3d
{
    /// <summary>
    /// The <b>BezierSurface</b> is the base class for <see cref="BSplineSurface"/> and implements
    /// <see cref="BezierSurface.ControlPoints"/>, which defines the surface by calculating the 3D-Bezierfunction.
    /// <seealso cref="Utils.BezierSurfacePt(xyz[,],double, double)"/>
    /// <remarks>The count of controlpoints is minimal 3 and maximal 4 for the BezierSurface</remarks>
    /// </summary>
    [Serializable]
    public class BezierSurface : Surface
    {
        private xyz[,] _ControlPoints = { { new xyz(0, 0, 0), (new xyz(0, 0, 0)) } };
        /// <summary>
        /// Sets or gets the Controlpoints, which determe the geometric outlook.
        /// In case of BazierSurface they are limited to 4. For <see cref="BSplineSurface"/> ther is no limit.
        /// </summary>
        public xyz[,] ControlPoints
        {
            get { return _ControlPoints; }
            set
            {
                _ControlPoints = value;
                 Invalid = true;
                 CheckPeriodic();
            }
        }
        /// <summary>
        /// Constructor with ControlPoints as parameter
        /// </summary>
        /// <param name="Points"></param>
        public BezierSurface(xyz[,] Points)
        {
            this.ControlPoints = Points;
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public BezierSurface()
        { }
        /// <summary>
        /// overrides the <see cref="Value(double, double)"/> method.
        /// </summary>
        /// <param name="u">the u parameter.</param>
        /// <param name="v">the v parameter.</param>
        /// <returns></returns>
        public override xyz Value(double u, double v)
        {
            if (ZHeight(u,v)!=0)
            return Base.Absolut(Utils.BezierSurfacePt(ControlPoints, u, v)+Normal(u,v)*ZHeight(u,v));
            else
            return Base.Absolut(Utils.BezierSurfacePt(ControlPoints, u, v));
        }
        /// <summary>
        /// Calulates the u-derivation of a bezierfunction for the two parameters u and v which are given in the interval [0, 1]
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>u-derivation</returns>
        public override xyz uDerivation(double u, double v)
        {
            return Base.Absolut(Utils.BezierSurfaceDeriveU(ControlPoints, u, v)) - Base.BaseO;
        }
        /// <summary>
        /// Calulates the v-derivation of a bezierfunction for the two parameters u and v which are given in the interval [0, 1]
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>v-derivation</returns>
        public override xyz vDerivation(double u, double v)
        {
            return Base.Absolut(Utils.BezierSurfaceDeriveV(ControlPoints, u, v)) - Base.BaseO;
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            BezierSurface Result = base.Copy() as BezierSurface;
            Result.ControlPoints = ControlPoints.Clone() as xyz[,];
            return Result;
        }
    }
}