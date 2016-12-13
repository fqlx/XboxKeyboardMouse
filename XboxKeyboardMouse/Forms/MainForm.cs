using System;
using System.Windows;
using System.Windows.Forms;
using XboxKeyboardMouse.Forms;

namespace XboxKeyboardMouse {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        public void StatusRunning() {
            if (InvokeRequired) {
                MethodInvoker method = new MethodInvoker(StatusRunning);
                Invoke(method);
                return;
            }

            Status.Text = string.Format("Running");
            Status.ForeColor = System.Drawing.Color.Green;
        }

        public void StatusStopped() {
            if (InvokeRequired) {
                MethodInvoker method = new MethodInvoker(StatusStopped);
                Invoke(method);
                return;
            }
            Status.Text = String.Format("Stopped");
            Status.ForeColor = System.Drawing.Color.Red;
        }

        public void StatusWaiting() {
            try {
                if (InvokeRequired) {
                    MethodInvoker method = new MethodInvoker(StatusWaiting);
                    Invoke(method);
                    return;
                }

                Status.Text = string.Format("Waiting");
                Status.ForeColor = System.Drawing.Color.YellowGreen;
            } catch (Exception) {
                // Object has been disposed
            }
        }

        Options frmOptions;
        private void button1_Click(object sender, EventArgs e) {
            if (frmOptions == null || frmOptions.IsDisposed)
                frmOptions = new Options();
            
            frmOptions.Show();
        }

        ControllerPreview cPreview;
        private void button2_Click(object sender, EventArgs e) {
            if (cPreview == null || cPreview.IsDisposed)
                cPreview = new ControllerPreview();

            cPreview.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            // Save the current active config 
            var file = Program.ActiveConfig.Name + ".ini";
            Config.Data.Save(file, Program.ActiveConfig);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            Hooks.LowLevelKeyboardHook.LockEscape = checkBox1.Checked;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }
    }
}
