using System;
using System.Collections.Generic;


namespace Drawing3d
{
    /// <summary>
    /// a <see cref="Solid"/>, which extrudes a <see cref="Loxy"/> with a <see cref="Height"/>.
    /// </summary>
    [Serializable]
    public class DiscreteExtrude : DiscreteSolid
    {
       /// <summary>
       /// a constructor with a <see cref="Loxy"/> L and a <see cref="Height"/>.
       /// </summary>
       /// <param name="L"></param>
       /// <param name="Height"></param>
        public DiscreteExtrude(Loxy L,double Height): base()
        {
            this.Polygon = L;
            this.Height = Height;
            Refresh();

        }
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public DiscreteExtrude():base()
        {
            Model = Model.Solid;
            
        }
        private Loxy _Polygon = new Loxy();
        /// <summary>
        /// a <see cref="Loxy"/>, which will be extruded.
        /// </summary>
        public Loxy Polygon
        {
            get { return _Polygon; }
            set
            {
                _Polygon = value;
                Refresh();
            }
        }
        private double _Height;
        /// <summary>
        /// the height of the extrusion.
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                Refresh();
            }
        }
        /// <summary>
        /// overrides the <see cref="Solid.Refresh"/> method and creates the extruder from the base <see cref="Loxy"/> and the <see cref="Height"/>.
        /// </summary>
        public override void Refresh()
        {
            if (Polygon.Count == 0) return;
           
            this.VertexList.Clear();
            this.EdgeCurveList.Clear();
            this.FaceList.Clear();


            List<List<Vertex3d>> DownVertices = new List<List<Vertex3d>>();
            List<List<Vertex3d>> UpVertices = new List<List<Vertex3d>>();
            List<List<Curve3D>> VertCurves = new List<List<Curve3D>>();
            List<List<Curve3D>> DownCurves = new List<List<Curve3D>>();
            List<List<Curve3D>> UpCurves = new List<List<Curve3D>>();
            List<List<PlaneSurface>> PlaneSurfaces = new List<List<PlaneSurface>>();
           
            for (int i = 0; i < _Polygon.Count; i++)
            {
                DownVertices.Add(new List<Vertex3d>());
                UpVertices.Add(new List<Vertex3d>());

                VertCurves.Add(new List<Curve3D>());
                DownCurves.Add(new List<Curve3D>());
                UpCurves.Add(new List<Curve3D>());



                PlaneSurfaces.Add(new List<PlaneSurface>());
                for (int j = 0; j < _Polygon[i].Count; j++)
                {
                    if (_Polygon[i][0].dist(_Polygon[i][_Polygon[i].Count - 1]) < 0.00001)
                        _Polygon[i].RemoveAt(_Polygon[i].Count - 1);
                }
                    for (int j = 0; j < _Polygon[i].Count; j++)
                {
                    Vertex3d V = new Vertex3d(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0));
                    VertexList.Add(V);

                    DownVertices[i].Add(V);
                    Curve3D C = null;
                    if (j < _Polygon[i].Count - 1)

                        C = new Line3D(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0), new xyz(_Polygon[i][j + 1].x, _Polygon[i][j + 1].y, 0));
                    else
                        C = new Line3D(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0), new xyz(_Polygon[i][0].x, _Polygon[i][0].y, 0));
                    DownCurves[i].Add(C);
                    if (!EdgeCurveList.Contains(C))
                        this.EdgeCurveList.Add(C);

                    if (j < _Polygon[i].Count - 1)
                        C = new Line3D(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, Height), new xyz(_Polygon[i][j + 1].x, _Polygon[i][j + 1].y, Height));
                    else
                        C = new Line3D(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, Height), new xyz(_Polygon[i][0].x, _Polygon[i][0].y, Height));
                    UpCurves[i].Add(C);
                    if (!EdgeCurveList.Contains(C))
                        EdgeCurveList.Add(C);

                    V = new Vertex3d(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, Height));
                    UpVertices[i].Add(V);
                    VertexList.Add(V);

                    C = new Line3D(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0), new xyz(_Polygon[i][j].x, _Polygon[i][j].y, Height));
                    VertCurves[i].Add(C);
                    if (!EdgeCurveList.Contains(C))
                        EdgeCurveList.Add(C);
                    if (j < _Polygon[i].Count - 1)
                        PlaneSurfaces[i].Add(new PlaneSurface(    new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0),
                                                                  new xyz(_Polygon[i][j + 1].x, _Polygon[i][j + 1].y, 0),
                                                                   new xyz(_Polygon[i][j + 1].x, _Polygon[i][j + 1].y, Height)
                                                              ));
                    else
                        PlaneSurfaces[i].Add(new PlaneSurface(new xyz(_Polygon[i][j].x, _Polygon[i][j].y, 0),
                                                           new xyz(_Polygon[i][0].x, _Polygon[i][0].y, 0),
                                                          new xyz(_Polygon[i][0].x, _Polygon[i][0].y, Height)
                                                          ));
                }
            }



            for (int i = 0; i < _Polygon.Count; i++)
            {

                for (int j = 0; j < _Polygon[i].Count; j++)
                {

                    Face Face = new Face();
                    FaceList.Add(Face);
                    Face.Surface = PlaneSurfaces[i][j];
                    Face.Bounds = new Bounds();
                    EdgeLoop Loop = new EdgeLoop();
                    Face.Bounds.Add(Loop);

                    // Von unten nach oben
                    Edge E = new Edge();
                    EdgeList.Add(E);
                    E.EdgeStart = DownVertices[i][j];
                    E.EdgeEnd = UpVertices[i][j];
                    E.EdgeCurve = VertCurves[i][j];
                    E.SameSense = true;
                    if (j > 0)
                        VertCurves[i][j].Neighbors[1] = Face;
                    else
                    {
                        VertCurves[i][0].Neighbors = new Face[2];
                        VertCurves[i][j].Neighbors[1] = Face;
                    }

                    Loop.Add(E);

                    // Oben
                    E = new Edge();
                    EdgeList.Add(E);
                    E.SameSense = true;
                    E.EdgeStart = UpVertices[i][j];
                    if (j < _Polygon[i].Count - 1)
                        E.EdgeEnd = UpVertices[i][j + 1];
                    else
                        E.EdgeEnd = UpVertices[i][0];
                    E.EdgeCurve = UpCurves[i][j];
                    E.EdgeCurve.Neighbors = new Face[2];
                    E.EdgeCurve.Neighbors[0] = Face;
                    Loop.Add(E);

                    // Rechts nach unten
                    E = new Edge();
                    EdgeList.Add(E);
                    int n = j + 1;
                    if (n == _Polygon[i].Count) n = 0;
                    E.EdgeStart = UpVertices[i][n];
                    E.EdgeEnd = DownVertices[i][n];
                    E.EdgeCurve = VertCurves[i][n];
                    if (j < _Polygon[i].Count - 1)
                        E.EdgeCurve.Neighbors = new Face[2];
                    E.EdgeCurve.Neighbors[0] = Face;
                    E.SameSense = false;
                    Loop.Add(E);

                    // unten nach links
                    E = new Edge();
                    EdgeList.Add(E);
                    if (j < _Polygon[i].Count - 1)

                        E.EdgeStart = DownVertices[i][j + 1];
                    else
                        E.EdgeStart = DownVertices[i][0];
                    E.EdgeEnd = DownVertices[i][j];
                    E.EdgeCurve = DownCurves[i][j];
                    E.EdgeCurve.Neighbors = new Face[2];
                    E.EdgeCurve.Neighbors[0] = Face;
                    E.SameSense = false;
                    Loop.Add(E);
                }


            }


            // Boden

            Face _Face = new Face();
            FaceList.Add(_Face);
            //    Face.Surface = PlaneSurfaces[i][j];
            _Face.Bounds = new Bounds();
            _Face.Surface = new PlaneSurface(DownVertices[0][0].Value, DownVertices[0][2].Value, DownVertices[0][1].Value);
            for (int i = 0; i < _Polygon.Count; i++)
            {
                EdgeLoop Loop = new EdgeLoop();
                _Face.Bounds.Add(Loop);
                for (int j = 0; j < _Polygon[i].Count; j++)
                {
                 
                    
                    // Von unten nach oben
                    Edge E = new Edge();
                    EdgeList.Add(E);
                    E.EdgeStart = DownVertices[i][j];
                    if (j + 1 < _Polygon[i].Count)
                        E.EdgeEnd = DownVertices[i][j + 1];
                    else
                        E.EdgeEnd = DownVertices[i][0];
                    E.EdgeCurve = DownCurves[i][j];
                    E.SameSense = true;
                    E.EdgeCurve.Neighbors[1] = _Face;
                    Loop.Add(E);
                }
            }

            // Deckel
            _Face = new Face();
         
            FaceList.Add(_Face);
            //    Face.Surface = PlaneSurfaces[i][j];
            _Face.Bounds = new Bounds();
            _Face.Surface = new PlaneSurface(UpVertices[0][0].Value, UpVertices[0][1].Value, UpVertices[0][2].Value);
            for (int i = 0; i < _Polygon.Count; i++)
            {
                EdgeLoop Loop = new EdgeLoop();
                _Face.Bounds.Add(Loop);
                for (int j = _Polygon[i].Count - 1; j >= 0; j--)
                {


                    Edge E = new Edge();
                    EdgeList.Add(E);
                    E.EdgeEnd = UpVertices[i][j];
                    if (j + 1 < _Polygon[i].Count)
                        E.EdgeStart = UpVertices[i][j + 1];
                    else
                        E.EdgeStart = UpVertices[i][0];

                    E.EdgeCurve = UpCurves[i][j];
                    E.SameSense = false;
                    E.EdgeCurve.Neighbors[1] = _Face;
                    Loop.Add(E);
                }
            }
             this.NewParamCurves();
       }
    }
}
