using System;

namespace Drawing3d
{
    /// <summary>
    /// implements a box as <see cref="DiscreteSolid"/> with <see cref="Origin"/> and the <see cref="Size"/>.
    /// </summary>
    [Serializable]
    public class SolidBox : DiscreteSolid
    {
        private xyz _Size = new xyz(1, 1, 1);
        /// <summary>
        /// Size of the box. Default is 1,1,1.
        /// </summary>
        public xyz Size
        {
            get { return _Size; }
            set
            {

                _Size = value;
                if (VertexList.Count >= 8)
                {
                    A.Value = xyz.Null;
                    B.Value = new xyz(Size.x, 0, 0);
                    C.Value = new xyz(Size.x, Size.y, 0);
                    D.Value = new xyz(0, Size.y, 0);
                    E.Value = A.Value + new xyz(0, 0, Size.z);
                    F.Value = B.Value + new xyz(0, 0, Size.z);
                    G.Value = C.Value + new xyz(0, 0, Size.z);
                    H.Value = D.Value + new xyz(0, 0, Size.z);
                    for (int i = 0; i < FaceList.Count; i++)
                    {
                        Base Bas = FaceList[i].Surface.Base;
                        Bas.BaseO = FaceList[i].Bounds[0][0].EdgeStart.Value;
                        FaceList[i].Surface.Base = Bas;
                        // Faces[i].SetCurveBorder();
                    }
                }
            }
        }
        Vertex3d A { get { return VertexList[0]; } }
        Vertex3d B { get { return VertexList[1]; } }
        Vertex3d C { get { return VertexList[2]; } }
        Vertex3d D { get { return VertexList[3]; } }
        Vertex3d E { get { return VertexList[4]; } }
        Vertex3d F { get { return VertexList[5]; } }
        Vertex3d G { get { return VertexList[6]; } }
        Vertex3d H { get { return VertexList[7]; } }
        /// <summary>
        /// an empty constructor. Default for the size is 1,1,1. The default <see cref="Origin"/> is 0,0,0.
        /// </summary>
        public SolidBox()
        {
            Model = Model.Solid;
            
       }
    

        /// <summary>
        /// constructor with <see cref="Size"/>.The <see cref="Origin"/> is 0,0,0.
         /// </summary>
        /// <param name="Size"><see cref="Size"/> of the box.</param>
        public SolidBox(xyz Size):this()
        {
            _Size = Size;
            Refresh();
        }
        /// <summary>
        /// constructor with <see cref="Origin"/> and <see cref="Size"/> .
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Size"></param>
        public SolidBox(xyz Origin,xyz Size):this()
        {
           
            _Size = Size;
            Transformation = Matrix.Translation(Origin);
            Refresh();

        }
        /// <summary>
        /// the origin of the box.
        /// </summary>
        public xyz Origin
        { get { return Transformation * new xyz(0, 0, 0); }
          set { Transformation = Matrix.Translation(value); }


        }
        /// <summary>
        /// overrides the <see cref="Solid.Refresh()"/> method.
        /// </summary>
       public override void Refresh()
       {
            VertexList.Clear();
            EdgeList.Clear();
            FaceList.Clear();
            EdgeCurveList.Clear();

            Vertex3d A = new Vertex3d(xyz.Null);
            Vertex3d B = new Vertex3d(new xyz(Size.x, 0, 0));
            Vertex3d C = new Vertex3d(new xyz(Size.x, Size.y, 0));
            Vertex3d D = new Vertex3d(new xyz(0, Size.y, 0));
            Vertex3d E = new Vertex3d(A.Value + new xyz(0, 0, Size.z));
            Vertex3d F = new Vertex3d(B.Value + new xyz(0, 0, Size.z));
            Vertex3d G = new Vertex3d(C.Value + new xyz(0, 0, Size.z));
            Vertex3d H = new Vertex3d(D.Value + new xyz(0, 0, Size.z));
            VertexList.Add(A);
            VertexList.Add(B);
            VertexList.Add(C);
            VertexList.Add(D);
            VertexList.Add(E);
            VertexList.Add(F);
            VertexList.Add(G);
            VertexList.Add(H);

            Vertex3dArray_2 Border = new Vertex3dArray_2();
            Vertex3dArray VA = new Vertex3dArray();
            Border.Add(VA);
            Face Face = null;
            VA.Clear();
            VA.Add(A);
            VA.Add(B);
            VA.Add(C);
            VA.Add(D);
            Face = Face.SolidPlane(this, Border);
            // FaceList.Add(Face);

            VA.Clear();
            VA.Add(A);
            VA.Add(E);
            VA.Add(F);
            VA.Add(B);

            Face = Face.SolidPlane(this, Border);
            // FaceList.Add(Face);

            VA.Clear();
            VA.Add(B);
            VA.Add(F);
            VA.Add(G);
            VA.Add(C);
            Face = Face.SolidPlane(this, Border);
            //  FaceList.Add(Face);

            VA.Clear();
            VA.Add(C);
            VA.Add(G);
            VA.Add(H);
            VA.Add(D);
            Face = Face.SolidPlane(this, Border);
            //  FaceList.Add(Face);

            VA.Clear();
            VA.Add(D);
            VA.Add(H);
            VA.Add(E);
            VA.Add(A);
            Face = Face.SolidPlane(this, Border);
            //   FaceList.Add(Face);

            VA.Clear();
            VA.Add(H);
            VA.Add(G);
            VA.Add(F);
            VA.Add(E);
            Face = Face.SolidPlane(this, Border);
            //for (int i = 0; i < FaceList.Count; i++)
            //{
            //    FaceList[i].DrawRelativToSurfaceBase = false;
            //    FaceList[i].Refresh();
            //}

        }
       
    }
}
