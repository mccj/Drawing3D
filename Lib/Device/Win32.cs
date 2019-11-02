using System;
using Drawing3d;

using System.Runtime.InteropServices;
using System.Security;
using System.Drawing;
using System.Collections;

namespace Drawing3d.Windows
{
    /// <summary>
    /// Zusammenfassung für FeaturesW32.
    /// </summary>
    [Serializable]
    public class FeaturesW32
	{
		#region string USER_NATIVE_LIBRARY
		/// <summary>
		///     Specifies User32's native library archive.
		/// </summary>
		/// <remarks>
		///     Specifies user32.dll for Windows.
		/// </remarks>
		private const string USER_NATIVE_LIBRARY = "user32.dll";
		#endregion string USER_NATIVE_LIBRARY
		private const string WGL_NATIVE_LIBRARY = "opengl32.dll";
		#region string GDI_NATIVE_LIBRARY
		/// <summary>
		///     Specifies GDI's native library archive.
		/// </summary>
		/// <remarks>
		///     Specifies gdi32.dll for Windows.
		/// </remarks>
		private const string GDI_NATIVE_LIBRARY = "gdi32.dll";
		#endregion string GDI_NATIVE_LIBRARY

		private const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;
		#region Public Constants
		#region wglSwapLayerBuffers Flags
		#region int WGL_SWAP_MAIN_PLANE
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the main plane.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_MAIN_PLANE     0x00000001
		public const int WGL_SWAP_MAIN_PLANE = 0x00000001;
		#endregion int WGL_SWAP_MAIN_PLANE

		#region int WGL_SWAP_OVERLAY1
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 1.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY1       0x00000002
		public const int WGL_SWAP_OVERLAY1 = 0x00000002;
		#endregion int WGL_SWAP_OVERLAY1

		#region int WGL_SWAP_OVERLAY2
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 2.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY2       0x00000004
		public const int WGL_SWAP_OVERLAY2 = 0x00000004;
		#endregion int WGL_SWAP_OVERLAY2

		#region int WGL_SWAP_OVERLAY3
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 3.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY3       0x00000008
		public const int WGL_SWAP_OVERLAY3 = 0x00000008;
		#endregion int WGL_SWAP_OVERLAY3

		#region int WGL_SWAP_OVERLAY4
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 4.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY4       0x00000010
		public const int WGL_SWAP_OVERLAY4 = 0x00000010;
		#endregion int WGL_SWAP_OVERLAY4

		#region int WGL_SWAP_OVERLAY5
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 5.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY5       0x00000020
		public const int WGL_SWAP_OVERLAY5 = 0x00000020;
		#endregion int WGL_SWAP_OVERLAY5

		#region int WGL_SWAP_OVERLAY1
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 6.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY6       0x00000040
		public const int WGL_SWAP_OVERLAY6 = 0x00000040;
		#endregion int WGL_SWAP_OVERLAY6

		#region int WGL_SWAP_OVERLAY7
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 7.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY7       0x00000080
		public const int WGL_SWAP_OVERLAY7 = 0x00000080;
		#endregion int WGL_SWAP_OVERLAY7

		#region int WGL_SWAP_OVERLAY8
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 8.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY8       0x00000100
		public const int WGL_SWAP_OVERLAY8 = 0x00000100;
		#endregion int WGL_SWAP_OVERLAY8

		#region int WGL_SWAP_OVERLAY9
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 9.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY9       0x00000200
		public const int WGL_SWAP_OVERLAY9 = 0x00000200;
		#endregion int WGL_SWAP_OVERLAY9

		#region int WGL_SWAP_OVERLAY10
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 10.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY10      0x00000400
		public const int WGL_SWAP_OVERLAY10 = 0x00000400;
		#endregion int WGL_SWAP_OVERLAY10

		#region int WGL_SWAP_OVERLAY11
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 11.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY11      0x00000800
		public const int WGL_SWAP_OVERLAY11 = 0x00000800;
		#endregion int WGL_SWAP_OVERLAY11

		#region int WGL_SWAP_OVERLAY12
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 12.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY12      0x00001000
		public const int WGL_SWAP_OVERLAY12 = 0x00001000;
		#endregion int WGL_SWAP_OVERLAY12

		#region int WGL_SWAP_OVERLAY13
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 13.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY13      0x00002000
		public const int WGL_SWAP_OVERLAY13 = 0x00002000;
		#endregion int WGL_SWAP_OVERLAY13

		#region int WGL_SWAP_OVERLAY14
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 14.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY14      0x00004000
		public const int WGL_SWAP_OVERLAY14 = 0x00004000;
		#endregion int WGL_SWAP_OVERLAY14

		#region int WGL_SWAP_OVERLAY15
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the overlay plane 15.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_OVERLAY15      0x00008000
		public const int WGL_SWAP_OVERLAY15 = 0x00008000;
		#endregion int WGL_SWAP_OVERLAY15

		#region int WGL_SWAP_UNDERLAY1
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 1.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY1      0x00010000
		public const int WGL_SWAP_UNDERLAY1 = 0x00010000;
		#endregion int WGL_SWAP_UNDERLAY1

		#region int WGL_SWAP_UNDERLAY2
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 2.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY2      0x00020000
		public const int WGL_SWAP_UNDERLAY2 = 0x00020000;
		#endregion int WGL_SWAP_UNDERLAY2

		#region int WGL_SWAP_UNDERLAY3
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 3.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY3      0x00040000
		public const int WGL_SWAP_UNDERLAY3 = 0x00040000;
		#endregion int WGL_SWAP_UNDERLAY3

		#region int WGL_SWAP_UNDERLAY4
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 4.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY4      0x00080000
		public const int WGL_SWAP_UNDERLAY4 = 0x00080000;
		#endregion int WGL_SWAP_UNDERLAY4

		#region int WGL_SWAP_UNDERLAY5
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 5.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY5      0x00100000
		public const int WGL_SWAP_UNDERLAY5 = 0x00100000;
		#endregion int WGL_SWAP_UNDERLAY5

		#region int WGL_SWAP_UNDERLAY6
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 6.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY6      0x00200000
		public const int WGL_SWAP_UNDERLAY6 = 0x00200000;
		#endregion int WGL_SWAP_UNDERLAY6

		#region int WGL_SWAP_UNDERLAY7
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 7.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY7      0x00400000
		public const int WGL_SWAP_UNDERLAY7 = 0x00400000;
		#endregion int WGL_SWAP_UNDERLAY7

		#region int WGL_SWAP_UNDERLAY8
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 8.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY8      0x00800000
		public const int WGL_SWAP_UNDERLAY8 = 0x00800000;
		#endregion int WGL_SWAP_UNDERLAY8

		#region int WGL_SWAP_UNDERLAY9
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 9.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY9      0x01000000
		public const int WGL_SWAP_UNDERLAY9 = 0x01000000;
		#endregion int WGL_SWAP_UNDERLAY9

		#region int WGL_SWAP_UNDERLAY10
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 10.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY10     0x02000000
		public const int WGL_SWAP_UNDERLAY10 = 0x02000000;
		#endregion int WGL_SWAP_UNDERLAY10

		#region int WGL_SWAP_UNDERLAY11
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 11.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY11     0x04000000
		public const int WGL_SWAP_UNDERLAY11 = 0x04000000;
		#endregion int WGL_SWAP_UNDERLAY11

		#region int WGL_SWAP_UNDERLAY12
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 12.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY12     0x08000000
		public const int WGL_SWAP_UNDERLAY12 = 0x08000000;
		#endregion int WGL_SWAP_UNDERLAY12

		#region int WGL_SWAP_UNDERLAY13
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 13.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY13     0x10000000
		public const int WGL_SWAP_UNDERLAY13 = 0x10000000;
		#endregion int WGL_SWAP_UNDERLAY13

		#region int WGL_SWAP_UNDERLAY14
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 14.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY14     0x20000000
		public const int WGL_SWAP_UNDERLAY14 = 0x20000000;
		#endregion int WGL_SWAP_UNDERLAY14

		#region int WGL_SWAP_UNDERLAY15
		/// <summary>
		///     <para>
		///         Swaps the front and back buffers of the underlay plane 15.
		///     </para>
		/// </summary>
		// #define WGL_SWAP_UNDERLAY15     0x40000000
		public const int WGL_SWAP_UNDERLAY15 = 0x40000000;
		#endregion int WGL_SWAP_UNDERLAY15
		#endregion wglSwapLayerBuffers Flags
		#region bool wglSwapLayerBuffers(IntPtr deviceContext, int planes)
		/// <summary>
		///     <para>
		///         The <b>wglSwapLayerBuffers</b> function swaps the front and back buffers in
		///         the overlay, underlay, and main planes of the window referenced by a
		///         specified device context.
		///     </para>
		/// </summary>
		/// <param name="deviceContext">
		///     <para>
		///         Specifies the device context of a window whose layer plane palette is to be
		///         realized into the physical palette.
		///     </para>
		/// </param>
		/// <param name="planes">
		///     <para>
		///         Specifies the overlay, underlay, and main planes whose front and back buffers
		///         are to be swapped.  The <b>bReserved</b> member of the
		///         <see cref="FeaturesW32.PIXELFORMATDESCRIPTOR" /> structure specifies the number of
		///         overlay and underlay planes.  The <i>planes</i> parameter is a bitwise
		///         combination of the following values:
		///     </para>
		///     <para>
		///         <list type="table">
		///             <listheader>
		///                 <term>Value</term>
		///                 <description>Meaning</description>
		///             </listheader>
		///             <item>
		///                 <term>WGL_SWAP_MAIN_PLANE</term>
		///                 <description>
		///                     Swaps the front and back buffers of the main plane.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>WGL_SWAP_OVERLAYi</term>
		///                 <description>
		///                     Swaps the front and back buffers of the overlay plane i, where
		///                     i is an integer between 1 and 15.  WGL_SWAP_OVERLAY1 identifies
		///                     the first overlay plane over the main plane, WGL_SWAP_OVERLAY2
		///                     identifies the second overlay plane over the first overlay plane,
		///                     and so on.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>WGL_SWAP_UNDERLAYi</term>
		///                 <description>
		///                     Swaps the front and back buffers of the underlay plane i, where i
		///                     is an integer between 1 and 15.  WGL_SWAP_UNDERLAY1 identifies
		///                     the first underlay plane under the main plane, WGL_SWAP_UNDERLAY2
		///                     identifies the second underlay plane under the first underlay
		///                     plane, and so on.
		///                 </description>
		///             </item>
		///         </list>
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         If the function succeeds, the return value is true.  If the function fails,
		///         the return value is false.  To get extended error information, call
		///         <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         When a layer plane doesn't include a back buffer, calling the
		///         <b>wglSwapLayerBuffers</b> function has no effect on that layer plane.  After
		///         you call <b>wglSwapLayerBuffers</b>, the state of the back buffer content is
		///         given in the corresponding /* see cref="Gdi.LAYERPLANEDESCRIPTOR" /> */ structure
		///         of the layer plane or in the <see cref="FeaturesW32.PIXELFORMATDESCRIPTOR" />
		///         structure of the main plane.  The <b>wglSwapLayerBuffers</b> function swaps
		///         the front and back buffers in the specified layer planes simultaneously.
		///     </para>
		///     <para>
		///         Some devices don't support swapping layer planes individually; they swap all
		///         layer planes as a group.  When the <b>PFD_SWAP_LAYER_BUFFERS</b> flag of the
		///         <see cref="FeaturesW32.PIXELFORMATDESCRIPTOR" /> structure is set, it indicates that
		///         a device can swap individual layer planes and that you can call
		///         <b>wglSwapLayerBuffers</b>.
		///     </para>
		///     <para>
		///         With applications that use multiple threads, before calling
		///         <b>wglSwapLayerBuffers</b>, clear all drawing commands in all threads drawing
		///         to the same window.
		///     </para>
		/// </remarks>
		/// /* seealso cref="Gdi.LAYERPLANEDESCRIPTOR" />*/
		/// <seealso cref="FeaturesW32.PIXELFORMATDESCRIPTOR" />
		/// <seealso cref="SwapBuffers" />
		// WINGDIAPI BOOL  WINAPI wglSwapLayerBuffers(HDC, UINT);
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		private static extern bool wglSwapLayerBuffers(IntPtr deviceContext, int planes);
		#endregion bool wglSwapLayerBuffers(IntPtr deviceContext, int planes)
      //  [DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
        /// <summary>
        /// Calls <see cref="wglSwapLayerBuffers(IntPtr,int)"/> with planes=<see cref="WGL_SWAP_MAIN_PLANE"/>.
        /// </summary>
        /// <param name="deviceContext">A created devicecontext</param>
        /// <returns></returns>
        [DllImport("opengl32", EntryPoint = "wglSwapBuffers")]
		public static extern bool wglSwapBuffers(IntPtr deviceContext);
	
		#region wglUseFontOutlines Formats
		#region int WGL_FONT_LINES
		/// <summary>
		///     <para>
		///         Fonts with line segments.
		///     </para>
		/// </summary>
		// #define WGL_FONT_LINES      0
		public const int WGL_FONT_LINES = 0;
		#endregion int WGL_FONT_LINES

		#region int WGL_FONT_POLYGONS
		/// <summary>
		///     <para>
		///         Fonts with polygons.
		///     </para>
		/// </summary>
		// #define WGL_FONT_POLYGONS   1
		public const int WGL_FONT_POLYGONS = 1;
		#endregion int WGL_FONT_POLYGONS
		#endregion wglUseFontOutlines Formats
		#endregion Public Constants

