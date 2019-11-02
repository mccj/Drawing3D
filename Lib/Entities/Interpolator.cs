using System;
namespace Drawing3d
{
    /// <summary>
    /// This class interpolates an array of 3d-points by <see cref="BSplineSurface"/>s of degree 3.
    /// </summary>
    [Serializable]
    public class Interpolator3D:Entity
    {
        int _UResolution = 10;
        int _VResolution = 10;
        /// <summary>
        /// gets and sets the u-resolution of the <see cref="BSplineSurface"/>. The default id 10.
        /// /// </summary>


        public int UResolution
        {
            get { return _UResolution; }
            set
            {
                _UResolution = value;
             }
        }
        /// <summary>
        /// gets and sets the v-resolution of the <see cref="BSplineSurface"/>. The default id 10.
        /// /// </summary>
        public int VResolution
        {
            get { return _VResolution; }
            set
            {
                _VResolution = value;
            }
        }

        xyz[,] _Points = new xyz [0,0];
        /// <summary>
        /// gets and sets an array of <see cref="xyz"/>points.
        /// /// </summary>
        public xyz[,] Points
        {
            get { return _Points; }
            set
            {
                _Points = value;
                InitializeBSplineSurfaces();
                Invalid = true;
            }
        }
        BSplineSurface[,] _BSplineSurfaces = new BSplineSurface[0,0];
        /// <summary>
        /// gets the <see cref="BSplineSurface"/> indexed by the left down point in <see cref="Points"/>.
        /// </summary>
        public BSplineSurface[,] BSplineSurfaces
        { get { return _BSplineSurfaces; } }
        BSplineSurface CreateSurface()
        {
            BSplineSurface Result = new BSplineSurface();
            Result.UDegree = 3;
            Result.VDegree = 3;
            Result.ControlPoints = new xyz[4, 4];
            Result.VKnots = Utils.StandardKnots(Result.ControlPoints.GetLength(1), 3);
            Result.UKnots = Utils.StandardKnots(Result.ControlPoints.GetLength(0), 3);
            Result.UResolution = UResolution;
            Result.VResolution = VResolution;
            return Result;
        }
     
        double factor = 0.5;
        double factor2 = 1;
        /// <summary>
        /// overrides <see cref="SetInvalid(bool)"/> to pass the invalidation to all surfaces.
        /// </summary>
        /// <param name="value"></param>
        protected override void SetInvalid(bool value)
        {
           
            base.SetInvalid(value);
        }
        
   
        void InitializeBSplineSurfaces()
        {
            _BSplineSurfaces = new BSplineSurface[Points.GetLength(0) - 1, Points.GetLength(1) - 1];
            for (int i = 0; i < BSplineSurfaces.GetLength(0); i++)
            {
                for (int j = 0; j < BSplineSurfaces.GetLength(1); j++)
                {
                    BSplineSurfaces[i, j] = CreateSurface();
                    BSplineSurface B = BSplineSurfaces[i, j];

                    B.ControlPoints[0, 0] = Points[i, j];
                    B.ControlPoints[3, 0] = Points[i + 1, j];
                    B.ControlPoints[0, 3] = Points[i, j + 1];
                    B.ControlPoints[3, 3] = Points[i + 1, j + 1];
                    double L = B.ControlPoints[0, 3].dist(B.ControlPoints[0, 0]);
                    B.ControlPoints[0, 1] = Points[i, j];
                    if (j > 0)
                        B.ControlPoints[0, 1] = Points[i, j] + (Points[i, j + 1] - Points[i, j - 1]).normalized() * L * factor;
                    B.ControlPoints[0, 2] = Points[i, j + 1];
                    if (j + 2 < Points.GetLength(1))
                        B.ControlPoints[0, 2] = Points[i, j + 1] - (Points[i, j + 2] - Points[i, j]).normalized() * L * factor;

                    L = B.ControlPoints[3, 0].dist(B.ControlPoints[3, 3]);
                    B.ControlPoints[3, 1] = Points[i + 1, j];
                    if (j > 0)
                        B.ControlPoints[3, 1] = Points[i + 1, j] + (Points[i + 1, j + 1] - Points[i + 1, j - 1]).normalized() * L * factor;
                    B.ControlPoints[3, 2] = Points[i + 1, j + 1];
                    if (j + 2 < Points.GetLength(1))
                        B.ControlPoints[3, 2] = Points[i + 1, j + 1] - (Points[i + 1, j + 2] - Points[i + 1, j]).normalized() * L * factor;

                    L = B.ControlPoints[0, 0].dist(B.ControlPoints[3, 0]);
                    B.ControlPoints[1, 0] = Points[i, j];
                    if (i > 0)
                        B.ControlPoints[1, 0] = Points[i, j] + (Points[i + 1, j] - Points[i - 1, j]).normalized() * L * factor;


                    B.ControlPoints[2, 0] = Points[i + 1, j];
                    if (i + 2 < Points.GetLength(0))
                        B.ControlPoints[2, 0] = Points[i + 1, j] - (Points[i + 2, j] - Points[i, j]).normalized() * L * factor;
                    L = B.ControlPoints[0, 3].dist(B.ControlPoints[3, 3]);
                    B.ControlPoints[1, 3] = Points[i, j + 1];

                    if (i > 0)
                        B.ControlPoints[1, 3] = Points[i, j + 1] + (Points[i + 1, j + 1] - Points[i - 1, j + 1]).normalized() * L * factor;
                    B.ControlPoints[2, 3] = Points[i + 1, j + 1];
                    if (i + 2 < Points.GetLength(0))
                        B.ControlPoints[2, 3] = Points[i + 1, j + 1] - (Points[i + 2, j + 1] - Points[i, j + 1]).normalized() * L * factor;
                    B.ControlPoints[1, 1] = B.ControlPoints[0, 1] + (B.ControlPoints[1, 0] - B.ControlPoints[0, 0]) * factor2;
                    B.ControlPoints[2, 1] = B.ControlPoints[3, 1] + (B.ControlPoints[2, 0] - B.ControlPoints[3, 0]) * factor2;
                    B.ControlPoints[1, 2] = B.ControlPoints[0, 2] + (B.ControlPoints[1, 3] - B.ControlPoints[0, 3]) * factor2;
                    B.ControlPoints[2, 2] = B.ControlPoints[3, 2] + (B.ControlPoints[2, 3] - B.ControlPoints[3, 3]) * factor2;
                }
            }
        }
        /// <summary>
        /// override <see cref="OnDraw(OpenGlDevice)"/> by calling the draw-method of the surfaces.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            for (int i = 0; i <BSplineSurfaces.GetLength(0); i++)
            {
                for (int j = 0; j <BSplineSurfaces.GetLength(1); j++)
                {
                    BSplineSurfaces[i, j].Paint(Device);
                    if (Device.RenderKind== RenderKind.SnapBuffer)
                    {
                        Selector.StoredSnapItems[Selector.StoredSnapItems.Count - 1].OfObject = this;
                    }
                }
            }
            base.OnDraw(Device);
        }
    }
}
