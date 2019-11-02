
using System;
using System.Collections.Generic;

using System.Text;


namespace Drawing3d
{
    public partial class OpenGlDevice
    {   
    /// <summary>
    /// is the fragment if the <see cref="SmallShader"/>
    /// </summary>
        public static string SmallFragment =
@"
// shader small
#version 410 core
uniform int ShadowFrameBufferSize;
uniform vec4 Light0Position;

uniform float ShadowIntensity;
//--------------------------- Normal mapping

uniform vec3 cameraPosition;
uniform bool NormalMap= false;
//---------------------------------------------------
// --------------- tesselation-----------------------
uniform bool Tesselation= false;
//---------------------------------------------------
//in vec4 ShadowCoord;

uniform sampler2D ShadowMap;
// Clipping
uniform int ClippingPlaneEnabled[6];
uniform float ClippingPlane[6*4];  // 6 Planes zu je 4 float
// normalMap
  mat3 cotangent_frame(vec3 N, vec3 p, vec2 uv)
        {
            /* get edge vectors of the pixel triangle */
            vec3 dp1 = dFdx(p);
            vec3 dp2 = dFdy(p);
            vec2 duv1 = dFdx(uv);
            vec2 duv2 = dFdy(uv);

            /* solve the linear system */
            vec3 dp2perp = cross(dp2, N);
            vec3 dp1perp = cross(N, dp1);
            vec3 T = dp2perp * duv1.x + dp1perp * duv2.x;
            vec3 B = dp2perp * duv1.y + dp1perp * duv2.y;

            /* construct a scale-invariant frame */
            float invmax = inversesqrt(max(dot(T, T), dot(B, B)));
            return mat3(T * invmax, B * invmax, N);
        }
uniform sampler2D Texture0_;
uniform sampler2D Texture1_;
        vec3 perturb_normal(vec3 N, vec3 V, vec2 texcoord)
        {
            /* assume N, the interpolated vertex normal and V, the view vector (vertex to eye) */
            vec3 map = texture2D(Texture1_, texcoord).xyz;
            // WITH_NORMALMAP_UNSIGNED
            map = map * 255./ 127. - 128./ 127.;
            // WITH_NORMALMAP_2CHANNEL
            // map.z = sqrt( 1. - dot( map.xy, map.xy ) );
            // WITH_NORMALMAP_GREEN_UP
            // map.y = -map.y;
            mat3 TBN = cotangent_frame(N, -V, texcoord);

            return normalize(TBN * map);
        }
float Visible(vec4 ShadowCoord)
{

 if (vec3(texture2D(ShadowMap,ShadowCoord.xy)).z >=ShadowCoord.z)
 return 1.0;
 return 0.0;
}

float shadowVisibility(sampler2D shadowMap,vec4 ShadowCoord)
{
float visibility = 1.0;
visibility = Visible(ShadowCoord);
visibility = 1.0-(1.0-visibility)*ShadowIntensity;
return  min(visibility, 1.0);

}

uniform bool Texture1Enabled;
uniform mat4 Texture1Matrix;
uniform bool Texture0Enabled;
uniform mat4 Texture0Matrix;
uniform bool ShadowEnable;
uniform float Darkness;
uniform vec4 Ambient;
uniform float Shininess;
uniform vec4  Emission;
uniform vec4  Specular;
uniform vec4  Diffuse;
uniform float Translucent; 
uniform int NormalsEnabled;
uniform vec4 Light0Ambient;

uniform int LightCount;
uniform vec4 Light0Diffuse;
uniform vec4 Light0Specular; 
uniform int Light0Enabled;
;
uniform int ColorEnabled;
uniform mat4 FromLight;
in vec4 outColor;
in vec4 WorldNormal;
in vec2 FS_Texture_in;
in vec4 WorldPosition;
out vec4 FragColor;
void main(void)
{

for (int i=0;i<6;i++)
{
if (ClippingPlaneEnabled[i]== 1)
   {
     if (dot(vec3(ClippingPlane[4*i],ClippingPlane[4*i+1],ClippingPlane[4*i+2]),vec3(WorldPosition)) <ClippingPlane[4*i+3]) 
    {
     discard;
      return;
    }
   } 
}
vec3   n=normalize(vec3(WorldNormal));
 if ((NormalMap) && (Texture1Enabled))
  n = perturb_normal(vec3(WorldNormal), normalize(cameraPosition - WorldPosition.xyz), FS_Texture_in.st);
//vec3 lightDirection= vec3(Light0Position) - vec3(WorldPosition);




const mat4 Bias = mat4(     

        0.5, 0.0, 0.0, 0.0, 
        0.0, 0.5, 0.0, 0.0, 
        0.0, 0.0, 0.5, 0.0, 
        0.5, 0.5, 0.5, 1.0);

  
   
  
vec4 ShadowCoord = Bias*FromLight*WorldPosition;
 
         
ShadowCoord /=ShadowCoord.w;
 vec3 VP = vec3(Light0Position) - vec3(WorldPosition);

    // Compute distance between surface and light position
 float   d = length(VP);

    // Normalize the vector from surface to light position
    VP = normalize(VP);
vec3 halfVector = normalize(VP + vec3(0,0,1));

 float   nDotVP = max(0.0, dot(n, VP));
 float    nDotHV = max(0.0, dot(n, halfVector));
float pf = 0.0;
    if (nDotVP == 0.0)
        pf = 0.0;
    else
        pf = pow(nDotHV, Shininess);
if (ColorEnabled==1)
{FragColor =outColor;}
else
{
if (Light0Enabled==1)
FragColor =Emission+(Specular*pf*Light0Specular+ Ambient*Light0Ambient+ Diffuse* nDotVP*Light0Diffuse);
else
FragColor =Emission;
}
float Visibility = 1.0;
if (nDotVP >0.0)
if (ShadowEnable)
Visibility = shadowVisibility(ShadowMap,ShadowCoord);





vec4 a = vec4(0.0,0.0,0.0,0.0);

FragColor *=Visibility;
FragColor.a = Translucent;

if (Texture0Enabled)
{
vec2 tpos= vec2(Texture0Matrix*vec4(FS_Texture_in,0.0,0.0));
a=texture(Texture0_,tpos);
FragColor *=a;
}
if ((Texture1Enabled)&&(!NormalMap)&&(!Tesselation))
{
vec2 tpos= vec2(Texture1Matrix*vec4(FS_Texture_in,0.0,0.0));
a=texture(Texture1_,tpos);
FragColor *=a;
}


}";
        /// <summary>
        /// is the vertex of the <see cref="SmallShader"/>.
        /// </summary>
        public static string SmallVertex =
@"// 
#version 410 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 Texture;
layout(location = 3) in vec3 Color;
out vec4 WorldNormal; 
out vec4 WorldPosition;
out vec4 outColor;
uniform mat4 ModelMatrix;
uniform mat4 ProjectionMatrix;
out vec2 FS_Texture_in;
uniform bool Texture2Enabled;
uniform sampler2D Texture2_;
uniform samplerBuffer tboTransform;
void main(){ 
 


outColor = vec4(Color,1);
WorldPosition=ModelMatrix *vec4(Position, 1.0);

WorldNormal = normalize(ModelMatrix * vec4(Normal, 0.0));
FS_Texture_in=Texture;
vec4 m= vec4(0,0,0,0);
if (Texture2Enabled)
{
vec4 v=texture(Texture2_,Texture);
float height=length(v);
 m= WorldNormal*height;
}
gl_Position = ProjectionMatrix*(ModelMatrix *vec4(Position, 1.0));

}   

";
    }

}
