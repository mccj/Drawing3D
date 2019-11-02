/*
* Copyright (c) 2012-2013 AssimpNet - Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assimp.Unmanaged;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
  
namespace Drawing3d
{
    /// <summary>
    /// Represents a single bone of a mesh. A bone has a name which allows it to be found in the frame
    /// hierarchy and by which it can be addressed by animations. In addition it has a number of
    /// influences on vertices.
    /// </summary>
    public sealed class Bone //: IMarshalable<Bone, AiBone>
    {
        private String m_name;
        private List<VertexWeight> m_weights;
        private Matrix m_offsetMatrix;

        /// <summary>
        /// Gets or sets the name of the bone.
        /// </summary>
        public String Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Gets the number of vertex influences the bone contains.
        /// </summary>
        public int VertexWeightCount
        {
            get
            {
                return m_weights.Count;
            }
        }

        /// <summary>
        /// Gets if the bone has vertex weights - this should always be true.
        /// </summary>
        public bool HasVertexWeights
        {
            get
            {
                return m_weights.Count > 0;
            }
        }

        /// <summary>
        /// Gets the vertex weights owned by the bone.
        /// </summary>
        public List<VertexWeight> VertexWeights
        {
            get
            {
                return m_weights;
            }
        }

        /// <summary>
        /// Gets or sets the matrix that transforms from mesh space to bone space in bind pose.
        /// </summary>
        public Matrix OffsetMatrix
        {
            get
            {
                return m_offsetMatrix;
            }
            set
            {
                m_offsetMatrix = value;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Bone"/> class.
        /// </summary>
        public Bone()
        {
            m_name = null;
            m_offsetMatrix = Matrix.identity; //Matrix3x3
            m_weights = new List<VertexWeight>();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Bone"/> class.
        /// </summary>
        /// <param name="name">Name of the bone</param>
        /// <param name="offsetMatrix">Bone's offset matrix</param>
        /// <param name="weights">Vertex weights</param>
        public Bone(String name, Matrix offsetMatrix, VertexWeight[] weights) // Matrix3ä3
        {
            m_name = name;
            m_offsetMatrix = offsetMatrix;
            m_weights = new List<VertexWeight>();

            if (weights != null)
                m_weights.AddRange(weights);
        }

        //#region IMarshalable Implementation

        ///// <summary>
        ///// Gets if the native value type is blittable (that is, does not require marshaling by the runtime, e.g. has MarshalAs attributes).
        ///// </summary>
        //bool IMarshalable<Bone, AiBone>.IsNativeBlittable
        //{
        //    get { return true; }
        //}

        ///// <summary>
        ///// Writes the managed data to the native value.
        ///// </summary>
        ///// <param name="thisPtr">Optional pointer to the memory that will hold the native value.</param>
        ///// <param name="nativeValue">Output native value</param>
        //void IMarshalable<Bone, AiBone>.ToNative(IntPtr thisPtr, out AiBone nativeValue)
        //{
        //    nativeValue.Name = new AiString(m_name);
        //    nativeValue.OffsetMatrix =AssimpConv.ConvertTransformToAssimp( m_offsetMatrix);
        //    nativeValue.NumWeights = (uint)m_weights.Count;
        //    nativeValue.Weights = IntPtr.Zero;

        //    if (nativeValue.NumWeights > 0)
        //        nativeValue.Weights = MemoryHelper.ToNativeArray<VertexWeight>(m_weights.ToArray());
        //}

        ///// <summary>
        ///// Reads the unmanaged data from the native value.
        ///// </summary>
        ///// <param name="nativeValue">Input native value</param>
        //void IMarshalable<Bone, AiBone>.FromNative(ref AiBone nativeValue)
        //{
        //    m_name = nativeValue.Name.GetString();
        //    m_offsetMatrix = AssimpConv.ConvertTransform(nativeValue.OffsetMatrix);
        //    m_weights.Clear();

        //    if (nativeValue.NumWeights > 0 && nativeValue.Weights != IntPtr.Zero)
        //        m_weights.AddRange(MemoryHelper.FromNativeArray<VertexWeight>(nativeValue.Weights, (int)nativeValue.NumWeights));
        //}

        /// <summary>
        /// Frees unmanaged memory created by <see cref="IMarshalable{Bone, AiBone}.ToNative"/>.
        /// </summary>
        /// <param name="nativeValue">Native value to free</param>
        /// <param name="freeNative">True if the unmanaged memory should be freed, false otherwise.</param>
        //public static void FreeNative(IntPtr nativeValue, bool freeNative)
        //{
        //    if (nativeValue == IntPtr.Zero)
        //        return;

        //    AiBone aiBone = MemoryHelper.Read<AiBone>(nativeValue);
        //    int numWeights = MemoryHelper.Read<int>(MemoryHelper.AddIntPtr(nativeValue, MemoryHelper.SizeOf<AiString>()));
        //    IntPtr weightsPtr = MemoryHelper.AddIntPtr(nativeValue, MemoryHelper.SizeOf<AiString>() + sizeof(uint));

        //    if (aiBone.NumWeights > 0 && aiBone.Weights != IntPtr.Zero)
        //        MemoryHelper.FreeMemory(aiBone.Weights);

        //    if (freeNative)
        //        MemoryHelper.FreeMemory(nativeValue);
        //}

        //#endregion
    }
    /// <summary>
    /// Represents an object that can be marshaled to and from a native representation.
    /// </summary>
    /// <typeparam name="Managed">Managed object type</typeparam>
    /// <typeparam name="Native">Native value type</typeparam>
    public interface IMarshalable<Managed, Native>
        where Managed : class, new()
        where Native : struct
    {

        /// <summary>
        /// Gets if the native value type is blittable (that is, does not require marshaling by the runtime, e.g. has MarshalAs attributes).
        /// </summary>
        bool IsNativeBlittable { get; }

        /// <summary>
        /// Writes the managed data to the native value.
        /// </summary>
        /// <param name="thisPtr">Optional pointer to the memory that will hold the native value.</param>
        /// <param name="nativeValue">Output native value</param>
        void ToNative(IntPtr thisPtr, out Native nativeValue);

        /// <summary>
        /// Reads the unmanaged data from the native value.
        /// </summary>
        /// <param name="nativeValue">Input native value</param>
        void FromNative(ref Native nativeValue);
    }
    /// <summary>
    /// Represents a single influence of a bone on a vertex.
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct VertexWeight
    {
        /// <summary>
        /// Index of the vertex which is influenced by the bone.
        /// </summary>
        public int VertexID;

        /// <summary>
        /// Strength of the influence in range of (0...1). All influences
        /// from all bones at one vertex amounts to 1.
        /// </summary>
        public float Weight;

        /// <summary>
        /// Constructs a new VertexWeight.
        /// </summary>
        /// <param name="vertID">Index of the vertex.</param>
        /// <param name="weight">Weight of the influence.</param>
        public VertexWeight(int vertID, float weight)
        {
            VertexID = vertID;
            Weight = weight;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{VertexID:{0} Weight:{1}}}",
                new Object[] { VertexID.ToString(info), Weight.ToString(info) });
        }
    }
    /// Caches vertex->bone (1:n) assignments for a mesh. This information is derived from
    /// the bone->vertex (1:n) assignments loaded by assimp.
    /// 
    /// The class can be used with meshes that have no bones at all.
    /// </summary>
    public class BoneByVertexMap
    {
        private readonly D3DMesh _mesh;

        // Back ported from a .net 4.5 Tuple<int, float>
        public struct IndexWeightTuple
        {
            public IndexWeightTuple(int index, float weight)
            {
                Item1 = index;
                Item2 = weight;
            }

            public int Item1;
            public float Item2;
        }

        private readonly uint[] _countBones;
        private readonly IndexWeightTuple[] _bonesByVertex;
        private readonly uint[] _offsets;


        /// <summary>
        /// Array of per-vertex bone assignments. Use GetOffsetAndCountForVertex to 
        /// access the entries for a single vertex.
        /// </summary>
        public IndexWeightTuple[] BonesByVertex
        {
            get { return _bonesByVertex; }
        }


        /// <summary>
        /// Get the number of bone influences for a vertex and the offset in the 
        /// BonesByVertex array where they are stored.
        /// </summary>
        /// <param name="vertex">Vertex index</param>
        /// <param name="offset">Receives the index of the first bone influence for the vertex</param>
        /// <param name="count">Receives the number of bone influenves for the vertex</param>
        public void GetOffsetAndCountForVertex(uint vertex, out uint offset, out uint count)
        {
            offset = _offsets[vertex];
            count = _countBones[vertex];
        }


        internal BoneByVertexMap(D3DMesh mesh)
        {
            Debug.Assert(mesh != null);
            _mesh = mesh;

            _offsets = new uint[mesh.Position.Length];
            _countBones = new uint[mesh.Position.Length];

            if (_mesh.Bones.Count == 0)
            {
                _bonesByVertex = new IndexWeightTuple[0];
                return;
            }

            // get per-vertex bone influence counts
            var countWeights = 0;
            for (var i = 0; i < mesh.Bones.Count; ++i)
            {
                var bone = mesh.Bones[i];
                countWeights += bone.VertexWeightCount;
                for (int j = 0; j < bone.VertexWeightCount; ++j)
                {
                    var weight = bone.VertexWeights[j];
                    ++_countBones[weight.VertexID];
                }
            }

            _bonesByVertex = new IndexWeightTuple[countWeights];

            // generate offset table
            uint sum = 0;
            for (var i = 0; i < _mesh.Position.Length; ++i)
            {
                _offsets[i] = sum;
                sum += _countBones[i];
            }

            // populate vertex-to-bone table, using the offset table
            // to keep track of how many bones have already been
            // written for a vertex.
            for (var i = 0; i < mesh.Bones.Count; ++i)
            {
                var bone = mesh.Bones[i];
                countWeights += bone.VertexWeightCount;
                for (int j = 0; j < bone.VertexWeightCount; ++j)
                {
                    var weight = bone.VertexWeights[j];
                    BonesByVertex[_offsets[weight.VertexID]++] = new IndexWeightTuple(i, weight.Weight);
                }
            }

            // undo previous changes to the offset table 
            for (var i = 0; i < _mesh.Position.Length; ++i)
            {
                _offsets[i] -= _countBones[i];
            }

            Debug.Assert(_offsets[0] == 0);
        }
    }
}
