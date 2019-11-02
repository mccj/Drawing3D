using System;

namespace Drawing3d
{
    /// <summary>
    /// is an <see cref="Entity"/>, which rotates a <see cref="Loca"/> between to angles the <see cref="FromAngle"/> and the <see cref="ToAngle"/>.
    /// </summary>
    [Serializable]
    public class PolyCurveRotator : Entity
    {
        double _FromAngle = 0;
        Loca _Loca = null;
        double _ToAngle = Math.PI * 2;
        /// <summary>
        /// FromAngle is clockwis to the x-axis.
        /// </summary>
       public double FromAngle
        {
            get { return _FromAngle; }
            set
            {
                _FromAngle = value;
 
            }
        }
        /// <summary>
        /// ToAngle is clockwis to the x-axis.
        /// </summary>
        public double ToAngle
        {
            get { return _ToAngle; }
            set
            {
                _ToAngle = value;

            }
        }
        /// <summary>
        /// is the <see cref="Loca"/>, which will be rotated.
        /// </summary>
        public Loca Loca
        {
            get { return _Loca; }
            set
            {
                _Loca = value;
 
            }
        }
        /// <summary>
        /// overrides <see cref="CustomEntity.OnDraw(OpenGlDevice)"/>
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            if (Loca == null) return;
            Curve2dRotator CR = new Curve2dRotator();
          
            for (int i = 0; i < Loca.Count; i++)
            {
                for (int j = 0; j < Loca[i].Count; j++)
                {
                    CR.Curve = Loca[i][j];
                    CR.FromAngle = FromAngle;
                    CR.ToAngle = ToAngle;
                    CR.Paint(Device);
                }
            }
            base.OnDraw(Device);
        }
    }

}