		#region bool wglMakeCurrent(IntPtr deviceContext, IntPtr renderingContext)
		/// <summary>
		///     <para>
		///         The <b>wglMakeCurrent</b> function makes a specified OpenGL rendering context
		///         the calling thread's current rendering context.  All subsequent OpenGL calls
		///         made by the thread are drawn on the device identified by <i>deviceContext</i>.
		///         You can also use <b>wglMakeCurrent</b> to change the calling thread's current
		///         rendering context so it's no longer current.
		///     </para>
		/// </summary>
		/// <param name="deviceContext">
		///     <para>
		///         Handle to a device context.  Subsequent OpenGL calls made by the calling
		///         thread are drawn on the device identified by <i>deviceContext</i>.
		///     </para>
		/// </param>
		/// <param name="renderingContext">
		///     <para>
		///         Handle to an OpenGL rendering context that the function sets as the calling
		///         thread's rendering context.
		///     </para>
		///     <para>
		///         If <i>rendingContext</i> is <see cref="IntPtr.Zero" />, the function makes
		///         the calling thread's current rendering context no longer current, and
		///         releases the device context that is used by the rendering context.  In this
		///         case, <i>deviceContext</i> is ignored.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         When the <b>wglMakeCurrent</b> function succeeds, the return value is true;
		///         otherwise the return value is false.  To get extended error information,
		///         call <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The <i>deviceContext</i> parameter must refer to a drawing surface supported
		///         by OpenGL.  It need not be the same <i>deviceContext</i> that was passed to
		///         <see cref="wglCreateContext" /> when <i>renderingContext</i> was created, but
		///         it must be on the same device and have the same pixel format.  GDI
		///         transformation and clipping in <i>deviceContext</i> are not supported by the
		///         rendering context.  The current rendering context uses the
		///         <i>deviceContext</i> device context until the rendering context is no longer
		///         current.
		///     </para>
		///     <para>
		///         Before switching to the new rendering context, OpenGL flushes any previous
		///         rendering context that was current to the calling thread.
		///     </para>
		///     <para>
		///         A thread can have one current rendering context.  A process can have multiple
		///         rendering contexts by means of multithreading.  A thread must set a current
		///         rendering context before calling any OpenGL functions.  Otherwise, all OpenGL
		///         calls are ignored.
		///     </para>
		///     <para>
		///         A rendering context can be current to only one thread at a time.  You cannot
		///         make a rendering context current to multiple threads.
		///     </para>
		///     <para>
		///         An application can perform multithread drawing by making different rendering
		///         contexts current to different threads, supplying each thread with its own
		///         rendering context and device context.
		///     </para>
		///     <para>
		///         If an error occurs, the <b>wglMakeCurrent</b> function makes the thread's
		///         current rendering context not current before returning.
		///     </para>
		/// </remarks>
		/// <seealso cref="wglCreateContext" />
		/// <seealso cref="wglDeleteContext" />
		/// <seealso cref="wglGetCurrentContext" />
		
