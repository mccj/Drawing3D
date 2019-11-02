
using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{


    /// <summary>
    /// The abstract class Light encapsulates the essential properties of a light like
    /// <see cref="Ambient"/>, <see cref="Specular"/>, <see cref="Diffuse"/>
    /// </summary>
    [Serializable]
    public class Light
    {
        private float FConstantAttenuation;

        private float FLinearAttenuation;
        private float FQuadraticAttenuation;
        [NonSerialized]
        private OpenGlDevice Fdevice;
        private Color FAmbient;



        private Color FSpecular;
        private Color FDiffuse;
        private float enable;

        private xyz FDirection;
        private xyzwf FPosition = new xyzwf(3, 5, 10, 1);








        /// <summary>
        /// The constructor initializes the light by the following values
        /// ConstantAttenuation =10
        ///	LinearAttenuation	=0
        ///	QuadraticAttenuation=0
        ///	Diffuse				= White
        /// Ambient				= White
        ///	Specular			= White
        ///	Position = (0,0,10);
        /// There are 16 spot lights possible. You must then use the <see cref="OpenGlDevice.LargeSchader"/>.
        /// </summary>
        public Light()
        {
            this.FConstantAttenuation = 1;
            this.FLinearAttenuation = 0;
            this.FQuadraticAttenuation = 0;
            this.FDiffuse = Color.White;
            this.FAmbient = Color.White;
            this.FSpecular = Color.White;

            // this.FDirection = Position * (-1);

            enable = 0;
        }

        /// <summary>
        /// Retrieves and sets the constant attenuation
        /// </summary>
        public virtual float ConstantAttenuation
        {
            set
            {
                SetConstantAttenuation(value);


            }
            get { return FConstantAttenuation; }

        }
        /// <summary>
        /// Set method of the property <see cref="ConstantAttenuation"/>
        /// </summary>
        /// <param name="value">ConstantAttenuation</param>
        protected virtual void SetConstantAttenuation(float value)
        {
            FConstantAttenuation = value;
        }
        /// <summary>
        /// Holds the device in which the light is used.
        /// </summary>
        [Browsable(false)]
        public OpenGlDevice Device
        {
            get { return Fdevice; }
            set
            {
                SetDevice(value);
            }
        }

        /// <summary>
        /// Retrieves and sets the linear attenuation
        /// </summary>

        public virtual float LinearAttenuation
        {
            set
            {
                SetLinearAttenuation(value);
            }
            get { return FLinearAttenuation; }

        }
        /// <summary>
        /// Setmethod of the <see cref="LinearAttenuation"/>-property
        /// </summary>
        /// <param name="value">Specifies the LinearAttenuation</param>
        protected virtual void SetLinearAttenuation(float value)
        {
            FLinearAttenuation = value;
            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].linearAttenuation");
            if (F != null) F.Update();
           

        }
        private void Assign(Light L)
        {
            setPosition(L.Position);
            setAmbient(L.Ambient);
            setDiffuse(L.Diffuse);
            setSpecular(L.Specular);
            SetConstantAttenuation(L.ConstantAttenuation);
            SetQuadraticAttenuation(L.QuadraticAttenuation);
            SetLinearAttenuation(L.LinearAttenuation);

            setDirection(L.Direction);

        }
        /// <summary>
        /// Retrieves and sets the quadratic attenuation
        /// </summary>

        public virtual float QuadraticAttenuation
        {
            set { SetQuadraticAttenuation(value); }
            get { return FQuadraticAttenuation; }
        }
        /// <summary>
        /// Setmethod of the <see cref="QuadraticAttenuation"/>-Property
        /// </summary>
        /// <param name="value">QuadraticAttenuation</param>
        protected virtual void SetQuadraticAttenuation(float value)
        {
            FQuadraticAttenuation = value;
        }



        /// <summary>
        /// Protected setmethod of the property <see cref="Device"/>.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetDevice(OpenGlDevice value)
        {
            Fdevice = value;
         
        }

        /// <summary>
        /// Virtual SetMethod of the property <see cref="Enable"/>
        /// </summary>
        protected virtual void SetEnable(float value)
        {
            enable = value;
            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].enabled");
            if (F != null) F.Update();
            else
            {
                if (id == 0)
                {
                    if (value == 0) Device.LightEnabled = false;
                    else Device.LightEnabled = true;

                }
            }
          

        }
        /// <summary>
        /// Before you call an UpDate, you have to enable the light
        /// </summary>
        public float Enable
        {
            get { return enable; }
            set { SetEnable(value); }
        }

        /// <summary>
        /// virtual getmethod of the property Ambient
        /// </summary>
        /// <returns></returns>

        protected virtual Color getAmbient()
        {
            return FAmbient;
        }
        /// <summary>
        /// virtual setmethod of the property Ambient
        /// </summary>
        /// <returns></returns>

        protected virtual void setAmbient(Color value)
        {


            FAmbient = value;
            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].ambient");
            if (F != null) F.Update();
            else
            {
                if (id == 0)
                {
                    F = Device.Shader.getvar("Light0Ambient");
                    if (F != null) F.Update();
                }
            }
       


        }
        /// <summary>
        /// Retrieves and sets the ambient part of the light
        /// </summary>
        public Color Ambient
        {
            get { return getAmbient(); }
            set { setAmbient(value); }
        }
        /// <summary>
        /// virtual getmethod of the property Diffuse
        /// </summary>
        /// <returns></returns>

        protected virtual Color getDiffuse()
        {
            return FDiffuse;
        }
        /// <summary>
        /// virtual setmethod of the property Diffuse
        /// </summary>
        /// <returns></returns>

        protected virtual void setDiffuse(Color value)
        {


            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].diffuse");
            if (F != null) F.Update();
            else
            {
                if (id == 0)
                {
                    F = Device.Shader.getvar("Light0Diffuse");
                    if (F != null) F.Update();
                }
            }
            FDiffuse = value;
        }
        /// Retrieves and sets the diffuse part of the light.
        public Color Diffuse
        {
            get { return getDiffuse(); }
            set { setDiffuse(value); }
        }
        /// <summary>
        /// virtual getmethod of the property Specular
        /// </summary>
        /// <returns></returns>

        protected virtual Color getSpecular()
        {
            return FSpecular;
        }
        /// <summary>
        /// virtual setmethod of the property Specular
        /// </summary>
        /// <returns></returns>

        protected virtual void setSpecular(Color value)
        {


            FSpecular = value;
            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].specular");
            if (F != null) F.Update();
            else
            {
                if (id == 0)
                {
                    F = Device.Shader.getvar("Light0Specular");
                    if (F != null) F.Update();
                }
            }
       }
        /// Retrieves and sets the specualar part of the light.	
        public Color Specular
        {
            get { return getSpecular(); }
            set { setSpecular(value); }
        }
       private xyz _spotDirection;
        /// <summary>
        /// gets and sets the direction of a spot light. E.g. (0, 0, -1)
        /// </summary>
        public xyz spotDirection
        {  
            get { return _spotDirection; }
            set { _spotDirection = value;  }

        }
     
        float _spotExponent;
        /// <summary>
        /// gets and sets the exponent for the spotlight. Its the measure for the decreasing of the light. E.g. 3.
        /// </summary>
        public float spotExponent
        {
            get { return _spotExponent; }
            set { _spotExponent = value; ; }

        }
        float _spotCutoff;
        /// <summary>
        /// it is 180 for PointLight, where the the w coordinate of the position is 1.
        /// For the <b>spot</b> you set <see cref="spotCosCutoff"/> with  the w coordinate of the position 0.
        /// </summary>
        public float spotCutoff
        {
            get { return _spotCutoff; }
            set { _spotCutoff=value; }

        }
        float _spotCosCutoff = 0;
        /// <summary>
        /// sets and gets the cosinus of the spotlight. E.g. (float)Math.Cos(Math.PI / 4f).
        /// </summary>
        public float spotCosCutoff
        {
            get { return _spotCosCutoff; }
            set { _spotCutoff = value; ; }

        }
        /// <summary>
        /// The virtual getMethod of the property <see cref="Direction"/>
        /// </summary>

        protected virtual xyz getDirection()
        {
            return FDirection;
        }
        /// <summary>
        /// The virtual setMethod of the property <see cref="Direction"/>
        /// </summary>

        protected virtual void setDirection(xyz value)
        {

            FDirection = value;
           


        }
        /// <summary>
        /// Retrieves or writes the direction of the light
        /// </summary>
        public xyz Direction
        {
            get { return getDirection(); }
            set { setDirection(value); }
        }
        /// <summary>
        /// The virtual getMethod of the property <see cref="Position"/>
        /// </summary>

        protected virtual xyzwf getPosition()
        {
            return FPosition;
        }
        /// <summary>
        /// The virtual setMethod of the property <see cref="Position"/>
        /// </summary>

        protected virtual void setPosition(xyzwf value)
        {


            FPosition = value;
            if ((Device == null) || (Device.Shader == null)) return;
            int id = Device.Lights.IndexOf(this);
            if ((Device == null) || (Device.Shader == null)) return;
            Field F = Device.Shader.getvar("Lights[" + id.ToString() + "].position");
            if (F != null) F.Update();
            else
            {
                if (id == 0)
                {
                    F = Device.Shader.getvar("Light0Position");
                    if (F != null) F.Update();
                }
            }

        



        }


        /// <summary>
        /// Reads and writes the position of the light
        /// </summary>
        public xyzwf Position
        {
            get { return getPosition(); }
            set { setPosition(value); }
        }


    }
}
