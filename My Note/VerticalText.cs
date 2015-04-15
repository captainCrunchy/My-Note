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
        /*
         *  Many variables were created and initialized here for reusability and to avoid repetition
         *  in order to increase performance. Some variables are initialized in the constructor.
         */
        public string logString = "logString empty"; // TEMP

        private String m_textString = "Enter Text";                     // Actual text of vertical text
        private Int32 m_textAngle = 0;                                  // The angle of vertical text
        private Font m_textFont = new Font("Times New Roman", 16);      // The font of vertical text
        private SolidBrush m_textBrush = new SolidBrush(Color.Blue);    // The brush use on vertical text
        private Point m_textOrigin;                                     // Origin of vertical text
        private Point m_textDestPt = new Point(0, 0);                   // Updated origin of vertical text after move
        
        private Button m_moveButton = new Button();                     // Move the text around the panel
        private bool m_isMoving = false;                                // Indicates whether the text is being moved
        private Point m_currentMovePoint = new Point();                 // Translates mouse location captured from entire screen to panel 

        private Button m_optionsButton = new Button();                  // Brings up options window to modify vertical text
        private VertTextOptionsBox m_optionsBox = new VertTextOptionsBox();  // Options windows that allows modification of vertical text attibutes

        private Button m_rotateButton = new Button();                   // Rotates the vertical text to use desired angles
        private bool m_isRotating = false;                              // Indicates whether the text is being rotated

        // NEED TO: make the button offsets global variables, make the buttons round, update the button click to be on left side

        /*
         * 3/31/15 7:15am
         * (MouseEventArgs e) is passed from the owner object (transparentPanel_MouseUp) method
         */
        public VerticalText(MouseEventArgs e)
        {
            m_textOrigin = e.Location;

            m_moveButton.Text = "m";
            m_moveButton.BackColor = Color.Yellow;
            m_moveButton.Location = new Point(e.Location.X-5, e.Location.Y-5);
            m_moveButton.Size = new Size(16, 16);
            m_moveButton.MouseDown += moveButton_MouseDown;
            m_moveButton.MouseMove += moveButton_MouseMove;
            m_moveButton.MouseUp += moveButton_MouseUp;

            m_optionsButton.Text = "o";
            m_optionsButton.BackColor = Color.Blue;
            m_optionsButton.Location = new Point(m_moveButton.Location.X + 50, m_moveButton.Location.Y);
            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseDown += optionsButton_MouseDown;
            m_optionsButton.MouseMove += optionsButton_MouseMove;
            m_optionsButton.MouseUp += optionsButton_MouseUp;

            m_rotateButton.Text = "r";
            m_rotateButton.BackColor = Color.Green;
            m_rotateButton.Location = new Point(m_moveButton.Location.X + 100, m_moveButton.Location.Y);
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown += rotateButton_MouseDown;
            m_rotateButton.MouseMove += rotateButton_MouseMove;
            m_rotateButton.MouseUp += rotateButton_MouseUp;

        }

        // This region contains m_moveButton Property and Event Handlers
        #region m_moveButton methods

        public Button MoveButton
        {
            get
            {
                return m_moveButton;
            }
            set
            {
                m_moveButton = value;
            }
        }

        /*
         * 3/31/15 8:10am
         */
        private void moveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isMoving = true;
                // translate the point offset from main screen to location on panel
                Point ptStartPosition = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                m_currentMovePoint.X = m_moveButton.Location.X - ptStartPosition.X;
                m_currentMovePoint.Y = m_moveButton.Location.Y - ptStartPosition.Y;
                m_rotateButton.Visible = false;
            }
            else
            {
                m_isMoving = false;
            }
        }
        
        /*
         * 3/31/15 8:13am
         */
        private void moveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMoving)
            {
                Point newPoint = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(m_currentMovePoint);
                m_moveButton.Location = newPoint;

                m_textDestPt.X = newPoint.X + 5;
                m_textDestPt.Y = newPoint.Y + 5;
                m_textOrigin = m_textDestPt;
            }
        }

        /*
         * 3/31/15 8:17am
         */
        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_isMoving)
            {
                m_isMoving = false;
                m_rotateButton.Refresh();
                m_rotateButton.Location = new Point(m_moveButton.Location.X + 100, m_moveButton.Location.Y);
                m_rotateButton.Visible = true;
            }
        }

        #endregion

        // This region contains m_optionsButton Event Handlers and Properties
        #region m_optionsButton methods

        public Button OptionsButton
        {
            get
            {
                return m_optionsButton;
            }
            set
            {
                m_optionsButton = value;
            }
        }

        /*
         * 7:56am 4/15/15
         */
        void optionsButton_MouseDown(object sender, MouseEventArgs e)
        {

        }

        /*
         * 7:57am 4/15/15
         */
        void optionsButton_MouseMove(object sender, MouseEventArgs e)
        {

        }

        /*
         * 7:58am 4/15/15
         */
        void optionsButton_MouseUp(object sender, MouseEventArgs e)
        {
            m_optionsBox.ShowDialog();
        }

        #endregion

        // This region contains m_rotateButton Property and Event Handlers
        #region m_rotateButton methos

        public Button RotateButton
        {
            get
            {
                return m_rotateButton;
            }
            set
            {
                m_rotateButton = value;
            }
        }

        /*
         * 4/13/15 6:36pm
         */
        private void rotateButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isRotating = true;

                // translate the point offset from main screen to location on panel
                Point ptStartPosition = m_rotateButton.PointToScreen(new Point(e.X, e.Y));
                m_currentMovePoint.X = m_rotateButton.Location.X - ptStartPosition.X;
                m_currentMovePoint.Y = m_rotateButton.Location.Y - ptStartPosition.Y;
            }
        }

        /*
         * 4/13/15 6:37pm
         */
        private void rotateButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isRotating)
            {

                float dx = m_rotateButton.Location.X - m_moveButton.Location.X;
                float dy = m_rotateButton.Location.Y - m_moveButton.Location.Y;

                float new_angle = (float) (Math.Atan2(dy, dx) * 180 / Math.PI);
                m_textAngle = (Int32)new_angle;
                
                Point newPoint = m_rotateButton.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(m_currentMovePoint);
                m_rotateButton.Location = newPoint;
            }
        }

        /*
         * 4/13/15 6:38pm
         */
        private void rotateButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_isRotating)
            {
                m_isRotating = false;
            }
        }

        #endregion

        // This region contains 'helper' methods
        #region Helper Methods

        /*
         * 3/31/15 7:35am
         */
        public void drawVerticalText(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(m_moveButton.Location.X, m_moveButton.Location.Y);
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.TranslateTransform(-m_moveButton.Location.X, -m_moveButton.Location.Y);
            
            logString = "m_textOrigin = " + m_textOrigin;
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();

            SizeF currentStringWidth = getStringWidth(e);
            logString += "\r\ncurrentStringWidth = " + currentStringWidth;
        }

        /*
         * 7:29am 4/15/2015
         */
        private SizeF getStringWidth(PaintEventArgs e)
        {
            int maxWidth = 200;
            SizeF retWidth = new SizeF();
            retWidth = e.Graphics.MeasureString(m_textString, m_textFont, maxWidth);

            return retWidth;
        }

        #endregion
    }
}
