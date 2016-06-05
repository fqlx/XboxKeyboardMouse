using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XboxKeyboardMouse;

namespace XboxKeyboardMouse
{
    class XboxStream
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static Thread tMouseMovement;

        public static void ToggleCursor()
        {
            const String XBOXAPP = "Xbox";
            bool started = false;           

            const int count = 512;
            StringBuilder text = new StringBuilder(count);
            while (true)
            {
                IntPtr handle = GetForegroundWindow();

                if (GetWindowText(handle, text, count) > 0)
                {
                    if (text.ToString().Equals(XBOXAPP))
                    {
                        if (started == false)
                        {

                            CursorView.CursorHide();

                            tMouseMovement = new Thread(TranslateMouse.MouseMovementInput);
                            tMouseMovement.SetApartmentState(ApartmentState.STA);
                            tMouseMovement.IsBackground = true;
                            tMouseMovement.Start();

                            started = true;
                        }
                    }
                    
                    else 
                    {
                        CursorView.CursorShow();
                        
                        if(tMouseMovement != null)
                            tMouseMovement.Abort();

                        started = false;
                    }
                    
                }
                Thread.Sleep(1000);
            }
        }
    }
}
