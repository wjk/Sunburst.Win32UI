﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

#pragma warning disable 0169
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct TBBUTTON
    {
        public int iBitmap;
        public int idCommand;
        public byte fsState;
        public byte fsStyle;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 6)]
        private byte[] bReserved;
        public IntPtr dwData;
        public string iString;
    }

    internal struct TBINSERTMARK
    {
        public int iButton;
        public int dwFlags;

        public const int TBIHMT_INSERTAFTER = 0x1;
        public const int TBIHMT_BACKGROUND = 0x2;
    }
}