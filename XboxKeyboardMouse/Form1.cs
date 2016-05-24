using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace XboxKeyboardMouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void startButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void activate_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(InputToController.ActivateKeyboardAndMouse);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
