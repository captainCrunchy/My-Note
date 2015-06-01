using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;

/* 
 *  TITLE:
 *      MainForm : Form
 *      
 *  DESCRIPTION:
 *    MainForm class:
 *      This class represents the main form for the application. It is the starting point and is always visible. It provides
 *      common controls such as 'File' and 'Help' menu options, text editing and drawing controls, and a 'combined' panel
 *      for text editing and drawing shapes. This MainForm class is divided into four (.cs) files, which are simply extensions
 *      of this class; i.e. each is a 'public partial class MainForm : Form'. This was done to keep the code organized and
 *      readable. The user is interacting with some part of this class at all times.
 *    mainForm.cs:
 *      This file implements tasks that are responsible for starting and running the application. It performs general tasks
 *      like handling the user inteface elements of the form and communication with data persistence objects. It is also
 *      responsible for coordinating tasks between other 'partial class' files.
 *    formMenuStrip.cs:
 *      This file handles events that are triggered by elements of the menu strip in the form and their appearances based on
 *      current data. Example: File, Edit, ..., Help.
 *    formToolbar.cs:
 *      This file is responsible for appearance of controls in the toolbar and their events. These controls trigger such tasks
 *      as text editing, drawing shapes, and erasing.
 *    formTextBox.cs: (YOU ARE HERE)
 *      This file is responsible for appearances and events of the richTextBox and its layers. Such additional layers are
 *      transparent and background panels. Events handled in this files are tasks such as applying text editing and drawing
 *      shapes onto the panels, and erasing them based on currently selected controls and options.
 *      
 *  CODE STRUCTURE:
 *    MainForm class:
 *      This class is divided into four (.cs) files based on functionality. Each is responsible for performing specific tasks
 *      based on the user interface elements and controls. Each (.cs) file declares and initializes member variables that are
 *      needed in that file. Some member variables can only be initialized in the constructor, which is in the mainForm.cs file.
 *    formTextbox.cs: (YOU ARE HERE)
 *      This file is organized by separating code into 'regions' based on specific functionalities. It contains member variables
 *      used in this file. Some member variables need to be initialized in the constructor which is in the mainForm.cs file.
 *      Regions contain related code representing UI elements and other functionality. Helper methods are toward the end and serve
 *      all other methods in this file.
 */

namespace My_Note
{
    public partial class MainForm : Form
    {
        // Region contains member variables for this class
        #region Member Variables

        private Graphics m_transparentPanelGraphics;                        // Used to reduce repetitive data creation
        private Pen m_transparentPanelPen;                                  // Used to reduce repetitive data creation

        private ShapeContainer m_shapesStorage = new ShapeContainer();      // Storage of all the drawing data
        private bool m_isDrawing = false;                                   // Is the mouse currently down and drawing
        private bool m_isErasing = false;                                   // Is the mouse currently down and erasing
        private Point m_lastPosition = new Point(0, 0);                     // Last cursor position, used to cut down on repetative data
        private Color m_currentDrawColor = Color.Black;                     // Current drawing color
        private float m_currentPenWidth = 1;                                // Current pen width (does not change)
        private int m_shapeNumber = 0;                                      // Record the shape numbers so they can be drawn separately
        private Point m_drawStartPoint = new Point(0, 0);                   // Start point of arrow or line
        private Point m_drawEndPoint = new Point(0, 0);                     // End point of arrow or line
        
        private Point m_arrowLeftSide = new Point(0, 0);                    // Left side of the arrow head
        private Point m_arrowRightSide = new Point(0, 0);                   // Right side of the arrow head
        private Int32 m_arrowFarPoint = 0;                                  // Used in the diagonal arrows

        private bool m_canDash = false;                                     // Used when saving dashed or dotted lines
        
        private List<VerticalText> m_verticalTextList = new List<VerticalText>();  // Used to store VerticalText instances
        
        private int m_richTextBoxSelectStartCharIndex = 0;                  // Record start index before text selection using mouse
        private int m_richTextBoxSelectCurrentCharIndex = 0;                // Record current index during text selection using mouse
        private bool m_isSelectingText = false;                             // Is the mouse currently down and selecting text

        #endregion

        // Region contains methods for richTextBox
        #region richTextBox Methods

        /*
         * NAME
         *  richTextBox_TextChanged() - repaints UI elements when text is changed
         *  
         * SYNOPSIS
         *  private void richTextBox_TextChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Calls invalidate on richTextBox and transparentPanel during text typing
         *  in order to update current graphics of UI elements.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:44am 3/11/2015
         */
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox.Invalidate();
            transparentPanel.Invalidate();
        } /* private void richTextBox_TextChanged(object sender, EventArgs e) */

        #endregion

        // Region contains event handler methods for transparentPanel
        #region transparentPanelEvent Methods

