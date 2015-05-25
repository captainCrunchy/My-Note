using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      VerticalText
 * 
 *  DESCRIPTION:
 *      The purpose of this class is to provide the capacity to draw a string that can be rotated to any angle, using
 *      several fonts, font sizes, and colors. Each instance of this VerticalText class comes with four buttons located
 *      just above it and spaced equally. Starting from the left, first button is to allow the user to move the text
 *      around the screen. Second button is to allow the user to enter an options pop-up window where they can set the
 *      text and its properties. Third button lets the user delete this instance from the panel in which it is used.
 *      Fourth button is to allow the user to rotate the text to set it to a desired angle. Buttons and their locations
 *      are created dynamically and their event handler are assigned programmatically in the constructor.
 *      
 *  CODE STRUCTURE:
 *      Variables were created and initialized immediately in the declaration section for reusability, to avoid repetition of
 *      variable creation in order to increase drawing performance. Some variables are initialized in the custom constructor
 *      Methods have been separated into regions based on their functionality each with appropriate comments; i.e. region for
 *      move button, region for options button, region for helper methods, etc.
 */

namespace My_Note
{
    [Serializable()]
    class VerticalText : ISerializable
    {
        /*S*/private String m_textString = "Enter Text";                     // Actual text, used in drawVerticalText(), updated from options
        /*S*/private Font m_textFont = new Font("Microsoft Sans Serif", 12); // Font of text, used in drawVerticalText(), updated from options
        
        // created because solid brush cannot be serialized but we still want to create it only once to optimize performance
        /*S*/private Color m_textBrushColor = Color.Black;
        //private SolidBrush m_textBrush;// Assigned in constructor
        private SolidBrush m_textBrush = new SolidBrush(Color.Black);

        ///*S??*/private SolidBrush m_textBrush = new SolidBrush(Color.Black);   // Brush of text, used in drawVerticalText(), updated from options
        /*S*/public Point m_textOrigin;                                     // Origin of text, used in drawVerticalText(), updated with m_moveButton
        /*S*/private Int32 m_textAngle = 0;                                  /* Text angle in degrees, used in drawVerticalText() and updateButtonLocations(), 
                                                                           updated in m_rotateButton_MouseMove(). */
        
        private Button m_moveButton = new Button();                     // Button that moves the text around the panel
        // added
        private Point m_moveButtonLocation = new Point();
        private bool m_isMoving = false;                                // Indicates whether the text is currently being moved
        private Point m_alteringButtonOffsetPoint = new Point();        /* Offset point calculated by subtracting 'm_moveButton.Location' or the
                                                                           'm_rotateButton.Location' minus 'current point' (captured from entire screen) */

        private Button m_optionsButton = new Button();                  // Brings up options window to modify text properties
        // added
        private Point m_optionsButtonLocation = new Point();
        private VertTextOptionsForm m_optionsForm = new VertTextOptionsForm();  // Options window to modify text properties
        /*S*/private float m_optButDistF = 32;                               // Distance between move and options buttons, used in
                                                                        // updateButtonLocations(), updated in drawVerticalText()

        private Button m_deleteButton = new Button();
        // added
        private Point m_deleteButtonLocation = new Point();
        /*S*/private float m_delButDistF = 64;

        private Button m_rotateButton = new Button();                   // Button that rotates text to user desired angles
        // added
        private Point m_rotateButtonLocation = new Point();
        private bool m_isRotating = false;                              // Indicates whether the text is being rotated
        /*S*/private float m_rotButDistF = 96;                               /* Distance between move and rotate buttons, used in
                                                                           udpateButtonLocations(), updated in drawVerticalText() */

        // essentially this is a pointer to a variable living in MainForm class
        private List<VerticalText> m_ownerVerticalTextList;             /* Used to access container in which 'this' will be in, to assist the
                                                                           removal of 'this' from the container. (m_deleteButton region) */

