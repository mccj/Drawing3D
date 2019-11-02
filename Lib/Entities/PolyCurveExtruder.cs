using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d
{

    /// <summary>
    /// definition of a delegate, which is used from <see cref="PolyCurveExtruder.CreateCurveExtruder"/>
    /// </summary>
    /// <param name="Sender"><see cref="PolyCurveExtruder"/></param>
    /// <param name="i">first index in <see cref="Loca"/>. Loca[i] is a <see cref="CurveArray"/></param>
    /// <param name="j">from ith curvearray gives the jth element a <see cref="Curve"/>. e.g <see cref="Curve"/> = Loca[0][1] </param>
    /// <returns>CurveExtruder</returns>
    public delegate CurveExtruder Creation(PolyCurveExtruder Sender, int i, int j);
    /// <summary>
    /// extrudes a <see cref="Loca"/>. She is restricted by two planes.
    /// </summary>
    [Serializable]
    public class PolyCurveExtruder : Entity
    {
        Loxy UpLoxy = new Loxy();
        Loxy DownLoxy = new Loxy();
        bool _ShowUpPlane = false;
        bool _ShowDownPlane = false;
        Matrix UpMatrix = Matrix.identity;
        Matrix DownMatrix = Matrix.identity;
        /// <summary>
        ///  a value <b>true</b> shows the upper plane. Default is <b>false</b>. See also <see cref="Height"/>.
        /// </summary>
        public bool ShowUpPlane
        {

            get { return _ShowUpPlane; }
            set
            {
                _ShowUpPlane = value;
                Check();
 
            }
        }
        CurveExtruder CE = new CurveExtruder();
        /// <summary>
        ///  a value <b>true</b> shows the downplane. Default is <b>false</b>. See also <see cref="Height"/>.
        /// </summary>
        public bool ShowDownPlane
        {

            get { return _ShowDownPlane; }
            set
            {

                _ShowDownPlane = value;
                Check();

            }
        }
        List<List<CurveExtruder>> Curvextruders = new List<List<CurveExtruder>>();

        Plane _DownPlane = null;
        /// <summary>
        /// gets and sets the down <see cref="Plane"/>. See also <see cref="Height"/>.
        /// </summary>
        public Plane DownPlane
        {
            get { return _DownPlane; }
            set
            {
                _DownPlane = value;
                for (int i = 0; i < Curvextruders.Count; i++)
                {
                    for (int j = 0; j < Curvextruders[i].Count; j++)
                    {
                        Curvextruders[i][j].DownPlane = DownPlane;
                    }

                }
               Check();
           }
        }

        Plane _UpPlane = null;
        /// <summary>
        /// gets and sets the upper <see cref="Plane"/>.  See also <see cref="Height"/>.
        /// </summary>
        public Plane UpPlane
        {
            get { return _UpPlane; }
            set
            {
                _UpPlane = value;
                for (int i = 0; i < Curvextruders.Count; i++)
                {
                    for (int j = 0; j < Curvextruders[i].Count; j++)
                    {

                        Curvextruders[i][j].UpPlane = UpPlane;
                    }

                }
                Check();
 
            }
        }
        double _Height = 1;
        /// <summary>
        /// gets and sets the height of the extruder.Improtant: If it is negative it respects the <see cref="DownPlane"/> and the <see cref="UpPlane"/> 
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                for (int i = 0; i < Curvextruders.Count; i++)
                {
                    for (int j = 0; j < Curvextruders[i].Count; j++)
                    {
                        Curvextruders[i][j].Height = Height;
                    }
                }
                Check();

            }
        }


        xyz _Direction = new xyz(0, 0, 0);
        /// <summary>
        /// gets and sets the direction in wich it will be extruded.
        /// </summary>
        public xyz Direction
        {
            get { return _Direction; }
            set
            {
                _Direction = value;
                for (int i = 0; i < Curvextruders.Count; i++)
                {
                    for (int j = 0; j < Curvextruders[i].Count; j++)
                    {
                        Curvextruders[i][j].Direction = Direction;
                    }

                }
                Check();

            }
        }
        /// <summary>
        /// is a event, wich is called from <see cref="CreateCurveExtruder"/>. so you can
        /// <br/>
        /// CreateCurveExtruder += MyCreator;<br/>
        /// with<br/>
        /// private CurveExtruder MyCreator(PolyCurveExtruder Sender, int i, int j)<br/>
        ///{<br/>
        ///    // i and j are theindices in the Loca<br/>
        ///    return = new MyCurveExtruder();<br/>
        ///}<br/>
        ///where MyCurveExtruder is a class, wich inherits from CurveExtruder.
        ///In this class youcan define som properties like color, texture, Material etc.
        ///The sides of the extruder has this properties.
        /// 
        /// 
        /// </summary>
        public event Creation CreateCurveExtruder;
        /// <summary>
        /// is a method, wich is called when a extruder will be created.For every curve of the <see cref="Loca"/> a <see cref="CurveExtruder"/> wil be created.
        /// The event <see cref="CreateCurveExtruder"/> is called.
        /// If you create you a class, wich inherits from PolyCurveExtruder you can override this method with result
        /// from a class which inherits from <see cref="CurveExtruder"/>.
        /// </summary>
        /// <param name="i">first index in <see cref="Loca"/>. Loca[i] is a <see cref="CurveArray"/></param>
        /// <param name="j">from ith curvearray gives the jth element a <see cref="Curve"/>. e.g <see cref="Curve"/> = Loca[0][1] </param>
        /// <returns>a <see cref="CurveExtruder"/></returns>
        virtual protected CurveExtruder OnCreateCurveExtruder(int i, int j)
        {
            if (CreateCurveExtruder != null) return CreateCurveExtruder(this, i, j);
            CurveExtruder Result = new CurveExtruder();
            return Result;
        }
        Loca _Loca = new Loca();
        /// <summary>
        /// this <see cref="Loca"/> holds the boundary. It is nessary that Loca[0] is clockwise.
        /// </summary>
        public Loca Loca
        {
            get { return _Loca; }
            set
            {
                Curvextruders.Clear();
                _Loca = value;
                if (Loca != null)
                {
                    for (int i = 0; i < Loca.Count; i++)
                    {
                        List<CurveExtruder> M = new List<CurveExtruder>();
                        Curvextruders.Add(M);
                        for (int j = 0; j < Loca[i].Count; j++)
                        {
                            CurveExtruder CR = OnCreateCurveExtruder(i, j);
                            CR.Curve = Loca[i][j];
                            CR.Height = Height;
                            CR.DownPlane = DownPlane;
                            CR.UpPlane = UpPlane;
                            CR.Direction = Direction;
                            M.Add(CR);
                        }
                    }
                }
                Check();
 
            }
        }

        void Check()
        {
            CheckLoxy();
        }
        void CheckLoxy()
        {
            Base DownBase = Base.UnitBase;
            Base UpBase = Base.UnitBase;
            DownLoxy = getLoxy(0, ref DownBase);
            UpLoxy = getLoxy(1, ref UpBase);
            DownBase.BaseZ = DownBase.BaseZ * (-1);
            UpMatrix = UpBase.ToMatrix();
            DownMatrix = DownBase.ToMatrix();
        }
        Loxy getLoxy(double UpDown, ref Base Base)
        {
            Loxy Result = new Loxy();
            if ((Curvextruders != null) && (Curvextruders.Count > 0) && (Curvextruders[0].Count > 3))
            {
                xyzArray A = new xyzArray();
                for (int j = 0; j < Curvextruders[0].Count; j++)
                    A.Add(Curvextruders[0][j].Value(0, UpDown));
                xyz N = A.normal();
                Base = Base.DoComplete(Curvextruders[0][0].Value(0, UpDown), N);
                for (int i = 0; i < Curvextruders.Count; i++)
                {
                    xyArray _A = new xyArray();
                    Result.Add(_A);
                    for (int j = 0; j < Curvextruders[i].Count; j++)
                    {
                        xy P = Base.Relativ(Curvextruders[i][j].Value(0, UpDown)).toXY();
                        _A.Add(P);
                    }
                    if (_A.Count>0)
                    _A.Add(_A[0]);
                }
            }
            return Result;
        }
        /// <summary>
        /// paints the upper plane. You can override this metod e.g to gives the uppPlane a materal.
        /// <br/>
        /// f.e:<br/>
        /// class MyPolyExtruder:PolyCurveExtruder<br/> 
        ///{<br/>
        ///protected override void DrawUpPlane(OpenGlDevice Device)<br/>
        ///{<br/>
        ///Material Save = Device.Material<br/>
        ///Device.Material = Materials.Gold;<br/>
        ///    base.DrawUpPlane(Device);<br/>
        ///Device.Material = Save;<br/>
        ///}
        ///}
        /// </summary>
        /// <param name="Device">the device in wich will drawn.</param>
        protected virtual void DrawUpPlane(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(UpMatrix);
            Device.drawPolyPolyLine(UpLoxy);
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {
                if (Selector.StoredSnapItems.Count > 0)
                {
                    SnappItem SI = Selector.StoredSnapItems[Selector.StoredSnapItems.Count - 1];
                   
                    if (SI != null)
                    { SI.OfObject = this; }
                }
            }
            Device.PopMatrix();
        }
        /// <summary>
        /// paints the upper plane. You can override this metod e.g to gives the uppPlane a materal. See also <see cref="DrawUpPlane(OpenGlDevice)"/> 
        /// </summary>
        /// <param name="Device">the device in wich will drawn.</param>
        protected virtual void DrawDownPlane(OpenGlDevice Device)
        {
            Device.PushMatrix();
            Device.MulMatrix(DownMatrix);
            Device.drawPolyPolyLine(DownLoxy);
            Device.PopMatrix();
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {
                SnappItem SI = Selector.StoredSnapItems[Selector.StoredSnapItems.Count - 1];
       
                { SI.OfObject = this; }
            }
        }
        /// <summary>
        /// overrides the <see cref="CustomEntity.Draw"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            for (int i = 0; i < Curvextruders.Count; i++)
            {
                for (int j = 0; j < Curvextruders[i].Count; j++)
                {
                    Curvextruders[i][j].Paint(Device);
                    if (Device.RenderKind == RenderKind.SnapBuffer)
                    {
                        SnappItem SI = Selector.StoredSnapItems[Selector.StoredSnapItems.Count - 1];
                        SI.OfObject = this;
                    }
                   
                }
            }
          

            if (ShowUpPlane) DrawUpPlane(Device);

            if (ShowDownPlane) DrawDownPlane(Device);
            base.OnDraw(Device);
        }
    }
}
