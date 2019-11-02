
using System.Drawing;

using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace Drawing3d
{
    /// <summary>
    /// is a class derived from <see cref="Mesh"/>. She is a plane, which has an origin by <see cref="x"/> and <see cref="y"/> and a size width <see cref="Width"/> and <see cref="Height"/>.
    /// You can draw it by quads or by triangles. See <see cref="UseQuads"/>. It is easy to represent a <b>highmaps</b> width <see cref="SetHighMap(Bitmap)"/> and <see cref="HightScale"/>.
    /// </summary>
    public class Terrain : Mesh
    {
        int[] TriangleIndices = null;
        int[] getTriangleIndices()
        {
            int[] Result = new int[(UResolution) * (VResolution) * 6];
            for (int u = 0; u < UResolution; u++)
            {

                for (int v = 0; v < VResolution; v++)
                {

                    Result[6 * u * (VResolution) + 6 * v] = (u * (VResolution + 1) + v);
                    Result[6 * u * (VResolution) + 6 * v + 1] = (u * (VResolution + 1) + v + 1);
                    Result[6 * u * (VResolution) + 6 * v + 2] = ((u + 1) * (VResolution + 1) + v);
                    Result[6 * u * (VResolution) + 6 * v + 3] = ((u + 1) * (VResolution + 1) + v);
                    Result[6 * u * (VResolution) + 6 * v + 4] = ((u) * (VResolution + 1) + v + 1);
                    Result[6 * u * (VResolution) + 6 * v + 5] = ((u + 1) * (VResolution + 1) + v + 1);

                }

            }

            return Result;

        }
        xyzf _Origin = new xyzf(0, 0, 0);
        /// <summary>
        /// gets and sets the left down point of the terrain.
        /// </summary>
        public xyzf Origin
        {
            get { return _Origin; }
            set { _Origin = value;
                RefreshVertices();
            }
        }
        void RefreshVertices()
        {
            Position = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Normals = new xyzf[(UResolution + 1) * (VResolution + 1)];

            Rectangled R = new Rectangled(0, 0, 1, 1);
            double xStep = (float)1 / (float)(UResolution);
            double yStep = (float)1 / (float)(VResolution);
            xyzf _Point = new xyzf(0, 0, 0);
            xyzf _Normal = new xyzf(0, 0, 0);
            float ustep = (float)(Width / (float)UResolution);
            float vstep = (float)(Height / (float)VResolution);
            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                for (int v = 0; v < VResolution + 1; v++)
                {
                    
                    Position[URow + v] = new xyzf((float)Origin.x + u * ustep, (float)Origin.y + v * vstep, (float)Origin.z);

                }

            }
            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                for (int v = 0; v < VResolution + 1; v++)
                {
                    Normals[URow + v] = new xyzf(0, 0, 1);
               }

            }
            TextureCoords = new xyf[(UResolution + 1) * (VResolution + 1)];
            ustep = (float)(1 / (float)UResolution);
            vstep = (float)(1 / (float)VResolution);
            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                for (int v = 0; v < VResolution + 1; v++)
                {
                    TextureCoords[URow + v] = new xyf(u * ustep, v * vstep);

                }
            }

            if (UseQuads)
            {
                Indices = new int[UResolution * VResolution * 4];
                int ID = 0;
                for (int u = 0; u < UResolution; u++)
                {
                    int URow = u * (VResolution + 1);
                    for (int v = 0; v < VResolution; v++)
                    {
                        Indices[ID] = u * (VResolution + 1) + v;
                        Indices[ID + 1] = u * (VResolution + 1) + v + 1;
                        Indices[ID + 2] = (u + 1) * (VResolution + 1) + v + 1;
                        Indices[ID + 3] = (u + 1) * (VResolution + 1) + v;
                        ID += 4;
                    }
                }
                TriangleIndices = getTriangleIndices();

            }
            else
            {
                Indices = getTriangleIndices();

            }
        }
       
        
        double _Width = 10;
        /// <summary>
        /// represents the width of the terrain.
        /// </summary>
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                RefreshVertices();

            }
        }
        double _Height = 10;
        /// <summary>
        /// represents the height of the terrain.
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                RefreshVertices();
            }
        }
        int _UResolution = 40;
        /// <summary>
        /// gets and sets the reulution for the x direction.
        /// </summary>
        public int UResolution
        {
            get { return _UResolution; }
            set
            {
                _UResolution = value;
                RefreshVertices();
            }
        }
        int _VResolution = 40;
        /// <summary>
        /// gets and sets the reulution for the v direction.
        /// </summary>
        public int VResolution
        {
            get { return _VResolution; }
            set
            {
                _VResolution = value;
                RefreshVertices();
            }
        }
        float _HightScale = 200;
        /// <summary>
        /// scales the height. This means the red channel of the bitmap will be devided by <b>HightScale</b>. The default is 200.
        /// </summary>
        public float HightScale
        {
            get { return _HightScale; }
            set
            {
                _HightScale = value;
            }
        }

        bool _UseQuads = false;
        /// <summary>
        /// get and sets a value for using <b>quads</b> instead of <b>triangles</b>. The default is <b>false</b>.
        /// </summary>
        public bool UseQuads
        {
            get { return _UseQuads; }
            set
            {
                Primitives = BeginMode.Quads;
                _UseQuads = value;

            }
        }

        /// <summary>
        /// activates a <b>high map</b>. The red channel of each texel will be divided by <see cref="HeighScale"/> and setted as z-coordinate if the <see cref="Terrain"/>.
        /// </summary>
        /// <param name="HighBitmap">is the Bitmap, which holds the inormatios about the height.</param>
        public void SetHighMap(Bitmap HighBitmap)
        {
            UResolution = HighBitmap.Width;
            VResolution = HighBitmap.Height;
            BitmapData SourceData = HighBitmap.LockBits(new Rectangle(0, 0, HighBitmap.Width, HighBitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int SourceStride = SourceData.Stride;

            int H = HighBitmap.Height;
            int W = HighBitmap.Width;
            unsafe
            {

                byte* SourcePtr = (byte*)(void*)SourceData.Scan0;
                int Ydisp = SourceStride * (H - 1);

                for (int _y = 0; _y < H; _y++)
                {
                    for (int _x = 0; _x < W; _x++)
                        Position[_y + (UResolution + 1) * _x].z = (float)SourcePtr[_x * 4 + 0 + Ydisp] / HightScale;

                    Ydisp = Ydisp - SourceStride;
                }
            }

            HighBitmap.UnlockBits(SourceData);

        }

     
       
    }
}
