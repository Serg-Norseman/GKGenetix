using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
