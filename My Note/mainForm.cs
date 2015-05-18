using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//  For readability: (Ctrl + m, Ctrl + o) to 'collapse', (Ctrl + m, Ctrl + l) to 'expand' definitions

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
 *      mainForm.cs - (YOU ARE HERE) This file is the starting point of the MainForm class. It
 *                    contains the constructor and is responsible for coordinating interactions
 *                    between other parts of the class and the application.
 *               
 *      formMenuStrip.cs - This file handles events that are triggered by elements
 *                         of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 *      formToolbar.cs - This file is responsible for controls in the toolbar and
 *                       their events in the main form. (Ex: Font, Text, Color, Line...)
 *                  
 *      formTextbox.cs - This file is responsible for appearance and events of the richTextBox and its
 *                       layers. Variables were created and initialized immediately in the declaration
 *                       section for reusability, to avoid repetition of creation in order to increase
 *                       drawing performance. Some variables are initialized in the main constructor.
 *                       Other components have been separated into regions each with appropriate comments.
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        private e_SelectedControl m_currentSelectedControl;     // Used to indicate the type of control the user selected
        private Color m_selectedControlButtonColor;             // Used to indicate the current color to be used by a control
        
        // The types of text and drawing controls available to the user
        private enum e_SelectedControl
        {
            TEXT, PENCIL, ERASER, WARROW, NWARROW, NARROW,
            NEARROW, EARROW, SEARROW, SARROW, SWARROW,
            RECTANGLE, ELLIPSE, SOLID, DASHED, DOTTED, VERTTEXT
        }

        public MainForm()
        {
            InitializeComponent();
            m_currentSelectedControl = e_SelectedControl.TEXT;
            m_selectedControlButtonColor = Color.SandyBrown;
            textSelectButton.BackColor = m_selectedControlButtonColor;
            fontComboBox.Text = "Microsoft Sans Serif";
            richTextBox.Font = new Font("Microsoft Sans Serif", 12);
            m_transparentPanelGraphics = this.transparentPanel.CreateGraphics();
            m_transparentPanelPen = new Pen(m_currentDrawColor);
        }

        /*
         * NAME
         *  private void MainForm_Load() - prepares elements before the form is shown on screen
         *  
         * SYNOPSIS
         *  private void MainForm_Load(object sender, System.EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called before elements are shown on screen. It is used to update and
         *  assign properties to elements before they are shown on screen. This is done here because
         *  it could not be accomplished in the constructor.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:28am 3/17/2015
         */
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            textSelectButton.Select();
        } /* private void MainForm_Load(object sender, System.EventArgs e) */
        
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

        // Temp (Begin)
        private void moveLogCursor()
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.SelectionLength = 0;
            logTextBox.ScrollToCaret();
        }
        private void mslog(string a_str)
        {
            logTextBox.Text += a_str + "\r\n";
            moveLogCursor();
        }
        private void clearLogButton_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
        }

        // Temp (End)
    }
}
/*private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (m_currentSelectedControl == e_SelectedControl.TEXT)
                {
                    Point newPoint = new Point(e.X - 40, e.Y - 35); // This is because text box is a little bit offset
                    m_richTextBoxSelectStartCharIndex = richTextBox.GetCharIndexFromPosition(newPoint);

                    //int charIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                    //richTextBox.SelectionStart = charIndex;
                    //m_richTextBoxSelectStartCharIndex = charIndex;
                    //mslog("SelectionStart = " + richTextBox.SelectionStart);
                }



                if (m_currentSelectedControl == e_SelectedControl.PENCIL)
                {
                    m_isDrawing = true;
                    m_shapeNumber++;
                    m_lastPosition = new Point(0, 0);
                }
                if (m_currentSelectedControl == e_SelectedControl.ERASER)
                {
                    m_isErasing = true;
                }

                // below can all be combined with a bunch of or statements
                // or it can be done to see if the selected control is within a certain value range

                // Technique is used to save over 50 lines of code
                if ((int)m_currentSelectedControl < 8)
                {

                }
                if (m_currentSelectedControl == e_SelectedControl.WARROW)
                {
                    mslog("selected warrow = " + (int)e_SelectedControl.WARROW);
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.NWARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.NARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.NEARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.EARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.SEARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.SARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.SWARROW)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.RECTANGLE)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.ELLIPSE)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.SOLID)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }
                if (m_currentSelectedControl == e_SelectedControl.DASHED)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                    m_canDash = true;
                }
                if (m_currentSelectedControl == e_SelectedControl.DOTTED)
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                    m_canDash = true;
                }
            }
        } /* private void transparentPanel_MouseDown(object sender, MouseEventArgs e) */

/*private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_currentSelectedControl == e_SelectedControl.TEXT)
                {
                    m_isSelectingText = true;
                    Point newPoint = new Point(e.X - 40, e.Y - 35);
                    m_richTextBoxSelectCurrentCharIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                    // can't select going backwards
                    richTextBox.SelectionStart = Math.Min(m_richTextBoxSelectStartCharIndex, m_richTextBoxSelectCurrentCharIndex);
                    richTextBox.SelectionLength = Math.Abs(m_richTextBoxSelectCurrentCharIndex - m_richTextBoxSelectStartCharIndex);
                    richTextBox.Select();
                }

                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.PENCIL))
                {
                    drawWithPencil(e);
                }
                if (m_isErasing && (m_currentSelectedControl == e_SelectedControl.ERASER))
                {
                    startErasing(e);
                }

                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.WARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X))
                    {
                        drawWestArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NWARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthWestArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NEARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthEastArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.EARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X))
                    {
                        drawEastArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SEARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthEastArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthArrow(e);
                    }
                }
                if (m_isDrawing & (m_currentSelectedControl == e_SelectedControl.SWARROW))
                {
                    if ((m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthWestArrow(e);
                    }
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.RECTANGLE))
                {
                    drawRectangle(e);
                }

                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.ELLIPSE))
                {
                    drawEllipse(e);
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SOLID))
                {
                    drawSolidLine(e);
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.DASHED))
                {
                    drawDashedLine(e);
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.DOTTED))
                {
                    drawDottedLine(e);
                }
            }
        } /* private void transparentPanel_MouseMove(object sender, MouseEventArgs e) */