
using System.Collections;
using System;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
   

namespace Drawing3d
{
    /// <summary>
    /// Is used, if a triangle is described by indices related to more than one list of coordinates.
    /// In that case ListA, ListB, ListC are references to the related coordinatelist.
    /// MultiIndexedTriangle is the default recordtype for the <see cref="MultiTriangleList"/>
    /// </summary>
    public struct MultiIndexedTriangle
    {
        /// <summary>
        /// Index of A in the List ListA
        /// </summary>
        public int A;
        /// <summary>
        /// Index of B in the List ListB
        /// </summary>

        public int B;
        /// <summary>
        /// Index of C in the List ListC
        /// </summary>

        public int C;
        /// <summary>
        /// Index of the ListA in an other List
        /// </summary>

        public int ListA;
        /// <summary>
        /// Index of the ListB in an other List
        /// </summary>

        public int ListB;
        /// <summary>
        /// Index of the ListC in an other List
        /// </summary>
        public int ListC;
    }

    /// <summary>
    /// This list manages MultiIndexedTriangles
    /// </summary>

    [Serializable]
    public class MultiTriangleList : ArrayList
    {
        /// <summary>
        /// The defaultindexer retrieves MultiIndexedTriangles
        /// </summary>

        public new MultiIndexedTriangle this[int i]   // Indexer declaration
        {
            get { return (MultiIndexedTriangle)base[i]; }
            set { base[i] = value; }
        }
    }

}
