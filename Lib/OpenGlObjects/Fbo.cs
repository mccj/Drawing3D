using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.

#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{

   /// <summary>
   /// a FBO, which is used to draw in the memory.
   /// </summary>
    public partial class FBO : IDisposable
    {
        delegate void TGenRenderbuffers(int Count, out int Handle);
        delegate void TBindRenderbuffer(RenderbufferTarget FT, int Handle);
        delegate void TRenderbufferStorage(RenderbufferTarget FT, RenderbufferStorage RS, int Width, int Height);
        delegate void TDeleteRenderbuffers(int Count, ref int Handle);
        delegate void TFramebufferRenderbuffer(FramebufferTarget FT, FramebufferAttachment DA, RenderbufferTarget RT, int depthbuffer);
        delegate void TGenFrameBuffers(int Count, out int Handle);
        delegate void TDeleteFrameBuffers(int Count, ref int Handle);
        delegate void TBindFrameBuffer(FramebufferTarget FT, int Handle);
        delegate FramebufferErrorCode TCheckFramebufferStatus(FramebufferTarget FT);
        delegate void TFramebufferTexture2D(FramebufferTarget FT, FramebufferAttachment A, TextureTarget TT, int F, int O);
        static TGenFrameBuffers _GenFramebuffers;
        static TDeleteFrameBuffers _DeleteFramebuffers;
        static TBindFrameBuffer _BindFramebuffer;
        static TCheckFramebufferStatus _CheckFramebufferStatus;
        static TFramebufferTexture2D _FramebufferTexture2D;
        static TFramebufferRenderbuffer _FramebufferRenderbuffer;
        static TGenRenderbuffers _GenRenderbuffers;
        static TBindRenderbuffer _BindRenderbuffer;
        static TRenderbufferStorage _RenderbufferStorage;
        static TDeleteRenderbuffers _DeleteRenderbuffers;
        /// <summary>
        /// loads the needed methods.
        /// </summary>
        internal static void LoadMethods() 
        {
            IntPtr fb1 = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glFramebufferRenderbufferEXT");
            if (fb1 != IntPtr.Zero)
                _FramebufferRenderbuffer = (TFramebufferRenderbuffer)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(fb1, typeof(TFramebufferRenderbuffer));
            else
                _FramebufferRenderbuffer = new TFramebufferRenderbuffer(GL.FramebufferRenderbuffer);

            IntPtr fb = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glGenFramebuffersEXT");
            if (fb != IntPtr.Zero)
                _GenFramebuffers = (TGenFrameBuffers)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(fb, typeof(TGenFrameBuffers));
            else
                _GenFramebuffers = new TGenFrameBuffers(GL.GenFramebuffers);

            IntPtr rb = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glGenRenderbuffersEXT");
            if (rb != IntPtr.Zero)
                _GenRenderbuffers = (TGenRenderbuffers)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(fb, typeof(TGenRenderbuffers));
            else
                _GenRenderbuffers = new TGenRenderbuffers(GL.GenRenderbuffers);
            IntPtr br = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glRenderbufferStorageEXT");
            if (br != IntPtr.Zero)
                _RenderbufferStorage = (TRenderbufferStorage)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(br, typeof(TRenderbufferStorage));
            else
                _RenderbufferStorage = new TRenderbufferStorage(GL.RenderbufferStorage);

            IntPtr dr = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glDeleteRenderbuffersEXT");
            if (dr != IntPtr.Zero)
                _DeleteRenderbuffers = (TDeleteRenderbuffers)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(dr, typeof(TDeleteRenderbuffers));
            else
                _DeleteRenderbuffers = new TDeleteRenderbuffers(GL.DeleteRenderbuffers);
            IntPtr rs = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glBindRenderbufferEXT");
            if (rs != IntPtr.Zero)
                _BindRenderbuffer = (TBindRenderbuffer)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(rs, typeof(TBindRenderbuffer));
            else
                _BindRenderbuffer = new TBindRenderbuffer(GL.BindRenderbuffer);
           IntPtr db = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glDeleteFramebuffersEXT");
            if (db != IntPtr.Zero)
                _DeleteFramebuffers = (TDeleteFrameBuffers)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(db, typeof(TDeleteFrameBuffers));
            else
                _DeleteFramebuffers = new TDeleteFrameBuffers(GL.DeleteFramebuffers);
            IntPtr bb = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glBindFramebufferEXT");
            if (bb != IntPtr.Zero)
                _BindFramebuffer = (TBindFrameBuffer)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(bb, typeof(TBindFrameBuffer));
            else
                _BindFramebuffer = new TBindFrameBuffer(GL.BindFramebuffer);

            IntPtr cb = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glCheckFramebufferStatusEXT");
            if (cb != IntPtr.Zero)
                _CheckFramebufferStatus = (TCheckFramebufferStatus)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(cb, typeof(TCheckFramebufferStatus));
            else
                _CheckFramebufferStatus = new TCheckFramebufferStatus(GL.CheckFramebufferStatus);

            IntPtr ft = Drawing3d.Windows.FeaturesW32.wglGetProcAddress("glFramebufferTexture2DEXT");
            if (ft != IntPtr.Zero)
                _FramebufferTexture2D = (TFramebufferTexture2D)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(ft, typeof(TFramebufferTexture2D));
            else
                _FramebufferTexture2D = new TFramebufferTexture2D(GL.FramebufferTexture2D);

        }
        int[] SaveViewPort = new int[4];
 
        int[] _Texture = new int[] { -1 };
        /// <summary>
        /// gets the handle of the texture, in which wil be drawn.
        /// </summary>
        public int FboTexture
        {
            get
            {
               return _Texture[0];
            }
        }

        int Depthbuffer
        {
            get
            { return _Depth[0]; }
        }
        internal int[] _Depth = new int[1] { -1 };
        /// <summary>
        /// gets the handle of the FBO.
        /// </summary>
        public int FboHandle
        {
            get
            { return _Fbo[0]; }
        }
        int[] _Fbo = null;
        Color _BackGround = Color.Black;
        int ActiveTexture = 0;
        /// <summary>
        /// gets and sets the background of the memory in which will be drawn.
        /// </summary>
        public Color BackGround
        {
            get { return _BackGround; }
            set { _BackGround = value; }
        }
        /// <summary>
        /// is an important method. Together with <see cref="DisableWriting"/> you can draw in the framebuffer by
        /// EnableWriting() <br/>
        /// draw something ....<br/>
        /// <see cref="DisableWriting"/><br/>
        /// </summary>
        public void EnableWriting()
        {
            EnableWriting(new Point(0, 0));
        }
        void EnableWriting(Point LeftTop)
        {
            ActiveTexture = GL.GetInteger(GetPName.TextureBinding2D);
           int at = GL.GetInteger(GetPName.ActiveTexture);
            GL.GetInteger(GetPName.Viewport, SaveViewPort);
            _BindFramebuffer(FramebufferTarget.Framebuffer, FboHandle);
            GL.BindTexture(TextureTarget.Texture2D, FboTexture);
            GL.Viewport((int)LeftTop.X, (int)LeftTop.Y,  Width, Height);
           
            GL.ClearColor(BackGround.R / 255f, BackGround.G / 255f, BackGround.B / 255f, BackGround.A / 255f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        }
        /// <summary>
        /// to gether with <see cref="EnableWriting()"/> is this a very important method.
        /// </summary>
        public void DisableWriting()
        {
            GL.Viewport(SaveViewPort[0], SaveViewPort[1], SaveViewPort[2], SaveViewPort[3]);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, ActiveTexture);
        }
       /// <summary>
       /// the empty constructor.
       /// </summary>
       public  FBO()
        {
            _Depth = new int[] { -1 };
            _Fbo = new int[] { -1 };
            _Texture = new int[] { -1 };

        }
        /// <summary>
        /// is a constructor with <b>with</b> and <b>height</b>.
        /// </summary>
        /// <param name="Width">the width of the frame buffer.</param>
        /// <param name="Height">the height of the frame buffer.</param>
        public FBO(int Width, int Height)
            : base()
        {
        
            _Depth = new int[] { -1 };
            _Fbo = new int[] { -1 };
            _Texture = new int[] { -1 };
            Init(Width, Height);
        }
        Texture FTexture = new Texture();
        /// <summary>
        /// gets a <see cref="Drawing3d.Texture"/> of the framebuffer.
        /// </summary>
        public Texture Texture
        {
            get
            {

                FTexture.Handle = FboTexture;
                return FTexture;
            }
        }
        const int GL_CLAMP = 0x00002900;
        int GL_LINEAR = 0x00002601;
        int GL_NEAREST = 9728;
        /// <summary>
        /// initializes the fbo for a drawing with depth. You have to call that, when <see cref="FboHandle"/> less than 0 or when the size of the fbo has changed.
        /// </summary>
        /// <param name="Width">the width.</param>
        /// <param name="Height">the height.</param>
        public void InitForDepth(int Width, int Height)
        {
            if ((FboTexture >= 0) && (Width == width) && (Height == height)) return;

            width = Width; height = Height;
            if (FboHandle < 0)
                _GenFramebuffers(1, out _Fbo[0]);
            _BindFramebuffer(FramebufferTarget.Framebuffer, FboHandle);
            _BindRenderbuffer(RenderbufferTarget.Renderbuffer, Depthbuffer);

            if (FboTexture == -1)
                GL.GenTextures(1, out _Texture[0]);
            GL.BindTexture(TextureTarget.Texture2D, FboTexture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, GL_LINEAR);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, GL_LINEAR);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GL_CLAMP);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GL_CLAMP);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);
            _FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, FboTexture, 0);
            FramebufferErrorCode status = _CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                throw new System.Exception(status.ToString());
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            _BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            // switch back to window-system-provided framebuffer
            _BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        /// <summary>
        /// initializes the Fbo with <b>with</b> and <b>height</b>. You have to call that, when <see cref="FboHandle"/> less than 0 or when the size of the fbo has changed.
        /// </summary>
        /// <param name="Width">is the width.</param>
        /// <param name="Height">is the height.</param>
        public virtual void Init(int Width, int Height)
        {
           
            if (FboHandle > 0) Dispose();
            width = Width; height = Height;
            ActiveTexture=GL.GetInteger(GetPName.TextureBinding2D);
            if (FboHandle < 0)
                _GenFramebuffers(1,out _Fbo[0]);
            if (FboHandle < 0)
                throw new System.Exception("Can not create a frame buffer");
            _BindFramebuffer(FramebufferTarget.Framebuffer, FboHandle);
            if (Depthbuffer < 0)
            {
                _GenRenderbuffers(1, out _Depth[0]);
            }

           
            _BindRenderbuffer(RenderbufferTarget.Renderbuffer, Depthbuffer);
             GL.TexImage2D(TextureTarget.Texture2D, 0,PixelInternalFormat.DepthComponent, width, height, 0,PixelFormat.DepthComponent, PixelType.UnsignedInt,IntPtr.Zero);
           if( GL.GetError()!= OpenTK.Graphics.OpenGL.ErrorCode.NoError)
            {
                Dispose();
                throw new Exception("OutOf Memory");
            }
           //     GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, Width, Height);
            _RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, Width, Height);
            _BindFramebuffer(FramebufferTarget.DrawFramebuffer, FboHandle);
            if (FboTexture < 0)
                GL.GenTextures(1, _Texture);
           
            GL.BindTexture(TextureTarget.Texture2D, FboTexture);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref GL_NEAREST);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref GL_NEAREST);
            {
       
                GL.TexImage2D(TextureTarget.Texture2D, 0,PixelInternalFormat.Rgba, Width, Height, 0,PixelFormat.Rgba,PixelType.UnsignedByte,IntPtr.Zero);
                _FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, FboTexture, 0);
                _FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, Depthbuffer);
            }
            int status = (int)GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            FramebufferStatus FS = (FramebufferStatus)(status - 0x8CD5);

            if (FS != FramebufferStatus.FRAMEBUFFER_COMPLETE)
            {

                int[] FB = new int[] { 1 };
                Dispose();
                throw new System.Exception(FS.ToString());
            }
            _BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            _BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            _BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, ActiveTexture);

            ;
        }
        int width, height = 0;
      
        int Width { get { return width; } }
        int Height { get { return height; } }
       
        /// <summary>
        /// Status of framebuffer operations
        /// </summary>
        public enum FramebufferStatus
        {
            /// <summary>
            /// Frame buffer is ok.
            /// </summary>
            FRAMEBUFFER_COMPLETE,//  0x8CD5
            /// <summary>
            /// Framebuffer incomplete, incomplete attachment
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_ATTACHMENT,
            /// <summary>
            /// Framebuffer incomplete, attached images must have same dimensions
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT,
            /// <summary>
            /// Framebuffer duplicate attachement
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT,
            /// <summary>
            /// Framebuffer incomplete, attached images must have same dimension
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_DIMENSIONS,
            /// <summary>
            /// Framebuffer incomplete, attached images must have same format
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_FORMATS,
            /// <summary>
            /// Framebuffer incomplete, missing draw buffer
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER,
            /// <summary>
            /// Framebuffer incomplete, missing read buffer
            /// </summary>
            FRAMEBUFFER_INCOMPLETE_READ_BUFFER,
            /// <summary>
            /// Framebuffer Unsupported framebuffer format
            /// </summary>
            FRAMEBUFFER_UNSUPPORTED,
            /// <summary>
            /// Framebuffer Status error
            /// </summary>
            FRAMEBUFFER_STATUS_ERROR

        }
        /// <summary>
        /// disposes all used handles. If <b>KeepTexture</b> is <b>true</b>, the <see cref="FboTexture"/> doesnt destroyed.
        /// </summary>
        /// <param name="KeepTexture"></param>
        public void Dispose(bool KeepTexture)
        {
            
            if (FboHandle >= 0)
                _DeleteFramebuffers(1, ref _Fbo[0]);
            
            if (Depthbuffer >= 0)
                _DeleteRenderbuffers(1, ref _Depth[0]);
          
            if (!KeepTexture)
                if (FboTexture >= 0)
                {
                  
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.DeleteTextures(1, _Texture);
                   
                }
            _Fbo[0] = -1;
            _Depth[0] = -1;
            _Texture[0] = -1;
          
        }
        /// <summary>
        /// disposes all used handles.
        /// </summary>
        public void Dispose()
        {
            Dispose(false);
        }
      
       
      /// <summary>
      /// reads a Bitmap from the FBO buffer.
      /// </summary>
      /// <returns>Bitmap</returns>
      public Bitmap ReadBitmap()
       {
            _BindFramebuffer(FramebufferTarget.Framebuffer, this.FboHandle);
            Bitmap b = new Bitmap(width, height);
            var bits = b.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
            _BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            b.UnlockBits(bits);
            b.RotateFlip(RotateFlipType.Rotate180FlipX);
            return b;
       }
        /// <summary>
        /// reads pixels from the buffer. The position is given by <b>x</b> and <b>y</b>. The size
        /// is determinated by Pixels.size.
        /// </summary>
        /// <param name="x">the x position, where it will be readed.</param>
        /// <param name="y">the y position, where it will be readed.</param>
        /// <param name="Pixels">the pixels</param>
        public void ReadPixel(int x, int y, uint[,] Pixels)
        {
            int Active = GL.GetInteger(GetPName.TextureBinding2D);
            int ColCount = Pixels.GetLength(0);
            int w = ColCount;
            int RowCount = Pixels.GetLength(1);
            int h = Pixels.GetLength(1);
            byte[] P = new byte[w * h * 4];
            _BindFramebuffer(FramebufferTarget.ReadFramebuffer, FboHandle);
            GL.BindTexture(TextureTarget.Texture2D, this.Depthbuffer);
            GL.ReadPixels(x, y, Pixels.GetLength(0), Pixels.GetLength(1), PixelFormat.Rgba, PixelType.UnsignedByte, P);
            _BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            int Row = 0;
            int Col = 0;
            int Mini = 100000;
            int Maxi = -1000000;
            int Minj = 100000;
            int Maxj = -1000000;
            for (int i = 0; i < w * h * 4; i += 4)
            {
                int K = P[i] * 16777216 + P[i + 1] * 65536 + P[i + 2] * 256 + P[i + 3];
                Pixels[Col, Row] = (uint)K;
                if (K != 0)
                {
                    if (Col < Mini) Mini = Col;
                    if (Col > Maxi) Maxi = Col;
                    if (Row < Minj) Minj = Row;
                    if (Row > Maxj) Maxj = Row;

                }

                Col++;
                if (Col >= ColCount)
                {
                    Col = 0;
                    Row++;
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, Active);
        }
    }
}







