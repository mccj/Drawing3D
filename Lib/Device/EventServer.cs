using System;
using System.Collections.Generic;

namespace Drawing3d
{
    /// <summary>
    /// this class inherits from <b>MouseEventArgs</b> and add a field <b>Handled</b>.
    /// This field is used from the <see cref="EventServer"/>.It stops the distributation of this event.
    /// </summary>
    public class HandledMouseEventArgs : System.Windows.Forms.MouseEventArgs {
        /// <summary>
        /// if you set this field, the <see cref="EventHandler"/> stops the distributation of this event. 
        /// </summary>
        public bool Handled = false;
        /// <summary>
        /// a constructor with a <see cref="System.Windows.Forms.MouseEventArgs"/> as parameter.
        /// </summary>
        /// <param name="e"> see <see cref="System.Windows.Forms.MouseEventArgs"/></param>
        public HandledMouseEventArgs(System.Windows.Forms.MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        { }
   }
    


    /// <summary>
    /// Interface IEvent, which holds all mouse- and key events.
    /// </summary>

    public interface IEvent
    {

        /// <summary>
        /// called by mousedown.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>

        void OnMouseDown(HandledMouseEventArgs e);
        /// <summary>
        /// called by MouseUp.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>
        void OnMouseUp(HandledMouseEventArgs e);
        /// <summary>
        /// this event happens when the mouse wheeling ends.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>

        void OnMouseWheelEnd(HandledMouseEventArgs e);

        /// <summary>
        /// called by MouseMove.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>
        void OnMouseMove(HandledMouseEventArgs e);
        /// <summary>
        /// called by MouseDoubleClick.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>
         void OnMouseDoubleClick(HandledMouseEventArgs e);
        /// <summary>
        /// called by MouseWheel.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>
        void OnMouseWheel(HandledMouseEventArgs e);
        /// <summary>
        /// called by LongClick.
        /// </summary>
        /// <param name="e">a <see cref="HandledMouseEventArgs"/></param>
       void OnLongClick(HandledMouseEventArgs e);
        /// <summary>
        /// called by KeyDown.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
        void OnKeyDown(System.Windows.Forms.KeyEventArgs e);
        /// <summary>
        /// called by KeyPres.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
        void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e);
        /// <summary>
        /// called by KeyUp.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
        void OnKeyUp(System.Windows.Forms.KeyEventArgs e);

    }
    /// <summary>
    /// this class is responsible for the distributation of events:
    /// MouseDown, MouseDoubleClick, MouseWheel, MouseUp, MouseMove, KeyDown, KeyPress, KeyUp and LongClick.
    /// The eventserver is a list of <see cref="IEvent"/>. It calls every member. If a IEvent return true (handled), the server stops the calls in the list.
    /// </summary>
    [Serializable]
    public class EventServer:List<IEvent>
    { internal void  InsertAt(int Pos,IEvent Item)
        {
            base.Insert(Pos, Item);
            Added.Add(Item);
          
        }
        new internal void   Insert(int Pos, IEvent Item)
        {
            base.Insert(Pos, Item);
            Added.Add(Item);

        }
        
        internal new  void Add(IEvent Item)
        {
            base.Add( Item);
            Added.Add(Item);

        }
        internal List<IEvent> Added = new List<IEvent>();
        /// <summary>
        /// calls OnMouseDown of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        /// <returns>is true, if one member has returned true.</returns>
        public void MouseDown(HandledMouseEventArgs e)
       {  
          
            for (int i = 0; i <Count; i++)
            {
                if (this[i] != null)
                    this[i].OnMouseDown(e);
                if (e.Handled) return;
               
            };
          
            
       }
        /// <summary>
        /// calls OnMouseDoubleClick of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
        public void MouseDoubleClick(HandledMouseEventArgs e)
       {
          
           for (int i = 0; i < Count; i++)
           {
                if (this[i] != null)
                {
                    this[i].OnMouseDoubleClick(e);
                    if (e.Handled) return;
                }
               
           };
           

       }
        /// <summary>
        /// calls OnMouseWheel of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
        public void MouseWheel(HandledMouseEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i].OnMouseWheel(e);
                    if (e.Handled) return;
                }

            };


        }

        /// <summary>
        /// this event happens when the mouse wheeling ends.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
       public void MouseWheelEnd(HandledMouseEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i].OnMouseWheelEnd(e);
                    if (e.Handled) return;
                }

            };


        }

        /// <summary>
        /// calls OnMouseUp of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
        public void MouseUp(HandledMouseEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i].OnMouseUp(e);
                    if (e.Handled) return;
                }

            };


        }






        /// <summary>
        /// calls OnMouseMove of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
        public void MouseMove(HandledMouseEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i].OnMouseMove(e);
                    if (e.Handled) return;
                }

            };


        }



        /// <summary>
        /// calls OnKeyDown of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
        public void KeyDown(System.Windows.Forms.KeyEventArgs e)
       {
          
           for (int i = 0; i < Count; i++)
           {

                if (this[i] != null)
                { this[i].OnKeyDown(e);
                    if (e.Handled) return;
                }
              
           };
          
       }
        /// <summary>
        /// calls OnKeyPress of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
         public void KeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {

                if (this[i] != null)
                {
                    this[i].OnKeyPress(e);
                    if (e.Handled) return;
                }

            };

        }



        /// <summary>
        /// calls OnKeyUp of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e">KeyEventArg</param>
 
        public void KeyUp(System.Windows.Forms.KeyEventArgs e)
        {

            for (int i = 0; i < Count; i++)
            {

                if (this[i] != null)
                {
                    this[i].OnKeyUp(e);
                    if (e.Handled) return;
                }

            };

        }

        /// <summary>
        /// calls OnLongClick of all members and stops, if a member returns true. See alse <see cref="IEvent"/>.
        /// </summary>
        /// <param name="e"><see cref="HandledMouseEventArgs"/></param>
        public void LongClick(HandledMouseEventArgs e)
        {

          
            for (int i = 0; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i].OnLongClick(e);
                    if (e.Handled) return;
          
                }
            };
           

        }
    }
}
