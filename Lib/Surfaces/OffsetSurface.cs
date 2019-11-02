using System;
using System.Collections.Generic;


namespace Drawing3d
{ 
    /// <summary>
    /// is a <see cref="Surface"/> which is <b>distance</b> away from an other surface <see cref="BasisSurface"/>.
    /// </summary>
    [Serializable]
    public class OffsetSurface : Surface
    {
        /// <summary>
        /// overrides <see cref="Surface.CheckPeriodic"/>.
        /// </summary>
        protected override void CheckPeriodic()
        {
            CheckPeriodic();
            UPeriodicity = BasisSurface.UPeriodicity;
            VPeriodicity = BasisSurface.VPeriodicity;
            UpPol = BasisSurface.UpPol;
            DownPol = BasisSurface.DownPol;
            LeftPol = BasisSurface.LeftPol;
            RightPol = BasisSurface.RightPol;
        }

        /// <summary>
        /// is the basic sufarce to which this surface has an offset of <see cref="Distance"/>.
        /// </summary>
        public Surface BasisSurface { get;
            set; }
        /// <summary>
        /// is the offset to the <see cref="BasisSurface"/>
        /// </summary>
        public double Distance { get; set ; }
        /// <summary>
        /// overrides <see cref="Surface.Value(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>point of the surface.</returns>
        public override xyz Value(double u, double v)
        {
           
            xyz Result = new xyz(0, 0, 0);
            if (BasisSurface != null)
            {
                Result = BasisSurface.Value(u, v) + (this.uDerivation(u, v) & this.vDerivation(u, v)).normalized() * Distance;
                if (ZHeight(u, v) > 0)
                   Result = Result + BasisSurface.Normal(u, v) * ZHeight(u, v);

                
            }
            return Result;
           
        }
        /// <summary>
        /// overrides <see cref="Surface.uDerivation(double, double)"/>
        /// </summary>
        /// <summary>
        /// overrides <see cref="Surface.uDerivation(double, double)"/>
        /// </summary>
        /// <param name="u">Specifies the parameter u in [0,1]</param>
        /// <param name="v">Specifies the parameter v in [0,1]</param>
        /// <returns>partial v derivation</returns>
        /// <returns>partial u derivation</returns>
        public override xyz uDerivation(double u, double v)
        {

            return BasisSurface.uDerivation(u, v);
         
        }
        /// <summary>
        /// overrides <see cref="Surface.Normal(double, double)"/>.
        /// </summary>
        /// <param name="u">Specifies the parameter u in [0,1]</param>
        /// <param name="v">Specifies the parameter v in [0,1]</param>
        /// <returns>the normal vector.</returns>
        public override xyz Normal(double u, double v)
        {
           
            if (Distance<0)
            
            return BasisSurface.vDerivation(u, v) & BasisSurface.uDerivation(u, v);
            return BasisSurface.uDerivation(u, v) & BasisSurface.vDerivation(u, v);
        }

        /// <summary>
        /// overrides <see cref="Surface.vDerivation(double, double)"/>
        /// </summary>
        /// <param name="u">Specifies the parameter u in [0,1]</param>
        /// <param name="v">Specifies the parameter v in [0,1]</param>
        /// <returns>partial v derivation</returns>
        public override xyz vDerivation(double u, double v)
        {

            return BasisSurface.vDerivation(u, v);
         
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            OffsetSurface Result = base.Copy() as OffsetSurface;
            Result.BasisSurface = BasisSurface.Copy();
            Result.Distance = Distance;
            return Result;
        }
    }
}