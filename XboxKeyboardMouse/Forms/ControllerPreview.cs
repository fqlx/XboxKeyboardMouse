using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.XInput;
using Gamepad;
using xKM = XboxKeyboardMouse;
using ScpDriverInterface;

namespace XboxKeyboardMouse.Forms {
    public partial class ControllerPreview : Form {

        GamepadState xb;
        Color on  = Color.FromArgb(93, 194, 30);
        Color off = Color.Transparent;

        public ControllerPreview() {
            InitializeComponent();
        }

        Timer t = new Timer();

        private void ControllerPreview_Load(object sender, EventArgs e) {
            xb = new GamepadState(0);

            axisLeft.DrawAxisDot  = true;
            axisRight.DrawAxisDot = true;

            t.Interval = 1;
            t.Tick += T_Tick;
            
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e) {

            /* The Fantastic Four, ABXY */ {
                if (xKM.Activate.Controller.Buttons == X360Buttons.A)
                     xbo_m_A.BackColor = on;
                else xbo_m_A.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.B)
                     xbo_m_B.BackColor = on;
                else xbo_m_B.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.X)
                     xbo_m_X.BackColor = on;
                else xbo_m_X.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.Y)
                     xbo_m_Y.BackColor = on;
                else xbo_m_Y.BackColor = off;
            }

            /* Special Buttons (Start, Back, Guide) */ {
                if (xKM.Activate.Controller.Buttons == X360Buttons.Start)
                     xbo_m_Start.BackColor = on;
                else xbo_m_Start.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.Back)
                     xbo_m_Back.BackColor = on;
                else xbo_m_Back.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.Guide)
                     xbo_m_Guide.BackColor = on;
                else xbo_m_Guide.BackColor = off;
            }

            /* Sticks (Press) */
            {
                if (xKM.Activate.Controller.Buttons == X360Buttons.RightStick)
                     xbo_m_RS.BackColor = on;
                else xbo_m_RS.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.LeftStick)
                     xbo_m_LS.BackColor = on;
                else xbo_m_LS.BackColor = off;
            }

            /* Bumpers */ { 
                if (xKM.Activate.Controller.Buttons == X360Buttons.LeftBumper)
                     xbo_m_SL.BackColor = on;
                else xbo_m_SL.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.RightBumper)
                     xbo_m_SR.BackColor = on;
                else xbo_m_SR.BackColor = off;
            }

            /* DPAD */ { 
                if (xKM.Activate.Controller.Buttons == X360Buttons.Up)
                     xbo_m_DpadUp.BackColor = on;
                else xbo_m_DpadUp.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.Down)
                     xbo_m_DpadDown.BackColor = on;
                else xbo_m_DpadDown.BackColor = off;
                if(xKM.Activate.Controller.Buttons == X360Buttons.Left)
                     xbo_m_DpadLeft.BackColor = on;
                else xbo_m_DpadLeft.BackColor = off;
                if (xKM.Activate.Controller.Buttons == X360Buttons.Right)
                     xbo_m_DpadRight.BackColor = on;
                else xbo_m_DpadRight.BackColor = off;
            }

            /* Triggers */ {
                if (xKM.Activate.Controller.LeftTrigger >= 1)
                     xbo_k_TLeft.BackColor = on;
                else xbo_k_TLeft.BackColor = off;

                if (xKM.Activate.Controller.RightTrigger >= 1)
                     xbo_k_TRight.BackColor = on;
                else xbo_k_TRight.BackColor = off;
            }

            /* Draw Joysticks */ {
                axisLeft.SetAxis(
                    xKM.Activate.Controller.LeftStickX,
                    xKM.Activate.Controller.LeftStickY);

                axisRight.SetAxis(
                    xKM.Activate.Controller.RightStickX,
                    xKM.Activate.Controller.RightStickY);

                leftX.Text  = "LX: " + xKM.Activate.Controller.LeftStickX;
                leftY.Text  = "LY: " + xKM.Activate.Controller.LeftStickY;
                rightX.Text = "RX: " + xKM.Activate.Controller.RightStickX;
                rightY.Text = "RY: " + xKM.Activate.Controller.RightStickY;
            }
        }

        private void leftY_Click(object sender, EventArgs e) {

        }

        private void leftX_Click(object sender, EventArgs e) {

        }
    }
}
