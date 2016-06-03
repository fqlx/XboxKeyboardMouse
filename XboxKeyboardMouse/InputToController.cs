using ScpDriverInterface;
using System;
using System.Threading;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    class InputToController
    {
        const int CONTROLLER_NUMBER = 1; 
        static ScpBus scpbus = null;

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

        public static void ActivateKeyboardAndMouse()
        {
            X360Controller controller = CreateController();

            TranslateMouse.InitMouse();

            Thread thrStream = new Thread(XboxStream.ToggleCursor);
            thrStream.SetApartmentState(ApartmentState.STA);
            thrStream.Start();

            while (true)
            {
                TranslateKeyboard.KeyboardInput(controller);
                TranslateMouse.MouseButtonsInput(controller);

                SendtoController(controller);
            }
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

