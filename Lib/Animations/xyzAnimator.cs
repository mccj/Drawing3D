using System.Drawing;
using System;

namespace Drawing3d
{
    /// <summary>
    /// interpolates two postions: <see cref="From"/> and <see cref="To"/>.
    /// </summary>
    [Serializable]
    public class xyzAnimator : OglAnimator
    {
        public override void Start()
        {
            base.Start();
            _value = From;
        }
        /// <summary>
        /// internal
        /// </summary>

        internal static xyz Linear(xyz A, xyz B, double t)
        {
            return A * (1 - t) + B * t; 

        }
        xyz _From = new xyz(0,0,0);
      
        /// <summary>
        /// the first point for the interpolation.
        /// </summary>
        public xyz From
        {
            get { return _From; }
            set { _From = value; }
        }
        xyz  _To = new xyz(0,0,0);
        /// <summary>
        /// the second point for the interpolation.
        /// </summary>
        public xyz To
        {
            get { return _To; }
            set { _To = value; }
        }

        private xyz _value = new xyz(0,0,0);
        /// <summary>
        /// gets or sets the interpolated color.
        /// </summary>
        public xyz Value { get { return _value; } }
        /// <summary>
        /// overrides OnAnimate and calculate the interpolation.
        /// </summary>
        public override void OnAnimate()
        {


            if (Duration > 0)
                if (TimeParam <= 1)
                    _value = Linear(From, To, TimeParam);
            base.OnAnimate();

        }
    }
}

