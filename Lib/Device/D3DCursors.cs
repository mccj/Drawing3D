using System;

using System.Windows.Forms;
using System.IO;

namespace Drawing3d
{
	/// <summary>
	/// Summary for cursors.
	/// </summary>
	[Serializable]
	public class D3DCursors
	{

		/// <summary>
		/// A cursor
		/// </summary>
        public static Cursor C200;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C201;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C1001;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C1002;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C1006;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C1007;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2033;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2070;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2071;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2072;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2073;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2078;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2079;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2080;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2081;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2082;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2083;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2084;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2085;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C2086;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C5001;
		/// <summary>
		/// A cursor <img src="5002.gif"/>
		/// </summary>
		public static Cursor C5002;
		/// <summary>
		/// A cursor 
		/// </summary>
		public static Cursor C5003;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C5004;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C5005;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C5006;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C5007;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor C379;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor arrow_i;
		/// <summary>
		/// A cursor
		/// </summary>
		public static Cursor arrow_r;
        /// <summary>
        /// an empty cursor.
        /// </summary>
        public static Cursor Empty;
        /// <summary>
        /// Initializes all cursor by loading from the stream.
        /// </summary>
        public D3DCursors()
		{
		
		if (C1001!= null) return;
         
        System.Reflection.AssemblyName n = this.GetType().Assembly.GetName();
        string Name = n.Name;
     
         C200 =  new Cursor(GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.200.cur"));
        C201 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.201.cur"));
        C379 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.379.cur"));
        C1001 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.1001.cur"));
        C1002 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.1002.cur"));
        C1006 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.1006.cur"));
        C1007 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.1007.cur"));
        C2033 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2033.cur"));
        C2070 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2070.cur"));
        C2071 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2071.cur"));
        C2072 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2072.cur"));
        C2073 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2073.cur"));
        C2078 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2078.cur"));
        C2079 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2079.cur"));
        C2080 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2080.cur"));
        C2081 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2081.cur"));
        C2082 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2082.cur"));
        C2083 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2083.cur"));					//
        C2084 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2084.cur"));					//
        C2085 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2085.cur"));					//
        C2086 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.2086.cur"));					//
        C5001 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5001.cur"));					//
        C5002 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5002.cur"));					//
        C5003 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5003.cur"));					//
        C5004 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5004.cur"));					//
        C5005 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5005.cur"));					//
        C5006 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5006.cur"));					//
        C5007 = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.5007.cur"));					//			
        arrow_i= new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.arrow_i.cur"));                    //			
            arrow_r= new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.arrow_r.cur"));                    //			
            Empty = new Cursor(this.GetType().Assembly.GetManifestResourceStream(Name + "." + "Cursors.Empty.cur"));
            return;

       
			//
		}
	}

}
