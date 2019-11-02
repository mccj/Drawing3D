
using System;
using System.Collections.Generic;

#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// is used from <see cref="MeshCreator"/>
    /// </summary>

    /// <summary>
    /// is responsible for <b>compiling</b>. See also <see cref="Compile(OpenGlDevice)"/>
    /// </summary>
    [Serializable]
    public class MeshContainer : Entity
    {   
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="M"></param>
        internal new void Transform(Matrix M)
        {
            for (int i = 0; i < Progs.Count; i++)
            {
                Mesh Me = Progs[i] as Mesh;
                if (Me != null)
                {

                    Me.Transform(M);
               
                }
            }
        }
        /// <summary>
        /// internal.
        /// </summary>
        public List<CustomEntity> Progs = new List<CustomEntity>();
        /// <summary>
        /// overrides the <b>Compile</b> method.
        /// </summary>
        /// <param name="Device">device, in which it will be drawn.</param>
        public override void Compile(OpenGlDevice Device)
        {
            setMesh(MeshCreator.CreateMeshes(Device, _Ondraw));

        }
        /// <summary>
        /// overrides this method by giving free internal meshes.
        /// </summary>
        public override void Dispose()
        {
            for (int i = 0; i < Progs.Count; i++)
            {
                Mesh M = Progs[i] as Mesh;
                if (M != null) M.Dispose();
            }
            base.Dispose();
        }
        /// <summary>
        /// override this method by drawing his (compiled) progs.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {


            for (int i = 0; i <Progs.Count; i++)
            {
                Progs[i]._Ondraw(Device);
            }

        }
        /// <summary>
        /// internal.
        /// </summary>
        public MeshContainer()
        {
            CompileEnable = false;
        }
    }
    /// <summary>
    /// is responsible for <b>compiling</b>. See also <see cref="CreateMeshes(OpenGlDevice, DrawAction)"/>, <see cref="Entity.Compiling"/>.
    /// <b>CreateMeshes</b> is the only not internal method.
    /// </summary>
    [Serializable]
    public class MeshCreator
    {
        /// <summary>
        /// is used from <see cref="MeshCreator"/>
        /// </summary>
        [Serializable]
        class MeshCreatorItem
        {
            List<xyzf> MeshVertices = null;
            List<xyzf> MeshNormals = null;
            List<xyf> MeshTextureCoords = null;
            List<IndexType> MeshIndices = null;
          public  MeshContainer MeshListCurrent = null;
    
            float PenWidth = 1;
           
            PolygonMode _MeshMode = PolygonMode.Fill;
            public MeshCreatorItem(
                List<xyzf> MeshVertices,
                List<xyzf> MeshNormals,
                List<xyf> MeshTextureCoords,
                List<IndexType> MeshIndices,
                MeshContainer MeshListCurrent,
                
                float PenWidth,
                SnappItem SnapItem,
                PolygonMode _MeshMode,
                List<SnappItem> StoredSnapItems)
            {
                this.MeshVertices = MeshVertices;
                this.MeshNormals = MeshNormals;
                this.MeshTextureCoords = MeshTextureCoords;
                this.MeshIndices = MeshIndices;
                this.MeshListCurrent = MeshListCurrent;
             
                this.PenWidth = PenWidth;
              
                this._MeshMode = PolygonMode.Fill;

            }
            /// <summary>
            /// Constructor, which is only for use in <see cref="MeshCreator"/>
            /// </summary>
            public MeshCreatorItem() : this(
                MeshCreator.MeshVertices,
                MeshCreator.MeshNormals,
                MeshCreator.MeshTextureCoords,
                MeshCreator.MeshIndices,
                MeshCreator.MeshListCurrent,
                
                MeshCreator.PenWidth,
                MeshCreator.SnapItem,
                MeshCreator._MeshMode,


                Selector.StoredSnapItems)
            { }
            /// <summary>
            /// internal.
            /// </summary>
            internal void Reset()
            {
                MeshCreator.MeshVertices = MeshVertices;
                MeshCreator.MeshNormals = MeshNormals;
                MeshCreator.MeshTextureCoords = MeshTextureCoords;
                MeshCreator.MeshIndices = MeshIndices;
                MeshCreator.MeshListCurrent = MeshListCurrent;
              
                MeshCreator.PenWidth = PenWidth;
                MeshCreator._MeshMode = _MeshMode;
            }
        }
        static System.Collections.Stack S = new System.Collections.Stack();
        /// <summary>
        /// internal.
        /// </summary>
        internal static void Push()
        {
            MeshCreatorItem MC = new MeshCreatorItem(MeshCreator.MeshVertices, MeshCreator.MeshNormals, MeshCreator.MeshTextureCoords,MeshIndices, MeshCreator.MeshListCurrent, MeshCreator.PenWidth, MeshCreator.SnapItem, MeshCreator.MeshMode,Selector.StoredSnapItems);
            S.Push(MC);
            Clear();

            //S.Push(MeshCreator.MeshListCurrent);
            //MeshCreator.MeshListCurrent = new MeshContainer();
          
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void CheckMeshCreator()
        {
            Renew();
            return;
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void Pop()
        {
            MeshCreatorItem M = S.Pop() as MeshCreatorItem;
            //MeshContainer M = S.Pop() as MeshContainer;
            //MeshCreator.MeshListCurrent = M;
              M.Reset();
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void Clear()
        {
            //MeshCurrent = new Mesh();
            //MeshCurrent.PenWidth = PenWidth;
            //MeshCurrent.MaterialAssign(Mat);
            MeshCreator.MeshVertices = new List<xyzf>();
            MeshCreator.MeshNormals = new List<xyzf>();
            MeshCreator.MeshTextureCoords = new List<xyf>();
            MeshCreator.MeshIndices = new List<IndexType>();
            MeshCreator.MeshListCurrent = new MeshContainer();
            MeshCreator.MeshMode = PolygonMode.Fill;
           
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<xyzf> MeshVertices = new List<xyzf>(200);
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<xyzf> MeshNormals = new List<xyzf>(200);
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<xyzf> MeshColors = new List<xyzf>(200);
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<xyf> MeshTextureCoords = new List<xyf>(200);
        /// <summary>
        /// internal.
        /// </summary>
        public static List<IndexType> MeshIndices = new List<IndexType>(200);
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<IndexType> MeshCurrentIndices = new List<IndexType>(200);
        /// <summary>
        /// internal.
        /// </summary>
        internal static MeshContainer MeshListCurrent = null;
        /// <summary>
        /// internal.
        /// </summary>
        internal static List<SnappItem> StoredSnapItems = new List<SnappItem>();
        static float _PenWidth = 1;
        /// <summary>
        /// internal.
        /// </summary>

        internal static bool HasPenWidth = false;
        internal static float PenWidth
        {
            set
            {
                _PenWidth = value;
                HasPenWidth = true;
            }
            get { return _PenWidth; }
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static SnappItem SnapItem = null;
        /// <summary>
        /// internal.
        /// </summary>
        internal static PolygonMode _MeshMode = PolygonMode.Fill;
        /// <summary>
        /// internal.
        /// </summary>
        internal static PolygonMode MeshMode
        {
            set { _MeshMode = value; }
            get { return _MeshMode; }
        }
        static Material _Material = Materials.Chrome;
        internal static bool HasMaterial = false;
        internal static Material Material
        {
            set { _Material = value;
                   HasMaterial = true;
                }
            get { return _Material; }
        }
        internal static System.Drawing.Color _Emission= System.Drawing.Color.Black;
        internal static bool HasEmission = false;
        internal static System.Drawing.Color Emission
        {
            set
            {
                _Emission = value;
                HasEmission = true;
            }
            get { return _Emission; }
        }
        static PenStyles _PenStyle = PenStyles.Full;
        internal static bool HasPenStyle = false;
        internal static PenStyles PenStyle
        {
            set
            {
                _PenStyle = value;
                HasPenStyle = true;
            }
            get { return _PenStyle; }
        }
        internal static bool HasTexture = false;
        static Texture _Texture = null;
        internal static Texture Texture
        {
            set
            {
                _Texture = value;
                HasTexture = true;
            }
            get { return _Texture; }
        }
        /// <summary>
        /// internal.
        /// </summary>
        public static void AddProg(CustomEntity Prog)
        {
            if (Entity.Compiling)
                MeshListCurrent.Progs.Add(Prog);
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void Renew()
        {

            if (MeshIndices.Count > 0)
            {
                xyzf[] _Colors = null;
                if ((MeshColors != null) && (MeshColors.Count == MeshVertices.Count))
                    _Colors = MeshColors.ToArray();
                xyf[] TextureCoords = null;
                if (MeshTextureCoords.Count > 0) TextureCoords = MeshTextureCoords.ToArray();
               

                CompiledMesh M = new CompiledMesh(MeshIndices.ToArray(), MeshVertices.ToArray(), MeshNormals.ToArray(), TextureCoords,_Colors);
                 // sollte nur im snap modus passieren
                M.SnapObject = Selector.StoredSnapItems.Count - 1;
                M.PenWidth = PenWidth;
                M.Mode = MeshMode;
                M.Material = Material;
                M.PenColor = Emission;
                M.PenStyle = PenStyle;
                M.Texture = Texture;
                MeshListCurrent.Progs.Add(M);
                MeshMode = PolygonMode.Fill;
                if (Selector.RegisterSnap)
                { Selector.SnapMesh.Progs.Add(M); }
            }
           
           
          
             PenWidth = 1;
            MeshVertices = new List<xyzf>();
            MeshNormals = new List<xyzf>();
            MeshTextureCoords = new List<xyf>();
            MeshIndices = new List<IndexType>();
            HasMaterial = false;
            _Material = Materials.Chrome;
            HasTexture = false;
            _Texture = null;
            HasPenWidth = false;
            _PenWidth = 1;
            HasEmission = false;
            _Emission = System.Drawing.Color.Black;
            HasPenStyle = false;
            _PenStyle = PenStyles.Full;
            SnapItem = null;
            MeshMode = PolygonMode.Fill;
        }

        static string VertexToString(xyzf Pt)
        {
          

            string s = "new xyzf(";
            if (Pt.x == 1) s += "Point.x+Size.x,"; else s += "Point.x,";
            if (Pt.y == 1) s += "Point.y+Size.y,"; else s += "Point.y,";
            if (Pt.z == 1) s += "Point.z+Size.z"; else s += "Point.z";
            s += ")";

            return s;



        }
        static string NormalToString(xyzf Pt)
        {
            //   x------

            string s = "new xyzf(";
            s += Pt.x.ToString() + "," + Pt.y.ToString() + "," + Pt.z.ToString() + ")";
            return s;
        }
        static string TextureToString(xyf Pt)
        {
            string s = "new xyf(";
            if (Pt.x == 1) s += "Size.x,"; else s += "0,";
            if (Pt.y == 1) s += "+Size.y"; else s += "0";
            s += ")";
            return s;
        }
       
        /// <summary>
        /// is the main method of this object. See also <see cref="Entity.CompileDraw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device">Device in which it willl be compiled.</param>
        /// <param name="Draw">a event, which holds the drawings.</param>
        public static MeshContainer CreateMeshes(OpenGlDevice Device, DrawAction Draw)
        {  
           // sollte nur von compilesnapbuffer aufgerufen werden !!!! und erstellt die triangles
            bool Compiling = Entity.Compiling;
            if (Compiling) MeshCreator.Push();
            bool SaveCompiling = Entity.Compiling;

           // Entity.Compiling = true; // Checken braucht es wegen line snap
            if (Selector.StoredSnapItems.Count == 0)
                Selector.StoredSnapItems.Add(null); // braucht es nicht

            MeshListCurrent = new MeshContainer();  // braucht es nicht
            MeshIndices.Clear(); // braucht es wahrscheinlich  nicht
                              
           
            RenderKind R = Device.RenderKind;// wieder heraus
          
            Draw(Device);
    
            Renew();
            Device.RenderKind = R;
            Entity.Compiling = SaveCompiling;
            MeshContainer Result = MeshListCurrent;
        //    MeshListCurrent.MySnapItem = new MeshSnapItem(MeshListCurrent);
            Device.RenderKind = RenderKind.SnapBuffer;
            TriangleList TL = new TriangleList();
            for (int i = 0; i < MeshListCurrent.Progs.Count; i++)
            {
                if (MeshListCurrent.Progs[i] is Mesh)
                {
                    Mesh M = MeshListCurrent.Progs[i] as Mesh;
                    M.getTriangles(TL );
                }

            }
            TriangleArrays TA= TL.ToArrays();
        


            Device.RenderKind = R;
           if (Compiling) MeshCreator.Pop();
            return Result;
        }
        /// <summary>
        /// compiles the <see cref="DrawAction"/> draw and returns a meshContainer.
        /// The <b>progs</b> of this is a list of <see cref="Mesh"/>.
        /// This method is called from <see cref="Entity.Compile(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device"><see cref="OpenGlDevice"/> in which will be drawn.</param>
        /// <param name="Draw">the <see cref="DrawAction"/>, which is used to produce a <see cref="MeshContainer"/></param>
        /// <returns></returns>
        public static MeshContainer Compile(OpenGlDevice Device, DrawAction Draw)
        {
            Device.PushMatrix();
            MeshListCurrent = new MeshContainer();
            bool Compiling = Entity.Compiling;
           if (Compiling) MeshCreator.Push();
            bool SaveCompiling = Entity.Compiling;
            Entity.Compiling = true;
            RenderKind SaveRenderKind = Device.RenderKind;
            Device.RenderKind = RenderKind.Render;
            MeshIndices.Clear();
          
            Draw(Device);
        
            Renew();
            Entity.Compiling = SaveCompiling;
            Device.RenderKind = SaveRenderKind;
            Device.PopMatrix();
            MeshContainer Result = MeshListCurrent;
            if (Compiling) MeshCreator.Pop();
            return Result;
        }
        
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshPaintEntity(OpenGlDevice Device, Entity Entity)
        {
            MeshCreator.Renew();
            Entity._Ondraw(Device);
            for (int i = 0; i < Entity.Children.Count; i++)
            {
                MeshPaintEntity(Device, Entity.Children[i]);
            }
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshdrawTriangles2d(OpenGlDevice Device, List<IndexType> Indices, xyf[] Points, xyf[] Texture)
        {
            float dir = 1;

            for (int i = 0; i < Indices.Count; i += 3)
            {
                xy A = new xy(Points[Indices[i + 1]].x - Points[Indices[i]].x, Points[Indices[i + 1]].y - Points[Indices[i]].y);
                xy B = new xy(Points[Indices[i + 2]].x - Points[Indices[i]].x, Points[Indices[i + 2]].y - Points[Indices[i]].y);

                double F = A & B;
                if (System.Math.Abs(F) > 0.0001)
                {
                    if (F < 0) dir = 1;
                    else dir = -1;
                    break;
                }
            }
            int ID = MeshVertices.Count;
            for (int i = 0; i < Indices.Count; i++)
                MeshIndices.Add((IndexType)(Indices[i] + ID));
            if (MeshVertices.Count + Points.Length > IndexType.MaxValue)
            {
                int CT = (MeshVertices.Count + Points.Length);
                MeshIndices = new List<IndexType>();
                Renew();
                throw new Exception("Points count to large " + CT.ToString());
            }
            for (int i = 0; i < Points.Length; i++)
                MeshVertices.Add(Device.ModelMatrix * new xyz(Points[i].x, Points[i].y, 0).toXYZF());
            xyzf NP = Device.ModelMatrix * new xyzf(0, 0, 0);
            xyzf N = Device.ModelMatrix * new xyzf(0, 0, dir) - NP;
            for (int i = 0; i < Points.Length; i++)
                MeshNormals.Add(N);
            if (Texture != null)
                MeshTextureCoords.AddRange(Texture);
            MeshMode = PolygonMode.Fill;
            if (Entity.Compiling)
                if (Device.RenderKind == RenderKind.SnapBuffer)
                    MeshCreator.Renew();
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshdrawLined(OpenGlDevice Device, xyArray A)
        {

            int ID = MeshVertices.Count;
            for (int i = 0; i < A.Count; i++)
                MeshIndices.Add((IndexType)(i + ID));
            for (int i = 0; i < A.Count; i++)
                MeshVertices.Add(((Device.ModelMatrix * A[i].toXYZ()).toXYZF()));
            MeshMode = PolygonMode.Line;

        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshdrawLined(OpenGlDevice Device, xyzf[] A)
        {
            int ID = MeshVertices.Count;
            for (int i = 0; i < A.Length; i++)
                MeshIndices.Add((IndexType)(i + ID));
            for (int i = 0; i < A.Length; i++)
                MeshVertices.Add(((Device.ModelMatrix * A[i])));
            // MeshVertices.Add((( A[i])));
            MeshMode = PolygonMode.Line;
            if (Entity.Compiling)
                if (Device.RenderKind == RenderKind.SnapBuffer)
                    MeshCreator.Renew();
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshdrawLined(OpenGlDevice Device, xyzArray A)
        {
            PolygonMode Save = Device.PolygonMode;
            int ID = MeshVertices.Count;
            for (int i = 0; i < A.Count; i++)
                MeshIndices.Add((IndexType)(i + ID));
            for (int i = 0; i < A.Count; i++)
                MeshVertices.Add(((Device.ModelMatrix * A[i]).toXYZF()));
            MeshMode = PolygonMode.Line;
            Device._PolygonMode = Save; // verursacht ein renew falls fill
            MeshCreator.Renew();
            if (Entity.Compiling)
                if (Device.RenderKind == RenderKind.SnapBuffer)
                    MeshCreator.Renew();
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void MeshdrawTriangles(OpenGlDevice Device, IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture)
        {

            if ((Device.RenderKind != RenderKind.SnapBuffer) && (!Entity.Compiling)) return;
            if ((Device.RenderKind == RenderKind.SnapBuffer)) // nur dann....
            Selector.StoredSnapItems[Selector.StoredSnapItems.Count-1].TriangleInfo= new TriangleInfo(Indices, Points, Normals, Texture);
            // die einzige Zuweisung der Triangleinfo
            int ID = MeshVertices.Count;
            for (int i = 0; i < Indices.Length; i++)
                MeshIndices.Add((IndexType)(Indices[i] + ID));
            if (MeshVertices.Count + Points.Length > IndexType.MaxValue)
            {
                int CT = (MeshVertices.Count + Points.Length);
                MeshIndices = new List<IndexType>();
                Renew();
                throw new Exception("Points count to large " + CT.ToString());
            }
            for (int i = 0; i < Points.Length; i++)
                MeshVertices.Add(Device.ModelMatrix * Points[i]);
            xyzf NP = Device.ModelMatrix * new xyzf(0, 0, 0);
            if (Normals != null)
                for (int i = 0; i < Normals.Length; i++)
                    MeshNormals.Add(Device.ModelMatrix * Normals[i] - NP);
            if (Texture != null)
                for (int i = 0; i < Texture.Length; i++)
                    MeshTextureCoords.Add(Texture[i]);
            MeshMode = Device.PolygonMode;
            OpenGlDevice.CheckError();

        }
    }
}
