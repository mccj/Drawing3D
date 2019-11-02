
using System;
using System.Collections.Generic;

using AssimpContext = Assimp.AssimpContext;
namespace Drawing3d
{
    /// <summary>
    /// is a animatable <see cref="Entity"/>. You can get it by <see cref="Reader.FromFile(string)"/>
    /// It has an animator <see cref="SceneAnimator"/>
    /// </summary>
    public class Scene : Entity
    { 
        /// <summary>
        ///  contains an <see cref="SceneAnimator"/> to animate.
        /// </summary>
        public SceneAnimator SceneAnimator = null;
        /// <summary>
        /// is a list of <see cref="Mesh"/>, which built the object.
        /// </summary>
        public List<Mesh> Meshes = new List<Mesh>();
        /// <summary>
        /// the evalutator for a animation. <see cref="CpuSkinningEvaluator"/>
        /// </summary>
        public CpuSkinningEvaluator SkinninEvaluator = null;
        /// <summary>
        /// internal.
        /// </summary>
        internal Assimp.Scene _Scene = null;
        /// <summary>
        /// is a list of <see cref="Assimp.Animation"/>.
        /// </summary>
        public List<Assimp.Animation> Animations
        {
            get { if (_Scene != null) return _Scene.Animations; return null; }
        }
        Box RekursiveGetBox(Assimp.Node node, Box BoxMin)
        {

            Box _Box = BoxMin;

            if ((node.HasMeshes))
            {
                foreach (var index in node.MeshIndices)
                {
                    D3DMesh mesh = (Meshes[index] as D3DMesh);

                    xyzf b = new xyzf(0, 0, 0);


                    CpuSkinningEvaluator.CachedMeshData MD = SkinninEvaluator.GetEntry(mesh);
                    SkinninEvaluator.GetTransformedVertexPosition(node, mesh, 0, out b);

                    xyzf[] P = new xyzf[MD._cachedPositions.Length];
                    for (int i = 0; i < P.Length; i++)
                    {
                        P[i] = mesh.Transformation * MD._cachedPositions[i];
                    }
                   _Box = Box.GetEnvBox(P, _Box);
               }
            }
            for (int i = 0; i < node.Children.Count; i++)
            {
                _Box = RekursiveGetBox(node.Children[i], _Box);
            }
            return _Box;
        }
        /// <summary>
        /// overrides the <see cref="GetMaxBox"/> of an <see cref="Entity"/>.
        /// </summary>
        /// <returns>the enveloping box.</returns>
        public override Box GetMaxBox()
        {

            Box MaxBox = Box.ResetBox();
            for (int i = 0; i < Meshes.Count; i++)
            {
                Box B = Meshes[i].GetMaxBox();
                if (HasAnimations)
                {

                    return RekursiveGetBox(_Scene.RootNode, Box.ResetBox());
                }
                MaxBox = MaxBox.GetMaxBox(B);
            }

            return MaxBox;
        }

