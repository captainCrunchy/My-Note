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
 *      VertTextOptionsForm : Form
 *      
 *  DESCRIPTION:
 *      This class creates a form which allows the user to select and set different properties for the
 *      VerticalText object. These properties include the actual text, text font, text font size, and
 *      text color. An instace of this class lives inside each VerticalText object and is triggered upon
 *      clicking the options button. Current values are passed to this object and are later returned to
 *      the calling VerticalText object. This object also uses a custom ComboBox in addition to regular
 *      ComboBox(es).
 * 
 *  CODE STRUCTURE:
 *      In order: member variables, load/initializer methods, private methods, public methods.
 *      Event handlers for UI objects correspond to the layout of the UI objects in the form.
 */

namespace My_Note
{
    public partial class VertTextOptionsForm : Form
    {
        
        private string m_oldText;                       // Used to get text of VerticalText object
        private string m_newText;                       // Used to set text of VerticalText object
        private string m_oldFontStyle;                  // Used to get font of VerticalText object
        private string m_newFontStyle;                  // Used to set font of VerticalText object
        private float m_oldFontSize = 12;               // Used to get font size of VerticalText object
        private float m_newFontSize;                    // Used to set font size of VerticalText object
        private Color m_oldTextColor = Color.Black;     // Used to get text color of VerticalText object
        private Color m_newTextColor;                   // Used to set text color of VerticalText object

        // Used to specify how the form is being closed in order to properly apply changes
        private enum e_selectedCloseButton { OKBUTTON, CANCELBUTTON }
        private e_selectedCloseButton m_selectedCloseButton = e_selectedCloseButton.CANCELBUTTON;

        /*
         * NAME
         *  VertTextOptionsForm() - default constructor
         *  
         * SYNOPSIS
         *  public VertTextOptionsForm();
         * 
         * DESCRIPTION
         *  This is the default consructor
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:48am 5/14/2015
         */
        public VertTextOptionsForm()
        {
            InitializeComponent();
        } /* public VertTextOptionsForm() */

        /*
         * NAME
         *  VertTextOptionsForm_Load() - gets called when this form loads
         *  
         * SYNOPSIS
         *  private void VertTextOptionsForm_Load(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler gets called when this form loads. It sets default values for a custom ComboBox
         *  control called "FontColorComboBox". This custom control is an instance of VTextColorComboBox.cs class
         *  which is designed to accomodate a selection of text color by using a ComboBox. Default values are set
         *  in order to prevent an exception to be thrown at load time, they are later dynamically updated.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:54am 5/14/2015
         */
        private void VertTextOptionsForm_Load(object sender, EventArgs e)
        {
            FontColorComboBox.AddStandardColors();
            FontColorComboBox.SelectedValue = m_oldTextColor;
        } /* private void VertTextOptionsForm_Load(object sender, EventArgs e) */
        
        /*
         * NAME
         *  VertTextOptionsForm_Closing() - gets called when this form is closing
         *  
         * SYNOPSIS
         *  private void VertTextOptionsForm_FormClosing(object sender, FormClosingEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler gets called when this form is closing. It checks to see how the form was closed:
         *  i.e. by clicking 'OK', 'Cancel', 'X', 'Alt + F4', etc... and updates/saves changes appropriately. 'OK'
         *  button click updates the text for VerticalText based on contents of RichTextBox. Other attributes are
         *  assigned as the user makes his selection using one of the ComboBox(s).
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:36am 5/14/2015
         */
        private void VertTextOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (m_selectedCloseButton == e_selectedCloseButton.OKBUTTON)
            {
                m_newText = TextRichTextBox.Text;
            }
            else
            {
                m_newText = m_oldText;
                m_newFontStyle = m_oldFontStyle;
                m_newFontSize = m_oldFontSize;
                m_newTextColor = m_oldTextColor;
            }
        } /* private void VertTextOptionsForm_FormClosing(object sender, FormClosingEventArgs e) */

