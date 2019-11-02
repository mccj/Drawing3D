using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System;
using System.Windows.Forms;

namespace Drawing3d
{ /// <summary>
  /// inherits from <see cref="CtrlEntity"/>. it manades a rectangle, wich can be ver helpful.
  /// Eg. <see cref="TransformItems"></see> colorsettings.
  ///
  /// </summary>
  [Serializable]
    public class CtrlRectangle : CtrlEntity
    { 
        delegate void DrawEventHandler(Entity sender, OpenGlDevice Device);
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public CtrlRectangle()
        {
           
            
        
        }

        

        /// <summary>
        /// a construcor which have a <see cref="OpenGlDevice"/>. See also <see cref="OpenGlDevice"/>
        /// </summary>
        /// <param name="Device"></param>
        public CtrlRectangle(OpenGlDevice Device) : base(Device)
        {
            
        }
        /// <summary>
        /// a construcor which have a <see cref="CtrlEntity.Base"/> and a <see cref="OpenGlDevice"/>. See also <see cref="OpenGlDevice"/>
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Device"></param>
        public CtrlRectangle(Base Base, OpenGlDevice Device) : this()
        {

            this.Base = Base;
            this.Device = Device;

        }
        /// <summary>
        /// a construcor which have a <see cref="Rectangled"/> and a <see cref="OpenGlDevice"/>. See also <see cref="OpenGlDevice"/>
        /// </summary>
        /// <param name="Rectangle"></param>
        /// <param name="Device"></param>
        public CtrlRectangle(Rectangled Rectangle, OpenGlDevice Device) : base(Device)
        {
            
            _A = new xy(Rectangle.X, Rectangle.Y);
            _B = new xy(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
          
           

        }
        xy _A = new xy(0, 0);
        /// <summary>
        /// on point of a rectangle.
        /// </summary>
        public xy A
        {
            get { return _A; }
            set { _A = value; }
        }
        xy _B = new xy(0, 0);
        /// <summary>
        /// on point of a rectangle.
        /// </summary>
        public xy B
        {
            get { return _B; }
            set { _B = value; }
        }
        /// <summary>
        /// gets or sets this property. A value of <b>false</b> loged the object ot of the <see cref="OpenGlDevice.EventServer"/>
        /// The default is true.
        /// </summary>
        public bool Keep = true;
        /// <summary>
        /// gets or sets this property. A value of <b>false</b> remove this object from screen. The default is white.
        /// </summary>
        public bool dontShowAfterMouseUp = false;

        Color _MarkerColor = Color.White;
        /// <summary>
        /// gets and sets the color of the marker. The default is white.
        /// </summary>
        [Browsable(false)]
        public Color MarkerColor
        {
            get { return _MarkerColor; }
            set { _MarkerColor = value; }
        }
        /// <summary>
        /// gets and sets the with of the pen. The default is 1.
        /// </summary>
        public float PenWidth = 1;
        Color _Color = Color.White;
        /// <summary>
        /// gets and sets the color of this object. The default is <b>white</b>.
        /// </summary>
        [Browsable(false)]
        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        enum State
        {
            Nothing,
            ShowMarked,
            Transform,
            Create_SetFirstPoint,
            Create_SetSecondPoint
        }

        void DrawSkeleton()
        {
            if (Device == null) return;
            Matrix P = Device.ProjectionMatrix;
            PolygonMode PM = Device.PolygonMode;
            Device.PolygonMode = PolygonMode.Line;
            float PW = Device.PenWidth;
            Device.PenWidth = PenWidth;
            Color C = Device.Emission;

            Device.Emission = Color;
            Device.PushMatrix();
            Device.ModelMatrix = Base.ToMatrix(); 
            bool LE = Device.LightEnabled;
            Device.LightEnabled = false;
            Device.drawRectangle(A, B);
            Device.LightEnabled = LE;
            Device.PopMatrix();
            Device.Emission = C;
            Device.PenWidth = PW;
            Device.PolygonMode = PM;

            Device.ProjectionMatrix = P;
        }
        State _CurrentState = State.Nothing;
        [Browsable(false)]
        State CurrentState
        {
            get
            {
                return _CurrentState;
            }
            set { _CurrentState = value; }
        }
        /// <summary>
        /// gets the current position of the mouse in world coordinates.
        /// </summary>
        public xy Currentxy
        {
            get { return Currentxyz.toXY(); }
        }
        bool _Creating = true;
        /// <summary>
        /// is a get and set property. When is <b>true</b> it starts a creation by every click ( outside).
        /// </summary>
        [Browsable(false)]
        public bool Creating
        {
            get
            {

                return _Creating;
            }
            set
            {
                _Creating = value;
            }
        }
        /// <summary>
        /// gets and sets the hightlight of the marker. The default is Yellow.
        /// </summary>
        public Color HighLightMarker = Color.Yellow;
        bool _KeepAspect = false;

        /// <summary>
        /// <b>true</b> holds fix the relation between the edges of the rectangle. Default is false;
        /// </summary>
        [Browsable(false)]
        public bool KeepAspect
        {
            get { return _KeepAspect; }
            set
            {
                _KeepAspect = value;
            }
        }
        enum Corner
        {
            none, C00, C10, C11, C01, Inside
        }
     
        List<Entity> VisibleEntities = new List<Entity>();
        /// <summary>
        /// is a list of <see cref="ITransform2d"/>s. These objects will bee automatically trabsformed wen a transform of the rectangle happens,
        /// </summary>
        public List<ITransform2d> TransformItems = new List<ITransform2d>();
        Matrix3x3 _CurrentMatrix = Matrix3x3.identity;
        Matrix3x3 CurrentMatrix
        { get { return _CurrentMatrix; }
          set { _CurrentMatrix = value;
               if (value. Equals(Matrix.identity))
                    { }
              }

        }

        Matrix3x3 RelativMatrix = Matrix3x3.identity;
        xy StartTrans = new xy(0, 0);
        xy StartA = new xy(0, 0);
        xy StartB = new xy(0, 0);
        /// <summary>
        /// gets and sets the dimension of the marker. Default is 0.1;
        /// </summary>
        public double MarkerWidth = 0.1;
        Corner CurrentCorner = Corner.none;


   
        /// <summary>
        /// is a constructor, which sets the  <b>Device</b>, the <see cref="Rectangle"/> and the <see cref="TransformItems"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> </param>
        /// <param name="Rectangle"></param>
        /// <param name="TransformList"></param>
        public CtrlRectangle(OpenGlDevice Device, System.Drawing.RectangleF Rectangle, List<ITransform2d> TransformList)
        {
            this.Device = Device;
            this.Rectangle = Rectangle;
            this.TransformItems = TransformList;
      }
        
        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnMouseDown(HandledMouseEventArgs)"/>method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnMouseDown(HandledMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Device == null) return;
            CurrentCorner = getCorner();
            
            TransFormAll = Matrix3x3.identity;
            
            CheckPoints();
            visible_ = true;
          
         
            Device.ForegroundDrawEnable = true;


         
            if ((CurrentCorner == Corner.none))
                if (Creating)
                {
                    A = B = Currentxy;
                    CurrentState = State.Create_SetSecondPoint;
                }
                else

                    OnLogout(false);

            e.Handled = true; return;
            
        }
        