        /// <summary>
        /// an empty constructor.
        /// </summary>
        public Scene()
        {
            SkinninEvaluator = new CpuSkinningEvaluator(this);
        }
        /// <summary>
        /// returns <b>true</b> if the scene has animations. 
        /// </summary>
        public bool HasAnimations
        {
            get
            {
                if (_Scene != null) return _Scene.HasAnimations;
                return false;
            }
        }
        class TransParencyItem
        {
            public TransParencyItem(D3DMesh Mesh, xyzf[] Position, xyzf[] Normals)
            {
                this.Mesh = Mesh;
                this.Position = Position;
                this.Normals = Normals;
            }
            public D3DMesh Mesh = null;
            public xyzf[] Position = null;
            public xyzf[] Normals = null;
        }
        List<TransParencyItem> TransparentMeshes = new List<TransParencyItem>();
        void TransparencyDraw(OpenGlDevice Device)
        {
            for (int i = 0; i < TransparentMeshes.Count; i++)
            {
                if (!HasAnimations)
                    TransparentMeshes[i].Mesh.Paint(Device);
                else
                {
                    xyzf[] SavePos = TransparentMeshes[i].Mesh.Position;
                    xyzf[] SaveNormals = TransparentMeshes[i].Mesh.Normals;
                    TransparentMeshes[i].Mesh.Position = TransparentMeshes[i].Position;
                    TransparentMeshes[i].Mesh.Normals = TransparentMeshes[i].Normals;
                    TransparentMeshes[i].Mesh.Paint(Device);
                    TransparentMeshes[i].Mesh.Position = SavePos;
                    TransparentMeshes[i].Mesh.Normals = SaveNormals;
                }


            }
        }
        /// <summary>
        /// overrides the <see cref="Dispose"/> method by disposing the children.
        /// </summary>
        override public void Dispose()
        {

            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Mesh) (Children[i] as Mesh).Dispose();
            }
        }
       
     
        void RekursiveDraw(Assimp.Node node, OpenGlDevice Device)
        {
           if ((node.HasMeshes))
            {
                foreach (var index in node.MeshIndices)
                {
                    D3DMesh mesh = (Meshes[index] as D3DMesh);

                    xyzf b = new xyzf(0, 0, 0);


                    CpuSkinningEvaluator.CachedMeshData MD = SkinninEvaluator.GetEntry(mesh);
                    SkinninEvaluator.GetTransformedVertexPosition(node, mesh, 0, out b);
                    if (mesh.Material.Translucent < 1)
                    {
                        TransparentMeshes.Add(new TransParencyItem(mesh, MD._cachedPositions, MD._cachedNormals));
                        continue;
                    }

                    xyzf[] SavePos = mesh.Position;
                    xyzf[] SaveNormals = mesh.Normals;
                    mesh.Position = MD._cachedPositions;
                    mesh.Normals = MD._cachedNormals;
                    mesh.Paint(Device);
                    mesh.Position = SavePos;
                    mesh.Normals = SaveNormals;

                }
            }
            for (int i = 0; i < node.Children.Count; i++)
            {
                RekursiveDraw(node.Children[i], Device);
            }

        }
        /// <summary>
        /// overrides the <see cref="OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            TransparentMeshes.Clear();

            if (HasAnimations)
                RekursiveDraw(this._Scene.RootNode, Device);
            else
            {
                foreach (D3DMesh M in Meshes)
                {
                    if (M.Material.Translucent < 1)
                    {
                        TransparentMeshes.Add(new TransParencyItem(M, null, null));

                    }
                    else
                        M.Paint(Device);
                }
            }
            TransparencyDraw(Device);
            base.OnDraw(Device);

        }
    }
    //public class D3DSceneAnimator : OglAnimator
    //{
    //    Scene Scene = null;
    //    public override void Start()
    //    {
    //        if (Scene.HasAnimations)
    //        {
    //            Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
    //            Scene.SceneAnimator.ActiveAnimation = 0;
    //            Scene.SceneAnimator.AnimationCursor = 0;
    //            Scene.IsAnimated = true;
    //            base.Start();
    //        }
    //    }
    //    public void Start(int AnimationId)
    //    {
    //        if (Scene.HasAnimations)
    //        {
    //            Scene.SceneAnimator.ActiveAnimation = -1;
    //            Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
    //            Scene.SceneAnimator.ActiveAnimation = AnimationId;
    //            Scene.SceneAnimator.AnimationCursor = 0;
    //            Scene.IsAnimated = true;


    //            base.Start();
    //        }
    //    }
    //    public override void End()
    //    {
    //        if (Scene.HasAnimations)
    //        {
    //            Scene.SceneAnimator.AnimationPlaybackSpeed = 0;

    //            Scene.IsAnimated = false;
    //        }
    //        base.End();
    //    }
    //    public void Continue()
    //    {
    //        if (Scene.HasAnimations)
    //        {
    //            Scene.SceneAnimator.Loop = true;
    //            Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
    //            int Save = Scene.SceneAnimator.ActiveAnimation;
    //            Scene.SceneAnimator.ActiveAnimation = -1;
    //            Scene.SceneAnimator.ActiveAnimation = Save;
    //            Scene.IsAnimated = true;


    //            base.Start();
    //        }
    //    }

    //    public override void OnAnimate()
    //    {

    //        Scene.SceneAnimator.Update((float)(CurrentTime) / 1000f);
    //        Scene.SkinninEvaluator.Update();
    //        ResetTime();

    //        base.OnAnimate();

    //    }
    //    public D3DSceneAnimator(OpenGlDevice Device, Scene Scene)
    //        : base(Device)
    //    {
    //        this.Scene = Scene;
    //        Duration = -1;
    //    }
    //}
}
