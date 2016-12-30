﻿using System;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    public sealed class DeferWindowPos : IDisposable
    {
        public DeferWindowPos(int expectedControlCount)
        {
            Handle = NativeMethods.BeginDeferWindowPos(expectedControlCount);
        }

        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            NativeMethods.EndDeferWindowPos(Handle);
        }

        public void AddControl(Window windowToMove, Rect frame, DeferWindowPosFlags flags = 0)
        {
            NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, IntPtr.Zero,
                frame.left, frame.top, frame.Width, frame.Height, flags | DeferWindowPosFlags.IgnoreZOrder);
        }

        public void AddControl(Window windowToMove, Window windowToInsertZOrderAfter, Rect frame, DeferWindowPosFlags flags = 0)
        {
            NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, windowToInsertZOrderAfter.Handle,
                frame.left, frame.top, frame.Width, frame.Height, flags);
        }

        public void AddControl(Window windowToMove, ZOrderPosition specialZOrderPosition, Rect frame, DeferWindowPosFlags flags = 0)
        {
            IntPtr hWndSpecial;
            switch (specialZOrderPosition)
            {
                case ZOrderPosition.Top: hWndSpecial = NativeMethods.HWND_TOP; break;
                case ZOrderPosition.Bottom: hWndSpecial = NativeMethods.HWND_BOTTOM; break;
                case ZOrderPosition.Topmost: hWndSpecial = NativeMethods.HWND_TOPMOST; break;
                case ZOrderPosition.AboveNonTopmost: hWndSpecial = NativeMethods.HWND_NOTOPMOST; break;
                default: throw new ArgumentException("Invalid ZOrderPosition value", nameof(specialZOrderPosition));
            }

            NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, hWndSpecial,
                frame.left, frame.top, frame.Width, frame.Height, flags);
        }
    }

    [Flags]
    public enum DeferWindowPosFlags : int
    {
        IgnoreSize = 0x1,
        IgnoreMove = 0x2,
        IgnoreZOrder = 0x4,
        DoNotRedraw = 0x8,
        DoNotActivate = 0x10,
        FrameChanged = 0x20
    }

    public enum ZOrderPosition
    {
        Top,
        Bottom,
        Topmost,
        AboveNonTopmost
    }
}
