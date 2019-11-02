


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

    public partial class OpenGlDevice
    {
        /// <summary>
        /// gives the kind, with uses <see cref="OpenGlDevice.drawPoint(xyz, double, PointKind)"/>.
        /// </summary>
        public enum PointKind {
            /// <summary>
            /// is a two dimensional marker.
            /// </summary>
            Marker2dim,
            /// <summary>
            /// is a cube.
            /// </summary>
            Cube,
            /// <summary>
            /// is a sphere.
            /// </summary>
            Sphere }
        /// <summary>
        /// draws a sphere with center 0,0,0. See also <see cref="OpenGlDevice.drawSphere(xyz, double)"/>.
        /// </summary>
        /// <param name="Radius">is the radius of the sphere.</param>
        public void drawSphere(double Radius)
        {
            drawSphere(new xyz(0, 0, 0), Radius);
          
        }
      
        void _Sphere(OpenGlDevice Device)
        {
            OpenGlDevice.CheckError();
            Primitives3d.drawSphere(this, 1);
        }
        /// <summary>
        /// draws a sphere with <b>Center</b> and <b>radius</b>.
        /// </summary>
        /// <param name="Center">is the center.</param>
        /// <param name="Radius">is the radius.</param>
        public void drawSphere(xyz Center, double Radius)
        {
            drawSphere(Center, Radius, 32, 16);

        }
        /// <summary>
        /// draws a sphere with <b>Center</b>, <b>radius</b> and the <b>resolution</b>.
        /// </summary>
        /// <param name="Center">is the center.</param>
        /// <param name="Radius">is the radius.</param>
        /// <param name="UResolution">is the u resolution.</param>
        /// <param name="VResolution">is the v resolution.</param>
        public void drawSphere(xyz Center, double Radius, int UResolution, int VResolution)
       {

            if (!Entity.Compiling)
            {

                PushMatrix();
                MulMatrix(Matrix.Translation(Center));
                object Handle = null;
                if ((RenderKind == RenderKind.SnapBuffer))
                {
                    xyz _Center = new xyz(0, 0, 0);
                    SphereSnappItem SI = new SphereSnappItem(Radius);
                    SI.Surface = new Sphere(_Center, Radius);
                    Handle = Selector.RegisterSnapItem(SI);
                }
                Primitives3d.drawSphere(this, Radius, UResolution, VResolution);
                if ((RenderKind == RenderKind.SnapBuffer))
                    Selector.UnRegisterSnapItem(Handle);
                PopMatrix();
            }
            else
            {
                PushMatrix();
                MulMatrix(Matrix.Translation(Center));
                Primitives3d.drawSphere(this, Radius, UResolution, VResolution);
                PopMatrix();
            }
        }
        /// <summary>
        /// draws a torus with <b>inner radius</b> and <b>outer radius</b>
        /// </summary>
        /// <param name="InnerRadius">inner radius.</param>
        /// <param name="OuterRadius">outer radius.</param>
        public void drawTorus(double InnerRadius, double OuterRadius)
        {
            drawTorus(new xyz(0, 0, 0), InnerRadius, OuterRadius);
        }
        /// <summary>
        /// draws a torus with <b>Center</b>, <b>inner radius</b> and <b>outer radius</b>.
        /// </summary>
        /// <param name="Center">is the center.</param>
        /// <param name="InnerRadius">inner radius.</param>
        /// <param name="OuterRadius">outer radius.</param>
        public void drawTorus(xyz Center, double InnerRadius, double OuterRadius)
        {
            object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer) )
            {

                SurfaceSnappItem SI = new SurfaceSnappItem();
                SI.Surface = new Torus(InnerRadius, OuterRadius, Center);

                Handle = Selector.RegisterSnapItem(SI);
            }
            PushMatrix();
            MulMatrix(Matrix.Translation(Center));
            Primitives3d.drawTorus(this, InnerRadius, OuterRadius);
            PopMatrix();

            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                Selector.UnRegisterSnapItem(Handle);
            }
        }
   /// <summary>
   /// draws a cone with <b>Center</b> <b>direction</b> of the axis and the <b>height</b>. See also <see cref="drawCone(double, double, double)"/>
   /// </summary>
   /// <param name="Center">is the base center.</param>
   /// <param name="Direction">is the direction of the axis.</param>
   /// <param name="Radius">is the radius of the base circle.</param>
   /// <param name="Height">is the height.</param>
   /// <param name="HalfAngle">is the half angle of the top.</param>
        public void drawCone(xyz Center, xyz Direction, double Radius, double Height, double HalfAngle)
        {
        
            PushMatrix();
            Matrix Rotation = Base.DoComplete(new xyz(0, 0, 0), Direction).ToMatrix();
            MulMatrix(Matrix.Translation(Center) * Rotation);
            Primitives3d.drawCone(this, Radius, Height, HalfAngle);
            PopMatrix();

        }
        /// <summary>
        /// draws a cone with 0,0,1 as axis and the <b>height</b>. See also <see cref="drawCone(xyz, xyz, double, double, double)"/>
        /// </summary>
        /// <param name="Center">is the base center.</param>
        /// <param name="Radius">is the radius of the base circle.</param>
        /// <param name="Height">is the height.</param>
        /// <param name="HalfAngle">is the half angle of the top.</param>
        public void drawCone(xyz Center, double Radius, double Height, double HalfAngle)
        {
            drawCone(Center, new xyz(0, 0, 1), Radius, Height, HalfAngle);
            
        }
        /// <summary>
        /// draws a cone with with 0,0,1 as axis and the <b>height</b> and the center 0,0,0. See also <see cref="drawCone(double, double, double)"/>
        /// </summary>
        /// <param name="Radius">is the radius of the base circle.</param>
        /// <param name="Height">is the height.</param>
        /// <param name="HalfAngle">is the half angle of the top.</param>
        public void drawCone(double Radius, double Height, double HalfAngle)
        {

            Primitives3d.drawCone(this, Radius, Height, HalfAngle);
        }
        /// <summary>
        /// draws a box at <b>point</b> with a <b>size</b>.
        /// </summary>
        /// <param name="Point">is the point, where the box will be drawn.</param>
        /// <param name="Size">is the size of the box.</param>
        public void drawBox(xyz Point, xyz Size)
        {    
            Primitives3d.drawBox(this, Point, Size);
        }
        /// <summary>
        /// draws a cylinder with <b>center</b>, the <b>direction</b> of the axis. See also <see cref="drawCylinder(xyz, double, double)"/>
        /// </summary>
        /// <param name="Center">is the base center.</param>
        /// <param name="Direction">is the <b>direction</b> of the axis.</param>
        /// <param name="Radius">is the radius.</param>
        /// <param name="Height">is the height.</param>
        public void drawCylinder(xyz Center, xyz Direction, double Radius, double Height)
        {
            drawCone(Center, Direction, Radius, Height, 0);
        }
        /// <summary>
        /// draws a cylinder with <b>center</b>,0,0,1 as axis. See also <see cref="drawCylinder(xyz, xyz, double, double)"/>
        /// </summary>
        /// <param name="Center">is the base center.</param>
        /// <param name="Radius">is the radius.</param>
        /// <param name="Height">is the height.</param>
        public void drawCylinder(xyz Center, double Radius, double Height)
        {
            drawCone(Center, Radius, Height, 0);
        }
        /// <summary>
        /// draws a cylinder with 0,0,0 as <b>center</b> and 0,0,1 as axis. See also <see cref="drawCylinder(xyz,xyz, double, double)"/>
        /// </summary>
        /// <param name="Radius">is the radius.</param>
        /// <param name="Height">is the height.</param>
        public void drawCylinder(double Radius, double Height)
        {
            drawCone(Radius, Height, 0);
        }
        /// <summary>
        /// draws a cone with center 0,0,0 the axis 0,0,1, a <b>height</b> and a <b>radius</b>.
        /// It will be used e.g. from <see cref="drawArc(xy, double, double, double, bool)"/>.
        /// </summary>
        /// <param name="Radius">is the radius.</param>
        /// <param name="Height">is the height.</param>
        public void drawConePointed(double Radius, double Height)
        {
            drawCone(Radius, Height, System.Math.Atan2(Radius, Height));
        }
        /// <summary>
        /// draws an arrow with <b>radius</b> of the shaft and a <b>size</b>. The top is a <b>cone</b>.
        /// You can use it e.g. for the representation of a <see cref="Base"/>.
        /// </summary>
        /// <param name="Radius">radius of the shaft</param>
        /// <param name="Size">size of the arrow.</param>
        public void drawArrow(double Radius, double Size)
        {
            Arc K = new Arc(new xy(0, 0), Radius);
            K.ClockWise = false;
            drawCurve(K);
            drawCylinder(Radius, Size);
            PushMatrix();
            MulMatrix(Matrix.Translation(0, 0, Size));
            drawConePointed(Radius * 1.5, Radius * 2);
            K = new Arc(new xy(0, 0), Radius * 1.5);
            K.ClockWise = false;
            drawCurve(K);
            PopMatrix();
            Entity.CheckCompiling();
        }
        /// <summary>
        /// draws a thre dimensional <see cref="Curve3D"/> relative to a <see cref="Base"/>.
        /// </summary>
        /// <param name="Base">the base for the curve</param>
        /// <param name="Curve">the curve, which will be drawn.</param>
        public void drawCurve(Base Base, Curve3D Curve)
        {
            PushMatrix();
            MulMatrix(Base.ToMatrix());
            drawCurve(Curve);
            PopMatrix();
        }
        /// <summary>
        /// draws a thre dimensional <see cref="Curve3D"/>.
        /// </summary>
        /// <param name="Curve">the curve, which will be drawn.</param>
        public void drawCurve(Curve3D Curve)
        {
            object Handle = null;
            xyzArray A = Curve.ToxyzArray();
            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                CurveSnappItem3D cs = new CurveSnappItem3D(Curve);
                Handle = Selector.RegisterSnapItem(cs);
                cs.PolygonMode = PolygonMode.Line;
            }
            drawPolyLine(A);
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
        }
        /// <summary>
        /// draws a mesh consisting of indices, points, normal vectors, texture coordinates and colors. three sequential indices determe a triangle
        /// points[indices[i]], points[indices[i+1]] and points[indices[i+2]]. In every point a normal vector is given, which is responsible for the lightning.
        /// Texture are coordinates for a <see cref="Texture"/>. For every point you can set a color.
        /// </summary>
        /// <param name="Indices"> are the indices, who determ a triangle points[indices[i]], points[indices[i+1]] and points[indices[i+2]].</param>
        /// <param name="Points">are the points of the mesh.</param>
        /// <param name="Normals">are the normals of the mesh.</param>
        /// <param name="Texture">are texture coordinates.</param>
        /// <param name="Colors">are the colors.</param>
        public void drawMesh(IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture, xyzf[] Colors)
        {
            if (RenderKind == RenderKind.SnapBuffer)
            {
                Mesh Mesh = new Mesh(Indices, Points, Normals, Texture, Colors);
                MeshSnappItem SI = new MeshSnappItem(Mesh);
                object SnapHandle = Selector.RegisterSnapItem(SI);
                if ((Normals!=null) &&(Normals.Length > 0))
                {
                    MeshCreator.MeshdrawTriangles(this, Indices, Points, Normals,Texture);
                    MeshCreator._MeshMode = PolygonMode.Fill;
                }
                else
                {


                    MeshCreator.MeshdrawLined(this, Points);
                    MeshCreator._MeshMode = PolygonMode.Line;
                }

                Selector.UnRegisterSnapItem(SnapHandle);
                return;

            }
            if ((Entity.Compiling) && (RenderKind != RenderKind.SnapBuffer))
            {
                Matrix M = ModelMatrix;
                for (int i = 0; i < Indices.Length; i++)
                {
                    MeshCreator.MeshIndices.Add(Indices[i]);

                }
                //TextureCoords
                if (Texture!=null)
                for (int i = 0; i <Texture.Length; i++)
                {
                    MeshCreator.MeshTextureCoords.Add(Texture[i]);

                }
                //Normals
                for (int i = 0; i < Normals.Length; i++)
                {
                    MeshCreator.MeshNormals.Add(Normals[i]);

                }
               
                // Position
                for (int i = 0; i < Points.Length; i++)
                {
                    MeshCreator.MeshVertices.Add(M * Points[i]);

                }
                return;
            }
           
               if ((Normals!=null)&&(Normals.Length>0))
                    Primitives3d.drawTriangles(this, Indices, Points, Normals, Texture, Colors);
                else
                {
                    
                    Primitives3d.DrawArrayLined(this, Points);
                }
                OpenGlDevice.CheckError();
           
      }
       /// <summary>
       /// draws a pixel as point. See also <see cref="drawPoint(xyz, double, PointKind)"/>
       /// </summary>
       /// <param name="A">coordinates of the point.</param>
        public void drawPoint(xyz A)
        {
            object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                Handle = Selector.RegisterSnapItem(new PointSnappItem(A));

            }
            drawLine(A, A);
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);

        }
        /// <summary>
        /// draws a points with a <see cref="PointKind"/>, whose <b>size</b> <b>color</b> and <b>fill mode</b> can be setted.
        /// </summary>
        /// <param name="A">is the position of the point.</param>
        /// <param name="Size">is the size of the <see cref="PointKind"/></param>
        /// <param name="PointKind">is the <see cref="PointKind"/></param>
        /// <param name="Color">is the color of the <see cref="PointKind"/></param>
        /// <param name="Filled">if you set filled <b>true</b> the <see cref="PointKind"/> will filled.</param>
        public void drawPoint(xyz A, double Size, PointKind PointKind, System.Drawing.Color Color, bool Filled)
        {
            PolygonMode PM = PolygonMode;
            if (Filled)
                PolygonMode = PolygonMode.Fill;
            else
                PolygonMode = PolygonMode.Line;
            drawPoint(A, Size, PointKind, Color);
            PolygonMode = PM;
        }
        /// <summary>
        /// draws a points with a <see cref="PointKind"/>, whose <b>size</b> color <b>color</b>. The fill mode can setted "outside" by <see cref="PolygonMode"/>.
        /// </summary>
        /// <param name="A">is the position of the point.</param>
        /// <param name="Size">is the size of the <see cref="PointKind"/></param>
        /// <param name="PointKind">is the <see cref="PointKind"/></param>
        /// <param name="Color">is the color of the <see cref="PointKind"/></param>
         public void drawPoint(xyz A, double Size, PointKind PointKind, System.Drawing.Color Color)
        {
            bool L = LightEnabled;
            LightEnabled = false;
            System.Drawing.Color C = Emission;
            Emission = Color;
            drawPoint(A, Size, PointKind);
            LightEnabled = L;
            Emission = C;

        }
        /// <summary>
        /// draws a points with a <see cref="PointKind"/>, whose <b>size</b> color <b>color</b>. The fill mode can setted "outside" by <see cref="PolygonMode"/>.
        /// </summary>
        /// <param name="A">is the position of the point.</param>
        /// <param name="Pixels">is the size in pixels of the <see cref="PointKind"/></param>
        /// <param name="PointKind">is the <see cref="PointKind"/></param>
        /// <param name="Color">is the color of the <see cref="PointKind"/></param>
        /// <param name="AsPixels">indicates, that <b>Pixels</b> is in pixelfromat and doesn't change the size by zooming. Only the value <b>true</b> is accepted.
        /// </param>
        public void drawPoint(xyz A, int Pixels, PointKind PointKind, System.Drawing.Color Color,bool AsPixels)
        {
            bool L = LightEnabled;
            LightEnabled = false;
            System.Drawing.Color C = Emission;
            Emission = Color;
            drawPoint(A,PixelToWorld(A,Pixels), PointKind);
            LightEnabled = L;
            Emission = C;

        }
        /// <summary>
        /// draws a box with <b>size</b> ant position <b>A</b>.
        /// </summary>
        /// <param name="A">is position of the point.</param>
        /// <param name="Size">is the size of the box, which will be drawn.</param>
        public void drawPoint(xyz A, double Size)
        {
            object Handle = null;

            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                Handle = Selector.RegisterSnapItem(new PointSnappItem(A));
            }
            drawBox(new xyz(A.x - Size / 2, A.y - Size / 2, A.z - Size / 2), new xyz(Size, Size, Size));
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
       }
        /// <summary>
        /// draws a point a the position <b>A</b> with <see cref="PointKind"/> and his <b>size</b>.
        /// </summary>
        /// <param name="A">is the position of the point.</param>
        /// <param name="Size">is the size of the <see cref="PointKind"/></param>
        /// <param name="PointKind">is the <see cref="PointKind"/></param>
        public void drawPoint(xyz A, double Size, PointKind PointKind)
        {
            object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                Handle = Selector.RegisterSnapItem(new PointSnappItem(A));
                if (PointKind == PointKind.Sphere)
                    drawSphere(A, Size / 2 );
                if (PointKind == PointKind.Cube)
                    drawBox(new xyz(A.x - Size / 2 , A.y - Size / 2 , A.z - Size / 2) , new xyz(Size , Size , Size ));
                if (PointKind == PointKind.Marker2dim)
                    drawBox(new xyz(A.x - Size / 2 , A.y - Size / 2, A.z - Size / 2 ), new xyz(Size , Size , 0));
            }
            else
            {
                if (PointKind == PointKind.Sphere)
                {
                   
                    drawSphere(A, Size / 2);
                }
                if (PointKind == PointKind.Cube)
                    drawBox(new xyz(A.x - Size / 2 , A.y - Size / 2 , A.z - Size / 2 ), new xyz(Size , Size , Size ));
                if (PointKind == PointKind.Marker2dim)
                    drawBox(new xyz(A.x - Size / 2 , A.y - Size / 2 , A.z - Size / 2 ), new xyz(Size , Size , 0));
            }
            if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);

        }
        /// <summary>
        /// draws a line between <b>A</b> and <b>B</b>.
        /// </summary>
        /// <param name="A">is the starting point.</param>
        /// <param name="B">is the end point.</param>
        public void drawLine(xyz A, xyz B)
        {
           object Handle = null;
            if ((RenderKind == RenderKind.SnapBuffer) )
            {
                Handle = Selector.RegisterSnapItem(new LineSnappItem(A, B));
            }
            xyzArray AR = new xyzArray(2);
            AR[0] = A;
            AR[1] = B;
            drawPolyLine(AR);
           if ((RenderKind == RenderKind.SnapBuffer) )
                Selector.UnRegisterSnapItem(Handle);
        }
        /// <summary>
        /// extrudes a <see cref="CurveArray"/> with a <b>height</b>.
        /// </summary>
        /// <param name="CA">is the <see cref="CurveArray"/>, which will be extruded.</param>
        /// <param name="Height">is the height for the extrusion.</param>
        public void drawExtruded(CurveArray CA, double Height)
        {
            Primitives3d.drawExtruded(this, CA, Height);
           
        }
        /// <summary>
        /// extrudes a <see cref="Loca"/> with a <b>height</b>.
        /// </summary>
        /// <param name="L">is the <see cref="Loca"/>, which will be extruded.</param>
        /// <param name="Height">is the height for the extrusion.</param>
        public void drawExtruded(Loca L, double Height)
        {
            for (int i = 0; i < L.Count; i++)
            {
                drawExtruded(L[i], Height);
            }
        }
    }
    /// <summary>
    /// contains a lot of static methods used in the <see cref="OpenGlDevice"/>.
    /// It contains also the acces to opengl.
    /// </summary>
    [Serializable]
    public class Primitives3d
    {
        delegate xyz fun2to3(double u, double v);
        delegate void fun2toTwoPoints(double u, double v, ref xyzf Pt, ref xyzf No);
        static double SphereRadius = 1;
        static double UFactor = System.Math.PI * 2;
        static double VFactor = System.Math.PI;
        static double TorusInnerRadius = 0;
        static double TorusOuterRadius = 0;
        static double ConeRadius = 0;
        static double ConeHalfAngle = 0;
        static xyz ConeFN(double u, double v)
        {
            double Factor = ConeRadius - v * VFactor * System.Math.Tan(ConeHalfAngle);
            double x = System.Math.Cos(u * UFactor) * Factor;
            double y = System.Math.Sin(u * UFactor) * Factor;
            double z = v * VFactor;
            return new xyz(x, y, z);

        }
        static xyz ConeFN_Normal(double u, double v)
        {

            double x = System.Math.Cos(ConeHalfAngle) * System.Math.Cos(u * UFactor);
            double y = System.Math.Cos(ConeHalfAngle) * System.Math.Sin(u * UFactor);
            double z = System.Math.Sin(ConeHalfAngle);
            return new xyz(x, y, z).normalized();
        }
        static xyz TorusFN(double u, double v)
        {
            double x = (TorusInnerRadius * System.Math.Cos(v * VFactor) + TorusOuterRadius) * System.Math.Cos(u * UFactor);
            double y = (TorusInnerRadius * System.Math.Cos(v * VFactor) + TorusOuterRadius) * System.Math.Sin(u * UFactor);
            double z = TorusInnerRadius * System.Math.Sin(v * VFactor);
            return new xyz(x, y, z);

        }
        static xyz TorusFN_Normal(double u, double v)
        {
            double x = (TorusInnerRadius * System.Math.Cos(v * VFactor)) * Utils.Cos(u * UFactor);
            double y = (TorusInnerRadius * System.Math.Cos(v * VFactor)) * Utils.Sin(u * UFactor);
            double z = TorusInnerRadius * System.Math.Sin(v * VFactor);
            return new xyz(x, y, z);

        }
        static xyz SphereFN(double u, double v)
        {
            double x = SphereRadius * Utils.Cos((u - 0.25) * UFactor) * Utils.Sin(v * VFactor);
            double y = SphereRadius * Utils.Sin((u - 0.25) * UFactor) * Utils.Sin(v * VFactor);
            double z = -SphereRadius * Utils.Cos(v * VFactor);
           
            return new xyz(x, y, z);
        }
        static xyz SphereFN_Normal(double u, double v)
        {
            double x = Utils.Cos((u - 0.25) * UFactor) * Utils.Sin(v * VFactor);
            double y = Utils.Sin((u - 0.25) * UFactor) * Utils.Sin(v * VFactor);
            double z = -Utils.Cos(v * VFactor);
            return new xyz(x, y, z);
        }
        static Curve ExtrudeData = null;
        static double ExtrudeHeight = 0;
        static xyz ExtrudeCurveFN(double u, double v)
        {

            xy P = ExtrudeData.Value(u);
            double x = P.x;
            double y = P.y;
            double z = v * ExtrudeHeight;
            return new xyz(x, y, z);
        }
        static xyz ExtrudeCurveFN_Normal(double u, double v)
        {
            xy N = ExtrudeData.Derivation(u).normal() * (-1);
            double x = N.x;
            double y = N.y;
            double z = 0;


            return new xyz(x, y, z);
        }
        static void GetConeGeometrie(int UResolution, int VResolution, double Radius, double Height, double HalfAngle, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Primitives3d.ConeRadius = Radius;
            Primitives3d.ConeHalfAngle = HalfAngle;
            UFactor = System.Math.PI * 2;
            VFactor = Height;
            GetTriangles(ConeFN, ConeFN_Normal, UResolution, VResolution, ref Indices, ref Points, ref Normals, ref Texture);

        }
        static void GetTorusGeometrie(int UResolution, int VResolution, double InnerRadius, double OuterRadius, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Primitives3d.TorusInnerRadius = InnerRadius;
            Primitives3d.TorusOuterRadius = OuterRadius;
            UFactor = System.Math.PI * 2;
            VFactor = System.Math.PI * 2;
           GetTriangles(TorusFN, TorusFN_Normal, UResolution, VResolution, ref Indices, ref Points, ref Normals, ref Texture);
        }
        static Surface CurrentSurface = null;
        static void GetSurfaceGeometrie(Surface Surface, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Primitives3d.CurrentSurface = Surface;
            UFactor = Surface.UFactor;
            VFactor = Surface.VFactor;
            GetTriangles(Surface.GetPointAndNormal, Surface.UResolution, Surface.VResolution, ref Indices, ref Points, ref Normals, ref Texture);

        }
        static void GetSphereGeometrie(double Radius, int UResolution, int VResolution, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Primitives3d.SphereRadius = Radius;
            UFactor = System.Math.PI * 2;
            VFactor = System.Math.PI;
            GetTriangles(SphereFN, SphereFN_Normal, UResolution, VResolution, ref Indices, ref Points, ref Normals, ref Texture);

        }
        static void GetExtrudeCurveGeometrie(Curve Curve, double Height, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            ExtrudeData = Curve;
            ExtrudeHeight = Height;
            UFactor = 1;
            VFactor = 1;
            GetTriangles(ExtrudeCurveFN, ExtrudeCurveFN_Normal, Curve.Resolution, 1, ref Indices, ref Points, ref Normals, ref Texture);
       }
        static xyzArray GetBoxPoints(xyz Point, xyz Size)
        {
            xyzArray Result = new xyzArray(20);
            Result[0] = new xyz(Point.x, Point.y, Point.z);
            Result[1] = new xyz(Point.x + Size.x, Point.y, Point.z);
            Result[2] = new xyz(Point.x + Size.x, Point.y + Size.y, Point.z);
            Result[3] = new xyz(Point.x, Point.y + Size.y, Point.z);
            Result[4] = new xyz(Point.x, Point.y, Point.z);

            Result[5] = new xyz(Point.x, Point.y, Point.z + Size.z);
            Result[6] = new xyz(Point.x, Point.y + Size.y, Point.z + Size.z);
            Result[7] = new xyz(Point.x, Point.y + Size.y, Point.z);
            Result[8] = new xyz(Point.x, Point.y, Point.z);


            Result[9] = new xyz(Point.x, Point.y, Point.z + Size.z);
            Result[10] = new xyz(Point.x + Size.X, Point.y, Point.z + Size.z);
            Result[11] = new xyz(Point.x + Size.X, Point.y, Point.z);
            Result[12] = new xyz(Point.x, Point.y, Point.z);

            Result[13] = new xyz(Point.x + Size.X, Point.y, Point.z);
            Result[14] = new xyz(Point.x + Size.X, Point.y, Point.z + Size.z);
            Result[15] = new xyz(Point.x + Size.X, Point.y + Size.y, Point.z + Size.z);

            Result[16] = new xyz(Point.x, Point.y + Size.y, Point.z + Size.z);
            Result[17] = new xyz(Point.x, Point.y + Size.y, Point.z);
            Result[18] = new xyz(Point.x + Size.X, Point.y + Size.y, Point.z);
            Result[19] = new xyz(Point.x + Size.X, Point.y + Size.y, Point.z + Size.z);
            return Result;
        }
        static void GetBoxGeometrie(xyzf Point, xyzf Size, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Indices = new IndexType[]
                            {0,1,2, 1,0,3,
                             4,5,6, 5,4,7,
                             8,9,10,9,8,11,
                             12,13,14,13,12,15,
                             16,17,18,17,16,19,
                             20,21,22,21,20,23};
            Points = new xyzf[]
            {
            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z+Size.z),
            new xyzf(Point.x,Point.y,Point.z+Size.z),
            new xyzf(Point.x+Size.x,Point.y,Point.z+Size.z),
            new xyzf(Point.x,Point.y+Size.y,Point.z+Size.z),


            new xyzf(Point.x,Point.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y,Point.z),

            new xyzf(Point.x,Point.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y,Point.z+Size.z),
            new xyzf(Point.x+Size.x,Point.y,Point.z),
            new xyzf(Point.x,Point.y,Point.z+Size.z),

            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x,Point.y+Size.y,Point.z+Size.z),
            new xyzf(Point.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z+Size.z),

            new xyzf(Point.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x,Point.y,Point.z+Size.z),
            new xyzf(Point.x,Point.y,Point.z),
            new xyzf(Point.x,Point.y+Size.y,Point.z+Size.z),

            new xyzf(Point.x+Size.x,Point.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z+Size.z),
            new xyzf(Point.x+Size.x,Point.y+Size.y,Point.z),
            new xyzf(Point.x+Size.x,Point.y,Point.z+Size.z)};
            

            Normals = new xyzf[]
              {new xyzf(0,0,1),new xyzf(0,0,1),new xyzf(0,0,1),new xyzf(0,0,1),new xyzf(0,0,-1),
            new xyzf(0,0,-1),new xyzf(0,0,-1),new xyzf(0,0,-1),new xyzf(0,-1,0),new xyzf(0,-1,0),new xyzf(0,-1,0),
            new xyzf(0,-1,0),new xyzf(0,1,0),new xyzf(0,1,0),new xyzf(0,1,0),new xyzf(0,1,0),new xyzf(-1,0,0),
            new xyzf(-1,0,0),new xyzf(-1,0,0),new xyzf(-1,0,0),new xyzf(1,0,0),new xyzf(1,0,0),new xyzf(1,0,0),new xyzf(1,0,0)};

            Texture = new xyf[]
            {
            new xyf(0,0),new xyf(Size.x,+Size.y),new xyf(Size.x,0),
            new xyf(0,+Size.y),new xyf(0,0),new xyf(Size.x,+Size.y),
            new xyf(0,+Size.y),new xyf(Size.x,0),new xyf(0,0),
            new xyf(Size.x,+Size.y),new xyf(Size.x,0),new xyf(0,+Size.y),
            new xyf(0,0),new xyf(Size.x,+Size.y),new xyf(Size.x,0),new xyf(0,+Size.y),new xyf(0,0),new xyf(Size.x,+Size.y),
            new xyf(Size.x,0),new xyf(0,+Size.y),new xyf(0,0),new xyf(Size.x,+Size.y),new xyf(Size.x,0),new xyf(0,+Size.y)
            };
        }
    
       

        /// <summary>
        /// this is the central acces of opengl calls. Only drawLined and drawfourangle acceses also to opengl.
        /// It contains indices, points, normal vectors, texture coordinates and colors. three sequential indices determe a triangle
        /// points[indices[i]], points[indices[i+1]] and points[indices[i+2]]. In every point a normal vector is given, which is responsible for the lightning.
        /// Texture are coordinates for a <see cref="Texture"/>. For every point you can set a color.
        /// </summary>
        /// <param name="Indices"> are the indices, who determ a triangle points[indices[i]], points[indices[i+1]] and points[indices[i+2]].</param>
        /// <param name="Points">are the points of the mesh.</param>
        /// <param name="Normals">are the normals of the mesh.</param>
        /// <param name="Texture">are texture coordinates.</param>
        /// <param name="Colors">are the colors.</param>
        /// <param name="Device">is the <see cref="OpenGlDevice"/> in which will be drawn.</param>
        internal static void drawTriangles(OpenGlDevice Device, IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture, xyzf[] Colors)
        {
            if (Points == null) return;
            Primitives2d.refIndices = Indices;
            Primitives2d.refPoints3d = Points;
            Primitives2d.refNormals = Normals;
            Primitives2d.refTexture = Texture;
           
            if ((Device.RenderKind == RenderKind.SnapBuffer))
            {
                MeshCreator.MeshdrawTriangles(Device, Indices, Points, Normals, Texture);
                return;
            }
            if ((Entity.Compiling))
            {

                MeshCreator.MeshdrawTriangles(Device, Indices, Points, Normals, Texture); // in drawsphere direkt
             
                return;
            }
            int MPosition = Device.Shader.Position.handle;
            if (MPosition >=0)
            GL.EnableVertexAttribArray(MPosition);
            int THandle = Device.Shader.Texture.handle;
            if ((THandle>=0) &&(Texture!=null))
            GL.EnableVertexAttribArray(THandle);
            int NHandle = Device.Shader.Normal.handle;
            if ((NHandle>=0) && (Normals!= null))
            GL.EnableVertexAttribArray(NHandle);
            Field C = Device.Shader.getvar("ColorEnabled");
            if (C!= null)
            C.SetValue(0);
            int CHandle = Device.Shader.Color.handle;
            if ((CHandle >= 0) && (Colors != null))
            {
                if (C!= null)
                C.SetValue(1);
                GL.EnableVertexAttribArray(CHandle);
            }
            // AttributPointer setzen
            if ((MPosition >= 0) && (Points.Length > 0))
                {
                       GL.VertexAttribPointer(MPosition, 3, VertexAttribPointerType.Float, false, 0, ref Points[0].x);
                       if ((Colors != null) && (Colors.Length == Points.Length) && (CHandle >= 0))
                        {
                            GL.VertexAttribPointer(CHandle, 3, VertexAttribPointerType.Float, false, 0, ref Colors[0].x);
                        }

                        if ((Texture != null) && (Texture.Length > 0) && (THandle >= 0))
                        {
                          GL.VertexAttribPointer(THandle, 2, VertexAttribPointerType.Float, false, 0, ref Texture[0].x);
                        }
                        if ((NHandle >= 0)&&(Normals!=null)&&(Normals.Length==Points.Length))
                        {
                           
                            GL.VertexAttribPointer(NHandle, 3, VertexAttribPointerType.Float, false, 0, ref Normals[0].x);
                        }
                   }
            // Ausgabe
            try
            {
                // Standard smallshader oder largeshader
                if (!Selector.WriteToSnap)
                {
                   
                        if (sizeof(IndexType) == 4)
                            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, ref Indices[0]);
                        else
                            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedShort, ref Indices[0]);

                   
                    OpenGlDevice.CheckError();
                }
                else // Pickingshader
                {
                    int ct = Indices.Length;
                    if (Device.PolygonMode == PolygonMode.Fill)
                    {
                        if (sizeof(IndexType) == 4)
                        {
                            if (Device.Selector.IntersectionUsed)
                                for (uint i = 0; i < ct; i += 3)
                                {
                                    Device.Selector.SetPrimId(i);
                                 
                                    GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, ref Indices[i]);
                                }
                            else
                                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedShort, ref Indices[0]);
                        }
                        else
                            if (Device.Selector.IntersectionUsed)
                        {
                            for (uint i = 0; i < ct; i += 3)
                            {
                                Device.Selector.SetPrimId(i);
                                
                                GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, ref Indices[i]);
                            }
                        }
                        else
                            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedShort, ref Indices[0]);
                    }
                    else
                    { // Line modus picking buffer
                        if (sizeof(IndexType) == 4)
                            for (int i = 0; i < Indices.Length; i = i + 3)
                            {

                                GL.DrawElements(PrimitiveType.LineLoop, 3, DrawElementsType.UnsignedInt, ref Indices[i]);

                            }
                        else
                            for (int i = 0; i < Indices.Length; i = i + 3)
                            {

                                GL.DrawElements(PrimitiveType.LineLoop, 3, DrawElementsType.UnsignedShort, ref Indices[i]);

                            }
                    }
                }
               

            }
            catch (System.Exception E)
            {
                System.Windows.Forms.MessageBox.Show(E.Message);


            }
            // Attribute freigeben
            if (MPosition >=0)
            GL.DisableVertexAttribArray(MPosition);
            if (NHandle >= 0)
                GL.DisableVertexAttribArray(NHandle);
            if ((THandle >= 0))
                GL.DisableVertexAttribArray(THandle);
            if ((CHandle >= 0))
                GL.DisableVertexAttribArray(CHandle);
            if (C != null)
                C.SetValue(0);

        }
        /// <summary>
        /// is called from sufaces to draw fourangles instead triangles
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Indices"></param>
        /// <param name="Points"></param>
        /// <param name="Normals"></param>
        /// <param name="Texture"></param>
        internal static void drawFouranglesLined(OpenGlDevice Device, IndexType[] Indices, xyzf[] Points, xyzf[] Normals, xyf[] Texture)
        {  // wird von surfaces verwendet, die im Line modus gezeichnet werden
            // zeichnet keine 4 ecke mehr
            Primitives3d.drawTriangles(Device, Indices, Points, Normals, Texture, null);

       
        }

        static void GetTriangles(fun2to3 fnPoint, fun2to3 fnNormal, int UResolution, int VResolution, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Points = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Normals = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Texture = new xyf[(UResolution + 1) * (VResolution + 1)];
            Indices = new IndexType[(UResolution) * (VResolution) * 6];
            double xStep = 1D / (double)UResolution;
            double yStep = 1D / (double)VResolution;
            double _u = 0;
            double _v = 0;
            int ptId = 0;

            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                _v = 0;
                for (int v = 0; v < VResolution + 1; v++)
                {
                    xyz Pt = fnPoint(_u, _v);
                    xyz N = fnNormal(_u, _v);
                    Points[ptId] = new xyzf((float)Pt.x, (float)Pt.y, (float)Pt.z);
                    if (Points[ptId].x < 0.00001)
                    {
                        if ((u != 0) && (u != 1))
                        { }
                    }
                    Normals[ptId] = new xyzf((float)N.x, (float)N.y, (float)N.z);
                    Texture[ptId] = new xyf((float)_u, (float)_v);

                    ptId++;


                    _v += yStep;
                }
                _u += xStep;
            }

            for (int u = 0; u < UResolution; u++)
            {
                for (int v = 0; v < VResolution; v++)
                {
                    Indices[6 * u * (VResolution) + 6 * v] = (IndexType)(u * (VResolution + 1) + v);
                    Indices[6 * u * (VResolution) + 6 * v + 1] = (IndexType)(u * (VResolution + 1) + v + 1);
                    Indices[6 * u * (VResolution) + 6 * v + 2] = (IndexType)((u + 1) * (VResolution + 1) + v);

                    Indices[6 * u * (VResolution) + 6 * v + 3] = (IndexType)((u + 1) * (VResolution + 1) + v);
                    Indices[6 * u * (VResolution) + 6 * v + 4] = (IndexType)((u) * (VResolution + 1) + v + 1);
                    Indices[6 * u * (VResolution) + 6 * v + 5] = (IndexType)((u + 1) * (VResolution + 1) + v + 1);
                }

            }
       }
        static void GetTriangles(fun2toTwoPoints PointsAndormal, int UResolution, int VResolution, ref IndexType[] Indices, ref xyzf[] Points, ref xyzf[] Normals, ref xyf[] Texture)
        {
            Points = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Normals = new xyzf[(UResolution + 1) * (VResolution + 1)];
            Texture = new xyf[(UResolution + 1) * (VResolution + 1)];
            Indices = new IndexType[(UResolution) * (VResolution) * 6];
            double xStep = 1D / (double)UResolution;
            double yStep = 1D / (double)VResolution;
            double _u = 0;
            double _v = 0;
            int ptId = 0;

            for (int u = 0; u < UResolution + 1; u++)
            {
                int URow = u * (VResolution + 1);
                _v = 0;
                for (int v = 0; v < VResolution + 1; v++)
                {
                    xyzf Pt = new xyzf(0, 0, 0);
                    xyzf PN = new xyzf(0, 0, 0);

                    PointsAndormal(_u, _v, ref Pt, ref PN);

                    Points[ptId] = Pt;
                    Normals[ptId] = PN;
                    Texture[ptId] = new xyf((float)_u, (float)_v);

                    ptId++;


                    _v += yStep;
                }
                _u += xStep;
            }

            for (int u = 0; u < UResolution; u++)
            {
                for (int v = 0; v < VResolution; v++)
                {
                    Indices[6 * u * (VResolution) + 6 * v] = (IndexType)(u * (VResolution + 1) + v);
                    Indices[6 * u * (VResolution) + 6 * v + 1] = (IndexType)(u * (VResolution + 1) + v + 1);
                    Indices[6 * u * (VResolution) + 6 * v + 2] = (IndexType)((u + 1) * (VResolution + 1) + v);

                    Indices[6 * u * (VResolution) + 6 * v + 3] = (IndexType)((u + 1) * (VResolution + 1) + v);
                    Indices[6 * u * (VResolution) + 6 * v + 4] = (IndexType)((u) * (VResolution + 1) + v + 1);
                    Indices[6 * u * (VResolution) + 6 * v + 5] = (IndexType)((u + 1) * (VResolution + 1) + v + 1);


                }

            }
       }
        // 1. drawbox,drawPolyPolyLine(Loxyz Loxyz),drawPolyLine(xyzArray Array)
        internal static void drawArrayLined(OpenGlDevice Device, xyzArray Array)
        {

            if (Array.Count == 0) return;
            if (Entity.Compiling)
            {
                MeshCreator.MeshdrawLined(Device, Array);
                return;
            }
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {
                MeshCreator.MeshdrawLined(Device, Array);
                return;
            }
            int MPosition = Device.Shader.Position.handle;
          
            if (MPosition >= 0)
            {
                GL.EnableVertexAttribArray(MPosition);
                xyzf[] A = Array.ToFArray();
                GL.VertexAttribPointer(MPosition, 3, VertexAttribPointerType.Float, false, 0, ref A[0].x);
                int NPosition = Device.Shader.Normal.handle;
                if (NPosition >= 0)
                {
                    GL.EnableVertexAttribArray(NPosition);
                    xyzf[] Norm = new xyzf[Array.Count];
                    GL.VertexAttribPointer(NPosition, 3, VertexAttribPointerType.Float, false, 0, ref Norm[0].x);
                }
                GL.DrawArrays(PrimitiveType.LineStrip, 0, Array.Count);

                GL.DisableVertexAttribArray(MPosition);
                if (NPosition >= 0)
                    GL.DisableVertexAttribArray(NPosition);
            }
           
        }
        // mesh
        internal static void DrawArrayLined(OpenGlDevice Device, xyzf[] A)
        {
            int MPosition = Device.Shader.Position.handle;
            if (MPosition >= 0)
            {  
                if ((Device.RenderKind == RenderKind.SnapBuffer))
                {
                    MeshCreator.MeshdrawLined(Device, A);
                }
                else
                     if (Entity.Compiling)
                {
                    if (MeshCreator.MeshMode == PolygonMode.Fill) MeshCreator.Renew();
                    int ID = MeshCreator.MeshVertices.Count;
                    for (int i = 0; i < A.Length; i++)
                        MeshCreator.MeshIndices.Add((IndexType)(i + ID));
                    for (int i = 0; i < A.Length; i++)
                        MeshCreator.MeshVertices.Add(((A[i])));
                    MeshCreator.MeshMode = PolygonMode.Line;
                    MeshCreator.Renew();
                    return;
                }
                if (Selector.WriteToSnap)
                    Device.Selector.SetPrimId(0);
                GL.EnableVertexAttribArray(MPosition);
                GL.VertexAttribPointer(MPosition, 3, VertexAttribPointerType.Float, false, 0, ref A[0].x);
                int NPosition = Device.Shader.Normal.handle;
                if (NPosition >= 0)
                {
                    GL.EnableVertexAttribArray(NPosition);
                    xyzf[] Norm = new xyzf[A.Length];
                    GL.VertexAttribPointer(NPosition, 3, VertexAttribPointerType.Float, false, 0, ref Norm[0].x);
                }
                GL.DrawArrays(PrimitiveType.LineStrip, 0, A.Length);
                GL.DisableVertexAttribArray(MPosition);
                if (NPosition >= 0)
                    GL.DisableVertexAttribArray(NPosition);
            }
        }
        /// <summary>
        /// internal. for device.drawSphere.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Radius"></param>
        internal static void drawSphere(OpenGlDevice Device, double Radius)
        {
            OpenGlDevice.CheckError();
            drawSphere(Device, Radius, 32, 16);
            
        }
        /// <summary>
        /// internal. for device.drawSphere.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Radius"></param>
        /// <param name="Uresolution"></param>
        /// <param name="Vresolution"></param>
        internal static void drawSphere(OpenGlDevice Device, double Radius, int Uresolution, int Vresolution)
        {
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            GetSphereGeometrie(Radius, Uresolution, Vresolution, ref Indices, ref Points, ref Normals, ref Texture);
            drawTriangles(Device, Indices, Points, Normals, Texture, null);
            Entity.CheckCompiling();
        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="InnerRadius"></param>
        /// <param name="OuterRadius"></param>
        internal static void drawTorus(OpenGlDevice Device, double InnerRadius, double OuterRadius)

        {
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;

            GetTorusGeometrie(32, 32, InnerRadius, OuterRadius, ref Indices, ref Points, ref Normals, ref Texture);
            drawTriangles(Device, Indices, Points, Normals, Texture, null);
            Entity.CheckCompiling();
       }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Point"></param>
        /// <param name="Size"></param>
        internal static void drawBox(OpenGlDevice Device, xyz Point, xyz Size)
       {
            object Handle = null;
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            if ((Device.RenderKind == RenderKind.SnapBuffer))
                Handle = Device.Selector.RegisterSnapItem(new BoxSnappItem(Point, Size));
            if (Device.PolygonMode == PolygonMode.Line)
            {
                xyzArray A = GetBoxPoints(Point, Size);
                drawArrayLined(Device, A);

            }

            else
            {
                GetBoxGeometrie(Point.toXYZF(), Size.toXYZF(), ref Indices, ref Points, ref Normals, ref Texture);
                drawTriangles(Device, Indices, Points, Normals, Texture, null);
            }
            if ((Device.RenderKind == RenderKind.SnapBuffer) )
            {
                Device.Selector.UnRegisterSnapItem(Handle);
                  if (Entity.Compiling) MeshCreator.Renew();  // War aus geklammert, man konnte die Buchstaben nicht fangen
            }

            Entity.CheckCompiling();
        }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="CA"></param>
        /// <param name="Height"></param>
        internal static void drawExtruded(OpenGlDevice Device, CurveArray CA, double Height)
        {
            List<xyzf> PointsList = new List<xyzf>();
            List<xyzf> NormalList = new List<xyzf>();
            List<xyf> TextureList = new List<xyf>();
            List<IndexType> IndicesList = new List<IndexType>();
            object Handle = null;
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            
            for (int i = 0; i < CA.Count; i++)
            {
                if ((Device.RenderKind == RenderKind.SnapBuffer) )
                {
                    SurfaceSnappItem SI = new SurfaceSnappItem();
                    SI.OfObject = CA;
                  
                    CurveExtruder CE = new CurveExtruder();
                    CE.Height = Height;
                    CE.Curve = CA[i];

                    SI.Surface = CE;

                    Handle = Device.Selector.RegisterSnapItem(SI);
                }
                GetExtrudeCurveGeometrie(CA[i], Height, ref Indices, ref Points, ref Normals, ref Texture);
                drawTriangles(Device, Indices, Points, Normals, Texture,null);

                if ((Device.RenderKind == RenderKind.SnapBuffer))
                {
                    Device.Selector.UnRegisterSnapItem(Handle);
                    //if (Entity.Compiling)
                    //    MeshCreator.Renew();

                }
                Points = Primitives2d.refPoints3d;
                Normals = Primitives2d.refNormals;
                Texture = Primitives2d.refTexture;
                Indices = Primitives2d.refIndices;
                IndexType ct = (IndexType)PointsList.Count;
                for (int j = 0; j < Indices.Length; j++)
                {
                   
                       IndicesList.Add((IndexType)( Indices[j] + ct));
                }
                for (int j = 0; j < Points.Length; j++)
                {
                    PointsList.Add(Points[j]);
                }
                for (int j = 0; j < Normals.Length; j++)
                {
                    NormalList.Add(Normals[j]);
                }
                for (int j = 0; j < Texture.Length; j++)
                {
                    TextureList.Add(Texture[j]);
                }

            }
            Primitives2d.refPoints3d = PointsList.ToArray();
            Primitives2d.refNormals = NormalList.ToArray();
            Primitives2d.refTexture = TextureList.ToArray();
            Primitives2d.refIndices = IndicesList.ToArray();
         }
        /// <summary>
        /// internal.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Radius"></param>
        /// <param name="Height"></param>
        /// <param name="HalfAngle"></param>
        public static void drawCone(OpenGlDevice Device, double Radius, double Height, double HalfAngle)
        {
            object Handle = null;
            IndexType[] Indices = null;
            xyzf[] Points = null;
            xyzf[] Normals = null;
            xyf[] Texture = null;
            GetConeGeometrie(40, 1, Radius, Height, HalfAngle, ref Indices, ref Points, ref Normals, ref Texture);
            //  GetSphereTriangles(Radius, 3, 2, ref Indices, ref Points, ref Normals, ref Texture);
            if ((Device.RenderKind == RenderKind.SnapBuffer))
            {

                SurfaceSnappItem SI = new SurfaceSnappItem();
                SI.Surface = new Cone(new xyz(0, 0, 0), Radius, Height, HalfAngle);
                //     SI.Surface.Base = (Matrix.Translation(Center) * Rotation).toBase();
                Handle = Device.Selector.RegisterSnapItem(SI);
            }
            drawTriangles(Device, Indices, Points, Normals, Texture,null);
            if ((Device.RenderKind == RenderKind.SnapBuffer) )
            {
                Device.Selector.UnRegisterSnapItem(Handle);
            }
            Entity.CheckCompiling();
       }
    }
}