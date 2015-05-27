using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* 
 *  TITLE:
 *      RenameSubjectForm : Form
 *      
 *  DESCRIPTION:
 *      This class is used when assigning or reassigning a new title to a subject. Its member variables are used to enhance
 *      functionality and a more appealing appearance of this form to the user. The m_subjectTitle string variable is used to
 *      store the title of the current subject being worked on and becomes a placeholder in the text box. The m_formTitle string
 *      variable is used to indicate the current operation performed, for example, when changing a current subject title or adding
 *      a new subject title. The m_invalidTitles container holds a list of invalid titles that the caller/owner of this object
 *      needs to compare against when assigning a new subject title.
 *      
 *  CODE STRUCTURE:
 *      Code structure in this file follows a simple order: member variables, properties, constructor/load methods, event handlers
 *      ordered based on their layout in the form immediately followed by each event handler's 'helper' methods. Comments for these
 *      methods do not follow the same coding standards used by the rest of the files in this application. This is because these
 *      methods perform very simple tasks. Additional commenting will greatly increase the amount of lines of code, which will lead
 *      to a less readable and a more confusing than a beneficial code structure.
 */

namespace My_Note
{
    public partial class RenameSubjectForm : Form
    {
        private string m_formTitle = "";                            // Stores form title passed by caller object
        private string m_subjectTitle = "";                         // Stores subject title passed by caller object
        private List<string> m_invalidTitles = new List<string>();  // Stores a list of invalid title strings

        public string FormTitle
        {
            get
            {
                return m_formTitle;
            }
            set
            {
                m_formTitle = value;
            }
        }

        public string SubjectTitle
        {
            get
            {
                return m_subjectTitle;
            }
            set
            {
                m_subjectTitle = value;
            }
        }

        public List<string> InvalidTitles
        {
            get
            {
                return m_invalidTitles;
            }
            set
            {
                m_invalidTitles = value;
            }
        }

        /*  This method is the default constructor and gets called to construct this object.
         *  
         *  Murat Zazi
         *  2:07pm 5/21/2015
         */
        public RenameSubjectForm()
        {
            InitializeComponent();
        }

        /*  This method gets called when the form is loading and is about to be presented to the user.
         *  It updates UI values of this form based on the values assigned by the caller. This is done
         *  to present to the user a more appealing functionality and user interface.
         *  
         *  Murat Zazi
         *  2:27pm 5/21/2015
         */
        private void RenameSubjectForm_Load(object sender, EventArgs e)
        {
            renameTextBox.Text = SubjectTitle;
            this.Text = FormTitle;
            checkValidTitle();
        }

        /*  This method gets called as the user enters characters into the text box. It checks for valid titles by
         *  calling another method, which enables or disables the 'OK' button.
         *  
         *  Murat Zazi
         *  3:13 5/21/2015
         */
        private void renameTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidTitle();
        }

        /*  This method gets called to validate and update UI elements based on the current or default titles.
         *  Invalid titles to compare against are populated in the InvalidTitles array by the calling object, and
         *  the default title is manually coded. If a valid title is found, then 'OK' button becomes enabled for
         *  the user to click and update changes.
         *  
         *  Murat Zazi
         *  3:02pm 5/21/2015
         */
        private void checkValidTitle()
        {
            foreach (string s in InvalidTitles)
            {
                if (s == renameTextBox.Text)
                {
                    renameOKButton.Enabled = false;
                    return;
                }
            }
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

        /*  This method gets called when the 'OK' button is clicked and uses a method to validate and
         *  update member variables, UI elements, and close this form.
         *  
         *  Murat Zazi
         *  2:12pm 5/21/2015
         */
        private void renameOKButton_Click(object sender, EventArgs e)
        {
            updateTitle();
        }

        /*  This method gets called when the 'Enter' key is pressed on the keyboard and uses a method to 
         *  validate and update member variables, UI elements, and close this form.
         *  
         *  Murat Zazi
         *  2:52pm 5/21/2015
         */
        private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateTitle();
            }
        }

        /*  This method gets called to validate and update member variables, UI elements, and close this
         *  form. It assigns DialogResult to 'OK' to accomodate this functionality when the user presses
         *  'Enter' on the keyboard.
         *  
         *  Murat Zazi
         *  3:00pm 5/21/2015
         */
        private void updateTitle()
        {
            this.DialogResult = DialogResult.OK;
            SubjectTitle = renameTextBox.Text;
            this.Close();
        }

        /*  This method gets called when the 'Cancel' button is clicked to close this form.
         * 
         *  Murat Zazi
         *  2:18pm 5/21/2015
         */
        private void renameCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}