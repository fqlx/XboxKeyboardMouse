using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XboxKeyboardMouse.Forms.Controls;

namespace XboxKeyboardMouse.Forms.Controls {
    public partial class FormRWE : RoundedForm {
        public FormRWE() {
            InitializeComponent();
        }

        // Called to change status color's
        public virtual void SetStatusColor(Color c) { }
    }
}
