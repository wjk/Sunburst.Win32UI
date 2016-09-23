﻿using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    [Flags]
    public enum StringDrawingFlags
    {
        None = 0,
        ExpandTabCharacters = 1,
        IgnoreAmpersands = 2,
        BreakOnWords = 4,
        AddWordEllipsis = 8,
        AddPathEllipsis = 16
    }
}
