﻿using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class GraphicsContext : NonOwnedGraphicsContext, IDisposable
    {
        public static GraphicsContext CreateOffscreenContext()
        {
            IntPtr desktopContext = NativeMethods.GetDC(IntPtr.Zero);
            var context = new GraphicsContext(NativeMethods.CreateCompatibleDC(desktopContext));
            NativeMethods.ReleaseDC(IntPtr.Zero, desktopContext);
            return context;
        }

        public GraphicsContext(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteDC(Handle);
        }
    }
}