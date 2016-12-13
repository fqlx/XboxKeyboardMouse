using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace XboxKeyboardMouse.Config {
    public class Data {

        // --> Config Settings
            public string Name = "default";
        // <-- Config Settings
        
        // --> Mouse
            public double Mouse_Sensitivity_X   = 6.213f;
            public double Mouse_Sensitivity_Y   = 6.213f;

            public bool Mouse_Invert_X          = false;
            public bool Mouse_Invert_Y          = false;

            public int Mouse_TickRate           = 40;
            public int Mouse_Eng_Type           = 0;
            
            public int Mouse_Eng_Relative_Val   = 1639;
        // <-- Mouse

        // --> Xbox Controls Keyboard

        // Main AXBY buttons
            public int Controls_KB_Xbox_A               = (int)Key.Space;
            public int Controls_KB_Xbox_B               = (int)Key.LeftCtrl;
            public int Controls_KB_Xbox_X               = (int)Key.R;
            public int Controls_KB_Xbox_Y               = (int)Key.D1;

            // Sholder Buttons
            public int Controls_KB_Xbox_LeftBumper      = (int)Key.Q;
            public int Controls_KB_Xbox_RightBumper     = (int)Key.E;

            // Xbox Access Buttons
            public int Controls_KB_Xbox_Guide           = (int)Key.Escape;
            public int Controls_KB_Xbox_Start           = (int)Key.B;
            public int Controls_KB_Xbox_Back            = (int)Key.V;

            // Xbox DPAD Buttons
            public int Controls_KB_Xbox_DPAD_Up         = (int)Key.Up;
            public int Controls_KB_Xbox_DPAD_Down       = (int)Key.Down;
            public int Controls_KB_Xbox_DPAD_Left       = (int)Key.Left;
            public int Controls_KB_Xbox_DPAD_Right      = (int)Key.Right;

            // Xbox Clickin Sticks
            public int Controls_KB_Xbox_Sticks_Left     = (int)Key.LeftShift;
            public int Controls_KB_Xbox_Sticks_Right    = (int)Key.C;

            // Xbox Triggers
            public int Controls_KB_Xbox_Trigger_Left    = (int)Key.None;
            public int Controls_KB_Xbox_Trigger_Right   = (int)Key.None;

            public int Controls_KB_Detach_MOD           = (int)Key.LeftAlt;
            public int Controls_KB_Detach_KEY           = (int)Key.C;

            // Sticks
            public int Controls_KB_Sticks_AXIS_L_Up     = (int)Key.W;
            public int Controls_KB_Sticks_AXIS_L_Down   = (int)Key.S;
            public int Controls_KB_Sticks_AXIS_L_Left   = (int)Key.A;
            public int Controls_KB_Sticks_AXIS_L_Right  = (int)Key.D;
            
            public int Controls_KB_Sticks_AXIS_R_Up     = (int)Key.None;
            public int Controls_KB_Sticks_AXIS_R_Down   = (int)Key.None;
            public int Controls_KB_Sticks_AXIS_R_Left   = (int)Key.None;
            public int Controls_KB_Sticks_AXIS_R_Right  = (int)Key.None;
            
            // Mouse Settings
            public int Controls_KB_MReset_MOD           = (int)Key.LeftAlt;
            public int Controls_KB_MReset_KEY           = (int)Key.P;
        // <-- Xbox Controls Keyboard

        // --> Xbox Controls Mouse 

            // Main AXBY buttons
            public int Controls_M_Xbox_A = -1;
            public int Controls_M_Xbox_B = -1;
            public int Controls_M_Xbox_X = -1;
            public int Controls_M_Xbox_Y = -1;

            // Sholder Buttons
            public int Controls_M_Xbox_LeftBumper = -1;
            public int Controls_M_Xbox_RightBumper = -1;

            // Xbox Access Buttons
            public int Controls_M_Xbox_Guide = -1;
            public int Controls_M_Xbox_Start = -1;
            public int Controls_M_Xbox_Back = -1;

            // Xbox DPAD Buttons
            public int Controls_M_Xbox_DPAD_Up = -1;
            public int Controls_M_Xbox_DPAD_Down = -1;
            public int Controls_M_Xbox_DPAD_Left = -1;
            public int Controls_M_Xbox_DPAD_Right = -1;

            // Xbox Clickin Sticks
            public int Controls_M_Xbox_Sticks_Left = -1;
            public int Controls_M_Xbox_Sticks_Right = -1;

            // Xbox Triggers
            public int Controls_M_Xbox_Trigger_Left = -1;
            public int Controls_M_Xbox_Trigger_Right = -1;

        // <-- Xbox Controls Mouse

        public static Data Load(string file) {
            Data d = new Data();

            if (!System.IO.Directory.Exists("profiles"))
                System.IO.Directory.CreateDirectory("profiles");

            if (!System.IO.File.Exists("profiles/" + file)) {
                System.IO.File.Create("profiles/" + file).Close();

                // Save the current config because it has the
                // default values
                Save(file, d);
                return d;
            }
            

            IniFile f = new IniFile("profiles/" + file);

            Read(f, "Config", "Name", ref d.Name);

            /* Mouse Settings */ {
                Read(f, "Mouse", "X_Sensitivity", ref d.Mouse_Sensitivity_X);
                Read(f, "Mouse", "Y_Sensitivity", ref d.Mouse_Sensitivity_Y);

                Read(f, "Mouse", "X_Inverted",  ref d.Mouse_Invert_X);
                Read(f, "Mouse", "Y_Inverted",  ref d.Mouse_Invert_Y);

                Read(f, "Mouse", "Type",        ref d.Mouse_Eng_Type);
                Read(f, "Mouse", "ERel_Val",    ref d.Mouse_Eng_Relative_Val);
            }

            /* Controls - Keyboard */ {
                Read(f, "Controls_Keyboard", "Button_A", ref d.Controls_KB_Xbox_A);
                Read(f, "Controls_Keyboard", "Button_B", ref d.Controls_KB_Xbox_B);
                Read(f, "Controls_Keyboard", "Button_X", ref d.Controls_KB_Xbox_X);
                Read(f, "Controls_Keyboard", "Button_Y", ref d.Controls_KB_Xbox_Y);

                Read(f, "Controls_Keyboard", "Bumper_Left", ref d.Controls_KB_Xbox_LeftBumper);
                Read(f, "Controls_Keyboard", "Bumper_Right", ref d.Controls_KB_Xbox_RightBumper);

                Read(f, "Controls_Keyboard", "Button_Guide", ref d.Controls_KB_Xbox_Guide);
                Read(f, "Controls_Keyboard", "Button_Start", ref d.Controls_KB_Xbox_Start);
                Read(f, "Controls_Keyboard", "Button_Back", ref d.Controls_KB_Xbox_Back);

                Read(f, "Controls_Keyboard", "Dpad_Up", ref d.Controls_KB_Xbox_DPAD_Up);
                Read(f, "Controls_Keyboard", "Dpad_Down", ref d.Controls_KB_Xbox_DPAD_Down);
                Read(f, "Controls_Keyboard", "Dpad_Left", ref d.Controls_KB_Xbox_DPAD_Left);
                Read(f, "Controls_Keyboard", "Dpad_Right", ref d.Controls_KB_Xbox_DPAD_Right);

                Read(f, "Controls_Keyboard", "Trigger_Left", ref d.Controls_KB_Xbox_Trigger_Left);
                Read(f, "Controls_Keyboard", "Trigger_Right", ref d.Controls_KB_Xbox_Trigger_Right);

                Read(f, "Controls_Keyboard", "Detach_Mod", ref d.Controls_KB_Detach_MOD);
                Read(f, "Controls_Keyboard", "Detach_Key", ref d.Controls_KB_Detach_KEY);

                Read(f, "Controls_Keyboard", "Sticks_Left", ref d.Controls_KB_Xbox_Sticks_Left);
                Read(f, "Controls_Keyboard", "Sticks_Right", ref d.Controls_KB_Xbox_Sticks_Right);
                
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Left_Up",     ref d.Controls_KB_Sticks_AXIS_L_Up);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Left_Down",   ref d.Controls_KB_Sticks_AXIS_L_Down);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Left_Left",   ref d.Controls_KB_Sticks_AXIS_L_Left);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Left_Right",  ref d.Controls_KB_Sticks_AXIS_L_Right);

                Read(f, "Controls_Keyboard", "Sticks_AXIS_Right_Up",    ref d.Controls_KB_Sticks_AXIS_R_Up);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Right_Down",  ref d.Controls_KB_Sticks_AXIS_R_Down);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Right_Left",  ref d.Controls_KB_Sticks_AXIS_R_Left);
                Read(f, "Controls_Keyboard", "Sticks_AXIS_Right_Right", ref d.Controls_KB_Sticks_AXIS_R_Right);

                Read(f, "Controls_Keyboard", "Mouse_Reset_Look_MOD", ref d.Controls_KB_MReset_MOD);
                Read(f, "Controls_Keyboard", "Mouse_Reset_Look_KEY", ref d.Controls_KB_MReset_KEY);
            }

            /* Controls - Mouse */ {
                Read(f, "Controls_Mouse", "Button_A", ref d.Controls_M_Xbox_A);
                Read(f, "Controls_Mouse", "Button_B", ref d.Controls_M_Xbox_B);
                Read(f, "Controls_Mouse", "Button_X", ref d.Controls_M_Xbox_X);
                Read(f, "Controls_Mouse", "Button_Y", ref d.Controls_M_Xbox_Y);

                Read(f, "Controls_Mouse", "Bumper_Left", ref d.Controls_M_Xbox_LeftBumper);
                Read(f, "Controls_Mouse", "Bumper_Right", ref d.Controls_M_Xbox_RightBumper);

                Read(f, "Controls_Mouse", "Button_Guide", ref d.Controls_M_Xbox_Guide);
                Read(f, "Controls_Mouse", "Button_Start", ref d.Controls_M_Xbox_Start);
                Read(f, "Controls_Mouse", "Button_Back", ref d.Controls_M_Xbox_Back);

                Read(f, "Controls_Mouse", "Dpad_Up", ref d.Controls_M_Xbox_DPAD_Up);
                Read(f, "Controls_Mouse", "Dpad_Down", ref d.Controls_M_Xbox_DPAD_Down);
                Read(f, "Controls_Mouse", "Dpad_Left", ref d.Controls_M_Xbox_DPAD_Left);
                Read(f, "Controls_Mouse", "Dpad_Right", ref d.Controls_M_Xbox_DPAD_Right);

                Read(f, "Controls_Mouse", "Sticks_Left", ref d.Controls_M_Xbox_Sticks_Left);
                Read(f, "Controls_Mouse", "Sticks_Right", ref d.Controls_M_Xbox_Sticks_Right);

                Read(f, "Controls_Mouse", "Trigger_Left", ref d.Controls_M_Xbox_Trigger_Left);
                Read(f, "Controls_Mouse", "Trigger_Right", ref d.Controls_M_Xbox_Trigger_Right);
            } 

            return d;
        }

        public static void Save(string file, Data d) {
            if (!System.IO.Directory.Exists("profiles")) {
                System.IO.Directory.CreateDirectory("profiles");
            } if (!System.IO.File.Exists("profiles/" + file)) {
                System.IO.File.Create("profiles/" + file).Close();
            }


            IniFile f = new IniFile("profiles/" + file);
            Write(f, "Config", "Name", d.Name);

            /* Mouse Settings */ {
                Write(f, "Mouse", "X_Sensitivity", d.Mouse_Sensitivity_X);
                Write(f, "Mouse", "Y_Sensitivity", d.Mouse_Sensitivity_Y);

                Write(f, "Mouse", "X_Inverted", d.Mouse_Invert_X);
                Write(f, "Mouse", "Y_Inverted", d.Mouse_Invert_Y);
                Write(f, "Mouse", "Type",       d.Mouse_Eng_Type);
                Write(f, "Mouse", "ERel_Val",   d.Mouse_Eng_Relative_Val);
            }

            /* Controls - Keyboard */ {
                Write(f, "Controls_Keyboard", "Button_A", d.Controls_KB_Xbox_A);
                Write(f, "Controls_Keyboard", "Button_B", d.Controls_KB_Xbox_B);
                Write(f, "Controls_Keyboard", "Button_X", d.Controls_KB_Xbox_X);
                Write(f, "Controls_Keyboard", "Button_Y", d.Controls_KB_Xbox_Y);

                Write(f, "Controls_Keyboard", "Bumper_Left", d.Controls_KB_Xbox_LeftBumper);
                Write(f, "Controls_Keyboard", "Bumper_Right", d.Controls_KB_Xbox_RightBumper);

                Write(f, "Controls_Keyboard", "Button_Guide", d.Controls_KB_Xbox_Guide);
                Write(f, "Controls_Keyboard", "Button_Start", d.Controls_KB_Xbox_Start);
                Write(f, "Controls_Keyboard", "Button_Back", d.Controls_KB_Xbox_Back);

                Write(f, "Controls_Keyboard", "Dpad_Up", d.Controls_KB_Xbox_DPAD_Up);
                Write(f, "Controls_Keyboard", "Dpad_Down", d.Controls_KB_Xbox_DPAD_Down);
                Write(f, "Controls_Keyboard", "Dpad_Left", d.Controls_KB_Xbox_DPAD_Left);
                Write(f, "Controls_Keyboard", "Dpad_Right", d.Controls_KB_Xbox_DPAD_Right);

                Write(f, "Controls_Keyboard", "Sticks_Left", d.Controls_KB_Xbox_Sticks_Left);
                Write(f, "Controls_Keyboard", "Sticks_Right", d.Controls_KB_Xbox_Sticks_Right);

                Write(f, "Controls_Keyboard", "Trigger_Left", d.Controls_KB_Xbox_Trigger_Left);
                Write(f, "Controls_Keyboard", "Trigger_Right", d.Controls_KB_Xbox_Trigger_Right);

                Write(f, "Controls_Keyboard", "Detach_Mod", d.Controls_KB_Detach_MOD);
                Write(f, "Controls_Keyboard", "Detach_Key", d.Controls_KB_Detach_KEY);

                Write(f, "Controls_Keyboard", "Sticks_AXIS_Left_Up", d.Controls_KB_Sticks_AXIS_L_Up);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Left_Down", d.Controls_KB_Sticks_AXIS_L_Down);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Left_Left", d.Controls_KB_Sticks_AXIS_L_Left);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Left_Right", d.Controls_KB_Sticks_AXIS_L_Right);

                Write(f, "Controls_Keyboard", "Sticks_AXIS_Right_Up",    d.Controls_KB_Sticks_AXIS_R_Up);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Right_Down",  d.Controls_KB_Sticks_AXIS_R_Down);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Right_Left",  d.Controls_KB_Sticks_AXIS_R_Left);
                Write(f, "Controls_Keyboard", "Sticks_AXIS_Right_Right", d.Controls_KB_Sticks_AXIS_R_Right);

                Write(f, "Controls_Keyboard", "Mouse_Reset_Look_MOD",    d.Controls_KB_MReset_MOD);
                Write(f, "Controls_Keyboard", "Mouse_Reset_Look_KEY",    d.Controls_KB_MReset_KEY);
            }

            /* Controls - Mouse */ {
	            Write(f, "Controls_Mouse", "Button_A", d.Controls_M_Xbox_A);
	            Write(f, "Controls_Mouse", "Button_B", d.Controls_M_Xbox_B);
	            Write(f, "Controls_Mouse", "Button_X", d.Controls_M_Xbox_X);
	            Write(f, "Controls_Mouse", "Button_Y", d.Controls_M_Xbox_Y);

	            Write(f, "Controls_Mouse", "Bumper_Left", d.Controls_M_Xbox_LeftBumper);
	            Write(f, "Controls_Mouse", "Bumper_Right", d.Controls_M_Xbox_RightBumper);

	            Write(f, "Controls_Mouse", "Button_Guide", d.Controls_M_Xbox_Guide);
	            Write(f, "Controls_Mouse", "Button_Start", d.Controls_M_Xbox_Start);
	            Write(f, "Controls_Mouse", "Button_Back", d.Controls_M_Xbox_Back);

	            Write(f, "Controls_Mouse", "Dpad_Up", d.Controls_M_Xbox_DPAD_Up);
	            Write(f, "Controls_Mouse", "Dpad_Down", d.Controls_M_Xbox_DPAD_Down);
	            Write(f, "Controls_Mouse", "Dpad_Left", d.Controls_M_Xbox_DPAD_Left);
	            Write(f, "Controls_Mouse", "Dpad_Right", d.Controls_M_Xbox_DPAD_Right);

	            Write(f, "Controls_Mouse", "Sticks_Left", d.Controls_M_Xbox_Sticks_Left);
	            Write(f, "Controls_Mouse", "Sticks_Right", d.Controls_M_Xbox_Sticks_Right);

	            Write(f, "Controls_Mouse", "Trigger_Left", d.Controls_M_Xbox_Trigger_Left);
	            Write(f, "Controls_Mouse", "Trigger_Right", d.Controls_M_Xbox_Trigger_Right);
            }

            f.SaveSettings();
            return;
        }
        
        private static void Read(IniFile ini, string Section, string Name, ref double @out) {
            string sTmp; double dTmp = 0;

            try {
                sTmp = ini.GetSetting(Section, Name);
                double.TryParse(sTmp, out dTmp);
                @out = dTmp;
            } catch (Exception ex) {
                MessageBox.Show($"Failed to read value [{Section}]{Name}, using default value...\n\nError: " + ex.Message);
            }
        }

        private static void Read(IniFile ini, string Section, string Name, ref string @out) {
            @out = ini.GetSetting(Section, Name);
        }

        private static void Read(IniFile ini, string Section, string Name, ref int @out) {
            string sTmp; int dTmp = 0;

            try {
                sTmp = ini.GetSetting(Section, Name);
                int.TryParse(sTmp, out dTmp);
                @out = dTmp;
            } catch (Exception ex) {
                MessageBox.Show($"Failed to read value [{Section}]{Name}, using default value...\n\nError: " + ex.Message);
            }
        }

        private static void Read(IniFile ini, string Section, string Name, ref bool @out) {
            string tf = ini.GetSetting(Section, Name).ToLower();
            string[] chks = { "1", "y", "t", "true" };
            @out = chks.Contains(tf.ToLower());
        }

        private static void Write(IniFile ini, string Section, string Name, double val) {
            ini.AddSetting(Section, Name, val + "");
        }

        private static void Write(IniFile ini, string Section, string Name, int val) {
            ini.AddSetting(Section, Name, val + "");
        }

        private static void Write(IniFile ini, string Section, string Name, bool val) {
            ini.AddSetting(Section, Name, val ? "true" : "false");
        }

        private static void Write(IniFile ini, string Section, string Name, string val) {
            ini.AddSetting(Section, Name, val);
        }

    }
}
