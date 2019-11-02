using System;

using System.Runtime.InteropServices;
namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        [Serializable]
        class WinApi
        {
             //static int WM_PAINT = 15;
             //static int WM_ERASEBKGND = 20;
             //static int WM_DRAWITEM = 43;
             //static int WM_QUIT = 18;
             //static int WM_CLOSE = 16;
             //static int WM_ACTIVATE = 6;
             //static int WM_TIMER = 49684;
             //static int WM_SIZE = 0x0005;
        }
         
   
        private IntPtr m_prevWndFunc;
        private delegate IntPtr WndProcCallBack(IntPtr hwnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WndProcCallBack wndProcCallBack);
        [DllImport("User32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr wndFunc);
        [DllImport("User32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int iMsg, IntPtr wParam, IntPtr lParam);

        private  bool TimerIgnore = false;
        private const int WM_MOUSEFIRST = 0x0200;
        private const int WM_MOUSELAST = 0x020A;
        private const int WM_NCMOUSEMOVE = 0x00A0;

       
        private void MouseBegin()
        {
            MouseAction = true;
        }
        private void MouseEnd()
        {
            MouseAction = false;
            if (RefreshMode == Mode.WhenMouseEvent)
            {
                Refresh();
              
                return;
            }
           
        }
      
        IntPtr SavewParam= IntPtr.Zero;
        IntPtr SavelParam= IntPtr.Zero;
        IntPtr SaveHWnd = IntPtr.Zero;
        private IntPtr _WndProc(IntPtr hWnd, int iMsg, IntPtr wParam, IntPtr lParam)
        {

          
            if ((iMsg > 0xc100) && (iMsg < 0xc300))
            {
                //49722
                if (TimerIgnore)
                {
                    wParam = (IntPtr)1;
                    return (IntPtr)1;
                }
            }
            //if (iMsg == 0x0112) // WM_SYSCOMMAND
            //{
            //    // Check your window state here
            //    if (wParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
            //    {
            //        _Maximized(null, null);
            //    }
            //}
            if ((iMsg == 0x0112) && ((int)wParam == 0xF120))
            {
                restore = true;
               // F_ResizeEnd(null, null);
            }
            if (iMsg == WM_NCMOUSEMOVE)
            {
                if (WinControl != null)
                {
                    int x = (short)lParam.ToInt32() - WinControl.Left;
                    int y = (lParam.ToInt32() >> 16) - WinControl.Top;
                    _NCMouseMove(new System.Drawing.Point(x, y));
                }
}
            if (iMsg == 792)
            { return (IntPtr)1; }
            if ((iMsg >= WM_MOUSEFIRST) && (iMsg <= WM_MOUSELAST))
            {
 
                MouseBegin();
                IntPtr Result = CallWindowProc(m_prevWndFunc, hWnd, iMsg, wParam, lParam);
                MouseEnd();
                return Result;
            }
            else
            {
             
              
                return CallWindowProc(m_prevWndFunc, hWnd, iMsg, wParam, lParam);
            }
        }
        private void AssignHandle()
        {
            m_wndProcCallBack = new WndProcCallBack(this._WndProc);
            m_prevWndFunc = SetWindowLong(WinControl .Handle, -4, m_wndProcCallBack);	// GWL_WNDPROC = -4
        }
        private WndProcCallBack m_wndProcCallBack;
      
        private void ReleaseHandle()
        {
            GC.Collect();
            SetWindowLong(WinControl.Handle, -4, m_prevWndFunc);	// GWL_WNDPROC = -4
        }
       
    }
    }

