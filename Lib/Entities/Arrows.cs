//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

using System;
using System.Drawing;
namespace Drawing3d
{
   
    /// <summary>
    /// is a class, how draws an arrow
    /// </summary>
    [Serializable]
    public  class Arrow:Entity
    {
        /// <summary>
        /// sets the position of the shaft and of the top.
        /// </summary>
        /// <param name="Shaft">the shaft</param>
        /// <param name="Top">the top</param>
        public void SetShaftAndTop(xyz Shaft,xyz Top)
        {
            Base B = Base.DoComplete(Shaft, Top - Shaft);
            this._Transformation = B.ToMatrix();
            Size = Top.dist(Shaft);
         

        }
        double _TopRadius = 1;
        double _TopHeight = 2;
        
        
        /// <summary>
        /// gets and sets radius of the topcone. Default is 1.
        /// </summary>
        public double TopRadius
        { get { return _TopRadius; }
          set { _TopRadius = value;
          
              }

        }
        /// <summary>
        /// gets and sets height of the topcone. Default is 2.
        /// </summary>
        public double TopHeight
        { get { return _TopHeight; }
          set { _TopHeight = value;
         
               }

        }
        Material TopMaterial = Materials.Chrome;
        Material ShaftMaterial = Materials.Chrome;
        /// <summary>
        /// gets and sets the color of the topcone. Default is blue.
        /// </summary>
        public Color TopColor
        {
            get { return TopMaterial.Emission; }
            set
            {
                TopMaterial = new Material("", Materials.Chrome.Ambient, Materials.Chrome.Diffuse, Materials.Chrome.Specular, value, Materials.Chrome.Shininess, 1);
               
            
            }

        }
        /// <summary>
        /// is an empty constructor
        /// </summary>
        public Arrow():base()
        {
            ShaftColor = Color.Green;
            TopColor = Color.Blue;
        }
        /// <summary>
        /// is a constructor with a Parent.
        /// </summary>
        public Arrow(Entity Parent) : base(Parent)
        {
            ShaftColor = Color.Green;
            TopColor = Color.Blue;
        }
        /// <summary>
        /// gets and sets the color of the shaft. Default is green.
        /// </summary>
        public Color ShaftColor
        {
            get { return ShaftMaterial.Emission; }
            set
            {
               
                ShaftMaterial = new Material("", Materials.Chrome.Ambient, Materials.Chrome.Diffuse, Materials.Chrome.Specular, value, Materials.Chrome.Shininess, 1);
            }

        }
        double _Radius = 0.5;
        /// <summary>
        /// gets and sets radius of the shaft. Default is 0,5.
        /// </summary>
        public double Radius
        {
            get { return _Radius; }
            set {
                _Radius = value;
            }

        }
        double _Size = 5;
        /// <summary>
        ///  gets and sets the size of the arrow. Default is 5.
        /// </summary>
        public double Size
        {
            get { return _Size; }
            set { _Size = value;
                }

        }
        Matrix _Transformation = Matrix.identity;
        /// <summary>
        /// gets the top of the arrow. See also <see cref="SetShaftAndTop(xyz, xyz)"/>
        /// </summary>
        public xyz Top
        { get { return _Transformation * new xyz(0, 0, Size); } }
        /// <summary>
        /// gets the top of the arrow. See also <see cref="SetShaftAndTop(xyz, xyz)"/>
        /// </summary>
        public xyz Shaft
        { get { return _Transformation * new xyz(0, 0, 0); } }
        /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/>method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(_Transformation);
            if (Device.RenderKind != RenderKind.SnapBuffer)
            {
                Material SaveE = Device.Material;
                Device.Material = ShaftMaterial;

                Arc K = new Arc(new xy(0, 0), Radius);
                K.ClockWise = false;
                Device.drawCurve(K);
                Device.drawCylinder(Radius, Size- TopHeight);
                Device.PushMatrix();

                Device.MulMatrix(Matrix.Translation(0, 0, Size-TopHeight));
                Device.Material = TopMaterial;
                Device.drawConePointed(TopRadius, TopHeight);
                K = new Arc(new xy(0, 0), TopRadius);
                K.ClockWise = false;
                Device.drawCurve(K);
                Device.PopMatrix();
                Device.Material = SaveE;
            }
           else
            {
                Device.PushTag(0);
                Device.drawLine(new xyz(0, 0, 0), new xyz(0, 0, Size));
                Device.PopTag();
                Device.PushTag(1);
                Device.drawPoint(new xyz(0, 0, 0),0.3);
                Device.PopTag();
                Device.PushTag(2);
                Device.drawPoint(new xyz(0, 0, Size),0.3);
                Device.PopTag();

            }
            Device.PopMatrix();
            base.OnDraw(Device);
        }
    }
}