using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace XboxKeyboardMouse.Forms {
    public partial class SelectKey : Form {
        Config.Data cfg;
        string Tag;

        int KeyCode = 0;

        public SelectKey(Config.Data cfg, string Tag) {
            InitializeComponent();

            this.cfg = cfg;
            this.Tag = Tag;

            this.Text += Tag;

            foreach (Control c in Controls)
                c.KeyUp += this.SelectKey_KeyUp;

            KeyUp += this.SelectKey_KeyUp;
        }

        public void SaveKeyCode() {
            switch (Tag) {

            case "LS":      cfg.Controls_KB_Xbox_Sticks_Left  = KeyCode; break;
            case "RS":      cfg.Controls_KB_Xbox_Sticks_Right = KeyCode; break;

            case "SL":      cfg.Controls_KB_Xbox_LeftBumper  = KeyCode; break;
            case "SR":      cfg.Controls_KB_Xbox_RightBumper = KeyCode; break;

            case "BACK":    cfg.Controls_KB_Xbox_Back  = KeyCode; break;
            case "START":   cfg.Controls_KB_Xbox_Start = KeyCode; break;
            case "GUIDE":   cfg.Controls_KB_Xbox_Guide = KeyCode; break;

            case "A":       cfg.Controls_KB_Xbox_A = KeyCode; break;
            case "B":       cfg.Controls_KB_Xbox_B = KeyCode; break;
            case "X":       cfg.Controls_KB_Xbox_X = KeyCode; break;
            case "Y":       cfg.Controls_KB_Xbox_Y = KeyCode; break;

            case "DPU":     cfg.Controls_KB_Xbox_DPAD_Up    = KeyCode; break;
            case "DPD":     cfg.Controls_KB_Xbox_DPAD_Down  = KeyCode; break;
            case "DPL":     cfg.Controls_KB_Xbox_DPAD_Left  = KeyCode; break;
            case "DPR":     cfg.Controls_KB_Xbox_DPAD_Right = KeyCode; break;

            case "TL":      cfg.Controls_KB_Xbox_Trigger_Left  = KeyCode; break;
            case "TR":      cfg.Controls_KB_Xbox_Trigger_Right = KeyCode; break;

            case "JLU":     cfg.Controls_KB_Sticks_AXIS_L_Up    = KeyCode; break;
            case "JLD":     cfg.Controls_KB_Sticks_AXIS_L_Down  = KeyCode; break;
            case "JLL":     cfg.Controls_KB_Sticks_AXIS_L_Left  = KeyCode; break;
            case "JLR":     cfg.Controls_KB_Sticks_AXIS_L_Right = KeyCode; break;

            case "JRU":     cfg.Controls_KB_Sticks_AXIS_R_Up    = KeyCode; break;
            case "JRD":     cfg.Controls_KB_Sticks_AXIS_R_Down  = KeyCode; break;
            case "JRL":     cfg.Controls_KB_Sticks_AXIS_R_Left  = KeyCode; break;
            case "JRR":     cfg.Controls_KB_Sticks_AXIS_R_Right = KeyCode; break;

            }
        }

        private void button1_Click(object sender, EventArgs e) {
            SaveKeyCode();
            this.Close();
        }

        private void SelectKey_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            // Get keycode
            KeyCode = (int)KeyInterop.KeyFromVirtualKey((int)e.KeyValue);
            label2.Text = e.KeyData.ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            KeyCode = (int)Key.None;
            SaveKeyCode();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
