using System;
using System.Collections.Generic;

namespace Drawing3d
{
    /// <summary>
    /// are used from <see cref="DiscreteSolid.CatMull"/>
    /// </summary>
    [Serializable]
    class SubDivisionDescriptor // in den Tag der Vertices
    {
        /// <summary>
        /// edges, which are used from <see cref="DiscreteSolid.CatMull"/>
        /// </summary>
       internal List<Edge> Edges = new List<Edge>();
        /// <summary>
        /// faces, which are used from <see cref="DiscreteSolid.CatMull"/>
        /// </summary>

        internal List<Face> Faces = new List<Face>();
    }

    /// <summary>
    /// a <b>DiscreteSolid</b> is nearly the same as a <see cref="Solid"/> 
    /// with the difference, that all <see cref="Face"/> have all as <see cref="Face.Surface"/> a <see cref="SmoothPlane"/>.
    /// </summary>
    [Serializable]
    public class DiscreteSolid : Solid
    {
        /// <summary>
        /// empty constructor, who sets <see cref="Model.Solid"/> as <see cref="Model"/>.
        /// </summary>
        public DiscreteSolid()
        {
            Model = Drawing3d.Model.Solid;
        }
        /// <summary>
        /// sets for every for every <see cref="Face"/> <see cref="SmoothPlane.Smooth"/> to the <b>value</b>.
        /// </summary>
        /// <param name="value"><b>true</b> activate the smoothing for the <see cref="SmoothPlane"/> of the <b>Faces</b>.</param>
        public void SetSmooth(bool value)
        {
            for (int i = 0; i < FaceList.Count; i++)
            {
                SmoothPlane F = FaceList[i].Surface as SmoothPlane;
                if (F != null) F.Smooth = value;
            }


        }
        private void insertVertexToSameSenseEdge(Face Fa, int Bounds, int Edge, Vertex3d V)
        { 
            EdgeLoop EL = Fa.Bounds[Bounds];
            Edge E = EL[Edge];
            Face Neigbhbor = null;
            int outloop = -1;
            double d = Face.GetDualEdge(Fa, Bounds, Edge + 0.5, ref outloop, ref Neigbhbor);
            EdgeLoop DualEL = Neigbhbor.Bounds[outloop];

            int DualEdge = (int)d;
            Edge F = DualEL[DualEdge];
            // Zuerst die neue Kante

            Edge NewEdge = new Edge();
            EdgeList.Add(NewEdge);
            NewEdge.SameSense = true;
            NewEdge.EdgeStart = E.EdgeStart;
            NewEdge.EdgeEnd = V;
            Line3D L = new Line3D(NewEdge.EdgeStart.Value, V.Value);
            EdgeCurveList.Add(L);
            L.Neighbors = new Face[2];
            L.Neighbors[0] = E.EdgeCurve.Neighbors[0];
            L.Neighbors[1] = E.EdgeCurve.Neighbors[1];
            NewEdge.EdgeCurve = L;

            VertexList.Add(V);

            E.EdgeStart = V;
            E.EdgeCurve.A = V.Value;

            EL.Insert(Edge, NewEdge);



            // Duale






            NewEdge = new Edge();
            NewEdge.SameSense = false;
            EdgeList.Add(NewEdge);
            if (DualEdge + 1 < DualEL.Count)
                DualEL.Insert(DualEdge + 1, NewEdge);
            else
                DualEL.Insert(0, NewEdge);
            NewEdge.EdgeStart = V;
            NewEdge.EdgeEnd = F.EdgeEnd;
            F.EdgeEnd = V;
            NewEdge.EdgeCurve = L;

        }
        /// <summary>
        /// activates a <b>CatMull</b> division.
        /// </summary>
        public void CatMull()
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex3d V = VertexList[i];
                V.Tag = new SubDivisionDescriptor();
            }

            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];
                F.DrawRelativToSurfaceBase = false;
                int n = 0;
                for (int j = 0; j < F.Bounds.Count; j++)
                {

                    xyz subDivCenter = new xyz(0, 0, 0);
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {
                        n++;
                        Edge E = EL[k];


                        subDivCenter = subDivCenter + E.EdgeStart.Value;

                    }
                    F.Tag = subDivCenter * (1f / (float)EL.Count);
                }

            }

            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];

                int n = 0;
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {
                        n++;
                        Edge E = EL[k];
                        Face Neighbor = null;
                        if (E.SameSense)
                            Neighbor = E.EdgeCurve.Neighbors[1] as Face;
                        else
                            Neighbor = E.EdgeCurve.Neighbors[0] as Face;
                        E.Tag = ((xyz)(E.EdgeCurve.Neighbors[1] as Face).Tag + (xyz)(E.EdgeCurve.Neighbors[0] as Face).Tag + E.EdgeStart.Value + E.EdgeEnd.Value) * (1f / 4f);
                        // Mittelwert der Facepunkte und der Anfangs und Endpunkte im Tag speichern
                        //     E.Tag = ((xyz)F.Tag + (xyz)Neighbor.Tag + E.EdgeStart.Value + E.EdgeEnd.Value) * 0.25;
                        //
                        // den Descriptor im Vertex E.EdgeEnd.Tag updaten
                        SubDivisionDescriptor SDD = E.EdgeEnd.Tag as SubDivisionDescriptor;
                        SDD.Edges.Add(E);
                        SDD.Faces.Add(F);



                    }
                }


            }

            // Update Vertixlist
            xyz Q = new xyz(0, 0, 0);
            xyz R = new xyz(0, 0, 0);
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex3d V = VertexList[i];
                SubDivisionDescriptor SDV = V.Tag as SubDivisionDescriptor;
                int n = SDV.Faces.Count;
                if (n == 0) return;
                Q = new xyz(0, 0, 0);
                R = new xyz(0, 0, 0);
                for (int j = 0; j < SDV.Faces.Count; j++)
                    Q = (xyz)SDV.Faces[j].Tag + Q;
                Q = Q * (1f / (float)SDV.Faces.Count);


                for (int j = 0; j < SDV.Edges.Count; j++)
                    R = (xyz)SDV.Edges[j].Tag + R;
                R = R * (1f / (float)SDV.Edges.Count);

                xyz S = V.Value;
                V.Value = (Q + R * 2 + S * (n - 3)) * (1f / (float)n);
            }

            // Kanten Ersetzen
            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];


           
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {


                        Edge E = EL[k];

                        if (E.SameSense)
                        {
                            E.EdgeCurve.A = E.EdgeStart.Value;
                            E.EdgeCurve.B = E.EdgeEnd.Value;
                        }
                    }
                }
            }

            // Kanten Ersetzen
            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];


              
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    for (int k = 0; k < EL.Count; k++)
                    {


                        Edge E = EL[k];
                        if (E.SameSense)
                        {

                            Vertex3d V = new Vertex3d((xyz)E.Tag);

                            insertVertexToSameSenseEdge(F, j, k, V);
                            k++;

                        }


                    }




                }

            }

            FaceList Temp = new FaceList();

            for (int i = 0; i < FaceList.Count; i++)
            {

                Face F = FaceList[i];
                for (int x = 0; x < F.Bounds.Count; x++)
                {
                    EdgeLoop EL = F.Bounds[x];





                    Edge E2 = null;

                    Vertex3d V1 = new Vertex3d();
                    V1.Value = (xyz)F.Tag;
                    VertexList.Add(V1);


                    Vertex3d V4 = null;
                    Vertex3d V2 = null;



                    int startIndex = 1;
                    if (EL[0].EdgeStart.Tag == null) // StartPunkt ist eingefügt
                        startIndex = 0;

                    int id = startIndex;

                    int counter = 0;
                    Edge First = null;
                    Edge Last = null;
                    while (id >= 0)
                    {
                        counter++;
                        EdgeLoop newEl = new EdgeLoop();

                        V2 = EL[id].EdgeStart;


                        if (id + 1 < EL.Count)
                        {
                            V4 = EL[id + 1].EdgeEnd;


                        }
                        else
                        {

                            V4 = EL[0].EdgeEnd;

                        }
                        Face newF = new Face();
                        newF.Parent = F.Parent;
                        Temp.Add(newF);


                        EdgeLoop ELF = new EdgeLoop();
                        List<xyz> Pt = new List<xyz>();
                        newF.Surface = new PlaneSurface(V1.Value, V2.Value, V4.Value);
                        //    newF.Surface = new PlaneSurface(V1.Value, V2.Value, V4.Value);
                        Edge EA = new Edge();
                        ELF.Add(EA);
                        EdgeList.Add(EA);
                        EA.EdgeStart = V1;
                        EA.EdgeEnd = V2;
                        if (Last == null)
                        {

                            EA.EdgeCurve = new Line3D(V1.Value, V2.Value);
                            Pt.Add(V1.Value);
                            EdgeCurveList.Add(EA.EdgeCurve);
                            EA.EdgeCurve.Neighbors = new Face[2];
                            EA.SameSense = true;
                            EA.EdgeCurve.Neighbors[0] = newF;
                            if (id == startIndex)
                                First = EA;


                        }
                        else
                        {
                            EA.EdgeStart = Last.EdgeEnd;
                            Pt.Add(EA.EdgeStart.Value);
                            EA.EdgeEnd = Last.EdgeStart;
                            EA.EdgeCurve = Last.EdgeCurve;
                            EA.EdgeCurve.Neighbors[1] = newF;
                            EA.SameSense = false;

                        }


                        newF.Bounds.Add(ELF);

                        E2 = EL[id];

                        if (E2.SameSense)
                            E2.EdgeCurve.Neighbors[0] = newF;
                        else

                            E2.EdgeCurve.Neighbors[1] = newF;
                        Pt.Add(E2.EdgeStart.Value);
                        ELF.Add(E2);


                        Edge E3 = null;
                        if (id + 1 < EL.Count)
                            E3 = EL[id + 1];
                        else
                            E3 = EL[0];
                        Pt.Add(E3.EdgeStart.Value);
                        if (E3.SameSense)
                            E3.EdgeCurve.Neighbors[0] = newF;
                        else

                            E3.EdgeCurve.Neighbors[1] = newF;



                        ELF.Add(E3);




                        Edge EE = new Edge();
                        EdgeList.Add(EE);
                        EE.EdgeStart = V4;
                        EE.EdgeEnd = V1;
                        ELF.Add(EE);
                        Pt.Add(V4.Value);


                        if (counter < 4)
                        {
                            EE.EdgeCurve = new Line3D(EE.EdgeStart.Value, EE.EdgeEnd.Value);
                            EdgeCurveList.Add(EE.EdgeCurve);

                            EE.EdgeCurve.Neighbors = new Face[2];
                            EE.EdgeCurve.Neighbors[0] = newF;
                            EE.SameSense = true;

                        }
                        else
                        {
                            EE.EdgeCurve = First.EdgeCurve;
                            EE.EdgeStart = First.EdgeEnd;
                            EE.EdgeEnd = First.EdgeStart;
                            EE.EdgeCurve.Neighbors[1] = newF;
                            EE.SameSense = false;
                        }
                        Last = EE;






                        id++;
                        id++;
                        // newF.Surface = new SmoothPlane(Pt[0],Pt[1],Pt[2],Pt[3], new xyz(1, 0, 0), new xyz(1, 0, 0), new xyz(1, 0, 0), new xyz(1, 0, 0));
                        //     newF.Surface = new SmoothPlane(Pt[0], Pt[1], Pt[2], Pt[3], (Pt[0] - Pt[1]) & (Pt[0] - Pt[3]), (Pt[1] - Pt[0]) & (Pt[1] - Pt[2]), (Pt[2] - Pt[1]) & (Pt[3] - Pt[1]), (Pt[0] - Pt[3]) & (Pt[2] - Pt[3]));
                        if (id + 1 > EL.Count)
                            break;

                        //   newF.Surface = new PlaneSurface(V1.Value, V2.Value, V4.Value);

                    }




                }

            }
            FaceList = Temp;
            for (int i = 0; i < FaceList.Count; i++)
            {
                Face F = FaceList[i];
                F.Surface.BoundedCurves = new Loca();
                List<xyz> P = new List<xyz>();
                xyz P1 = new xyz(0, 0, 0);
                xyz P2 = new xyz(0, 0, 0);
                xyz P3 = new xyz(0, 0, 0);
                xyz P4 = new xyz(0, 0, 0);

                for (int k = 0; k < F.Bounds.Count; k++)
                {
                    CurveArray CA = new CurveArray();
                    F.Surface.BoundedCurves.Add(CA);
                    for (int l = 0; l < F.Bounds[k].Count; l++)
                    {
                        Edge E = F.Bounds[k][l];

                        CA.Add(new Line(F.Surface.ProjectPoint(E.EdgeStart.Value), F.Surface.ProjectPoint(E.EdgeEnd.Value)));
                        if (l < 4)
                            P.Add(E.EdgeStart.Value);


                    }
                    if (i == 3)
                    {
                        List<Edge> L1 = new List<Edge>();
                        for (int j = 0; j < 3; j++)
                        {

                            for (int m = 0; m < FaceList[j].Bounds.Count; m++)
                            {
                                xyz A = new xyz(0, 0, 0);
                                for (int h = 0; h < FaceList[j].Bounds[m].Count; h++)
                                {
                                    Edge E = FaceList[j].Bounds[m][h];

                                    L1.Add(E);
                                }
                            }
                        }

                        for (int h = 0; h < L1.Count; h++)
                        {
                            for (int m = 0; m < L1.Count; m++)
                            {
                                if ((L1[h].EdgeStart.Value.dist(L1[m].EdgeStart.Value) < 0.001) && (h != m))
                                { }
                            }
                        }
                    }
                }

                xyz n = ((P[2] - P[0]) & (P[1] - P[0]));
                xyz m1 = ((P[3] - P[0]) & (P[2] - P[0]));
                Loca L = F.Surface.BoundedCurves;
                if (i / 2 * 2 == i)

                    F.Surface = new SmoothPlane(P[0], P[1], P[2], P[3], n);
                else
                    F.Surface = new SmoothPlane(P[0], P[1], P[2], P[3], m1);
                F.Surface.BoundedCurves = L;



                //   (P[1] - P[0]) & (P[1] - P[2]), (P[2] - P[1]) & (P[2] - P[3]), (P[3] - P[0]) & (P[3] - P[2]));
                F.DrawRelativToSurfaceBase = false;
            }
            

        }

    }
}
  
   