        /* Next three member variables point to the objects that live in the form (in formTextbox.cs) which owns this (VerticalText)
           object as well. They are declared here only to assist the 'm_optionsButton_MouseUp' event handler with repainting this object
           when changes are made to it. This technique seems to be the only way to access objects that are outside of this object without
           breaking the rules of encapsulation. These member variables are accessed via their properties defined in m_optionsButton region. */
        // essentially these are pointers to variables and UI objects living in MainFormClass
        private TransparentPanel m_ownerTranspPanel;
        private RichTextBox m_ownerRichTextBox;
        private Panel m_ownerBackPanel;
        /*
         *  TODO: The above three member variables may not be able to be saved because they are references
         *        need to think of a different way to assign their values. That is, maybe thy should not be
         *        assigned when they are first created, instead let them be assigned as the options button is
         *        clicked. This can be captured with the help of transparentPanelEvent
         *        More comments were added to references from previous class.
         *        Data persistence was added on the bottom.
         *        Brush serialized?
         *        Modified drawVertical text by udpating its brush color
         *        Modified m_optionsButton_MouseUp
         *        added setButtonProperties()
         *        added m_textBrushColor
         *        added m_moveButtonLocation
         *        added m_optionsButtonLocation
         *        added m_deleteButtonLocation
         *        added m_rotateButtonLocation
         *        modified m_moveButtonMouseMove
         *        modified updateButtonLocations();
         *        altered the constructor with the move button location restriction and new text origin assignment
         */

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
            //m_textOrigin = e.Location;

            m_moveButton.BackColor = Color.Transparent;
            m_moveButton.BackgroundImage = Properties.Resources.moveArrow;

            // Center the m_moveButton.Location with the mouse cursor location,
            // and ensure that it is not placed outside the bounds of its container
            Point newMoveButtonLocation = new Point(e.Location.X - 8, e.Location.Y - 8);
            if (newMoveButtonLocation.X < 0) newMoveButtonLocation.X = 0;
            if (newMoveButtonLocation.X > 504) newMoveButtonLocation.X = 504;
            if (newMoveButtonLocation.Y < 0) newMoveButtonLocation.Y = 0;
            if (newMoveButtonLocation.Y > 589) newMoveButtonLocation.Y = 589;
            m_moveButton.Location = newMoveButtonLocation;
            //m_moveButton.Location = new Point(e.Location.X-8, e.Location.Y-8);

            m_textOrigin = new Point(m_moveButton.Location.X + 8, m_moveButton.Location.Y + 8);

            m_moveButton.Size = new Size(16, 16);
            m_moveButton.MouseDown += m_moveButton_MouseDown;
            m_moveButton.MouseMove += m_moveButton_MouseMove;
            m_moveButton.MouseUp += m_moveButton_MouseUp;

            m_optionsButton.BackColor = Color.Transparent;
            m_optionsButton.BackgroundImage = Properties.Resources.optionsImg;
            m_optionsButton.Location = new Point(m_moveButton.Location.X + (Int32)m_optButDistF, m_moveButton.Location.Y);
            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseUp += m_optionsButton_MouseUp;

            //m_deleteButton.BackColor = Color.White;
            m_deleteButton.BackColor = Color.Transparent;

            m_deleteButton.BackgroundImage = Properties.Resources.removeX;
            m_deleteButton.Location = new Point(m_moveButton.Location.X + (Int32)m_delButDistF, m_moveButton.Location.Y);
            m_deleteButton.Size = new Size(16, 16);
            m_deleteButton.MouseUp += m_deleteButton_MouseUp;

            m_rotateButton.BackColor = Color.Transparent;
            m_rotateButton.BackgroundImage = Properties.Resources.rotateArrow;
            m_rotateButton.Location = new Point(m_moveButton.Location.X + (Int32)m_delButDistF, m_moveButton.Location.Y);
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown += m_rotateButton_MouseDown;
            m_rotateButton.MouseMove += m_rotateButton_MouseMove;
            m_rotateButton.MouseUp += m_rotateButton_MouseUp;

            //m_textBrush = new SolidBrush(m_textBrushColor);
        } /* public VerticalText(MouseEventArgs e) */
        
