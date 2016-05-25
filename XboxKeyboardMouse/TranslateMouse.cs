using ScpDriverInterface;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XboxMouse_Keyboard
{
    class TranslateMouse
    {
        //tick = ~100ns, 1frame/60fps = 16.67ms,  16.67ms * 100ticks = 1667ticks/frame
        private const uint FRAME_PER_TICK = 1667;

        static private DirectInput device;
        static private Mouse mouse;
        static bool started = false;  //fix bool start flag
        static Point originalMouseState = Control.MousePosition;
        static long tick = 0;
        private static void mouseMovementInput(X360Controller controller)
        {
            Stopwatch s = Stopwatch.StartNew();
            Point currentMouseState = Control.MousePosition;

            if (tick > FRAME_PER_TICK || tick == 0)
            {
                    int xDifference = currentMouseState.X - originalMouseState.X;
                    int yDifference = currentMouseState.Y - originalMouseState.Y;

                    if (xDifference > 0)
                        controller.RightStickX = short.MaxValue;
                    else if (xDifference < 0)
                        controller.RightStickX = short.MinValue;
                    else
                        controller.RightStickX = 0;

                    if (yDifference > 0)
                        controller.RightStickY = short.MinValue;
                    else if (yDifference < 0)
                        controller.RightStickY = short.MaxValue;
                    else
                        controller.RightStickY = 0;

                    Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);

                    tick = 0;
               
            }
            s.Stop();
            tick += s.ElapsedTicks;

            return;
        }

        private static void mouseButtonsInput(X360Controller controller)
        {

            MouseState state = mouse.GetCurrentState();

            if (state.IsPressed(0))
                controller.RightTrigger = 255;
            else
                controller.RightTrigger = 0;

            if (state.IsPressed(1))
                controller.LeftTrigger = 255;
            else
                controller.LeftTrigger = 0;
        }

        private static void init()
        {
            if (started == false)
            {
                device = new DirectInput();
                mouse = new Mouse(device);
                mouse.Acquire();
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                originalMouseState = Control.MousePosition;
                started = true;
            }
        }

        public static void translateMouse(X360Controller controller)
        {
            init();
            mouseMovementInput(controller);
            mouseButtonsInput(controller);
        }
    }

}
