using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace XboxKeyboardMouse.Forms.Old {
    public partial class Old_Options : Form {
        //
        // Summary:
        //     Defines values that specify the buttons on a mouse device.
        public enum MouseButton {
            //
            // Summary:
            //     Nothing
            None = -1,

            //
            // Summary:
            //     The left mouse button.
            Left = 0,
            //
            // Summary:
            //     The middle mouse button.
            Middle = 1,
            //
            // Summary:
            //     The right mouse button.
            Right = 2,
            //
            // Summary:
            //     The first extended mouse button.
            XButton1 = 3,
            //
            // Summary:
            //     The second extended mouse button.
            XButton2 = 4
        }
        
        public Old_Options() {
            InitializeComponent();
        }

        Config.Data cfg = new Config.Data();
        string originalName = "";

        private void Options_Load(object sender, EventArgs e) {
            ResetXboxInputButtons();
            RefreshConfigList();
            ApplyInputStyles();
        }

        string SelectedProfile = "";

        public void ApplyInputStyles() {   
            foreach (Control ctrl in editor_InputKeyboard.Controls) {
                if(ctrl is Label) {
                    ctrl.MouseHover += XBO_Input_OnEnter;
                    ctrl.MouseLeave += XBO_Input_OnLeave;
                    ctrl.MouseDoubleClick += XBO_Input_SelectKey;
                }
            }

            xbo_k_TLeft.MouseHover += XBO_Input_OnEnter;
            xbo_k_TLeft.MouseLeave += XBO_Input_OnLeaveTrig;
            xbo_k_TLeft.MouseDoubleClick += XBO_Input_SelectKey;
            xbo_k_TRight.MouseHover += XBO_Input_OnEnter;
            xbo_k_TRight.MouseLeave += XBO_Input_OnLeaveTrig;
            xbo_k_TRight.MouseDoubleClick += XBO_Input_SelectKey;

            foreach (Control ctrl in editor_InputMouse.Controls) {
                if (ctrl is Label) {
                    ctrl.MouseHover += XBO_Input_OnEnter;
                    ctrl.MouseLeave += XBO_Input_OnLeave;
                    //ctrl.MouseDoubleClick += XBO_Input_SelectKey;
                }
            }
        }

        private void XBO_Input_SelectKey(object sender, MouseEventArgs e) {
            var lbl = (Label)sender;

            var bEscape = Hooks.LowLevelKeyboardHook.LockEscape;
            Hooks.LowLevelKeyboardHook.LockEscape = false; {
                SelectKey k = new SelectKey(cfg, (string)lbl.Tag);
                k.ShowDialog();
            } Hooks.LowLevelKeyboardHook.LockEscape = bEscape;

            LoadXboxInputButtons();
        }

        private void XBO_Input_OnLeave(object sender, EventArgs e) {
            var lbl = (Label)sender;
            var lblTag = (string)lbl.Tag;
            if (lblTag.StartsWith("JR") || lblTag.StartsWith("JL"))
                 lbl.ForeColor = Color.Black;
            else lbl.ForeColor = Color.White;
        }

        private void XBO_Input_OnLeaveTrig(object sender, EventArgs e) {
            var lbl = (Label)sender;
            lbl.ForeColor = Color.Black;
        }

        private void XBO_Input_OnEnter(object sender, EventArgs e) {
            var lbl = (Label)sender;
            lbl.ForeColor = Color.FromArgb(93, 194, 30);
        }

        public string GetSelectedListProfile() {
            return (string)lbPresets.SelectedItem;
        }

        public string GetSelectedListProfilePath() {
            return "profiles/" + GetSelectedListProfile() + ".ini";
        }

        public string GetSelectedProfile() {
            return SelectedProfile;
        }
        
        public string GetSelectedProfilePath() {
            return "profiles/" + GetSelectedProfile() + ".ini";
        }

        private void RefreshConfigList() {
            lbPresets.Items.Clear();

            var d = Directory.GetFiles("profiles", "*.ini", SearchOption.TopDirectoryOnly);
            foreach(var f in d) 
                lbPresets.Items.Add(f.Split('\\')[1].Split('.')[0]);
        }

        private void btnPresOptRef_Click(object sender, EventArgs e) {
            RefreshConfigList();
        }

        private void ResetXboxInputButtons() {
            // Keyboard
            xbo_k_A.Text              = "";
            xbo_k_B.Text              = "";
            xbo_k_X.Text              = "";
            xbo_k_Y.Text              = "";

            xbo_k_LeftSholder.Text    = "";
            xbo_k_DpadLeft.Text       = "";
            xbo_k_LeftStick.Text      = "";

            xbo_k_RightSholder.Text   = "";
            xbo_k_DpadRight.Text      = "";
            xbo_k_RightStick.Text     = "";

            xbo_k_DpadUp.Text         = "";
            xbo_k_DpadDown.Text       = "";

            xbo_k_Start.Text          = "";
            xbo_k_Back.Text           = "";
            xbo_k_Guide.Text          = "";

            xbo_k_TLeft.Text          = "";
            xbo_k_TRight.Text         = "";

            xbo_k_joy_l_up.Text       = "";
            xbo_k_joy_l_down.Text     = "";
            xbo_k_joy_l_left.Text     = "";
            xbo_k_joy_l_right.Text    = "";

            xbo_k_joy_r_up.Text       = "";
            xbo_k_joy_r_down.Text     = "";
            xbo_k_joy_r_left.Text     = "";
            xbo_k_joy_r_right.Text    = "";

            // Mouse
            xbo_m_A.Text              = "";
            xbo_m_B.Text              = "";
            xbo_m_X.Text              = "";
            xbo_m_Y.Text              = "";

            xbo_m_Start.Text          = "";
            xbo_m_Back.Text           = "";
            xbo_m_Guide.Text          = "";

            xbo_m_RS.Text             = "";
            xbo_m_LS.Text             = "";

            xbo_m_SR.Text             = "";
            xbo_m_SL.Text             = "";

            xbo_m_DpadUp.Text         = "";
            xbo_m_DpadDown.Text       = "";
            xbo_m_DpadLeft.Text       = "";
            xbo_m_DpadRight.Text      = "";

            xbo_m_TLeft.Text          = "";
            xbo_m_TRight.Text         = "";
        }

        private void LoadXboxInputButtons() {
            // Keyboard
            xbo_k_A.Text              = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_A).ToString();
            xbo_k_B.Text              = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_B).ToString();
            xbo_k_X.Text              = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_X).ToString();
            xbo_k_Y.Text              = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Y).ToString();

            xbo_k_LeftSholder.Text    = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_LeftBumper).ToString();
            xbo_k_DpadLeft.Text       = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_DPAD_Left).ToString();
            xbo_k_LeftStick.Text      = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Sticks_Left).ToString();

            xbo_k_RightSholder.Text   = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_RightBumper).ToString();
            xbo_k_DpadRight.Text      = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_DPAD_Right).ToString();
            xbo_k_RightStick.Text     = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Sticks_Right).ToString();

            xbo_k_DpadUp.Text         = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_DPAD_Up).ToString();
            xbo_k_DpadDown.Text       = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_DPAD_Down).ToString();

            xbo_k_Start.Text          = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Start).ToString();
            xbo_k_Back.Text           = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Back).ToString();
            xbo_k_Guide.Text          = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Guide).ToString();

            xbo_k_TLeft.Text          = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Trigger_Left).ToString();
            xbo_k_TRight.Text         = ((System.Windows.Input.Key)cfg.Controls_KB_Xbox_Trigger_Right).ToString();

            xbo_k_joy_l_up.Text       = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_L_Up).ToString(); ;
            xbo_k_joy_l_down.Text     = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_L_Down).ToString(); ;
            xbo_k_joy_l_left.Text     = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_L_Left).ToString(); ;
            xbo_k_joy_l_right.Text    = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_L_Right).ToString(); ;

            xbo_k_joy_r_up.Text       = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_R_Up).ToString(); ;
            xbo_k_joy_r_down.Text     = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_R_Down).ToString(); ;
            xbo_k_joy_r_left.Text     = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_R_Left).ToString(); ;
            xbo_k_joy_r_right.Text    = ((System.Windows.Input.Key)cfg.Controls_KB_Sticks_AXIS_R_Right).ToString();;

            // Mouse
            xbo_m_A.Text              = ((MouseButton)cfg.Controls_M_Xbox_A).ToString();
            xbo_m_B.Text              = ((MouseButton)cfg.Controls_M_Xbox_B).ToString();
            xbo_m_X.Text              = ((MouseButton)cfg.Controls_M_Xbox_X).ToString();
            xbo_m_Y.Text              = ((MouseButton)cfg.Controls_M_Xbox_Y).ToString();

            xbo_m_Start.Text          = ((MouseButton)cfg.Controls_M_Xbox_Start).ToString();
            xbo_m_Back.Text           = ((MouseButton)cfg.Controls_M_Xbox_Back).ToString();
            xbo_m_Guide.Text          = ((MouseButton)cfg.Controls_M_Xbox_Guide).ToString();

            xbo_m_RS.Text             = ((MouseButton)cfg.Controls_M_Xbox_Sticks_Right).ToString();
            xbo_m_LS.Text             = ((MouseButton)cfg.Controls_M_Xbox_Sticks_Left).ToString();

            xbo_m_SR.Text             = ((MouseButton)cfg.Controls_M_Xbox_RightBumper).ToString();
            xbo_m_SL.Text             = ((MouseButton)cfg.Controls_M_Xbox_LeftBumper).ToString();

            xbo_m_DpadUp.Text         = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Up).ToString();
            xbo_m_DpadDown.Text       = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Down).ToString();
            xbo_m_DpadLeft.Text       = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Left).ToString();
            xbo_m_DpadRight.Text      = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Right).ToString();

            xbo_m_TLeft.Text          = ((MouseButton)cfg.Controls_M_Xbox_Trigger_Left).ToString();
            xbo_m_TRight.Text         = ((MouseButton)cfg.Controls_M_Xbox_Trigger_Right).ToString();
        }

        private void btnPresOptDel_Click(object sender, EventArgs e) {
            var res = MessageBox.Show(
                "Are you sure you want to delete: " + GetSelectedListProfile() + "\nFile: " + GetSelectedListProfilePath(),
                "Are you sure?",
                MessageBoxButtons.OKCancel
            );

            if (res != DialogResult.OK)
                return;
            if (!System.IO.File.Exists(GetSelectedListProfilePath())) {
                MessageBox.Show("The selected profile: " + GetSelectedListProfile() +
                    " no longer exists...\nFile: " + GetSelectedListProfilePath());
                return;
            }

            if (GetSelectedListProfile().Trim() == "default.ini") {
                // Remake the default ini
                Config.Data d = new Config.Data();
                Config.Data.Save("profiles/default.ini", d);
            }

            if (Program.ActiveConfig.Name.Trim() == GetSelectedListProfile().Trim()) {
                Program.SetActiveConfig("default.ini");
            }

            System.IO.File.Delete(GetSelectedListProfilePath());
            RefreshConfigList();
        }

        private void btnPreOptLoad_Click(object sender, EventArgs e) {
            if (!System.IO.File.Exists(GetSelectedListProfilePath())) {
                MessageBox.Show("The selected profile: " + GetSelectedProfile() +
                    " no longer exists...\nFile: " + GetSelectedProfilePath());
                RefreshConfigList();

                return;
            }

            // Load the config
            cfg = Config.Data.Load(GetSelectedListProfile() + ".ini");

            SelectedProfile = GetSelectedListProfile();

            // Load the controls onto the labels
            LoadXboxInputButtons();

            // Load the values for the options
            optName.Text = cfg.Name;
            mouseXSense.Value = (decimal)cfg.Mouse_Sensitivity_X;
            mouseYSense.Value = (decimal)cfg.Mouse_Sensitivity_Y;
            mouseInvertX.Checked = cfg.Mouse_Invert_X;
            mouseInvertY.Checked = cfg.Mouse_Invert_Y;
            mouseMouseEngine.SelectedIndex = cfg.Mouse_Eng_Type;
            mouseMouseModifier.Value = (decimal)cfg.Mouse_FinalMod;

            // Ensure tickrate is not 0
            if (cfg.Mouse_TickRate == 0) {
                cfg.Mouse_TickRate = 40;
                Config.Data.Save(GetSelectedProfile() + ".ini", cfg);
            }

            mouseTickRate.Value = cfg.Mouse_TickRate;

            // Ensure that there is a detach key
            var k1 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_KEY;
            var no = System.Windows.Input.Key.None;

            if (k1 == no && k2 == no) {
                MessageBox.Show("You cant disable the detach key, reset to default (LeftAlt + C)!");

                cfg.Controls_KB_Detach_MOD = (int)System.Windows.Input.Key.LeftAlt;
                cfg.Controls_KB_Detach_KEY = (int)System.Windows.Input.Key.C;

                Config.Data.Save(GetSelectedProfile() + ".ini", cfg);
            } else {
                detachKeyCheckup(true);
            }

            // Load detach key 
            string kbMod = ((System.Windows.Input.Key)cfg.Controls_KB_Detach_MOD).ToString();
            kbMod = (kbMod == "None" ? "" : $"{kbMod} +");

            string kbKey = ((System.Windows.Input.Key)cfg.Controls_KB_Detach_KEY).ToString();
            optDetachKey.Text = $"On/Off Key: {kbMod} {kbKey}";

            // Enable the editors
            editor_Preset.Text = "Preset - " + GetSelectedListProfile();
            editor_Preset.Enabled = true;
            editor_InputKeyboard.Enabled = true;
            editor_InputMouse.Enabled = true;

            originalName = cfg.Name;

            // Check if the mouse setting is valid
            if (cfg.Mouse_Eng_Type > TranslateMouse.MaxMouseMode || cfg.Mouse_Eng_Type < 0) {
                MessageBox.Show("Invalid mouse engine selected -> Reset to default!");
                cfg.Mouse_Eng_Type = 0;
                Config.Data.Save(GetSelectedProfile() + ".ini", cfg);
            }

            mouseMouseEngine.SelectedIndex = cfg.Mouse_Eng_Type;
        }

        private void btnPresOptActive_Click(object sender, EventArgs e) {
            if (!System.IO.File.Exists(GetSelectedListProfilePath())) {
                MessageBox.Show("The selected profile: " + GetSelectedProfile() +
                    " no longer exists...\nFile: " + GetSelectedProfilePath());
                RefreshConfigList();

                return;
            }

            var prof = GetSelectedProfile();

            Program.SetActiveConfig(prof + ".ini");
        }

        private void ResetNameOptionToDefault(object sender, LinkLabelLinkClickedEventArgs e) {
            optName.Text = cfg.Name;
        }

        private bool presetNameExists() {
            return System.IO.File.Exists("profiles/" + tbCreateName.Text + ".ini");
        }

        private void btnCreatePreset_Click(object sender, EventArgs e) {
            var filePath = "profiles/" + tbCreateName.Text + ".ini";

            if (presetNameExists()) {
                lblCreateStatus.ForeColor = Color.Red;
                lblCreateStatus.Text = "Preset already exists...";
                return;
            }

            // Create new config with default values
            Config.Data d = new Config.Data();
            d.Name = tbCreateName.Text;

            Config.Data.Save(tbCreateName.Text + ".ini", d);

            // Saved
            lblCreateStatus.ForeColor = Color.Orange;
            lblCreateStatus.Text = "Saved...";

            MessageBox.Show("Created new config in file (" + tbCreateName.Text + ".ini)");
            RefreshConfigList();
        }

        private void tbCreateName_TextChanged(object sender, EventArgs e) {
            if (presetNameExists()) {
                lblCreateStatus.ForeColor = Color.Red;
                lblCreateStatus.Text = "Preset already exists...";
            } else {
                lblCreateStatus.ForeColor = Color.Green;
                lblCreateStatus.Text = "Does not exist.";
            }
        }

        private void SaveToFile(object sender, EventArgs e) {
            // The dreaded save button ;(

            // Ignore name for now, that will
            // require extra work

            // Mouse settings
            cfg.Mouse_Invert_X      = mouseInvertX.Checked;
            cfg.Mouse_Invert_Y      = mouseInvertX.Checked;
            cfg.Mouse_Sensitivity_X = (double)mouseXSense.Value;
            cfg.Mouse_Sensitivity_Y = (double)mouseYSense.Value;
            cfg.Mouse_TickRate      = (int)mouseTickRate.Value;
            cfg.Mouse_Eng_Type      = (int)mouseMouseEngine.SelectedIndex;
            cfg.Mouse_FinalMod      = (double)mouseMouseModifier.Value;

            if (cfg.Name.Trim() != optName.Text.Trim()) {
                cfg.Name = optName.Text.Trim();
                File.Delete("profiles/" + GetSelectedProfile() + ".ini");
            }

            // Saved
            Config.Data.Save(cfg.Name + ".ini", cfg);

            MessageBox.Show(
                "The selected profile: " + cfg.Name +
                " has been saved...\nFile: " + $"profiles/{cfg.Name}.ini");

            if (Program.ActiveConfig.Name == originalName)
                Program.ActiveConfig = cfg;

            // Refresh list
            RefreshConfigList();
        }

        private void detachKeyCheckup(bool save = false) {
            var k1 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_KEY;

            if (k2 == System.Windows.Input.Key.None & k1 != System.Windows.Input.Key.None) {
                k2 = k1;
                k1 = System.Windows.Input.Key.None;
            }

            cfg.Controls_KB_Detach_MOD = (int)k1;
            cfg.Controls_KB_Detach_KEY = (int)k2;

            if (save)
                Config.Data.Save(GetSelectedProfile() + ".ini", cfg);
        }

        private void optDetachKey_Click(object sender, EventArgs e) {
            // Get current modifers
            Models.MSelectKey_Storage storage = new Models.MSelectKey_Storage() {
                Cancel = false,
                inputKey = cfg.Controls_KB_Detach_KEY,
                inputMod = cfg.Controls_KB_Detach_MOD
            };

            var bEscape = Hooks.LowLevelKeyboardHook.LockEscape;
            Hooks.LowLevelKeyboardHook.LockEscape = false; {
                var frm = new SelectKey_Modifier(storage, true);
                frm.ShowDialog();
            } Hooks.LowLevelKeyboardHook.LockEscape = bEscape;

            if (storage.Cancel) goto CheckReturn;

            cfg.Controls_KB_Detach_KEY = storage.inputKey;
            cfg.Controls_KB_Detach_MOD = storage.inputMod;

        CheckReturn:
            var k1 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (System.Windows.Input.Key)cfg.Controls_KB_Detach_KEY;
            var no = System.Windows.Input.Key.None;

            if (k1 == no && k2 == no) {
                MessageBox.Show("You cant disable the detach key, reset to default (LeftAlt + C)!");

                cfg.Controls_KB_Detach_MOD = (int)System.Windows.Input.Key.LeftAlt;
                cfg.Controls_KB_Detach_KEY = (int)System.Windows.Input.Key.C;
            }

            detachKeyCheckup();

            string kbMod = ((System.Windows.Input.Key)cfg.Controls_KB_Detach_MOD).ToString();
            kbMod = (kbMod == "None" ? "" : $"{kbMod} +");

            string kbKey = ((System.Windows.Input.Key)cfg.Controls_KB_Detach_KEY).ToString();
            optDetachKey.Text = $"On/Off Key: {kbMod} {kbKey}";
        }

        OptionsFrms.Mouse_Engine_Relative meRelative = null;
        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show("There is no options for this engine.");

            /*
            if (optMouseEngine.SelectedIndex == 0) {
                MessageBox.Show("There is no options for this engine.");
            } else if (optMouseEngine.SelectedIndex == 1) {
                if (meRelative == null || meRelative.IsDisposed)
                    meRelative = new OptionsFrms.Mouse_Engine_Relative(cfg);
                
                meRelative.ShowDialog();
            }*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            int index = mouseMouseEngine.SelectedIndex;

            if (index > TranslateMouse.MaxMouseMode) {
                MessageBox.Show("Invalid selected mouse engine");
                index = 0;
            }

            cfg.Mouse_Eng_Type = index;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            Hooks.LowLevelKeyboardHook.LockEscape = settings_LockEscape.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            Program.HideCursor = settings_HideCursor.Checked;
        }

        private void optTickRate_ValueChanged(object sender, EventArgs e) {

        }

        private void optInvertY_CheckedChanged(object sender, EventArgs e) {

        }
    }
}
