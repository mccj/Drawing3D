using System.Drawing;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        private Material FMaterial = Materials.Chrome;
        /// <summary>
        /// sets and gets a <see cref="Drawing3d.Material"/> to the <see cref="OpenGlDevice"/>
        /// </summary>
        public Material Material
        {
            get
            {

                return FMaterial;
            }
            set
            {
                if (Entity.Compiling)
                {
                    if (MeshCreator.HasMaterial)
                    {
                        MeshCreator.Renew();
                    }
                    MeshCreator.Material = value;
                }
                else
                UpDateMaterial(value);
                FMaterial = value;

            }
        }
        /// <summary>
        /// gets and set the ambient part of the <see cref="Material"/>
        /// </summary>
        public Color Ambient
        {
            get
            {
                return this.Material.Ambient;
            }
            set
            {

                setAmbient(value);

            }
        }
        /// <summary>
        /// gets and set the diffuse part of the <see cref="Material"/>
        /// </summary>
        public Color Diffuse
        {
            get
            {
                return this.Material.Diffuse;
            }
            set
            {

                setDiffuse(value);
            }
        }
        /// <summary>
        /// gets and set the specular part of the <see cref="Material"/>
        /// </summary>
        public Color Specular
        {
            get
            {
                return Material.Specular;
            }
            set
            {
                setSpecular(value);

            }
        }
        /// <summary>
        /// gets and set the emission part of the <see cref="Material"/>
        /// </summary>
        public Color Emission
        {
            get
            {
                return Material.Emission;

            }
            set
            {

                setEmission(value);

            }
        }
        /// <summary>
        /// gets and set the shininess part of the <see cref="Material"/>
        /// </summary>
        public double Shininess
        {
            get
            {
                return Material.Shininess;
            }
            set
            {

                setShininess(value);

            }
        }
        /// <summary>
        /// gets and set the translucent part of the <see cref="Material"/>
        /// </summary>
        public double Translucent
        {
            get
            {
                return Material.Translucent;
            }
            set
            {


                SetTranslucent(value);


            }
        }
        private  bool ColorEnabled = true;
        private void SetTranslucent(double Value)
        {
            FMaterial.Translucent = Value;

            if ((Shader != null) && (Shader.Using))
            {
              Field  C = Shader.getvar("Translucent");
                if (C != null) C.Update();
           
            }
        }
        private void CheckCompiling(Color value, ref Color MaterialColor)
        {
            if (Entity.Compiling)
            {
                if (MeshCreator.Material.Emission != value)
                {
                    MeshCreator.Renew();
                    MeshCreator.Material=FMaterial;

                }

            }
        }
        private void setAmbient(Color value)
        {
            FMaterial.Ambient = value;


            if ((Shader != null) && (Shader.Using))
            {

                Field A = Shader.getvar(" Ambient");
                if (A != null) A.Update();
             
            }
        }
        private void setDiffuse(Color value)
        {
            FMaterial.Diffuse = value;
 

            if ((Shader != null) && (Shader.Using))
            {
                Field A = Shader.getvar("Diffuse");
                if (A != null) A.Update();
       
            }
        }
        private void setEmission(Color value)
        {
            FMaterial.Emission = value;
            if (Entity.Compiling)
            {

                if (MeshCreator.HasEmission) MeshCreator.Renew();
                MeshCreator.Emission = value;
            }
            else
           if ((Shader != null) && (Shader.Using))
            {
                Field A = Shader.getvar("Emission");
                if (A != null) A.Update();
       
            }

        }
        private void setShininess(double value)
        {
 
            FMaterial.Shininess = value;
            if ((Shader != null) && (Shader.Using))
            {
                Field A = Shader.getvar("Shininess");
                if (A != null) A.Update();
         
            }
        }
        private void setSpecular(Color value)
        {
            FMaterial.Specular = value;


            if ((Shader != null) && (Shader.Using))
            {
                Field A = Shader.getvar("Specular");
                if (A != null) A.Update();
             
            }
        }


        private void UpDateMaterial(Material M)
        {



         
            bool ToShader = ((Shader != null) && (Shader.Using));
            if (!ColorEnabled) return;

            
         
            {
                Ambient = M.Ambient;

               
                if (ToShader)
                {
                    Field C = Shader.getvar("Ambient");
                    if (C != null) C.Update();
                    
                }
              
            }
     
            {

                Diffuse = M.Diffuse;
                if (ToShader)
                {
                    Field C = Shader.getvar("Diffuse");
                    if (C != null) C.Update();
 
                   
                }

            }

            {

                Specular = M.Specular;
                if (ToShader)
                {
                    Field C = Shader.getvar("Specular");
                    if (C != null) C.Update();
 
                }
            }

            {

                Emission = M.Emission;
                if (ToShader)
                {
                    Field C = Shader.getvar("Emission");
                    if (C != null) C.Update();

                }
            }

            {

                Shininess = M.Shininess;
                if (ToShader)
                {
                    Field C = Shader.getvar("Shininess");
                    if (C != null) C.Update();

                }
            }

            {

                Translucent = M.Translucent;
                if (ToShader)
                {
                    Field C = Shader.getvar("Translucent");
                    if (C != null) C.Update();
 
                }
               
            }
            CheckError();
       }
   }
}