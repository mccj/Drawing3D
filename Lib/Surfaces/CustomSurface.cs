using System;

namespace Drawing3d
{
    /// <summary>
    /// is the definition of an evevent, which is used from <see cref="CustomSurface.OnGetValue"/>, <see cref="CustomSurface.OnGetUDerivation"/> and <see cref="CustomSurface.OnGetVDerivation"/>.
    /// </summary>
    /// <param name="sender">is the <see cref="Surface"/></param>
    /// <param name="u">first parameter.</param>
    /// <param name="v">second parameter.</param>
    /// <returns></returns>
    public delegate xyz RealFunction2d3d(object sender, Double u, double v);
    /// <summary>
    /// is a <see cref="Surface"/>,which can be self modelled by setting the events
    /// <see cref="OnGetValue"/>, <see cref="OnGetUDerivation"/> and <see cref="OnGetVDerivation"/>.
    /// E.g.: you define <br/>
    /// private xyz CustomSurface_OnGetValue(object sender, double u, double v)<br/>
    ///    {<br/>
    ///double x = 5 * System.Math.Cos(u * 2 * Math.PI);<br/>
    ///double y = 5 * System.Math.Sin(u * 2 * Math.PI);<br/>
    ///double z = 4 * v;<br/>
    ///        return new xyz(x, y, z);<br/>
    ///}<br/>
    ///<br/>
    ///private xyz CustomSurface_OnGetVDerivation(object sender, double u, double v)<br/>
    ///<br/>
    ///double x = -5 * System.Math.Sin(u * 2 * Math.PI);<br/>
    ///double y = 5 * System.Math.Cos(u * 2 * Math.PI);<br/>
    ///double z = 0;<br/>
    /// new xyz(x, y, z);<br/>
    ///}<br/>
    ///<br/>
    ///private xyz CustomSurface_OnGetUDerivation(object sender, double u, double v)<br/>
    ///{<br/>
    ///return (new xyz(0, 0, 1));<br/>
    ///}<br/>
    /// <br/>
    /// you get a cylinder.
    /// </summary>
    [Serializable]
    public class CustomSurface : Surface
    {
       /// <summary>
       /// this event is call from <see cref="Surface.Value(xyf)"/>.
       /// </summary>
        public event RealFunction2d3d OnGetValue;
        /// <summary>
        /// this event is call from <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        public event RealFunction2d3d OnGetUDerivation;
        /// <summary>
        /// this event is call from <see cref="Surface.vDerivation(double, double)"/>.
        /// </summary>
        public event RealFunction2d3d OnGetVDerivation;

        /// <summary>
        /// overrides <see cref="Surface.Value(xyf)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns></returns>
        public override xyz Value(double u, double v)
        {
            if (OnGetValue != null) return OnGetValue(this, u, v);
            return xyz.Null;
        }
        /// <summary>
        /// overrides <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>gives the partial u derivation.</returns>
        public override xyz uDerivation(double u, double v)
        {
            if (OnGetUDerivation != null) return OnGetUDerivation(this, u, v);
            return xyz.Null;
        }
        /// <summary>
        /// overrides <see cref="Surface.vDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>gives the partial v derivation.</returns>
        public override xyz vDerivation(double u, double v)
        {
            if (OnGetVDerivation != null) return OnGetVDerivation(this, u, v);
            return xyz.Null;
        }
    }
}
