using System.Windows.Forms;
namespace Drawing3d
{
    public partial class OpenGlDevice
    {
      /// <summary>
      /// internal
      /// </summary>
      /// <returns></returns>
        internal Plane[] ClippingPlane = new Plane[6];
        internal float[] getClippingPlanes()
        {
            float[] result = new float[6 * 4];
            for (int i = 0; i < 6; i++)
            {
                if (ClippingPlane[i] != null)
                {
                    result[i * 4] = (float)ClippingPlane[i].NormalUnit.x;
                    result[i * 4 + 1] = (float)ClippingPlane[i].NormalUnit.y;
                    result[i * 4 + 2] = (float)ClippingPlane[i].NormalUnit.z;
                    double d = ClippingPlane[i].NormalUnit * ClippingPlane[i].P;
                    result[i * 4 + 3] = (float)d;
                }
            }
            return result;
        }
        /// <summary>
        /// clipps the scene with the plane.It will be drawn only this side of the plane in that the normal vector shows.
        /// </summary>
        /// <param name="Index">index of the clippingplane:0..5 are allowed.</param>
        /// <param name="value">the plane, which works as clipping plane. A value = null removes the clipping plane.</param>
        public  void SetClippingPlane(int Index,Plane value)
        {
            if (value==null)
            { ClippEnable(Index, false);
                return;
            }
            ClippEnabled[Index] = 1;
            ClippingPlane[Index] = value;
            if (Shader != null)
            {
               
                   Field CE = Shader.getvar("ClippingPlane");
                if (CE != null) CE.Update();


            }
           
        }
        /// <summary>
        /// internal
        /// </summary>
        internal int[] ClippEnabled = new int[6];

        private void ClippEnable(int Id, bool On)
        {
            if (On)
                ClippEnabled[Id] = 1;
            else
                ClippEnabled[Id] = 0;
            if (Shader != null)
            { 
            Field CE = Shader.getvar("ClippingPlaneEnabled");
            if (CE != null) CE.Update();
           }
        
            OpenGlDevice.CheckError();
        }
    }
}
