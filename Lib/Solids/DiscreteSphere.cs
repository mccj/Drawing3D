using System;
using System.Collections.Generic;

using System.Text;


namespace Drawing3d
{ 
    /// <summary>
    /// defines a sphere as <see cref="DiscreteSolid"/> with <see cref="Radius"/> and <see cref="Center"/>.
    /// </summary>
    [Serializable]
    public class DiscreteSphere : DiscreteSolid
    { 
        /// <summary>
       /// empty constructor.
       /// </summary>
        public DiscreteSphere():base()
        {
        }
        /// <summary>
        /// constructor with Radius.<see cref="Center"/> is 0,0,0. UResolution and VResolution are 20.
        /// </summary>
        /// <param name="Radius">the <see cref="Radius"/>.</param>
        public DiscreteSphere(double Radius):this(new xyz(0,0,0),Radius,20,20)
         
        {
           
          
        }
        /// <summary>
        /// constructor with Center and Radius. UResolution and VResolution are setted to 20.
        /// </summary>
        /// <param name="Center">the <see cref="Center"/>.</param>
        /// <param name="Radius">the <see cref="Radius"/>.</param>
        public DiscreteSphere(xyz Center, double Radius)
            : this(Center, Radius, 20, 20)
        {

                
        }
        /// <summary>
        /// constructor with Center, Radius, UResolution and VResolution.
        /// </summary>
        /// <param name="Center">the <see cref="Center"/>.</param>
        /// <param name="Radius">the <see cref="Radius"/>.</param>
        /// <param name="UResolution">the <see cref="UResolution"/>.</param>
        /// <param name="VResolution">the <see cref="VResolution"/>.</param>
        public DiscreteSphere(xyz Center, double Radius, int UResolution, int VResolution)
            : base()
        {
            this.UResolution = UResolution;
            this.VResolution = VResolution;
            this.Radius = Radius;
            Transformation = Matrix.Translation(Center);
            Refresh();
        }
        /// <summary>
        /// <b>Center</b> of the sphere.
        /// </summary>
        public xyz Center
        {
            get { return Transformation * new xyz(0, 0, 0); }
            set { Transformation = Matrix.Translation(value); }
        }
        private double _radius;
        /// <summary>
        /// <b>radius</b> of the sphere.
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
        int _UResolution = 20;
        /// <summary>
        /// the resolution in x direction. Default = 20.
        /// </summary>
        public int UResolution
        { get { return _UResolution; } set { _UResolution = value; } }
        int _VResolution = 20;
        /// <summary>
        /// the resolution in y direction. Default = 20.
        /// </summary>
        public int VResolution
        { get { return _VResolution; } set { _VResolution = value; } }
        /// <summary>
        /// overrides <see cref="Solid.Refresh"/>.
        /// </summary>
        public override void Refresh()
        {   

            Clear();
            Sphere SphereSurface = new Sphere();
            SphereSurface.VResolution = VResolution;
            SphereSurface.UResolution = UResolution;

            SphereSurface.Radius = Radius;
            Vertex3d NP = new Vertex3d(new xyz(0, 0, Radius));
            Vertex3d SP = new Vertex3d(new xyz(0, 0, -Radius));
            Vertex3d[,] Points = new Vertex3d[SphereSurface.UResolution + 1, SphereSurface.VResolution + 1];
            Line3D[,] HorzCurves = new Line3D[SphereSurface.UResolution, SphereSurface.VResolution];
            Line3D[,] VertCurves = new Line3D[SphereSurface.UResolution, SphereSurface.VResolution];
            xyz[,] Normals = new xyz[SphereSurface.UResolution + 1, SphereSurface.VResolution + 1];
            VertexList.Add(NP);
            VertexList.Add(SP);
            for (int i = 0; i < SphereSurface.UResolution + 1; i++)
                for (int j = 0; j < SphereSurface.VResolution + 1; j++)
                {
                    Normals[i, j] = SphereSurface.Normal(((float)i / (float)SphereSurface.UResolution), (float)j / (float)SphereSurface.VResolution);

                    if (j == 0) { Points[i, j] = SP; continue; }
                    if (j == SphereSurface.VResolution) { Points[i, j] = NP; continue; }
                    if (i == SphereSurface.UResolution) { Points[i, j] = Points[0, j]; continue; }
                    Points[i, j] = new Vertex3d(SphereSurface.Value((float)i / (float)SphereSurface.UResolution, (float)j / (float)SphereSurface.VResolution));
                    VertexList.Add(Points[i, j]);

                }
            for (int i = 0; i < SphereSurface.UResolution; i++)
                for (int j = 0; j < SphereSurface.VResolution; j++)
                {
                    if (i < SphereSurface.UResolution)
                    {
                        if (i + 1 < SphereSurface.UResolution)
                            HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[i + 1, j].Value);
                        else
                            HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[0, j].Value);
                        HorzCurves[i, j].Neighbors = new Face[2];
                        if (HorzCurves[i, j].A.dist(HorzCurves[i, j].B) > 0.0001)
                            EdgeCurveList.Add(HorzCurves[i, j]);
                    }
                    if (j < SphereSurface.VResolution)
                    {
                        VertCurves[i, j] = new Line3D(Points[i, j].Value, Points[i, j + 1].Value);
                        VertCurves[i, j].Neighbors = new Face[2];

                        EdgeCurveList.Add(VertCurves[i, j]);
                    }
                }
            // if (false)
            for (int i = 0; i < SphereSurface.UResolution; i++)
            {
                for (int j = 0; j < SphereSurface.VResolution; j++)
                {
                    Vertex3d A = Points[i, j];
                    Vertex3d B = Points[i, j + 1];

                    Vertex3d C = null;
                    if (i + 1 < SphereSurface.UResolution)
                        C = Points[i + 1, j + 1];
                    else
                        C = Points[0, j + 1];

                    Vertex3d D = null;
                    if (i + 1 < SphereSurface.UResolution)
                        D = Points[i + 1, j];
                    else
                        D = Points[0, j];


                    Face F = new Face();
                    FaceList.Add(F);
                    F.Surface = new SmoothPlane(Points[i, j].Value,  Points[i + 1, j + 1].Value, Points[i, j + 1].Value, Points[i + 1, j].Value, Normals[i, j], Normals[i + 1, j + 1], Normals[i, j + 1],Normals[i + 1, j]); ;

               
                    EdgeLoop EL = new EdgeLoop();
                    F.Bounds.Add(EL);

                    if (A != B)
                    {
                        Edge E = new Edge();
                        EdgeList.Add(E);
                        EL.Add(E);
                        E.EdgeStart = A;
                        E.EdgeEnd = B;
                        E.EdgeCurve = VertCurves[i, j];

                        E.EdgeCurve.Neighbors[0] = F;
                        E.SameSense = true;
                        E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                    }
                    if (B != C)
                    {
                        Edge E = new Edge();
                        EL.Add(E);
                        EdgeList.Add(E);
                        E.EdgeStart = B;
                        E.EdgeEnd = C;
                        if (j + 1 < SphereSurface.VResolution)
                            E.EdgeCurve = HorzCurves[i, j + 1];
                        else
                            E.EdgeCurve = HorzCurves[i, 0];
                        if (E.EdgeCurve.A.dist(B.Value) > 0.001)
                        {
                        }
                        E.EdgeCurve.Neighbors[0] = F;

                        E.SameSense = true;
                        E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                    }


                    if (C != D)
                    {
                        Edge E = new Edge();
                        EL.Add(E);
                        EdgeList.Add(E);
                        E.EdgeStart = C;
                        E.EdgeEnd = D;
                        if (i + 1 < SphereSurface.UResolution)
                            E.EdgeCurve = VertCurves[i + 1, j];
                        else
                            E.EdgeCurve = VertCurves[0, j];
                        E.EdgeCurve.Neighbors[1] = F;
                        E.SameSense = false;
                        E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                        E.ParamCurve.Invert();
                    }
                    if (A != D)
                    {
                        Edge E = new Edge();
                        EL.Add(E);

                        EdgeList.Add(E);
                        E.EdgeStart = D;
                        E.EdgeEnd = A;
                        E.EdgeCurve = HorzCurves[i, j];
                        E.EdgeCurve.Neighbors[1] = F;
                        E.SameSense = false;
                        E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                        E.ParamCurve.Invert();
                    }
                }
            }

            RefreshParamCurves();
            //      base.Refresh();
            //if (!Check())
            //{
            //}
        }

    }
}
