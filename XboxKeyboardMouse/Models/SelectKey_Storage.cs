using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxKeyboardMouse.Models {
    public class MSelectKey_Storage {
        public int inputMod = (int)System.Windows.Input.Key.None;
        public int inputKey = (int)System.Windows.Input.Key.None;

        public bool Cancel = false;
    }
}
