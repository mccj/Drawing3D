using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drawing3d;
using System.Drawing;
namespace _CtrlRectMrkered
{
    public delegate void EventxyzHandler(object sender, LineType ViewLine, xyz Point);
    //public struct Drawing3d.PrimIdObj
    //{
    //    public short Object;
    //    short _PrimId;
    //    public short PrimId

    //    {
    //        get { return _PrimId; }
    //        set { _PrimId = value; }
    //    }



    //}
    [Serializable]
    public class Selector1
    {

        public OpenGlDevice Device;
        public List<SnapItem> SnapList = new List<SnapItem>();
        FBO _SnapBuffer = null;
        public void newSize()
        {
            if (_SnapBuffer != null) _SnapBuffer.Dispose();
            _SnapBuffer = null;
        }
        public FBO SnapBuffer
        {
            get
            {
                if (_SnapBuffer == null)

                {
                    _SnapBuffer = new FBO();

                    _SnapBuffer.BackGround = Color.FromArgb(0, 0, 0, 0);

                    _SnapBuffer.Init((int)Device.ViewPort.Width, (int)Device.ViewPort.Height);


                }
                return _SnapBuffer;



            }
        }

        GLShader _PickingShader = null;
        public GLShader PickingShader
        {
            set
            { _PickingShader = value; }
            get { return _PickingShader; }
        }


        public void SetObjectNumber(int ObjectId)
        {
            //if (Device.Shader == PickingShader)
            //    GL.Uniform1(CurrentObjectIndex, ObjectId);


        }
        Drawing3d.PrimIdObj[,] ReadPickingBuffer(int x, int y, int w, int h)
        {





            uint[,] Pixels = new uint[w, h];
            Drawing3d.PrimIdObj[,] Result = new Drawing3d.PrimIdObj[w, h];
            ShaderKind Save = Device.ShaderKind;
            Device.ShaderKind = ShaderKind.SmallShader;
            SnapBuffer.ReadPixel(x, y, Pixels);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {

                    Result[i, j].Object = (short)(Pixels[i, j] & 0XFFFF);
                    if (Result[i, j].Object != 0)
                    {

                    }
                    Result[i, j].PrimId = (short)(Pixels[i, j] >> 16);
                    if (Result[i, j].PrimId != 0)
                    {

                    }
                }
            Device.ShaderKind = Save;
            return Result;
        }
        public enum Snap { none, Static };
        Snap _SnapKind = Snap.Static;
        public Snap SnapKind
        {
            get { return _SnapKind; }
            set
            {
                _SnapKind = value;
                //Device.OutLookChanged();
            }
        }
        Drawing3d.PrimIdObj[,] _ObjectBuffer;
       public Drawing3d.PrimIdObj[,] ObjectBuffer
        {
            get { return _ObjectBuffer; }
            set
            {
                _ObjectBuffer = value;

                for (int i = 0; i < _ObjectBuffer.GetLength(0); i++)
                {
                    for (int j = 0; j < _ObjectBuffer.GetLength(1); j++)
                    {
                        if (_ObjectBuffer[i, j].Object != 0)
                        { }
                    }
                }

            }
        }
        static bool _SnapBufferDirty = true;
        public static bool SnapBufferDirty
        {
            get { return _SnapBufferDirty; }
            set
            {

                _SnapBufferDirty = value;
            }
        }
        void CompileDraw(OpenGlDevice Device)
        {
            Device.OnPaint();
        }
        public bool CreateSnap = false;
        MeshContainer CompileSnapBuffer()
        {
            CreateSnap = true;
            RenderKind Save = Device.RenderKind;
            StoredSnapItems.Clear();
            StoredSnapItems.Add(null);
            RegisterLocked = false;
            Device.RenderKind = RenderKind.SnapBuffer;
            MeshContainer Result = MeshCreator.CreateMeshes(Device, CompileDraw);
            Device.RenderKind = Save;
            RegisterLocked = true;
            CreateSnap = false;
            return Result;
        }
        public bool FastRefresh = false;
        static MeshContainer _SnapMesh = null;
        public static MeshContainer SnapMesh
        {
            get { return _SnapMesh; }
            set { _SnapMesh = value; }
        }

