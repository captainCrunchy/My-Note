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
 * This is the main form and is the starting point of the application. For clarity and
 * organization, it is divided into several files, which are all responisble for performing
 * a specific task. The files are simply extensions of this class, i.e. '.. partial class ...'
 * Below are the related files of the MainForm class:
 * 
 * mainForm.cs - (You are here) This file is the starting point of the MainForm class. It
 *               contains the constructor and is responsible for coordinating interactions
 *               between other parts of the class and the application
 *               
 * formMenuStrip.cs - This file handles events that are triggered by elements
 *                    of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 * formToolbar.cs - This file is responsible for controls in the toolbar and
 *                  their events in the main form. (Ex: Font, Text, Color, Line...)
 *                  
 * formTextbox.cs - This file is responsible for appearance and events of the
 *                  richTextBox and its layers
 */
namespace My_Note
{
    public partial class MainForm : Form
    {
        private enum e_SelectedControl
        {
            TEXT,
            PENCIL,
            ERASER
        }

        private e_SelectedControl m_currentSelectedControl;
        private Color selectedControlButtonColor;

        public MainForm()
        {
            InitializeComponent();
            /*
            //Set Double Buffering (Set by original PaintPanel project)
            transparentPanel.GetType().GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.NonPublic).Invoke(transparentPanel, 
                new object[] { System.Windows.Forms.ControlStyles.UserPaint | 
                    System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | 
                    System.Windows.Forms.ControlStyles.DoubleBuffer, true });
            */

            // Initialize variables and values of controls
            m_currentSelectedControl = e_SelectedControl.TEXT;
            selectedControlButtonColor = Color.SandyBrown;

            // Set default values
            textSelectButton.BackColor = selectedControlButtonColor;
            fontComboBox.Text = "Microsoft Sans Serif";
            richTextBox.Font = new Font("Microsoft Sans Serif", 12);

        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            //mslog("form loaded");
            textSelectButton.Select();
        }

        private void MainForm_Activated(object sender, System.EventArgs e)
        {

        }

        private void MainForm_Closed(object sender, System.EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, System.EventArgs e)
        {

        }

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
                    //richTextBox.Font = new Font("Times New Roman", 12);
                    richTextBox.SelectionFont = new Font("Times New Roman", 12);
                    break;
            }
        }

        // Temp
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

        private void transparentPanel_MouseHover(object sender, EventArgs e)
        {
            //mslog("MouseHover");
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                transparentPanel.Cursor = Cursors.IBeam;
                //mslog("IBeam");
            }
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                transparentPanel.Cursor = Cursors.Cross;
                //mslog("Cross");
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                transparentPanel.Cursor = Cursors.Hand;
                //mslog("Hand");
            }
        }
    }
}
