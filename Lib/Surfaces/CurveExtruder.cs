using System;
using System.Collections.Generic;


namespace Drawing3d
{
    /// <summary>
    /// This class defines an extrusion of a  <see cref="Curve"/>. The direction is default in z direction.
    /// </summary>
    [Serializable]
    public class CurveExtruder : Surface
    {
     
        Plane _DownPlane = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 0));
        /// <summary>
        /// is the plane, wich restricts the extrusuion from below. To acivate this plane the <see cref="Height"/> must be negative!. See also <see cref="UpPlane"/>
        /// </summary>
        public Plane DownPlane
        {
            get { return _DownPlane; }
            set
            {
                _DownPlane = value;
                 CheckPlanes();
                Invalid = true;
            }
        }

        Plane _UpPlane = new Plane(new xyz(0, 0, 0), new xyz(0, 0, 0));
        /// <summary>
        /// is the plane, wich restricts the extrusuion from above. 
        /// To acivate this plane the <see cref="Height"/> must be negative!. See also <see cref="DownPlane"/>
        /// </summary>
        public Plane UpPlane
        {
            get { return _UpPlane; }
            set
            {
                _UpPlane = value;
                CheckPlanes();
                Invalid = true;

            }
        }


        xyz _Direction = new Drawing3d.xyz(0, 0, 1);
        /// <summary>
        /// gets and sets the direction, in which it will be extruded, The default is (0,0,1).
        /// </summary>
        public xyz Direction
        {
            get { return _Direction; }
            set
            {
                _Direction = value;
                SetCurveArray();
                CheckBase();
                Invalid = true;

            }
        }

        /// <summary>
        /// overrides the <see cref="Normal(double, double)"/> method. the direction shows to the left of the curve tangent.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public override xyz Normal(double u, double v)
        {
            Base C = Base.DoComplete(new xyz(0, 0, 0), Direction);
            xy Der = Curve.Derivation(u);
            xyz P = C.BaseX * Der.x + C.BaseY * Der.y;
            return (P & Direction).normalized()* (-1);
        }

  
        double FHeight = 1;
        private Curve FCurve = new Line();
        /// <summary>
        /// is the height of the extrusion. See also <see cref="UpPlane"/> and <see cref="DownPlane"/>.
        /// </summary>
        public double Height
        {
            get { return FHeight; }
            set
            {
                FHeight = value;
                Invalid = true;

            }
        }
        /// <summary>
        /// Sets or gets the curve that will be extruded.
        /// </summary>
        public Curve Curve
        {
            get { return FCurve; }
            set
            {
                FCurve = value;
                if (value != null)
                    UResolution = value.Resolution;
                VResolution = 1;
                SetCurveArray();
                CheckPlanes();
                Invalid = true;
            }

        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            CurveExtruder Result = base.Copy() as CurveExtruder;
            Result.Curve = Curve.Clone() as Curve;
            Result.Height = Height;
            Result.Direction = Direction;
            Result.UpPlane = new Plane(UpPlane.P, UpPlane.NormalUnit);
            Result.DownPlane = new Plane(DownPlane.P, DownPlane.NormalUnit);
            return Result;
        }
        void CheckPlanes()
        {
            if (Curve != null)
           {

                if (Value(0, 1).z < Value(0, 0).z)
                { throw new System.ArgumentException("Upplane and downPlane Intersect", ""); }
                if (Value(1, 1).z < Value(1, 0).z)
                { throw new System.ArgumentException("Upplane and donPlane Intersect", ""); }
            }
        }
        Base StandardBase = Base.UnitBase;
        void CheckBase()
        {
            if (Curve != null)
                StandardBase = Base.DoComplete(new xyz(0, 0, 0), Direction);
            CurveArray = StandardBase.ToMatrix() * _CurveArray;
        }

        /// <summary>
        /// overrides the <see cref="Value(double, double)"/> method of a <see cref="Surface"/>.
        /// </summary>
        /// <param name="u">first u-parameter</param>
        /// <param name="v">second v-parameter</param>
        /// <returns>coordinate of the extruded object</returns>
        public override xyz Value(double u, double v)
        {
            double Lam = -1;
            xyz Q = new xyz(0, 0, 0);
            xy P = Curve.Value(u);
            xyz K = StandardBase.BaseO + StandardBase.BaseX * P.x + StandardBase.BaseY * P.y;

            if (Height < 0)
            {
                xyz Pkt = new xyz(0, 0, 0);
                xyz Pkt2 = new xyz(0, 0, 0);
                DownPlane.Cross(new LineType(K, Direction), out Lam, out Pkt);
                UpPlane.Cross(new LineType(K, Direction), out Lam, out Pkt2);
                xyz N = Base.Absolut(Pkt + (Pkt2 - Pkt) * v);
                if (ZHeight(u,v)>0)
                    return Base.Absolut(Pkt + (Pkt2 - Pkt) * v+Normal(u,v)*ZHeight(u,v));
                else
                return Base.Absolut(Pkt + (Pkt2 - Pkt) * v);
            }
            else
            {
                LineType L = new LineType(K, Direction);
                Plane PL = new Plane(Base.BaseO, Base.BaseZ);
                Q = new xyz(0, 0, 0);
                PL.Cross(L, out Lam, out Q);
                if (ZHeight(u, v) > 0)
                    return (Q + Direction.normalized() * (Height * v)+Normal(u,v)*ZHeight(u,v)); else
                return (Q + Direction.normalized() * (Height * v));
            }
        }
        xyzArray CurveArray = new xyzArray();
        xyzArray _CurveArray = new xyzArray();
        void SetCurveArray()
        {
            Matrix M = StandardBase.ToMatrix();// xyz K = B.BaseO + B.BaseX * P.x + B.BaseY * P.y;
            _CurveArray = new xyzArray();
            if ((Direction.length() == 0) || (Curve == null))
            { return; }

            xyArray xyA = Curve.ToXYArray();
            for (int i = 0; i < xyA.Count; i++)
            {
                _CurveArray.Add(xyA[i].toXYZ());
            }
            CurveArray = M * _CurveArray;
       }

       
        /// <summary>
        /// overrides the <see cref="ProjectPoint(xyz)"/> method of <see cref="Surface"/>.
        /// </summary>
        /// <param name="Point">Point, wich will be projected th the surface</param>
        /// <returns>u amd v parameter. A call <b>Point</b></returns>
        public override xy  ProjectPoint(xyz Point)
        {
            xyz p = Base.Relativ(Point);
            xyz PD = new xyz(0, 0, 0);
            xyz PU = new xyz(0, 0, 0);
            double Lam = -1;
            double Param = -1;
            DownPlane.Cross(new LineType(p, Direction), out Lam, out PD);
            UpPlane.Cross(new LineType(p, Direction), out Lam, out PU);
          
            xyzArray A = CurveArray;
         
            xyz R = StandardBase.Relativ(p);
           
            double u = Curve.Arcus(new xy(R.X, R.y));
            xy PP11=  Curve.Value(u);
            if (PP11.dist(new xy(R.X,R.Y))>0.5)
            { }
            else
            { }
            A.Distance(new LineType(p, Direction), 1e10, out Param, out Lam);
            if (Height < 0)
            {
               
             
                double v = p.dist(PD) / PU.dist(PD);
            
               xyz PP= Value(u, v);

                return (new xy(u, v));
            }
            else
            {
               
              
                xy pt = Curve.Value(u);
                xyz K = StandardBase.BaseX * pt.x + StandardBase.BaseY * pt.y;
                Plane P = new Plane(Base.BaseO, Base.BaseZ);
                LineType L = new LineType(K, Direction);
                xyz pkt = new xyz(0, 0, 0);
                P.Cross(L, out Lam, out pkt);
                double v = pkt.dist(p) / Height;
                xyz PP1 = Value(u, v);

                return new xy(u, v);
            }
        }

        /// <summary>
        /// Calculates the partial uDerivation
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>value of the partial uDerivation</returns>
        public override xyz uDerivation(double u, double v)
        {
            return Base.Absolut(Curve.Derivation(u).toXYZ());

        }
        /// <summary>
        /// Calculates the partial vDerivation by returning constant (0,0,1)
        /// </summary>
        /// <param name="u">u-parameter</param>
        /// <param name="v">v-parameter</param>
        /// <returns>value of the partial vDerivation</returns>

        public override xyz vDerivation(double u, double v)
        {
            return Direction;

        }



     

    }
   
}