        /*
         * NAME
         *  FontStyleComboBox_SelectedIndexChanged() - gets called when the font ComboBox value is changed
         *  
         * SYNOPSIS
         *  private void FontStyleComboBox_SelectedIndexChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler gets called when the value is changed in the ComboBox that changes the font.
         *  It updates the newly selected font to be passed back to the VertText.cs object as well as to
         *  diplay it to the user in the 'local' RichTextBox.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:36am 5/15/2015
         */
        private void FontStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selectedObject = FontStyleComboBox.SelectedItem;
            m_newFontStyle = selectedObject.ToString();
            TextRichTextBox.Font = new Font(m_newFontStyle, m_newFontSize);
        } /* private void FontStyleComboBox_SelectedIndexChanged(object sender, EventArgs e) */

        /*
         * NAME
         *  FontSizeComboBox_SelectedIndexChanged() - gets called when the font size ComboBox value is changed
         *  
         * SYNOPSIS
         *  private void FontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler gets called when ththe value is changed in the ComboBox that changes the font
         *  size. It updates the newly selected fot to be passed back to the VerticalText object as well as
         *  to display it to the user in the 'local' RichTextBox.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:27am 5/15/2015
         */
        private void FontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selectedObject = FontSizeComboBox.SelectedItem;
            m_newFontSize = Convert.ToSingle(selectedObject);
            TextRichTextBox.Font = new Font(m_newFontStyle, m_newFontSize);
        } /* private void FontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e) */

        /*
         * NAME
         *  FontColorComboBox_SelectedIndexChanged() - gets called when the font color ComboBox value is changed
         *  
         * SYNOPSIS
         *  private void FontColorComboBox_SelectedIndexChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler gets called when the value is changed in the ComboBox that changes font
         *  color. It updates the newly selected color to be passed back to the VerticalText object as
         *  well as to display it to the user in the 'local' RichTextBox.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:22am 5/15/2015
         */
        private void FontColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_newTextColor = FontColorComboBox.SelectedValue;
            TextRichTextBox.ForeColor = m_newTextColor;
        } /* private void FontColorComboBox_SelectedIndexChanged(object sender, EventArgs e) */

        /*
         * NAME
         *  TextRichTextBox_TextChanged() - detects when text is changed
         *  
         * SYNOPSIS
         *  private void TextRichTextBox_TextChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method checks to see that there is at least one character in the RichTextBox before allowing
         *  the user to make any changes to the VerticalText object by enabling or disabling 'OK' button.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:04pm 5/17/2015
         */
        private void TextRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextRichTextBox.Text.Length == 0)
            {
                OKOptionsButton.Enabled = false;
            }
            else
            {
                OKOptionsButton.Enabled = true;
            }
        } /* private void TextRichTextBox_TextChanged(object sender, EventArgs e) */

        /*
         * NAME
         *  OKOptionsButton_Click() - gets called upon OK button click
         *  
         * SYNOPSIS
         *  private void OKOptionsButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> checks to see if the left mouse button was clicked
         * 
         * DESCRIPTION
         *  This event handler gets called when the 'OK' button is clicked. It updates the enum property in order
         *  to properly save changes; it also closes this form which triggers this form's _FormClosing event handler.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:10am 5/15/2015
         */
        private void OKOptionsButton_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                m_selectedCloseButton = e_selectedCloseButton.OKBUTTON;
                this.Close();
            }
        } /* private void OKOptionsButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  CancelOptionsButton_Click() - gets called upon Cancel button click
         *  
         * SYNOPSIS
         *  private void CancelOptionsButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> checks to see if the left mouse button was clicked
         * 
         * DESCRIPTION
         *  This event handler gets called when the 'Cancel' button is clicked. It updates the enum property in order
         *   to properly save changes; it also closes this form which triggers this form's _FormClosing event handler.
         *   Functionally, using 'Cancel' button to close this form does the same thing as clicking the 'X' or using
         *   'Alt + F4'; its real purpose is to give the user a sense of control when using this form.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:17am 5/15/2015
         */
        private void CancelOptionsButton_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                m_selectedCloseButton = e_selectedCloseButton.CANCELBUTTON;
                this.Close();
            }
        } /* private void CancelOptionsButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  public void CaptureUIAttributes() - gets current attributes of the VerticalText object
         *  
         * SYNOPSIS
         *  public void CaptureUIAttributes(Font a_font, string a_text, SolidBrush a_brush);
         *      a_font  -> used to save the current font and size of the VerticalText object
         *      a_text  -> used to save the current text of the VerticalText object
         *      a_brush -> used to save the current color of the VerticalText object
         * 
         * DESCRIPTION
         *  This method gets called before this form is displayed to the user. It takes current values of the
         *  VerticalText object and uses them to update the UI objects of this form and saves values for later use.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:26pm 5/14/2015
         */
        public void CaptureUIAttributes(Font a_font, string a_text, SolidBrush a_brush)
        {
            m_oldText = m_newText = a_text;
            m_oldFontStyle = m_newFontStyle = a_font.Name;
            m_oldFontSize = m_newFontSize = a_font.Size;
            m_oldTextColor = m_newTextColor = a_brush.Color;

            FontStyleComboBox.Text = m_oldFontStyle;
            FontSizeComboBox.Text = Convert.ToString(m_oldFontSize);
            FontColorComboBox.SelectedValue = m_oldTextColor;
            TextRichTextBox.Text = m_oldText;
            TextRichTextBox.Font = a_font;
        } /* public void CaptureUIAttributes(Font a_font, string a_text, SolidBrush a_brush) */

        /*
         * NAME
         *  UpdateUIAttributes() - sets the values of VerticalText object to the ones used in this form
         *  
         * SYNOPSIS
         *  public void UpdateUIAttributes(ref Font a_font, ref string a_text, ref SolidBrush a_brush);
         *      a_font  -> used to update the current font of the VerticalText object
         *      a_text  -> updates the current text of the VerticalText object
         *      a_brush -> updates the current color of the VerticalText object
         * 
         * DESCRIPTION
         *  This method gets called from VerticalText object in order to update the newly selected values,
         *  which are assigned through the arguments passed by reference.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:43am 5/15/2015
         */
        public void UpdateUIAttributes(ref Font a_font, ref string a_text, ref SolidBrush a_brush)
        {
            a_font = new Font(m_newFontStyle, m_newFontSize);
            a_text = m_newText;
            a_brush.Color = m_newTextColor;
        } /* public void UpdateUIAttributes(ref Font a_font, ref string a_text, ref SolidBrush a_brush) */
    }
}