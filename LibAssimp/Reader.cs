using System;

using Assimp;


namespace Drawing3d
{
     /// <summary>
     /// reads a lot of graphic files by using the great <b>assimp</b>. <see cref="GetSupportedImportFormats"/>.
     /// </summary>
    public class Reader
    {
        public static string[] GetSupportedImportFormats()
        {
            Assimp.AssimpContext C = new AssimpContext();
            string[] _Formats = C.GetSupportedImportFormats();
            C.Dispose();
            return _Formats;
        }
    

        /// <summary>
        /// loads a graphic file with the name.
        /// </summary>
        /// <param name="FileName">the pathname of the file.</param>
        /// <returns>the graphic file.</returns>
        public static Scene FromFile(string FileName)
       {
         AssimpContext C = new AssimpContext();
           
        string[] _Formats =   C.GetSupportedImportFormats();
        Assimp.Scene SC = null;
          
        try
        {
               
            SC = C.ImportFile(FileName);
                System.IO.FileStream FS = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
      
        }
        catch (Exception E)
        {

                System.Windows.Forms.MessageBox.Show(E.Message);
                return null;
        }
          
           ConvertAssimp.BaseDir = System.IO.Path.GetDirectoryName(FileName);
           return ConvertAssimp.ConvertFromAssimp(SC);
       }
    }
}