		// WINGDIAPI BOOL WINAPI wglMakeCurrent(HDC, HGLRC);
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern bool wglMakeCurrent(IntPtr deviceContext, IntPtr renderingContext);
		#endregion bool wglMakeCurrent(IntPtr deviceContext, IntPtr renderingContext)
		#region IntPtr wglCreateContext(IntPtr deviceContext)
		/// <summary>
		///     <para>
		///         The <b>wglCreateContext</b> function creates a new OpenGL rendering context,
		///         which is suitable for drawing on the device referenced by
		///         <i>deviceContext</i>.  The rendering context has the same pixel format as
		///         the device context.
		///     </para>
		/// </summary>
		/// <param name="deviceContext">
		///     <para>
		///         Handle to a device context for which the function creates a suitable OpenGL
		///         rendering context.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         If the function succeeds, the return value is a valid handle to an OpenGL
		///         rendering context.
		///     </para>
		///     <para>
		///         If the function fails, the return value is <see cref="IntPtr.Zero"/>.  To get
		///         extended error information, call <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         A rendering context is not the same as a device context.  Set the pixel
		///         format of the device context before creating a rendering context.  For more
		///         information on setting the device context's pixel format, see the
		///         <see cref="SetPixelFormat" /> function.
		///     </para>
		///     <para>
		///         To use OpenGL, you create a rendering context, select it as a thread's
		///         current rendering context, and then call OpenGL functions.  When you are
		///         finished with the rendering context, you dispose of it by calling the
		///         <see cref="wglDeleteContext" /> function.
		///     </para>
		///     <para>
		///         The following code example shows <b>wglCreateContext</b> usage:
		///     </para>
		///     <para>
		///         <code>
		///             IntPtr hdc;
		///             IntPtr hglrc;
		///
		///             // create a device context
		///
		///             // create a rendering context
		///             hglrc = Wgl.wglCreateContext(hdc);
		///
		///             // make it the calling thread's current rendering context
		///             Wgl.wglMakeCurrent(hdc, hglrc);
		///
		///             // call OpenGL APIs as desired...
		///
		///             // when the rendering context is no longer needed...
		///
		///             // make the rendering context not current
		///             Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
		///
		///             // delete the rendering context
		///             Wgl.wglDeleteContext(hglrc);
		///         </code>
		///     </para>
		/// </remarks>
		/// <seealso cref="SetPixelFormat" />
		/// <seealso cref="wglDeleteContext" />
		/// <seealso cref="wglGetCurrentContext" />
		/// <seealso cref="wglMakeCurrent" />
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr
            wglCreateContext(IntPtr deviceContext);
		#endregion IntPtr wglCreateContext(IntPtr deviceContext)
		#region bool wglDeleteContext(IntPtr renderingContext)
		/// <summary>
		///     <para>
		///         The <b>wglDeleteContext</b> function deletes a specified OpenGL rendering
		///         context.
		///     </para>
		/// </summary>
		/// <param name="renderingContext">
		///     <para>
		///         Handle to an OpenGL rendering context that the function will delete.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         If the function succeeds, the return value is true.
		///     </para>
		///     <para>
		///         If the function fails, the return value is false.  To get extended error
		///         information, call <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         It is an error to delete an OpenGL rendering context that is the current
		///         context of another thread.  However, if a rendering context is the calling
		///         thread's current context, the <b>wglDeleteContext</b> function changes the
		///         rendering context to being not current before deleting it.
		///     </para>
		///     <para>
		///         The <b>wglDeleteContext</b> function does not delete the device context
		///         associated with the OpenGL rendering context when you call the
		///         <b>wglMakeCurrent</b> function.
		///     </para>
		/// </remarks>
		/// <seealso cref="wglCreateContext" />
		/// <seealso cref="wglGetCurrentContext" />
		/// <seealso cref="wglMakeCurrent" />
		// WINGDIAPI BOOL WINAPI wglDeleteContext(HGLRC);
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern bool wglDeleteContext(IntPtr renderingContext);
		#endregion bool wglDeleteContext(IntPtr renderingContext)
		#region bool wglUseFontBitmaps(IntPtr deviceContext, int first, int count, int listBase)
		/// <summary>
		///     <para>
		///         The <b>wglUseFontBitmaps</b> function creates a set of bitmap display lists
		///         for use in the current OpenGL rendering context.  The set of bitmap display
		///         lists is based on the glyphs in the currently selected font in the device
		///         context.  You can then use bitmaps to draw characters in an OpenGL image.
		///     </para>
		///     <para>
		///         The <b>wglUseFontBitmaps</b> function creates <i>count</i> display lists,
		///         one for each of a run of <i>count</i> glyphs that begins with the
		///         <i>first</i> glyph in the <i>deviceContext</i> parameter's selected fonts.
		///     </para>
		/// </summary>
		/// <param name="deviceContext">
		///     <para>
		///         Specifies the device context whose currently selected font will be used to
		///         form the glyph bitmap display lists in the current OpenGL rendering context.
		///     </para>
		/// </param>
		/// <param name="first">
		///     <para>
		///         Specifies the first glyph in the run of glyphs that will be used to form
		///         glyph bitmap display lists.
		///     </para>
		/// </param>
		/// <param name="count">
		///     <para>
		///         Specifies the number of glyphs in the run of glyphs that will be used to
		///         form glyph bitmap display lists.  The function creates <i>count</i> display
		///         lists, one for each glyph in the run.
		///     </para>
		/// </param>
		/// <param name="listBase">
		///     <para>
		///         Specifies a starting display list.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         If the function succeeds, the return value is true.
		///     </para>
		///     <para>
		///         If the function fails, the return value is false.  To get extended error
		///         information, call <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The <b>wglUseFontBitmaps</b> function defines <i>count</i> display lists in
		///         the current OpenGL rendering context.  Each display list has an identifying
		///         number, starting at <i>listBase</i>.  Each display list consists of a single
		///         call to Gl.glBitmap.  The definition of bitmap
		///         <i>listBase + i</i> is taken from the glyph <i>first + i</i> of the font
		///         currently selected in the device context specified by <i>deviceContext</i>.
		///         If a glyph is not defined, then the function defines an empty display list
		///         for it.
		///     </para>
		///     <para>
		///         The <b>wglUseFontBitmaps</b> function creates bitmap text in the plane of the
		///         screen.  It enables the labeling of objects in OpenGL.
		///     </para>
		///     <para>
		///         In the current version of Microsoft's implementation of OpenGL in Windows NT
		///         and Windows 95, you cannot make GDI calls to a device context that has a
		///         double-buffered pixel format.  Therefore, you cannot use the GDI fonts and
		///         text functions with such device contexts.  You can use the
		///         <b>wglUseFontBitmaps</b> function to circumvent this limitation and draw text
		///         in a double-buffered device context.
		///     </para>
		///     <para>
		///         The function determines the parameters of each call to
		///         Gl.glBitmap as follows:
		///     </para>
		///     <para>
		///         <list type="table">
		///             <listheader>
		///                 <term>glBitmap Parameter</term>
		///                 <description>Meaning</description>
		///             </listheader>
		///             <item>
		///                 <term>width</term>
		///                 <description>
		///                     The width of the glyph's bitmap, as returned in the
		///                     <b>gmBlackBoxX</b> member of the glyph's
		///                  /*   <see cref="GLYPHMETRICS" /> */structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>height</term>
		///                 <description>
		///                     The height of the glyph's bitmap, as returned in the
		///                     <b>gmBlackBoxY</b> member of the glyph's
		///                 /*    <see cref="GLYPHMETRICS" /> */structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>xorig</term>
		///                 <description>
		///                     The x offset of the glyph's origin, as returned in the
		///                     <b>gmptGlyphOrigin.x</b> member of the glyph's
		///                    /* see cref="Gdi.GLYPHMETRICS" /> */ structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>yorig</term>
		///                 <description>
		///                     The y offset of the glyph's origin, as returned in the
		///                     <b>gmptGlyphOrigin.y</b> member of the glyph's
		///                     /* see cref="Gdi.GLYPHMETRICS" /> */structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>xmove</term>
		///                 <description>
		///                     The horizontal distance to the origin of the next character cell,
		///                     as returned in the <b>gmCellIncX</b> member of the glyph's
		///                    /* see cref="Gdi.GLYPHMETRICS" /> */structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>ymove</term>
		///                 <description>
		///                     The vertical distance to the origin of the next character cell as
		///                     returned in the <b>gmCellIncY</b> member of the glyph's
		///                     /* see cref="Gdi.GLYPHMETRICS" /> */structure.
		///                 </description>
		///             </item>
		///             <item>
		///                 <term>bitmap</term>
		///                 <description>
		///                     The bitmap for the glyph, as returned by
		///                    /* see cref="Gdi.GetGlyphOutline" />*/  with <i>uFormat</i> equal to 1.
		///                 </description>
		///             </item>
		///         </list>
		///     </para>
		///     <para>
		///         The following code example shows how to use <b>wglUseFontBitmaps</b> to draw
		///         some text:
		///     </para>
		///     <para>
		///         <code>
		///             IntPtr hdc;
		///             IntPtr hglrc;
		///
		///             // create a rendering context
		///             hglrc = Wgl.wglCreateContext(hdc);
		///
		///             // make it the calling thread's current rendering context
		///             Wgl.wglMakeCurrent(hdc, hglrc);
		///
		///             // now we can call OpenGL API
		///
		///             // make the system font the device context's selected font
		///             Gdi.SelectObject(hdc, Gdi.GetStockObject(SYSTEM_FONT));
		///
		///             // create the bitmap display lists
		///             // we're making images of glyphs 0 thru 255
		///             // the display list numbering starts at 1000, an arbitrary choice
		///             Wgl.wglUseFontBitmaps(hdc, 0, 255, 1000);
		///
		///             // display a string:
		///             // indicate start of glyph display lists
		///             GL.glListBase(1000);
		///
		///             z/ now draw the characters in a string
		///             GL.glCallLists(24, GL.GL_UNSIGNED_SHORT, "Hello Win32 OpenGL World");
		///         </code>
		///     </para>
		/// </remarks>
		/// <seealso cref="wglUseFontOutlines" />
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		private static extern bool wglUseFontBitmaps(IntPtr deviceContext, int first, int count, int listBase);
		#endregion bool wglUseFontBitmaps(IntPtr deviceContext, int first, int count, int listBase)
		#region bool wglUseFontOutlines(IntPtr deviceContext, int first, int count, int listBase, float deviation, float extrusion, int format, [Out, MarshalAs(UnmanagedType.LPArray)] Gdi.GLYPHMETRICSFLOAT[] glyphMetrics)
		/// <summary>
		///     <para>
		///         The <b>wglUseFontOutlines</b> function creates a set of display lists, one
		///         for each glyph of the currently selected outline font of a device context,
		///         for use with the current rendering context.  The display lists are used to
		///         draw 3-D characters of TrueType fonts.  Each display list describes a glyph
		///         outline in floating-point coordinates.
		///     </para>
		///     <para>
		///         The run of glyphs begins with the <i>first</i> glyph of the font of the
		///         specified device context.  The em square size of the font, the notional grid
		///         size of the original font outline from which the font is fitted, is mapped to
		///         1.0 in the x- and y-coordinates in the display lists.  The <i>extrusion</i>
		///         parameter sets how much depth the font has in the z direction.
		///     </para>
		///     <para>
		///         The <i>glyphMetrics</i> parameter returns a
		///         <see cref="GLYPHMETRICSFLOAT" /> structure that contains information
		///         about the placement and orientation of each glyph in a character cell.
		///     </para>
		/// </summary>
		/// <param name="deviceContext">
		///     <para>
		///         Specifies the device context with the desired outline font.  The outline font
		///         of <i>deviceContext</i> is used to create the display lists in the current
		///         rendering context.
		///     </para>
		/// </param>
		/// <param name="first">
		///     <para>
		///         Specifies the first of the set of glyphs that form the font outline display
		///         lists.
		///     </para>
		/// </param>
		/// <param name="count">
		///     <para>
		///         Specifies the number of glyphs in the set of glyphs used to form the font
		///         outline display lists.  The <b>wglUseFontOutlines</b> function creates
		///         <i>count</i> display lists, one display list for each glyph in a set of
		///         glyphs.
		///     </para>
		/// </param>
		/// <param name="listBase">
		///     <para>
		///         Specifies a starting display list.
		///     </para>
		/// </param>
		/// <param name="deviation">
		///     <para>
		///         Specifies the maximum chordal deviation from the original outlines.  When
		///         <i>deviation</i> is zero, the chordal deviation is equivalent to one design
		///         unit of the original font.  The value of <i>deviation</i> must be equal to
		///         or greater than 0.
		///     </para>
		/// </param>
		/// <param name="extrusion">
		///     <para>
		///         Specifies how much a font is extruded in the negative z direction.  The
		///         value must be equal to or greater than 0.  When <i>extrusion</i> is 0, the
		///         display lists are not extruded.
		///     </para>
		/// </param>
		/// <param name="format">
		///     <para>
		///         Specifies the format, either <see cref="WGL_FONT_LINES" /> or
		///         <see cref="WGL_FONT_POLYGONS" />, to use in the display lists.  When
		///         <i>format</i> is <see cref="WGL_FONT_LINES" />, the
		///         <b>wglUseFontOutlines</b> function creates fonts with line segments.  When
		///         <i>format</i> is <see cref="WGL_FONT_POLYGONS" />, <b>wglUseFontOutlines</b>
		///         creates fonts with polygons.
		///     </para>
		/// </param>
		/// <param name="glyphMetrics">
		///     <para>
		///         Points to an array of <i>count</i> <see cref="GLYPHMETRICSFLOAT" />
		///         structures that is to receive the metrics of the glyphs.  When
		///         <i>glyphMetrics</i> is null, no glyph metrics are returned.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         When the function succeeds, the return value is true.
		///     </para>
		///     <para>
		///         When the function fails, the return value is false and no display lists are
		///         generated.  To get extended error information, call
		///         <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The <b>wglUseFontOutlines</b> function defines the glyphs of an outline font
		///         with display lists in the current rendering context.  The
		///         <b>wglUseFontOutlines</b> function works with TrueType fonts only; stroke and
		///         raster fonts are not supported.
		///     </para>
		///     <para>
		///         Each display list consists of either line segments or polygons, and has a
		///         unique identifying number starting with the <i>listBase</i> number.
		///     </para>
		///     <para>
		///         The <b>wglUseFontOutlines</b> function approximates glyph outlines by
		///         subdividing the quadratic B-spline curves of the outline into line segments,
		///         until the distance between the outline and the interpolated midpoint is
		///         within the value specified by <i>deviation</i>.  This is the final format
		///         used when <i>format</i> is <see cref="WGL_FONT_LINES" />.  When you specify
		///         <see cref="WGL_FONT_LINES" />, the display lists created don't contain any
		///         normals; thus lighting doesn't work properly.  To get the correct lighting of
		///         lines use <see cref="WGL_FONT_POLYGONS" /> and set
		///         <c>Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINE)</c>.  When you specify
		///         <i>format</i> as <see cref="WGL_FONT_POLYGONS" /> the outlines are further
		///         tessellated into separate triangles, triangle fans, triangle strips, or
		///         quadrilateral strips to create the surface of each glyph.  With
		///         <see cref="WGL_FONT_POLYGONS" />, the created display lists call
		///         <c>Gl.glFrontFace(Gl.GL_CW)</c> or <c>Gl.glFrontFace(Gl.GL_CCW)</c>; thus
		///         the current front-face value might be altered.  For the best appearance of
		///         text with <see cref="WGL_FONT_POLYGONS" />, cull the back faces as follows:
		///     </para>
		///     <para>
		///         <code>
		///             Gl.glCullFace(Gl.GL_BACK);
		///             Gl.glEnable(Gl.GL_CULL_FACE);
		///         </code>
		///     </para>
		///     <para>
		///         A <see cref="GLYPHMETRICSFLOAT" /> structure contains information about
		///         the placement and orientation of each glyph in a character cell.  The
		///         <i>glyphMetrics</i> parameter is an array of
		///         <see cref="GLYPHMETRICSFLOAT" /> structures holding the entire set of
		///         glyphs for a font.  Each display list ends with a translation specified with
		///         the <b>gmfCellIncX</b> and <b>gmfCellIncY</b> members of the corresponding
		///         <see cref="GLYPHMETRICSFLOAT" /> structure.  The translation enables the
		///         drawing of successive characters in their natural direction with a single
		///         call to Gl.glCallLists.
		///     </para>
		///     <para>
		///         <b>NOTE</b>
		///     </para>
		///     <para>
		///         With the current release of OpenGL for Windows NT and Windows 95, you cannot
		///         make GDI calls to a device context when a pixel format is double-buffered.
		///         You can work around this limitation by using <b>wglUseFontOutlines</b> and
		///         <see cref="wglUseFontBitmaps" />, when using double-buffered device contexts.
		///     </para>
		///     <para>
		///         The following code example shows how to draw text using
		///         <b>wglUseFontOutlines</b>:
		///     </para>
		///     <para>
		///         <code>
		///             IntPtr hdc;  // A TrueType font has already been selected
		///             IntPtr hglrc;
		///             Gdi.GLYPHMETRICSFLOAT[] agmf = new Gdi.GLYPHMETRICSFLOAT[256];
		///
		///             // Make hglrc the calling thread's current rendering context
		///             wglMakeCurrent(hdc, hglrc);
		///
		///             // create display lists for glyphs 0 through 255 with 0.1 extrusion
		///             // and default deviation. The display list numbering starts at 1000
		///             // (it could be any number)
		///             Wgl.wglUseFontOutlines(hdc, 0, 255, 1000, 0.0f, 0.1f, Wgl.WGL_FONT_POLYGONS, ref agmf);
		///
		///             // Set up transformation to draw the string
		///             Gl.glLoadIdentity();
		///             Gl.glTranslate(0.0f, 0.0f, -5.0f)
		///             Gl.glScalef(2.0f, 2.0f, 2.0f);
		///
		///             // Display a string
		///             Gl.glListBase(1000); // Indicates the start of display lists for the glyphs
		///
		///             // Draw the characters in a string
		///             Gl.glCallLists(24, Gl.GL_UNSIGNED_SHORT, "Hello Win32 OpenGL World.");
		///         </code>
		///     </para>
		/// </remarks>
		/// <seealso cref="GLYPHMETRICSFLOAT" />
		/// <seealso cref="wglUseFontBitmaps" />
		[DllImport(WGL_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern bool wglUseFontOutlines(IntPtr deviceContext, int first, int count, int listBase, float deviation, float extrusion, int format, [Out, MarshalAs(UnmanagedType.LPArray)] GLYPHMETRICSFLOAT[] glyphMetrics);
			
		#endregion bool wglUseFontOutlines(IntPtr deviceContext, int first, int count, int listBase, float deviation, float extrusion, int format, [Out, MarshalAs(UnmanagedType.LPArray)] Gdi.GLYPHMETRICSFLOAT[] glyphMetrics)
		#region IntPtr wglGetCurrentContext()
		/// <summary>
		///     <para>
		///         The <b>wglGetCurrentContext</b> function obtains a handle to the current
		///         OpenGL rendering context of the calling thread.
		///     </para>
		/// </summary>
		/// <returns>
		///     <para>
		///         If the calling thread has a current OpenGL rendering context,
		///         <b>wglGetCurrentContext</b> returns a handle to that rendering context.
		///         Otherwise, the return value is <see cref="IntPtr.Zero" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The current OpenGL rendering context of a thread is associated with a device
		///         context by means of the <see cref="wglMakeCurrent" /> function. 
		///     </para>
		/// </remarks>
		/// <seealso cref="wglCreateContext" />
		/// <seealso cref="wglDeleteContext" />
		/// <seealso cref="wglMakeCurrent" />
		// WINGDIAPI HGLRC WINAPI wglGetCurrentContext(VOID);
		[DllImport(WGL_NATIVE_LIBRARY), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr wglGetCurrentContext();
		#endregion IntPtr wglGetCurrentContext()

        #region bool wglShareLists(IntPtr source, IntPtr destination)
        /// <summary>
        ///     <para>
        ///         The <b>wglShareLists</b> function enables multiple OpenGL rendering contexts
        ///         to share a single display-list space.
        ///     </para>
        /// </summary>
        /// <param name="source">
        ///     <para>
        ///         Specifies the OpenGL rendering context with which to share display lists.
        ///     </para>
        /// </param>
        /// <param name="destination">
        ///     <para>
        ///         Specifies the OpenGL rendering context to share display lists with
        ///         <i>source</i>.  The <i>destination</i> parameter should not contain any
        ///         existing display lists when <b>wglShareLists</b> is called.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         When the function succeeds, the return value is true.
        ///     </para>
        ///     <para>
        ///         When the function fails, the return value is false and the display lists are
        ///         not shared.  To get extended error information, call
        ///         <see cref="Marshal.GetLastWin32Error" />.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When you create an OpenGL rendering context, it has its own display-list
        ///         space.  The <b>wglShareLists</b> function enables a rendering context to
        ///         share the display-list space of another rendering context; any number of
        ///         rendering contexts can share a single display-list space.  Once a rendering
        ///         context shares a display-list space, the rendering context always uses the
        ///         display-list space until the rendering context is deleted.  When the last
        ///         rendering context of a shared display-list space is deleted, the shared
        ///         display-list space is deleted.  All the indexes and definitions of display
        ///         lists in a shared display-list space are shared.
        ///     </para>
        ///     <para>
        ///         You can only share display lists with rendering contexts within the same
        ///         process.  However, not all rendering contexts in a process can share display
        ///         lists.  Rendering contexts can share display lists only if they use the same
        ///         implementation of OpenGL functions.  All client rendering contexts of a given
        ///         pixel format can always share display lists.
        ///     </para>
        ///     <para>
        ///         All rendering contexts of a shared display list must use an identical pixel
        ///         format.  Otherwise the results depend on the implementation of OpenGL used.
        ///     </para>
        ///     <para>
        ///         <b>NOTE</b>
        ///     </para>
        ///     </remarks>
        // WINGDIAPI BOOL  WINAPI wglShareLists(HGLRC, HGLRC);
        [DllImport(WGL_NATIVE_LIBRARY, SetLastError = true), SuppressUnmanagedCodeSecurity]
        public static extern bool wglShareLists(IntPtr source, IntPtr destination);
        #endregion bool wglShareLists(IntPtr source, IntPtr destination)


		#region PIXELFORMATDESCRIPTOR Struct
		/// <summary>
		///     The <b>PIXELFORMATDESCRIPTOR</b> structure describes the pixel format of a drawing surface.
		/// </summary>
		/// <remarks>
		///     Please notice carefully, as documented in the members, that certain pixel format properties are not supported
		///     in the current generic implementation. The generic implementation is the Microsoft GDI software
		///     implementation of OpenGL. Hardware manufacturers may enhance parts of OpenGL, and may support some
		///     pixel format properties not supported by the generic implementation.
		/// </remarks>
		/// <seealso cref="ChoosePixelFormat" />
		/// <seealso cref="SetPixelFormat" />
		[StructLayout(LayoutKind.Sequential)]
			public struct PIXELFORMATDESCRIPTOR 
		{
			/// <summary>
			/// Specifies the size of this data structure. This value should be set to <c>sizeof(PIXELFORMATDESCRIPTOR)</c>.
			/// </summary>
			public Int16 nSize;

			/// <summary>
			/// Specifies the version of this data structure. This value should be set to 1.
			/// </summary>
			public Int16 nVersion;

			/// <summary>
			/// A set of bit flags that specify properties of the pixel buffer. The properties are generally not mutually exclusive;
			/// you can set any combination of bit flags, with the exceptions noted.
			/// </summary>
			/// <remarks>
			///     <para>The following bit flag constants are defined:</para>
			///     <list type="table">
			///			<listheader>
			///				<term>Value</term>
			///				<description>Meaning</description>
			///			</listheader>
			///			<item>
			///				<term>PFD_DRAW_TO_WINDOW</term>
			///				<description>The buffer can draw to a window or device surface.</description>
			///			</item>
			///			<item>
			///				<term>PFD_DRAW_TO_BITMAP</term>
			///				<description>The buffer can draw to a memory bitmap.</description>
			///			</item>
			///			<item>
			///				<term>PFD_SUPPORT_GDI</term>
			///				<description>
			///					The buffer supports GDI drawing. This flag and PFD_DOUBLEBUFFER are mutually exclusive
			///					in the current generic implementation.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_SUPPORT_OPENGL</term>
			///				<description>The buffer supports OpenGL drawing.</description>
			///			</item>
			///			<item>
			///				<term>PFD_GENERIC_ACCELERATED</term>
			///				<description>
			///					The pixel format is supported by a device driver that accelerates the generic implementation.
			///					If this flag is clear and the PFD_GENERIC_FORMAT flag is set, the pixel format is supported by
			///					the generic implementation only.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_GENERIC_FORMAT</term>
			///				<description>
			///					The pixel format is supported by the GDI software implementation, which is also known as the
			///					generic implementation. If this bit is clear, the pixel format is supported by a device
			///					driver or hardware.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_NEED_PALETTE</term>
			///				<description>
			///					The buffer uses RGBA pixels on a palette-managed device. A logical palette is required to achieve
			///					the best results for this pixel type. Colors in the palette should be specified according to the
			///					values of the <b>cRedBits</b>, <b>cRedShift</b>, <b>cGreenBits</b>, <b>cGreenShift</b>,
			///					<b>cBluebits</b>, and <b>cBlueShift</b> members. The palette should be created and realized in
			///					the device context before calling <see cref="wglMakeCurrent" />.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_NEED_SYSTEM_PALETTE</term>
			///				<description>
			///					Defined in the pixel format descriptors of hardware that supports one hardware palette in
			///					256-color mode only. For such systems to use hardware acceleration, the hardware palette must be in
			///					a fixed order (for example, 3-3-2) when in RGBA mode or must match the logical palette when in
			///					color-index mode.
			///					
			///					When this flag is set, you must call SetSystemPaletteUse in your program to force a one-to-one
			///					mapping of the logical palette and the system palette. If your OpenGL hardware supports multiple
			///					hardware palettes and the device driver can allocate spare hardware palettes for OpenGL, this
			///					flag is typically clear.
			///					
			///					This flag is not set in the generic pixel formats.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_DOUBLEBUFFER</term>
			///				<description>
			///					The buffer is double-buffered. This flag and PFD_SUPPORT_GDI are mutually exclusive in the
			///					current generic implementation.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_STEREO</term>
			///				<description>
			///					The buffer is stereoscopic. This flag is not supported in the current generic implementation.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_SWAP_LAYER_BUFFERS</term>
			///				<description>
			///					Indicates whether a device can swap individual layer planes with pixel formats that include
			///					double-buffered overlay or underlay planes. Otherwise all layer planes are swapped together
			///					as a group. When this flag is set, <b>wglSwapLayerBuffers</b> is supported.
			///				</description>
			///			</item>
			///		</list>
			///		<para>You can specify the following bit flags when calling <see cref="ChoosePixelFormat" />.</para>
			///		<list type="table">
			///			<listheader>
			///				<term>Value</term>
			///				<description>Meaning</description>
			///			</listheader>
			///			<item>
			///				<term>PFD_DEPTH_DONTCARE</term>
			///				<description>
			///					The requested pixel format can either have or not have a depth buffer. To select
			///					a pixel format without a depth buffer, you must specify this flag. The requested pixel format
			///					can be with or without a depth buffer. Otherwise, only pixel formats with a depth buffer
			///					are considered.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_DOUBLEBUFFER_DONTCARE</term>
			///				<description>The requested pixel format can be either single- or double-buffered.</description>
			///			</item>
			///			<item>
			///				<term>PFD_STEREO_DONTCARE</term>
			///				<description>The requested pixel format can be either monoscopic or stereoscopic.</description>
			///			</item>
			///		</list>
			///		<para>
			///			With the <b>glAddSwapHintRectWIN</b> extension function, two new flags are included for the
			///			<b>PIXELFORMATDESCRIPTOR</b> pixel format structure.
			///		</para>
			///		<list type="table">
			///			<listheader>
			///				<term>Value</term>
			///				<description>Meaning</description>
			///			</listheader>
			///			<item>
			///				<term>PFD_SWAP_COPY</term>
			///				<description>
			///					Specifies the content of the back buffer in the double-buffered main color plane following
			///					a buffer swap. Swapping the color buffers causes the content of the back buffer to be copied
			///					to the front buffer. The content of the back buffer is not affected by the swap. PFD_SWAP_COPY
			///					is a hint only and might not be provided by a driver.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_SWAP_EXCHANGE</term>
			///				<description>
			///					Specifies the content of the back buffer in the double-buffered main color plane following a
			///					buffer swap. Swapping the color buffers causes the exchange of the back buffer's content
			///					with the front buffer's content. Following the swap, the back buffer's content contains the
			///					front buffer's content before the swap. PFD_SWAP_EXCHANGE is a hint only and might not be
			///					provided by a driver.
			///				</description>
			///			</item>
			///		</list>
			/// </remarks>
			public Int32 dwFlags;

			/// <summary>
			/// Specifies the type of pixel data. The following types are defined.
			/// </summary>
			/// <remarks>
			///		<list type="table">
			///			<listheader>
			///				<term>Value</term>
			///				<description>Meaning</description>
			///			</listheader>
			///			<item>
			///				<term>PFD_TYPE_RGBA</term>
			///				<description>
			///					RGBA pixels. Each pixel has four components in this order: red, green, blue, and alpha.
			///				</description>
			///			</item>
			///			<item>
			///				<term>PFD_TYPE_COLORINDEX</term>
			///				<description>Color-index pixels. Each pixel uses a color-index value.</description>
			///			</item>
			///		</list>
			/// </remarks>
			public Byte iPixelType;

			/// <summary>
			/// Specifies the number of color bitplanes in each color buffer. For RGBA pixel types, it is the size
			/// of the color buffer, excluding the alpha bitplanes. For color-index pixels, it is the size of the
			/// color-index buffer.
			/// </summary>
			public Byte cColorBits;

			/// <summary>
			/// Specifies the number of red bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cRedBits;

			/// <summary>
			/// Specifies the shift count for red bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cRedShift;

			/// <summary>
			/// Specifies the number of green bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cGreenBits;

			/// <summary>
			/// Specifies the shift count for green bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cGreenShift;

			/// <summary>
			/// Specifies the number of blue bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cBlueBits;

			/// <summary>
			/// Specifies the shift count for blue bitplanes in each RGBA color buffer.
			/// </summary>
			public Byte cBlueShift;

			/// <summary>
			/// Specifies the number of alpha bitplanes in each RGBA color buffer. Alpha bitplanes are not supported.
			/// </summary>
			public Byte cAlphaBits;

			/// <summary>
			/// Specifies the shift count for alpha bitplanes in each RGBA color buffer. Alpha bitplanes are not supported.
			/// </summary>
			public Byte cAlphaShift;

			/// <summary>
			/// Specifies the total number of bitplanes in the accumulation buffer.
			/// </summary>
			public Byte cAccumBits;

			/// <summary>
			/// Specifies the number of red bitplanes in the accumulation buffer.
			/// </summary>
			public Byte cAccumRedBits;

			/// <summary>
			/// Specifies the number of green bitplanes in the accumulation buffer.
			/// </summary>
			public Byte cAccumGreenBits;

			/// <summary>
			/// Specifies the number of blue bitplanes in the accumulation buffer.
			/// </summary>
			public Byte cAccumBlueBits;

			/// <summary>
			/// Specifies the number of alpha bitplanes in the accumulation buffer.
			/// </summary>
			public Byte cAccumAlphaBits;

			/// <summary>
			/// Specifies the depth of the depth (z-axis) buffer.
			/// </summary>
			public Byte cDepthBits;

			/// <summary>
			/// Specifies the depth of the stencil buffer.
			/// </summary>
			public Byte cStencilBits;

			/// <summary>
			/// Specifies the number of auxiliary buffers. Auxiliary buffers are not supported.
			/// </summary>
			public Byte cAuxBuffers;

			/// <summary>
			/// Ignored. Earlier implementations of OpenGL used this member, but it is no longer used.
			/// </summary>
			/// <remarks>Specifies the type of layer.</remarks>
			public Byte iLayerType;

			/// <summary>
			/// Specifies the number of overlay and underlay planes. Bits 0 through 3 specify up to 15 overlay planes and
			/// bits 4 through 7 specify up to 15 underlay planes.
			/// </summary>
			public Byte bReserved;

			/// <summary>
			/// Ignored. Earlier implementations of OpenGL used this member, but it is no longer used.
			/// </summary>
			/// <remarks>
			///		Specifies the layer mask. The layer mask is used in conjunction with the visible mask to determine
			///		if one layer overlays another.
			/// </remarks>
			public Int32 dwLayerMask;

			/// <summary>
			/// Specifies the transparent color or index of an underlay plane. When the pixel type is RGBA, <b>dwVisibleMask</b>
			/// is a transparent RGB color value. When the pixel type is color index, it is a transparent index value.
			/// </summary>
			public Int32 dwVisibleMask;

			/// <summary>
			/// Ignored. Earlier implementations of OpenGL used this member, but it is no longer used.
			/// </summary>
			/// <remarks>
			///		Specifies whether more than one pixel format shares the same frame buffer. If the result of the bitwise
			///		AND of the damage masks between two pixel formats is nonzero, then they share the same buffers.
			/// </remarks>
			public Int32 dwDamageMask;
		};
		#endregion PIXELFORMATDESCRIPTOR Struct
		#region PIXELFORMATDESCRIPTOR Pixel Types
		#region int PFD_TYPE_RGBA
		/// <summary>
		///     RGBA pixels.  Each pixel has four components in this order: red, green, blue,
		///     and alpha.
		/// </summary>
		// #define PFD_TYPE_RGBA        0
		public const int PFD_TYPE_RGBA = 0;
		#endregion int PFD_TYPE_RGBA

		#region int PFD_TYPE_COLORINDEX
		/// <summary>
		///     Color-index pixels.  Each pixel uses a color-index value.
		/// </summary>
		// #define PFD_TYPE_COLORINDEX  1
		public const int PFD_TYPE_COLORINDEX = 1;
		#endregion int PFD_TYPE_COLORINDEX
		#endregion PIXELFORMATDESCRIPTOR Pixel Types
		#region PIXELFORMATDESCRIPTOR Layer Types
		#region int PFD_MAIN_PLANE
		/// <summary>
		///     The layer is the main plane.
		/// </summary>
		// #define PFD_MAIN_PLANE       0
		public const int PFD_MAIN_PLANE = 0;
		#endregion int PFD_MAIN_PLANE

		#region int PFD_OVERLAY_PLANE
		/// <summary>
		///     The layer is the overlay plane.
		/// </summary>
		// #define PFD_OVERLAY_PLANE    1
		public const int PFD_OVERLAY_PLANE = 1;
		#endregion int PFD_OVERLAY_PLANE

		#region int PFD_UNDERLAY_PLANE
		/// <summary>
		///     The layer is the underlay plane.
		/// </summary>
		// #define PFD_UNDERLAY_PLANE   (-1)
		public const int PFD_UNDERLAY_PLANE = -1;
		#endregion int PFD_UNDERLAY_PLANE
		#endregion PIXELFORMATDESCRIPTOR Layer Types
		#region PIXELFORMATDESCRIPTOR Flags
		#region int PFD_DOUBLEBUFFER
		/// <summary>
		///     <para>
		///         The buffer is double-buffered.  This flag and <see cref="PFD_SUPPORT_GDI" />
		///         are mutually exclusive in the current generic implementation.
		///     </para>
		/// </summary>
		// #define PFD_DOUBLEBUFFER            0x00000001
		public const int PFD_DOUBLEBUFFER = 0x00000001;
		#endregion int PFD_DOUBLEBUFFER

		#region int PFD_STEREO
		/// <summary>
		///     <para>
		///         The buffer is stereoscopic.  This flag is not supported in the current
		///         generic implementation.
		///     </para>
		/// </summary>
		// #define PFD_STEREO                  0x00000002
		public const int PFD_STEREO = 0x00000002;
		#endregion int PFD_STEREO

		#region int PFD_DRAW_TO_WINDOW
		/// <summary>
		///     <para>
		///         The buffer can draw to a window or device surface.
		///     </para>
		/// </summary>
		// #define PFD_DRAW_TO_WINDOW          0x00000004
		public const int PFD_DRAW_TO_WINDOW = 0x00000004;
		#endregion int PFD_DRAW_TO_WINDOW

		#region int PFD_DRAW_TO_BITMAP
		/// <summary>
		///     <para>
		///         The buffer can draw to a memory bitmap.
		///     </para>
		/// </summary>
		// #define PFD_DRAW_TO_BITMAP          0x00000008
		public const int PFD_DRAW_TO_BITMAP = 0x00000008;
		#endregion int PFD_DRAW_TO_BITMAP

		#region int PFD_SUPPORT_GDI
		/// <summary>
		///     <para>
		///         The buffer supports GDI drawing.  This flag and
		///         <see cref="PFD_DOUBLEBUFFER" /> are mutually exclusive in the current generic
		///         implementation.
		///     </para>
		/// </summary>
		// #define PFD_SUPPORT_GDI             0x00000010
		private const int PFD_SUPPORT_GDI = 0x00000010;
		#endregion int PFD_SUPPORT_GDI

		#region int PFD_SUPPORT_OPENGL
		/// <summary>
		///     <para>
		///         The buffer supports OpenGL drawing.
		///     </para>
		/// </summary>
		// #define PFD_SUPPORT_OPENGL          0x00000020
		public const int PFD_SUPPORT_OPENGL = 0x00000020;
		#endregion int PFD_SUPPORT_OPENGL

		#region int PFD_GENERIC_FORMAT
		/// <summary>
		///     <para>
		///         The pixel format is supported by the GDI software implementation, which is
		///         also known as the generic implementation.  If this bit is clear, the pixel
		///         format is supported by a device driver or hardware.
		///     </para>
		/// </summary>
		// #define PFD_GENERIC_FORMAT          0x00000040
		public const int PFD_GENERIC_FORMAT = 0x00000040;
		#endregion int PFD_GENERIC_FORMAT

		#region int PFD_NEED_PALETTE
		/// <summary>
		///     <para>
		///         The buffer uses RGBA pixels on a palette-managed device.  A logical palette
		///         is required to achieve the best results for this pixel type.  Colors in the
		///         palette should be specified according to the values of the <b>cRedBits</b>,
		///         <b>cRedShift</b>, <b>cGreenBits</b>, <b>cGreenShift</b>, <b>cBluebits</b>,
		///         and <b>cBlueShift</b> members.  The palette should be created and realized in
		///         the device context before calling <see cref="FeaturesW32.wglMakeCurrent" />.
		///     </para>
		/// </summary>
		// #define PFD_NEED_PALETTE            0x00000080
		public const int PFD_NEED_PALETTE = 0x00000080;
		#endregion int PFD_NEED_PALETTE

		#region int PFD_NEED_SYSTEM_PALETTE
		/// <summary>
		///     <para>
		///         Defined in the pixel format descriptors of hardware that supports one
		///         hardware palette in 256-color mode only.  For such systems to use
		///         hardware acceleration, the hardware palette must be in a fixed order
		///         (for example, 3-3-2) when in RGBA mode or must match the logical palette
		///         when in color-index mode.
		///     </para>
		///     <para>
		///         This flag is not set in the generic pixel formats.
		///     </para>
		/// </summary>
		// #define PFD_NEED_SYSTEM_PALETTE     0x00000100
		public const int PFD_NEED_SYSTEM_PALETTE = 0x00000100;
		#endregion int PFD_NEED_SYSTEM_PALETTE

		#region int PFD_SWAP_EXCHANGE
		/// <summary>
		///     <para>
		///         Specifies the content of the back buffer in the double-buffered main color
		///         plane following a buffer swap.  Swapping the color buffers causes the
		///         exchange of the back buffer's content with the front buffer's content.
		///         Following the swap, the back buffer's content contains the front buffer's
		///         content before the swap. <b>PFD_SWAP_EXCHANGE</b> is a hint only and might
		///         not be provided by a driver.
		///     </para>
		/// </summary>
		// #define PFD_SWAP_EXCHANGE           0x00000200
		public const int PFD_SWAP_EXCHANGE = 0x00000200;
		#endregion int PFD_SWAP_EXCHANGE

		#region int PFD_SWAP_COPY
		/// <summary>
		///     <para>
		///         Specifies the content of the back buffer in the double-buffered main color
		///         plane following a buffer swap.  Swapping the color buffers causes the content
		///         of the back buffer to be copied to the front buffer.  The content of the back
		///         buffer is not affected by the swap.  <b>PFD_SWAP_COPY</b> is a hint only and
		///         might not be provided by a driver.
		///     </para>
		/// </summary>
		// #define PFD_SWAP_COPY               0x00000400
		public const int PFD_SWAP_COPY = 0x00000400;
		#endregion int PFD_SWAP_COPY

		#region int PFD_SWAP_LAYER_BUFFERS
		/// <summary>
		///     <para>
		///         Indicates whether a device can swap individual layer planes with pixel
		///         formats that include double-buffered overlay or underlay planes.
		///         Otherwise all layer planes are swapped together as a group.  When this
		///         flag is set, <see cref="FeaturesW32.wglSwapLayerBuffers" /> is supported.
		///     </para>
		/// </summary>
		// #define PFD_SWAP_LAYER_BUFFERS      0x00000800
		public const int PFD_SWAP_LAYER_BUFFERS = 0x00000800;
		#endregion int PFD_SWAP_LAYER_BUFFERS

		#region int PFD_GENERIC_ACCELERATED
		/// <summary>
		///     <para>
		///         The pixel format is supported by a device driver that accelerates the generic
		///         implementation.  If this flag is clear and the
		///         <see cref="PFD_GENERIC_FORMAT" /> flag is set, the pixel format is supported
		///         by the generic implementation only.
		///     </para>
		/// </summary>
		// #define PFD_GENERIC_ACCELERATED     0x00001000
		public const int PFD_GENERIC_ACCELERATED = 0x00001000;
		#endregion int PFD_GENERIC_ACCELERATED

		#region int PFD_SUPPORT_DIRECTDRAW
		/// <summary>
		///     <para>
		///         The buffer supports DirectDraw drawing.
		///     </para>
		/// </summary>
		// #define PFD_SUPPORT_DIRECTDRAW      0x00002000
		public const int PFD_SUPPORT_DIRECTDRAW = 0x00002000;
        private const int PFD_SUPPORT_COMPOSITION = 0x00008000;
		#endregion int PFD_SUPPORT_DIRECTDRAW
		#endregion PIXELFORMATDESCRIPTOR Flags
		#region PIXELFORMATDESCRIPTOR Flags For Use In ChoosePixelFormat Only
		#region int PFD_DEPTH_DONTCARE
		/// <summary>
		///     <para>
		///         The requested pixel format can either have or not have a depth buffer.  To
		///         select a pixel format without a depth buffer, you must specify this flag.
		///         The requested pixel format can be with or without a depth buffer.  Otherwise,
		///         only pixel formats with a depth buffer are considered.
		///     </para>
		/// </summary>
		// #define PFD_DEPTH_DONTCARE          0x20000000
		public const int PFD_DEPTH_DONTCARE = 0x20000000;
		#endregion int PFD_DEPTH_DONTCARE

		#region int PFD_DOUBLEBUFFER_DONTCARE
		/// <summary>
		///     <para>
		///         The requested pixel format can be either single- or double-buffered.
		///     </para>
		/// </summary>
		// #define PFD_DOUBLEBUFFER_DONTCARE   0x40000000
		public const int PFD_DOUBLEBUFFER_DONTCARE = 0x40000000;
		#endregion int PFD_DOUBLEBUFFER_DONTCARE

		#region int PFD_STEREO_DONTCARE
		/// <summary>
		///     <para>
		///         The requested pixel format can be either monoscopic or stereoscopic.
		///     </para>
		/// </summary>
		// #define PFD_STEREO_DONTCARE         0x80000000
		public const int PFD_STEREO_DONTCARE = unchecked((int) 0x80000000);
		#endregion int PFD_STEREO_DONTCARE
		#endregion PIXELFORMATDESCRIPTOR Flags For Use In ChoosePixelFormat Only

        // --------------------------- NEUNEUNEU
       // [System.Runtime.InteropServices.DllImport(WGL_NATIVE_LIBRARY,CharSet = CharSet.Auto)]
        /// <summary>
        ///     <para>
        ///         The <b>wglGetProcAddress</b> function returns the address of an OpenGL
        ///         extension function for use with the current OpenGL rendering context.
        ///     </para>
        /// </summary>
        /// <param name="extension">
        ///     <para>
        ///         Points to a null-terminated string that is the name of the extension
        ///         function.  The name of the extension function must be identical to a
        ///         corresponding function implemented by OpenGL.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         When the function succeeds, the return value is the address of the extension
        ///         function.
        ///     </para>
        ///     <para>
        ///         When no current rendering context exists or the function fails, the return
        ///         value is <see cref="IntPtr.Zero" />.  To get extended error information, call
        ///         <see cref="Marshal.GetLastWin32Error" />.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The OpenGL library supports multiple implementations of its functions.
        ///         Extension functions supported in one rendering context are not necessarily
        ///         available in a separate rendering context.  Thus, for a given rendering
        ///         context in an application, use the function addresses returned by the
        ///         <b>wglGetProcAddress</b> function only.
        ///     </para>
        ///     <para>
        ///         The spelling and the case of the extension function pointed to by
        ///         <i>extension</i> must be identical to that of a function supported and
        ///         implemented by OpenGL.  Because extension functions are not exported by
        ///         OpenGL, you must use <b>wglGetProcAddress</b> to get the addresses of
        ///         vendor-specific extension functions.
        ///     </para>
        ///     <para>
        ///         The extension function addresses are unique for each pixel format.  All
        ///         rendering contexts of a given pixel format share the same extension function
        ///         addresses.
        ///     </para>
        /// </remarks>
        ///    /// <seealso cref="wglMakeCurrent" />
        [DllImport(WGL_NATIVE_LIBRARY, EntryPoint = "wglGetProcAddress", ExactSpelling = true)]
        public static extern IntPtr wglGetProcAddress(String extension);
        /// <summary>
        /// Call this member function to compute the width and height of a line of text using the current font to determine the dimensions.
        /// </summary>
        /// <param name="hDC">Device context</param>
        /// <param name="lpsz">The text</param>
        /// <param name="cbString">Specifies the number of characters in the string.</param>
        /// <param name="lpSize">You get the size of the text</param>
        /// <returns></returns>
        [DllImport(GDI_NATIVE_LIBRARY, CharSet = CharSet.Auto)]
        public static extern int
        GetTextExtentPoint32(IntPtr hDC, string lpsz, int cbString,
        ref Point lpSize);
        /// <summary>
        /// The TextOut function writes a character string at the specified location, using the currently selected font, background color, and text color.
        /// </summary>
        /// <param name="hDC">A handle to the device context.</param>
        /// <param name="x">The x-coordinate, in logical coordinates, of the reference point that the system uses to align the string.</param>
        /// <param name="y">The y-coordinate, in logical coordinates, of the reference point that the system uses to align the string.</param>
        /// <param name="lpString">A pointer to the string to be drawn. The string does not need to be zero-terminated, because cchString specifies the length of the string.</param>
        /// <param name="nCount">The length of the string pointed to by lpString, in characters.</param>
        /// <returns></returns>
        [DllImport(GDI_NATIVE_LIBRARY, CharSet = CharSet.Auto)]
        public static extern int
        TextOut(IntPtr hDC, int x, int y, string lpString, int nCount);
        /// <summary>
        /// This function selects an object into a specified device context. The new object replaces the previous object of the same type. 
        /// </summary>
        /// <param name="hDC">A handle to the device context.</param>
        /// <param name="font">Handle to the object to be selected. </param>
        /// <returns></returns>
        [DllImport(GDI_NATIVE_LIBRARY, EntryPoint = "SelectObject",
        CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr SelectObject(
        [In] IntPtr hDC,
        [In] UIntPtr font);


        /// <summary>
        /// The EndPath function closes a path bracket and selects the path defined by the bracket into the specified device context.
        /// </summary>
        /// <param name="hDC">A handle to the device context.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        ///If the function fails, the return value is zero.
        ///</returns>
        ///<seealso cref="BeginPath"/>
        [DllImport(GDI_NATIVE_LIBRARY)]
        public static extern int EndPath(IntPtr hDC);
        /// <summary>
        /// The BeginPath function opens a path bracket in the specified device context.
        /// </summary>
        /// <param name="hDC">A handle to the device context.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        ///If the function fails, the return value is zero.
        /// </returns>
        [DllImport(GDI_NATIVE_LIBRARY)]
        public static extern int BeginPath(IntPtr hDC);
        /// <summary>
        /// The FlattenPath function transforms any curves in the path that is selected into the current device context (DC), turning each curve into a sequence of lines.
        /// </summary>
        /// <param name="hDC">A handle to a DC that contains a valid path.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        ///If the function fails, the return value is zero.
        /// </returns>
        [DllImport(GDI_NATIVE_LIBRARY)]
        public static extern int FlattenPath(IntPtr hDC);
        /// <summary>
        /// The GetPath function retrieves the coordinates defining the endpoints of lines and the control points of curves found in the path that is selected into the specified device context.
        /// </summary>
        /// <param name="hDC">A handle to a device context that contains a closed path.</param>
        /// <param name="lpPoint">A pointer to an array of POINT structures that receives the line endpoints and curve control points, in logical coordinates.</param>
        /// <param name="lpTypes">A pointer to an array of bytes that receives the vertex types. This parameter can be one of the following values:
        /// PT_MOVETO, PT_LINETO, PT_BEZIERTO</param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport(GDI_NATIVE_LIBRARY)]
        public static extern int GetPath(IntPtr hDC, ref Point lpPoint, ref byte lpTypes, int nSize);
        /// <summary>
        /// <see cref="GetPath(IntPtr,ref Point,ref byte,int)"/>
        /// </summary>
        /// <param name="hDC">A handle to a device context that contains a closed path.</param>
        /// <param name="lpPoint">A pointer to an array of POINT structures that receives the line endpoints and curve control points, in logical coordinates.</param>
        /// <param name="lpTypes">A pointer to an array of bytes that receives the vertex types. This parameter can be one of the following values:
        /// PT_MOVETO, PT_LINETO, PT_BEZIERTO</param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport(GDI_NATIVE_LIBRARY)]
        public static extern int GetPath(IntPtr hDC, IntPtr lpPoint, IntPtr lpTypes, int nSize); 

        // ---------------------------- NEUNEUNEU

		// --- Public Externs ---
		/// <summary>
		/// Windows structure. Contains metric descriptions about a char.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TEXTMETRIC 
		{ 
			/// <summary>
			/// </summary>
			public int tmHeight; 
			/// <summary>
			/// </summary>

			public int tmAscent; 
			/// <summary>
			/// </summary>
			public int tmDescent; 
			/// <summary>
			/// </summary>
			public int tmInternalLeading; 
			/// <summary>
			/// </summary>
			public int tmExternalLeading; 
			/// <summary>
			/// </summary>
			public int tmAveCharWidth; 
			/// <summary>
			/// </summary>
			public int tmMaxCharWidth; 
			/// <summary>
			/// </summary>
			public int tmWeight; 
			/// <summary>
			/// </summary>
			public int tmOverhang; 
			/// <summary>
			/// </summary>
			public int tmDigitizedAspectX; 
			/// <summary>
			/// </summary>
			public int tmDigitizedAspectY; 
			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=1)]
			public String tmFirstChar; 
			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=1)]
			public String tmLastChar; 
			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=1)]
			public String tmDefaultChar; 
			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=1)]
			public String tmBreakChar; 
			/// <summary>
			/// </summary>
			public byte tmItalic; 
			/// <summary>
			/// </summary>
			public byte tmUnderlined; 
			/// <summary>
			/// </summary>
			public byte tmStruckOut; 
			/// <summary>
			/// </summary>
			public byte tmPitchAndFamily; 
			/// <summary>
			/// </summary>
			public byte tmCharSet; 
		}

		#region BOOL SetPixelFormat(HDC hdc, int iPixelFormat, PIXELFORMATDESCRIPTOR* ppfd)
		/// <summary>
		/// The <b>SetPixelFormat</b> function sets the pixel format of the specified device context to the format
		/// specified by the <i>iPixelFormat</i> index.
		/// </summary>
		/// <param name="deviceContext">
		///		Specifies the device context whose pixel format the function attempts to set.
		/// </param>
		/// <param name="pixelFormat">
		///		Index that identifies the pixel format to set. The various pixel formats supported by a device
		///		context are identified by one-based indexes.
		/// </param>
		/// <param name="pixelFormatDescriptor">
		///		Pointer to a <see cref="FeaturesW32.PIXELFORMATDESCRIPTOR" /> structure that contains the logical pixel
		///		format specification. The system's metafile component uses this structure to record the logical
		///		pixel format specification. The structure has no other effect upon the behavior of the
		///		<b>SetPixelFormat</b> function.
		/// </param>
		/// <returns>
		///		If the function succeeds, the return value is true.<br /><br />
		///		If the function fails, the return value is false. 
		/// </returns>
		/// <remarks>
		///		If <i>hdc</i> references a window, calling the <b>SetPixelFormat</b> function also changes the pixel format
		///		of the window. Setting the pixel format of a window more than once can lead to significant complications
		///		for the Window Manager and for multithread applications, so it is not allowed. An application can only set
		///		the pixel format of a window one time. Once a window's pixel format is set, it cannot be changed.<br /><br />
		///
		///		You should select a pixel format in the device context before calling the <see cref="wglCreateContext" />
		///		function. The <b>wglCreateContext</b> function creates a rendering context for drawing on the device in the
		///		selected pixel format of the device context.<br /><br />
		///
		///		An OpenGL window has its own pixel format. Because of this, only device contexts retrieved for the client
		///		area of an OpenGL window are allowed to draw into the window. As a result, an OpenGL window should be created
		///		with the WS_CLIPCHILDREN and WS_CLIPSIBLINGS styles. Additionally, the window class attribute should not
		///		include the CS_PARENTDC style.<br /><br />
		///
		///		The following code example shows <b>SetPixelFormat</b> usage:<br /><br />
		///
		///		<code>
		///			HDC hdc;
		///			int pixelFormat;
		///			FeaturesW32.PIXELFORMATDESCRIPTOR pfd;
		///
		///			// size of this pfd
		///			pfd.nSize = (IndexType) sizeof(FeaturesW32.PIXELFORMATDESCRIPTOR);
		///
		///			// version number
		///			pfd.nVersion = 1;
		///
		///			// support window, support OpenGL, double buffered
		///			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
		///
		///			// RGBA type
		///			pfd.iPixelType = Gdi.PFD_TYPE_RGBA;
		///
		///			// 24-bit color depth
		///			pfd.cColorBits = 24;
		///
		///			// color bits and shift bits ignored
		///			pfd.cRedBits = 0;
		///			pfd.cRedShift = 0;
		///			pfd.cGreenBits = 0;
		///			pfd.cGreenShift = 0;
		///			pfd.cBlueBits = 0;
		///			pfd.cBlueShift = 0;
		///			pfd.cAlphaBits = 0;
		///			pfd.cAlphaShift = 0;
		///
		///			// no accumulation buffer, accum bits ignored
		///			pfd.cAccumBits = 0;
		///			pfd.cAccumRedBits = 0;
		///			pfd.cAccumGreenBits = 0;
		///			pfd.cAccumBlueBits = 0;
		///			pfd.cAccumAlphaBits = 0;
		///
		///			// no stencil buffer
		///			pfd.cStencilBits = 0;
		///
		///			// no auxiliary buffer
		///			pfd.cAuxBuffers = 0;
		///
		///			// main layer
		///			pfd.iLayerType = Gdi.PFD_MAIN_PLANE;
		///
		///			// reserved
		///			pfd.bReserved = 0;
		///
		///			// layer masks ignored
		///			pfd.dwLayerMask = 0;
		///			pfd.dwVisibleMask = 0;
		///			pfd.dwDamageMask = 0;
		///
		///			pixelFormat = Gdi.ChoosePixelFormat(hdc, &amp;pfd);
		///			
		///			// make that the pixel format of the device context
		///			Gdi.SetPixelFormat(hdc, pixelFormat, &amp;pfd);
		///		</code>
		/// </remarks>
		/// <seealso cref="ChoosePixelFormat" />
	
		[DllImport(GDI_NATIVE_LIBRARY, EntryPoint="SetPixelFormat", SetLastError=true)]// CLSCompliant(false), SuppressUnmanagedCodeSecurity]
		public static extern bool SetPixelFormat(IntPtr deviceContext, int pixelFormat, ref PIXELFORMATDESCRIPTOR pixelFormatDescriptor);

		#endregion BOOL SetPixelFormat(HDC hdc, int iPixelFormat, PIXELFORMATDESCRIPTOR* ppfd)
		#region IntPtr GetDC(IntPtr windowHandle)
		/// <summary>
		///     <para>
		///         The <b>GetDC</b> function retrieves a handle to a display device context (DC)
		///         for the client area of a specified window or for the entire screen.  You can
		///         use the returned handle in subsequent GDI functions to draw in the DC.
		///     </para>
		///     <para>
		///         The <b>GetDCEx </b> function is an extension to <b>GetDC</b>, which
		///         gives an application more control over how and whether clipping occurs in the
		///         client area.
		///     </para>
		/// </summary>
		/// <param name="windowHandle">
		///     <para>
		///         Handle to the window whose DC is to be retrieved.  If this value is null,
		///         <b>GetDC</b> retrieves the DC for the entire screen.
		///     </para>
		///     <para>
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         If the function succeeds, the return value is a handle to the DC for the
		///         specified window's client area.
		///     </para>
		///     <para>
		///         If the function fails, the return value is null.
		///     </para>
		///     <para>
		///         <b>Windows NT/2000/XP:</b> To get extended error information, call
		///         <see cref="Marshal.GetLastWin32Error" />.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The <b>GetDC</b> function retrieves a common, class, or private DC depending
		///         on the class style of the specified window.  For class and private DCs,
		///         <b>GetDC</b> leaves the previously assigned attributes unchanged.  However,
		///         for common DCs, <b>GetDC</b> assigns default attributes to the DC each time
		///         it is retrieved.  For example, the default font is System, which is a bitmap
		///         font.  Because of this, the handle for a common DC returned by <b>GetDC</b>
		///         does not tell you what font, color, or brush was used when the window was
		///         drawn. 
		///     </para>
		///     <para>
		///         Note that the handle to the DC can only be used by a single thread at any one
		///         time.
		///     </para>
		///     <para>
		///         After painting with a common DC, the <see cref="ReleaseDC" /> function must
		///         be called to release the DC.  Class and private DCs do not have to be
		///         released.  <see cref="ReleaseDC" /> must be called from the same thread that
		///         called <b>GetDC</b>.  The number of DCs is limited only by available memory.
		///     </para>
		///     <para>
		///         <b>Windows 95/98/Me:</b> There are only 5 common DCs available per thread,
		///         thus failure to release a DC can prevent other applications from accessing
		///         one.
		///     </para>
		/// </remarks>
		/// <seealso cref="ReleaseDC" />
		// WINUSERAPI HDC WINAPI GetDC(IN HWND hWnd);
		[DllImport(USER_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetDC(IntPtr windowHandle);
		#endregion IntPtr GetDC(IntPtr windowHandle)

		#region bool ReleaseDC(IntPtr windowHandle, IntPtr deviceContext)
		/// <summary>
		///     <para>
		///         The <b>ReleaseDC</b> function releases a device context (DC), freeing it for
		///         use by other applications.  The effect of the <b>ReleaseDC</b> function
		///         depends on the type of DC.  It frees only common and window DCs.  It has no
		///         effect on class or private DCs.
		///     </para>
		/// </summary>
		/// <param name="windowHandle">
		///     <para>
		///         Handle to the window whose DC is to be released.
		///     </para>
		/// </param>
		/// <param name="deviceContext">
		///     <para>
		///         Handle to the DC to be released.
		///     </para>
		/// </param>
		/// <returns>
		///     <para>
		///         The return value indicates whether the DC was released.  If the DC was
		///         released, the return value is true.
		///     </para>
		///     <para>
		///         If the DC was not released, the return value is false.
		///     </para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The application must call the <b>ReleaseDC</b> function for each call to the
		///			each call to the <see cref="GetDC" /> function that retrieves a common DC.
		///     </para>
		/// 
		/// </remarks>
		/// <seealso cref="GetDC" />
		// WINUSERAPI int WINAPI ReleaseDC(IN HWND hWnd, IN HDC hDC);
		[DllImport(USER_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern bool ReleaseDC(IntPtr windowHandle, IntPtr deviceContext);
		#endregion bool ReleaseDC(IntPtr windowHandle, IntPtr deviceContext)
		/// <summary>
		/// Swaps the double buffer for the frame buffer.
		/// </summary>
		/// <param name="deviceContext">Devicecontext</param>
		/// <returns></returns>
		[DllImport(GDI_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		
		public static extern bool SwapBuffers(IntPtr deviceContext);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deviceContext"></param>
		/// <returns></returns>
		[DllImport(GDI_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION, EntryPoint="SwapBuffers"), SuppressUnmanagedCodeSecurity]
		private static extern int SwapBuffersFast([In] IntPtr deviceContext);
		[DllImport(GDI_NATIVE_LIBRARY, EntryPoint="SetPixelFormat", SetLastError=true)]//, CLSCompliant(false), SuppressUnmanagedCodeSecurity]
		private static extern bool _SetPixelFormat(IntPtr deviceContext, int pixelFormat, ref PIXELFORMATDESCRIPTOR pixelFormatDescriptor);
	

		// --- Public Externs ---
		#region int ChoosePixelFormat(HDC hdc, PIXELFORMATDESCRIPTOR* ppfd)
		/// <summary>
		/// The <b>ChoosePixelFormat</b> function attempts to match an appropriate pixel format supported by a device context
		/// to a given pixel format specification.
		/// </summary>
		/// <param name="deviceContext">
		/// Specifies the device context that the function examines to determine the best match for the pixel format
		/// descriptor pointed to by <i>ppfd</i>.
		/// </param>
		/// <param name="pixelFormatDescriptor">
		/// <para>
		///		Pointer to a <see cref="FeaturesW32.PIXELFORMATDESCRIPTOR" /> structure that specifies the requested pixel format.
		///		In this context, the members of the <b>PIXELFORMATDESCRIPTOR</b> structure that <i>ppfd</i>
		///		points to are used as follows:
		///	</para>
		///	<para>
		///		<b>nSize</b><br />
		///		Specifies the size of the <b>PIXELFORMATDESCRIPTOR</b> data structure. Set this member to
		///		<c>sizeof(PIXELFORMATDESCRIPTOR)</c>.
		///	</para>
		///	<para>
		///		<b>nVersion</b><br />
		///		Specifies the version number of the <b>PIXELFORMATDESCRIPTOR</b> data structure. Set this member to 1.
		///	</para>
		///	<para>
		///		<b>dwFlags</b><br />
		///		A set of bit flags that specify properties of the pixel buffer. You can combine the following bit
		///		flag constants by using bitwise-OR.<br /><br />
		///		If any of the following flags are set, the <b>ChoosePixelFormat</b> function attempts to match pixel
		///		formats that also have that flag or flags set. Otherwise, <b>ChoosePixelFormat</b> ignores that flag
		///		in the pixel formats:<br /><br />
		///		PFD_DRAW_TO_WINDOW<br />
		///		PFD_DRAW_TO_BITMAP<br />
		///		PFD_SUPPORT_GDI<br />
		///		PFD_SUPPORT_OPENGL<br /><br />
		///		If any of the following flags are set, <b>ChoosePixelFormat</b> attempts to match pixel formats that
		///		also have that flag or flags set. Otherwise, it attempts to match pixel formats without that flag set:<br /><br />
		///		PFD_DOUBLEBUFFER<br />
		///		PFD_STEREO<br /><br />
		///		If the following flag is set, the function ignores the PFD_DOUBLEBUFFER flag in the pixel formats:<br /><br />
		///		PFD_DOUBLEBUFFER_DONTCARE<br /><br />
		///		If the following flag is set, the function ignores the PFD_STEREO flag in the pixel formats:<br /><br />
		///		PFD_STEREO_DONTCARE<br />
		///	</para>
		///	<para>
		///		<b>iPixelType</b><br />
		///		Specifies the type of pixel format for the function to consider:<br /><br />
		///		PFD_TYPE_RGBA<br />
		///		PFD_TYPE_COLORINDEX<br />
		///	</para>
		///	<para>
		///		<b>cColorBits</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>cRedBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cRedShift</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cGreenBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cGreenShift</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cBlueBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cBlueShift</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cAlphaBits</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>cAlphaShift</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cAccumBits</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>cAccumRedBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cAccumGreenBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cAccumBlueBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cAccumAlphaBits</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>cDepthBits</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>cStencilBits</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>cAuxBuffers</b><br />
		///		Zero or greater.
		///	</para>
		///	<para>
		///		<b>iLayerType</b><br />
		///		Specifies one of the following layer type values:<br /><br />
		///		PFD_MAIN_PLANE<br />
		///		PFD_OVERLAY_PLANE<br />
		///		PFD_UNDERLAY_PLANE<br />
		///	</para>
		///	<para>
		///		<b>bReserved</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>dwLayerMask</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>dwVisibleMask</b><br />
		///		Not used.
		///	</para>
		///	<para>
		///		<b>dwDamageMask</b><br />
		///		Not used.
		///	</para>
		/// </param>
		/// <returns>
		///		If the function succeeds, the return value is a pixel format index (one-based) that is the closest match
		///		to the given pixel format descriptor.<br /><br />
		///		If the function fails, the return value is zero. 
		/// </returns>
		/// <remarks>
		///		You must ensure that the pixel format matched by the <b>ChoosePixelFormat</b> function satisfies your
		///		requirements. For example, if you request a pixel format with a 24-bit RGB color buffer but the device
		///		context offers only 8-bit RGB color buffers, the function returns a pixel format with an 8-bit RGB color
		///		buffer.<br /><br />
		///		The following code sample shows how to use <b>ChoosePixelFormat</b> to match a specified pixel
		///		format:<br /><br />
		///		<code>
		///			HDC hdc;
		///			int pixelFormat;
		///			FeaturesW32.PIXELFORMATDESCRIPTOR pfd;
		///
		///			// size of this pfd
		///			pfd.nSize = (IndexType) sizeof(FeaturesW32.PIXELFORMATDESCRIPTOR);
		///
		///			// version number
		///			pfd.nVersion = 1;
		///
		///			// support window, support OpenGL, double buffered
		///			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
		///
		///			// RGBA type
		///			pfd.iPixelType = Gdi.PFD_TYPE_RGBA;
		///
		///			// 24-bit color depth
		///			pfd.cColorBits = 24;
		///
		///			// color bits and shift bits ignored
		///			pfd.cRedBits = 0;
		///			pfd.cRedShift = 0;
		///			pfd.cGreenBits = 0;
		///			pfd.cGreenShift = 0;
		///			pfd.cBlueBits = 0;
		///			pfd.cBlueShift = 0;
		///			pfd.cAlphaBits = 0;
		///			pfd.cAlphaShift = 0;
		///
		///			// no accumulation buffer, accum bits ignored
		///			pfd.cAccumBits = 0;
		///			pfd.cAccumRedBits = 0;
		///			pfd.cAccumGreenBits = 0;
		///			pfd.cAccumBlueBits = 0;
		///			pfd.cAccumAlphaBits = 0;
		///
		///			// no stencil buffer
		///			pfd.cStencilBits = 0;
		///
		///			// no auxiliary buffer
		///			pfd.cAuxBuffers = 0;
		///
		///			// main layer
		///			pfd.iLayerType = Gdi.PFD_MAIN_PLANE;
		///
		///			// reserved
		///			pfd.bReserved = 0;
		///
		///			// layer masks ignored
		///			pfd.dwLayerMask = 0;
		///			pfd.dwVisibleMask = 0;
		///			pfd.dwDamageMask = 0;
		///
		///			pixelFormat = Gdi.ChoosePixelFormat(hdc, &amp;pfd);
		///		</code>
		/// </remarks>
		/// <seealso cref="SetPixelFormat" />
		[DllImport(GDI_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern int ChoosePixelFormat(IntPtr deviceContext, ref PIXELFORMATDESCRIPTOR pixelFormatDescriptor);
		#endregion int ChoosePixelFormat(HDC hdc, PIXELFORMATDESCRIPTOR* ppfd)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="height"></param>
		/// <param name="width"></param>
		/// <param name="escapement"></param>
		/// <param name="orientation"></param>
		/// <param name="weight"></param>
		/// <param name="italic"></param>
		/// <param name="underline"></param>
		/// <param name="strikeOut"></param>
		/// <param name="charSet"></param>
		/// <param name="outputPrecision"></param>
		/// <param name="clipPrecision"></param>
		/// <param name="quality"></param>
		/// <param name="pitchAndFamily"></param>
		/// <param name="typeFace"></param>
		/// <returns></returns>
		[DllImport(GDI_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateFont(int height, int width, int escapement, int orientation, int weight, bool italic, bool underline, bool strikeOut, int charSet, int outputPrecision, int clipPrecision, int quality, int pitchAndFamily, string typeFace);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectHandle"></param>
		/// <returns></returns>
		[DllImport(GDI_NATIVE_LIBRARY, SetLastError=true), SuppressUnmanagedCodeSecurity]
		public static extern bool DeleteObject(IntPtr objectHandle);
		#region GLYPHMETRICSFLOAT Struct
		/// <summary>
		/// The <b>GLYPHMETRICSFLOAT</b> structure contains information about the placement and orientation of a glyph in a
		/// character cell.
		/// </summary>
		/// <remarks>The values of <b>GLYPHMETRICSFLOAT</b> are specified as notional units.</remarks>
		/// <seealso cref="POINTFLOAT" />
		/// <seealso cref="wglUseFontOutlines" />
		[StructLayout(LayoutKind.Sequential)]
			public struct GLYPHMETRICSFLOAT 
		{
			/// <summary>
			/// Specifies the width of the smallest rectangle (the glyph's black box) that completely encloses the glyph.
			/// </summary>
			public float gmfBlackBoxX;

			/// <summary>
			/// Specifies the height of the smallest rectangle (the glyph's black box) that completely encloses the glyph.
			/// </summary>
			public float gmfBlackBoxY;

			/// <summary>
			/// Specifies the x and y coordinates of the upper-left corner of the smallest rectangle that completely encloses the glyph.
			/// </summary>
			public POINTFLOAT gmfptGlyphOrigin;

			/// <summary>
			/// Specifies the horizontal distance from the origin of the current character cell to the origin of the next character cell.
			/// </summary>
			public float gmfCellIncX;

			/// <summary>
			/// Specifies the vertical distance from the origin of the current character cell to the origin of the next character cell.
			/// </summary>
			public float gmfCellIncY;
		};
		#endregion GLYPHMETRICSFLOAT Struct
       #region POINTFLOAT Struct
        /// <summary>
        /// The <b>POINTFLOAT</b> structure contains the x and y coordinates of a point.
        /// </summary>
        /// <seealso cref="GLYPHMETRICSFLOAT" />
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTFLOAT {
            /// <summary>
            /// Specifies the horizontal (x) coordinate of a point.
            /// </summary>
            public float X;

            /// <summary>
            /// Specifies the vertical (y) coordinate of a point.
            /// </summary>
            public float Y;
        };
        #endregion POINTFLOAT Struct

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deviceContext"></param>
		/// <param name="objectHandle"></param>
		/// <returns></returns>
		[DllImport(GDI_NATIVE_LIBRARY), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SelectObject(IntPtr deviceContext, IntPtr objectHandle);

	
        /// <summary>
        /// The TTPOLYGONHEADER structure specifies the starting position and type of a contour in a TrueType character outline.
        /// </summary>
		[StructLayout(LayoutKind.Sequential)]

			public struct TTPOLYGONHEADER
		{
            /// <summary>
            /// The number of bytes required by the TTPOLYGONHEADER structure and TTPOLYCURVE structure or structures required to describe the contour of the character.
            /// </summary>
			public int cb;
            /// <summary>
            /// The type of character outline returned. Currently, this value must be TT_POLYGON_TYPE.
            /// </summary>
			public int dwType;
            /// <summary>
            /// The starting point of the contour in the character outline.
            /// </summary>
			[MarshalAs(UnmanagedType.Struct)] public POINTFX pfxStart;
		}
        /// <summary>
        /// 
        /// </summary>
		[StructLayout(LayoutKind.Sequential)]

			public struct TTPOLYCURVEHEADER
		{
/// <summary>
/// 
/// </summary>
			public short wType;
        /// <summary>
        /// 
        /// </summary>
			public short cpfx;
		}
			/// <summary>
			/// Windows structure
			/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct FIXED
		{
			/// <summary>
			/// Fraction
			/// </summary>
			public short Fract;
			/// <summary>
			/// value
			/// </summary>
			public short value;

		}
		/// <summary>
		/// Windows structure
		/// </summary>

		[StructLayout(LayoutKind.Sequential)]
			public struct POINTFX
		{
			/// <summary>
			/// x-value
			/// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED x;
			/// <summary>
			/// y-value
			/// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED y;
		}
		/// <summary>
		/// Windows structure, which describes a glyph>
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct GLYPHMETRICS
		{
			/// <summary>
			/// Blackboxx
			/// </summary>
			public int gmBlackBoxX;
			/// <summary>
			/// Blackboxy
			/// </summary>
			public int gmBlackBoxY;
			/// <summary>
			/// Glyphorigin
			/// </summary>
			[MarshalAs(UnmanagedType.Struct)] public POINTFX gmptGlyphOrigin;
			/// <summary>
			/// Cellincrement x
			/// </summary>
			public short gmCellIncX;
			/// <summary>
			/// Cellincrement y
			/// </summary>
			public short gmCellIncY;
		}
        /// <summary>
        /// The MAT2 structure contains the values for a transformation matrix used by the <see cref="GetGlyphOutlineW"/> function.
        /// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct MAT2
		{
            /// <summary>
            /// 
            /// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM11;
            /// <summary>
            /// 
            /// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM12;
            /// <summary>
            /// 
            /// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM21;
            /// <summary>
            /// 
            /// </summary>
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM22;
		}

		/// <summary>
		/// Gets the Matrixs of a font
		/// </summary>
		/// <param name="hDC">Dc who has a font</param>
		/// <param name="lpMetrics">Metrics <see cref="TEXTMETRIC"/></param>
		/// <returns></returns>
		[DllImport("gdi32", EntryPoint="GetTextMetrics")] 
		public static extern int GetTextMetrics(IntPtr hDC, ref TEXTMETRIC lpMetrics);
		/// <summary>
		/// Get the outline of a char.
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="uChar"></param>
		/// <param name="uFormat"></param>
		/// <param name="lpgm"></param>
		/// <param name="cbBuffer"></param>
		/// <param name="lpvBuffer"></param>
		/// <param name="lpmat2"></param>
		/// <returns></returns>
	//	[DllImport("gdi32.dll")]
	//	private static extern int GetGlyphOutline(IntPtr hdc, uint uChar, uint uFormat,
	//		out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);
        [DllImport( "gdi32.dll" )]
        public static extern int GetGlyphOutlineW( IntPtr hdc, uint uChar, uint uFormat,
            out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2 );
        /// <summary>
        /// Stores the curves and the x-increment of a char
        /// </summary>
        [Serializable]
        public class GlyphInfo
		{
		
			/// <summary>
			/// Curves
			/// </summary>
			public Loca Curves;
			/// <summary>
			/// x-increment
			/// </summary>
			public double Deltax;
            


		}
        /// <summary>
        /// Converts a <see cref="FIXED"/>-number to a double.
        /// </summary>
        /// <param name="f">The fixed number, that will be casted.</param>
        /// <returns></returns>
		public static double FixedToNumber (FIXED f)
		{
			double result = f.value + ((double)f.Fract/(double)65536.0);
			if ( result != f.value)
			{
				result = 1;
			}

			return f.value + ((double)f.Fract/(double)65536.0);
		}


		/// <summary>
		/// The method retrieves a Loca, which holds the curves of a character in the font style, given by the parameter
		/// 'Font'. CellIncx gets the distance to a following next character.
		/// <code>
		/// ...
		/// double CellIncX;
		/// Font F = new Font(FontName, 30);
		/// // set attributes
		/// Font.Style = System.Drawing.FontStyle.Bold; // for example
		/// Loca L = GetOutLine( F , 'S', out CellIncX);
		/// device.DrawPolyPolyCurve(L); // draws the character to the device
		/// 
		/// </code>
		/// </summary>
		/// <param name="Font">A font which determines the outlook of the char</param>
		/// <param name="c">Character for which the contour will be determined</param>
		/// <param name="CellIncX">Increment to the next character</param>
		/// <returns>A loca which holds the curves of a character</returns>
        public static Loca GetOutLine(System.Drawing.Font Font, Char c, out double CellIncX)
		{

			GLYPHMETRICS metrics = new GLYPHMETRICS();
            Loca L = GetOutLine(Font, c, 1, out metrics);
			CellIncX = (float)metrics.gmCellIncX;
			return L;

		}
		/// <summary>
		/// Contains all loaded fonts. GetValueList holds <see cref="FontItem"/>.
		/// </summary>
		public static ArrayList LoadedFonts = new ArrayList();
        /// <summary>
        /// Find the index of a Font in the <see cref="LoadedFonts"/>
        /// </summary>
        /// <param name="Name">Name of the Font</param>
        /// <param name="FS">Fontsyle of the font</param>
        /// <returns>Index in the LoadedFonts array, if it exists else -1.
        /// </returns>
        public static int FindFont(string Name, FontStyle FS)
        {
            for (int i = 0; i < LoadedFonts.Count ; i++)
			{
			 FontItem FI = LoadedFonts[i] as FontItem;

               if ((FI.Name.ToUpper()==Name.ToUpper())
                   &&(FI.FontStyle == FS)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Describes a loaded font <seealso cref="LoadedFonts"/>
        /// </summary>
        [Serializable]
        public class FontItem
		{
			/// <summary>
			/// Contains geometric infos about chars.
			/// </summary>
			public GlyphInfo[] GlyphInfo = new GlyphInfo[256];
			/// <summary>
			/// Contains metric infos about the font.
			/// </summary>
			public TEXTMETRIC lpMetrics;
			/// <summary>
			/// Contains metric infos about the chars.
			/// </summary>

			public GLYPHMETRICSFLOAT[] agmf = new GLYPHMETRICSFLOAT[256];
            /// <summary>
            /// The FontStyle
            /// </summary>
            public FontStyle FontStyle;
            /// <summary>
            /// The name of the font.
            /// </summary>
            public String Name;
            /// <summary>
            /// If the font type is modern this field has value true.
            /// </summary>
            public bool IsModernType = false;
            /// <summary>
            /// Opengl stores a displaylist of then font, which will be drawn only with his contur. 
            /// </summary>
            public int DisplayListLine = -1;
            
           // public bool[] HasDisplayListsLine = new bool[256];
            /// <summary>
            /// Opengl stores in this value the displaylist of a font, which will be filled drawn.
            /// </summary>
            public int DisplayListFill = -1;
            /// <summary>
            /// OpenGl looks in this array, whether a displaylist for a char is loaded.
            /// </summary>
            public bool[] HasDisplayListsFill = new bool[256];


		}

        private static Loca GetOutLine(System.Drawing.Font Font, Char c, double Scale, out GLYPHMETRICS metrics)
		{
			Bitmap b = new Bitmap(1,1);
			Graphics g = Graphics.FromImage(b);
			IntPtr hdc = g.GetHdc(); 
			IntPtr FHdc = Font.ToHfont();		
		   
			SelectObject(hdc,FHdc);
			TEXTMETRIC lpMetrics = new TEXTMETRIC();
			GetTextMetrics(hdc, ref  lpMetrics);
          
			Loca L = GetOutLine(hdc , Scale, c, out metrics);
			g.ReleaseHdc(hdc);
			return L;
		}
        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="DC"></param>
        /// <param name="Scale"></param>
        /// <param name="yTrans"></param>
        /// <param name="c"></param>
        /// <param name="metrics"></param>
        /// <returns></returns>
        internal static Loca GetModernOutLine(IntPtr DC, double Scale,double yTrans, Char c, out GLYPHMETRICS metrics)
        {
            Loca Result = new Loca();

            // Describe a 24-point truetype font of normal weight


            // use a path to record how the text was drawn
            
            BeginPath(DC);
            //TextOut(DC, 100, 100, c.ToString(), 1);
            TextOut(DC, 0, 0, c.ToString(), 1);
            EndPath(DC);
            metrics = new GLYPHMETRICS();
            Point P = new Point(0,0);
            FeaturesW32.GetTextExtentPoint32(DC, c.ToString(),1, ref P);
            metrics.gmCellIncX=(short)P.X;

            metrics.gmBlackBoxX = (short)P.X;
            metrics.gmBlackBoxY = (short)P.Y;
            metrics.gmCellIncX = (short)P.X;
            metrics.gmCellIncY = (short)P.Y;


            double ox = (float)metrics.gmptGlyphOrigin.x.Fract / (float)(metrics.gmBlackBoxY);
            double oy = (float)metrics.gmptGlyphOrigin.y.Fract / (float)(metrics.gmBlackBoxY);
                      
            // Find out how many points are in the path. Note that
            // for long strings or complex fonts, this number might be
            // gigantic!
            int nNumPts = GetPath(DC, IntPtr.Zero, IntPtr.Zero, 0);
            if (nNumPts == 0)
                return Result;

            // Allocate memory to hold points and stroke types from
            // the path.
            Point[] lpPoints = new Point[nNumPts];
            if (lpPoints.Length == 0)
                return Result;
            byte[] lpTypes = new byte[nNumPts];

            // Now that we have the memory, really get the path data.

            //nNumPts = pDC->
            nNumPts = GetPath(DC, ref lpPoints[0], ref lpTypes[0], nNumPts);

            
            const int PT_CLOSEFIGURE = 1;
            const int PT_LINETO = 2;
            const int PT_BEZIERTO = 4;
            const int PT_BEZIERTOANDCLOSE = 5;
            const int PT_MOVETO = 6;
            const int PT_LINETOANDCLOSEFIGURE = 3;

            // Start
            CurveArray Current = new CurveArray();
            Result.Add(Current);

            xy CurrentPoint = new xy(-100, -100);



            if (nNumPts != -1)
                for (int i = 0; i < lpPoints.Length; i++)
                {

                    switch (lpTypes[i])
                    {

                        case PT_CLOSEFIGURE:
                            Line L1 =(Line)Current[Current.Count-1];
                            Line L2 =(Line)Current[0];
                            Current.Add(new Line(L1.B * (1 / Scale), L2.A * (1 / Scale)));

                            break;
                        case PT_LINETO:
                            Current.Add(new Line(CurrentPoint * (1 / Scale), new xy(lpPoints[i].X * (1 / Scale), lpPoints[i].Y * (1 / Scale))));
                            CurrentPoint = new xy(lpPoints[i].X, lpPoints[i].Y);
                            
                            break;

                        case PT_LINETOANDCLOSEFIGURE:
                            Current.Add(new Line(CurrentPoint * (1 / Scale), new xy(lpPoints[i].X * (1 / Scale), lpPoints[i].Y * (1 / Scale))));
                            CurrentPoint = new xy(lpPoints[i].X, lpPoints[i].Y);
                            L1 = (Line)Current[Current.Count - 1];
                            L2 = (Line)Current[0];
                            Current.Add(new Line(L1.B * (1 / Scale), L2.A * (1 / Scale)));
                            break; // Lineto and Close

                        case PT_BEZIERTO:
                            Current.Add(new Line(CurrentPoint * (1 / Scale), new xy(lpPoints[i].X * (1 / Scale), lpPoints[i].Y * (1 / Scale))));
                            CurrentPoint = new xy(lpPoints[i].X, lpPoints[i].Y);
                            break;
                        case PT_BEZIERTOANDCLOSE:
                            Current.Add(new Line(CurrentPoint * (1 / Scale), new xy(lpPoints[i].X * (1 / Scale), lpPoints[i].Y * (1 / Scale))));
                            CurrentPoint = new xy(lpPoints[i].X, lpPoints[i].Y);
                            L1 = (Line)Current[Current.Count - 1];
                            L2 = (Line)Current[0];
                            Current.Add(new Line(L1.B * (1 / Scale), L2.A * (1 / Scale)));
                            break; // Lineto and Close

                        case PT_MOVETO:

                            Current = new CurveArray();
                            Result.Add(Current);
                            CurrentPoint = new xy(lpPoints[i].X , lpPoints[i].Y );
                            break;

                    }
                }
            if (Result.Count >= 2)
            {
                Result.RemoveAt(0);
                Result.RemoveAt(0);
            }
            
            Result.Transform(Matrix3x3.Mirror(new xy(0,0.5),new xy(1,0.5)));
           Result.Transform(Matrix3x3.Translation(new xy(0,-yTrans)));

            return Result;
        }
        /// <summary>
        /// Loads a character of a font and creates a <see cref="Drawing3d.Loca"/> for the outlined curves.
        /// </summary>
        /// <param name="hdc">A device context, which holds the font</param>
        /// <param name="Scale">The outlined curves will be scaled by this factor</param>
        /// <param name="c">The chareacter, for which a <see cref="Drawing3d.Loca"/> will be created.</param>
        /// <param name="metrics">Metrics of the font</param>
        /// <returns></returns>
		public static Loca GetOutLine(IntPtr hdc, double Scale, Char c, out GLYPHMETRICS metrics)
		{
			Loca L = new Loca(0);

			POINTFX pfxB ;
			POINTFX pfxC ;
			bool firstInList = true;
			xy pA = new xy(0,0);
			xy pB = new xy(0,0);
			xy pC = new xy(0,0);
			xy ipA = new xy(0,0);
			xy ipB = new xy(0,0);
			xy startP = new xy(0,0);
			Line Li2;
			CurveArray ca2;
			

				
			//		GLYPHMETRICS metrics = new GLYPHMETRICS(); 
            
			MAT2 matrix = new MAT2();
			matrix.eM11.value = 1;
			matrix.eM12.value = 0;
			matrix.eM21.value = 0;
			matrix.eM22.value = 1;
	
		
			int bufferSize = FeaturesW32.GetGlyphOutlineW(hdc, (uint)c, (uint)2, out metrics, 0, IntPtr.Zero, ref matrix);
			if (bufferSize <=0) 
				return L;
			IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
			try
			{            
				int ret;
						
				if((ret = FeaturesW32.GetGlyphOutlineW(hdc, (uint)c, 2, out metrics, (uint)bufferSize, buffer, ref matrix)) > 0)
				{                            
					int polygonHeaderSize = Marshal.SizeOf(typeof(TTPOLYGONHEADER));
					int curveHeaderSize = Marshal.SizeOf(typeof(TTPOLYCURVEHEADER));
					int pointFxSize = Marshal.SizeOf(typeof(POINTFX));

					int index = 0;
					while(index < bufferSize)
					{                                
						L.Count= L.Count+1;
						TTPOLYGONHEADER header = (TTPOLYGONHEADER)Marshal.PtrToStructure(new IntPtr(buffer.ToInt32()+index), typeof(TTPOLYGONHEADER));
						startP.x = FixedToNumber (header.pfxStart.x);
						startP.y = FixedToNumber (header.pfxStart.y);
						ipB = startP;
						int endCurvesIndex = index+header.cb;
						index+=polygonHeaderSize;
						if (firstInList)
						{
							ipA = startP;
							ipB = ipA;
							firstInList = false;
						}

						while(index < endCurvesIndex)
						{
							TTPOLYCURVEHEADER curveHeader = (TTPOLYCURVEHEADER)Marshal.PtrToStructure(new IntPtr(buffer.ToInt32()+index), typeof(TTPOLYCURVEHEADER));
							index+=curveHeaderSize;
							POINTFX[] curvePoints = new POINTFX[curveHeader.cpfx];

							for(int i = 0; i < curveHeader.cpfx; i++)
							{
								curvePoints[i] = (POINTFX)Marshal.PtrToStructure(new IntPtr(buffer.ToInt32()+index), typeof(POINTFX));
								index+=pointFxSize;
							}

							if(curveHeader.wType == (int)1)
							{
								// POLYLINE
								for(int i=0; i < curveHeader.cpfx; i++)
								{
									double x = FixedToNumber (curvePoints[i].x);
									double y = FixedToNumber (curvePoints[i].y);

									ipB = new xy(x, y);
									Li2 = new Line (ipA*(1/Scale), ipB* (1/Scale));
									ipA = ipB;
									CurveArray ca = L[L.Count-1];
									ca.Add(Li2);
												
								}
							}
							else
							{
                               if (curveHeader.wType!=2)
                                { }

                                pA = ipB;
								// CURVE
								for(int i=0; i < curveHeader.cpfx-1 ; i++)
								{
									pfxB = curvePoints[i];
									pfxC = curvePoints[i+1];
												  
									pC.x = FixedToNumber (pfxB.x);
									pC.y = FixedToNumber (pfxB.y);
												 
									if(i == curveHeader.cpfx-1)
									{
										pB.x = FixedToNumber (pfxB.x);
										pB.y = FixedToNumber (pfxB.y);
									}
									else if (curveHeader.cpfx == 2) 
									{
										pB.x = FixedToNumber (pfxC.x);
										pB.y = FixedToNumber (pfxC.y);
									}
									else 
									{
										if (i == curveHeader.cpfx - 2) 
										{
											pB.x = FixedToNumber (pfxC.x);
											pB.y = FixedToNumber (pfxC.y);
										}
										else
										{
											pB.x = (FixedToNumber (pfxB.x)+FixedToNumber (pfxC.x)) /(float) 2;
											pB.y = (FixedToNumber (pfxB.y)+FixedToNumber (pfxC.y)) /(float) 2;
										}
									};

									QSpline Li = new QSpline(pA*(1/Scale), pB*(1/Scale), pC*(1/Scale));
									ipA = pB; ipB = pB; pA = pB;
									ca2 = L[L.Count-1];
									ca2.Add(Li);
								}
							}
							if (index == endCurvesIndex)
							{
								ipA = ipB;
								ipB = startP;
								Li2 = new Line (ipA*(1/Scale), ipB* (1/Scale));
								ipA = ipB;
								ca2 = L[L.Count-1];
								ca2.Add(Li2);
							}										
						}   
						firstInList = true;
					}
				}
				else
				{
					throw new Exception("Could not retrieve glyph (GDI Error: 0x"+ret.ToString("X")+")");
				}

			}
			finally
			{                        
				Marshal.FreeHGlobal(buffer);                    
			}
            if ((L.Count > 0) && (!L[0].ClockWise))
                L.Invert();
			return L;
		}
		/// <summary>
		/// The method retrieves a Loca, which holds the curves of a character in the font, given by
		/// 'Fontname'. CellIncx gets the distance to a following next character.
		/// <code>
		/// ...
		/// double CellIncX;
		/// Loca L = GetOutLine("Arial", 'S', out CellIncX);
		/// device.DrawPolyPolyCurve(L); // draws the character to the device
		/// 
		/// </code>
		/// </summary>
		/// <param name="FontName">Name of the specific Font</param>
		/// <param name="c">Character for which the contour will be determined</param>
		/// <param name="CellIncX">Increment to the next character</param>
		/// <returns>A loca which holds the curves of a character</returns>
		public static Loca GetOutLine(string FontName, Char c, out double CellIncX)
		{
            System.Drawing.Font F = new System.Drawing.Font(FontName, 30);
           
			return GetOutLine(F, c ,  out CellIncX);
		}
        private static bool IsModernFont(IntPtr hdc)
        {
            
            GLYPHMETRICS Metrics = new GLYPHMETRICS();
            char c = 'A';
            MAT2 matrix = new MAT2();
            matrix.eM11.value = 1;
            matrix.eM12.value = 0;
            matrix.eM21.value = 0;
            matrix.eM22.value = 1;
            matrix.eM11.value = 1;
            matrix.eM12.value = 0;
            matrix.eM21.value = 0;
            matrix.eM22.value = 1;

             
            int b=FeaturesW32.GetGlyphOutlineW(hdc, (uint)c, (uint)2, out Metrics, 0, IntPtr.Zero, ref matrix);
            return (b < 0);
			
        }
        /// <summary>
        /// Loads a font, which is a stroke font like "modern"
        /// </summary>
        /// <param name="FontName"></param>
        /// <param name="FS"></param>
        public static void LoadModernFont(string FontName, FontStyle FS)
        {
            int id = FindFont(FontName, FS);
            if (id < 0)
            {
                Bitmap B = new Bitmap(500, 500);
                Graphics G = Graphics.FromImage(B);
                FontItem FI = new FontItem();
                FI.Name = FontName;
                FI.FontStyle = FS;


                
                IntPtr F = CreateFont(80, 0, 0, 0, 0, false, false,false, 255, 0, 0, 0, 16, FontName);

             
                IntPtr hdc = G.GetHdc();
                IntPtr FHdc = F;
                SelectObject(hdc, FHdc);
                GLYPHMETRICS Metrics;
                			
                GetTextMetrics(hdc, ref FI.lpMetrics);
                
                LoadedFonts.Add(FI);// 51 19
                double Scale = (float)FI.lpMetrics.tmHeight;
                double ModernyTrans = (float)FI.lpMetrics.tmDescent / Scale;
                for (int i = 0; i < 256; i++)
                {
                    try
                    {
                        FI.GlyphInfo[i] = new GlyphInfo();
                       // FI.GlyphInfo[i].Curves = GetOutLine(hdc, Scale, (Char)i, out Metrics);
                        FI.GlyphInfo[i].Curves = GetModernOutLine(hdc, Scale,ModernyTrans, (Char)i, out Metrics);
                        FI.GlyphInfo[i].Deltax = (float)(Metrics.gmCellIncX) / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfBlackBoxX = (float)Metrics.gmBlackBoxX / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfBlackBoxY = (float)Metrics.gmBlackBoxY / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfCellIncX = (float)Metrics.gmCellIncX / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfCellIncY = (float)Metrics.gmCellIncY / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfptGlyphOrigin.X = (float)Metrics.gmptGlyphOrigin.x.Fract / (float)(FI.lpMetrics.tmHeight);
                        FI.agmf[i].gmfptGlyphOrigin.Y = (float)Metrics.gmptGlyphOrigin.y.Fract / (float)(FI.lpMetrics.tmHeight);
                        FI.GlyphInfo[i].Curves.Transform(Matrix3x3.Translation(new xy(-FI.agmf[i].gmfptGlyphOrigin.X, -FI.agmf[i].gmfptGlyphOrigin.Y)));
                    }
                    catch
                    {
                        id = -1;

                    }

                }
                G.ReleaseHdc(hdc);
                DeleteObject(FHdc);
            }

        }
    
		/// <summary>
		/// Loads a font and stores a <see cref="FontItem"/> in the <see cref="LoadedFonts"/>-list.
		/// </summary>
		/// <param name="FontName"></param>
        /// <param name="FS">Fontstyle for the font, which will be loaded.</param>
		public static void LoadFont(string FontName,System.Drawing.FontStyle FS)
        {
            System.Drawing.Font FF = new System.Drawing.Font(FontName, 400, FS);
            bool ModernFont = (FF.Name.ToUpper() != FontName.ToUpper());
            FF.Dispose();
            int id = FindFont(FontName, FS);
			if (id < 0)
			{
                




                Bitmap B = new Bitmap(500,500);
                Graphics G = Graphics.FromImage(B);
                FontItem FI = new FontItem();
                FI.Name = FontName;
                FI.FontStyle = FS;
                
				
               ////Font F = new Font(FontName, 400, FS);
               ////* IntPtr F = CreateFont(400, 0, 0, 0, 0, ((FS&FontStyle.Italic)==FontStyle.Italic),
               ////                                         ((FS&FontStyle.Underline)==FontStyle.Underline),
               ////                                         ((FS&FontStyle.Strikeout)==FontStyle.Strikeout),
               ////                                         255, 0, 0, 0, 16, FontName);
               //// */
               //// IntPtr F = CreateFont(400, 0, 0, 0, 0,false,false,false,0,0,0,0,2,FontName);
               //// IntPtr F = CreateFont(400, 0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 2, FontName);
               ////                             /*            ((FS & FontStyle.Underline) == FontStyle.Underline),
               ////                                         ((FS & FontStyle.Strikeout) == FontStyle.Strikeout),
               ////                                         255, 0, 0, 0, 16, FontName);
               ////                              */
               ////IntPtr hdc = G.GetHdc();
               //////IntPtr FHdc = F.ToHfont();
               //// IntPtr FHdc = F;
 		
               //// IntPtr PP= SelectObject(hdc,FHdc);
              
                IntPtr F;
                if (ModernFont)
                   F = CreateFont(80, 0, 0, 0, 0, false, false,false, 255, 0, 0, 0, 16, FontName);
                else
                   //F = CreateFont(400, 0, 0, 0, 0, ((FS&FontStyle.Italic)==FontStyle.Italic),
                   //                                         ((FS&FontStyle.Underline)==FontStyle.Underline),
                   //                                         ((FS&FontStyle.Strikeout)==FontStyle.Strikeout),
                   //                                         0, 0, 0, 0, 2, FontName);
                   
                  F = CreateFont(400, 0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 2, FontName);
             
                IntPtr hdc = G.GetHdc();
                IntPtr FHdc = F;
                SelectObject(hdc, FHdc);
                System.Drawing.Font FFF = new System.Drawing.Font(FontName, 100);

                GLYPHMETRICS Metrics;
                FHdc = FFF.ToHfont();
                SelectObject(hdc, FHdc);
                GetTextMetrics(hdc, ref FI.lpMetrics);
               // Metrics.gmBlackBoxY = FI.lpMetrics.tmDescent;
               
                //bool ModernFont = (IsModernFont(hdc));
                


                
                //if (ModernFont)
                //{
                //    LoadModernFont(FontName, FS);
                //    return;
                //}
                
				LoadedFonts.Add(FI);
				double Scale = (float)FI.lpMetrics.tmHeight;
                double ModernyTrans = (float)FI.lpMetrics.tmDescent/Scale;
				for (int i= 0; i < 256; i++)
				{
					try
					{
                        FI.IsModernType = ModernFont;
						FI.GlyphInfo[i] = new GlyphInfo();
                        if (!ModernFont)
						FI.GlyphInfo[i].Curves=GetOutLine(hdc, Scale, (Char)i, out Metrics);
                        else
                        FI.GlyphInfo[i].Curves = GetModernOutLine(hdc, Scale, ModernyTrans, (Char)i, out Metrics);
                        if ((FI.GlyphInfo[i].Curves.Count >0)&&(FI.GlyphInfo[i].Curves[0].CrossProduct() >=0))
                         FI.GlyphInfo[i].Curves.Invert();


						FI.GlyphInfo[i].Deltax =(float) (Metrics.gmCellIncX)/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfBlackBoxX = (float)Metrics.gmBlackBoxX/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfBlackBoxY = (float)Metrics.gmBlackBoxY/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfCellIncX = (float)Metrics.gmCellIncX/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfCellIncY = (float)Metrics.gmCellIncY/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfptGlyphOrigin.X = (float)Metrics.gmptGlyphOrigin.x.Fract/(float)(FI.lpMetrics.tmHeight);
						FI.agmf[i].gmfptGlyphOrigin.Y = (float)Metrics.gmptGlyphOrigin.y.Fract/(float)(FI.lpMetrics.tmHeight);

					}
					catch

					{
						id = -1;

					}
				
				}
				G.ReleaseHdc(hdc);
				DeleteObject(FHdc);
			}
		
		}
		
	}
}
