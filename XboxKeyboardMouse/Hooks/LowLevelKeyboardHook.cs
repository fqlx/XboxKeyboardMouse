using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace XboxKeyboardMouse.Hooks
{
    class LowLevelKeyboardHook {
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int SW_HIDE = 0;
        public static IntPtr _hookID = IntPtr.Zero;
        public static LowLevelKeyboardProc _proc = HookCallback;

        public static bool LockEscape = true;

        static bool _escape = false;
        static bool _escape_pr = false;

        public static bool EscapePressed {
            get {
                // Return if escape is pressed
                // if its not then return if it has to
                // be processed.
                var processIt = _escape_pr;
                    _escape_pr = false;
                return _escape || processIt;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                // Our pressed Key
                int vkCode = Marshal.ReadInt32(lParam);
                var key = KeyInterop.KeyFromVirtualKey(vkCode);

                if (key == Key.Escape) {
                    if ((wParam == (IntPtr)WM_KEYDOWN)) {
                        _escape = true;
                        _escape_pr = true;
                    } else {
                        _escape = false;
                    }

                    //Activate.SendGuide((wParam == (IntPtr)WM_KEYDOWN));

                    if (LockEscape) {
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.Second + " Cancelled Escape");
                        return (IntPtr)1;
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc) {
            using (Process curProcess = Process.GetCurrentProcess()) {
                using (ProcessModule curModule = curProcess.MainModule) {
                    var handle = GetModuleHandle(curModule.ModuleName);
                    var r = SetWindowsHookEx(WH_KEYBOARD_LL, proc, handle, 0);
                    return r;
                }
            }
        }
    }
}
