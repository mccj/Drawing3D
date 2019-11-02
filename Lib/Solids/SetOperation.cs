using System;
using System.Collections.Generic;
using System.Collections;

namespace Drawing3d
{   
    /// <summary>
    /// for every <see cref="SetOperations"/> the crossing curve of the two solids build
    /// a closed way. Every part has two <see cref="Face"/>s from the the solids the <b>FaceA</b> and <b>FaceB</b>.
    /// May be such a part is interrupted. This is stored in a list of <see cref="CrossItem"/>s.
    /// 
    /// </summary>
    [Serializable]
    class TraceInfo
    {  
        /// <summary>
        /// internal. 
        /// </summary>
        internal int Index;
        /// <summary>
        /// a list of <see cref="CrossItem"/>, which is sorted by <see cref="CrossItem.Param2"/>.
        /// </summary>
        public List<CrossItem> SortedParams;
        /// <summary>
        /// is one face who cross the other.
        /// </summary>
        public Face FaceA = null;
        /// <summary>
        /// is the other crossing face.
        /// </summary>
        public Face FaceB = null;
        /// <summary>
        /// Constructor with <b>Index</b>, <b>SortedParams</b>, <b>FaceA</b>, <b>FaceB</b>.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="SortedParams"></param>
        /// <param name="FaceA"></param>
        /// <param name="FaceB"></param>
        public TraceInfo(int Index, List<CrossItem> SortedParams, Face FaceA, Face FaceB)
        {
            this.Index = Index;
            this.SortedParams = SortedParams;
            this.FaceA = FaceA;
            this.FaceB = FaceB;
        }
    }
    /// <summary>
    /// this class produces set thoretical solids from two <see cref="DiscreteSolid"/>. See also <see cref="SetOperation"/>.
    /// </summary>
    [Serializable]
    public class SetOperation3D
    {
        /// <summary>
        /// gives informations about the intersection of two <see cref="Face"/>s: <b>Face1</b> and <b>Face2</b>.
        /// First a cross <see cref="LineType"/> L will be calculated with the direction of the cross product of the normals from
        /// Face1 and Face2. Then the <see cref="CrossItem"/> from the <see cref="Face.ParamCurves"/> of Face1 with the the Line L in Face1 gives the param1 a1 and e1, if the have a crosspoint with L.
        /// The same will be maded for Face2 gives the parameters a2 and e2.
        /// 
        /// case : a1 .. a2 .. e2 .. e1: gives <see cref="Edge1ParamFrom"/>=-1 and a <see cref="Edge1ParamTo"/>=-1 <see cref="Edge2ParamFrom"/>=a2 and a <see cref="Edge2ParamTo"/>=e2
        /// case : a2 .. a1 .. e1 .. e2: gives <see cref="Edge1ParamFrom"/>=a1 and a <see cref="Edge1ParamTo"/>=e1 <see cref="Edge2ParamFrom"/>=-1 and a <see cref="Edge2ParamTo"/>=-1
        /// case : a1 .. a2 .. e1 .. e2: gives <see cref="Edge1ParamFrom"/>=-1 and a <see cref="Edge1ParamTo"/>=e1 <see cref="Edge2ParamFrom"/>=a2 and a <see cref="Edge2ParamTo"/>=-1
        /// case : a2 .. a1 .. e2 .. e1: gives <see cref="Edge1ParamFrom"/>=a1 and a <see cref="Edge1ParamTo"/>=-1 <see cref="Edge2ParamFrom"/>=-1 and a <see cref="Edge2ParamTo"/>=e2
        /// </summary>
        [Serializable]
        internal class IntersectResult
        {
            /// <summary>
            /// empty constructor.
            /// </summary>
            public IntersectResult()
            {
            }
            /// <summary>
            /// constructor with <b>Face1</b>, <b>Face2</b>, <b>Edge1ParamFrom</b>, <b>Edge1ParamTo</b>, <b>Edge2ParamFrom</b> and <b>Edge2ParamTo</b>.
            /// </summary>
            /// <param name="Face1">1. <see cref="Face"/></param>
            /// <param name="Face2">2. <see cref="Face"/></param>
            /// <param name="Edge1ParamFrom">you get the point by <see cref="Eval(Face, double)"/> with Edge1ParamFrom. </param>.
            /// <param name="Edge1ParamTo">you get the point by <see cref="Eval(Face, double)"/> with Edge1ParamTo.</param>
            /// <param name="Edge2ParamFrom">you get the point by <see cref="Eval(Face, double)"/> with Edge2ParamFrom.</param>
            /// <param name="Edge2ParamTo">you get the point by <see cref="Eval(Face, double)"/> with Edge2ParamTo.</param>
            public IntersectResult(Face Face1, Face Face2, double Edge1ParamFrom, double Edge1ParamTo, double Edge2ParamFrom, double Edge2ParamTo)
            {
                this.Face1 = Face1;
                this.Face2 = Face2;
                this.Edge1ParamTo = Edge1ParamTo;
                this.Edge2ParamTo = Edge2ParamTo;
                this.Edge1ParamFrom = Edge1ParamFrom;
                this.Edge2ParamFrom = Edge2ParamFrom;

            }
         /// <summary>
         /// internal.
         /// </summary>
           internal void Invert()
            {
                Vertex3d V = EndVertex;
                EndVertex = StartVertex;
                StartVertex = V;
                double R = Edge1ParamFrom;
                Edge1ParamFrom = Edge1ParamTo;
                Edge1ParamTo = R;
                R = Edge2ParamFrom;
                Edge2ParamFrom = Edge2ParamTo;
                Edge2ParamTo = R;
            }
            /// <summary>
            /// internal.
            /// </summary>
            internal TraceInfo TraceInfo = null;
            /// <summary>
            /// internal.
            /// </summary>
            internal IntersectResult Copy()
            {
                IntersectResult Result = new IntersectResult(Face1, Face2, Edge1ParamFrom, Edge1ParamTo, Edge2ParamFrom, Edge2ParamTo);
                Result.Neighbors = Neighbors;
                Result.EndVertex = EndVertex;
                Result.StartVertex = StartVertex;
                Result.SortedParams = SortedParams;
                Result.StartIndexInSortedList = StartIndexInSortedList;
                return Result;
            }
            /// <summary>
            /// <see cref="Face"/> in the first solid of <see cref="GetCombinedSolids(DiscreteSolid, DiscreteSolid, SetOperation)"/>.
            /// </summary>
            public Face Face1 = null;
            /// <summary>
            /// <see cref="Face"/> in the second solid of <see cref="GetCombinedSolids(DiscreteSolid, DiscreteSolid, SetOperation)"/>.
            /// </summary>
            public Face Face2 = null;
            /// <summary>
            /// you get the point by <see cref="Eval(Face, double)"/> with Edge1ParamTo. 
            /// </summary>
            public double Edge1ParamTo = -1;
            /// <summary>
            /// you get the point by <see cref="Eval(Face, double)"/> with Edge2ParamTo. 
            /// </summary>
            public double Edge2ParamTo = -1;
            /// <summary>
            /// you get the point by <see cref="Eval(Face, double)"/> with Edge1ParamFrom. 
            /// </summary>         
            public double Edge1ParamFrom = -1;
            /// <summary>
            /// you get the point by <see cref="Eval(Face, double)"/> with Edge2ParamFrom. 
            /// </summary> 
            public double Edge2ParamFrom = -1;
            /// <summary>
            /// internal.
            /// </summary>
            internal Object[] Neighbors = null;
            /// <summary>
            /// internal.
            /// </summary>
            internal Vertex3d StartVertex = null;
            /// <summary>
            /// internal.
            /// </summary>
            internal Vertex3d EndVertex = null;
            /// <summary>
            /// internal.
            /// </summary>
            internal int StartIndexInSortedList = -1;
            /// <summary>
            /// internal.
            /// </summary>
            internal List<CrossItem> SortedParams = null; 

        }
   
        static List<Vertex3d> ListOfVertices = new List<Vertex3d>();
        static Vertex3d FindVertex( Vertex3d v)
        {
            for (int i = 0; i < ListOfVertices.Count; i++)
                if (v.Value.dist(ListOfVertices[i].Value) < 0.00001) return ListOfVertices[i];
            
            Vertex3d V=new Vertex3d(v.Value);
            ListOfVertices.Add(V);
            return V;

        }
 
