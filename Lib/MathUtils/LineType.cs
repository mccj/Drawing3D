using System;


namespace Drawing3d
{
    /// <summary>
    /// This structure belongs to the geometrical object 3d-line. It contains two Points P and Q and of course a direction,
    /// which is given by Q - P.
    /// In addition it contains a Valuefunction, which returns a point for every parameter lam on the line.
    /// In case lam = 0, the Value is P and in case lam = 1, the Value is Q.
    /// Sometimes the LineType is regarded as infinite line, somtimes as bounded line, bounded by P and Q.
    /// To avoid confusions with the Curveclass 'Line', it`s called LineType.
    /// 
    /// </summary>

    [Serializable]
    public struct LineType
    { 
    /// <summary>
    /// Overrides the equals-method and returns true, if point P and direction are equal.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>

    public override bool Equals(object obj)
    {
        LineType L = (LineType)obj;
        if (((L.Direction & Direction) == new xyz(0, 0, 0)) &&
            ((Direction & (L.P - P)) == new xyz(0, 0, 0))) return true;
        return false;

    }
    /// <summary>
    /// Overrides the equals-method and returns true, if point P and direction are equal.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return this.P.GetHashCode() + Q.GetHashCode();
    }

    /// <summary>
    /// The starting point of the line
    /// </summary>
    public xyz P;
    /// <summary>
    /// The endpoint of the line
    /// </summary>
    public xyz Q
    {
        get { return P + Direction; }
        set { Direction = value - P; }
    }
    /// <summary>
    /// The direction of the line i.e Q - P
    /// </summary>

    public xyz Direction;
    /// <summary>
    /// A valuator for the points on the line.
    /// The values lam between 0 and 1 give the points between P and Q.
    /// It`s the linear function P + (Q - P)*lam.
    /// </summary>
    /// <param name="lam">the parameter for the linear function P + (Q - P)*lam</param>
    /// <returns></returns>
    public xyz Value(double lam)
    {
        return P + (Q - P) * lam;
    }

    /// <summary>
    /// This constructor initializes the line by the startpoint <see cref="P"/> and the <see cref="Direction"/>.
    /// </summary>
    /// <param name="P">Starting point</param>
    /// <param name="Direction">Direction</param>
    public LineType(xyz P, xyz Direction)
    {
        this.P = P;
        this.Direction = Direction;
    }

    /// <summary>
    /// Transforms a LineType by a transformation M.
    /// <seealso cref="mul"/>
    /// </summary>
    ///
    ///<example>
    /// <code>
    /// Matrix M = Matrix.Translation( new xyz(3, 2, 4));
    /// LineType L = new LineType (new xyz(1, 1, 1), new xyz (2, 1, 3)).
    /// L = L * M;
    /// 
    /// // L.P is now ( 4, 3, 5)
    /// // L.Direction is still (2, 1, 3)
    /// 
    /// </code>
    /// </example> 
    /// <param name="L">LineType, which will be transformed</param>
    /// <param name="M">Transformation</param>
    /// <returns></returns>
    public static LineType operator *(LineType L, Matrix M)
    {
        return L.mul(M);
    }

    /// <summary>
    /// This is the same as Operator "*" 
    /// </summary>
    /// <param name="M">This matrix is used for the transformation</param>
    /// <returns>Returns a LineType, which is transform by the Matrix M</returns>
    public LineType mul(Matrix M)
    {
        xyz Q = M * (P + Direction);
        xyz R = M * P;
           
        return new LineType(R, Q - R);
    }
    /// <summary>
    /// Checks whether the LineType crosses an other LineType or not. In this method bounded lines are 
    /// considered. If the cross point is between P and Q and also between the P and Q of the other line,
    /// the result is true, otherwise it is false.
    /// </summary>
    /// <param name="L">An other LineType to check crossing</param>
    /// <returns>true, if a cross point exists between P and Q, otherwise false is returned</returns>

    public bool CrossBounded(LineType L)
    {
        double lam, mue;
        bool result = Cross(L, out lam, out mue);
        if (!result) return result;
        if (Utils.Less(0, lam)) return false;
        if (Utils.Less(0, mue)) return false;
        if (Utils.Less(lam, 1)) return false;
        if (Utils.Less(mue, 1)) return false;
        return true;
    }
    /// <summary>
    /// Calculates the cross point with an other LineType L if this exists.
    /// In this case the result is true and with the parameters lam and mue you
    /// can calculate the cross point by Value(lam) resp. L.value(mue)
    /// </summary>
    /// <param name="L">The other line to calculate the crosspoint</param>
    /// <param name="lam">Parameter to calculate the cross point with Value(lam)</param>
    /// <param name="mue">Parameter to calculate the cross point with L.Value(mue)</param>
    /// <returns></returns>

