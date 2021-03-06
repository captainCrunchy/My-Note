﻿using System;
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
 *      Fourth button is to allow the user to rotate the text to set it to a desired angle. Buttons, their locations,
 *      and their event handlers are created programmatically and are updated dynamically. To perform certain operations
 *      like deleting this object from its container or updating that container, references to owner containers are assigned
 *      to member variables. These four variables are prefixed with m_owner... and are weak references, which will not
 *      create a circular reference cycles. This class implements the 'ISerializable' interface, which allows this object
 *      to control its own serialization and deserialization. This class is marked with the 'SerializableAttribute' and
 *      is 'sealed' to prevent inheritance. Custom constructor is used in the deserialization process and regular constructor
 *      is used for creation and initialization of this object in the application. GetObjectData() is used for serialization
 *      of this object. Some objects, like buttons and brush, cannot be saved because 'Button' and 'Brush' classes do not
 *      implement 'ISerializable' intefrace. Instead of sub-classing and overriding these controls, simpler and more efficient
 *      approach was taken. For buttons, only their location values are saved and restored. Event handlers for these buttons
 *      also receive special care upon 'restore' of this object during data persistence operations. For brush, only the brush
 *      color needs to be saved and restored. Data persistence methods and their details are in the appropriate 'region'.
 *      
 *  CODE STRUCTURE:
 *      Member Variables and Constructor - Region contains member variables and constructor for this class
 *      m_moveButton Methods - Region contains elements associated with m_moveButton
 *      m_optionsButon Methods - Region contains elements associated with m_optionsButton
 *      m_deleteButton Methods - Region contains elements associated with m_deleteButton
 *      m_rotateButton Methods - Region contains elements associated with m_rotateButton
 *      Helper and Public Methods - Region contains 'helper' and public methods
 *      Data Persistence Methods - Region contains methods to assist data persistence\
 */
namespace My_Note
{
    [Serializable()]
    sealed class VerticalText : ISerializable
    {
        // Region contains member variables and constructor for this class
        #region Member Variables and Constructor

        private String m_textString = "Enter Text";         // Actual text used in drawVerticalText() updated in options
        private Font m_textFont = new Font("Microsoft Sans Serif", 12);  // Used in drawVerticalText() updated in options
        
        private SolidBrush m_textBrush = new SolidBrush(Color.Black);  // Used in drawVerticalText(), updated from options
        private Color m_textBrushColor = Color.Black;       // Used because SolidBrush object does not support 'serialization'

        public Point m_textOrigin;                          // Text origin used in drawVerticalText() updated with m_moveButton
        private Int32 m_textAngle = 0;                      // Text angle in degrees, used in drawVerticalText() and
                                                            // updateButtonLocations(), updated in m_rotateButton_MouseMove()
        
        private Button m_moveButton = new Button();         // Used to move the text around the panel
        private Point m_moveButtonLocation = new Point();   // Used because Button object does not support 'serialization'
        private bool m_isMoving = false;                    // Indicates whether the text is currently being moved
        private Point m_alteringButtonOffsetPoint = new Point();  // Offset point calculated by subtracting 'm_moveButton'
                                                                  // location or the 'm_rotateButton.Location' minus 
                                                                  // 'current point' (captured from main screen)

        private Button m_optionsButton = new Button();      // Brings up options window to modify text properties
        private VertTextOptionsForm m_optionsForm = new VertTextOptionsForm();  // Options window to modify text properties
        private float m_optButDistF = 32;                   // Distance between move and options buttons, used in
                                                            // updateButtonLocations(), updated in drawVerticalText()

        private Button m_deleteButton = new Button();       // Used to delete this object from its container
        private float m_delButDistF = 64;                   // Distance between move and delete buttons, used in
                                                            // updateButtonLocations(), updated in drawVerticalText()

        private Button m_rotateButton = new Button();       // Button that rotates text to user desired angles
        private bool m_isRotating = false;                  // Indicates whether the text is being rotated
        private float m_rotButDistF = 96;                   // Distance between move and rotate buttons, used in
                                                            // udpateButtonLocations(), updated in drawVerticalText()

        private List<VerticalText> m_ownerVerticalTextList; // Used to access container in which 'this' will be in, to
                                                            // assist the removal of 'this' object from that container
        private TransparentPanel m_ownerTranspPanel;        // Used to access panel on which 'this' will be drawn, to trigger the
                                                            // repaint upon the change and update of options of 'this' object
        private RichTextBox m_ownerRichTextBox;             // Used to access text box on which 'this' will be drawn to trigger
                                                            // the repaint upon the change and update of options of 'this' object
        private Panel m_ownerBackPanel;                     // Used to access panel on which 'this' will be drawn, to trigger
                                                            // the repaint upon the change and update of options of 'this' object

