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
 *      This class is the main form, it is the starting point of the application, and it is always
 *      visible. It provides common controls such as 'File' and 'Help' menu options, text and draw
 *      controls, and a 'combined' panel for text editing and drawing.
 *      
 *  CODE STRUCTURE:
 *      This class is divided into several files, which are all responsible for performing a specific
 *      task. The files are simply extensions of this class, i.e. '... partial class...'. Below is a
 *      description of each 'partial class' and its purpose.
 * 
 *      mainForm.cs - This file is the starting point of the MainForm class. It contains the
 *                    constructor and is responsible for coordinating interactions between
 *                    other parts of the class and the application.
 *               
 *      formMenuStrip.cs - This file handles events that are triggered by elements
 *                         of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 *      formToolbar.cs - (YOU ARE HERE) This file is responsible for controls in the toolbar and their
 *                       events in the main form. (Ex: Font, Text, Color, Line...). Due to the amount of
 *                       event methods and simplicity of their functionality, not all methods are fully
 *                       commented.
 *                  
 *      formTextbox.cs - This file is responsible for appearance and events of the richTextBox and its
 *                       layers. Variables were created and initialized immediately in the declaration
 *                       section for reusability, to avoid repetition of creation in order to increase
 *                       drawing performance. Some variables are initialized in the main constructor.
 *                       Other components have been separated into regions each with appropriate comments.
 */

