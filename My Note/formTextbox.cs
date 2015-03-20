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
        private ShapeContainer m_shapesStorage = new ShapeContainer();      // Storage of all the drawing data
        private bool m_isDrawing = false;                                   // Is the mouse currently down, used in MouseMove
        private bool m_isErasing = false;                                   // Is the mouse currently down, used in MouseMove
        private Point m_lastPosition = new Point(0, 0);                     // Last Position, used to cut down on repetative data
        private Color m_currentDrawColor = Color.Black;                     // Current drawing color
        private float m_currentPenWidth = 1;                                // Current pen width
        private int m_shapeNumber = 0;                                      // Record the shape numbers so they can be drawn separately

        private Point m_drawStartPoint = new Point(0, 0);                   // Start point of arrow or line
        private Point m_drawEndPoint = new Point(0, 0);                     // End point of arrow or line
        private Point m_arrowLeftSide = new Point(0, 0);                    // Left side of the arrow (dynamic), used in MouseMove
        private Point m_arrowRightSide = new Point(0, 0);                   // Right side of the arrow (dynamic), used in MouseMove
        private Int32 m_arrowFarPoint = 0;                                  // Used in diagonal arrow, updated in MouseMove and read in MouseUp
        private Graphics transparentPanelGraphics;                          // Used to reduce repetitive data creation (init in Constuctor)
        private Pen transparentPanelPen;                                    // Used to reduce repetitive data creation (init in Constuctor)

        // This region contains all methods and event handlers
        // of the richTextBox, which is the main text box
        #region richTextBoxMethods
        /*
         * 3/11/15 8:44am
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

        /*  When textControl is selected, this method will pass down the click coordinates
         *  from transparent panel on top to the richTextBox below so that the cursor is positioned
         *  accordingly. Coordinate modifications were created because transparentPanel and
         *  richTextBox do not have the same origin. 
         *  Murat Zazi 12:47pm 3/7/15
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
         *  Prepare to draw shapes by initializing or resetting values of member
         *  variable that will be used in MouseMove event handler
         *  
         *  Murat Zazi 3/10/15 9:01am
         */
        private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Prepare to draw using pencil
            if (m_currentSelectedControl == e_SelectedControl.PENCIL) // DONE
            {
                m_isDrawing = true;
                m_shapeNumber++;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to erase anything that is not text
            if (m_currentSelectedControl == e_SelectedControl.ERASER) // DONE
            {
                m_isErasing = true;
            }
            // Prepare to draw arrow, west direction only
            if (m_currentSelectedControl == e_SelectedControl.WARROW) // DONE
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.NWARROW) // DONE
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
                m_arrowFarPoint = 0;
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.NARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.NEARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.EARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.SEARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.SARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw arrow, north west direction only
            if (m_currentSelectedControl == e_SelectedControl.SWARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw a solid line
            if (m_currentSelectedControl == e_SelectedControl.SOLID) // NOT SAVING
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                //m_lastPosition = new Point(0, 0);
            }
        } /* private void transparentPanel_MouseDown(object sender, MouseEventArgs e) */

        /*
         *  Based on the tool selected start drawing, saving, or erasing. Some of the shapes are drawn
         *  and saved in this method. Other shapes, like lines, arrow, ovals, rectangles, are only drawn
         *  here to display to the user a dynamic response; they are saved on MouseUp event.
         *  
         *  Murat Zazi 3/10/15 9:01AM
         */
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                if (m_isDrawing)
                {
                    // Free hand draw any shape in any direction, DRAWING and SAVING occurs here
                    if (m_lastPosition != e.Location)
                    {
                        m_lastPosition = e.Location;
                        m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                    }
                    transparentPanel.Refresh();
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                if (m_isErasing)
                {
                    // Remove any point within a certain distance of the mouse, SAVING occurs here
                    m_shapesStorage.RemoveShape(e.Location, 10);

                    transparentPanel.Invalidate();
                    richTextBox.Invalidate();
                    backPanel.Invalidate();
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                if (m_isDrawing)
                { 
                    // Draw an arrow pointing West, in real time (as user has MouseDown and dragging)
                    // SAVING does NOT occur in the code below, it occurs on MouseUp event
                    if (m_lastPosition != e.Location && e.Location.X < m_drawStartPoint.X)
                    {
                        drawWestArrow(e);
                    }
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.NWARROW)
            {
                if (m_isDrawing)
                {
                    // Draw an arrow pointing North West, in real time (as user has MouseDown and dragging)
                    // SAVING does NOT occur in the code below, only on MouseUp event
                    if (m_lastPosition != e.Location &&
                        e.Location.X < m_drawStartPoint.X &&
                        e.Location.Y < m_drawStartPoint.Y)
                    {
                        drawNorthWestArrow(e);
                    }
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                if (m_isDrawing)
                {
                    m_lastPosition = e.Location;
                    Graphics g = this.transparentPanel.CreateGraphics();
                    Pen pen = new Pen(m_currentDrawColor);
                    g.DrawLine(pen, m_drawStartPoint, m_lastPosition);

                    pen.Dispose();
                    g.Dispose();

                    transparentPanel.Invalidate();
                    richTextBox.Invalidate();
                    backPanel.Invalidate(); 
                }
            }
        } /* private void transparentPanel_MouseMove(object sender, MouseEventArgs e) */

        /*
         *  Reset values, save values, redraw transparentPanel and richTexbox. Points for all shapes are saved
         *  one point at a time instead of having a start point and end point. This is because the eraser tool
         *  targets individual points on the panel and must be able to erase, for example, the middle of a line.
         *  
         *  Murat Zazi 3/10/15 9:49am
         */
        private void transparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                if (m_isDrawing)
                {
                    m_isDrawing = false;
                }
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                if (m_isErasing)
                {
                    m_isErasing = false;
                }
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
            }
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                //m_drawEndPoint = e.Location; // this needs to be called here
                m_drawEndPoint = m_lastPosition;
                // Draw and save all the points that form an arrow not just begining and end
                // this is so that it works in erase funcitonality
                if (m_isDrawing && (m_drawEndPoint.X < m_drawStartPoint.X))
                {
                    // Add points to draw and save the line part of the arrow
                    m_shapeNumber++;
                    for (int i = m_drawStartPoint.X; i >= m_drawEndPoint.X; i--)
                    {
                        m_lastPosition.X = i;
                        m_lastPosition.Y = m_drawStartPoint.Y;
                        m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, 
                            m_currentDrawColor, m_shapeNumber);
                    }

                    // Add points to draw and save the arrowhead of the arrow
                    m_shapeNumber++;
                    m_arrowRightSide.X = m_drawEndPoint.X;
                    m_arrowRightSide.Y = m_drawStartPoint.Y;
                    for (int i = 0; i < 5; i++)
                    {
                        m_arrowRightSide.X++;
                        m_arrowRightSide.Y--;
                        m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, 
                            m_currentDrawColor, m_shapeNumber);
                    }

                    m_shapeNumber++;
                    m_arrowLeftSide.X = m_drawEndPoint.X;
                    m_arrowLeftSide.Y = m_drawStartPoint.Y;
                    for (int i = 0; i < 5; i++)
                    {
                        m_arrowLeftSide.X++;
                        m_arrowLeftSide.Y++;
                        m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                    }

                    m_isDrawing = false;
                    transparentPanel.Refresh();
                }

                // Draw nothing since there were no valid points
                if (m_isDrawing)
                {
                    m_isDrawing = false;
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.NWARROW)
            {
                if (m_isDrawing)
                {
                    // Add points to draw and save the line part of the arrow
                    m_shapeNumber++;
                    for (int i = 0; i < m_arrowFarPoint; i++)
                    {
                        m_lastPosition.X = m_drawStartPoint.X - i;
                        m_lastPosition.Y = m_drawStartPoint.Y - i;
                        m_shapesStorage.AddShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                    }

                    // Add points to draw and save the arrowhead of the arrow
                    m_shapeNumber++;

                    for (int i = 0; i < 8; i++)
                    {
                        m_arrowRightSide.X--;
                        m_shapesStorage.AddShape(m_arrowRightSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                        //m_arrowRightSide.X--;
                    }

                    m_shapeNumber++;
                    for (int i = 0; i < 8; i++)
                    {
                        m_arrowLeftSide.Y--;
                        m_shapesStorage.AddShape(m_arrowLeftSide, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                        //m_arrowLeftSide.Y--;
                    }

                        m_isDrawing = false;
                    transparentPanel.Refresh();
                }
            }
            if( m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                if(m_isDrawing)
                {
                    m_isDrawing = false;
                }
            }
        } /* private void transparentPanel_MouseUp(object sender, MouseEventArgs e) */

        /*  
         *  Change the cursor style while it is hovering over the panel,
         *  based on the current selected tool.
         *  
         *  Murat Zazi 3/17/15 12:38pm
         */
        private void transparentPanel_MouseHover(object sender, EventArgs e)
        {
            mslog("selected control is " + m_currentSelectedControl);
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                transparentPanel.Cursor = Cursors.IBeam;
            }
            else if ( m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                transparentPanel.Cursor = Cursors.Hand;
            }
            else
            {
                transparentPanel.Cursor = Cursors.Cross;
            }
        } /* private void transparentPanel_MouseHover(object sender, EventArgs e) */

        /*
         *  Paint and update the panel graphics. This method is triggered from
         *  the OnPaint() method in the transparentPanel class implementation.
         *  
         *  Murat Zazi 3/10/15 9:52am
         */
        private void transparentPanel_Paint(object sender, PaintEventArgs e)
        {
            //Apply a smoothing mode to smooth out the line.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //DRAW THE LINES
            for (int i = 0; i < m_shapesStorage.NumberOfShapes() - 1; i++)
            {
                Shape T = m_shapesStorage.GetShape(i);
                Shape T1 = m_shapesStorage.GetShape(i + 1);
                //make sure shape the two ajoining shape numbers are part of the same shape
                if (T.ShapeNumber == T1.ShapeNumber)
                {
                    //create a new pen with its width and colour
                    Pen p = new Pen(T.LineColor, T.LineWidth);
                    e.Graphics.DrawLine(p, T.PointLocation, T1.PointLocation);
                    p.Dispose();
                }
            }
        } /* private void transparentPanel_Paint(object sender, PaintEventArgs e) */

        #endregion

        // This region contains methods used in transparentPanel,
        // also contains helper methods for event handler methods.
        #region transparentPanelMethods

        /*
         *  This method is called from the MouseMove event handler. It takes
         *  an argument of type MouseEventArgs to get the current position.
         *  Murat Zazi 3/20/15 9:20am
         */
        private void drawWestArrow(MouseEventArgs e)
        {
            // Draw the line part of the arrow
            m_lastPosition.X = e.Location.X;  // Restrict drawing direction (West)
            m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
            transparentPanelGraphics.DrawLine(transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = e.Location.X + 5;
            m_arrowRightSide.Y = m_drawStartPoint.Y - 5;
            transparentPanelGraphics.DrawLine(transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_arrowRightSide.X;
            m_arrowLeftSide.Y = m_drawStartPoint.Y + 5;
            transparentPanelGraphics.DrawLine(transparentPanelPen, m_lastPosition, m_arrowLeftSide);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate(); 
        } /* private void drawWestArrow(MouseEventArgs e) */

        /*  This method is called from the MouseMove event handler. It takes
         *  an argument of type MouseEventArgs to get the current position.
         *  Murat Zazi 3/20/15 9:38am
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

            transparentPanelGraphics.DrawLine(transparentPanelPen, m_drawStartPoint, m_lastPosition);

            // Draw the arrowhead part of the arrow
            m_arrowRightSide.X = m_lastPosition.X + 8;
            m_arrowRightSide.Y = m_lastPosition.Y;
            transparentPanelGraphics.DrawLine(transparentPanelPen, m_arrowRightSide, m_lastPosition);
            m_arrowLeftSide.X = m_lastPosition.X;
            m_arrowLeftSide.Y = m_lastPosition.Y + 8;
            transparentPanelGraphics.DrawLine(transparentPanelPen, m_arrowLeftSide, m_lastPosition);

            transparentPanel.Invalidate();
            richTextBox.Invalidate();
            backPanel.Invalidate(); 
        } /* private void drawNorthWestArrow(MouseEventArgs e) */

        #endregion
    }
}
