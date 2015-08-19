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
 *    MainForm class:
 *      This class represents the main form for the application. It is the starting point and is always visible. It provides
 *      common controls such as 'File' and 'Help' menu options, text editing and drawing controls, and a 'combined' panel
 *      for text editing and drawing shapes. This MainForm class is divided into four (.cs) files, which are simply extensions
 *      of this class; i.e. each is a 'public partial class MainForm : Form'. This was done to keep the code organized and
 *      readable. The user is interacting with some part of this class at all times.
 *    mainForm.cs: (YOU ARE HERE)
 *      This file implements tasks that are responsible for starting and running the application. It performs general tasks
 *      like handling the user inteface elements of the form and communication with data persistence objects. It is also
 *      responsible for coordinating tasks between other 'partial class' files.
 *    formMenuStrip.cs:
 *      This file handles events that are triggered by elements of the menu strip in the form and their appearances based on
 *      current data. Example: File, Edit, ..., Help.
 *    formToolbar.cs:
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
 *    mainForm.cs: (YOU ARE HERE)
 *      This file is organized by separating code into 'regions' based on specific functionalities. It contains member variables
 *      used in this file. It initializes member variables used in other files, which are parts of this 'partial class', because
 *      the main constructor is in this file. Regions contain code that is specific to elements of this file and this application. 
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        // Region contains member variables for this class
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

        // Member variables for each 'Subject' are created here in order to reduce dynamic creation
        // of drawing objects in order to optimize performance during drawing, saving, and loading
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
        private StringFormat m_drawFormat = new StringFormat();             // Format for drawing a title string vertically

        private Label m_pageNumberLabel = new Label();                      // Indicates current page number in 'Subject'
        private int m_currentPageNumber = 1;                                // Used for save/load data and UI objects
        private Subject m_currentSubject;                                   // Reference to current subject on screen
        private const string m_newSubjectTitle = "New Subject";             // Used to assign and compare labels

        /* Save path for the entire notebook. Placed in 'C:\Users\YourName\AppData\Local\savedNotes.txt'. It is created
           at startup, the moment the user enters the subject title for the first subject in the notebook. Used in this
           class and data persistence classes. */
        private string m_savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "savedNotes.txt");
        

        #endregion

        // Region contains methods for MainForm load, save, and other general functionality
        #region MainForm Methods

        /*
         * NAME
         *  MainForm() - initializes this class and its elements
         *  
         * SYNOPSIS
         *  public MainForm();
         * 
         * DESCRIPTION
         *  This constructor is responsible for initializing and assigning values to member variables and
         *  UI elements of this class. Some variables initialized here are declared in different files, which
         *  are also parts of this class.
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
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "My Note";

            m_autoSaveTimer.Tick += new EventHandler(TimerEventProcessor);  // formmMenuStrip.cs
            m_autoSaveNotifyTimer.Tick += new EventHandler(autoSaveNotify);  // formmMenuStrip.cs
            m_autoSaveNotifyLabel.Location = new Point(8, 90);  // formmMenuStrip.cs (UI element)
            m_autoSaveNotifyLabel.Size = new Size(132, 13);
            m_autoSaveNotifyLabel.Text = "Work saved automatically!";
            m_autoSaveNotifyLabel.ForeColor = Color.Black;
            
            m_currentSelectedControl = e_SelectedControl.TEXT;  // mainForm.cs
            m_selectedControlButtonColor = Color.SandyBrown;  // mainForm.cs

            textSelectButton.BackColor = m_selectedControlButtonColor;  // formToolbar.cs (UI element)
            fontComboBox.Text = "Microsoft Sans Serif";  // formToolbar.cs (UI element)
            richTextBox.Font = new Font("Microsoft Sans Serif", 12);  // formTextbox.cs (UI element)

            m_transparentPanelGraphics = this.transparentPanel.CreateGraphics();  // formTextbox.cs
            m_transparentPanelPen = new Pen(m_currentDrawColor);  // formTextbox.cs
            m_dashLinePen = new Pen(m_currentDrawColor, m_currentPenWidth);  // formTextbox.cs
            m_dashLinePen.DashPattern = m_dashLineValues;  // formTextbox.cs
            m_dottedLinePen = new Pen(m_currentDrawColor, m_currentPenWidth);  // formTextbox.cs
            m_dottedLinePen.DashPattern = m_dottedLineValues;  // formTextbox.cs

            m_subjectOnePanelGraphics = subjectOnePanel.CreateGraphics();  // mainForm.cs
            m_subjectTwoPanelGraphics = subjectTwoPanel.CreateGraphics();  // mainForm.cs
            m_subjectThreePanelGraphics = subjectThreePanel.CreateGraphics();  // mainForm.cs
            m_subjectFourPanelGraphics = subjectFourPanel.CreateGraphics();  // mainForm.cs
            m_subjectFivePanelGraphics = subjectFivePanel.CreateGraphics();  // mainForm.cs
            m_drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;  // mainForm.cs

            m_pageNumberLabel.Text = "1";  // mainForm.cs (UI element)
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
         *  The main functionality of this method will be applied the very first time the user runs this
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
                    DialogResult newNameDialog = new DialogResult();
                    RenameSubjectForm newNameSubjectForm = new RenameSubjectForm();
                    newNameSubjectForm.SubjectTitle = m_currentSubject.SubjectTitle;
                    newNameSubjectForm.FormTitle = "New Subject Title";
                    newNameSubjectForm.StartPosition = FormStartPosition.CenterParent;
                    newNameSubjectForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                    newNameSubjectForm.MaximizeBox = false;
                    newNameSubjectForm.MinimizeBox = false;
                    newNameDialog = newNameSubjectForm.ShowDialog();
                    if (newNameDialog == DialogResult.OK)
                    {
                        m_currentSubject.SubjectTitle = newNameSubjectForm.SubjectTitle;
                        assignSubjectsAndTitles();
                        m_mainStoreHandler.SaveMyNoteStore(m_savePath, m_mainMyNoteStore);
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
            m_autoSaveTimeInterval = m_mainMyNoteStore.AutoSaveTimeInterval;
            updateAutoSaveTimer();
            updateToolStripMenuItems();
            updateCheckMarksForAutoSaveItems();
        } /* private void MainForm_Shown(object sender, EventArgs e) */

        /*
         * NAME
         *  MainForm_Paint() - repaints the elements on the form
         *  
         * SYNOPSIS
         *  private void MainForm_Paint(object sender, PaintEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called automatically based on the events within the form to repaint the form. The
         *  amount of tasks performed by this event handler are kept to a minimum due to the frequency of this
         *  method call. It triggers other methods that update and repaint the 'tabs' for 'Subject' titles.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:00pm 5/19/2015
         */
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            updateSubjectTabs();
        } /* private void MainForm_Paint(object sender, PaintEventArgs e) */

        /*
         * NAME
         *  MainForm_FormClosing() - asks the user if they want to save changes
         *  
         * SYNOPSIS
         *  private void MainForm_FormClosing(object sender, FormClosingEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called automatically when the user is exiting the application, regardless of the method they
         *  used to exit; i.e. by clicking the 'x', using Alt+F4, etc... It presents to the user a dialog asking them if
         *  they would like to save changes to their work. The user has three options to choose from: Yes, No, or Cancel.
         *  Selecting 'Yes' saves changes and exits the program. Selecting 'No' exits the application without saving any
         *  changes. Selecting 'Cancel' aborts the operation; that is, does not save or exit the application. If the
         *  application is running for the first time and the user decided to exit before they gave their first subject
         *  a title, then no save prompt is presented and the application just closes.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:04pm 5/25/2015
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If application is running the first time (prevents exceptions)
            if (!File.Exists(m_savePath))
            {
                return;
            }
            // If application has been run before
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
        } /* private void MainForm_FormClosing(object sender, FormClosingEventArgs e) */

        /*
         * NAME
         *  saveAllContent() - saves all content to data persistence object and saves it to disk
         *  
         * SYNOPSIS
         *  private void saveAllContent();
         * 
         * DESCRIPTION
         *  The purpose of this method is to save all changes in the application and write them to a
         *  binary file on disk. First, current page and subject are saved by calling the appropriate
         *  method. Then, the main data persistence handler object is called to perform a write to disk.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:09pm 5/25/2015
         */
        private void saveAllContent()
        {
            saveCurrentPageDisplayed();
            m_mainStoreHandler.SaveMyNoteStore(m_savePath, m_mainMyNoteStore);
        } /* private void saveAllContent() */

        #endregion

        // Region contains methods that handle Subjects in the notebook
        #region Subjects Methods

        /*
         * NAME
         *  assignSubjectsAndTitles() - assigns 'Subject(s)' and their titles
         *  
         * SYNOPSIS
         *  private void assignSubjectsAndTitles();
         * 
         * DESCRIPTION
         *  This method assigns values to member variables that represent current 'Subject(s)'. These
         *  new values are then used when processing data, like saving and loading, and to update the
         *  values for 'Subject' title tabs so that they can be repainted with the right titles. This
         *  method gets called during the application startup and as subjects are added or deleted.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:57am 5/21/2015
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
        } /* private void assignSubjectsAndTitles() */

        /*
         * NAME
         *  updateSubjectTabs() - updates subject titles on the subject 'tab' panels
         *  
         * SYNOPSIS
         *  private void updateSubjectTabs();
         * 
         * DESCRIPTION
         *  This method repaints the subject titles on the panels that are used like subject 'tabs'
         *  in a real notebook. It gets called from MainForm_Paint() and is kept as simple as it can
         *  possibly be in order to minimize resource usage.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:06pm 5/19/2015
         */
        private void updateSubjectTabs()
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
        } /* private void updateSubjectTabs() */

        /*
         * NAME
         *  drawSubjectTitle() - draws a 'Subject' title as a vertical string
         *  
         * SYNOPSIS
         *  private void drawSubjectTitle(string a_subjectTitleString, Graphics a_subjectPanelGraphics);
         *      a_subjectTitleString        -> the string to be used for the drawing
         *      a_subjectPanelGraphics      -> panel representing the 'Subject' tab on which to draw string
         * 
         * DESCRIPTION
         *  This method is used to draw 'Subject' titles vertically on the panels that are used like subject tabs in a
         *  regular notebook. It specifically draws the string using two different colors based on the availability of the
         *  'Subject'. If the subject has a regular title, then string is drawn using a darker brush. If the 'Subject'
         *  is empty and has not been assigned a non-default title, then the string is drawn using a lighter brush to
         *  help visually indicate so. Many of the variables used for the DrawString() method have already been declared
         *  and initialized in this class to prevent creating and assigning them dynamically in order to increase
         *  drawing performance. The original code used in this method was taken from Microsoft's .NET website and
         *  modified to fit the needs of this application. Credits have been documented.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi using code from Microsoft's website:
         *  https://msdn.microsoft.com/en-us/library/k7282y7x(v=vs.110).aspx
         *  
         *  
         * DATE
         *  1:13pm 5/19/2015
         */
        private void drawSubjectTitle(string a_subjectTitleString, Graphics a_subjectPanelGraphics)
        {
            if (a_subjectTitleString == m_newSubjectTitle)
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_emptySubjectPanelBrush, 2, 2, m_drawFormat);
            }
            else
            {
                a_subjectPanelGraphics.DrawString(a_subjectTitleString, m_subjectPanelFont, m_fullSubjectPanelBrush, 2, 2, m_drawFormat);
            }
        } /* private void drawSubjectTitle(string a_subjectTitleString, Graphics a_subjectPanelGraphics) */

        /*
         * NAME
         *  subjectOnePanel_MouseDown() - presents first subject UI and its data
         *  
         * SYNOPSIS
         *  private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler is triggered upon clickig the panel representing the first 'Subject' in the
         *  notebook. It checks to see if this subject is currently empty or if it is already presented before
         *  it performs its tasks. Its tasks include saving the current page, updating the user interface to the
         *  first 'Subject', and updating the colors for 'Subject' tabs and their titles. It presents the page
         *  that was last used in this 'Subject' in order to deliver a more appealing functionality to the user.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:10pm 5/19/2015
         */
        private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectOneTitle == m_newSubjectTitle) return;
            if (m_subjectOneTitle == m_currentSubject.SubjectTitle) return;
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectOne, m_subjectOne.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectOnePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        } /* private void subjectOnePanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  subjectTwoPanel_MouseDown() - presents second subject UI and its data
         *  
         * SYNOPSIS
         *  private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler is triggered upon clickig the panel representing the second 'Subject' in the
         *  notebook. It checks to see if this subject is currently empty or if it is already presented before
         *  it performs its tasks. Its tasks include saving the current page, updating the user interface to the
         *  second 'Subject', and updating the colors for 'Subject' tabs and their titles. It presents the page
         *  that was last used in this 'Subject' in order to deliver a more appealing functionality to the user.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:12pm 5/19/2015
         */
        private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectTwoTitle == m_newSubjectTitle) return;
            if (m_subjectTwoTitle == m_currentSubject.SubjectTitle) return;
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectTwo, m_subjectTwo.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectTwoPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        } /* private void subjectTwoPanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  subjectThreePanel_MouseDown() - presents third subject UI and its data
         *  
         * SYNOPSIS
         *  private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler is triggered upon clickig the panel representing the third 'Subject' in the
         *  notebook. It checks to see if this subject is currently empty or if it is already presented before
         *  it performs its tasks. Its tasks include saving the current page, updating the user interface to the
         *  third 'Subject', and updating the colors for 'Subject' tabs and their titles. It presents the page
         *  that was last used in this 'Subject' in order to deliver a more appealing functionality to the user.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:14pm 5/19/2015
         */
        private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectThreeTitle == m_newSubjectTitle) return;
            if (m_subjectThreeTitle == m_currentSubject.SubjectTitle) return;
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectThree, m_subjectThree.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectThreePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        } /* private void subjectThreePanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  subjectFourPanel_MouseDown() - presents fourth subject UI and its data
         *  
         * SYNOPSIS
         *  private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler is triggered upon clickig the panel representing the fourth 'Subject' in the
         *  notebook. It checks to see if this subject is currently empty or if it is already presented before
         *  it performs its tasks. Its tasks include saving the current page, updating the user interface to the
         *  fourth 'Subject', and updating the colors for 'Subject' tabs and their titles. It presents the page
         *  that was last used in this 'Subject' in order to deliver a more appealing functionality to the user.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:15pm 5/19/2015
         */
        private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFourTitle == m_newSubjectTitle) return;
            if (m_subjectFourTitle == m_currentSubject.SubjectTitle) return;
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectFour, m_subjectFour.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectFourPanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        } /* private void subjectFourPanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  subjectFivePanel_MouseDown() - presents fifth subject UI and its data
         *  
         * SYNOPSIS
         *  private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This event handler is triggered upon clickig the panel representing the fifth 'Subject' in the
         *  notebook. It checks to see if this subject is currently empty or if it is already presented before
         *  it performs its tasks. Its tasks include saving the current page, updating the user interface to the
         *  fifth 'Subject', and updating the colors for 'Subject' tabs and their titles. It presents the page
         *  that was last used in this 'Subject' in order to deliver a more appealing functionality to the user.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:17pm 5/19/2015
         */
        private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_subjectFiveTitle == m_newSubjectTitle) return;
            if (m_subjectFiveTitle == m_currentSubject.SubjectTitle) return;
            saveCurrentPageDisplayed();
            updateCurrentPageDisplayForSubject(m_subjectFive, m_subjectFive.CurrentPageNumber);
            setDefaultBackColorForTabs();
            subjectFivePanel.BackColor = SystemColors.ControlDark;
            this.Invalidate();
        } /* private void subjectFivePanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  setDefaultBackColorForTabs() - sets default colors for 'Subject' tabs
         *  
         * SYNOPSIS
         *  private void setDefaultBackColorForTabs();
         * 
         * DESCRIPTION
         *  This method is called to update the default back color for panels that represent 'Subject' tabs in
         *  the notebook. It gets called upon startup and during the application usage.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:20pm 5/19/2015
         */
        private void setDefaultBackColorForTabs()
        {
            subjectOnePanel.BackColor = SystemColors.ControlLight;
            subjectTwoPanel.BackColor = SystemColors.ControlLight;
            subjectThreePanel.BackColor = SystemColors.ControlLight;
            subjectFourPanel.BackColor = SystemColors.ControlLight;
            subjectFivePanel.BackColor = SystemColors.ControlLight;
        } /* private void setDefaultBackColorForTabs() */

        #endregion

        // Region contains methods that handle Pages in each Subject
        #region Pages Methods

        /*
         * NAME
         *  prevPageButton_Click() - updates UI and data elements for page before current
         * 
         * SYNOPSIS
         *  private void prevPageButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler method handles tasks associated with when the user clicks the button to go to the previous
         *  page, if they are currently not on the first page. It first saves the current page by calling the appropriate
         *  method. Then, it updates the user interface elements based on current values.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:19pm 5/20/2015
         */
        private void prevPageButton_Click(object sender, EventArgs e)
        {
            if (m_currentPageNumber == 1) return;
            saveCurrentPageDisplayed();
            m_currentPageNumber--;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updateCurrentPageDisplayForSubject(m_currentSubject, m_currentPageNumber);
        } /* private void prevPageButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  nextPageButton_Click() - updates UI and data elements for next page
         * 
         * SYNOPSIS
         *  private void nextPageButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler method handles tasks associated when the user clicks the button to go to the next page.
         *  It first checks to see if the user is currently on the last page; if so, then the user is notified of the
         *  situation using a message box. Then, it checks to see if the user is currently on the last page and it is blank.
         *  If so, then the user is notified with another message of the situation. Checking to see if the current blank page
         *  is also the last page is done so because the user can create 'empty' pages in the middle of the 'Subject' by 
         *  deleting their content. This means empty pages are allowed as long as they are between populated pages and no more
         *  than one empty page is allowed to be used and saved at the end of each 'Subject'. Finally, this method saves the
         *  current page and updates the user interface elements with appropriate values before the page is 'turned'.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:23pm 5/20/2015
         */
        private void nextPageButton_Click(object sender, EventArgs e)
        {
            if (m_currentPageNumber == 50)
            {
                string endOfSubjectString = "Subject cannot exceed 50 pages. " +
                "Please start a new subject if you wish to continue.";
                MessageBox.Show(endOfSubjectString, "Maximum Page Error", MessageBoxButtons.OK);
                selectTextControl();
                return;
            }
            if ((currentPageIsEmpty() && m_currentPageNumber == m_currentSubject.TotalNumberOfPages) ||
                (currentPageIsEmpty() && m_currentSubject.TotalNumberOfPages == 0))
            {
                string blankPageString = "Current page is blank, please enter some content before you continue.";
                MessageBox.Show(blankPageString, "Blank Page Error", MessageBoxButtons.OK);
                selectTextControl();
                return;
            }
            saveCurrentPageDisplayed();
            m_currentPageNumber++;
            m_pageNumberLabel.Text = Convert.ToString(m_currentPageNumber);
            updateCurrentPageDisplayForSubject(m_currentSubject, m_currentPageNumber);
        } /* private void nextPageButton_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  currentPageIsEmpty() - checks to see if current page displayed is empty
         * 
         * SYNOPSIS
         *  private bool currentPageIsEmpty();
         *      
         * DESCRIPTION
         *  This method checks to see if the currently displayed page in the user interface is empty. It starts its testing
         *  one element at a time and returns false the moment any one of them has content that is used by current page.
         * 
         * RETURNS
         *  True if currently displayed page has no content, otherwise it returns false.
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:23pm 5/20/2015
         */
        private bool currentPageIsEmpty()
        {
            if (richTextBox.TextLength != 0) return false;
            if (m_shapesStorage.NumberOfShapes() != 0) return false;
            if (m_verticalTextList.Count != 0) return false;
            return true;
        } /* private bool currentPageIsEmpty() */

        /*
         * NAME
         *  saveCurrentPageDisplayed() - saves the content displayed on the current page
         * 
         * SYNOPSIS
         *  private void saveCurrentPageDisplayed();
         *      
         * DESCRIPTION
         *  This method is called to save the elements displayed on the current page to its 'Subject'. First, it gets the
         *  'Page' object corresponding to the current page number from the 'Subject' container instead of creating a new
         *  page. This is done to minimize the use of resources by not creating and disposing of objects dynamically. Second,
         *  it updates the values for variables in the page to be saved. VerticalText objects need special care, due to the
         *  complex nature of their class, that is why they are copied into the container individually. Third, the updated
         *  'Page' is reinserted in its 'Subject' container with its new values; including the current page, which is saved
         *  as the last page viewed in this 'Subject' in order to present a more appealing functionality to the user when
         *  they return to the 'Subject' during an application session.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:04am 5/21/2015
         */
        private void saveCurrentPageDisplayed()
        {
            Page pageToSave = m_currentSubject.getPageForPageNumber(m_currentPageNumber);
            pageToSave.RTFTextCode = richTextBox.Rtf;
            pageToSave.ShapeContainer = m_shapesStorage;
            pageToSave.VerticalTextList.Clear();
            foreach (VerticalText v in m_verticalTextList)
            {
                pageToSave.VerticalTextList.Add(v);
            }
            m_currentSubject.savePageWithPageNumber(pageToSave, m_currentPageNumber);
            m_currentSubject.CurrentPageNumber = m_currentPageNumber;
        } /* private void saveCurrentPageDisplayed() */

        /*
         * NAME
         *  updateCurrentPageDisplayForSubject() - updates the UI page based on subject and page number
         * 
         * SYNOPSIS
         *  private void updateCurrentPageDisplayForSubject(Subject a_subject, int a_pageNumber);
         *      a_subject           -> 'Subject' in which the page to be used is contained
         *      int a_pageNumber    -> indicates page number for the 'Page' in the 'Subject' to be used
         *      
         * DESCRIPTION
         *  This method is used to update the content for the page currently displayed to the user. First, it gets the
         *  'Page' object corresponding to the current page number from the 'Subject' container. If page does not exist,
         *  such as the case when the user is at the end of 'Subject', then a blank page is returned from the 'Subject'
         *  container. Second, content corresponding to the page to be displayed is populated in the user interface elements.
         *  VerticalText objects require special handling, due to the complex nature of their class, that is why each one
         *  is copied individually. Each one of them also has several buttons that must also be populated in the UI element.
         *  Finally, page number label is updated and content is redrawn on screen.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:40am 5/21/2015
         */
        private void updateCurrentPageDisplayForSubject(Subject a_subject, int a_pageNumber)
        {
            Page pageToDisplay = a_subject.getPageForPageNumber(a_pageNumber);
            richTextBox.Rtf = pageToDisplay.RTFTextCode;
            m_shapesStorage = pageToDisplay.ShapeContainer;
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
            canHideVertTextButtons = true;
            selectTextControl();
        } /* private void updateCurrentPageDisplayForSubject(Subject a_subject, int a_pageNumber) */

        #endregion
    }
}