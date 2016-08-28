using ScpDriverInterface;
using SlimDX.DirectInput;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Timers;

namespace XboxKeyboardMouse
{
    class TranslateMouse
    {
        static private DirectInput device;
        static private Mouse mouse;

        static System.Timers.Timer timer = new System.Timers.Timer();
        static bool done = true;

        public static void InitMouse()
        {
            device = new DirectInput();
            mouse = new Mouse(device);
            mouse.Acquire();
        }

        public static void MouseMovementInput()
        {
            timer.Elapsed += new ElapsedEventHandler(tick);
            timer.Interval = 50;
            timer.Enabled = true;

            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            Point originalMouseState = Control.MousePosition;

            while (true)
            {
                Point currentMouseState = Control.MousePosition;
                if (done)
                {
                    double xDifference = currentMouseState.X - originalMouseState.X;
                    double yDifference = currentMouseState.Y - originalMouseState.Y;

                    xDifference /= 150;
                    yDifference /= -150;

                    if (xDifference > 1)
                        xDifference = 1;
                    if (xDifference < -1)
                        xDifference = -1;
                    if (yDifference > 1)
                        yDifference = 1;
                    if (yDifference < -1)
                        yDifference = -1;

                    short scaledX = (short) (xDifference * short.MaxValue);
                    short scaledY = (short) (yDifference * short.MaxValue);

                    Activate.controller.RightStickX = scaledX;
                    Activate.controller.RightStickY = scaledY;

                    Cursor.Position = originalMouseState;
                    done = false;

                }

                Activate.SendtoController(Activate.controller);
            }

        }

        public static void tick(object source, ElapsedEventArgs e)
        {
            done = true;
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
    }
}
