using System;
namespace Drawing3d
{
    /// <summary>
    /// is an <see cref="Entity"/> which draws a <b>sphere</b>.
    /// </summary>
    [Serializable]
    public class SphereEntity:Entity
    {
        xyz _Center = new xyz(0, 0, 0);
        /// <summary>
        /// Center of the <b>sphere</b>. The default is 0,0,0.
        /// </summary>
        public xyz Center
        {    
            get { return _Center; }
            set
            {  
                _Center = value; 
            }
        }
        double _Radius = 1;
        /// <summary>
        /// is the radius of the sphere.
        /// </summary>
        public double Radius
        {
            get { return _Radius; }
            set
            {
                _Radius = value;
 
            }
        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public SphereEntity() : base()
        {

        }
        /// <summary>
        /// is a constructor with <b>center</b> and <b>radius</b>.
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="Radius"></param>
        public SphereEntity(xyz Center, double Radius):base()
        {
            this.Center = Center;
            this.Radius = Radius;
        }
       /// <summary>
       /// overrides the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
       /// </summary>
       /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {  
            Device.drawSphere(Center, Radius);
            base.OnDraw(Device);
        }
    }
}
