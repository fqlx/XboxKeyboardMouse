using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    class XboxStream {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT Rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int X;  //left
            public int Y;  //top
            public int Width;  //right
            public int Height;  //bottom
        }


        public static Thread tMouseMovement;

        public static void XboxAppDetector() {
            const String XBOXAPP = "Xbox";
            bool started = false;

            const int count = 512;
            StringBuilder text = new StringBuilder(count);
            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    IntPtr handle = GetForegroundWindow();

                    if (GetWindowText(handle, text, count) > 0)
                    {
                        if (!text.ToString().Equals(XBOXAPP))
                        {
                            ShowAndFreeCursor();
                            started = false;
                            Program.MainForm.StatusWaiting();

                            continue;
                        }

                        /*if (IsFullscreen(handle) == false) {
                            ShowAndFreeCursor();
                            started = false;
                            Program.mainform.StatusWaiting();

                            continue;
                        }*/

                        if (!started)
                        {
                            LockAndHideCursor();
                            started = true;
                            Program.MainForm.StatusRunning();
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // Ensures that the current thread does not just crash and cause issues
            }
        }

        private static void LockAndHideCursor() {
            if (Program.HideCursor)
                CursorView.CursorHide();

            tMouseMovement = new Thread(TranslateMouse.MouseMovementInput);
            tMouseMovement.SetApartmentState(ApartmentState.STA);
            tMouseMovement.IsBackground = true;
            tMouseMovement.Start();
        }

        private static void ShowAndFreeCursor() {
            CursorView.CursorShow();

            if (tMouseMovement != null)
                tMouseMovement.Abort();
        }

        private static bool IsFullscreen(IntPtr handle) {
            bool runningFullScreen = false;
            RECT appBounds;
            Rectangle screenBounds;

            GetWindowRect(handle, out appBounds);

            screenBounds = Screen.FromHandle(handle).Bounds;
            if ((appBounds.Height - appBounds.Y) == screenBounds.Height && (appBounds.Width - appBounds.X) == screenBounds.Width) {
                runningFullScreen = true;
            }

            return runningFullScreen;
        }
    }
}
