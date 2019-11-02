using System;
using System.Collections.Generic;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// constructor with a <see cref="OpenGlDevice"/>.
    /// </summary>
    [Serializable]
    class GoForwardAnimator : LinearAnimator
    {
        float LastPos = 0;
        /// <summary>
        /// internal.
        /// </summary>
        internal LineType L = new LineType();
        /// <summary>
        /// overrides the start method from <see cref="LinearAnimator"/>.
        /// </summary>
        public override void Start()
        {
            LastPos = FromValue;
            base.Start();
        }
        /// <summary>
        /// overrides the OnAnimate method from <see cref="LinearAnimator"/>. 
        /// </summary>
        public override void OnAnimate()
        {
            base.OnAnimate();
            {
                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Translation(L.Direction.normalized() * (Value - LastPos));
                LastPos = (float)Value;
            }
        } 
        /// <summary>
        /// constructor with a <see cref="OpenGlDevice"/>.
        /// </summary>
        /// <param name="myDevice">a <see cref="OpenGlDevice"/>.</param>
        public GoForwardAnimator(OpenGlDevice myDevice):base(myDevice)
        {

            
        }
    }
    /// <summary>
    /// is used from <see cref="Camera"/> and allows animated rotations, like <see cref="LookRight(double, bool)"/>.
    /// </summary>
    [Serializable]
    public class AnimatedCamera
    {
        [Serializable]
        class RotationAnimator : LinearAnimator
        {
            public RotationAnimator(OpenGlDevice myDevice) : base(myDevice)
            {

            }

            private List<Entity> Entities = new List<Entity>();
            
            private Matrix StartProjectionMatrix = Matrix.identity;
            private float LastAngle = 0;
            public override void Start()
            {
                LastAngle = FromValue;
                StartProjectionMatrix = Device.ProjectionMatrix;
                base.Start();
            }
          
            public override void OnAnimate()
            {
                base.OnAnimate();
                if (Entities.Count == 0)
                {
                    Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(RotationAxis, Value - LastAngle);
                    LastAngle = (float)Value;
                }
                else

                {
                    for (int i = 0; i < Entities.Count; i++)
                    {
                        Entities[i].Transformation = Matrix.Rotation(RotationAxis, Value);
                        if (Value > 2 * System.Math.PI)
                        { }
                    }

                }
            }
        }
       [NonSerialized]
        private RotationAnimator LeftRight = null;
        [NonSerialized]
        private RotationAnimator UpDown = null;
        [NonSerialized]
        private RotationAnimator Roll = null;
        [NonSerialized]
        private GoForwardAnimator Forward = null;
        [NonSerialized]
        private   Camera Camera = null;
        private OpenGlDevice Device { get { return Camera.Device; } }
        private xyz Center { get { return Camera.Anchor; } }
        private Base Base { get { return Camera.Base; } }
        private xyz Position { get { return Camera.Position; } }
        private xyz UpVector { get { return Camera.UpVector; } }
        /// <summary>
        /// internal
        /// </summary>
        internal static  LineType RotationAxis = new LineType();
        private void SetAnimatorSpeed(RotationAnimator Animator, int Kind, double Speed, bool KeepCenter)
        {

            Camera.MakeConsistent();
            Animator.End();
            Animator.Duration = -1;
            Animator.SpeedInUnitsPerMilliSec = Speed;
            switch (Kind)
            {
                case 0:// LeftRight
                    if (KeepCenter)
                        RotationAxis = new LineType(Center, UpVector);
                    else
                        RotationAxis = new LineType(Position, UpVector);
                    break;
                case 1:// UpDown
                    if (KeepCenter)
                        RotationAxis = new LineType(Center, Base.BaseX);
                    else
                        RotationAxis = new LineType(Position, Base.BaseX);
                    break;
                case 2:// Rollup
                    RotationAxis = new LineType(Position, Base.BaseZ);
                    break;
                default:
                    break;
            }
            Animator.Start();
        }
        private void SetAnimator(RotationAnimator Animator, int Kind, float value, long Duration, bool KeepCenter)
        {

            Camera.MakeConsistent();
            Animator.End();
            Animator.Duration = Duration;
            Animator.ToValue = value;
            switch (Kind)
            {
                case 0:// LeftRight
                    if (KeepCenter)
                        RotationAxis = new LineType(Center, UpVector);
                    else
                        RotationAxis = new LineType(Position, UpVector);
                    break;
                case 1:// UpDown
                    if (KeepCenter)
                        RotationAxis = new LineType(Center, Base.BaseX);
                    else
                        RotationAxis = new LineType(Position, Base.BaseX);
                    break;
                case 2:// Rollup
                    RotationAxis = new LineType(Position, Base.BaseZ);
                    break;
                default:
                    break;
            }
            Animator.Start();
        }
        /// <summary>
        /// stops all animators.
        /// </summary>
        public void StopAllAnimations()
        {
            LeftRight.End();
            UpDown.End();
            Roll.End();
            Forward.End();
        }
        /// <summary>
        /// constructor with a device. Is called from <see cref="Camera"/>
        /// </summary>
        /// <param name="Camera"></param>
        public AnimatedCamera(Camera Camera)
        {
            this.Camera = Camera;
            LeftRight = new RotationAnimator(Device);
            UpDown = new RotationAnimator(Device);
            Roll = new RotationAnimator(Device);
            Forward = new GoForwardAnimator(Device);
           
        }
        /// <summary>
        /// walks forward infinitly. See <see cref="StopAllAnimations"/>
        /// </summary>
        /// <param name="SpeedInUnitsPerMilliSec">The speed for the walk.</param>
        /// <returns></returns>
        public OglAnimator WalkForward(double SpeedInUnitsPerMilliSec)
        {
            Camera.MakeConsistent();
            Forward.L = new LineType(Base.BaseO, Base.BaseZ);
            Forward.SpeedInUnitsPerMilliSec = SpeedInUnitsPerMilliSec;
            Forward.FromValue = 0;
            Forward.ToValue = 1;
            Forward.Duration = -1;
            Forward.Start();
            return Forward;
        }
        /// <summary>
        /// walks forward infinitly. See <see cref="StopAllAnimations"/>
        /// </summary>
        /// <param name="Distance">the distance in world units.</param>
        /// <param name="Duration">the duration.</param>
        /// <returns></returns>
        public OglAnimator WalkForward(float Distance, long Duration )
        {
            Camera.MakeConsistent();
            Forward.L = new LineType(Base.BaseO, Base.BaseZ);
            Forward.SpeedInUnitsPerMilliSec = Distance*1000/Duration;
            Forward.FromValue = 0;
            Forward.ToValue = Distance;
            Forward.Duration = Duration;
            Forward.Start();
            return Forward;
        }
        /// <summary>
        /// looks down with Angle, Duration,  KeepCenter
        /// </summary>
        /// <param name="Angle">angle for the rotation</param>
        /// <param name="Duration">duration in millisec.</param>
        /// <param name="KeepCenter">if is true <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns>an OglAnimator. </returns>
        public OglAnimator LookDown(float Angle, long Duration, bool KeepCenter)
        {
            if (Duration <1)
            {
                LineType L = new LineType();
                    if (KeepCenter)
                    L = new LineType(Center, Base.BaseX);
                else
                    L = new LineType(Position, Base.BaseX);
               
              
                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
                Device.Refresh();
            }
            else
            SetAnimator(UpDown, 1, Angle, Duration, KeepCenter);
            return UpDown;
        }
        /// <summary>
        /// looks down with degree per millisecond,   KeepCenter
        /// </summary>
        /// <param name="RadPerMilliSecond">speed in rad per millisecond </param>
        /// <param name="KeepCenter">if is true, the <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns></returns>
        public OglAnimator LookDown( double RadPerMilliSecond, bool KeepCenter)
        {
            SetAnimatorSpeed(UpDown, 1, RadPerMilliSecond, KeepCenter);
            return UpDown;
        }
        /// <summary>
        /// rotates around the z-axis of the camera.
        /// </summary>
        /// <param name="Angle">angle in rad.</param>
        /// <param name="Duration">duration</param>
        /// <returns>OglAnimator</returns>
        public OglAnimator RollRight(float Angle, long Duration)
        {
            if (Duration < 1)
            {
                LineType L = new LineType();
               
                    L = new LineType(Position, Base.BaseZ);
                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
                Device.Refresh();
            }
            else
                SetAnimator(Roll, 2, Angle, Duration, false);
            return Roll;
        }
        /// <summary>
        /// rotates around the z-axis of the camera.
        /// </summary>
        /// <param name="Angle">angle.</param>
        /// <param name="Duration">duration.</param>
        /// <param name="KeepCenter">if is true, the <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns>OglAnimator</returns>
        public OglAnimator RollRight(float Angle, long Duration, bool KeepCenter)
        {
            SetAnimator(UpDown, 2, Angle, Duration, KeepCenter);
            return Roll;
        }
        /// <summary>
        /// rotates around the z-axis of the camera.
        /// </summary>
        /// <param name="RadPerMilliSecond">speed in rad per millisecond</param>
        /// <param name="KeepCenter">if is true, the <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns>OglAnimator</returns>
        public OglAnimator RollRight(double RadPerMilliSecond, bool KeepCenter)
        {

            SetAnimatorSpeed(Roll, 2, RadPerMilliSecond, KeepCenter);
            return Roll;
        }
        /// <summary>
        /// rotates to right.
        /// </summary>
        /// <param name="Angle">angle.</param>
        /// <param name="Duration">duration in millisecond.</param>
        /// <param name="KeepCenter">if is true, the <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns>OglAnimator</returns>
        public OglAnimator LookRight(float Angle, long Duration, bool KeepCenter)
        {
            if (Duration < 1)
            {
                LineType L = new LineType();
                if (KeepCenter)
                    L = new LineType(Center, Base.BaseY);
                else
                    L = new LineType(Position, Base.BaseY);

               
                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
                Device.Refresh();
            }
            else
            SetAnimator(LeftRight, 0, Angle, Duration, KeepCenter);
            return LeftRight;
        }
        /// <summary>
        /// turns right
        /// </summary>
        /// <param name="RadPerMilliSecond">speed in rad per millisecond.</param>
        /// <param name="KeepCenter">if is true, the <see cref="Camera.Anchor"/> is fix.</param>
        /// <returns>OglAnimator</returns>
        public OglAnimator LookRight( double RadPerMilliSecond, bool KeepCenter)
        {

            SetAnimatorSpeed(LeftRight, 0, RadPerMilliSecond, KeepCenter);
            return LeftRight;
        }
    }

}
