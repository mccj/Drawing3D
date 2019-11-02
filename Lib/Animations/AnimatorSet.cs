using System.Collections.Generic;
using System;
namespace Drawing3d
{
    /// <summary>
    /// contains the list <see cref="ChildAnimations"/>. The AnimatorSet animates the animators of this list sequentially.
    /// </summary>
    [Serializable]
    public class AnimatorSet : OglAnimator
    {
        private Children _ChildAnimations = new Children();
        /// <summary>
        /// is the constructor. 
        /// </summary>
        public AnimatorSet()
        {
            _ChildAnimations.Owner = this;
        }
        /// <summary>
        /// is the constructor with the <b>Device</b> in which the animator will be played.
        /// </summary>
        /// <param name="Device">the Device in which the animator will be played.</param>
        public AnimatorSet(OpenGlDevice Device) : this()
        {
            this.Device = Device;
            _ChildAnimations.Owner = this;
        }
        /// <summary>
        /// list, which contains the animators and animates these sequentially.
        /// </summary>
        public Children ChildAnimations
        {
            get { return _ChildAnimations; }
        }
        int _Current = -1;
        bool Allways = false;
        /// <summary>
        /// gets the the actual index in <see cref="ChildAnimations"/>list. This <see cref="OglAnimator"/> is running.
        /// </summary>
        public int Current
        { get { return _Current; } }
        int _repeating = -2;
        /// <summary>
        /// overrides the start method and calls the members of <see cref="ChildAnimations"/>.
        /// </summary>
        public override void Start()
        {

            base.Start();
            if (_repeating < 0) _repeating = Repeating; // _repeating ist das aktuelle repeating
            if (_repeating == -1) Allways = true;
            for (int i = 0; i < ChildAnimations.Count; i++)
            {
                if (!(ChildAnimations[i] is AnimatorSet))
                    ChildAnimations[i].Device = null;
            }
            _Current = 0;
            if (ChildAnimations.Count > 0)
                ChildAnimations[_Current].Start();
        }
        /// <summary>
        /// overrides the <see cref="End"/> method to save the <see cref="_repeating"/> value.
        /// </summary>
        public override void End()
        {
            base.End();
            _repeating = -2;
        }
        /// <summary>
        /// overrides the <see cref="DoAnimate"/> method and calls the <b>DoAnimate</b> from the <see cref="_Current"/> <see cref="ChildAnimations"/>.
        /// </summary>
        /// <returns>true.</returns>
        public override bool DoAnimate()
        {
            ChildAnimations[_Current].DoAnimate();
            return true;
        }
        /// <summary>
        /// gets the duration as sum of the member duration in <see cref="ChildAnimations"/>.
        /// </summary>
        new public double Duration
        {
            get
            {
                if (_repeating == -1) return double.MaxValue; ;
                double Result = 0;
                for (int i = 0; i < ChildAnimations.Count; i++)

                {

                    if (ChildAnimations[i].Duration == -1)
                        return double.MaxValue;
                    Result += (ChildAnimations[i].Repeating + 1) * ChildAnimations[i].Duration + ChildAnimations[i].StartDelay;

                }

                return Result * (Repeating + 1);
            }
        }
        /// <summary>
        /// overrides the <see cref="OglAnimator.Finished(OglAnimator)"/> method to set the the next <see cref="_Current"/> to the necessary value.
        /// </summary>
        /// <param name="Animator">is the child animator. </param>
        override public void Finished(OglAnimator Animator)
        {

            int Id = ChildAnimations.IndexOf(Animator) + 1;
            if (Id < ChildAnimations.Count)
            {
                
                ChildAnimations[Id].Device = Device;
                ChildAnimations[Id].Start();
                _Current = Id;
            }
            else

            {
                _repeating--;
                if ((_repeating >= 0) || (Allways))
                    Start();
                else
            if (Owner != null)
                    Owner.Finished(this);
            }
        }
    }

}
