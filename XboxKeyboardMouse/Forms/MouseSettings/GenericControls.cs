using System;
using System.Windows.Forms;
using XboxKeyboardMouse.Config;

namespace XboxKeyboardMouse.Forms.MouseSettings
{
    public partial class GenericControls : MouseEngineSettings {
        private Data config;
        
        public GenericControls (Data config) : base(0, 5) {
            InitializeComponent();

            SetConfig(config);
        }

        public override void SetConfig(Data config) {
            this.config = config;

            decimal dSensitivityX,
                    dSensitivityY,
                    dFinalMod;

            dSensitivityX = (decimal)config.Mouse_Sensitivity_X;
            dSensitivityY = (decimal)config.Mouse_Sensitivity_Y;
            dFinalMod     = (decimal)config.Mouse_FinalMod;

            if (dSensitivityX < mouseXSense.Minimum || dSensitivityX > mouseXSense.Maximum) {
                var def = mouseXSense.Value;
                var defS = def.ToString("0.#####");
                var curS = dSensitivityX.ToString("0.#####");

                MessageBox.Show($"X Sensitivity is to small/low, it has been reset to ({defS}) from ({curS}).");

                dSensitivityX = def;
            }

            if (dSensitivityY < mouseYSense.Minimum || dSensitivityY > mouseYSense.Maximum) {
                var def = mouseYSense.Value;
                var defS = def.ToString("0.#####");
                var curS = dSensitivityY.ToString("0.#####");

                MessageBox.Show($"Y Sensitivity is to small/low, it has been reset to ({defS}) from ({curS}).");

                dSensitivityY = def;
            }

            if (dFinalMod < mouseMouseModifier.Minimum || dFinalMod > mouseMouseModifier.Maximum) {
                var def = mouseMouseModifier.Value;
                var defS = def.ToString("0.#####");
                var curS = dFinalMod.ToString("0.#####");

                MessageBox.Show($"Final Modifier is to small/low, it has been reset to ({defS}) from ({curS}).");

                dFinalMod = def;
            }


            mouseXSense.Value          = dSensitivityX;
            mouseYSense.Value          = dSensitivityY;
            mouseMouseModifier.Value   = dFinalMod;
        }

        private void mouseMouseModifier_ValueChanged(object sender, EventArgs e) {
            config.Mouse_FinalMod = (double)mouseMouseModifier.Value;
        }

        private void mouseYSense_ValueChanged(object sender, EventArgs e) {
            config.Mouse_Sensitivity_Y = (double)mouseYSense.Value;
        }

        private void mouseXSense_ValueChanged(object sender, EventArgs e) {
            config.Mouse_Sensitivity_X = (double)mouseXSense.Value;
        }
    }
}
