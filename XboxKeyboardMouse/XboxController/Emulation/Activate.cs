using ScpDriverInterface;
using System;
using System.Threading;
using System.Windows.Forms;
using XboxKeyboardMouse.Libs;

namespace XboxKeyboardMouse {
    class Activate {
        const int CONTROLLER_NUMBER = 1;
        static ScpBus scpbus = null;
        public static X360Controller Controller;

        public static Thread tXboxStream, tKMInput;

        private static X360Controller CreateController() {
            X360Controller controller = new X360Controller();

            try {
                scpbus = new ScpBus();
            } catch (Exception ex) {
                MessageBox.Show("SCP Bus failed to initialize");
                MessageBox.Show(ex.ToString());
            }

            scpbus.PlugIn(1);

            return controller;
        }

        private static void Init() {
            Controller = CreateController();

            TranslateMouse.InitMouse();
        }

        private static void KeyboardMouseInput() {
            while (true) {
                TranslateMouse.MouseButtonsInput(Controller);
                TranslateKeyboard.KeyboardInput(Controller);

                SendtoController(Controller);
            }
        }


        public static void ActivateKeyboardAndMouse(bool ActivateStreamThread = true, bool ActivateInputThread = true) {
            Init();

            // Cursor Toggle thread
            if (ActivateStreamThread) {
                tXboxStream = new Thread(XboxStream.ToggleCursor);
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

        public static void SendtoController(X360Controller controller) {
            byte[] report = controller.GetReport();
            byte[] output = new byte[8];

            // Send our data back to the virtual scp bus.
            scpbus.Report(CONTROLLER_NUMBER, report, output);
        }

        public static void SendGuide(bool state) {
            if (state)
                 Controller.Buttons = Controller.Buttons |  X360Buttons.Guide;
            else Controller.Buttons = Controller.Buttons & ~X360Buttons.Guide;
            
            SendtoController(Controller);
        }

        public static void OnProcessExit(object sender, EventArgs e) {
            scpbus.Unplug(CONTROLLER_NUMBER);
            Application.ExitThread();
        }

        public static void ResetController() {
            Controller.RightStickX = 0;
            Controller.RightStickY = 0;
            Controller.LeftStickX = 0;
            Controller.LeftStickY = 0;

            Controller.LeftTrigger = 0;
            Controller.RightTrigger = 0;

            Controller.Buttons = X360Buttons.None;
        }
    }
}

