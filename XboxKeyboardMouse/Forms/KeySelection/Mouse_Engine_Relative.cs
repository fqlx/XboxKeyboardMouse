using System;
using System.Windows.Forms;

namespace XboxKeyboardMouse.Forms.OptionsFrms {
    public partial class Mouse_Engine_Relative : Form {
        Config.Data cfg;

        public Mouse_Engine_Relative(Config.Data cfg) {
            InitializeComponent();

            this.cfg = cfg;

            Controls_KB_MReset_KEY = cfg.Controls_KB_MReset_KEY;
            Controls_KB_MReset_MOD = cfg.Controls_KB_MReset_MOD;

            resetButtonText();
        }

        int Controls_KB_MReset_KEY;
        int Controls_KB_MReset_MOD;

        private void resetButtonText() {
            string kbMod = ((System.Windows.Input.Key)Controls_KB_MReset_KEY).ToString();
            kbMod = (kbMod == "None" ? "" : $"{kbMod} +");

            string kbKey = ((System.Windows.Input.Key)Controls_KB_MReset_MOD).ToString();
            button1.Text = $"On/Off Key: {kbMod} {kbKey}";
        }

        private void button1_Click(object sender, EventArgs e) {
            // Get current modifers
            Models.MSelectKey_Storage storage = new Models.MSelectKey_Storage() {
                Cancel = false,
                inputKey = Controls_KB_MReset_KEY,
                inputMod = Controls_KB_MReset_MOD
            };

            Hooks.LowLevelKeyboardHook.LockEscape = false; {
                var frm = new SelectKey_Modifier(storage, true);
                frm.ShowDialog();
            }

            if (!storage.Cancel)
            {
                Controls_KB_MReset_KEY = storage.inputKey;
                Controls_KB_MReset_MOD = storage.inputMod;
            }

            resetButtonText();
        }

        private void button2_Click(object sender, EventArgs e) {
            cfg.Controls_KB_MReset_KEY = Controls_KB_MReset_KEY;
            cfg.Controls_KB_MReset_MOD = Controls_KB_MReset_MOD;

            // Save config
            Config.Data.Save(cfg.Name + ".ini", cfg);
                
            Close();
        }
    }
}
