using System;
using System.Collections.Generic;
using System.Text;


    namespace Drawing3d
    {
        public partial class Selector
        {
            /// <summary>
            /// is the vertex of the <see cref="PickingShader"/>
            /// </summary>
            public static string PickingVertexShader = @"
// shader picking
attribute vec3 Position;
varying vec4 Posi;
uniform mat4 ModelMatrix;
uniform mat4 ProjectionMatrix;

varying float pos;// out
void main()
{   mat4 M;
    M[0] = vec4(1,0,0,0);
    M[1] = vec4(0,1,0,0);
    M[2] = vec4(0,0,1,0);
    M[3] = vec4(0,0,0,1);
 mat4 P;
    P[0] = vec4(1,0,0,0);
    P[1] = vec4(0,1,0,0);
    P[2] = vec4(0,0,1,0);
    P[3] = vec4(0,0,0,1);
    Posi=  ProjectionMatrix *  ModelMatrix * vec4(Position, 1.0); 
    gl_Position =  Posi; 
 //gl_Position =  P *  M * vec4(Position, 1.0); 
    pos = gl_Position.z/gl_Position.w;
} 
";
        /// <summary>
        /// is the fragment if the <see cref="PickingShader"/>.
        /// </summary>
            public static string PickingFragmentShader = @"
// Picking shader
varying float pos; // in
varying vec4 Posi;
uniform int TheObject;
uniform int ClippingPlaneEnabled[6];
uniform float ClippingPlane[6*4];  // 6 Planes zu je 4 float
vec4 pack (float depth)
{
    const vec4 bitSh = vec4(256.0 * 256.0 * 256.0,
                            256.0 * 256.0,
                            256.0,
                            1.0);
    const vec4 bitMsk = vec4(0,
                             1.0 / 256.0,
                             1.0 / 256.0,
                             1.0 / 256.0);
     vec4 comp = fract(depth * bitSh);
    comp -= comp.xxyz * bitMsk;
    return comp;
}
vec4 Packint(int depth)
{

int d = depth;
int A=d/(256*256*256);
 d = d - A * (256*256*256);
int B=d/(256*256);
 d = d - B * (256*256); 
int C = d/256;
int D = d - C * 256; 
return vec4(float(A)/255.0,float(B)/255.0,float(C)/255.0,float(D)/255.0);   



}

//precision highp float;
void main()
{
//for (int i=0;i<6;i++)
//{
//if (ClippingPlaneEnabled[i]== 1)
//   {if (dot(vec3(1,0,0),vec3(Posi)) ClippingPlane[4*i+3])
//    {
     
//     discard;
//      return;
//    }
     
//   } 

//}
gl_FragColor=Packint(TheObject);



} ";
        }
    }