        //public void ReorgSnapBuffer()
        //{
        //    if (SnapMesh != null)
        //        for (int i = 0; i < SnapMesh.Progs.Count; i++)
        //        {
        //            Mesh M = SnapMesh.Progs[i] as Mesh;
        //            if (M != null)
        //            {
        //                SnapItem S = M.SnapItem;
        //                M.ObjectNumber = StoredSnapItems.IndexOf(S);
        //            }
        //            SnapBufferDirty = true;
        //        }
        //}
        public void RefreshSnapBuffer()
        {

            if (!Device.SnapEnable) return;
            //    if ((SnapKind == Snap.Static) && (SnapMesh == null))
            {
                SnapMesh = CompileSnapBuffer();

            }


            //GL.Disable(EnableCap.Blend);
            //if (PickingShader == null)
            //{
            //    PickingShader = new GLShader(PickingFragmentShader, PickingVertexShader);
            //    PickingShader.CompileAndLink();

            //    OpenGlDevice.CheckError();


            //}

            CurrentObjectIndex = PickingShader.GetVarHandle("TheObject");



            GLShader Save = Device.Shader;
            Device.Shader = PickingShader;
            RenderKind SaveRenderKind = Device.RenderKind;
            Device.RenderKind = RenderKind.SnapBuffer;
            if (!RegisterLocked)
            {
                StoredSnapItems.Clear();
                StoredSnapItems.Add(null);
            }


            SnapBuffer.Init((int)Device.ViewPort.Width, (int)Device.ViewPort.Height);

            SnapBuffer.EnableWriting();

            if (SnapMesh != null)
            {
                Device.Selector.FastRefresh = true;
                System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();
                SW.Start();
                SnapMesh.Paint(Device);
                SW.Stop();
                Device.Selector.FastRefresh = false;

            }
            else
                Device.OnPaint();
            SnapBuffer.DisableWriting();

            Device.RenderKind = SaveRenderKind;
            SnapBufferDirty = false;
            ObjectBuffer = ReadPickingBuffer(0, 0, (int)Device.ViewPort.Width, (int)Device.ViewPort.Height);
           // GL.Enable(EnableCap.Blend);
            Device.Shader = Save;
            CreateSnapList();
        }
        Drawing3d.PrimIdObj[,] SubArray(int x, int y, int Width, int Height)
        {


            Drawing3d.PrimIdObj[,] Result = new Drawing3d.PrimIdObj[Width, Height];
            if (ObjectBuffer == null) return Result;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if ((i + x >= 0) && (j + y >= 0) && (i + x < ObjectBuffer.GetLength(0)) && (j + y < ObjectBuffer.GetLength(1)))
                        Result[i, j] = ObjectBuffer[i + x, j + y];


                }

            return Result;
        }

        public static List<SnapItem> StoredSnapItems = new List<SnapItem>();
        public int IndexOfTag(object Tag)
        {
            for (int i = 0; i < SnapList.Count; i++)
            {
                if (SnapList[i].Tag.Equals(Tag))
                    return i;
            }
            return -1;
        }
        public int IndexOfEntity(Entity Entity)
        {
            for (int i = 0; i < SnapList.Count; i++)
            {
                if (SnapList[i].Object == Entity)
                    return i;
            }
            return -1;
        }

        public uint CurrentObject
        {
            get
            {

                return (uint)StoredSnapItems.Count - 1;
            }


        }





        public void ToSnapBuffer(uint PrimId)
        {

            if (!Entity.Compiling)
            {
                if (!FastRefresh)
                    SetObjectNumber((int)((PrimId << 16) + CurrentObject));
                else
                {
                    Mesh M = Entity.CurrentEntity as Mesh;
                    if (M != null)
                    {
                        SetObjectNumber((int)((PrimId << 16) + M.ObjectNumber));
                    }
                }

            }
            //else
            //    MeshCreator.MeshCurrent.ObjectNumber = (int)((PrimId << 16) + CurrentObject);

        }
        int CurrentObjectIndex = 2;
        public SnapItem CurrentSnapItem = null;
        public bool IsRegisteredSnapItem
        {
            get { return CurrentSnapItem != null; }
        }
        public void UnRegisterSnapItem(object Handle)
        {
            if ((Handle != CurrentSnapItem) || (CurrentSnapItem == null)) return;
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {


            }
           // if (Entity.Compiling) MeshCreator.Renew();
            CurrentSnapItem = null;
        }
        public bool RegisterLocked = false;
        public object RegisterSnapItem(SnapItem Item)
        {
            if (Item == null) return null;
            if (RegisterLocked) return null;
            if ((Device.RenderKind != RenderKind.SnapBuffer)
                && (!Entity.Compiling))

