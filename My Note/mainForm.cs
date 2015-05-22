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


/* 5/22/2015 (morning)latest
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
        private e_SelectedControl m_currentSelectedControl;     // Used to indicate the type of control the user selected
        private Color m_selectedControlButtonColor;             // Used to indicate the current color to be used by a control
        // This was added, it does not need to be a Singleton.
        private MyNoteStore m_mainMyNoteStore;// = new MyNoteStore();
        // this was added, this can probably be initialized here
        private StoreHandler m_mainStoreHandler = new StoreHandler();
        private Subject m_subjectOne;
        private Subject m_subjectTwo;
        private Subject m_subjectThree;
        private Subject m_subjectFour;
        private Subject m_subjectFive;
        // these are created to optimize redrawing
        private string m_subjectOneTitle;
        private string m_subjectTwoTitle;
        private string m_subjectThreeTitle;
        private string m_subjectFourTitle;
        private string m_subjectFiveTitle;
        // these are created to optimize redrawing (must be initialized in the constructor)
        private Graphics m_subjectOnePanelGraphics;
        private Graphics m_subjectTwoPanelGraphics;
        private Graphics m_subjectThreePanelGraphics;
        private Graphics m_subjectFourPanelGraphics;
        private Graphics m_subjectFivePanelGraphics;
        // these are created to optimize redrawing
        private Font m_subjectPanelFont = new Font("Microsoft Sans Serif", 12);
        private SolidBrush m_subjectPanelBrush = new SolidBrush(Color.Black);
        private SolidBrush m_emptySubjectPanelBrush = new SolidBrush(Color.DarkGray);
        private int m_currentPageNumber = 1;
        private Subject m_currentSubject;

        /*
         *  7:57am 5/21/15
         */
        private void refreshUISubjectTitles()
        {
            m_subjectOne = m_mainMyNoteStore.SavedSubjects[0];
            m_subjectOneTitle = m_subjectOne.SubjectTitle;
            //m_subjectOneTitle = "Software Des.";
            m_subjectTwo = m_mainMyNoteStore.SavedSubjects[1];
            m_subjectTwoTitle = m_subjectTwo.SubjectTitle;
            //m_subjectTwoTitle = "Comp Sci";
            m_subjectThree = m_mainMyNoteStore.SavedSubjects[2];
            m_subjectThreeTitle = m_subjectThree.SubjectTitle;
            m_subjectFour = m_mainMyNoteStore.SavedSubjects[3];
            m_subjectFourTitle = m_subjectFour.SubjectTitle;
            m_subjectFive = m_mainMyNoteStore.SavedSubjects[4];
            m_subjectFiveTitle = m_subjectFive.SubjectTitle;
        }
        // The types of text and drawing controls available to the user
        private enum e_SelectedControl
        {
            TEXT, PENCIL, ERASER, WARROW, NWARROW, NARROW,
            NEARROW, EARROW, SEARROW, SARROW, SWARROW,
            RECTANGLE, ELLIPSE, SOLID, DASHED, DOTTED, VERTTEXT
        }

        // this is the first method created so go back to the original file and add date
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

            m_pageNumberLabel.Location = new Point(2, 2);
            m_pageNumberLabel.Text = "1";
            m_pageNumberLabel.TextAlign = ContentAlignment.MiddleCenter;
            backPanel.Controls.Add(m_pageNumberLabel);
            Size newLabelSize = new Size(m_pageNumberLabel.Size.Width, m_pageNumberLabel.Size.Height);
            newLabelSize.Width = 30;
            m_pageNumberLabel.Size = newLabelSize;
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

            /************************************/
            // ATTN: Can some of these be moved to the constructor?
            // This is to set the first subject as selected tab when the system loads up
            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;
            
            this.Invalidate();
            

            // Added below, it should probably be moved to be by itself in a method that gets called
            // from constructor in order to initialize the mainMyNoteStore, if needed.
            string savedNotes = "savedNotes.txt";
            if (File.Exists(savedNotes))
            {
                mslog("File Exists");
                m_mainMyNoteStore = m_mainStoreHandler.OpenMyNoteStore(savedNotes);
            }
            else
            {
               mslog("File does not exist");
                // No file exists so create it
                m_mainMyNoteStore = new MyNoteStore(); // Created with empty subjects
                //m_mainStoreHandler.SaveMyNoteStore(savedNotes, m_mainMyNoteStore);
            }
            refreshUISubjectTitles();
            m_currentSubject = m_subjectOne;
        } /* private void MainForm_Load(object sender, System.EventArgs e) */


        /*  Intended to execute the very first time. It will be tested each time after that but will not execute
         *  anything. The reason for loading the form first is and not declaring this functionality in the _Load method is to
         *  show the user some interface while they are choosing a title for their first subject.
         *  3:36 5/21/2015
         */
        private void MainForm_Shown(object sender, EventArgs e)
        {
            string savedNotes = "savedNotes.txt";
            if (!File.Exists(savedNotes))
            {
                string newMyNoteMessage = "Thank you for trying My Note!\r\n" +
                "Prepare to enter a title for the first subject.\r\n" +
                "Click 'OK' to start, 'Cancel' to quit.";
                DialogResult newNoteBookDialog = MessageBox.Show(newMyNoteMessage, "Welcome To My Note!",
                    MessageBoxButtons.OKCancel);
                if (newNoteBookDialog == DialogResult.OK)
                {
                    DialogResult renameDialog = new DialogResult();
                    RenameSubjectForm renameSubjectForm = new RenameSubjectForm();
                    renameSubjectForm.SubjectTitle = m_currentSubject.SubjectTitle;
                    renameSubjectForm.FormTitle = "Create Subject Title";
                    renameDialog = renameSubjectForm.ShowDialog();
                    if (renameDialog == DialogResult.OK)
                    {
                        m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                        refreshUISubjectTitles();
                        // Create data storage file
                        m_mainStoreHandler.SaveMyNoteStore(savedNotes, m_mainMyNoteStore);
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
            // This gets called here because it depends on if the first item was created, because
            // then the DeleteSubject toolMenuStrip item will be disabled.
            updateToolStripMenuItems();
        }

        /* // this needs to stay minimum because it gets called by _Paint event method
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
            if (a_subjectTitleString == "New Subject")
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_emptySubjectPanelBrush, 2, 2, drawFormat);
            }
            else
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_subjectPanelBrush, 2, 2, drawFormat);
            }
            //a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_subjectPanelBrush, 2, 2, drawFormat);
        }

        /*
         *  1:00pm 5/19/2015
         */
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            updateSubjectTabs(e);
        }

        /*  a_pageNumber may be able to be eliminated since it will point to current page anyway
         *  In fact, the name of this method can be changed to be more descriptive to the situation to
         *  something like 'updateCurrentPageDisplayForSubject'
         *  8:40am 5/21/15
         */
        private void updatePageDisplayForSubject(Subject a_subject, int a_pageNumber)
        {
            Page pageToDisplay = a_subject.getPageForPageNumber(a_pageNumber);
            richTextBox.Text = pageToDisplay.PageText;
            m_shapesStorage = pageToDisplay.ShapeContainer;
            m_verticalTextList = pageToDisplay.VerticalTextList;
            transparentPanel.Controls.Clear();
            foreach(VerticalText v in m_verticalTextList)
            {
                v.setButtonProperties();
                transparentPanel.Controls.Add(v.MoveButton);
                transparentPanel.Controls.Add(v.OptionsButton);
                transparentPanel.Controls.Add(v.DeleteButton);
                transparentPanel.Controls.Add(v.RotateButton);
            }
            m_currentSubject = a_subject;
            m_currentPageNumber = a_pageNumber;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        }

        /*
         *  10:04am 5/21/2015
         */
        private void saveCurrentPageDisplayed()
        {
            Page pageToSave = m_currentSubject.getPageForPageNumber(m_currentPageNumber);
            pageToSave.PageText = richTextBox.Text;
            pageToSave.ShapeContainer = m_shapesStorage;
            pageToSave.VerticalTextList = m_verticalTextList;
            m_currentSubject.savePageWithPageNumber(pageToSave, m_currentPageNumber);
        }

        /*
         *  12:12pm 5/21/2015
         *  
         */
        private void saveCurrentSubjectDisplayed()
        {
            saveCurrentPageDisplayed();
            m_currentSubject.CurrentPageNumber = m_currentPageNumber;
        }

        /*
         *  4:10pm 5/19/2015
         */
        private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectOneTitle == "New Subject") return;
            saveCurrentSubjectDisplayed();

            // save current page number in each subject for convenience
            //m_currentSubject = m_subjectOne;
            //m_currentPageNumber = m_subjectOne.CurrentPageNumber;

            updatePageDisplayForSubject(m_subjectOne, m_subjectOne.CurrentPageNumber);

            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:12pm 5/19/2015
         */
        private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectTwoTitle == "New Subject") return;
            saveCurrentSubjectDisplayed();
            // save current page number in each subject for convenience
            //m_currentSubject = m_subjectTwo;
            //m_currentPageNumber = m_subjectTwo.CurrentPageNumber;

            updatePageDisplayForSubject(m_subjectTwo, m_subjectTwo.CurrentPageNumber);

            setDefaultBackColorForTabs();
            subjectTwoPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:14pm 5/19/2015
         */
        private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectThreeTitle == "New Subject") return;
            setDefaultBackColorForTabs();
            subjectThreePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:15pm 5/19/2015
         */
        private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFourTitle == "New Subject") return;
            setDefaultBackColorForTabs();
            subjectFourPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        /*
         *  4:17pm 5/19/2015
         */
        private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFourTitle == "New Subject") return;
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
            
            // Save current page
            saveCurrentPageDisplayed();

            // 'Turn' to next page
            m_currentPageNumber--;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updatePageDisplayForSubject(m_currentSubject, m_currentPageNumber);
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
                return;
            }
            if (currentPageIsEmpty())
            {
                string blankPageString = "Current page is blank, please enter some content before you continue.";
                MessageBox.Show(blankPageString, "Blank Page Error", MessageBoxButtons.OK);
                return;
            }

            // Save current page
            saveCurrentPageDisplayed();

            // 'Turn' to next page
            m_currentPageNumber++;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updatePageDisplayForSubject(m_currentSubject, m_currentPageNumber);
        }
        private bool currentPageIsEmpty()
        {
            if (richTextBox.TextLength != 0) return false;
            if (m_shapesStorage.NumberOfShapes() != 0) return false;
            if (m_verticalTextList.Count != 0) return false;
            return true;
        }
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

        /*
         * 8:02am 5/20/2015
         */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {   // Save based on subject?
            string textToSave = richTextBox.Text;
            ShapeContainer shapesToSave = m_shapesStorage;
            List<VerticalText> verticalTextListToSave = m_verticalTextList;

            Page pageToSave = new Page();
            pageToSave.PageText = textToSave;
            pageToSave.ShapeContainer = shapesToSave;
            pageToSave.VerticalTextList = verticalTextListToSave;
            
            Subject subjectToSave = new Subject();
            subjectToSave.Pages.Add(pageToSave);
            
            MyNoteStore myNoteToSave = new MyNoteStore();
            myNoteToSave.SavedSubjects.Add(subjectToSave);
            
            StoreHandler storeHandler = new StoreHandler();
            storeHandler.SaveMyNoteStore("savedNotes.txt", myNoteToSave);
        }
        /*
         *  8:04am 5/20/2015
         */
        private void restoreButton_Click(object sender, EventArgs e)
        {
            MyNoteStore myNoteToRestore = new MyNoteStore();
            StoreHandler storeHandler = new StoreHandler();
            myNoteToRestore = storeHandler.OpenMyNoteStore("savedNotes.txt");
            
            // Restore based on subject?
            Subject subjectToRestore = myNoteToRestore.SavedSubjects[0];
            
            Page pageToRestore = subjectToRestore.Pages[0];
            richTextBox.Text = pageToRestore.PageText;
            m_shapesStorage = pageToRestore.ShapeContainer;
            m_verticalTextList = pageToRestore.VerticalTextList;

            // quick fix
            transparentPanel.Controls.Clear();
            foreach (VerticalText v in m_verticalTextList)
            {
                v.setButtonProperties();
                transparentPanel.Controls.Add(v.MoveButton);
                transparentPanel.Controls.Add(v.OptionsButton);
                transparentPanel.Controls.Add(v.DeleteButton);
                transparentPanel.Controls.Add(v.RotateButton);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
        }

        /*
         *  1:41pm 5/21/2015
         */
        private void renameSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This can be created 'on the fly' because it will not be done very often
            DialogResult renameDialog = new DialogResult();
            RenameSubjectForm renameSubjectForm = new RenameSubjectForm();
            renameSubjectForm.SubjectTitle = m_currentSubject.SubjectTitle;
            renameSubjectForm.FormTitle = "Rename Subject Title";
            renameDialog = renameSubjectForm.ShowDialog(); 
            if (renameDialog == DialogResult.OK)
            {
                m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                refreshUISubjectTitles();
                this.Invalidate();
            }
        }

        /*  This belongs in the formMenuStrip.cs
         *  5:08pm 5/21/2015
         */
        private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if last subject was added, then delete subject is disabled
            updateToolStripMenuItems();

            // This can be created 'on the fly' because it will not be done very often
            DialogResult renameDialog = new DialogResult();
            RenameSubjectForm renameSubjectForm = new RenameSubjectForm();
            renameSubjectForm.SubjectTitle = "New Subject";
            renameSubjectForm.FormTitle = "New Subject Title";
            renameDialog = renameSubjectForm.ShowDialog();
            if (renameDialog == DialogResult.OK)
            {
                // Create new subject
                  // get count of subjects
                int nextSubjectIndex = m_mainMyNoteStore.NumberOfSubjects();
                  // use that count as index for next subject to be created
                m_currentSubject = m_mainMyNoteStore.SavedSubjects[nextSubjectIndex];
                m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                saveCurrentSubjectDisplayed();
                updatePageDisplayForSubject(m_currentSubject, m_currentSubject.CurrentPageNumber);
                setDefaultBackColorForTabs();
                // adding these below, since it is not much work it may not be need to be in its own method
                switch (nextSubjectIndex)
                {
                    case 1:
                        subjectTwoPanel.BackColor = SystemColors.ControlDark;
                        m_subjectTwoTitle = m_currentSubject.SubjectTitle;
                        break;
                    case 2:
                        subjectThreePanel.BackColor = SystemColors.ControlDark;
                        m_subjectThreeTitle = m_currentSubject.SubjectTitle;
                        break;
                    case 3:
                        subjectFourPanel.BackColor = SystemColors.ControlDark;
                        m_subjectFourTitle = m_currentSubject.SubjectTitle;
                        break;
                    case 4:
                        subjectFivePanel.BackColor = SystemColors.ControlDark;
                        m_subjectFiveTitle = m_currentSubject.SubjectTitle;
                        break;
                }
                this.Invalidate();
                //m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                //refreshUISubjectTitles();
                //this.Invalidate();
            }
            //updateToolStripMenuItems();
        }
        
        /*  This belongs in the formMenuStrip.cs
         *  If there is only one subject that, then disable DeleteSubject menu item
         *  If there are already Five Subjects, then disable AddSubject menu item.
         *  5:11pm 5/21/2015
         */
        private void updateToolStripMenuItems()
        {
            int numberOfSubjects = m_mainMyNoteStore.NumberOfSubjects();
            if (numberOfSubjects == 1)
            {
                deleteSubjectToolStripMenuItem.Enabled = false;
            }
            else
            {
                deleteSubjectToolStripMenuItem.Enabled = true;
            }
            if (numberOfSubjects == 5)
            {
                addNewSubjectToolStripMenuItem.Enabled = false;
            }
            else
            {
                addNewSubjectToolStripMenuItem.Enabled = true;
            }
        }
    }
}