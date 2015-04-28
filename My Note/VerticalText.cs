using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

/*
 *  TITLE:
 *      VerticalText
 * 
 *  DESCRIPTION:
 *      The purpose of this class is to provide the capacity to draw a string that can be rotated to any angle, using
 *      several fonts and font sizes. Each instance of this VerticalText class comes with three buttons located just
 *      above it and spaced: one on each end and one in the middle. The left button is to allow the user to move the
 *      text around the screen. The middle button is to allow the user to enter an options pop-up window where they
 *      set the text and its properties. The button on the right side is to allow the user to rotate the text and set
 *      it to a desired angle.
 *      
 *  STRUCTURE:
 *      Variables were created and initialized immediately in the declaration section for reusability, to avoid
 *      repetion of creation in order to increase drawing performance. Some variables are initialized in the custom
 *      constructor. Other components have been separated into regions each with appropriate comments.
 */

namespace My_Note
{
    class VerticalText
    {
        public string logString = "logString empty"; // TEMP

        private String m_textString = "Enter Text";                     // Actual text of vertical text
        private Int32 m_textAngle = 0;                                  // The angle of vertical text
        private Font m_textFont = new Font("Microsoft Sans Serif", 12); // The font of vertical text
        private SolidBrush m_textBrush = new SolidBrush(Color.Blue);    // The brush use on vertical text
        private Point m_textOrigin;                                     // Origin of vertical text
        private Point m_textDestPt = new Point(0, 0);                   // Updated origin of vertical text after move
        
        private float m_optButtDistF = 48;                              // Distance between move and options buttons
        private Int32 m_optButOffsetX = 48;                             // x-offset from move to options button (used dynamically)
        private Int32 m_optButOffsetY = 0;                              // y-offset from move to options button (used dynamically)

        private float m_rotButDistF = 96;                               // Distance between move and rotate buttons
        private Int32 m_rotButOffsetX = 96;                             // x-offset from move to rotate button (used dynamically)
        private Int32 m_rotButOffsetY = 0;                              // y-offset from move to rotate button (used dynamically)

        private Point m_alteringButtonOffsetPoint = new Point();        // Offset point saved by subtracting 'm_moveButton.Location' (assigned by
                                                                        // constructor) from 'current point' (captured from entire screen)

        private Button m_moveButton = new Button();                     // Move the text around the panel
        private bool m_isMoving = false;                                // Indicates whether the text is being moved

        private Button m_optionsButton = new Button();                  // Brings up options window to modify vertical text
        private VertTextOptionsBox m_optionsBox = new VertTextOptionsBox();  // Options windows that allows modification of vertical text attibutes

        private Button m_rotateButton = new Button();                   // Rotates the vertical text to use desired angles
        private bool m_isRotating = false;                              // Indicates whether the text is being rotated

        // TODO: implement vertText options box
        // TODO: handle the spacing of buttons based on text size
        // TODO: update initial button locations (eventually) to even out the sides
        // TODO: new button images (round)

        /*
         * NAME
         *  VerticalText() - custom constructor for VerticalText object
         *  
         * SYNOPSIS
         *  public VerticalText(MouseEventArgs e);
         *      e       -> is used to get the location of mouse click
         * 
         * DESCRIPTION
         *  This constructor is called, specifically, from trapsarentPanel_MouseUp event handler.
         *  IMPORTANT: It utilizes (MouseEventArgs e) to get the location from the object using
         *  this class so that locations can be set INITIALLY and used CONTINUALLY.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:15am 3/31/2015
         */
        public VerticalText(MouseEventArgs e)
        {
            m_textOrigin = e.Location;

            m_moveButton.Text = "m";
            m_moveButton.BackColor = Color.Yellow;
            m_moveButton.Location = new Point(e.Location.X-8, e.Location.Y-8); // important
            m_moveButton.Size = new Size(16, 16);
            m_moveButton.MouseDown += moveButton_MouseDown;
            m_moveButton.MouseMove += moveButton_MouseMove;
            m_moveButton.MouseUp += moveButton_MouseUp;

            m_optionsButton.Text = "o";
            m_optionsButton.BackColor = Color.Blue;
            // This is default spacing and will be updated dynamically based on text width
            m_optionsButton.Location = new Point(m_moveButton.Location.X + 48, m_moveButton.Location.Y);

            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseUp += optionsButton_MouseUp;

            m_rotateButton.Text = "r";
            m_rotateButton.BackColor = Color.Green;
            // This is default spacing and will be updated dynamically based on text width
            m_rotateButton.Location = new Point(m_moveButton.Location.X + 96, m_moveButton.Location.Y);
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown += rotateButton_MouseDown;
            m_rotateButton.MouseMove += rotateButton_MouseMove;
            m_rotateButton.MouseUp += rotateButton_MouseUp;
        } /* public VerticalText(MouseEventArgs e) */

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
         * NAME
         *  moveButton_MouseDown() - prepares the VerticalText object to be moved
         *  
         * SYNOPSIS
         *  private void moveButton_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to get location of mouse cursor upon click and
         *                 confirm that left mouse button was clicked
         *                  
         * 
         * DESCRIPTION
         *  Prepares this VerticalText object to be moved by capturing the mouse click location
         *  within the main screen, then it translates/calculates this initial location to the
         *  location within the panel to be used in by getting the difference between location
         *  in the big screen and the location of m_moveButton in the panel. This value is then
         *  stored in m_alteringButtonOffsetPoint and used in moveButton_MouseMove event. Hides
         *  other buttons for nice appearances.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:10am 3/31/2015
         */
        private void moveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isMoving = true;
                Point ptStartPosition = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                m_alteringButtonOffsetPoint.X = m_moveButton.Location.X - ptStartPosition.X;
                m_alteringButtonOffsetPoint.Y = m_moveButton.Location.Y - ptStartPosition.Y;

