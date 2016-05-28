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
        //tick = ~100ns, 1frame/60fps = 16.67ms,  16.67ms * 100ns/ticks = 1667ticks/frame
        //3334 = 30fps inbetween is a good value
        //idealy we want a low tick count and we increase in game sensitivity to offset it
        private const uint FRAME_PER_TICK = 3334;

        static private DirectInput device;
        static private Mouse mouse;
        static bool started = false;  //fix bool start flag
        static Point originalMouseState = Control.MousePosition;
        static Stopwatch s = new Stopwatch();

        private static void MouseMovementInput(X360Controller controller)
        {
            Point currentMouseState = Control.MousePosition;

            if ((s.ElapsedTicks / Stopwatch.Frequency) >= FRAME_PER_TICK || s.ElapsedTicks == 0)
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

                s.Restart();
            }

            return;
        }

        private static void MouseButtonsInput(X360Controller controller)
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

        private static void Init()
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

        public static void TranslateMouse(X360Controller controller)
        {
            Init();
            MouseMovementInput(controller);
            MouseButtonsInput(controller);
        }
    }

}
