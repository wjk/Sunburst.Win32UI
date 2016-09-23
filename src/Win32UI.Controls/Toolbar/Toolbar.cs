﻿using System;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class Toolbar : Window
    {
        #region Messages
        private const uint WM_USER = 0x400;
        private const uint TB_ENABLEBUTTON = (WM_USER + 1);
        private const uint TB_CHECKBUTTON = (WM_USER + 2);
        private const uint TB_PRESSBUTTON = (WM_USER + 3);
        private const uint TB_HIDEBUTTON = (WM_USER + 4);
        private const uint TB_INDETERMINATE = (WM_USER + 5);
        private const uint TB_MARKBUTTON = (WM_USER + 6);
        private const uint TB_ISBUTTONENABLED = (WM_USER + 9);
        private const uint TB_ISBUTTONCHECKED = (WM_USER + 10);
        private const uint TB_ISBUTTONPRESSED = (WM_USER + 11);
        private const uint TB_ISBUTTONHIDDEN = (WM_USER + 12);
        private const uint TB_ISBUTTONINDETERMINATE = (WM_USER + 13);
        private const uint TB_ISBUTTONHIGHLIGHTED = (WM_USER + 14);
        private const uint TB_SETSTATE = (WM_USER + 17);
        private const uint TB_GETSTATE = (WM_USER + 18);
        private const uint TB_ADDBITMAP = (WM_USER + 19);
        private const uint TB_DELETEBUTTON = (WM_USER + 22);
        private const uint TB_GETBUTTON = (WM_USER + 23);
        private const uint TB_BUTTONCOUNT = (WM_USER + 24);
        private const uint TB_COMMANDTOINDEX = (WM_USER + 25);
        private const uint TB_SAVERESTOREA = (WM_USER + 26);
        private const uint TB_SAVERESTOREW = (WM_USER + 76);
        private const uint TB_CUSTOMIZE = (WM_USER + 27);
        private const uint TB_ADDSTRINGA = (WM_USER + 28);
        private const uint TB_ADDSTRINGW = (WM_USER + 77);
        private const uint TB_GETITEMRECT = (WM_USER + 29);
        private const uint TB_BUTTONSTRUCTSIZE = (WM_USER + 30);
        private const uint TB_SETBUTTONSIZE = (WM_USER + 31);
        private const uint TB_SETBITMAPSIZE = (WM_USER + 32);
        private const uint TB_AUTOSIZE = (WM_USER + 33);
        private const uint TB_GETTOOLTIPS = (WM_USER + 35);
        private const uint TB_SETTOOLTIPS = (WM_USER + 36);
        private const uint TB_SETPARENT = (WM_USER + 37);
        private const uint TB_SETROWS = (WM_USER + 39);
        private const uint TB_GETROWS = (WM_USER + 40);
        private const uint TB_SETCMDID = (WM_USER + 42);
        private const uint TB_CHANGEBITMAP = (WM_USER + 43);
        private const uint TB_GETBITMAP = (WM_USER + 44);
        private const uint TB_GETBUTTONTEXTA = (WM_USER + 45);
        private const uint TB_GETBUTTONTEXTW = (WM_USER + 75);
        private const uint TB_REPLACEBITMAP = (WM_USER + 46);
        private const uint TB_SETINDENT = (WM_USER + 47);
        private const uint TB_SETIMAGELIST = (WM_USER + 48);
        private const uint TB_GETIMAGELIST = (WM_USER + 49);
        private const uint TB_LOADIMAGES = (WM_USER + 50);
        private const uint TB_GETRECT = (WM_USER + 51); // wParam is the Cmd instead of index
        private const uint TB_SETHOTIMAGELIST = (WM_USER + 52);
        private const uint TB_GETHOTIMAGELIST = (WM_USER + 53);
        private const uint TB_SETDISABLEDIMAGELIST = (WM_USER + 54);
        private const uint TB_GETDISABLEDIMAGELIST = (WM_USER + 55);
        private const uint TB_SETSTYLE = (WM_USER + 56);
        private const uint TB_GETSTYLE = (WM_USER + 57);
        private const uint TB_GETBUTTONSIZE = (WM_USER + 58);
        private const uint TB_SETBUTTONWIDTH = (WM_USER + 59);
        private const uint TB_SETMAXTEXTROWS = (WM_USER + 60);
        private const uint TB_GETTEXTROWS = (WM_USER + 61);
        private const uint TB_GETBUTTONTEXT = TB_GETBUTTONTEXTW;
        private const uint TB_SAVERESTORE = TB_SAVERESTOREW;
        private const uint TB_ADDSTRING = TB_ADDSTRINGW;
        private const uint TB_GETOBJECT = (WM_USER + 62);
        private const uint TB_GETHOTITEM = (WM_USER + 71);
        private const uint TB_SETHOTITEM = (WM_USER + 72);
        private const uint TB_SETANCHORHIGHLIGHT = (WM_USER + 73);
        private const uint TB_GETANCHORHIGHLIGHT = (WM_USER + 74);
        private const uint TB_MAPACCELERATORA = (WM_USER + 78);
        private const uint TB_GETINSERTMARK = (WM_USER + 79);
        private const uint TB_SETINSERTMARK = (WM_USER + 80);
        private const uint TB_INSERTMARKHITTEST = (WM_USER + 81);
        private const uint TB_MOVEBUTTON = (WM_USER + 82);
        private const uint TB_GETMAXSIZE = (WM_USER + 83);
        private const uint TB_SETEXTENDEDSTYLE = (WM_USER + 84);
        private const uint TB_GETEXTENDEDSTYLE = (WM_USER + 85);
        private const uint TB_GETPADDING = (WM_USER + 86);
        private const uint TB_SETPADDING = (WM_USER + 87);
        private const uint TB_SETINSERTMARKCOLOR = (WM_USER + 88);
        private const uint TB_GETINSERTMARKCOLOR = (WM_USER + 89);
        private const uint TB_SETCOLORSCHEME = 0x2002;
        private const uint TB_GETCOLORSCHEME = 0x2003;
        private const uint TB_MAPACCELERATORW = (WM_USER + 90);
        private const uint TB_MAPACCELERATOR = TB_MAPACCELERATORW;
        #endregion

        public const string WindowClass = "ToolbarWindow32";
        public override string WindowClassName => WindowClass;

        public bool GetButtonEnabled(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONENABLED, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public bool GetButtonChecked(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONCHECKED, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public bool GetButtonPressed(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONPRESSED, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public bool IsButtonHidden(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONHIDDEN, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public bool IsButtonIndeterminate(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONINDETERMINATE, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public ToolbarItemState GetState(int buttonId)
        {
            return (ToolbarItemState)(int)SendMessage(TB_GETSTATE, (IntPtr)buttonId, IntPtr.Zero);
        }

        public void SetState(int buttonId, ToolbarItemState state)
        {
            SendMessage(TB_SETSTATE, (IntPtr)buttonId, (IntPtr)(int)state);
        }

        public ToolbarItem GetItem(int index)
        {
            using (StructureBuffer<TBBUTTON> ptr = new StructureBuffer<TBBUTTON>())
            {
                bool success = (int)SendMessage(TB_GETBUTTON, (IntPtr)index, ptr.Handle) != 0;
                if (!success) return null;
                return new ToolbarItem(ptr.Value);
            }
        }

        public int ItemCount => (int)SendMessage(TB_BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);

        public Rect GetItemRect(int index)
        {
            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                SendMessage(TB_GETITEMRECT, (IntPtr)index, ptr.Handle);
                return ptr.Value;
            }
        }

        public void SetIconSize(Size sz)
        {
            SendMessage(TB_SETBITMAPSIZE, IntPtr.Zero, (IntPtr)((sz.width & 0xFFFF) | ((sz.height & 0xFFFF) << 16)));
        }

        public ToolTip ToolTip
        {
            get
            {
                return new ToolTip() { Handle = SendMessage(TB_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TB_SETTOOLTIPS, value.Handle, IntPtr.Zero);
            }
        }

        public void SetNotificationTarget(Window window)
        {
            SendMessage(TB_SETPARENT, window.Handle, IntPtr.Zero);
        }

        public int Rows
        {
            get
            {
                return (int)SendMessage(TB_GETROWS, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                Rect ignored;
                SetRows(value, true, out ignored);
            }
        }

        public void SetRows(int rowCount, bool allowMoreRows, out Rect controlRect)
        {
            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                SendMessage(TB_SETROWS, (IntPtr)(rowCount | ((allowMoreRows ? 1 : 0) << 16)), ptr.Handle);
                controlRect = ptr.Value;
            }
        }

        public bool SetButtonCommand(int buttonIndex, int command)
        {
            return (int)SendMessage(TB_SETCMDID, (IntPtr)buttonIndex, (IntPtr)command) != 0;
        }

        public bool UsesLargeIcons
        {
            get
            {
                const uint TB_GETBITMAPFLAGS = WM_USER + 41;
                int flags = (int)SendMessage(TB_GETBITMAPFLAGS, IntPtr.Zero, IntPtr.Zero);
                return ((flags & 0x1) == 0x1);
            }
        }

        public int GetButtonBitmapIndex(int buttonIndex)
        {
            return (int)SendMessage(TB_GETBITMAP, (IntPtr)buttonIndex, IntPtr.Zero);
        }

        public NonOwnedImageList NormalImageList
        {
            get
            {
                return new NonOwnedImageList() { Handle = SendMessage(TB_GETIMAGELIST, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TB_SETIMAGELIST, IntPtr.Zero, value.Handle);
            }
        }

        public NonOwnedImageList DisabledImageList
        {
            get
            {
                return new NonOwnedImageList() { Handle = SendMessage(TB_GETDISABLEDIMAGELIST, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TB_SETDISABLEDIMAGELIST, IntPtr.Zero, value.Handle);
            }
        }

        public NonOwnedImageList MouseOverImageList
        {
            get
            {
                return new NonOwnedImageList() { Handle = SendMessage(TB_GETHOTIMAGELIST, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TB_SETHOTIMAGELIST, IntPtr.Zero, value.Handle);
            }
        }

        public Size ButtonSize
        {
            get
            {
                int dwRet = (int)SendMessage(TB_GETBUTTONSIZE, IntPtr.Zero, IntPtr.Zero);
                Size sz = new Size();
                sz.width = dwRet & 0xFFFF;
                sz.height = (dwRet >> 16) & 0xFFFF;
                return sz;
            }

            set
            {
                SendMessage(TB_SETBUTTONSIZE, IntPtr.Zero, (IntPtr)((value.width & 0xFFFF) | ((value.height & 0xFFFF) << 16)));
            }
        }

        public Rect GetButtonRect(int buttonIndex)
        {
            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                SendMessage(TB_GETRECT, (IntPtr)buttonIndex, ptr.Handle);
                return ptr.Value;
            }
        }

        public int TextRows
        {
            get
            {
                return (int)SendMessage(TB_GETTEXTROWS, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public bool SetButtonWidth(int minWidth, int maxWidth)
        {
            return (int)SendMessage(TB_SETBUTTONWIDTH, IntPtr.Zero, (IntPtr)((minWidth & 0xFFFF) | ((maxWidth & 0xFFFF) << 16))) != 0;
        }

        public bool SetIndent(int indent)
        {
            return (int)SendMessage(TB_SETINDENT, (IntPtr)indent, IntPtr.Zero) != 0;
        }

        public bool SetMaximumTextRows(int maxRows)
        {
            return (int)SendMessage(TB_SETMAXTEXTROWS, (IntPtr)maxRows, IntPtr.Zero) != 0;
        }

        public bool AnchorHighlight
        {
            get
            {
                return (int)SendMessage(TB_GETANCHORHIGHLIGHT, IntPtr.Zero, IntPtr.Zero) != 0;
            }

            set
            {
                SendMessage(TB_SETANCHORHIGHLIGHT, (IntPtr)(value ? 1 : 0), IntPtr.Zero);
            }
        }

        public bool GetIsButtonHighlighted(int buttonId)
        {
            return (int)SendMessage(TB_ISBUTTONHIDDEN, (IntPtr)buttonId, IntPtr.Zero) != 0;
        }

        public void SetLabelDrawingFlags(TextAlignment align, VerticalTextAlignment valign, StringDrawingFlags flags)
        {
            const int DT_LEFT = 0x00000000;
            const int DT_CENTER = 0x00000001;
            const int DT_RIGHT = 0x00000002;
            const int DT_VCENTER = 0x00000004;
            const int DT_BOTTOM = 0x00000008;
            const int DT_WORDBREAK = 0x00000010;
            const int DT_EXPANDTABS = 0x00000040;
            const int DT_NOPREFIX = 0x00000800;
            const int DT_PATH_ELLIPSIS = 0x00004000;
            const int DT_WORD_ELLIPSIS = 0x00040000;

            int value = 0;
            switch (align)
            {
                case TextAlignment.Left: value |= DT_LEFT; break;
                case TextAlignment.Center: value |= DT_CENTER; break;
                case TextAlignment.Right: value |= DT_RIGHT; break;
            }

            switch (valign)
            {
                case VerticalTextAlignment.Top: break; // there is no DT_* flag for top alignment
                case VerticalTextAlignment.Center: value |= DT_VCENTER; break;
                case VerticalTextAlignment.Bottom: value |= DT_BOTTOM; break;
            }

            if (flags.HasFlag(StringDrawingFlags.ExpandTabCharacters)) value |= DT_EXPANDTABS;
            if (flags.HasFlag(StringDrawingFlags.IgnoreAmpersands)) value |= DT_NOPREFIX;
            if (flags.HasFlag(StringDrawingFlags.BreakOnWords)) value |= DT_WORDBREAK;
            if (flags.HasFlag(StringDrawingFlags.AddPathEllipsis)) value |= DT_PATH_ELLIPSIS;
            if (flags.HasFlag(StringDrawingFlags.AddWordEllipsis)) value |= DT_WORD_ELLIPSIS;

            const uint TB_SETTEXTDRAWFLAGS = WM_USER + 70;
            SendMessage(TB_SETTEXTDRAWFLAGS, (IntPtr)value, (IntPtr)value);
        }

        public void GetInsertMark(out int buttonIndex, out bool insertAfter)
        {
            using (StructureBuffer<TBINSERTMARK> ptr = new StructureBuffer<TBINSERTMARK>())
            {
                SendMessage(TB_GETINSERTMARK, IntPtr.Zero, ptr.Handle);
                TBINSERTMARK mark = ptr.Value;
                buttonIndex = mark.iButton;
                insertAfter = ((mark.dwFlags & TBINSERTMARK.TBIHMT_INSERTAFTER) == TBINSERTMARK.TBIHMT_INSERTAFTER);
            }
        }

        public void SetInsertMark(int buttonIndex, bool insertAfter)
        {
            TBINSERTMARK mark = new TBINSERTMARK();
            mark.iButton = buttonIndex;
            mark.dwFlags |= insertAfter ? TBINSERTMARK.TBIHMT_INSERTAFTER : 0;

            using (StructureBuffer<TBINSERTMARK> ptr = new StructureBuffer<TBINSERTMARK>())
            {
                ptr.Value = mark;
                SendMessage(TB_SETINSERTMARK, IntPtr.Zero, ptr.Handle);
            }
        }

        public Color InsertMarkColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TB_GETINSERTMARKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TB_SETINSERTMARKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }
    }
}