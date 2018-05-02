using SlimDX.DirectInput;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Threading;
using SimWinInput;

namespace XboxKeyboardMouse {
    class TranslateMouse {
        /// <summary>The magnitude of dead zone size increases, each mouse polling, when running at regular calibration speed.</summary>
        /// <remarks>Hand-tested value for balancing calibration speed versus accuracy, with a fine-tune calibration option in mind.</remarks>
        private static short RegularCalibrationSpeedAdvancementPerPoll = 50;

        /// <summary>The magnitude of dead zone size increases, each mouse polling, when running at fine-tuning calibration speed.</summary>
        /// <remarks>Hand-tested value for balancing accuracy over speed, knowing our starting value should already be "close" to our target.option in mind.</remarks>
        private static short FineTuneCalibrationSpeedAdvancementPerPoll = 10;

        public static bool TRIGGER_LEFT_PRESSED  = false;
        public static bool TRIGGER_RIGHT_PRESSED = false;

        static private DirectInput device;
        static private Mouse mouse;

        public static void InitMouse() {
            device = new DirectInput();
            mouse = new Mouse(device);
            mouse.Acquire();
            stopwatch.Start();
        }

        static Point centered = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
        static short iMax = short.MaxValue;
        static short iMin = short.MinValue;
        
        public static void MouseMovementInput() {
            centered = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            
            while (true) {
                switch (Program.ActiveConfig.Mouse_Eng_Type) {
                    case MouseTranslationMode.Percentage: MouseMovement_Percentage(); break;
                    case MouseTranslationMode.Relative:   MouseMovement_Relative();   break;
                    case MouseTranslationMode.Raw:        MouseMovement_Raw();        break;
                    case MouseTranslationMode.RawSens:    MouseMovement_Raw_S();      break;
                    case MouseTranslationMode.DeadZoning: MouseMovement_DeadZoning(); break;
                    default: break;
                }

                Thread.Sleep(Program.ActiveConfig.Mouse_TickRate);
            }
        }

        private static DeadZoneCalibrator deadZoneCalibrator = null;

        private static Stopwatch stopwatch = Stopwatch.StartNew();

        private static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
        }
        
        public static void RunCalibrateDeadZone()
        {
            HandleCalibrator(0, RegularCalibrationSpeedAdvancementPerPoll);
        }

        public static void RunFineTuneDeadZone()
        {
            // Fine tuning will go back a bit from where the last calibration ended, since human response
            // times are limited, and iterate slower this time so the user end with a more accurate value.
            HandleCalibrator((short)(Program.ActiveConfig.DeadZoneSize * 0.85), FineTuneCalibrationSpeedAdvancementPerPoll);
        }

        private static void HandleCalibrator(short startSize, short incrementSize)
        {
            if (deadZoneCalibrator != null)
            {
                Program.ActiveConfig.DeadZoneSize = deadZoneCalibrator.CurrentDeadZone;
                deadZoneCalibrator = null;
            }
            else
            {
                deadZoneCalibrator = new DeadZoneCalibrator(startSize, incrementSize);
            }
        }

