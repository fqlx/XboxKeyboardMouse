using System;
using System.Drawing;
using System.Linq;
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

        private static readonly string[] defaultAppNames = {
            "Xbox",
            "Xbox Console Companion",
            "Xbox Console Companion - Beta",
            "Xbox Companion-console",
            "Xbox Companion-console - bèta",
            "Xbox-konsol Companion",
            "Xbox-konsol Companion - beta",
            "Xbox konsoles palīgs",
            "Xbox Konsolu Yardımcısı",
            "Xbox Konsolu Yardımcısı - Beta",
            "Xbox 主機小幫手",
            "Xbox 主機小幫手 - beta 搶鮮版",
            "Xbox 本体コンパニオン",
            "Xbox 本体コンパニオン - ベータ",
            "Xbox 본체 도우미",
            "Xbox 본체 도우미 - 베타 버전",
            "Xboxi konsooliabiline",
            "Xboxi konsooliabiline - beetaversioon",
            "„Xbox“ konsolės pagalbinė priemonė",
            "„Xbox“ konsolės pagalbinė priemonė - beta versija",
            "Compagnon de la console Xbox",
            "Compagnon de la console Xbox - bêta",
            "Compañero de la consola Xbox",
            "Compañero de la consola Xbox - beta",
            "Complemento da Consola Xbox",
            "Complemento da Consola Xbox - beta",
            "Companion console Xbox",
            "Companion console Xbox - beta",
            "Компаньон консоли Xbox",
            "Компаньон консоли Xbox - бета-версия",
            "Pomoćnik konzole Xbox",
            "Pomoćnik konzole Xbox - beta-verzija",
            "Pomocnik konsoli Xbox",
            "Pomocnik konsoli Xbox - wersja Beta",
            "Spremljevalec za konzolo Xbox",
            "Spremljevalec za konzolo Xbox - beta",
            "Teman Konsol Xbox",
            "Teman Konsol Xbox - beta",
            "Ứng dụng Đồng hành Bảng điều khiển Xbox",
            "Ứng dụng Đồng hành Bảng điều khiển Xbox - beta",
            "مصاحب وحدة تحكم Xbox",
            "مصاحب وحدة تحكم Xbox - إصدار تجريبي",
            "מסייע קונסולת Xbox",
            "מסייע קונסולת Xbox - בתא",
        };

        public static void XboxAppDetector() {
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
                        if (!defaultAppNames.Contains(text.ToString()))
                        {
                            ShowAndFreeCursor();
                            started = false;
                            Program.MainForm.StatusWaiting();
                        }
                        else if (!started)
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
