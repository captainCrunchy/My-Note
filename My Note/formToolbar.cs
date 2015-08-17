using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

/* 
 *  TITLE:
 *      MainForm : Form
 *      
 *  DESCRIPTION:
 *    MainForm class:
 *      This class represents the main form for the application. It is the starting point and is always visible. It provides
 *      common controls such as 'File' and 'Help' menu options, text editing and drawing controls, and a 'combined' panel
 *      for text editing and drawing shapes. This MainForm class is divided into four (.cs) files, which are simply extensions
 *      of this class; i.e. each is a 'public partial class MainForm : Form'. This was done to keep the code organized and
 *      readable. The user is interacting with some part of this class at all times.
 *    mainForm.cs:
 *      This file implements tasks that are responsible for starting and running the application. It performs general tasks
 *      like handling the user inteface elements of the form and communication with data persistence objects. It is also
 *      responsible for coordinating tasks between other 'partial class' files.
 *    formMenuStrip.cs:
 *      This file handles events that are triggered by elements of the menu strip in the form and their appearances based on
 *      current data. Example: File, Edit, ..., Help.
 *    formToolbar.cs: (YOU ARE HERE)
 *      This file is responsible for appearance of controls in the toolbar and their events. These controls trigger such tasks
 *      as text editing, drawing shapes, and erasing.
 *    formTextBox.cs:
 *      This file is responsible for appearances and events of the richTextBox and its layers. Such additional layers are
 *      transparent and background panels. Events handled in this files are tasks such as applying text editing and drawing
 *      shapes onto the panels, and erasing them based on currently selected controls and options. The mechanics of drawing
 *      certain shapes like arrows, rectangles, ovals, and lines have been separated into two categories. One category is
 *      while the user has the mouse down and is moving it, shapes are being drawn and displayed at optimal speed. Other category
 *      is when the user releases the mouse, shapes are saved using individual points and are redrawn so in the future; this is
 *      done in order to accomodate the erase functionality.
 *      
 *  CODE STRUCTURE:
 *    MainForm class:
 *      This class is divided into four (.cs) files based on functionality. Each is responsible for performing specific tasks
 *      based on the user interface elements and controls. Each (.cs) file declares and initializes member variables that are
 *      needed in that file. Some member variables can only be initialized in the constructor, which is in the mainForm.cs file.
 *    formToolbar.cs: (YOU ARE HERE)
 *      This file is organized by separating methods into regions based on their collective functinality. The following region
 *      names contain the organized code: Member Variables, Text Controls, Drawing Controls, and Drawing Color Controls. Some
 *      member variables need to be initialized in the constructor which is in the mainForm.cs file.
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        // Region contains member variables for this class
        #region Member Variables

        private bool m_textHighlightColorEnabled = false;                   // Is text highlighting currently enabled 
        private Color m_currentTextHighlightColor = SystemColors.Window;    // Current text highlight color
        private Color m_currentTextColor = SystemColors.WindowText;         // Current text color
        private bool m_textColorEnabled = false;                            // Is text color currenly enabled
        private bool m_boldTextEnabled = false;                             // Is bold text option currently enabled
        private bool m_italicTextEnabled = false;                           // Is italic text option currently enabled
        private bool m_underlineTextEnabled = false;                        // Is underline text option currently enabled
        private bool m_strikeoutTextEnabled = false;                        // Is strikeout text option currently enabled
        private bool canHideVertTextButtons = true;                         /* Used to assist in hiding buttons of 
                                                                               VerticalText when it is not selected */

        #endregion

        // Region contains text control and formatting methods
        #region Text Controls

        /*
         * NAME
         *  textSelectButton_Click() - selects text editing tool
         * 
         * SYNOPSIS
         *  private void textSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method calls another method that sets the current control to text editing. Reason for calling a different
         *  method is because that method is also called by other objects, hence this technique reduces extra lines of code.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:12pm 3/17/2015
         */
        private void textSelectButton_Click(object sender, EventArgs e)
        {
            selectTextControl();
        } /* private void textSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  fontComboBox_DropDown() - used to re-select text, if any is selected
         * 
         * SYNOPSIS
         *  private void fontComboBox_DropDown(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method is trigerred when the user drops down the menu of font combo box. It ensures
         *  that the selected text stays selected even when the focus shifts to the combo box. This gives the user
         *  the ability to continuosly apply new fonts to currently selected text and shows to the user which text
         *  they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:30am 6/11/2015
         */
        private void fontComboBox_DropDown(object sender, EventArgs e)
        {
            richTextBox.Select();
            transparentPanel.Refresh();
        } /* private void fontComboBox_DropDown(object sender, EventArgs e) */

        /*
         * NAME
         *  fontComboBox_SelectedIndexChanged() - changes the text font
         *  
         * SYNOPSIS
         *  private void fontComboBox_SelectedIndexChanged(object sender, System.EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called upon selecting a choice of font style from a combo box. If no text is selected, then
         *  this method simply sets the future text font and updates the title in the combo box. If some text is selected,
         *  then the font of the selected text is updated to the selected font and future text font is set. If selected
         *  text contains more than one type of font, then this feature does nothing.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:30am 3/17/2015
         */
        private void fontComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int selectedIndex = fontComboBox.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    m_currentRichTextBoxFont = new Font("Calibri", 12);
                    break;
                case 1:
                    m_currentRichTextBoxFont = new Font("Consolas", 12);
                    break;
                case 2:
                    m_currentRichTextBoxFont = new Font("Microsoft Sans Serif", 12);
                    break;
                case 3:
                    m_currentRichTextBoxFont = new Font("Times New Roman", 12);
                    break;
            }
            updateCurrentFontStyles();
            richTextBox.SelectionFont = m_currentRichTextBoxFont;
            richTextBox.Select();
        } /* private void fontComboBox_SelectedIndexChanged(object sender, System.EventArgs e) */

        /*
         * NAME
         *  boldButton_Click() - sets text to bold
         * 
         * SYNOPSIS
         *  private void boldButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method applies bold formatting option to text. If no text is selected, then this method
         *  allows bold format to be enabled or disabled as the user enters text and appropriately updates the selection
         *  color of the bold text button. If some text is selected, then only that text is set to bold, further bold
         *  format options are disabled, and the button is never highlighted. If selected text contains more than one
         *  type of font, then this feature does nothing.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:52am 5/31/2015
         */
        private void boldButton_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont == null) return;
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                Font selFont = richTextBox.SelectionFont;
                Font nextFont = new Font(selFont, selFont.Style ^ FontStyle.Bold);
                richTextBox.SelectionFont = nextFont;
            }
            else
            {
                if (m_boldTextEnabled == true)
                {
                    m_boldTextEnabled = false;
                    boldButton.BackColor = Color.Transparent;
                }
                else
                {
                    m_boldTextEnabled = true;
                    boldButton.BackColor = m_selectedControlButtonColor;
                }
                updateCurrentFontStyles();
            }
            richTextBox.Select();
        } /* private void boldButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  italicButton_Click() - sets text to italic
         * 
         * SYNOPSIS
         *  private void italicButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method applies italic formatting option to text. If no text is selected, then this method
         *  allows italic format to be enabled or disabled as the user enters text and appropriately updates the selection
         *  color of the italic text button. If some text is selected, then only that text is set to italic, further
         *  italic format options are disabled, and the button is never highlighted. If selected text contains more than
         *  one type of font, then this feature does nothing.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:54am 5/31/2015
         */
        private void italicButton_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont == null) return;
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                Font selFont = richTextBox.SelectionFont;
                Font nextFont = new Font(selFont, selFont.Style ^ FontStyle.Italic);
                richTextBox.SelectionFont = nextFont;
            }
            else
            {
                if (m_italicTextEnabled == true)
                {
                    m_italicTextEnabled = false;
                    italicButton.BackColor = Color.Transparent;
                }
                else
                {
                    m_italicTextEnabled = true;
                    italicButton.BackColor = m_selectedControlButtonColor;
                }
                updateCurrentFontStyles();
            }
            richTextBox.Select();
        } /* private void italicButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  underlineButton_Click() - sets text to underline
         * 
         * SYNOPSIS
         *  private void underlineButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method applies underline formatting option to text. If no text is selected, then this method
         *  allows underline format to be enabled or disabled as the user enters text and appropriately updates the selection
         *  color of the underline text button. If some text is selected, then only that text is set to underline, further 
         *  underline format options are disabled, and the button is never highlighted. If selected text contains more than
         *  one type of font, then this feature does nothing.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:57am 5/31/2015
         */
        private void underlineButton_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont == null) return;
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                Font selFont = richTextBox.SelectionFont;
                Font nextFont = new Font(selFont, selFont.Style ^ FontStyle.Underline);
                richTextBox.SelectionFont = nextFont;
            }
            else
            {
                if (m_underlineTextEnabled == true)
                {
                    m_underlineTextEnabled = false;
                    underlineButton.BackColor = Color.Transparent;
                }
                else
                {
                    m_underlineTextEnabled = true;
                    underlineButton.BackColor = m_selectedControlButtonColor;
                }
                updateCurrentFontStyles();
            }
            richTextBox.Select();
        } /* private void underlineButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  strikeoutButton_Click() - sets text to strikeout
         * 
         * SYNOPSIS
         *  private void strikeoutButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method applies strikeout formatting option to text. If no text is selected, then this method
         *  allows strikeout format to be enabled or disabled as the user enters text and appropriately updates the selection
         *  color of the strikeout text button. If some text is selected, then only that text is set to strikeout, further 
         *  strikeout format options are disabled, and the button is never highlighted. If selected text contains more than
         *  one type of font, then this feature does nothing.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:03pm 5/31/2015
         */
        private void strikeoutButton_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont == null) return;
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                Font selFont = richTextBox.SelectionFont;
                Font nextFont = new Font(selFont, selFont.Style ^ FontStyle.Strikeout);
                richTextBox.SelectionFont = nextFont;
            }
            else
            {
                if (m_strikeoutTextEnabled == true)
                {
                    m_strikeoutTextEnabled = false;
                    strikeoutButton.BackColor = Color.Transparent;
                }
                else
                {
                    m_strikeoutTextEnabled = true;
                    strikeoutButton.BackColor = m_selectedControlButtonColor;
                }
                updateCurrentFontStyles();
            }
            richTextBox.Select();
        } /* private void strikeoutButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  highlightColorButton_Click() - highlights the text
         * 
         * SYNOPSIS
         *  private void highlightColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method applies highlight formatting option to text. If no text is selected, then this method
         *  allows highlight format to be enabled or disabled as the user enters text and appropriately updates the selection
         *  color of the highlight text button. If some text is selected, then only that text is set to highlight, further 
         *  highlight format options are disabled, and the button is never highlighted. If selected text contains more than
         *  one type of font, then this feature still highlights the text to the set color.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:30pm 5/31/2015
         */
        private void highlightColorButton_Click(object sender, EventArgs e)
        {
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                // Change selection back color
                richTextBox.SelectionBackColor = changeHighlightColorButton.BackColor;

                // Place the cursor at the end of the selection
                richTextBox.SelectionStart = richTextBox.SelectionStart + textSelectionLength;
                richTextBox.SelectionLength = 0;
                richTextBox.Select();
            }
            else
            {
                if (m_textHighlightColorEnabled == true)
                {
                    m_textHighlightColorEnabled = false;
                    highlightColorButton.BackColor = Color.Transparent;
                    m_currentTextHighlightColor = SystemColors.Window;
                }
                else
                {
                    m_textHighlightColorEnabled = true;
                    highlightColorButton.BackColor = m_selectedControlButtonColor;
                    m_currentTextHighlightColor = changeHighlightColorButton.BackColor;
                }
                richTextBox.Select();  // Place the cursor back where it was
            }
        } /* private void highlightColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  changeHighlightColorButton_Click() - changes the highlighter color
         * 
         * SYNOPSIS
         *  private void changeHighlightColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method shows a color dialog box allowing the user to set the text highlight color. If no text
         *  is selected, then it simply sets the new color for future text to be typed. If some text is selected, then that
         *  text highlight color is changed whether it was previously highlighted or not. The new highlight text color is also
         *  updated for future text. If selected text contains more than one type of font, then all of the selected text will
         *  be set to the newly selected text highlight color.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  3:15pm 6/2/2015
         */
        private void changeHighlightColorButton_Click(object sender, EventArgs e)
        {
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)  // redraw lines only if necessary
            {
                transparentPanel.Refresh();
            }
            DialogResult highlightColorDialogResult = drawColorDialog.ShowDialog();
            if (highlightColorDialogResult == DialogResult.OK)
            {
                changeHighlightColorButton.BackColor = drawColorDialog.Color;
                if (richTextBox.SelectionLength > 0)
                {
                    richTextBox.SelectionBackColor = drawColorDialog.Color;
                    richTextBox.SelectionStart = richTextBox.SelectionStart + textSelectionLength;
                    richTextBox.SelectionLength = 0;
                }
            }
            richTextBox.Select();
            transparentPanel.Invalidate();
        } /* private void changeHighlightColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  textColorButton_Click() - changes color of selected text or enables custom color for future text
         * 
         * SYNOPSIS
         *  private void textColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method changes the color of text. If no text is selected, then this method allows the color
         *  to be enabled or disabled as the user enters text and appropriately updates the selection color of text color
         *  button. If some text is selected, then only that text color is changed, further text color option is disabled,
         *  and the button is never highlighted. If selected text contains more than one type of font, then this feature will
         *  change the color of all the selected text.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:47am 5/31/2015
         */
        private void textColorButton_Click(object sender, EventArgs e)
        {
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)
            {
                // Change selection color
                richTextBox.SelectionColor = changeTextColorButton.BackColor;

                // Place the cursor at the end of the selection
                richTextBox.SelectionStart = richTextBox.SelectionStart + textSelectionLength;
                richTextBox.SelectionLength = 0;
                richTextBox.Select();
            }
            else
            {
                if (m_textColorEnabled == true)
                {
                    m_textColorEnabled = false;
                    textColorButton.BackColor = Color.Transparent;
                    m_currentTextColor = SystemColors.WindowText;
                }
                else
                {
                    m_textColorEnabled = true;
                    textColorButton.BackColor = m_selectedControlButtonColor;
                    m_currentTextColor = changeTextColorButton.BackColor;
                }
                richTextBox.Select();  // Place the cursor back where it was
            }
        } /* private void textColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  changeTextColorButton_Click() - changes the color of text color option
         * 
         * SYNOPSIS
         *  private void changeTextColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler method shows a color dialog box allowing the user to set the color for text color option . If no
         *  text is selected, then it simply sets the new color for future text to be typed. If some text is selected, then that
         *  text color is changed. The new color of text color option is also updated. If selected text contains more than one
         *  type of font, then all of the selected text will be set to the newly selected text color.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  3:21pm 6/2/2015
         */
        private void changeTextColorButton_Click(object sender, EventArgs e)
        {
            int textSelectionLength = richTextBox.SelectionLength;
            if (textSelectionLength > 0)  // redraw lines only if necessary
            {
                transparentPanel.Refresh();
            }
            DialogResult textColorDialogResult = drawColorDialog.ShowDialog();
            if (textColorDialogResult == DialogResult.OK)
            {
                changeTextColorButton.BackColor = drawColorDialog.Color;
                if (richTextBox.SelectionLength > 0)
                {
                    richTextBox.SelectionColor = drawColorDialog.Color;
                    richTextBox.SelectionStart = richTextBox.SelectionStart + textSelectionLength;
                    richTextBox.SelectionLength = 0;
                }
            }
            richTextBox.Select();
            transparentPanel.Invalidate();
        } /* private void changeTextColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  updateCurrentFontStyles() - updates font styles when user selects new font style
         * 
         * SYNOPSIS
         *  private void updateCurrentFontStyles();
         * 
         * DESCRIPTION
         *  This method gets called by several other methods when the user is applying new font styles to
         *  either currently selected text or simply setting up for future text to be typed. More than one
         *  font style can be applied to the same text.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  5:08pm 6/2/2015
         */
        private void updateCurrentFontStyles()
        {
            string currentFontName = m_currentRichTextBoxFont.Name;
            float currentFontSize = m_currentRichTextBoxFont.Size;
            Font nextFont = new Font(currentFontName, currentFontSize);
            if (m_boldTextEnabled == true)
                nextFont = new Font(nextFont, nextFont.Style ^ FontStyle.Bold);
            if (m_italicTextEnabled == true)
                nextFont = new Font(nextFont, nextFont.Style ^ FontStyle.Italic);
            if (m_underlineTextEnabled == true)
                nextFont = new Font(nextFont, nextFont.Style ^ FontStyle.Underline);
            if (m_strikeoutTextEnabled == true)
                nextFont = new Font(nextFont, nextFont.Style ^ FontStyle.Strikeout);
            m_currentRichTextBoxFont = nextFont;
        } /* private void updateCurrentFontStyles() */

        /*
         * NAME
         *  private void updateUIForTextControls() - updates UI for text editing controls
         * 
         * SYNOPSIS
         *  updateUIForTextControls();
         * 
         * DESCRIPTION
         *  Updates UI for text controls when multi-text selection is made, or when selecting new control. When text control is
         *  selected or enabled, then user interface for text controls is updated based on current or text values. For example,
         *  Font and Font Style can only be applied to one word at a time, hence their buttons become either enabled or disabled.
         *  Empty spaces are never formatted to any font or font style because their font regulates line height. When selecting a
         *  control other than text editing control, then all text controls are disabled.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  4:51pm 6/10/2015
         */
        private void updateUIForTextControls()
        {
            // Currently in text editing mode
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                highlightColorButton.Enabled = true;
                textColorButton.Enabled = true;
                changeHighlightColorButton.Enabled = true;
                changeTextColorButton.Enabled = true;
                // If more than one font is detected within text selection
                if (richTextBox.SelectionFont == null)
                {
                    fontComboBox.Enabled = false;
                    boldButton.Enabled = false;
                    italicButton.Enabled = false;
                    underlineButton.Enabled = false;
                    strikeoutButton.Enabled = false;
                }
                // If empty space is found, then disable font/style and exit
                if (richTextBox.SelectionLength > 0)
                {
                    for (int i = richTextBox.SelectionStart; i < richTextBox.SelectionStart + richTextBox.SelectionLength; i++)
                    {
                        if (richTextBox.Text[i] == ' ')
                        {
                            fontComboBox.Enabled = false;
                            boldButton.Enabled = false;
                            italicButton.Enabled = false;
                            underlineButton.Enabled = false;
                            strikeoutButton.Enabled = false;
                            break;
                        }
                    }
                }
                // If only text characters are detected with one font/style, then enable all text UI controls
                else
                {
                    fontComboBox.Enabled = true;
                    boldButton.Enabled = true;
                    italicButton.Enabled = true;
                    underlineButton.Enabled = true;
                    strikeoutButton.Enabled = true;
                }
            }
            // Not in text editing mode or just leaving text editing mode
            else
            {
                fontComboBox.Enabled = false;
                boldButton.Enabled = false;
                italicButton.Enabled = false;
                underlineButton.Enabled = false;
                strikeoutButton.Enabled = false;
                highlightColorButton.Enabled = false;
                textColorButton.Enabled = false;
                changeHighlightColorButton.Enabled = false;
                changeTextColorButton.Enabled = false;
            }
            transparentPanel.Refresh();
        } /* private void updateUIForTextControls() */

        #endregion

        // Region contains drawing control methods
        #region Drawing Controls

        /*
         * NAME
         *  pencilSelectButton_Click() - selects pencil tool
         * 
         * SYNOPSIS
         *  private void pencilSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to pencil editing. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:13pm 3/17/2015
         */
        private void pencilSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.PENCIL;
            setDefaultBackColorForControls();
            pencilSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void pencilSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  eraserSelectButton_Click() - selects eraser tool
         * 
         * SYNOPSIS
         *  private void eraserSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to eraser. Updates the panel's mouse cursor
         *  to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:14pm 3/17/2015
         */
        private void eraserSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Hand;
            m_currentSelectedControl = e_SelectedControl.ERASER;
            setDefaultBackColorForControls();
            eraserSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void eraserSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  WarrowButton_Click() - selects west pointing arrow tool
         * 
         * SYNOPSIS
         *  private void WarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw west arrow. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  12:15pm 3/17/2015
         */
        private void WarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.WARROW;
            setDefaultBackColorForControls();
            WarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void WarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  NWarrowButton_Click() - selects northwest pointing arrow tool
         * 
         * SYNOPSIS
         *  private void NWarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing northwest. Updates the panel's
         *  mouse cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:27am 3/19/2015
         */
        private void NWarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NWARROW;
            setDefaultBackColorForControls();
            NWarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void NWarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  NarrowButton_Click() - selects north pointing arrow tool
         * 
         * SYNOPSIS
         *  private void NarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing north. Updates the panel's
         *  mouse cursor match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:28am 3/19/2015
         */
        private void NarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NARROW;
            setDefaultBackColorForControls();
            NarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void NarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  NEarrowButton_Click() - selects northeast pointing arrow tool
         * 
         * SYNOPSIS
         *  private void NEarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing northeast. Updates the panel's
         *  mouse cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:29am 3/19/2015
         */
        private void NEarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NEARROW;
            setDefaultBackColorForControls();
            NEarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void NEarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  EarrowButton_Click() - selects east pointing arrow tool
         * 
         * SYNOPSIS
         *  private void EarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing east. Updates the panel's
         *  mouse cursor match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:30am 3/19/2015
         */
        private void EarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.EARROW;
            setDefaultBackColorForControls();
            EarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void EarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  SEarrowButton_Click() - selects southeast pointing arrow tool
         * 
         * SYNOPSIS
         *  private void SEarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing southeast. Updates the panel's
         *  mouse cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:31am 3/19/2015
         */
        private void SEarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SEARROW;
            setDefaultBackColorForControls();
            SEarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void SEarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  SarrowButton_Click() - selects south pointing arrow tool
         * 
         * SYNOPSIS
         *  private void SarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing south. Updates the panel's
         *  mouse cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:32am 3/19/2015
         */
        private void SarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SARROW;
            setDefaultBackColorForControls();
            SarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void SarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  private void SWarrowButton_Click() - selects southwest pointing arrow tool
         * 
         * SYNOPSIS
         *  private void SWarrowButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw an arrow pointing southwest. Updates the panel's
         *  mouse cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:33am 3/19/2015
         */
        private void SWarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SWARROW;
            setDefaultBackColorForControls();
            SWarrowButton.BackColor = m_selectedControlButtonColor;
        } /* private void SWarrowButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  rectangleSelectButton_Click() - selects rectangle drawing tool
         * 
         * SYNOPSIS
         *  private void rectangleSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw rectangles. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:34am 3/19/2015
         */
        private void rectangleSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.RECTANGLE;
            setDefaultBackColorForControls();
            rectangleSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void rectangleSelectButton_Click(object sender, EventArgs e) */

        /*  This method sets the current control to draw ellipse. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:35am 3/19/2015
         */
        /*
         * NAME
         *  ovalSelectButton_Click() - selects oval drawing tool
         * 
         * SYNOPSIS
         *  private void ovalSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw ellipses. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:35am 3/19/2015
         */
        private void ovalSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.ELLIPSE;
            setDefaultBackColorForControls();
            ovalSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void ovalSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  solidSelectButton_Click() - selects solid line drawing tool
         * 
         * SYNOPSIS
         *  private void solidSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw solid line. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:23am 3/18/2015
         */
        private void solidSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SOLID;
            setDefaultBackColorForControls();
            solidSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void solidSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  dashedSelectButton_Click() - selects dashed line drawing tool
         * 
         * SYNOPSIS
         *  private void dashedSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw dashed line. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:36am 3/19/2015
         */
        private void dashedSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.DASHED;
            setDefaultBackColorForControls();
            dashedSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void dashedSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  dottedSelectButton_Click() - selects dotted line drawing tool
         * 
         * SYNOPSIS
         *  private void dottedSelectButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw dotted line. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:37am 3/19/2015
         */
        private void dottedSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.DOTTED;
            setDefaultBackColorForControls();
            dottedSelectButton.BackColor = m_selectedControlButtonColor;
        } /* private void dottedSelectButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  vertTextButton_Click() - selects the vertical text tool
         * 
         * SYNOPSIS
         *  private void vertTextButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method sets the current control to draw rotatable text. Updates the panel's mouse
         *  cursor to match the current control. Updates selection colors for controls. Sets the
         *  buttons of each VerticalText object on the form to 'Visible'.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:38am 3/19/2015
         */
        private void vertTextButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.VERTTEXT;
            setDefaultBackColorForControls();
            vertTextButton.BackColor = m_selectedControlButtonColor;
            canHideVertTextButtons = true;
            foreach (VerticalText v in m_verticalTextList)
            {
                v.showButtons();
            }
            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void vertTextButton_Click(object sender, EventArgs e) */

        #endregion

        // Region contains drawing color selection controls
        #region Drawing Color Controls

        /*
         * NAME
         *  drawColorButton_Click() - presents user with a custom color selection
         * 
         * SYNOPSIS
         *  private void drawColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method brings up a color dialog box to select a custom color for currently selected
         *  drawing tool.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  10:54am 3/1/2015
         */
        private void drawColorButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Refresh();
            DialogResult drawColorDialogResult = drawColorDialog.ShowDialog();
            if (drawColorDialogResult == DialogResult.OK)
            {
                // Apply new color
                m_currentDrawColor = drawColorDialog.Color;
                m_transparentPanelPen.Color = m_currentDrawColor;
                m_dashLinePen.Color = m_currentDrawColor;
                m_dottedLinePen.Color = m_currentDrawColor;
                drawColorButton.BackColor = m_currentDrawColor;
            }
        } /* private void drawColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  blackColorButton_Click() - sets current drawing color to black
         * 
         * SYNOPSIS
         *  private void blackColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to black and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:13am 3/17/2015
         */
        private void blackColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blackColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void blackColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  whiteColorButton_Click() - sets current drawing color to white
         * 
         * SYNOPSIS
         *  private void whiteColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to white and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:14am 3/17/2015
         */
        private void whiteColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = whiteColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void whiteColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  grayColorButton_Click() - sets current drawing color to gray
         * 
         * SYNOPSIS
         *  private void grayColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to gray and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:15am 3/17/2015
         */
        private void grayColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = grayColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void grayColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  greenColorButton_Click() - sets current drawing color to green
         * 
         * SYNOPSIS
         *  private void greenColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to greent and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:16am 3/17/2015
         */
        private void greenColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = greenColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void greenColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  redColorButton_Click() - sets current drawing color to red
         * 
         * SYNOPSIS
         *  private void redColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to red and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:17am 3/17/2015
         */
        private void redColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = redColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void redColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  tealColorButton_Click() - sets current drawing color to teal
         * 
         * SYNOPSIS
         *  private void tealColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:18am 3/17/2015
         */
        private void tealColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = tealColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void tealColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  orangeColorButton_Click() - sets current drawing color to orange
         * 
         * SYNOPSIS
         *  private void orangeColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to orange and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:19am 3/17/2015
         */
        private void orangeColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = orangeColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void orangeColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  blueColorButton_Click() - sets current drawing color to blue
         * 
         * SYNOPSIS
         *  private void blueColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to blue and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:20am 3/17/2015
         */
        private void blueColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blueColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void blueColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  yellowColorButton_Click() - sets current drawing color to yellow
         * 
         * SYNOPSIS
         *  private void yellowColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to yellow and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:21am 3/17/2015
         */
        private void yellowColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = yellowColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void yellowColorButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  purpleColorButton_Click() - sets current drawing color to purple
         * 
         * SYNOPSIS
         *  private void purpleColorButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Set current drawing color to purple and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  11:22am 3/17/2015
         */
        private void purpleColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = purpleColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            m_dashLinePen.Color = m_currentDrawColor;
            m_dottedLinePen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
            transparentPanel.Refresh();
        } /* private void purpleColorButton_Click(object sender, EventArgs e) */

        #endregion

        // Region contains 'helper' methods
        #region Helper Methods

        /*
         * NAME
         *  setDefaultBackColorForControls() - updates current colors for all controls in toolbar
         * 
         * SYNOPSIS
         *  private void setDefaultBackColorForControls();
         * 
         * DESCRIPTION
         *  This method gets called to reset the BackColor of all controls to default and set current
         *  control's BackColor to a color to indicate 'selected'. If vertTextButton was the last
         *  control selected, then it hides the buttons associated with all instances of VerticalText.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  7:12pm 3/9/2015
         */
        private void setDefaultBackColorForControls()
        {
            textSelectButton.BackColor = Color.Transparent;
            pencilSelectButton.BackColor = Color.Transparent;
            eraserSelectButton.BackColor = Color.Transparent;
            NWarrowButton.BackColor = Color.Transparent;
            NarrowButton.BackColor = Color.Transparent;
            NEarrowButton.BackColor = Color.Transparent;
            EarrowButton.BackColor = Color.Transparent;
            SEarrowButton.BackColor = Color.Transparent;
            SarrowButton.BackColor = Color.Transparent;
            SWarrowButton.BackColor = Color.Transparent;
            WarrowButton.BackColor = Color.Transparent;
            rectangleSelectButton.BackColor = Color.Transparent;
            ovalSelectButton.BackColor = Color.Transparent;
            solidSelectButton.BackColor = Color.Transparent;
            dashedSelectButton.BackColor = Color.Transparent;
            dottedSelectButton.BackColor = Color.Transparent;
            vertTextButton.BackColor = Color.Transparent;
            if (canHideVertTextButtons)
            {
                foreach (VerticalText v in m_verticalTextList)
                {
                    v.hideButtons();
                }
                canHideVertTextButtons = false;
                
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
                backPanel.Invalidate();
            }
            if (m_currentSelectedControl != e_SelectedControl.TEXT)
            {
                richTextBox.SelectionLength = 0;
            }
            updateUIForTextControls();
        } /* private void setDefaultBackColorForControls() */

        /*
         * NAME
         *  selectTextControl() - selects text editing tool
         * 
         * SYNOPSIS
         *  private void selectTextControl();
         * 
         * DESCRIPTION
         *  This method sets the current control to 'text' edit control. It updates the mouse cursor to match one for text
         *  editing. It then sets richTextBox as the 'focus' of the user interface with the character index at the end of
         *  the text on the page. This method gets called upon changing editing control, 'flipping' pages, changing subjects,
         *  and during start of the application.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         * 
         * DATE
         *  2:53pm 5/25/2015
         */
        private void selectTextControl()
        {
            transparentPanel.Cursor = Cursors.IBeam;
            m_currentSelectedControl = e_SelectedControl.TEXT;
            setDefaultBackColorForControls();
            textSelectButton.BackColor = m_selectedControlButtonColor;

            int charIndex = richTextBox.Text.Length;
            richTextBox.SelectionStart = charIndex;
            richTextBox.SelectionLength = 0;
            richTextBox.Select();
        } /* private void selectTextControl() */

        #endregion
    }
}