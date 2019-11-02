
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif



//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
#if LONGDEF

    using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif


    /// <summary>
    /// is the speediest <see cref="Entity"/>. You can use <b>VAO</b> by <see cref="ActivateVao"/> it writes the data directly to the card.
    /// </summary>
    [Serializable]
    public class Mesh : Entity
    {
        /// <summary>
        /// if you set the compiler directive LONGDEF IndexType is Int else IndexType is Uint16.
        /// Uint16 is for graphic cards with a small memory. 
        /// </summary>
        public static Type IndexType;
        /// <summary>
        /// an empty contructor.
        /// </summary>
        public Mesh()
        {
            VBOIndices = new VBO();
            VBOIndices.IndexArray = new IndexType[0];
            VBOPosition = new VBO();
            VBOPosition.VarName = "Position";
            VBOPosition.xyzPoints = null;
            VBOColors = new VBO();
            VBOColors.VarName = "Color";
            VBOColors.xyzPoints = null;
            VBONormals = new VBO();
            VBONormals.VarName = "Normal";
            VBONormals.xyzPoints = null;
            VBOTexture = new VBO();
            VBOTexture.xyPoints = null;
            VBOTexture.VarName = "Texture";
        }

        /// <summary>
        /// overrides the <see cref="Entity.GetMaxBox"/>method.
        /// </summary>
        /// <returns></returns>
        public override Box GetMaxBox()
        {
            Box B = base.GetMaxBox();
            xyzf[] P = new xyzf[Position.Length];
            for (int i = 0; i < Position.Length; i++)
                P[i] = Transformation * Position[i];
            return B.GetMaxBox(Base.UnitBase, P);
        }
        /// <summary>
        /// gets the triangles of the mesh.
        /// </summary>
        /// <param name="TrList">is the list in which the triangles are posed.</param>
        public void getTriangles(TriangleList TrList)
        {
            TriangleArrays T = new TriangleArrays(Indices, Position, Normals, Colors, TextureCoords);
            TrList.AddToList(T);
        }

        /// <summary>
        /// is the constructor with <b>indices</b>, <b>positions</b>, <b>normal vectors</b>, <b>texture coordinates</b> and <b>colors</b>. Three sequential indices determe a triangle
        /// Points[indices[i]], Points[indices[i+1]] and Points[indices[i+2]]. In every point a normal vector is given, which is responsible for the lightning.
        /// Texture are coordinates for a <see cref="Texture"/>. For every point you can define a color.
        /// </summary>
        /// <param name="Indices"> are the indices, who determ a triangle points[indices[i]], points[indices[i+1]] and points[indices[i+2]].</param>
        /// <param name="Positions">are the points of the mesh.</param>
        /// <param name="Normals">are the normals of the mesh.</param>
        /// <param name="TextureCoords">are texture coordinates.</param>
        /// <param name="Colors">are the colors.</param>
        public Mesh(IndexType[] Indices, xyzf[] Positions, xyzf[] Normals, xyf[] TextureCoords, xyzf[] Colors)
        {

            VBOIndices = new VBO();
            VBOIndices.IndexArray = new IndexType[0];
            VBOPosition = new VBO();
            VBOPosition.VarName = "Position";
            VBOPosition.xyzPoints = Positions;
            VBOColors = new VBO();
            VBOColors.VarName = "Color";
            VBOColors.xyzPoints = Colors;
            VBOTexture = new VBO();
            VBOTexture.xyPoints = null;
            VBOTexture.VarName = "Texture";
            VBONormals = new VBO();
            VBONormals.VarName = "Normal";
            VBONormals.xyzPoints = Normals;
            if (TextureCoords != null)
                this.TextureCoords = TextureCoords;
            this.Indices = Indices;
        }
        /// <summary>
        /// is the VAP handle. It is greather 0 when <see cref="ActivateVao"/> is called.
        /// </summary>
       public int VAO = -1;
        void DestroyVaoHandles()
        {
            if (VAO == -1) return;
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffers(1, ref VBOPosition._Handle);
            GL.DeleteBuffers(1, ref VBONormals._Handle);
            GL.DeleteBuffers(1, ref VBOColors._Handle);
            GL.DeleteBuffers(1, ref VBOTexture._Handle);
            GL.DeleteBuffers(1, ref VBOIndices._Handle);

        }
        /// <summary>
        /// if you want to use VAO call <b>ActivateVao</b>. Then the data will be stored on the graphic card. It is faster.
        /// </summary>
        public void ActivateVao()
        {
            DestroyVaoHandles();
            VAO = GL.GenVertexArray();

            VBOPosition._Handle = GL.GenBuffer();
            VBONormals._Handle = GL.GenBuffer();
            VBOColors._Handle = GL.GenBuffer();
            VBOTexture._Handle = GL.GenBuffer();
            VBOIndices._Handle = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPosition._Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 3 * VBOPosition.xyzPoints.Length), ref VBOPosition.xyzPoints[0].x, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            if (VBONormals.xyzPoints != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormals._Handle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 3 * VBONormals.xyzPoints.Length), ref VBONormals.xyzPoints[0].x, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);
            }
            if (VBOTexture.xyPoints != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTexture._Handle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 2 * VBOTexture.xyPoints.Length), ref VBOTexture.xyPoints[0].x, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(2);
            }
            if (VBOColors.xyzPoints != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOColors._Handle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 3 * VBOColors.xyzPoints.Length), ref VBOColors.xyzPoints[0].x, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(3);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBOIndices._Handle);
            if (VBOIndices.IndexArray.Length > 0)
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * VBOIndices.IndexArray.Length), ref VBOIndices.IndexArray[0], BufferUsageHint.StaticDraw);



            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        /// <summary>
        /// overrides the <see cref="Transform(Matrix)"/> method.
        /// </summary>
        /// <param name="T">transfrom matrix.</param>
        public new void Transform(Matrix T)
        {
            if (Position != null)
            {
                for (int i = 0; i < Position.Length; i++)
                    Position[i] = T * Position[i];

            }
            xyzf NP = T * new xyzf(0, 0, 0);
            if (Normals != null)
            {
                for (int i = 0; i < Normals.Length; i++)
                    Normals[i] = T * Normals[i] - NP;

            }

        }
        /// <summary>
        /// gets and sets the indices. three sequently indices reference a triangle. See also <see cref="Position"/>
        /// </summary>
        public IndexType[] Indices
        {
            get { return VBOIndices.IndexArray; }
            set
            {
                VBOIndices.IndexArray = value;
                SetInvalid(true);
            }
        }
        VBO VBOIndices;
        /// <summary>
        /// gets and sets the position as array of <see cref="xyzf"/>.
        /// </summary>
        public xyzf[] Position
        {
            get { return VBOPosition.xyzPoints; }
            set
            {
                VBOPosition.xyzPoints = value;
                SetInvalid(true);
            }
        }
        VBO VBOColors;
        /// <summary>
        /// gets and sets the colors as array of <see cref="xyzf"/>.
        /// </summary>
        public xyzf[] Colors
        {
            get { return VBOColors.xyzPoints; }
            set
            {
                VBOColors.xyzPoints = value;
                SetInvalid(true);
            }
        }
       VBO VBOPosition;

        /// <summary>
        /// gets and sets the normals as array of <see cref="xyzf"/>
        /// </summary>
        public xyzf[] Normals
        {
            get { return VBONormals.xyzPoints; }
            set
            {
                VBONormals.xyzPoints = value;
                SetInvalid(true);
            }
        }
        VBO VBONormals;
        /// <summary>
        /// gets and sets the <see cref="Entity.Texture"/> coordinates as array of <see cref="xyf"/>
        /// </summary>
        public xyf[] TextureCoords
        {
            get { return VBOTexture.xyPoints; }
            set
            {
                if (VBOTexture.xyPoints != value)
                    SetInvalid(true);
                VBOTexture.xyPoints = value;
            }
        }
        VBO VBOTexture;

        private Drawing3d.PolygonMode _Mode = Drawing3d.PolygonMode.Fill;
        /// <summary>
        /// gets and sets the with of a line. See also <see cref="OpenGlDevice.PenWidth"/>
        /// </summary>
        float _PenWidth = 1;
        /// <summary>
        /// can used to set a <see cref="PolygonMode"/>.
        /// </summary>

        public float PenWidth
        {
            get { return _PenWidth; }
            set
            {
                if (_PenWidth != value)
                    SetInvalid(true);
                _PenWidth = value;

            }
        }
        Color _PenColor = Color.Black;
        public Color PenColor
        {
            get { return _PenColor; }
            set { _PenColor = value; }
        }
        PenStyles _PenStyle = PenStyles.Full;
        public PenStyles PenStyle
        {
            get { return _PenStyle; }
            set { _PenStyle = value; }

        }
        /// <summary>
        /// gets and sets the polygon mode for the mesh.
        /// </summary>
        public Drawing3d.PolygonMode Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                if (_Mode != value)
                    SetInvalid(true);
            }
        }
 
        /// <summary>
        /// disposes the VAO.
        /// </summary>
        public void VAODispose()
        {
            if (VAO >= 0)
                GL.DeleteVertexArray(VAO);
            VAO = -1;
            if (VBOPosition._Handle >= 0)
                GL.DeleteBuffer(VBOPosition._Handle);
            VBOPosition._Handle = -1;
            if (VBONormals._Handle >= 0)
                GL.DeleteBuffer(VBONormals._Handle);
            VBONormals._Handle = -1;
            if (VBOColors._Handle >= 0)
                GL.DeleteBuffer(VBOColors._Handle);
            VBOColors._Handle = -1;
            if (VBOTexture._Handle >= 0)
                GL.DeleteBuffer(VBOTexture._Handle);
            VBOTexture._Handle = -1;
            if (VBOIndices._Handle >= 0)
                GL.DeleteBuffer(VBOIndices._Handle);
            VBOIndices._Handle = -1;

        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        internal void _Draw(OpenGlDevice Device)
        {

            OnDraw(Device);
            base.OnDraw(Device);
        }
        int[] QuadsToTriangle(int[] Indices)
        {
            int[] Result = new int[Indices.Length / 4 * 6];
            int TId = 0;
            for (int i = 0; i < Indices.Length / 4; i++)
            {
                Result[TId] = Indices[i * 4];
                Result[TId + 1] = Indices[i * 4 + 1];
                Result[TId + 2] = Indices[i * 4 + 2];


                Result[TId + 3] = Indices[i * 4];
                Result[TId + 4] = Indices[i * 4 + 2];
                Result[TId + 5] = Indices[i * 4 + 3];
                TId += 6;
            }

            return Result;
        }
        void DrawQuads(OpenGlDevice Device)
        {
            object SnapHandle = null;
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {

                MeshSnappItem SI = new MeshSnappItem(this);
                SnapHandle = Device.Selector.RegisterSnapItem(SI);
                MeshCreator.MeshdrawTriangles(Device, QuadsToTriangle(Indices), Position, Normals, TextureCoords);
            }
                int MPosition = Device.Shader.Position.handle;
            if (MPosition >= 0)
                GL.EnableVertexAttribArray(MPosition);
            GL.VertexAttribPointer(MPosition, 3, VertexAttribPointerType.Float, false, 0, ref Position[0]);
            int THandle = Device.Shader.Texture.handle;
            if ((THandle >= 0) && (Texture != null))
                GL.EnableVertexAttribArray(THandle);
            int NHandle = Device.Shader.Normal.handle;
            if ((NHandle >= 0) && (Normals != null))
                GL.EnableVertexAttribArray(NHandle);
            Field C = Device.Shader.getvar("ColorEnabled");
            if (C != null)
                C.SetValue(0);
            int CHandle = Device.Shader.Color.handle;
            if ((CHandle >= 0) && (Colors != null))
            {
                if (C != null)
                    C.SetValue(1);
                GL.EnableVertexAttribArray(CHandle);
            }
            if ((MPosition >= 0) && (Position.Length > 0))
            {
                GL.VertexAttribPointer(MPosition, 3, VertexAttribPointerType.Float, false, 0, ref Position[0].x);
                if ((Colors != null) && (Colors.Length == Position.Length) && (CHandle >= 0))
                {
                    GL.VertexAttribPointer(CHandle, 3, VertexAttribPointerType.Float, false, 0, ref Colors[0].x);
                }

                if ((Texture != null) && (TextureCoords.Length > 0) && (THandle >= 0))
                {
                    GL.VertexAttribPointer(THandle, 2, VertexAttribPointerType.Float, false, 0, ref TextureCoords[0].x);
                }
                if ((NHandle >= 0) && (Normals != null) && (Normals.Length == Position.Length))
                {

                    GL.VertexAttribPointer(NHandle, 3, VertexAttribPointerType.Float, false, 0, ref Normals[0].x);
                }
            }

            // Draw:  

            GL.DrawElements(PrimitiveType.Quads, Indices.Length, DrawElementsType.UnsignedInt, ref Indices[0]);
            OpenGlDevice.CheckError();

            // Attribute freigeben
            if (MPosition >= 0)
                GL.DisableVertexAttribArray(MPosition);
            if (NHandle >= 0)
                GL.DisableVertexAttribArray(NHandle);
            if ((THandle >= 0))
                GL.DisableVertexAttribArray(THandle);
            if ((CHandle >= 0))
                GL.DisableVertexAttribArray(CHandle);
            if (C != null)
                C.SetValue(0);
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {
                Device.Selector.UnRegisterSnapItem(SnapHandle);
            }
        }
    
        Field FieldResolutionFactor = null;
        Field FieldEyePos = null;
        Texture _TessDisplacementMap = null;
        /// <summary>
        /// gets and sets the displacement texture. The value from the red channel of a color ( normalized to [0,1]) in the texture gives the value for a normal translation of the surface.
        /// This value will be multiplied by <see cref="TessDispFactor"/>. See also <see cref="TesselateOn"/>.
        /// </summary>
        public Texture TessDisplacementMap
        { get { return _TessDisplacementMap; }
          set {

                if (value == null)
                {
                   if ( _TessDisplacementMap != null)
                    {
                        _TessDisplacementMap.Tesselation = false;
                        
                    }
                   TesselateOn = false;
                }
                _TessDisplacementMap = value;
                if (value == null) return;
                    if (value.Handle > 0)
                    GL.DeleteTexture(value.Handle);
                OpenGlDevice.CheckError();
                value.Handle = 0;
                value.Tesselation = true;
                TesselateOn = true;
            }            
        }
        /// <summary>
        /// gets and sets a factor for the refinement by the tesselation.
        /// </summary>
        public float TessResolutionFactor = 1f;
        bool _TesselateOn = false;
        /// <summary>
        /// indicates that a tesselation will be made.See also <see cref="TessDisplacementMap"/> and <see cref="TessDispFactor"/>.
        /// </summary>
         bool TesselateOn
        {
            get { return _TesselateOn; }
            set {
                if (value)
                  { if (!_TesselateOn)
                    {if (VAO < 0)
                            ActivateVao();
                    }
                  }
                _TesselateOn = value; 
                        
                 }
        }
        
        /// <summary>
        /// together with the red part of the <see cref="TessDisplacementMap"/> the normal elevation of the surface will be calculated.
        /// </summary>
        public float TessDispFactor = 1;
        /// <summary>
        /// sets and gets the kind of painting.
        /// </summary>
        public BeginMode Primitives = BeginMode.Triangles;
        Field FieldDispFactor = null;
        /// <summary>
        /// overrides the draw method .
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
           
              int StoredObject = -1;
            if (Selector.WriteToSnap)
            {
                CompiledMesh C = this as CompiledMesh;
                if (C != null)
                    StoredObject = C.SnapObject;
                Device.Selector.SetObjectNumber(StoredObject);
            }
            Entity.CurrentEntity = this;
           
          
            if (Compiling)
            {
                MeshCreator.AddProg(this);
                return;
            }
            float PW = Device.PenWidth;
            PenStyles PS = Device.PenStyle;
            Material Save = Device.Material;
            if (Mode == PolygonMode.Line)
            {
                Device.PenWidth = PenWidth;
                Device.PenStyle = PenStyle;
                Device.Emission = PenColor;
            }
            else
            Device.Material = Material;
            PolygonMode SavePolygonMode = Device.PolygonMode;
            Device.PolygonMode = Mode;
            if ((VAO >= 0) && (Device.RenderKind == RenderKind.Render))
            {
                OpenGlDevice.CheckError();
                Field C = null;
                if (Colors != null)
                {
                    C = Device.Shader.getvar("ColorEnabled");
                    if (C != null)
                        C.SetValue(1);
                }
                GL.BindVertexArray(VAO);
                if (!TesselateOn)
                {
                    GL.DrawElements(Primitives, VBOIndices.IndexArray.Length, DrawElementsType.UnsignedInt, 0);
                    OpenGlDevice.CheckError();
                }
                else
                {
                     GLShader SaveShader = Device.Shader;
                    Device.Shader = Device.TesselationtShader;
                     if (FieldResolutionFactor == null) FieldResolutionFactor = Device.Shader.getvar("ResolutionFactor");
                    if (FieldDispFactor == null) FieldDispFactor = Device.Shader.getvar("gDispFactor");
                    if (FieldEyePos == null) FieldEyePos = Device.Shader.getvar("gEyeWorldPos");
                    if (FieldDispFactor != null) FieldDispFactor.SetValue((float)TessDispFactor);
                    if (FieldEyePos != null) FieldEyePos.SetValue(Device.Camera.Position);
                    Device.texture = TessDisplacementMap;
                    //GL.ActiveTexture(TextureUnit.Texture0 + 1);
                    //GL.BindTexture(TextureTarget.Texture2D, TessDisplacementMap.Handle);
                    if (Texture != null)
                    {
                        Device.texture = Texture;
                    }

                    if (FieldResolutionFactor != null)
                    {
                        FieldResolutionFactor.SetValue((float)TessResolutionFactor);
                    }

                    GL.DrawElements(BeginMode.Patches, VBOIndices.IndexArray.Length, DrawElementsType.UnsignedInt, 0);
                    TessDisplacementMap.Hide();
                    Device.Shader = SaveShader;
                    Device.texture = null;

                }

                GL.BindVertexArray(0);
                if (Colors != null)
                {

                    if (C != null)
                        C.SetValue(0);
                }
            }
            else
            {
                if (Primitives == BeginMode.Quads)
                    DrawQuads(Device);
                else
                {
                    Texture SaveTexture = Device.texture;
                    Device.texture = Texture;
                    Device.drawMesh(Indices, VBOPosition.xyzPoints, VBONormals.xyzPoints, VBOTexture.xyPoints, VBOColors.xyzPoints);
                    Device.texture = SaveTexture;
                }
            }
            Device.PolygonMode = SavePolygonMode;
            if (Mode == PolygonMode.Line)
            {
                Device.PenWidth = PW;
                Device.PenStyle = PS;
                Device.Emission = Color.Black;
            }
            Device.Material = Save;
        }
    }
}
