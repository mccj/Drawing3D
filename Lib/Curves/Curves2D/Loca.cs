using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.





namespace Drawing3d
{
    /// <summary>
	/// Loca represents a <b>L</b>ist <b>o</b>f <b>c</b>urve<b>A</b>rrays.
    /// The class is mostly needed for describing areas
    /// which are bounded by curves and have additional holes,
	/// which are bounded by curves as well.<br/>
	/// With drawPolyPolyCurve of the <see cref="OpenGlDevice"/> you can draw a Loca. 
	/// In this case the CurveArrays are assumed to be coherently. If you want to make visible holes,
	/// every curve array, which lays inside of an other, must have an orientation, which is different to that.
	/// For example:
	/// If Loca[0] has a clockwise orientation and Loca[1] lays completely inside of Loca[0] then Loca[1] must have a counterclocwise orientation.
	/// In this case you have a hole, described by Loca[1]- inside Loca[0].
	/// 
	/// </summary>
	/// 
	[Serializable]
   public class Loca : ArrayList, ITransform2d
    {

        /// <summary>
        /// Copies the loca
        /// </summary>
        /// <returns>The copied list.</returns>
        public virtual Loca Copy()
        {

            Loca Result = Activator.CreateInstance(GetType()) as Loca;
            for (int i = 0; i < this.Count; i++)
                Result.Add(this[i].Clone());

            return Result;
        }
        /// <summary>
        /// The constructor creates a list with Count entries, which are null of course. 
        /// </summary>
        /// <param name="Count"></param>
        public Loca(int Count)
        {
            this.Count = Count;
        }
        /// <summary>
        /// check all the members are clesed.
        /// </summary>
        /// <returns>true if all the curve arrays are closed.</returns>
        public bool CheckClosed()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IsClosed() >= 0) return false;
            }
            return true;
        }
        //public bool getClockwise()
        //{
        //    return Area() < 0;
        //}
        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public Loca()
        {
        }
        /// <summary>
        /// converts the loca to a <see cref="Loxy"/>
        /// </summary>
        /// <returns></returns>
        public Loxy ToLoxy()
        {
            Loxy Result = new Loxy();
            for (int i = 0; i < Count; i++)
            {
                CurveArray CA = this[i];
                Result.Add(CA.getxyArray());
            }
            return Result;
        }
        /// <summary>
        /// Set operations are applied. See also <see cref="SetOperations"/> and <see cref="Loxy.SetOperation(SetOperations, Loxy)"/>
        /// </summary>
        /// <param name="Operation">Setoperation</param>
        /// <param name="Clip">the other loca</param>
        /// <returns>the settheoretical Loxy.</returns>
        public Loxy SetOperation(SetOperations Operation, Loca Clip)
        {
           Loxy L= ToLoxy();
            Loxy LClip = Clip.ToLoxy();
            return L.SetOperation(Operation, LClip);
        }
  
        /// <summary>
        /// the default indexer of the list, which has the type CurveArray.
        /// </summary>
        public new CurveArray this[int i]   // Indexer declaration
        {
            get {if (i < Count) return (CurveArray)base[i]; return null; }
            set { base[i] = (CurveArray)value; }
        }
        /// <summary>
        /// Returns and writes the number of Curvearrays.
        /// </summary>
        [BrowsableAttribute(false)]
        public new int Count
        {
            get { return base.Count; }
            set
            {
                while (value < Count) base.RemoveAt(Count - 1);
                while (value > Count) base.Add(new CurveArray(0));
            }
        }
      
        /// <summary>
        /// Inverts the CurveArrays by calls of their invert method.
        /// </summary>
        public void Invert()
        {
            for (int i = 0; i < Count; i++)
            {
                CurveArray CA = this[i];
                CA.Invert();
            }
           

        }

        //public Loca Union(List<CurveArray> Arrays)
        //{
        //    Loca Result = new Loca();
        //    if (Arrays.Count == 0) return Result;
        //    if (Arrays.Count == 1)
        //    {
        //        Result.Add(Arrays[0]);
        //        return Result;
        //    }
        //    Result = SetOperation(Arrays[0], Arrays[1], SetOperations.None);


        //}



        //public Loca SetOperation(CurveArray value1, CurveArray value, SetOperations Operation)
        //{
        //    int i, j;
        //    Loca result = new Loca();


        //    value = value.Copy();
        //    value1 = value1.Copy();
        //    int findfirst = 1;
        //    switch (Operation)
        //    {
        //        case SetOperations.Union:
        //            {

        //                if (!value1.ClockWise)
        //                    value1.Invert();
        //                if (!value.ClockWise)
        //                    value.Invert();


        //                break;
        //            }
        //        case SetOperations.InterSection:
        //            {
        //                if (value1.ClockWise)
        //                    value1.Invert();
        //                if (value.ClockWise)
        //                    value.Invert();


        //                break;
        //            }

        //        case SetOperations.Difference:
        //            {
        //                if (value1.ClockWise)
        //                    value1.Invert();
        //                if (!value.ClockWise)
        //                    value.Invert();
        //                break;
        //            }


        //    };
        //    CrossList C = value1.getCrossList(value);
        //    if (C.Count == 0)
        //    {
        //        //    value1.ClockWise = SaveClockWise;
        //        //    value.ClockWise = SaveClockWise1;
        //        return result;
        //    }

        //    //// Sort Crossitems ordered by Param2 to Alternativ
        //    ArrayList Alternativ = new ArrayList();
        //    for (i = 0; i < C.Count; i++)
        //    {
        //        j = 0; while ((j < Alternativ.Count) && ((!(Utils.Less(C[i].Param2, ((CrossItem)Alternativ[j]).Param2))))) j++;
        //        Alternativ.Insert(j, C[i]);
        //    }
        //    CurveArray Contur = new CurveArray();
        //    double from, to;


        //    from = C[0].Param1;
        //    int first = 0;

        //    do
        //    {
        //        // find first
        //        i = 0;
        //        while ((i < C.Count) && ((C[i].Tag != null) || (C[i].CrossKind != findfirst))) i++;
        //        if (i == C.Count) break;
        //        Contur = new CurveArray();
        //        result.Add(Contur);
        //        Contur.Clear();
        //        first = i;
        //        CurveArray Work1 = null;
        //        CurveArray Work2 = null;


        //        do
        //        {
        //            from = C[i].Param1;
        //            C[i].Tag = true;
        //            i++;
        //            if (i >= C.Count) i = 0;

        //            to = C[i].Param1;
        //            Work1 = value1.Copy();
        //            Work1.Slice(from, to, Contur);
        //            C[i].Tag = true;


        //            j = Alternativ.IndexOf(C[i]);
        //            from = ((CrossItem)Alternativ[j]).Param2;
        //            ((CrossItem)Alternativ[j]).Tag = true;


        //            j++; if (j >= Alternativ.Count) j = 0;
        //            to = ((CrossItem)Alternativ[j]).Param2;
        //            ((CrossItem)Alternativ[j]).Tag = true;
        //            Work2 = value.Copy();
        //            Work2.Slice(from, to, Contur);
        //            i = C.IndexOf(Alternativ[j]);
        //            /* if (i == first)
        //             {
        //                 if (Contur.Count > 1)
        //                     if (Contur[0].dist(Contur[Contur.Count - 1]) < 0.0001)
        //                         Contur[Contur.Count - 1] = Contur[0];
        //                 break;
        //             }
        //             */
        //        } while (i != first);

        //    } while (true);
        //    //value1.ClockWise = SaveClockWise;
        //    //value.ClockWise = SaveClockWise1;
        //    return result;
        //}

        /// <summary>
        /// Implements the <see cref="ITransform"/>-interface by the Transfommethods of every CurveArray contained in
        /// the Loca.
        /// </summary>
        /// <param name="M">Matrix of the transformation</param>

        public void Transform(Matrix3x3 M)
        {
            for (int i = 0; i < Count; i++)
            {


                CurveArray CA = this[i];
                CA.Transform(M);
            }
        }
        /// <summary>
        /// calculate the area of the loca
        /// </summary>
        /// <returns>the area of the loca</returns>
        public double Area()
        {
            double Result = 0;
            for (int i = 0; i < Count; i++)
            {
               CurveArray CA = this[i];
                Result = Result + CA.Area();
            }
            return Result;
        }
    }
}
