using ScpDriverInterface;
using System;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    class InputToController
    {
        static ScpBus scpbus = null;

        public static void startSCPBus()
        {
            X360Controller controller = new X360Controller();
            byte[] output = new byte[8];
     
            try
            {
                scpbus = new ScpBus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SCP Bus failed to initialize");
            }

            scpbus.PlugIn(1);

            while(true)
            {
               controller = TranslateInput.keyboardInput(controller);
                scpbus.
            }
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            scpbus.Unplug(1);
        }
    }
}

