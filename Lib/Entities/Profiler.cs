using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d
{
    /// <summary>
    /// is an entity, which shows a profile.F.e is needed in the lumber industry.
    /// </summary>
    [Serializable]
    public class Profiler : Entity
    {
        xyzArray _Trace = null;
        /// <summary>
        /// is the way for the profile.
        /// </summary>


        public xyzArray Trace
        {
            get { return _Trace; }
            set
            {
                _Trace = value;

            }
        }
        Loca _Transverse = null;
        /// <summary>
        /// describes the transverse section.
        /// </summary>


        public Loca Transverse
        {
            get { return _Transverse; }
            set
            {
                _Transverse = value;
 
            }
        }
        bool _CloseFirst = true;
        /// <summary>
        /// the front side will be filled, when <b>true</b>. Defualt is <b>true</b>.
        /// </summary>


        public bool CloseFirst
        {
            get { return _CloseFirst; }
            set
            {
                _CloseFirst = value;

            }
        }
        bool _CloseLast = true;
        /// <summary>
        ///  the back side will be filled, when <b>true</b>. Defualt is <b>true</b>.
        /// </summary>

        public bool CloseLast
        {
            get { return _CloseLast; }
            set
            {
                _CloseLast = value;
 
            }
        }
         /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            base.OnDraw(Device);
            if (Transverse == null) return;
            if (Transverse.Count == 0) return;

            for (int i = 1; i > 0; i--)
                if (Trace[i].dist(Trace[i - 1]) < Utils.epsilon) Trace.RemoveAt(i);
            if ((Trace.Count == 3) && (Trace.Closed())) Trace.RemoveAt(2);
            if (Trace.Count <= 1) return;
            //    if (Trace.Count <= 1) return;
            xyz Basey = new xyz(0, 0, 0);

            if (Trace.Count == 2)
            {
                Basey = new xyz(0, 0, -1);
                if ((Basey&(Trace[1]-Trace[0])).length() <0.000001)
                { Basey = new xyz(0, -1, 0); }
            }
            else
                Basey = (((Trace[2] - Trace[1]) & (Trace[0] - Trace[1]))).normalized() * (-1);

            for (int i = 0; i < Trace.Count - 1; i++)
            {
                xyz N1, N2;
                if (i == 0)
                {
                    if (Trace.Closed())
                        N1 = (Trace[i + 1] - Trace[i]).normalized() - (Trace[Trace.Count - 2] - Trace[i]).normalized();
                    else
                        N1 = (Trace[i + 1] - Trace[i]).normalized();

                    if ((i + 2) == Trace.Count)
                        N2 = (Trace[i] - Trace[i + 1]).normalized() & Basey;
                    else
                        N2 = (Trace[i] - Trace[i + 1]).normalized() - (Trace[i + 2] - Trace[i + 1]).normalized();

                }
                else
                    if (i == (Trace.Count - 2))
                {
                    N1 = (Trace[i + 1] - Trace[i]).normalized() - (Trace[i - 1] - Trace[i]).normalized();
                    if (Trace.Closed())
                        N2 = (Trace[i] - Trace[0]).normalized() - (Trace[1] - Trace[0]).normalized();
                    else
                        N2 = (Trace[i] - Trace[Trace.Count - 1]).normalized();

                }
                else
                {
                    N1 = (Trace[i + 1] - Trace[i]).normalized() - (Trace[i - 1] - Trace[i]).normalized();
                    N2 = (Trace[i] - Trace[i + 1]).normalized() - (Trace[i + 2] - Trace[i + 1]).normalized();
                }
                if (Trace.Count == 2)
                {
                    N1 = Trace[1] - Trace[0];
                    N2 = Trace[0] - Trace[1];
                }
                Plane P1 = new Plane(Trace[i], N1.normalized());
                Plane P2 = new Plane(Trace[i + 1], N2.normalized());

                if (i > 0)
                    Basey = Matrix.Mirror(P1) * Basey - Matrix.Mirror(P1) * new xyz(0, 0, 0);
                       Base B = new Base();
                        B.BaseO = Trace[i];
                        B.BaseZ = (Trace[i + 1] - Trace[i]).normalized();
                        B.BaseY = Basey.normalized();
                        B.BaseX = (B.BaseY & B.BaseZ).normalized();
                        B.BaseY = B.BaseZ & B.BaseX * (-1);
               

                
               
                
                Matrix BInvert = B.ToMatrix().invert();
                Plane DownPlane = BInvert * P1;
                Plane UpPlane = BInvert * P2;
                Device.PushMatrix();
                Device.MulMatrix(B.ToMatrix());
                for (int m = 0; m < Transverse.Count; m++)
                {
                    for (int n = 0; n < Transverse[m].Count; n++)
                    {

                        CurveExtruder CE = new CurveExtruder();
                        CE.Height = -1;
                      
                       
                        CE.DownPlane = BInvert * P1;
                        CE.UpPlane = BInvert * P2;
                        CE.Curve = Transverse[m][n];
                        CE.Paint(Device);
                        if (Device.RenderKind== RenderKind.SnapBuffer)
                        {
                            SnappItem SI = Selector.StoredSnapItems[Selector.StoredSnapItems.Count - 1];
                            if (SI!=null)
                                SI.OfObject = this;
                        }

                    }
                }
               
                double L = (Trace[i + 1] - Trace[i]).length();

                if ((i == 0) && (CloseFirst))
                    Device.drawPolyPolyCurve(Transverse);
                if ((i == Trace.Count - 2) && (CloseLast))
                {
                    Device.PushMatrix();
                    Device.MulMatrix(Matrix.Translation(new xyz(0, 0, L)));
                    Device.drawPolyPolyCurve(Transverse);
                    Device.PopMatrix();
                }
                Device.PopMatrix();
            }
        }
    }
}