/*  TODO: update CODE STRUCTURE comments in this class and in other partial classes to match this class
 *  
 *  Added: fontComboBox_SelectedIndexChaned() at the bottom, if this is not needed then handle it appropriately
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        private bool canHideVertTextButtons = true;    /* Used to assist in hiding the buttons for VerticalText
                                                           object when its control is not selected */

        // This region contains methods that update UI when new control is selected
        #region Control Selection Changed

        /*  This method sets the current control to text editing. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  12:12pm 3/17/2015
         */
        private void textSelectButton_Click(object sender, EventArgs e)
        {
            selectTextControl();
            //transparentPanel.Cursor = Cursors.IBeam;
            //m_currentSelectedControl = e_SelectedControl.TEXT;
            //setDefaultBackColorForControls();
            //textSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to pencil editing. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  12:13pm 3/17/2015
         */
        private void pencilSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.PENCIL;
            setDefaultBackColorForControls();
            pencilSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to eraser. Updates the panel's cursor
         *  to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  12:14pm 3/17/2015
         */
        private void eraserSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Hand;
            m_currentSelectedControl = e_SelectedControl.ERASER;
            setDefaultBackColorForControls();
            eraserSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw west arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  
         *  12:15pm 3/17/2015
         */
        private void WarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.WARROW;
            setDefaultBackColorForControls();
            WarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw north west arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:27am 3/19/2015
         */
        private void NWarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NWARROW;
            setDefaultBackColorForControls();
            NWarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw north arrow. Updates the panel's
         *  cursor match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:28am 3/19/2015
         */
        private void NarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NARROW;
            setDefaultBackColorForControls();
            NarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw north east arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:29am 3/19/2015
         */
        private void NEarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.NEARROW;
            setDefaultBackColorForControls();
            NEarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw east arrow. Updates the panel's
         *  cursor match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:30am 3/19/2015
         */
        private void EarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.EARROW;
            setDefaultBackColorForControls();
            EarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw south east arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:31am 3/19/2015
         */
        private void SEarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SEARROW;
            setDefaultBackColorForControls();
            SEarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw south arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:32am 3/19/2015
         */
        private void SarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SARROW;
            setDefaultBackColorForControls();
            SarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw south west arrow. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:33am 3/19/2015
         */
        private void SWarrowButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SWARROW;
            setDefaultBackColorForControls();
            SWarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw rectangle. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:34am 3/19/2015
         */
        private void rectangleSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.RECTANGLE;
            setDefaultBackColorForControls();
            rectangleSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw ellipse. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:35am 3/19/2015
         */
        private void ovalSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.ELLIPSE;
            setDefaultBackColorForControls();
            ovalSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw solid line. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  11:23am 3/18/2015
         */
        private void solidSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.SOLID;
            setDefaultBackColorForControls();
            solidSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw dashed line. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:36am 3/19/2015
         */
        private void dashedSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.DASHED;
            setDefaultBackColorForControls();
            dashedSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw dotted line. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls.
         *  
         *  Murat Zazi
         *  7:37am 3/19/2015
         */
        private void dottedSelectButton_Click(object sender, EventArgs e)
        {
            transparentPanel.Cursor = Cursors.Cross;
            m_currentSelectedControl = e_SelectedControl.DOTTED;
            setDefaultBackColorForControls();
            dottedSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*  This method sets the current control to draw rotatable text. Updates the panel's
         *  cursor to match the current control. Updates selection colors for controls. Sets
         *  the buttons of each VerticalText object on the form to 'Visible'.
         *  
         *  Murat Zazi
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
        }

        /*  This method brings up a color dialog box to select a color for current drawing tools
         * 
         *  Murat Zazi
         *  10:54am 3/1/2015 
         */
        private void drawColorButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = drawColorDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // Apply new color
                m_currentDrawColor = drawColorDialog.Color;
                m_transparentPanelPen.Color = m_currentDrawColor;
                drawColorButton.BackColor = m_currentDrawColor;
            }
        }

        /*  Set current drawing color to black and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:13am 3/17/2015
         */
        private void blackColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blackColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to white and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         *  
         *  Murat Zazi
         *  11:14am 3/17/2015
         */
        private void whiteColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = whiteColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to gray and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         *  
         *  Murat Zazi
         *  11:15am 3/17/2015
         */
        private void grayColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = grayColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to greent and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:16am 3/17/2015
         */
        private void greenColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = greenColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to red and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:17am 3/17/2015
         */
        private void redColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = redColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to teal and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         *  
         *  Murat Zazi
         *  11:18am 3/17/2015
         */
        private void tealColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = tealColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to orange and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:19am 3/17/2015
         */
        private void orangeColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = orangeColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to blue and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:20am 3/17/2015
         */
        private void blueColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blueColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to yellow and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:21am 3/17/2015
         */
        private void yellowColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = yellowColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*  Set current drawing color to purple and update it in the drawColorButton.BackColor
         *  to let the user know which color they selected.
         * 
         *  Murat Zazi
         *  11:22am 3/17/2015
         */
        private void purpleColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = purpleColorButton.BackColor;
            m_transparentPanelPen.Color = m_currentDrawColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

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
         *  This method gets called upon selecting a choice of font style from a combo box. This will
         *  only happen before the user types anything in the text box. This is because each font has
         *  specific line spacing assigned to it. Furthermore, text has to stay coordinated with any
         *  drawing on the panel. Modifying text font will change its size and spacing relative to any
         *  drawing. After the user makes initial modification to the text box or the panel, the combo
         *  box will be disabled and this method will no longer be called.
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
                    richTextBox.Font = new Font("Calibri", 12);
                    break;
                case 1:
                    richTextBox.Font = new Font("Consolas", 12);
                    break;
                case 2:
                    richTextBox.Font = new Font("Microsoft Sans Serif", 12);
                    break;
                case 3:
                    richTextBox.SelectionFont = new Font("Times New Roman", 12);
                    break;
            }
        } /* private void fontComboBox_SelectedIndexChanged(object sender, System.EventArgs e) */

        #endregion

        // This region contains 'helper' methods
        #region Helper Methods

        /*  This method gets called to reset the BackColor of all controls to default and set current
         *  control's BackColor to a color to indicate 'selected'. If vertTextButton was the last
         *  control selected, then it hides the buttons associated with all instances of VerticalText.
         *  
         *  Murat Zazi
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
        }
        /*  Called here to respond to UI event handler. Also selects text control on startup and
         *  when changing pages and subjects.
         * 
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
        }

        #endregion
    }
}