        /*
         * 2:32pm 5/20/2015
         * maybe this is better here because it will also be used in data persistence
         */
        public void setButtonProperties()
        {
            m_moveButton.BackColor = Color.Transparent;
            m_moveButton.BackgroundImage = Properties.Resources.moveArrow;
            m_moveButton.Location = m_moveButtonLocation;
            m_moveButton.Size = new Size(16, 16);

            // Best solution, it is not professional looking but it is safe,
            // effective, and efficient. Helps with data persistence between when
            // the application is running, closed, or switching between subjects and pages.
            m_moveButton.MouseDown -= m_moveButton_MouseDown;
            m_moveButton.MouseMove -= m_moveButton_MouseMove;
            m_moveButton.MouseUp -= m_moveButton_MouseUp;
            m_moveButton.MouseDown += m_moveButton_MouseDown;
            m_moveButton.MouseMove += m_moveButton_MouseMove;
            m_moveButton.MouseUp += m_moveButton_MouseUp;

            m_optionsButton.BackColor = Color.Transparent;
            m_optionsButton.BackgroundImage = Properties.Resources.optionsImg;
            m_optionsButton.Location = m_optionsButtonLocation;
            m_optionsButton.Size = new Size(16, 16);

            m_optionsButton.MouseUp -= m_optionsButton_MouseUp;
            m_optionsButton.MouseUp += m_optionsButton_MouseUp;

            //m_deleteButton.BackColor = Color.White;
            m_deleteButton.BackColor = Color.Transparent;
            m_deleteButton.BackgroundImage = Properties.Resources.removeX;
            m_deleteButton.Location = m_deleteButtonLocation;
            m_deleteButton.Size = new Size(16, 16);

            m_deleteButton.MouseUp -= m_deleteButton_MouseUp;
            m_deleteButton.MouseUp += m_deleteButton_MouseUp;

            m_rotateButton.BackColor = Color.Transparent;
            m_rotateButton.BackgroundImage = Properties.Resources.rotateArrow;
            m_rotateButton.Location = m_rotateButtonLocation;
            m_rotateButton.Size = new Size(16, 16);

            m_rotateButton.MouseDown -= m_rotateButton_MouseDown;
            m_rotateButton.MouseMove -= m_rotateButton_MouseMove;
            m_rotateButton.MouseUp -= m_rotateButton_MouseUp;
            m_rotateButton.MouseDown += m_rotateButton_MouseDown;
            m_rotateButton.MouseMove += m_rotateButton_MouseMove;
            m_rotateButton.MouseUp += m_rotateButton_MouseUp;
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
         *  other buttons for nice appearances while text is being moved
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

                    // Restrict location to stay within boundaries of panel
                    if (newPoint.X < 0) newPoint.X = 0;
                    if (newPoint.X > 500) newPoint.X = 500;
                    if (newPoint.Y < 0) newPoint.Y = 0;
                    if (newPoint.Y > 590) newPoint.Y = 590;

                    // Assign new location
                    m_moveButton.Location = newPoint;
                    // added this for data persistence
                    m_moveButtonLocation = newPoint;
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
         *  Updates the locations of all the buttons and sets them to visible. Refreshes
         *  values of member variables.
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
            get
            {
                return m_ownerTranspPanel;
            }
            set
            {
                m_ownerTranspPanel = value;
            }
        }

        public RichTextBox OwnerRichTextBox
        {
            get
            {
                return m_ownerRichTextBox;
            }
            set
            {
                m_ownerRichTextBox = value;
            }
        }

