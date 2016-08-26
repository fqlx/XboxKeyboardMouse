using System;
using System.Windows;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    public partial class MainForm : Form
    {
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
            SetTickCount(1);
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

    }
}
