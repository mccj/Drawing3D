
using System;
using System.Drawing;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
  

namespace Drawing3d
{
    /// <summary>
    /// the centra class, which is enable for the most camera manipulations, like zooming, panning, rotation et. al.
    /// </summary>
    [Serializable]
    public class Camera
    {
        private Matrix OldProjection = Matrix.identity;

        private double SumXAngle = 0;
        private double SumYAngle = 0;
        private xyz XAxis = new xyz();
        private xyz YAxis = new xyz();
        private Matrix _NearFar(double N, double F)
        {
            double left;
            double right;
            double top;
            double bottom;
            double near;
            double far;
            Matrix Result = Matrix.identity;
            double Aspect = (float)Device.ViewPort.Width / (float)Device.ViewPort.Height;
            if (Utils.Equals(Device.FieldOfView, 0))
            {
                xy worldext = Device.WorldExtensions;
                double WorldWidth = worldext.x;
                left = -WorldWidth / 2;
                right = WorldWidth / 2;
                double WorldHeight = worldext.y;
                top = WorldHeight / 2;
                bottom = -WorldHeight / 2;
                near = N;
                far = F;
                Result = Matrix.Orthogonal(left, right, bottom, top, near, far);


            }
            else  //perspective
            {
                near = N;
                far = F;
                left = -near * System.Math.Tan(Device.FieldOfView);
                right = -left;
                top = right / Aspect;
                bottom = -right / Aspect;
                Result = Matrix.Frustum(left * Device.PerspektiveFactor, right * Device.PerspektiveFactor, bottom * Device.PerspektiveFactor, top * Device.PerspektiveFactor, near, far);
            }
            return Result;
        }
        /// <summary>
        /// sets the <see cref="OpenGlDevice.ProjectionMatrix"/> with near clipping and far clipping.
        /// If <see cref="OpenGlDevice.FieldOfView"/> is 0 it is orthagonal els perspectively.
        /// </summary>
        /// <param name="Near">is the near clipping.</param>
        /// <param name="Far">is the far clipping.</param>
        public void NearFar(double Near, double Far)
        {
            Device.ProjectionMatrix = _NearFar(Near, Far) * _NearFar(NearClipping, FarClipping).invert() * Device.ProjectionMatrix;
            NearClipping = Near;
            FarClipping = Far;
        }
        private bool _Consistent = false;
        private bool Consistent
        {
            get { return _Consistent; }
            set { _Consistent = value; }
        }
        private double Zoomf = 1;
        private double getZoomFactor()
        {
            //Matrix PrInv = Device.ProjectionMatrix.invert();
            //double z = Device.ViewPort.Width/Device.PixelsPerUnit/((PrInv * new xyz(1, 0,0)).x - (PrInv * new xyz(-1, 0, 0)).x);
            xyz P1 = Device.ProjectionMatrix.multaffin(new xyz(0, 1, 0));
            xyz P0 = Device.ProjectionMatrix.multaffin(new xyz(0, 0, 0));
            return P1.dist(P0) / DefZoom;



        }
        private double DefZoom = 1;


        private xyz TONorm(Point P)
        {
            

            xyz Pt = new xyz((float)P.X / (float)Device.ViewPort.Width, (float)((float)Device.ViewPort.Height - P.Y) / (float)Device.ViewPort.Height, 0);
            Matrix M = Drawing3d.Matrix.BiasInvert;
            return M * Pt;
 

        }
        private Matrix RelativSystem = Matrix.identity;
        /// <summary>
        /// te current camera base will be setted as base for the <see cref="Angles"/>.
        /// </summary>
        public void SetRelativeSystem()
        {
            Base B = Base;
            B.BaseO = new xyz(0, 0, 0);
            RelativSystem = B.ToMatrix();
        }
        private void LookAt(xyz CameraPosition, xyz CameraLookAt, xyz CameraUpVector, bool KeepZoomfactor)
        {

            Position = CameraPosition;
            Anchor = CameraLookAt;
            UpVector = CameraUpVector;
            Device.FieldOfView = Device.FieldOfView;
            Matrix MM = Matrix.identity;
            if (KeepZoomfactor)
                MM = Matrix.Scale(new xyz(ZoomFactor, ZoomFactor, 1));

            Device.ProjectionMatrix = MM * Device.ProjectionMatrix;

            MakeConsistent();

            setDefZoom();
        }

