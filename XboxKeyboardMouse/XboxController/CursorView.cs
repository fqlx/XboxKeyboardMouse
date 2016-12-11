using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XboxKeyboardMouse {
    class CursorView {
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);
        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, uint id);
        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr pcur);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO {
            public Int32 cbSize;            // Specifies the size, in bytes, of the structure. 
                                            // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;             // Specifies the cursor state. This parameter can be one of the following values:
                                            //    0                 The cursor is hidden.
                                            //    CURSOR_SHOWING    The cursor is showing.
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        private static POINT cursorPosition;
        private static IntPtr cursorHandle;
        //private const int OCR_NORMAL = 32512;
        private const int OCR_HAND = 32649;

        [DllImport("user32")]
        internal static extern long SystemParametersInfo(uint uAction, IntPtr lpvParam, uint uParam, uint fuWinIni);

        public static void CursorHide() {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            cursorPosition = pci.ptScreenPos;
            cursorHandle = CopyIcon(pci.hCursor);

            MemoryStream cursorMemoryStream = new MemoryStream(Properties.Resources.nocursor);
            Cursor cursor = new Cursor(cursorMemoryStream);

            SetSystemCursor(cursor.Handle, OCR_HAND);
        }

        public static void CursorShow() {
            const uint SPI_SETCURSORS = 0x0057;

            SystemParametersInfo(SPI_SETCURSORS, IntPtr.Zero, 0, 0);
        }

    }
}
