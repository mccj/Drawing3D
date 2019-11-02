
using System;


//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

using System.Drawing;
namespace Drawing3d
{


    /// <summary>
    /// Defines a rectangle structure like <see cref="Rectangle"/> but the fields are all of type double.
    /// </summary>
    [Serializable]
    public struct Rectangled
    {
       
        private  double _Left;
        private double _Bottom;
        private double _Width;
        private double _Height;
        /// <summary>
        /// Constructor of rectangled
        /// </summary>
        /// <param name="Left">the left coordinate of the rectangle</param>
        /// <param name="Bottom">the bottom coordinate of the rectangle</param>
        /// <param name="Width">the width of the left</param>
        /// <param name="Height">the Height of the rectangle</param>
        public Rectangled(double Left, double Bottom, double Width, double Height)
        {
            _Left = Left;
            _Bottom = Bottom;
            _Width = Width;
            _Height = Height;
            this.Left = Left;
            this.Bottom = Bottom;
            this.Width = Width;
            this.Height = Height;
        }
        /// <summary>
        /// the left coordinate of the rectangle
        /// </summary>
        public double Left { get { return _Left; } set { _Left = value; } }
        /// <summary>
        /// Gets the right coordinate of the rectangle
        /// </summary>
        public double Right { get { return _Left + _Width; } }
        /// <summary>
        /// Gets the top coordinate of the rectangle
        /// </summary>
        public double Top { get { return _Bottom + _Height; } }
        /// <summary>
        /// Gets or sets the bottom coordinate of the rectangle
        /// </summary>
        public double Bottom { get { return _Bottom; } set { _Bottom = value; } }
        /// <summary>
        /// Gets or sets the width of the rectangle
        /// </summary>
        public double Width { get { return _Width; } set { _Width = value; } }
        /// <summary>
        /// Gets or sets the height of the rectangle
        /// </summary>
        public double Height { get { return _Height; } set { _Height = value; } }
        /// <summary>
        /// just the same as <see cref="Left"/>
        /// </summary>
        public double X { get { return _Left; } set { _Left = value; } }
        /// <summary>
        /// just the same as <see cref="Bottom"/>
        /// </summary>
        public double Y { get { return _Bottom; } set { _Bottom = value; } }
    }
}