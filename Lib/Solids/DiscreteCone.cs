using System;
using System.Collections.Generic;

using System.Drawing;
namespace Drawing3d
{
    /// <summary>
    /// a <b>DiscreteCone </b> is nearly the same as a <see cref="Cone"/> with the difference thate the shell consists of planes corresponding to the <see cref="Surface.UResolution"/> and the <see cref="Surface.VResolution"/>.
    /// </summary>
    [Serializable]
          public class DiscreteCone : DiscreteSolid
          {
        /// <summary>
        /// constructor of a <see cref="DiscreteCone"/> with <see cref="Radius"/>, <see cref="Height"/>, <see cref="HalfAngle"/> and  <see cref="UResolution"/>.
        /// </summary>
        /// <param name="Radius">the <see cref="Radius"/>.</param>
        /// <param name="Height">the <see cref="Height"/>.</param>
        /// <param name="HalfAngle">the <see cref="HalfAngle"/>.</param>
        /// <param name="Resolution">the <see cref="UResolution"/></param>
        public DiscreteCone(double Radius, double Height, double HalfAngle, int Resolution):base()
              {
                  this.Radius = Radius;
                  this.Height = Height;
                  this.HalfAngle = HalfAngle;
                  UResolution = Resolution;
                  VResolution = 1;
            Refresh();
           
              }
        /// <summary>
        /// constructor of a <see cref="DiscreteCone"/> with <see cref="Radius"/>, <see cref="Height"/>, <see cref="HalfAngle"/>. The rsolution is set to 20.
        /// </summary>
        /// <param name="Radius">the <see cref="Radius"/>.</param>
        /// <param name="Height">the <see cref="Height"/>.</param>
        /// <param name="HalfAngle">the <see cref="HalfAngle"/>.</param>
      
        public DiscreteCone(double Radius, double Height, double HalfAngle):this(Radius,Height,HalfAngle,20)
              {
                  
              }
              private double _radius=1;
              /// <summary>
              /// radius of the base circle.
              /// </summary>
              public double Radius
              {
                  get { return _radius; }
                  set { _radius = value; }
              }

              private double _Height;
              /// <summary>
              /// height of the cone.
              /// </summary>
              public double Height
              {
                  get { return _Height; }
                  set { _Height = value; }
              }
              private double _HalfAngle;
              /// <summary>
              /// the half angle on top.
              /// </summary>
              public double HalfAngle
              {
                  get { return _HalfAngle; }
                  set { _HalfAngle = value; }
              }
              int _UResolution = 20;
              /// <summary>
              /// the resolution for the shall. The default is 20.
              /// </summary>
              public int UResolution
              { get { return _UResolution; } set { _UResolution = value; } }
              int _VResolution = 2;
             /// <summary>
             /// the resolution parallel to the base. Default = 2;
             /// </summary>
             public int VResolution
              { get { return _VResolution; } set { _VResolution = value; } }
              /// <summary>
              /// empty constructor.
              /// </summary>
              public DiscreteCone():base()
              {
            
              }
              /// <summary>
              /// overrides <see cref="Solid.Refresh()"/>.
              /// </summary>
              public override void Refresh()
              {
                  Clear();
                  Cone ConeSurface = new Cone();
                  ConeSurface.VResolution = VResolution;
                  ConeSurface.UResolution = UResolution;
                  
                
                  ConeSurface.Radius = Radius;
                  ConeSurface.HalfAngle = HalfAngle;
                  ConeSurface.Height = Height;
                  Vertex3d[,] Points = new Vertex3d[ConeSurface.UResolution, ConeSurface.VResolution + 1];
                  Line3D[,] HorzCurves = new Line3D[ConeSurface.UResolution, ConeSurface.VResolution + 1];
                  Line3D[,] VertCurves = new Line3D[ConeSurface.UResolution, ConeSurface.VResolution];
                  Vertex3d[,] Normals = new Vertex3d[ConeSurface.UResolution + 1, ConeSurface.VResolution + 1];

            for (int i = 0; i < ConeSurface.UResolution; i++)
                      for (int j = 0; j <= ConeSurface.VResolution; j++)
                      {
                          Points[i, j] = new Vertex3d(ConeSurface.Value((float)i / (float)ConeSurface.UResolution, (float)j / (float)ConeSurface.VResolution));
                          VertexList.Add(Points[i, j]);
                          Normals[i, j] = new Vertex3d(ConeSurface.Normal((float)i / (float)ConeSurface.UResolution, (float)j / (float)ConeSurface.VResolution));

                     }
            for (int i = 0; i < ConeSurface.UResolution; i++)
                      for (int j = 0; j <= ConeSurface.VResolution; j++)
                      {
                          if (i < ConeSurface.UResolution)
                          {
                              if (i + 1 < ConeSurface.UResolution)
                                  HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[i + 1, j].Value);
                              else
                                  HorzCurves[i, j] = new Line3D(Points[i, j].Value, Points[0, j].Value);
                              HorzCurves[i, j].Neighbors = new Face[2];
                              EdgeCurveList.Add(HorzCurves[i, j]);
                          }
                          if (j < ConeSurface.VResolution)
                          {
                              VertCurves[i, j] = new Line3D(Points[i, j].Value, Points[i, j + 1].Value);
                              VertCurves[i, j].Neighbors = new Face[2];
                              EdgeCurveList.Add(VertCurves[i, j]);
                          }
                      }
  
