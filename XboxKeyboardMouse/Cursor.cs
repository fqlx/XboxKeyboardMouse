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

            form.TransparencyKey = System.Drawing.Color.Turquoise;  //hacky method for transparent background, acts like a greenscreen
            form.BackColor = System.Drawing.Color.Turquoise;
            form.FormBorderStyle = FormBorderStyle.None;

           form.Show();
        }
        
        //making a hidden top most form and setcursor(false)
        public static void HideCursorWhenOn()
        {
            //todo, check if Xbox app is open
            bool XboxAppOpen = true;
            
            while(true)
            {
                if (XboxAppOpen == true && form != null)
                {
                    CreateTopMostHiddenForm();

                    Cursor.Hide();
                }
                else if(XboxAppOpen == false)
                {
                    //Cursor.Show();
                    form.Close();
                }
                Thread.Sleep(1000);
            }
        }
    }
}
