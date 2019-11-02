
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

using System;
namespace Drawing3d
{
    /// <summary>
    /// This class holds a mesh edge given by the neighbor and the edge
    /// </summary>
    [Serializable]
    public class MeshEdge
    {/// <summary>
     /// constructor which set the Neighbor and the Edge, which are given by an integer related to a list. F.e <see cref="INeighbors"/>
     /// </summary>
     /// <param name="Neighbor">Id if the Neighbor</param>
     /// <param name="Edge">Id of the edge</param>
        public MeshEdge(int Neighbor, int Edge)
        {
            this.Neighbor = Neighbor;
            this.Edge = Edge;
        }
        /// <summary>
        /// Neighbor as int
        /// </summary>
        public int Neighbor = -1;
        /// <summary>
        /// Edge as int
        /// </summary>
        public int Edge = -1;
    }
    /// <summary>
    /// Interface for Neighbor
    /// </summary>
    public interface INeighbors
    {
        /// <summary>
        /// returs the neighbor of en edge
        /// </summary>
        /// <param name="Id">Index in a list of <see cref="MeshEdge"/></param>
        /// <returns></returns>
        int GetNeighbor(int Id);
        /// <summary>
        /// Store the neighbor
        /// </summary>
        /// <param name="Id">Index in a list of <see cref="MeshEdge"/></param>
        /// <param name="value">Neighbor</param>
        void SetNeighbor(int Id, int value);
        /// <summary>
        /// Returns the neighbor
        /// </summary>
        /// <param name="Id">Index in a list of <see cref="MeshEdge"/></param>
        /// <returns>Neighbor</returns>
        int GetNeighborEdge(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">Index in a list of <see cref="MeshEdge"/></param>
        /// <param name="value">Edge</param>
        void SetNeighborEdge(int Id, int value);
    }
}
