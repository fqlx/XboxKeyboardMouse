using System;
using System.Windows;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    public partial class MainForm : Form
    {
        private static string helpText = "In order to have the most lag-free, precise mouse movements, you can change how fast the program calculates for mouse inputs." + System.Environment.NewLine + System.Environment.NewLine +
            "The lower the number, the more precise your mouse movements will be. With number such as 50 ticks, the program will calculate your mouse inputs every 50 milliseconds (around 20 fps)." + System.Environment.NewLine + System.Environment.NewLine +
            "If you have a faster computer, you can set the ticks lower, such as 10 ticks to get 100 fps." + System.Environment.NewLine + System.Environment.NewLine +
            "HOWEVER, if you set the ticks too low and your computer can’t handle it, you will see noticeable lag and stuttering in your game. At this point you should set your ticks higher to a comfortable level until the stuttering disappears." + System.Environment.NewLine + System.Environment.NewLine +
            "Changing your ticks a significant amount may alter your mouse sensitivity slightly. To offset this, you can change your sensitivity in-game.";

        public MainForm()
        {
            InitializeComponent();
        }

        void startButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static void OnProcessExit(object sender, EventArgs e)
        {

        }

        public void StatusRunning()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(StatusRunning);
                Invoke(method);
                return;
            }
            Status.Text = string.Format("Running");
            Status.ForeColor = System.Drawing.Color.Green;
        }

        public void StatusStopped()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(StatusStopped);
                Invoke(method);
                return;
            }
            Status.Text = String.Format("Stopped");
            Status.ForeColor = System.Drawing.Color.Red;
        }

        public void StatusWaiting()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(StatusWaiting);
                Invoke(method);
                return;
            }
            Status.Text = string.Format("Waiting");
            Status.ForeColor = System.Drawing.Color.YellowGreen;
        }


        public void StatusTickCountSaved()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(StatusTickCountSaved);
                Invoke(method);
                return;
            }
            Status.Text = string.Format("Tick Count Saved");
            Status.ForeColor = System.Drawing.Color.Green;
        }

        private void Tickcount_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericUpDown numeric = (NumericUpDown)sender;
            decimal value = numeric.Value;
            SetTickCount((int)value);
        }

        public void SetTickCount(int tickcount)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    SetTickCount(tickcount);
                });
                return;
            }

            numericUpDown1.Value = tickcount;
            TranslateMouse.SetFramePerTick(tickcount);
            StatusTickCountSaved();
        }

        public int GetTickCount()
        {
            return (int)numericUpDown1.Value;
        }

        private void helpTicksBtn_Click(object sender, EventArgs e)
        {
            // TODO: put text helping the user here.    Matthew Mistele 8/29/16
            MessageBox.Show(helpText, "What are ticks?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
