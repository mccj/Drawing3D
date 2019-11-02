using Drawing3d;
using System.Drawing;
using System;
namespace Drawing3d
{
    /// <summary>
    /// is a class, wich draws a coordinate system
    /// </summary>
   
    [Serializable]
    public class CoordinateAxis:Entity
    {
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public CoordinateAxis()
        {
          
        }
        /// <summary>
        /// gets and sets the color wich is used for drawing.
        /// </summary>
        public Color Color = Color.Black;
        xyz _Size = new xyz(5, 5, 5);
    
        /// gets and sets the sicze of the coordinate system. See also <see cref="FullSize"/>  
        public xyz Size { get { return _Size; } set { _Size = value; } }
        xyz _Devider = new xyz(1, 1, 1);
        /// <summary>
        /// gets and sets the distances for the divider for every axis. Default is 1.
        /// </summary>
        public xyz Devider { get { return _Devider; } set { _Devider = value;} }
        
        double _DeviderLineLength = 0.1;
        /// <summary>
        /// an a deviderpoint will drawn a line with <b>DeviderLineLength</b>. Default = 0.1.
        /// </summary>
        public double DeviderLineLength { get { return _DeviderLineLength; } set { _DeviderLineLength = value;  } }
        bool _LeftAndRight = true;
        /// <summary>
        /// a value <b>false</b> shows only the right side of the axis. See also <see cref="Dim3d"/>
        /// </summary>
        public bool LeftAndRight { get { return _LeftAndRight; } set { _LeftAndRight = value;  } }

        double _TextHeight = 0.3;
        /// <summary>
        /// gets and sets the (world)size of the text.Default is 0.3;
        /// </summary>
        public double TextHeight { get { return _TextHeight; } set { _TextHeight = value;  } }
        bool _ShowText = true;
        /// <summary>
        /// Showtext indicates, wether the text will be painted. Default is <b>true</b>.
        /// </summary>
        public bool ShowText
        {
            get { return _ShowText; }
            set
            {
                _ShowText = value;
             
            }
        }

        
        bool _Dim3d = true;
        /// <summary>
        /// indicates the painting of the z-axis. the default is <b>true</b>.
        /// </summary>
        public bool Dim3d { get { return _Dim3d; } set { _Dim3d = value; } }

        Base _AxesBase = Base.UnitBase;
        /// <summary>
        /// gets and sets the Base for the axis.
        /// </summary>
        public Base AxesBase { get { return _AxesBase; } set { _AxesBase = value; } }
        string _FontName = "Arial";
  
        bool _FullSize = false;
        /// <summary>
        /// if is set <b>true</b> the axis fills the completely screen. the default is false.
        /// </summary>

        public bool FullSize
        {
            get { return _FullSize; }
            set
            {
                _FullSize = value;
               
            }
        }
      
        /// <summary>
        /// gets and sets the fontname for the text. Default is "Arial".
        /// </summary>
        public string FontName { get { return _FontName; } set { _FontName = value; } }
        Drawing3d.Font Font= null;
        double SaveFontSize = 1;
        /// <summary>
        /// indicates whether the devicer are drawn. Default is <b>true</b>.
        /// </summary>
        
        bool _ShowDevider = true;
        /// <summary>
        /// gets and sets, whether the <b>devider</b> will be drawn.
        /// </summary>
        public bool ShowDevider
        {
            get { return _ShowDevider; }
            set
            {
                _ShowDevider = value;
                
            }
        }
      
        /// <summary>
        /// overrides the draw nethod and paints the axis.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        { if (FullSize)
            {
                xy WE = Device.WorldExtensions;
                if (WE.dist(Size.toXY()) > 0.2)
                    SetInvalid(true);
                Size = new xyz(WE.x, WE.y, WE.x);
            }
            if (Font == null)
            {
                Font = new Drawing3d.Font(FontName);
                
            }
            SaveFontSize = Font.FontSize;
            Font.FontSize = TextHeight;
            base.OnDraw(Device);
            Device.PushMatrix();
            Device.MulMatrix(AxesBase.ToMatrix());
            bool SaveLighting = Device.LightEnabled;
            Device.LightEnabled = false;
            Color Save = Device.Emission;
            Device.Emission = Color;
            
            if (LeftAndRight)
            {


                Device.drawLine(new xyz(-Size.x, 0, 0), new xyz(Size.x, 0, 0));
                Device.drawLine(new xyz(0, -Size.y, 0), new xyz(0, Size.y, 0));
                if (Dim3d)
                    Device.drawLine(new xyz(0, 0, -Size.z), new xyz(0, 0, Size.z));

            }
           
            {
                Device.drawLine(new xyz(0, 0, 0), new xyz(Size.x, 0, 0));
                Device.drawLine(new xyz(0, 0, 0), new xyz(0, Size.y, 0));
                if (Dim3d)
                    Device.drawLine(new xyz(0, 0, 0), new xyz(0, 0, Size.z));

            }

