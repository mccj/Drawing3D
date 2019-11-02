using System;
using System.ComponentModel;
using System.Drawing;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{/// <summary>
/// Holds the red, green, blue, alpha structure of a color
/// </summary>
    [Serializable]
    public struct RGBA
    {/// <summary>
     /// holds the RGBA values in Array[0],Array[1],Array[2],Array[3].
     /// </summary>
        public float[] Array;
        /// <summary>
        /// construcor with RGBA-values
        /// </summary>
        public RGBA(double R, double G, double B, double A)
        {
            Array = new float[4];
            this.R = (float)R;
            this.G = (float)G;
            this.B = (float)B;
            this.A = (float)A;

        }
        /// <summary>
        /// red part 0&lt;= value &lt;=1
        /// </summary>
        public float R
        {
            get { return Array[0]; }
            set { Array[0] = value; }

        }
        /// <summary>
        /// green part 0&lt;= value &lt;=1
        /// </summary>

        public float G
        {
            get { return Array[1]; }
            set { Array[1] = value; }

        }
        /// <summary>
        /// blue part 0&lt;= value &lt;=1
        /// </summary>

        public float B
        {
            get { return Array[2]; }
            set { Array[2] = value; }

        }
        /// <summary>
        /// A part 0&lt;= value &lt;=1
        /// </summary>

        public float A
        {
            get { return Array[3]; }
            set { Array[3] = value; }

        }
        /// <summary>
        /// Calculates a normalized color RGBA from a <see cref="System.Drawing.Color"/> by dividing the different parts by 255.
        /// </summary>
        /// <param name="c">Color, which will be converted</param>
        /// <returns>Normalized color</returns>

        public static RGBA NormalColor(Color c)
        {


            return new RGBA(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
           
        }
    }
}