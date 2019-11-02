using System;
namespace Drawing3d
{
    /// <summary>
    /// contains the values <see cref="FromValue"/> and <see cref="ToValue"/>.
    /// In the field <see cref="Value"/> there is the line linear value.
    /// </summary>
    [Serializable]
    public class LinearAnimator  : OglAnimator
    {/// <summary>
     /// a constructor with a device.
     /// </summary>
     /// <param name="Device">the device in which the animator works.</param>
        public LinearAnimator(OpenGlDevice Device):base(Device)
        {

        }
        /// <summary>
        /// empty constructor.
        /// </summary>
        public LinearAnimator() : base()
        { }
        /// <summary>
        /// a consturctor with device, from, to and duration.
        /// Rem: SpeedInUnitsPerMilliSec is set to 1.
        /// </summary>
        /// <param name="Device">the device in which the animator works.</param>
        /// <param name="from">from value</param>
        /// <param name="to">tovalue</param>
        /// <param name="Duration">duration</param>
        public LinearAnimator(OpenGlDevice Device, float from, float to, long Duration)
            : base(Device)
        {
            this.FromValue = from;
            this.ToValue = to;
            this.Duration = Duration;
        }
        /// <summary>
        /// overrides the <see cref="Start"/> method.
        /// </summary>
        public override void Start()
        {
            ResetTime();
      
            base.Start();


            _value = FromValue;

            ResetTime();

           
       
            if ((SpeedInUnitsPerMilliSec==0) && (Duration >0))
            SpeedInUnitsPerMilliSec = Math.Abs(ToValue - FromValue) / (float)Duration;
        }
        /// <summary>
        /// a consturctor with device, from, to and SpeedInUnitsPerMilliSec.
        /// </summary>
        /// <param name="Device">the device in which the animator works.</param>
        /// <param name="from">from value</param>
        /// <param name="to">to value</param>
        /// <param name="SpeedInUnitsPerMilliSec"></param>
        public LinearAnimator(OpenGlDevice Device, float from, float to, float SpeedInUnitsPerMilliSec)
            : base(Device)
        {
            this.FromValue = from;
            this.ToValue = to;
           
            this.SpeedInUnitsPerMilliSec = SpeedInUnitsPerMilliSec;
        }
        
        private float _FromValue = 0;
        /// <summary>
        /// gets or set the from value
        /// </summary>
        public float FromValue
        {
            get { return _FromValue; }
            set { _FromValue = value; }
        }
        /// <summary>
        /// gets or sets the speed.
        /// </summary>
        public double SpeedInUnitsPerMilliSec=1;
        /// <summary>
        /// gets or sets the to value
        /// </summary>
        public float ToValue;
        private double _value = 0;
        /// <summary>
        /// gets the linear value between fromvalue and tovalue
        /// </summary>
        public double Value { get { return _value; } }
        /// <summary>
        /// overrides OnAnimate and calculates the <see cref="Value"/>.
        /// </summary>
        public override void OnAnimate()
        {

            {
                if ((TimeParam <= 1))
                    _value = FromValue * (1 - TimeParam)  + ToValue * TimeParam ;
             
            }

            base.OnAnimate();
           
        }
        /// <summary>
        /// the duration in milliseconds.
        /// </summary>
        public override long Duration
        {
            get
            {
                return base.Duration;
            }

            set
            {
                SpeedInUnitsPerMilliSec = Math.Abs(ToValue - FromValue) / (float)value;
                base.Duration = value;
            }
        }

    }
}
