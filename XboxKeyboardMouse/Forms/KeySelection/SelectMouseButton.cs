using System.Windows.Forms;

namespace XboxKeyboardMouse.Forms {
    public partial class SelectMouseButton : Form {
        Config.Data cfg;
        string Tag;
        
        private int convert(MouseButtons btn) {

            if (btn == MouseButtons.None)
                return -1;

            else if (btn == MouseButtons.Left)
                return 0;

            else if (btn == MouseButtons.Right)
                return 2;

            else if (btn == MouseButtons.Middle)
                return 1;

            else if (btn == MouseButtons.XButton1)
                return 3;

            else if (btn == MouseButtons.XButton2)
                return 4;

            return -1;
        }


        public SelectMouseButton(Config.Data cfg, string Tag) {
            InitializeComponent();

            this.cfg = cfg;
            this.Tag = Tag;
        }

        private void SelectMouseButton_MouseUp(object sender, MouseEventArgs e) {
            int usableValue = convert(e.Button);
            label2.Text = e.Button.ToString();
        }
    }
}
