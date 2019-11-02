using System;
using System.Collections.Generic;
using System.Collections;
namespace Drawing3d
{
    /// <summary>
    /// contains the info about a cross from a <see cref="Face"/> F with another <b>Face</b>. This information is
    /// stored in a <see cref="Hashtable"/> on the <see cref="Face.Tag"/> of F. The <see cref="Hashtable.Keys"/> is the <b>Face</b>.
    /// </summary>
    [Serializable]
    public class CrossFaceListInfo
    {
        /// <summary>
        /// is the constructor of CrossFaceListInfo, which sets the list of <see cref="CrossItem"/> and the <see cref="Face"/>.
        /// </summary>
        /// <param name="Face">the face.</param>
        /// <param name="SortedParams">the by <see cref="CrossItem.Param2"/>sorted list of <see cref="CrossItem"/></param>
        internal CrossFaceListInfo(Face Face,List<CrossItem> SortedParams)
        {
            this.Face = Face;
            this.SortedParams = SortedParams;
        }
        /// <summary>
        /// is the othe <see cref="Face"/>.
        /// </summary>
         Face Face;
        /// <summary>
        /// is a <see cref="List{T}"/> of <see cref="CrossItem"/>. This list is sorted by the <see cref="CrossItem.Param2"/>, which belongs to the <see cref="Face"/>. This parameter belongs to <see cref="Face.Bounds"/>
        /// </summary>
        public List<CrossItem> SortedParams;
    }
    /// <summary>
    /// implements <see cref="CrossAllFaceList(Solid, Solid)"/> and <see cref="IntersectFaces(Face, Face)"/>.
    /// </summary>
    [Serializable]
    public class Cross
    {
         static double Tolerance = 0.000001;
         static double EdgeTolerance = Tolerance;
         static int ToSide = 1;
         static double Distance(xy Pt, xy LineA, xy LineB)
         {
             xy Direction = (LineB - LineA).normal().normalize();
             return (Pt - LineA) * Direction;

         }
         static int Prev(int i)
         {
             xy V1 = new xy(0, 0);
             int result = i - 1;
             do
             {

                 if (result < 0)
                     result = _Array1.Count - 2;
                 V1 = _Array1[result] - _Array1[i];
                 if (V1.length() < 0.001)
                     result--;
             } while (V1.dist(xy.Null) < 0.000001);
             return result;
         }
         static int Succ(int i)
         {
             xy V1 = new xy(0, 0);
             int result = i + 1;
             do
             {

                 if (result == _Array1.Count)
                     result = 1;
                 V1 = _Array1[result] - _Array1[i];
                 if (V1.length() < LuckyEpsilon)
                     result++;
             } while (V1.length() < LuckyEpsilon);
             return result;
         }
          static SetOperation3D.IntersectResult StartTrace(Solid Solid1, Solid Solid2)
         {
             SetOperation3D.IntersectResult R = null;

             for (int i = 0; i < Solid1.FaceList.Count; i++)
             {
                 for (int j = 0; j < Solid2.FaceList.Count; j++)
                 {    CrossFaceListInfo CI = null;
                     if (Solid1.FaceList[i].Tag != null)
                       CI = (Solid1.FaceList[i].Tag as Hashtable)[Solid2.FaceList[j]] as CrossFaceListInfo;
                   
 
                     if ((CI != null)  && 
                        (CI.SortedParams.Count >= 4) &&
                        (System.Math.Abs(CI.SortedParams[1].Param2 - CI.SortedParams[2].Param2) > 0.000002)
                       && (System.Math.Abs(CI.SortedParams[2].Param2 - CI.SortedParams[3].Param2) > 0.000002)
                      )
                     {
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

                        R.TraceInfo = new TraceInfo(1,R.SortedParams, R.Face1, R.Face2);
                         return R;
                     }
                 }
             }
             return null;
         }
         static bool TwoLines(xy A1, xy B1, xy A2, xy B2, ref double Lam, ref double Mue)
         {
             if (A1.dist(B1) < 0.0000001) return false;
             if (A2.dist(B2) < 0.0000001) return false;

             if ((A2.dist(B1) < Tolerance))
             {
                 Lam = 1 - LuckyEpsilon;
                 Mue = 0;
                 return true;
             }
             if ((A1.dist(B2) < Tolerance))
             {
                 Mue = 1 - LuckyEpsilon;
                 Lam = 0;
                 return true;
             }
             Lam = -1;
             Mue = -1;
             xy Direction1 = B1 - A1;
             xy Direction2 = B2 - A2;
             double d = Direction1 & Direction2;
             if (!(System.Math.Abs(d) < 0.000000001))
             {
                 Mue = System.Math.Round((Direction1 & (B1 - A2)) / d, 9);
                 Lam = System.Math.Round((Direction2 & (A1 - B2)) / d, 9);
                 return true;
             }
             return false;
         }
         static xyArray _Array1 = null;
        static CrossList CrossWithLine(xy A, xy B, xyArray Array,EdgeLoop E)
        {
           
            CrossList Result = new CrossList();
            double Lam = -1;
            double Mue = -1;
            _Array1 = Array;
            xy Last = Array[Array.Count - 1];

            for (int i = 0; i < Array.Count - 1; i++)
            {
                if (i < Array.Count - 1)
                    if (Array[i].dist(Array[i + 1]) < LuckyEpsilon) continue;
                int Nexti = Succ(i);

                xy Next = Array[Nexti];
                if (!(TwoLines(A, B, Array[i], Next, ref Lam, ref Mue))) continue;
                Lam = System.Math.Round(Lam, 8);
                Mue = System.Math.Round(Mue, 8);

                double D1 = Distance(Next, A, B);
                double D2 = Distance(Array[i], A, B);
                int Prv = Prev(i);
                double D3 = Distance(Array[Prv], A, B);

                if ((System.Math.Abs(D1) < EdgeTolerance)
                    && (System.Math.Abs(D2) < EdgeTolerance))
                {
                    Last = Array[i];
                    continue;
                }
                if ((System.Math.Abs(D1) >= EdgeTolerance)
                    && (System.Math.Abs(D2) < EdgeTolerance))
                {

                    if ((System.Math.Abs(D3) < EdgeTolerance))
                    {
                        CrossItem CI = new CrossItem(i + LuckyEpsilon, Lam, -1);// FromBorder
                        if (D1 > 0)
                            CI.CrossKind = 1;
                        else
                            CI.CrossKind = -1;
             
                        CI.Border1 = BorderBehavior.BorderEnd;
                        Result.Add(CI);
                        continue;
                    }


                }
                if ((System.Math.Abs(D1) < EdgeTolerance)
                 && (System.Math.Abs(D2) >= EdgeTolerance))
                {
                    int succ = Succ(Nexti);
                    double SuccDistance = System.Math.Abs(Distance(Array[succ], A, B));
                    if (SuccDistance <= EdgeTolerance)
                    {
                        // ToBorder
                        CrossItem CI = new CrossItem(i + (1 - LuckyEpsilon), Lam, -1);// ToBorder
                        CI.CrossKind = 1;
                        if (D2 < 0)
                            CI.CrossKind = 1;
                        else
                            CI.CrossKind = -1;
                        CI.Border1 = BorderBehavior.BorderBegin;
                        Result.Add(CI);
                        Last = Array[i];
                      
                    }

                    continue;

                }


                if ((System.Math.Abs(D2) < EdgeTolerance) && (D1 * D3 < 0))
                {
                    // Mue ==0 Durchgehend
                    int kk = ToSide;
                    if (D1 < 0) kk = -ToSide;

 
                    CrossItem CI = new CrossItem(i - Mue-0.000000001 , Lam, -1);
                    CI.CrossKind = 1;
                    if (D1 < 0)
                        CI.CrossKind = -1;

                    Result.Add(CI);
                    Last = Array[i];
                    continue;
                }
                else
                    //else // neu------------------------------------------------------------
                    if ((System.Math.Abs(D2) < EdgeTolerance) && (D1 * D3 > 0))
                    {

                        if ((Mue >= -LuckyEpsilon) && ((Mue < 1 - LuckyEpsilon)
                           && (Lam >= -LuckyEpsilon) && ((Lam <= 1 + LuckyEpsilon))))
                        {
                            xy v1 = Array[i] - Array[Prv];
                            xy v2 = Next - Array[i];
                            double kk = v1 & v2;
                            if (kk * D1 < 0)
                            {
                            }
                            //D1 = v1 & v2;
                            //D1 = -D1;


                            CrossItem CI = null;// new CrossItem(i + Mue, Lam, -1);// ToBorder
                            if (D1 > 0)
                                CI = new CrossItem(i + Mue - LuckyEpsilon, Lam + LuckyEpsilon, -1);// FromBorder
                            else
                                CI = new CrossItem(i + Mue - LuckyEpsilon, Lam - LuckyEpsilon, -1);// FromBorder

                            if (D1 < 0)
                                CI.CrossKind = 1;
                            else
                                CI.CrossKind = -1;
                     
                            CI.Border1 = BorderBehavior.BorderBegin;
                            Result.Add(CI);
                            if (D1 < 0)
                                CI = new CrossItem(i + Mue + LuckyEpsilon, Lam + LuckyEpsilon, -1);// FromBorder
                            else
                                CI = new CrossItem(i + Mue + LuckyEpsilon, Lam - LuckyEpsilon, -1);// FromBorder
                            if (D1 > 0)
                                CI.CrossKind = 1;
                            else
                                CI.CrossKind = -1;
                           
                            CI.Border1 = BorderBehavior.BorderEnd;
                            Result.Add(CI);
                            continue;
                        }
                    }
                if (D1 * D2 < 0)
                {
                    CrossItem CI = new CrossItem(i + Mue, Lam, -1);
                    CI.CrossKind = 1;
                    if (D1 < 0)
                        CI.CrossKind = -1;
                    Result.Add(CI);
                    Last = Array[i];
                    continue;
                }
            }

            return Result;
        }

    
       static CrossList CrossWithLine(xy A, xy B, CurveArray Array,EdgeLoop E)
       {

           xyArray xyArray = Array.getxyArray();
            if (xyArray[xyArray.Count - 1].dist(xyArray[0]) > 0.0001)   // 17.6 ausgeklammert?-----------
                xyArray.Add(xyArray[0]);
            CrossList CL = CrossWithLine(A, B, xyArray,E);
           for (int i = 0; i < CL.Count; i++)
           {
              Curve C= E[i].ParamCurve;

               CL[i].Param1 = Array.xyArrayIndexToCurveArrayIndex(CL[i].Param1);
               if (CL[i].Param1 == 4)
               {
               }
               int First = (int)CL[i].Param1;
               int Second = First + 1;
               if (Second >= Array.Count) Second = 0;
               CL[i].A = Array.Value(First);
               CL[i].B = Array.Value(Second);

           }


           return CL;

       }
       static bool Solid2Complemented = false;
       static bool Solid1Complemented = false;
  