                return null;
            if (CurrentSnapItem == null)
            {
                OpenGlDevice.CheckError();
                CurrentSnapItem = Item;
                if (!Item.Udated)
                {
                    Item.Device = Device;
                    Item.ModelMatrix = Device.ModelMatrix;
                    if (Item.PolygonMode != PolygonMode.Point)
                        Item.PolygonMode = Device.PolygonMode;
                    Item.Object = Entity.CurrentEntity;
                    //if ((Item.Object != null) && (Item.Object is Entity))
                    //    (Item.Object as Entity).MySnapItem = Item;
                    Item.Udated = true;
                }
                else
                { }
                if (Tags.Count > 0)
                    Item.Tag = Tags.Peek();

                StoredSnapItems.Add(Item);

                GLShader S = Device.Shader;
                if (Entity.Compiling)
                {
                    //MeshCreator.SnapItem = Item;
                    //if (MeshCreator.MeshCurrent != null)
                    //    MeshCreator.MeshCurrent.ObjectNumber = (int)CurrentObject;

                }
                OpenGlDevice.CheckError();
                if (Device.Shader == PickingShader)
                    S.SetVar(CurrentObjectIndex, (int)CurrentObject);
                OpenGlDevice.CheckError();
                return Item;
            }
            return null;
        }
        internal System.Collections.Stack Tags = new System.Collections.Stack();
        internal int SnapDistance = 4;
        List<Drawing3d.PrimIdObj> Intermediate = new List<Drawing3d.PrimIdObj>();
        int IndexOf(Drawing3d.PrimIdObj R)
        {
            for (int i = 0; i < Intermediate.Count; i++)
            {


                if ((Intermediate[i].Object == R.Object))
                    return i;
            }
            return -1;
        }
        public bool IntersectionUsed = false;
      public  _CtrlRectMrkered.SnapItem CopySI(Drawing3d.SnapItem SI)
        {
            _CtrlRectMrkered.SnapItem Result = new SnapItem();
            Result.TriangleInfo = CopyTr(SI.TriangleInfo);
            return Result;
        }
        public _CtrlRectMrkered.TriangleInfo CopyTr(Drawing3d.TriangleInfo TI)
        {
            _CtrlRectMrkered.TriangleInfo Result = new _CtrlRectMrkered.TriangleInfo();
            Result.Indices = TI.Indices;
            Result.Points = TI.Points;
            return Result;
        }
        List<TriangleF> GetTriangles(Drawing3d.PrimIdObj[,] P, int Object)
        {
            List<TriangleF> Result = new List<TriangleF>();
            List<PrimIdObj> PrimIds = new List<PrimIdObj>();
          
            SnapItem SI =CopySI( Device.Selector.SnapList[Object]);
            Matrix M = SI.ModelMatrix;
            for (int i = 0; i < P.GetLength(0); i++)
            {
                for (int j = 0; j < P.GetLength(1); j++)
                {
                    if (P[i, j].Object >= 0)
                    {
                        int ll = P[i, j].Object;
                    }
                    if (P[i, j].Object == 2)
                    {

                    }
                    if ((PrimIds.IndexOf(P[i, j]) < 0))
                    {


                        if ((P[i, j].Object == Object) && (SI.TriangleInfo != null))
                        {
                            TriangleF T = new TriangleF(SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId * 3]],
                                                      SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId * 3 + 1]],
                                                      SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId * 3 + 2]]);

                            {
                                PrimIds.Add(P[i, j]);
                                Result.Add(T);
                            }


                        }
                    }

                }
            }
            return Result;
        }
        void ReadKonzentrisch(OpenGlDevice Device, Drawing3d.PrimIdObj[,] P, int Centeri, int Centerj, int radius)
        {
            for (int i = 0; i < P.GetLength(0); i++)
            {
                for (int j = 0; j < P.GetLength(1); j++)
                {
                    if (P[i, j].Object != 0)
                    { }
                }
            }
            if (radius == 0)
            {

                if ((int)P[Centeri, Centerj].Object == 0) return;
                Intermediate.Add(P[Centeri, Centerj]);
                return;

            }
            for (int i = Centeri - radius; i < Centeri + radius; i++)
            {

                if ((int)P[i, Centerj - radius].Object == 0)
                    continue;
                if (IndexOf(P[i, Centerj - radius]) < 0)
                    Intermediate.Add(P[i, Centerj - radius]);

            }

            for (int j = Centerj - radius; j < Centerj + radius; j++)
            {
                if ((int)P[Centeri + radius, j].Object == 0) continue;

                if (IndexOf(P[Centeri + radius, j]) < 0)
                    Intermediate.Add(P[Centeri + radius, j]);

            }
            for (int i = Centeri + radius; i > Centeri - radius; i--)
            {
                if ((int)P[i, Centerj + radius].Object == 0) continue;
                if (IndexOf(P[i, Centerj + radius]) < 0)
                    Intermediate.Add(P[i, Centerj + radius]);

            }
            for (int j = Centerj + radius; j > Centerj - radius; j--)
            {
                if ((int)P[Centeri - radius, j].Object == 0) continue;

                if (IndexOf(P[Centeri - radius, j]) < 0)
                    Intermediate.Add(P[Centeri - radius, j]);

            }

        }

        public event EventxyzHandler SetCurrent = null;
        virtual protected void Setxyz(LineType ViewLine, xyz Point)
        {
            if (SetCurrent != null)
            {
                SetCurrent(Device, ViewLine, Point);

            }

            Device.Currentxyz = Point;
        }
        internal LineType VL;
        public static bool Touched = false;
        public void CreateSnapList()
        {




            if (!Device.SnapEnable) return;
            Device.Emission = Color.Black;

            try
            {

                {


                    SnapList.Clear();
                    LineType ViewLine = Device.FromScr(Device.MousePos);
                    VL = ViewLine;
                    int siz = SnapDistance * 2 + 1;

                    Drawing3d.PrimIdObj[,] P = SubArray(Device.MousePos.X - siz / 2, (int)Device.ViewPort.Height - Device.MousePos.Y - siz / 2, siz, siz);


                    Intermediate.Clear();

                    for (int i = 0; i <= siz / 2; i++)
                        ReadKonzentrisch(Device, P, siz / 2, siz / 2, i);
                    SnapItem[] BB = new SnapItem[Intermediate.Count];

                    SnapItem.Snapdist = (double)SnapDistance / Device.PixelsPerUnit / Device.Camera.ZoomFactor;
                    for (int i = 0; i < Intermediate.Count; i++)
                    {
                        if (Intermediate.Count > 0)
                        { }
                        SnapItem SI = null;
                        int Index = Intermediate[i].Object;
                        if (Index < 0) continue;
                        if (StoredSnapItems.Count > Index)
                            SI = StoredSnapItems[Index];
                        if (SI == null) continue;
                        if ((((SI != null) && (i == 0)) || (SI.PolygonMode == PolygonMode.Line)) ||
                            ((SI != null) && (i == 1) && (SI.PolygonMode == PolygonMode.Line)) ||
                            (((SI != null) && (i == 1)) && (((SnapList.Count > 0) && (SnapList[0].OfObject == null)) || (SI.OfObject == null) || (SnapList[0].OfObject != SI.OfObject))))
                        {

                            SI.PrimId = (short)Intermediate[i].PrimId;
                            Matrix ToLocal = SI.ModelMatrix.invert();
                            LineType L = ViewLine * ToLocal;
                            SI.Crossed = false;
                            SI.doExchange = false;
                            SI.Device = Device;
                            SI.Point = SI.ModelMatrix * SI.Cross(L);
                            SI.PtCrossed = false;
                            double lam = -1;
                            xyz Pt = new xyz(0, 0, 0);
                            ViewLine.Distance(SI.Point, out lam, out Pt);
                            SI.Depth = ViewLine.Q.dist(Pt);
                            SI.Depth = lam;
                            int Id = 0;


                            for (int k = 0; k < SnapList.Count; k++)
                            {

                                if (SI is PointSnapItem) { Id = 0; break; };
                                if (SnapList[k].Depth > SI.Depth)
                                { Id = k; break; }
                                Id = k + 1;
                            }

                            SnapList.Insert(Id, SI);
                        }

                    }
                    double dummy = -1;
                    xyz _P = new xyz(0, 0, 0);
                    if (SnapList.Count >= 2)

                        if (SnapList[1].doExchange)
                            if ((ViewLine.Distance(SnapList[1].Point, out dummy, out _P) < ViewLine.Distance(SnapList[0].Point, out dummy, out _P)))
                            {
                                SnapItem Ex = SnapList[0];
                                SnapList[0] = SnapList[1];
                                SnapList[1] = Ex;
                            }

                    CheckIntersect(ViewLine);
                    if (SnapList.Count > 0)
                    {



                    }
                    if (SnapList.Count > 0)
                        Setxyz(ViewLine, SnapList[0].Point);
                    else
                    {

                        double lam;
                        Base B = Device.getProjectionBase();
                        Plane pl = new Plane(Device.Camera.Anchor, B.BaseZ);
                        xyz Pt;
                        pl.Cross(ViewLine, out lam, out Pt);
                        Setxyz(ViewLine, Pt);
                    }


                }
            }
            finally
            {

            }


        }
        void IntersectLines(List<xyzArray> Arrays, List<SnapItem> Snap)
        {
            xyzArray A = Arrays[0];
            xyzArray B = Arrays[1];
            List<CrossTag1> L = CrossTag1.GetCrossList(A, B);
            for (int i = 0; i < L.Count; i++)
            {
                if (A.Value(L[i].Lam1).dist(Snap[0].Point) <= SnapItem.Snapdist)
                {

                    Snap[0].Point = A.Value(L[i].Lam1);
                    Snap[1].Point = Snap[0].Point;
                    Snap[0].Crossed = true;
                    Snap[1].Crossed = true;
                    break;
                }
            }
        }
        bool IntersectLines(xyzArray A, xyzArray B, xyz NearPoint, ref xyz CrossPoint)
        {

            List<CrossTag1> L = CrossTag1.GetCrossList(A, B);
            for (int i = 0; i < L.Count; i++)
            {
                if (A.Value(L[i].Lam1).dist(NearPoint) <= SnapItem.Snapdist)
                {
                    CrossPoint = A.Value(L[i].Lam1);

                    return true;
                }
            }
            return false;
        }
        public List<Line3D> Cross(List<TriangleF> LL1, List<TriangleF> LL2, LineType ViewLine)
        {
            List<Line3D> Lines = new List<Line3D>();
            if ((LL1 != null) && (LL2 != null))
            {

                for (int i = 0; i < LL1.Count; i++)
                {

                    for (int j = 0; j < LL2.Count; j++)
                    {
                        xyzf Pt1 = new xyzf(0, 0, 0);
                        xyzf Pt2 = new xyzf(0, 0, 0);


                        if (TriangleF.Cross(LL1[i], LL2[j], ref Pt1, ref Pt2))
                            Lines.Add(new Line3D(Pt1.Toxyz(), Pt2.Toxyz()));
                    }
                }
            }
            return Lines;
        }
        public void SnapInside(RectangleF Rectangle)
        {
            if (!Device.SnapEnable) return;
            if ((Rectangle.Width < 3) && (Rectangle.Height < 3))
                return;

            xyz A = new xyz(Rectangle.X, Rectangle.Y, 0);
            xyz B = new xyz(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height, 0);
            Matrix M = Device.Camera.Base.ToMatrix();
            A = Device.ProjectionMatrix * M * A;
            B = Device.ProjectionMatrix * M * B;
            SnapList.Clear();
            for (int i = 1; i < StoredSnapItems.Count; i++)
            {
                if (StoredSnapItems[i].TriangleInfo != null)
                    for (int j = 0; j < StoredSnapItems[i].TriangleInfo.Points.Length; j++)
                    {
                        xyz P1 = new xyz(StoredSnapItems[i].TriangleInfo.Points[j].x, StoredSnapItems[i].TriangleInfo.Points[j].y, StoredSnapItems[i].TriangleInfo.Points[j].z);
                        xyz P2 = Device.ProjectionMatrix * StoredSnapItems[i].ModelMatrix * P1;

                        {
                            if ((P2.x >= A.x) && (P2.x <= B.x) && (P2.y >= A.Y) && (P2.Y <= B.Y))
                            {
                                SnapList.Add(StoredSnapItems[i]);
                                break;
                            }


                        }

                    }
            }
        }
        public bool Cross(List<TriangleF> Triangles, xyzArray Poly, ref double Lam, ref xyz Pt)
        {
            for (int j = 0; j < Triangles.Count; j++)
            {
                Plane P = new Plane(Triangles[j].A.Toxyz(), (Triangles[j].B - Triangles[j].A).Toxyz() & (Triangles[j].C - Triangles[j].A).Toxyz());
                for (int i = 0; i < Poly.Count - 1; i++)
                {
                    Lam = -1;
                    xyz Pkt = new xyz(0, 0, 0);

                    if (P.Cross(new LineType(Poly[i], (Poly[i + 1] - Poly[i])), out Lam, out Pkt))
                    {
                        if ((-0.000001 <= Lam) && (Lam <= 1.00000001))
                            if (Triangles[j].Inside(Pkt.toXYZF()))
                            {
                                Pt = Pkt;
                                Lam += i;
                                return true;
                            }
                    }
                }
            }
            return false;
        }

        public static List<TriangleF> ListOfTriangles1 = new List<TriangleF>();

        public static List<TriangleF> ListOfTriangles2 = new List<TriangleF>();
        public bool IntersectPlaneWithPlane(SnapItem Plane1, SnapItem Plane2, LineType ViewLine)
        {
            int Object1 =StoredSnapItems.IndexOf(Plane1);
            int Object2 = StoredSnapItems.IndexOf(Plane2);
            int siz = 2 * SnapDistance + 1;

            PrimIdObj[,] P = SubArray(Device.MousePos.X - siz / 2, (int)Device.ViewPort.Height - Device.MousePos.Y - siz / 2, siz, siz);
            ListOfTriangles1 = GetTriangles(P, Object1);
            ListOfTriangles1.Clear();
            Matrix M = Plane1.ModelMatrix;
            for (int i = 0; i < Plane1.TriangleInfo.Indices.Length; i++)
            {
                TriangleInfo TI = Plane1.TriangleInfo;
                TriangleF T = new TriangleF(M * TI.Points[TI.Indices[i]], M * TI.Points[TI.Indices[i + 1]], M * TI.Points[TI.Indices[i + 2]]);
                i = i + 2;
                //   if (T.Inside(ViewLine.P.toXYZF()))
                ListOfTriangles1.Add(T);
            }
            for (int i = 0; i < ListOfTriangles1.Count; i++)
            {
                if (ListOfTriangles1[i].Inside(ViewLine.P.toXYZF()))
                {
                    TriangleF F = ListOfTriangles1[i];
                    ListOfTriangles1.RemoveAt(i);
                    ListOfTriangles1.Insert(0, F);
                }
            }
            ListOfTriangles2 = GetTriangles(P, Object2);
            ListOfTriangles2.Clear();
            M = Plane2.ModelMatrix;
            for (int i = 0; i < Plane2.TriangleInfo.Indices.Length; i++)
            {
                TriangleInfo TI = Plane2.TriangleInfo;
                TriangleF T = new TriangleF(M * TI.Points[TI.Indices[i]], M * TI.Points[TI.Indices[i + 1]], M * TI.Points[TI.Indices[i + 2]]);
                i = i + 2;
                //  if (T.Inside(ViewLine.P.toXYZF()))
                ListOfTriangles2.Add(T);
            }
            for (int i = 0; i < ListOfTriangles2.Count; i++)
            {
                if (ListOfTriangles2[i].Inside(ViewLine.P.toXYZF()))
                {
                    TriangleF F = ListOfTriangles2[i];
                    ListOfTriangles2.RemoveAt(i);
                    ListOfTriangles2.Insert(0, F);
                }
            }
            List<Line3D> Lines = Cross(ListOfTriangles1, ListOfTriangles2, ViewLine);
            xyz P1 = new xyz(0, 0, 0);
            xyz P2 = new xyz(0, 0, 0);
            if (Lines.Count < 2)
            {
                double Lam = -1;
                double Mue = -1;
                List<TriangleF> L1 = new List<TriangleF>();
                List<TriangleF> L2 = new List<TriangleF>();
                for (int i = 0; i < ListOfTriangles1.Count; i++)
                {
                    if (ListOfTriangles1[i].Inside(ViewLine.P.toXYZF()))
                    {
                        L1.Add(ListOfTriangles1[i]);
                    }
                }
                for (int i = 0; i < ListOfTriangles2.Count; i++)
                {
                    if (ListOfTriangles2[i].Inside(ViewLine.P.toXYZF()))
                    {
                        L2.Add(ListOfTriangles2[i]);
                    }
                }
                L1 = ListOfTriangles1;
                L2 = ListOfTriangles2;
                for (int i = 0; i < L1.Count; i++)
                {
                    for (int j = 0; j < L2.Count; j++)
                    {
                        if (TriangleF.Cross(L1[i], L2[j], out Lam, out Mue, out P1, out P2))
                        {

                            int _lam = (int)Lam;
                            switch (_lam)
                            {
                                case 0:
                                    Lines.Add(new Line3D(L1[i].A.Toxyz(), L1[i].B.Toxyz()));
                                    break;
                                case 1:
                                    Lines.Add(new Line3D(L1[i].B.Toxyz(), L1[i].C.Toxyz()));
                                    break;
                                case 2:
                                    Lines.Add(new Line3D(L1[i].C.Toxyz(), L1[i].A.Toxyz()));
                                    break;
                                default:
                                    break;
                            }
                            int _mue = (int)Mue;
                            switch (_mue)
                            {
                                case 0:
                                    Lines.Add(new Line3D(L2[j].A.Toxyz(), L2[j].B.Toxyz()));
                                    break;
                                case 1:
                                    Lines.Add(new Line3D(L2[j].B.Toxyz(), L2[j].C.Toxyz()));
                                    break;
                                case 2:
                                    Lines.Add(new Line3D(L2[j].C.Toxyz(), L2[j].A.Toxyz()));
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;


                    }
                }
                //if (TriangleF.Cross(ListOfTriangles1[0], ListOfTriangles2[0], out Lam, out Mue, out P1, out P2))
                //{
                //    int _lam = (int)Lam;
                //    switch (_lam)
                //    {case  0: Lines.Add(new Line3D(ListOfTriangles1[0].A.Toxyz(),ListOfTriangles1[0].B.Toxyz()));
                //           break;
                //        case 1: Lines.Add(new Line3D(ListOfTriangles1[0].B.Toxyz(), ListOfTriangles1[0].C.Toxyz()));
                //            break;
                //        case 2: Lines.Add(new Line3D(ListOfTriangles1[0].C.Toxyz(), ListOfTriangles1[0].A.Toxyz()));
                //            break;
                //        default:
                //            break;
                //    }
                //    int _mue = (int)Mue;
                //    switch (_mue)
                //    {
                //        case 0:
                //            Lines.Add(new Line3D(ListOfTriangles2[0].A.Toxyz(), ListOfTriangles2[0].B.Toxyz()));
                //            break;
                //        case 1: Lines.Add(new Line3D(ListOfTriangles2[0].B.Toxyz(), ListOfTriangles2[0].C.Toxyz()));
                //            break;
                //        case 2: Lines.Add(new Line3D(ListOfTriangles2[0].C.Toxyz(), ListOfTriangles2[0].A.Toxyz()));
                //            break;
                //        default:
                //            break;
                //    }
                //}
            }

            double di = 1e10;
            xyz Pos = new xyz(0, 0, 0);
            int Id = -1;
            for (int i = 0; i < Lines.Count; i++)
            {
                LineType L = new LineType(Lines[i].A, Lines[i].B - Lines[i].A);
                double Lam = -1;

                double d = ViewLine.Distance(L, SnapItem.Snapdist, false, false, out Lam);
                xyz Nearest = L.Value(Lam);
                if ((Lam >= -0.00001) && (Lam <= 1.00001))
                    if (d < di)
                    {
                        Pos = Nearest;
                        di = d;
                        Id = i;
                    }
            }
            if ((di < 1e10) && (di <= SnapItem.Snapdist))
            {


                Plane1.Crossed = true;
                Plane1.Point = Pos;
                Plane2.Crossed = true;
                Plane2.Point = Pos;
                return true;
            }

            xyzArray A = Plane1.getxyzArray();
            xyzArray B = Plane2.getxyzArray();
            if ((A != null) && (B != null))
                return IntersectLineWithLine(Plane1, Plane2, ViewLine);





            return false;
        }
        bool intersectPlaneWithLine(SnapItem Plane, SnapItem Line, xyzArray LineArray, ref double Lam, ref xyz Result)
        {
            TriangleF T = Plane.GetTriangle();
            if (T == null) return false;
            xyz N1 = ((T.B - T.A) & (T.C - T.A)).Toxyz();
            if (LineArray == null)
            { }
            xyz N2 = LineArray.cross();
            if ((N1 & N2).length() < 0.00001)   // Parallel
            {
                xyzArray A = Plane.getxyzArray();
                if (A != null)
                {
                    xyzArray B = LineArray;
                    List<CrossTag1> L = CrossTag1.GetCrossList(A, B);

                    for (int i = 0; i < L.Count; i++)
                    {
                        if (A.Value(L[i].Lam1).dist(Plane.Point) <= SnapItem.Snapdist)
                        {
                            Lam = L[i].Lam1;

                            Result = A.Value(L[i].Lam1);
                            return true;
                        }
                    }
                }
            }

            int Object = StoredSnapItems.IndexOf(Plane);
            int siz = SnapDistance * 2 + 1;
            //  int siz = SnapDistance;
            Drawing3d.PrimIdObj[,] P = SubArray(Device.MousePos.X - siz / 2, (int)Device.ViewPort.Height - Device.MousePos.Y - siz / 2, siz, siz);
            List<TriangleF> LT = GetTriangles(P, Object);
            Lam = -1;



            bool R = Cross(LT, LineArray, ref Lam, ref Result);
            return R;

        }
        bool IntersectLineWithLine(SnapItem Line1, SnapItem Line2, LineType ViewLine)
        {
            xyzArray A = Line1.getxyzArray();
            int h = A.Count;
            xyzArray B = Line2.getxyzArray();
            double d1 = 1e10;
            int i1 = -1;
            double d2 = 1e10;
            int i2 = -1;
            for (int i = 0; i < A.Count; i++)
            {
                double d = Line1.Point.dist(A[i]);
                if (d < d1)
                {
                    d1 = d;
                    i1 = i;
                }

            }
            for (int i = 0; i < B.Count; i++)
            {
                double d = Line1.Point.dist(B[i]);
                if (d < d2)
                {
                    d2 = d;
                    i2 = i;
                }

            }
            if ((A == null) || (B == null)) return false;
            xyz CrossPoint = new xyz(0, 0, 0);
            if (IntersectLines(A, B, Line1.Point, ref CrossPoint))
            {
                Line1.Point = CrossPoint;
                Line2.Point = CrossPoint;
                Line1.Crossed = true;
                Line2.Crossed = true;
                return true;
            }

            return false;

        }
        void IntersectPlaneWithLine(SnapItem Plane, SnapItem Line, LineType ViewLine)
        {
            double Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            if (intersectPlaneWithLine(Plane, Line, Line.getxyzArray(), ref Lam, ref Result))
            {
                if (Result.dist(Plane.Point) <= SnapItem.Snapdist)
                {

                    Plane.Point = Result;
                    Line.Point = Plane.Point;
                    Line.Crossed = true;
                    Plane.Crossed = true;
                }
            }





        }
        
        public void CheckIntersect(LineType ViewLine)
        {  
            if ( SnapList.Count < 2) return;
            if ((SnapList[0].PolygonMode == PolygonMode.Line) && (SnapList[1].PolygonMode == PolygonMode.Line))
            {
                IntersectLineWithLine(SnapList[0], SnapList[1], ViewLine);
                return;
            }

            if ((Device.Selector.SnapList[0].PolygonMode == PolygonMode.Fill) && (Device.Selector.SnapList[1].PolygonMode == PolygonMode.Line))
            {
                IntersectPlaneWithLine(SnapList[0], SnapList[1], ViewLine);
                return;
            }
            if ((Device.Selector.SnapList[0].PolygonMode == PolygonMode.Line) && (Device.Selector.SnapList[1].PolygonMode == PolygonMode.Fill))
            {
                IntersectPlaneWithLine(SnapList[1], SnapList[0], ViewLine);
                return;
            }
            if ((Device.Selector.SnapList[0].PolygonMode == PolygonMode.Fill) && (SnapList[1].PolygonMode == PolygonMode.Fill))
            {
                IntersectPlaneWithPlane(SnapList[0], SnapList[1], ViewLine);
                return;
            }


        }
    }
    [Serializable]
   public class CrossTag1
    {
        static bool Cross(LineType L1, LineType L2, ref double Lam1, ref double Lam2)
        {
            xyz PQ = L2.P - L1.P;
            double PQV = PQ * L1.Direction;
            double PQW = PQ * L2.Direction;
            double vv = L1.Direction * L1.Direction;
            double ww = L2.Direction * L2.Direction;
            double vw = L2.Direction * L1.Direction;
            double Det = vv * ww - vw * vw;
            if (Det == 0)// parallele
            {
                return false;
            }
            else
            {
                Lam1 = (PQV * ww - PQW * vw) / Det;
                Lam2 = -(PQW * vv - PQV * vw) / Det;

                double d = PQ * (L2.Direction & L1.Direction).normalized();

                return (System.Math.Abs(d) < 0.0000001) && (0 <= Lam1) && (Lam1 <= 1) && (0 <= Lam2) && (Lam2 <= 1);
            }

        }
        internal static List<CrossTag1> GetCrossList(xyzArray A, xyzArray B)
        {
            List<CrossTag1> Result = new List<CrossTag1>();
            for (int i = 0; i < A.Count - 1; i++)
            {

                LineType L1 = new LineType(A[i], A[i + 1] - A[i]);
                for (int j = 0; j < B.Count - 1; j++)
                {
                    LineType L2 = new LineType(B[j], B[j + 1] - B[j]);
                    double Lam1 = -1;
                    double Lam2 = -1;
                    if (Cross(L1, L2, ref Lam1, ref Lam2))
                    {
                        Result.Add(new CrossTag1(i + Lam1, j + Lam2));
                    }
                }
            }
            return Result;
        }
        internal static List<CrossTag1> GetCrossList1(xyzArray A, xyzArray B)
        {
            List<CrossTag1> Result = new List<CrossTag1>();
            for (int i = 0; i < A.Count - 1; i++)
            {
                LineType L1 = new LineType(A[i], A[i + 1] - A[i]);
                for (int j = 0; j < B.Count - 1; j++)
                {
                    LineType L2 = new LineType(B[j], B[j + 1] - B[j]);
                    double Lam1 = -1;
                    double Lam2 = -1;
                    if (Cross(L1, L2, ref Lam1, ref Lam2))
                    {
                        Result.Add(new CrossTag1(i + Lam1, j + Lam2));
                    }
                }
            }
            return Result;
        }
        public CrossTag1(double Lam1, double Lam2)
        {
            this.Lam1 = Lam1;
            this.Lam2 = Lam2;
        }
        public double Lam1 = -1;
        public double Lam2 = -1;
    }
    /// <summary>
    /// This class represents a container for three points A, B, C
    /// </summary>
    [System.Serializable]
    public class TriangleF
    {
        /// <summary>
        /// Point of the Triangle
        /// </summary>
        public xyzf A;
        /// <summary>
        /// Point of the Triangle
        /// </summary>

        public xyzf B;
        /// <summary>
        ///  Point of the Triangle
        /// </summary>

        public xyzf C;

        /// <summary>
        /// The constructor initializes the class with the values A, B, C
        /// </summary>
        /// <param name="A">1. point</param>
        /// <param name="B">2. point</param>
        /// <param name="C">3. point</param>
        public TriangleF(xyzf A, xyzf B, xyzf C)
        {
            this.A = A;
            this.B = B;
            this.C = C;

        }
        public static bool Cross(TriangleF t1, TriangleF t2, out double Lam, out double Mue, out xyz P, out xyz Q)
        {
            xyzArray A = new xyzArray();
            xyzArray B = new xyzArray();
            A.data = new xyz[] { t1.A.Toxyz(), t1.B.Toxyz(), t1.C.Toxyz() };
            B.data = new xyz[] { t2.A.Toxyz(), t2.B.Toxyz(), t2.C.Toxyz() };
            Lam = -1;
            Mue = -1;
            P = new Drawing3d.xyz(0, 0, 0);
            Q = new Drawing3d.xyz(0, 0, 0);

            double d = (A.Distance(B, 1e10, out Lam, out Mue));
            if (d < 2)
            {
                P = A.Value(Lam);
                Q = B.Value(Mue);
                return true;

            }
            return false;
        }
        public bool Cross(LineType L, ref xyzf Pt, ref double Lam)
        {
            int PtCount = 0;
            Plane P1 = new Plane(A, B, C);
            xyz P = new xyz(0, 0, 0);
            Lam = -1;
            double Mue = -1;

            if (P1.Cross(L, out Lam, out P))
            {
                xyz PP = L.Value(Lam);
                Pt = new Drawing3d.xyzf((float)P.x, (float)P.y, (float)P.z);
                if (Inside(P.toXYZF()))
                {
                    Pt = P.toXYZF();
                    return true;
                }

            }
            return false;
        }
        public static bool Cross(TriangleF T1, TriangleF T2, ref xyzf Pt1, ref xyzf Pt2)
        {
            int PtCount = 0;
            Plane P1 = new Plane(T1.A, T1.B, T1.C);

            Check(P1, T1, ref PtCount, T2.A, T2.B, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;
            Check(P1, T1, ref PtCount, T2.B, T2.C, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;
            Check(P1, T1, ref PtCount, T2.C, T2.A, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;

            Plane P2 = new Plane(T2.A, T2.B, T2.C);

            Check(P2, T2, ref PtCount, T1.A, T1.B, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;
            Check(P2, T2, ref PtCount, T1.B, T1.C, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;
            Check(P2, T2, ref PtCount, T1.C, T1.A, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;

            return false;

        }
        static void Check(Plane P1, TriangleF T, ref int PtCount, xyzf A, xyzf B, ref xyzf Pt1, ref xyzf Pt2)
        {

            double Lam = -1;
            xyz Pt = new xyz(0, 0, 0);

            if (P1.Cross(new LineType(A.Toxyz(), (B.Toxyz() - A.Toxyz())), out Lam, out Pt))
            {
                xyz N = A.Toxyz() + (B.Toxyz() - A.Toxyz()) * Lam;

                if ((Lam >= -0.000000001) && (Lam < 1.000000001))
                    if (T.Inside(Pt.toXYZF()))
                    {
                        if (PtCount == 0)
                        {
                            Pt1 = Pt.toXYZF();
                            PtCount = 1;
                        }
                        else
                    if (PtCount == 1)
                            if (Pt1.dist(Pt.toXYZF()) > 0.0001)
                            {
                                Pt2 = Pt.toXYZF();
                                PtCount++;
                            }


                    }

            }
        }
        public bool Inside(xyzf _P)
        {

            xyzf N = ((B - A) & (C - A)).normalized();
            return (((((A - _P) & (B - _P)) * N > -0.01)
                  && (((B - _P) & (C - _P)) * N > -0.01)
                && (((C - _P) & (A - _P)) * N > -0.01)
                )
                ||
                ((((A - _P) & (B - _P)) * N < 0.01)
                  && (((B - _P) & (C - _P)) * N < 0.01)
                && (((C - _P) & (A - _P)) * N < 0.01)
                ))




                ;
        }


    }
}
