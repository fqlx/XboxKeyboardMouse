using XboxMouse_Keyboard;

namespace XboxKeyboardMouse
{
    class CursorView
    {
        private static void CreateTopMostHiddenForm()
        {
            Form form = new Form();

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
        
            CreateTopMostHiddenForm();

            //todo, check if Xbox app is open
            boolean XboxAppOpen = true;
            
            while(true)
            {
                if(XboxAppOpen == true)
                   Cursor.Hide()
                else
                   Cursor.Show();
                
                Thread.Sleep(1000);
            }
        }
    }
}
