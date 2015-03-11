using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
/*
 * This is one of several 'partial' classes of the MainForm class. It is responsible
 * for performing a specific task. Below are the related files of the MainForm class:
 * 
 * mainForm.cs - This file is the starting point of the MainForm class. It contains the
 *               constructor and is responsible for coordinating interactions between
 *               other parts of the class and the application
 *               
 * formMenuStrip.cs - This file handles events that are triggered by elements
 *                    of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 * formToolbar.cs - (You are here) This file is responsible for controls in the toolbar
 *                  and their events in the main form. (Ex: Font, Text, Color, Line...)
 *                  
 * formTextbox.cs - This file is responsible for appearance and events of the
 *                  richTextBox and its layers
 */
namespace My_Note
{
    public partial class MainForm : Form
    {

        private void textSelectButton_Click(object sender, EventArgs e)
        {
            //mslog("textSelectButton clicked");
            m_currentSelectedControl = e_SelectedControl.TEXT;
            setDefaultBackColorForControls();
            textSelectButton.BackColor = selectedControlButtonColor;
        }
        private void pencilSelectButton_Click(object sender, EventArgs e)
        {
            // Appearance
            //mslog("pencilSelectButton clicked");
            m_currentSelectedControl = e_SelectedControl.PENCIL;
            setDefaultBackColorForControls();
            pencilSelectButton.BackColor = selectedControlButtonColor;

            // Functionality
            Brush = true;
        }
        private void eraserSelectButton_Click(object sender, EventArgs e)
        {
            // Appearance
            //mslog("eraserSelectButton clicked");
            m_currentSelectedControl = e_SelectedControl.ERASER;
            setDefaultBackColorForControls();
            eraserSelectButton.BackColor = selectedControlButtonColor;

            // Functionality
            Brush = false;
        }

        /* This method gets called to reset the BackColor of all controls to default, it is
         * called by any control before it sets its BackColor to a color that indicates
         * that it has been selected  3/9/15 7:12pm */
        private void setDefaultBackColorForControls()
        {
            textSelectButton.BackColor = Color.Transparent;
            pencilSelectButton.BackColor = Color.Transparent;
            eraserSelectButton.BackColor = Color.Transparent;
            NWarrowButton.BackColor = Color.Transparent;
            NarrowButton.BackColor = Color.Transparent;
            NEarrowButton.BackColor = Color.Transparent;
            EarrowButton.BackColor = Color.Transparent;
            SEarrrowButton.BackColor = Color.Transparent;
            SarrowButton.BackColor = Color.Transparent;
            SWarrowButton.BackColor = Color.Transparent;
            WarrowButton.BackColor = Color.Transparent;
            squareSelectButton.BackColor = Color.Transparent;
            circleSelectButton.BackColor = Color.Transparent;
            solidSelectButton.BackColor = Color.Transparent;
            dashedSelectButton.BackColor = Color.Transparent;
            dottedSelectButton.BackColor = Color.Transparent;
            vertTextButton.BackColor = Color.Transparent;

        }
    }
}
