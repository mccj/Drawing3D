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
{ /// <summary>
  /// The delegate, which is the type of <see cref="CurveArray.OnRemove"/>.
  /// </summary>
    public delegate void RemoveNotify(object sender, object obj);
    /// <summary>
    /// Is called, if a Curve is inserted in a CurveArray.
    /// </summary>
    public delegate void InsertNotify(object sender, int id, object obj);
    /// <summary>
    /// The delegate, which is the type of <see cref="CurveArray.OnInsert"/>.
    /// </summary>

    public delegate void CopyArrayNotify(CurveArray sender, ref CurveArray NewArray);


    /// <summary>
    /// This class is a container for curves. <br/>
    /// This class is a container for curves. <br/>
    /// In general the curves don`t havo to be coherently.<br/> 
    /// The Count-property is read- and writeable.
    /// A CurveArrray can be drawn directly by calling <see cref="OpenGlDevice.drawPolyCurve(CurveArray)"/>
    /// It is a ArrayList which contains an <see cref="ITransform"/>-Interface.
    /// </summary>
    [Serializable]
    public class CurveArray : ArrayList, ITransform2d
    {

        /// <summary>
        /// This operator transforms an <see cref="CurveArray"/> by a Matrix.
        /// </summary>
        /// <param name="m">Transformation matrix</param>
        /// <param name="a">CurveArray, that will be transformed.</param>
        /// <returns></returns>
        public static CurveArray operator *(Matrix3x3 m, CurveArray a)
        {
            CurveArray Result = new CurveArray(a.Count);
            for (int i = 0; i < a.Count; i++)
               Result[i]= a[i];
            return a;
        }
        /// <summary>
        /// Indicates, whether a point is inside of a curvearray.
        /// </summary>
        /// <param name="Pt">A Point</param>
        /// <returns>true, if the point is inside the curvearray</returns>
        public bool InsidePt(xy Pt)
        {
            xyArray A = getxyArray();
            return (A.Inside(Pt));

        }
      
        /// <summary>
        /// Returns the crossproduct of all interpolating points of the curve array.
        /// </summary>
        /// <returns></returns>
        public double CrossProduct()
        {
            xyArray A = getxyArray();
            return A.cross();
        }
       
        /// <summary>
        /// Gets the sense of the Curvearray.
        /// </summary>
        public bool ClockWise
        {
            get { return getxyArray().ClockWise; }

        }
        /// <summary>
        /// A free programmable object.
        /// </summary>
        [NonSerialized]
        [BrowsableAttribute(false)]
        public object Tag;
        /// <summary>
        /// The Constructor initializes a list with Count elements.
        /// These elements are of course null. So you have to fill in curves.
        /// </summary>
        /// <param name="Count"></param>
        public CurveArray(int Count)
        {
            this.Count = Count;
        }

        /// <summary>
        /// Is called, if a curve is removed from the CurveArray.
        /// </summary>
        [BrowsableAttribute(false)]
        public RemoveNotify OnRemove;
        /// <summary>
        /// Is called, if a Curve is inserted in a CurveArray.
        /// </summary>
        [BrowsableAttribute(false)]
        public InsertNotify OnInsert;
        /// <summary>
        /// Overrides the Remove-Method from ArrayList an fire the <see cref="OnRemove"/>event
        /// </summary>
        /// <param name="obj">Object the was removed from CurveArray</param>
        public override void Remove(object obj)
        {
            base.Remove(obj);
            if (OnRemove != null) OnRemove(this, obj);
        }
        /// <summary>
        /// Overrides the Insert-Method from ArrayList and fire the <see cref="OnInsert"/>event
        /// </summary>
        /// <param name="value">Object the was inserted to the CurveArray</param>
        /// <param name="index">Position, where the object should be inserted.</param>
        public override void Insert(int index, object value)
        {

            base.Insert(index, value);
            //  (value as Curve).Owner = this;
            if (OnInsert != null) OnInsert(this, index, value);
        }
        /// <summary>
        /// Overrides the Add-Method from ArrayList and sets the Owner
        /// </summary>
        /// <param name="value">A curve</param>
        /// <returns></returns>
        public override int Add(object value)
        {
            // base.Add(value);
            // (value as Curve).Owner = this;
            return base.Add(value);
        }

        /// <summary>
        /// Moves a curve to a given position in the array.
        /// </summary>
        /// <param name="C">Curve which will be moved</param>
        /// <param name="ToPosition">The new position of the curve C</param>
        public void MoveTo(Curve C, int ToPosition)
        {
            if (IndexOf(C) < 0) return;
            Insert(ToPosition, null);
            int id = IndexOf(C);
            this[ToPosition] = C;
            RemoveAt(id);
        }
        /// <summary>
        /// Inverts the array and evers curve by <see cref="Curve.Invert"/>
        /// </summary>
        public void Invert()
        {
            for (int i = 0; i < Count; i++)
            {
                Curve C = this[i];
                C.Invert();
                C.Dirty = true;
            }
            this.Reverse();
        }
        /// <summary>
        /// returns the id of the curve for which B not equals the next curve A.
        /// </summary>
        /// <returns>curve id</returns>
        public int IsClosed()
        {


            for (int i = 0; i < Count; i++)
            {
                int n = i + 1; if (n == Count) n = 0;
                if (this[i].B.dist(this[n].A) > 0.01)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// A constructor without parameters.
        /// </summary>
        public CurveArray()
        {
        }

        /// <summary>
        /// Read- and writeable property which contains the number of curves in the list.
        /// </summary>
        [BrowsableAttribute(false)]
        public new int Count
        {
            get { return base.Count; }
            set
            {
                while (value < Count) base.RemoveAt(Count - 1);
                while (value > Count) base.Add(null);
            }
        }
        /// <summary>
        /// This is the default indexer property, which deals with curves.
        /// </summary>
        public new Curve this[int i]   // Indexer declaration
        {
            get
            {


                return (Curve)base[i];

            }
            set { base[i] = (Curve)value; }
        }
       
        /// <summary>
        /// Gets the area of a CurveArray. This is valid, if the array is closed and have no windings.
        /// </summary>
        /// <returns>Value of Area</returns>
        public double Area()
        {

            xyArray a = getxyArray();
            return a.cross();
        }
        /// <summary>
        /// Get the xy-value, which is assigned to the parameter param.
        /// 
        /// </summary>
        /// <example>
        /// CA = new CurveArray();
        /// CA.Count = 4;
        /// CA[0] = new Line(new xy(0,0),new xy(3,0));
        /// CA[1] = new Line(new xy(3,0),new xy(3,2));
        /// CA[2] = new Line(new xy(3,2),new xy(0,2));
        /// CA[3] = new Line(new xy(0,2),new xy(3,0));
        /// if param = 1.5 then the point (1.5/2) is returned
        /// 
        /// 
        /// </example>
        /// <param name="param">Parameter</param>
        /// <returns>xy-value</returns>

        public xy Value(double param)

        {
            if (param < 0)
                if (System.Math.Abs(param) < 0.001) param = 0;

            int id = Utils.trunc(param);
            if (id == Count) id += -1;
            if (id == -1)
            {
                return new xy(0, 0); //Käse
            }
            double lam = param - id;
            
            return this[id].Value(lam);

        }
        /// <summary>
        /// Returns the direction at a parameter position.
        /// <seealso cref="Value"/>
        /// </summary>
        /// 
        /// <param name="param">Parametet</param>
        /// <returns>Direction</returns>
        public xy Direction(double param)
        {
            int id = Utils.trunc(param);
            if (id == Count) id += -1;
            double lam = param - id;
            return this[id].Derivation(lam);
        }
        /// <summary>
        /// Converts a parameter to the real length of the curves.
        /// <seealso cref="LengthToParam"/>
        /// </summary>
        ///
        /// <param name="param">Parameter</param>
        /// <returns>Real length</returns>
        public double ParamToLength(double param)
        {
            double result = 0;

            int id = Utils.trunc(param);
            if (id == Count) id = Count - 1;
            double lam = param - id;
            for (int i = 0; i < id; i++)
                result += this[i].CurveLength;
            result += this[id].ParamToLength(lam);
            return result;
        }
        /// <summary>
        /// Converts the real length to the parameter value.
        /// <seealso cref="LengthToParam"/>
        /// </summary>
        /// <param name="Len">Real length</param>
        /// <returns>Parameter</returns>

        public double LengthToParam(double Len)
        {
            int id = 0;
            while (Len > this[id].CurveLength)
            {
                Len -= this[id].CurveLength;
                id++;
                if (id >= Count) return Count;
            }

            return ((float)id + this[id].LengthToParam(Len));
        }
        /// <summary>
        /// The event is fired before the <see cref="Clone"/>- method starts with copying.
        /// </summary>
        public static CopyArrayNotify OnCopy;


        /// <summary>
        /// Copy the CurveArray by calling <see cref="Curve.Clone"/> for all curves.
        /// </summary>
        /// <returns>A curve array, which contains the copied curves.</returns>
        public override object Clone()
        {

            try
            {

          
            BinaryFormatter formatter = new BinaryFormatter();
            System.IO.MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, this);

            stream.Position = 0;
            CurveArray result = formatter.Deserialize(stream) as CurveArray;
            stream.Close();

            // OnChanged = S;
            return result;
            }
            catch (Exception E)
            {

                System.Windows.Forms.MessageBox.Show(E.Message);
            }
            return null;
            //CurveArray result = null;
            //if (OnCopy != null)
            //{
            //    OnCopy(this, ref result);

            //}
            //if (result == null)
            //{
            //    result = Activator.CreateInstance(GetType()) as CurveArray;
            //}
            //MemoryStream S = new MemoryStream();
            //BinaryReader R = new BinaryReader(S);
            //BinaryWriter W = new BinaryWriter(S);
            //try
            //{


            //    SaveToStream(W);
            //    S.Position = 0;
            //    result.LoadFromStream(R);


            //}
            //catch (Exception)
            //{


            //}

            //S.Close();
            //R.Close();
            //W.Close();
            //return result;
        }
        /// <summary>
        /// Gets a copy of the CurveArray.
        /// </summary>
        /// <returns></returns>
		public CurveArray Copy()
        {

            return Clone() as CurveArray;


        }
       
        /// <summary>
        /// Generate a CurveArray by eliminating all cross points of the given Array and inserting 
        /// new curves.
        /// </summary>
        /// <returns>CurveArray</returns>
        public CurveArray ToNetWork()
        {
            CurveArray result = new CurveArray();
            CrossList CL = getCrossList(this, true);
            int k = 0;
            for (int i = 0; i < CL.Count; i++)
            {
                CrossItem CI = CL[i];
                while (k < Utils.trunc(CI.Param1))
                {
                    Curve C = this[k];
                    result.Add(C.Clone());
                    k++;

                }
                double From = 0;
                while (k == Utils.trunc(CI.Param1))
                {
                    double Param = CI.Param1;
                    Param = System.Math.Round(Param, 8);
                    if (System.Math.Abs(From - Param - k) > 0.00001)
                    {
                        Curve C = this[k].Clone();
                        C.Slice(From, Param - k);
                        result.Add(C);
                    }
                    From = CI.Param1 - k;
                    if ((i + 1 < CL.Count) && (Utils.trunc(CL[i + 1].Param1) == k))
                    {
                        i++;
                        CI = CL[i];
                    }
                    else
                    {
                        if (System.Math.Abs(System.Math.Round(From, 8) - 1) > 0.00001)
                        {
                            Curve C1 = this[k].Clone();

                            double F = System.Math.Round(From, 8);
                            C1.Slice(F, 1);
                            result.Add(C1);
                        }
                        break;
                    }


                }
                k++;
            }

            while (k < Count)
            {
                Curve C = this[k];
                result.Add(C.Clone());
                k++;
            }
            return result;
        }
        /// <summary>
        /// Gets a curve C, which has the property, that C.B  is nearly the
        /// same as value.A. If no such curve exists, null will be returned.
        /// <seealso cref="Successor"/>,<seealso cref="Next"/>.
        /// </summary>
        /// 
        /// <param name="value">A Curve</param>
        /// <returns>A connecting curve</returns>
        public Curve Predecessor(Curve value)
        {
            if (!(value is Curve)) return null;

            int id = IndexOf(value);
            if (id < 0) return null;
            id--;
            if (id < 0) id = Count - 1;
            if ((this[id] is Curve) &&
                (Utils.Equals(((Curve)this[id]).B, ((Curve)value).A)))
                return this[id];
            //------- brutal -------------
            for (int i = 0; i < Count; i++)
            {
                if ((this[i] is Curve) &&
                    (Utils.Equals(((Curve)this[i]).B, ((Curve)value).A)))
                    return this[i];
            }
            return null;
        }
        /// <summary>
        /// Returns the next curve by increment the related index.
        /// If this index id is &gt;Count-1 and the CurveArray is not closed, 
        ///  null will be returned.
        /// <seealso cref="Successor"/><seealso cref="Prev"/>
        /// </summary>
        /// <param name="value">Curve</param>
        /// <returns>the next curve</returns>
        public Curve Next(Curve value)
        {
            int id = IndexOf(value);
            if (id < 0) return null;
            id++;
            if (id >= Count)
            {
                if (!Closed) return null;
                id = 0;
            }
            if (this[id] == value) return null;
            return this[id];



        }
        /// <summary>
        /// Returns a curve by decrementing the related index.
        /// If this index id is &gt;Count-1 and the CurveArray is not closed, 
        /// null will be returned.
        /// <seealso cref="Successor"/><seealso cref="Prev"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Curve Prev(Curve value)
        {
            int id = IndexOf(value);
            if (id < 0) return null;
            id--;
            if (id < 0)
            {
                if (!Closed) return null;
                id = Count - 1;
            }
            if (this[id] == value) return null;
            return this[id];
        }
        /// <summary>
        /// Gets a curve C, which has the property that C.A  is nearly the
        /// same as value.B. If such curve does not exist, null will be returned.
        /// <seealso cref="Predecessor"/>,<seealso cref="Prev"/>.
        /// </summary>
        /// 
        /// <param name="value">A Curve</param>
        /// <returns>A connecting curve</returns>
        public Curve Successor(Curve value)
        {
            if (!(value is Curve)) return null;

            int id = IndexOf(value);
            if (id < 0) return null;
            id++;
            if (id >= Count) id = 0;
            if ((this[id] is Curve) &&
                (Utils.Equals(((Curve)this[id]).A, ((Curve)value).B)))
                return this[id];
            //------- brutal -------------
            for (int i = 0; i < Count; i++)
            {
                if ((this[i] is Curve) &&
                (Utils.Equals(((Curve)this[i]).A, ((Curve)value).B)))
                    return this[i];
            }
            return null;
        }
        /// <summary>
        /// Gets and sets the property Closed, which is true, if Curve[0].A is equal 
        /// to Curve[Count-1].B.
        /// If you set this property true, a line will be inserted if the curve array was not closed.
        /// </summary>
        [BrowsableAttribute(false)]
        public bool Closed
        {
            get
            {
                if (Count == 0) return false;
                for (int i = 0; i < Count; i++)
                {
                    int n = i + 1;
                    if (n >= Count) n = 0;
                    if (this[i].B.dist(this[n].A) > 0.001) return false;

                }
                //  return this[Count - 1].Value(this[Count - 1].toParam).Equals(this[0].Value(this[0].fromParam));
                return true;
            }
            set
            {
                if (value)
                {
                    if (!Closed)
                    {
                        if (Count > 0)
                            Add(new Line(this[Count - 1].Value(1), this[0].Value(0)));
                    }

                }
                else { };

            }

        }
        /// <summary>
        /// gets a <see cref="xyArray"/>. it is closed, if addEndPoint is true;
        /// </summary>
        /// <param name="addEndPoint">if is true the returned xyarray is closed by adding a point.</param>
        /// <returns>a <see cref="xyArray"/>.</returns>
        public xyArray getxyArrayClosed(bool addEndPoint)
        {
            int Resolution = 0;
            for (int i = 0; i < Count; i++)
            {

                Curve c = this[i];

                Resolution = Resolution + c.Resolution;
            }

            Resolution++;
            xyArray xyarray = new xyArray(Resolution);
            int index = 0;
            for (int i = 0; i < Count; i++)
            {

                Curve c = this[i];
                c.ToArray(xyarray, index);
                index = index + c.Resolution;
            }
            if (!addEndPoint) xyarray.RemoveAt(xyarray.Count - 1);
            return xyarray;
        }
        /// <summary>
        /// Returns a <see cref="Drawing3d.xyArray"/> by calling the <see cref="Curve.ToArray"/>-method of the
        /// curves.
        /// </summary>
        /// <returns></returns>
        public xyArray getxyArray()
        {
            int Resolution = 0;
            for (int i = 0; i < Count; i++)
            {

                Curve c = this[i];

                Resolution = Resolution + c.Resolution;
            }
            xyArray xyarray = new xyArray(Resolution + 1);
            int index = 0;
            for (int i = 0; i < Count; i++)
            {

                Curve c = this[i];
                c.ToArray(xyarray, index);
                index = index + c.Resolution;
            }
            return xyarray;
        }

        /// <summary>
        /// Gets a minimal rectangle (transformed by a matrix), which contains the curveArray.
        /// </summary>
        /// <returns>Minimal rectangle</returns>
        public RectangleF GetMaxRect()
        {
            RectangleF R = Utils.Resetrectangle();
            RectangleF R2;

            for (int i = 0; i < Count; i++)
            {

                R2 = this[i].GetMaxrect();
                R = Utils.GetMaxRectangle(R, new xy(R2.Right, R2.Bottom));
                R = Utils.GetMaxRectangle(R, new xy(R2.Left, R2.Top));
            }
            return R;


        }


        private ArrayList GetConnections()
        {
            ArrayList Result = new ArrayList();
            if (Count == 0) return Result;
            int i = 0;
            do
            {
                CurveArray CA = new CurveArray();
                Result.Add(CA);
                CA.Add(this[i]);
                while ((i < Count - 1) && ((Curve)this[i]).B.Equals(((Curve)this[i + 1]).A))
                {
                    CA.Add(this[i + 1]);
                    i++;
                }
                i++;
            } while (i < Count);
            if (Result.Count > 1)
            {
                CurveArray First = (CurveArray)Result[0];
                CurveArray Last = (CurveArray)Result[Result.Count - 1];
                if (((Curve)First[0]).A.Equals(((Curve)Last[Last.Count - 1]).B))
                {
                    for (i = Last.Count - 1; i >= 0; i--)
                        First.Insert(0, Last[i]);
                    Result.Remove(Last);
                }
            }
            return Result;

        }

        /// <summary>
        /// If you get xyArrays by <see cref="Curve.ToArray"/> for each curve of the CurveArray,
        /// you have a point array for the whole curve array. To convert a parameter
        /// relative to this xyArray to the curveArray, you can call this method.
        /// </summary>
        /// <param name="param">Parameter relative to the xyArray</param>
        /// <returns>Parameter relative to the CurveArray</returns>
        public double xyArrayIndexToCurveArrayIndex(double param)
        {

            int i = 0;
            double PC = param;
            while ((i < Count) && (Utils.Less((float)this[i].Resolution, PC)))
            {
                PC = PC - ((float)this[i].Resolution);
                i++;
            }
            double Result = 0;
            if ((i == Count))

                return (i - 0.000001);

            Result = (float)i + PC / ((float)this[i].Resolution);
            //if (Result == Count) 
            //    Result -= 1;  // Todo
            if (Result < 0)
                Result = Result + Count;
            if (Result > Count)
                Result = Result - Count;
            return Result;
        }
        /// <summary>
        /// Calucalates a <see cref="CrossList"/> for this CurveArray with an other CurveArray.
        /// The CrossItemparameter <see cref="CrossItem.Param1"/> is relative to this CurveArray
        /// and the parameter <see cref="CrossItem.Param2"/>  relative to the "value"-CurveArray.
        /// </summary>
        /// <param name="value">An other CurveArray</param>
        /// <returns>A CrossList</returns>
        public CrossList getCrossList(CurveArray value)
        {
            return getCrossList(value, false);
        }
        /// <summary>
        /// It`s the same as the overloaded method, but you can set, whether Tangential points
        /// should be taken or not.
        /// </summary>
        /// <param name="value">An other CurveArray</param>
        /// <param name="TangentialPoints">True, if the tangential points are taken</param>
        /// <returns>A Crosslist</returns>
        public CrossList getCrossList(CurveArray value, bool TangentialPoints)
        {
            ArrayList L1 = GetConnections();
            ArrayList L2 = value.GetConnections();

            int StartJ;
            CrossList CL;
            CrossList Result = new CrossList();
            Result.AllowMultiCross = TangentialPoints;// for instant
            int AIndex = 0;

            for (int i = 0; i < L1.Count; i++)
            {
                CurveArray CA = (CurveArray)L1[i];
                xyArray A = CA.getxyArray();
                int BIndex = 0;
                StartJ = 0;
                for (int j = StartJ; j < L2.Count; j++)
                {

                    CurveArray CB = (CurveArray)L2[j];
                    xyArray B = CB.getxyArray();
                    CL = A.getCrossList(B, TangentialPoints);
                    for (int k = 0; k < CL.Count; k++)
                    {
                        CrossItem CI = CL[k];
                        double aID = CA.xyArrayIndexToCurveArrayIndex(CI.Param1);
                        double bID = CB.xyArrayIndexToCurveArrayIndex(CI.Param2);




                        CI.Param1 = (float)AIndex + CA.xyArrayIndexToCurveArrayIndex(CI.Param1);
                        CI.Param2 = (float)BIndex + CB.xyArrayIndexToCurveArrayIndex(CI.Param2);
                        Curve C1 = CA[Utils.trunc(aID)];
                        Curve C2 = CB[Utils.trunc(bID)];
                        double Lam = CI.Param1 - Utils.trunc(CI.Param1);
                        double Mue = CI.Param2 - Utils.trunc(CI.Param2);

                        CI.Param1 = IndexOf(C1) + Lam;
                        CI.Param2 = value.IndexOf(C2) + Mue;
                        Mue = CI.Param2 - Utils.trunc(CI.Param2);

                        Lam = CI.Param1 - Utils.trunc(CI.Param1);

                        if (C1.Cross(C2, Lam, Mue, out Lam, out Mue))
                        {
                            if ((Lam < 0) && ((System.Math.Abs(Lam) < 0.000001))) Lam = 0;
                            if (System.Math.Abs(Lam - 1) < 0.00001) Lam = 0.999999;
                            CI.Param1 = Utils.trunc(CI.Param1) + Lam;

                            if ((Mue < 0) && ((System.Math.Abs(Mue) < 0.000001))) Mue = 0;
                            if (System.Math.Abs(Mue - 1) < 0.00001) Mue = 0.999999;
                            CI.Param2 = Utils.trunc(CI.Param2) + Mue;

                        }

                        else
                        {


                            Lam = 0;
                            if (C1.Equals(C2))
                                Lam = 1;
                        }


                        if ((Utils.trunc(CI.Param1) < 0) && (System.Math.Abs(CI.Param1) < 0.00001)) CI.Param1 = 0;
                        if ((Utils.trunc(CI.Param2) < 0) && (System.Math.Abs(CI.Param2) < 0.00001)) CI.Param2 = 0;





                        //if (((Utils.trunc(System.Math.Round(CI.Param1,3)) !=System.Math.Round(CI.Param1,3)))
                        //    ||
                        //    ((Utils.trunc(System.Math.Round(CI.Param1,3)) !=System.Math.Round(CI.Param1,3))))
                        Result.Add(CI);



                    }
                    BIndex += CB.Count;
                }
                AIndex += CA.Count;
            }
            return Result;

        }
        /// <summary>
        /// Calucalates a <see cref="CrossList"/> for this CurveArray with an other <see cref="Loca"/>.
        /// The CrossItemparameter <see cref="CrossItem.Param1"/> is relative to this CurveArray.
        /// <br/>
        /// The parameter <see cref="CrossItem.Param2"/>  relative to the "value"-Loca.
        /// If (<see cref="CrossItem.Param2"/> greater Loca[0].Count the take id =<see cref="CrossItem.Param2"/> - Loca[0].count,
        /// If (<see cref="CrossItem.Param2"/> greater Loca[1].Count the take id =<see cref="CrossItem.Param2"/> - Loca[1].count,
        /// and so on.
        /// </summary>
        /// <param name="value">A Loca</param>
        /// <param name="TangentialPoints">Respect curves, who are tangential an intersection.</param>
        /// <returns>A CrossList</returns>
        public CrossList getCrossList(Loca value, bool TangentialPoints)
        {

            CrossList Result = new CrossList();
            int ct = 0;
            for (int i = 0; i < value.Count; i++)
            {
                CrossList CL = getCrossList(value[i], TangentialPoints);
                for (int j = 0; j < CL.Count; j++)
                {
                    CrossItem CI = CL[j];
                    CI.Param2 = CI.Param2 + ct;
                    Result.Add(CI);
                    CI.CrossList = CL;
                }
                ct += value[i].Count;
            }

            return Result;
        }


        /// <summary>
        /// Gets a minimal rectangle, that contains the CurveArray.
        /// </summary>

        public RectangleF MaxRect
        {
            get
            {

                RectangleF R = Utils.Resetrectangle();
                RectangleF R2;
                for (int i = 0; i < Count; i++)
                {
                    R2 = this[i].Maxrect;
                    R = Utils.GetMaxRectangle(R, new xy(R2.Right, R2.Bottom));
                    R = Utils.GetMaxRectangle(R, new xy(R2.Left, R2.Top));
                }
                return R;
            }
        }
        #region ITransform Member
        /// <summary>
        /// Implements the <see cref="ITransform"/>-interface by the Transfommethods of every Curve contained in
        /// the CurveArray
        /// </summary>
        /// <param name="T">Matrix of the transformation</param>
        public void Transform(Matrix3x3 T)
        {
            for (int i = 0; i < Count; i++)
                this[i].Transform(T);

        }

        #endregion
    }

}


