

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

using System;
using System.Drawing;
namespace Drawing3d
{
    /// <summary>
    /// gives a visual image of a <see cref="Base"/>.
    /// </summary>
    public class VisualBase : Entity
    {
        /// <summary>
        /// the xAxis. See also <see cref="Arrow"/>.
        /// </summary>
        public Arrow xAxis = new Arrow();
        /// <summary>
        /// the yAxis. See also <see cref="Arrow"/>.
        /// </summary>
        public Arrow yAxis = new Arrow();
        /// <summary>
        /// the zAxis. See also <see cref="Arrow"/>.
        /// </summary>
        public Arrow zAxis = new Arrow();
        /// <summary>
        /// gets and sets the <b>size</b> of the <b>base</b>.
        /// </summary>
        public double Size
        {
            get { return xAxis.Size; }
            set
            {
                xAxis.Size = value;
                yAxis.Size = value;
                zAxis.Size = value;


            }
        }
        /// <summary>
        /// gets and sets the thicjnes of the shafts of the arrows.
        /// </summary>
        public double Thickness
        {
            get { return xAxis.Radius * 2; }
            set
            {
                xAxis.Radius = value / 2;
                yAxis.Radius = value / 2;
                zAxis.Radius = value / 2;


            }
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public VisualBase()
        {
            xAxis.SetShaftAndTop(new xyz(0, 0, 0), new xyz(5, 0, 0));
            yAxis.SetShaftAndTop(new xyz(0, 0, 0), new xyz(0, 5, 0));
            zAxis.SetShaftAndTop(new xyz(0, 0, 0), new xyz(0, 0, 5));
            CompileEnable = false;
        }
        /// <summary>
        /// override <see cref="CustomEntity.Draw"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            Color Save = Device.Diffuse;
       
            xAxis.Paint(Device);
      
            yAxis.Paint(Device);
      
            zAxis.Paint(Device);
            Device.Diffuse = Save;
            base.OnDraw(Device);
        }
    }
   
    
}

