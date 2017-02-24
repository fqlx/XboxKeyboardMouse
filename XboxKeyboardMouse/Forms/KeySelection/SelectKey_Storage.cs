using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XboxKeyboardMouse.Forms {
    public partial class SelectKey_Storage : Form {
        public SelectKey_Storage(Models.MSelectKey_Storage storage) {
            this.storage = storage;

            InitializeComponent();

            foreach (Control c in Controls)
                c.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectKey_Storage_KeyUp);
            KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectKey_Storage_KeyUp);
        }

        Models.MSelectKey_Storage storage;

        private void SelectKey_Storage_KeyUp(object sender, KeyEventArgs e) {
            // Get keycode
            storage.inputKey = (int)System.Windows.Input.KeyInterop.KeyFromVirtualKey((int)e.KeyValue);
            label2.Text = e.KeyData.ToString();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            storage.inputKey = (int)System.Windows.Input.Key.None;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            storage.Cancel = true;
            this.Close();
        }
    }
}
