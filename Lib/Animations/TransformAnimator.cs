using System.Collections.Generic;
using System;
namespace Drawing3d
{
    /// <summary>
    /// gets an interpolation between two tranformations: <see cref="FromTransFormation"/> and <see cref="ToTransFormation"/>
    /// </summary>
    [Serializable]
    public class TransformAnimator : OglAnimator

    {
        public override void Start()
        {
            base.Start();
            _value = FromTransFormation;
        }
        private Matrix LinearMatrix(double t)
        {
            if (t > 1) return ToTransFormation;   
           return Matrix.FromEuler( Eulerfrom * (1 - t) + Eulerto * t)*Matrix.scale(new xyz(facx1*(1-t)+facx2*t, facy1 * (1 - t) + facy2 * t, facy1 * (1 - t) + facz2 * t))*Matrix.Translation(O1*(1-t)+O2*t);
        }
        private xyz O1 = new xyz(0, 0, 0);
        private xyz O2 = new xyz(0, 0, 0);
        private double facx1 = 0;
        private double facy1 = 0;
        private double facz1 = 0;
        private double facx2 = 0;
        private double facy2 = 0;
        private double facz2 = 0;
        private xyz Eulerfrom = new xyz(0, 0, 0);
        private xyz Eulerto = new xyz(0, 0, 0);
        Matrix _value = Matrix.identity;
        /// <summary>
        /// the interpolated transformation.
        /// </summary>
        public Matrix Value { get {  return _value; } }
        private Matrix _FromTransFormation = Matrix.identity;
        /// <summary>
        /// the first transformation that will be interpolated. See <see cref="ToTransFormation"/>
        /// </summary>
        public Matrix FromTransFormation
        { get { return _FromTransFormation; }
          set { _FromTransFormation = value;
                 O1 = FromTransFormation * new xyz(0, 0, 0);
                facx1 = FromTransFormation.multaffin(new xyz(1, 0, 0)).length();
                facy1 = FromTransFormation.multaffin(new xyz(0, 1, 0)).length();
                facz1 = FromTransFormation.multaffin(new xyz(0, 0, 1)).length();
                Eulerfrom = Matrix.toEuler(FromTransFormation);
           }
       }

        private Matrix _ToTransFormation = Matrix.identity;
        /// <summary>
        /// the second transformation that will be interpolated. See <see cref="FromTransFormation"/>
        /// </summary>
        public Matrix ToTransFormation
        {
            get { return _ToTransFormation; }
            set { _ToTransFormation = value;
                O2 = ToTransFormation * new xyz(0, 0, 0);
                facx2 = ToTransFormation.multaffin(new xyz(1, 0, 0)).length();
                facy2 = ToTransFormation.multaffin(new xyz(0, 1, 0)).length();
               facz2 = ToTransFormation.multaffin(new xyz(0, 0, 1)).length();
                Eulerto = Matrix.toEuler(ToTransFormation);

            }
       }
        /// <summary>
        /// overrides OnAnimate and calculates the interpolated transformation <see cref="Value"/>.
        /// </summary>
        public override void OnAnimate()
        {
            if (Duration > 0)
            {
                if (TimeParam <= 1)
                    _value = LinearMatrix(TimeParam);
            }
            //else
            //{
            //    _value = LinearMatrix(TimeParam);

            //}
            base.OnAnimate();
        }
       
    }
}

