using System;
using System.Collections.Generic;


namespace Drawing3d
{
    /// <summary>
    /// Defines a Curve lying on a Surface by mapping a 2d-Curve to a surface.
    /// It contains a surface <see cref="Mapper"/> and a 2D-Curve <see cref="Curve2d"/>.
    /// The 3D-Curve.value is given by a call to Curve2d.value(t), which gives the parameter
    /// u,v.  The <see cref="Surface.Value(double, double)"/> of u and v retrieves the points of a 
    /// Curve on the surface.
    /// <code>
    ///  xy v = Curve2d.Value(t);
    ///  Mapper.Value(v.x, v.y);
    /// </code>
    /// </summary>
    [Serializable]
    public class MappedCurve : Curve3D
    {
    
        /// <summary>
        /// An empty constructor
        /// </summary>
        public MappedCurve():base()
        {

        }
        /// <summary>
        /// a constructor, which sets the mapper and the curve.
        /// </summary>
        public MappedCurve(Surface Mapper, Curve Curve2d) : base()
        {
            this.Mapper = Mapper;
            this.Curve2d = Curve2d;
        }
        Surface _Mapper = null;
        /// <summary>
        /// Surface, which maps a 2D-Curve to 3D.
        /// </summary>
        public Surface Mapper
        {
            get { return _Mapper; }
            set { _Mapper = value; }
        }

        private Curve _Curve2d = null;
        /// <summary>
        /// A 2D-Curve, which will be mapped by the <see cref="Mapper"/> to a 3D-Curve lying in the
        /// Surface.
        /// </summary>
        public Curve Curve2d
        {
            get { return _Curve2d; }
            set { _Curve2d = value; }
        }
        /// <summary>
        /// Overrides the value function.
        /// <code>
        /// xy v = Curve2d.Value(t);
        /// return Mapper.Value(v.x, v.y);
        /// </code>
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <returns>3DPoint</returns>
        public override xyz Value(double t)
        {

            xy v = Curve2d.Value(t);
            return Mapper.Value(v.x, v.y);
        }
        /// <summary>
        /// Overrides the Derivation.
        /// <code>
        /// 
        ///    xy c = Curve2d.Derivation(t);
        ///    xy v = Curve2d.Value(t);
        ///    return Mapper.uDerivation(v.x, v.y) * c.x + Mapper.vDerivation(v.x, v.y) * c.y; 
        /// 
        /// </code>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override xyz Derivation(double t)
        {

            xy c = Curve2d.Derivation(t);
            xy v = Curve2d.Value(t);
            return Mapper.uDerivation(v.x, v.y) * c.x + Mapper.vDerivation(v.x, v.y) * c.y;
        }
     

    }
  
}