            xyz n1 = new xyz(0, DeviderLineLength, 0);
            int from = (int)(-Size.x / Devider.x);
            if (!LeftAndRight)
                from = 0;
           
            if (Device.RenderKind == RenderKind.Render)
            {  
                for (int i = from; i <= Size.x / Devider.x; i++)
                {
                   
                    if (ShowDevider)
                        Device.drawLine(new xyz(Devider.x * i, 0, 0) - n1, new xyz(Devider.x * i, 0, 0) + n1);
                    if (ShowText)
                    {

                        double Pos = Devider.x * i - ((float)Device.getEnvText(Font, i.ToString()).x / 2f);

                        Device.drawText(Font,Matrix.Translation(new xyz(Pos, DeviderLineLength, 0)), i.ToString(), 0);
                    }
                }
               

                from = (int)(-Size.y / Devider.y);
                if (!LeftAndRight)
                    from = 0;
                n1 = new xyz(DeviderLineLength, 0, 0);
                for (int i = from; i <= Size.y / Devider.y; i++)
                {  
                    if (i != 0)
                    {
                        if (ShowDevider)
                            Device.drawLine(new xyz(0, Devider.y * i, 0) - n1, new xyz(0, Devider.y * i, 0) + n1);
                        if (ShowText)
                        {

                            double Pos = Devider.x * i - ((float)Device.getEnvText(Font, i.ToString()).y / 2f);

                            Device.drawText(Font, Matrix.Translation(new xyz( DeviderLineLength, Pos, 0)), i.ToString(), 0);

                           
                        }
                    }
                }
                if (Dim3d)
                    for (int i = from; i <= Size.z / Devider.z; i++)
                    {
                        if (i != 0)
                        {
                            if (ShowDevider)
                                Device.drawLine(new xyz(0,0, Devider.z * i) - n1, new xyz(0, 0, Devider.z * i) + n1);
                            if (ShowText)
                            {
                                double Pos = Devider.z * i - ((float)Device.getEnvText(Font, i.ToString()).y / 2f);

                                Device.drawText(Font, Matrix.Translation(new xyz( DeviderLineLength, 0, Pos)), i.ToString(), 0);

                          
                            }
                        }
                    }

            }
           if (Device.RenderKind== RenderKind.SnapBuffer)
            {
                if (LeftAndRight)
                {


                    Device.drawLine(new xyz(-Size.x, 0, 0), new xyz(Size.x, 0, 0));
                    Device.drawLine(new xyz(0, -Size.y, 0), new xyz(0, Size.y, 0));
                    if (Dim3d)
                        Device.drawLine(new xyz(0, 0, -Size.z), new xyz(0, 0, Size.z));

                }
                else
             if (LeftAndRight)
                {
                    Device.drawLine(new xyz(0, 0, 0), new xyz(Size.x, 0, 0));
                    Device.drawLine(new xyz(0, 0, 0), new xyz(0, Size.y, 0));
                    if (Dim3d)
                        Device.drawLine(new xyz(0, 0, 0), new xyz(0, 0, Size.z));

                }
                for (int i = from; i <= Size.x / Devider.x; i++)
                {
                    
                    Device.drawPoint(new xyz(Devider.x * i, 0, 0), Devider.x/4);
                    if (ShowText)
                    {

                        double Pos = Devider.x * i - ((float)Device.getEnvText(Font, i.ToString()).x / 2f);
                        MeshContainer M = MeshCreator.MeshListCurrent;
                        Device.drawText(Font, Matrix.Translation(new xyz(Pos, DeviderLineLength, 0)), i.ToString(), 0);
                        M = MeshCreator.MeshListCurrent;
                    }
                }


                from = (int)(-Size.y / Devider.y);
                if (!LeftAndRight)
                    from = 0;
                n1 = new xyz(DeviderLineLength, 0, 0);
                for (int i = from; i <= Size.y / Devider.y; i++)
                {
                    if (i != 0)
                    {
                        Device.drawPoint(new xyz(0, Devider.y * i, 0), Devider.x / 4);
                        if (ShowText)
                        {

                            double Pos = Devider.x * i - ((float)Device.getEnvText(Font, i.ToString()).y / 2f);

                            Device.drawText(Font, Matrix.Translation(new xyz(DeviderLineLength, Pos, 0)), i.ToString(), 0);


                        }
                    }
                }
                if (Dim3d)
                    for (int i = from; i <= Size.z / Devider.z; i++)
                    {
                        if (i != 0)
                        {
                            Device.drawPoint(new xyz(0,0, Devider.z * i), Devider.x / 4);
         
                            if (ShowText)
                            {
                                double Pos = Devider.z * i - ((float)Device.getEnvText(Font, i.ToString()).y / 2f);

                                Device.drawText(Font, Matrix.Translation(new xyz(DeviderLineLength, 0, Pos)), i.ToString(), 0);


                            }
                        }
                    }

            }
        
            Device.Emission = Save;
            Device.LightEnabled = SaveLighting;
            Font.FontSize = SaveFontSize;
            Device.PopMatrix();

        }
       
    }
}
