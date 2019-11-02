using System;
namespace Drawing3d
{
   
    public partial class OpenGlDevice
    {
       private Matrix _ProjectionMatrix = Matrix.identity;
        /// <summary>
        /// GetMethod of the <see cref="ProjectionMatrix"/>.
        /// </summary>
        /// <returns></returns>
        private Matrix getProjectionMatrix()
        {
            return _ProjectionMatrix;
        }
        /// <summary>
        /// gets the world base of the <see cref="ProjectionMatrix"/>. See also <see cref="getProjectionMatrix"/>
        /// </summary>
        /// <returns>the projection base.</returns>
        public Base getProjectionBase()
        {
           return ProjectionMatrix.invert().toBase().Orthogonalyze();
            
        }
        /// <summary>
        /// gets and set the <see cref="ProjectionMatrix"/>. It projects the world coordinates to [-1,1]x[-1,1]x[-1,1]. Only this world coordinates will be drawn.
        /// See also <see cref="ModelMatrix"/>.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get
            {
               return getProjectionMatrix();
            }
            set
            { setProjectionMatrix(value); }
        }
    }
}
