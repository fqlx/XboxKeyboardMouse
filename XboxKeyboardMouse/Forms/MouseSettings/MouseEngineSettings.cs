using System.Drawing;
using System.Windows.Forms;
using XboxKeyboardMouse.Config;

namespace XboxKeyboardMouse.Forms.MouseSettings
{
    public partial class MouseEngineSettings : UserControl {
        private Size control_size = new Size(457, 254);

        private int minIndex, maxIndex;

        public bool IsCorrectIndex(int index) {
            return (index >= minIndex && index <= maxIndex);
        }

        public MouseEngineSettings(int minIndex, int maxIndex) {
            InitializeComponent();

            this.minIndex = minIndex;
            this.maxIndex = maxIndex;
            this.Dock = DockStyle.Fill;
        }

        public new System.Drawing.Size Size {
            get { return control_size; }
            set { }
        }

        public virtual void SetConfig(Data config) { }
    }
}
