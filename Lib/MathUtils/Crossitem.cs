
using System;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// CrossItem is a member of a <see cref="CrossList"/>. Every intersection of two
    /// xyArrays get a Crossitem. <seealso cref="xyArray.getCrossList(xyArray)"/>
    /// </summary>
    [Serializable]
    public class CrossItem
    {
        /// <summary>
        /// Can be used to set the bound for a cross with more than 1 xyArray.
        /// </summary>
        public int Bound = 0;
        /// <summary>
        /// Will be used from some methods.
        /// </summary>
        public object Intern = null;
        internal bool Visited = false;
        /// <summary>
        /// the constructor initializes the fields Param1, Param2, Tag by null, CrossKind.
        /// </summary>
        /// <param name="Param1">Parameter in the first array. See <see cref="xyArray.getCrossList(xyArray)"/></param>
        /// <param name="Param2">Parameter in the second array.See <see cref="xyArray.getCrossList(xyArray)"/></param>allow
        /// 
        /// <param name="CrossKind">See <see cref="CrossKind"/></param>
        public CrossItem(double Param1, double Param2, int CrossKind)
        {

            this.Param1 = System.Math.Round(Param1, 6);
            this.Param2 = System.Math.Round(Param2, 6);

            this.Tag = null;
            this.CrossKind = CrossKind;
            this.CrossList = null;
        }
        double _Param1 = -1;
        /// <summary>
        /// Param1 represents an intersection parameter . It is the value for a crosspoint.
        /// If you call for to xyArrays A and B the method A.getCrossList(B) you get
        /// the CrossPoint by A.Value(Param1) and also by B.Value(Param2);
        /// 
        /// </summary>

        public double Param1
        {
            set
            {
                if (value == 9.999999)
                { }

                _Param1 = value;
            }
            get { return _Param1; }
        }
     
        /// <summary>
        /// Param2 represents an intersectionparameter. It is the value for a crosspoint.
        /// If you call for to xyArrays A and B the method A.getCrossList(B you get
        /// the CrossPoint by A.Value(Param1) and also by B.Value(Param2);
        /// 
        /// </summary>

        public double Param2;
        /// <summary>
        /// Can be setted to indicates the kind of the Crosspoint of curve1 with curve2.
        /// </summary>
        public BorderBehavior Border1;
        /// <summary>
        /// Can be setted to indicates the kind of the Crosspoint of curve2 with curve1.
        /// </summary>
        public BorderBehavior Border2;

        /// <summary>
        /// Tag is a free useable object
        /// </summary>
        public object Tag;
        /// <summary>
        /// CrossKind gets the direction of the intersection:
        /// 1 the first xyarray is crossed from a xyarray from the right to the left side.
        /// -1 the first xyarray is crossed from a xyarray from the left to the right side.
        /// </summary>
        public int CrossKind; //  1 crossed from right to left -1 crossed left to right
        /// <summary>
        /// Crosslist is a pointer to the created Crosslist by a call of
        /// <see cref="xyArray.getCrossList(xyArray)"/>
        /// </summary>
        public CrossList CrossList;
        internal xy A;
        internal xy B;
        internal object Face = null;
    }
}
