using System;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// restricted a <see cref="Face"/>. In case of <see cref="Drawing3d.Model.Solid"/> 
    /// it gives for two Points of the <see cref="Solid"/> A and B two <see cref="Edge"/>, one with <see cref="Edge.EdgeStart"/> = A and <see cref="Edge.EdgeEnd"/> = B, and an othe <see cref="Edge"/> with
    /// with <see cref="Edge.EdgeStart"/> = B and <see cref="Edge.EdgeEnd"/> = A. We say its dual to the other.
    /// <see cref="Edge.EdgeStart"/> and <see cref="Edge.EdgeEnd"/> are the start and the end point of the <see cref="Edge"/>. To every <see cref="Edge"/> is unique dedicated a <see cref="Face"/>.
    /// The right side determes the area of the solid, which belongs to the <see cref="Face"/>. The <see cref="Edge"/>s run arround the <see cref="Face"/> in the clockwise sense.
    /// The field <see cref="Edge.ParamCurve"/> is a two dimensional curve in the room of parameters of <see cref="Face.Surface"/>. You get the 3D-Points by <b>Face</b>.<see cref="Surface.Value(double, double)"/>. This curve determes the border of the <see cref="Face"/>
    /// Is e.g. the shell of a cylinder (see also <see cref="Cylinder"/>) and <see cref="Edge.ParamCurve"/>
    /// a line from (0,0) to (PI,0), so gives this a half circle in R³.<br/>
    ///   <br/>
    /// A <see cref="Edge.EdgeCurve"/> is a the 3-dimensional curve, which is identically with the by <see cref="Face.Surface"/> evaluated <see cref="Edge.ParamCurve"/>.
    /// An <see cref="Edge"/> and the dual <see cref="Edge"/> have only one <see cref="Edge.EdgeCurve"/>. The curves are identic.
    /// E.g.: a Box has 12 EdgeCurves. Siehe <see cref="Solid.EdgeCurveList"/>.
    ///   <br/>
    /// A 3D-Curve has the property <see cref="Curve3D.Neighbors"/> consisting of the both Neighbors.
    /// This property is important to get the neighbor of a <see cref="Face"/> at a bounding curve.
    /// If <see cref="Edge.SameSense"/> is <b>true</b> the <see cref="Edge.ParamCurve"/> gets a Curve with <see cref="Face.Surface"/>, which has the same sense as the <see cref="Edge.EdgeCurve"/>
    /// else she is inverted.
    /// </summary>

    [Serializable]
    public class Edge
    {
 
  
        static Curve3D GetEdgeCurve(Solid Solid, Vertex3d A, Vertex3d B)
        {
            Edge E = Solid.GetSurfaceEdge(A, B);
            if (E != null)
                return E.EdgeCurve;
            return null;
        }
        /// <summary>
        /// Frei zu verwendendes object.
        /// </summary>
        [NonSerialized]
        public object Tag;
        Vertex3d _EdgeStart = null;
        /// start vertex of an <see cref="Edge"/>.
        public Vertex3d EdgeStart
        {
            get
            {
                return _EdgeStart;
            }
            set
            {
                _EdgeStart = value;
            }

        }

 
        /// <summary>
        /// end vertex of an <see cref="Edge"/>.
        /// </summary>
        public Vertex3d EdgeEnd;

        /// <summary>
        /// A 3D-Curve. She has the property <see cref="Curve3D.Neighbors"/> consisting of the both Neighbors.
        /// </summary>
        public Curve3D EdgeCurve;
        int CurveId = -1;
        private bool _SameSense;
        /// <summary>
        /// is a two dimensional curve in the room of parameters for the <see cref="Face.Surface"/>, who gives -evaluated- the 3D Curve.
        /// </summary>
        [NonSerialized]
        public Curve ParamCurve;
        /// <summary>
        /// copy the the edge <b>TargetSolid</b> and adds the copy to the EdgeList of the targetSolid .
        /// </summary>
        /// <param name="TargetSolid">solid, in which a copied edge should be created.</param>
        /// <returns>copy of the edge.</returns>
        public virtual Edge Copy(Solid TargetSolid)
        {
            Edge Result = Activator.CreateInstance(this.GetType()) as Edge;
            Vertex3d VStart = null;
            Vertex3d VEnd = null;
            {
                VStart = new Vertex3d(EdgeStart.Value);
                EdgeStart.Tag = VStart;
                if (TargetSolid != null)
                    TargetSolid.VertexList.Add(VStart);
            }
            {
                VEnd = new Vertex3d(EdgeEnd.Value);
                EdgeEnd.Tag = VEnd;
                if (TargetSolid != null)
                    TargetSolid.VertexList.Add(VEnd);
            }

            Result.EdgeStart = VStart;
            Result.EdgeEnd = VEnd;

            Curve3D ECurve = null;
            if (TargetSolid != null)
            {
                ECurve = GetEdgeCurve(TargetSolid, VStart, VEnd);
                if (ECurve == null)
                    ECurve = GetEdgeCurve(TargetSolid, VEnd, VStart);
            }
            if (ECurve == null)
            {
                ECurve = EdgeCurve.Copy();
                if (TargetSolid != null)
                    TargetSolid.EdgeCurveList.Add(ECurve);
            }
            Result.EdgeCurve = ECurve;
            Result.ParamCurve = ParamCurve.Clone();
            Result.CurveId = CurveId;
            Result.SameSense = SameSense;
            if (TargetSolid != null)
            {
                TargetSolid.EdgeList.Add(Result);
            }
            return Result;
        }
      
        /// <summary>
        /// static method, who generates a edge to a Line in the <see cref="Drawing3d.Model.Shell"/> model.
        /// </summary>
        /// <param name="A">start point.</param>
        /// <param name="B">end point.</param>
        /// <returns>Sell Modell Gerade</returns>
        public static Edge ShellLineEdge(xyz A, xyz B)
        {
            return ShellEdge(new Line3D(A, B));

        }
        /// <summary>
        /// static method, who generates a edge an arbitrary <see cref="Curve3D"/> in the <see cref="Drawing3d.Model.Shell"/> model.
        /// </summary>
        /// <param name="Curve">a <see cref="Curve3D"/></param>
        /// <returns>an edge.</returns>
        public static Edge ShellEdge(Curve3D Curve)
        {
            Edge Result = new Edge();
            Result.EdgeStart = new Vertex3d(Curve.A);
            Result.EdgeEnd = new Vertex3d(Curve.B);
            Result.EdgeCurve = Curve;

            Result.SameSense = true;
            return Result;

        }
        /// <summary>
        /// generates in <see cref="Curve3D"/> in the <see cref="Drawing3d.Model.Solid"/> model an edge. 
        /// It needs the belonging <b>Face</b>, the start point, the end point and the 3D-curve.
        /// </summary>
        /// <param name="Solid">solid, in which the edge should be generated.</param>
        /// <param name="Face"><see cref="Face"/>, in which the edge should be generated.</param>
        /// <param name="A">start point.</param>
        /// <param name="B">end point.</param>
        /// <param name="Curve">a 3D curve</param>
        /// <returns>an edge in the <see cref="Drawing3d.Model.Solid"/> model.</returns>
        public static Edge SolidEdge(Solid Solid, Face Face, Vertex3d A, Vertex3d B, Curve3D Curve)
        {
           bool Orientation = true;
            Curve3D SC = GetEdgeCurve(Solid, A, B);
            if (SC == null)
            {
                SC = GetEdgeCurve(Solid, B, A);
                if (SC != null)
                {
                    Orientation = false;
                    SC.Neighbors[1] = Face;
                }
            }


            if (SC == null)
            {
                Curve.Neighbors = new Face[2];
                Curve.Neighbors[0] = Face;
                if (Solid != null)
                    Solid.EdgeCurveList.Add(Curve);
                SC = Curve;
            }

            Edge Result = new Edge();
            Result.EdgeCurve = SC;
            Result.EdgeStart = A;
            Result.EdgeEnd = B;
            Result.SameSense = Orientation;
            if (Solid != null)
                Solid.EdgeList.Add(Result);


            Result.ParamCurve = Face.Surface.To2dCurve(Curve);
            return Result;

        }
        /// <summary>
        /// generates an edge in <see cref="Drawing3d.Model.Solid"/> model. The belonging <b>Face</b>,
        /// the start point, the end point and with the difference to <see cref="SolidEdge(Solid,Face, Vertex3d, Vertex3d,Curve3D)"/> 
        /// the the paramcurve on the <b>Face</b> must be specified.
        /// </summary>
        /// <param name="Solid"><see cref="Solid"/>, in which the <see cref="Edge"/> should be generated.</param>
        /// <param name="Face">the <see cref="Face"/> for the <see cref="Edge"/>.</param>
        /// <param name="A">start point</param>
        /// <param name="B">end point</param>
        /// <param name="ParamCurve">paramcurve on the <b>Face</b></param>
        /// <returns>a <b>edge</b>in the <see cref="Drawing3d.Model.Solid"/> model.</returns>
        public static Edge SolidEdge(Solid Solid, Face Face, Vertex3d A, Vertex3d B, Curve ParamCurve)
        {

            Curve3D Curve3d = GetEdgeCurve(Solid, B, A);

            Edge Result = new Edge();

            if (Curve3d == null)
            {
                Result.SameSense = true;
                Curve3d = Face.Surface.To3dCurve(ParamCurve);
                Result.ParamCurve = ParamCurve;
                Result.EdgeStart = A;
                Result.EdgeEnd = B;

            }
            else
            {
                Result.EdgeStart = B;
                Result.EdgeEnd = A;
                ParamCurve.Invert();
                Result.ParamCurve = ParamCurve;
                Result.SameSense = false;
            }
            Result.EdgeCurve = Curve3d; // muss vor Edges.add stehen weil es dort in die sufacelist aufgenommen wird
            Solid.EdgeList.Add(Result);
            return Result;
        }
        /// <summary>
        /// calls <see cref="SolidEdge(Solid,Face, Vertex3d, Vertex3d,Curve3D)"/> and returns a <see cref="Line3D"/>
        /// Kurve.
        /// </summary>
        /// <param name="Solid"><see cref="Solid"/>, in which the <see cref="Edge"/> should be created.</param>
        /// <param name="Face"><see cref="Face"/>, in which the <see cref="Edge"/> should be created.</param>
        /// <param name="A">start point.</param>
        /// <param name="B">end point.</param>
        /// <returns>an <see cref="Edge"/>.</returns>
        public static Edge SolidEdgeLine(Solid Solid, Face Face, Vertex3d A, Vertex3d B)
        {
           return SolidEdge(Solid, Face, A, B, new Line3D(A.Value, B.Value));
        }
       /// <summary>
        /// is <b>true</b> if the <see cref="Edge.ParamCurve"/> gets a Curve by <see cref="Face.Surface"/>, which has the same sense as the <see cref="Edge.EdgeCurve"/>
        /// </summary>
        public bool SameSense
        {
            get { return _SameSense; }
            set
            {
               _SameSense = value;
            }
        }
    }
}