        private OpenGlDevice _Device = null;

        private xyz _Position = new xyz(0, 0, 30);
        /// <summary>
        /// sets the zoomfactor. The point source goes to the point destination. 
        /// </summary>
        /// <param name="Source">the point, which wil be mapped toDestination</param>
        /// <param name="Destination">the destination</param>
        /// <param name="Factor">the zoom factor.</param>
        public void ZoomTransform(Point Source, Point Destination, double Factor)
        {

            xyz _Source = TONorm(Source);
          
            xyz _Destination = TONorm(Destination);
            Matrix MM = Matrix.Translation(_Destination) * Matrix.Scale(new xyz(Factor, Factor, 1)) * Matrix.Translation(_Source * (-1));
            //LeftOperator = MM * LeftOperator;
            Device.ProjectionMatrix = MM * Device.ProjectionMatrix;
         
            if (Device.ForegroundDrawEnable)
                Device.OutFitChanged = true;
            Device.Camera.MakeConsistent();
            Zoomf = Zoomf * Factor;

        }
        /// <summary>
        /// sets and gets the position of the camera.
        /// </summary>
        public xyz Position
        {
            get { return _Position; }
            set
            {
                MakeConsistent();
                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Translation(Position - value);
                _Position = value;
                MakeConsistent();
            }
        }
        /// <summary>
        /// the base of the camera, where basey is the <see cref="UpVector"/> and basez looks to the scene.
        /// </summary>
        public Base Base
        {
            get
            {

                Base B = new Base();


                B.BaseO = Position;
                B.BaseZ = Direction * (-1);
                B.BaseY = UpVector;
                B.BaseX = B.BaseY & B.BaseZ;

                return B;
            }
            set
            {

                _Position = value.BaseO;
                _Direction = value.BaseZ * (-1);
                _UpVector = value.BaseY;

            }

        }

        /// <summary>
        /// gets and set the field of view. A value from 0 gwts a orthogonal scene.
        /// </summary>
        public double FieldOfView
        {
            get { return Device.FieldOfView; }
            set { Device.FieldOfView = value; }
        }
        /// <summary>
        /// internal
        /// </summary>
        internal void setDefZoom()
        {
            xyz P1 = Device.ProjectionMatrix.multaffin(new xyz(0, 1, 0));
            xyz P0 = Device.ProjectionMatrix.multaffin(new xyz(0, 0, 0));
            DefZoom = P1.dist(P0);
            //Matrix PrInv = Device.ProjectionMatrix.invert();
            //DefZoom = ((PrInv * new xyz(1, 0, -1)).x - (PrInv * new xyz(-1, 0, -1)).x) ;


        }

        /// <summary>
        /// gets and sets the roll angle.See <see cref="Yaw"/> and <see cref="Pitch"/>.
        /// </summary>
        public double Roll
        {
            get { return Angles.z; }
            set
            {
                xyz A = Angles;
                Angles = new xyz(A.x, A.y, value);

            }
        }


        /// <summary>
        /// gets and sets the pitch angle.See <see cref="Yaw"/> and <see cref="Roll"/>.
        /// </summary>
        public double Pitch
        {
            get { return Angles.y; }
            set
            {
                xyz A = Angles;
                Angles = new xyz(A.x, value, A.z);
            }
        }
        /// <summary>
        /// gets and sets the yaw angle.See  <see cref="Pitch"/> and <see cref="Roll"/>.
        /// </summary>
        public double Yaw
        {
            get { return Angles.x; }
            set
            {
                xyz A = Angles;
                Angles = new xyz(value, A.y, A.z);
            }
        }
        /// <summary>
        /// Sets the zoom factor with a fix point.
        /// </summary>
        /// <param name="FixPoint">fix point</param>
        /// <param name="Factor">zoom factor</param>
        public void Zoom(Point FixPoint, double Factor)
        {
            if (Device.Navigating)
            {
                ZoomTransform(FixPoint, FixPoint, Factor);
                if (Device.RefreshMode == OpenGlDevice.Mode.WhenNeeded)
                    Device.Refresh();
            }

        }
        /// <summary>
        /// sets and gets the zoom factor.
        /// </summary>
        public double ZoomFactor
        {
            get { return getZoomFactor(); }
            set { ZoomTransform(new Point((int)(Device.ViewPort.Width / 2), (int)(Device.ViewPort.Y / 2)), new Point((int)(Device.ViewPort.Width / 2), (int)(Device.ViewPort.Y / 2)), value); }
        }
        /// <summary>
        /// translate the camera by the vector delta.
        /// </summary>
        /// <param name="Delta">the vector delta.</param>
        public void Translate(xy Delta)
        {

            Device.ProjectionMatrix = Matrix.Translation((float)Delta.x * 2f / (float)Device.ViewPort.Width, (float)Delta.y * 2f / (float)Device.ViewPort.Height, 0) * Device.ProjectionMatrix;
            MakeConsistent();
        }

