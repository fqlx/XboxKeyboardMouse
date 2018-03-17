using SimWinInput;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace XboxKeyboardMouse.Forms
{
    public partial class ControllerPreview : Form {
        Color on  = Color.FromArgb(93, 194, 30);
        Color off = Color.Transparent;
        private SimulatedGamePadState state;

        public ControllerPreview() {
            InitializeComponent();
        }

        Timer t = new Timer();

        private void ControllerPreview_Load(object sender, EventArgs e) {
            state = SimGamePad.Instance.State[0];

            axisLeft.DrawAxisDot  = true;
            axisRight.DrawAxisDot = true;

            t.Interval = 1;
            t.Tick += T_Tick;
            
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e) {

            /* The Fantastic Four, ABXY */ {
                if (state.Buttons == GamePadControl.A)
                     xbo_m_A.BackColor = on;
                else xbo_m_A.BackColor = off;
                if (state.Buttons == GamePadControl.B)
                     xbo_m_B.BackColor = on;
                else xbo_m_B.BackColor = off;
                if (state.Buttons == GamePadControl.X)
                     xbo_m_X.BackColor = on;
                else xbo_m_X.BackColor = off;
                if (state.Buttons == GamePadControl.Y)
                     xbo_m_Y.BackColor = on;
                else xbo_m_Y.BackColor = off;
            }

            /* Special Buttons (Start, Back, Guide) */ {
                if (state.Buttons == GamePadControl.Start)
                     xbo_m_Start.BackColor = on;
                else xbo_m_Start.BackColor = off;
                if (state.Buttons == GamePadControl.Back)
                     xbo_m_Back.BackColor = on;
                else xbo_m_Back.BackColor = off;
                if (state.Buttons == GamePadControl.Guide)
                     xbo_m_Guide.BackColor = on;
                else xbo_m_Guide.BackColor = off;
            }

            /* Sticks (Press) */
            {
                if (state.Buttons == GamePadControl.RightStickClick)
                     xbo_m_RS.BackColor = on;
                else xbo_m_RS.BackColor = off;
                if (state.Buttons == GamePadControl.LeftStickClick)
                     xbo_m_LS.BackColor = on;
                else xbo_m_LS.BackColor = off;
            }

            /* Bumpers */ { 
                if (state.Buttons == GamePadControl.LeftShoulder)
                     xbo_m_SL.BackColor = on;
                else xbo_m_SL.BackColor = off;
                if (state.Buttons == GamePadControl.RightShoulder)
                     xbo_m_SR.BackColor = on;
                else xbo_m_SR.BackColor = off;
            }

            /* DPAD */ { 
                if (state.Buttons == GamePadControl.DPadUp)
                     xbo_m_DpadUp.BackColor = on;
                else xbo_m_DpadUp.BackColor = off;
                if (state.Buttons == GamePadControl.DPadDown)
                     xbo_m_DpadDown.BackColor = on;
                else xbo_m_DpadDown.BackColor = off;
                if(state.Buttons == GamePadControl.DPadLeft)
                     xbo_m_DpadLeft.BackColor = on;
                else xbo_m_DpadLeft.BackColor = off;
                if (state.Buttons == GamePadControl.DPadRight)
                     xbo_m_DpadRight.BackColor = on;
                else xbo_m_DpadRight.BackColor = off;
            }

            /* Triggers */ {
                if (state.LeftTrigger >= 1)
                     xbo_k_TLeft.BackColor = on;
                else xbo_k_TLeft.BackColor = off;

                if (state.RightTrigger >= 1)
                     xbo_k_TRight.BackColor = on;
                else xbo_k_TRight.BackColor = off;
            }

            /* Draw Joysticks */ {
                axisLeft.SetAxis(
                    state.LeftStickX,
                    state.LeftStickY);

                axisRight.SetAxis(
                    state.RightStickX,
                    state.RightStickY);

                leftX.Text  = "LX: " + state.LeftStickX;
                leftY.Text  = "LY: " + state.LeftStickY;
                rightX.Text = "RX: " + state.RightStickX;
                rightY.Text = "RY: " + state.RightStickY;
            }
        }
    }
}