        /// <summary>
        /// calculates from a pair <b>Loop</b> and <b>Param</b> the global parameter.
        /// </summary>
        /// <param name="Face">the underlying <see cref="Face"/></param>
        /// <param name="Loop">the index in <see cref="Face.Bounds"/></param>
        /// <param name="Param">the paremeter relatively to Face.Bounds[Loop]. See also <see cref="Eval(int, double, Face)"/></param>
        /// <returns></returns>
        static double GetGlobalParam(Face Face, int Loop, double Param)
        {
            double Result = Param;
            for (int i = 0; i < Loop; i++)
                Result += Face.Bounds[i].Count;
           
            return Result;
        }
        /// <summary>
        /// Berechnet aus globalen Parametern lokale Paare Loop und Param
        /// </summary>
        /// <param name="Face">Zugrunde liegendes Face</param>
        /// <param name="GlobalParam">Globaler Parameter</param>
        /// <param name="Loop">Index in Face.Bounds</param>
        /// <param name="Param">Der Parameter bezüglich  Bounds[Loop]</param>
        static void GetLocalParam(Face Face, double GlobalParam, ref int Loop, ref double Param)
        {
            Loop = 0;
            Param = GlobalParam;
            while (Param >= Face.Bounds[Loop].Count) { Param -= Face.Bounds[Loop].Count; Loop++; }

        }
        /// <summary>
        /// Ermittelt den xy-Wert des Loops Face.Bounds[Loop] für den Parameterwert Param
        /// </summary>
        /// <param name="Face">Zugrunde liegendes Face</param>
        /// <param name="Loop">Der Index in Face.Bounds</param>
        /// <param name="Param">Parameter bezüglich Face.Bounds[Loop]</param>
        /// <returns></returns>
        static xyz Eval(Face Face, int Loop, double Param)
        {
            //int id = (int)Param;
            //EdgeLoop EL = Face.Bounds[Loop];
            //Edge E = EL[id];
            //Line3D L = new Line3D(E.EdgeStart.Value, E.EdgeEnd.Value);
            //xyz R = L.Value(Param - id);
            //return R;
            xy AP = Face.ParamCurves[Loop].Value(Param);
            xyz Result = Face.Surface.Value(AP.x, AP.y);
            return Result;
        }
        /// <summary>
        /// Ermittelt den xy Wert von einem Face bezüglich des globalen Parameters
        /// </summary>
        /// <param name="Face">Zugrunde liegendes Face</param>
        /// <param name="GlobalParam">globaler Parameter</param>
        /// <returns></returns>
       static xyz Eval(Face Face, double GlobalParam)
        {
            int Loop = -1;
            double Param = -1;
            GetLocalParam(Face, GlobalParam, ref Loop, ref Param);
            return Eval(Face, Loop, Param);
        }
        static double DualEdge(Face Face, double GlobalParam, ref Face Neighbor)
        {
            int loop = -1;
            double LocalParam = -1;
            GetLocalParam(Face, GlobalParam, ref loop, ref LocalParam);
            int OutLoop = -1;
            if (LocalParam == -1) return -1;
            if (LocalParam >= Face.Bounds[loop].Count)
                LocalParam = 0;
            double Param = GetDualEdge(Face, loop, LocalParam, ref OutLoop, ref Neighbor);

            if (Param == -1)
            {
                return -1;
            }
            double Result = GetGlobalParam(Neighbor, OutLoop, Param);

            return Result;
        }
        static double GetDualEdge(Face Face, int inLoop, double Param, ref int OutLoop, ref Face Neighbor)
        {
            Edge E = Face.Bounds[inLoop][(int)Param];
            Vertex3d A = E.EdgeStart;
            Vertex3d B = E.EdgeEnd;
            Neighbor = Face.Neighbor(inLoop, (int)Param) as Face;
            if (Neighbor == null)
                return -1;
            OutLoop = -1;
            for (int i1 = 0; i1 < Neighbor.Bounds.Count; i1++)
            {
                OutLoop = i1;

                for (int i = 0; i < Neighbor.Bounds[i1].Count; i++)
                {
                    if ((Neighbor.Bounds[i1][i].EdgeStart == B) &&
                        (Neighbor.Bounds[i1][i].EdgeEnd == A))
                    {
                    //    if ((int)Param == Param) return i;
                        if (i + (1 - (Param - (int)Param)) == Neighbor.Bounds[i1].Count)
                            return 0;
                     
                        return i + (1 - (Param - (int)Param));
                       

                    }
                    if ((Neighbor.Bounds[i1][i].EdgeStart == A) &&
                        (Neighbor.Bounds[i1][i].EdgeEnd == B))
                    {
                        if (Param != (int)Param)
                            return i + (1 - (Param - (int)Param));
                        return i;

                    }
                }
            }
            return -1;
       }
      static Edge GetDualEdge(Face Face, int inLoop, int id)
        {
            Edge E = Face.Bounds[inLoop][id];
            Vertex3d A = E.EdgeStart;
            Vertex3d B = E.EdgeEnd;
            Face Neighbor = Face.Neighbor(inLoop, id) as Face;
            if (Neighbor == null)
                return null;
           int  OutLoop = -1;
            for (int i1 = 0; i1 < Neighbor.Bounds.Count; i1++)
            {
                OutLoop = i1;

                for (int i = 0; i < Neighbor.Bounds[i1].Count; i++)
                {
                    if (((Neighbor.Bounds[i1][i].EdgeStart.Value.dist(B.Value)<0.00001) &&
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
     
      static IntersectResult StartEdge = null;
      static SetOperation3D.IntersectResult StartTrace(Solid Solid1, Solid Solid2)
        {
            SetOperation3D.IntersectResult R = null;

            for (int i = 0; i < Solid1.FaceList.Count; i++)
            {
                for (int j = 0; j < Solid2.FaceList.Count; j++)
                {

                    CrossFaceListInfo CI = (Solid1.FaceList[i].Tag as Hashtable)[Solid2.FaceList[j]] as CrossFaceListInfo;
                    if ((CI != null)  &&(CI.SortedParams!=null)&& (CI.SortedParams.Count >= 4)
                    && (System.Math.Abs(CI.SortedParams[1].Param2 - CI.SortedParams[2].Param2) > Cross.LuckyEpsilon)
                    && (!CI.SortedParams[1].Visited)
                        )
                    {
                        bool OK = true;
                        for (int i1 = 0; i1 < CI.SortedParams.Count-1; i1++)
                        {

                            if (((int)CI.SortedParams[i1].Tag == (int)CI.SortedParams[i1 + 1].Tag)
                                && ((CI.SortedParams[i1].Param2 == CI.SortedParams[i1 + 1].Param2)))
                            {
                                OK = false;
                                break;
                            } ;
                        }
                        if (!OK) continue;
                                R = new SetOperation3D.IntersectResult();
                             if ((int)CI.SortedParams[1].Tag == 1)
                                {
                                    R.Edge1ParamFrom = CI.SortedParams[1].Param1;
                                }
                                else
                                    R.Edge2ParamFrom = CI.SortedParams[1].Param1;

                                if ((int)CI.SortedParams[2].Tag == 1)
                                {
                                    R.Edge1ParamTo = CI.SortedParams[2].Param1;
                                }
                                else
                                    R.Edge2ParamTo = CI.SortedParams[2].Param1;


                                R.SortedParams = CI.SortedParams;
                                R.Face1 = Solid1.FaceList[i] as Face;
                                R.Face2 = Solid2.FaceList[j] as Face;
                                StartEdge = R;
                                if (R.Edge1ParamTo >= 0) OldtPt = Eval(R.Face1, R.Edge1ParamTo);
                                else OldtPt = Eval(R.Face2, R.Edge2ParamTo);
                                R.TraceInfo = new TraceInfo(1, R.SortedParams, R.Face1, R.Face2);
                                return R;
                        
                    }
                }
            }
            return null;
        }

        static CrossFaceListInfo IntersectFaces(Face Face1, Face Face2)
        {

            return Cross.IntersectFaces(Face1, Face2);
        }
        xyz Eval(int Loop, double Param, Face Face)
        {
            xy AP = Face.ParamCurves[Loop].Value(Param);
            xyz Result = Face.Surface.Value(AP.x, AP.y);
            return Result;
        }
      
        static bool DoRemove = true;
        static xyz _OldPt = new xyz(0, 0, 0);
        static xyz OldtPt
        { get { return _OldPt; }
            set
            { _OldPt = value; }
        }
       static List<CrossItem> OldParams = null;
       static TraceInfo Intersecting(Face F1, Face F2)
        {
            CrossFaceListInfo CI = null;
            xyz B = new xyz(0, 0, 0);
            xyz C = new xyz(0, 0, 0);
            int Idx = -1;
            if (F1.Tag is Hashtable)  CI = (F1.Tag as Hashtable)[F2] as CrossFaceListInfo;
            else
            if (F2.Tag is Hashtable) CI = (F2.Tag as Hashtable)[F1] as CrossFaceListInfo;
            if (CI!= null)
            {
               
                if (( (CI.SortedParams!=null) && (CI.SortedParams.Count >= 4)))
                {
                    if (OldParams != null)
                       // if (OldParams[2].Param1 != CI.SortedParams[2].Param1)
                        {
                        for (int i = 1; i < CI.SortedParams.Count-1; i= i+2)
                        {
                            C = Eval(CI.SortedParams[i].Face as Face, CI.SortedParams[i].Param1);
                            if (C.dist(OldtPt) < 0.001)
                            {
                                Idx = i;
                                break;
                            }
                        }
                        
                    
                    }

                    if (Idx == -1) return null;
                    int CurrentTag = (int)CI.SortedParams[Idx].Tag;
                    int CountOverlapping = 0;
                    for (int i = Idx; i >= 0; i--)
                    {
                        if ((int)CI.SortedParams[i].Tag == CurrentTag)
                            CountOverlapping++;
                    }
                    if (CountOverlapping / 2 * 2 == CountOverlapping)
                        return null;




                    if (System.Math.Abs(CI.SortedParams[Idx].Param2 - CI.SortedParams[Idx+1].Param2) > 0.00000000002)
                    {
                        if (CI.SortedParams[Idx].Visited) return null;
                        if (DoRemove)
                        {
                            CI.SortedParams[Idx + 1].Visited = true;
                            CI.SortedParams[Idx].Visited = true;
                        }
                        OldtPt = Eval(CI.SortedParams[Idx+1].Face as Face, CI.SortedParams[Idx + 1].Param1) ;
                        return new TraceInfo(Idx, CI.SortedParams, CI.SortedParams[2].Face as Face, CI.SortedParams[3].Face as Face); // Case 1
                    }
                    else
                        return null;
                  
                }
                return null;
            }
           return null;
        }
      static TraceInfo TurnAround(Face Turnface, double Param, Face Target)
       {
           Face Dummy = Turnface;
           TraceInfo R= Intersecting(Dummy, Target);
           while (R==null)
           {
                Face F = Dummy;
               Param = DualEdge(Dummy, Param, ref Dummy);
               if (Dummy == Turnface) 
                   return null;
               R = Intersecting(Dummy, Target);
              
           }
        
           return R;
       }
        static TraceInfo FindNextFace(TraceInfo CrossIt )
       {
           Face DualA = null;
           Face DualB = null;
           List<CrossItem> CrossItems = CrossIt.SortedParams;
           TraceInfo CI = null;
           int Idx = CrossIt.Index;
           Face A = CrossItems[Idx+1].Face as Face;
           double ParamA = CrossItems[Idx+1].Param1;
            Face B = null;
            int BIndex = -1;
            for (int i = Idx+2; i < CrossItems.Count; i++)
            {
                if (CrossItems[i].Face!=A)
                {   BIndex = i;
                    B = CrossItems[i].Face as Face;
                    break;
                }
            }


            if (B == null) return null;
           double ParamB = CrossItems[BIndex].Param1;

           if (ParamB == B.Bounds[0].Count)
                ParamB = 0;
            if ((CrossItems[BIndex].Param2 != CrossItems[Idx + 1].Param2) && (CrossItems[BIndex].Border1 == BorderBehavior.NoBorder))
            {


                double Param = DualEdge(A, ParamA, ref DualA);

                double Param0 = DualEdge(A, ParamA, ref DualA);
                CI = Intersecting(DualA, B);

                if (CI != null) return CI;
                else
                {
                    DualEdge(B, ParamB, ref DualB);
                    CI = TurnAround(DualB, Param0, A);
                }
                if (CI != null) return CI;
            }
             double Param2 = DualEdge(B, ParamB, ref DualB);
             double Param1 = DualEdge(A, ParamA, ref DualA);
             Face StartDummy = DualA;
             while (CI == null)
             {
                CI = TurnAround(DualB, Param2, DualA);
                if (CI != null) return CI;
                Param1 = DualEdge(DualA, Param1, ref DualA);
                if (DualA == StartDummy)
                     break;
               }
            return CI;
       }
       static void CheckDouble(SetOperation3D.IntersectResult OutPutSection, TraceInfo C)
       {
            int Idx = C.Index;
           if (Math.Abs(C.SortedParams[Idx].Param2 - C.SortedParams[Idx-1].Param2)< 0.0000001)
           {
               if ((int)C.SortedParams[Idx - 1].Tag == 1)
                   OutPutSection.Edge1ParamFrom = C.SortedParams[Idx - 1].Param1;
               else
                   OutPutSection.Edge2ParamFrom = C.SortedParams[Idx - 1].Param1;
                
           }
           if (Math.Abs(C.SortedParams[Idx + 1].Param2 - C.SortedParams[Idx + 1].Param2) < 0.0000001)
          
           {
               if ((int)C.SortedParams[Idx + 1].Tag == 1)
                   OutPutSection.Edge1ParamTo = C.SortedParams[Idx + 1].Param1;
               else
                   OutPutSection.Edge2ParamTo = C.SortedParams[Idx + 1].Param1;
           }
       }
         static SetOperation3D.IntersectResult NextTraceElement(SetOperation3D.IntersectResult InputSection)
        {
            SetOperation3D.IntersectResult OutPutSection = new SetOperation3D.IntersectResult();// &---------
            OldParams = InputSection.SortedParams;
            TraceInfo C = FindNextFace(InputSection.TraceInfo);
            if (C == null)
                return null;
            int idx = C.Index;
            OutPutSection.StartIndexInSortedList = idx;
           List<CrossItem> CrossItems = C.SortedParams;
            if ((int)CrossItems[idx].Tag == 1)
            {
                OutPutSection.Edge1ParamFrom = CrossItems[idx].Param1;
                OutPutSection.Face1 = CrossItems[idx].Face as Face;
            }
            else
            {
                OutPutSection.Edge2ParamFrom = CrossItems[idx].Param1;
                OutPutSection.Face2 = CrossItems[idx].Face as Face;
            }
            if ((int)CrossItems[idx + 1].Tag == 1)
            {
                OutPutSection.Edge1ParamTo = CrossItems[idx + 1].Param1;
                OutPutSection.Face1 = CrossItems[idx+1].Face as Face;
            }
            else
            {
                OutPutSection.Edge2ParamTo = CrossItems[idx + 1].Param1;
                OutPutSection.Face2 = CrossItems[idx+1].Face as Face;
            }
            if (OutPutSection.Face1 == null)
            {
                for (int i = idx + 1; i < C.SortedParams.Count; i++)
                {
                    if (C.SortedParams[i].Face != OutPutSection.Face2)
                    {
                        OutPutSection.Face1 = C.SortedParams[i].Face as Face;
                        break;
                    }
                }
            }
            if (OutPutSection.Face2 == null)
            {
                for (int i = idx + 1; i < C.SortedParams.Count; i++)
                {
                    if (C.SortedParams[i].Face != OutPutSection.Face1)
                    {
                        OutPutSection.Face2 = C.SortedParams[i].Face as Face;
                        break;
                    }
                }
            }

            OutPutSection.SortedParams = C.SortedParams;
            OutPutSection.TraceInfo = C;


            return OutPutSection;
        }
    
    
        static CrossFaceListInfo GetCrossInfo(Face Face1, Face Face2)
        {
            if (Face1 == null) return null;
            if (Face2 == null) return null;
            if (Face1.Tag as Hashtable == null) return null;
            CrossFaceListInfo CI = ((Face1.Tag as Hashtable)[Face2] as CrossFaceListInfo);
            return CI;

        }
        static List<SetOperation3D.IntersectResult>  DebugTrace = null;
        static List<SetOperation3D.IntersectResult> GetIntersectionTrace(SetOperation3D.IntersectResult R)
        {
            SetOperation3D.IntersectResult StartR = R;
            R = StartR;
 
            List<SetOperation3D.IntersectResult> Result = new List<SetOperation3D.IntersectResult>();
            DebugTrace = Result;
            DoRemove = false;
            SetOperation3D.IntersectResult RStart = R;
           RStart = NextTraceElement(RStart);

            if (RStart == null)
               return Result;
           
            R = RStart;
            //R.SortedParams[1].Visited = true;
            DoRemove = true;
            bool DoBreak = false;
         
           
            do
            {
                SetOperation3D.IntersectResult Q = R;
                R = Q;
                R = NextTraceElement(R);
               
                if (R != null)
                  Result.Add(R.Copy());
             
                DoBreak = ((R == null) ||  // Error
                          (R.SortedParams.Count < 4)||
                          ((R.Face1 == RStart.Face1) && (R.Face2 == RStart.Face2) && // Die selben FaceList
                          ((R.Edge2ParamTo == RStart.Edge2ParamTo)
                          && (R.Edge1ParamTo == RStart.Edge1ParamTo)) // gleiche Schnittparameter
                   ));
            }
            while (!DoBreak);
            if (R == null)
                Result.Clear();
           
        
            return Result;
        }

        [Serializable]
        class Fillet : List<IntersectResult>
        {
            bool _Hole = false;
            public bool Hole
            { 
                get { return _Hole;}
                set {
                    _Hole = value; }
                }
             internal bool Visited = false;
     
            internal double From
            {
                get{
                    if (Count == 0) return -1;
                    IntersectResult First = this[0];
                    //----- Debug------------
                    IntersectResult Last = this[Count - 1];
                    if ((((First.Edge1ParamFrom > -1) && ((Last.Edge2ParamTo > -1)) ||
                        (((First.Edge2ParamFrom > -1) && ((Last.Edge1ParamTo > -1)))))))
                    {
                        CrossFaceListInfo FaceListInfo = SetOperation3D.GetCrossInfo(First.Face1, Last.Face2);
                        // Error------------------------------------
                    }
                    else
                    {
                    }
                    if (Type == 1) return First.Edge1ParamFrom;
                    else
                        return First.Edge2ParamFrom;
                  
                   

                }
                set
                {if (Count == 0) return;

                   IntersectResult First = this[0];
                   if (Type == 1) First.Edge1ParamFrom = value;
                   else
                       First.Edge2ParamFrom = value;
                   
                }
                 
            }
            /// <summary>
            /// internal.
            /// </summary>
            internal int Type = 1;
            /// <summary>
            /// internal.
            /// </summary>
            internal double To
            {
                get
                {
                    if (Count == 0) return -1;
                    IntersectResult Last = this[Count-1];
                    if (Type == 1) return Last.Edge1ParamTo;
                    else
                        return Last.Edge2ParamTo;
                   
                }
                set
                {
                    if (Count > 0)
                    {
                        IntersectResult Last = this[Count - 1];
                        if (Type == 1) Last.Edge1ParamTo = value;
                        
                        else
                       Last.Edge2ParamTo = value;
                    }
                }

            }
            public Vertex3d FromVertex
            {
                get
                {
                    if (Count == 0) return null;
                    IntersectResult First = this[0];
                    return First.StartVertex;
                    

                }

            }
            public Vertex3d ToVertex
            {
                get
                {
                    if (Count == 0) return null;
                    IntersectResult Last = this[Count - 1];
                    return Last.EndVertex;
                    
                }

            }
         
            public void Invert()
            {
               
                base.Reverse();
              
                for (int i = 0; i < Count; i++)
                {
                    IntersectResult IntersectResult = this[i];
                    IntersectResult.Invert();
             
                }
            }
        }
        [Serializable]
         class Fillets : List<Fillet>
        {


            /// <summary>
            /// internal.
            /// </summary>
            /// <returns></returns>
            internal static int getStartFillet()
            {
                for (int i = 0; i < SortedFillets.Count; i++)
                {
                    if (SortedFillets[i].Visited) continue;
                    return i;
                }
                return -1;
           }
            /// <summary>
            /// internal.
            /// </summary>
            internal static int getNextFillet(double start, Face Face)
            {
                int BoundId = 0;
                int BoundOrigin = 0;
               
                while (start > Face.Bounds[BoundId].Count )
                {
                    start -= Face.Bounds[BoundId].Count;
                    BoundId++;
                    BoundOrigin += Face.Bounds[BoundId].Count;
                }
                
                double Distance = 1e10;
                int Nearest = -1;
                for (int i = 0; i < SortedFillets.Count; i++)
                {
                    if (SortedFillets[i].Visited) continue;
                    // in the Same Bound ??
                    double D = SortedFillets[i].From - BoundOrigin;
                    if ((D >=0) && (D < Face.Bounds[BoundId].Count))
                    {// in the Same Bound !!

                        double di = D - start;
                        if (di < 0) di += Face.Bounds[BoundId].Count; // Distance
                        if (di <= Distance)
                        {
                            Nearest = i;
                            Distance = di;

                        }
                    }
                    //else
                    //{ }
                }
              
                return Nearest;


            }
            /// <summary>
            /// internal.
            /// </summary>
            internal int getNextFillet1(Face Face,int  EdgeLoop, int start)
            {
                int Result = -1;
                Fillet StartFillet = this[start];
                double From = -1;
                int StartLoop = -1;
                SetOperation3D.GetLocalParam(Face, StartFillet.To, ref StartLoop, ref From);
                double Distance = 1e10;
                int loop = -1;
                double Param = -1;
                int modulo = Face.Bounds[EdgeLoop].Count;
                int Id = start+1; ; if (Id >= Count) Id = 0;
                int To = Id;
                do
                {
                    Fillet Candidate = this[Id];
                    if (Candidate.Visited)
                    {

                        Id++;
                        if (Id >= Count) Id = 0;
                        continue;
                    }



                    SetOperation3D.GetLocalParam(Face, Candidate.From, ref loop, ref Param);
                    if (loop == StartLoop)
                    {
                        double Dist = 0;
                        if (Param < From)
                            Dist = Param + modulo;


                        if (Dist < Distance)
                        {
                            Result = Id;
                            Distance = Dist-From;

                        }

                    }
                    Id++;
                    if (Id >= Count) Id = 0;


                }
                while (Id != To);
                return Result;
            }
           

        }
     
        
        static int TraceIsHole(List<IntersectResult> Trace)
        {
            if (Trace.Count == 0) return -1;
            double Param1 = -1;
            double Param2 = -1;
            int Ok1 = 0;
            int Ok2 = 0;
            for (int i = 0; i < Trace.Count; i++)
            {
                if (Trace[i].Edge1ParamTo >= 0) Ok1++;
                if (Trace[i].Edge2ParamTo >= 0) Ok2++;
                if (Param1 == -1)
               
                    Param1 = Trace[i].Edge1ParamTo;
                if (Param2 == -1)
                    Param2 = Trace[i].Edge2ParamTo;
                
            }
            if ((Param1 > -1) && (Param2 > -1))
               if  ((Ok1 < Trace.Count) && (Ok2 < Trace.Count))
                return -1;
            if (Param1 > -1)
            {
                if (Trace.Count > 2)
                {

                    if (Trace[0].StartVertex.Value.dist(Trace[Trace.Count - 1].EndVertex.Value) < 0.01)
                    {
                      
                        return 1;
                    }
                }
               
            }
            else
            {
                if (Trace.Count > 2)
                {

                    if (Trace[0].StartVertex.Value.dist(Trace[Trace.Count - 1].EndVertex.Value) < 0.01)
                    {
                        return 2;
                    }
                }
            }
            return -1;

  
           
        }
        /// <summary>
        /// internal.
        /// </summary>
        static void FilletFaceList(List<IntersectResult> Trace,List<Face> Infected1,List<Face> Infected2)
        {
            List<Fillet> SegmentList2 = new List<Fillet>();
            List<Fillet> SegmentList1 = new List<Fillet>();
            if (Trace.Count == 0) return;
            Face StartFace1 = Trace[0].Face1;
            Face Test = StartFace1;
            Face StartFace2 = Trace[0].Face2;
          
            Face CurrentFace1 = Trace[0].Face1;
           
                if (!Infected1.Contains(CurrentFace1)) Infected1.Add(CurrentFace1);
            Face CurrentFace2 = Trace[0].Face2;
            if (!Infected2.Contains(CurrentFace2)) Infected2.Add(CurrentFace2);
           
            Fillet CurrentStartSegments1 = new Fillet(); ;
            SegmentList1.Add(CurrentStartSegments1);
            CurrentStartSegments1.Type = 1;
            CurrentStartSegments1.Hole = false;
           Fillet CurrentStartSegments2 = new Fillet();
           SegmentList2.Add(CurrentStartSegments2);
            CurrentStartSegments2.Type = 2;
            CurrentStartSegments2.Hole = false;
            Fillets S1 = getFillets(CurrentFace1);
            Fillets S2 = getFillets(CurrentFace2);
            S1.Add(CurrentStartSegments1);
            S2.Add(CurrentStartSegments2);
            Fillet CurrentSegement1 = CurrentStartSegments1;
            Fillet CurrentSegement2 = CurrentStartSegments2;
          
            for (int i = 0; i < Trace.Count; i++)
            {
               if (Trace[i].Face1 != CurrentFace1)
                {
                    CurrentFace1 = Trace[i].Face1;
                   
                    if (!Infected1.Contains(CurrentFace1)) Infected1.Add(CurrentFace1);
                    S1 = getFillets(CurrentFace1);
                    CurrentSegement1 = new Fillet();
                    S1.Add(CurrentSegement1);
                }
                if (Trace[i].Face2 != CurrentFace2)
                {
                    CurrentFace2 = Trace[i].Face2;
                    
                    if (!Infected2.Contains(CurrentFace2)) Infected2.Add(CurrentFace2);
                    S2 = getFillets(CurrentFace2);
                    CurrentSegement2 = new Fillet();
                    SegmentList2.Add(CurrentStartSegments2);
                    CurrentSegement2.Type = 2;
                    S2.Add(CurrentSegement2);
                }
              
                int Id2 = (SolidA.FaceList.IndexOf(CurrentFace1));
                CurrentSegement1.Add(Trace[i].Copy());
                CurrentSegement2.Add(Trace[i].Copy());
            }

            if (CurrentFace1 == StartFace1)
            {
               
                if (CurrentStartSegments1 != CurrentSegement1)
                {
                    CurrentSegement1.AddRange(CurrentStartSegments1);
                  
                    getFillets(StartFace1).Remove(CurrentStartSegments1);
                    SegmentList1.RemoveAt(0);
                }
                int II = TraceIsHole(CurrentSegement1);
                if (II >= 0)
                {
                    CurrentSegement1.Type = II;
                }

            }
           
            if (CurrentFace2 == StartFace2)
            {

                if (CurrentStartSegments2 != CurrentSegement2)
                {
                    CurrentSegement2.AddRange(CurrentStartSegments2);
                 
                    getFillets(StartFace2).Remove(CurrentStartSegments2);
                    SegmentList2.RemoveAt(0);
                }
                int II = TraceIsHole(CurrentSegement2);
                if (II >= 0)
                {
                    CurrentSegement2.Type = II;
                }

            }
     

        }
        static double GetParam(xyz Pt, Face F)
        {
           Bounds B = F.Bounds;
           double Lam = -1;
            LineType L = new LineType(Pt, F.Surface.Base.BaseZ);
            for (int j = 0; j < B.Count; j++)
            {
                EdgeLoop EL = B[j];
                xyzArray PTs = new xyzArray(EL.Count + 1);

                for (int k = 0; k < EL.Count; k++)

                    PTs[k] = EL[k].EdgeStart.Value;
                PTs[EL.Count] = EL[EL.Count - 1].EdgeEnd.Value;
                //   PTs.Invert();
                double di = PTs.Distance(Pt, 100000, out Lam);
                if (di < 0.0001)
                {
                    Lam = Math.Round(Lam, 6);
                    return Lam;
                }
                else
                { }
            }
            return -1;
        }
      
       static Fillets getFillets(Face Face)
        {
            Fillets Result = null;
            if (Face.Bounds[0].Tag == null)
            {
                Result = new Fillets();
                Face.Bounds[0].Tag = Result;
            }
            else
                Result = Face.Bounds[0].Tag as Fillets;
           
            return Result;
        }
    //---------------------------Split FaceList--------------------------

       
      static  CurveArray GetParamCurves(EdgeLoop EL, Surface S)
        {
            CurveArray CA = new CurveArray();
            for (int i = 0; i < EL.Count; i++)
            {
                CA.Add(S.To2dCurve( EL[i].EdgeCurve));
                if (!EL[i].SameSense)
                    CA[CA.Count - 1].Invert();
            }
            return CA;
        }
       
        static Fillets SortedFillets = null;
        static Fillets SortFillets(Fillets SegmentList)
        {
        
            Fillets Result = new Fillets();
            for (int i = 0; i < SegmentList.Count; i++)
            {
                Fillet Segments = SegmentList[i];
                int k = 0;
                double From =System.Math.Round( Segments.From,8);
                while ((k < Result.Count) && (From>   Result[k].From +0.000000001)) k++;
               Result.Insert(k, Segments);
                // Sort to Result
            }
            SortedFillets = Result;
            return Result;
        }
    
       
         static void TransformRelativ(Solid Solid, Matrix RelativTransformation)

        {
            Matrix Relativ = RelativTransformation;
            Solid.CoordTransform(Relativ);
            Solid.Transformation = Matrix.identity;
            
        }
       
      
        //---------------- GetConnectedSolid ------------------------------
         static void getConnectedSolid(Solid Solid, Face StartFace)
        {
            // Find not influenced Neighbor
            
            if (StartFace.Tag != null) return;
            StartFace.Tag = 1;
            bool Infected = (Infected1.Contains(StartFace) || Infected2.Contains(StartFace));
            if (!Solid.FaceList.Contains(StartFace))
                Solid.FaceList.Add(StartFace);
           
                        for (int j = 0; j < StartFace.Bounds.Count; j++)
            {
                
            if(    SolidA.FaceList.IndexOf(StartFace)==1)
                { }
                for (int i = 0; i < StartFace.Bounds[j].Count; i++)
                {
                    bool OK = CEdges(StartFace);
                    if (!OK)
                    { int id1 = SolidA.FaceList.IndexOf(StartFace);
                        int id2 = SolidB.FaceList.IndexOf(StartFace);
                    }
                    if (StartFace.Bounds[j][i].EdgeStart.Value.dist(StartFace.Bounds[j][i].EdgeEnd.Value) < 0.0001) continue;
                   if (!Infected)
                        StartFace.Bounds[j][i] = newEdge(StartFace.Bounds[j][i].EdgeStart, StartFace.Bounds[j][i].EdgeEnd, StartFace.Bounds[j][i].EdgeCurve.Neighbors);
                  
                    Edge E = StartFace.Bounds[j][i];

                    Solid.EdgeList.Add(E);
                    if (!Solid.EdgeCurveList.Contains(E.EdgeCurve))
                        Solid.EdgeCurveList.Add(E.EdgeCurve);
                    
                    if (!Solid.VertexList.Contains(E.EdgeEnd))
                        Solid.VertexList.Add(E.EdgeEnd);
                    if (!Solid.VertexList.Contains(E.EdgeStart))
                        Solid.VertexList.Add(E.EdgeStart);
                    OK = CEdges(StartFace);
                    if (!OK)
                    { }
                    Face Neigbor = StartFace.Neighbor(j, i) as Face;
                    if (Neigbor != null)
                    {
                        if (!Solid.FaceList.Contains(Neigbor))
                            getConnectedSolid(Solid, Neigbor);
                        
                    }
                } 
            }
        }
 
        static void InvertFillets(Face Face)
        {
            Fillets S = getFillets(Face);
            for (int i = 0; i < S.Count; i++)
               S[i].Invert();    
        }
       
        static void Complement(Solid Solid)
        {
            for (int i = 0; i < Solid.FaceList.Count; i++)
            {
                Face F = Solid.FaceList[i];
                F.Surface.SameSense = !F.Surface.SameSense;
                for (int j = 0; j < F.Bounds.Count; j++)
                {
                    EdgeLoop EL = F.Bounds[j];
                    EL.Reverse();
                    
                    for (int k = 0; k < EL.Count; k++)
                    {
                        Edge E = EL[k];
                        Vertex3d A = E.EdgeStart;
                        E.SameSense = !E.SameSense;
                        E.EdgeStart = E.EdgeEnd;
                        E.EdgeEnd = A;
                       
                    }
                }
            }
           Solid.RefreshParamCurves();
        }
        /// <summary>
        /// are the set operations for <see cref="GetCombinedSolids(DiscreteSolid, DiscreteSolid, SetOperation)"/>.
        /// </summary>
        public enum SetOperation
        {
            /// <summary>
            /// <b>and</b> operator.
            /// </summary>
            AandB,
            /// <summary>
            /// <b>not</b> operator.
            /// </summary>
            AnotB,
            /// <summary>
            /// <b>or</b> operator.
            /// </summary>
            AorB
        }
         static List<List<IntersectResult>> _AllTraces = new List<List<IntersectResult>>();
         static List<List<IntersectResult>> getTraces(DiscreteSolid Solid1, DiscreteSolid Solid2)
        {   
            Matrix M = Solid2.Transformation;
            TransformRelativ(Solid2, M);
            M = Solid1.Transformation;
            TransformRelativ(Solid1, M);
            // sets for all faces of Solid1 in the Tag a Hashtable with Key Face of Solid2 and Object a CrossFaceListInfo
            Drawing3d.Cross.CrossAllFaceList(Solid1, Solid2);
            List<List<IntersectResult>> AllTraces = new List<List<IntersectResult>>();
            List<IntersectResult> Trace = null;
            IntersectResult Cross = null;
            do
            {
                Cross = StartTrace(Solid1, Solid2);

                if (Cross != null)
                {

                    Trace = GetIntersectionTrace(Cross);

                    if (Trace.Count > 2)
                        AllTraces.Add(Trace);
                    else
                    { }
                    if (Trace.Count == 0) return AllTraces;



                }

            }
         
          while ((Cross != null));
            
          return AllTraces;
        }
      
       static void DoCompleteTraceItems(List<List<IntersectResult>> AllTraces)
        {
            List<IntersectResult> Trace = null;
            for (int i = 0; i < AllTraces.Count; i++)
            {
                Trace = AllTraces[i];
                if (Trace.Count < 2)
                    continue;
                Vertex3d StartVertex =newEndVertex(Trace[Trace.Count - 1]);
                for (int j = 0; j < Trace.Count; j++)
                {


                    if (Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 2].Param2 ==
                        Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1].Param2
                        )

                    {


                        int idx = SolidA.FaceList.IndexOf(Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1].Face);
                        int idx2 = SolidB.FaceList.IndexOf(Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1].Face);
                        object Tag = Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1].Tag;
                        object Tag2 = Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1].Tag;
                        CrossItem C = Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 2];
                        Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 2] = Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1];
                        Trace[j].SortedParams[Trace[j].StartIndexInSortedList + 1] = C;
                    }

                }
            }
         
            for (int i = 0; i < AllTraces.Count; i++)
            {
                Trace = AllTraces[i];
                if (Trace.Count < 2)
                    continue;
               Vertex3d StartVertex = newEndVertex(Trace[Trace.Count - 1]);
                for (int j = 0; j < Trace.Count; j++)
                {
                    Vertex3d EndVertex = null;
                    if (j == Trace.Count - 1)
                        EndVertex = Trace[0].StartVertex;
                    else
                        EndVertex = newEndVertex(Trace[j]);
                
                    Trace[j].StartVertex = StartVertex;
                    Trace[j].EndVertex = EndVertex;
                    Edge Edge = new Drawing3d.Edge();
                    Edge.EdgeStart = StartVertex;
                    Edge.EdgeEnd = EndVertex;
                   
                    StartVertex = EndVertex;
                    Trace[j].Neighbors = new Face[2];
                    Trace[j].Neighbors[0] = Trace[j].Face1;
                    Trace[j].Neighbors[1] = Trace[j].Face2;
                }
           }
          
        }
        static void SegmentToEdgeLoop(Fillet Segment,EdgeLoop EL,Surface Surface)
        {
            for (int i = 0; i < Segment.Count; i++)
            {
                  Edge Edge = newEdge(Segment[i].StartVertex, Segment[i].EndVertex, Segment[i].Neighbors);
                    EL.Add(Edge);

             }
           
        }
        static void SubEdgeLoop(EdgeLoop EL,Vertex3d Start,Vertex3d End, double from, double to, EdgeLoop Destination)
        {
            if (to < 0)
            {
                return;
            }
            if (from < 0)
            {
                if (Math.Abs(from) < 0.000001) from = 0;
                else
                return;
            }
           
          
            int fromId = (int)from;
            if (EL.Count == fromId)
            {
                fromId =0;
                from = 0;
            }
            int toId = (int)to;
        
            if (EL.Count == toId)
            {
                toId = EL.Count - 1;
                to = fromId + 0.9999999;
            }
        
            if ((fromId == toId)&&(to -from >0.000001))
            {
                Edge EE = newEdge(Start,End, EL[fromId].EdgeCurve.Neighbors);
                Destination.Add(EE);
                
            }
             
            else
            {

                // start
             if ((Math.Abs(1-(from - fromId))>0.00001))
                {
                   
                    int N = fromId + 1;
                    if (N >= EL.Count) N = 0;
                    Vertex3d EdgeStart = EL[N].EdgeStart;
                    Edge EE = newEdge(Start, EdgeStart, EL[fromId].EdgeCurve.Neighbors);//<---------------
                    Destination.Add(EE);
                    
                }
             else
             {
             }
                int i = fromId + 1;
                if (i >= EL.Count) i = 0;

                while (i != toId)
                {
               
                    Edge NewEdge = newEdge(EL[i].EdgeStart, EL[i].EdgeEnd,EL[i].EdgeCurve.Neighbors);
                    Destination.Add(NewEdge);
                    i = i + 1;
                    if (i >= EL.Count) i = 0;
                }
              
      
              if (System.Math.Abs(  to - toId)>0.0001)
                {
                    Vertex3d EdgeStart = EL[toId].EdgeStart;
                
                    Edge EE = newEdge(EdgeStart, End, EL[toId].EdgeCurve.Neighbors);
                    if (Destination.Count > 0)
                    {
                       
                        Vertex3d A = Destination[Destination.Count - 1].EdgeEnd;
                        Vertex3d B = EE.EdgeStart;
                      
       
                    }
                    Destination.Add(EE);
                }
            }
        }
  
        static Edge newEdge(Vertex3d StartVertex, Vertex3d EndVertex, Object[] Neighbors)
        {
          StartVertex = FindVertex(StartVertex);
           
                EndVertex = FindVertex(EndVertex);

            Edge E = null;
            Curve3D C = null;
            if (StartVertex.Tag != null)
                C = ((Hashtable)StartVertex.Tag)[EndVertex] as Curve3D;
     

            if (C == null)
            {
               Hashtable H = EndVertex.Tag as Hashtable;
                if (H == null)
                {
                    H = new Hashtable();
                    EndVertex.Tag = H;

                }
                Edge[] Edges = null;
                C = H[StartVertex] as Curve3D;
                if (C != null)
                {
                    // Error
                    Edges = C.Tag as Edge[];
                 
                }
                else
               {
                    C = new Line3D(StartVertex.Value, EndVertex.Value); // OK
                    C.Tag = new Edge[2];
                    C.Neighbors = Neighbors;
                    H.Add(StartVertex, C);
                    
                }
                Edges = C.Tag as Edge[];
                E = new Edge();
                E.EdgeStart = StartVertex;
                E.EdgeEnd = EndVertex;
                E.EdgeCurve = C;
                E.SameSense = true;
                if (Edges[0] == null) Edges[0] = E; else Edges[1] = E;
            }
            else
            {
                object[] N = C.Neighbors;
                E = new Edge();
                Edge[] Edges = C.Tag as Edge[];
                E.EdgeStart = StartVertex;
                E.EdgeEnd = EndVertex;
                E.EdgeCurve = C;
                E.SameSense = false;
                if (Edges[0] == null) Edges[0] = E; else Edges[1] = E;
            }
            return E;
     }
      
   
         static void ComplementFillets(List<Face> Infected)
        {
            for (int i = 0; i < Infected.Count; i++)
            {
                ComplementFillets(Infected[i]);
            }
        }
        static double ComplementGlobalParams(Face Face,double Param)
        {
            double T = Param;
            int B = 0;
            int i = 0;
            for ( i = 0; i < Face.Bounds.Count; i++)
            {
                if (T - Face.Bounds[i].Count > 0)
                {
                    T -= Face.Bounds[i].Count;
                    B += Face.Bounds[i].Count;
                }
                else break;
            }
            if (Param > 4)
            {
            }
            if (T > 0)
                return B + (Face.Bounds[i].Count - T);
            else
                return B;
        }
         static void ComplementFillets(Face Face)

        {
            Fillets Fillets = getFillets(Face);
            for (int i = 0; i < Fillets.Count; i++)
            {
                Fillet Fillet = Fillets[i];
              if (Fillet.Count == 0) continue;
              if (Fillet[0].Face1 == Face)
                {
                    for (int j = 0; j < Fillet.Count; j++)
                    {
                        if (Fillet[j].Edge1ParamFrom >= 0)
                            Fillet[j].Edge1ParamFrom = ComplementGlobalParams(Face, Fillet[j].Edge1ParamFrom);
                        if (Fillet[j].Edge1ParamTo >= 0)
                            Fillet[j].Edge1ParamTo = ComplementGlobalParams(Face, Fillet[j].Edge1ParamTo);

                    }
                }

                   if (Fillet[0].Face2 == Face)
                    {
                      
                        for (int j = 0; j < Fillet.Count; j++)
                        {
                          
                            if (Fillet[j].Edge2ParamFrom >= 0)
                                Fillet[j].Edge2ParamFrom = ComplementGlobalParams(Face, Fillet[j].Edge2ParamFrom);
                            if (Fillet[j].Edge2ParamTo >= 0)
                                Fillet[j].Edge2ParamTo = ComplementGlobalParams(Face, Fillet[j].Edge2ParamTo);
                       }

                    }

                }
            
        }
    
         static void CheckHoles(Face Face)
        {
            Fillets F = getFillets(Face);
            
            for (int i = 0; i < F.Count; i++)
            {

             int hh=   TraceIsHole(F[i]);
                if (hh >= 0)
                {
                    F[i].Hole = true;
                    F[i].Type = hh;


                }
                else
                    F[i].Hole = false;
            }
           
        }
       static void MakeNewBounds(List<Face> Infected)
        {
            for (int i = 0; i < Infected.Count; i++)
            {
                SetOperation3D.MakeBoundWithFillets(Infected[i]);
              
            }
        }

      
       
        static bool CEdges(Face F)
        {
            for (int i = 0; i < F.Bounds.Count; i++)
            {
                for (int j = 0; j < F.Bounds[i].Count; j++)
                {
                    Edge E = F.Bounds[i][j];
                    object[] N = E.EdgeCurve.Neighbors;
                    if ((N[0] != F) && (N[1] != F))
                        return false;
                }
            }

            return true;
        }




        static bool[] touched(Face F)
        {
            bool[] Result = new bool[F.Bounds.Count];
            Fillets Fillets = getFillets(F);
            int Loop = -1;
            double dummy = -1;
            for (int i = 0; i < Fillets.Count; i++)
            {

                for (int j = 0; j < Fillets[i].Count; j++)
                {
                //    if (Fillets[i].Hole) continue;

                    IntersectResult IR = Fillets[i][j];
                    if ((IR.Edge1ParamFrom >= 0) && (IR.Face1 == F))
                    {
                        GetLocalParam(F, IR.Edge1ParamFrom, ref Loop, ref dummy);
                        Result[Loop] = true;
                    }
                    if ((IR.Edge2ParamFrom >= 0) && (IR.Face2 == F))
                    {
                        GetLocalParam(F, IR.Edge2ParamFrom, ref Loop, ref dummy);
                        Result[Loop] = true;
                    }
                    if ((IR.Edge1ParamTo >= 0) && (IR.Face1 == F))
                    {
                        GetLocalParam(F, IR.Edge1ParamTo, ref Loop, ref dummy);
                        Result[Loop] = true;
                    }
                    if ((IR.Edge2ParamTo >= 0) && (IR.Face2 == F))
                    {
                        GetLocalParam(F, IR.Edge2ParamTo, ref Loop, ref dummy);
                        Result[Loop] = true;
                    }
                }
            }

            return Result;
        }
        static void MakeBoundWithFillets(Face Face)
        {
            Bounds NewBounds = new Bounds();
            Fillets Fillets = getFillets(Face);
            if (Fillets.Count == 0) return;      
            EdgeLoop Destination = new EdgeLoop();
            NewBounds.Add(Destination);
            Fillets = SortFillets(Fillets);
            
            List<Fillet> Holes = new List<Fillet>();
            for (int i = 0; i < Fillets.Count; i++)
            {
                if (Fillets[i].Hole)
                {
                    //   Holes.Add( Fillets[i]);
                    Holes.Insert(0, Fillets[i]);
                    Fillets.RemoveAt(i);
                    i--;
                }

            }
              if (Holes.Count > 0)

                {
                    {
                        NewBounds.Clear();
                       for (int i = Holes.Count - 1; i >= 0; i--)
                        {
                            Destination = new EdgeLoop();
                            SegmentToEdgeLoop(Holes[i], Destination, Face.Surface);
                            NewBounds.Add(Destination);
                        }
                        CurveArray CA = GetParamCurves(Destination, Face.Surface);
                        CurveArray CAOuter = GetParamCurves(Face.Bounds[0], Face.Surface);

                        if (CAOuter.ClockWise && (!CA.ClockWise))//

                        {
                            EdgeLoop D = new EdgeLoop();
                            for (int i = 0; i < Face.Bounds[0].Count; i++)
                            {
                                D.Add(newEdge(Face.Bounds[0][i].EdgeStart, Face.Bounds[0][i].EdgeEnd, Face.Bounds[0][i].EdgeCurve.Neighbors));
                            }

                            NewBounds.Insert(0, D);

                        }
                        else
                       
                      if (CA.ClockWise)
                        {

                            EdgeLoop D1 = new EdgeLoop();

                            for (int i = 0; i < Face.Bounds[0].Count; i++)
                            {
                                D1.Add(newEdge(Face.Bounds[0][i].EdgeStart, Face.Bounds[0][i].EdgeEnd, Face.Bounds[0][i].EdgeCurve.Neighbors));
                            }

                            NewBounds.Insert(0, D1);
                          
                        }
                        if (NewBounds.Count == 0)
                            NewBounds.Add(new EdgeLoop()); // Empty
                    }

                }

                if (Fillets.Count > 0)
                {
                    int CurrentLoop = -1;
                    int CurrentLoopv = -1;
                    int Idx = 0;
                    while (Idx != -1)
                    {
                        int Start = Idx;


                        do
                        {
                            int OldIdx = Idx;

                            Idx = Fillets.getNextFillet(Fillets[Idx].To, Face);
                            if (Idx != -1)
                            {
                                Fillets[Idx].Visited = true;
                                int DC = Destination.Count;
                              
                                if (Fillets[OldIdx].Count > 0)
                                    SegmentToEdgeLoop(Fillets[OldIdx], Destination, Face.Surface);

                                double from = -1;
                                GetLocalParam(Face, Fillets[OldIdx].To, ref CurrentLoop, ref from);
                                double To = -1;
                                GetLocalParam(Face, Fillets[Idx].From, ref CurrentLoopv, ref To);
                                DC = Destination.Count;

                                SubEdgeLoop(Face.Bounds[CurrentLoop], Fillets[OldIdx].ToVertex, Fillets[Idx].FromVertex, from, To, Destination);
                            }

                        }

                        while ((Idx != Start) && (Idx != -1));

                        Destination = new EdgeLoop();
                        NewBounds.Add(Destination);
                        Fillets[Start].Visited = true;
                        if (Idx >= 0)
                            Idx = Fillets.getStartFillet();
                    }
              }
                bool[] Touched = touched(Face);
                for (int i = 1; i < Face.Bounds.Count; i++)
                {

                    if (!Touched[i])
                    {
                       
                        bool _Inside = false;
                        for (int j = 1; j < NewBounds.Count; j++)
                        {
                            if (Face.Bounds[i].Count > 0)
                            {
                                bool Ins = Inside(Face.Bounds[i][0].EdgeStart.Value, NewBounds[j], Face.Surface);
                                if (Ins)
                                {
                                   _Inside = true;
                                    break;
                                }
                            }
                        }
                        if (Inside(Face.Bounds[i][0].EdgeStart.Value, NewBounds[0], Face.Surface))
                            {
                            if (!_Inside)
                            {
                                EdgeLoop D1 = new EdgeLoop();
                                for (int j = 0; j < Face.Bounds[i].Count; j++)
                                {
                                    D1.Add(newEdge(Face.Bounds[i][j].EdgeStart, Face.Bounds[i][j].EdgeEnd, Face.Bounds[i][j].EdgeCurve.Neighbors));
                                }
                                NewBounds.Insert(0, D1);
                            }
                           }
                    }
                }
                if (Destination.Count == 0)
                    NewBounds.Remove(Destination);
                Face.Bounds = NewBounds;
                Face.RefreshParamCurves();


            

        }
        static bool Inside(xyz Pt, EdgeLoop E,Surface S)
        {
            CurveArray CA = GetParamCurves(E, S);
            xy P = S.Base.Relativ(Pt).toXY();
            return CA.InsidePt(P);
        }
     
        static void InvertFillets(List<Face> infected)
        {
            for (int i = 0; i < infected.Count; i++)
            {
                SetOperation3D.Fillets F = SetOperation3D.getFillets(infected[i]);
                 for (int j = 0; j < F.Count; j++)
                     F[j].Invert();
            }
        }
      static void RepairFillets_From_To(List<Face> infected)
        {
            for (int i = 0; i < infected.Count; i++)
            {
              
               SetOperation3D.Fillets F = SetOperation3D.getFillets(infected[i]);
              SetOperation3D.CheckHoles(infected[i]);
                for (int j = 0; j < F.Count; j++)
                {
                    if (F[j].To < 0)
                       F[j].To = SetOperation3D.GetParam(F[j][F[j].Count - 1].EndVertex.Value, infected[i]);
                 
                    if (F[j].From < 0)
                     F[j].From = SetOperation3D.GetParam(F[j][0].StartVertex.Value, infected[i]);

                 
                }
            }
        }
       
        static void GetFillets(List<List<IntersectResult>> AllTraces,List<Face> Infected1,List<Face> Infected2)
        {
            for (int i = 0; i < AllTraces.Count; i++)
			{
                FilletFaceList(AllTraces[i],Infected1,Infected2);
			}
             
        }
        static void ClearTags(Solid Solid)
        {
            for (int i = 0; i < Solid.FaceList.Count; i++)
            {
                Solid.FaceList[i].Tag = null;
            }
          
        }
     
        static Vertex3d newEndVertex(IntersectResult IntersectResult)
        {
          Vertex3d Result = null;
          if (IntersectResult.Edge1ParamTo > -1)
             Result = new Vertex3d(Eval(IntersectResult.Face1, IntersectResult.Edge1ParamTo));
           if (IntersectResult.Edge2ParamTo > -1)
               Result = new Vertex3d(Eval(IntersectResult.Face2, IntersectResult.Edge2ParamTo));
         
            return Result;

           
         
        }

        static Solid SolidA = null; // zu Testzwecken
        static Solid SolidB = null;
        static List<DiscreteSolid> GetCombinedSolids(DiscreteSolid Solid1, DiscreteSolid Solid2, SetOperation SetOperation)
        {
            return GetCombinedSolids(Solid1, Solid2, SetOperation, null);
        }
     
        static List<Face> Infected1 = new List<Face>();
        static void TraceRemoveEmpties(List<List<IntersectResult>> IR)
        {
            for (int i = 0; i < IR.Count; i++)
            {
                for (int j = 0; j < IR[i].Count; j++)
                {
                    IntersectResult G = IR[i][j];
                    xyz A = new xyz(0, 0, 0);
                    xyz B = new xyz(0, 0, 0);
                    if (G.Edge2ParamFrom < 0)
                    {
                        A = Eval(G.Face1, G.Edge1ParamFrom);
                       
                    }
                    else
                        A = Eval(G.Face2, G.Edge2ParamFrom);
                    if (G.Edge2ParamTo < 0)
                    {
                        B = Eval(G.Face1, G.Edge1ParamTo);
                       
                    }
                    else
                        B= Eval(G.Face2, G.Edge2ParamTo);
                    if (A.dist(B) <0.0001)
                    {
                        IR[i].Remove(G);
                        j--;
                    }
                }
            }
        }
        static void SetParams(List<List<IntersectResult>> IR)
        {
           
                    for (int i = 0; i < IR.Count; i++)
            {
                for (int j = 0; j < IR[i].Count; j++)
                {
                    int id = SolidA.FaceList.IndexOf(IR[i][j].Face1);

                    IntersectResult G = IR[i][j];
                   
                    if ((G.Edge1ParamFrom < 0))
                    {
                        xyz P = Eval(G.Face2, G.Edge2ParamFrom);
                        G.Edge1ParamFrom = GetParam(P, G.Face1);
                        G.SortedParams[G.StartIndexInSortedList].Param1 = G.Edge1ParamFrom;
                        if (G.SortedParams[G.StartIndexInSortedList + 1].Param2 == G.SortedParams[G.StartIndexInSortedList + 2].Param2)
                        {

                        }
                    }
                    
                    if (G.Edge1ParamTo < 0)

                    {


                        xyz P = Eval(G.Face2, G.Edge2ParamTo);
                        G.Edge1ParamTo = GetParam(P, G.Face1);
                        xyz Q = Eval(G.Face1, G.Edge1ParamTo);
                        G.SortedParams[G.StartIndexInSortedList + 1].Param1 = G.Edge1ParamTo;
                        if (G.SortedParams[G.StartIndexInSortedList + 1].Param2 == G.SortedParams[G.StartIndexInSortedList + 2].Param2)
                        {

                        }
                    }
               

                    //---------------------------------------------
                    if (G.Edge2ParamFrom == -1)
                    {

                        xyz P = Eval(G.Face1, G.Edge1ParamFrom);
                        G.Edge2ParamFrom = GetParam(P, G.Face2);
                        if (G.Edge2ParamFrom >= 0)
                            G.SortedParams[G.StartIndexInSortedList].Param1 = G.Edge2ParamFrom;
                    }
                    if (G.Edge2ParamTo == -1)
                    {

                        xyz P = Eval(G.Face1, G.Edge1ParamTo);
                        G.Edge2ParamTo = GetParam(P, G.Face2);
                        if (G.Edge2ParamTo >= 0)
                            G.SortedParams[G.StartIndexInSortedList + 1].Param1 = G.Edge2ParamTo;
                    }


                    if ((Math.Abs((int)G.Edge1ParamFrom - G.Edge1ParamFrom) < 0.0000001) && (Math.Abs((int)G.Edge1ParamTo - G.Edge1ParamTo) < 0.0000001))
                        if ((G.Edge1ParamFrom >= 0) || (G.Edge1ParamTo >= 0))
                        {

                            if (G.Edge1ParamFrom == -1)
                            {

                                xyz P = Eval(G.Face2, G.Edge2ParamFrom);
                                G.Edge1ParamFrom = GetParam(P, G.Face1);
                            }
                            if (G.Edge1ParamTo == -1)
                            {

                                xyz P = Eval(G.Face2, G.Edge2ParamTo);
                                G.Edge1ParamTo = GetParam(P, G.Face1);
                            }




                        }
                    if (G.Edge1ParamFrom >= 0)
                        if (G.Edge1ParamFrom == G.Edge1ParamTo)
                        {
                            IR[i].Remove(G);
                            j--;
                            continue;
                        }


                }
            }
        }


       static bool MoveToNeighbor(ref Face Face,ref double FromParam, ref double ToParam,List<CrossItem> SortedParams,int Id)
        {
          
                if ((FromParam >= 0) && (ToParam >= 0))
                    {
               
                int Loop = -1;
                double Param = -1;
                GetLocalParam(Face, FromParam, ref Loop, ref Param);
                double From = Param;
                GetLocalParam(Face, ToParam, ref Loop, ref Param);
                double To = Param;
             
                double n = From + 1;
                if (n >= Face.Bounds[Loop].Count) n -= Face.Bounds[Loop].Count;
           
               if ((((int)From==(int)To)||((int)n==To)) || (n==To))
                 {
                              
                    Face Neighbor = null;
                    double DualFrom = -1;
                    double DualTo = -1;
                    int DualFromLoop = 0;
                    int DualToLoop = 0;
                    if ((int)From != From)
                    {
                        DualFrom = GetDualEdge(Face, Loop, From, ref DualFromLoop, ref Neighbor);

                     
                        if (Math.Abs(To - (int)To) > 0.0001)
                            DualTo = GetDualEdge(Face, Loop, To - 0.000001, ref DualToLoop, ref Neighbor);
                        else
                   
                        {
                            DualToLoop = DualFromLoop;
                            DualTo = (int)DualFrom;
                        }

                    }
                    else
                        if ((int)To != To)
                        {
                        DualTo = GetDualEdge(Face, Loop, To, ref DualToLoop, ref Neighbor);
                        DualFrom = (int)(DualTo + 1);
                        if (DualFrom >= Neighbor.Bounds[Loop].Count)
                            DualFrom = 0;
                        DualFromLoop = DualToLoop;
                       }
                    else
                    {
                        DualFrom = GetDualEdge(Face, Loop, From, ref DualFromLoop, ref Neighbor);
                        DualTo = DualFrom - 1;
                        if (DualTo < 0)
                            DualTo = Neighbor.Bounds[Loop].Count - 1;
                        DualToLoop = DualFromLoop;

                    }
                       
                        DualFrom = GetGlobalParam(Neighbor, DualFromLoop, DualFrom);
                   
                    Face = Neighbor;
                    FromParam = DualFrom;
                    ToParam = DualTo;
                    SortedParams[Id].Param1 = DualFrom;
                    SortedParams[Id + 1].Param1 = DualTo;
                    SortedParams[Id].Face = Neighbor;
                    SortedParams[Id + 1].Face = Neighbor;

                    return true;
                       
                      }
                    }

            return false;
        }
       static void CheckTrace(List<List<IntersectResult>> IR,int Kind)
        {
          for (int i = 0; i < IR.Count; i++)
            {
                for (int j = 0; j < IR[i].Count; j++)
                {
                    IntersectResult G = IR[i][j];
                    if (Kind == 1)
                        MoveToNeighbor(ref G.Face1, ref G.Edge1ParamFrom, ref G.Edge1ParamTo, G.SortedParams, G.StartIndexInSortedList);
                    else
                        MoveToNeighbor(ref G.Face2, ref G.Edge2ParamFrom, ref G.Edge2ParamTo, G.SortedParams, G.StartIndexInSortedList);
                }
            }
        }
      
        static List<Face> Infected2 = new List<Face>();
        /// <summary>
        /// apply a <see cref="SetOperation"/> to two <see cref="DiscreteSolid"/>s.
        /// </summary>
        /// <param name="Solid1">first <see cref="DiscreteSolid"/></param>
        /// <param name="Solid2">second <see cref="DiscreteSolid"/></param>
        /// <param name="SetOperation">the <see cref="SetOperation"/>.</param>
        /// <param name="Traces">a list of <see cref="Line3D"/>, which are going along the cross lines.</param>
        /// <returns>list of <see cref="DiscreteSolid"/>.</returns>
        public static List<DiscreteSolid> GetCombinedSolids(DiscreteSolid Solid1, DiscreteSolid Solid2, SetOperation SetOperation, List<List<Line3D>> Traces)
        { 
            ListOfVertices.Clear();
            Solid1 = Solid1.Copy() as DiscreteSolid;
            Solid2 = Solid2.Copy() as DiscreteSolid;
        //// Zu TestZwecken   
            SolidA = Solid1;
            SolidB = Solid2;

            // initialize the debugtag
            for (int i = 0; i < Solid2.FaceList.Count; i++)
            {
                //Solid2.FaceList[i].DebugTag = 1;
                //Solid2.FaceList[i].DebugTag1 = i + 1;
            }
            for (int i = 0; i < Solid1.FaceList.Count; i++)
            {
                //Solid1.FaceList[i].DebugTag = 2;
                //Solid1.FaceList[i].DebugTag1 = -(i + 1);
            }
            List<List<IntersectResult>> IR = SetOperation3D.getTraces(Solid1, Solid2);
            if ((IR.Count == 0) &&((SetOperation == SetOperation.AandB)|| (SetOperation == SetOperation.AorB)))
                IR = SetOperation3D.getTraces(Solid2, Solid1);
            TraceRemoveEmpties(IR);
            SetParams(IR);
            if (SetOperation == SetOperation.AorB)
            CheckTrace(IR,2);
               
            if (SetOperation == SetOperation.AandB)
                CheckTrace(IR, 1);
           
            DoCompleteTraceItems(IR);
            if (Traces!= null)
            {
                Traces.Clear();
                for (int i = 0; i < IR.Count; i++)
                {

                    xyz Start = new xyz();
                    xyz End = new xyz();
                    List<Line3D> L = new List<Line3D>();
                    Traces.Add(L);
                    for (int j = 0; j < IR[i].Count; j++)
                    {

                        if (IR[i][j].Edge1ParamFrom >= 0) Start = Eval(IR[i][j].Face1, IR[i][j].Edge1ParamFrom);
                        if (IR[i][j].Edge2ParamFrom >= 0) Start = Eval(IR[i][j].Face2, IR[i][j].Edge2ParamFrom);

                        if (IR[i][j].Edge1ParamTo >= 0) End = Eval(IR[i][j].Face1, IR[i][j].Edge1ParamTo);
                        if (IR[i][j].Edge2ParamTo >= 0) End = Eval(IR[i][j].Face2, IR[i][j].Edge2ParamTo);
                        
                        L.Add(new Line3D(Start, End));
                    }

                }
            }
            Infected1.Clear();
            Infected2.Clear();
            GetFillets(IR, Infected1, Infected2);
            RepairFillets_From_To(Infected1);
            RepairFillets_From_To(Infected2);


            InvertFillets(Infected2);
      
            if ((SetOperation == SetOperation.AandB))
            {
                SetOperation3D.Complement(Solid2);// ?---------
               ComplementFillets(Infected2);
                SetOperation3D.Complement(Solid1);// ?---------
                ComplementFillets(Infected1);
            }
            if ((SetOperation == SetOperation.AnotB))
            {
                SetOperation3D.Complement(Solid2);// ?---------
                ComplementFillets(Infected2);
               
            }

            ClearTags(Solid1);
            ClearTags (Solid2);
            MakeNewBounds(Infected1);
           MakeNewBounds(Infected2);
            List<DiscreteSolid>  Solids = new List<DiscreteSolid>();
   
            for (int i = 0; i < IR.Count; i++)
            {
                DiscreteSolid S = new DiscreteSolid();
                if (IR[i].Count > 0)
                    getConnectedSolid(S, IR[i][0].Face1);
                if (S.VertexList.Count > 0)
                {
                    if ((SetOperation == SetOperation.AandB))
                    {
                        SetOperation3D.Complement(S);
                    }
                    Solids.Add(S);
                 
                }
           }
     
           
            return Solids;
        }
    }
}

    