       static void ToSortedParams(List<CrossItem> SortedParams, CrossItem CP)
       {


           int id = -1;
           for (int i = 0; i < SortedParams.Count; i++)
           {
              
               if (SortedParams[i].Param2 >= CP.Param2)
               {
                   id = i;
                   break;
               }
             
           }
           if (id < 0)
               SortedParams.Add(CP);
           else
               SortedParams.Insert(id, CP);

       }
      static Curve3D GetCrossCurve3D(Face Face1, Face Face2)
       {

           // ---------- Get CrosssCurve ---------------------------------
           Face F1 = Face1;
           Face F2 = Face2;
           double Dir = 1;
           if (Solid1Complemented) Dir = -1; else Dir = 1;
           
            Plane P1 = new Plane(F1.Surface.Value(0, 0), F1.Surface.Base.BaseZ * Dir);
            if (Solid2Complemented) Dir = -1; else Dir = 1;
            Plane P2 = new Plane(F2.Surface.Value(0, 0), F2.Surface.Base.BaseZ * Dir);

            LineType L = P1.Cross(P2);
           if ((P1.NormalUnit & P2.NormalUnit) == new xyz(0, 0, 0))
           {
               if ((P1.NormalUnit * P2.NormalUnit < 0))
                   if (System.Math.Abs((P2.P - P1.P) * P1.NormalUnit) < 0.00001)
                       L.P = P1.P;

           }

           if (L.Direction == new xyz(0, 0, 0)) return null;
           return new Line3D(L.P - L.Direction * 100, L.P + L.Direction * 100);
       }
     