                m_optionsButton.Visible = false;
                m_rotateButton.Visible = false;
            }
            else
            {
                m_isMoving = false;
            }
        } /* private void moveButton_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  moveButton_MouseMove() - moves the VerticalText object
         *  
         * SYNOPSIS
         *  private void moveButton_MouseMove(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to get location of mouse cursor upon click and
         *                 confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Moves the VerticalText object by updating its new location based on the MouseEventArg and the
         *  offset calculated and recorded in the MouseDown event earlier. First it updates the position of
         *  the m_moveButton itself, then it calculates and updates the position of the m_textOrigin point.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:13am 3/31/2015
         */
        private void moveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isMoving)
                {
                    Point newPoint = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                    newPoint.Offset(m_alteringButtonOffsetPoint);
                    m_moveButton.Location = newPoint;

                    m_textDestPt.X = newPoint.X + 8;
                    m_textDestPt.Y = newPoint.Y + 8;
                    m_textOrigin = m_textDestPt;
                }
            }
        } /* private void moveButton_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  moveButton_MouseUp() - updates/refreshes values
         *  
         * SYNOPSIS
         *  private void moveButton_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Updates the locations of other buttons and sets them to visible. Refreshes
         *  values that were changed during MouseDown and MouseMove events.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:17am 3/31/2015
         */
        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isMoving)
                {
                    m_isMoving = false;

                    m_optionsButton.Refresh();
                    m_optionsButton.Location = new Point(m_moveButton.Location.X - m_optButOffsetX,
                                                         m_moveButton.Location.Y - m_optButOffsetY);
                    m_optionsButton.Visible = true;

                    m_rotateButton.Refresh();
                    m_rotateButton.Location = new Point(m_moveButton.Location.X - m_rotButOffsetX,
                                                        m_moveButton.Location.Y - m_rotButOffsetY);
                    m_rotateButton.Visible = true;
                }
            }
        } /* private void moveButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // This region contains m_optionsButton Property and Event Handlers
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
         * NAME
         *  optionsButton_MouseUp() - shows option pop-up menu for changing text options
         *  
         * SYNOPSIS
         *  private void optionsButton_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Shows option pop-up menu for changing text options such as text size, font, and the
         *  text itself. Also provides a delete button to delete the entire vertical text instance.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:58am 4/15/2015
         */
        private void optionsButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_optionsBox.ShowDialog();
            }
        } /* private void optionsButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // This region contains m_rotateButton Property and Event Handlers
        #region m_rotateButton methods

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
         * NAME
         *  rotateButton_MouseDown() - prepares the vertical text object to be rotated
         *  
         * SYNOPSIS
         *  private void rotateButton_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Prepares this VerticalText object to be rotated by capturing the mouse click location
         *  within the main screen, then it translates/calculates this initial location to the
         *  location within the panel to be used in by getting the difference between location
         *  in the big screen and the location of m_moveButton in the panel. This value is then
         *  stored in m_alteringButtonOffsetPoint and used in rotateButton_MouseMove event. Hides
         *  other buttons for nice appearances.

         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:36pm 4/13/2015
         */
        private void rotateButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isRotating = true;
                Point ptStartPosition = m_rotateButton.PointToScreen(new Point(e.X, e.Y));
                m_alteringButtonOffsetPoint.X = m_rotateButton.Location.X - ptStartPosition.X;
                m_alteringButtonOffsetPoint.Y = m_rotateButton.Location.Y - ptStartPosition.Y;

                m_moveButton.Visible = false;
                m_optionsButton.Visible = false;
            }
        } /* private void rotateButton_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  rotateButton_MouseMove() - rotates the vertical text box
         *  
         * SYNOPSIS
         *  private void rotateButton_MouseMove(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to get location of mouse cursor upon click and
         *                 confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Rotates the VerticalText object by using the m_moveButton as the achnor point. Calculates
         *  the difference between the current mouse cursor position (m_rotateButton) and the pivot
         *  position (m_moveButton), then uses the results in a trigonometric function to calculate and
         *  assign a new angle to the text string. Updates the location of m_rotateButton based on the
         *  (MouseEventArg e) and the offset calculated and recorded in the MouseDown event earlier.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:37pm 4/13/2015
         */
        private void rotateButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isRotating)
                {
                    float dx = m_rotateButton.Location.X - m_moveButton.Location.X;
                    float dy = m_rotateButton.Location.Y - m_moveButton.Location.Y;
                    float new_angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
                    m_textAngle = (Int32)new_angle;
                    Point newPoint = m_rotateButton.PointToScreen(new Point(e.X, e.Y));
                    newPoint.Offset(m_alteringButtonOffsetPoint);
                    m_rotateButton.Location = newPoint;
                }
            }
        } /* private void rotateButton_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  rotateButton_MouseUp() - rotates the vertical text box
         *  
         * SYNOPSIS
         *  private void rotateButton_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Calculates new locations and repositions the other two buttons based on (a) the newly generated
         *  text angle and (b) the assigned distance of each button from the text. The pivot point is still
         *  the m_moveButton and the angle used is one generated during mouseMove event. Third and fourth
         *  variables to be used for calculations of button locations are the distances from m_moveButton
         *  to m_optionsButton and from m_moveButton to m_rotateButton locations, respectively. Trigonometric
         *  equations simply calculate the destination point (as mentioned) given the pivot point, angle, and
         *  distance to destination. After calculations are peformed and locations reassigned, the values of
         *  some member variables are updated so they can be utilized in the moveButton_MouseMove event.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:38pm 4/13/2015
         */
        private void rotateButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isRotating)
                {
                    float X = m_moveButton.Location.X;
                    float Y = m_moveButton.Location.Y;
                    float radAngleF = (float)(Math.PI * (double)m_textAngle / 180.0);

                    float optButDistF = m_optButtDistF;
                    float newOptLocX = (float)(X + Math.Cos(radAngleF) * optButDistF);
                    float newOptLocY = (float)(Y + Math.Sin(radAngleF) * optButDistF);
                    PointF newOptButPtF = new PointF(newOptLocX, newOptLocY);
                    m_optionsButton.Location = Point.Round(newOptButPtF);
                    m_optButOffsetX = m_moveButton.Location.X - m_optionsButton.Location.X;
                    m_optButOffsetY = m_moveButton.Location.Y - m_optionsButton.Location.Y;

                    float rotButDistF = m_rotButDistF;
                    float newRotLocX = (float)(X + Math.Cos(radAngleF) * rotButDistF);
                    float newRotLocY = (float)(Y + Math.Sin(radAngleF) * rotButDistF);
                    PointF newRotButPtF = new PointF(newRotLocX, newRotLocY);
                    m_rotateButton.Location = Point.Round(newRotButPtF);
                    m_rotButOffsetX = m_moveButton.Location.X - m_rotateButton.Location.X;
                    m_rotButOffsetY = m_moveButton.Location.Y - m_rotateButton.Location.Y;
                    
                    // Reset values
                    m_isRotating = false;
                    m_moveButton.Visible = true;
                    m_optionsButton.Visible = true;
                }
            }
        } /* private void rotateButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // This region contains 'helper' methods
        #region Helper Methods

        /*
         * NAME
         *  drawVerticalText() - draws the vertical text
         *  
         * SYNOPSIS
         *  public void drawVerticalText(PaintEventArgs e);
         *      e       -> used to indicate location and size of text
         * 
         * DESCRIPTION
         *  This method draws text at a desired angle. The 'engine' of this method was taken from a
         *  book, documented and credited appropriately; i.e. the transform methods. Arguments for
         *  these methods are member variables that were generated using other methods of this class.
         *  The string width is calculated to accomodate the position and spacing of buttons.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi using code from text:
         *  C# Graphics Programming, Rod Stephens, 2008
         *  
         * DATE
         *  7:35am 3/31/2015
         */
        public void drawVerticalText(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(m_textOrigin.X, m_textOrigin.Y);
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.TranslateTransform(-m_moveButton.Location.X, -m_moveButton.Location.Y);
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();

            int maxWidth = 200;
            SizeF currentStringSize = new SizeF();
            currentStringSize = e.Graphics.MeasureString(m_textString, m_textFont, maxWidth);
            logString = "currentStringSize = " + currentStringSize;
        } /* public void drawVerticalText(PaintEventArgs e) */

        #endregion
    }
}
