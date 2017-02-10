using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using XboxKeyboardMouse;
using XboxKeyboardMouse.Config;

namespace XboxKeyboardMouse {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static MainForm mainform = new MainForm();

        /** Public Variables Across the Whole Application */
        public static Config.Data ActiveConfig;
        public static bool DoneLoadingCfg = false;
        public static bool HideCursor     = true;
        public static string ActiveConfigFile = "";
        public static IntPtr ptrKeyboardHook;


        public static bool SetActiveConfig(string File) {
            var cfg = "profiles/" + File;

            if (!System.IO.File.Exists(cfg)) 
                 return false;

            lock (ActiveConfig) {
                Data d = Data.Load(File);
                ActiveConfig = d;
            }

            // Make sure the ini file exists
            ReadConfiguration(File);
            
            IniFile appcfg = new IniFile("config.ini");
            appcfg.AddSetting("Xbox", "KeyProfile", File);
            appcfg.SaveSettings();

            ReloadControlScheme();

            return true;
        }

        public static void ReloadControlScheme() {
            TranslateKeyboard.ClearAllDicts();

            lock (TranslateKeyboard.buttons) {
            lock (TranslateKeyboard.mapLeftStickY)  { lock (TranslateKeyboard.mapLeftStickX) { 
            lock (TranslateKeyboard.mapRightStickX) { lock (TranslateKeyboard.mapRightStickY) { 
                if (ActiveConfig.Controls_KB_Xbox_A != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_A, ScpDriverInterface.X360Buttons.A);
                if (ActiveConfig.Controls_KB_Xbox_B != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_B, ScpDriverInterface.X360Buttons.B);
                if (ActiveConfig.Controls_KB_Xbox_X != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_X, ScpDriverInterface.X360Buttons.X);
                if (ActiveConfig.Controls_KB_Xbox_Y != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Y, ScpDriverInterface.X360Buttons.Y);

                if (ActiveConfig.Controls_KB_Xbox_DPAD_Up != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Up, ScpDriverInterface.X360Buttons.Up);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Down != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Down, ScpDriverInterface.X360Buttons.Down);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Left != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Left, ScpDriverInterface.X360Buttons.Left);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Right != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Right, ScpDriverInterface.X360Buttons.Right);

                if (ActiveConfig.Controls_KB_Xbox_Guide != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Guide, ScpDriverInterface.X360Buttons.Guide);
                if (ActiveConfig.Controls_KB_Xbox_Start != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Start, ScpDriverInterface.X360Buttons.Start);
                if (ActiveConfig.Controls_KB_Xbox_Back != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Back, ScpDriverInterface.X360Buttons.Back);

                if (ActiveConfig.Controls_KB_Xbox_LeftBumper != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_LeftBumper, ScpDriverInterface.X360Buttons.LeftBumper);
                if (ActiveConfig.Controls_KB_Xbox_RightBumper != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_RightBumper, ScpDriverInterface.X360Buttons.RightBumper);

                if (ActiveConfig.Controls_KB_Xbox_Sticks_Left != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Sticks_Left, ScpDriverInterface.X360Buttons.LeftStick);
                if (ActiveConfig.Controls_KB_Xbox_Sticks_Right != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Sticks_Right, ScpDriverInterface.X360Buttons.RightStick);

                if (ActiveConfig.Controls_KB_Xbox_Trigger_Left != 0)
                    TranslateKeyboard.triggers.Add((Key)ActiveConfig.Controls_KB_Xbox_Trigger_Left, TranslateKeyboard.TriggerType.LeftTrigger);
                if (ActiveConfig.Controls_KB_Xbox_Trigger_Right != 0)
                    TranslateKeyboard.triggers.Add((Key)ActiveConfig.Controls_KB_Xbox_Trigger_Right, TranslateKeyboard.TriggerType.RightTrigger);


                if (ActiveConfig.Controls_KB_Sticks_AXIS_L_Up != 0)
                    TranslateKeyboard.mapLeftStickY.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_L_Up, short.MaxValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_L_Down != 0)
                    TranslateKeyboard.mapLeftStickY.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_L_Down, short.MinValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_L_Left != 0)
                    TranslateKeyboard.mapLeftStickX.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_L_Left, short.MinValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_L_Right != 0)
                    TranslateKeyboard.mapLeftStickX.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_L_Right, short.MaxValue);

                if (ActiveConfig.Controls_KB_Sticks_AXIS_R_Up != 0)
                    TranslateKeyboard.mapRightStickY.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_R_Up, short.MaxValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_R_Down != 0)
                    TranslateKeyboard.mapRightStickY.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_R_Down, short.MinValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_R_Left != 0)
                    TranslateKeyboard.mapRightStickX.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_R_Left, short.MinValue);
                if (ActiveConfig.Controls_KB_Sticks_AXIS_R_Right != 0)
                    TranslateKeyboard.mapRightStickX.Add((Key)ActiveConfig.Controls_KB_Sticks_AXIS_R_Right, short.MaxValue);
            } } } } }
        }

        public static void ReadConfiguration(string defaultProfile = "default.ini") {
            // Setup the default application cfg file if does not exist
            IniFile appcfg = null;
            
            if (!System.IO.File.Exists("config.ini")) {
                System.IO.File.Create("config.ini").Close();

                appcfg = new IniFile("config.ini");
                appcfg.AddSetting("Xbox", "KeyProfile", defaultProfile);
            }

            if (appcfg == null)
                appcfg = new IniFile("config.ini");

            if (string.IsNullOrWhiteSpace(appcfg.GetSetting("Xbox", "KeyProfile")))
                appcfg.AddSetting("Xbox", "KeyProfile", defaultProfile);

            appcfg.SaveSettings();

            ActiveConfigFile = appcfg.GetSetting("Xbox", "KeyProfile");
        }

        [STAThread]
        static void Main(string[] args) {
            ReadConfiguration();

            Thread tApplicationRun = new Thread(ApplicationRun);
            tApplicationRun.SetApartmentState(ApartmentState.STA);
            tApplicationRun.Start();

            while (!DoneLoadingCfg)
                Thread.Sleep(100);
            
            Thread tPause = new Thread(Pause);
            tPause.SetApartmentState(ApartmentState.STA);
            tPause.IsBackground = true;
            tPause.Start();

            Thread tActivateKM = new Thread(Activate.ActivateKeyboardAndMouse);
            tActivateKM.SetApartmentState(ApartmentState.STA);
            tActivateKM.IsBackground = true;
            tActivateKM.Start();
        }

        private static void ApplicationRun() {
            Application.EnableVisualStyles();

            // Load configuration Files
            // My improvised Config file
            ActiveConfig = Data.Load(ActiveConfigFile);
            ReloadControlScheme();
            DoneLoadingCfg = true;

            // Start our lowlevel keyboard hook
            if (IntPtr.Size == 8) {
                Hooks.LowLevelKeyboardHook._hookID =
                    Hooks.LowLevelKeyboardHook.SetHook(Hooks.LowLevelKeyboardHook._proc);

                if (Hooks.LowLevelKeyboardHook._hookID == IntPtr.Zero) {
                    MessageBox.Show("Failed to find the Xbox Application to disable Escape.");
                }
            } else {
                MessageBox.Show("In 32bit mode you cannot disable the Escape key!", "Notice about 32bit", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

            Application.Run(mainform);
        }

        private static void Pause() {
            while (true) {
                var useModifier = (ActiveConfig.Controls_KB_Detach_MOD != (int)Key.None);
                bool detachKey  = (useModifier ?
                    Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_MOD) &&
                        Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_KEY) :
                    Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_KEY));

                if (detachKey) {
                    if (Activate.tKMInput.IsAlive == true && Activate.tXboxStream.IsAlive == true) {
                        Activate.tXboxStream.Abort();
                        Activate.tKMInput.Abort();

                        try {
                            XboxStream.tMouseMovement.Abort();
                        } catch (Exception) { }

                        CursorView.CursorShow();

                        //if (Activate.tKMInput.IsAlive == true && Activate.tXboxStream.IsAlive == true) {
                            // TODO: Handle failed threads
                        //    MessageBox.Show("Error:  Threads failed to abort");
                            
                        //}  
                        
                        //else {
                            // Reset the controller
                            Activate.ResetController();
                            
                            mainform.StatusStopped();
                        //}
                    } else if (Activate.tKMInput.IsAlive == false && Activate.tXboxStream.IsAlive == false) {
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