        /// <summary>
        /// sets and gets the angles, wich defines the camera (Euer angles). It is
        /// calculated. See also <see cref="RelativSystem"/>, <see cref="Roll"/>, <see cref="Pitch"/> and <see cref="Yaw"/>, which uses also the angles.
        /// </summary>
        public xyz Angles
        {
            get
            {
                Device.Camera.MakeConsistent();
                return Matrix.toEuler(Base.ToMatrix() * RelativSystem);

            }
            set
            {

                Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Translation(Anchor) * (Matrix.FromEuler(Angles) * Matrix.FromEuler(value).invert()) * Matrix.Translation(Anchor * (-1));
                Device.Camera.MakeConsistent();
               

            }
        }

        private xyz _Direction = new xyz(0, 0, -1);
        /// <summary>
        /// gets and sets the direction, in which the camera points.
        /// </summary>
        public xyz Direction
        {
            get { return _Direction.normalized(); }
            set
            {
                _Direction = value.normalized();

            }
        }
        private xyz _UpVector = new xyz(0, 1, 0);
        /// <summary>
        /// gets and sets the upvector of the camera.
        /// </summary>
        public xyz UpVector
        {
            get { return _UpVector; }
            set
            {
                _UpVector = value.normalized();

            }
        }
        private double _NearClipping = 1;
        private double _FarClipping = 1000;
        /// <summary>
        /// sets or gets the nearclipping. It is the distance of the camera, where the scene is clipped.
        /// The value must be greater then 0.
        /// </summary>
        public double NearClipping
        {
            get
            {
                return _NearClipping;

            }
            set
            {

                _NearClipping = value;


            }
        }

        /// <summary>
        /// sets or gets the farclipping. It is the distance of the camera, where the scene is clipped.
        /// </summary>
        public double FarClipping
        {
            get
            {
                return _FarClipping;
            }
            set
            {
                _FarClipping = value;
            }
        }
        /// <summary>
        /// sets the <see cref="Position"/> the <see cref="UpVector"/> and the point where the camera look to.
        /// </summary>
        /// <param name="CameraPosition">position.</param>
        /// <param name="CameraLookAt">look at point.</param>
        /// <param name="CameraUpVector">up vector.</param>
        public void LookAt(xyz CameraPosition, xyz CameraLookAt, xyz CameraUpVector)
        {
            LookAt(CameraPosition, CameraLookAt, CameraUpVector, false);
        }

        /// <summary>
        /// internal
        /// </summary>
        internal void Navigate(double Alfa, double Beta)
        {
            if (!Device.Navigating) return;
            SumXAngle += Alfa;
            SumYAngle += Beta;
            Matrix Rot1 = Matrix.Rotation(new LineType(new xyz(0, 0, 0), YAxis), -SumYAngle);
            Matrix Rot2 = Matrix.Rotation(new LineType(new xyz(0, 0, 0), XAxis), SumXAngle);
            Rot1 = Matrix.Translation(Anchor) * Rot2 * Rot1 * Matrix.Translation(Anchor * (-1));
            Device.ProjectionMatrix = OldProjection * Rot1;
       
            MakeConsistent();
            if (Device.RefreshMode == OpenGlDevice.Mode.WhenNeeded)

                Device.Refresh();
        }
        /// <summary>
        /// internal
        /// </summary>
        internal void MakeConsistent()
        {

            Matrix P = ((Device.ProjectionMatrix)).invert();
            _Direction = ((P * new xyz(0, 0, 1) - P * new xyz(0, 0, -1))).normalized();
            _Position = (P * new xyz(0, 0, -1) + _Direction * (-NearClipping));
            _UpVector = ((P * new xyz(0, 1, -1) - P * new xyz(0, 0, -1))).normalized();


        }
        /// <summary>
        /// gets and sets the point, which is fix, you use keepcenter=true.See <see cref="LookDown(float, bool)"/> and <see cref="LookRight(float, bool)"/>.
        /// </summary>
        public xyz Anchor = new xyz(0, 0, 0);


