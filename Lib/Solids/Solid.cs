using System;
using System.Collections.Generic;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// defines the kind in which the solid will be constructed.
    /// </summary>
    public enum Model
    {
        /// <summary>
        /// the solid has only the list <see cref="FaceList"/>, all the other like <see cref="EdgeList"/> <see cref="VertexList"/> and <see cref="Solid.EdgeCurveList"/> have a count = 0;
        /// The <see cref="Vertex3d"/> and the <see cref="Edge"/>s you get from <see cref="Face.Bounds"/>.
        /// Therefore there is for every <see cref="Vertex3d"/> at most one <see cref="Face"/> defined. A <see cref="Vertex3d"/> is not used from two <see cref="Face"/>s.
        /// The property <see cref="Curve3D.Neighbors"/> of the <see cref="Curve3D"/> of such an <see cref="Edge"/> is not setted. Therefor it is not possible
        /// to get the neighbor of a <see cref="Face"/> at an <see cref="Edge"/>.
        /// with <see cref="Face.ParamCurves"/> the 3D-representation of the solid can be produced.
        /// </summary>
        Shell,
        /// <summary>
        /// all lists : the <see cref="FaceList"/>, the <see cref="EdgeList"/> the <see cref="VertexList"/> and the <see cref="Solid.EdgeCurveList"/> has entries.
        /// Das Solid enthält nur eine gefüllte Liste für FaceList, alle anderen
        /// The <see cref="Vertex3d"/> points are unique. A physical edge has two <see cref="Edge"/>, 
        /// but the have only one belonging <see cref="Curve3D"/> with a start <see cref="Vertex3d"/> and a end <see cref="Vertex3d"/>. In the field <see cref="Curve3D.Neighbors"/> are stored the two <see cref="Face"/>, whith border on the curves.
        /// So is it possible with <see cref="Curve3D.Neighbors"/> to get the neighbor of a <see cref="Face"/>.
        /// Only in this model it is possible to ralize <see cref="SetOperations"/>s.
        /// </summary>        
        Solid
    }

    ///<summary>
    ///the class solid describes a solid body. It contains the following lists: 
    ///The following lists are important.
    /// 1. the <see cref="VertexList"/> 
    /// 2. the <see cref="EdgeList"/>
    /// 3. the <see cref="FaceList"/>
    /// 4. the <see cref="EdgeCurveList"/><br/>
    ///In the <see cref="Model.Shell"/> only the <see cref="FaceList"/> has entries.<br/>
    ///the <see cref="VertexList"/><br/>
    ///contains all points of a <see cref="Solid"/>. They are the edgestart and the edgeend of an <see cref="Edge"/> resp. of the associated <see cref="Curve3D"/>.
    ///In the <see cref="Model.Solid"/> they are in general dfferent.<br/>
    ///the <see cref="EdgeList"/><br/>
    ///The <see cref="Edge.EdgeStart"/> and <see cref="Edge.EdgeEnd"/> gives a pair of <see cref="Vertex3d"/>. 
    ///To every <see cref="Edge"/> there is a dual <see cref="Edge"/>, which
    ///is inverted and goes from EdgeEnd to Edgestart. To every <see cref="Edge"/> 
    ///is associated a  <see cref="Edge"/>
    ///A <see cref="Edge"/> is member of <see cref="Face.Bounds"/>. 
    ///The property <see cref="Edge.ParamCurve"/> can be evaluated by <see cref="Face.Surface"/> 
    ///and gives a 3D-Curve.
    ///Is this the same as <see cref="Edge.EdgeCurve"/> the value of <see cref="Edge.SameSense"/> 
    ///is true.
    ///The curve <see cref="Edge.EdgeCurve"/> has a field <see cref="Curve3D.Neighbors"/>,
    ///which is needed to determe the neighbor of the face.<br/>
    ///the <see cref="FaceList"/><br/>
    ///A <see cref="Face"/> contains a <see cref="Face.Bounds"/>, 
    ///which represent the bound of the <see cref="Face"/>.
    ///This bounds are consisting of <see cref="Edge"/>s. Every <see cref="Edge"/> 
    ///has a <see cref="Edge.ParamCurve"/>, which can be used to
    ///calculate the border of the face with <see cref="Surface.Value(double, double)"/>.
    ///The <see cref="Edge"/>s in <see cref="Face.Bounds"/> has a <see cref="Edge.EdgeCurve"/> 
    ///whose field <see cref="Curve3D.Neighbors"/> can used to get the
    ///neighbor of the <see cref="Face"/>.<br/>
    ///the <see cref="EdgeCurveList"/>
    ///contains all <see cref="Curve3D"/> associated to the <see cref="Face.Bounds"/>.
    ///</summary>
    [Serializable]
    public class Solid : Entity
    {

     
         [Serializable]
        class SubDivisionDescriptor // in den Tag der Vertices
        {
            /// <summary>
            /// a list of <see cref="Edge"/>
            /// </summary>
            public List<Edge> Edges = new List<Edge>();
            /// <summary>
            /// a list of <see cref="Face"/>
            /// </summary>
            public List<Face> Faces = new List<Face>();
        }
       
     
       
         private FaceList _Faces = new FaceList();
        /// <summary>
        /// is the list of all used <see cref="Face"/>s.
        /// </summary>
        public FaceList FaceList
        {  
            get { return GetFaceList(); }
            set { SetFaceList(value); value.Parent = this; }
        }
        /// <summary>
        /// empty constructor, which sets the <see cref="FaceList.Parent"/> to <b>this</b>.
        /// </summary>
        public Solid()
        {
            FaceList.Parent = this;
        }
        /// <summary>
        /// gets the dual <see cref="Edge"/> of the <see cref="Face"/>. The Edge is <see cref="Face.Bounds"/>[inLoop][id].
        /// </summary>
        /// <param name="Face">Face from the edge.</param>
        /// <param name="inLoop">Index in <see cref="Face.Bounds"/>.</param>
        /// <param name="id">Index in <see cref="Face.Bounds"/>[inLoop]</param>
        /// <returns>the dual <see cref="Edge"/>.</returns>
        public static Edge GetDualEdge(Face Face, int inLoop, int id)
        {
            Edge E = Face.Bounds[inLoop][id];
            Vertex3d A = E.EdgeStart;
            Vertex3d B = E.EdgeEnd;
            Face Neighbor = Face.Neighbor(inLoop, id) as Face;
            if (Neighbor == null)
                return null;
            int OutLoop = -1;
            for (int i1 = 0; i1 < Neighbor.Bounds.Count; i1++)
            {
                OutLoop = i1;

                for (int i = 0; i < Neighbor.Bounds[i1].Count; i++)
                {
                        if (((Neighbor.Bounds[i1][i].EdgeStart.Value.dist(B.Value) < 0.00001) &&
                    (Neighbor.Bounds[i1][i].EdgeEnd.Value.dist(A.Value) < 0.00001)) ||
                    ((Neighbor.Bounds[i1][i].EdgeStart.Value.dist(A.Value) < 0.00001)) &&
                    (Neighbor.Bounds[i1][i].EdgeEnd.Value.dist(B.Value) < 0.00001))
                        {
                      
                        return Neighbor.Bounds[i1][i];
                       }
                }
            }
            return null;
        }
        bool CheckEdges()
        {
            for (int k = 0; k < FaceList.Count; k++)
            {
                Face F = FaceList[k];
                for (int j = 0; j < F.Bounds.Count; j++)

                    for (int i = 0; i < F.Bounds[j].Count; i++)
                    {
                        Edge E = F.Bounds[j][i];
                        Edge DualEdge = GetDualEdge(F, j, i);
                   }
           }
            return true;
      }
        bool Check()
        {
            int FC = FaceList.Count;
            int KC = EdgeCurveList.Count;
            int EC = VertexList.Count;
            int CT = EC - KC + FC - 2; // Euler
            if (CT == 0)
            {
                
                int ED = EdgeList.Count;
                if (ED == 2 * KC)
                {  /* Ok*/      }
                else
                {//   return false;
                }
            }
            else
            {
                //  return false;
                for (int i = 0; i < EdgeCurveList.Count; i++)
                {

                    Edge[] Edges = EdgeCurveList[i].Tag as Edge[];
                    if (Edges!= null)
                    if ((Edges[0] == null)|| (Edges[1] == null))
                    { }
                  else
                    {
                        if (EdgeList.IndexOf(Edges[0])==-1)
                        { }

                        if (EdgeList.IndexOf(Edges[1]) == -1)
                        { }
                    }
                    
                    if (EdgeCurveList[i].A.dist(EdgeCurveList[i].B)<0.0001)
                    { }
                }
            }
            if (!CheckEdges()) { };// return false;
            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {
                        Edge E = EL[k];
                        int OutLoop = -1;

                        if ((E.SameSense) && (
                            (E.EdgeCurve.A.dist(E.EdgeStart.Value) > 0.0001) ||
                            (E.EdgeCurve.B.dist(E.EdgeEnd.Value) > 0.0001)))
                        {
                             return false;
                        }
                        if ((!E.SameSense) && (
                          (E.EdgeCurve.B.dist(E.EdgeStart.Value) > 0.0001) ||
                          (E.EdgeCurve.A.dist(E.EdgeEnd.Value) > 0.0001)))
                        {
                              return false;
                        }
                        Face FF = null;
                        if (E.EdgeCurve.Neighbors[0] == null)
                            return false;

                       
            
                       if (E.EdgeCurve is Line3D)
                        {
                            Line3D Curve3d = E.EdgeCurve as Line3D;
                            xyz _A = F.Surface.Base.Relativ(Curve3d.A);
                            xyz _B = F.Surface.Base.Relativ(Curve3d.B);
                            if (System.Math.Abs(_A.z) > 0.001)
                            {
                                return false;
                            }
                            double g = F.Surface.Base.BaseX.length();
                            double h = F.Surface.Base.BaseY.length();
                            xyz AA = F.Surface.Base.Absolut(_A);
                            xyz BB = F.Surface.Base.Absolut(_B);
                            if (AA.dist(Curve3d.A) > 0.001)
                            {
                                return false;
                            }
                        }
                        if (E.EdgeCurve.Neighbors[0] == E.EdgeCurve.Neighbors[1])
                            return false;
                        if (!FaceList.Contains(E.EdgeCurve.Neighbors[0]))
                        { } 
                        if (!FaceList.Contains(E.EdgeCurve.Neighbors[1]))
                            return false;


                        double d = Face.GetDualEdge(F, j, k+0.01, ref OutLoop, ref FF);
                   }
                }
            }
            return true;
        }
        private double _SmoothAngle = 0.6;
        /// <summary>
        /// if two <see cref="Face"/>s meet together, it can happens. that  in the edges a small
        /// undesired difference of the normal vector is. When it will be drawn, so gives this "hard" edges.
        /// To avoid this you can set <see cref="SmoothAngle"/>. Is the difference of the normal directions smaller than the smoothangle, the normals
        /// will be adapt so, that passage is "smooth". The default value is 0.6.
        /// </summary>  
        public double SmoothAngle
        {
            get
            {
                return _SmoothAngle;
            }
            set { _SmoothAngle = value; }
        }

        private VertexList _VertexList = new VertexList();
        /// <summary>
        /// contains all <see cref="Vertex3d"/> of the <see cref="Solid"/>.
        /// </summary>
         public VertexList VertexList
        {
            get { return _VertexList; }
            set { _VertexList = value; }
        }
        /// <summary>
        /// transforms the all coordinates with the Matrix <b>Transform</b>.
        /// </summary>
        /// <param name="Transformation">the transformation <see cref="Matrix"/></param>
        public void CoordTransform(Matrix Transformation)
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                VertexList[i] = Transformation * VertexList[i];
            }
            for (int i = 0; i < EdgeCurveList.Count; i++)
            {
                EdgeCurveList[i].Transform(Transformation);
            }

            for (int i = 0; i < FaceList.Count; i++)
            {
                Base B = Transformation * FaceList[i].Surface.Base;
               FaceList[i].Surface.Base = B;
            }
            RefreshParamCurves();
        }
 
        private EdgeList _EdgeList = new EdgeList();
        /// <summary>
        /// contains all <see cref="Edge"/>s of the <see cref="Solid"/>
        /// </summary>
         public EdgeList EdgeList
        {
            get { return _EdgeList; }

        }
        //  [NonSerialized]
        CurveList3D _EdgeCurveList = new CurveList3D();
        /// <summary>
        /// constains all EdgeCurves.
        /// </summary>
        public CurveList3D EdgeCurveList { get { return _EdgeCurveList; } set { _EdgeCurveList = value; } }

        void LinkFaces()
        {
            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop E = F.Bounds[j];
                    for (int k = 0; k < E.Count; k++)
                    {
                        Edge Edge = E[k];
                        if (Edge.SameSense)
                            Edge.EdgeCurve.Neighbors[0] = F;
                        else
                            Edge.EdgeCurve.Neighbors[1] = F;
                  }
                }
            }
        }

        /// <summary>
        /// copies the <see cref="Solid"/>.
        /// </summary>
        /// <returns>the copy.</returns>
        public Solid Copy()
        {

            System.IO.MemoryStream F = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BF = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            BF.Serialize(F, this);
            F.Position = 0;
           object S=  BF.Deserialize(F);
           (S as Solid).Refresh();
           return S as Solid;
        }
        /// <summary>
        /// creates new <see cref="Face.ParamCurves"/> by calling <see cref="Surface.To2dCurve(Curve3D)"/> for every EdgeCurve of all <see cref="Face"/>s.
        /// Finately will be called <see cref="RefreshParamCurves"/>,
        /// </summary>
        public void NewParamCurves()
        {
            for (int i = 0; i < this.FaceList.Count; i++)
            {
                Face F = FaceList[i];
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {
                        Edge E = EL[k];
                        if (E.EdgeCurve.Neighbors[0] == null)
                            E.EdgeCurve.Neighbors[0] = F;
                        else
                            if (E.EdgeCurve.Neighbors[1] == null)
                                E.EdgeCurve.Neighbors[1] = F;
                        if (E.ParamCurve == null)
                            E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                        if (!E.SameSense)
                            E.ParamCurve.Invert();
                    }
                }

            }
            RefreshParamCurves();
       }
        /// <summary>
        /// refreshes the <see cref="Face.ParamCurves"/> from the <see cref="Edge.ParamCurve"/> of all <b>edges</b>.
        /// </summary>
        public void RefreshParamCurves()
        {
            for (int i = 0; i < FaceList.Count; i++)
            {
                FaceList[i].RefreshParamCurves();
            }

        }
      
        /// <summary>
        /// virtual get method for <see cref="FaceList"/>
        /// </summary>
        /// <returns></returns>
        protected virtual FaceList GetFaceList()
        {
            return _Faces;
        }
        /// <summary>
        /// virtual set method for <see cref="FaceList"/>
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void SetFaceList(FaceList Value)
        {
            _Faces = Value;
        }

        /// <summary>
        /// clears all lists.
        /// </summary>
        public void Clear()
        {
            VertexList.Clear();
            EdgeList.Clear();
            FaceList.Clear();
            EdgeCurveList.Clear();
        }
        /// <summary>
        /// Eine statische methode die einen Quader erstellt mit Ursprung <b>Origin</b> und
        /// Größe <b>Size</b>
        /// </summary>
        /// <param name="Origin">Ursprung des Quaders (rechts unten)</param>
        /// <param name="Size">Größe des Quaders</param>
        /// <returns>Ein solider Quader (Model.SolidSurface) </returns>
        public static Solid CreateSolidBox(xyz Origin, xyz Size)
        {
            Solid Box = new Solid();
            Box.Model = Model.Solid;
            Vertex3d A = new Vertex3d(Origin);
            Vertex3d B = new Vertex3d(Origin + new xyz(Size.x, 0, 0));

            Vertex3d C = new Vertex3d(Origin + new xyz(Size.x, Size.y, 0));
            Vertex3d D = new Vertex3d(Origin + new xyz(0, Size.y, 0));
 



            Vertex3d E = new Vertex3d(A.Value + new xyz(0, 0, Size.z));
            Vertex3d F = new Vertex3d(B.Value + new xyz(0, 0, Size.z));
            Vertex3d G = new Vertex3d(C.Value + new xyz(0, 0, Size.z));
            Vertex3d H = new Vertex3d(D.Value + new xyz(0, 0, Size.z));
            Vertex3dArray_2 Border = new Vertex3dArray_2();
            Vertex3dArray VA = new Vertex3dArray();
            Border.Add(VA);
            Face Face = null;
            VA.Clear();
            VA.Add(A);
            VA.Add(B);
            VA.Add(C);
            VA.Add(D);
            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);

            VA.Clear();
            VA.Add(A);
            VA.Add(E);
            VA.Add(F);
            VA.Add(B);

            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);

            VA.Clear();
            VA.Add(B);
            VA.Add(F);
            VA.Add(G);
            VA.Add(C);
            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);

            VA.Clear();
            VA.Add(C);
            VA.Add(G);
            VA.Add(H);
            VA.Add(D);
            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);

            VA.Clear();
            VA.Add(D);
            VA.Add(H);
            VA.Add(E);
            VA.Add(A);
            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);

            VA.Clear();
            VA.Add(H);
            VA.Add(G);
            VA.Add(F);
            VA.Add(E);
            Face = Face.SolidPlane(Box, Border);
            Box.FaceList.Add(Face);
            return Box;
        }
        /// <summary>
        /// gets an <see cref="Edge"/> belonging to the <see cref="Vertex3d"/> A and B.
        /// </summary>
        /// <param name="A">start point</param>
        /// <param name="B">end point</param>
        /// <returns>the <see cref="Edge"/> if it exists, else null.</returns>
        public Edge GetSurfaceEdge(Vertex3d A, Vertex3d B)
        {
            for (int i = 0; i < EdgeList.Count; i++)
            {
                Edge E = EdgeList[i];
                if ((E.EdgeStart == A) && (E.EdgeEnd == B))
                    return E;
            }
            return null;
        }
        /// <summary>
        /// gets a box in the <see cref="Model.Shell"/> mode.
        /// </summary>
        /// <param name="Origin">origin</param>
        /// <param name="Size">size.</param>
        /// <returns>a box in <see cref="Model.Shell"/> mode.</returns>
        public static Solid CreateShellBox(xyz Origin, xyz Size)
        {
            Solid Box = new Solid();
            Box.Model = Model.Shell;
            xyz A = Origin;
            xyz B = Origin + new xyz(Size.x, 0, 0);
            xyz C = Origin + new xyz(Size.x, Size.y, 0);
            xyz D = Origin + new xyz(0, Size.y, 0);

            xyz E = A + new xyz(0, 0, Size.z);
            xyz F = B + new xyz(0, 0, Size.z);
            xyz G = C + new xyz(0, 0, Size.z);
            xyz H = D + new xyz(0, 0, Size.z);
            xyzArray Points = new xyzArray(4);
            Points[0] = A;
            Points[1] = B;
            Points[2] = C;
            Points[3] = D;
            Face _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);

            Points[0] = A;
            Points[1] = E;
            Points[2] = F;
            Points[3] = B;
            _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);

            Points[0] = B;
            Points[1] = F;
            Points[2] = G;
            Points[3] = C;
            _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);

            Points[0] = C;
            Points[1] = G;
            Points[2] = H;
            Points[3] = D;
            _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);

            Points[0] = D;
            Points[1] = H;
            Points[2] = E;
            Points[3] = A;
            _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);


            Points[0] = H;
            Points[1] = G;
            Points[2] = F;
            Points[3] = E;
            _Face = Face.ShellPlane(Points);
            Box.FaceList.Add(_Face);
            return Box;
        }





        /// <summary>
        /// gets and sets the  <see cref="Model"/>.
        /// </summary>
        public Model Model = Model.Solid;


        /// <summary>
        /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method by calling
        /// the <see cref="Entity.Paint(OpenGlDevice)"/> method of every <see cref="Face.Surface"/>.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which will be drawn.</param>
        protected override void OnDraw(OpenGlDevice Device)
        {
    

            for (int i = 0; i < FaceList.Count; i++)
            {
             //   (FaceList[i] as Face).Surface.CompileEnable = false;
                (FaceList[i] as Face).Surface.Paint(Device);
            }
            base.OnDraw(Device);
        }

        /// <summary>
        /// calls the <see cref="Face.Refresh"/> for every <see cref="Face"/>.
        /// </summary>
        public virtual void Refresh()
        {
           for (int i = 0; i < FaceList.Count; i++)
            {
                FaceList[i].Refresh();
            }
        }

    }

}
