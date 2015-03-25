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
            if (m_currentSelectedControl == e_SelectedControl.ELLIPSE)
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

            if ( m_isDrawing && (m_currentSelectedControl == e_SelectedControl.ELLIPSE) )
            {
                drawEllipse(e);
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
                backPanel.Invalidate();
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
            if (m_currentSelectedControl == e_SelectedControl.RECTANGLE)
            {
                if (m_isDrawing)
                {
                    saveRectangle(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.ELLIPSE)
            {
                if (m_isDrawing)
                {
                    saveEllipse(e);
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                if (m_isDrawing)
                {
                    saveSolidLine(e);
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
         *  drawRectangle() - draws a rectangle in any direction
         * 
         * SYNOPSIS
         *  private void drawRectangle(MouseEventArgs e);
         *      e       -> used to get the current location of the cursor
         *      
         * DESCRIPTION
         *  Draws a rectangle in any direction with user desired size. This method is optimized and used by the
         *  MouseMove event handler when the user has a mouse button down and is dragging it to desired length.
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
         *  3:19pm 3/22/15
         */
        private void drawRectangle(MouseEventArgs e)
        {
            Int32 rectWidth = e.Location.X - m_drawStartPoint.X;
            Int32 rectHeight = e.Location.Y - m_drawStartPoint.Y;

            Rectangle currentRect = new Rectangle(Math.Min(e.X, m_drawStartPoint.X), Math.Min(e.Y, m_drawStartPoint.Y), Math.Abs(e.X - m_drawStartPoint.X), Math.Abs(e.Y - m_drawStartPoint.Y));
            
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
         *  called by MouseUp event handler, rectangle is saved as one shape.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  7:38pm 3/23/15
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
         *  MouseMove event handler when the user has a mouse button down and is dragging it to desired length.
         *  Drawing is very dynamic in order to respond to user input, so saving does not occur here.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:27pm 3/23/15
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
         * 9:00am 3/24/15
         */
        private void saveEllipse(MouseEventArgs e)
        {
            mslog("Ellipse");
            mslog("start point = " + m_drawStartPoint);
            mslog("end point = " + e.Location + "\r\n");

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
            mslog("point count = " + pointCount);

            GraphicsPathIterator iterator = new GraphicsPathIterator(ellipsePath);
            int pathCount = iterator.Count;
            int subPathCount = iterator.SubpathCount;
            mslog("pathCount = " + pathCount);
            mslog("subPathCount = " + subPathCount);

            PointF[] points = new PointF[iterator.Count];
            byte[] types = new byte[iterator.Count];
            int numPoints = iterator.Enumerate(ref points, ref types);

            m_shapeNumber++;
            for (int i = 0; i < points.Length; i++)
            {
                //mslog("it point = " + points[i]);
                Point newPoint = new Point(Convert.ToInt32(points[i].X), Convert.ToInt32(points[i].Y));
                //mslog("now point = " + newPoint);
                m_shapesStorage.AddShape(newPoint, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

                //m_shapeNumber++;
                for (int i = 0; i < ellipsePoints.Length; i++)
                {
                    //mslog("point = " + ellipsePoints[i]);
                    //Point nextPoint = Point.Round(ellipsePoints[i]);
                    //Point nextPoint = new Point(0, 0);
                    //nextPoint.X = (int)ellipsePoints[i].X;
                    //nextPoint.Y = (int)ellipsePoints[i].Y;
                    //m_shapesStorage.AddShape(nextPoint, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
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
        } /* private void drawSolidLine(MouseEventArgs e) */

        /*
         * 9:05am 3/24/15
         */
        private void saveSolidLine(MouseEventArgs e)
        {
            // Logging results
            Int32 coordWidth = Math.Abs(m_drawStartPoint.X - e.Location.X);
            Int32 coordHeight = Math.Abs(m_drawStartPoint.Y - e.Location.Y);
            double hypotenuse = Math.Sqrt(coordWidth * coordWidth + coordHeight * coordHeight);
            mslog("Solid line");
            mslog("width = " + coordWidth);
            mslog("height = " + coordHeight);
            mslog("hypotenuse = " + hypotenuse);
            mslog("m = " + coordHeight + "/" + coordWidth);
            mslog("start point = " + m_drawStartPoint);
            mslog("end point = " + e.Location + "\r\n");

            m_shapeNumber++;
            IEnumerable<Point> points = GetPointsOnLine(m_drawStartPoint.X, m_drawStartPoint.Y, e.Location.X, e.Location.Y);
            List<Point> pointList = points.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                m_shapesStorage.AddShape(pointList[i], m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
            }

            // Refreshing
            transparentPanel.Refresh();
            //transparentPanel.Invalidate();
            //richTextBox.Invalidate();
            //backPanel.Invalidate();
        } /* private void saveSolidLine(MouseEventArgs e) */
        
        #endregion
        /* http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html // 3/24/15 */
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
        }
    }
}