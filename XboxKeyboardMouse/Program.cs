using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using XboxKeyboardMouse;

namespace XboxKeyboardMouse
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static MainForm mainform = new MainForm();

        [STAThread]
        static void Main()
        {
            Thread tApplicationRun = new Thread(ApplicationRun);
            tApplicationRun.SetApartmentState(ApartmentState.STA);
            tApplicationRun.Start();

            Thread.Sleep(3000);  //just for status "change effect"

            Thread tPause = new Thread(Pause);
            tPause.SetApartmentState(ApartmentState.STA);
            tPause.IsBackground = true;
            tPause.Start();

            Thread tActivateKM = new Thread(Activate.ActivateKeyboardAndMouse);
            tActivateKM.SetApartmentState(ApartmentState.STA);
            tActivateKM.IsBackground = true;
            tActivateKM.Start();
        }

        private static void ApplicationRun()
        {
            Application.EnableVisualStyles();
            Application.Run(mainform);
        }

        private static void Pause()
        {
            while (true)
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C))
                {
                    if (Activate.tKMInput.IsAlive == true && Activate.tXboxStream.IsAlive == true)
                    {
                        Activate.tXboxStream.Abort();
                        Activate.tKMInput.Abort();
                        XboxStream.tMouseMovement.Abort();
                        CursorView.CursorShow();

                        if (Activate.tKMInput.IsAlive == true && Activate.tXboxStream.IsAlive == true)
                            MessageBox.Show("Error:  Threads failed to abort");
                        else
                            mainform.StatusStopped();
                    }
                    else if (Activate.tKMInput.IsAlive == false && Activate.tXboxStream.IsAlive == false)
                    {
                        Thread tActivateKM = new Thread(Activate.ActivateKeyboardAndMouse);
                        tActivateKM.SetApartmentState(ApartmentState.STA);
                        tActivateKM.IsBackground = true;
                        tActivateKM.Start();
                    }
                }
                Thread.Sleep(100);
            }
        }

    }
}
