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
using Assimp.Unmanaged;
using Drawing3d;

namespace Sample
{
    /// <summary>
    /// A node in the imported model hierarchy.
    /// </summary>
    public sealed class Node : IMarshalable<Node, AiNode>
    {
        private String m_name;
        private Matrix m_transform;
        private Node m_parent;
        private NodeCollection m_children;
        private List<int> m_meshes;

        /// <summary>
        /// Gets or sets the name of the node.
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
        /// Gets or sets the transformation of the node relative to its parent.
        /// </summary>
        public Matrix Transform
        {
            get
            {
                return m_transform;
            }
            set
            {
                m_transform = value;
            }
        }

        /// <summary>
        /// Gets the node's parent, if it exists. 
        /// </summary>
        public Node Parent
        {
            get
            {
                return m_parent;
            }
            set { m_parent = value; }
        }

        /// <summary>
        /// Gets the number of children that is owned by this node.
        /// </summary>
        public int ChildCount
        {
            get
            {
                return m_children.Count;
            }
        }

        /// <summary>
        /// Gets if the node contains children.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return m_children.Count > 0;
            }
        }

        /// <summary>
        /// Gets the node's children.
        /// </summary>
        public NodeCollection Children
        {
            get
            {
                return m_children;
            }
        }

        /// <summary>
        /// Gets the number of meshes referenced by this node.
        /// </summary>
        public int MeshCount
        {
            get
            {
                return m_meshes.Count;
            }
        }

        /// <summary>
        /// Gets if the node contains mesh references.
        /// </summary>
        public bool HasMeshes
        {
            get
            {
                return m_meshes.Count > 0;
            }
        }

        /// <summary>
        /// Gets the indices of the meshes referenced by this node. Meshes can be
        /// shared between nodes, so there is a mesh collection owned by the scene
        /// that each node can reference.
        /// </summary>
        public List<int> MeshIndices
        {
            get
            {
                return m_meshes;
            }
            set { m_meshes = value; }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            m_name = String.Empty;
            m_transform = Matrix.identity;
            m_parent = null;
            m_children = new NodeCollection(this);
            m_meshes = new List<int>();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="name">Name of the node</param>
        public Node(String name)
            : this()
        {
            m_name = name;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="name">Name of the node</param>
        /// <param name="parent">Parent of the node</param>
        public Node(String name, Node parent)
            : this()
        {
            m_name = name;
            m_parent = parent;
        }

        /// <summary>
        /// Removes this node from its parent.
        /// </summary>
        public void Remove()
        {
            if (m_parent == null)
            {
                return;
            }
            m_parent.Children.Remove(this);
            m_parent = null;
        }

        //Internal use - sets the node parent in NodeCollection
        internal void SetParent(Node parent)
        {
            m_parent = parent;
        }

        /// <summary>
        /// Finds a node with the specific name, which may be this node
        /// or any children or children's children, and so on, if it exists.
        /// </summary>
        /// <param name="name">Node name</param>
        /// <returns>The node or null if it does not exist</returns>
        public Node FindNode(String name)
        {
            if (name.Equals(m_name))
            {
                return this;
            }
            if (HasChildren)
            {
                foreach (Node child in m_children)
                {
                    Node found = child.FindNode(name);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            //No child found
            return null;
        }

        private IntPtr ToNativeRecursive(IntPtr parentPtr, Node node)
        {
            if (node == null)
                return IntPtr.Zero;

            int sizeofNative = MemoryHelper.SizeOf<AiNode>();

            //Allocate the memory that will hold the node
            IntPtr nodePtr = MemoryHelper.AllocateMemory(sizeofNative);

            //First fill the native struct
            AiNode nativeValue = new AiNode();
            nativeValue.Name = new AiString(node.m_name);
            nativeValue.Transformation =AssimpConv.ConvertTransformToAssimp( node.m_transform);
            nativeValue.Parent = parentPtr;

            nativeValue.NumMeshes = (uint)node.m_meshes.Count;
            nativeValue.Meshes = MemoryHelper.ToNativeArray<int>(node.m_meshes.ToArray());
#if WITH_NODE_METADATA
            nativeValue.MetaData = IntPtr.Zero;
#endif

            //Now descend through the children
            nativeValue.NumChildren = (uint)node.m_children.Count;

            int numChildren = (int)nativeValue.NumChildren;
            int stride = IntPtr.Size;

            IntPtr childrenPtr = IntPtr.Zero;

            if (numChildren > 0)
            {
                childrenPtr = MemoryHelper.AllocateMemory(numChildren * IntPtr.Size);

                for (int i = 0; i < numChildren; i++)
                {
                    IntPtr currPos = MemoryHelper.AddIntPtr(childrenPtr, stride * i);
                    Node child = node.m_children[i];

                    IntPtr childPtr = IntPtr.Zero;

                    //Recursively create the children and its children
                    if (child != null)
                    {
                        childPtr = ToNativeRecursive(nodePtr, child);
                    }

                    //Write the child's node ptr to our array
                    MemoryHelper.Write<IntPtr>(currPos, ref childPtr);
                }
            }

            //Finall finish writing to the native struct, and write the whole thing to the memory we allocated previously
            nativeValue.Children = childrenPtr;
            
            MemoryHelper.Write<AiNode>(nodePtr, ref nativeValue);

            return nodePtr;
        }

        #region IMarshalable Implemention

        bool IMarshalable<Node, AiNode>.IsNativeBlittable
        {
            get { return false; }
        }

        /// <summary>
        /// Writes the managed data to the native value.
        /// </summary>
        /// <param name="thisPtr">Optional pointer to the memory that will hold the native value.</param>
        /// <param name="nativeValue">Output native value</param>
        void IMarshalable<Node, AiNode>.ToNative(IntPtr thisPtr, out AiNode nativeValue)
        {
            nativeValue = new AiNode(); // --------------------------------
            nativeValue.Name = new AiString(m_name);
            nativeValue.Transformation =AssimpConv.ConvertTransformToAssimp( m_transform);
            nativeValue.Parent = IntPtr.Zero;

            nativeValue.NumMeshes = (uint)m_meshes.Count;
            nativeValue.Meshes = IntPtr.Zero;
#if WITH_NODE_METADATA
            nativeValue.MetaData = IntPtr.Zero;
#endif

            if (nativeValue.NumMeshes > 0)
                nativeValue.Meshes = MemoryHelper.ToNativeArray<int>(m_meshes.ToArray());

            //Now descend through the children
            nativeValue.NumChildren = (uint)m_children.Count;

            int numChildren = (int)nativeValue.NumChildren;
            int stride = IntPtr.Size;

            IntPtr childrenPtr = IntPtr.Zero;

            if (numChildren > 0)
            {
                childrenPtr = MemoryHelper.AllocateMemory(numChildren * IntPtr.Size);

                for (int i = 0; i < numChildren; i++)
                {
                    IntPtr currPos = MemoryHelper.AddIntPtr(childrenPtr, stride * i);
                    Node child = m_children[i];

                    IntPtr childPtr = IntPtr.Zero;

                    //Recursively create the children and its children
                    if (child != null)
                    {
                        childPtr = ToNativeRecursive(thisPtr, child);
                    }

                    //Write the child's node ptr to our array
                    MemoryHelper.Write<IntPtr>(currPos, ref childPtr);
                }
            }

            //Finally finish writing to the native struct
            nativeValue.Children = childrenPtr;
        }

        /// <summary>
        /// Reads the unmanaged data from the native value.
        /// </summary>
        /// <param name="nativeValue">Input native value</param>
        void IMarshalable<Node, AiNode>.FromNative(ref AiNode nativeValue)
        {
            m_name = nativeValue.Name.GetString();
            m_transform =AssimpConv.ConvertTransform( nativeValue.Transformation);
            m_parent = null;
            m_children.Clear();
            m_meshes.Clear();

            if (nativeValue.NumMeshes > 0 && nativeValue.Meshes != IntPtr.Zero)
                m_meshes.AddRange(MemoryHelper.FromNativeArray<int>(nativeValue.Meshes, (int)nativeValue.NumMeshes));

            if (nativeValue.NumChildren > 0 && nativeValue.Children != IntPtr.Zero)
                m_children.AddRange(MemoryHelper.FromNativeArray<Node, AiNode>(nativeValue.Children, (int)nativeValue.NumChildren, true));
        }

        /// <summary>
        /// Frees unmanaged memory created by <see cref="IMarshalable{Node, AiNode}.ToNative"/>.
        /// </summary>
        /// <param name="nativeValue">Native value to free</param>
        /// <param name="freeNative">True if the unmanaged memory should be freed, false otherwise.</param>
        public static void FreeNative(IntPtr nativeValue, bool freeNative)
        {
            if (nativeValue == IntPtr.Zero)
                return;

            AiNode aiNode = MemoryHelper.Read<AiNode>(nativeValue);

            if (aiNode.NumMeshes > 0 && aiNode.Meshes != IntPtr.Zero)
                MemoryHelper.FreeMemory(aiNode.Meshes);

            if (aiNode.NumChildren > 0 && aiNode.Children != IntPtr.Zero)
                MemoryHelper.FreeNativeArray<AiNode>(aiNode.Children, (int)aiNode.NumChildren, FreeNative, true);

            if (freeNative)
                MemoryHelper.FreeMemory(nativeValue);
        }

        #endregion
    }
    /// <summary>
    /// A collection of child nodes owned by a parent node. Manages access to the collection while maintaing parent-child linkage.
    /// </summary>
    public sealed class NodeCollection : IList<Node>
    {
        private Node m_parent;
        private List<Node> m_children;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count
        {
            get
            {
                return m_children.Count;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The child index</param>
        public Node this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    return null;

                return m_children[index];
            }
            set
            {
                if (index < 0 || index > Count || value == null)
                    return;

                m_children[index] = value;
                value.SetParent(m_parent);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="NodeCollection"/> class.
        /// </summary>
        /// <param name="parent">Parent node</param>
        internal NodeCollection(Node parent)
        {
            m_parent = parent;
            m_children = new List<Node>();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(Node item)
        {
            if (item != null)
            {
                m_children.Add(item);
                item.SetParent(m_parent);
            }
        }

        /// <summary>
        /// Adds a range of items to the list.
        /// </summary>
        /// <param name="items">Item array</param>
        public void AddRange(Node[] items)
        {
            if (items == null || items.Length == 0)
                return;

            foreach (Node child in items)
            {
                if (child != null)
                {
                    m_children.Add(child);
                    child.SetParent(m_parent);
                }
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            foreach (Node node in m_children)
            {
                node.SetParent(null);
            }

            m_children.Clear();
        }


        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(Node item)
        {
            return m_children.Contains(item);
        }

        /// <summary>
        /// Copies collection contents to the array
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">Index of the array to start copying.</param>
        public void CopyTo(Node[] array, int arrayIndex)
        {
            m_children.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(Node item)
        {
            return m_children.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, Node item)
        {
            if (index < 0 || index > Count || item == null)
                return;

            m_children.Insert(index, item);
            item.SetParent(m_parent);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            Node child = this[index];

            if (child == null)
            {
                child.SetParent(null);
                m_children.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(Node item)
        {
            if (item != null && m_children.Remove(item))
            {
                item.SetParent(null);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies elements in the collection to a new array.
        /// </summary>
        /// <returns>Array of copied elements</returns>
        public Node[] ToArray()
        {
            return m_children.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Node> GetEnumerator()
        {
            return m_children.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_children.GetEnumerator();
        }
    }
}
