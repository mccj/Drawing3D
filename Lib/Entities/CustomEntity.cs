using System;
using System.Collections.Generic;

using System.Collections;
namespace Drawing3d
{
    /// <summary>
    /// the proper baseclass, but your class should <b>allways</b> inherit from <see cref="Entity"/>
    /// </summary>
    [Serializable]
    public class CustomEntity
    {
        /// <summary>
        /// an <see cref="DrawEvent"/>, which is called by the <see cref="OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        [field: NonSerialized]
        public event DrawEvent Draw;
        /// <summary>
        /// is a definition of an event for drawing. See e.g <see cref="CustomEntity.Draw"/>.
        /// </summary>
        /// <param name="Sender">is a <see cref="Entity"/></param>
        /// <param name="Device">Device in which it will be drawn.</param>
        public delegate void DrawEvent(CustomEntity Sender, OpenGlDevice Device);
        //public  EventServer E = new EventServer();
     
        //public event DrawEvent Draw;
        // static CustomEntity _CurrentEntity = null;
        /// <summary>
        /// the current entity, which will be drawn.
        /// </summary>
  


        public CustomEntity()
        { }
        /// <summary>
        /// is the most important method of this class. You have to override this Method.
        /// To call method you have to call <see cref="Entity.Paint(OpenGlDevice)"/> or you add <b>this</b> to the
        /// <see cref="Entity.Children"/> of an entity and call the paint method of any <b>parent</b> ancestor.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice" /> in which will be drawn.</param>
        protected virtual void OnDraw(OpenGlDevice Device)
        {
            if (Draw != null)  Draw(this, Device);
            
        }
      
        internal void _Ondraw(OpenGlDevice Device)
        {
          
            OnDraw(Device);
     
            OpenGlDevice.CheckError();

        }

    }
}


