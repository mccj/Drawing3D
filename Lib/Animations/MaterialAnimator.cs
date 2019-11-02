using System.Collections.Generic;
using System;
namespace Drawing3d
{
    /// <summary>
    /// interpolates two materials: <see cref="FromMaterial"/> and <see cref="ToMaterial"/>.
    /// </summary>
    [Serializable]
    public class MaterialAnimator : OglAnimator
    {
        /// <summary>
        /// a constructor containing the device
        /// </summary>
        /// <param name="Device">the device in which the animator works.</param>
        public MaterialAnimator(OpenGlDevice Device):base(Device)
        {

        }
        /// <summary>
        /// an empty constructor.
        /// </summary>
        public MaterialAnimator() : base()
        {

        }

        public override void Start()
        {
            base.Start();
            _value = FromMaterial;
        }
        Material LinearMaterial(Material MaterialFrom, Material MaterialTo, double t)
        {
            if (t > 1)
                return MaterialTo;
            if (t < 0)
                return MaterialFrom;
         return   new Material("", ColorAnimator.LinearColor(MaterialFrom.Ambient, MaterialTo.Ambient, t),
                ColorAnimator.LinearColor(MaterialFrom.Diffuse, MaterialTo.Diffuse, t),
                ColorAnimator.LinearColor(MaterialFrom.Specular, MaterialTo.Specular, t),
                ColorAnimator.LinearColor(MaterialFrom.Emission, MaterialTo.Emission, t),
                MaterialFrom.Shininess * (1 - t) + MaterialTo.Shininess * t,
                MaterialFrom.Translucent * (1 - t) + MaterialTo.Translucent * t);
        }
        private Material _FromMaterial = Materials.Chrome;
        /// <summary>
        /// the first material
        /// </summary>
        public Material FromMaterial
        {
            get { return _FromMaterial; }
            set { _FromMaterial = value; }
        }
        private Material _ToMaterial = Materials.Chrome;
        /// <summary>
        /// the second material
        /// </summary>
        public Material ToMaterial
        {
            get { return _ToMaterial; }
            set { _ToMaterial = value; }
        }

        Material _value = Materials.Chrome;
        /// <summary>
        /// gets the value of the interpolated material.
        /// </summary>
        public Material Value { get { return _value; } }
        /// <summary>
        /// overrides onAnimate and calculates the interpolated material. See <see cref="Value"/>.
        /// </summary>
        public override void OnAnimate()
        {
            if ((Duration > 0) /*&& (TimeParam<=1)*/)
                _value = LinearMaterial(FromMaterial, ToMaterial, TimeParam);
            if (_value.Equals(FromMaterial))
            { }
                base.OnAnimate();
            
        }
    }
}