                  for (int i = 0; i < ConeSurface.UResolution; i++)
                  {
                      for (int j = 0; j < ConeSurface.VResolution; j++)
                      {
                    int IInd = -1;
                          Vertex3d A = Points[i, j];
                          Vertex3d B = Points[i, j + 1];

                          Vertex3d C = null;
                    if (i + 1 < ConeSurface.UResolution)
                    {
                        IInd = i + 1;
                        C = Points[i + 1, j + 1];
                    }
                    else
                    {
                        IInd = 0;
                        C = Points[0, j + 1];
                    }
                          Vertex3d D = null;
                          if (i + 1 < ConeSurface.UResolution)
                              D = Points[i + 1, j];
                          else
                              D = Points[0, j];


                          Face F = new Face();
                          FaceList.Add(F);
                        
                    F.Surface = new SmoothPlane(Points[i, j].Value, Points[IInd, j].Value, Points[i, j + 1].Value, Points[IInd, j + 1].Value,
                               Normals[i, j].Value, Normals[IInd, j + 1].Value, Normals[i, j + 1].Value, Normals[IInd, j].Value);
                  
                          EdgeLoop EL = new EdgeLoop();
                          F.Bounds.Add(EL);

                          Edge E = new Edge();
                          EdgeList.Add(E);
                          EL.Add(E);
                          E.EdgeStart = A;
                          E.EdgeEnd = B;
                          E.EdgeCurve = VertCurves[i, j];

                          E.EdgeCurve.Neighbors[0] = F;
                          E.SameSense = true;
                          E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);

                          E = new Edge();
                          EL.Add(E);
                          EdgeList.Add(E);
                          E.EdgeStart = B;
                          E.EdgeEnd = C;
                          E.EdgeCurve = HorzCurves[i, j + 1];
                          E.EdgeCurve.Neighbors[0] = F;

                          E.SameSense = true;
                          E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                          E = new Edge();
                          EL.Add(E);
                          EdgeList.Add(E);
                          E.EdgeStart = C;
                          E.EdgeEnd = D;
                          if (i + 1 < ConeSurface.UResolution)
                              E.EdgeCurve = VertCurves[i + 1, j];
                          else
                              E.EdgeCurve = VertCurves[0, j];
                          E.EdgeCurve.Neighbors[1] = F;
                          E.SameSense = false;
                          E.ParamCurve = F.Surface.To2dCurve(E.EdgeCurve);
                          E.ParamCurve.Invert();

                          E = new Edge();
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

                  // Boden
                  Base BodenBase = Base.From4Points(Points[0, 0].Value, Points[2, 0].Value, Points[1, 0].Value, Points[3, 0].Value);
                //  Base BodenBase = Base.UnitBase;
                  BodenBase.BaseO = Points[0, 0].Value;
                  {
                      Face F = new Face();
                      FaceList.Add(F);
                      PlaneSurface S = null;
                      S = new PlaneSurface();
                      S.Base = BodenBase;
                      F.Surface = S;

                      EdgeLoop EL = new EdgeLoop();
                      F.Bounds.Add(EL);
                      for (int i =0; i < ConeSurface.UResolution; i++)
                      {

                          Edge E = new Edge();
                          EL.Add(E);
                          EdgeList.Add(E);
                          E.EdgeStart = Points[i, 0];
                          if (i + 1 < ConeSurface.UResolution)
                              E.EdgeEnd = Points[i + 1, 0];
                          else
                              E.EdgeEnd = Points[0, 0];
                          E.EdgeCurve = HorzCurves[i, 0];
                          E.EdgeCurve.Neighbors[0] = F;
                          E.SameSense = true;
                          E.ParamCurve = S.To2dCurve(E.EdgeCurve);

                      }
                  }
                  // Deckel
                  {
                    

                      
                      PlaneSurface S = null;
                      S = new PlaneSurface();
                      BodenBase.BaseO = Points[0, ConeSurface.VResolution].Value;
                      //S.Base = BodenBase;
                        try
                      {
                      S.Base = Base.From4Points(Points[0, ConeSurface.VResolution].Value, Points[2, ConeSurface.VResolution].Value, Points[3, ConeSurface.VResolution].Value, Points[1, ConeSurface.VResolution].Value);
                      }
                      catch (Exception)
                      {
                          Base B = Base.UnitBase;
                          B.BaseO = Points[0, ConeSurface.VResolution].Value;
                          S.Base = B;
                         

                      }
                      Face F = new Face();
                      FaceList.Add(F);
                      F.Surface = S;

                      EdgeLoop EL = new EdgeLoop();
                      F.Bounds.Add(EL);
                      for (int i = ConeSurface.UResolution - 1; i >= 0; i--)
                       //   for (int i = 0; i < ConeSurface.UResolution; i++)
                         
                      {

                          Edge E = new Edge();
                          EL.Add(E);
                          EdgeList.Add(E);
                          E.EdgeStart = Points[i, ConeSurface.VResolution];
                          //if (i + 1 < ConeSurface.UResolution)
                          //    E.EdgeEnd = Points[i + 1, ConeSurface.VResolution];
                          //else
                          //    E.EdgeEnd = Points[0, ConeSurface.VResolution];
                          E.EdgeEnd = Points[i, ConeSurface.VResolution];
                          if (i + 1 < ConeSurface.UResolution)
                              E.EdgeStart = Points[i + 1, ConeSurface.VResolution];
                          else
                              E.EdgeStart = Points[0, ConeSurface.VResolution];
                          E.EdgeCurve = HorzCurves[i, ConeSurface.VResolution];
                          if (E.EdgeStart.Value.dist(E.EdgeCurve.B) > 0.001)
                          {
                          }
                          E.EdgeCurve.Neighbors[1] = F;
                          E.SameSense = false;
                          E.ParamCurve = S.To2dCurve(E.EdgeCurve);
                          E.ParamCurve.Invert();
                      }
                    
                  }
                  RefreshParamCurves();
                
                 base.Refresh();
              }


          }
   
             
}
