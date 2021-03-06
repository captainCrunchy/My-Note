﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
 *  TITLE:
 *      TransparentPanel : Panel
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to provide the application with a transparent panel tool in the
 *      toolbox. An instance of this class is used to 'overlay' any other form, control, panel, etc...
 *      to allow the user to be able to apply graphics to those objects that normally do not allow
 *      any graphics or drawing. It is currently used as a layer on top of a RichTexbox. It draws
 *      lines to resemble those of a notebook and also allows user to 'freehand' draw custom shapes
 *      anywhere on the panel. Initial code for this class was taken from a website and slightly modified
 *      to accommodate the needs of this application. These modifications include addding new member
 *      variables updating the original methods, and updating elements to match the naming conventions
 *      of this application. A link to the author and site is provided below:
 *      Patrick Bailey, February 5, 2014
 *      http://www.whiteboardcoder.com/2014/02/visual-studio-2012-c-transparent.html
 *      
 *  CODE STRUCTURE:
 *      - Member variables
 *      - Properties
 *      - Regular methods
 */

namespace My_Note
{
    class TransparentPanel : Panel
    {
        private float m_thisWidth;                      // Width of this panel
        private float m_thisHeight;                     // Height of this panel
        private float m_lineBegin;                      // X-coordinate of the line beginning
        private float m_lineEnd;                        // X-coordinate of the line end
        private float m_nextLine;                       // Y-coordinate of the line (dynamic)
        private Pen m_bluePen = new Pen(Color.Blue);    // Used to draw horizontal lines
        private Pen m_redPen = new Pen(Color.Red);      // Used to draw vertical lines

        // The modification to make the panel 'transparent'
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // WS_EX_TRANSPARENT
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }
        
        /*
         * NAME
         *  OnPaintBackground() - assigns values to member variables
         *  
         * SYNOPSIS
         *  protected override void OnPaintBackground(PaintEventArgs e);
         *      e   -> does nothing
         * 
         * DESCRIPTION
         *  This method is used by owner objects of instances of this class. It assigns values to
         *  member variables and properties of this class.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:11pm 3/1/2015
         */
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            m_thisWidth = this.Size.Width;
            m_thisHeight = this.Size.Height;
            m_lineBegin = 5;
            m_lineEnd = m_thisWidth - 10;
            m_nextLine = 55;
        } /* protected override void OnPaintBackground(PaintEventArgs e) */
        
        /*
         * NAME
         *  OnPaint() - used to draw lines on panel
         *  
         * SYNOPSIS
         *  protected override void OnPaint(PaintEventArgs e);
         *      e   -> used in base class
         * 
         * DESCRIPTION
         *  Draws a set of lines on panel to resemble a page in a notebook.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:15pm 3/1/2015
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            // This draws the shapes by 'owner' object
            base.OnPaint(e);
            // The rest draws lines
            for (int i = 0; i < 28; i++)
            {
                e.Graphics.DrawLine(m_bluePen, m_lineBegin, m_nextLine, m_lineEnd, m_nextLine);
                m_nextLine += 20;
            }
            e.Graphics.DrawLine(m_redPen, 35, 5, 35, m_thisHeight - 5);
        } /* protected override void OnPaint(PaintEventArgs e) */
    }
}
