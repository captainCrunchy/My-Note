using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

/*
 * This is one of several 'partial' classes of the MainForm class. It is responsible
 * for performing a specific task. Below are the related files of the MainForm class:
 * 
 * mainForm.cs - This file is the starting point of the MainForm class. It contains the
 *               constructor and is responsible for coordinating interactions between
 *               other parts of the class and the application
 *               
 * formMenuStrip.cs - This file handles events that are triggered by elements
 *                    of the menu strip in the form. (Ex: File, Edit, ... Help)
 *                    
 * formToolbar.cs - This file is responsible for controls in the toolbar and
 *                  their events in the main form. (Ex: Font, Text, Color, Line...)
 * 
 * formTextbox.cs - (You are here) This file is responsible for appearance and events
 *                  of the richTextBox and its layers
 */
namespace My_Note
{
    public partial class MainForm : Form
    {
        /*
         *  Many variables were created and initialized here for reusability and to avoid repetition
         *  in order to increse performance. Some variables are initialized in the MainForm() constructor.
         */
        private ShapeContainer m_shapesStorage = new ShapeContainer();      // Storage of all the drawing data
        private bool m_isDrawing = false;                                   // Is the mouse currently down (transparentPanel_Mouse...)
        private bool m_isErasing = false;                                   // Is the mouse currently down (transparentPanel_Mouse...)
        private Point m_lastPosition = new Point(0, 0);                     // Last cursor position, used to cut down on repetative data
        private Color m_currentDrawColor = Color.Black;                     // Current drawing color (updated immediately on change)
        private float m_currentPenWidth = 1;                                // Current pen width (does not change)
        private int m_shapeNumber = 0;                                      // Record the shape numbers so they can be drawn separately
        private Point m_drawStartPoint = new Point(0, 0);                   // Start point of arrow or line
        private Point m_drawEndPoint = new Point(0, 0);                     // End point of arrow or line
        private Point m_arrowLeftSide = new Point(0, 0);                    // Left side of the arrow
        private Point m_arrowRightSide = new Point(0, 0);                   // Right side of the arrow
        private Int32 m_arrowFarPoint = 0;                                  // Used in the diagonal arrows
        private Graphics m_transparentPanelGraphics;                        // Used to reduce repetitive data creation
        private Pen m_transparentPanelPen;                                  // Used to reduce repetitive data creation

        // This region contains all methods and event handlers
        // of the richTextBox, which is the main text box
        #region richTextBoxMethods

        /*
         * NAME
         *  richTextBox_TextChanged() - event handler for richTextBox
         *  
         * SYNOPSIS
         *  private void richTextBox_TextChanged(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         * 
         * DESCRIPTION
         *  Calls invalidate on richTextBox and transparentPanel during text typing
         *  in order to update user drawings presented on transparentPanel
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:44am 3/11/15
         */
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox.Invalidate();
            transparentPanel.Invalidate();
        }
        #endregion

        // This region contains event handler methods of the
        // transparentPanel, which is the main drawing panel
        #region transparentPanelEventMethods

