namespace Drawing3d
{
    public partial class OpenGlDevice{

        //-----------------------------------------------------------
        //                Depthshader
        //-----------------------------------------------------------------
        /// <summary>
        /// is the fragment of the ShadowDepthShader
        /// </summary>
        public static string DepthFragment = @"#version 330
// depth shader
out vec4 Color;
in float depth;
void main(){


}
";

        /// <summary>
        /// is the vertex of the ShadowDepthShader
        /// </summary>
        public static string DepthVertex = @"
// depth shader
#version 330
uniform mat4 ProjectionMatrix;
uniform mat4 ModelMatrix;
uniform mat4 FromLight;
 in vec3 Position;
 out float depth;
void main(){
gl_Position=FromLight*  ModelMatrix * vec4(Position, 1.0);

}";


    }
}