    public bool Cross(LineType L, out double lam, out double mue)
    {
        lam = -1;
        mue = -1;
        xyz VP = (Direction & L.Direction);
        xyz D = L.P + L.Direction;
        if ((VP == new xyz(0, 0, 0))) return false;
        xyz AD = D - this.P;
        if (!Utils.Equals(VP * AD, 0)) return false;
        xyz AC = L.P - this.P;
        xyz _Lam = AC & L.Direction;
        xyz _Mue = AC & Direction;
        if (!Utils.Equals(VP.x, 0))
        {
            lam = _Lam.x / VP.x;
            mue = _Mue.x / VP.x;
            return true;
        }
        if (!Utils.Equals(VP.y, 0))
        {
            lam = _Lam.y / VP.y;
            mue = _Mue.y / VP.y;
            return true;
        }
        if (!Utils.Equals(VP.z, 0))
        {
            lam = _Lam.z / VP.z;
            mue = _Mue.z / VP.z;
            return true;
        }
        return false;
    }
    /// <summary>
    /// This method calculates the distance to a point Pt.
    /// The parameter Lam can be used to calculate the nearest point of the LineType, which is 
    /// also returned by the outvalue Nearest
    /// </summary>
    /// <param name="Pt">Point to calculate the distance to the LineType</param>
    /// <param name="Lam">Parameter to calculate the nearest point</param>
    /// <param name="Nearest">Point on the Line, with the lowest distance to Pt</param>
    /// <returns>Returns the distance from the line to the point Pt</returns>
    public double Distance(xyz Pt, out double Lam, out xyz Nearest)
    {
        if (Utils.Equals(Direction.length(), 0))
        {
            Lam = 0;
            Nearest = P;
            return Pt.dist(P);
        }
        Lam = Direction.normalized().Scalarproduct(Pt.sub(P)) / Direction.length();
        Nearest = P.add(Direction.mul(Lam));
        return Nearest.dist(Pt);
    }

    /// <summary>
    /// This method calculates the distance to a Line L
    /// The parameter Lam1 can be taken to calculate the point, which has the lowest distance to
    /// the other Line L . Nearest1 = Value(Lam1).
    /// The parameter Lam2 can be taken to calculate the point of L , which has the lowest distance to
    /// the Linetype this. Nearest2 = L.Value(Lam2).
    /// </summary>
    /// <param name="L">The other LineType</param>
    /// <param name="Lam1">Paramter which belongs to this line : Nearest1 = Value(Lam1)</param>
    /// <param name="Lam2">Paramter which belongs to the Line L: Nearest2 = L.Value(Lam2)</param>
    /// <param name="Nearest1">The point on the Line, which has the smallest distance to the Line L</param>
    /// <param name="Nearest2">The point on the Line L, which has the smallest distance </param> 
    /// <returns>Returns the distance to the line L</returns>

