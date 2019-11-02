using System;
using OpenTK.Graphics.OpenGL;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// this class contains a <b>VarName</b>, a <b>Handle</b>, an array of <b>Indices</b> of type <b>IndexType</b>,   an array <b>xyzPoints</b> of <see cref="xyzf"/>
    /// and an an array of <b>xyPoints</b>. <see cref="Mesh"/> uses this class to store Indices, Position, Normals, Colors and Textures
    /// </summary>
    [Serializable]
    public class VBO
    {
        /// <summary>
        /// the buffer handle, if it is used as VBO member
        /// </summary>
       public int _Handle = -1;
        /// <summary>
        /// the name of the variable.
        /// </summary>
        public string VarName = "";
        
       xyzf[] _xyzPoints = null;
        /// <summary>
        /// Points of type <see cref="xyzf"/>.
        /// </summary>
        public xyzf[] xyzPoints
        {
            get { return _xyzPoints; }
            set
            {
                if ((_xyPoints != null) || (_ElementArray != null))
                    throw new Exception("A VBO can handle only one array");
                _xyzPoints = value;


            }
        }

        xyf[] _xyPoints = null;
        /// <summary>
        /// Points of type <see cref="xyf"/>.
        /// </summary>
        public xyf[] xyPoints
        {
            get { return _xyPoints; }
            set
            {
                if ((_xyzPoints != null) || (_ElementArray != null))
                    throw new Exception("A VBO can handle only one array");
                _xyPoints = value;
            }
        }

        IndexType[] _ElementArray = null;
        /// <summary>
        /// an Index array of type <b>IndexType</b>.
        /// </summary>
        public IndexType[] IndexArray
        {
            get { return _ElementArray; }
            set
            {
                if ((_xyPoints != null) || (_xyzPoints != null))
                    throw new Exception("A VBO can handle only one array");
                _ElementArray = value;

              
            }
        }
    }
}