using ScpDriverInterface;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XboxMouse_Keyboard
{
    class TranslateMouse
    {
        static class MouseKeyIO
        {

            // These are copies of DirectX constants
            public const int INPUT_MOUSE = 0;
            public const int INPUT_KEYBOARD = 1;
            public const int INPUT_HARDWARE = 2;
            public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
            public const uint KEYEVENTF_KEYUP = 0x0002;
            public const uint KEYEVENTF_UNICODE = 0x0004;
            public const uint KEYEVENTF_SCANCODE = 0x0008;
            public const uint XBUTTON0 = 0x0000;  //added this line for left click, i dont think it's in actual struct
            public const uint XBUTTON1 = 0x0001;
            public const uint XBUTTON2 = 0x0002;
            public const uint MOUSEEVENTF_MOVE = 0x0001;
            public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
            public const uint MOUSEEVENTF_LEFTUP = 0x0004;
            public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
            public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
            public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
            public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
            public const uint MOUSEEVENTF_XDOWN = 0x0080;
            public const uint MOUSEEVENTF_XUP = 0x0100;
            public const uint MOUSEEVENTF_WHEEL = 0x0800;
            public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
            public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

            [StructLayout(LayoutKind.Sequential)]
            public struct MOUSEINPUT
            {
                public int dx;
                public int dy;
                public uint mouseData;
                public uint dwFlags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct KEYBDINPUT
            {
                public ushort wVk;
                public ushort wScan;
                public uint dwFlags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct HARDWAREINPUT
            {
                public uint uMsg;
                public ushort wParamL;
                public ushort wParamH;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT
            {
                [FieldOffset(0)]
                public int type;
                [FieldOffset(4)]
                public MOUSEINPUT mi;
                [FieldOffset(4)]
                public KEYBDINPUT ki;
                [FieldOffset(4)]
                public HARDWAREINPUT hi;
            }
        }


        private static DirectInput directInputInstance = new DirectInput();
        private static SlimDX.DirectInput.Mouse mouseDevice;
        private static MouseState currentMouseState;

        public static void initMouse()
        {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            mouseDevice = new SlimDX.DirectInput.Mouse(directInputInstance);
            if (mouseDevice == null)
                throw new Exception("Failed to create mouse device");

            mouseDevice.SetCooperativeLevel(handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);

            mouseDevice.Properties.AxisMode = DeviceAxisMode.Relative;
            mouseDevice.Acquire();
        }

        static bool started = false;  //fix bool start flag
        public static void mouseInput(X360Controller controller)
        {
            if (started == false)
            {
                initMouse();
                started = true;
            }

            int deltaX = 0;
            int deltaY = 0;

            currentMouseState = mouseDevice.GetCurrentState();

            deltaX = currentMouseState.X;
            deltaY = currentMouseState.Y;

            if (deltaX > 0)
                controller.RightStickX = short.MaxValue;
            else if (deltaX < 0)
                controller.RightStickX = short.MinValue;
            else {
                if (controller.RightStickX > 500)
                    controller.RightStickX = (short)(controller.RightStickX - 10);
                else if (controller.RightStickX < -500)
                    controller.RightStickX = (short)(controller.RightStickX + 10);
                else
                    controller.RightStickX = 0;
            }
            
            if (deltaY > 0)
                controller.RightStickY = short.MinValue;
            else if (deltaY < 0)
                controller.RightStickY = short.MaxValue;
            else {
                if (controller.RightStickY < 500)
                    controller.RightStickY = (short)((controller.RightStickY + 10));
                else if (controller.RightStickY > -500)
                    controller.RightStickY = (short)((controller.RightStickY - 10));
                else
                    controller.RightStickY = 0;
            }
            
            
            const int OUTERDEADZONEX = 300;  //not even sure if this is actually called a deadzone
            const int OUTERDEADZONEY = 100;  

            if (Cursor.Position.X >= Screen.PrimaryScreen.Bounds.Width - OUTERDEADZONEX)
                controller.RightStickX = short.MaxValue;
            else if (Cursor.Position.X <= OUTERDEADZONEX)
                controller.RightStickX = short.MinValue;


            if (Cursor.Position.Y >= Screen.PrimaryScreen.Bounds.Height - OUTERDEADZONEY - 1)  //minus 1 because you can't move to the last pixel
                controller.RightStickY = short.MinValue;
            else if (Cursor.Position.Y <= OUTERDEADZONEY)
                controller.RightStickY = short.MaxValue;

            //mouse clicks to triggers
            if (currentMouseState.IsPressed((int)MouseKeyIO.XBUTTON0))
                controller.RightTrigger = byte.MaxValue;
            else
                controller.RightTrigger = 0;

            if (currentMouseState.IsPressed((int)MouseKeyIO.XBUTTON1))
                controller.LeftTrigger = byte.MaxValue;
            else
                controller.LeftTrigger = 0;

            return;
        }
    }

}