        static List<CrossItem> CreateSortedParams(Face F1, Face F2)
       {
           //------ Erstelle Schnittgerade von F1 und F2---------------
           PlaneSurface S1 = F1.Surface as PlaneSurface;
           PlaneSurface S2 = F2.Surface as PlaneSurface;
           Curve3D Curve3D = null;
           Curve3D = GetCrossCurve3D(F1, F2); // Schnittgerade der Ebenen
           if (Curve3D == null)
               return null; // Parallel
           //-----------------------------------------------------------
          

           List<CrossItem> SortedParams = new List<CrossItem>();
           //---------- Schneide im Parameterraum Face1 mit Schnittgeraden
           Line L2D = S1.To2dCurve(Curve3D) as Line; // Die Schnittgerade wird auf die Ebene Face1 Projiziert
           CurveArray Ca2 = new CurveArray();
           Ca2.Add(L2D);                  // und in den zweiten CurveArray gestellt
          
            Loca Loca = F1.ParamCurves;    // die ParamCurves dieser Ebene werden in die Loca gestellt
           
           double GlobalIndex = 0;
           List<CrossItem> CrossList = new List<CrossItem>();
           for (int i = 0; i < Loca.Count; i++)
           {
               CurveArray Ca = Loca[i];
               
            EdgeLoop EL=   F1.Bounds[i];
               CrossList CL = CrossWithLine(L2D.A, L2D.B, Ca,EL);
               for (int g = 0; g < CL.Count; g++)
               {
                   if (CL[g].Param1 == EL.Count)
                       CL[g].Param1 = 0;
               }
               if (CL.Count == 0) { continue; }  // Offener Array oder kein schnittpunkt
               
               for (int k = 0; k < CL.Count; k++) // Schittpunke der geraden mit Face 1
               {
                   CL[k].Tag = 1;  
                    CL[k].Bound = i;
                   CL[k].Param1 += GlobalIndex;
 
                   CL[k].Intern = Ca;
                   CL[k].Face = F1;
                    if (!(
                        (CL[k].Border1 == BorderBehavior.BorderEnd) && (CL[k].CrossKind == -1)
                        ||
                        (CL[k].Border1 == BorderBehavior.BorderBegin) && (CL[k].CrossKind == 1)
                        ))

                        ToSortedParams(CrossList, CL[k]); // werden nach Param2 einsortiert

                }
             
               GlobalIndex += Loca[i].Count;
           }
          
           // Analog für Face 2
           // Analog für Face2
           //---------- Schneide um Parameterraum Face2 mit Schnittgeraden
           L2D = S2.To2dCurve(Curve3D) as Line;// Die Schnittgerade wird auf die Ebene Face2 Projiziert
           Ca2 = new CurveArray();
           Ca2.Add(L2D);                   // und in den zweiten CurveArray gestellt

           // die ParamCurves dieser Ebene werden in die Loca gestellt
           Loca = F2.ParamCurves;

           GlobalIndex = 0;

           for (int j = 0; j < Loca.Count; j++)
           {

               EdgeLoop EL = F2.Bounds[j];
               CurveArray Ca = Loca[j];
               CrossList CL = CrossWithLine(L2D.A, L2D.B, Ca,EL);
               for (int g = 0; g < CL.Count; g++)
               {
                   if (CL[g].Param1 == EL.Count)
                       CL[g].Param1 = 0;
               }
               if (CL.Count == 0) { continue; }  // Offener Array oder kein schnittpunkt
               for (int k = 0; k < CL.Count; k++) // Schittpunke der geraden mit Face 2
                {
                   CL[k].Tag = 2;
                   CL[k].Bound = j;
                   CL[k].Param1 += GlobalIndex;
                   CL[k].Intern = Ca;
                   CL[k].Face = F2;
                    if (!(
                     (CL[k].Border1 == BorderBehavior.BorderEnd) && (CL[k].CrossKind == -1)
                     ||
                     (CL[k].Border1 == BorderBehavior.BorderBegin) && (CL[k].CrossKind == 1)
                     ))
                        ToSortedParams(CrossList, CL[k]); // werden nach Param1 einsortiert
                    else
                    { }
                }

               GlobalIndex += Loca[j].Count;
           }
           if (CrossList.Count > 4)
           {
           }
           int Face1Status = 0;
           int Face2Status = 0;
           int id = 0;
           while (id < CrossList.Count)
           {

               if ((int)CrossList[id].Tag == 1)
                   Face1Status += CrossList[id].CrossKind;
               if ((int)CrossList[id].Tag == 2)
                   Face2Status += CrossList[id].CrossKind;

               //if (id < CrossList.Count-1)
               if ((Face2Status == 0) && (Face1Status == 0))
               {

                   if (id > 0)
                   {
                       if ((int)CrossList[id - 1].Tag == (int)CrossList[id].Tag)
                       {
                           if (id + 1 < CrossList.Count)
                               if (System.Math.Abs(CrossList[id + 1].Param2 - CrossList[id].Param2) > 0.000001)
                               {
                                   CrossList.RemoveAt(id - 1);
                                   CrossList.RemoveAt(id - 1);
                                   id--;
                               }
                               else
                                   id++;
                       }
                       else
                           id++;
                   }

               }
               else
                   id++;

           }

            return CrossList;


       }
       static CrossFaceListInfo GetCrossInfo(Face Face1, Face Face2)
       {
           if (Face1 == null) return null;
           if (Face2 == null) return null;
           if (Face1.Tag as Hashtable == null) return null;
           CrossFaceListInfo CI = ((Face1.Tag as Hashtable)[Face2] as CrossFaceListInfo);

           return CI;

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
           while (Param > Face.Bounds[Loop].Count) { Param -= Face.Bounds[Loop].Count; Loop++; }

       }
       //   public static int zFighting = 1; // or 2
       /// <summary>
       /// Berechnet aus einem Paar Bound und Param einen globalen Parameter.
       /// </summary>
       /// <param name="Face">Zugrunde liegendes Face</param>
       /// <param name="Loop">Der Index in Face.Bounds</param>
       /// <param name="Param">Der Parameter bezüglich Bounds[Loop]</param>
       /// <returns></returns>
       static double GetGlobalParam(Face Face, int Loop, double Param)
       {
           double Result = Param;
           for (int i = 0; i < Loop; i++)
               Result += Face.Bounds[i].Count;
           return Result;
       }
       static double DualEdge(Face Face, double GlobalParam, ref Face Neighbor)
       {
           int loop = -1;
           double LocalParam = -1;
           GetLocalParam(Face, GlobalParam, ref loop, ref LocalParam);
           int OutLoop = -1;
           if (LocalParam == -1) return -1;
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
        /// <summary>
        /// intersect two <see cref="Face"/>s and writes a <see cref="CrossFaceListInfo"/> in a Hashtable in <see cref="Face.Tag"/> of <b>Face1</b> with <b>key</b>= <b>Face2</b>.
        /// </summary>
        /// <param name="Face1">First <see cref="Face"/></param>
        /// <param name="Face2">Second <see cref="Face"/></param>
        /// <returns><see cref="CrossFaceListInfo"/> of the cross between <b>Face1</b> and <b>Face2</b>.</returns>
        public static CrossFaceListInfo IntersectFaces(Face Face1, Face Face2)
       {
         // in the Tag of Face1 wil be puted a Hashtable. who contains as Key Fac2 and content crossfaceListInfo
           if (Face1.Tag == null)
               Face1.Tag = new Hashtable();
           CrossFaceListInfo CI = new CrossFaceListInfo(Face2,null);
           if (!(Face1.Tag as Hashtable).Contains(Face2))
               (Face1.Tag as Hashtable).Add(Face2, CI);
           CI.SortedParams = CreateSortedParams(Face1, Face2);

            if (CI.SortedParams == null)
            {
                CI.SortedParams = new List<CrossItem>();
            }
           return CI;
       }
        internal static double LuckyEpsilon = 0.0000001;
        /// <summary>
        /// crosses two <see cref="Solid"/> by call <see cref="IntersectFaces(Face, Face)"/>.
        /// </summary>
        /// <param name="Solid1">first solid.</param>
        /// <param name="Solid2">first solid.</param>
        public static void CrossAllFaceList(Solid Solid1, Solid Solid2)
       {

           for (int i = 0; i < Solid1.FaceList.Count; i++)
           {
               Solid1.FaceList[i].Tag = null;
               for (int j = 0; j < Solid2.FaceList.Count; j++)
               {
                   IntersectFaces(Solid1.FaceList[i] as Face, Solid2.FaceList[j] as Face);

               }
           }
           ;
       }
    }
}
