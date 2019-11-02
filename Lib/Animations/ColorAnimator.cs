using System.Drawing;
using System;

namespace Drawing3d
{  
    /// <summary>
    /// interpolates two colors: <see cref="FromColor"/> and <see cref="ToColor"/>.
    /// </summary>
    [Serializable]
    public class ColorAnimator : OglAnimator
    {
        public override void Start()
        {
            base.Start();
            _value = FromColor;
        }
        /// <summary>
        /// internal
        /// </summary>

        internal static Color LinearColor(Color ColorFrom, Color ColorTo,double t)
        {
            if (t > 1) return ColorTo;
            if (t < 0) return ColorFrom;
            return Color.FromArgb(
               (int)( ColorFrom.A * (1 - t) + ColorTo.A * t),
                (int)(ColorFrom.R * (1 - t) + ColorTo.R * t),
                 (int)(ColorFrom.G * (1 - t) + ColorTo.G * t),
                  (int)(ColorFrom.B * (1 - t) + ColorTo.B * t));

        }
        Color _FromColor = Color.Black;
        /// <summary>
        /// the first color for the interpolation.
        /// </summary>
        public Color FromColor
        {
            get { return _FromColor; }
            set { _FromColor = value; }
        }
        Color _ToColor = Color.Black;
        /// <summary>
        /// the second color for the interpolation.
        /// </summary>
        public Color ToColor
        {
            get { return _ToColor; }
            set { _ToColor = value; }
        }

        private Color _value = Color.Black;
        /// <summary>
        /// gets or sets the interpolated color.
        /// </summary>
        public Color Value { get { return _value; } }
        /// <summary>
        /// overrides OnAnimate and calculates the interpolation.
        /// </summary>
        public override void OnAnimate()
        {


            if (Duration > 0)
                if (TimeParam <=1)
                _value = LinearColor(FromColor, ToColor, TimeParam);
              base.OnAnimate();
            
        }
    }
}
