using MaterialSkin;
using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace XboxKeyboardMouse.Forms
{
    public partial class Options : Controls.FormRWE {

        public Options() {
            InitializeComponent();

            this.MouseDown += FormMouseMove;

            materialTabSelector1.MouseMove += MaterialTabSelector1_MouseMove;
            btnExit.MouseMove              += MaterialTabSelector1_MouseMove;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            // Setup preset creation
            file_CreatePreset_Button.SetFontColor = true;
            file_CreatePreset_Button.SetFontDisabledColor = true;
            file_CreatePreset_Button.FontColorDisabled = preset_Color_Exists;
            file_CheckIfExists(null, null);

            PrepareInfoTab();
        }

        public override void SetStatusColor(Color c) {
            btnExit.BackColor = c;
        }

        #region Events
        // -----------

        private void MaterialTabSelector1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                FormMouseMove(this, e);
            }
        }

        private void ExitForm(object sender, EventArgs e) {
            this.Hide();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            // Automatically set the loaded active profile as the editing profile, for convenience,
            // especially to help ensure users discover the editing tabs, and so on.
            EditingProfile = Program.ActiveConfig.Name;

            ResetXboxInputButtons();
            ApplyInputEvents();

            materialTabControl1.Controls.Remove(tabSettings);
            materialTabControl1.Controls.Remove(tabKeyboard);
            materialTabControl1.Controls.Remove(tabMouse);
            materialTabControl1.Controls.Remove(tabPage3);
            materialTabSelector1.Invalidate();

            // Finish configuring the EditingProfile as the "loaded preset" for editing.
            RefreshConfigList(EditingProfile);
            file_LoadPreset_Click(null, null);
        }

        // -----------
        #endregion

        #region Xbox Input Editor
        /// <summary>Defines values that specify the buttons on a mouse device.</summary>
        public enum MouseButton {
            /// <summary>Nothing</summary>
            None = -1,

            /// <summary>The left mouse button.   
            Left = 0,
            
            /// <summary>The middle mouse button.</summary>
            Middle = 1,
            
            /// <summary>The right mouse button.</summary>
            Right = 2,

            /// <summary>The first extended mouse button.</summary>
            XButton1 = 3,

            /// <summary>The second extended mouse button.</summary>
            XButton2 = 4
        }

        private void ResetXboxInputButtons() {
            // Keyboard
            xbo_k_A.Text = "";
            xbo_k_B.Text = "";
            xbo_k_X.Text = "";
            xbo_k_Y.Text = "";

            xbo_k_LeftShoulder.Text = "";
            xbo_k_DpadLeft.Text = "";
            xbo_k_LeftStick.Text = "";

            xbo_k_RightShoulder.Text = "";
            xbo_k_DpadRight.Text = "";
            xbo_k_RightStick.Text = "";

            xbo_k_DpadUp.Text = "";
            xbo_k_DpadDown.Text = "";

            xbo_k_Start.Text = "";
            xbo_k_Back.Text = "";
            xbo_k_Guide.Text = "";

            xbo_k_TLeft.Text = "";
            xbo_k_TRight.Text = "";

            xbo_k_joy_l_up.Text = "";
            xbo_k_joy_l_down.Text = "";
            xbo_k_joy_l_left.Text = "";
            xbo_k_joy_l_right.Text = "";

            xbo_k_joy_r_up.Text = "";
            xbo_k_joy_r_down.Text = "";
            xbo_k_joy_r_left.Text = "";
            xbo_k_joy_r_right.Text = "";

            /*/ Mouse
            xbo_m_A.Text = "";
            xbo_m_B.Text = "";
            xbo_m_X.Text = "";
            xbo_m_Y.Text = "";

            xbo_m_Start.Text = "";
            xbo_m_Back.Text = "";
            xbo_m_Guide.Text = "";

            xbo_m_RS.Text = "";
            xbo_m_LS.Text = "";

            xbo_m_SR.Text = "";
            xbo_m_SL.Text = "";

            xbo_m_DpadUp.Text = "";
            xbo_m_DpadDown.Text = "";
            xbo_m_DpadLeft.Text = "";
            xbo_m_DpadRight.Text = "";

            xbo_m_TLeft.Text = "";
            xbo_m_TRight.Text = "";
            //*/
        }

        private void LoadXboxInputButtons() {
            // Keyboard
            xbo_k_A.Text = ((Key)cfg.Controls_KB_Xbox_A).ToString();
            xbo_k_B.Text = ((Key)cfg.Controls_KB_Xbox_B).ToString();
            xbo_k_X.Text = ((Key)cfg.Controls_KB_Xbox_X).ToString();
            xbo_k_Y.Text = ((Key)cfg.Controls_KB_Xbox_Y).ToString();

            xbo_k_LeftShoulder.Text = ((Key)cfg.Controls_KB_Xbox_LeftBumper).ToString();
            xbo_k_DpadLeft.Text = ((Key)cfg.Controls_KB_Xbox_DPAD_Left).ToString();
            xbo_k_LeftStick.Text = ((Key)cfg.Controls_KB_Xbox_Sticks_Left).ToString();

            xbo_k_RightShoulder.Text = ((Key)cfg.Controls_KB_Xbox_RightBumper).ToString();
            xbo_k_DpadRight.Text = ((Key)cfg.Controls_KB_Xbox_DPAD_Right).ToString();
            xbo_k_RightStick.Text = ((Key)cfg.Controls_KB_Xbox_Sticks_Right).ToString();

            xbo_k_DpadUp.Text = ((Key)cfg.Controls_KB_Xbox_DPAD_Up).ToString();
            xbo_k_DpadDown.Text = ((Key)cfg.Controls_KB_Xbox_DPAD_Down).ToString();

            xbo_k_Start.Text = ((Key)cfg.Controls_KB_Xbox_Start).ToString();
            xbo_k_Back.Text = ((Key)cfg.Controls_KB_Xbox_Back).ToString();
            xbo_k_Guide.Text = ((Key)cfg.Controls_KB_Xbox_Guide).ToString();

            xbo_k_TLeft.Text = ((Key)cfg.Controls_KB_Xbox_Trigger_Left).ToString();
            xbo_k_TRight.Text = ((Key)cfg.Controls_KB_Xbox_Trigger_Right).ToString();

            xbo_k_joy_l_up.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_L_Up).ToString(); ;
            xbo_k_joy_l_down.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_L_Down).ToString(); ;
            xbo_k_joy_l_left.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_L_Left).ToString(); ;
            xbo_k_joy_l_right.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_L_Right).ToString(); ;

            xbo_k_joy_r_up.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_R_Up).ToString(); ;
            xbo_k_joy_r_down.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_R_Down).ToString(); ;
            xbo_k_joy_r_left.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_R_Left).ToString(); ;
            xbo_k_joy_r_right.Text = ((Key)cfg.Controls_KB_Sticks_AXIS_R_Right).ToString(); ;

            /*/ Mouse
            xbo_m_A.Text = ((MouseButton)cfg.Controls_M_Xbox_A).ToString();
            xbo_m_B.Text = ((MouseButton)cfg.Controls_M_Xbox_B).ToString();
            xbo_m_X.Text = ((MouseButton)cfg.Controls_M_Xbox_X).ToString();
            xbo_m_Y.Text = ((MouseButton)cfg.Controls_M_Xbox_Y).ToString();

            xbo_m_Start.Text = ((MouseButton)cfg.Controls_M_Xbox_Start).ToString();
            xbo_m_Back.Text = ((MouseButton)cfg.Controls_M_Xbox_Back).ToString();
            xbo_m_Guide.Text = ((MouseButton)cfg.Controls_M_Xbox_Guide).ToString();

            xbo_m_RS.Text = ((MouseButton)cfg.Controls_M_Xbox_Sticks_Right).ToString();
            xbo_m_LS.Text = ((MouseButton)cfg.Controls_M_Xbox_Sticks_Left).ToString();

            xbo_m_SR.Text = ((MouseButton)cfg.Controls_M_Xbox_RightBumper).ToString();
            xbo_m_SL.Text = ((MouseButton)cfg.Controls_M_Xbox_LeftBumper).ToString();

            xbo_m_DpadUp.Text = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Up).ToString();
            xbo_m_DpadDown.Text = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Down).ToString();
            xbo_m_DpadLeft.Text = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Left).ToString();
            xbo_m_DpadRight.Text = ((MouseButton)cfg.Controls_M_Xbox_DPAD_Right).ToString();

            xbo_m_TLeft.Text = ((MouseButton)cfg.Controls_M_Xbox_Trigger_Left).ToString();
            xbo_m_TRight.Text = ((MouseButton)cfg.Controls_M_Xbox_Trigger_Right).ToString();
            //*/
        }

        public void ApplyInputEvents() {
            foreach (Control ctrl in editor_InputKeyboard.Controls) {
                if (ctrl is Label) {
                    ctrl.MouseHover += XBO_Input_OnEnter;
                    ctrl.MouseLeave += XBO_Input_OnLeave;
                    ctrl.MouseDoubleClick += XBO_Input_SelectKey;
                }
            }

            xbo_k_TLeft.MouseHover        += XBO_Input_OnEnter;
            xbo_k_TLeft.MouseLeave        += XBO_Input_OnLeaveTrig;
            xbo_k_TLeft.MouseDoubleClick  += XBO_Input_SelectKey;
            xbo_k_TRight.MouseHover       += XBO_Input_OnEnter;
            xbo_k_TRight.MouseLeave       += XBO_Input_OnLeaveTrig;
            xbo_k_TRight.MouseDoubleClick += XBO_Input_SelectKey;
        }

        #region Input Handlers
        private void XBO_Input_SelectKey(object sender, System.Windows.Forms.MouseEventArgs e) {
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
        #endregion

        #endregion

        #region Configuration
        // ------------------
        Config.Data cfg = new Config.Data();
        string originalName = "";
        private string editingProfile = "";

        private void RefreshConfigList(string selected = "") {
            lbPresets.Items.Clear();

            var files = Directory.GetFiles("profiles", "*.ini", SearchOption.TopDirectoryOnly);
            foreach (var filePath in files) {
                var presetFile = Path.GetFileNameWithoutExtension(filePath);
                lbPresets.Items.Add(new ListViewItem(presetFile) {
                    Selected = presetFile == selected,
                });
            }
        }

        private string SelectedListProfile
        {
            get
            {
                // Workaround: Sometimes the SelectedItems lags behind; query the Items for IsSelected directly instead.
                return (from item in lbPresets.Items.Cast<ListViewItem>() where item.Selected select item.Text).FirstOrDefault();
            }
        }

        private string SelectedListProfilePath
        {
            get { return Path.Combine("profiles", SelectedListProfile + ".ini"); }
        }

        private string EditingProfile
        {
            get { return editingProfile; }
            set
            {
                editingProfile = value;
                file_Editing.Text = "Editing: " + value;
            }
        }
        
        private string EditingProfilePath
        {
            get
            {
                return Path.Combine("profiles", EditingProfile + ".ini");
            }
        }

        #region Preset Creation
        Color preset_Color_Default = Color.FromArgb(0xFF, 0x21, 0x21, 0x21);
        Color preset_Color_Exists  = ((int)Primary.Red800).ToColor();
        Color preset_Color_Created = ((int)Primary.Green500).ToColor();

        // Check if a preset exists
        private bool presetNameExists() {
            return File.Exists(Path.Combine("profiles", file_CreatePreset_Text.Text + ".ini"));
        }

        // Refresh the listing
        private void file_RefreshList_Click(object sender, EventArgs e) {
            RefreshConfigList();
        }

        // Check if preset exists
        private void file_CheckIfExists(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (presetNameExists()) {
                file_CreatePreset_Button.Enabled = false;
            } else {
                file_CreatePreset_Button.Enabled = true;
                file_CreatePreset_Button.FontColor = preset_Color_Default;
            }
        }

        // Create the preset
        private void file_CreatePreset_Button_Click(object sender, EventArgs e) {
            var presetString = file_CreatePreset_Text.Text;
            var filePath = Path.Combine("profiles", presetString + ".ini");
            
            if (presetNameExists()) {
                file_CreatePreset_Button.Enabled = false;
                MessageBox.Show("Preset already exists (" + presetString + ".ini)");
                return;
            }

            // Create new config with default values
            Config.Data d = new Config.Data();
            d.Name = presetString;

            Config.Data.Save(presetString + ".ini", d);

            // Saved
            file_CreatePreset_Button.FontColor = preset_Color_Created;
            
            MessageBox.Show(string.Format("Created preset '{0}'", presetString));
            RefreshConfigList();
        }
        #endregion

        // ------------------
        #endregion

        #region Tabs
        // -------------------

        #region File
        // ---------

        bool TabsAdded = false;
        private void file_LoadPreset_Click(object sender, EventArgs e) {
            if (SelectedListProfile == null)
            {
                MessageBox.Show("You must select a profile first.");
                return;
            }

            if (!File.Exists(SelectedListProfilePath)) {
                MessageBox.Show(string.Format("The profile '{0}' no longer exists (at {1})", SelectedListProfile, SelectedListProfilePath));
                RefreshConfigList();
                return;
            }

            // Load the config for editing
            cfg = Config.Data.Load(SelectedListProfile + ".ini");
            EditingProfile = SelectedListProfile;

            // Load the controls onto the labels
            LoadXboxInputButtons();

            // Load the values for the options
            preset_Name.Text = cfg.Name;
            mouseInvertAxisX.Checked = cfg.Mouse_Invert_X;
            mouseInvertAxisY.Checked = cfg.Mouse_Invert_Y;

            // Check if the mouse setting is valid
            if (cfg.Mouse_Eng_Type == MouseTranslationMode.INVALID) {
                MessageBox.Show("Invalid mouse engine selected -> Reset to default!");
                cfg.Mouse_Eng_Type = MouseTranslationMode.DeadZoning;
                Config.Data.Save(EditingProfile + ".ini", cfg);
            }

            mouseEngineList.SelectedIndex = (int)cfg.Mouse_Eng_Type;

            // Ensure tickrate is not 0
            if (cfg.Mouse_TickRate == 0) {
                cfg.Mouse_TickRate = 40;
                Config.Data.Save(EditingProfile + ".ini", cfg);
            }

            mouse_TickRate.Text = "" + cfg.Mouse_TickRate;

            // Ensure that there is a detach key
            var k1 = (Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (Key)cfg.Controls_KB_Detach_KEY;

            if (k1 == Key.None && k2 == Key.None) {
                MessageBox.Show("You can't disable the detach key. Reset to default (LeftAlt + C)!");

                cfg.Controls_KB_Detach_MOD = (int)Key.LeftAlt;
                cfg.Controls_KB_Detach_KEY = (int)Key.C;

                Config.Data.Save(EditingProfile + ".ini", cfg);
            } else {
                detachKeyCheckup(true);
            }

            // Load detach key 
            string kbMod = ((Key)cfg.Controls_KB_Detach_MOD).ToString();
            kbMod = (kbMod == "None" ? "" : $"{kbMod} +");

            string kbKey = ((Key)cfg.Controls_KB_Detach_KEY).ToString();
            settings_DetachKey.Text = $"On/Off Key: {kbMod} {kbKey}";

            // Display our current file and active files
            file_Active.Text  = "Active Preset: " + Program.ActiveConfig.Name;
            
            editor_InputKeyboard.Enabled = true;

            originalName = cfg.Name;

            // Load our application settings
            settings_LockEscape.Checked = cfg.Application_LockEscape;
            settings_ShowCursor.Checked = cfg.Application_ShowCursor;

            // Load our mouse engine
            LoadMouseEngineSettings();

            // Setup our sticks
            comboBox1.SelectedIndex = (cfg.Mouse_Is_RightStick ? 1 : 0);

            // Enable the tabs
            tabSettings.Enabled = true;
            tabKeyboard.Enabled = true;
            tabMouse.Enabled = true;

            // Add the tabs if not already added
            if (!TabsAdded) {
                TabsAdded = true;

                // Add the tabs as controls
                materialTabControl1.Controls.Add(tabSettings);
                materialTabControl1.Controls.Add(tabKeyboard);
                materialTabControl1.Controls.Add(tabMouse);
                materialTabControl1.Controls.Add(tabPage3);
            }
        }

        private void file_SetAsActive_Click(object sender, EventArgs e) {
            if (SelectedListProfile == null)
            {
                MessageBox.Show("You must select a profile first.");
                return;
            }

            if (!File.Exists(SelectedListProfilePath)) {
                MessageBox.Show(string.Format("The profile '{0}' no longer exists (at {1})", SelectedListProfile, SelectedListProfilePath));
                RefreshConfigList();
                return;
            }

            SetActive(SelectedListProfile + ".ini");
        }

        private void SetActive(string profileName)
        {
            Program.SetActiveConfig(profileName);
            file_Active.Text = "Active Preset: " + Program.ActiveConfig.Name;
        }

        private void file_DeletePreset_Click(object sender, EventArgs e)
        {
            if (SelectedListProfile == null)
            {
                MessageBox.Show("You must select a profile first.");
                return;
            }

            if (!File.Exists(SelectedListProfilePath))
            {
                MessageBox.Show(string.Format("The profile '{0}' no longer exists (at {1})", SelectedListProfile, SelectedListProfilePath));
                RefreshConfigList();
                return;
            }

            var message = string.Format("Are you sure you want to delete the profile '{0}'?", SelectedListProfile, SelectedListProfilePath);
            var result = MessageBox.Show(message, "Are you sure?", MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
                return;

            File.Delete(SelectedListProfilePath);

            if (SelectedListProfile == "default") {
                // Remake the default ini
                Config.Data d = new Config.Data();
                Config.Data.Save("default.ini", d);
            }

            if (Program.ActiveConfig.Name == SelectedListProfile) {
                SetActive("default.ini");
            }

            RefreshConfigList();
        }

        // ---------
        #endregion

        #region Settings
        // -------------

        private void settings_LockEscape_CheckedChanged(object sender, EventArgs e) {
            cfg.Application_LockEscape = settings_LockEscape.Checked;
        }

        private void settings_ShowCursor_CheckedChanged(object sender, EventArgs e) {
            //Program.HideCursor = !settings_ShowCursor.Checked;
            cfg.Application_ShowCursor = settings_ShowCursor.Checked;
        }

        private void settings_DetachKey_Click(object sender, EventArgs e) {
            // Get current modifers
            Models.MSelectKey_Storage storage = new Models.MSelectKey_Storage() {
                Cancel = false,
                inputKey = cfg.Controls_KB_Detach_KEY,
                inputMod = cfg.Controls_KB_Detach_MOD
            };

            var bEscape = Hooks.LowLevelKeyboardHook.LockEscape;
            Hooks.LowLevelKeyboardHook.LockEscape = false;
            {
                var frm = new SelectKey_Modifier(storage, true);
                frm.ShowDialog();
            }
            Hooks.LowLevelKeyboardHook.LockEscape = bEscape;

            if (!storage.Cancel)
            {
                cfg.Controls_KB_Detach_KEY = storage.inputKey;
                cfg.Controls_KB_Detach_MOD = storage.inputMod;
            }

            var k1 = (Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (Key)cfg.Controls_KB_Detach_KEY;
            var no = Key.None;

            if (k1 == no && k2 == no) {
                MessageBox.Show("You cant disable the detach key, reset to default (LeftAlt + C)!");

                cfg.Controls_KB_Detach_MOD = (int)Key.LeftAlt;
                cfg.Controls_KB_Detach_KEY = (int)Key.C;
            }

            detachKeyCheckup();

            string kbMod = ((Key)cfg.Controls_KB_Detach_MOD).ToString();
            kbMod = (kbMod == "None" ? "" : $"{kbMod} +");

            string kbKey = ((Key)cfg.Controls_KB_Detach_KEY).ToString();
            settings_DetachKey.Text = $"On/Off Key: {kbMod} {kbKey}";
        }

        private void settings_SaveAllChanges_Click(object sender, EventArgs e) {
            // The dreaded save button ;(

            // Ignore name for now, that will
            // require extra work

            // Mouse settings
            cfg.Mouse_Invert_X = mouseInvertAxisX.Checked;
            cfg.Mouse_Invert_Y = mouseInvertAxisY.Checked;

            // TODO: Mouse Engine
            //cfg.Mouse_Sensitivity_X = (double)mouseXSense.Value;
            //cfg.Mouse_Sensitivity_Y = (double)mouseYSense.Value;
            //cfg.Mouse_Eng_Type = (int)mouseMouseEngine.SelectedIndex;
            //cfg.Mouse_FinalMod = (double)mouseMouseModifier.Value;

            if (cfg.Name.Trim() != preset_Name.Text.Trim()) {
                cfg.Name = preset_Name.Text.Trim();
                File.Delete(EditingProfilePath);
            }

            // Saved
            Config.Data.Save(cfg.Name + ".ini", cfg);

            MessageBox.Show(string.Format("The profile '{0}' has been saved.", cfg.Name));

            if (Program.ActiveConfig.Name == originalName) {
                Program.ActiveConfig = cfg;
                Program.ReloadActiveConfig();
            }
            
            // Refresh list
            RefreshConfigList();
        }

        private void settings_ChangePresetName_Click(object sender, EventArgs e) {
            var name = cfg.Name.Trim();

            if (cfg.Name.Trim() != preset_Name.Text.Trim()) {
                cfg.Name = preset_Name.Text.Trim();
                File.Delete(EditingProfilePath);
            }

            // Saved
            Config.Data.Save(cfg.Name + ".ini", cfg);

            // Check if cfg is default
            if (name == "default") {
                // Remake the default ini
                Config.Data d = new Config.Data();
                Config.Data.Save("profiles/default.ini", d);
            }

            // Refresh listings
            RefreshConfigList();
        }
        
        // -------------
        #endregion

        #region Mouse
        // ----------

        private void mouseEngineList_SelectedIndexChanged(object sender, EventArgs e) {
            var index = mouseEngineList.SelectedIndex;
            cfg.Mouse_Eng_Type = (MouseTranslationMode)index;

            // Load our mouse engine settings
            LoadMouseEngineSettings();
        }

        private void mouseInvertAxisX_CheckedChanged(object sender, EventArgs e) {
            cfg.Mouse_Invert_X = mouseInvertAxisX.Checked;
        }

        private void mouseInvertAxisY_CheckedChanged(object sender, EventArgs e) {
            cfg.Mouse_Invert_Y = mouseInvertAxisY.Checked;
        }

        private void mouse_TickRate_TextChanged(object sender, EventArgs e) {
            string strTick = mouse_TickRate.Text;

            int tickRate = 0;
            if (! int.TryParse(strTick, out tickRate)) {
                mouse_TickInvalid.Visible = true;
                return;
            }

            // Min is 1 MS!
            if (tickRate <= 0) 
                tickRate = 1;
            

            mouse_TickInvalid.Visible = false;
            cfg.Mouse_TickRate = tickRate;
        }

        private void mouse_TickRate_Reset_Click(object sender, EventArgs e) {
            cfg.Mouse_TickRate = 16;
            mouse_TickRate.Text = "" + 16;
        }

        private void mouseSelectStick_SelectedIndexChanged(object sender, EventArgs e) {
            cfg.Mouse_Is_RightStick = comboBox1.SelectedIndex == 1;
        }

        MouseSettings.MouseEngineSettings mouseEnginePanel;
        private void LoadMouseEngineSettings() {
            if (mouseEnginePanel != null)  
                if (mouseEngineContainer.Controls.Contains(mouseEnginePanel))
                    mouseEngineContainer.Controls.Remove  (mouseEnginePanel);
            
            // By default we want to set engine as GenericControls
            if (cfg.Mouse_Eng_Type != MouseTranslationMode.NONE && cfg.Mouse_Eng_Type != MouseTranslationMode.INVALID)
                mouseEnginePanel = new MouseSettings.GenericControls(cfg);
            else
                mouseEnginePanel = new MouseSettings.NoControls();
            
            mouseEngineContainer.Controls.Add(mouseEnginePanel);
            mouseEnginePanel.Dock = DockStyle.Fill;
        }

        // ----------
        #endregion

        #region Keyboard
        // -------------

        private void detachKeyCheckup(bool save = false) {
            var k1 = (Key)cfg.Controls_KB_Detach_MOD;
            var k2 = (Key)cfg.Controls_KB_Detach_KEY;

            if (k2 == Key.None && k1 != Key.None) {
                k2 = k1;
                k1 = Key.None;
            }

            cfg.Controls_KB_Detach_MOD = (int)k1;
            cfg.Controls_KB_Detach_KEY = (int)k2;

            if (save)
                Config.Data.Save(EditingProfile + ".ini", cfg);
        }

        // -------------
        #endregion

        private void PrepareInfoTab()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            StringBuilder fullInfo = new StringBuilder();

            fullInfo.AppendLine("XboxKeyboardMouse");
            fullInfo.Append("Version: ");
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                fullInfo.Append("ClickOnce-v");
                fullInfo.AppendLine(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString());
            }
            else
            {
                fullInfo.Append("Loose-v");
                fullInfo.AppendLine(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
            fullInfo.AppendLine();
            fullInfo.AppendLine();

            fullInfo.Append(resources.GetString("infoText"));

            infoTextBox.Text = fullInfo.ToString();
        }

        // -------------------
        #endregion
    }
}