        private static void MouseMovement_DeadZoning()
        {
            // Each game may use "dead zones" of different size and shape to eliminate near-but-not-zero readings
            // that controllers give at rest. (See https://itstillworks.com/12629709/what-is-a-dead-zone-in-an-fps.)
            // Since mice do not suffer from this phenomenon at rest, users expect reaction out of the minimum
            // possible mouse movements. To translate this to joystick positions effectively, we want that single
            // pixel of mouse movement to position the joystick just past the joystick dead zone. (For effective 
            // mouse control of any given game, we will have to discover and track dead zone details as part of the 
            // control profile settings, or provide an auto-discovery mechanism that watches for screen change in
            // order to automatically determine the dead zone details. Difficult, but doable. TODO: ATTEMPT THIS!)
            // Possibly other details would need to be stored per game, as advanced techniques to combat other 
            // game-specific joystick assists (weird accelerations and such) get figured out. Either way, usually
            // the user should get best results out of tuning their in-game sensitivities way up, to allow for the 
            // mouse-to-joystick translation to yield a high, accurate range, including fast mouse flicks to attain
            // fast turning.
            // Dead zones are usually square or circular, meaning either each axis is remapped to zero out small
            // values axis-independently (for a square dead zone), or the vector is remapped to zero out small 
            // vector lengths (for a circular dead zone). Thus, a slightly different algorithm will need to be used
            // for each dead zone type.

            // Grab and reset mouse position and time passed, for calculating mouse velocity for this polling.
            var mouseX = Cursor.Position.X;
            var mouseY = Cursor.Position.Y;
            Cursor.Position = centered;
            var timeSinceLastPoll = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            // Mouse velocity here is average pixels per milliseconds passed since the last polling. This helps 
            // get correct-feeling, smooth movements, even if the thread pool is not being particularly nice to
            // our polling thread ATM. For example, if the thread pool gives us a 22ms cycle from last poll with
            // 11 pixels of movement on one pass, then gave us a 16ms cycle with 8 pixels of movement on another,
            // we'd want the same final stick position for both since the user did not vary their mouse velocity.
            // Also invert these values now if the user has configured input inversion.
            var changeX = Program.ActiveConfig.Mouse_Invert_X ? centered.X - mouseX : mouseX - centered.X;
            var changeY = Program.ActiveConfig.Mouse_Invert_Y ? mouseY - centered.Y : centered.Y - mouseY;
            double velocityX = changeX / timeSinceLastPoll;
            double velocityY = changeY / timeSinceLastPoll;

            short joyX, joyY;

            var deadZoneSize = Program.ActiveConfig.DeadZoneSize;
            if (deadZoneCalibrator != null)
            {
                // Pretend the mouse is always moving one pixel in each axis, until user intervention.
                deadZoneSize = deadZoneCalibrator.AdvanceDeadZoneSize();
                joyX = Convert.ToInt16(deadZoneSize);
                joyY = Convert.ToInt16(deadZoneSize);
            }
            else // TODO: IF DEAD ZONE MODE IS SQUARE!
            {
                // For a square dead zone, each axis can be scaled independently.
                double maxRespectedVelocity = 5d;
                double percentMouseX = velocityX == 0 ? 0 : velocityX / maxRespectedVelocity;
                double percentMouseY = velocityY == 0 ? 0 : velocityY / maxRespectedVelocity;

                // Clamp the movement to +/- 100% to avoid overflowing the final joystick values after scaling.
                // (TODO: BASE THESE MAX RESPECTED MOUSE MOVEMENT ON CURRENT CONFIG SETTINGS TOO!)
                percentMouseX = Clamp(percentMouseX, -1d, 1d);
                percentMouseY = Clamp(percentMouseY, -1d, 1d);

                // Start each axis adjusted to the appropriate dead zone edge (left/right or top/bottom).
                var deadAdjustX = velocityX == 0 ? 0 : deadZoneSize * (velocityX > 0 ? 1 : -1);
                var deadAdjustY = velocityY == 0 ? 0 : deadZoneSize * (velocityY > 0 ? 1 : -1);

                // The final axis position adds in the percentage of remaining stick magnitude.
                // (Minimum mouse movement should become just-past-deadzone; maximum respected mouse movement
                // should become short.MaxValue in the end.)
                var remainingStickMagnitude = short.MaxValue - deadZoneSize;
                joyX = Convert.ToInt16(remainingStickMagnitude * percentMouseX + deadAdjustX);
                joyY = Convert.ToInt16(remainingStickMagnitude * percentMouseY + deadAdjustY);
            }
            // TODO: ELSE: If dead zone mode is circular! (Vector math.)

            // Send Axis
            SetAxis(joyX, joyY);
        }

