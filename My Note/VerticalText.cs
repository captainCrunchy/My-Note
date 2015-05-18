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
 *      several fonts, font sizes, and colors. Each instance of this VerticalText class comes with three buttons located
 *      just above it and spaced: one on each end and one in the middle. The left button is to allow the user to move the
 *      text around the screen. The middle button is to allow the user to enter an options pop-up window where they set
 *      the text and its properties. The button on the right side is to allow the user to rotate the text and set it to
 *      a desired angle. Buttons and their locations are created dynamically and their event handlers are assigned in 
 *      the constructor programmatically.
 *      
 *  CODE STRUCTURE:
 *      Variables were created and initialized immediately in the declaration section for reusability, to avoid repetition of
 *      variable creation in order to increase drawing performance. Some variables are initialized in the custom constructor
 *      Methods have been separated into regions based on their functionality each with appropriate comments; i.e. region for
 *      move button, region for options button, region for helper methods, etc.
 */

namespace My_Note
{
    class VerticalText
    {
        public string logString = "logString empty"; // TEMP

        private String m_textString = "Enter Text";                     // Actual text, used in drawVerticalText(), updated from options
        private Font m_textFont = new Font("Microsoft Sans Serif", 12); // Font of text, used in drawVerticalText(), updated from options
        private SolidBrush m_textBrush = new SolidBrush(Color.Black);   // Brush of text, used in drawVerticalText(), updated from options
        private Point m_textOrigin;                                     // Origin of text, used in drawVerticalText(), updated with m_moveButton
        private Int32 m_textAngle = 0;                                  /* Angle of text, used in drawVerticalText() and updateButtonLocations(), 
                                                                           updated in m_rotateButton_MouseUp() */
        
        private Button m_moveButton = new Button();                     // Button that moves the text around the panel
        private bool m_isMoving = false;                                // Indicates whether the text is currently being moved
        private Point m_alteringButtonOffsetPoint = new Point();        /* Offset point saved by subtracting 'm_moveButton.Location' (assigned
                                                                           by constructor) minus 'current point' (captured from entire screen) */

        private Button m_optionsButton = new Button();                  // Brings up options window to modify text properties
        private VertTextOptionsForm m_optionsForm = new VertTextOptionsForm();  // Options window to modify text properties
        private float m_optButDistF = 32;                               // Distance between move and options buttons, used in
                                                                        // updateButtonLocations(), updated in drawVerticalText()

        private Button m_deleteButton = new Button();
        private float m_delButDistF = 64;

        private Button m_rotateButton = new Button();                   // Button that rotates text to user desired angles
        private bool m_isRotating = false;                              // Indicates whether the text is being rotated
        private float m_rotButDistF = 96;                               /* Distance between move and rotate buttons, used in
                                                                           udpateButtonLocations(), updated in drawVerticalText() */

        /* Next three member variables point to the objects that live in the form (in formTextbox.cs) which owns this (VerticalText)
           object as well. They are declared here only to assist the 'm_optionsButton_MouseUp' event handler with repainting this object
           when changes are made to it. This technique seems to be the only way to access objects that are outside of this object without
           breaking the rules of encapsulation. These member variables are accessed via their properties defined in m_optionsButton region. */
        private TransparentPanel m_ownerTranspPanel;
        private RichTextBox m_ownerRichTextBox;
        private Panel m_ownerBackPanel;

        public List<VerticalText> m_ownerVerticalTextList;
        // TODO: new button images (round)
        // TODO: add a delete button?
        // TODO: refresh all comments when done (drawVerticalText - != (), options mouse up)

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
         *  this class so that locations can be set INITIALLY and used CONTINUALLY afterwards.
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
            m_moveButton.MouseDown += m_moveButton_MouseDown;
            m_moveButton.MouseMove += m_moveButton_MouseMove;
            m_moveButton.MouseUp += m_moveButton_MouseUp;

            m_optionsButton.Text = "o";
            m_optionsButton.BackColor = Color.Blue;
            //m_optionsButton.Location = new Point(m_moveButton.Location.X + 48, m_moveButton.Location.Y);
            m_optionsButton.Location = new Point(m_moveButton.Location.X + (Int32)m_optButDistF, m_moveButton.Location.Y);
            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseUp += m_optionsButton_MouseUp;

