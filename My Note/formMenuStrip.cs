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
 *    formMenuStrip.cs: (YOU ARE HERE)
 *      This file handles events that are triggered by elements of the menu strip in the form and their appearances based on
 *      current data. Example: File, Edit, ..., Help.
 *    formToolbar.cs:
 *      This file is responsible for appearance of controls in the toolbar and their events. These controls trigger such tasks
 *      as text editing, drawing shapes, and erasing.
 *    formTextBox.cs:
 *      This file is responsible for appearances and events of the richTextBox and its layers. Such additional layers are
 *      transparent and background panels. Events handled in this files are tasks such as applying text editing and drawing
 *      shapes onto the panels, and erasing them based on currently selected controls and options.
 *      
 *  CODE STRUCTURE:
 *    MainForm class:
 *      This class is divided into four (.cs) files based on functionality. Each is responsible for performing specific tasks
 *      based on the user interface elements and controls. Each (.cs) file declares and initializes member variables that are
 *      needed in that file. Some member variables can only be initialized in the constructor, which is in the mainForm.cs file.
 *    formMenuStrip.cs: (YOU ARE HERE)
 *      This file organizes code by placing the event handler methods for UI elements towards the beginning and all helper
 *      methods towards the end.
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
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
         *  This event handler is triggered by the user to save all changes up to this point. It calls a
         *  method which updates all data in data persistence objects and writes them to disk.
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
         *  addNewSubjectToolStripMenuItem_Click() - adds new 'Subject' to the application
         * 
         * SYNOPSIS
         *  private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This event handler is triggered by the user to add a new 'Subject' to this application. First, it
         *  prompts the user to enter a title for the new 'Subject', which cannot match an existing title. The
         *  invalid titles are passed to a container in the RenameSubjectForm object and tested there. Upon user
         *  clicking 'OK' in the result dialog, current page is saved to its 'Subject'. Then the new 'Subject'
         *  is selected from the first available empty subject in the data persistence object and is assigned the
         *  new, user entered, non-default title. Next, this new 'Subject' is displayed to the user by becoming the
         *  new focus of the user interface. Finally, all UI elements are redrawn based on updated values. Data is
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
         *  This event handler is triggered by the user to rename the currently displayed subject. First, it prompts
         *  the user to enter a new title for the current 'Subject', which cannot match an existing title. The invalid
         *  titles are passed to a container in the RenameSubjectForm object and tested there. Upon user clicking 'OK'
         *  in the result dialog, title of the currently displayed subject is updated and UI elements are redrawn. Data
         *  is not saved in the data persistence object and is not written to disk from this method.
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
         *  This event handler is triggered by the user to delete the currently displayed 'Subject'. First, it asks
         *  the user to confirm a subject delete. Upon user clicking 'OK', this method iterates through the container
         *  of the data persistence object and removes the currently marked subject. Existing subjects 'move up' towards
         *  the first one and a new empty subject is appended to the end of the container. Finally, UI elements are
         *  redrawn on screen based on current data values.
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
         *  This method is used to set and update the availability of tool strip menu items based on existing data
         *  values. If there is only one 'Subject' being used in the application, then the 'Delete Subject' option
         *  is disabled. If all the subjects are currently being used in the application, then the 'Add Subject'
         *  option is disabled. This method gets called when the application loads or whenever any 'Subject' related
         *  modifications are being made.
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
    }
}