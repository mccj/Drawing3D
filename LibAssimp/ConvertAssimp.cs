using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

using System.Drawing;
namespace Drawing3d
{
    class ConvertAssimp
    {
        
       
       
        private  static void LoadTextures(Assimp.Scene _raw)
        {
            var materials = _raw.Materials;
            foreach (var mat in materials)
            {
                var textures = mat.GetAllMaterialTextures();
                foreach (var tex in textures)
                {
                    var path = tex.FilePath;

                }
            }
        }
        /// <summary>
        /// internal.
        /// </summary>
       internal static string BaseDir = "";
       static Texture LoadTexture(Assimp.Material M)
        {
            Texture Result = null;
            var textures = M.GetAllMaterialTextures();
           
            foreach (var tex in textures)
            {
                var path = tex.FilePath;
                if (path != "")
                {
                    try
                    {
                       
                        int id = (path.IndexOf("./"));
                        if (id >= 0)
                            path = path.Remove(id, 2);
                        path = System.IO.Path.GetFileName(path);
                        Result = new Texture();
                        Result.LoadFromFile(BaseDir + "\\" + path);
                     
                    }
                    catch (Exception E)
                    {
                        System.Windows.Forms.MessageBox.Show(E.Message);
                     }

                }
            }
            return Result;
        }
       static void LoadNodeRekursiv( Assimp.Node Node, Scene _Scene)
        {
            if (Node.HasMeshes)
            {
                for (int i = 0; i < Node.MeshIndices.Count; i++)
                {
                    _Scene.Meshes[Node.MeshIndices[i]].Transformation = AssimpConv.ConvertTransform(Node.Transform);
                }
            }

            for (int i = 0; i < Node.Children.Count; i++)
            {
                LoadNodeRekursiv(Node.Children[i], _Scene);
            }
        }
        static void MoveTranslucentAtEnd(Scene _Scene)
        {
            //int last = _Scene.Meshes.Count;
            //for (int i = 0; i < last; i++)
            //{
            //    if ((_Scene.Children[i] as M3dsMesh).Material.getTranslucent() < 1)
            //    {
            //        Entity E = _Scene.Children[i];
            //        _Scene.Children.RemoveAt(i);
            //        i--;
            //        last--;
            //        _Scene.Children.Add(E);
            //    }
            //}
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static void LoadNode(Assimp.Scene Scene, Scene _Scene)
        {
            LoadNodeRekursiv(Scene.RootNode, _Scene);
        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static Scene ConvertFromAssimp(Assimp.Scene _Scene)
        {
            
            Scene Result = new Scene();
            LoadTextures(_Scene);
            Result.CompileEnable = false;
            for (int i = 0; i < _Scene.Meshes.Count; i++)
            {
                D3DMesh _M = new D3DMesh();
                _M.CompileEnable = false;
                Assimp.Mesh M = _Scene.Meshes[i];
                List<int> _Indices = new List<int>(M.Faces.Count * 4);
                if (M.Faces.Count >0)
                {
                    for (int j = 0; j < M.Faces.Count; j++)
                    {
                      
                            if (M.Faces[j].Indices.Count>4)
                        {
                            _Indices.Add(M.Faces[j].Indices[0]);
                            _Indices.Add(M.Faces[j].Indices[1]);
                            _Indices.Add(M.Faces[j].Indices[4]);
                            _Indices.Add(M.Faces[j].Indices[4]);
                            _Indices.Add(M.Faces[j].Indices[1]);
                            _Indices.Add(M.Faces[j].Indices[2]);
                            _Indices.Add(M.Faces[j].Indices[4]);
                            _Indices.Add(M.Faces[j].Indices[2]);
                            _Indices.Add(M.Faces[j].Indices[3]);


                        }
                        else
                        if (M.Faces[j].Indices.Count > 3)
                        {
                            _Indices.Add(M.Faces[j].Indices[0]);
                            _Indices.Add(M.Faces[j].Indices[1]);
                            _Indices.Add(M.Faces[j].Indices[3]);
                            _Indices.Add(M.Faces[j].Indices[3]);
                            _Indices.Add(M.Faces[j].Indices[1]);
                            _Indices.Add(M.Faces[j].Indices[2]);
                        }
                        else
                        {
                            _Indices.Add(M.Faces[j].Indices[0]);
                            _Indices.Add(M.Faces[j].Indices[1]);
                            _Indices.Add(M.Faces[j].Indices[2]);
                        }
                     
                    }
                    _M.Indices = _Indices.ToArray();
                }

                Result.Meshes.Add(_M);
                //----------------Indices--------------------

                int[] Indices = M.GetIndices();
               
                _M.Material = AssimpConv.ConvertMaterial(_Scene.Materials[M.MaterialIndex]);
                _M.Texture = LoadTexture(_Scene.Materials[M.MaterialIndex]);
                List<Vector3D> Vertices = M.Vertices;
                _M.Position = new xyzf[Vertices.Count];
                for (int j = 0; j < Vertices.Count; j++)
                {
                    Vector3D V = Vertices[j];
                    _M.Position[j] = new xyzf(V.X, V.Y, V.Z);
                }
                List<Vector3D> Normals = M.Normals;
                _M.Normals = new xyzf[Normals.Count];

                for (int j = 0; j < Normals.Count; j++)
                {
                    Vector3D V = Normals[j];
                    _M.Normals[j] = new xyzf(V.X, V.Y, V.Z).normalized();
                   
                }
                if (M.TextureCoordinateChannelCount > 0)
                {
                    _M.TextureCoords = new xyf[M.TextureCoordinateChannels[0].Count];
                    for (int j = 0; j < M.TextureCoordinateChannels[0].Count; j++)
                    {
                        Vector3D V = M.TextureCoordinateChannels[0][j];
                        _M.TextureCoords[j] = new xyf(V.X, V.Y);
                        
                    }

                }
               
                
                _M.Bones = new List<Bone>();
                for (int k = 0; k < M.BoneCount; k++)
                {
                    Assimp.Bone B = M.Bones[k];
                    VertexWeight[] VW = new VertexWeight[B.VertexWeights.Count];
                    for (int g = 0; g < VW.Length; g++)
                    {
                        VW[g] = new VertexWeight(B.VertexWeights[g].VertexID, B.VertexWeights[g].Weight);
                    }
                    Bone _B = new Bone(B.Name,AssimpConv.ConvertTransform( B.OffsetMatrix),VW);
                    _M.Bones.Add(_B);
                }
            
            }

            LoadNode(_Scene, Result);
            MoveTranslucentAtEnd(Result);
            Result._Scene = _Scene;
            Result.SceneAnimator = new SceneAnimator(Result);
            return Result;
        }
    }
}
