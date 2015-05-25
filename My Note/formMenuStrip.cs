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
 *      formMenuStrip.cs - (YOU ARE HERE) This file handles events that are triggered by
 *                         elements of the menu strip in the form. (Ex: File, Edit, ... Help)
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

        /*
         * 8:02am 5/20/2015
         */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAllContent();
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
            foreach (Subject s in m_mainMyNoteStore.SavedSubjects)
            {
                string existingTitle = s.SubjectTitle;
                renameSubjectForm.InvalidTitles.Add(existingTitle);
            }
            renameSubjectForm.FormTitle = "Rename Subject Title";
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
        }

        /*  This belongs in the formMenuStrip.cs
         *  5:08pm 5/21/2015
         */
        private void addNewSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if last subject was added, then delete subject is disabled
            //updateToolStripMenuItems(); // should this be on the bottom?

            // This can be created 'on the fly' because it will not be done very often
            DialogResult renameDialog = new DialogResult();
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
            renameDialog = newNameSubjectForm.ShowDialog();
            if (renameDialog == DialogResult.OK)
            {
                // Save current subject
                //saveCurrentSubjectDisplayed();
                saveCurrentPageDisplayed();

                // Create new subject
                
                // nextSubjectIndex uses a number of subjects so that a new subject can be
                // added to the very end.
                int nextSubjectIndex = m_mainMyNoteStore.NumberOfSubjects();
                m_currentSubject = m_mainMyNoteStore.SavedSubjects[nextSubjectIndex];
                m_currentSubject.SubjectTitle = newNameSubjectForm.SubjectTitle;
                m_currentPageNumber = 1;
                // page display will be updated to empty subject because each subject
                updateCurrentPageDisplayForSubject(m_currentSubject, m_currentSubject.CurrentPageNumber);
                
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
                //assignSubjectsAndTitles();
                //this.Invalidate();
            }
            updateToolStripMenuItems();
        }

        /*
         *  1:08pm 5/25/2015
         */
        private void deleteSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // confirm delete with user
            string deleteSubjectString = "Are you sure you want to delete this subject?";
            DialogResult deleteSubjectDialog = MessageBox.Show(deleteSubjectString, "Confirm Delete", MessageBoxButtons.OKCancel);

            // get current subject to delete
            if (deleteSubjectDialog == DialogResult.OK)
            {
                // Deleting
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

            // remove from data

            // reload ui
            
        }

        /*  This belongs in the formMenuStrip.cs
         *  If there is only one subject that, then disable DeleteSubject menu item
         *  If there are already Five Subjects, then disable AddSubject menu item.
         *  5:11pm 5/21/2015
         *  Called from:
         *  addNewSubjectToolStripMenuItem_Click(),
         *  MainForm_Shown()
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

        // Temp
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
    }
}