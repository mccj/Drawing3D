
using System;
using System.Collections.Generic;

using System.Text;


namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        /// <summary>
        /// is the fragment of the <see cref="LargeSchader"/>
        /// </summary>
        public static string LargeFragment =
  @"
// shader long
#version 330
uniform int ShadowFrameBufferSize;

uniform float BIAS;
uniform float ShadowIntensity;
uniform int SamplingCount;

//in vec4 ShadowCoord;
uniform float Smoothwidth;
uniform sampler2D ShadowMap;
uniform mat4 FromLight;
// Clipping
uniform int ClippingPlaneEnabled[6];
uniform float ClippingPlane[6*4];  // 6 Planes zu je 4 float
vec2 poissonDisk[16] = vec2[]( 
   vec2( -0.94201624, -0.39906216 ), 
   vec2( 0.94558609, -0.76890725 ), 
   vec2( -0.094184101, -0.92938870 ), 
   vec2( 0.34495938, 0.29387760 ), 
   vec2( -0.91588581, 0.45771432 ), 
   vec2( -0.81544232, -0.87912464 ), 
   vec2( -0.38277543, 0.27676845 ), 
   vec2( 0.97484398, 0.75648379 ), 
   vec2( 0.44323325, -0.97511554 ), 
   vec2( 0.53742981, -0.47373420 ), 
   vec2( -0.26496911, -0.41893023 ), 
   vec2( 0.79197514, 0.19090188 ), 
   vec2( -0.24188840, 0.99706507 ), 
   vec2( -0.81409955, 0.91437590 ), 
   vec2( 0.19984126, 0.78641367 ), 
   vec2( 0.14383161, -0.14100790 ) 
);
struct LightType 
{   
        vec4 ambient;
        vec4 diffuse;
        vec4 specular;
        vec4 position;
   vec3 spotDirection;
   float spotExponent;
   float spotCutoff;
   float spotCosCutoff;
   float constantAttenuation;
        float linearAttenuation;
        float quadraticAttenuation;

        float enabled;
};
uniform int ColorEnabled;
in vec4 outColor; 
uniform LightType Lights[16];
// Returns a random number based on a vec3 and an int.
float random(vec3 seed, int i){
	vec4 seed4 = vec4(seed,i);
	float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));
	return fract(sin(dot_product) * 43758.5453);
}

float Visible(vec4 ShadowCoord    )
{

 if (vec3(texture2D(ShadowMap,ShadowCoord.xy)).z >=ShadowCoord.z)
 return 1.0;
 return 0.0;
}

float shadowVisibility(sampler2D shadowMap,vec4 ShadowCoord)
{
if (SamplingCount== 0) return 1.0;
float visibility = 1.0;
visibility = Visible(ShadowCoord);

//visibility = Visible(vec4(vec2(ShadowCoord.xy), ShadowCoord.z-BIAS,1.0));
// Sample the shadow map 

int anz= SamplingCount;


if (anz >1)
{
	for (int i=1;i<anz;i++){
   vec2 d=vec2(Smoothwidth,Smoothwidth);
    visibility +=Visible(vec4(vec2(ShadowCoord.xy +((Smoothwidth/ShadowFrameBufferSize/2)*poissonDisk[i])), ShadowCoord.z-BIAS,1.0));
	}
}  
visibility /=(anz);
visibility = 1.0-(1.0-visibility)*ShadowIntensity;
return  min(visibility, 1.0);

}
uniform bool NormalMap= false;
//---------------------------------------------------
// --------------- tesselation-----------------------
uniform bool Tesselation= false;
//---------------------------------------------------
// Interpolated values from the vertex shaders
uniform bool Texture1Enabled;
uniform mat4 Texture1Matrix;
uniform sampler2D Texture1_;

uniform sampler2D Texture0_;
uniform bool Texture0Enabled;

