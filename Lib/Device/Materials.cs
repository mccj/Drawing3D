
using System;
using System.ComponentModel;
using System.Drawing;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
   /// <summary>
   /// an enumeration, with the parts of a material.
   /// </summary>
    public enum MaterialColor
    { 
        /// <summary>
        /// 
        /// </summary>
        Ambient,
        /// <summary>
        /// 
        /// </summary>
        Diffuse,
        /// <summary>
        /// 
        /// </summary>
        Specular,
        /// <summary>
        /// 
        /// </summary>
        Emission,
        /// <summary>
        /// 
        /// </summary>
        Shininess,
         /// <summary>
        /// 
        /// </summary>
        Translucent
    }
    /// <summary>
    /// the material for the objects of a scene.
    /// </summary>
    [Serializable]
	public struct Material
    {
        /// <summary>
        /// override equals and returns if the materials are equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>tru, if the materials are equals</returns>
        public override bool Equals(object obj)
        {
            if (obj is Material)
            {

                Material M = (Material)obj;
                if (Name != M.Name) return false;
                if (FAmbient != M.Ambient) return false;
                if (FDiffuse != M.Diffuse) return false;
                if (FSpecular != M.Specular) return false;
                if (FEmission != M.Emission) return false;
                if (FShininess != M.Shininess) return false;
                if (FTranslucent != M.Translucent) return false;
                return true;
            }
            return base.Equals(obj);
        }
        private bool IsEmpty()
        {
            return Ambient.IsEmpty;
        }
        /// <summary>
        /// overrides GetHashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// construcotor with an initializing material.
        /// </summary>
        /// <param name="M"></param>
        public Material(Material M)
        {
            this.Name = M.Name;
            this.Tag = M.Tag;
            FAmbient = M.Ambient;
            FDiffuse = M.Diffuse;
            FSpecular = M.Specular;
            FTranslucent = M.Translucent;
            FEmission = M.Emission;
            FShininess = M.Shininess;
            
        }
        /// <summary>
        /// gets a copy of the material.
        /// </summary>
        /// <returns>a copy of the material</returns>
        public Material Copy()
         {
            Material Result = new Material();
            Result.assign(this);
            return Result;
         
         }
        /// <summary>
        /// a free programmable tag.
        /// </summary>
        [BrowsableAttribute(false)]
        [NonSerialized]
        public object Tag;
        [BrowsableAttribute(false)]
        [NonSerialized]
       
		private Color FAmbient;
        private Color FDiffuse;
        private Color FEmission;
        private Color FSpecular;
        private double FShininess;
        /// <summary>
        /// Overrides the ToString method. If a matrial has a name, then this name will be returned.
        /// </summary>
        /// <returns>If a matrial has a name, then this name will be returned.</returns>
        public override string ToString()
        {
            if ((Name != null) && (Name != ""))
                return Name;
            return base.ToString();
        }
		private double FTranslucent;
		
		/// <summary>
		/// The void constructor initializes the material with <see cref="Materials.Chrome"/>.
		/// </summary>
		public static Material newMaterial()
		{
            Material Result = new Material();
			 Result.assign( Materials.Chrome);
            return Result;
		}
		/// <summary>
		/// This constructor needs all properties of the material.
		/// </summary>
		/// <param name="Name">describes the material</param>
		/// <param name="Ambient">part of ambient </param>
		/// <param name="Diffuse">part of diffuse</param>
		/// <param name="Specular">part of specular</param>
		/// <param name="Emission">part of emission</param>
		/// <param name="Shininess">shininess (brilliance)</param>
		/// <param name="Translucent">translucence in the range form 0 to 1</param>
		public Material(
			String Name,
			Color Ambient, 
			Color Diffuse,
			Color Specular,
			Color Emission,
			double Shininess,
			double Translucent
			)
		{
			this.Name		= Name;
            Tag = 0;
       
            FAmbient = Ambient;
            FTranslucent = Translucent;
           
			FDiffuse	= Diffuse;
			FSpecular	= Specular;
			FEmission	= Emission;
			FShininess	= Shininess;
			
		}
		/// <summary>
		/// For every material a name is given, which describes it. It can be void.
		/// </summary>
		public string Name;
		
		
		/// <summary>
		/// Getmethod of the property Ambient
		/// </summary>
		/// <returns>Ambientcolor</returns>
		public Color getAmbient()
		{
			return FAmbient;
		}
		/// <summary>
		/// Sets the meterial properties to an other material
		/// </summary>
		/// <param name="value"></param>
		public void assign(Material value)
		{
			
             
			this.Ambient=value.Ambient;
			this.Diffuse=value.Diffuse;
			this.Specular=value.Specular;
			this.Translucent=value.Translucent;
			this.Emission=value.Emission;
			this.Shininess=value.Shininess;
         
		
		}
		/// <summary>
		/// Virtual setmethod of the property <see cref="Ambient"/>.
		/// </summary>
		/// <param name="value">Ambient color</param>
		private void setAmbient(Color value)
		{
			
			FAmbient=value;
			
		}
		/// <summary>
		/// Retrieves and sets the ambient part.
		/// </summary>
		public  Color Ambient
		{
			get { return getAmbient();}
            set { setAmbient(value); }
		}
		/// <summary>
		/// Virtual getmethod of <see cref="Diffuse"/>-property
		/// </summary>
		/// <returns>Diffuse part of the material</returns>


		private Color getDiffuse()
		{
			return FDiffuse;
		}
		/// <summary>
		/// Virtual setmethod of <see cref="Diffuse"/>-property
		/// </summary>
		/// <param name="value">Diffuse part of the material</param>
		private void setDiffuse(Color value)
		{
		
			FDiffuse=value;
		
		}
        /// <summary>
        /// Retrieves and sets the diffuse part.
        /// </summary>
        public Color Diffuse
		{
			get { return getDiffuse();}
            set { setDiffuse(value); }
		}
		/// <summary>
		/// Virtual getmethod of <see cref="Specular"/>-property
		/// </summary>
		/// <returns>Specular part of the material</returns>
	

		private Color getSpecular()
		{
			return FSpecular;
		}
        /// <summary>
        /// Virtual setmethod of <see cref="Specular"/>-property
        /// </summary>
        /// <param name="value">Specular part of the material</param>

        private void setSpecular(Color value)
		{
			
			FSpecular=value;
		
		}
		/// <summary>
		/// Retrieves and sets the specular part.
		/// </summary>

		public Color Specular
		{
			get { return getSpecular();}
            set { setSpecular(value); }
		}
        /// <summary>
        /// Virtual getmethod of <see cref="Emission"/>-property
        /// </summary>
        /// <returns>Emission part of the material</returns>



        private Color getEmission()
		{
			return FEmission;
		}
        /// <summary>
        /// Virtual setmethod of <see cref="Emission"/>-property
        /// </summary>
        /// <param name="value">Emission part of the material</param>

        private void setEmission(Color value)
		{
			
			FEmission=value;
		
		}
        /// <summary>
        /// Retrieves and sets the emission part.
        /// </summary>

        public Color Emission
		{
			get { return getEmission();}
		   set 
            {
               
                setEmission(value);}
		}
        /// <summary>
        /// Virtual getmethod of <see cref="Shininess"/>-property
        /// </summary>
        /// <returns>Shininessvalue of the material</returns>


        private double getShininess()
		{
			return FShininess;
		}
        /// <summary>
        /// Virtual setmethod of <see cref="Shininess"/>-property
        /// </summary>
        /// <param name="value">Shininessvalue of the material</param>

        private void setShininess(double value)
		{
		
			FShininess=value;
		
		}
       
        /// <summary>
        /// Retrieves and sets the Shininessvalue.
        /// </summary>

        public double Shininess
		{
			get { return getShininess();}
            set { setShininess(value); }
		}

        /// <summary>
        /// Virtual getmethod of <see cref="Translucent"/>-property
        /// </summary>
        /// <returns>Translucentvalue of the material</returns>

        private double getTranslucent()
		{
			return FTranslucent;
		}
        /// <summary>
        /// Virtual setmethod of <see cref="Translucent"/>-property
        /// </summary>
        /// <param name="value">Translucentvalue of the material</param>

        private void setTranslucent(double value)
		{
			
			FTranslucent=value;
			
		}
        /// <summary>
        /// Retrieves and sets the translucentvalue. It has to be between 0 ( translucent ) and 1 ( solid)
        /// </summary>

        public double Translucent
		{
			get { return getTranslucent();}
            set
            { 
                
                
                setTranslucent(value);}
		}
	}
	/// <summary>
	/// This class describes a color by normalized R, G, B, A - values.
	/// 
	/// </summary>
	/// <example>
	/// Red : RGBA.R = 1, RGBA.B = 0; RGBA.G = 0; RGBA.A = 0;
	/// </example>
 
	/// <summary>
	/// This class contains a collection of predefined static materials.
	/// </summary>
    [Serializable]
	public class Materials
    {

        /// <summary>
        /// Gets an array of predefined materials.
        /// </summary>
        /// <returns></returns>
        private static Material[] getMaterials()
        {
            Material[] result = {
                Brass,
                PolishedBronze,
                Bronze,
                Chrome,
                Copper,
                PolishedCopper,
                Gold,
                PolishedGold,
                Pewter,
                Silver,
                PolishedSilver,
                Emerald,
                Jade,
                Obsidian,
                Pearl,
		        Ruby,
		        Turquoise,
		        BlackPlastic,
		        BlackRubber};
            return result;
	




        }
		private static Color CNorm (double R, double G, double B, double A)
		{
        Color result =   Color.FromArgb(Utils.Round(A * 255),Utils.Round(R * 255), Utils.Round(G * 255), Utils.Round(B * 255));
    
		
		return	result;
  		}
        private static Color CZero()
        {
            return Color.Empty;
        }
		/// <summary>
		/// Predefined Brassmaterial
		/// </summary>
		public static Material Brass = new Material(
		"Brass",
		CNorm(0.329412, 0.223529, 0.027451, 1.0),
		CNorm(0.780392, 0.568627, 0.113725, 1.0),
		CNorm(0.992157, 0.941176, 0.807843, 1.0),
		Color.Black,
		27.8974,
		1.0);
		/// <summary>
		/// Predefined PolishedBronzematerial
		/// </summary>
		
		public static Material PolishedBronze = new Material(
		"PolishedBronze",
		CNorm(0.25, 0.148, 0.06475, 1.0),
		CNorm(0.4, 0.2368, 0.1036, 1.0),
		CNorm(0.774597, 0.458561, 0.200621, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		76.8,
		1.0
		);
		/// <summary>
		/// Predefined Bronzematerial
		/// </summary>

		public static Material Bronze = new Material(
		"Bronze",
		CNorm(0.2125, 0.1275, 0.054, 1.0),
		CNorm(0.714, 0.4284, 0.18144, 1.0),
		CNorm(0.393548, 0.271906, 0.166721, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		25.6,
		1.0
		);
        /// <summary>
        /// Predefined Zeromaterial (black)
        /// </summary>
        public static Material Zero = new Material(
        null,
        CZero(),
        CZero(),
        CZero(),
        CZero(),
        0.0,
        1.0
        );
        /// <summary>
        /// Predefined Chromematerial
        /// </summary>
        public static Material Chrome = new Material(
		"Crome",
		CNorm(0.25, 0.25, 0.25, 1.0),
      
        CNorm(0.6, 0.6, 0.6, 1.0), // Diffuse neu
        CNorm(0.774597, 0.774597, 0.774597, 1.0),
        Color.Black,
		76.8,
		1.0
		);
		/// <summary>
		/// Predefined Coppermaterial
		/// </summary>

		public static Material Copper = new Material(
		"Copper",
		CNorm(0.19125, 0.0735, 0.0225, 1.0),
		CNorm(0.7038, 0.27048, 0.0828, 1.0),
		CNorm(0.256777, 0.137622, 0.086014, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		12.8,
		1.0
		);
		/// <summary>
		/// Predefined PolishedCoppermaterial
		/// </summary>

		public static Material PolishedCopper = new Material(
		"PolishedCopper",
		CNorm(0.2295, 0.08825, 0.0275, 1.0),
		CNorm(0.5508, 0.2118, 0.066, 1.0),
		CNorm(0.580594, 0.223257, 0.0695701, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		51.2,
		1.0
		);
		/// <summary>
		/// Predefined Goldmaterial
		/// </summary>

		public static Material Gold = new Material(
		"Gold",
		CNorm(0.24725, 0.1995, 0.0745, 1.0),
		CNorm(0.75164, 0.60648, 0.22648, 1.0),
		CNorm(0.628281, 0.555802, 0.366065, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		51.2,
		1.0
		);
		/// <summary>
		/// Predefined PolishedGold
		/// </summary>

		public static Material PolishedGold = new Material(
		"PolishedGold",
		CNorm(0.24725, 0.2245, 0.0645, 1.0),
		CNorm(0.34615, 0.3143, 0.0903, 1.0),
		CNorm(0.797357, 0.723991, 0.208006, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		83.2,
		1.0
		);

		/// <summary>
		/// Predefined Pewtermaterial
		/// </summary>

		public static Material Pewter = new Material(
		"Pewter",
		CNorm(0.105882, 0.058824, 0.113725, 1.0),
		CNorm(0.427451, 0.470588, 0.541176, 1.0),
		CNorm(0.333333, 0.333333, 0.521569, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		9.84615,
		1.0
		);
		/// <summary>
		/// Predefined Silvermaterial
		/// </summary>

		public static Material Silver = new Material(
		"Silver",
		CNorm(0.19225, 0.19225, 0.19225, 1.0),
		CNorm(0.50754, 0.50754, 0.50754, 1.0),
		CNorm(0.508273, 0.508273, 0.508273, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		51.2,
		1.0
		);
		/// <summary>
		/// Predefined polished Silvermaterial
		/// </summary>

		public static Material PolishedSilver = new Material(
		"PolishedSilver",
		CNorm(0.23125, 0.23125, 0.23125, 1.0),
		CNorm(0.2775, 0.2775, 0.2775, 1.0),
		CNorm(0.773911, 0.773911, 0.773911, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		89.6,
		1.0
		);
		/// <summary>
		/// Predefined Emerald
		/// </summary>

		public static Material Emerald = new Material(
		"Emerald",
		CNorm(0.0215, 0.1745, 0.0215, 1.0),
		CNorm(0.07568, 0.61424, 0.07568, 1.0),
		CNorm(0.633, 0.727811, 0.633, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		76.8,
		0.55
		);
        /// <summary>
        /// Predefined Jade
        /// </summary>
        public static Material Jade = new Material(
		"Jade",
		CNorm(0.135, 0.2225, 0.1575, 1.0),
		CNorm(0.54, 0.89, 0.63, 1.0),
		CNorm(0.316228, 0.316228, 0.316228, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		12.8,
		0.5
		);
		/// <summary>
		/// Predefined Obsidian
		/// </summary>

		public static Material Obsidian = new Material(
		"Obsidian",
		CNorm(0.05375, 0.05, 0.06625, 1.0),
		CNorm(0.18275, 0.17, 0.22525, 1.0),
		CNorm(0.332741, 0.328634, 0.346435, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		38.4,
		0.82
		);
		/// <summary>
		/// Predefined Pearl
		/// </summary>

		public static Material Pearl = new Material(
		"Pearl",
		CNorm(0.25, 0.20725, 0.20725, 1.0),
		CNorm(1.0, 0.829, 0.829, 1.0),
		CNorm(0.296648, 0.296648, 0.296648, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		11.264,
		0.922
		);
		/// <summary>
		/// Predefined Ruby
		/// </summary>

		public static Material Ruby = new Material(
		"Ruby",
		CNorm(0.1745, 0.01175, 0.01175, 1.0),
		CNorm(0.61424, 0.04136, 0.04136, 1.0),
		CNorm(0.727811, 0.626959, 0.626959, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		76.8,
		0.55
		);
		/// <summary>
		/// Predefined Turquoise
		/// </summary>

		public static Material Turquoise = new Material(
		"Turqoise",
		CNorm(0.1, 0.18725, 0.1745, 1.0),
		CNorm(0.396, 0.74151, 0.69102, 1.0),
		CNorm(0.297254, 0.30829, 0.306678, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		12.8,
		0.8
		);
		/// <summary>
		/// Predefined black plastic
		/// </summary>

		public static Material BlackPlastic = new Material(
		"BlackPlastic",
		CNorm(0, 0, 0, 1.0),
		CNorm(0.01, 0.01, 0.01, 1.0),
		CNorm(0.5, 0.5, 0.5, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		32,
		1.0
		);
		/// <summary>
		/// Predefined black rubber
		/// </summary>

		public static Material BlackRubber = new Material(
		"BlackRubber",
		CNorm(0.02, 0.02, 0.02, 1.0),
		CNorm(0.01, 0.01, 0.01, 1.0),
		CNorm(0.4, 0.4, 0.4, 1.0),
		CNorm(0.0,0.0,0.0, 1.0),
		10,
		1.0 );
	}
}