        public Panel OwnerBackPanel
        {
            get
            {
                return m_ownerBackPanel;
            }
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
         *  Shows option pop-up form for changing text options such as text size, font, color, and
         *  the text itself. It first passes current values into the m_optionsForm in order to update
         *  the UI display in the form. After the user has performed changes, current values are
         *  passed in again by reference in order to be updated with new values.
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
                m_optionsForm.StartPosition = FormStartPosition.CenterParent;
                m_optionsForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                m_optionsForm.MaximizeBox = false;
                m_optionsForm.MinimizeBox = false;
                m_optionsForm.ShowDialog();
                m_optionsForm.UpdateUIAttributes(ref m_textFont, ref m_textString, ref m_textBrush);
                // added this 'for data persistence'
                m_textBrushColor = m_textBrush.Color;
                
                m_ownerTranspPanel.Invalidate();
                m_ownerRichTextBox.Invalidate();
                m_ownerBackPanel.Invalidate();
                m_ownerTranspPanel.Refresh();
            }
        } /* private void m_optionsButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // This region contains m_deleteButton Property and Event Handlers
        #region m_deleteButton methods

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

        public List<VerticalText>OwnerVerticalTextList
        {
            get
            {
                return m_ownerVerticalTextList;
            }
            set
            {
                m_ownerVerticalTextList = value;
            }
        }

        /*
         * NAME
         *  m_deleteButton_MouseUp() - triggers delete options
         *  
         * SYNOPSIS
         *  private void m_deleteButton_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  This method triggers the options to delete 'this' instance of VerticalText. It uses the
         *  references passed to 'this' object from the owner of 'this'. First reference is to the
         *  TransparentPanel object in which 'this' resides as a UI element. Other reference is to
         *  the List<> container in which 'this' resides as a data persistence element. The point is
         *  to remove 'this' from both upon the user's request; i.e. upon clicking 'OK' button. The
         *  removal of 'this' also includes removing the buttons that belong to 'this' from the
         *  TransparentPanel object.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  4:15pm 5/17/2015
         */
        private void m_deleteButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this text?",
                    "Click yes or no", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
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
            }
        } /* private void m_deleteButton_MouseUp(object sender, MouseEventArgs e) */

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
         *  in the big screen and the location of m_rotateButton in the panel. This value is then
         *  stored in m_alteringButtonOffsetPoint and used in rotateButton_MouseMove event. Hides
         *  other buttons for nice appearances while text is being rotated.

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
         *  Updates the value of m_textAngle so that it can be used in drawVerticalText() when called
         *  externally.
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
         *      e       -> used to confirm that left mouse button was clicked
         * 
         * DESCRIPTION
         *  Updates the locations of all the buttons and sets them to visible. Refreshes
         *  values of member variables.
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
         *  these methods are member variables whose values were genereated using their respective
         *  event handlers. The string width is calculated to accomodate the posion and spacing of
         *  the buttons.
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
            // added this for data persistence
            m_textBrush.Color = m_textBrushColor;
            e.Graphics.DrawString(m_textString, m_textFont, m_textBrush, m_textOrigin);
            e.Graphics.ResetTransform();

            // Update values for button locations
            int maxWidth = 300;
            SizeF currentStringSize = new SizeF();
            currentStringSize = e.Graphics.MeasureString(m_textString, m_textFont, maxWidth);
            int intVal = Convert.ToInt32(currentStringSize.Width);
            
            // Don't let buttons get too close to each other when text is very short
            m_rotButDistF = Math.Abs(intVal + 14);
            if (m_rotButDistF < 36)
            {
                m_rotButDistF = 36;
            }
            m_optButDistF = m_rotButDistF / 3; // equal spacing
            m_delButDistF = m_rotButDistF / 3 * 2; // equal spacing
            if (oldRotButDistF != m_rotButDistF) // limit unnecessary calls to update button locations
            {
                updateButtonLocations();
            }
        } /* public void drawVerticalText(PaintEventArgs e) */

        /*
         * NAME
         *  updateButtonLocations() - updates the locations of the buttons
         *  
         * SYNOPSIS
         *  private void updateButtonLocations();
         * 
         * DESCRIPTION
         *  This method gets called by the _MouseUp events of m_rotateButton and m_moveButton, and by
         *  drawVerticalText() in order to update the locations of all the buttons. The calculations are 
         *  performed by using the location of 'm_moveButton' as the 'pivot point' and the angle of the
         *  text to project the direction of locations for the other buttons. The process is to first convert
         *  the text angle to radians; then use an equation based on (a) the pivot point, (b) the distance
         *  value of each button from the pivot point, (c) the text angle in radians used in a trigonometric
         *  function. Values are converted to 'float' and back when necessary in order to preserve accuracy.
         *  Finally, new locations are assigned to current buttons.
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
            m_moveButtonLocation = m_moveButton.Location;

            float optButDistF = m_optButDistF;
            float newOptLocX = (float)(X + Math.Cos(radAngleF) * optButDistF);
            float newOptLocY = (float)(Y + Math.Sin(radAngleF) * optButDistF);
            PointF newOptButPtF = new PointF(newOptLocX, newOptLocY);
            m_optionsButton.Location = Point.Round(newOptButPtF);
            // added for data persistence
            m_optionsButtonLocation = m_optionsButton.Location;

            float delButDistF = m_delButDistF;
            float newDelLocX = (float)(X + Math.Cos(radAngleF) * delButDistF);
            float newDelLocY = (float)(Y + Math.Sin(radAngleF) * delButDistF);
            PointF newDelButPtF = new PointF(newDelLocX, newDelLocY);
            m_deleteButton.Location = Point.Round(newDelButPtF);
            // added for data persistence
            m_deleteButtonLocation = m_deleteButton.Location;

            float rotButDistF = m_rotButDistF;
            float newRotLocX = (float)(X + Math.Cos(radAngleF) * rotButDistF);
            float newRotLocY = (float)(Y + Math.Sin(radAngleF) * rotButDistF);
            PointF newRotButPtF = new PointF(newRotLocX, newRotLocY);
            m_rotateButton.Location = Point.Round(newRotButPtF);
            // added for data persistence
            m_rotateButtonLocation = m_rotateButton.Location;

        } /* private void updateButtonLocations() */

        /*
         * NAME
         *  isNew() - checks to see if this VerticalText object is new
         *  
         * SYNOPSIS
         *  public bool isNew();
         * 
         * DESCRIPTION
         *  This method gets called to check and see if this object is new; i.e. was created and has not yet
         *  been modified. Such functionality will assist the owner of this object in preventing the creation
         *  of too many new instance by accidentally clicking on the panel to fast or too many times. Object
         *  is considered not new when the text has been changed.
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
         *  This method gets called to set the 'Visible' properties of all the buttons associated with this
         *  object to false. This is done to indicate to the user that VerticalText control is not currrently
         *  selected and to deliver a more presentable UI.
         * 
         * RETURNS
         *  Nothing
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
         *  This method gets called to set the 'Visible' properties of all the buttons associated with this
         *  object to true. This is done to indicate to the user that VerticalText control is currently
         *  selected in order to give the user tools to modify each instance of this VerticalText.
         * 
         * RETURNS
         *  Nothing
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

        // Data persistence
        /* 10:51am 5/20/2015
         *  // used in deserializer 
         */
        public VerticalText(SerializationInfo a_info, StreamingContext a_context)
        {
            m_textString = (String)a_info.GetValue("TextString", typeof(String));
            m_textFont = (Font)a_info.GetValue("TextFont", typeof(Font));
            //m_textBrush = (SolidBrush)a_info.GetValue("TextBrush", typeof(SolidBrush));
            m_textBrushColor = (Color)a_info.GetValue("TextBrushColor", typeof(Color));
            m_textOrigin = (Point)a_info.GetValue("TextOrigin", typeof(Point));
            m_textAngle = (Int32)a_info.GetValue("TextAngle", typeof(Int32));
            m_optButDistF = (float)a_info.GetValue("OptButDist", typeof(float));
            m_delButDistF = (float)a_info.GetValue("DelButDist", typeof(float));
            m_rotButDistF = (float)a_info.GetValue("RotButDist", typeof(float));
            //m_moveButton = (Button)a_info.GetValue("MoveButton", typeof(Button));
            m_moveButtonLocation = (Point)a_info.GetValue("MoveButtonLocation", typeof(Point));
        }
        /*
         *  11:08am 5/20/15
         *  
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("TextString", m_textString);
            a_info.AddValue("TextFont", m_textFont);
            //a_info.AddValue("TextBrush", m_textBrush);
            a_info.AddValue("TextBrushColor", m_textBrushColor);
            a_info.AddValue("TextOrigin", m_textOrigin);
            a_info.AddValue("TextAngle", m_textAngle);
            a_info.AddValue("OptButDist", m_optButDistF);
            a_info.AddValue("DelButDist", m_delButDistF);
            a_info.AddValue("RotButDist", m_delButDistF);
            //a_info.AddValue("MoveButton", m_moveButton);
            a_info.AddValue("MoveButtonLocation", m_moveButtonLocation);
        }
    }
}