using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XboxKeyboardMouse.Models;

namespace XboxKeyboardMouse.Forms {

    public partial class SelectKey_Modifier : Form {
        public SelectKey_Modifier(MSelectKey_Storage st, bool loadKeys = false) {
            storage = st;

            InitializeComponent();

            if (loadKeys)
                updateKeyText();
        }

        public MSelectKey_Storage storage;
        private bool close;

        private void updateKeyText() {
            lnkMod.Text = ((System.Windows.Input.Key)storage.inputMod).ToString();
            lnkKey.Text = ((System.Windows.Input.Key)storage.inputKey).ToString();
        }

        private void lnkKeyRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            storage.inputKey = (int)System.Windows.Input.Key.None;
            updateKeyText();
        }

        private void lnkModRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            storage.inputMod = (int)System.Windows.Input.Key.None;
            updateKeyText();
        }

        private void button2_Click(object sender, EventArgs e) {
            storage.inputMod = (int)System.Windows.Input.Key.None;
            storage.inputKey = (int)System.Windows.Input.Key.None;

            // Return dialog
            ReturnDialog();
        }

        private void ReturnDialog() {
            this.close = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            ReturnDialog();
        }

        private void lnkMod_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Models.MSelectKey_Storage s = new Models.MSelectKey_Storage();

            var frm = new SelectKey_Storage(s);
            frm.ShowDialog();

            if (s.Cancel) return;

            storage.inputMod = s.inputKey;
            updateKeyText();
        }

        private void lnkKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Models.MSelectKey_Storage s = new Models.MSelectKey_Storage();

            var frm = new SelectKey_Storage(s);
            frm.ShowDialog();

            if (s.Cancel) return;

            storage.inputKey = s.inputKey;
            updateKeyText();
        }

        private void SelectKey_Modifier_FormClosing(object sender, FormClosingEventArgs e) {
            if (!close)
                storage.Cancel = true;
        }
    }
}
