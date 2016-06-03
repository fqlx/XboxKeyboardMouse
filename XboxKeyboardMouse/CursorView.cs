using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using XboxMouse_Keyboard;

namespace XboxKeyboardMouse
{
    class CursorView
    {
        private const int FORM_SIZE_TO_HIDE_CURSOR = 200;

        private static Form form = null;

        private static void CreateTopMostHiddenForm()
        {
            form = new Form();

            form.Size = new Size(FORM_SIZE_TO_HIDE_CURSOR, FORM_SIZE_TO_HIDE_CURSOR);

            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - form.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - form.Height) / 2);

            form.TopMost = true;

            /*
            form.TransparencyKey = System.Drawing.Color.Turquoise;  //hacky method for transparent background, acts like a greenscreen
            form.BackColor = System.Drawing.Color.Turquoise;
            form.FormBorderStyle = FormBorderStyle.None;
            */
        
            form.Opacity = .2;

            form.Focus();
            form.Show();
        }

        public static void CursorHide()
        {
            if(form == null)
                CreateTopMostHiddenForm();

            if(form != null)
                Cursor.Hide();
        }

        public static void CursorShow()
        {
            if (form != null)
            {
                Cursor.Show();
                form.Dispose();
                form = null;  //dispose doesn't set form to null?
            }
        }
        
    }
}
