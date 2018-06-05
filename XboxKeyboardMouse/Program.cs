using MaterialSkin;
using SimWinInput;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using XboxKeyboardMouse.Config;
using XboxKeyboardMouse.Forms;
#if (DEBUG)
using XboxKeyboardMouse.Libs;
#endif

namespace XboxKeyboardMouse
{
    static class Program {
        public static MainForm MainForm;

        /** Public Variables Across the Whole Application */
        public static Config.Data         ActiveConfig;
        public static bool                DoneLoadingCfg = false;
        public static bool                HideCursor     = true;
        public static string              ActiveConfigFile = "";
        public static MaterialSkinManager mSkin;

        public static bool                ToggleStatusState = false;

        [DllImport("kernel32.dll")]
            static extern bool AttachConsole(int input);
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int FreeConsole();
        
        public static bool SetActiveConfig(string file) {
            var profilePath = Path.Combine("profiles", file);

            if (!File.Exists(profilePath)) 
                 return false;

            lock (ActiveConfig) {
                Data d = Data.Load(file);
                ActiveConfig = d;
            }

            // Make sure the ini file exists
            ReadConfiguration(file);

            ReloadActiveConfig();

            IniFile appcfg = new IniFile("config.ini");
            appcfg.AddSetting("Xbox", "KeyProfile", file);
            appcfg.SaveSettings();
            
            return true;
        }

        public static void ReloadActiveConfig() {
            ReloadControlScheme();

            // Load our application settings
            HideCursor = !ActiveConfig.Application_ShowCursor;
            Hooks.LowLevelKeyboardHook.LockEscape = ActiveConfig.Application_LockEscape;
        }

        public static void ReloadControlScheme() {
            TranslateKeyboard.ClearAllDicts();

            lock (TranslateKeyboard.buttons)
            {
                TranslateKeyboard.mapRunTimeOptions[RunTimeOptionType.CalibrateDeadZone] = ActiveConfig.Controls_Calibrate_DeadZone == 0 ? null : new RunTimeOption()
                {
                    Key = (Key)ActiveConfig.Controls_Calibrate_DeadZone,
                    Run = TranslateMouse.RunCalibrateDeadZone,
                };

                TranslateKeyboard.mapRunTimeOptions[RunTimeOptionType.FineTuneDeadZone] = ActiveConfig.Controls_Calibrate_FineDeadZone == 0 ? null : new RunTimeOption()
                {
                    Key = (Key)ActiveConfig.Controls_Calibrate_FineDeadZone,
                    Run = TranslateMouse.RunFineTuneDeadZone,
                };

                if (ActiveConfig.Controls_KB_Xbox_A != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_A, GamePadControl.A);
                if (ActiveConfig.Controls_KB_Xbox_B != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_B, GamePadControl.B);
                if (ActiveConfig.Controls_KB_Xbox_X != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_X, GamePadControl.X);
                if (ActiveConfig.Controls_KB_Xbox_Y != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Y, GamePadControl.Y);

                if (ActiveConfig.Controls_KB_Xbox_DPAD_Up != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Up, GamePadControl.DPadUp);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Down != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Down, GamePadControl.DPadDown);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Left != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Left, GamePadControl.DPadLeft);
                if (ActiveConfig.Controls_KB_Xbox_DPAD_Right != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_DPAD_Right, GamePadControl.DPadRight);

                if (ActiveConfig.Controls_KB_Xbox_Guide != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Guide, GamePadControl.Guide);
                if (ActiveConfig.Controls_KB_Xbox_Start != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Start, GamePadControl.Start);
                if (ActiveConfig.Controls_KB_Xbox_Back != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Back, GamePadControl.Back);

                if (ActiveConfig.Controls_KB_Xbox_LeftBumper != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_LeftBumper, GamePadControl.LeftShoulder);
                if (ActiveConfig.Controls_KB_Xbox_RightBumper != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_RightBumper, GamePadControl.RightShoulder);

