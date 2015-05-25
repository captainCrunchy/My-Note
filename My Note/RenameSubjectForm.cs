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
    public partial class RenameSubjectForm : Form
    {
        public string SubjectTitle = "";
        public string FormTitle = "";
        public List<string> InvalidTitles = new List<string>();

        public RenameSubjectForm()
        {
            InitializeComponent();
        }

        /*  Small description is probably ok
         *  2:12pm 5/21/2015
         */
        private void renameOKButton_Click(object sender, EventArgs e)
        {
            updateTitle();
        }

        /*  Small description is probably ok
         *  2:18pm 5/21/2015
         */
        private void renameCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*  Small description is probably ok
         *  2:27pm 5/21/2015
         */
        private void RenameSubjectForm_Load(object sender, EventArgs e)
        {
            renameTextBox.Text = SubjectTitle;
            this.Text = FormTitle;
            checkValidTitle();
        }

        /*  Small description is probably ok
         *  2:52pm 5/21/2015
         */ 
        private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateTitle();
            }
        }

        /*
         *  3:00pm 5/21/2015
         */
        private void updateTitle()
        {
            this.DialogResult = DialogResult.OK;
            SubjectTitle = renameTextBox.Text;
            this.Close();
        }

        /*  Validate the new title
         *  Bigger description is probably ok
         * 3:02pm 5/21/2015
         */
        private void checkValidTitle()
        {
            // to prevent same title subjects
            foreach (string s in InvalidTitles)
            {
                if (s == renameTextBox.Text)
                {
                    renameOKButton.Enabled = false;
                    return;
                }
            }
            // to prevent default name for a real subject title
            if (renameTextBox.Text == "New Subject" ||
                String.IsNullOrWhiteSpace(renameTextBox.Text))
            {
                renameOKButton.Enabled = false;
            }
            else
            {
                renameOKButton.Enabled = true;
            }
        }

        /*
         *  3:13 5/21/2015
         */
        private void renameTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidTitle();
        }
    }
}
