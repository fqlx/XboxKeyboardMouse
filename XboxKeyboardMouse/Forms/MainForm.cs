using MaterialSkin;
using System;
using System.Drawing;
using System.Windows.Forms;
using XboxKeyboardMouse.Libs;

namespace XboxKeyboardMouse.Forms
{
    public partial class MainForm : Controls.RoundedForm {
        enum STATE {
            Running = 1,
            Waiting = 2,
            Stopped = 3,
            None    = 4
        }

        public class 
        MultiThreadColorSyncCheckingClassThatActuallyWorksUnlikeDamnEnumsLikeWtfMicrosoftMakeYourDamnFrameworkClassesWorkViaMultithreadingSuchAsEnums_MadFace {
            public int State  = 1;
            public int LState = 4;
        }

        public Color Color_Stopped              = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public Color Color_Waiting              = Color.FromArgb(0xFF, 0x3F, 0x51, 0xB5);
        public Color Color_On                   = Color.FromArgb(0xFF, 0x4C, 0xAF, 0x50);
        public const Primary Color_Stopped_P    = Primary.Red800;    // C62828
        public const Primary Color_Waiting_P    = Primary.Indigo500; // 3F51B5
        public const Primary Color_On_P         = Primary.Green500;  // 4CAF50

        Timer ColorChangeCheck = new Timer();

        // TLDR; Enums do not like multi-threading
        //       ask the 1 and a half~ hours of my
        //       life ;(
        static 
        MultiThreadColorSyncCheckingClassThatActuallyWorksUnlikeDamnEnumsLikeWtfMicrosoftMakeYourDamnFrameworkClassesWorkViaMultithreadingSuchAsEnums_MadFace
        StatusSync = new 
        MultiThreadColorSyncCheckingClassThatActuallyWorksUnlikeDamnEnumsLikeWtfMicrosoftMakeYourDamnFrameworkClassesWorkViaMultithreadingSuchAsEnums_MadFace();

        public Controls.FormRWE    cOptions;
        public ControllerPreview cPreview;
        
        public MainForm() {
            InitializeComponent();
            
            materialLabel1.MouseDown += FormMouseMove;
            panel1.MouseDown         += FormMouseMove;
            powerButton.MouseDown    += FormPreMouseMove;
            powerButton.MouseUp      += setStatus;

            ColorChangeCheck.Tick    += CSTimr_Tick;
            ColorChangeCheck.Interval = 100;
            ColorChangeCheck.Start();
        }

        private void CSTimr_Tick(object sender, EventArgs e) {
            if (StatusSync.State != StatusSync.LState) {
                Primary p; 

                if (StatusSync.State == 1) {
                    p = Color_On_P;
                    powerButton.BackColor = Color_On;
                } else if (StatusSync.State == 2) {
                    p = Color_Waiting_P;
                    powerButton.BackColor = Color_Waiting;
                } else if (StatusSync.State == 3) {
                    p = Color_Stopped_P;
                    powerButton.BackColor = Color_Stopped;
                } else {
                    p = Primary.Blue800;
                }
                 
                Program.mSkin.ColorScheme = 
                   new ColorScheme(p, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

                if (cOptions != null)
                    cOptions.SetStatusColor(powerButton.BackColor);

                StatusSync.LState = StatusSync.State;
            }
        }

        #region Status
        public void StatusRunning() {
            if (StatusSync.State != 1)
                Logger.appendLogLine("Status", "Changed to Running", Logger.Type.Controller);

            StatusSync.State = 1;
        }

        public void StatusWaiting() {
            if (StatusSync.State != 2)
                Logger.appendLogLine("Status", "Changed to Waiting", Logger.Type.Controller);

            StatusSync.State = 2;
        }

        public void StatusStopped() {
            if (StatusSync.State != 3)
                Logger.appendLogLine("Status", "Changed to Stopped", Logger.Type.Controller);

            StatusSync.State = 3;
        }
    #endregion

        public bool HitTest(PictureBox control, int x, int y) {
            var result = false;
            if (control.Image == null)
                return result;
            var method = typeof(PictureBox).GetMethod("ImageRectangleFromSizeMode",
              System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var r = (Rectangle)method.Invoke(control, new object[] { control.SizeMode });
            using (var bm = new Bitmap(r.Width, r.Height)) {
                using (var g = Graphics.FromImage(bm))
                    g.DrawImage(control.Image, 0, 0, r.Width, r.Height);
                if (r.Contains(x, y) && bm.GetPixel(x - r.X, y - r.Y).A != 0)
                    result = true;
            }
            return result;
        }

        public void FormPreMouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            Bitmap b = new Bitmap(powerButton.Image);
            Color color = b.GetPixel(e.X, e.Y);

            // Ensure we are not blocking our power button
            if (!HitTest((PictureBox)sender, e.X, e.Y) && e.Button == MouseButtons.Left)
                 FormMouseMove(sender, e);
        }

        private void setStatus(object sender, MouseEventArgs e) {
            if (HitTest((PictureBox)sender, e.X, e.Y) && e.Button == MouseButtons.Left) {
                // Toggle the thread on/off
                Program.ToggleStatusState = true;

                #if (DEBUG)
                    Logger.appendLogLine("MainForm", "Set ToggleStatusState = True", Logger.Type.Debug);
                #endif

                ColorChangeCheck.Start();
            }
        }

        private void updateCursorIcon(object sender, MouseEventArgs e) {
            if (HitTest(((PictureBox)sender), e.X, e.Y))
                 ((PictureBox)sender).Cursor = Cursors.Hand;
            else ((PictureBox)sender).Cursor = Cursors.Default;
        }

        private void materialFlatButton2_Click(object sender, EventArgs e) {
            if (cPreview == null || cPreview.IsDisposed)
                cPreview = new ControllerPreview();
            
            cPreview.Show();
            cPreview.Focus();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e) {
            if (cOptions == null || cOptions.IsDisposed) {
                cOptions = new Options();
                cOptions.SetStatusColor(powerButton.BackColor);
            }
            

            cOptions.Show();
            cOptions.Focus();
        }

        private void materialFlatButton3_Click(object sender, EventArgs e) {
            this.Close();
        }
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            // Save the current active config 
            var file = Program.ActiveConfig.Name + ".ini";
            Config.Data.Save(file, Program.ActiveConfig);
        }
    }
}
