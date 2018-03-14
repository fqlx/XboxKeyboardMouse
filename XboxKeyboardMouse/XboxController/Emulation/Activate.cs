using SimWinInput;
using System;
using System.Threading;
using System.Windows.Forms;
using XboxKeyboardMouse.Libs;

namespace XboxKeyboardMouse {
    class Activate {
        public static Thread tXboxStream, tKMInput;
        private static SimGamePad simPad;
        private static SimulatedGamePadState state;
        private static bool shuttingDown = false;

        private static void Init()
        {
            simPad = SimGamePad.Instance;
            try
            {
                simPad.Initialize();
                simPad.PlugIn();
                state = simPad.State[0];
            }
            catch
            {
                ShutDown();
                MessageBox.Show("Could not initialize SimGamePad / ScpBus. Shutting down.");
                Application.Exit();
            }
            
            TranslateMouse.InitMouse();
        }

        private static void KeyboardMouseInput() {
            while (!shuttingDown) {
                TranslateMouse.MouseButtonsInput(state);
                TranslateKeyboard.KeyboardInput(state);
                SimGamePad.Instance.Update();

                // Poll aggressively, but avoid completely pegging the CPU to 100%.
                Thread.Sleep(1);
            }
        }

        public static void ActivateKeyboardAndMouse(bool ActivateStreamThread = true, bool ActivateInputThread = true) {
            Init();

            // Cursor Toggle thread
            if (ActivateStreamThread) {
                tXboxStream = new Thread(XboxStream.XboxAppDetector);
                tXboxStream.SetApartmentState(ApartmentState.STA);
                tXboxStream.IsBackground = true;
                tXboxStream.Start();

                #if (DEBUG)
                    Logger.appendLogLine("Threads", "Starting: tXboxStream thread", Logger.Type.Info);
                #endif
            }

            // Keyboard and Mouse input thread
            if (ActivateInputThread) {
                tKMInput = new Thread(Activate.KeyboardMouseInput);
                tKMInput.SetApartmentState(ApartmentState.STA);
                tKMInput.IsBackground = true;
                tKMInput.Start();

                #if (DEBUG)
                    Logger.appendLogLine("Threads", "Starting: tKMInput thread", Logger.Type.Info);
                #endif
            }

            // Set our status to waiting
            Program.MainForm.StatusWaiting();
        }

        public static void ShutDown()
        {
            shuttingDown = true;
            simPad.ShutDown();
        }
        
        public static void SendGuide(bool buttonDown) {
            if (buttonDown)
                 state.Buttons |= GamePadControl.Guide;
            else state.Buttons &= ~GamePadControl.Guide;
            simPad.Update();
        }
        
        public static void ResetController() {
            state.RightStickX = 0;
            state.RightStickY = 0;
            state.LeftStickX = 0;
            state.LeftStickY = 0;

            state.LeftTrigger = 0;
            state.RightTrigger = 0;

            state.Buttons = GamePadControl.None;
        }
    }
}
