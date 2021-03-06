using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Windows font (GDI <c>HFONT</c>).
    /// </summary>
    public sealed class Font : IDisposable
    {
        public static Font CreateSystemUIFont()
        {
            using (StructureBuffer<NONCLIENTMETRICSW> metricsPtr = new StructureBuffer<NONCLIENTMETRICSW>())
            {
                const int SPI_GETNONCLIENTMETRICS = 0x29;

                NONCLIENTMETRICSW metrics = new NONCLIENTMETRICSW();
                metrics.cbSize = Marshal.SizeOf<NONCLIENTMETRICSW>();
                metricsPtr.Value = metrics;

                NativeMethods.SystemParametersInfoW(SPI_GETNONCLIENTMETRICS, metricsPtr.Size, metricsPtr.Handle, 0);
                return new Font(metricsPtr.Value.lfMessageFont);
            }
        }

        private static LOGFONT CreatePointFontStruct(string fontName, int pointSize, bool bold, bool italic)
        {
            const byte DEFAULT_CHARSET = 1;
            const long FW_BOLD = 700;
            const int LOGPIXELSY = 90;

            LOGFONT font = new LOGFONT();
            font.lfCharSet = DEFAULT_CHARSET;
            font.lfFaceName = fontName;
            if (bold) font.lfWeight = FW_BOLD;
            if (italic) font.lfItalic = 1;

            using (GraphicsContext context = GraphicsContext.CreateOffscreenContext())
            {
                font.lfHeight = -NativeMethods.MulDiv(pointSize, context.GetCapability(LOGPIXELSY), 72);
            }

            return font;
        }

        public Font(string fontName, int pointSize, bool bold = false, bool italic = false)
            : this(CreatePointFontStruct(fontName, pointSize, bold, italic)) { }

        internal Font(LOGFONT font_struct)
            : this(NativeMethods.CreateFontIndirect(ref font_struct)) { }

        /// <summary>
        /// Creates a new instance of Font.
        /// </summary>
        /// <param name="ptr">
        /// The native handle to the font data.
        /// </param>
        public Font(IntPtr ptr)
        {
            Handle = ptr;

            using (StructureBuffer<LOGFONT> logFontPtr = new StructureBuffer<LOGFONT>())
            {
                int returnedSize = NativeMethods.GetObject(Handle, logFontPtr.Size, logFontPtr.Handle);
                if (logFontPtr.Size != returnedSize)
                    throw new InvalidOperationException($"GetObject() returned incorrect size (got {returnedSize} bytes, expected {logFontPtr.Size} bytes)");
                mFontDescriptor = logFontPtr.Value;
            }
        }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        private LOGFONT mFontDescriptor;

        /// <summary>
        /// The native handle to the font data.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// The name of the font face.
        /// </summary>
        public string FaceName => mFontDescriptor.lfFaceName;

        /// <summary>
        /// Gets the size of the font, in points.
        /// </summary>
        public int PointSize
        {
            get
            {
                using (GraphicsContext dc = GraphicsContext.CreateOffscreenContext())
                {
                    const int LOGPIXELSY = 90;
                    return NativeMethods.MulDiv(Convert.ToInt32(-mFontDescriptor.lfHeight), 72, dc.GetCapability(LOGPIXELSY));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Sunburst.Win32UI.Graphics.Font"/> is bold.
        /// </summary>
        public bool IsBold => mFontDescriptor.lfWeight >= 700; // FW_BOLD == 700

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Sunburst.Win32UI.Graphics.Font"/> is italic.
        /// </summary>
        public bool IsItalic => mFontDescriptor.lfItalic != 0;
    }
}
