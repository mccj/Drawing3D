//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    public partial class OpenGlDevice
    {
       
       static string BackGroundFragment = @"
         // Background shader
                uniform sampler2D Texture0_;
                uniform bool Texture0Enabled;
                uniform mat4 Texture0Matrix;
                varying vec2 texcoord;
                
                void main(void)
                {

                   vec4 FragColor= vec4(1.0,1.0,1.0,1.0);
                   if ((Texture0Enabled) )
                    {
                     vec2 tpos = vec2(Texture0Matrix * vec4(texcoord, 0.0, 1.0));
                     FragColor = texture2D(Texture0_, tpos); 
                    }
                     gl_FragColor = FragColor;  
                }
                ";
       static string BackGroundVertex = @"
 // Background shader
        attribute vec3 Position;
        attribute vec2 Texture;
        uniform mat4 ModelMatrix;
      
        uniform mat4 ProjectionMatrix;

      
        varying vec2 texcoord;
            void main(void)
            {
             gl_Position= ProjectionMatrix*ModelMatrix *vec4(Position, 1.0);
             texcoord=Texture;
          
            }
";
    }
}