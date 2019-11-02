using System;
using System.Windows.Forms;


namespace Drawing3d
{  
    /// <summary>
    /// is an abstract <see cref="Surface"/>, who implements <see cref="INurbs3d"/>.
    /// </summary>
    [Serializable]
    public abstract class PrimitiveSurface : Surface, INurbs3d
    {




        /// <summary>
        /// implementiert <see cref="INurbs3d.getUDegree"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract int getUDegree();

        /// <summary>
        /// implementiert <see cref="INurbs3d.getVDegree"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract int getVDegree();

        /// <summary>
        /// implementiert <see cref="INurbs3d.getCtrlPoints"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract xyz[,] getCtrlPoints();

        /// <summary>
        /// implementiert <see cref="INurbs3d.getUKnots"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract double[] getUKnots();

        /// <summary>
        /// implementiert <see cref="INurbs3d.getVKnots"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract double[] getVKnots();

        /// <summary>
        /// implementiert <see cref="INurbs3d.getWeights"/> as abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract double[,] getWeights();
       

    }
}