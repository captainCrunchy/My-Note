using System;
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
 *      to allow the user to be able to apply graphics to those objects that noremally do not allow
 *      any graphics or drawing. It is currently used as a layer on top of a rich text box. It draws
 *      lines to resemble those of a notebook and also allows user to 'freehand' draw custom shapes
 *      anywhere on the panel. Initial code for this class was taken from a website and slightly modified
 *      to accomodate the needs of this application. These modifications are updates to member variables
 *      to match the naming conventions of this application and an additional 'struct'.
 *      A link to the author and site is provided below:
 *      Patrick Bailey, February 5, 2014
 *      http://www.whiteboardcoder.com/2014/02/visual-studio-2012-c-transparent.html
 *      
 *  STRUCTURE:
 *      This class contains basic (self-explanatory) member variables and methods
 */

namespace My_Note
{
    class TransparentPanel : Panel
    {
        // Adjusts line spacing based on user selected font
        private struct lineSpaceForFont
        {
            public const float MICROSOFT_SANS_SERIF = 20;
        }

        private float m_thisWidth;
        private float m_thisHeight;
        private float m_lineBegin;
        private float m_lineEnd;
        private float m_nextLine;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            m_thisWidth = this.Size.Width;
            m_thisHeight = this.Size.Height;
            m_lineBegin = 5;
            m_lineEnd = m_thisWidth - 10;
            m_nextLine = 55;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // This draws the shapes by 'owner' object
            // The rest draws lines
            Graphics g = e.Graphics;
            Pen bluePen = new Pen(Color.Blue);
            for (int i = 0; i < 30; i++)
            {
                e.Graphics.DrawLine(bluePen, m_lineBegin, m_nextLine, m_lineEnd, m_nextLine);
                m_nextLine += lineSpaceForFont.MICROSOFT_SANS_SERIF;
            }

            Pen redPen = new Pen(Color.Red);
            e.Graphics.DrawLine(redPen, 35, 5, 35, m_thisHeight - 5);
        }
    }
}