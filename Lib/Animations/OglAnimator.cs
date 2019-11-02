using System;
using System.Collections.Generic;


//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        /// <summary>
        /// holds a list of <see cref="OglAnimator"/>.
        /// </summary>
        internal List<OglAnimator> Animators = new List<OglAnimator>();
        /// <summary>
        /// adds an <see cref="OglAnimator"/> to the internal list of <see cref="OglAnimator"/>s.
        /// This list will be played.
        /// </summary>
        /// <param name="Animator">the <b>Animator</b>, which will be added.</param>
        public void AddAnimator(OglAnimator Animator)
        {
          
            if (Animators.IndexOf(Animator) < 0)
            Animators.Add(Animator);
            
        }
        private void RemoveAnimator(OglAnimator Animator)
        {
            Animators.Remove(Animator);
        }
       private void CallAnimation()
        {

            for (int i = 0; i < Animators.Count; i++)
            {
                try
                {
                    Animators[i].DoAnimate();
                }
                catch (Exception E)
                {
                    System.Windows.Forms.MessageBox.Show(E.Message);
                
                }
              
            } 
       
        }

    }
    /// <summary>
    /// is a list of <see cref="OglAnimator"/>s, which sets the <see cref="OglAnimator.Owner"/> when you add an animation the the <see cref="AnimatorSet.ChildAnimations"/>.
    /// </summary>
    public class Children : List<OglAnimator>
    {   
        /// <summary>
        /// internal.
        /// </summary>
        internal OglAnimator Owner = null;
        /// <summary>
        /// defines the add-method new and sets the owner.
        /// </summary>
        /// <param name="Child"></param>
        public new void Add(OglAnimator Child)
        {
            base.Add(Child);
            Child.Owner = Owner;
        }
    }

    /// <summary>
    /// the base object for all animators.
    /// </summary> 
    [Serializable]
    public class OglAnimator
    {
        OpenGlDevice _Device = null;
        /// <summary>
        /// the device, in which the animator works.
        /// </summary>
        public OpenGlDevice Device
        {
            get { return _Device; }
            set { _Device = value; }
        }
       /// <summary>
       /// Event, which is called from <see cref="OnAnimate"/>
       /// </summary>
        public event EventHandler Animate;
        /// <summary>
        /// Event, which is called from <see cref="End"/>
        /// </summary>
        public event EventHandler EndAnimate;
        /// <summary>
        /// Event, which is called from <see cref="OglAnimator.Start()"/>
        /// </summary>
        public event EventHandler StartAnimate;
        private int CallCount = 0;
        /// <summary>
        /// a contructor with the device
        /// </summary>
        /// <param name="Device">the device, in which the animator runs.</param>
        public OglAnimator(OpenGlDevice Device) :this()
        {
            this.Device = Device;
        }
        /// <summary>
        /// internal
        /// </summary>
        public object Tag = null;
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public OglAnimator()
        {
      
        
           
        }
        /// <summary>
        /// is the owner. E.g. in a <see cref="AnimatorSet"/> the members of the <see cref="AnimatorSet.ChildAnimations"/> has the animatorset as owner.
        /// </summary>
        public OglAnimator Owner = null;
        /// <summary>
        /// gets the time since the start in milliseconds.
        /// </summary>
        public long CurrentTime
        { 
            get { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - _StartTime; }
        }
        /// <summary>
        /// gets a parameter between 0 and 1 by dividing the current time by the duration.
        /// </summary>
        public double TimeParam
        { 
            get {

                if ((double)Duration < (double)CurrentTime)
                    return 1;
                return (double)CurrentTime / (double)Duration; }
        }
        private int _Repeating = 0;
        /// <summary>
        /// get or sets the repating count. A value of -1 means that it repeats infinitely.
        /// </summary>
        public int Repeating
        {
            get { return _Repeating; }
            set { _Repeating = value; }

        }
          private   bool TimeElapsed()
        {
            if ((Duration == -1)||Repeating<0 ) return false;
           if (CurrentTime > Duration)
                {
                long C = CurrentTime;
                long D = Duration;

                }
            return (CurrentTime > Duration);
        }
        private bool StartTimeChecked()
        {
            return (CurrentTime >= StartDelay);
        }
        /// <summary>
        /// internal: is called by the device.
        /// </summary>
        /// <returns></returns>
        public virtual bool DoAnimate()
        { if (!IsRunning) return false;
            if (TimeElapsed())
            {
                OnAnimate();
                if ((Repeating>=0) &&(CallCount >= Repeating))

                {

                    CallCount = 0;
                    End();
                    return false;
                }
                  
                else
                {
                    CallCount++;
                    Start();
                }
            }
            if (StartTimeChecked())
            {
                   OnAnimate();
            }
            return true;
        }
        /// <summary>
        /// is called when the animator is processing.
        /// </summary>
        public virtual void OnAnimate()
        {
            if (Animate != null) Animate(this, new EventArgs());
          
        }
       /// <summary>
       /// ends the animator.
       /// </summary>
        public virtual void End()
        {
           
            if (Device != null)
                Device.Animators.Remove(this);
            _IsRunning = false;
            if (Owner != null)
                Owner.Finished(this);
          
            if (EndAnimate != null) EndAnimate(this, new EventArgs());

        }
        /// <summary>
        /// will be called if the <b>Animator</b> is a ChildAnimation. See <see cref="AnimatorSet.ChildAnimations"/>.
        /// </summary>
        /// <param name="Animator"></param>
        public virtual void Finished(OglAnimator Animator)
        {
            if (Owner != null) Owner.Finished(this);
        }
       /// <summary>
       /// resets the timer for animations.
       /// </summary>
        public void ResetTime()
        {
            _StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; 
        }
        /// <summary>
        /// starts the animator.
        /// </summary>
        /// <param name="StopallAnimators">a value true stops all running animators</param>
        public void Start(bool StopallAnimators)
        {
            if (StopallAnimators)
            {
                if (Device != null)
                    while (Device.Animators.Count > 0) Device.Animators[0].End();
                
            }
            Start();
        }
      
        /// <summary>
        /// starts the animator.
        /// </summary>
        public virtual void Start()
        {
            

            _IsRunning = true;
            if (Device != null)
                Device.AddAnimator(this);
            ResetTime();
           
            if (StartAnimate != null)
                StartAnimate(this, new EventArgs());
     
        }

            
        
        private long _Duration =3000;
        private OglAnimator SetDuration(long duration)
        {
         
            _Duration = duration;
            return this;
        }
       /// <summary>
       /// gets or sets the duration in milliseconds of the animator. Default is 3000.
       /// </summary>
        public virtual long Duration
            {
          
                get
                {
             
                    return _Duration;
                }
            set { SetDuration(value); }
            }
        private   long _StartTime = 0;
         
        private bool _IsRunning = false;
        /// <summary>
        /// indicates whether the animator is running or not.
        /// </summary>
            public virtual bool IsRunning
            {
                get
                {
                return _IsRunning;
               }
            }
         private   long _StartDelay = 0;
        /// <summary>
        /// gets or sets the time, when the animator starts running.
        /// </summary>
            public  long StartDelay
            {
                get
                {
                    return _StartDelay;
                }

                set
                {
                    _StartDelay = value;
                }
            }
           
         
         
        }
 
}
