using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shaders
{
    public partial class FrmShader : Form
    {

string Fragment= @"
// shader small
#version 330
uniform int ShadowFrameBufferSize;
uniform vec4 Light0Position;
uniform float BIAS;
uniform float ShadowIntensity;
uniform int SamplingCount;

in vec4 ShadowCoord;
in float _Smoothwidth;
uniform sampler2D ShadowMap;
// Clipping
uniform int ClippingPlaneEnabled[6];
uniform float ClippingPlane[6*4];  // 6 Planes zu je 4 float


float Visible(vec4 ShadowCoord    )
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
in float NdotL;
uniform sampler2D Texture0_;
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
in vec4 outColor;
in vec4 normal;
in vec2 texcoord;
in vec4 Posi;
out vec4 FragColor;

void main(void)
{
for (int i=0;i<6;i++)
{
if (ClippingPlaneEnabled[i]== 1)
   {
     if (dot(vec3(ClippingPlane[4*i],ClippingPlane[4*i+1],ClippingPlane[4*i+2]),vec3(Posi)) <ClippingPlane[4*i+3]) 
    {
     discard;
      return;
    }
     
   } 

}
if (NdotL <= -900.0) // Line
{
   FragColor = Emission;
return;
}
vec4 amb;
vec4 diff;
vec4 spec;
vec3 n=vec3(normal);
n=normalize(n); 

vec3 VP = vec3(Light0Position) - vec3(Posi);

    // Compute distance between surface and light position
 float   d = length(VP);

    // Normalize the vector from surface to light position
    VP = normalize(VP);
vec3 halfVector = normalize(VP + vec3(0,0,1));

 float   nDotVP = max(0.0, dot(vec3(normal), VP));
 float    nDotHV = max(0.0, dot(vec3(normal), halfVector));
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
vec2 tpos= vec2(Texture0Matrix*vec4(texcoord,0.0,0.0));
a=texture(Texture0_,tpos);
FragColor *=a;
}
}";


                      

string Vertex = @"// 
    #version 330
layout(location = 0) in vec3 Position;
uniform float WindowWidth;
uniform float WindowHeight;
uniform float Smoothwidth;   
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 Texture;
layout(location = 3) in vec3 Color;

out vec4 ShadowCoord; 
out vec4 lightDir; 
out vec4 normal; 
out vec4 UVText;
out float NdotHV;
out float NdotL;
out float dis;
out vec4 Posi;
out vec4 outColor;
out float _Smoothwidth;
uniform int Lined;

uniform vec4 Light0Direction;
uniform mat4 ModelMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 FromLight;
uniform bool Texture0Enabled;
uniform int DepthTexture; 
uniform sampler2DShadow shadowMap; 

out vec2 texcoord;
out float _smootwith;

void main(){ 
 if (Lined == 1)
{

gl_Position =ProjectionMatrix*ModelMatrix *vec4(Position, 1.0);
NdotL = -1000.0;
return;

}
vec3 N=vec3(0.0,0.0,0.0);
vec3 P=vec3(0.0,0.0,0.0);
vec2 T=vec2(0.0,0.0);
outColor = vec4(Color,1);
N= normalize(Normal);
P=Position;

T= Texture;

Posi=ModelMatrix *vec4(Position, 1.0);
_Smoothwidth=WindowWidth*(ProjectionMatrix*ModelMatrix *vec4(Smoothwidth,Smoothwidth,Smoothwidth, 1.0)).x;
  
 
normal = normalize(vec4(N, 0.0));
 normal = normalize(ModelMatrix * vec4(N, 0.0)/*-ModelMatrix * vec4(0,0,0, 1.0)*/);
  vec4 Pos= ProjectionMatrix*ModelMatrix *vec4(P, 1.0);
_Smoothwidth=Smoothwidth;
 //_Smoothwidth= length(ProjectionMatrix*ModelMatrix *vec4(Smoothwidth,Smoothwidth,Smoothwidth, 1.0));
texcoord=T;
 const mat4 Bias = mat4(     

        0.5, 0.0, 0.0, 0.0, 
        0.0, 0.5, 0.0, 0.0, 
        0.0, 0.0, 0.5, 0.0, 
        0.5, 0.5, 0.5, 1.0);

  
   
   vec4 Eye =ModelMatrix * vec4(P, 1.0);
ShadowCoord = Bias*FromLight*ModelMatrix*vec4(P, 1.0);
 
         
ShadowCoord /=ShadowCoord.w;
  

gl_Position = Pos;

}   

";
        public MyDevice Device = null;
        public FrmShader()
        {
            InitializeComponent();
            tbFragment.Text = Fragment;
            tbVertex.Text = Vertex;
        }

        private void Shader_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            if (openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                if ((FragmentShader != null) && (FragmentShader != ""))
                {
                    if ((tbFragment.Modified)|| (tbVertex.Modified))
                        {
                        if (MessageBox.Show("Save Changes","Info",MessageBoxButtons.YesNo)== DialogResult.Yes)
                        {
                            saveShader();
                        }
                    }
               }
                string _ShaderName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                string _PathName = System.IO.Directory.GetParent(openFileDialog1.FileName).FullName;
                string _FragmentShader = _PathName + "\\" + _ShaderName + ".frag";
                string _VertexShader = _PathName + "\\" + _ShaderName + ".vert";
                if (!System.IO.File.Exists(_FragmentShader))
                {
                    MessageBox.Show(_FragmentShader + " does not exists");
                    return;
                }
                if (!System.IO.File.Exists(_VertexShader))
                {
                    MessageBox.Show(_VertexShader + " does not exists");
                    return;
                }
                try
                {
                   
                 tbFragment.Text=   System.IO.File.ReadAllText(_FragmentShader);
                 tbVertex.Text = System.IO.File.ReadAllText(_VertexShader);
                   
                    ShaderName = _ShaderName;
                    PathName = _PathName;
                    FragmentShader = _FragmentShader;
                    VertexShader = _VertexShader;
                    this.Text = ShaderName;

                }
                catch (Exception E)
                {

                    MessageBox.Show(E.Message);
                }
            }
      }

     
        private void tbFragment_MouseMove(object sender, MouseEventArgs e)
        {
            Point P = tbFragment.PointToClient(MousePosition);
            int k = tbFragment.GetCharIndexFromPosition(P);
            int line = tbFragment.GetLineFromCharIndex(k)+1;
            lbLineFragment.Text = line.ToString();
        }
        private void tbVertex_MouseMove(object sender, MouseEventArgs e)
        {
            Point P = tbVertex.PointToClient(MousePosition);
            int k = tbVertex.GetCharIndexFromPosition(P);
            int line = tbVertex.GetLineFromCharIndex(k)+1;
            lbLineVertex.Text = line.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Device.ActivateShader(tbFragment.Text, tbVertex.Text);
        }
        string ShaderName;
        string PathName;
        string FragmentShader;
        string VertexShader;

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog()== DialogResult.OK)
            {
                string   _ShaderName = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                string _PathName = System.IO.Directory.GetParent(saveFileDialog1.FileName).FullName;
                string _FragmentShader = _PathName + "\\" + _ShaderName + ".frag";
                string _VertexShader = _PathName + "\\" + _ShaderName + ".vert";
                                  {

                        try
                        {
                            System.IO.File.WriteAllText(_FragmentShader, tbFragment.Text);
                            System.IO.File.WriteAllText(_VertexShader, tbVertex.Text);
                        ShaderName = _ShaderName;
                        PathName = _PathName;
                        FragmentShader = _FragmentShader;
                        VertexShader = _VertexShader;
                            this.Text = ShaderName;
                        }
                        catch (Exception E)
                        {

                            MessageBox.Show(E.Message);
                        }

                        
                    }

            }
        }
    
        void saveShader()
        {
            if ((FragmentShader != null) && (FragmentShader != ""))
                try
                {
                    System.IO.File.WriteAllText(FragmentShader, tbFragment.Text);
                    System.IO.File.WriteAllText(VertexShader, tbVertex.Text);


                }
                catch (Exception E)
                {

                    MessageBox.Show(E.Message);
                }

          
            tbFragment.Modified = false;
            tbVertex.Modified = false;
        }
      
     

      

      


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tbVertex.Focused)
            tbVertex.Undo();
            if (tbFragment.Focused)
                tbFragment.Undo();
        }

        private void FrmShader_Load(object sender, EventArgs e)
        {
            undoToolStripMenuItem.ShortcutKeys = Keys.Back | Keys.Alt;
            undoToolStripMenuItem.Visible = false;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveShader();
        }
    }
}
