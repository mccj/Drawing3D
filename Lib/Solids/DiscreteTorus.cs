using System;


namespace Drawing3d
{
    /// <summary>
    /// implements a <b>torus</b> with <see cref="InnerRadius"/> and <see cref="OuterRadius"/> as <see cref="DiscreteSolid"/>.
    /// The model is <see cref="Model.Solid"/>.
    /// </summary>
    [Serializable]
    public class DiscreteTorus : DiscreteSolid
    {
       /// <summary>
       /// an empty constructor.
       /// </summary>
       public DiscreteTorus()
        : base()
        {
            Model = Model.Solid;
         
        }
        /// <summary>
        /// a constructor with <see cref="InnerRadius"/> and <see cref="OuterRadius"/>. <see cref="UResolution"/> and <see cref="VResolution"/> are setted to 30.
        /// </summary>
        /// <param name="InnerRadius">the <see cref="InnerRadius"/> </param>
        /// <param name="OuterRadius">the <see cref="OuterRadius"/></param>
        public DiscreteTorus(double InnerRadius, double OuterRadius)
            : this(InnerRadius, OuterRadius, 30, 30)
        {

        }
        /// <summary>
        /// a constructor with <see cref="InnerRadius"/>, <see cref="OuterRadius"/> the <see cref="UResolution"/> and the <see cref="VResolution"/>.
        /// </summary>
        /// <param name="InnerRadius">the <see cref="InnerRadius"/> </param>
        /// <param name="OuterRadius">the <see cref="OuterRadius"/></param>
        /// <param name="UResolution">the <see cref="UResolution"/></param>
        /// <param name="VResolution">the <see cref="VResolution"/> </param>
        public DiscreteTorus(double InnerRadius, double OuterRadius, int UResolution, int VResolution) : base()
        {
            this.UResolution = UResolution;
            this.VResolution = VResolution;
            this.InnerRadius = InnerRadius;
            this.OuterRadius = OuterRadius;
            Model = Model.Solid;
            Refresh();

        }
        private double _OuterRadius = 1;
        /// <summary>
        /// The outer radius of the torus.
        /// </summary>
        public double OuterRadius
        {
            get { return _OuterRadius; }
            set { _OuterRadius = value; }

        }

        private double _InnerRadius = 20;

        /// <summary>
        /// The inner radius of the torus.
        /// </summary>
        public double InnerRadius
        {
            get { return _InnerRadius; }
            set { _InnerRadius = value; }

        }
        int _UResolution = 30;
        /// <summary>
        /// the resolution in x-direction. Default is 30;
        /// </summary>
        public int UResolution
        { get { return _UResolution; } set { _UResolution = value; } }
        int _VResolution = 30;
        /// <summary>
        /// the resolution in y-direction. Default is 30;
        /// </summary>
        public int VResolution
        {
            get { return _VResolution; }
            set
            { _VResolution = value; }
        }
        [NonSerialized]
        Torus TorusSurface = new Torus();
        /// <summary>
        /// overrides <see cref="Solid.Refresh"/>.
        /// </summary>
        public override void Refresh()
        {

            Clear();
            Torus TorusSurface = new Torus();
            TorusSurface.VResolution = VResolution;
            TorusSurface.UResolution = UResolution;

            TorusSurface.InnerRadius = InnerRadius;
            TorusSurface.OuterRadius = OuterRadius;

            Vertex3d[,] Points = new Vertex3d[TorusSurface.UResolution, TorusSurface.VResolution];
            xyz[,] Normals = new xyz[TorusSurface.UResolution, TorusSurface.VResolution];

            Line3D[,] HorzCurves = new Line3D[TorusSurface.UResolution, TorusSurface.VResolution];
            Line3D[,] VertCurves = new Line3D[TorusSurface.UResolution, TorusSurface.VResolution];

            for (int i = 0; i < TorusSurface.UResolution; i++)
                for (int j = 0; j < TorusSurface.VResolution; j++)
                {

                    Normals[i, j] = TorusSurface.Normal(((float)i / (float)TorusSurface.UResolution), (float)j / (float)TorusSurface.VResolution);

                    Points[i, j] = new Vertex3d(TorusSurface.Value((float)i / (float)TorusSurface.UResolution, (float)j / (float)TorusSurface.VResolution));
                    VertexList.Add(Points[i, j]);

                }
            for (int i = 0; i < TorusSurface.UResolution; i++)
                for (int j = 0; j < TorusSurface.VResolution; j++)
                {


                    if (i + 1 < TorusSurface.UResolution)
                        HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[i + 1, j].Value);
                    else
                        HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[0, j].Value);
                    HorzCurves[i, j].Neighbors = new Face[2];

                    EdgeCurveList.Add(HorzCurves[i, j]);

                    if (j + 1 < TorusSurface.VResolution)
                        VertCurves[i, j] = new Line3D(Points[i, j].Value, Points[i, j + 1].Value);
                    else
                        VertCurves[i, j] = new Line3D(Points[i, j].Value, Points[i, 0].Value);
                    VertCurves[i, j].Neighbors = new Face[2];

                    EdgeCurveList.Add(VertCurves[i, j]);

                }
         
            for (int i = 0; i < TorusSurface.UResolution; i++)
            {
                for (int j = 0; j < TorusSurface.VResolution; j++)
                {
                    int jIndex = -1;
                    if (j + 1 < TorusSurface.VResolution)
                        jIndex = j + 1;
                    else
                        jIndex = 0;
                    int iIndex = -1;
                    if (i + 1 < TorusSurface.UResolution)
                        iIndex = i + 1;
                    else
                        iIndex = 0;
                    Vertex3d A = Points[i, j];
                    Vertex3d B = null;
                    B = Points[i, jIndex];
                    Vertex3d C = null;
                    C = Points[iIndex, jIndex];
                    Vertex3d D = null;
                    D = Points[iIndex, j];
                    Face F = new Face();
                    FaceList.Add(F);
                    F.Surface = new SmoothPlane(Points[i, j].Value, Points[iIndex, jIndex].Value, Points[i, jIndex].Value, Points[iIndex, j].Value, Normals[i, j], Normals[iIndex, jIndex], Normals[i, jIndex], Normals[iIndex, j]); ;
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
                    else
                    { }
                    if (B != C)
                    {
                        Edge E = new Edge();
                        EL.Add(E);
                        EdgeList.Add(E);
                        E.EdgeStart = B;
                        E.EdgeEnd = C;
                        if (j + 1 < TorusSurface.VResolution)
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
                    else
                    { }


                    if (C != D)
                    {
                        Edge E = new Edge();
                        EL.Add(E);
                        EdgeList.Add(E);
                        E.EdgeStart = C;
                        E.EdgeEnd = D;
                        if (i + 1 < TorusSurface.UResolution)
                            E.EdgeCurve = VertCurves[i + 1, j];
                        else
                            E.EdgeCurve = VertCurves[0, j];
                        E.EdgeCurve.Neighbors[1] = F;
                        E.SameSense = false;
                        E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                        E.ParamCurve.Invert();
                    }
                    else
                    { }
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
                    else
                    { }
                }
            }

            RefreshParamCurves();

        }
    }
}
