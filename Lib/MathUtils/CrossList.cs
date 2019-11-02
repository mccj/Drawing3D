using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;
using System.Collections.Generic;

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
    /// 
    /// Is the result of <see cref="xyArray.getCrossList(xyArray)"/>.
    /// It contains all intersections of two xyArrays.
    /// </summary>
    [Serializable]
    public class CrossList : ArrayList
    {
        private Object _Tag;
        /// <summary>
        /// Free for use
        /// </summary>
        public Object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        /// <summary>
        /// if AllowMultiCross is set to true and a line is crossed at the same point
        /// by more then one line, every cross is added as <see cref="CrossItem"/> to the <see cref="CrossList"/> otherwise
        /// only one CrossItem is added.
        /// </summary>
        public bool AllowMultiCross = false;
        /// <summary>
        /// Inserts a <see cref="CrossItem"/> to the CrossList and overrides the standard method
        /// </summary>
        public override void Insert(int index, object value)
        {

            CrossItem c = (CrossItem)value;
            c.CrossList = this;
            int i = 0;
            while ((i < Count) && (this[i].Param1 < c.Param1)) i++;

            base.Insert(i, value);
        }
        /// <summary>
        /// Overrides the standard add-method
        /// </summary>
        public override int Add(object value)
        {
            Insert(Count, value);
            return Count - 1;
        }
        /// <summary>
        /// Gets or sets the i-th value as Crossitem.
        /// </summary>
        public new CrossItem this[int i]   // Indexer declaration
        {
            get
            {

                return (CrossItem)base[i];
            }
            set { base[i] = value; }
        }


    }
    /// <summary>
    /// Defines a type for a cross of a line with a xyArray
    /// </summary>
    public enum BorderBehavior
    {
        /// <summary>
        /// Complete Cross
        /// </summary>
        NoBorder,
        /// <summary>
        /// Line goes to the xyArray
        /// </summary>
        BorderBegin,
        /// <summary>
        /// Line leaves the xyArray
        /// </summary>
        BorderEnd
    };

}
