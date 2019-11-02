using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
namespace Drawing3d
{
    /// <summary>
    /// an interface for the undo-redo mechanism
    /// </summary>
    public interface IUndo
    {
       
        /// <summary>
        /// the related method should handle a desire for undoing the last step, which is made from <b>undo</b>.
        /// </summary>
        void ReDo();
        /// <summary>
        /// the related method should handle a desire for redoing the last step.
        /// </summary>
        void UnDo();
    }
 
    /// <summary>
    /// is a definition of a handler with a boolean result.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void MouseEventHandler(object sender, HandledMouseEventArgs e);
    /// <summary>
    /// is a definition of a handler with a boolean result.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    /// <summary>
    /// is a definition of a handler with a boolean result.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);
    /// <summary>
    /// implments the <see cref="IEvent"/>s, which are needed from the <see cref="OpenGlDevice.EventServer"/>.
    /// it is the base element for all object, wich needs mouse- and key events.
    /// </summary>
    [Serializable]

    public class CtrlEntity : Entity, IEvent, IUndo
    {
        Base _Base = new Base(true);
        /// <summary>
        /// gets and sets the base. See also <see cref="Currentxyz"/>
        /// </summary>
        [Browsable(false)]
        public Base Base
        {
            get { return _Base; }
            set { _Base = value;
                 }
        }

        /// <summary>
        /// indicates, whether the cursor is over the object. See also <see cref="OnEnterEvent"/>
        /// </summary>
        [NonSerialized]
        public bool Entered = false;
        /// <summary>
        /// gives the mouse position relative to the <see cref="Base"/>
        /// </summary>
        [Browsable(false)]
        public xyz Currentxyz
        {
            get
            {
               
                
                xyz R = Base.Relativ(Device.Currentxyz);
                return Base.Relativ(Device.Currentxyz);
            }
        }
        /// <summary>
        /// is a event, wich is called from <see cref="OnLogout"/>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler LogOut;
        /// <summary>
        /// is a event, wich is called from <see cref="OnLogin(OpenGlDevice, int)"/>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler LogIn;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseUp(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseUp;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseDown(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseDown;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseMove(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseMove;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseWheelEnd(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseWheelEnd;
        /// <summary>
        /// is a event, wich is called from <see cref="OnKeyDown(KeyEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// is a event, wich is called from <see cref="OnKeyPress(KeyPressEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event KeyPressEventHandler KeyPress;
        /// <summary>
        /// is a event, wich is called from <see cref="OnKeyUp(KeyEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event KeyEventHandler KeyUp;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseWheel(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseWheel;
        /// <summary>
        /// is a event, wich is called from <see cref="OnMouseWheel(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler MouseDoubleClick;
        /// <summary>
        /// is a event, wich is called from <see cref="OnLongClick(HandledMouseEventArgs)"/>
        /// </summary>
        [field: NonSerialized]
        public event MouseEventHandler LongClick;
       
        /// <summary>
        /// is allways called if <see cref="Currentxyz"/> is set.
        /// </summary>

        /// <summary>
        /// call, when you want to refresh the Propertygrid.
        /// </summary>
        public void Changed()
        {
            //if (PropertyGridEnable)
            //PropertyGrid.Refresh();
        }
        /// <summary>
        /// definition of an event, which has a boolean result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate bool BoolEventHandler(object sender, EventArgs e);
        /// <summary>
        /// will be called, when the cursor is over the object of the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        protected virtual void OnEnterEvent()
        {
            Entered = true;
        }


        /// <summary>
        /// will be called, when the cursor is not over the object of the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        protected virtual void OnOutEvent()
        {
            Entered = false;
        }
        SnappItem Inside()
        {
            SnappItem Result = null;
            for (int j = 0; j < Device.SnappItems.Count; j++)
            {
                if (Device.SnappItems[j].Object == this)
                {
                    Result = Device.SnappItems[j];
                    return Result;

                }
            }
            return Result;

        }
        /// <summary>
        /// freeze the <see cref="Entity"/>
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
      
      
        /// <summary>
        /// is a <see cref="EventxyzHandler"/>, which is called from the <see cref="Selector"/>, when he sets the <see cref="OpenGlDevice.Currentxyz"/> posistion from the snapped graphical object.
        /// </summary>
        [NonSerialized]
        protected EventxyzHandler SetXYZ = null;
        /// <summary>
        /// happens, when the object is logged out. See also <see cref="OpenGlDevice.EventServer"/>.
        /// </summary>
        public virtual void OnLogout(bool KeepTheDevice)
        {
            SetDevice(null, 0);
           
            if (LogOut != null) LogOut(this, new EventArgs());

        }
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public CtrlEntity()
        {
            CompileEnable = false;
        }
         /// <summary>
         /// overrides <see cref="Entity.Clone"/> without the field <see cref="Device"/>.
         /// </summary>
         /// <returns></returns>
        public override Entity Clone()
        {
            OpenGlDevice O = null;
            if (Device != null)
            {
                O = Device;
               
                _Device = null;
            }
            Entity E = this.Clone() as Entity;
            _Device = O;
          
            return E;
        }
        /// <summary>
        /// loged in the object. See also <see cref="OpenGlDevice.EventServer"/> 
        /// </summary>
        /// <param name="Device">is the used device.</param>
        /// <param name="EventPosition">is the position in the eventserver, where the object is positioned.</param>
        public virtual void OnLogin(OpenGlDevice Device, int EventPosition)
        {
          
            SetDevice(Device, EventPosition);
            if (LogIn != null) LogIn(this, new EventArgs());
            
        }
        /// <summary>
        /// sets the device and login automatical in the <see cref="OpenGlDevice.EventServer"/>.
        /// </summary>
        /// <param name="Device">the used device</param>
        /// <param name="EventPosition">the position in the eventServer. Use -1 if the object will be addded to eventserver.</param>
        protected virtual void SetDevice(OpenGlDevice Device, int EventPosition)
        {

            if (Device == null)
            {
                if (_Device != null)
                {
                    _Device.EventServer.Remove(this);
                    _Device.Selector.SetCurrentXYZ -= SetXYZ;
                    _Device.Controls.Remove(this);
                    _Device = null;
                }
                return;
            }
            _Device = Device;
            if (Device.Controls.IndexOf(this) < 0) Device.Controls.Add(this);
            int id = Device.EventServer.IndexOf(this);
            if (id >= 0)

            {
                if (id != EventPosition)
                {
                    Device.EventServer.Remove(this);
                    if (EventPosition >= 0)
                        Device.EventServer.Insert(EventPosition, this);
                    else
                        Device.EventServer.Add(this);
                }
            }
            else
            {
                Device.EventServer.Insert(EventPosition, this);
                if (SetXYZ == null) SetXYZ = new EventxyzHandler(xyzHandler);
                Device.Selector.SetCurrentXYZ += SetXYZ;
                Device.Selector.CreateSnappList();
              
                Changed();
            }
           
        }
        /// <summary>
        /// a construcor which have a <see cref="OpenGlDevice"/>. See also <see cref="Device"/>
        /// </summary>
        /// <param name="Device"></param>
        public CtrlEntity(OpenGlDevice Device) : this()
        {
            this.Device = Device;
        }
        
