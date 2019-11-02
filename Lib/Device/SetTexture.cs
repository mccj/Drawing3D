
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        static int GL_CLAMP_TO_BORDER = 33069;
        static int GL_NEAREST = 9728;
        Matrix _TextureMatrix = Matrix.identity;
        static int GL_REPEAT = 10497;
        int GL_MIRRORED_REPEAT = 33648;
        
        private Matrix GetTextureMatrix()
        {
            return _TextureMatrix;
        }
        /// <summary>
        /// this matrix is applied to the current texture.
        /// </summary>
        public Matrix TextureMatrix
        {
            get { return GetTextureMatrix(); }
            set { SetTextureMatrix(value); }
        }

        private Bitmap ConvertToRBA(Bitmap b, Color TransParentColor)
        {  
            Bitmap Result = new Bitmap(b.Width, b.Height);
            BitmapData SourceData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData DestData = Result.LockBits(new Rectangle(0, 0, Result.Width, Result.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            int SourceStride = SourceData.Stride;
            int DestStride = DestData.Stride;
            int H = b.Height;
            int W = b.Width;
            int BytesPerPixel = (int)(SourceStride / b.Width);

            unsafe
            {
                byte* DestPtr = (byte*)(void*)DestData.Scan0;
                byte* SourcePtr = (byte*)(void*)SourceData.Scan0;
                int Ydisp = 0;
                int ydispDest = 0;
                for (int y = 0; y < H; y++)
                {
                    for (int x = 0; x < W; x++)
                    {
                        DestPtr[x * 4 + 0 + ydispDest] = SourcePtr[x * 4 + 2 + Ydisp];
                        DestPtr[x * 4 + 1 + ydispDest] = SourcePtr[x * 4 + 1 + Ydisp];
                        DestPtr[x * 4 + 2 + ydispDest] = SourcePtr[x * 4 + 0 + Ydisp];



                        if (Color.FromArgb(DestPtr[x * 4 + 0 + ydispDest], DestPtr[x * 4 + 1 + ydispDest], DestPtr[x * 4 + 2 + ydispDest])
                            ==
                            Color.FromArgb(255, TransParentColor))
                            DestPtr[x * 4 + 3 + ydispDest] = 0;
                        else
                            DestPtr[x * 4 + 3 + ydispDest] = SourcePtr[x * 4 + 3 + Ydisp];

                    }
                    Ydisp = Ydisp + SourceStride;
                    ydispDest = ydispDest + DestStride;

                }
            }
            Result.UnlockBits(DestData);
            b.UnlockBits(SourceData);

            return Result;
        }
        private List<Image> Images = new List<Image>();
        private List<int> Textures = new List<int>();
        /// <summary>
        /// disoses all textures and images.
        /// </summary>
        public void DisposeTextures()
        {
            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].Dispose();
            }
            Images = new List<Image>();
            for (int i = 0; i < Textures.Count; i++) // kann entfernt werden, wird von OpenGl entfernt, liefert immer eine exception
            {
                try
                {
                    GL.DeleteTexture(Textures[i]);
                }
                catch (System.Exception)
                {


                }

            }
            Textures = new List<int>();
            System.GC.Collect();
        }
      void RegisterTexture(Texture value)
        {
            if (value.Handle == 0)
            {
              //  GL.ActiveTexture(value.TextureUnit);
                int h;
                GL.GenTextures(1, out h);
                Textures.Add(h);
                value.Handle = h;
                Bitmap image = null;
                image = value.Bitmap;
                if (image != null)
                {
                    Images.Add(image);
                    if ((value.HasTransparentColor))
                    { image = ConvertToRBA(image, value.TransparentColor); }
                    Bitmap I = image;
                   
                    xyz A = ProjectionMatrix * new xyz(0, 1, 0);
                    xyz B = ProjectionMatrix * new xyz(0, 0, 0);
                    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                    BitmapData bitmapdata = null;
                    bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                  
                    GL.BindTexture(TextureTarget.Texture2D, value.Handle);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)4, image.Width, image.Height,
                        0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapdata.Scan0);
                    image.UnlockBits(bitmapdata);

                    if (image is System.Drawing.Bitmap)
                    {
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    }
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, value.Handle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, GL_NEAREST);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, GL_NEAREST);
            if (value.XRepeating)
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GL_REPEAT);
            else
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GL_CLAMP_TO_BORDER);


            if (value.YRepeating)
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GL_REPEAT);
            else
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GL_CLAMP_TO_BORDER);
            if (value.Mirrored)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GL_MIRRORED_REPEAT);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GL_MIRRORED_REPEAT);
            }
            GL.ActiveTexture(TextureUnit.Texture0);
        }
        private void _SetTexture(OpenGlDevice Device, object data)
        {
            Device.texture = (Texture)data;
        }
        private Texture _texture = null;

        private void setTexture(Texture value)
        {  
            int Active = GL.GetInteger(GetPName.TextureBinding2D);
           if (RenderKind == Drawing3d.RenderKind.SnapBuffer) return;

            //if (value == _texture)
            //     return;

            if (Entity.Compiling)
            {
                if (MeshCreator.HasTexture) MeshCreator.Renew();
                MeshCreator.Texture= value;

            }
            _texture = value;

            if ((value == null))
            {

                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Texture2D);
                if (Shader != null)
                {
                    Field C = Shader.getvar("Texture0Enabled");
                    if (C != null)
                            C.SetValue(0);
                    C = Shader.getvar("Texture1Enabled");
                    if (C != null)
                        C.SetValue(0);
                }

                return;
            }
            if ((value.Handle == 0) && (value.Bitmap == null))
                return;
            if (value.Handle == 0)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + value.Index);
                RegisterTexture(value);
                GL.ActiveTexture(TextureUnit.Texture0);
            }
            if ((Shader != null) && (Shader.Using))
               value.SetAttributes(Shader);
        }

        private void SetTextureMatrix(Matrix value)
        {
            _TextureMatrix = value;
            if (Shader != null)
            {
                Field F = Shader.getvar("Texture0Matrix");
                if (F != null)
                    F.Update();
            }
        }
    }
}
