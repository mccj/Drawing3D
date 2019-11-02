using System.Collections.Generic;
using System;
using OpenTK.Graphics.OpenGL;
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    [Serializable]
    public partial class OpenGlDevice
    {   
    /// <summary>
    /// draws a circle with center, radius and orientation.
    /// </summary>
    /// <param name="Mp">center.</param>
    /// <param name="Radius">radius.</param>
    /// <param name="Orientation">orientation.</param>
        public void drawCircle(xy Mp,double Radius, bool Orientation)
        {
            Arc A = new Arc();
            A.Alfa = 0;
            A.Beta = 2 * System.Math.PI;
            A.ClockWise = Orientation;
            A.Center = Mp;
            A.aRadius = Radius;
            A.bRadius = Radius;
            
            drawCurve(A);
            Entity.CheckCompiling();
        }
        /// <summary>
        /// draws an arc width center, radius, startangle, endangle and orientation.
        /// </summary>
        /// <param name="Mp">center.</param>
        /// <param name="Radius">radius.</param>
        /// <param name="Alfa">start angle.</param>
        /// <param name="Beta">end angle.</param>
        /// <param name="Orientation">orientation.</param>
        public void drawArc(xy Mp, double Radius,double Alfa,Double Beta, bool Orientation)
        {
            Arc A = new Arc();
            A.Alfa = Alfa;
            A.Beta = Beta;
            A.ClockWise = Orientation;
            A.Center = Mp;
            A.aRadius = Radius;
            A.bRadius = Radius;

            drawCurve(A);

        }
    
       
        /// <summary>
        /// draws a list of 2d-curve lists. See also <see cref="Loca"/>.
        /// </summary>
        /// <param name="Loca">list of curve lists.</param>
        public void drawPolyPolyCurve(Loca Loca)
        {
            Loxy L = new Loxy();
            for (int i = 0; i < Loca.Count; i++)
                L.Add(Loca[i].getxyArray());
            _drawPolyPolyCurve(L, Loca, 1);

        }
        /// <summary>
        /// draws a list of <see cref="xyArray"/>s.
        /// </summary>
        /// <param name="Loxy">is a <see cref="Loxy"/>, which will be drawn.</param>
        public void drawPolyPolyLine(Loxy Loxy)
        {
            _drawPolyPolyCurve(Loxy, null, 0);
        }
        SnappItem getSI(int id, Loca Loca, Loxy Loxy, int path)
        {
            if (id == 0)
            {
                PolyPolyLineSnappItem SI = new PolyPolyLineSnappItem(Loxy);
                SI.Path = path;
                return SI;
            }
            else
            {
                PolyPolyCurveSnappItem SI = new PolyPolyCurveSnappItem(Loca);
                SI.Path = path;
                return SI;
            }
        }
         void _drawPolyPolyCurve(Loxy L, Loca Loca, int id)
        {
            Object Handle = null;
            if (PolygonMode == PolygonMode.Fill)
            {

                   if ((RenderKind == RenderKind.SnapBuffer))
                    Handle = Selector.RegisterSnapItem(getSI(id, Loca, L, -1));
                drawFilledArray2d(this, L);

                if ((RenderKind == RenderKind.SnapBuffer))
                {
                    MeshCreator.Renew();
                    Selector.UnRegisterSnapItem(Handle);
                }
                return;
            }
            
            if ((RenderKind == RenderKind.SnapBuffer))
            {
                SnappItem S = (getSI(id, Loca, L, -1));

                Handle = Selector.RegisterSnapItem(S);
            }
            {
                for (int i = 0; i < L.Count; i++)
                {
                    //IndexType[] Indices = new IndexType[L[i].Count];
                    //for (int k = 0;k < L[i].Count; k++)
                    //{
                    //    Indices[k] = k;
                    //}
                    //xyzf[] Points = new xyzf[L[i].Count];
                    //for (int k = 0; k < L[i].Count; k++)
                    //{
                    //    Points[k] = new xyzf((float)L[i][k].X, (float)L[i][k].y, 0f);
                    //}
                    //Primitives3d.drawTriangles(this,Indices, Points, null, null, null);
                    ////if ((RenderKind == RenderKind.SnapBuffer))
                    ////  Selector.ToSnapBuffer((uint)i);
                    Primitives2d.drawArrayLined(this, L[i]);
               
                }
              

            }
            if ((RenderKind == RenderKind.SnapBuffer))
                Selector.UnRegisterSnapItem(Handle);
        }
        /// <summary>
        /// gets and sets the <see cref="Selector"/>
        /// </summary>
        public Selector Selector = new Selector();
        /// <summary>
        /// draws a list of 2D curves. See also <see cref="CurveArray"/>
        /// </summary>
        /// <param name="CA"><see cref="CurveArray"/></param>
        public void drawPolyCurve(CurveArray CA)
        {
            xyArray A=CA.getxyArray();
            Object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer))
             Handle=   Selector.RegisterSnapItem(new PolyCurveSnappItem(CA));
              drawPolyLine(A);
            if ((RenderKind == RenderKind.SnapBuffer))
                Selector.UnRegisterSnapItem(Handle);
        }
        /// <summary>
        /// draws a <see cref="Surface"/> by calling <see cref="Surface.Paint(OpenGlDevice)"/>.
        /// It is just the same, when you call Surface.Paint(OpenGlDevice).
        /// </summary>
        /// <param name="Surface"></param>
        public void drawSurface(Surface Surface)
        {
            Surface.Paint(this);
        }
        /// <summary>
        /// draws a 2D curve.
        /// </summary>
        /// <param name="C"><see cref="Curve"/></param>
        public void drawCurve(Curve C)
        {
            xyArray A = C.ToXYArray();
            PolygonMode save = PolygonMode;
            if (A.Count == 2) PolygonMode = PolygonMode.Line;
            Object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer) )
             Handle=   Selector.RegisterSnapItem(new CurveSnappItem(C));
            drawPolyLine(A);
            if ((RenderKind == RenderKind.SnapBuffer))
                Selector.UnRegisterSnapItem(Handle);
            PolygonMode = save;
        }
        /// <summary>
        /// draws a rectangle withe edge points A and B.
        /// </summary>
        /// <param name="A">1. point of the rectangle.</param>
        /// <param name="B">2. point of the rectangle.</param>
        public void drawRectangle(xy A, xy B)
        {
            xyArray AR = new xyArray(5);
            AR[0] = A;
            AR[1] = new xy(A.x, B.y);
            AR[2] = B;
            AR[3] = new xy(B.x, A.y);
            AR[4] = A;
            drawPolyLine(AR);

        }
        /// <summary>
        /// draws a 2d-polygon, which is stored in Array. See also <see cref="xyArray"/>.
        /// </summary>
        /// <param name="Array"><see cref="xyArray"/>.</param>
        public void drawPolyLine(xyArray Array)
        {
            Object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer))
             Handle =   Selector.RegisterSnapItem(new PolyLineSnappItem(Array));
            if (PolygonMode == PolygonMode.Fill)
            {
                Loxy L = new Loxy();
                L.Add(Array);

                  drawFilledArray2d(this,L);
                if ((RenderKind == RenderKind.SnapBuffer))
                    Selector.UnRegisterSnapItem(Handle);
                if (PolygonMode == PolygonMode.Fill)
                    return;
            }
            Primitives2d.drawArrayLined(this, Array);
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
        }
        /// <summary>
        /// draws a 3d-polygon, which is stored in Array. See also <see cref="xyzArray"/> if the <see cref="OpenGlDevice.PolygonMode"/> = <b>Fill</b> and the array is planar then
        /// the polygon is drawn as filled polygon else as a lined polygon.
        /// </summary>
        /// <param name="Array"><see cref="xyzArray"/>hollds the array information</param>
        public void drawPolyLine(xyzArray Array)
        {
            Object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer))
                Handle = Selector.RegisterSnapItem(new PolyLineSnappItem3D(Array));
            if ((PolygonMode == PolygonMode.Fill) &&(Array.Count>2)&&(Array.planar))
            {
                Loxy L = new Loxy();
                PushMatrix();
                MulMatrix(Array.Base.ToMatrix());
                xyArray A = Array.ToxyArray();
                L.Add(A);
                drawFilledArray2d(this, L);
                PopMatrix();
                if ((RenderKind == RenderKind.SnapBuffer) )
                    Selector.UnRegisterSnapItem(Handle);
                if (PolygonMode == PolygonMode.Fill)
                    return;
            }
           

            Primitives3d.drawArrayLined(this, Array);
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
        }

        /// <summary>
        /// draws a list of <see cref="xyzArray"/>.The first array is the outer contur the other are holes.
        /// </summary>
        /// <param name="Loxyz">a list of <see cref="xyzArray"/></param>
        public void drawPolyPolyLine(Loxyz Loxyz)
        {







            Object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer))
                Handle = Selector.RegisterSnapItem(new PolyPolyLineSnappItem3D(Loxyz));
            if ((PolygonMode == PolygonMode.Fill)&&(Loxyz.planar))
            {
                Loxy L= Loxyz.ToLoxy();
                PushMatrix();
                MulMatrix(Loxyz.Base.ToMatrix());
                drawFilledArray2d(this, L);
                PopMatrix();
                if ((RenderKind == RenderKind.SnapBuffer) )
                    Selector.UnRegisterSnapItem(Handle);
             
                    return;
            }
          
            for (int i = 0; i < Loxyz.Count; i++)
            {
                Primitives3d.drawArrayLined(this, Loxyz[i]);
            }
           
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
        }

      

        private void drawFilledArray2d(OpenGlDevice Device, Loxy L)
        {
            if (L.Count == 0) return;
           double xMax = -10000000;
            double yMax = -10000000;
            double xMin = 10000000;
            double yMin = 10000000;
            if ((Device.texture != null) && (Device.texture.SoBigAsPossible != Drawing3d.Texture.BigAsPosible.None))

            {
                for (int i = 0; i < L[0].Count; i++)
                {
                    if (L[0][i].X > xMax) xMax = L[0][i].X;
                    if (L[0][i].X < xMin) xMin = L[0][i].X;
                    if (L[0][i].Y > yMax) yMax = L[0][i].Y;
                    if (L[0][i].Y < yMin) yMin = L[0][i].Y;
                }
                
            }
            double xDiff = xMax - xMin;
            double yDiff = yMax - yMin;

       
            List<IndexType> Indices = new List<IndexType>();
            
            xyf[] Points = null;
            L.TriAngulation(Indices, ref Points);
            xyf[] Texture = new xyf[Points.Length];
            float Aspectx = 1;
            float Aspecty = 1;
            if ((Device.texture != null) && (Device.texture.SoBigAsPossible != Drawing3d.Texture.BigAsPosible.None))

            {

                if (Device.texture.Bitmap != null)
                {
                    double w = (float)Device.texture.Bitmap.Width / Device.PixelsPerUnit;
                    double h = (float)Device.texture.Bitmap.Height / Device.PixelsPerUnit;
                    if (Device.texture.KeepAspect)
                        if (Device.texture.SoBigAsPossible == Drawing3d.Texture.BigAsPosible.Height)
                        {
                            Aspectx = (float)(h / w);
                        }
                        else
                            Aspecty = (float)(w / h);
                }
                for (int i = 0; i < Points.Length; i++)
                {

                    if (Device.texture.KeepAspect)
                    {
                        if (Aspecty != 1)
                            Texture[i] = new xyf((Points[i].x - (float)xMin) / (float)(xDiff), (Points[i].y - (float)yMin) / (float)(xDiff / Aspecty));
                        if (Aspectx != 1)
                            Texture[i] = new xyf((Points[i].x - (float)xMin) / (float)(yDiff / Aspectx), (Points[i].y - (float)yMin) / (float)(yDiff));
                    }
                    else
                        Texture[i] = new xyf((Points[i].x - (float)xMin) / (float)(xDiff), (Points[i].y - (float)yMin) / (float)(yDiff));

                }
            }
            else
                for (int i = 0; i < Points.Length; i++)
                {
                    Texture[i] = Points[i] + Points[0] * (-1);
                }
           
           Primitives2d.drawTriangles2d(Device, Indices, Points, Texture);
        }
    }

    /// <summary>
    /// all members are static.they use the shader and the <see cref="OpenGlDevice"/>.
    /// </summary>
    [Serializable]
    public class Primitives2d
    {   internal static IndexType[] refIndices;
        internal static xyzf[] refPoints3d;
        internal static xyf[] refPoints2d;
        internal static xyzf[] refNormals;
        internal static xyf[] refTexture;
      /// <summary>
      /// internal.
      /// </summary>
      /// <param name="Device"></param>
      /// <param name="A"></param>
        public static void drawArrayLined(OpenGlDevice Device, xyArray A)
        {

           
           
            if (Entity.Compiling)
            {
                PolygonMode P = Device.PolygonMode;
                MeshCreator.MeshdrawLined(Device, A);
                Device.PolygonMode= P;
                return;
            }
            if (Device.RenderKind== RenderKind.SnapBuffer)
            {
                MeshCreator.MeshdrawLined(Device, A);
                return;
            }
         
          
            int MPosition = Device.Shader.Position.handle;
            if (MPosition >= 0)
            {
               
                GL.EnableVertexAttribArray(MPosition);
               
                xyf[] Array = (A.ToFloatArray());
                GL.VertexAttribPointer(MPosition, 2, VertexAttribPointerType.Float, false, 0, ref Array[0].x);
                GL.DrawArrays(PrimitiveType.LineStrip, 0, Array.Length);
                GL.DisableVertexAttribArray(MPosition);
              

            }
        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Indices"></param>
        /// <param name="Points"></param>
        /// <param name="Texture"></param>
        public static void drawTriangles2d(OpenGlDevice Device, List<IndexType> Indices, xyf[] Points, xyf[] Texture)
        {//ok
            refIndices = Indices.ToArray();
            refPoints2d = Points;
            
            
            refTexture = Texture;
            double dir = 1;
            for (int i = 0; i < Indices.Count; i += 3)
            {
                xy A = new xy(Points[Indices[i + 1]].x - Points[Indices[i]].x, Points[Indices[i + 1]].y - Points[Indices[i]].y);
                xy B = new xy(Points[Indices[i + 2]].x - Points[Indices[i]].x, Points[Indices[i + 2]].y - Points[Indices[i]].y);
                double F = A & B;
                if (System.Math.Abs(F) > 0.0001)
                {
                    if (F < 0) dir = 1; // schaut welche Richtung der Normal vektor haben muss
                    else dir = -1;
                   break;
                }
            }
            xyzf[] Normals = new xyzf[Points.Length];
            for (int i = 0; i < Points.Length; i++)
                Normals[i] = new xyz(0, 0, dir).toXYZF();
            xyzf[] _Points = new xyzf[Points.Length];
            for (int i = 0; i < Points.Length; i++)
                _Points[i] = new xyzf(Points[i].x, Points[i].y, 0);
            refNormals = Normals;
            Primitives3d.drawTriangles(Device, Indices.ToArray(), _Points, Normals, Texture, null);
          
       }
    }
}

