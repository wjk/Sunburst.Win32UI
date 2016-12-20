﻿using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Handles;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public class CustomWindow : EventedWindow
    {
        private static int mTopmostControlTag = 1;
        private static readonly ConcurrentDictionary<int, CustomWindow> mControls = new ConcurrentDictionary<int, CustomWindow>();

        internal delegate IntPtr WndProcType(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        internal static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                const string ControlTagPropertyName = "Microsoft.Win32.UserInterface.CustomControl.Tag";

                if (msg == WindowMessages.WM_CREATE)
                {
                    CREATESTRUCT createStruct = Marshal.PtrToStructure<CREATESTRUCT>(lParam);
                    NativeMethods.SetProp(hWnd, ControlTagPropertyName, createStruct.lpCreateParams);
                }

                int tag = (int)NativeMethods.GetProp(hWnd, ControlTagPropertyName);
                CustomWindow control; mControls.TryGetValue(tag, out control);
                if (control == null) return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);

                // Assign the hWnd to the Dialog's Handle property here as well,
                // so it is set if it is used before CreateDialogParam() returns.
                if (msg == WindowMessages.WM_CREATE) control.Handle = hWnd;

                IntPtr result = control.ProcessMessage(msg, wParam, lParam);

                if (msg == WindowMessages.WM_DESTROY)
                {
                    CustomWindow ignored; mControls.TryRemove(tag, out ignored);
                    NativeMethods.RemoveProp(hWnd, ControlTagPropertyName);
                }

                return result;
            }
            catch (Exception ex)
            {
                Application.OnUnhandledException(ex);
                return IntPtr.Zero;
            }
        }

        public CustomWindow() : base() { }
        public CustomWindow(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        protected virtual IntPtr ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            bool handled;
            IntPtr result = ProcessCommonMessage(msg, wParam, lParam, out handled);
            if (handled) return result;
            
            return NativeMethods.DefWindowProc(Handle, msg, wParam, lParam);
        }

        public new void CreateHandle(Rect frame, string text, int style = 0, int extendedStyle = 0,
            Window parent = null, IMenuHandle hMenu = null)
        {
            if (WindowClassName == null) throw new InvalidOperationException($"{nameof(WindowClassName)} must be overridden");

            int tag = mTopmostControlTag++;
            mControls.TryAdd(tag, this);

            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
            Handle = NativeMethods.CreateWindowEx(extendedStyle, WindowClassName, text,
                style, frame.left, frame.top, frame.Width, frame.Height,
                parentHandle, hMenu?.Handle ?? IntPtr.Zero, IntPtr.Zero, (IntPtr)tag);
        }
    }
}
