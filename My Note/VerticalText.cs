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
        private Point m_textOrigin;// = new Point(0,0);
        //public Button moveButton = new Button();
        
        private Button optionButton = new Button();
        public string logString = "logString empty";
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
        private bool isRotating = false;
        private float StartAngle = 0;
        private float CurrentAngle = 0;
        private float TotalAngle = 0;

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

            //m_textOrigin = moveButton.Location;

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
                isRotating = true;
                //logString = "moveButton location = " + moveButton.Location;
                float dx = moveButton.Location.X;
                float dy = moveButton.Location.Y;
                StartAngle = (float)Math.Atan2(dy, dx);
                //logString += "\r\nStartAngle = " + StartAngle;
                //logString += "\r\nrotateButton.location = " + rotateButton.Location;

                // translate the point offset from main screen to location on panel
                Point ptStartPosition = rotateButton.PointToScreen(new Point(e.X, e.Y));
                currentMovePoint.X = rotateButton.Location.X - ptStartPosition.X;
                currentMovePoint.Y = rotateButton.Location.Y - ptStartPosition.Y;
            }
            /*
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
            }*/
        }

        /*
         * 4/13/15 6:37pm
         */
        private void rotateButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRotating)
            {

                //logString = "\r\ne.Location = " + e.Location + "\r\n";
                float dx = rotateButton.Location.X - moveButton.Location.X;
                float dy = rotateButton.Location.Y - moveButton.Location.Y;

                //float new_angle = (float)Math.Atan2(dy, dx);
                float new_angle = (float) (Math.Atan2(dy, dx) * 180 / Math.PI);
                //logString += "new_angle = " + new_angle + "\r\n";
                m_textAngle = (Int32)new_angle;
                /*
                CurrentAngle = new_angle - StartAngle;
                logString += "CurrentAngle = " + CurrentAngle + " radians\r\n";

                CurrentAngle = CurrentAngle * 180 / (float)Math.PI;
                logString += "CurrentAngle = " + CurrentAngle + " degrees\r\n";
                */
                
                Point newPoint = rotateButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(currentMovePoint);
                rotateButton.Location = newPoint;
                //logString += "rorateButton.Location = " + rotateButton.Location;


                //logString = CurrentAngle.ToString("0.00") + " deg";
            }
            /*
            if (isDragged)
            {
                Point newPoint = rotateButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(currentMovePoint);
                rotateButton.Location = newPoint;

                //destPtText.X = newPoint.X + 5;
                //destPtText.Y = newPoint.Y + 5;
                //m_textOrigin = destPtText;
            }*/
        }

        /*
         * 4/13/15 6:38pm
         */
        private void rotateButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (isRotating)
            {
                isRotating = false;
                //logString = "rotateButton_MouseUP";
                //logString = "logString empty";
            }
            /*
            if (isDragged)
            {
                isDragged = false;
                moveButton.Refresh();
                moveButton.Location = new Point(rotateButton.Location.X - 100, rotateButton.Location.Y);
                moveButton.Visible = true;
            }*/
        }

        /*
         * 3/31/15 7:35am
         */
        public void drawVerticalText(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(moveButton.Location.X, moveButton.Location.Y);
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.TranslateTransform(-moveButton.Location.X, -moveButton.Location.Y);
            
            logString = "m_textOrigin = " + m_textOrigin;
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();
        }
    }
}