        /// <summary>
        /// indicates, whether the marker should be drawn. The default is true.
        /// </summary>
        public bool MarkerVisible = true;
        /// <summary>
        /// overrides the <see cref="Entity.OnForegroundDraw(OpenGlDevice)"/>method.
        /// </summary>
        /// <param name="Device"></param>
        public override void OnForegroundDraw(OpenGlDevice Device)
        {
            if (Device == null) return;
            if (!visible_) return;
            if (!VisibilityChecked) return;

            ShowMarker();
            DrawSkeleton();
           
            
            base.OnForegroundDraw(Device);
        }
        /// <summary>
        /// gets and sets the rectangle, which has <see cref="A"/> and <see cref="B"/> as diametrical points.
        /// </summary>
        [Browsable(false)]
        public System.Drawing.RectangleF Rectangle
        {
            get { return Utils.ToRectangle(A, B); }
            set
            {
                A = new xy(value.Location.X, value.Location.Y);
                B = new xy(value.Location.X + value.Size.Width, value.Location.Y + value.Size.Height);
            }
        }
        /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw"/>method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            if (Device == null) return;
            if (Device.RenderKind == RenderKind.SnapBuffer)
            { }
            if (!visible) return;
            if (Device.RenderKind != RenderKind.SnapBuffer)
                base.OnDraw(Device);
            if (!Device.ForegroundDrawEnable)
            {  if (MarkerVisible)
                ShowMarker();
                DrawSkeleton();
            }

            if (Device.RenderKind == RenderKind.SnapBuffer)
                ShowMarker();
        }
        bool _visible = true;
        bool visible_ = true;
        /// <summary>
        /// overrides the <see cref="OnLogout(bool)"/> method and reset <see cref="OpenGlDevice.ForegroundDrawEnable"/>.
        /// </summary>
        /// <param name="KeepTheDevice"></param>
        public override void OnLogout(bool KeepTheDevice)
        {
            Device.ForegroundDrawEnable = false;
            base.OnLogout(KeepTheDevice);
        }
        bool visible
        {
            get { return _visible; }
            set { _visible = value; visible_ = value; }
        }
     
