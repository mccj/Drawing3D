using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        /// <summary>
        /// the used TessVertexShader.
        /// </summary>
        public static string TessVertexShader =
         @"
#version 410 core                                                                               
 layout(location = 0) in vec3 Position;                                                                                               
 layout(location = 1) in vec3 Normal;                                                 
 layout(location = 2) in vec2 Texture;                                                     
                                                                                            
uniform mat4 ModelMatrix;                                                                            
                                                                                                
out vec3 WorldPos_CS_in;                                                                        
out vec2 TexCoord_CS_in;                                                                        
out vec3 Normal_CS_in;                                                                          
                                                                                               
void main()                                                                                     
{                                                                                               

    WorldPos_CS_in = (ModelMatrix *vec4(Position, 1.0)).xyz;                                  
    TexCoord_CS_in = Texture;                                                            
    Normal_CS_in   = (ModelMatrix *vec4(Normal, 0.0)).xyz;   
 //   gl_Position =vec4(Position, 1.0);
                                       
}

          ";
        /// <summary>
        /// the used TessControlShader.
        /// </summary>
        public static string TessControlShader = @"

#version 410 core

layout(vertices = 3) out;

uniform vec3 gEyeWorldPos;
in vec3 WorldPos_CS_in[];                                                                       
in vec2 TexCoord_CS_in[];                                                                       
in vec3 Normal_CS_in[];                                                                         
 uniform float ResolutionFactor;                                                                                               
// attributes of the output CPs                                                                 
out vec3 WorldPos_ES_in[];                                                                      
out vec2 TexCoord_ES_in[];                                                                      
out vec3 Normal_ES_in[];  
float GetTessLevel(float Distance0, float Distance1)                                            
{                                                                                              
    float AvgDistance = (Distance0 + Distance1) / 2.0;                                          
                                                                                                
    if (AvgDistance <= 2.0) {                                                                   
        return 10.0*ResolutionFactor;                                                                            
    }                                                                                           
    else if (AvgDistance <= 5.0) {                                                              
        return 7.0*ResolutionFactor;                                                                             
    }                                                                                           
    else {                                                                                      
        return 3.0*ResolutionFactor;                                                                             
    }                                                                                           
}       
        void main(void)
       {

    WorldPos_ES_in[gl_InvocationID] = WorldPos_CS_in[gl_InvocationID];  
    TexCoord_ES_in[gl_InvocationID] = TexCoord_CS_in[gl_InvocationID];                          
    Normal_ES_in[gl_InvocationID]   = Normal_CS_in[gl_InvocationID];                            
   // Calculate the distance from the camera to the three control points                       
    float EyeToVertexDistance0 = distance(gEyeWorldPos, WorldPos_ES_in[0]);                     
    float EyeToVertexDistance1 = distance(gEyeWorldPos, WorldPos_ES_in[1]);                     
    float EyeToVertexDistance2 = distance(gEyeWorldPos, WorldPos_ES_in[2]);                     
                                                                                                
    // Calculate the tessellation levels                                                        
    gl_TessLevelOuter[0] = GetTessLevel(EyeToVertexDistance1, EyeToVertexDistance2);            
    gl_TessLevelOuter[1] = GetTessLevel(EyeToVertexDistance2, EyeToVertexDistance0);            
    gl_TessLevelOuter[2] = GetTessLevel(EyeToVertexDistance0, EyeToVertexDistance1);            
    gl_TessLevelInner[0] = gl_TessLevelOuter[2];           
            // Everybody copies their input to their output
              
        }";
        /// <summary>
        /// the used TessEvaluationShader.
        /// </summary>
        public static string TessEvaluationShader = @"
        # version 410 core
       in vec3 WorldPos_ES_in[];                                                                       
       in vec2 TexCoord_ES_in[];                                                                       
       in vec3 Normal_ES_in[];                                                                         
       out vec4 WorldPosition;                                                                                         
       out vec2 FS_Texture_in;                                                                        
      
        out vec4 WorldNormal;  
        uniform mat4 ProjectionMatrix; 
       // uniform sampler2D gDisplacementMap; 
        uniform sampler2D Texture2_; 
        uniform float gDispFactor;                 
          layout(triangles, equal_spacing, cw) in;
         
           vec2 interpolate2D(vec2 v0, vec2 v1, vec2 v2)                                                   
           {                                                                                               
            return vec2(gl_TessCoord.x) * v0 + vec2(gl_TessCoord.y) * v1 + vec2(gl_TessCoord.z) * v2;   
            }                                                                                               
                                                                                                
            vec3 interpolate3D(vec3 v0, vec3 v1, vec3 v2)                                                   
             {                                                                                               
               return vec3(gl_TessCoord.x) * v0 + vec3(gl_TessCoord.y) * v1 + vec3(gl_TessCoord.z) * v2;   
             }  
         void main(void)
        {
          vec3 WorldPos_FS_in; 
          vec3 Normal_FS_in;     
           // Interpolate the attributes of the output vertex using the barycentric coordinates        
           FS_Texture_in = interpolate2D(TexCoord_ES_in[0], TexCoord_ES_in[1], TexCoord_ES_in[2]);    
           Normal_FS_in = interpolate3D(Normal_ES_in[0], Normal_ES_in[1], Normal_ES_in[2]);            
      if (length(Normal_FS_in) <0.1)
          Normal_FS_in= vec3(0,0,1);
           Normal_FS_in = normalize(Normal_FS_in); 
           WorldPos_FS_in = interpolate3D(WorldPos_ES_in[0], WorldPos_ES_in[1], WorldPos_ES_in[2]); 
        
           float Displacement =texture(Texture2_, FS_Texture_in.xy).x;                                                
           WorldPos_FS_in += Normal_FS_in * Displacement*gDispFactor ; 

           WorldPosition=   vec4(WorldPos_FS_in, 1.0);   
           WorldNormal=vec4( Normal_FS_in,0.0);
           gl_Position =ProjectionMatrix *  vec4(WorldPos_FS_in, 1.0);    
        }
      ";
    }
}