        /*
         * NAME
         *  VerticalText() - custom constructor for VerticalText object
         *  
         * SYNOPSIS
         *  public VerticalText(MouseEventArgs e);
         *      e       -> is used to get the location of mouse click
         * 
         * DESCRIPTION
         *  This constructor is called, specifically, from trapsarentPanel_MouseUp event handler. It
         *  utilizes (MouseEventArgs e) to get the location from the object that is using this class.
         *  This location is used to assign locations of other elements and save them for future uses.
         *  m_moveButton location is most important because it determines the location of all other
         *  elements and it is responsible for keeping 'this' object within its container. In this
         *  case, the container is the transparentPanel UI object, whose boundary values have been
         *  hard coded.
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
            m_moveButton.Size = new Size(16, 16);
            m_moveButton.MouseDown += m_moveButton_MouseDown;
            m_moveButton.MouseMove += m_moveButton_MouseMove;
            m_moveButton.MouseUp += m_moveButton_MouseUp;

            m_textOrigin = new Point(m_moveButton.Location.X + 8, m_moveButton.Location.Y + 8);

            m_optionsButton.BackColor = Color.Transparent;
            m_optionsButton.BackgroundImage = Properties.Resources.optionsImg;
            m_optionsButton.Location = new Point(m_moveButton.Location.X + (Int32)m_optButDistF, m_moveButton.Location.Y);
            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseUp += m_optionsButton_MouseUp;

            m_deleteButton.BackColor = Color.Transparent;
            m_deleteButton.BackgroundImage = Properties.Resources.removeX;
            m_deleteButton.Location = new Point(m_moveButton.Location.X + (Int32)m_delButDistF, m_moveButton.Location.Y);
            m_deleteButton.Size = new Size(16, 16);
            m_deleteButton.MouseUp += m_deleteButton_MouseUp;

            m_rotateButton.BackColor = Color.Transparent;
            m_rotateButton.BackgroundImage = Properties.Resources.rotateArrow;
            m_rotateButton.Location = new Point(m_moveButton.Location.X + (Int32)m_rotButDistF, m_moveButton.Location.Y);
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown += m_rotateButton_MouseDown;
            m_rotateButton.MouseMove += m_rotateButton_MouseMove;
            m_rotateButton.MouseUp += m_rotateButton_MouseUp;
        } /* public VerticalText(MouseEventArgs e) */

        #endregion

        // Region contains elements associated with m_moveButton
        #region m_moveButton Methods

        // Gets or sets move button
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
         *  Prepares this VerticalText object to be moved by capturing the mouse click location within
         *  the main screen, then it translates/calculates this initial location to the location within
         *  the panel to be used in by getting the difference between location in the big screen and
         *  the location of m_moveButton in the panel. This value is then stored in m_alteringButtonOffsetPoint
         *  and used in moveButton_MouseMove event. Hides other buttons for nice appearance while text
         *  is being moved.
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
         *  Moves the VerticalText object by updating its new location based on the MouseEventArg
         *  and the offset calculated and recorded in the MouseDown event earlier. First it updates
         *  the position of the m_moveButton itself, then it calculates and updates the position
         *  of the m_textOrigin point.
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
         *  Updates the locations of all the buttons and sets them to visible. Refreshes values of
         *  member variables.
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

        // Region contains elements associated with m_optionsButton
        #region m_optionsButton Methods

        // Gets or sets options button
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

        // Gets or sets transparent panel reference
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

        // Gets or sets rich text box reference
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

        // Gets or sets back panel reference
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
                m_textBrushColor = m_textBrush.Color;
                
