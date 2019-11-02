using System;
using System.Collections;



namespace Drawing3d
{
    /// <summary>
    /// defines a list of <see cref="Face"/>s.
    /// </summary>
    [Serializable]
    public class FaceList : ArrayList
    {
        [NonSerialized]
        internal Solid Parent;
 
        /// <summary>
        /// redefines the default indexer and returns <see cref="Face"/>
        /// </summary>
        public new Face this[int i]   // Indexer declaration
        {
            get {
                return base[i] as Face; }
            set { base[i] = value; }
        }
      
    }

}