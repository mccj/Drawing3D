using System;



namespace Drawing3d
{

    /// <summary>
    /// This class extrude a <see cref="xyArray"/> with a height. You can use it to
    /// represent any prism as a quader, a box ..
    /// </summary>
    [Serializable]
    public class ArrayExtruder : Surface
    {
        
        private xyArray _Array = new xyArray();
        /// <summary>
        /// gets or sets the height of the prism
        /// </summary>
        public double Height
        {
            get { return VFactor; }
            set {
                  Invalid = true;
                  VFactor = value; }

        }
        /// <summary>
        /// is <see cref="xyArray"/> which will be extruded with the height "<see cref="Height"/>.
        /// </summary>
        public xyArray Array
        {
            get { return _Array; }
            set { _Array = value;
                UResolution = Array.Count;
                VResolution = 1;
                Invalid = true;
                }

        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            ArrayExtruder AE = base.Copy() as ArrayExtruder;
            AE.Array = Array.copy();
            AE.Height = Height;
            return AE;
        }
        /// <summary>
        /// paramtisizes the a prism for the to parameters u and v which are given in the interval [0, 1]
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>prismcoordinate</returns>
        public override xyz Value(double u, double v)
        { 
            int Id = (int)(u * Array.Count);
            if (Id == Array.Count) Id--;
            double _ZHeight = ZHeight(u, v);
            xy N = new xy(0, 0);
            if (_ZHeight >0)
            { N = Normal(u * Array.Count) *_ZHeight; }
            
            
            return Base.Absolut((Array.Value(u * Array.Count)+N).toXYZ() + new xyz(0, 0, v * VFactor));
           
        }

        xy Normal(double param)
        {
            int ID = Utils.trunc(param);
            if (ID==1)
            { }
            if (Array.Count <2) return new xy(0,0);
            if (ID < 0) return (Array[1] - Array[0]).normal().normalize();
            if (ID >= Array.Count - 2) return (Array[Array.Count-1] - Array[Array.Count - 2]).normal().normalize();
            return (Array[ID+1] - Array[ID]).normal().normalize(); 

        }
        /// <summary>
        /// Calculates the partial <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>value of the partial uDerivation</returns>
        public override xyz uDerivation(double u, double v)
        {
            return Base.Absolut(Array.Direction(u * Array.Count).toXYZ()) - Base.BaseO;
        }
        /// <summary>
        /// Calculates the partial <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>value of the partial vDerivation</returns>
        public override xyz vDerivation(double u, double v)
        {
            return Base.Absolut(new xyz(0, 0, 1)) - Base.BaseO;
        }
    }
}