        /// <summary>
        /// a construcor which have a <see cref="CtrlEntity.Base"/> and a <see cref="OpenGlDevice"/>. See also <see cref="Device"/>
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Device"></param>
        public CtrlEntity(Base Base, OpenGlDevice Device) : base()
        {
            this.Device = Device;
            this.Base = Base;
        }
        private void PropertyGrid_Disposed(object sender, EventArgs e)
        {


        }

        void xyzHandler(object sender, LineType ViewLine, xyz Point)
        {

            if (Device == null) return;
            LineType LT = new LineType(Device.Currentxyz, Device.getProjectionBase().BaseZ);
            Plane B = new Plane(Base.BaseO, Base.BaseZ);
            double Lam = -1;
            xyz PT = new xyz(0, 0, 0);
            B.Cross(LT, out Lam, out PT);
            PT = Base.Relativ(PT);

        }
      
        /// <summary>
        /// for the use of the redo mechanism.See also <see cref="UnDo"/>.
        /// </summary>
        public virtual void ReDo()
        {

        }
        /// <summary>
        /// for the use of the undo mechanism.See also <see cref="ReDo"/>.
        /// </summary>
        public virtual void UnDo()
        {

        }

        OpenGlDevice __Device;
        OpenGlDevice _Device
        {
            get { return __Device; }
            set { __Device = value;
               }
        }
        /// <summary>
        /// if you set a Device not null then automatically the object will be inserted at position 0.
        /// For severall method is a device necessary. See e.g. <see cref="Currentxyz"/>.
        /// </summary>
        [Browsable(false)]
        public OpenGlDevice Device
        {
            set
            {

                SetDevice(value, 0);
                SetInvalid(true);
            }
            get
            {
                return _Device;
            }
        }
        /// <summary>
        /// is call by a double click of the mouse. See also <see cref="MouseDoubleClick"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>
        public void OnMouseDoubleClick(HandledMouseEventArgs e)
        {
            MouseDoubleClick?.Invoke(this, e);

        }
        [NonSerialized]
        MouseEventArgs Save = null;
        /// <summary>
        /// is call by wheel with the mouse. See also <see cref="MouseWheel"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnMouseWheel(HandledMouseEventArgs e)
        {
            Save = e;
            MouseWheel?.Invoke(this, e);
           
        }
        /// <summary>
        /// is call by a  mouse down. See also <see cref="MouseDown"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnMouseDown(HandledMouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);

        }
        /// <summary>
        /// is call by a  mouse up. See also <see cref="MouseUp"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnMouseUp(HandledMouseEventArgs e)
        {
            if (MouseUp != null) MouseUp(this, e);
            
        }
          /// <summary>
        /// is called when a wheel is finished. See also <see cref="MouseWheel"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns>interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled".</returns>
        public virtual void OnMouseWheelEnd(HandledMouseEventArgs e)
        {

            if (MouseWheelEnd != null) MouseWheelEnd(this, e);
            
        }
        /// <summary>
        /// is call when by a mousemove. See also <see cref="MouseMove"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled".</returns>

        public virtual void OnMouseMove(HandledMouseEventArgs e)
        {
            if (Device.SnappItems.Count > 0)
            {
                if (Inside() != null)
                    OnEnterEvent();
                else
                    OnOutEvent();

            }

            if (MouseMove != null)  MouseMove(this, e);
           
        }
        /// <summary>
        /// is call when a key is pressed down. See also <see cref="KeyDown"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyDown != null) KeyDown(this, e);
           
        }
        /// <summary>
        /// is call when a key is pressed. See also <see cref="KeyPress"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);

        }
        /// <summary>
        /// is call when a key let loose. See also <see cref="KeyUp"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>

        public virtual void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);

        }
        /// <summary>
        /// is called when a click is longer than 500 millisecond. See also <see cref="LongClick"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns><b>true</b> interupt the <see cref="OpenGlDevice.EventServer"/> in the sense of "handled". </returns>
        public virtual void OnLongClick(HandledMouseEventArgs e)
        {

            LongClick?.Invoke(this, e);
           
        }

       
    }
}


