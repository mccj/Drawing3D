
using System;

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
    /// Encapsulates the structure <see cref="xyz"/> in a <b>class</b>.
    /// </summary>
    [Serializable]
    public class Vertex3d
    {
        /// <summary>
        /// Multipicates the coordinates with a <see cref="Matrix"/>
        /// </summary>
        /// <param name="m">A Matrix</param>
        /// <param name="a">A Vertex</param>
        /// <returns></returns>
        public static Vertex3d operator *(Matrix m, Vertex3d a)
        {
            a._xyz = m * a._xyz;
            return a;

        }
        private xyz _xyz;
        /// <summary>
        /// Empty contructor
        /// </summary>
        public Vertex3d()
        {
        }
        /// <summary>
        /// Get and sets the <see cref="xyz"/> coords.
        /// </summary>
        public xyz Value
        {
            get { return _xyz; }
            set
            {

                if (value.dist(new xyz(2, 2, 2)) < 0.01)
                {
                }
                _xyz = value;
            }
        }
        /// <summary>
        /// A constructor with parameters <see cref="xyz"/>
        /// </summary>
        /// <param name="Value">The initializing Point</param>
        public Vertex3d(xyz Value)
        {

            this.Value = Value;
        }
        /// <summary>
        /// A object, which can be used by the coder
        /// </summary>
        public object Tag;

    }
}
