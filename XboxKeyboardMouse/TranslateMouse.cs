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

namespace XboxKeyboardMouse
{
    class TranslateMouse
    {
        //tick = ~100ns, 1frame/60fps = 16.67ms,  16.67ms * 100ns/ticks = 1667ticks/frame
        //3334 = 30fps inbetween is a good value
        //idealy we want a low tick count and we increase in game sensitivity to offset it
        private const double FRAME_PER_30FPS_IN_MS = 33.3333333333;
        private const double FRAME_PER_60FPS_IN_MS = 16.6666666667;

        static private DirectInput device;
        static private Mouse mouse;
        static Stopwatch s = new Stopwatch();
        

        public static void MouseMovementInput()
        {
            const uint ESTIMATED_LOOP_COMPLETION = 50;

            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            Point originalMouseState = Control.MousePosition;

            double nanosecPerTick = (1000D * 1000D * 1000D) / Stopwatch.Frequency;

            while (true)
            {
                Point currentMouseState = Control.MousePosition;
                //todo: profile code on load and subtract the time it takes to calc/send mousemovement to controller and subtract for ESTIMATED_LOOP_COMPLETION
                double frame_per_tick = FRAME_PER_60FPS_IN_MS * nanosecPerTick - ESTIMATED_LOOP_COMPLETION;

                if (s.ElapsedTicks >= frame_per_tick || s.ElapsedTicks == 0)
                {
                    int xDifference = currentMouseState.X - originalMouseState.X;
                    int yDifference = currentMouseState.Y - originalMouseState.Y;

                    if (xDifference > 0)
                        Activate.controller.RightStickX = short.MaxValue;
                    else if (xDifference < 0)
                        Activate.controller.RightStickX = short.MinValue;
                    else
                        Activate.controller.RightStickX = 0;

                    if (yDifference > 0)
                        Activate.controller.RightStickY = short.MinValue;
                    else if (yDifference < 0)
                        Activate.controller.RightStickY = short.MaxValue;
                    else
                        Activate.controller.RightStickY = 0;

                    Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);

                    s.Restart();
                }

                Activate.SendtoController(Activate.controller);
            }
        }

        public static void MouseButtonsInput(X360Controller controller)
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

        public static void InitMouse()
        {
                device = new DirectInput();
                mouse = new Mouse(device);
                mouse.Acquire();
        }
    }

}