        private static void MouseMovement_Percentage() {
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

            // Send Axis
            SetAxis(joyX, joyY);

            Cursor.Position = centered;
        }

        private static void MouseMovement_Relative() {

            Point mouse = Control.MousePosition;
            short joyX = SimGamePad.Instance.State[0].RightStickX;
            short joyY = SimGamePad.Instance.State[0].RightStickY;

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
            
            // Send Axis
            SetAxis(joyX, joyY);

            Cursor.Position = centered;
        }

        private static void MouseMovement_Raw() {
            Point mouse = Control.MousePosition;

            short joyX = 0;
            short joyY = 0;

            double x = Program.ActiveConfig.Mouse_Invert_X ?
                       centered.X - mouse.X : mouse.X - centered.X;
            double y = Program.ActiveConfig.Mouse_Invert_Y ?
                       mouse.Y - centered.Y : centered.Y - mouse.Y;

            if (x > short.MaxValue)
                x = short.MaxValue;
            else if (x < short.MinValue)
                x = short.MinValue;

            if (y > short.MaxValue)
                y = short.MaxValue;
            else if (y < short.MinValue)
                y = short.MinValue;

            // Lets just first time apply raw values 
            // to the sticks
            joyX = (short)x;
            joyY = (short)y;

            // Send Axis
            SetAxis(joyX, joyY);

            Cursor.Position = centered;
        }
        
        private static void MouseMovement_Raw_S() {
            Point mouse = Control.MousePosition;

            short joyX = 0;
            short joyY = 0;

            double x = Program.ActiveConfig.Mouse_Invert_X ? 
                       centered.X - mouse.X : mouse.X - centered.X;
            double y = Program.ActiveConfig.Mouse_Invert_Y ?
                       mouse.Y - centered.Y : centered.Y - mouse.Y;

            x = x * Program.ActiveConfig.Mouse_Sensitivity_X;
            y = y * Program.ActiveConfig.Mouse_Sensitivity_Y;

            if (x > short.MaxValue)
                x = short.MaxValue;
            else if (x < short.MinValue)
                x = short.MinValue;

            if (y > short.MaxValue)
                y = short.MaxValue;
            else if (y < short.MinValue)
                y = short.MinValue;

            // Lets just first time apply raw values 
            // to the sticks
            joyX = (short)x;
            joyY = (short)y;

            // Send Axis
            SetAxis(joyX, joyY);

            Cursor.Position = centered;
        }

        public static void MouseButtonsInput(SimulatedGamePadState controller) {
            MouseState state = mouse.GetCurrentState();

            TRIGGER_LEFT_PRESSED  = state.IsPressed(0);
            TRIGGER_RIGHT_PRESSED = state.IsPressed(1);

            if (state.IsPressed(0)) {
                controller.RightTrigger = 255;
            } else {
                if (!TranslateKeyboard.TRIGGER_RIGHT_PRESSED)
                    controller.RightTrigger = 0;
            }

            if (state.IsPressed(1)) {
                controller.LeftTrigger = 255;
            } else {
                if (!TranslateKeyboard.TRIGGER_LEFT_PRESSED)
                    controller.LeftTrigger = 0;
            }
        }
        
        private static void SetAxis(short x, short y) {
        #if (DEBUG)
            //Logger.appendLogLine("Mouse", $"Mouse (X, Y) = ({x.ToString().PadRight(6, ' ')}, {y.ToString().PadRight(6, ' ')})", Logger.Type.Controller);
        #endif

            if (Program.ActiveConfig.Mouse_Is_RightStick) {
                SimGamePad.Instance.State[0].RightStickX = x;
                SimGamePad.Instance.State[0].RightStickY = y;
            } else {
                SimGamePad.Instance.State[0].LeftStickX = x;
                SimGamePad.Instance.State[0].LeftStickY = y;
            }

            SimGamePad.Instance.Update();
        }
    }
}
