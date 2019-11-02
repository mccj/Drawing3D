using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using System.Drawing;
  
namespace Drawing3d
{
   public class AssimpConv
    {
        /// <summary>
        /// internal.
        /// </summary>
        public static Color ConvertColor(Color4D Color4d)
        {

            return Color.FromArgb((int)(System.Math.Min(Color4d.A, 1) * 255), (int)(System.Math.Min(Color4d.R, 1) * 255), (int)(System.Math.Min(Color4d.G, 1) * 255), (int)(System.Math.Min(Color4d.B, 1) * 255));

        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static Drawing3d.Material ConvertMaterial(Assimp.Material AMAterial)
        {
           
            return new Drawing3d.Material(AMAterial.Name, ConvertColor(AMAterial.ColorAmbient), ConvertColor(AMAterial.ColorDiffuse), ConvertColor(AMAterial.ColorSpecular), ConvertColor(AMAterial.ColorEmissive), AMAterial.Shininess, AMAterial.Opacity);

        }
        /// <summary>
        /// internal.
        /// </summary>
        internal static Matrix ConvertTransform(Assimp.Matrix4x4 mat)
        {
            Matrix Result = new Matrix();
            Result.a00 = mat.A1;
            Result.a01 = mat.A2;
            Result.a02 = mat.A3;
            Result.a03 = mat.A4;
            Result.a10 = mat.B1;
            Result.a11 = mat.B2;
            Result.a12 = mat.B3;
            Result.a13 = mat.B4;
            Result.a20 = mat.C1;
            Result.a21 = mat.C2;
            Result.a22 = mat.C3;
            Result.a23 = mat.C4;
            Result.a30 = mat.D1;
            Result.a31 = mat.D2;
            Result.a32 = mat.D3;
            Result.a33 = mat.D4;
            return Result;
        }
      static Matrix4x4 ConvertTransformToAssimp1(Matrix D3DMatrix)
      {
          Matrix4x4 mat = new Matrix4x4();
            mat.A1=D3DMatrix.a00;
            mat.A2=D3DMatrix.a01;
            mat.A3=D3DMatrix.a02;
            mat.A4=D3DMatrix.a03;
            mat.B1=D3DMatrix.a10;
            mat.B2=D3DMatrix.a11;
            mat.B3=D3DMatrix.a12;
            mat.B4=D3DMatrix.a13;
            mat.C1=D3DMatrix.a20;
            mat.C2=D3DMatrix.a21;
            mat.C3=D3DMatrix.a22;
            mat.C4=D3DMatrix.a23;
            mat.D1=D3DMatrix.a30;
            mat.D2=D3DMatrix.a31;
            mat.D3=D3DMatrix.a32;
            mat.D4 = D3DMatrix.a33;
          return mat;
      }
       
       
    }
}
