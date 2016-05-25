using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(InputToController.OnProcessExit);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Form1.OnProcessExit);

            while (true)
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.F4))
                    Environment.Exit(0);
                Thread.Sleep(1000);
            }

        }

    }
}
