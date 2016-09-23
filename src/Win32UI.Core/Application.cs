﻿using System;
using System.Collections.Generic;
using Microsoft.Win32.UserInterface.Events;
using Microsoft.Win32.UserInterface.Handles;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public sealed class Application
    {
        private static Stack<Window> mDialogBoxes = new Stack<Window>();
        private static Stack<Tuple<Window, IAcceleratorTableHandle>> mAcceleratorTables = new Stack<Tuple<Window, IAcceleratorTableHandle>>();

        public static void PushAcceleratorTable(Window hWnd, IAcceleratorTableHandle hAccel)
        {
            mAcceleratorTables.Push(new Tuple<Window, IAcceleratorTableHandle>(hWnd, hAccel));
        }

        public static void PopAcceleratorTable()
        {
            try
            {
                mAcceleratorTables.Pop();
            }
            catch (InvalidOperationException)
            {
                // Ignore the exception - trying to pop an accelerator-table handle when there is none is silently ignored.
            }
        }

        public static void PushDialog(Window hWnd)
        {
            mDialogBoxes.Push(hWnd);
        }

        public static void PopDialog()
        {
            try
            {
                mDialogBoxes.Pop();
            }
            catch (InvalidOperationException)
            {
                // Ignore the exception - trying to pop a dialog handle when there is none is silently ignored.
            }
        }

        public static int Run()
        {
            MSG msg = new MSG();

            while (NativeMethods.GetMessageW(out msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (mDialogBoxes.Count != 0)
                {
                    if (NativeMethods.IsDialogMessage(mDialogBoxes.Peek().Handle, ref msg)) continue;
                }

                if (mAcceleratorTables.Count != 0)
                {
                    var table = mAcceleratorTables.Peek();
                    if (NativeMethods.TranslateAcceleratorW(table.Item1.Handle, table.Item2.Handle, ref msg) != 0) continue;
                }

                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessageW(ref msg);
            }

            return (int)msg.wParam;
        }

        public static void Exit()
        {
            NativeMethods.PostQuitMessage(0);
        }

        public static EventHandler<UnhandledExceptionEventArgs> UnhandledException;

        public static void OnUnhandledException(Exception ex)
        {
            var args = new UnhandledExceptionEventArgs(ex);
            UnhandledException?.Invoke(null, args);

            // Don't ever return from this method, or else the program will most
            // likely crash in a far less dignified method.
            Environment.FailFast("Unhandled exception in .NET Core application", ex);
        }
    }
}