        void ShowMarker()
        {
            if (A.dist(B) < 0.0001) return;

            Color Save = Device.Emission;
            Device.Emission = MarkerColor;
            double MW = MarkerWidth;
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {


                MW *= 2;
                // To snap it easier
                PolygonMode PM = Device.PolygonMode;
                Device.PolygonMode = PolygonMode.Fill;
                Device.PushTag(Corner.Inside);
                Device.drawRectangle(A, B);
                Device.PopTag();
                Device.PolygonMode = PM;
            }
            Device.Emission = MarkerColor;
            Device.PushTag(Corner.C00);
            if ((Device.SnappItems.Count > 0) && (Device.SnappItems[0].Tag != null) && ((Corner)Device.SnappItems[0].Tag == Corner.C00))
                Device.Emission = HighLightMarker;
            Device.drawPoint(A.toXYZ(), 2*MW, OpenGlDevice.PointKind.Cube);
            Device.Emission = MarkerColor;
            Device.PopTag();
            Device.PushTag(Corner.C01);
            if ((Device.SnappItems.Count > 0) && (Device.SnappItems[0].Tag != null) && ((Corner)Device.SnappItems[0].Tag == Corner.C01))
                Device.Emission = HighLightMarker;
            Device.drawPoint(new xyz(A.x, B.Y, 0), 2 * MW, OpenGlDevice.PointKind.Cube);
            Device.Emission = MarkerColor;
            Device.PopTag();
            Device.PushTag(Corner.C11);
            if ((Device.SnappItems.Count > 0) && (Device.SnappItems[0].Tag != null) && ((Corner)Device.SnappItems[0].Tag == Corner.C11))
                Device.Emission = HighLightMarker;
            Device.drawPoint(B.toXYZ(), 2 * MW, OpenGlDevice.PointKind.Cube);
            Device.Emission = MarkerColor;
            Device.PopTag();

            Device.PushTag(Corner.C10);
            if ((Device.SnappItems.Count > 0) && (Device.SnappItems[0].Tag != null) && ((Corner)Device.SnappItems[0].Tag == Corner.C10))
                Device.Emission = HighLightMarker;
            Device.drawPoint(new xyz(B.x, A.Y, 0), 2 * MW, OpenGlDevice.PointKind.Cube);
            Device.Emission = MarkerColor;
            Device.PopTag();
            Device.Emission = Save;
        }
        bool VisibilityChecked = true;
        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnMouseUp(HandledMouseEventArgs)"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnMouseUp(HandledMouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Device == null) 
            return;


            CurrentCorner = Corner.none;
            CurrentState = State.Nothing;
            if (dontShowAfterMouseUp) visible = false;
            if (!LiveTransform)
                for (int i = 0; i < TransformItems.Count; i++)
                {
                    TransformItems[i].Transform(TransFormAll);
               
                }
  
