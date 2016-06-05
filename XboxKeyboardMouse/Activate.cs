using ScpDriverInterface;
using System;
using System.Threading;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    class Activate
    {
        const int CONTROLLER_NUMBER = 1; 
        static ScpBus scpbus = null;
        public static X360Controller controller;

        public static Thread tXboxStream, tKMInput;

        private static X360Controller CreateController()
        {
            X360Controller controller = new X360Controller();

            try
            {
                scpbus = new ScpBus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SCP Bus failed to initialize");
                MessageBox.Show(ex.ToString());
            }

            scpbus.PlugIn(1);

            return controller;
        }

        private static void Init()
        {
            controller = CreateController();

            TranslateMouse.InitMouse();
        }

        private static void KeyboardMouseInput()
        {
            while (true)
            {
                TranslateKeyboard.KeyboardInput(controller);
                TranslateMouse.MouseButtonsInput(controller);

                SendtoController(controller);
            }
        }


        public static void ActivateKeyboardAndMouse()
        {
            Init();

            tXboxStream = new Thread(XboxStream.ToggleCursor);
            tXboxStream.SetApartmentState(ApartmentState.STA);
            tXboxStream.IsBackground = true;
            tXboxStream.Start();

            tKMInput = new Thread(Activate.KeyboardMouseInput);
            tKMInput.SetApartmentState(ApartmentState.STA);
            tKMInput.IsBackground = true;
            tKMInput.Start();
            
            Program.mainform.StatusRunning();
        }

        public static void SendtoController(X360Controller controller)
        {
            byte[] report = controller.GetReport();
            byte[] output = new byte[8];

            scpbus.Report(CONTROLLER_NUMBER, report, output);
        }

        public static void OnProcessExit(object sender, EventArgs e)
        {
            scpbus.Unplug(CONTROLLER_NUMBER);
            Application.ExitThread();
        }
    }
}

