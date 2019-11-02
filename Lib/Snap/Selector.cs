using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// is the definition for a event, that happens when the <see cref="OpenGlDevice.Currentxyz"/> value is setted from
    /// the <see cref="Selector.Setxyz(LineType, xyz)"/>
    /// </summary>
    /// <param name="sender">the <see cref="Selector"/></param>
    /// <param name="ViewLine">the line from the mouse pointer to the eye.</param>
    /// <param name="Point">the coordinates, that will be setted to <see cref="OpenGlDevice.Currentxyz"/>.</param>
    public delegate void EventxyzHandler(object sender, LineType ViewLine, xyz Point);

    /// <summary>
    /// the <see cref="Selector.ObjectBuffer"/> consists of this structure.
    /// </summary>
    public struct PrimIdObj
    {   
    /// <summary>
    /// is the number of the index of the <see cref="SnappItem"/> in the <see cref="Selector.StoredSnapItems"/> list.
    /// </summary>
        public short Object;
        private short _PrimId;
        /// <summary>
        /// holds the id of a triangle, which is drawn by a graphical object.See also <see cref="Primitives3d.drawTriangles(OpenGlDevice, int[], xyzf[], xyzf[], xyf[], xyzf[])"/>.
        /// A <see cref="SnappItem"/> has a field <see cref="SnappItem.PrimId"/>, where the triangle is given, which is on the mouse position.
        /// </summary>
        public short PrimId
        {
            get { return _PrimId; }
            set { _PrimId = value; }
        }
   }
    /// <summary>
    /// is the central class for the snapp handling. See <see cref="SnappItem"/>. In the method <see cref="RefreshSnapBuffer"/> first the
    /// geometrie of the graphical objects will be produced and stored in in <see cref="SnapMesh"/>. It contains in the <see cref="MeshContainer.Progs"/> a list of <see cref="CompiledMesh"/>es. Every this entry has a field <see cref="CompiledMesh.SnapObject"/>, which is the index of the graphical
    /// object in the <see cref="StoredSnapItems"/> . This value together with the <see cref="PrimIdObj.PrimId"/> is given to the shader <see cref="PickingShader"/> as quasi color.
    /// He paints this to <see cref="SnapBuffer"/>, where you can the graphical object and the <see cref="PrimIdObj.PrimId"/>.
    /// Every <see cref="OpenGlDevice"/> contains a <see cref="Selector"/>.
    /// </summary>
    [Serializable]
    public partial class Selector
    {
        /// <summary>
        /// the <see cref="OpenGlDevice"/> which has the selector as field.
        /// </summary>
        public OpenGlDevice Device;
        /// <summary>
        /// this list contains all graphical objects, which are under the actual mouse position ordered by the depth.
        /// Its the same as <see cref="OpenGlDevice.SnappItems"/>.
        /// </summary>
        internal List<SnappItem> SnappList = new List<SnappItem>();
        FBO _SnapBuffer = null;
        /// <summary>
        /// is the buffer, which contains the <see cref="PrimIdObj"/>
        /// </summary>
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
        /// <summary>
        /// this <see cref="GLShader"/> is used to write the <see cref="PrimIdObj"/> to the <see cref="SnapBuffer"/>.
        /// </summary>
        public GLShader PickingShader
        {
            set
            { _PickingShader = value; }
            get { return _PickingShader; }
        }

        /// <summary>
        /// <see cref="CompiledMesh"/> set the object number <see cref="CompiledMesh.SnapObject"/> to a field, which is used from <see cref="SetPrimId(uint)"/>, which makes a <see cref="PrimIdObj"/> sendig to the <see cref="PickingShader"/>.
        /// </summary>
        /// <param name="ObjectId"></param>
        internal void SetObjectNumber(int ObjectId)
        {
            CurrentObject = (uint)ObjectId;

         }
        PrimIdObj[,] ReadPickingBuffer(int x, int y, int w, int h)
        {
            GLShader SaveShader = Device.Shader;
            uint[,] Pixels = new uint[w, h];
            PrimIdObj[,] Result = new PrimIdObj[w, h];
            SnapBuffer.ReadPixel(x, y, Pixels);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {

                    Result[i, j].Object = (short)(Pixels[i, j] & 0XFFFF);
                    Result[i, j].PrimId = (short)(Pixels[i, j] >> 16);
                }
            Device.Shader = SaveShader;
            return Result;
        }

      
        PrimIdObj[,] ObjectBuffer;
  
     
        void CompileDraw(OpenGlDevice Device)
        {
            Device.OnPaint();
        }
        /// <summary>
        /// is used in <see cref="Entity.Paint(OpenGlDevice)"/> to create <see cref="CompiledMesh"/>es.
        /// </summary>
        internal bool CreateSnap = false;
        /// <summary>
        /// produces <see cref="MeshContainer"/> with <see cref="CompiledMesh"/>es in <see cref="MeshContainer.Progs"/>.
        /// </summary>
        /// <returns></returns>
        MeshContainer CompileSnapBuffer()
        {
            bool SaveCompile = Entity.Compiling;
         //   Entity.Compiling = true; // darf nicht drinnen sein
            CreateSnap = true;                // gebraucht in Entity.Paint
            RenderKind Save = Device.RenderKind;
            StoredSnapItems.Clear();
            StoredSnapItems.Add(null);
         
      
            Device.RenderKind = RenderKind.SnapBuffer;
            MeshContainer Result = MeshCreator.CreateMeshes(Device, CompileDraw);// zuerst ins Device.Paint
            Device.RenderKind = Save;
        
            CreateSnap = false;
            Entity.Compiling = SaveCompile;
            return Result;
        }
      

        static MeshContainer _SnapMesh = null;
        /// <summary>
        /// holds geometrie of the graphical objects, who are register a <see cref="CompiledMesh.SnapObject"/> by  <see cref="RegisterSnapItem(SnappItem)"/> in the list <see cref="MeshContainer.Progs"/>. All this members are <see cref="CompiledMesh"/> so thes have in <see cref="CompiledMesh.SnapObject"/> the id for <see cref="RegisterSnapItem(SnappItem)"/>.
        /// object in the <see cref="StoredSnapItems"/> .
        /// </summary>
        public static MeshContainer SnapMesh
        {
            get { return _SnapMesh; }
            set { _SnapMesh = value; }
        }

        /// <summary>
        /// used from MeshCreator to create <see cref="CompiledMesh"/> in SnapMesh.Progs.
        /// </summary>
        internal static bool RegisterSnap = false;  // wird nur von MeshCreator verwendet um CompiledMesh in Selector.SnapMesh.Progs zu stellen
        /// <summary>
        /// is setted while the <see cref="PickingShader"/> the <see cref="PrimIdObj"/> writes to <see cref="SnapBuffer"/>.
        /// </summary>
        public static bool WriteToSnap = false;
        /// <summary>
        /// a very important method.<br/>
        /// 1. the <see cref="OpenGlDevice.RenderKind"/> gets the value <see cref="Drawing3d.RenderKind.SnapBuffer"/> and the <see cref="StoredSnapItems"/> will be cleared.
        /// in the draw method will be registered a <see cref="SnappItem"/> by <see cref="RegisterSnapItem(SnappItem)"/>.<br/>
        /// 2. the geometry of the graphical object will be stored in <see cref="CompiledMesh"/> as member of the progs of <see cref="SnapMesh"/>.
        /// Every member has a <see cref="CompiledMesh.SnapObject"/>. this is the id in the <see cref="StoredSnapItems"/>.<br/>
        /// 3. the <see cref="PickingShader"/> writes the geometry to the <see cref="SnapBuffer"/> with "color" <see cref="PrimIdObj"/> containing the objectId and the <see cref="PrimIdObj.PrimId"/>.<br/>
        /// 4. the values from <see cref="SnapBuffer"/> will be transferred to the <see cref="ObjectBuffer"/> .
        /// </summary>
        public void RefreshSnapBuffer()
        {
            Texture Savet = Device.texture;
            if (!Device.SnapEnable) return;
            RegisterSnap = true;
            if (SnapMesh == null) SnapMesh = new MeshContainer();
            SnapMesh.Progs.Clear();
          
         
            CompileSnapBuffer();  // snapitems registrieren !!! CompiledMesh erzeugen
           
            RegisterSnap = false;
                // zu den Compilierten Objectindex
            
            GLShader Save = Device.Shader;
            Device.Shader = PickingShader;
            CurrentObjectIndex = PickingShader.getvar("TheObject").handle; 
            RenderKind SaveRenderKind = Device.RenderKind;                 // mit Selector.SetObjectNumber(ObjectNumber= Storedsnapitemlist.Count-1) der letzte in den buffer geschrieben werden kann,
            Device.RenderKind = RenderKind.Render;
            WriteToSnap = true;
            SnapBuffer.Init((int)Device.ViewPort.Width, (int)Device.ViewPort.Height);
            SnapBuffer.EnableWriting();


       
                GL.Disable(EnableCap.Blend);                   // kein vermischen der werte (Farben
                SnapMesh.Paint(Device);                       // Pickingshader schreibt die indices in den snabuffer
                ObjectBuffer = ReadPickingBuffer(0, 0, (int)Device.ViewPort.Width, (int)Device.ViewPort.Height);
           
                GL.Enable(EnableCap.Blend);
      
            SnapBuffer.DisableWriting();
            Device.texture = Savet;
            Device.RenderKind = SaveRenderKind;

         
            // Liest the Snapbuffer aus und stellt die Werte nach objectbuffer
            Device.Shader = Save;
            WriteToSnap = false;
            CreateSnappList();  // Erzeugt eine aktuelle Snaplist
         }
        PrimIdObj[,] SubArray(int x, int y, int Width, int Height)
        {
            for (int i = 0; i < ObjectBuffer.GetLength(0); i++)
                for (int j = 0; j < ObjectBuffer.GetLength(1); j++)
                {
                   
                      if (ObjectBuffer[i, j].PrimId!=0)
                    { }
                }
            PrimIdObj[,] Result = new PrimIdObj[Width, Height];
            if (ObjectBuffer == null) return Result;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if ((i + x >= 0) && (j + y >= 0) && (i + x < ObjectBuffer.GetLength(0)) && (j + y < ObjectBuffer.GetLength(1)))
                        Result[i, j] = ObjectBuffer[i + x, j + y];
                 }
             return Result;
        }
        /// <summary>
        /// contains the registered <see cref="SnappItem"/>. See also <see cref="RegisterSnapItem(SnappItem)"/>.
        /// </summary>
        public static List<SnappItem> StoredSnapItems = new List<SnappItem>();
        /// <summary>
        /// gets the index in the <see cref="SnappList"/> of the graphical object, which is marked by <see cref="OpenGlDevice.PushTag(object)"/> and <see cref="OpenGlDevice.PopTag"/>
        /// </summary>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public int IndexOfTag(object Tag)
        {
            for (int i = 0; i < SnappList.Count; i++)
            {
                if (SnappList[i].Tag.Equals(Tag))
                    return i;
            }
            return -1;
        }
      

        /// <summary>
        /// will be setted by the <see cref="CompiledMesh"/> when the <see cref="PickingShader"/> the graphical objects 
        /// writes to the <see cref="SnapBuffer"/>. See also <see cref="RefreshSnapBuffer"/>. It is also used by <see cref="SetPrimId(uint)"/> in <see cref="Primitives3d.drawTriangles(OpenGlDevice, int[], xyzf[], xyzf[], xyf[], xyzf[])"/>.
        /// </summary>
        public uint CurrentObject = 0;
        /// <summary>
        /// gets the <see cref="PrimIdObj.PrimId"/> to the <see cref="PickingShader"/> in <see cref="Primitives3d.drawTriangles(OpenGlDevice, int[], xyzf[], xyzf[], xyf[], xyzf[])"/>, which is the index of the triangle. See also <see cref="SnappItem.TriangleInfo"/>.
        /// </summary>
        /// <param name="PrimId">index of the triangle, which is painted by the <see cref="PickingShader"/> in <see cref="Primitives3d.drawTriangles(OpenGlDevice, int[], xyzf[], xyzf[], xyf[], xyzf[])"/></param>
        internal void SetPrimId(uint PrimId)
        {
           int C= (int)((PrimId << 16) + CurrentObject);
            
           GL.Uniform1(CurrentObjectIndex, C);
       }
 
         /// <summary>
         /// the handle of the field "TheObject" from the <see cref="PickingShader"/>.
         /// </summary>
         public  int CurrentObjectIndex = 2;
         SnappItem _CurrentSnapItem = null;
        /// <summary>
        /// is used from <see cref="RegisterSnapItem(SnappItem)"/> and <see cref="UnRegisterSnapItem(object)"/> to
        /// avoid that for a graphical object more than one <see cref="SnappItem"/> will be registered. This could happen,
        /// when a graphic method calls an other graphic method, who has also a registration.
        /// E.g. <see cref="OpenGlDevice.drawCurve(Curve)"/> calls <see cref="OpenGlDevice.drawPolyLine(xyArray)"/>.
        /// </summary>
        public SnappItem CurrentSnapItem
        { get { return _CurrentSnapItem; }

            set { _CurrentSnapItem=value; }

        }
        /// <summary>
        /// is true, when a <see cref="SnappItem"/> is already registered. See also <see cref="CurrentSnapItem"/> or <see cref="RegisterSnapItem(SnappItem)"/>.
        /// </summary>
        public bool IsRegisteredSnapItem
        {
            get {return CurrentSnapItem != null; }
        }
        /// <summary>
        /// is called, when ever a <see cref="RegisterSnapItem(SnappItem)"/> will be called.
        /// E.g.:<br/>
        ///  public void drawsomething(xyz A)<br/>
        ///  {<br/>
        ///  object Handle = null;<br/>
        ///      if (RenderKind == RenderKind.SnapBuffer) <br/>
        ///       Handle = Selector.RegisterSnapItem(new PointSnappItem(A));<br/>
        ///       ..............;<br/>
        ///      if (RenderKind == RenderKind.SnapBuffer) <br/>
        ///        Selector.UnRegisterSnapItem(Handle);<br/>
        ///   }<br/>
        /// </summary>
        /// <param name="Handle"></param>
        public void UnRegisterSnapItem(object Handle)
        {
            if ((Handle != CurrentSnapItem) || (CurrentSnapItem == null)) return;
            MeshCreator.Renew();
            CurrentSnapItem = null;
        }
   
        /// <summary>
        /// register a <see cref="SnappItem"/>. It must be combined with <see cref="UnRegisterSnapItem(object)"/>.
        /// </summary>
        /// <param name="Item">the <see cref="SnappItem"/>, which will be registered.</param>
        /// <returns>handle, which will be used by <see cref="UnRegisterSnapItem(object)"/></returns>
        public object RegisterSnapItem(SnappItem Item)
        {
            if (Item == null) return null;
    
            if ((Device.RenderKind != RenderKind.SnapBuffer)
                && (!Entity.Compiling))
               return null;
            if ((CurrentSnapItem == null))
            {
                CurrentSnapItem = Item;
                if (!Item.Udated)
                {  
                    Item.Device = Device;
                    Item.ModelMatrix = Device.ModelMatrix;
                    if (Item.PolygonMode != PolygonMode.Point)
                        Item.PolygonMode = Device.PolygonMode;
                    Item.Object = Entity.CurrentEntity;

                    Item.Udated = true;
                }
               if (Tags.Count > 0)
                    Item.Tag = Tags.Peek();

                StoredSnapItems.Add(Item);

                return Item;
            }
            return null;
        }
        /// <summary>
        /// the stack, which holds the tags, which are setted by <see cref="OpenGlDevice.PushTag(object)"/> and removed by <see cref="OpenGlDevice.PopTag"/>.
        /// </summary>
        internal System.Collections.Stack Tags = new System.Collections.Stack();
        /// <summary>
        /// is the same as <see cref="OpenGlDevice.SnapMagnetism"/>. Default are 4 pixels.
        /// </summary>
        internal int SnapDistance = 4;
        /// <summary>
        /// intern used.
        /// </summary>
        List<PrimIdObj> Intermediate = new List<PrimIdObj>();
        /// <summary>
        /// inern used. Gives the index of a PrimIdObj in Intermediate.
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        int IndexOf(PrimIdObj R)
        {
            for (int i = 0; i < Intermediate.Count; i++)
            {
               if ((Intermediate[i].Object == R.Object))
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// activiert the <see cref="PrimIdObj.PrimId"/> mechanism.
        /// </summary>
        public bool IntersectionUsed = true;
        //...................................................
      List<  List<short>> NeighborPrims = new List<List<short>>();
        internal void AddNeighborsPrimId(PrimIdObj[,] P,int Id,int i,int j)
        {


           int Centeri = i;
           int Centerj = j;
            if (NeighborPrims.Count > Id) return;
            List<short> Primids = new List<short>();
            NeighborPrims.Add(Primids);
            Primids.Add(P[Centeri, Centerj].PrimId);
            short Object= P[Centeri, Centerj].Object;
            if (Centeri + 1 < P.GetLength(0))
                if (P[Centeri + 1, Centerj].Object==Object)
                    if (!Primids.Contains(P[Centeri + 1, Centerj].PrimId))
                    Primids.Add(P[Centeri + 1, Centerj].PrimId);
            if ((Centeri + 1 < P.GetLength(0))&& (Centerj + 1 < P.GetLength(1)))
                if (P[Centeri + 1, Centerj+1].Object == Object)
                    if (!Primids.Contains(P[Centeri + 1, Centerj+1].PrimId))
                        Primids.Add(P[Centeri + 1, Centerj+1].PrimId);
            if (Centeri - 1 >=0 )
                if (P[Centeri - 1, Centerj].Object == Object)
                    if (!Primids.Contains(P[Centeri - 1, Centerj].PrimId))
                        Primids.Add(P[Centeri - 1, Centerj].PrimId);
            if ((Centeri - 1 >= 0) && (Centerj - 1 >=0))
                if (P[Centeri - 1, Centerj - 1].Object == Object)
                    if (!Primids.Contains(P[Centeri - 1, Centerj - 1].PrimId))
                        Primids.Add(P[Centeri - 1, Centerj - 1].PrimId);
            if (Centerj + 1 < P.GetLength(1))
                if (P[Centeri, Centerj + 1].Object == Object)
                    if (!Primids.Contains(P[Centeri, Centerj + 1].PrimId))
                        Primids.Add(P[Centeri , Centerj+1].PrimId);
            if ((Centeri - 1 >= 0) && (Centerj + 1 < P.GetLength(1)))
                if (P[Centeri - 1, Centerj + 1].Object == Object)
                    if (!Primids.Contains(P[Centeri - 1, Centerj + 1].PrimId))
                        Primids.Add(P[Centeri - 1, Centerj + 1].PrimId);
            if ((Centeri + 1 < P.GetLength(0)) && (Centerj - 1 >=0))
                if (P[Centeri + 1, Centerj - 1].Object == Object)
                    if (!Primids.Contains(P[Centeri + 1, Centerj - 1].PrimId))
                        Primids.Add(P[Centeri + 1, Centerj - 1].PrimId);
            if (Centerj - 1 >= 0)
                if (P[Centeri, Centerj - 1].Object == Object)
                    if (!Primids.Contains(P[Centeri, Centerj - 1].PrimId))
                        Primids.Add(P[Centeri, Centerj - 1].PrimId);
        }
        /// <summary>
        /// reads concentric in a subarray <b>P</b> of the <see cref="ObjectBuffer"/>.
        /// </summary>
        /// <param name="P">subarray <b>P</b> of the <see cref="ObjectBuffer"/></param>
        /// <param name="Centeri">x koordinate of the reading center.</param>
        /// <param name="Centerj">y koordinate of the reading center.</param>
        /// <param name="radius">radius to read.</param>
        void ReadKonzentrisch(PrimIdObj[,] P, int Centeri, int Centerj, int radius)
        {
           if (radius == 0)
            {

                if ((int)P[Centeri, Centerj].Object == 0) return;
                int id = IndexOf(P[Centeri, Centerj]);
                if (id < 0)
                {
                    Intermediate.Add(P[Centeri, Centerj]);
                    AddNeighborsPrimId(P, Intermediate.Count - 1, Centeri, Centerj);
                }
               
                return;
            }
            for (int i = Centeri - radius; i < Centeri + radius; i++)
            {

                if ((int)P[i, Centerj - radius].Object == 0)
                    continue;
              
                int id = IndexOf(P[i, Centerj - radius]);
                if (id < 0)
                {
                    Intermediate.Add(P[i, Centerj - radius]);
                    AddNeighborsPrimId(P, Intermediate.Count - 1, i, Centerj - radius);
                }

            }

            for (int j = Centerj - radius; j < Centerj + radius; j++)
            {
                if ((int)P[Centeri + radius, j].Object == 0) continue;
                int id = IndexOf(P[Centeri + radius, j]);
                if (id < 0)
                {
                    Intermediate.Add(P[Centeri + radius, j]);
                    AddNeighborsPrimId(P, Intermediate.Count - 1, Centeri + radius, j);
                }
               

            }
            for (int i = Centeri + radius; i > Centeri - radius; i--)
            {
                if ((int)P[i, Centerj + radius].Object == 0) continue;
                int id = IndexOf(P[i, Centerj + radius]);
                if (id < 0)
                {
                    Intermediate.Add(P[i, Centerj + radius]);
                    AddNeighborsPrimId(P, Intermediate.Count - 1, i, Centerj + radius);
                }
                

            }
            for (int j = Centerj + radius; j > Centerj - radius; j--)
            {
                if ((int)P[Centeri - radius, j].Object == 0) continue;
                int id = IndexOf(P[Centeri - radius, j]);
                if (id < 0)
                {
                    Intermediate.Add(P[Centeri - radius, j]);
                    AddNeighborsPrimId(P, Intermediate.Count - 1, Centeri - radius, j);
                }
               ;

            }

        }
        /// <summary>
        /// this event is called from <see cref="SetCurrentXYZ"/>, when ever the <see cref="Selector"/> sets the <see cref="SnappItem.Point"/> of the first entry in <see cref="OpenGlDevice.SnappItems"/> to <see cref="OpenGlDevice.Currentxyz"/>.
        /// </summary>
        public event EventxyzHandler SetCurrentXYZ = null;
        /// <summary>
        /// sets sets the <see cref="SnappItem.Point"/> of the first entry in <see cref="OpenGlDevice.SnappItems"/> to <see cref="OpenGlDevice.Currentxyz"/>.
        /// </summary>
        /// <param name="ViewLine">Line from Eye to the mouse position</param>
        /// <param name="Point">the <see cref="SnappItem.Point"/></param>
        virtual protected void Setxyz(LineType ViewLine, xyz Point)
        {
            if (SetCurrentXYZ != null)
            {
                SetCurrentXYZ(Device, ViewLine, Point);

            }

            Device.Currentxyz = Point;
        }
        /// <summary>
        /// if <b>CrossTheSnappItems</b> is <b>true</b>, twos snappitems will be crossed and the magnetism point is setted to a crosspoint. The default = false;
        /// </summary>
        public static bool CrossTheSnappItems = false;
        /// <summary>
        /// crates for every mouse move a <see cref="OpenGlDevice.SnappItems"/> list.
        /// </summary>
        public void CreateSnappList()
        {
            System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch SW1 = new System.Diagnostics.Stopwatch();

            if (!Device.SnapEnable) return;
           if (Device.CurrentNavigationsModus != OpenGlDevice.NavigateModus.Nothing) return;
            Device.Emission = Color.Black;
            LineType VL;
            try
            {
                {
                    SW.Reset();
                    SW.Start();
                    SnappList.Clear();
                    LineType ViewLine = Device.FromScr(Device.MousePos);
                    VL = ViewLine;
                    int siz = SnapDistance * 2 + 1;

                    PrimIdObj[,] P = SubArray(Device.MousePos.X - siz / 2, (int)Device.ViewPort.Height - Device.MousePos.Y - siz / 2, siz, siz);


                    Intermediate.Clear();
                    NeighborPrims.Clear();   
                    for (int i = 0; i <= siz / 2; i++)
                        ReadKonzentrisch(P, siz / 2, siz / 2, i);
                    SnappItem[] BB = new SnappItem[Intermediate.Count];
                   
                    SnappItem.Snappdist = Device.PixelToWorld(new xyz(0,0,0),SnapDistance);
                    for (int i = 0; i < Intermediate.Count; i++)
                    {
                        if (Intermediate.Count > 1)
                        { }
                        SnappItem SI = null;
                        int Index = Intermediate[i].Object;
                        if (Index < 0) continue;
                        if (StoredSnapItems.Count > Index)
                            SI = StoredSnapItems[Index];
                        if (SI == null) continue;
                        if ((((SI != null) && (i == 0)) || (SI.PolygonMode == PolygonMode.Line)) ||
                            ((SI != null) && (i == 1) && (SI.PolygonMode == PolygonMode.Line)) ||
                            (((SI != null) && (i == 1)) && (((SnappList.Count > 0) /*&& (SnapList[0].OfObject == null)*/) || (SI.OfObject == null) || (SnappList[0].OfObject != SI.OfObject))))
                        {
                            SnappItem.PrimIds = NeighborPrims[i];
                            SI.PrimId = (short)Intermediate[i].PrimId;
                            Matrix ToLocal = SI.ModelMatrix.invert();
                            LineType L = ViewLine * ToLocal;
                            SI.Crossed = false;
                            SI.doExchange = false;
                            SI.Point =  SI.Cross(L);
            
                            double lam = -1;
                            xyz Pt = new xyz(0, 0, 0);
                            ViewLine.Distance(SI.Point, out lam, out Pt);
                            SI.Depth = ViewLine.Q.dist(Pt);
                            SI.Depth = lam;
                            int Id = 0;
                            for (int k = 0; k < SnappList.Count; k++)
                            {

                                if (SI is PointSnappItem) { Id = 0; break; };
                                if (SnappList[k].Depth > SI.Depth)
                                { Id = k; break; }
                                Id = k + 1;
                            }

                            SnappList.Insert(Id, SI);
                        }
     
                    }
                    SW.Stop();
                    SW1.Reset();
                    SW1.Start();
                    double dummy = -1;
                    xyz _P = new xyz(0, 0, 0);
                    if (SnappList.Count >= 2)

                        if (SnappList[1].doExchange)
                            if ((ViewLine.Distance(SnappList[1].Point, out dummy, out _P) < ViewLine.Distance(SnappList[0].Point, out dummy, out _P)))
                            {
                                SnappItem Ex = SnappList[0];
                                SnappList[0] = SnappList[1];
                                SnappList[1] = Ex;
                            }
                    if (CrossTheSnappItems)
                    CheckIntersect(ViewLine);
                    if (SnappList.Count > 0)
                        Setxyz(ViewLine, SnappList[0].Point);
                    else
                    {
                       double lam;
                        Base B = Device.getProjectionBase();
                        Plane pl = new Plane(Device.Camera.Anchor, B.BaseZ);
                        xyz Pt;
                        pl.Cross(ViewLine, out lam, out Pt);
                        Setxyz(ViewLine, Pt);
                    }
                    SW1.Stop();


                }
            }
            finally
            {

            }
       }
     
        // from IntersectLineWithLine
        bool IntersectLines(xyzArray A, xyzArray B, xyz NearPoint, ref xyz CrossPoint)
        {
            List<CrossTag> L = CrossTag.GetCrossList(A, B);
            for (int i = 0; i < L.Count; i++)
            {  
                if (A.Value(L[i].Lam).dist(NearPoint) <= SnappItem.Snappdist)
                {
                    CrossPoint = A.Value(L[i].Lam);

                     return true;
                }
            }
            return false;
        }
       // from IntersectPlaneWithPlane
        List<Line3D> Cross(List<TriangleF> LL1, List<TriangleF> LL2, LineType ViewLine)
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

                        Plane P1 = new Plane(LL1[i].A, LL1[i].B, LL1[i].C);
                        Plane P2 = new Plane(LL2[j].A, LL2[j].B, LL2[j].C);
                            if (TriangleF.Cross(LL1[i], LL2[j],/*ViewLine,*/ ref Pt1, ref Pt2))
                            //checken ob die dreiecke zum betrachter gerichtet sind
                            Lines.Add(new Line3D(Pt1.Toxyz(), Pt2.Toxyz()));
                    }
                }
            }
            return Lines;
        }
        /// <summary>
        /// snapps all objects inside a <see cref="RectangleF"/>
        /// </summary>
        /// <param name="Rectangle"></param>
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
            SnappList.Clear();
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
                                SnappList.Add(StoredSnapItems[i]);
                                break;
                            }


                        }

                    }
            }
        }
        //   from intersectPlaneWithLine
        bool Cross(List<TriangleF> Triangles, xyzArray Poly, ref double Lam, ref xyz Pt)
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

      
        bool IntersectPlaneWithPlane(SnappItem Plane1, SnappItem Plane2, LineType ViewLine)
        {
            List<TriangleF> ListOfTriangles1 = new List<TriangleF>();
            List<TriangleF> ListOfTriangles2 = new List<TriangleF>();
            int Object1 = StoredSnapItems.IndexOf(Plane1);
            int Object2 = StoredSnapItems.IndexOf(Plane2);
            ListOfTriangles1 = _GetTriangles(Plane1, Device);
          
            ListOfTriangles2 = _GetTriangles(Plane2, Device);
         
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
                xyz dummy = new xyz(0, 0, 0);
                for (int i = 0; i < L1.Count; i++)
                {
                    for (int j = 0; j < L2.Count; j++)
                    {
                       
                        if (TriangleF.Cross(L1[i], L2[j], out Lam, out Mue, out P1, out P2))
                            if ((ViewLine.Distance(P1, out Lam, out dummy) < SnappItem.Snappdist * 2) // damit nicht zu weite abstäde von der Viewline sind
                                && (ViewLine.Distance(P2, out Lam, out dummy) < SnappItem.Snappdist * 2))
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
                              //  break;
                            }
                     


                    }
                }

            }
              
            double di = 1e10;
            xyz Pos = new xyz(0, 0, 0);
            int Id = -1;
            for (int i = 0; i < Lines.Count; i++)
            {
                LineType L = new LineType(Lines[i].A, Lines[i].B - Lines[i].A);
                double Lam = -1;
                double Mue = -1;
                double d = ViewLine.Distance(L, 1e10, out Lam, out Mue);
                xyz PP = L.Value(Mue);
                xyz Nearest = ViewLine.Value(Lam);
                double HH = (PP - Nearest).length();
                if ((Mue >= -0.00001) && (Mue <= 1.00001))
                    if (d < di)
                    {
                        Pos = L.Value(Mue);
                        di = d;
                        Id = i;
                    }
            }
            if ((di < 1e10) && (di <= SnappItem.Snappdist))
            {

                if (((Plane1.OfObject == null) && (Plane2.OfObject == null)) || ((Plane1.OfObject != Plane2.OfObject)))

                {
                    Plane1.Crossed = true;
                    Plane2.Crossed = true;
                }
                Plane1.Point = Pos;
                Plane2.Point = Pos;
                return true;
            }
         
            xyzArray A = Plane1.getxyzArray();
            xyzArray B = Plane2.getxyzArray();
            if ((A != null) && (B != null))
                return IntersectLineWithLine(Plane1, Plane2, ViewLine);





            return false;
        }
        bool intersectPlaneWithLine(SnappItem Plane, SnappItem Line, xyzArray LineArray, ref double Lam, ref xyz Result)
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
                    List<CrossTag> L = CrossTag.GetCrossList(A, B);

                    for (int i = 0; i < L.Count; i++)
                    {
                        if (A.Value(L[i].Lam).dist(Plane.Point) <= SnappItem.Snappdist)
                        {
                            Lam = L[i].Lam;

                            Result = A.Value(L[i].Lam);
                            return true;
                        }
                    }
                }
            }

            int Object = StoredSnapItems.IndexOf(Plane);
            int siz = SnapDistance * 2 + 1;
          //  siz = SnapDistance;
            PrimIdObj[,] P = SubArray(Device.MousePos.X - siz / 2, (int)Device.ViewPort.Height - Device.MousePos.Y - siz / 2, siz, siz);
            List<TriangleF> LT = GetTriangles(P, Object);
            Lam = -1;



            bool R = Cross(LT, LineArray, ref Lam, ref Result);
            return R;

        }
        bool IntersectLineWithLine(SnappItem Line1, SnappItem Line2, LineType ViewLine)
        {
            xyzArray A = Line1.getxyzArray();
            if (A == null) return false;
            int h = A.Count;
            xyzArray B = Line2.getxyzArray();
            if (B == null) return false;
            double d1 = 1e10;
            int i1 = -1;
            double d2 = 1e10;
            int i2 = -1;
            for (int i = 0; i < A.Count; i++)
            {
                double d = Line1.Point.dist(A[i]);
                if (d < d1)
                { d1 = d;
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
                if (((Line1.OfObject == null)&& (Line2.OfObject == null))|| ((Line1.OfObject != Line2.OfObject)))
                {
                    Line1.Crossed = true;
                    Line2.Crossed = true;
                }
                return true;
            }

            return false;

        }
        void IntersectPlaneWithLine(SnappItem Plane, SnappItem Line, LineType ViewLine)
        {
            double Lam = -1;
            xyz Result = new xyz(0, 0, 0);
            if (intersectPlaneWithLine(Plane, Line, Line.getxyzArray(), ref Lam, ref Result))
            {
                if (Result.dist(Plane.Point) <= SnappItem.Snappdist)
                {

                    Plane.Point = Result;
                    Line.Point = Plane.Point;
                    if (((Plane.OfObject == null) && (Line.OfObject == null)) || ((Plane.OfObject != Line.OfObject)))
                    {
                        Line.Crossed = true;
                        Plane.Crossed = true;
                    }
                }
            }
        }

        void CheckIntersect(LineType ViewLine)
        {
            if (SnappList.Count < 2) return;
            if ((SnappList[0].PolygonMode == PolygonMode.Line) && (SnappList[1].PolygonMode == PolygonMode.Line))
            {
                IntersectLineWithLine(SnappList[0], SnappList[1], ViewLine);
                return;
            }

            if ((SnappList[0].PolygonMode == PolygonMode.Fill) && (SnappList[1].PolygonMode == PolygonMode.Line))
            {
                IntersectPlaneWithLine(SnappList[0], SnappList[1], ViewLine);
                return;
            }
            if ((SnappList[0].PolygonMode == PolygonMode.Line) && (SnappList[1].PolygonMode == PolygonMode.Fill))
            {
                IntersectPlaneWithLine(SnappList[1], SnappList[0], ViewLine);
                return;
            }
            if ((SnappList[0].PolygonMode == PolygonMode.Fill) && (SnappList[1].PolygonMode == PolygonMode.Fill))
                if  ((!(SnappList[0] is TextSnappItem)) && (!(SnappList[0] is TextSnappItem)))
            {
                IntersectPlaneWithPlane(SnappList[0], SnappList[1], ViewLine);
                return;
            }


        }
        // from intersectplanewithPlane
        List<TriangleF> _GetTriangles(SnappItem SI, OpenGlDevice Device)
        {
            List<TriangleF> Result = new List<TriangleF>();
            LineType ViewLine = Device.FromScr(Device.MousePos);
            for (int i = 0; i < SI.TriangleInfo.Indices.Length - 2; i++)
            {

                Matrix M = SI.ModelMatrix;
                TriangleInfo TI = SI.TriangleInfo;
                TriangleF T = new TriangleF(M * TI.Points[TI.Indices[i]], M * TI.Points[TI.Indices[i + 1]], M * TI.Points[TI.Indices[i + 2]]);
                i = i + 2;
                Plane P = new Plane(T.A.Toxyz(), T.B.Toxyz(), T.C.Toxyz());
                double Lam = -1;
                xyz PT = new xyz(0, 0, 0);
                P.Cross(ViewLine, out Lam, out PT);
                if (T.Inside(PT.toXYZF()))
                    Result.Add(T);
            }
            return Result;

        }
        // from Plane with Line
        List<TriangleF> GetTriangles(PrimIdObj[,] P, int Object)
        {
            List<TriangleF> Result = new List<TriangleF>();
            List<PrimIdObj> PrimIds = new List<PrimIdObj>();
            SnappItem SI = StoredSnapItems[Object];
            Matrix M = SI.ModelMatrix;
            for (int i = 0; i < P.GetLength(0); i++)
            {
                for (int j = 0; j < P.GetLength(1); j++)
                {

                    if ((PrimIds.IndexOf(P[i, j]) < 0))
                    {
                        if ((P[i, j].Object == Object) && (SI.TriangleInfo != null))
                        {

                            TriangleF T = new TriangleF(SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId]],
                                                      SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId + 1]],
                                                      SI.ModelMatrix * SI.TriangleInfo.Points[SI.TriangleInfo.Indices[P[i, j].PrimId  + 2]]);

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
    }
    /// <summary>
    /// this class is only used from the <see cref="Selector"/> and holds two fields <b>Lam</b> and <b>mue</b>.
    /// </summary>
    [Serializable]
    class CrossTag
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
            if (Det==0)// parallele
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
        //von IntersectLines -->cross line with line
        internal static List<CrossTag> GetCrossList(xyzArray A, xyzArray B)
        {
            List<CrossTag> Result = new List<CrossTag>();
            for (int i = 0; i < A.Count - 1; i++)
            {
                if (A[i].dist(A[i + 1]) < 0.0000001) continue;
                LineType L1 = new LineType(A[i], A[i + 1] - A[i]);
                
                for (int j = 0; j < B.Count - 1; j++)
                {
                    if (B[j].dist(B[j + 1]) < 0.0000001) continue;
                    LineType L2 = new LineType(B[j], B[j + 1] - B[j]);
                    double Lam1 = -1;
                    double Lam2 = -1;
                    if (Cross(L1, L2, ref Lam1, ref Lam2))
                    {
                        Result.Add(new CrossTag(i + Lam1, j + Lam2));
                    }
                }
            }
            return Result;
        }
        /// <summary>
        /// Constructor with <see cref="Lam"/> and <see cref="Mue"/>.
        /// </summary>
        /// <param name="Lam">is the first parameter.</param>
        /// <param name="Mue">is the second parameter.</param>
        public CrossTag(double Lam, double Mue)
        {
            this.Lam = Lam;
            this.Mue = Mue;
        }
        /// <summary>
        /// is the first parameter.
        /// </summary>
        internal double Lam = -1;
        /// <summary>
        /// is the second parameter.
        /// </summary>
        internal double Mue = -1;
    }
   
}












