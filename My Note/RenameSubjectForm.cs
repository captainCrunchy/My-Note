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
 *      variable is used to indicate which title is currently being changed. for example, when changing a current subject title 
 *      or adding a new subject title. The m_invalidTitles container holds a list of invalid titles that the caller/owner of this
 *      object needs to compare against when assigning a new subject title. Invalid titles are simply different versions of the
 *      current title, i.e. strings containing spacing or different capitalization but all the same character. 'New Subject' is
 *      also an invalid title.
 *      
 *  CODE STRUCTURE:
 *      Code structure in this file follows a simple order: member variables, properties, constructor/load methods, event handlers
 *      ordered based on their layout in the form immediately followed by each event handler's 'helper' methods. 
 */

namespace My_Note
{
    public partial class RenameSubjectForm : Form
    {
        private string m_formTitle = "";                            // Assigns form title based on caller-passed title
        private string m_subjectTitle = "";                         // Stores subject title passed by caller object
        private List<string> m_invalidTitles = new List<string>();  /* Stores a list of invalid title strings, gets
                                                                       assigned dynamically (current) subject title */

        // Assigns form title
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

        // Stores subject title
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

        // Stores a list of invalid title strings
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

        /*
         * NAME
         *  public RenameSubjectForm() - default constructor
         * 
         * SYNOPSIS
         *  public RenameSubjectForm();
         *      
         * DESCRIPTION
         *  This is the default constructor.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:07pm 5/21/2015
         */
        public RenameSubjectForm()
        {
            InitializeComponent();
        } /* public RenameSubjectForm() */

        /*
         * NAME
         *  private void RenameSubjectForm_Load() - loads values after they have been initialized by the constructor
         * 
         * SYNOPSIS
         *  private void RenameSubjectForm_Load(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This method gets called after the constructor has been called, when the form is loading and is about to be
         *  presented to the user. It updates UI values of this form based on the values assigned by the caller. This
         *  is done to present to the user a more appealing functionality and user interface.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:27pm 5/21/2015
         */
        private void RenameSubjectForm_Load(object sender, EventArgs e)
        {
            renameTextBox.Text = SubjectTitle;
            this.Text = FormTitle;
            checkValidTitle();
        } /* private void RenameSubjectForm_Load(object sender, EventArgs e) */

        /*
         * NAME
         *  private void renameTextBox_TextChanged() - checks the validity of the new user-entered title
         * 
         * SYNOPSIS
         *  private void renameTextBox_TextChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler method gets called as the user enters characters into the text box. It checks the validity
         *  of the new title that the user is entering with each keystroke and enables or disables the 'OK button based
         *  on the validity of the new title.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:13pm 5/21/2015
         */
        private void renameTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidTitle();
        } /* private void renameTextBox_TextChanged(object sender, EventArgs e) */

        /*
         * NAME
         *  private void checkValidTitle() - checks the validity of text
         * 
         * SYNOPSIS
         *  private void checkValidTitle();
         *      
         * DESCRIPTION
         *  This method gets called to validate and update UI elements based on the current or default titles.
         *  Invalid titles to compare against are populated in the InvalidTitles array by the calling object, and
         *  the default title ('New Subject') is manually coded. If a valid title is found, then 'OK' button 
         *  becomes enabled for the user to click on and update changes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
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
        } /* private void checkValidTitle() */

        /*
         * NAME
         *  private void renameOKButton_Click() - saves values and closes 'this' form
         * 
         * SYNOPSIS
         *  private void renameOKButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This method gets called when the 'OK' button is clicked and uses a method to validate and
         *  update member variables, UI elements, and close this form.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:12pm 5/21/2015
         */
        private void renameOKButton_Click(object sender, EventArgs e)
        {
            updateTitle();
        } /* private void renameOKButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  private void renameTextBox_KeyPress() - saves values and closes 'this' form
         * 
         * SYNOPSIS
         *  private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This method gets called when the 'Enter' key is pressed on the keyboard and uses a method to 
         *  validate and update member variables, UI elements, and close this form.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:52pm 5/21/2015
         */
        private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateTitle();
            }
        } /* private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e) */

        /*
         * NAME
         *  private void updateTitle() - updates values and closes 'this' form
         * 
         * SYNOPSIS
         *  private void updateTitle();
         *      
         * DESCRIPTION
         *  This method gets called to validate and update member variables, UI elements, and close this
         *  form. It assigns DialogResult = 'OK' to accomodate this functionality when the user presses
         *  'Enter' on the keyboard.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:00pm 5/21/2015
         */
        private void updateTitle()
        {
            this.DialogResult = DialogResult.OK;
            SubjectTitle = renameTextBox.Text;
            this.Close();
        } /* private void updateTitle() */

        /*  This method gets called when the 'Cancel' button is clicked to close this form.
         * 
         *  Murat Zazi
         *  2:18pm 5/21/2015
         */
        /*
         * NAME
         *  private void renameCancelButton_Click() - cancels operation and closes 'this' form
         * 
         * SYNOPSIS
         *  private void renameCancelButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This method gets called when the 'Cancel' button is clicked to dismiss changed and close this form.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:18pm 5/21/2015
         */
        private void renameCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        } /* private void renameCancelButton_Click(object sender, EventArgs e) */
    }
}