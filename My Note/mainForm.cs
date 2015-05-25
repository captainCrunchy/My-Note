using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/*  // 5/25/2015 (latest) finished mainForm_Shown comments
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
 *      description of each 'partial class' and its purpose. Each one contains member variables specific
 *      to their task. Variables that need to be initialized in the constructor are done so in the main
 *      constructor in mainForm.cs
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
 *        Need to add a method to load pages or start new page
 *        Maybe I need to redo the entire CODE STRUCURE SEGMENT
 *        Make sure everything gets initialized and assigned at the right times in this file
 *  
 *  Modified: Added some code at the bottom, 
 *            Modified MaiForm(),
 *            Modified MainFormLoad()
 *            Added m_mainMyNoteStore
 *            Added m_mainStoreHandler
 *            Added subjects
 *            Added a lot, need to update the description of this .cs in the comments
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        // Member variables for this class
        #region Member Variables

        // The types of text and drawing controls available to the user
        private enum e_SelectedControl
        {
            TEXT, PENCIL, ERASER, WARROW, NWARROW, NARROW, NEARROW, EARROW, SEARROW,
            SARROW, SWARROW, RECTANGLE, ELLIPSE, SOLID, DASHED, DOTTED, VERTTEXT
        }
        private e_SelectedControl m_currentSelectedControl;                 // Current control selected
        private Color m_selectedControlButtonColor;                         // Current color for current control

        private MyNoteStore m_mainMyNoteStore;                              // Holds data before it is written to disk
        private StoreHandler m_mainStoreHandler = new StoreHandler();       // Writes data to disk

        // Member variables for each 'Subject' are created to help
        // optimize performance during drawing, saving, and loading
        private Subject m_subjectOne;
        private Subject m_subjectTwo;
        private Subject m_subjectThree;
        private Subject m_subjectFour;
        private Subject m_subjectFive;
        private Graphics m_subjectOnePanelGraphics;
        private Graphics m_subjectTwoPanelGraphics;
        private Graphics m_subjectThreePanelGraphics;
        private Graphics m_subjectFourPanelGraphics;
        private Graphics m_subjectFivePanelGraphics;
        private string m_subjectOneTitle;
        private string m_subjectTwoTitle;
        private string m_subjectThreeTitle;
        private string m_subjectFourTitle;
        private string m_subjectFiveTitle;
        
        private Font m_subjectPanelFont = new Font("Microsoft Sans Serif", 12);         // Font for 'Subject' title
        private SolidBrush m_fullSubjectPanelBrush = new SolidBrush(Color.Black);       // Brush for used 'Subject' title
        private SolidBrush m_emptySubjectPanelBrush = new SolidBrush(Color.DarkGray);   // Brush for empty 'Subject' title
        private Label m_pageNumberLabel = new Label();                      // Indicates current page number in 'Subject'
        private int m_currentPageNumber = 1;                                // Used for save/load data and UI objects
        private Subject m_currentSubject;                                   // Reference to current subject on screen
        private const string m_savePath = "savedNotes.txt";                 // References save path on disk 
        private const string m_newSubjectTitle = "New Subject";             // Used to assign and compare labels

        #endregion

        // Methods for MainForm load, save, and general functionality
        #region MainForm Methods

        /*
         * NAME
         *  MainForm() - initializes this class and its elements
         *  
         * SYNOPSIS
         *  public void MainForm();
         * 
         * DESCRIPTION
         *  This constructor is responsible for initializig and assigning values to its self, member variables, and
         *  UI elements of this class. Some variables initialized here are declared in different files, which are
         *  part of this class.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  11:32am 3/1/2015
         */
        public MainForm()
        {
            InitializeComponent();
            // fix start position and prevent resizing
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            m_currentSelectedControl = e_SelectedControl.TEXT;
            m_selectedControlButtonColor = Color.SandyBrown;

            textSelectButton.BackColor = m_selectedControlButtonColor;
            fontComboBox.Text = "Microsoft Sans Serif";
            richTextBox.Font = new Font("Microsoft Sans Serif", 12);

            m_transparentPanelGraphics = this.transparentPanel.CreateGraphics();
            m_transparentPanelPen = new Pen(m_currentDrawColor);

            m_subjectOnePanelGraphics = subjectOnePanel.CreateGraphics();
            m_subjectTwoPanelGraphics = subjectTwoPanel.CreateGraphics();
            m_subjectThreePanelGraphics = subjectThreePanel.CreateGraphics();
            m_subjectFourPanelGraphics = subjectFourPanel.CreateGraphics();
            m_subjectFivePanelGraphics = subjectFivePanel.CreateGraphics();
            
            m_pageNumberLabel.Text = "1";
            m_pageNumberLabel.TextAlign = ContentAlignment.MiddleCenter;
            m_pageNumberLabel.BackColor = Color.Transparent;
            m_pageNumberLabel.BorderStyle = BorderStyle.Fixed3D;
            m_pageNumberLabel.Location = new Point(406, 697);
            m_pageNumberLabel.Size = new Size(30, 18);
            this.Controls.Add(m_pageNumberLabel);
        } /* public MainForm() */

        /*
         * NAME
         *  MainForm_Load() - prepares elements before the form is shown on screen
         *  
         * SYNOPSIS
         *  private void MainForm_Load(object sender, System.EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called before elements are shown on screen. It is used to update and assign
         *  properties to elements before they are shown on screen after they have been initialized in the
         *  in the constructor. It also prepares the 'data persistence' object by creating it or getting
         *  it from disk using the data persistence handler object. If file exists, then it is loaded into
         *  memory. Otherwise 'data store' is created and will be populated in MainForm_Shown().
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
            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;

            if (File.Exists(m_savePath))
            {
                m_mainMyNoteStore = m_mainStoreHandler.OpenMyNoteStore(m_savePath);
                assignSubjectsAndTitles();
                m_currentSubject = m_subjectOne;
                updateCurrentPageDisplayForSubject(m_currentSubject, m_currentSubject.CurrentPageNumber);
            }
            else
            {
                m_mainMyNoteStore = new MyNoteStore();
            }
            selectTextControl();
        } /* private void MainForm_Load(object sender, System.EventArgs e) */

        /*
         * NAME
         *  MainForm_Shown() - handles data as form is being shown on screen
         *  
         * SYNOPSIS
         *  private void MainForm_Shown(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  The functionality of this method will be applied the very first time the user runs this
         *  application. It asks the user to enter a title for the first 'Subject' in the notebook. A data
         *  file is then created and written to disk for the very first time. Reason for performing this
         *  task here is for appearances; i.e. to show to the user the background and an empty notebook with
         *  subject tabs that have no titles. It uses a custom rename form to which an existing 'Subject'
         *  title is passed, which returns a non-default title.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:36pm 5/21/2015
         */
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!File.Exists(m_savePath))
            {
                string newMyNoteMessage = "Thank you for trying My Note!\r\n" +
                "Prepare to enter a title for the first subject.\r\n" +
                "Click 'OK' to start, 'Cancel' to quit.";
                DialogResult newNoteBookDialog = MessageBox.Show(newMyNoteMessage, "Welcome To My Note!",
                    MessageBoxButtons.OKCancel);
                if (newNoteBookDialog == DialogResult.OK)
                {
                    m_currentSubject = m_mainMyNoteStore.SavedSubjects[0];
                    DialogResult renameDialog = new DialogResult();
                    RenameSubjectForm renameSubjectForm = new RenameSubjectForm();
                    renameSubjectForm.SubjectTitle = m_currentSubject.SubjectTitle;
                    renameSubjectForm.FormTitle = "Create Subject Title";
                    renameSubjectForm.StartPosition = FormStartPosition.CenterParent;
                    renameSubjectForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                    renameSubjectForm.MaximizeBox = false;
                    renameSubjectForm.MinimizeBox = false;
                    renameDialog = renameSubjectForm.ShowDialog();
                    if (renameDialog == DialogResult.OK)
                    {
                        m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                        assignSubjectsAndTitles();
                        m_mainStoreHandler.SaveMyNoteStore(m_savePath, m_mainMyNoteStore);
                        updateToolStripMenuItems();
                        this.Invalidate();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }

        /*
         *  1:00pm 5/19/2015
         */
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            updateSubjectTabs(e);
        }

        /*
         *  3:04pm 5/25/2015
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string saveNoteString = "Do you want to save your work?";
            DialogResult saveNoteDialog = MessageBox.Show(saveNoteString, "Save Changes", MessageBoxButtons.YesNoCancel);
            if (saveNoteDialog == DialogResult.Yes)
            {
                saveAllContent();
            }
            else if (saveNoteDialog == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }


        /*
         *  3:09pm 5/25/2015
         */
        private void saveAllContent()
        {
            saveCurrentPageDisplayed();
            m_mainStoreHandler.SaveMyNoteStore(m_savePath, m_mainMyNoteStore);
        }

        #endregion

        // Methods that handle Subjects in the notebook
        #region Subject Methods

        /*  This can be called refresh subjects and titles, if assigning subjects is not necessary
         *  then assign subjecttTitles more directly and eliminate subject assignment
         *  7:57am 5/21/15
         *  Called from MainForm_Load(), MainForm_Shown(), renameSubjectToolStripMenuItem_Click()
         */
        private void assignSubjectsAndTitles()
        {
            m_subjectOne = m_mainMyNoteStore.SavedSubjects[0];
            m_subjectOneTitle = m_subjectOne.SubjectTitle;
            m_subjectTwo = m_mainMyNoteStore.SavedSubjects[1];
            m_subjectTwoTitle = m_subjectTwo.SubjectTitle;
            m_subjectThree = m_mainMyNoteStore.SavedSubjects[2];
            m_subjectThreeTitle = m_subjectThree.SubjectTitle;
            m_subjectFour = m_mainMyNoteStore.SavedSubjects[3];
            m_subjectFourTitle = m_subjectFour.SubjectTitle;
            m_subjectFive = m_mainMyNoteStore.SavedSubjects[4];
            m_subjectFiveTitle = m_subjectFive.SubjectTitle;
        }

        /*  this needs to stay minimum because it gets called by _Paint event method
         *  1:06pm 5/19/2015
         *  Called from MainForm_Paint()
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
         * Called from updateSubjectTabs() x 5
         */
        private void drawSubjectTitle(string a_subjectTitleString, Graphics a_subjectPanelGraphics)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            //if (a_subjectTitleString == "New Subject")
            if (a_subjectTitleString == m_newSubjectTitle)
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_emptySubjectPanelBrush, 2, 2, drawFormat);
            }
            else
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_fullSubjectPanelBrush, 2, 2, drawFormat);
            }
            //a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_fullSubjectPanelBrush, 2, 2, drawFormat);
        }


        /*  This may be able to be ELIMINATED because saveCurrentPageDisplayed() may perform equivalent
         *  functionality? This means moving the update of current subject's page to the method suggested.
         *  
         *  DESCRIPTION: Calls
         *  12:12pm 5/21/2015
         *  Called from:
         *  addNewSubjectToolStripMenuItem_Click(),
         *  subjectOnePanel_MouseDown(),
         *  subjectTwoPanel_MouseDown(),
         *  ...
         */
        //private void saveCurrentSubjectDisplayed()
        //{
        //    saveCurrentPageDisplayed();
        //    m_currentSubject.CurrentPageNumber = m_currentPageNumber;
        //}

        /*
         *  4:10pm 5/19/2015
         */
        private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectOneTitle == m_newSubjectTitle) return;
            if (m_subjectOneTitle == m_currentSubject.SubjectTitle) return;
            //saveCurrentSubjectDisplayed();
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectOne, m_subjectOne.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:12pm 5/19/2015
         */
        private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectTwoTitle == m_newSubjectTitle) return;
            if (m_subjectTwoTitle == m_currentSubject.SubjectTitle) return;
            //saveCurrentSubjectDisplayed();
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectTwo, m_subjectTwo.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectTwoPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:14pm 5/19/2015
         */
        private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectThreeTitle == m_newSubjectTitle) return;
            if (m_subjectThreeTitle == m_currentSubject.SubjectTitle) return;
            //saveCurrentSubjectDisplayed();
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectThree, m_subjectThree.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectThreePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:15pm 5/19/2015
         */
        private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFourTitle == m_newSubjectTitle) return;
            if (m_subjectFourTitle == m_currentSubject.SubjectTitle) return;
            //saveCurrentSubjectDisplayed();
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectFour, m_subjectFour.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectFourPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:17pm 5/19/2015
         */
        private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFiveTitle == m_newSubjectTitle) return;
            if (m_subjectFiveTitle == m_currentSubject.SubjectTitle) return;
            //saveCurrentSubjectDisplayed();
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectFive, m_subjectFive.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectFivePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:20pm 5/19/2015
         *  Called from: MainFormLoad(),
         *  addNewSubjectToolStripMenuItem_Click(),
         *  subjectOnePanel_MouseDown(),
         *  subjectTwoPanel_MouseDown(),
         *  ...
         */
        private void setDefaultBackColorForTabs()
        {
            subjectOnePanel.BackColor = SystemColors.ControlLight;
            subjectTwoPanel.BackColor = SystemColors.ControlLight;
            subjectThreePanel.BackColor = SystemColors.ControlLight;
            subjectFourPanel.BackColor = SystemColors.ControlLight;
            subjectFivePanel.BackColor = SystemColors.ControlLight;
        }

        #endregion

        // Methods that handle Pages in each subject
        #region Pages Methods

        /*  DESCRIPTION: This method loads a page to be displayed. When switching between existing subjects
         *  full of pages, it loads the last page viewed by the user in the subject to enhance user experience.
         *  If user clicks 'next page' and page does not exist, then one is created and added to the subject.
         *  The working 'engine' for this functionality is 'getPageForPageNumber()' method.
         *  8:40am 5/21/15
         *  Called from:
         *  addNewSubjectToolStripMenuItem_Click(),
         *  prevPageButon_Click(),
         *  nextPageButton_Click(),
         *  subjectOnePanel_MouseDown(),
         *  subjectTwoPanel_MouseDown(),
         *  ...
         *  TODO - Consider adding setDefaultBackColorForTabs() and backColor = SystemColors.ControlDark in this
         *  method for professional looking code
         */
        private void updateCurrentPageDisplayForSubject(Subject a_subject, int a_pageNumber)
        {
            Page pageToDisplay = a_subject.getPageForPageNumber(a_pageNumber);
            richTextBox.Text = pageToDisplay.PageText;
            m_shapesStorage = pageToDisplay.ShapeContainer;
            //m_verticalTextList = pageToDisplay.VerticalTextList;
            m_verticalTextList.Clear();
            foreach (VerticalText vOne in pageToDisplay.VerticalTextList)
            {
                m_verticalTextList.Add(vOne);
            }
            transparentPanel.Controls.Clear();
            foreach (VerticalText vTwo in m_verticalTextList)
            {
                vTwo.setButtonProperties();
                vTwo.OwnerTranspPanel = transparentPanel;
                vTwo.OwnerRichTextBox = richTextBox;
                vTwo.OwnerBackPanel = backPanel;
                vTwo.OwnerVerticalTextList = m_verticalTextList;
                transparentPanel.Controls.Add(vTwo.MoveButton);
                transparentPanel.Controls.Add(vTwo.OptionsButton);
                transparentPanel.Controls.Add(vTwo.DeleteButton);
                transparentPanel.Controls.Add(vTwo.RotateButton);
            }
            m_currentSubject = a_subject;
            m_currentPageNumber = a_pageNumber;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
            selectTextControl();
        }

        /*  DESCRIPTION: Saves UI elements on current page to current Page object. It gets the existing Page
         *  object instead of creating a new one every time a new or an existing page needs to be saved. This
         *  is done in order to minimize the use of resources.
         *  10:04am 5/21/2015
         *  Called from:
         *  prevPageButton_Click(),
         *  nextPageButton_Click()
         *  saveCurrentSubjectDisplayed(),
         *  saveTooStripMenuItem_Click()
         */
        private void saveCurrentPageDisplayed()
        {
            Page pageToSave = m_currentSubject.getPageForPageNumber(m_currentPageNumber);
            pageToSave.PageText = richTextBox.Text;
            pageToSave.ShapeContainer = m_shapesStorage;
            //pageToSave.VerticalTextList = m_verticalTextList;
            // Populate with new or 'refresh' existing data
            pageToSave.VerticalTextList.Clear();
            foreach(VerticalText v in m_verticalTextList)
            {
                pageToSave.VerticalTextList.Add(v);
            }
            m_currentSubject.savePageWithPageNumber(pageToSave, m_currentPageNumber);
            m_currentSubject.CurrentPageNumber = m_currentPageNumber;
        }

        /*
         * 5:19pm 5/19/2015
         */
        private void prevPageButton_Click(object sender, EventArgs e)
        {
            if (m_currentPageNumber == 1) return;

            // Save current page
            saveCurrentPageDisplayed();

            // 'Turn' to next page
            m_currentPageNumber--;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updateCurrentPageDisplayForSubject(m_currentSubject, m_currentPageNumber);
        }

        /*  
         *  5:23pm 5/20/2015
         */
        private void nextPageButton_Click(object sender, EventArgs e)
        {
            // Allowed to continue?
            if (m_currentPageNumber == 50)
            {
                string endOfSubjectString = "Subject cannot exceed 50 pages. " +
                "Please start a new subject if you wish to continue.";
                MessageBox.Show(endOfSubjectString, "Maximum Page Error", MessageBoxButtons.OK);
                selectTextControl();
                return;
            }
            //if (currentPageIsEmpty())
            // Prevent from adding blank pages at the end of the subject. One blank page is allowed. Also
            // if somehow 
            if (currentPageIsEmpty() && m_currentPageNumber == m_currentSubject.TotalNumberOfPages)
            {
                string blankPageString = "Current page is blank, please enter some content before you continue.";
                MessageBox.Show(blankPageString, "Blank Page Error", MessageBoxButtons.OK);
                selectTextControl();
                return;
            }

            // Save current page
            saveCurrentPageDisplayed();

            // 'Turn' to next page
            m_currentPageNumber++;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updateCurrentPageDisplayForSubject(m_currentSubject, m_currentPageNumber);
        }

        /*
         *  5:27pm 5/20/2015
         *  Called from: nextPageButton_Click()
         */
        private bool currentPageIsEmpty()
        {
            if (richTextBox.TextLength != 0) return false;
            if (m_shapesStorage.NumberOfShapes() != 0) return false;
            if (m_verticalTextList.Count != 0) return false;
            return true;
        }

        #endregion

        // Temporary methods
        #region Temporary Methods

        // Temp
        private void moveLogCursor()
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.SelectionLength = 0;
            logTextBox.ScrollToCaret();
        }
        
        // Temp
        private void mslog(string a_str)
        {
            logTextBox.Text += a_str + "\r\n";
            moveLogCursor();
        }

        // Temp
        private void clearButton_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
        }

        #endregion
    }
}