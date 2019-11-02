
using System.Drawing;
using OpenTK.Graphics.OpenGL;
namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        private RectangleF _ViewPort = new RectangleF(0, 0, 100, 100);
        /// <summary>
        /// gets and sets the dimensions of the view port. This are most the values of the <see cref="WinControl"/>
        /// </summary>
        public RectangleF ViewPort
        {
           get
            {
                
                return _ViewPort;
            }
            set
            {    
             
                _ViewPort = value;
               if (this.IsReady())
                 GL.Viewport((int)value.Left, (int)value.Top, (int)value.Width, (int)value.Height);
               
                if (Shader!=null)
                {
                    Field W = Shader.getvar("WindowWidth");
                    if (W != null) W.Update();
                    W = Shader.getvar("WindowHeight");
                    if (W != null) W.Update();
                    CheckError();
                }
             
                CheckError();       
            }


        }
    }
}
