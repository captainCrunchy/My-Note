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
        private Point m_textOrigin = new Point(0,0);
        public Button moveButton = new Button();
        private Button optionButton = new Button();
        public string logString = "";
        private bool canMove = false;
        private Point startPoint = new Point(0, 0);
        private Point endPointButton = new Point(0, 0);
        private Point endPointText = new Point(0, 0);

        bool isDragged = false;
        Point ptOffset;
        /*
         * 3/31/15 7:15am
         */
        public VerticalText(MouseEventArgs e)
        {
            moveButton.Text = "m";
            moveButton.BackColor = Color.Yellow;
            moveButton.Location = new Point(e.Location.X-5, e.Location.Y-5);
            moveButton.Size = new Size(15, 15);
            moveButton.MouseDown += moveButton_MouseDown;
            moveButton.MouseMove += moveButton_MouseMove;
            moveButton.MouseUp += moveButton_MouseUp;

            m_textOrigin = e.Location;
        }

        /*
         * 3/31/15 8:10am
         */
        private void moveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = moveButton.PointToScreen(new Point(e.X, e.Y));
                ptOffset = new Point();
                ptOffset.X = moveButton.Location.X - ptStartPosition.X;
                ptOffset.Y = moveButton.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }
        
        /*
         * 3/31/15 8:13am
         */
        private void moveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = moveButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                moveButton.Location = newPoint;
                endPointText.X = newPoint.X + 5;
                endPointText.Y = newPoint.Y + 5;
                m_textOrigin = endPointText;
            }
        }

        /*
         * 3/31/15 8:17am
         */
        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        /*
         * 3/31/15 7:35am
         */
        public void drawVerticalText(PaintEventArgs e)
        {
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();
        }
    }
}