            if (!TransFormAll.Equals(Matrix.identity))
            {
                while (UndoList.Count > CurrentUndoPosition + 1) { if (UndoList.Count > 0) UndoList.RemoveAt(UndoList.Count - 1); };
                 CurrentUndoPosition = CurrentUndoPosition + 1;
            }
            Device.ForegroundDrawEnable = false;
            return;
        }
        /// <summary>
        /// sets to <b>true</b> if you want tranformation by every mouse move.
        /// If is f<b>false</b> only after the bouse up will be transformed. See alse <see cref="TransformItems"/>
        /// </summary>
        public bool LiveTransform = true;
        Matrix3x3 TransFormAll = Matrix3x3.identity;
        int _CurrentUndoPosition = -1;
        int CurrentUndoPosition
        {
            get { return _CurrentUndoPosition; }
            set { _CurrentUndoPosition = value; }
        }
        
       
        bool FirstMove = true;
        xy AFirst = new xy(0, 0);
        xy BFirst = new xy(0, 0);
        xy _FloatPoint = new xy(1e10, 1e10);
        xy FloatPoint
        {
            get { return _FloatPoint; }

            set { _FloatPoint = value; }
        }
        xy _FixPoint = new xy(1e10, 1e10);
        xy FixPoint
        {
            get { return _FixPoint; }

            set { _FixPoint = value; }
        }
        xy _DiametralFixPoint = new xy(1e10, 1e10);
        xy DiametralFixPoint
        {
            get { return _DiametralFixPoint; }
            set { _DiametralFixPoint = value; }
        }

        void CheckPoints()
        {
            StartB = Currentxy;
            StartA = Currentxy;

            switch (CurrentCorner)
            {
                case Corner.none:
                    break;
                case Corner.Inside:
                    {
                        FloatPoint = Currentxy;
                        FixPoint = Currentxy;
                        break;
                    }
                case Corner.C00:
                    FloatPoint = StartA;
                    FixPoint = StartB;
                    DiametralFixPoint = B;
                    break;
                case Corner.C10:
                    FloatPoint = new xy(StartB.x, StartA.y);
                    FixPoint = new xy(StartA.x, StartB.y);
                    DiametralFixPoint = new xy(A.x, B.y);
                    break;
                case Corner.C11:
                    FloatPoint = StartB;
                    FixPoint = StartA;
                    DiametralFixPoint = A;

                    break;

                case Corner.C01:

                    FixPoint = new xy(StartB.x, StartA.y);
                    FloatPoint = new xy(StartA.x, StartB.y);
                    DiametralFixPoint = new xy(B.x, A.y);
                    break;

                default:
                    break;
            }
        }

  
        void _Trafo()
        {
            if (KeepAspect)
            {
                double _F = 1;
                if ((FloatPoint.X - DiametralFixPoint.X) != 0)
                    _F = (Currentxy.X - DiametralFixPoint.x) / (FloatPoint.X - DiametralFixPoint.X);
                else
                     if ((FloatPoint.Y - DiametralFixPoint.Y) != 0)
                    _F = (Currentxy.Y - DiametralFixPoint.Y) / (FloatPoint.Y - DiametralFixPoint.Y);

                CurrentMatrix = Matrix3x3.Translation(DiametralFixPoint) * Matrix3x3.Scale(new xy(_F, _F)) * Matrix3x3.Translation(DiametralFixPoint).invert();
            }
            else
            if (((FloatPoint.X - DiametralFixPoint.X) != 0) && ((FloatPoint.X - DiametralFixPoint.X) != 0))
            {
                xy Factor = new xy(1, 1);
                Factor = new xy((Currentxy.X - DiametralFixPoint.x) / (FloatPoint.X - DiametralFixPoint.X), (Currentxy.Y - DiametralFixPoint.Y) / (FloatPoint.Y - DiametralFixPoint.Y));

                CurrentMatrix = Matrix3x3.Translation(DiametralFixPoint) * Matrix3x3.Scale(new xy(Factor.x, Factor.Y)) * Matrix3x3.Translation(DiametralFixPoint).invert();


             
               
            }


        }
        bool Inside()
        {
            double MinX = System.Math.Min(A.X, B.X);
            double MinY = System.Math.Min(A.Y, B.Y);
            double MaxX = System.Math.Max(A.X, B.X);
            double MaxY = System.Math.Max(A.Y, B.Y);
            return ((MinX < Currentxy.X) && (MinY < Currentxy.Y) && (MaxX > Currentxy.X) && (MaxY > Currentxy.Y));

        }
        Corner getCorner()
        {
            Corner Result = Corner.none;
            if (Device.SnappItems.Count > 0)
                if (Device.SnappItems[0].Tag != null)
                    Result = (Corner)Device.SnappItems[0].Tag;
            if (Result == Corner.none) if (Inside()) Result = Corner.Inside;
            return Result;
        }

        List<Matrix> UndoList = new List<Matrix>();

        /// <summary>
        /// overrides the <see cref="CtrlEntity.OnMouseMove(HandledMouseEventArgs)"/>method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override void OnMouseMove(HandledMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Device == null) return;
            if (!Device.ForegroundDrawEnable) return;
            if (FirstMove)
            {

                AFirst = A;
                BFirst = B;
                FirstMove = false;

            }
            
            if (CurrentState == State.Create_SetSecondPoint)
                B = Currentxy;
            xy Factor = new xy(1, 1);
            if (CurrentCorner != Corner.none)
            {
                switch (CurrentCorner)
                {
                    case Corner.none:
                        break;
                    case Corner.C00:
                        _Trafo();


                        break;
                    case Corner.C10:
                        _Trafo();
                        break;
                    case Corner.C11:

                        _Trafo();
                        break;
                    case Corner.C01:
                        _Trafo();
                        break;
                    case Corner.Inside:
                        { CurrentMatrix = Matrix3x3.Translation((Currentxy - FloatPoint)); }
                        break;
                    default:
                        break;
                }
                if (!CurrentMatrix.Equals(Matrix3x3.identity))
                {
                    A = CurrentMatrix * A;
                    B = CurrentMatrix * B;
                }
                if (Math.Abs(A.x-B.X)<0.01)
                { }
                FloatPoint = Currentxy;
            }
            if (LiveTransform)
            {
                for (int i = 0; i < TransformItems.Count; i++)
                {
                   
                    TransformItems[i].Transform(CurrentMatrix);
                  
                }
                
                if (Device.ForegroundDrawEnable)
                    if (LiveTransform)
                    {
                        Device.RefreshBackGround();
                        Device.CopyFromBackGround();
                    }
                
            }
            TransFormAll = TransFormAll * CurrentMatrix;
            
            return;
        }
    }
    
}