            m_deleteButton.Text = "d";
            m_deleteButton.BackColor = Color.Red;
            m_deleteButton.Location = new Point(m_moveButton.Location.X + (Int32)m_delButDistF, m_moveButton.Location.Y);
            m_deleteButton.Size = new Size(16, 16);
            m_deleteButton.MouseUp += m_deleteButton_MouseUp;

            m_rotateButton.Text = "r";
            m_rotateButton.BackColor = Color.Green;
            //m_rotateButton.Location = new Point(m_moveButton.Location.X + 96, m_moveButton.Location.Y);
            m_rotateButton.Location = new Point(m_moveButton.Location.X + (Int32)m_delButDistF, m_moveButton.Location.Y);
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown += m_rotateButton_MouseDown;
            m_rotateButton.MouseMove += m_rotateButton_MouseMove;
            m_rotateButton.MouseUp += m_rotateButton_MouseUp;
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
         *  m_moveButton_MouseDown() - prepares the VerticalText object to be moved
         *  
         * SYNOPSIS
         *  private void m_moveButton_MouseDown(object sender, MouseEventArgs e);
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
        private void m_moveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isMoving = true;
                Point ptStartPosition = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                m_alteringButtonOffsetPoint.X = m_moveButton.Location.X - ptStartPosition.X;
                m_alteringButtonOffsetPoint.Y = m_moveButton.Location.Y - ptStartPosition.Y;

