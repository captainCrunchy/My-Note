using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Note
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        /*
         *  8:19am 5/31/2015
         */
        private void HelpOKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
