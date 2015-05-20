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


/*
 *  TODO: Add comments to code structure about some of the member variables created in order to avoid
 *        recreating them to improve performance.
 *        Maybe I need to redo the entire CODE STRUCURE SEGMENT
 *  
 *  Modified: MaiForm(),
 * 
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

            /******************************************************************/
            m_subjectOnePanelGraphics = subjectOnePanel.CreateGraphics();
            m_subjectTwoPanelGraphics = subjectTwoPanel.CreateGraphics();
            m_subjectThreePanelGraphics = subjectThreePanel.CreateGraphics();
            m_subjectFourPanelGraphics = subjectFourPanel.CreateGraphics();
            m_subjectFivePanelGraphics = subjectFivePanel.CreateGraphics();
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
            this.Invalidate();
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


        /******************************************************************/
        private string m_subjectOneTitle = "New Subject 1";
        private string m_subjectTwoTitle = "New Subject 2";
        private string m_subjectThreeTitle = "New Subject 3";
        private string m_subjectFourTitle = "New Subject 4";
        private string m_subjectFiveTitle = "New Subject 5";
        private Graphics m_subjectOnePanelGraphics;
        private Graphics m_subjectTwoPanelGraphics;
        private Graphics m_subjectThreePanelGraphics;
        private Graphics m_subjectFourPanelGraphics;
        private Graphics m_subjectFivePanelGraphics;

        private Font m_subjectPanelFont = new Font("Microsoft Sans Serif", 12);
        private SolidBrush m_subjectPanelBrush = new SolidBrush(Color.Black);

        /*
         *  1:06pm 5/19/2015
         */
        private void updateSubjectTabs(PaintEventArgs e)
        {
            subjectOnePanel.Refresh();
            subjectTwoPanel.Refresh();
            subjectThreePanel.Refresh();
            subjectFourPanel.Refresh();
            subjectFivePanel.Refresh();

            drawSubjectTitle(m_subjectOneTitle, m_subjectOnePanelGraphics);
            drawSubjectTitle(m_subjectTwoTitle, m_subjectTwoPanelGraphics);
            drawSubjectTitle(m_subjectThreeTitle, m_subjectThreePanelGraphics);
            drawSubjectTitle(m_subjectFourTitle, m_subjectFourPanelGraphics);
            drawSubjectTitle(m_subjectFiveTitle, m_subjectFivePanelGraphics);
        }

        /*
         * 1:13pm 5/19/2015
         */
        private void drawSubjectTitle(string a_subjectTitleString, Graphics a_subjectPanelGraphics)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_subjectPanelBrush, 2, 2, drawFormat);
        }

        /*
         *  1:00pm 5/19/2015
         */
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            updateSubjectTabs(e);
        }

        /*
         *  4:10pm 5/19/2015
         */
        private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e)
        {
            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:12pm 5/19/2015
         */
        private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            setDefaultBackColorForTabs();
            subjectTwoPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:14pm 5/19/2015
         */
        private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e)
        {
            setDefaultBackColorForTabs();
            subjectThreePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:15pm 5/19/2015
         */
        private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e)
        {
            setDefaultBackColorForTabs();
            subjectFourPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:17pm 5/19/2015
         */
        private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e)
        {
            setDefaultBackColorForTabs();
            subjectFivePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:20pm 5/19/2015
         */
        private void setDefaultBackColorForTabs()
        {
            subjectOnePanel.BackColor = SystemColors.ControlLight;
            subjectTwoPanel.BackColor = SystemColors.ControlLight;
            subjectThreePanel.BackColor = SystemColors.ControlLight;
            subjectFourPanel.BackColor = SystemColors.ControlLight;
            subjectFivePanel.BackColor = SystemColors.ControlLight;
        }

        /*
         * 5:19pm 5/19/2015
         */
        private void prevPageButton_Click(object sender, EventArgs e)
        {
            if (m_currentPageNumber == 1) return;
            m_currentPageNumber--;
            pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
        }

        /*
         *  5:23pm 5/20/2015
         */
        private void nextPageButton_Click(object sender, EventArgs e)
        {
            if (m_currentPageNumber == 50) return;
            m_currentPageNumber++;
            pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
        }
    }
}