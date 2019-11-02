using System;
using System.Collections.Generic;
namespace Drawing3d
{
#if LONGDEF

    using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
    /// <summary>
    /// is an <see cref="Entity"/> which draws a <b>text</b>. It has the fields <see cref="Text"/>, <see cref="Font"/>, <see cref="Italic"/> <see cref="Extrusion"/>,  <see cref="Size"/> and <see cref="Position"/>.
    /// </summary>
    [Serializable]
    public class TextEntity:Entity
    {
        System.Drawing.FontStyle saveStyle = System.Drawing.FontStyle.Regular;
        Font _Font = new Font("Times Roman");
        /// <summary>
        /// gets or sets the <see cref="Drawing3d.Font"/>.
        /// </summary>
        public Font Font {
            get { return _Font; }
            set { _Font = value; }
        }
      
       
        string _Text = "";
        /// <summary>
        /// is the text, which will be drawn.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
  
            }
        }
        bool _Italic = false;
        /// <summary>
        /// gets or sets, whether the <see cref="Text"/> is drawn in italic style. The default is false.
        /// </summary>
        public bool Italic
        {
            get { return _Italic; }
            set
            {
                _Italic = value;
            }
        }
        double _Extrusion = 1;
        /// <summary>
        /// get or sets the height of the <see cref="Text"/> in z-direction. The default is 1.
        /// </summary>
        public double Extrusion
        {
            get { return _Extrusion; }
            set
            {
                _Extrusion = value;
  
            }
        }
        double _Size = 1;
        /// <summary>
        /// gets or sets the <b>size</b> of the drawn text. The default is 1.
        /// </summary>
        public double Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
 
            }
        }
       xyz _Position = new xyz(0, 0,0);
        /// <summary>
        /// gets or sets the position of the text. The default is 0,0,0.
        /// </summary>
        public xyz Position
        {
            get { return _Position; }
            set { _Position = value;

                 }
        }
        xy getExtent()
        {
            if (Font == null) return new xy(-1, -1);
            double Save = Font.FontSize;
            Font.FontSize = Size;
            xy Result = Font.getEnvText(Text);
            Font.FontSize = Save;
            return Result;
        }
        OpenGlDevice Device = null;
     

        /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method to draw the text.
        /// </summary>
        /// <param name="Device"><see cref="OpenGlDevice"/> in which will be drawn.</param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            saveStyle = Font.FontStyle;
            this.Device = Device;
            base.OnDraw(Device);
            if (Italic)
            Font.FontStyle = System.Drawing.FontStyle.Italic;
            double Save = Font.FontSize;
            Font.FontSize = Size;
            Device.drawText(Font,  Matrix.Translation(Position), Text, Extrusion);
            Font.FontSize = Save;
            Font.FontStyle = saveStyle;
         }
    }
}
