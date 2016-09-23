﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct MSG
    {
        public IntPtr hWnd;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public Point pt;
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate IntPtr WNDPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
