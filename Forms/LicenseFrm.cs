using System;
using System.Windows.Forms;

namespace GenetixKit
{
    public partial class LicenseFrm : Form
    {
        public LicenseFrm()
        {
            InitializeComponent();
        }

        private void LicenseFrm_Activated(object sender, EventArgs e)
        {
            textBox1.SelectionLength = 0;
        }
    }
}
