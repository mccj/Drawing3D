
using System;
using System.Collections;
using System.Collections.Generic;

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
    /// Loxyz is a <b>L</b>ist <b>o</b>f <b>xyz</b>Arrays. This is sometimes needed in geometry, when a polygon
    /// contains holes.
    /// </summary>
    [Serializable]
    public class Loxyz : List<xyzArray>, ITransform
    {
        /// <summary>
        /// gets a Base if the array is see also <see cref="planar"/> is else
        /// the Base.Basez vector is zero.
        /// 
        /// </summary>
        public Base Base
        {get { if ((Count > 0) && (planar)) return this[0].Base;
                else return new Base();
            }


        }
        /// <summary>
        /// Indicates that the array is planar. See also <see cref="Base"/>.
        /// </summary>
        public bool planar
        { get { bool result = true;
                for (int i = 0; i < Count; i++)
                    result = result && this[i].planar;
               return result;
              }
        }

       /// <summary>
       /// multiplies the <see cref="Loxyz"/> with a <see cref="Matrix"/>.
       /// </summary>
       /// <param name="a">the matrix</param>
       /// <param name="b">the Loxyz</param>
       /// <returns></returns>
        public static Loxyz operator *(Matrix a, Loxyz b)
        {
            Loxyz Result = new Loxyz();
            for (int i = 0; i < b.Count; i++)
            {
                Result.Add(a * b[i]);
            }
           
            return Result;
        }
        #region ITransform Member

        void ITransform.Transform(Matrix T)
        {
            for (int i = 0; i < Count; i++) this[i] = T * this[i];

        }
        /// <summary>
        /// restricted the <see cref="Loxyz"/> to a <see cref="Loxy"/> by omitting the z-value.
        /// </summary>
        /// <returns>a <see cref="Loxy"/></returns>
        public Loxy ToLoxy()
        {

            Loxy result = new Loxy();
           
                for (int i = 0; i < Count; i++)
                {
                xyArray A = new xyArray(this[i].Count);
                for (int j = 0; j < A.Count; j++)
                {
                   A[j]= Base.Relativ(this[i][j]).toXY();
                }

                result.Add(A);
                }
             return result;

        }
        #endregion
        /// <summary>
        /// An array which holds the xyzArrays
        /// </summary>
        /// 
        public xyzArray[] ListOfxyzArrays
        {
            get { return this.ToArray(); }


            set { this.AddRange(value); }
        }
     
        /// <summary>
        /// The number of xyzArrays contained in the array.
        /// This property is also settable.
        /// 
        /// </summary>
        public new int Count
        {
            get
            {
                return base.Count;
                
            }
            set
            {
                if (value > Count)
                {

                    AddRange(new xyzArray[value - Count]);

                }

            }
        }
        [Serializable]
        class WorkarrayItem
        {
            public WorkarrayItem(int aid, int aList)
            {
                id = aid;
                List = aList;
            }
            public int id;
            public int List;


        }
        [Serializable]
        class Workarray : ArrayList
        {
            public new WorkarrayItem this[int i]   // Indexer declaration
            {
                get { return (WorkarrayItem)base[i]; }
                set { base[i] = value; }
            }
        }

       
        /// <summary>
        /// Calculates the cross product over all xyzArray by adding the cross products.
        /// If the Loxyz is plane then cross is a normal of this plane?
        /// </summary>
        /// <returns>Return the cross product over all xyzArrays contained in it</returns>
        public xyz cross()
        {
            xyz result = new xyz(0, 0, 0);
            for (int i = 0; i < Count; i++)
                result = result.add(this[i].cross());
            return result;

        }
    }
    /// <summary>
    /// SetOperations is a parameter of <see cref="Loxy.SetOperation"/>
    /// </summary>
    public enum SetOperations
    {

        /// <summary>
        ///  <see cref="Loxy.SetOperation"/> gets a settheoretical intersection
        /// </summary>
        InterSection,
        /// <summary>
        ///  <see cref="Loxy.SetOperation"/> gets a settheoretical union
        /// </summary>
        Union,

        /// <summary>
        ///  <see cref="Loxy.SetOperation"/> gets a settheoretical difference
        /// </summary>
        Difference,
        /// <summary>
        ///  <see cref="Loxy.SetOperation"/> gets a settheoretical difference
        /// </summary>
        Xor,
        /// <summary>
        /// No setoperation 
        /// </summary>
        None
    };
}