        /*
         * NAME
         *  transparentPanel_Click() - event handler for transparentPanel
         * 
         * SYNOPSIS
         *  private void transparentPanel_Click(object sender, System.EventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture the location of the cursor upon click and
         *                 confirm that the left mouse button was clicked
         *      
         * DESCRIPTION
         *  When textControl is selected, this method will pass down the mouse location coordinates through
         *  the transparentPanel and to the richTextBox so that the text cursor is positioned in the right
         *  place. Additional calculations are performed because the origin point of richTextBox is away from
         *  the origin points of its front and back layers.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:47pm 3/7/2015
         */
        private void transparentPanel_Click(object sender, System.EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                // If currently in text editing mode
                if (m_currentSelectedControl == e_SelectedControl.TEXT)
                {
                    Point capturedPoint = new Point(mouseEventArgs.X - 40, mouseEventArgs.Y - 35);
                    int charIndex = richTextBox.GetCharIndexFromPosition(capturedPoint);
                    int lastChar = richTextBox.TextLength;
                    Point lastCharPoint = richTextBox.GetPositionFromCharIndex(lastChar);

                    if (lastCharPoint.X == capturedPoint.X ||
                        lastCharPoint.Y < capturedPoint.Y)
                    {
                        charIndex++;
                    }

                    richTextBox.SelectionStart = charIndex;
                    richTextBox.SelectionLength = 0;
                    richTextBox.Select();
                    transparentPanel.Refresh();
                }
            }
        } /* private void transparentPanel_Click(object sender, System.EventArgs e) */

        /*
         * NAME
         *  transparentPanel_MouseDown() - prepares drawing controls to be used 
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture current location of cursor, assign it to other
         *                 values, and confirm that left mouse button was clicked
         *      
         * DESCRIPTION
         *  Prepares drawing controls to be used by setting/updating values of member variables.
         *  If text control is selected, then this method prepares a multi-character text selection.
         *  Pencil and eraser controls need simpler preparation processes. Last two conditions cast
         *  the 'enum' type m_currentSelectedControl to 'int' in order to test as many conditions as
         *  possible while keeping a minimum required lines of code and maintaining readability.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:01am 3/10/2015
         */
        private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (m_currentSelectedControl == e_SelectedControl.TEXT)
                {
                    Point newPoint = new Point(e.X - 40, e.Y - 35); // Text box origin has an offset
                    m_richTextBoxSelectStartCharIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                }
                if (m_currentSelectedControl == e_SelectedControl.PENCIL)
                {
                    m_isDrawing = true;
                    m_shapeNumber++;
                    m_lastPosition = new Point(0, 0);
                }
                if (m_currentSelectedControl == e_SelectedControl.ERASER)
                {
                    m_isErasing = true;
                }
                // This technique saves many lines of code and  uses less test conditions
                if (((int)m_currentSelectedControl > 2) &&
                    ((int)m_currentSelectedControl < 14))
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                }

                if (((int)m_currentSelectedControl > 13) &&
                    ((int)m_currentSelectedControl < 16))
                {
                    m_isDrawing = true;
                    m_drawStartPoint = e.Location;
                    m_canDash = true;
                }
            }
        } /* private void transparentPanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  transparentPanel_MouseMove() - performs drawing operations for each of the selected shapes
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseMove(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture current location of cursor, pass it to helper methods,
         *                 and confirm that left mouse button was clicked
         *      
         * DESCRIPTION
         *  Based on the control selected start drawing, saving, or erasing. Some of the shapes are drawn
         *  and saved immediately in this method (pencil, eraser). Other shapes (lines, arrows, ovals, rectangles)
         *  are only drawn here to display to the user a dynamic response. They get saved on MouseUp event.
         *  Eraser only removes anything that is not text. When text control is selected this method is used
         *  for multi-character and/or multi-line selection in the richTextBox, as the mouse is down and being
         *  dragged. Such technique is necessary because the richTextBox is 'under' the transparentPanel and is
         *  capturing a Point 'through' the trasparentPanel, which it then translates into a character index.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:01am 3/10/2015
         */
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_currentSelectedControl == e_SelectedControl.TEXT)  // Highlight/select text
                {
                    m_isSelectingText = true;
                    Point newPoint = new Point(e.X - 40, e.Y - 35);
                    m_richTextBoxSelectCurrentCharIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                    richTextBox.SelectionStart = Math.Min(m_richTextBoxSelectStartCharIndex, m_richTextBoxSelectCurrentCharIndex);
                    richTextBox.SelectionLength = Math.Abs(m_richTextBoxSelectCurrentCharIndex - m_richTextBoxSelectStartCharIndex);
                    richTextBox.Select();
                }
                if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.PENCIL))  // Draw using pencil
                {
                    drawWithPencil(e);
                }
                if (m_isErasing && (m_currentSelectedControl == e_SelectedControl.ERASER))  // Erase shapes
                {
                    startErasing(e);
                }
                if (m_isDrawing && m_lastPosition != e.Location)  // Draw an arrow in one of the directions
                {
                    if ((m_currentSelectedControl == e_SelectedControl.WARROW) && (e.Location.X < m_drawStartPoint.X))
                    {
                        drawWestArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.NWARROW) &&
                        (e.Location.X < m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthWestArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.NARROW) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.NEARROW) && 
                        (e.Location.X > m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        drawNorthEastArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.EARROW) && (e.Location.X > m_drawStartPoint.X))
                    {
                        drawEastArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.SEARROW) &&
                        (e.Location.X > m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthEastArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.SARROW) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthArrow(e);
                    }
                    if ((m_currentSelectedControl == e_SelectedControl.SWARROW) &&
                        (e.Location.X < m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        drawSouthWestArrow(e);
                    }
                }
                if (m_isDrawing)  // Draw rectangle, ellipse, or a line (solid, dashed, dotted)
                {
                    if (m_currentSelectedControl == e_SelectedControl.RECTANGLE)
                    {
                        drawRectangle(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.ELLIPSE)
                    {
                        drawEllipse(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.SOLID)
                    {
                        drawSolidLine(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.DASHED)
                    {
                        drawDashedLine(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.DOTTED)
                    {
                        drawDottedLine(e);
                    }
                }
            }
        } /* private void transparentPanel_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  transparentPanel_MouseUp() - updates values and saves data
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture current location of cursor, pass it to helper methods,
         *                 and confirm that left mouse button was clicked
         *      
         * DESCRIPTION
         *  Updates values, saves data, redraws transparentPanel and richTextBox. Pencil and eraser
         *  actions are already saved up to this point. Lines, arrows, and other shapes use special
         *  methods for saving, which are triggered by this method. Some conditions test for several
         *  values at once to ensure/restrict the direction of the arrow being drawn. When adding a
         *  VerticalText object (rotatable text feature) a condition tests to see if one has been recently
         *  created and modified. Such mechanism is used to ensure that the user does not accidentally
         *  create too many such objects.
         *  
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:49am 3/10/2015
         */
        private void transparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((m_currentSelectedControl == e_SelectedControl.TEXT) && (m_isSelectingText == true))  // Finished highlighting/selecting
                {
                    richTextBox.SelectionStart = Math.Min(m_richTextBoxSelectStartCharIndex, m_richTextBoxSelectCurrentCharIndex);
                    richTextBox.SelectionLength = Math.Abs(m_richTextBoxSelectCurrentCharIndex - m_richTextBoxSelectStartCharIndex);
                    richTextBox.Select();
                    richTextBox.Invalidate();
                    transparentPanel.Invalidate();
                    m_isSelectingText = false;
                }
                if (m_currentSelectedControl == e_SelectedControl.PENCIL)  // Finished using pencil
                {
                    transparentPanel.Invalidate();
                    richTextBox.Invalidate();
                    backPanel.Invalidate();
                }
                if (m_currentSelectedControl == e_SelectedControl.ERASER)  // Finished erasing
                {
                    transparentPanel.Invalidate();
                    richTextBox.Invalidate();
                }
                if (m_isDrawing) // Finished using an arrow, rectangle, ellipse, and line (dashed, solid, dotted). Now save.
                {
                    if (m_currentSelectedControl == e_SelectedControl.WARROW && (e.Location.X < m_drawStartPoint.X))
                    {
                        saveWestArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.NWARROW &&
                        (e.Location.X < m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        saveNorthWestArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.NARROW && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        saveNorthArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.NEARROW &&
                        (e.Location.X > m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y))
                    {
                        saveNorthEastArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.EARROW && (e.Location.X > m_drawStartPoint.X))
                    {
                        saveEastArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.SEARROW &&
                        (e.Location.X > m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        saveSouthEastArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.SARROW && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        saveSouthArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.SWARROW &&
                        (e.Location.X < m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y))
                    {
                        saveSouthWestArrow(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.RECTANGLE)
                    {
                        saveRectangle(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.ELLIPSE)
                    {
                        saveEllipse(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.SOLID)
                    {
                        saveSolidLine(e);
                    }
                    if (m_currentSelectedControl == e_SelectedControl.DASHED)
                    {
                        saveDashedLine(e);
                        m_canDash = false;
                    }
                    if (m_currentSelectedControl == e_SelectedControl.DOTTED)
                    {
                        saveDottedLine(e);
                        m_canDash = false;
                    }
                    m_isDrawing = false;
                }
                if (m_currentSelectedControl == e_SelectedControl.VERTTEXT)  // Added VerticalText object
                {
                    foreach (VerticalText v in m_verticalTextList)
                    {
                        if (v.isNew()) { return; }
                    }
                    createVertTextObject(e);
                    transparentPanel.Invalidate();
                    richTextBox.Invalidate();
                    backPanel.Invalidate();
                    transparentPanel.Refresh();
                }
            }
        } /* private void transparentPanel_MouseUp(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  transparentPanel_Layout() - triggers _Paint event method
         *  
         * SYNOPSIS
         *  private void transparentPanel_Layout(object sender, LayoutEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  This method gets called automatically whenever any element within transparentPanel makes any kind of
         *  change, including the object whose behavior it is declared to accomodate. That is, the object of the
         *  VerticalTextOptionsForm class needs to call _Paint method of the transparentPanel when it is closed.
         *  This is the simplest approach for accomplishing the task.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:02pm 4/12/2015
         */
        private void transparentPanel_Layout(object sender, LayoutEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.VERTTEXT)
            {
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
                backPanel.Invalidate();
            }
        } /* private void transparentPanel_Layout(object sender, LayoutEventArgs e) */

        /*
         * NAME
         *  transparentPanel_Paint() - updates the graphics drawn on the transparent panel
         * 
         * SYNOPSIS
         *  private void transparentPanel_Paint(object sender, PaintEventArgs e);
         *      sender  -> does nothing
         *      e       -> used for smoothing drawing and drawing lines
         *      
         * DESCRIPTION
         *  Redraws the tranparentPanel graphics. This is done by accessing the m_shapesStorage object of the
         *  ShapeContainer class, which holds a list of shapes. Each shape in this list is represented as a
         *  set of points with a common shape number. The 'for-loop' below accesses two points from the list
         *  at a time; if they share the same shape number, then they are connected to form a line. If the shape
         *  numbers are different, then a new shape begins to form. This method gets triggered by the OnPaint()
         *  method of the TransparentPanel.cs custom subclass and by the base.OnPaint(e) method. It also redraws
         *  the VerticalText objects by iterating over the container that holds them.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:52am 3/10/2015
         */
        private void transparentPanel_Paint(object sender, PaintEventArgs e)
        {
            // Apply a smoothing mode to smooth out the line.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            // Begin drawing
            for (int i = 0; i < m_shapesStorage.NumberOfShapes() - 1; i++)
            {
                Shape pointOne = m_shapesStorage.GetShape(i);
                Shape pointTwo = m_shapesStorage.GetShape(i + 1);
                if (pointOne.ShapeNumber == pointTwo.ShapeNumber)
                {
                    Pen p = new Pen(pointOne.LineColor, pointOne.LineWidth);
                    e.Graphics.DrawLine(p, pointOne.PointLocation, pointTwo.PointLocation);
                    p.Dispose();
                }
            }
            for (int i = 0; i < m_verticalTextList.Count; i++)
            {
                m_verticalTextList[i].drawVerticalText(e);
            }
        } /* private void transparentPanel_Paint(object sender, PaintEventArgs e) */

        #endregion

        // Region contains 'helper' methods
        #region Helper Methods

        /*
         * NAME
         *  drawWithPencil() - draws shapes using a pencil
         * 
         * SYNOPSIS
         *  private void drawWithPencil(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws any shape using a pencil. This method draws and saves the shapes at the same time.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:44pm 3/22/2015
         */
        private void drawWithPencil(MouseEventArgs e)
        {
            if (m_lastPosition != e.Location)
            {
                m_lastPosition = e.Location;
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                transparentPanel.Refresh();
            }
        } /* private void drawWithPencil(MouseEventArgs e) */

        /*
         * NAME
         *  startErasing() - erases contents on the panel
         * 
         * SYNOPSIS
         *  private void startErasing(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Erases shapes from the screen by first removing them from the m_shapesStorage container
         *  and redraws the panels to give the effect of erasing. Eraser removes anything that is
         *  not text and saving occurs immediately. Erases sets of points and not entire shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:49pm 3/22/2015
         */
        private void startErasing(MouseEventArgs e)
        {
            m_shapesStorage.RemoveShape(e.Location, 10);
            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void startErasing(MouseEventArgs e) */

        /*
         * NAME
         *  drawWestArrow() - draws an arrow pointing west
         * 
         * SYNOPSIS
         *  private void drawWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to west direction as the user has the mouse down and dragging it. The
         *  location of endpoint of the arrow is dictated by the current value of the X coordinate in the
         *  appropriate direction away from the start point. This method simply draws the arrow to display to
         *  the user a real-time response. Saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:20am 3/20/2015
         */
        private void drawWestArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            m_lastPosition.X = e.Location.X;  // Restrict drawing direction (west)
            m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X + 5;
            m_arrowRightSide.Y = m_drawStartPoint.Y - 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X + 5;
            m_arrowLeftSide.Y = m_drawStartPoint.Y + 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_lastPosition, m_arrowLeftSide);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate(); 
        } /* private void drawWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveWestArrow() - saves an arrow drawn by the user pointing west
         * 
         * SYNOPSIS
         *  private void saveWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to west direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:37am 3/22/2015
         */
        private void saveWestArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = m_drawStartPoint.X; i >= e.Location.X; i--)
            {
                m_lastPosition.X = i;  // Restrict drawing direction (west)
                m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;
            m_arrowRightSide.X = e.Location.X;
            m_arrowRightSide.Y = m_drawStartPoint.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowRightSide.X++;
                m_arrowRightSide.Y--;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            m_shapeNumber++;
            m_arrowLeftSide.X = e.Location.X;
            m_arrowLeftSide.Y = m_drawStartPoint.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowLeftSide.X++;
                m_arrowLeftSide.Y++;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawNorthWestArrow() - draws an arrow pointing north west
         * 
         * SYNOPSIS
         *  private void drawNorthWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to north west direction as the user has the mouse down and dragging it.
         *  The location of endpoint of the arrow is dictated by the furthest point of either horizontal or
         *  vertical value towards the appropriate direction from the start point. This method simply draws the
         *  arrow to display to the user a real-time response. Saving does not occur here. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:38am 3/20/2015
         */
        private void drawNorthWestArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            // Get the longest distance from the starting point and use that as the limit of diagonal line
            if ((m_drawStartPoint.X - e.Location.X) > (m_drawStartPoint.Y - e.Location.Y))
            {
                m_arrowFarPoint = m_drawStartPoint.X - e.Location.X;
            }
            else
            {
                m_arrowFarPoint = m_drawStartPoint.Y - e.Location.Y;
            }

            m_lastPosition.X = m_drawStartPoint.X - m_arrowFarPoint;
            m_lastPosition.Y = m_drawStartPoint.Y - m_arrowFarPoint;

            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X + 8;
            m_arrowRightSide.Y = m_lastPosition.Y;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X;
            m_arrowLeftSide.Y = m_lastPosition.Y + 8;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawNorthWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveNorthWestArrow() - saves an arrow drawn by the user pointing north west
         * 
         * SYNOPSIS
         *  private void saveNorthWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to north west direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:52am 3/22/2015
         */
        private void saveNorthWestArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = 0; i < m_arrowFarPoint; i++)
            {
                m_lastPosition.X = m_drawStartPoint.X - i;
                m_lastPosition.Y = m_drawStartPoint.Y - i;
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;

            for (int i = 0; i < 8; i++)
            {
                m_arrowRightSide.X--;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_shapeNumber++;
            for (int i = 0; i < 8; i++)
            {
                m_arrowLeftSide.Y--;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveNorthWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawNorthArrow() - draws an arrow pointing north
         * 
         * SYNOPSIS
         *  private void drawNorthArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to north direction as the user has the mouse down and dragging it. The
         *  location of endpoint of the arrow is dictated by the current value of the Y coordinate in the
         *  appropriate direction away from the start point. This method simply draws the arrow to display to
         *  the user a real-time response. Saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:51pm 3/21/2015
         */
        private void drawNorthArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            m_lastPosition.X = m_drawStartPoint.X;  // Restrict horizontal movement
            m_lastPosition.Y = e.Location.Y;  // Restrict drawing direction (north)
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X + 5;
            m_arrowRightSide.Y = m_lastPosition.Y + 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X - 5;
            m_arrowLeftSide.Y = m_lastPosition.Y + 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawNorthArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveNorthArrow() - saves an arrow drawn by the user pointing north
         * 
         * SYNOPSIS
         *  private void saveNorthArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to north direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:07am 3/22/2015
         */
        private void saveNorthArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = m_drawStartPoint.Y; i >= e.Location.Y; i--)
            {
                m_lastPosition.X = m_drawStartPoint.X;  // Restrict horizontal movement
                m_lastPosition.Y = i;  // Restrict drawing direction (north)
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;
            m_arrowRightSide.X = m_drawStartPoint.X;
            m_arrowRightSide.Y = e.Location.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowRightSide.X++;
                m_arrowRightSide.Y++;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            m_shapeNumber++;
            m_arrowLeftSide.X = m_drawStartPoint.X;
            m_arrowLeftSide.Y = e.Location.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowLeftSide.X--;
                m_arrowLeftSide.Y++;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveNorthArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawNorthEastArrow() - draws an arrow pointing north east
         * 
         * SYNOPSIS
         *  private void drawNorthEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to north east direction as the user has the mouse down and dragging it.
         *  The location of endpoint of the arrow is dictated by the furthest point of either horizontal or
         *  vertical value towards the appropriate direction from the start point. This method simply draws the
         *  arrow to display to the user a real-time response. Saving does not occur here. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:19pm 3/22/2015
         */
        private void drawNorthEastArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            // Get the longest distance from the starting point and use that as the limit of diagonal line
            if ((e.Location.X - m_drawStartPoint.X) > (e.Location.Y - m_drawStartPoint.Y))
            {
                m_arrowFarPoint = e.Location.X - m_drawStartPoint.X;
            }
            else
            {
                m_arrowFarPoint = e.Location.Y - m_drawStartPoint.Y;
            }

            m_lastPosition.X = m_drawStartPoint.X + m_arrowFarPoint;
            m_lastPosition.Y = m_drawStartPoint.Y - m_arrowFarPoint;

            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X;
            m_arrowRightSide.Y = m_lastPosition.Y + 8;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X - 8;
            m_arrowLeftSide.Y = m_lastPosition.Y;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawNorthEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveNorthEastArrow() - saves an arrow drawn by the user pointing north east
         * 
         * SYNOPSIS
         *  private void saveNorthEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to north east direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:44pm 3/22/2015
         */
        private void saveNorthEastArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = 0; i < m_arrowFarPoint; i++)
            {
                m_lastPosition.X = m_drawStartPoint.X + i;
                m_lastPosition.Y = m_drawStartPoint.Y - i;
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;

            for (int i = 0; i < 8; i++)
            {
                m_arrowRightSide.Y--;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_shapeNumber++;
            for (int i = 0; i < 8; i++)
            {
                m_arrowLeftSide.X++;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveNorthEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawEastArrow() - draws an arrow pointing east
         * 
         * SYNOPSIS
         *  private void drawEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to east direction as the user has the mouse down and dragging it. The
         *  location of endpoint of the arrow is dictated by the current value of the X coordinate in the
         *  appropriate direction away from the start point. This method simply draws the arrow to display to
         *  the user a real-time response. Saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:01pm 3/22/2015
         */
        private void drawEastArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            m_lastPosition.X = e.Location.X;  // Restrict drawing direction (east)
            m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X - 5;
            m_arrowRightSide.Y = m_drawStartPoint.Y + 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X - 5;
            m_arrowLeftSide.Y = m_drawStartPoint.Y - 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_lastPosition, m_arrowLeftSide);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveEastArrow() - saves an arrow drawn by the user pointing east
         * 
         * SYNOPSIS
         *  private void saveEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to east direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:12pm 3/22/2015
         */
        private void saveEastArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = m_drawStartPoint.X; i <= e.Location.X; i++)
            {
                m_lastPosition.X = i;  // Restrict drawing direction (east)
                m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;
            m_arrowRightSide.X = e.Location.X;
            m_arrowRightSide.Y = m_drawStartPoint.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowRightSide.X--;
                m_arrowRightSide.Y++;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            m_shapeNumber++;
            m_arrowLeftSide.X = e.Location.X;
            m_arrowLeftSide.Y = m_drawStartPoint.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowLeftSide.X--;
                m_arrowLeftSide.Y--;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawSouthEastArrow() - draws an arrow pointing south east
         * 
         * SYNOPSIS
         *  private void drawSouthEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to south east direction as the user has the mouse down and dragging it.
         *  The location of endpoint of the arrow is dictated by the furthest point of either horizontal or
         *  vertical value towards the appropriate direction from the start point. This method simply draws the
         *  arrow to display to the user a real-time response. Saving does not occur here. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:31pm 3/22/2015
         */
        private void drawSouthEastArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            // Get the longest distance from the starting point and use that as the limit of diagonal line
            if ((e.Location.X - m_drawStartPoint.X) > (e.Location.Y - m_drawStartPoint.Y))
            {
                m_arrowFarPoint = e.Location.X - m_drawStartPoint.X;
            }
            else
            {
                m_arrowFarPoint = e.Location.Y - m_drawStartPoint.Y;
            }

            m_lastPosition.X = m_drawStartPoint.X + m_arrowFarPoint;
            m_lastPosition.Y = m_drawStartPoint.Y + m_arrowFarPoint;

            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X - 8;
            m_arrowRightSide.Y = m_lastPosition.Y;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X;
            m_arrowLeftSide.Y = m_lastPosition.Y - 8;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawSouthEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveSouthEastArrow() - saves an arrow drawn by the user pointing south east
         * 
         * SYNOPSIS
         *  private void saveSouthEastArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to south east direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:42pm 3/22/2015
         */
        private void saveSouthEastArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = 0; i < m_arrowFarPoint; i++)
            {
                m_lastPosition.X = m_drawStartPoint.X + i;
                m_lastPosition.Y = m_drawStartPoint.Y + i;
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;

            for (int i = 0; i < 8; i++)
            {
                m_arrowRightSide.X++;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_shapeNumber++;
            for (int i = 0; i < 8; i++)
            {
                m_arrowLeftSide.Y++;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveSouthEastArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawSouthArrow() - draws an arrow pointing south
         * 
         * SYNOPSIS
         *  private void drawSouthArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to south direction as the user has the mouse down and dragging it. The
         *  location of endpoint of the arrow is dictated by the current value of the Y coordinate in the
         *  appropriate direction away from the start point. This method simply draws the arrow to display to
         *  the user a real-time response. Saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:54pm 3/22/2015
         */
        private void drawSouthArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            m_lastPosition.X = m_drawStartPoint.X;  // Restrict horizontal movement
            m_lastPosition.Y = e.Location.Y;  // Restrict drawing direction (south)
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X - 5;
            m_arrowRightSide.Y = m_lastPosition.Y - 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X + 5;
            m_arrowLeftSide.Y = m_lastPosition.Y - 5;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawSouthArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveSouthArrow() - saves an arrow drawn by the user pointing south
         * 
         * SYNOPSIS
         *  private void saveSouthArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to south direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:00pm 3/22/2015
         */
        private void saveSouthArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = m_drawStartPoint.Y; i <= e.Location.Y; i++)
            {
                m_lastPosition.X = m_drawStartPoint.X;  // Restrict horizontal movement
                m_lastPosition.Y = i;  // Restrict drawing direction (south)
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;
            m_arrowRightSide.X = m_drawStartPoint.X;
            m_arrowRightSide.Y = e.Location.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowRightSide.X++;
                m_arrowRightSide.Y--;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            m_shapeNumber++;
            m_arrowLeftSide.X = m_drawStartPoint.X;
            m_arrowLeftSide.Y = e.Location.Y;
            for (int i = 0; i < 5; i++)
            {
                m_arrowLeftSide.X--;
                m_arrowLeftSide.Y--;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveSouthArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawSouthWestArrow() - draws an arrow pointing south west
         * 
         * SYNOPSIS
         *  private void drawSouthWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an arrow restricted to south west direction as the user has the mouse down and dragging it.
         *  The location of endpoint of the arrow is dictated by the furthest point of either horizontal or
         *  vertical value towards the appropriate direction from the start point. This method simply draws the
         *  arrow to display to the user a real-time response. Saving does not occur here. 
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:08pm 3/22/2015
         */
        private void drawSouthWestArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            // Get the longest distance from the starting point and use that as the limit of diagonal line
            if ((m_drawStartPoint.X - e.Location.X) > (m_drawStartPoint.Y - e.Location.Y))
            {
                m_arrowFarPoint = m_drawStartPoint.X - e.Location.X;
            }
            else
            {
                m_arrowFarPoint = m_drawStartPoint.Y - e.Location.Y;
            }

            m_lastPosition.X = m_drawStartPoint.X - m_arrowFarPoint;
            m_lastPosition.Y = m_drawStartPoint.Y + m_arrowFarPoint;

            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X;
            m_arrowRightSide.Y = m_lastPosition.Y - 8;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X + 8;
            m_arrowLeftSide.Y = m_lastPosition.Y;
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawSouthWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  saveSouthWestArrow() - saves an arrow drawn by the user pointing south west
         * 
         * SYNOPSIS
         *  private void saveSouthWestArrow(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves an arrow drawn by the user which is restricted to south west direction. This method saves
         *  all the points that form the arrow and not just the beginning and end. Such functionality
         *  is necessary in order to accomodate the erase functionality, where a user can erase only the
         *  desired points of the arrow. This method is called by _MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:14pm 3/22/2015
         */
        private void saveSouthWestArrow(MouseEventArgs e)
        {
            // Add points to save and redraw the line part of the arrow
            m_shapeNumber++;
            for (int i = 0; i < m_arrowFarPoint; i++)
            {
                m_lastPosition.X = m_drawStartPoint.X - i;
                m_lastPosition.Y = m_drawStartPoint.Y + i;
                m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Add points to save and redraw the arrowhead of the arrow
            m_shapeNumber++;

            for (int i = 0; i < 8; i++)
            {
                m_arrowRightSide.Y++;
                m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_shapeNumber++;
            for (int i = 0; i < 8; i++)
            {
                m_arrowLeftSide.X--;
                m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            m_isDrawing = false;
            transparentPanel.Refresh();
        } /* private void saveSouthWestArrow(MouseEventArgs e) */

        /*
         * NAME
         *  drawRectangle() - draws a rectangle in any direction
         * 
         * SYNOPSIS
         *  private void drawRectangle(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a rectangle in any direction with user desired size. This method is optimized and used by the
         *  _MouseMove event handler when the user has a mouse button down and is dragging it to desired size.
         *  Drawing is very dynamic in order to respond to user input, so saving does not occur here. Rectangle
         *  constructor uses complex Math methods in order to allow the user to draw the rectangle in any direction.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:19pm 3/22/2015
         */
        private void drawRectangle(MouseEventArgs e)
        {
            Int32 rectWidth = e.Location.X - m_drawStartPoint.X;
            Int32 rectHeight = e.Location.Y - m_drawStartPoint.Y;

            Rectangle currentRect = new Rectangle(Math.Min(e.X, m_drawStartPoint.X), Math.Min(e.Y, m_drawStartPoint.Y), 
                Math.Abs(e.X - m_drawStartPoint.X), Math.Abs(e.Y - m_drawStartPoint.Y));
            
            m_transparentPanelGraphics.DrawRectangle(m_transparentPanelPen, currentRect);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawRectangle(MouseEventArgs e) */

        /*
         * NAME
         *  saveRectangle() - saves a rectangle drawn by the user
         * 
         * SYNOPSIS
         *  private void saveRectangle(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Saves a rectangle drawn by the user. This method saves all the points that form the rectangle
         *  not just origin + size. Such functionality is necessary in order to accomodate the erase
         *  functionality, where a user can erase only the desired points of the rectangle. rectOrigin
         *  variable gets coordinates that are the most upper left part of the rectangle. This method is 
         *  called by _MouseUp event handler. Rectangle is saved as one shape with many points.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:38pm 3/23/2015
         */
        private void saveRectangle(MouseEventArgs e)
        {
            Point rectOrigin = new Point(Math.Min(m_drawStartPoint.X, e.Location.X), Math.Min(m_drawStartPoint.Y, e.Location.Y));
            Int32 rectLength = Math.Abs(m_drawStartPoint.X - e.Location.X);
            Int32 rectWidth = Math.Abs(m_drawStartPoint.Y - e.Location.Y);
            m_shapeNumber++;
            for (int i = 0; i < rectLength; i++)
            {
                rectOrigin.X++;
                m_shapesStorage.AddShape(rectOrigin, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            for (int i = 0; i < rectWidth; i++)
            {
                rectOrigin.Y++;
                m_shapesStorage.AddShape(rectOrigin, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            for (int i = 0; i < rectLength; i++)
            {
                rectOrigin.X--;
                m_shapesStorage.AddShape(rectOrigin, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            for (int i = 0; i < rectWidth; i++)
            {
                rectOrigin.Y--;
                m_shapesStorage.AddShape(rectOrigin, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            
            transparentPanel.Refresh();
        } /* private void saveRectangle(MouseEventArgs e) */

        /*
         * NAME
         *  drawEllipse() - draws an ellipse
         * 
         * SYNOPSIS
         *  private void drawEllipse(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws an ellipse in any direction with user desired size. This method is optimized and used by the
         *  _MouseMove event handler when the user has a mouse button down and is dragging it to desired size.
         *  Drawing is very dynamic in order to respond to user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:27pm 3/23/2015
         */
        private void drawEllipse(MouseEventArgs e)
        {
            Int32 rectWidth = e.Location.X - m_drawStartPoint.X;
            Int32 rectHeight = e.Location.Y - m_drawStartPoint.Y;

            Rectangle currentEllipse = new Rectangle(m_drawStartPoint.X, m_drawStartPoint.Y, rectWidth, rectHeight);
            m_transparentPanelGraphics.DrawEllipse(m_transparentPanelPen, currentEllipse);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawEllipse(MouseEventArgs e) */

        /*
         * NAME
         *  saveEllipse() - saves points generated by an ellipse shape
         *  
         * SYNOPSIS
         *  private void saveEllipse(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         * 
         * DESCRIPTION
         *  Saves points generated by an ellipse shape. First calculate the absolute values of width and height
         *  of the ellipse based on m_drawStartPoint and e.Location (current point) variables. Second, use a
         *  GraphicsPath object to plot a set of points on the panel based on generated origin and size.
         *  Third, use GraphicsPathIterator object to extract a set of points from GraphicsPant object, this
         *  technique seems to be best for extracting the most points. Finally, save the generated points in the
         *  m_shapesStorage object. Ellipse is saved as one shape with many points The reason for saving a set of 
         *  points is to accomodate erase functionality.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:00am 3/24/2015
         */
        private void saveEllipse(MouseEventArgs e)
        {
            Point origin = new Point(0, 0);
            origin.X = Math.Min(m_drawStartPoint.X, e.Location.X);
            origin.Y = Math.Min(m_drawStartPoint.Y, e.Location.Y);
            Int32 width = Math.Abs(m_drawStartPoint.X - e.Location.X);
            Int32 height = Math.Abs(m_drawStartPoint.Y - e.Location.Y);

            GraphicsPath ellipsePath = new GraphicsPath();
            ellipsePath.AddEllipse(origin.X, origin.Y, width, height);
            m_transparentPanelGraphics.DrawPath(m_transparentPanelPen, ellipsePath);
            PointF[] ellipsePoints = ellipsePath.PathPoints;
            ellipsePath.Flatten();
            Int32 pointCount = ellipsePoints.Length;

            GraphicsPathIterator iterator = new GraphicsPathIterator(ellipsePath);
            PointF[] points = new PointF[iterator.Count];
            byte[] types = new byte[iterator.Count];
            int numPoints = iterator.Enumerate(ref points, ref types);

            m_shapeNumber++;
            for (int i = 0; i < points.Length; i++)
            {
                Point newPoint = new Point(Convert.ToInt32(points[i].X), Convert.ToInt32(points[i].Y));
                m_shapesStorage.AddShape(newPoint, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
        } /* private void saveEllipse(MouseEventArgs e) */

        /*
         * NAME
         *  drawSolidLine() - draws a solid line in any direction
         * 
         * SYNOPSIS
         *  private void drawSolidLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a solid line in any direction with user desired length. This method is optimized and used by
         *  the _MouseMove event handler when the user has a mouse button down and is dragging the line to desired
         *  length. Drawing is very dynamic in order to respond to user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:39pm 3/22/2015
         */
        private void drawSolidLine(MouseEventArgs e)
        {
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, e.Location);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawSolidLine(MouseEventArgs e) */

        /*
         * NAME
         *  saveSolidLine() - saves points generated by a solid line
         *  
         * SYNOPSIS
         *  private void saveSolidLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         * 
         * DESCRIPTION
         *  Saves points generated by a solid line. This methods uses a 'helper' method GetPointsOnLine to generate
         *  a set of points based on start and end points and saves these points in a m_shapesStorage object. Solid
         *  line is saved as one shape with many points. The reason for saving a set of points is to accomodate 
         *  erase functionality.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:05am 3/24/2015
         */
        private void saveSolidLine(MouseEventArgs e)
        {
            m_shapeNumber++;
            IEnumerable<Point> points = GetPointsOnLine(m_drawStartPoint.X, m_drawStartPoint.Y, e.Location.X, e.Location.Y);
            List<Point> pointList = points.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                m_shapesStorage.AddShape(pointList[i], m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }
            transparentPanel.Refresh();
        } /* private void saveSolidLine(MouseEventArgs e) */

        /*
         * NAME
         *  drawDashedLine() - draws a dashed line in any direction
         * 
         * SYNOPSIS
         *  private void drawDashedLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a dashed line in any direction with user desired length. This method is optimized and used by
         *  the _MouseMove event handler when the user has a mouse button down and is dragging the line to desired
         *  length. Drawing is very dynamic in order to respond to user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:40am 3/30/2015
         */
        private void drawDashedLine(MouseEventArgs e)
        {
            float[] dashValues = { 5, 5 };
            Pen dashPen = new Pen(m_currentDrawColor, m_currentPenWidth);
            dashPen.DashPattern = dashValues;
            m_transparentPanelGraphics.DrawLine(dashPen, m_drawStartPoint, e.Location);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawDashedLine(MouseEventArgs e) */

        /*
         * NAME
         *  saveDashedLine() - saves points generated by a dashed line
         *  
         * SYNOPSIS
         *  private void saveDashedLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         * 
         * DESCRIPTION
         *  Saves points generated by a dashed line. This methods uses a 'helper' method GetPointsOnLine to
         *  generate a set of points based on start and end points. A for-loop iterates over the newly generated set
         *  of points to selectively save only those points that form a dashed line. The newly selected points are
         *  saved in m_shapesStorage object. Dashed line is saved as one shape with many points. The reason for saving
         *  a set of points is to accomodate erase functionality.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:58am 3/30/2015
         */
        private void saveDashedLine(MouseEventArgs e)
        {
            IEnumerable<Point> points = GetPointsOnLine(m_drawStartPoint.X, m_drawStartPoint.Y, e.Location.X, e.Location.Y);
            List<Point> pointList = points.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                if (i % 5 == 0)
                {
                    m_canDash = false;
                }
                if (i % 10 == 0)
                {
                    m_canDash = true;
                    m_shapeNumber++;
                }
                if (m_canDash)
                {
                    m_shapesStorage.AddShape(pointList[i], m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                }
            }
            transparentPanel.Refresh();
        } /* private void saveDashedLine(MouseEventArgs e) */

        /*
         * NAME
         *  drawDottedLine() - draws a dotted line in any direction
         * 
         * SYNOPSIS
         *  private void drawDashedLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a dotted line in any direction with user desired length. This method is optimized and used by
         *  the _MouseMove event handler when the user has a mouse button down and is dragging the line to desired
         *  length. Drawing is very dynamic in order to respond to user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:09pm 3/30/2015
         */
        private void drawDottedLine(MouseEventArgs e)
        {
            float[] dashValues = { 2, 2 };
            Pen dashPen = new Pen(m_currentDrawColor, m_currentPenWidth);
            dashPen.DashPattern = dashValues;
            m_transparentPanelGraphics.DrawLine(dashPen, m_drawStartPoint, e.Location);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        } /* private void drawDottedLine(MouseEventArgs e) */

        /*
         * NAME
         *  saveDottedLine() - saves points generated by a dashed line
         *  
         * SYNOPSIS
         *  private void saveDottedLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         * 
         * DESCRIPTION
         *  Saves points generated by a dotted line. This methods uses a 'helper' method GetPointsOnLine to
         *  generate a set of points based on start and end points. A for-loop iterates over the newly generated set
         *  of points to selectively save only those points that form a dotted line. The newly selected points are
         *  saved in m_shapesStorage object. Dotted line is saved as one shape with many points. The reason for saving
         *  a set of points is to accomodate erase functionality.
         * 
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:13pm 3/30/2015
         */
        private void saveDottedLine(MouseEventArgs e)
        {
            IEnumerable<Point> points = GetPointsOnLine(m_drawStartPoint.X, m_drawStartPoint.Y, e.Location.X, e.Location.Y);
            List<Point> pointList = points.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    m_canDash = false;
                }
                if (i % 4 == 0)
                {
                    m_canDash = true;
                    m_shapeNumber++;
                }
                if (m_canDash)
                {
                    m_shapesStorage.AddShape(pointList[i], m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                }
            }
            transparentPanel.Refresh();
        } /* private void saveDottedLine(MouseEventArgs e) */

        /*
         * NAME
         *  GetPointsOnLine() - calculates and returns a set of points between two points
         *  
         * SYNOPSIS
         *  public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1);
         *      x0      -> x-coordinate of starting point
         *      y0      -> y-coordinate of starting point
         *      x1      -> x-coordinate of end point
         *      y1      -> y-coordinate of end point
         * 
         * DESCRIPTION
         *  This method utilizes the famous Bresenham's line algorithms to calculate and return a set of points
         *  between a start point and end point. This method is used by several other methods that save solid,
         *  dashed, and dotted lines by using a set of points. The need to generate individual points is to 
         *  accomodate erase functionality. This method was taken from an author who customized this algorithm
         *  specifically for C#, the author's credits are documented.
         *  
         * RETURNS
         *  IEnumerable<Point> type, which is a list of Point structures
         *  
         * AUTHOR
         *  Geoff Samuel, May 23, 2011
         *  http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
         *  
         * DATE
         *  10:00am 3/24/2015
         */
        public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        } /* public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1) */

        /*
         * NAME
         *  createVertTextObject() - creates an instance of VerticalText and adds it to the Panel
         *  
         * SYNOPSIS
         *  private void createVertTextObject(MouseEventArgs e);
         *      e       -> is passed down to the constructor to establish the current location
         * 
         * DESCRIPTION
         *  This method creates and saves an instance of VerticalText class. When created, it is added to
         *  a List<> of VerticalText objects which gets used to redraw them in _Paint event. Also, each instance
         *  of VerticalText comes with buttons which are added to this panel. Some references are passed in
         *  to assist certain functions (options, delete) to be executed within the object. Specifically, these
         *  functions will be triggered using options and delete buttons in the VerticalText object. This method
         *  only gets called from transparentPanel_MouseUp() event.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:43pm 5/18/2015
         */
        private void createVertTextObject(MouseEventArgs e)
        {
            VerticalText nextText = new VerticalText(e);
            nextText.OwnerTranspPanel = transparentPanel;
            nextText.OwnerRichTextBox = richTextBox;
            nextText.OwnerBackPanel = backPanel;
            nextText.OwnerVerticalTextList = m_verticalTextList;

            m_verticalTextList.Add(nextText);
            transparentPanel.Controls.Add(nextText.MoveButton);
            transparentPanel.Controls.Add(nextText.OptionsButton);
            transparentPanel.Controls.Add(nextText.DeleteButton);
            transparentPanel.Controls.Add(nextText.RotateButton);
        } /* private void createVertTextObject(MouseEventArgs e) */

        #endregion
    }
}