                m_optionsButton.Visible = false;
                m_deleteButton.Visible = false;
                m_rotateButton.Visible = false;
            }
            else
            {
                m_isMoving = false;
            }
        } /* private void m_moveButton_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  m_moveButton_MouseMove() - moves the VerticalText object
         *  
         * SYNOPSIS
         *  private void m_moveButton_MouseMove(object sender, MouseEventArgs e);
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
        private void m_moveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isMoving)
                {
                    // Get new location
                    Point newPoint = m_moveButton.PointToScreen(new Point(e.X, e.Y));
                    newPoint.Offset(m_alteringButtonOffsetPoint);

                    // Keep location within the panel
                    if (newPoint.X < 0) newPoint.X = 0;
                    if (newPoint.X > 500) newPoint.X = 500;
                    if (newPoint.Y < 0) newPoint.Y = 0;
                    if (newPoint.Y > 590) newPoint.Y = 590;

                    // Assign new location
                    m_moveButton.Location = newPoint;
                    m_textOrigin = new Point(newPoint.X + 8, newPoint.Y + 8);
                }
            }
        } /* private void m_moveButton_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  m_moveButton_MouseUp() - updates/refreshes values
         *  
         * SYNOPSIS
         *  private void m_moveButton_MouseUp(object sender, MouseEventArgs e);
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
        private void m_moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isMoving)
                {
                    updateButtonLocations();
                    
                    // Reset values
                    m_isMoving = false;
                    m_optionsButton.Visible = true;
                    m_deleteButton.Visible = true;
                    m_rotateButton.Visible = true;
                }
            }
        } /* private void m_moveButton_MouseUp(object sender, MouseEventArgs e) */

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

        public TransparentPanel OwnerTranspPanel
        {
            set
            {
                m_ownerTranspPanel = value;
            }
        }

        public RichTextBox OwnerRichTextBox
        {
            set
            {
                m_ownerRichTextBox = value;
            }
        }

        public Panel OwnerBackPanel
        {
            set
            {
                m_ownerBackPanel = value;
            }
        }

        /*
         * NAME
         *  m_optionsButton_MouseUp() - shows option pop-up menu for changing text options
         *  
         * SYNOPSIS
         *  private void m_optionsButton_MouseUp(object sender, MouseEventArgs e);
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
        private void m_optionsButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_optionsForm.CaptureUIAttributes(m_textFont, m_textString, m_textBrush);
                m_optionsForm.ShowDialog();
                m_optionsForm.UpdateUIAttributes(ref m_textFont, ref m_textString, ref m_textBrush);
                
                m_ownerTranspPanel.Invalidate();
                m_ownerRichTextBox.Invalidate();
                m_ownerBackPanel.Invalidate();
                m_ownerTranspPanel.Refresh();
            }
        } /* private void m_optionsButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        public Button DeleteButton
        {
            get
            {
                return m_deleteButton;
            }
            set
            {
                m_deleteButton = value;
            }
        }

        /*
         *  4:15pm 5/17/15
         */
        private void m_deleteButton_MouseUp(object sender, MouseEventArgs e)
        {
            VerticalText vOne = this;
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this text?",
                "Click yes or no", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                logString = "yes delete";
                foreach (VerticalText v in m_ownerVerticalTextList)
                {
                    if (v == this)
                    {
                        m_ownerTranspPanel.Controls.Remove(m_moveButton);
                        m_ownerTranspPanel.Controls.Remove(m_optionsButton);
                        m_ownerTranspPanel.Controls.Remove(m_deleteButton);
                        m_ownerTranspPanel.Controls.Remove(m_rotateButton);
                        m_ownerVerticalTextList.Remove(v);
                        return;
                    }
                }
            }
            else
            {
                logString = "no delete";
            }
            VerticalText vTwo = this;
        }


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
         *  m_rotateButton_MouseDown() - prepares the vertical text object to be rotated
         *  
         * SYNOPSIS
         *  private void m_rotateButton_MouseDown(object sender, MouseEventArgs e);
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
        private void m_rotateButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isRotating = true;
                Point ptStartPosition = m_rotateButton.PointToScreen(new Point(e.X, e.Y));
                m_alteringButtonOffsetPoint.X = m_rotateButton.Location.X - ptStartPosition.X;
                m_alteringButtonOffsetPoint.Y = m_rotateButton.Location.Y - ptStartPosition.Y;

                m_moveButton.Visible = false;
                m_optionsButton.Visible = false;
                m_deleteButton.Visible = false;
            }
        } /* private void m_rotateButton_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  m_rotateButton_MouseMove() - rotates the vertical text box
         *  
         * SYNOPSIS
         *  private void m_rotateButton_MouseMove(object sender, MouseEventArgs e);
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
        private void m_rotateButton_MouseMove(object sender, MouseEventArgs e)
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
        } /* private void m_rotateButton_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  m_rotateButton_MouseUp() - rotates the vertical text box
         *  
         * SYNOPSIS
         *  private void m_rotateButton_MouseUp(object sender, MouseEventArgs e);
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
        private void m_rotateButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_isRotating)
                {
                    updateButtonLocations();
                    
                    // Reset values
                    m_isRotating = false;
                    m_moveButton.Visible = true;
                    m_optionsButton.Visible = true;
                    m_deleteButton.Visible = true;
                }
            }
        } /* private void m_rotateButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // This region contains 'helper' and public methods
        #region Helper and Public Methods

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
            // Save original value
            float oldRotButDistF = m_rotButDistF;
            
            // Draw rotated text
            e.Graphics.TranslateTransform(m_textOrigin.X, m_textOrigin.Y);
            e.Graphics.RotateTransform(m_textAngle);
            e.Graphics.TranslateTransform(-m_moveButton.Location.X, -m_moveButton.Location.Y);
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();

            // Update values for button locations
            int maxWidth = 300;
            SizeF currentStringSize = new SizeF();
            currentStringSize = e.Graphics.MeasureString(m_textString, m_textFont, maxWidth);
            int intVal = Convert.ToInt32(currentStringSize.Width);
            
            // Limit the proximity of buttons and assign values
            m_rotButDistF = Math.Abs(intVal + 14);
            if (m_rotButDistF < 36)
            {
                m_rotButDistF = 36;
            }
            m_optButDistF = m_rotButDistF / 3;
            m_delButDistF = m_rotButDistF / 3 * 2;
            if (oldRotButDistF != m_rotButDistF)
            {
                updateButtonLocations();
            }
            //m_rotButDistF = Math.Abs(intVal + 14);
            //if (m_rotButDistF < 36)
            //{
            //    m_rotButDistF = 36;
            //}
            //m_optButDistF = m_rotButDistF / 2;
            //if (oldRotButDistF != m_rotButDistF)
            //{
            //    updateButtonLocations();
            //}
        } /* public void drawVerticalText(PaintEventArgs e) */

        /*
         * NAME
         *  updateButtonLocations() - updates the locations of the buttons
         *  
         * SYNOPSIS
         *  private void updateButtonLocations();
         * 
         * DESCRIPTION
         *  This method gets called by the _MouseUp events of m_rotateButton and m_moveButton in
         *  order to update the locations of all the buttons. The calculations are performed by using
         *  the location of m_moveButton as the pivot point and the angle of the text to project
         *  the direction of the other buttons. 
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:18pm 5/15/2015
         */
        private void updateButtonLocations()
        {
            float X = m_moveButton.Location.X;
            float Y = m_moveButton.Location.Y;
            float radAngleF = (float)(Math.PI * (double)m_textAngle / 180.0);

            float optButDistF = m_optButDistF;
            float newOptLocX = (float)(X + Math.Cos(radAngleF) * optButDistF);
            float newOptLocY = (float)(Y + Math.Sin(radAngleF) * optButDistF);
            PointF newOptButPtF = new PointF(newOptLocX, newOptLocY);
            m_optionsButton.Location = Point.Round(newOptButPtF);

            float delButDistF = m_delButDistF;
            float newDelLocX = (float)(X + Math.Cos(radAngleF) * delButDistF);
            float newDelLocY = (float)(Y + Math.Sin(radAngleF) * delButDistF);
            PointF newDelButPtF = new PointF(newDelLocX, newDelLocY);
            m_deleteButton.Location = Point.Round(newDelButPtF);

            float rotButDistF = m_rotButDistF;
            float newRotLocX = (float)(X + Math.Cos(radAngleF) * rotButDistF);
            float newRotLocY = (float)(Y + Math.Sin(radAngleF) * rotButDistF);
            PointF newRotButPtF = new PointF(newRotLocX, newRotLocY);
            m_rotateButton.Location = Point.Round(newRotButPtF);
        } /* private void updateButtonLocations() */

        /*
         * NAME
         *  isNew() - checks to see if this VerticalText object is new
         *  
         * SYNOPSIS
         *  public bool isNew();
         * 
         * DESCRIPTION
         *  This method gets called to check and see if this object is new; i.e. was
         *  created and not yet modified. Such functionality will assist the owner of
         *  this object in preventing the creation of too many new VerticalText instances
         *  by accidentally clicking on the panel too fast/too many times.
         * 
         * RETURNS
         *  True if this object was just created and false if it has been modified.
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:48pm 5/17/2015
         */
        public bool isNew()
        {
            if (m_textString == "Enter Text")
            {
                return true;
            }
            else
            {
                return false;
            }
        } /* public bool isNew() */

        /*
         * NAME
         *  hideButtons() - hides all the buttons associated with this object
         *  
         * SYNOPSIS
         *  public void hideButtons();
         * 
         * DESCRIPTION
         *  This method gets called to set the 'Visible' properties of all the buttons associated
         *  with this object to false. This is done to indicate to the user that VerticalText
         *  control is not selected and to deliver a more presentable UI.
         * 
         * RETURNS
         *  True if this object was just created and false if it has been modified.
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:05pm 5/17/2015
         */
        public void hideButtons()
        {
            m_moveButton.Visible = false;
            m_optionsButton.Visible = false;
            m_deleteButton.Visible = false;
            m_rotateButton.Visible = false;
        } /* public void hideButtons() */

        /*
         * NAME
         *  showButtons() - shows all the buttons associated with this object
         *  
         * SYNOPSIS
         *  public void showButtons();
         * 
         * DESCRIPTION
         *  This method gets called to set the 'Visible' properties of all the buttons associated with
         *  this object to true. This is done to indicate to the user that VerticalText control is
         *  currently selected to give the user tools to modify each instance of VerticalText.
         * 
         * RETURNS
         *  True if this object was just created and false if it has been modified.
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:06pm 5/17/2015
         */
        public void showButtons()
        {
            m_moveButton.Visible = true;
            m_optionsButton.Visible = true;
            m_deleteButton.Visible = true;
            m_rotateButton.Visible = true;
        } /* public void showButtons() */

        #endregion
    }
}