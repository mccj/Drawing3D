using System;
using System.Collections.Generic;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// inherits from <see cref="EdgeList"/>.
    /// </summary>
    [Serializable]
    public class EdgeLoop : EdgeList
    {
        /// <summary>
        /// copies a list of <see cref="Edge"/>s in a <see cref="Solid"/> Targetsolid.
        /// </summary>
        /// <param name="TargetSolid">target, in which the edges will be copied.</param>
        /// <returns>a list of copied <see cref="Edge"/>s.</returns>
        public virtual EdgeLoop Copy(Solid TargetSolid)
        {
            EdgeLoop Result = new EdgeLoop();
            for (int i = 0; i < Count; i++)
                Result.Add(this[i].Copy(TargetSolid));
            return Result;
        }
 
        /// <summary>
        /// Ein frei programmierbares Tag
        /// </summary>
        public object Tag;
    }
}