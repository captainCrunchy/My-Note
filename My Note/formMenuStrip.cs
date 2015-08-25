using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;

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
 *      readable. The following are descriptions of the four (.cs) files, including this one, that make up this class.
 *      
 *      mainForm.cs:
 *          This file implements tasks that are responsible for starting and running the application. It performs general tasks
 *          like handling the user inteface elements of the form and communication with data persistence objects. It is also
 *          responsible for coordinating tasks between other 'partial class' files.
 *      formMenuStrip.cs: (CURRENTLY HERE)
 *          This file handles events that are triggered by elements of the menu strip in the form and their appearances based
 *          on current data. Example: File, Edit, ..., Help.
 *      formToolbar.cs:
 *          This file is responsible for appearance of controls in the toolbar and their events. These controls trigger such
 *          tasks as text editing, drawing shapes, and erasing.
 *      formTextBox.cs:
 *          This file is responsible for appearances and events of the richTextBox and its layers. Such additional layers are
 *          the transparent panel and the background panel. Events handled in this files are tasks such as applying text editing
 *          and drawing shapes onto the panels, and erasing them based on currently selected controls and options. The mechanics
 *          of drawing certain shapes like arrows, rectangles, ovals, and lines have been separated into two categories. First
 *          category is when the user has the mouse down and is moving it, shapes are being drawn and displayed at optimal speed.
 *          Second category is when the user releases the mouse, shapes are saved using individual points and are redrawn again
 *          from the saved container, this is done in order to accomodate the erase functionality.
 *      
 *  CODE STRUCTURE:
 *      Member Variables - Region contains member variables for this class
 *      File Menu Item Methods - Region contains methods for 'File' menu items
 *      Edit Menu Item Methods - Region contains methods for 'Edit' menu items
 *      Help Menu Item Methods - Region contains methods for 'Help' menu items
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        // Region contains member variables for this class
        #region Member Variables

        private Bitmap m_pageToPrint = new Bitmap(520, 605);  // Used to capture current page for printing, as early as possible
        private int m_autoSaveTimeInterval = 0;               // Determines how often current work gets automatically saved
        private Timer m_autoSaveTimer = new Timer();          // Used to save current content automatically based on set time
        private Timer m_autoSaveNotifyTimer = new Timer();    // Used to help label 'fade out' after auto-save notification
        private Label m_autoSaveNotifyLabel = new Label();    // Appears in top left corner of form to notify the user when an
                                                              // an auto-save task has executed, then gradually fades out.

        #endregion

        // Region contains methods for 'File' menu items
        #region File Menu Item Methods

        /*
         * NAME
         *  saveToolStripMenuItem_Click() - saves current content
         * 
         * SYNOPSIS
         *  private void saveToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to save all changes up to this point. It calls
         *  a method which updates all data in data persistence objects and writes them to disk.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:02am 5/20/2015
         */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAllContent();
        } /* private void saveToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  previewMarginsAreaToolStripMenuItem_Click() - brings up a print preview dialog
         * 
         * SYNOPSIS
         *  private void previewMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to preview current page before it is printed.
         *  The current page is sized to fit the portion of the print page inside the margins. The
         *  technique used in this method is necessary in order to capture the contents of two
         *  'overlaying' elements at once. First one is the transparent panel and below it is the
         *  rich text box. This strategy was taken from an online forum and properly documented.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  http://stackoverflow.com/questions/4974276/richtextbox-drawtobitmap-does-not-draw-containing-text
         *  
         * DATE
         *  8:56am 5/28/2015
         */
        private void previewMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromImage(m_pageToPrint))
            {
                gr.CopyFromScreen(transparentPanel.PointToScreen(Point.Empty), Point.Empty, transparentPanel.Size);
            }
            PrintDocument documentToPreview = new PrintDocument();
            documentToPreview.OriginAtMargins = false;
            documentToPreview.PrintPage += marginsArea_PrintPage;
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.ShowIcon = false;
            previewDialog.Document = documentToPreview;
            previewDialog.ShowDialog();
        } /* private void previewMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  previewFullPageAreaToolStripMenuItem_Click() - brings up a print preview dialog
         * 
         * SYNOPSIS
         *  private void previewFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to preview current page before it is printed.
         *  The current page is sized to fit the full area of the print page. The technique used in
         *  this method is necessary in order to capture the contents of two 'overlaying' elements
         *  at once. First one is the transparent panel and below it is the rich text box. This
         *  strategy was taken from an online forum and properly documented.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  http://stackoverflow.com/questions/4974276/richtextbox-drawtobitmap-does-not-draw-containing-text
         *  
         * DATE
         *  9:12am 5/28/2015
         */
        private void previewFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromImage(m_pageToPrint))
            {
                gr.CopyFromScreen(transparentPanel.PointToScreen(Point.Empty), Point.Empty, transparentPanel.Size);
            }
            PrintDocument documentToPreview = new PrintDocument();
            documentToPreview.OriginAtMargins = false;
            documentToPreview.PrintPage += fullPageArea_PrintPage;
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.ShowIcon = false;
            previewDialog.Document = documentToPreview;
            previewDialog.ShowDialog();
        } /* private void previewFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  printMarginsAreaToolStripMenuItem_Click() - brings up a print dialog
         * 
         * SYNOPSIS
         *  private void printMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to print the current page. Current page is
         *  sized to fit the portion of the print page inside the margins. The technique used in this
         *  method is necessary in order to capture the contents of two 'overlaying' elements at once.
         *  First one is the transparent panel and below it is the rich text box. This strategy was
         *  taken from an online forum and properly documented.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  http://stackoverflow.com/questions/4974276/richtextbox-drawtobitmap-does-not-draw-containing-text
         *  
         * DATE
         *  9:17am 5/28/2015
         */
        private void printMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromImage(m_pageToPrint))
            {
                gr.CopyFromScreen(transparentPanel.PointToScreen(Point.Empty), Point.Empty, transparentPanel.Size);
            }
            PrintDocument documentToPrint = new PrintDocument();
            documentToPrint.OriginAtMargins = false;
            documentToPrint.PrintPage += marginsArea_PrintPage;
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = documentToPrint;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                documentToPrint.Print();
            }
        } /* private void printMarginsAreaToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  printFullPageAreaToolStripMenuItem_Click() - brings up a print dialog
         * 
         * SYNOPSIS
         *  private void printFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to print the current page. Current page is
         *  sized to fit the full area of the print page. The technique used in this method is
         *  necessary in order to capture the contents of two 'overlaying' elements at once. First
         *  one is the transparent panel and below it is the rich text box. This strategy was taken
         *  from an online forum and properly documented.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  http://stackoverflow.com/questions/4974276/richtextbox-drawtobitmap-does-not-draw-containing-text
         *  
         * DATE
         *  9:47am 5/28/2015
         */
        private void printFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromImage(m_pageToPrint))
            {
                gr.CopyFromScreen(transparentPanel.PointToScreen(Point.Empty), Point.Empty, transparentPanel.Size);
            }
            PrintDocument documentToPrint = new PrintDocument();
            documentToPrint.OriginAtMargins = false;
            documentToPrint.PrintPage += fullPageArea_PrintPage;
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = documentToPrint;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                documentToPrint.Print();
            }
        } /* private void printFullPageAreaToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  marginsArea_PrintPage() - creates a printable graphics area
         * 
         * SYNOPSIS
         *  private void marginsArea_PrintPage(object sender, PrintPageEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler gets assigned to a PrintPage event of a PrintDocument object. It uses
         *  an image stored in a local variable to fill graphics that are to be printed. The DrawImage()
         *  method sizes the image to fit the portion of the print page inside the margins. This
         *  method gets used for preview and print tasks.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:05am 5/28/2015
         */
        private void marginsArea_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage((Image)m_pageToPrint, e.MarginBounds);
        } /* private void marginsArea_PrintPage(object sender, PrintPageEventArgs e) */

        /*
         * NAME
         *  fullPageArea_PrintPage() - creates a printable graphics area
         * 
         * SYNOPSIS
         *  private void fullPageArea_PrintPage(object sneder, PrintPageEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler gets assigned to a PrintPage event of a PrintDocument object. It uses
         *  an image stored in a local variable to fill graphics that are to be printed. The DrawImage()
         *  method sizes the image to fit the full area of the print page. This method gets used for
         *  preview and print tasks.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:14am 5/28/2015
         */
        private void fullPageArea_PrintPage(object sneder, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage((Image)m_pageToPrint, e.PageBounds);
        } /* private void fullPageArea_PrintPage(object sneder, PrintPageEventArgs e) */

        /*
         * NAME
         *  closeToolStripMenuItem_Click() - triggers a task to close this form
         * 
         * SYNOPSIS
         *  private void closeToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler gets triggered by the user upon selecting the 'Close' option in the
         *  'File' menu. It does not close this form immediately because the MainForm_FormClosing()
         *  event handler will be triggered to handle the necessary confirmations from the user.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:09pm 5/27/2015
         */
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        } /* private void closeToolStripMenuItem_Click(object sender, EventArgs e) */

        #endregion

        // Region contains methods for 'Edit' menu items
        #region Edit Menu Item Methods

        /*
         * NAME
         *  addNewSubjectToolStripMenuItem_Click() - adds new 'Subject' to the application
         * 
         * SYNOPSIS
         *  private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to add a new 'Subject' to this application.
         *  First, it prompts the user to enter a title for the new 'Subject', which cannot match
         *  an existing title. The invalid titles are passed to a container in the RenameSubjectForm
         *  object and tested there. Upon user clicking 'OK' in the result dialog, current page is
         *  saved to its 'Subject'. Then the new 'Subject' is selected from the first available empty
         *  subject in the data persistence object and is assigned the new, user entered, non-default
         *  title. Next, this new 'Subject' is displayed to the user by becoming the new focus of the
         *  user interface. Finally, all UI elements are redrawn based on updated values. Data is
         *  saved in the data persistence object but it is not written to disk in this method.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:08pm 5/21/2015
         */
        private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult newNameDialog = new DialogResult();
            RenameSubjectForm newNameSubjectForm = new RenameSubjectForm();
            newNameSubjectForm.SubjectTitle = m_newSubjectTitle;
            foreach (Subject s in m_mainMyNoteStore.SavedSubjects)
            {
                string existingTitle = s.SubjectTitle;
                newNameSubjectForm.InvalidTitles.Add(existingTitle);
            }
            newNameSubjectForm.FormTitle = "New Subject Title";
            newNameSubjectForm.StartPosition = FormStartPosition.CenterParent;
            newNameSubjectForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            newNameSubjectForm.MaximizeBox = false;
            newNameSubjectForm.MinimizeBox = false;
            newNameDialog = newNameSubjectForm.ShowDialog();
            if (newNameDialog == DialogResult.OK)
            {
                saveCurrentPageDisplayed();
                int nextSubjectIndex = m_mainMyNoteStore.NumberOfSubjects();
                m_currentSubject = m_mainMyNoteStore.SavedSubjects[nextSubjectIndex];
                m_currentSubject.SubjectTitle = newNameSubjectForm.SubjectTitle;
                m_currentPageNumber = 1;
                updateCurrentPageDisplayForSubject(m_currentSubject, m_currentSubject.CurrentPageNumber);
                setDefaultBackColorForTabs();
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
            }
            updateToolStripMenuItems();
        } /* private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  renameSubjectToolStripMenuItem_Click() - renames the currently displayed subject
         * 
         * SYNOPSIS
         *  private void renameSubjectToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to rename the currently displayed subject.
         *  First, it prompts the user to enter a new title for the current 'Subject', which cannot
         *  match an existing title. The invalid titles are passed to a container in the RenameSubjectForm
         *  object and tested there. Upon user clicking 'OK' in the result dialog, title of the
         *  currently displayed subject is updated and UI elements are redrawn. Data is not saved in
         *  the data persistence object and is not written to disk from this method.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:41pm 5/21/2015
         */
        private void renameSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult renameDialog = new DialogResult();
            RenameSubjectForm renameSubjectForm = new RenameSubjectForm();
            renameSubjectForm.SubjectTitle = m_currentSubject.SubjectTitle;
            foreach (Subject s in m_mainMyNoteStore.SavedSubjects)
            {
                string existingTitle = s.SubjectTitle;
                renameSubjectForm.InvalidTitles.Add(existingTitle);
            }
            renameSubjectForm.FormTitle = "Change Subject Title";
            renameSubjectForm.StartPosition = FormStartPosition.CenterParent;
            renameSubjectForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            renameSubjectForm.MaximizeBox = false;
            renameSubjectForm.MinimizeBox = false;
            renameDialog = renameSubjectForm.ShowDialog();
            if (renameDialog == DialogResult.OK)
            {
                m_currentSubject.SubjectTitle = renameSubjectForm.SubjectTitle;
                assignSubjectsAndTitles();
                this.Invalidate();
            }
        } /* private void renameSubjectToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  deleteSubjectToolStripMenuItem_Click() - deletes the currently displayed subject
         * 
         * SYNOPSIS
         *  private void deleteSubjectToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to delete the currently displayed 'Subject'.
         *  First, it asks the user to confirm a subject delete. Upon user clicking 'OK', this method
         *  iterates through the container of the data persistence object and removes the currently
         *  marked subject. Existing subjects 'move up' towards the first one and a new empty subject
         *  is appended to the end of the container. Finally, UI elements are redrawn on screen based
         *  on current data values.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:08pm 5/25/2015
         */
        private void deleteSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string deleteSubjectString = "Are you sure you want to delete this subject?";
            DialogResult deleteSubjectDialog = MessageBox.Show(deleteSubjectString,
                "Confirm Delete", MessageBoxButtons.OKCancel);
            if (deleteSubjectDialog == DialogResult.OK)
            {
                foreach (Subject s in m_mainMyNoteStore.SavedSubjects)
                {
                    if (m_currentSubject.SubjectTitle == s.SubjectTitle)
                    {
                        m_mainMyNoteStore.SavedSubjects.Remove(s);
                        Subject endSubject = new Subject();
                        m_mainMyNoteStore.SavedSubjects.Add(endSubject);
                        break;
                    }
                }
                m_currentSubject = m_mainMyNoteStore.SavedSubjects[0];
                setDefaultBackColorForTabs();
                subjectOnePanel.BackColor = SystemColors.ControlDark;
                assignSubjectsAndTitles();
                updateCurrentPageDisplayForSubject(m_currentSubject, m_currentSubject.CurrentPageNumber);
                this.Invalidate();
                updateToolStripMenuItems();
            }
        } /* private void deleteSubjectToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  updateToolStripMenuItems() - updates availability of tool strip menu items
         * 
         * SYNOPSIS
         *  private void updateToolStripMenuItems();
         *      
         * DESCRIPTION
         *  This method is used to set and update the availability of tool strip menu items based on
         *  existing data values. If there is only one 'Subject' being used in the application, then
         *  the 'Delete Subject' option is disabled. If all the subjects are currently being used in
         *  the application, then the 'Add Subject' option is disabled. This method gets called when
         *  the application loads or whenever any 'Subject' related modifications are being made.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
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
        } /* private void updateToolStripMenuItems() */

        /*
         * NAME
         *  oneMinuteToolStripMenuItem_Click() - set the auto-save option to one minute
         * 
         * SYNOPSIS
         *  private void oneMinuteToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to set the auto-save option to one minute
         *  intervals. It updates the local and data persistence variables to the value of the user
         *  preferred option. Then check marks are updated in the menu appropriately. Finally, it
         *  calls a method to adjust the behavior of the timer responsible for automatically saving
         *  current work. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:28am 5/28/2015
         */
        private void oneMinuteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_autoSaveTimeInterval = 1;
            m_mainMyNoteStore.AutoSaveTimeInterval = 1;
            updateCheckMarksForAutoSaveItems();
            updateAutoSaveTimer();
        } /* private void oneMinuteToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  fiveMinuteToolStripMenuItem_Click() - set the auto-save option to five minutes
         * 
         * SYNOPSIS
         *  private void fiveMinuteToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to set the auto-save option to five minute
         *  intervals. It updates the local and data persistence variables to the value of the user
         *  preferred option. Then check marks are updated in the menu appropriately. Finally, it
         *  calls a method to adjust the behavior of the timer responsible for automatically saving
         *  current work. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:31am 5/28/2015
         */
        private void fiveMinutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_autoSaveTimeInterval = 5;
            m_mainMyNoteStore.AutoSaveTimeInterval = 5;
            updateCheckMarksForAutoSaveItems();
            updateAutoSaveTimer();
        } /* private void fiveMinutesToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  tenMinuteToolStripMenuItem_Click() - set the auto-save option to ten minutes
         * 
         * SYNOPSIS
         *  private void tenMinuteToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to set the auto-save option to ten minute
         *  intervals. It updates the local and data persistence variables to the value of the user
         *  preferred option. Then check marks are updated in the menu appropriately. Finally, it
         *  calls a method to adjust the behavior of the timer responsible for automatically saving
         *  current work. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:33am 5/28/2015
         */
        private void tenMinutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_autoSaveTimeInterval = 10;
            m_mainMyNoteStore.AutoSaveTimeInterval = 10;
            updateCheckMarksForAutoSaveItems();
            updateAutoSaveTimer();
        } /* private void tenMinutesToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  noneToolStripMenuItem_Click() - sets the auto-save option to 'None'
         * 
         * SYNOPSIS
         *  private void noneToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to disable the auto-save option. It uses the
         *  '0' value as a reference to indicate that no auto-save will be used. It updates the local
         *  and data persistence variables to remember this value. Then check marks are updated in
         *  the menu appropriately. Finally, it calls a method to adjust the behavior of the timer
         *  responsible for automatically saving current work.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:35am 5/28/2015
         */
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_autoSaveTimeInterval = 0;
            m_mainMyNoteStore.AutoSaveTimeInterval = 0;
            updateCheckMarksForAutoSaveItems();
            updateAutoSaveTimer();
        } /* private void noneToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  updateCheckMarksForAutoSaveItems() - updates the 'Checkmark' properties for menu items
         * 
         * SYNOPSIS
         *  private void updateCheckMarksForAutoSaveItems();
         *      
         * DESCRIPTION
         *  This method gets called to update the 'Checkmark' properties for menu items in the
         *  'Auto Save' sub menu, based on current values, to indicate to the user current setting
         *  for the auto-save feature. It gets called once on startup and then anytime a new auto-save
         *  option is selected from the menu.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:46am 5/28/2015
         */
        private void updateCheckMarksForAutoSaveItems()
        {
            oneMinuteToolStripMenuItem.Checked = false;
            fiveMinutesToolStripMenuItem.Checked = false;
            tenMinutesToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = false;
            switch (m_autoSaveTimeInterval)
            {
                case (1):
                    oneMinuteToolStripMenuItem.Checked = true;
                    break;
                case (5):
                    fiveMinutesToolStripMenuItem.Checked = true;
                    break;
                case (10):
                    tenMinutesToolStripMenuItem.Checked = true;
                    break;
                case (0):
                    noneToolStripMenuItem.Checked = true;
                    break;
                default:
                    break;
            }
        } /* private void updateCheckMarksForAutoSaveItems() */

        /*
         * NAME
         *  updateAutoSaveTimer() - updates the values for the auto-save timer
         * 
         * SYNOPSIS
         *  private void updateAutoSaveTimer();
         *      
         * DESCRIPTION
         *  This method gets called to update the values for the auto-save timer and adjust its
         *  behavior. A new time interval (in milliseconds) is assigned to the m_autoSaveTimer
         *  variable or it is disabled based on the user's selection. Value used for testing the
         *  user selection is the local variable which gets saved in the data persistence object
         *  to remember the user 'preference'. If the user is changing the time interval, then the
         *  timer stops, a new interval is assigned, and timer starts again.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  11:36am 5/28/2015
         */
        private void updateAutoSaveTimer()
        {
            int newTimerInterval = 0;
            switch (m_autoSaveTimeInterval)
            {
                case (1):
                    newTimerInterval = 60000;
                    break;
                case (5):
                    newTimerInterval = 300000;
                    break;
                case (10):
                    newTimerInterval = 600000;
                    break;
                default:
                    break;
            }
            // disable auto-save (or leave it disabled)
            if (m_autoSaveTimeInterval == 0)
            {
                // currently running
                if (m_autoSaveTimer.Enabled == true)
                {
                    m_autoSaveTimer.Stop();
                }
            }
            // enable auto-save (or update the time interval)
            else
            {
                // currently not running
                if (m_autoSaveTimer.Enabled == false)
                {
                    m_autoSaveTimer.Interval = newTimerInterval;
                    m_autoSaveTimer.Start();
                }
                // currently running
                else
                {
                    m_autoSaveTimer.Stop();
                    m_autoSaveTimer.Interval = newTimerInterval;
                    m_autoSaveTimer.Start();
                }
            }
        } /* private void updateAutoSaveTimer() */

        /*
         * NAME
         *  TimerEventProcessor() - performs an auto-save task
         * 
         * SYNOPSIS
         *  private void TimerEventProcessor(Object a_object, EventArgs a_eventArgs);
         *      a_object        -> does nothing
         *      a_eventArgs     -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is assigned to the local 'm_autoSaveTimer' variable's 'Tick' event
         *  in the constructor in mainForm.cs. It first triggers a save task, which saves all current
         *  content to a data persistence object and writes it to disk. Then it strategically utilizes
         *  a 'notify' event, which mechanically adds a label to the formm and triggers an additional
         *  timer. The new label 'subtly' shows up in top left corner of the form to remind the user
         *  that the application has just automatically saved current content. This new timer is then
         *  used to help to make the notification label fade out and disappear. This method and its
         *  usage was taken directly from a MSDN website and properly documented. Change made is the
         *  addition of the custom 'notify' technique.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  https://msdn.microsoft.com/en-us/library/system.windows.forms.timer.tick(v=vs.110).aspx
         *  
         * DATE
         *  11:03am 5/28/2015
         */
        private void TimerEventProcessor(Object a_object, EventArgs a_eventArgs)
        {
            saveAllContent();
            // prevent adding too many labels
            if (!this.Controls.Contains(m_autoSaveNotifyLabel))
            {
                this.Controls.Add(m_autoSaveNotifyLabel);
            }
            m_autoSaveNotifyTimer.Start();
        } /* private void TimerEventProcessor(Object a_object, EventArgs a_eventArgs) */

        /*
         * NAME
         *  autoSaveNotify() - helps notify the user that an auto-save event has just occurred
         * 
         * SYNOPSIS
         *  private void autoSaveNotify(Object a_object, EventArgs a_eventArgs);
         *      a_object        -> does nothing
         *      a_eventArgs     -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is assigned to the local 'm_autoSaveNotifyTimer' variable's 'Tick'
         *  event in the constructor in mainForm.cs. It is triggered by and is meant to work with
         *  'TimerEventProcessor()' event when an auto-save task occurs. Its task is to visually
         *  fade the notification label and remove it from the 'Controls' container. The 'fade'
         *  process uses the RGB values and fadingSpeed to gradually increase the RGB values for
         *  the ForeColor of the label until it becomes transparent while the autoSaveNotify() event
         *  is being called. It then removes the label from the form and stops the notify timer.
         *  This technique was taken from an online forum and is properly documented.
         *  
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi using code from:
         *  http://stackoverflow.com/questions/5470967/creating-a-fade-out-label
         *  
         * DATE
         *  12:53pm 5/28/2015
         */
        private void autoSaveNotify(Object a_object, EventArgs a_eventArgs)
        {
            int fadingSpeed = 3;
            m_autoSaveNotifyLabel.ForeColor = Color.FromArgb(m_autoSaveNotifyLabel.ForeColor.R + fadingSpeed,
                m_autoSaveNotifyLabel.ForeColor.G + fadingSpeed, m_autoSaveNotifyLabel.ForeColor.B + fadingSpeed);
            if (m_autoSaveNotifyLabel.ForeColor.R >= this.BackColor.R)
            {
                m_autoSaveNotifyTimer.Stop();
                m_autoSaveNotifyLabel.ForeColor = this.ForeColor;
                if (this.Controls.Contains(m_autoSaveNotifyLabel))
                {
                    this.Controls.Remove(m_autoSaveNotifyLabel);
                }
            }
        } /* private void autoSaveNotify(Object a_object, EventArgs a_eventArgs) */

        #endregion

        // Region contains methods for 'Help' menu items
        #region Help Menu Item Methods

        /*
         * NAME
         *  aboutToolStripMenuItem_Click() - displays a form with information about this application
         * 
         * SYNOPSIS
         *  private void aboutToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is displays a form with short information about this application.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:35am 5/31/2015
         */
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.StartPosition = FormStartPosition.CenterParent;
            aboutForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            aboutForm.ShowDialog();
        } /* private void aboutToolStripMenuItem_Click(object sender, EventArgs e) */

        /*
         * NAME
         *  myNoteHelpToolStripMenuItem_Click - displays a form with help information
         * 
         * SYNOPSIS
         *  private void myNoteHelpToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is displays a form with information that provides help to the user of
         *  this application.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:15am 5/31/2015
         */
        private void myNoteHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.StartPosition = FormStartPosition.CenterParent;
            helpForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            helpForm.ShowDialog();
        } /* private void myNoteHelpToolStripMenuItem_Click(object sender, EventArgs e) */

        #endregion
    }
}
