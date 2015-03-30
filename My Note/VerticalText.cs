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
        //private Int32 m_textAngle = 45;
        private Int32 m_textAngle = 0;
        private Font m_textFont = new Font("Times New Roman", 16);
        private SolidBrush m_textBrush = new SolidBrush(Color.Blue);
        // this origin is going to be set by the class that is using this object
        // possibly in a custom constructor?
        //private Point m_textOrigin = new Point(400, 100);
        private Point m_textOrigin = new Point(0,0);
        public Button moveButton = new Button();
        private Button optionButton = new Button();
        //public SizeF stringSize = new SizeF();

        //private bool m_isControlSelected = false;
        public VerticalText()
        {
            moveButton.Text = "Z";
            moveButton.BackColor = Color.Yellow;
            moveButton.Location = new Point(0,0);
            moveButton.Size = new  Size(20, 20);
        }
        public VerticalText(MouseEventArgs e)
        {
            moveButton.Text = "Z";
            moveButton.BackColor = Color.Yellow;
            moveButton.Location = new Point(0, 0);
            moveButton.Size = new Size(20, 20);
            m_textOrigin = e.Location;
        }
        public void addVerticalText(MouseEventArgs e)
        {
            m_textOrigin = e.Location;
        }
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
