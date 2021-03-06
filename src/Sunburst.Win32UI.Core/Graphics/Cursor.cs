﻿using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Windows cursor.
    /// </summary>
    public class Cursor
    {
        /// <summary>
        /// Gets or sets the currently active cursor.
        /// </summary>
        public static Cursor Current
        {
            get
            {
                return new Cursor(NativeMethods.GetCursor());
            }

            set
            {
                NativeMethods.SetCursor(value.Handle);
            }
        }

        /// <summary>
        /// The standard arrow cursor.
        /// </summary>
        public static Cursor Arrow
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32650)); }
        }

        /// <summary>
        /// The crosshair cursor.
        /// </summary>
        public static Cursor Crosshair
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32512)); }
        }

        /// <summary>
        /// The hand cursor.
        /// </summary>
        public static Cursor Hand
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32515)); }
        }

        /// <summary>
        /// The I-beam (text insertion) cursor.
        /// </summary>
        public static Cursor IBeam
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32513)); }
        }

        /// <summary>
        /// The "slashed-circle" cursor, used to indicate an invalid operation during a drag-and-drop sequence.
        /// </summary>
        public static Cursor SlashedCircle
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32648)); }
        }

        /// <summary>
        /// A four-way resize cursor.
        /// </summary>
        public static Cursor SizeAll
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32646)); }
        }

        /// <summary>
        /// A two-way resize cursor.
        /// </summary>
        public static Cursor SizeNESW
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32643)); }
        }

        /// <summary>
        /// A two-way resize cursor.
        /// </summary>
        public static Cursor SizeNS
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32645)); }
        }

        /// <summary>
        /// A two-way resize cursor.
        /// </summary>
        public static Cursor SizeNWSE
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32642)); }
        }

        /// <summary>
        /// A two-way resize cursor.
        /// </summary>
        public static Cursor SizeWE
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32644)); }
        }

        /// <summary>
        /// The Windows "busy" cursor.
        /// </summary>
        public static Cursor Hourglass
        {
            get { return new Cursor(NativeMethods.LoadCursor(IntPtr.Zero, (IntPtr)32514)); }
        }

        public static Cursor Load(ResourceLoader loader, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new Cursor(NativeMethods.LoadCursor(loader.ModuleHandle, buffer.Handle));
            }
        }

        public static Cursor Load(ResourceLoader loader, ushort resourceId)
        {
            return new Cursor(NativeMethods.LoadCursor(loader.ModuleHandle, (IntPtr)resourceId));
        }

        /// <summary>
        /// Creates a new instance of Cursor.
        /// </summary>
        /// <param name="hCursor">
        /// The handle to the native cursor data.
        /// </param>
        public Cursor(IntPtr hCursor)
        {
            Handle = hCursor;
        }

        /// <summary>
        /// The handle to the native cursor data.
        /// </summary>
        public IntPtr Handle { get; protected set; }
    }
}