in vec3 lightVecNormalized;// = normalize(vec3(0.5, 0.5, 2.0));


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
uniform bool LightEnabled;
uniform int LightCount;
uniform vec4 Light0Diffuse;
uniform vec4 Light0Specular; 
;
//in float NdotHV;
//in float NdotL;
in vec4 WorldNormal;
//in vec2 texcoord;
in vec2 FS_Texture_in;
in vec4 WorldPosition;
out vec4 FragColor;
void DirectionalLight(in int i, in vec3 normal, inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    float nDotVP;         // normal . light direction
    float nDotHV;         // normal . light half vector
    float pf;             // power factor
   vec3  VP = normalize(vec3(Lights[i].position) - vec3(WorldPosition));
   vec3  halfVector = normalize(VP + vec3(0,0,1));    
    nDotVP = max(0.0, dot(normal,     normalize(vec3(Lights[i].position))));
    nDotHV = max(0.0, dot(normal, halfVector ));

    if (nDotVP == 0.0)
        pf = 0.0;
    else
        pf = pow(nDotHV, Shininess);

    ambient += Lights[i].ambient;
    diffuse += Lights[i].diffuse * nDotVP;
    specular += Lights[i].specular * pf;
}
void SpotLight(in int i, in vec3 eye, vec3 ecPosition3, in vec3 normal, inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    float nDotVP;           // normal . light direction
    float nDotHV;           // normal . light half vector
    float pf;               // power factor
    float spotDot;          // cosine of angle between spotlight
    float spotAttenuation;  // spotlight attenuation factor
    float attenuation;      // computed attenuation factor
    float d;                // distance from surface to light source
    vec3 VP;                // direction from surface to light position
    vec3 halfVector;        // direction of maximum highlights

    // Compute vector from surface to light position
    VP = vec3(Lights[i].position) - ecPosition3;

    // Compute distance between surface and light position
    d = length(VP);

    // Normalize the vector from surface to light position
    VP = normalize(VP);

    // Compute attenuation
    attenuation = 1.0 / (Lights[i].constantAttenuation +
                         Lights[i].linearAttenuation * d +
                         Lights[i].quadraticAttenuation * d * d);

    // See if point on surface is inside cone of illumination
    spotDot = dot(-VP, normalize(Lights[i].spotDirection));

    if (spotDot < Lights[i].spotCosCutoff)
        spotAttenuation = 0.0; // light adds no contribution
    else
        spotAttenuation = pow(spotDot, Lights[i].spotExponent);

    // Combine the spotlight and distance attenuation.
    attenuation *= spotAttenuation;

    halfVector = normalize(VP + eye);

    nDotVP = max(0.0, dot(normal, VP));
    nDotHV = max(0.0, dot(normal, halfVector));

    if (nDotVP == 0.0)
        pf = 0.0;
    else
        pf = pow(nDotHV, Shininess);
   
    ambient += Lights[i].ambient * attenuation;
    diffuse += Lights[i].diffuse * nDotVP * attenuation;
    specular += Lights[i].specular * pf * attenuation;
}
void PointLight(in int i, in vec3 eye, in vec3 ecPosition3, in vec3 normal, inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    float nDotVP;         // normal . light direction
    float nDotHV;         // normal . light half vector
    float pf;             // power factor
    float attenuation;    // computed attenuation factor
    float d;              // distance from surface to light source
    vec3 VP;             // direction from surface to light position
    vec3 halfVector;     // direction of maximum highlights

    // Compute vector from surface to light position
    VP = vec3(Lights[i].position) - ecPosition3;

    // Compute distance between surface and light position
    d = length(VP);

    // Normalize the vector from surface to light position
    VP = normalize(VP);

    // Compute attenuation
    attenuation = 1.0 / (Lights[i].constantAttenuation +
                         Lights[i].linearAttenuation * d +
                         Lights[i].quadraticAttenuation * d * d);

    halfVector = normalize(VP + eye);
 
    nDotVP = max(0.0, dot(normal, VP));
    nDotHV = max(0.0, dot(normal, halfVector));

    if (nDotVP == 0.0)
        pf = 0.0;
    else
        pf = pow(nDotHV, Shininess);

    ambient += Lights[i].ambient * attenuation;
    diffuse += Lights[i].diffuse * nDotVP * attenuation;
    specular += Lights[i].specular * pf * attenuation;
}
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
////if (NdotL <= -900.0) // Line
//{
//   FragColor = Emission;
//return;
//}
vec4 amb;
vec4 diff;
vec4 spec;
vec3 n=vec3(WorldNormal);
n=normalize(n); 
int ct=0;
for (int i=0;i<16;i++)
{
if (Lights[i].enabled==1)
{
  if (Lights[i].position.w == 0.0)
            DirectionalLight(i, n, amb, diff, spec);
        else if (Lights[i].spotCutoff == 180.0)
           PointLight(i, vec3(0.0,0.0,1.0), vec3(WorldPosition), n, amb, diff, spec);
        else
            SpotLight(i, vec3(0.0,0.0,1.0), vec3(WorldPosition), n, amb, diff, spec);

ct +=1;
}
}
if (ColorEnabled==1)
{FragColor =outColor;}
else
FragColor =Emission+(Specular*spec+ Ambient*amb+ Diffuse* diff)/float(ct);

float Visibility = 1.0;

if (ShadowEnable)
{
const mat4 Bias = mat4(     
        0.5, 0.0, 0.0, 0.0, 
        0.0, 0.5, 0.0, 0.0, 
        0.0, 0.0, 0.5, 0.0, 
        0.5, 0.5, 0.5, 1.0);
vec4 ShadowCoord = Bias*FromLight*WorldPosition;
ShadowCoord /=ShadowCoord.w;
Visibility = shadowVisibility(ShadowMap,ShadowCoord);
}




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
        /// is the vertex of the <see cref="LargeSchader"/>.
        /// </summary>
        public static string LargeVertex =
@"// shader long
    #version 330
 
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

void main(){ 

FS_Texture_in=Texture;
outColor = vec4(Color,1);
WorldPosition=ModelMatrix *vec4(Position, 1.0);
WorldNormal = normalize(ModelMatrix * vec4(Normal, 0.0));
gl_Position = ProjectionMatrix*ModelMatrix *vec4(Position, 1.0);;

}   

";
    }

}

