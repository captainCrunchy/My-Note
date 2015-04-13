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
        //public Button moveButton = new Button();
        
        private Button optionButton = new Button();
        public string logString = "";
        //private bool canMove = false;
        private Point srcPtMoveButton = new Point(0, 0);
        private Point destPtMoveButton = new Point(0, 0);
        private Point destPtText = new Point(0, 0);

        bool isDragged = false;

        public Button moveButton = new Button();
        // currentMovePoint helps translate location captured from screen to location within panel
        private Point currentMovePoint = new Point();

        public Button rotateButton = new Button();
        //private Point rotateButtonOrigin = new Point();

        // NEED TO: make the button offsets global variables

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

            rotateButton.Text = "r";
            rotateButton.BackColor = Color.Green;
            rotateButton.Location = new Point(moveButton.Location.X + 100, moveButton.Location.Y);
            rotateButton.Size = new Size(15, 15);
            rotateButton.MouseDown += rotateButton_MouseDown;
            rotateButton.MouseMove += rotateButton_MouseMove;
            rotateButton.MouseUp += rotateButton_MouseUp;

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
                /* translate the point offset from main screen to location on panel
                 * 
                 */
                Point ptStartPosition = moveButton.PointToScreen(new Point(e.X, e.Y));
                //currentMovePoint = new Point();
                currentMovePoint.X = moveButton.Location.X - ptStartPosition.X;
                currentMovePoint.Y = moveButton.Location.Y - ptStartPosition.Y;
                //moveButton.Visible = false;
                rotateButton.Visible = false;
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
                newPoint.Offset(currentMovePoint);
                moveButton.Location = newPoint;

                destPtText.X = newPoint.X + 5;
                destPtText.Y = newPoint.Y + 5;
                m_textOrigin = destPtText;
            }
        }

        /*
         * 3/31/15 8:17am
         */
        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                isDragged = false;
                rotateButton.Refresh();
                rotateButton.Location = new Point(moveButton.Location.X + 100, moveButton.Location.Y);
                rotateButton.Visible = true;
            }
        }

        /*
         * 4/13/15 6:36pm
         */
        private void rotateButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                // translate the point offset from main screen to location on panel
                Point ptStartPosition = rotateButton.PointToScreen(new Point(e.X, e.Y));
                currentMovePoint.X = rotateButton.Location.X - ptStartPosition.X;
                currentMovePoint.Y = rotateButton.Location.Y - ptStartPosition.Y;
                moveButton.Visible = false;
            }
            else
            {
                isDragged = false;
            }
        }

        /*
         * 4/13/15 6:37pm
         */
        private void rotateButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = rotateButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(currentMovePoint);
                rotateButton.Location = newPoint;

                //destPtText.X = newPoint.X + 5;
                //destPtText.Y = newPoint.Y + 5;
                //m_textOrigin = destPtText;
            }
        }

        /*
         * 4/13/15 6:38pm
         */
        private void rotateButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                isDragged = false;
                moveButton.Refresh();
                moveButton.Location = new Point(rotateButton.Location.X - 100, rotateButton.Location.Y);
                moveButton.Visible = true;
            }
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