        /// <summary>
        /// starts a rotation of the scene. It is call by the device when the mouse button is pressed.
        /// </summary>
        public void BeginRotation()
        {

            //Device.StartNavigate = Device.MousePos;
            OldProjection = Device.ProjectionMatrix;
     

            XAxis = Base.BaseX;
            YAxis = Base.BaseY;
            SumXAngle = 0;
            SumYAngle = 0;
        }
        /// <summary>
        /// gets the device in which the camera works.
        /// </summary>
        public OpenGlDevice Device
        {
            get { return _Device; }
        }
        OglAnimator AnimatedWalkForward(double SpeedInUnitsPerMilliSec)
        { return Animated.WalkForward(SpeedInUnitsPerMilliSec); }
        /// <summary>
        /// gets the animated camera. See <see cref="AnimatedCamera"/>.
        /// </summary>
        public AnimatedCamera Animated = null;
        /// <summary>
        /// constructor with a device.
        /// </summary>
        /// <param name="Device">the device in which the camera works.</param>
        public Camera(OpenGlDevice Device)
        {
            _Device = Device;
            Animated = new AnimatedCamera(this);

        }
        /// <summary>
        /// rotates the camera about the angle down. If KeepCenter = true the <see cref="Anchor"/> is fix.
        /// </summary>
        /// <param name="Angle">the angle for the rotation.</param>
        /// <param name="KeepCenter">fixes the <see cref="Anchor"/></param>
        public void LookDown(float Angle, bool KeepCenter)
        {

            LineType L = new LineType();
            if (KeepCenter)
                L = new LineType(Anchor, Base.BaseX);
            else
                L = new LineType(Position, Base.BaseX);


            Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
            Device.Refresh();


        }
        /// <summary>
        /// rotates the camera about the angle right. If KeepCenter = true the <see cref="Anchor"/> is fix.
        /// </summary>
        /// <param name="Angle">the angle for the rotation.</param>
        /// <param name="KeepCenter">fixes the <see cref="Anchor"/></param>
        public void LookRight(float Angle, bool KeepCenter)
        {

            LineType L = new LineType();
            if (KeepCenter)
                L = new LineType(Anchor, Base.BaseY);
            else
                L = new LineType(Position, Base.BaseY);
            Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
            Device.Refresh();
        }
        /// <summary>
        /// roll motion. This means a rotation around the <see cref="Direction"/> with the angle.
        /// </summary>
        /// <param name="Angle">the angle for the rotation.</param>
        public void RollRight(float Angle)
        {
            LineType L = new LineType(Position, Base.BaseZ);
            Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Rotation(L, Angle);
            Device.Refresh();
            MakeConsistent();
        }

        /// <summary>
        /// gets and sets the panning.It gives the screen coordinates of the world point (0,0,0).
        /// </summary>
        public Point Panning
        {
            get
            {
                xyz B = Device.ProjectionMatrix * new xyz(0, 0, 0);
                Point Result = new Point((int)(Device.ViewPort.Width / 2 + B.x * Device.ViewPort.Width / 2), (int)(Device.ViewPort.Height / 2 - B.y * Device.ViewPort.Height / 2));
                return Result;
            }
            set
            {

                Point P = Panning;
                xy Delta = new xy(value.X - P.X, value.Y - P.Y);
                Device.ProjectionMatrix = Matrix.Translation((float)Delta.x * 2f / (float)Device.ViewPort.Width, (float)Delta.y * 2f / (float)Device.ViewPort.Height, 0) * Device.ProjectionMatrix;

            }
        }
        /// <summary>
        /// the camera will be moved to right.
        /// </summary>
        /// <param name="Width">the distance for the movement.</param>
        public void GoRight(float Width)
        {



            Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Translation(Base.BaseX * Width);
            Device.Refresh();
            MakeConsistent();

        }
        /// <summary>
        /// the camera will be moved to forward.
        /// </summary>
        /// <param name="Width">the distance for the movement.</param>
        public void WalkForward(float Width)
        {
            Device.ProjectionMatrix = Device.ProjectionMatrix * Matrix.Translation(Base.BaseZ * Width);
            Device.Refresh();
            MakeConsistent();
        }
    }

}

