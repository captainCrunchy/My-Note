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
            TEXT, PENCIL, ERASER, WARROW, NWARROW, NARROW,
            NEARROW, EARROW, SEARROW, SARROW, SWARROW,
            RECTANGLE, ELLIPSE, SOLID, DASHED, DOTTED, VERTTEXT
        }

        private e_SelectedControl m_currentSelectedControl;
        private Color m_selectedControlButtonColor;

        public MainForm()
        {
            InitializeComponent();

            // Initialize variables and values of controls
            m_currentSelectedControl = e_SelectedControl.TEXT;
            m_selectedControlButtonColor = Color.SandyBrown;

            // Set default values
            textSelectButton.BackColor = m_selectedControlButtonColor;
            fontComboBox.Text = "Microsoft Sans Serif";
            richTextBox.Font = new Font("Microsoft Sans Serif", 12);
            m_transparentPanelGraphics = this.transparentPanel.CreateGraphics();
            m_transparentPanelPen = new Pen(m_currentDrawColor);
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            textSelectButton.Select();
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

        private void button1_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
        }
    }
}
