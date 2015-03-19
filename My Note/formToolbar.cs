﻿using System;
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
        /*
         * 3/17/15 12:12pm
         */
        private void textSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.TEXT;
            setDefaultBackColorForControls();
            textSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/17/15 12:13pm
         */
        private void pencilSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.PENCIL;
            setDefaultBackColorForControls();
            pencilSelectButton.BackColor = m_selectedControlButtonColor;
            //Brush = true;
        }

        /*
         * 3/17/15 12:14pm
         */
        private void eraserSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.ERASER;
            setDefaultBackColorForControls();
            eraserSelectButton.BackColor = m_selectedControlButtonColor;
            //Brush = false;
        }
        
        /*
         * 3/17/15 12:15pm
         */
        private void WarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.WARROW;
            setDefaultBackColorForControls();
            WarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:27AM
         */
        private void NWarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.NWARROW;
            setDefaultBackColorForControls();
            NWarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:28AM
         */
        private void NarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.NARROW;
            setDefaultBackColorForControls();
            NarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:29AM
         */
        private void NEarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.NEARROW;
            setDefaultBackColorForControls();
            NEarrowButton.BackColor = m_selectedControlButtonColor;
        }
        /*
         * 3/19/15 7:30AM
         */
        private void EarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.EARROW;
            setDefaultBackColorForControls();
            EarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:31AM
         */
        private void SEarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.SEARROW;
            setDefaultBackColorForControls();
            SEarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:32AM
         */
        private void SarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.SARROW;
            setDefaultBackColorForControls();
            SarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:33AM
         */
        private void SWarrowButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.SWARROW;
            setDefaultBackColorForControls();
            SWarrowButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:34AM
         */
        private void rectangleSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.RECTANGLE;
            setDefaultBackColorForControls();
            rectangleSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:35AM
         */
        private void ovalSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.OVAL;
            setDefaultBackColorForControls();
            ovalSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/18/15 11:23am
         */
        private void solidSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.SOLID;
            setDefaultBackColorForControls();
            solidSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:36AM
         */
        private void dashedSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.DASHED;
            setDefaultBackColorForControls();
            dashedSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:37AM
         */
        private void dottedSelectButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.DOTTED;
            setDefaultBackColorForControls();
            dottedSelectButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * 3/19/15 7:38AM
         */
        private void vertTextButton_Click(object sender, EventArgs e)
        {
            m_currentSelectedControl = e_SelectedControl.VERTTEXT;
            setDefaultBackColorForControls();
            vertTextButton.BackColor = m_selectedControlButtonColor;
        }

        /*
         * Displays a color dialog to select drawing color
         * 3/17/15 10:54am 
         */
        private void drawColorButton_Click(object sender, EventArgs e)
        {
            mslog("drawColorButon_Click");
            DialogResult dr = drawColorDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // Apply new color
                m_currentDrawColor = drawColorDialog.Color;
                drawColorButton.BackColor = m_currentDrawColor;
                mslog("selected color = " + drawColorDialog.Color);
            }
        }

        /*
         * Set current drawing color to black and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:13am
         */
        private void blackColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blackColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to white and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:14am
         */
        private void whiteColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = whiteColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to gray and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:15am
         */
        private void grayColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = grayColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to greent and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:16am
         */
        private void greenColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = greenColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to red and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:17am
         */
        private void redColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = redColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to teal and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:18am
         */
        private void tealColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = tealColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to orange and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:19am
         */
        private void orangeColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = orangeColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to blue and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:20am
         */
        private void blueColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = blueColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to yellow and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:21am
         */
        private void yellowColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = yellowColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
        }

        /*
         * Set current drawing color to purple and update it in the drawColorButton.BackColor
         * to let the user know which color they selected.
         * 3/17/15 11:22am
         */
        private void purpleColorButton_Click(object sender, EventArgs e)
        {
            m_currentDrawColor = purpleColorButton.BackColor;
            drawColorButton.BackColor = m_currentDrawColor;
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
            SEarrowButton.BackColor = Color.Transparent;
            SarrowButton.BackColor = Color.Transparent;
            SWarrowButton.BackColor = Color.Transparent;
            WarrowButton.BackColor = Color.Transparent;
            rectangleSelectButton.BackColor = Color.Transparent;
            ovalSelectButton.BackColor = Color.Transparent;
            solidSelectButton.BackColor = Color.Transparent;
            dashedSelectButton.BackColor = Color.Transparent;
            dottedSelectButton.BackColor = Color.Transparent;
            vertTextButton.BackColor = Color.Transparent;
        }
    }
}
