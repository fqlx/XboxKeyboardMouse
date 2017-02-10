﻿using ScpDriverInterface;
using SlimDX.DirectInput;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Timers;
using System.Threading;

namespace XboxKeyboardMouse {
    class TranslateMouse {
        static private DirectInput device;
        static private Mouse mouse;

        public static void InitMouse() {
            device = new DirectInput();
            mouse = new Mouse(device);
            mouse.Acquire();
        }

        static Point centered = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
        static short iMax = short.MaxValue;
        static short iMin = short.MinValue;

        public static void MouseMovementInput() {
            centered = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            Cursor.Position = centered;

            const int Mode_Default  = 0;
            const int Mode_Relative = 1;

            while (true) {
                var mode = Program.ActiveConfig.Mouse_Eng_Type;

                switch (mode) {
                case Mode_Default:  MouseMovement_Default(); break;
                case Mode_Relative: MouseMovement_Relative(); break;
                default:            break;
                }

                Thread.Sleep(Program.ActiveConfig.Mouse_TickRate);
            }
        }

        private static void MouseMovement_Default() {
            Point mouse = Control.MousePosition;

            short joyX;
            short joyY;

            // 
            // I need to find a way to make this
            // more efficent.
            // 
            // How it works, its a simple concept that
            // killed my brain because  i suck at maths 
            // somehow?
            //
            // We want to get the percentage of the mouse
            // from the center and we use that value to
            // get the value of (mouse)% in Short.MaxValue, 
            // this gives us a relative value for the joysticks
            // 
            // So in kinda laymans code terms: 
            // let Percentage = % of Mouse from Center
            //  - This is the % of the mouse from the center
            //    of the screen, this allows us to make it 
            //    relative to the joystick
            // Percentage = Percentage + (Percentage * Sensitivity)
            // let JoystickAXI = Calculate Value of (% of Max Short)
            // set Xbox.Joysticks.AXI = JoystickAXI
            //
            // How it works past that, not even i know.
            // How well it works? Try it for your self its not perfect but it'll do.

            /* Calculate Joystick X */ {
                double x;
                if (Program.ActiveConfig.Mouse_Invert_X)
                     x = centered.X - mouse.X;
                else x = mouse.X - centered.X;

                double max = (Screen.PrimaryScreen.Bounds.Width / 4) * 1.5;
                double current = x + (x * Program.ActiveConfig.Mouse_Sensitivity_X);
                double percentage = (current / max) * Program.ActiveConfig.Mouse_FinalMod;
                double number = iMax;
                double final = number * percentage / 100;

                if      (final >= iMax && final > 0) final = iMax;
                else if (final <= iMin && final < 0) final = iMin;

                joyX = Convert.ToInt16(final);
            }

            /* Calculate Joystick Y */ {
                double y;
                if (Program.ActiveConfig.Mouse_Invert_Y)
                     y = mouse.Y - centered.Y;
                else y = centered.Y - mouse.Y;

                double max = Screen.PrimaryScreen.Bounds.Height / 2;
                double current = y + (y * Program.ActiveConfig.Mouse_Sensitivity_Y);
                double percentage = (current / max) * Program.ActiveConfig.Mouse_FinalMod;
                double number = iMax;
                double final = number * percentage / 100;

                if (final >= iMax && final > 0) final = iMax;
                else if (final <= iMin && final < 0) final = iMin;
                joyY = Convert.ToInt16(final);
            }

            //PrintVals(joyX, joyY);
            Activate.Controller.RightStickX = joyX;
            Activate.Controller.RightStickY = joyY;
            Activate.SendtoController(Activate.Controller);

            Cursor.Position = centered;
        }

        private static void MouseMovement_Relative() {

            Point mouse = Control.MousePosition;
            short joyX = Activate.Controller.RightStickX;
            short joyY = Activate.Controller.RightStickY;

            /* 
             * The idea is to do this
             * just increment the current JOY X and Y by using
             * translated values from the mouse's X and Y new positions
             * same as above/default but instead we are incrementing position
             * and decrementing rather than just setting
             */

            /* Calculate Joystick X */ {
                double x;

                if (Program.ActiveConfig.Mouse_Invert_X)
                     x = centered.X - mouse.X;
                else x = mouse.X - centered.X;

                double max = (Screen.PrimaryScreen.Bounds.Width / 4) * 1.5;
                double current = x + (x * Program.ActiveConfig.Mouse_Sensitivity_X);
                double percentage = (current / max) * Program.ActiveConfig.Mouse_FinalMod;
                double number = iMax;
                double final = number * percentage / 100;

                if (final >= iMax && final > 0) final = iMax;
                else if (final <= iMin && final < 0) final = iMin;

                short tJoyX = Convert.ToInt16(final);
                joyX += tJoyX;
            }

            /* Calculate Joystick Y */ {
                double y;
                if (Program.ActiveConfig.Mouse_Invert_Y)
                    y = mouse.Y - centered.Y;
                else y = centered.Y - mouse.Y;

                double max = Screen.PrimaryScreen.Bounds.Height / 2;
                double current = y + (y * Program.ActiveConfig.Mouse_Sensitivity_Y);
                double percentage = (current / max) * Program.ActiveConfig.Mouse_FinalMod;
                double number = iMax;
                double final = number * percentage / 100;

                if (final >= iMax && final > 0) final = iMax;
                else if (final <= iMin && final < 0) final = iMin;

                short TJoyY = Convert.ToInt16(final);

                joyY += TJoyY;
            }


            Activate.Controller.RightStickX = joyX;
            Activate.Controller.RightStickY = joyY;
            Activate.SendtoController(Activate.Controller);

            Cursor.Position = centered;
        }

        public static void MouseButtonsInput(X360Controller controller) {
            MouseState state = mouse.GetCurrentState();

            if (state.IsPressed(0))
                 controller.RightTrigger = 255;
            else controller.RightTrigger = 0;

            if (state.IsPressed(1))
                 controller.LeftTrigger = 255;
            else controller.LeftTrigger = 0;
        }
    }
}
