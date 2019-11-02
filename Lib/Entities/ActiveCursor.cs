using System.Drawing;
using System;
namespace Drawing3d
{

    /// <summary>
    /// shows a cursor, wich follows tangents and normals drawings. Often you dont need the window cursor. See <see cref="OpenGlDevice.WincursorVisible"/>.  For the distance wehre the cursor is magnetic see <see cref="OpenGlDevice.SnapMagnetism"/>
    /// </summary>
    [Serializable]
    public class ActiveCursor : Entity
    {
        /// <summary>
        /// enumeration, which contains the kind when the cursor is over a cross.
        /// </summary>
        public enum Cross
        {
        /// <summary>
        /// the cursor doesnt draw a symbol in case of cross.
        /// </summary>
          None,
            /// <summary>
            /// the cursor draws a sphere in case of cross.
            /// </summary>
            Sphere,
            /// <summary>
            /// the cursor draws a cube in case of cross.
            /// </summary>
            Cube
        };

         Cross _CrossStyle = Cross.Sphere;
        /// <summary>
        /// sets and gets the <see cref="Cross"/>style. The default is a sphere.
        /// </summary>
        public Cross CrossStyle
        {
            get { return _CrossStyle; }
            set
            {
                _CrossStyle = value;
                
            }
        }

        Color _CrossColor = Color.Silver;
        /// <summary>
        /// gets and sets the color of the cross symbol. Default is silver.
        /// </summary>
        public Color ColoroFtheCrossSymbol
        {
            get { return _CrossColor; }
            set
            {
                _CrossColor = value;
              
            }
        }

        int _CrossSize = 12;
        /// <summary>
        /// gets and sets the size of the cross symbol in pixel. The default is 12.
        /// </summary>
        public int CrossSize
        {
            get { return _CrossSize; }
            set
            {
                _CrossSize = value;
             
            }
        }

        Base _Base = Base.UnitBase;
        /// <summary>
        /// gets the base of the active cursor.
        /// </summary>
        public Base Base
        {
            get { return _Base; }
           
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public ActiveCursor() : base()
        {
            CompileEnable = false;
            Selector.CrossTheSnappItems = true;
        }
        Color _xAxisColor = Color.Black;
        /// <summary>
        /// gets and sets the color of the x axis. The default is black.
        /// </summary>
        public Color xAxisColor
        {
            get { return _xAxisColor; }
            set
            {
                _xAxisColor = value;
               
            }
        }
       
        Color _yAxisColor = Color.Black;
        /// <summary>
        /// gets and sets the color of the y axis. The default is black.
        /// </summary>
        public Color yAxisColor
        {
            get { return _yAxisColor; }
            set
            {
                _yAxisColor = value;
              
            }
        }
        Color _zAxisColor = Color.Black;
        /// <summary>
        /// gets and sets the color of the z axis. The default is black.
        /// </summary>
        public Color zAxisColor
        {
            get { return _zAxisColor; }
            set
            {
                _zAxisColor = value;
          
            }
        }
        float _PenWidth = 1;
        /// <summary>
        /// gets and sets the width of the axis. The default is 1.
        /// </summary>
        public float PenWidth
        {
            get { return _PenWidth; }
            set
            {
                _PenWidth = value;
            
            }
        }

        int _Size = 12;
        /// <summary>
        /// gets and sets the size of the axis (in pixels). The default is 12.
        /// </summary>
        public int Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
            
            }
        }
        xyz BaseZ = new xyz(0, 0, 0);
        bool _AxisFix = false;
        /// <summary>
        /// the cursor ignores the direction of tangents. Default is false.
        /// </summary>
        public bool AxisFix
        {
            get { return _AxisFix; }
            set
            {
                _AxisFix = value;
      
            }
        }
     
        /// <summary>
        /// overriders the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {


          
            if (Device.RenderKind != RenderKind.Render) return;
            Device.PushMatrix();
            Device.ModelMatrix = Matrix.identity;
            _Base = new Base(true);  
          
           
                if (Device.SnappItems.Count > 0)
            {
            

                if ( (Device.SnappItems[0].Crossed) && (CrossStyle != Cross.None))
                {

                    Base Base = new Base(true);
                    Base.BaseO = Device.SnappItems[0].Point;
                    
                    Color C = Device.Ambient;
                    Device.Ambient = ColoroFtheCrossSymbol;
                    double r = Device.PixelToWorld(Device.SnappItems[0].Point,CrossSize);
                    if (CrossStyle == Cross.Sphere)
                        Device.drawSphere(Device.SnappItems[0].Point, r / 2f);
                    if (CrossStyle == Cross.Cube)
                    {
                        Base B = Device.SnappItems[0].GetBase();
                        
                        Matrix M = B.ToMatrix();
                        Device.PushMatrix();
                        Device.ModelMatrix = M;
                        Device.drawBox(new xyz(-r / 2, -r / 2, -r / 2), new xyz(r, r, r));
                        Device.PopMatrix();
                    }
                    if ((_Base.BaseO.X == 2) && (_Base.BaseO.y == 2) && (_Base.BaseO.Z == 2))
                    { }
                    Device.Ambient = C;
                    return;
                }

                else
                {
                    _Base.BaseO = Device.SnappItems[0].Point;
                    if (_Base.BaseO.dist(Device.Currentxyz)>0.1)
                    { }
                    if (!AxisFix)
                    {
                   
                        {
                            _Base = Device.SnappItems[0].GetBase();
                           
                        }
                    }
                }
            }
            else
            {
                _Base = Base.UnitBase;
                _Base.BaseO = Device.Currentxyz;
            }
          
            float PenW = Device.PenWidth;
            Device.PenWidth = PenWidth;
            Color SaveEmission = Device.Emission;
            bool Save = Device.LightEnabled;
            Device.LightEnabled = true;
  
            Device.Emission = xAxisColor;
            Device.drawLine(Base.BaseO - Base.BaseX *Device.PixelToWorld(Base.BaseO,Size), Base.BaseO + Base.BaseX * Device.PixelToWorld(Base.BaseO, Size));
            double d=Device.PixelToWorld(Base.BaseO, Size);
            Device.Emission = yAxisColor;
            Device.drawLine(Base.BaseO - Base.BaseY * Device.PixelToWorld(Base.BaseO, Size), Base.BaseO + Base.BaseY * Device.PixelToWorld(Base.BaseO, Size));
            Device.Emission = zAxisColor;
            Device.drawLine(Base.BaseO - Base.BaseZ * Device.PixelToWorld(Base.BaseO, Size), Base.BaseO + Base.BaseZ * Device.PixelToWorld(Base.BaseO, Size));
            Device.LightEnabled = Save;
            Device.Emission = SaveEmission;
            Device.PenWidth = PenW;
            Device.PopMatrix();
           
            BaseZ = Base.BaseZ;
           
            base.OnDraw(Device);
            
        }
    }
}
