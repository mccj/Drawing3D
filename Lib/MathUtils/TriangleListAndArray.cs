using System;
using System.Collections.Generic;
using System.Text;
using Drawing3d;
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
   
{
    /// <summary>
    /// is a class, which constains lists of <b>Indices</b>, <b>Normals</b>, <b>Points</b> and <b>Texture</b>.
    /// </summary>
    [Serializable]
    public class TriangleList
    {
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public TriangleList()
        {
        }
        /// <summary>
        /// a list of <b>Indices</b>.
        /// </summary>
        public List<IndexType> Indices = new List<IndexType>();
        /// <summary>
        /// a list of <b>Normals</b>.
        /// </summary>
        public List<xyzf> Normallist = new List<xyzf>();
        /// <summary>
        /// a list of <b>Textures</b>.
        /// </summary>
        public List<xyf> Texturelist = new List<xyf>();
        /// <summary>
        /// a list of <b>Points</b>.
        /// </summary>
        public List<xyzf> Pointslistf = new List<xyzf>();
        /// <summary>
        /// a list of <b>Colors</b>.
        /// </summary>
        public List<xyzf> ColorList = new List<xyzf>();
        /// <summary>
        /// adds the lists to a <see cref="TriangleArrays"/> R.
        /// </summary>
        /// <param name="R">the <see cref="TriangleArrays"/></param>
        public void AddToList(TriangleArrays R)
        {
            int ct = Pointslistf.Count;
            if (R.Indices!=null)
            for (int i = 0; i < R.Indices.Length; i++)
                Indices.Add((IndexType)(ct + R.Indices[i]));
            if (R.Points != null)
                for (int i = 0; i < R.Points.Length; i++)
                Pointslistf.Add(/*Device.ModelMatrix**/R.Points[i]);
            if (R.Texture != null)
                for (int i = 0; i < R.Texture.Length; i++)
                Texturelist.Add(R.Texture[i]);
            if (R.Normals != null)
                for (int i = 0; i < R.Normals.Length; i++)
                Normallist.Add(/*Device.ModelMatrix.multaffin*/(R.Normals[i]));
            if (R.Colors != null)
                for (int i = 0; i < R.Colors.Length; i++)
                ColorList.Add(R.Colors[i]);

        }
        /// <summary>
        /// converts a <see cref="TriangleList"/> to an <see cref="TriangleArrays"/>.
        /// </summary>
        /// <returns><see cref="TriangleArrays"/>.</returns>
        public TriangleArrays ToArrays()
        {
            return new TriangleArrays(Indices.ToArray(), Pointslistf.ToArray(), Normallist.ToArray(),ColorList.ToArray(), Texturelist.ToArray());
          
        }
    }
    /// <summary>
    /// is a class, which constains arrays of <b>Indices</b>, <b>Normals</b>, <b>Points</b> and <b>Texture</b>.
    /// </summary>
    public class TriangleArrays
    {
        /// <summary>
        /// is the constructor
        /// </summary>
        /// <param name="Indices">array of Indices.</param>
        /// <param name="Points">array of Points.</param>
        /// <param name="Normals">array of Normals.</param>
        /// <param name="Colors">array of Colors.</param>
        /// <param name="Texture">array  of Textures.</param>
        public TriangleArrays(IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyzf[] Colors, xyf[] Texture)
        {

            this.Indices = Indices;
            this.Points = Points;
            this.Normals = Normals;
            this.Texture = Texture;
            this.Colors = Colors;
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public TriangleArrays()
        { }
        /// <summary>
        /// an array of Indices.
        /// </summary>
        public IndexType[] Indices = new IndexType[0];
        /// <summary>
        /// an array of points.
        /// </summary>
        public xyzf[] Points = new xyzf[0];
        /// <summary>
        /// an array of normals.
        /// </summary>
        public xyzf[] Normals = new xyzf[0];
        /// <summary>
        /// an array of colors.
        /// </summary>
        public xyzf[] Colors = new xyzf[0];
        /// <summary>
        /// an array of texture coordinates.
        /// </summary>
        public xyf[] Texture = new xyf[0];
     

    }
}
