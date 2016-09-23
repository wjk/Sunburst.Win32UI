﻿using System;

#pragma warning disable CS0649
namespace Microsoft.Win32.UserInterface.Interop
{
    // This struct corresponds to the LPBITMAP type in wingdi.h.
    internal struct BitmapHeader
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public IntPtr bmBits;
    }
}
