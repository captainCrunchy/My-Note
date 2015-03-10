using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
/*
 * This is one of several 'partial' classes of the MainForm class. It is responsible
 * for performing a specific task. Below are the related files of the MainForm class:
 * 
 * mainForm.cs - This file is the starting point of the MainForm class. It contains the
 *               constructor and is responsible for coordinating interactions between
 *               other parts of the class and the application
 *               
 * formMenuStrip.cs - This file handles events that are triggered by elements
 *                    of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 * formToolbar.cs - This file is responsible for controls in the toolbar and
 *                  their events in the main form. (Ex: Font, Text, Color, Line...)
 * 
 * formTextbox.cs - (You are here) This file is responsible for appearance and events
 *                  of the richTextBox and its layers
 */
namespace My_Note
{
    public partial class MainForm : Form
    {
        #region richTextBoxMethods
        private void richTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox.TextLength == richTextBox.MaxLength)
            {
                MessageBox.Show("Current page is at maximum limit of characters",
                    "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
        }
        #endregion

        #region transparentPanelMethods
        /* When textControl is selected, this method will pass down the click coordinates
         * from transparent panel on top to the richTextBox below so that the cursor is positioned
         * accordingly. Coordinate modifications were created because transparentPanel and
         * richTextBox do not have the same origin. 12:47pm 3/7/15
         */
        private void transparentPanel_Click(object sender, System.EventArgs e)
        {
            logTextBox.Text = "transparentPanel clicked";

            // If currently in text editing mode
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                int charCount = richTextBox.TextLength;
                logTextBox.Text = "charCount = " + charCount;
                //logTextBox.Text = "transparentPanel clicked in text mode";
                var mouseEventArgs = e as MouseEventArgs;
                //if (mouseEventArgs != null)
                //{
                //    logTextBox.Text = "X = " + mouseEventArgs.X
                //    + "\r\nY = " + mouseEventArgs.Y;
                //}
                Point newPoint = new Point(mouseEventArgs.X - 40, mouseEventArgs.Y - 35);
                int charIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                richTextBox.SelectionStart = charIndex;
                richTextBox.Select();
            }
            else
            {
                //logTextBox.Text = "transparentPanel clicked NOT in text mode";
            }

        }
        #endregion
    }
}

/* notes
 * 
 * Microsoft Sans Serif 12pt - line height = 20; max size = 1866 (using lorem ipsum)
 * 
 */
