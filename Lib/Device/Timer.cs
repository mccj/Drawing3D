using System;

using System.Windows.Forms;

namespace Drawing3d
{
    public partial class OpenGlDevice
    {   [NonSerialized]
        System.Timers.Timer _SystemTimer = null;
        private System.Timers.Timer SystemTimer
        {
            get { return _SystemTimer; }
           
        }
        /// <summary>
        /// gets and sets the repeating intervall, where the scene will be refreshed. See also <see cref="RefreshMode"/>.
        /// </summary>
       public long Intervall = 16;
  
        private void SetTimer()
        {
            if (Devices.Count == 1)
            {
                _SystemTimer = new System.Timers.Timer();
                SystemTimer.SynchronizingObject = WinControl;
                SystemTimer.Interval = Intervall;
                SystemTimer.Elapsed += Timer_Elapsed;
                SystemTimer.Enabled = true;
                SystemTimer.Start();
            }      
         }
        private void UnSetTimer()
        {
            if (SystemTimer != null)
            {
                SystemTimer.Stop();
                SystemTimer.Dispose();
                _SystemTimer = null;
            }
        }
        private void DoEvents()
        {
            TimerIgnore = true;
            Application.DoEvents();
            TimerIgnore = false;
        }

         private  void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           
             if (SystemTimer!=null)
            if ((OpenGlDevice.Devices != null) && (OpenGlDevice.Devices.Count > 0))
            {
                for (int i = 0; i < OpenGlDevice.Devices.Count; i++)
                {
                        if (OpenGlDevice.Devices[i].ErrorMsg == null)
                        {
                            
                            OpenGlDevice.Devices[i].RenderKind = Drawing3d.RenderKind.Render;
                            OpenGlDevice.Devices[i].Refresh();
                            OpenGlDevice.Devices[i].RenderKind = Drawing3d.RenderKind.None;

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(OpenGlDevice.Devices[i].ErrorMsg);
                            OpenGlDevice.Devices[i].ErrorMsg = null;
                        }
                }

                  GC.Collect(); //<--------------------------------------------------------------------
            }
            DoEvents();
        }
    }
}