                m_ownerTranspPanel.Invalidate();
                m_ownerRichTextBox.Invalidate();
                m_ownerBackPanel.Invalidate();
                m_ownerTranspPanel.Refresh();
            }
        } /* private void m_optionsButton_MouseUp(object sender, MouseEventArgs e) */

        #endregion

        // Region contains elements associated with m_deleteButton
        #region m_deleteButton Methods

        // Gets or sets delete button
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

        // Gets or sets VerticalText container reference
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
         *  This method triggers message box asking to delete 'this' instance of VerticalText. If the
         *  user clicks 'OK', then a delete process is started. Since 'this' instance has a reference
         *  to the container (transparentPanel) that holds the buttons of 'this' class, it removes
         *  those buttons from that container. Since 'this' instance also has a reference to the
         *  container (m_verticalTextList) that holds 'this' instance, 'this' removes itself from
         *  that container.
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
                    "Confirm Delete", MessageBoxButtons.OKCancel);
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

        // Region contains elements associated with m_rotateButton
        #region m_rotateButton Methods

        // Gets or sets rotate button
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
         *  location within the panel to be used in by getting the difference between location in
         *  the big screen and the location of m_rotateButton in the panel. This value is then stored
         *  in m_alteringButtonOffsetPoint and used in rotateButton_MouseMove event. Hides other
         *  buttons for nice appearances while text is being rotated.

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
         *  position (m_moveButton), then uses the results in a trigonometric function to calculate
         *  and assign a new angle to the text string. Updates the location of m_rotateButton based
         *  on the (MouseEventArg e) and the offset calculated and recorded in the MouseDown event
         *  earlier. Updates the value of m_textAngle so that it can be used in drawVerticalText()
         *  when called externally.
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
         *  Updates the locations of all the buttons and sets them to visible. Refreshes values of
         *  member variables.
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

        // Region contains 'helper' and public methods
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
         *  methods used in this method are member variables whose values were genereated using their
         *  respective event handlers. The string width is calculated to accomodate the position and
         *  spacing of the buttons.
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
            // equal spacing
            m_optButDistF = m_rotButDistF / 3;
            // equal spacing
            m_delButDistF = m_rotButDistF / 3 * 2;
            // limit unnecessary calls to update button locations
            if (oldRotButDistF != m_rotButDistF)
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
         *  drawVerticalText() in order to update the locations of all the buttons. The calculations
         *  are performed by using the location of 'm_moveButton' as the 'pivot point' and the angle
         *  of the text to project the direction of locations for the other buttons. The process is
         *  to first convert the text angle to radians; then use an equation based on (a) the pivot
         *  point (b) the distance value of each button from the pivot point (c) the text angle in
         *  radians used in a trigonometric function. Values are converted to 'float' and back when
         *  necessary in order to preserve accuracy. Finally, new locations are assigned to current
         *  buttons.
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
         *  This method gets called to check and see if this object is new; i.e. was created and
         *  has not yet been modified. Such functionality will assist the owner of this object in
         *  preventing the creation of too many new instance by accidentally clicking on the panel
         *  to fast or too many times. Object is considered not new when the text has been changed.
         * 
         * RETURNS
         *  True if this object was just created and false if it has been modified
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
         *  with this object to false. This is done to indicate to the user that VerticalText control
         *  is not currrently selected and to deliver a more presentable UI.
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
         *  This method gets called to set the 'Visible' properties of all the buttons associated
         *  with this object to true. This is done to indicate to the user that VerticalText control
         *  is currently selected in order to give the user tools to modify each instance of this
         *  VerticalText.
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

        // Region contains methods to assist data persistence
        #region Data Persistence Methods

        /*
         * NAME
         *  setButtonProperties() - sets button properties
         *  
         * SYNOPSIS
         *  public void setButtonProperties();
         * 
         * DESCRIPTION
         *  This method is used to set properties for buttons belonging to this class. This method
         *  was added to meet the needs of data persistence technique used in this application. Since
         *  'Button' class does not support serialization its attributes are saved and written to
         *  disk instead. Upon restoring this object, upon startup or 'Page' change, 'Button' member
         *  variables are recreated but their values do not get reassigned again because the constructor
         *  does not get called again. Their values get reassigned in this method, called by an outside
         *  object when restoring 'this'. Special technique was used to reassign event handlers to
         *  'Button' objects because, unlike their other properties, event handlers do not get replaced
         *  by new ones with the same name, instead they are added to existing ones. That is why they
         *  are first removed, then added again. This safe and effective technique was used to because
         *  the alternative solution would be far more complicated.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:32pm 5/20/2015
         */
        public void setButtonProperties()
        {
            m_moveButton.BackColor = Color.Transparent;
            m_moveButton.BackgroundImage = Properties.Resources.moveArrow;
            m_moveButton.Location = m_moveButtonLocation;
            m_moveButton.Size = new Size(16, 16);
            m_moveButton.MouseDown -= m_moveButton_MouseDown;
            m_moveButton.MouseMove -= m_moveButton_MouseMove;
            m_moveButton.MouseUp -= m_moveButton_MouseUp;
            m_moveButton.MouseDown += m_moveButton_MouseDown;
            m_moveButton.MouseMove += m_moveButton_MouseMove;
            m_moveButton.MouseUp += m_moveButton_MouseUp;

            m_optionsButton.BackColor = Color.Transparent;
            m_optionsButton.BackgroundImage = Properties.Resources.optionsImg;
            m_optionsButton.Size = new Size(16, 16);
            m_optionsButton.MouseUp -= m_optionsButton_MouseUp;
            m_optionsButton.MouseUp += m_optionsButton_MouseUp;

            m_deleteButton.BackColor = Color.White;
            m_deleteButton.BackColor = Color.Transparent;
            m_deleteButton.BackgroundImage = Properties.Resources.removeX;
            m_deleteButton.Size = new Size(16, 16);
            m_deleteButton.MouseUp -= m_deleteButton_MouseUp;
            m_deleteButton.MouseUp += m_deleteButton_MouseUp;

            m_rotateButton.BackColor = Color.Transparent;
            m_rotateButton.BackgroundImage = Properties.Resources.rotateArrow;
            m_rotateButton.Size = new Size(16, 16);
            m_rotateButton.MouseDown -= m_rotateButton_MouseDown;
            m_rotateButton.MouseMove -= m_rotateButton_MouseMove;
            m_rotateButton.MouseUp -= m_rotateButton_MouseUp;
            m_rotateButton.MouseDown += m_rotateButton_MouseDown;
            m_rotateButton.MouseMove += m_rotateButton_MouseMove;
            m_rotateButton.MouseUp += m_rotateButton_MouseUp;
        } /* public void setButtonProperties() */

        // Data persistence
        /* 10:51am 5/20/2015
         *  // used in deserializer 
         */
        /*
         * NAME
         *  VerticalText() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public VerticalText(SerializationInfo a_info, StreamingContext a_context);
         *      a_info      -> provides data it has stored
         *      a_context   -> does nothing (required)
         *      
         * DESCRIPTION
         *  This constructor gets called when instances of this object are to be deserialized. Since
         *  not all member variables were saved, additional member variables were saved to help restore
         *  process. Such variables are m_textBrushColor and m_moveButtonLocation.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:27pm 5/19/2015
         */
        public VerticalText(SerializationInfo a_info, StreamingContext a_context)
        {
            m_textString = (String)a_info.GetValue("TextString", typeof(String));
            m_textFont = (Font)a_info.GetValue("TextFont", typeof(Font));
            m_textBrushColor = (Color)a_info.GetValue("TextBrushColor", typeof(Color));
            m_textOrigin = (Point)a_info.GetValue("TextOrigin", typeof(Point));
            m_textAngle = (Int32)a_info.GetValue("TextAngle", typeof(Int32));
            m_optButDistF = (float)a_info.GetValue("OptButDist", typeof(float));
            m_delButDistF = (float)a_info.GetValue("DelButDist", typeof(float));
            m_rotButDistF = (float)a_info.GetValue("RotButDist", typeof(float));
            m_moveButtonLocation = (Point)a_info.GetValue("MoveButtonLocation", typeof(Point));
        } /* public VerticalText(SerializationInfo a_info, StreamingContext a_context) */

        /*
         * NAME
         *  GetObjectData() - used to serialize this object
         *  
         * SYNOPSIS
         *  public void GetObjectData(SerializationInfo a_info, StreamingContext a_context);
         *      a_info      -> stores data needed to serialize an object
         *      a_context   -> does nothing (required)
         *      
         * DESCRIPTION
         *  This method is used to serialize this object by storing member variables into SerializationInfo
         *  object. Not all member variables of this class can be saved, therefore additional variables
         *  are used, m_textBrushColor and m_moveButtonLocation.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  11:08am 5/20/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("TextString", m_textString);
            a_info.AddValue("TextFont", m_textFont);
            a_info.AddValue("TextBrushColor", m_textBrushColor);
            a_info.AddValue("TextOrigin", m_textOrigin);
            a_info.AddValue("TextAngle", m_textAngle);
            a_info.AddValue("OptButDist", m_optButDistF);
            a_info.AddValue("DelButDist", m_delButDistF);
            a_info.AddValue("RotButDist", m_delButDistF);
            a_info.AddValue("MoveButtonLocation", m_moveButtonLocation);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */

        #endregion
    }
}
