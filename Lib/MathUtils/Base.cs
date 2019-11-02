
using System;

using System.Collections;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// Base is a class which represents a base in the mathematical sense. So it contains a baseorigin and three
    /// axes for x, y and z.
    /// These values are stored in the fields BaseO, BaseX, BaseY and BaseZ, each of them from type xyz.
    /// In general doesn`t have to be normalized, but some methods require this property ( <see cref="Relativ"/>,<see cref="Absolut"/>).
    /// Bases and matrices which have (0, 0, 0, 1) in the fourth row are
    /// very similar. 
    /// If the columns of such a matrix are considered, 
    /// they represent a base when we omit the fourth coordinate.
    /// On the other side, a base defines a transformation matrix, if baseX, baseY, baseZ, baseO are taken as columns
    /// of a matrix (and adding  (0, 0, 0, 1) in the fourth row).
    /// <see cref="FromString(string)"/>
    /// </summary>
    [Serializable]

    public struct Base
    {   /// <summary>
        /// Calculates a string belonging to the Base.e.g. to the unit base you get 0/0/0; 1/0/0; 0/1/0/;0/0/1
        /// </summary>
        /// <returns>string belonging to the Base</returns>
        public override string ToString()
        {
            return "[" + BaseO.ToString() + ";" + BaseX.ToString() + ";" + BaseY.ToString() + ";" + BaseZ.ToString() + "]";
        }
        /// <summary>
        /// Calculates a base from a string f.e: string s = "0/0/0; 1/0/0; 0/1/0/;0/0/1";
        /// </summary>
        /// <param name="value">a base</param>
        /// <returns></returns>
        public static Base FromString(string value)
        {
            Base result = new Base();
            string[] s = value.Split(new char[3] { ';', '[', ']' });

            int k = 0;
            ArrayList a = new ArrayList();
            for (k = 0; k < s.Length; k++)
                if (s[k].Trim() != "") a.Add(s[k]);
            if (a.Count != 4)
                throw new Exception("Converterror to Base");
            result.BaseO = xyz.FromString((string)a[0]);
            result.BaseX = xyz.FromString((string)a[1]);
            result.BaseY = xyz.FromString((string)a[2]);
            result.BaseZ = xyz.FromString((string)a[3]);

            return result;
        }

        /// <summary>
        /// Overrides the Equals-method. Two bases are equal, if the parameter BaseO, BaseX,BaseY and BaseZ are equals
        /// in the sense of <see cref="xyz.Equals"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Base)
            {
                Base Other = (Base)obj;
                return (BaseO.Equals(Other.BaseO) &&
                        BaseX.Equals(Other.BaseX) &&
                        BaseY.Equals(Other.BaseY) &&
                        BaseZ.Equals(Other.BaseZ));

            }
            else
                if (obj is String)
            {

                return true;
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// Overrides GetHashCode. 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return BaseO.GetHashCode() +
                    BaseX.GetHashCode() +
                    BaseY.GetHashCode() +
                    BaseZ.GetHashCode();
        }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="BaseO">Origin of the base</param>
        /// <param name="BaseX">x-axis of the base</param>
        /// <param name="BaseY">y-axis of the base</param>
        /// <param name="BaseZ">z-axis of the base</param>
        public Base(xyz BaseO, xyz BaseX, xyz BaseY, xyz BaseZ)
        {
            this.BaseO = BaseO;
            this.BaseX = BaseX;
            this.BaseY = BaseY;
            this.BaseZ = BaseZ;


        }
        /// <summary>
        /// Searches three independent points of A, B, C, D and
        /// calls <see cref="From3IndependentPoints"/> for this three points and returns this base.
        /// </summary>
        /// <param name="A">1.Point</param>
        /// <param name="B">2.Point</param>
        /// <param name="C">3.Point</param>
        /// <param name="D">4.Point</param>
        /// <returns>A normalized base</returns>
        public static Base From4Points(xyz A, xyz B, xyz C, xyz D)
        {
            if (((B - A) & (C - A)).length() > 0.0001)
                return Base.From3IndependentPoints(A, B, C);
            if (((C - A) & (D - A)).length() > 0.0001)
                return Base.From3IndependentPoints(A, D, C);
            if (((C - B) & (D - B)).length() > 0.0001)
                return Base.From3IndependentPoints(B, C, D);
            if (((D - B) & (A - B)).length() > 0.0001)
                return Base.From3IndependentPoints(B, D, A);

            throw new Exception("No independent points to create a Base");

        }
        /// <summary>
        /// Gets a Base with Origin "Origin" <br/>
        /// the x-axis has the direction B-Origin<br/>
        /// the y-axis is in the plane of the x-axis and the Point C <br/>
        /// the z-axis is normal to this plane
        /// <remarks>the three points have to be independent ( not in a line ></remarks>
        /// </summary>
        /// <param name="Origin">The origin of the desired base</param>
        /// <param name="B">the direction B-Origin gives the x-axis</param>
        /// <param name="C">the y-axis is in the plane of the x-axis and this Point C</param>
        /// <returns>a normalized base</returns>
        public static Base From3IndependentPoints(xyz Origin, xyz B, xyz C)
        {
            Base Result = Base.UnitBase;

            xyz BA = B - Origin;
            xyz CA = C - Origin;
            Result.BaseZ = BA & CA;
            Result.BaseZ = Result.BaseZ.normalized();
            if (Result.BaseZ.length() < 0.000001)
            {
                throw new Exception("No independent points to create a Base");
            }
            Result.BaseX = (CA & Result.BaseZ).normalized();
            Result.BaseY = Result.BaseZ & Result.BaseX;
            Result.BaseO = Origin;
            return Result;
        }
        /// <summary>
        /// Tries to complete the point Origin and the Zaxis to a normalized Base and returns this.
        /// </summary>
        /// <param name="Origin">The origin of the desired base</param>
        /// <param name="ZAxis">The z-Axis</param>
        /// <returns>a normalized Base</returns>
        public static Base DoComplete(xyz Origin, xyz ZAxis)
        {
            Base Result = new Base();
            Result.BaseO = Origin;
            Result.BaseZ = ZAxis.normalized();
            if (((Result.BaseZ & (new xyz(0, 1, 0))).length()) > 0.01)
                Result.BaseX = (new xyz(0, 1, 0) & Result.BaseZ).normalized();
            else
                Result.BaseX = (new xyz(1, 0, 0) & Result.BaseZ).normalized();
            Result.BaseY = Result.BaseZ & Result.BaseX;
            return Result;
        }
        /// <summary>
        /// Produce an orthogonal base with Origin a z-axis,the x-axis and a y-axis, which is normal to XAxis cross YVector
        /// </summary>
        /// <param name="Origin">The origin</param>
        /// <param name="XAxis">the x axis</param>
        /// <param name="YVector">the x axis is normal to XAxis cross YVector</param>
        /// <returns>orthogonal base</returns>
        public static Base DoComplete(xyz Origin, xyz XAxis, xyz YVector)
        {
            Base Result = new Base();
            if (YVector.length() == 0)
            {
                YVector = XAxis & new xyz(0, 0, 1);
                if (YVector.length() == 0)
                    YVector = XAxis & new xyz(0, 1, 0);
                if (YVector.length() == 0)
                    YVector = XAxis & new xyz(1, 0, 0);

            }
            Result.BaseO = Origin;
            Result.BaseX = XAxis.normalized();
            Result.BaseZ = (XAxis & YVector).normalized();

            Result.BaseY = Result.BaseZ & Result.BaseX;

            return Result;
        }
        /// <summary>
        /// Implements an operator '*' which results
        /// the multiplication between a matrix M and a Base b.
        /// </summary>
        /// <param name="M">A Traformation</param>
        /// <param name="b">A base</param>
        /// <returns></returns>
        public static Base operator *(Matrix M, Base b)
        {
            Base Result = new Base();
            xyz n = M * new xyz(0, 0, 0);
            Result.BaseO = M * b.BaseO;
            Result.BaseX = (M * b.BaseX) - n;
            Result.BaseY = (M * b.BaseY) - n;
            Result.BaseZ = (M * b.BaseZ) - n;
            return Result;
        }

        /// <summary>
        /// Returns a base with Origin "Cameraposition"<br/> 
        /// the z-axis is the direction from CameraTarget to CameraPosition<br/>
        /// and the CameraUpVector fixes the y-axis
        /// </summary>
        /// <param name="CameraPosition">Position of the camera</param>
        /// <param name="CameraTarget">Looks to that point</param>
        /// <param name="CameraUpVector">Normalvector of the camera</param>
        /// <returns>A normalized </returns>
        public static Base LookAt(xyz CameraPosition, xyz CameraTarget, xyz CameraUpVector)
        {
            Base B = Base.UnitBase;
            B.BaseO = CameraPosition;
            B.BaseZ = ((CameraTarget - CameraPosition).normalized()).mul(-1);
            B.BaseX = (CameraUpVector & B.BaseZ).normalized();
            B.BaseY = B.BaseZ & B.BaseX;


            return B;

        }


        /// <summary>
        /// Origin of the Base
        /// </summary>
        public xyz BaseO;
        /// <summary>
        /// x-axis
        /// </summary>
        public xyz BaseX;
        /// <summary>
        /// y-axis
        /// </summary>
        public xyz BaseY;
        /// <summary>
        /// z-axis
        /// </summary>
        public xyz BaseZ;
        /// <summary>
        /// The constructor produces a copy of the initBase, which is used as parameter.
        /// In the most cases the <see cref="UnitBase"/> would be taken
        /// <code>
        /// Base b = new Base(UnitBase);
        /// 
        /// </code>
        /// </summary>
        /// <param name="initBase">a clone for the new base</param>

        public Base(Base initBase)
        {

            BaseO = initBase.BaseO;
            BaseX = initBase.BaseX;
            BaseY = initBase.BaseY;
            BaseZ = initBase.BaseZ;
        }
        /// <summary>
        /// This constructor initializes the base with unitbase
        /// </summary>
        /// <param name="Init">No matter</param>
        public Base(bool Init)
        {

            BaseO = UnitBase.BaseO;
            BaseX = UnitBase.BaseX;
            BaseY = UnitBase.BaseY;
            BaseZ = UnitBase.BaseZ;
        }
        /// <summary>
        /// UnitBase is the standard normalized base with BaseO = (0, 0, 0)
        /// </summary>
        public static Base UnitBase
        {
            get { return new Base(new xyz(0, 0, 0), new xyz(1, 0, 0), new xyz(0, 1, 0), new xyz(0, 0, 1)); }

        }

        /// <summary>
        /// The base will be transformed by the Matrix m.
        /// </summary>
        /// <param name="m">m holds the transform matrix which will be applied to the base</param>
        /// <returns></returns>
        public Base mul(Matrix m)
        {
            Base result = new Base();
            result.BaseO = BaseO.mul(m);
            xyz InHomogen = m * new xyz(0, 0, 0);
            result.BaseX = BaseX.mul(m) - InHomogen;
            result.BaseY = BaseY.mul(m) - InHomogen;
            result.BaseZ = BaseZ.mul(m) - InHomogen;
            return result;
        }
        /// <summary>
        /// Calculates the relative coordinates from point.
        /// The base is assumed to be normalized
        /// The invert method is <see cref="Absolut"/>.
        /// </summary>
        /// <param name="P">Point</param>
        /// <returns>returns the coordinates relative to the base</returns>
        public xyz Relativ(xyz P)
        {
            if ((System.Math.Abs(BaseX * BaseY) < double.Epsilon) && (System.Math.Abs(BaseX * BaseZ) < double.Epsilon) && (System.Math.Abs(BaseZ * BaseY) < double.Epsilon)
                ) // Orthogonal
            {
                xyz d = P.sub(BaseO);

                return new xyz(d.Scalarproduct(BaseX.normalized()), d.Scalarproduct(BaseY.normalized()), d.Scalarproduct(BaseZ.normalized()));
            }

            P = P - BaseO;
            double Det = xyz.dot(BaseX, BaseY, BaseZ);
            if (System.Math.Abs(Det) < 0.0000000000000000001)
            {
                xyz d = P.sub(BaseO);
                return new xyz(d.Scalarproduct(BaseX), d.Scalarproduct(BaseY), d.Scalarproduct(BaseZ));
            }
            else
            {
            }
            return new xyz(xyz.dot(P, BaseY, BaseZ) / Det, xyz.dot(BaseX, P, BaseZ) / Det, xyz.dot(BaseX, BaseY, P) / Det);

        }
        /// <summary>
        /// Calculates the relative coordinates from the point.
        /// It`s not necessary to normalize the base.
        /// See also <see cref="Relativ"/>
        /// The invert method is <see cref="Absolut"/>.
        /// </summary>
        /// <param name="Pt">Point</param>
        /// <returns>returns the coords relative to the base</returns>

        public xyz RelativAffin(xyz Pt)
        {
            Matrix M = this.ToMatrix();
            return M.invert() * Pt;
        }

        /// <summary>
        /// Calculates the absolute coordinates of the point, whose coordinates are related to the Base.
        /// The base is assumed to be normalized.
        /// The invert method is <see cref="Relativ"/>.
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>returns the absolut coordinates in a worldbase</returns>
        public xyz Absolut(xyz pt)
        {
            //xyz d=pt.sub(BaseO);
            if ((!Utils.Equals(BaseX.length(), 1f)) || (!Utils.Equals(BaseY.length(), 1f)) || (!Utils.Equals(BaseZ.length(), 1f)))
            {
            }
            return BaseO.add(BaseX.mul(pt.x)).add(BaseY.mul(pt.y)).add(BaseZ.mul(pt.z));
        }
        /// <summary>
        /// Tries to make a orthogonal base and returns this.
        /// </summary>
        /// <returns></returns>
        public Base Orthogonalyze()
        {
            Base Result = new Base();
            Result.BaseO = BaseO;
            Result.BaseX = BaseX.normalized();
            Result.BaseZ = (BaseX & BaseY).normalized();
            Result.BaseY = (Result.BaseZ & Result.BaseX);
            return Result;
        }



        /// <summary>
        /// Converts a base to matrix by setting the columns to BaseX, BaseY, BaseZ, BaseO 
        /// and adding (0, 0, 0, 1) in the fourth row.
        /// The invers method is <see cref="Matrix.toBase"/>
        /// </summary>
        /// <returns>a matrix, which is associated to the base</returns>
        public Matrix ToMatrix()
        {
            Matrix a = new Matrix(1);
            a.a00 = (float)BaseX.x;
            a.a10 = (float)BaseX.y;
            a.a20 = (float)BaseX.z;
            a.a01 = (float)BaseY.x;
            a.a11 = (float)BaseY.y;
            a.a21 = (float)BaseY.z;
            a.a02 = (float)BaseZ.x;
            a.a12 = (float)BaseZ.y;
            a.a22 = (float)BaseZ.z;
            a.a03 = (float)BaseO.x;
            a.a13 = (float)BaseO.y;
            a.a23 = (float)BaseO.z;
            a.a33 = (float)1;

            return a;
        }

    }
}
