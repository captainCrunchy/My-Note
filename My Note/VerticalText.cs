using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

/*
 * 
 * The three lines of code RotateTransform, DrawString, ResetTransform()
 * came from a book: C# Graphics Programming, Rod Stephens, 2008
 * 9:21am 3/29/15
 */

namespace My_Note
{
    class VerticalText
    {
        private String m_textString = "Rotated 45";
        private Int32 m_textAngle = 45;
        private Font m_textFont = new Font("Times New Roman", 16);
        private SolidBrush m_textBrush = new SolidBrush(Color.Blue);
        private Point m_textOrigin = new Point(100, 60);

        //private bool m_isControlSelected = false;

        public void drawVerticalText(PaintEventArgs e)
        {
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();
        }

        private void showButtons()
        {

        }
    }
}
