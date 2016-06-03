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

        public static void ToggleCursor()
        {
            const String XBOXAPP = "Xbox";
            bool started = false;
            Thread thrMouseMovement = null;
           

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

                            thrMouseMovement = new Thread(TranslateMouse.MouseMovementInput);
                            thrMouseMovement.SetApartmentState(ApartmentState.STA);
                            thrMouseMovement.Start();

                            started = true;
                        }
                    }
                    
                    else 
                    {
                        CursorView.CursorShow();
                        
                        if(thrMouseMovement != null)
                            thrMouseMovement.Abort();

                        started = false;
                    }
                    
                }
                Thread.Sleep(1000);
            }
        }
    }
}