        /*
         * NAME
         *  transparentPanel_Click() - event handler for transparentPanel
         * 
         * SYNOPSIS
         *  private void transparentPanel_Click(object sender, System.EventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture the location of the cursor upon click
         *      
         * DESCRIPTION
         *  When textControl is selected, this method will pass down the mouse location coordinates
         *  through the transparentPanel and to the richTextBox so that the text cursor is positioned
         *  in the right place. Additional calculations are performed because the origin of the
         *  richTextBox is away from the origins of its front and back layers.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:47pm 3/7/15
         */
        private void transparentPanel_Click(object sender, System.EventArgs e)
        {
            // If currently in text editing mode
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                int charCount = richTextBox.TextLength;
                var mouseEventArgs = e as MouseEventArgs;
                Point newPoint = new Point(mouseEventArgs.X - 40, mouseEventArgs.Y - 35);
                int charIndex = richTextBox.GetCharIndexFromPosition(newPoint);
                richTextBox.SelectionStart = charIndex;
                richTextBox.Select();
            }
        } /*private void transparentPanel_Click(object sender, System.EventArgs e)*/

        /*
         * NAME
         *  transparentPanel_MouseDown() - prepares drawing controls to be used 
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseDown(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  Prepares drawing controls to be used by setting/updating member variables
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:01 am 3/10/15
         */
        private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
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
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.NWARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_arrowFarPoint = 0;
            }
            if (m_currentSelectedControl == e_SelectedControl.NARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.NEARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.EARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.SEARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.SARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.SWARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.RECTANGLE)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
            if (m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
            }
        } /* private void transparentPanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  transparentPanel_MouseMove() - performs drawing operations for each of the selected shapes
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseMove(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture current location of cursor and pass to helper method
         *      
         * DESCRIPTION
         *  Based on the tool selected start drawing, saving, or erasing. Some of the shapes are drawn
         *  and saved in this method (pencil, eraser). Other shapes (lines, arrows, ovals, rectangles) are
         *  only drawn here to display to the user a dynamic response. They get saved on MouseUp event.
         *  Eraser only removes anything that is not text.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:01am 3/10/15
         */
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.PENCIL) )
            {
                drawWithPencil(e);
            }
            if ( m_isErasing && (m_currentSelectedControl == e_SelectedControl.ERASER) )
            {
                startErasing(e);
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.WARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X) )
                {
                    drawWestArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NWARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    drawNorthWestArrow(e);
                }
            }
            if (m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    drawNorthArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.NEARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    drawNorthEastArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.EARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X) )
                {
                    drawEastArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SEARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    drawSouthEastArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    drawSouthArrow(e);
                }
            }
            if ( m_isDrawing & (m_currentSelectedControl == e_SelectedControl.SWARROW) )
            {
                if ( (m_lastPosition != e.Location) && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    drawSouthWestArrow(e);
                }
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.RECTANGLE) )
            {
                drawRectangle(e);
            }
            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.SOLID) )
            {
                drawSolidLine(e);
            }
        } /* private void transparentPanel_MouseMove(object sender, MouseEventArgs e) */

        /*
         * NAME
         *  transparentPanel_MouseUp() - updates values and saves data
         * 
         * SYNOPSIS
         *  private void transparentPanel_MouseUp(object sender, MouseEventArgs e);
         *      sender  -> does nothing
         *      e       -> used to capture current location of cursor and pass to helper methods
         *      
         * DESCRIPTION
         *  Updates values, saves data, redraws transparentPanel and richTextBox. Pencil and
         *  eraser actions are already saved up to this point. Lines, arrows, and other shapes
         *  use special methods for saving, which are triggered by this method. 'if' statements
         *  test for several values to ensure/restrict the direction of the shape being drawn
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:49am 3/10/15
         */
        private void transparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
            }
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                if ( m_isDrawing && (e.Location.X < m_drawStartPoint.X) )
                {
                    saveWestArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.NWARROW)
            {
                if ( (m_isDrawing) && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    saveNorthWestArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.NARROW)
            {
                if ( m_isDrawing && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    saveNorthArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.NEARROW)
            {
                if ( m_isDrawing && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y < m_drawStartPoint.Y) )
                {
                    saveNorthEastArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.EARROW)
            {
                if ( m_isDrawing && (e.Location.X > m_drawStartPoint.X) )
                {
                    saveEastArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.SEARROW)
            {
                if ( m_isDrawing && (e.Location.X > m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    saveSouthEastArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.SARROW)
            {
                if ( m_isDrawing && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    saveSouthArrow(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.SWARROW)
            {
                if ( m_isDrawing && (e.Location.X < m_drawStartPoint.X) && (e.Location.Y > m_drawStartPoint.Y) )
                {
                    saveSouthWestArrow(e);
                }
            }
            if (m_isDrawing)
            {
                m_isDrawing = false;
            }
            else
            {
                m_isErasing = false;
            }
        } /* private void transparentPanel_MouseUp(object sender, MouseEventArgs e) */

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
         *  Redraws the tranparentPanel graphics. This is done by accessing the m_shapesStorage
         *  object of the ShapeContainer class, which holds a list of shapes. Each shape in this
         *  list is represented as a set of points with a common shape number. The 'for-loop' below
         *  accesses two points/shapes from the list at a time and if they share the same shape
         *  number, then they are connected to form a line. If the shape numbers are different,
         *  then a new shape begins to form. This method gets triggered by the OnPaint() method
         *  of the TransparentPanel.cs subclass by the command: base.OnPaint(e);
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:52am 3/10/15
         */
        private void transparentPanel_Paint(object sender, PaintEventArgs e)
        {
            // Apply a smoothing mode to smooth out the line.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
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
        } /* private void transparentPanel_Paint(object sender, PaintEventArgs e) */

        #endregion

        // This region contains methods used in transparentPanel,
        // also contains helper methods for event handler methods.
        #region transparentPanelMethods

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
         *  2:44pm 3/22/15
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
         *  startErasing() - draws shapes using a pencil
         * 
         * SYNOPSIS
         *  private void startErasing(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Erases shapes from the screen by first removing them from the m_shapesStorage container
         *  and redraws the panels to give the effect of erasing. Eraser removes anything that is
         *  not text and saving occurs immediately.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:49pm 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to west direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:20am 3/20/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:37 am 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to north west direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:38am 3/20/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:52am 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to north direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:51pm 3/21/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:07am 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to north east direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:19pm 3/22/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  12:44pm 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to east direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:01pm 3/22/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:12pm 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to south east direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:31pm 3/22/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:42pm 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to south direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  1:54pm 3/22/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:00pm 3/22/15
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
         *  Draws an arrow with user desired length which is restricted to south west direction. This method
         *  is optimized and used by the MouseMove event handler when the user has a mouse button down
         *  and is dragging the arrow to desired length. Drawing is very dynamic in order to respond to
         *  user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:08pm 3/22/15
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
         *  desired points of the arrow. This method is called by MouseUp event handler and stores each
         *  arrow as three shapes.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:14pm 3/22/15
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
         *  drawSolidLine() - draws a solid line in any direction
         * 
         * SYNOPSIS
         *  private void drawSolidLine(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a solid line in any direction with user desired length. This method is optimized and used by
         *  the MouseMove event handler when the user has a mouse button down and is dragging the line to desired
         *  length. Drawing is very dynamic in order to respond to user input.so saving does not occur here
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  3:19pm 3/22/15
         */
        private void drawRectangle(MouseEventArgs e)
        {
            Int32 rectWidth = m_drawStartPoint.X - e.Location.X;
            Int32 rectHeight = m_drawStartPoint.Y - e.Location.Y;
            Rectangle currentRect = new Rectangle(m_drawStartPoint.X, m_drawStartPoint.Y, rectWidth, rectHeight);
            m_transparentPanelGraphics.DrawRectangle(m_transparentPanelPen, currentRect);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        }

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
         *  the MouseMove event handler when the user has a mouse button down and is dragging the line to desired
         *  length. Drawing is very dynamic in order to respond to user input.so saving does not occur here
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  2:39pm 3/22/15
         */
        private void drawSolidLine(MouseEventArgs e)
        {
            m_transparentPanelGraphics.DrawLine(m_transparentPanelPen, m_drawStartPoint, e.Location);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate();
        }

        #endregion
    }
}
