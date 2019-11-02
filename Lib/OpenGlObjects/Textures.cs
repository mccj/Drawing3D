using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// Texture encapsulates the properties, which are needed for drawing.
    /// The most important property is <see cref="Bitmap"/>, which holds the bitmadata.
    /// 
    /// </summary>
       
    [Serializable]
    public class Texture 
    {
        OGLShader _Shader = null;
        bool _NormalMap = false;
        /// <summary>
        /// to activate a <b>normalmap</b> set this value <b>true</b>.
        /// </summary>

        public bool NormalMap
        {
            get { return _NormalMap; }
            set
            {
                _NormalMap = value;
                if (value) Index = 1;
            }
        }
      
        bool _Tesselation = false;
        /// <summary>
        /// to activate a <b>Tessalation</b> set this value <b>true</b>.
        /// </summary>

        public bool Tesselation
        {
            get { return _Tesselation; }
            set
            {
                _Tesselation = value;
                if (value) Index = 2;
            }
        }
       
        /// <summary>
        /// hide the texture.
        /// </summary>
        internal void Hide()
        {
            Field C = null;
            if (Index == 0)
            {   if (_Shader!= null)
                C = _Shader.getvar("Texture0Enabled");
                if (C != null)
                    C.SetValue(0);
            }
            else
            {
                if (_Shader != null)
                    C = _Shader.getvar("Texture1Enabled");
                if (C != null)
                    C.SetValue(0);
            }
        }
        /// <summary>
        /// is called from <see cref="OpenGlDevice.setTexture(Texture)"/> to set the values to the current <see cref="OpenGlDevice.Shader"/>.
        /// </summary>
        /// <param name="Shader">the current <see cref="OpenGlDevice.Shader"/>.</param>
        public void SetAttributes(GLShader Shader)
        {

            Matrix M = Matrix.Scale(new xyz(1 / WorldWidth, -1 / WorldHeight, 1 / WorldWidth));
            Matrix TextureMatrix = Matrix.identity;
            if (SoBigAsPossible != Texture.BigAsPosible.None)
                TextureMatrix = Matrix.Scale(new xyz(1, -1, 1));
            else
                TextureMatrix = M * Matrix.Translation(new xyz(-Translation.x, -Translation.y, 0)) * Matrix.Rotation(new xyz(0, 0, 1), -Rotation);

            GL.ActiveTexture(TextureUnit.Texture0+Index);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            OpenGlDevice.CheckError();
            Field T = Shader.getvar("Texture"+Index.ToString()+"Enabled");
            if (T != null)
            {
               
                T.SetValue(1);
            }

            T = Shader.getvar("Texture" + Index.ToString()+"_");
            if (T != null) T.SetValue(Index);
            T = Shader.getvar("Texture" + Index.ToString()+"Matrix");
            if (T != null) T.SetMatrix(TextureMatrix);
            if (NormalMap)
            {
                T = Shader.getvar("NormalMap");
                if (T != null)
                    T.SetValue(true);
            }
            if (Tesselation)
            {
                T = Shader.getvar("Tesselation");
                if (T != null)
                    T.SetValue(true);
            }

           

        }
        int _index = 0;
        /// <summary>
        /// gets and sets an index.
        /// The Texture will be generated with ActiveTexture Texture0+Index.
        /// You can use "uniform sampler2D Texture<i>Index</i>_ in your shader.
        /// A field bool Texture<i>Index</i>+"Enabled willbe setted true.
        /// The number 1 is reserved for normal mapping, the number 2 for tesselation.
        /// From the program you must use <see cref="OpenGlDevice.texture"/>.
        /// </summary>
        public int Index
        { get { return _index; }
          set { _index = value;
                 
               }
        }
       
        private double _Rotation;
        /// <summary>
        /// gets or sets an angle for the rotation of the texture
        /// </summary>
     
        public double Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }

        private xy _Translation;
        /// <summary>
        /// sets or gets a translation vector for the texture
        /// </summary>
        
        public xy Translation
        {
            get { return _Translation; }
            set { _Translation = value; }
        }
        /// <summary>
        /// gets or sets a name.
        /// </summary>
        public string TextureName = "";
        /// <summary>
        /// is an enumerator, which is used from <see cref="SoBigAsPossible"/>.
        /// </summary>
        public enum BigAsPosible
        {
            /// <summary>
            /// nor enlargement of the <see cref="Texture"/> will be made.
            /// </summary>
            None,
            /// <summary>
            /// the enlargement of the <see cref="Texture"/> in the wight.
            /// </summary>
            Width,
            /// <summary>
            /// the enlargement of the <see cref="Texture"/> in the Height.
            /// </summary>
            Height
        }
        /// <summary>
        /// gets and sets the <see cref="BigAsPosible"/>.
        /// </summary>
        public BigAsPosible SoBigAsPossible = BigAsPosible.None;
        /// <summary>
        /// if this is <b>true</b> the aspect of the <see cref="Texture"/> will be keeped. 
        /// </summary>
        public bool KeepAspect = false;
        private Bitmap FBitmap;
        bool _HasTransparentColor = false;
        /// <summary>
        /// To activate <see cref="TransparentColor"/> this field must be true;
        /// </summary>
        public bool HasTransparentColor
        {
            get { return _HasTransparentColor; }
            set { _HasTransparentColor = value; }
        }
        private void SetBitmap(Bitmap value)
        {
            if (FBitmap != null) { FBitmap.Dispose(); };

            FBitmap = value;
        }
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public Texture()
        {
        }
        /// <summary>
        /// is a constructor with the filename of the image.
        /// </summary>
        /// <param name="FileName">filename of the image</param>
        public Texture(string FileName):this()
        {
            LoadFromFile(FileName);
        }
        /// <summary>
        /// Sets the Bitmap and the <see cref="TransparentColor"/>
        /// </summary>
        /// <param name="value">A bitmap</param>
        /// <param name="TransparentColor">The transparentcolor</param>
        public void SetBitmap(Bitmap value, Color TransparentColor)
        {
            FBitmap = value;
            this.TransparentColor = TransparentColor;
            HasTransparentColor = true;
        }
      
     
        /// <summary>
        /// Set and returns the Bitmapdata, which will be drawn as texture
        /// </summary>
      
        public Bitmap Bitmap { get { return FBitmap; } set { SetBitmap(value); } }
        /// <summary>
        /// This handle will be used by OpenGldevice to bind a texture.
        /// </summary>
        public int Handle = 0;
        double _WorldWidth = 1;
        /// <summary>
        /// With WorldWidth you can fix the with of a texture in world coordinates.
        /// </summary>
       public double WorldWidth
        {
            get { return _WorldWidth; }
            set { _WorldWidth = value; }
        }
        double _WorldHeight = 1;
        /// <summary>
        /// With WorldHeight you can fix the with of a texture in world coordinates.
        /// </summary>
     
        public double WorldHeight
        {
            get { return _WorldHeight; }
            set { _WorldHeight = value; }
        }


        /// <summary>
        /// This matrix can be used to transform the texture.
        /// </summary>
        public Matrix Transformation = new Matrix(1);
        /// <summary>
        /// Loads a Bitmap from a file.
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadFromFile(string FileName)
        {
            try
            {

            
            {if ((System.IO.Path.GetExtension(FileName)).ToUpper()==".TGA")
            {
                TargaImage TI = new TargaImage(FileName);
                Bitmap = TI.Image;
                Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                 Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            else

                Bitmap = (Bitmap)System.Drawing.Bitmap.FromFile(FileName);
            }
            }
            catch (Exception E)
            {

                System.Windows.Forms.MessageBox.Show(E.Message);
            }
        }

        bool fMirrored = false;
        /// <summary>
        /// If mirrored is true every repeated bitmap is mirrored.
        /// <seealso cref="XRepeating"/><seealso cref="YRepeating"/>
        /// </summary>
   
        public bool Mirrored
        {
            get { return fMirrored; }
            set { fMirrored = value; }
        }
       
    
        private Color _TransparentColor = Color.Black;

        /// <summary>
        /// Sets or gets the transparentcolor for the texture.
        /// <seealso cref="HasTransparentColor"/>. Remark: the texture should not be a jpg-file.
        /// </summary>
 
        public Color TransparentColor
        {
            get { return _TransparentColor; }
            set { HasTransparentColor = true;
                _TransparentColor = value; }
        }
        private bool _XRepeating = true;
        /// <summary>
        /// Indicates, whether the bitmap is repeated on the x-direction
        /// </summary>

        public bool XRepeating
        {
            get { return _XRepeating; }
            set { _XRepeating = value; }
        }

        private bool _YRepeating = true;
        /// <summary>
        /// Indicates, whether the bitmap is repeated on the x-direction
        /// </summary>
   
        public bool YRepeating
        {
            get { return _YRepeating; }
            set { _YRepeating = value; }
        }
    
    }
}