    public double Distance(LineType L, out double Lam1, out double Lam2, out xyz Nearest1, out xyz Nearest2)
    {

     

        xyz PQ = L.P.sub(P);
        double PQV = PQ.Scalarproduct(Direction);
        double PQW = PQ.Scalarproduct(L.Direction);
        double vv = Direction.Scalarproduct(Direction);
        double ww = L.Direction.Scalarproduct(L.Direction);
        double vw = L.Direction.Scalarproduct(Direction);
        double Det = vv * ww - vw * vw;
        if (Det == 0)// parallele
        {
            xyz n = PQ.cross(Direction).cross(Direction).normalized();
            double result = PQ.Scalarproduct(n);
            Nearest2 = P.add(n.mul(result));
            Nearest1 = P;
            Lam1 = 0;
            Lam2 = L.Direction.normalized().Scalarproduct(Nearest2.sub(L.P));
            return System.Math.Abs(result);
        }
        else
        {
            Lam1 = (PQV * ww - PQW * vw) / Det;
            Lam2 = -(PQW * vv - PQV * vw) / Det;
            Nearest1 = P.add(Direction.mul(Lam1));
            Nearest2 = L.P.add(L.Direction.mul(Lam2));

            return Nearest1.dist(Nearest2);
        }

    }
        /// <summary>
        /// This method calculates the distance to a Line L
        /// The parameter Lam1 can be taken to calculate the point, which has the lowest distance to
        /// the other Line L . Nearest1 = Value(Lam1).
        /// The parameter Lam2 can be taken to calculate the point of L , which has the lowest distance to
        /// the Linetype this. Nearest2 = L.Value(Lam2).
        /// </summary>
        /// <param name="L">The other LineType</param>
        /// <param name="MaxDist">is the maximal distance for which the calculation will be made.</param>
        /// <param name="Lam1">Paramter which belongs to this line : Nearest1 = Value(Lam1)</param>
        /// <param name="Lam2">Paramter which belongs to the Line L: Nearest2 = L.Value(Lam2)</param>
        /// <returns>Returns the distance to the line L</returns>
        public double Distance(LineType L,double MaxDist, out double Lam1, out double Lam2)
        {
            xyz dummy1 = new xyz(0, 0, 0);
            xyz dummy2 = new xyz(0, 0, 0);
            Lam1 = -1;
            Lam2 = -1;
            if (Direction.length() < 0.000000001) return L.P.dist(P);
            double di= Distance(L, out Lam1, out Lam2, out dummy1, out dummy2);
        
            if (di > MaxDist)
            { Lam1 = -1; Lam2 = -1; return MaxDist; }
            else
                return di;
        }
        /// <summary>
        /// This method calculates the distance to an other line only if the distance is
        /// smaller than MaxDist, otherwise Utils.big will be returned.
        /// Imagine a cylinder with radius MaxDist around the line. If now a line passes the
        /// cylinder, a reasonable result for the lenght of the distance will be returned.
        /// If CheckP is true, additional to the Cylinder a halfsphere with center P and radius Maxdist is
        /// considered and analogusly for CheckQ.
        /// </summary>
        /// <param name="L">The other line which will be tested</param>
        /// <param name="MaxDist">The maximal distance at which a reasonable result is returned.</param>
        /// <param name="CheckP">If CheckP is true:  a line, which has to point P a distance less than Maxdist, a result will be returned. </param>
        /// <param name="CheckQ">If CheckQ is true: a line, which has to point Q a distance less than Maxdist, a result will be returned. </param>
        /// <param name="Lam">The param Lam can be taken to calculate the nearest point on the line by Value(Lam)</param>
        /// <param name="LineLam">The param relative to the other Line. The nearest point on the line by L.Value(LineLam)</param>
        /// <returns>In case the distance of the line is smaller than Maxdist, the distance is returned otherwise. <see cref="Utils.big"/>
        /// </returns>
        public double __Distance(LineType L, double MaxDist, bool CheckP, bool CheckQ, out double Lam, out double LineLam)
    {
        double dil = Utils.big;
        double dia = Utils.big;
        double dib = Utils.big, Lam1, Lam2;
        double result = Utils.big;
        xyz dummy, xyz2;
        //CheckP = false;
        //CheckQ = false;

        Lam = -1;
        LineLam = -1;
        double di = Distance(L, out Lam1, out Lam2, out dummy, out xyz2);
        xyz NN = Value(Lam1);
        if (Utils.Equals(di, Utils.big)) return di;

        if (!Utils.Less(MaxDist, di) && !Utils.Less(Lam2, 0) && !Utils.Less(1, Lam2)) dil = di;
        if ((!CheckP) && (!CheckQ))
            if (di < MaxDist)
            {
                Lam = Lam2;
                LineLam = Lam1;
                return di;
            }
        double LL = -1;
        if (CheckP)
            dia = Distance(L.P, out LL, out dummy);
        if (CheckQ)
            dib = Distance(L.Q, out LL, out dummy);

        if (!Utils.Less(MaxDist, dia) && !Utils.Less(dil, dia) && !Utils.Less(dib, dia)) { Lam = 0; LineLam = Lam1; result = dia; }
        else
            if (!Utils.Less(MaxDist, dib) && !Utils.Less(dil, dib) && !Utils.Less(dia, dib)) { Lam = 1; LineLam = Lam1; result = dib; }
        else
                if (!Utils.Less(MaxDist, dil) && Utils.Less(dil, dia) && Utils.Less(dil, dib)) { Lam = Lam2; LineLam = Lam1; result = dil; }
        return result;
    }
        /// <summary>
        /// This method calculates the distance to an other line only if the distance is
        /// smaller than MaxDist, otherwise Utils.big will be returned.
        /// Imagine a cylinder with radius MaxDist around the line. If now a line passes the
        /// cylinder, a reasonable result for the lenght of the distance will be returned.
        /// If CheckP is true, additional to the Cylinder a halfsphere with center P and radius Maxdist is
        /// considered and analogusly for CheckQ.
        /// </summary>
        /// <param name="L">The other line which will be tested</param>
        /// <param name="MaxDist">The maximal distance at which a reasonable result is returned.</param>
        /// <param name="CheckP">If CheckP is true:  a line, which has to point P a distance less than Maxdist, a result will be returned. </param>
        /// <param name="CheckQ">If CheckQ is true: a line, which has to point Q a distance less than Maxdist, a result will be returned. </param>
        /// <param name="Lam">The param Lam can be taken to calculate the nearest point on the line by Value(Lam)</param>
       /// <returns>In case the distance of the line is smaller than Maxdist, the distance is returned otherwise. <see cref="Utils.big"/>
        /// </returns>
        public double __Distance(LineType L, double MaxDist, bool CheckP, bool CheckQ, out double Lam)
    {
        double DummyLam = -1;
        return Distance(L, MaxDist, out DummyLam, out Lam);

    }



}
}