                if (ActiveConfig.Controls_KB_Xbox_Sticks_Left != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Sticks_Left, GamePadControl.LeftStickClick);
                if (ActiveConfig.Controls_KB_Xbox_Sticks_Right != 0)
                    TranslateKeyboard.buttons.Add((Key)ActiveConfig.Controls_KB_Xbox_Sticks_Right, GamePadControl.RightStickClick);

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
            }
        }

        public static void ReadConfiguration(string defaultProfile = "default.ini") {
            // Setup the default application cfg file if does not exist
            IniFile appcfg = null;
            
            if (!File.Exists("config.ini")) {
                File.Create("config.ini").Close();

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
            // Check if we are debugging
            // if so then attach the console
            #if (DEBUG)
                int conPtr = AllocConsole();
                AttachConsole(conPtr);
                Logger.appendLogLine("State", "Console Attached!", Logger.Type.Info);
            #endif
            
            ReadConfiguration();
            ActiveConfig = Data.Load(ActiveConfigFile);
            ReloadActiveConfig();
            ReloadControlScheme();

            Thread tApplicationRun = new Thread(ApplicationRun);
            tApplicationRun.SetApartmentState(ApartmentState.STA);
            tApplicationRun.Start();

            while (MainForm == null)
                Thread.Sleep(100);

            MainForm.FormClosing += (sender, e) => { Activate.ShutDown(); };

            Thread tPause = new Thread(Pause);
            tPause.SetApartmentState(ApartmentState.STA);
            tPause.IsBackground = true;
            tPause.Start();
            
            Thread tActivateKM = new Thread(() => { Activate.ActivateKeyboardAndMouse();  });
            tActivateKM.SetApartmentState(ApartmentState.STA);
            tActivateKM.IsBackground = true;
            tActivateKM.Start();
        }

        private static void ApplicationRun() {
            Application.EnableVisualStyles();
            
            // Start our lowlevel keyboard hook
            if (IntPtr.Size == 8) {
                Hooks.LowLevelKeyboardHook._hookID =
                    Hooks.LowLevelKeyboardHook.SetHook(Hooks.LowLevelKeyboardHook._proc);

                if (Hooks.LowLevelKeyboardHook._hookID == IntPtr.Zero) {
                    MessageBox.Show("Failed to find the Xbox Application to disable Escape.");
                }
            } else {
                //MessageBox.Show("In 32bit mode you cannot disable the Escape key!", "Notice about 32bit", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

            mSkin = MaterialSkinManager.Instance;
            MainForm = new MainForm();
            Application.Run(MainForm);
        }

        private static void Pause() {
            while (true) {
                var useModifier = (ActiveConfig.Controls_KB_Detach_MOD != (int)Key.None);
                bool detachKey  = (useModifier ?
                    Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_MOD) &&
                        Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_KEY) :
                    Keyboard.IsKeyDown((Key)ActiveConfig.Controls_KB_Detach_KEY));

                if (detachKey || ToggleStatusState) {
                    if (ToggleStatusState) {
                        ToggleStatusState = false;
                    }

                    bool inputDead = !Activate.tKMInput.IsAlive;
                    bool streamDead = !Activate.tXboxStream.IsAlive;

                    #if (DEBUG)
                        Logger.appendLogLine("Threads", $"Threads Status: Input - {!inputDead}, Stream - {!streamDead}",
                                             Logger.Type.Debug);
                    #endif

                    if (!inputDead && !streamDead) {
                        #if (DEBUG)
                            Logger.appendLogLine("Threads", "Aborting all threads!", Logger.Type.Info);
                        #endif

                        try { Activate.tXboxStream.Abort(); }       catch (Exception) { }
                        try { Activate.tKMInput.Abort(); }          catch (Exception) { }
                        try { XboxStream.tMouseMovement.Abort(); }  catch (Exception) { } 

                        CursorView.CursorShow();

                        /*
                        if (Activate.tKMInput.IsAlive == true && Activate.tXboxStream.IsAlive == true) {
                            // TODO: Handle failed threads
                            MessageBox.Show("Error:  Threads failed to abort");
                        }  //*/

                        // Reset the controller
                        Activate.ResetController();
                            
                        MainForm.StatusStopped();
                    } else {
                        // && Activate.tXboxStream.IsAlive == false
                        if (inputDead || streamDead) {
                            // Start the required threads
                            Thread tActivateKM = new Thread(() => {
                                Activate.ActivateKeyboardAndMouse(streamDead, inputDead);
                            });
                            tActivateKM.SetApartmentState(ApartmentState.STA);
                            tActivateKM.IsBackground = true;
                            tActivateKM.Start();
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
