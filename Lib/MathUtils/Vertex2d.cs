
using System;


using LibTessDotNet;
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
    /// Encapsulates the structure <see cref="xy"/> in a class.
    /// </summary>
    [Serializable]
    public class Vertex2d
    {
        private xy _xy;
        /// <summary>
        /// Empty contructor
        /// </summary>
        public Vertex2d()
        {
        }
        /// <summary>
        /// Get and sets the xy coords.
        /// </summary>
        public xy Value
        {
            get { return _xy; }
            set { _xy = value; }
        }
        /// <summary>
        /// A constructor with parameters xy
        /// </summary>
        /// <param name="Value">The initializing Point</param>
        public Vertex2d(xy Value)
        {
            this.Value = Value;
        }

    }
}
