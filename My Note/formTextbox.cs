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
// WARNING should I add Pen and Graphics variables here so that it performs faster?
        private Shapes m_shapesStorage = new Shapes();      // Storage of all the drawing data
        private bool m_isDrawing = false;                   // Is the mouse currently down, used in MouseMove
        private bool m_isErasing = false;                   // Is the mouse currently down, used in MouseMove
        private Point m_lastPosition = new Point(0, 0);     // Last Position, used to cut down on repetative data
        private Color m_currentDrawColor = Color.Black;     // Current drawing color
        private float m_currentPenWidth = 1;                // Current pen width
        private int m_shapeNumber = 0;                      // Record the shape numbers so they can be drawn separately
        private Point m_drawStartPoint = new Point(0, 0);   // Start point of arrow or line
        private Point m_drawEndPoint = new Point(0, 0);     // End point of arrow or line
        private Point m_arrowLeftSide = new Point(0, 0);    // Left side of the arrow (dynamic), used in MouseMove
        private Point m_arrowRightSide = new Point(0, 0);   // Right side of the arrow (dynamic), used in MouseMove

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

        #region transparentPanelMethods

        /* When textControl is selected, this method will pass down the click coordinates
         * from transparent panel on top to the richTextBox below so that the cursor is positioned
         * accordingly. Coordinate modifications were created because transparentPanel and
         * richTextBox do not have the same origin. 12:47pm 3/7/15
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
        }

        /*
         * If tool is selected, start drawing or erasing 3/10/15 9:01AM
         */
        private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Prepare to draw using pencil
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                m_isDrawing = true;
                m_shapeNumber++;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to erase anything that is not text
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                m_isErasing = true;
            }
            // Prepare to draw arrow, west direction only
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                m_lastPosition = new Point(0, 0);  // This needs to be here to prevent duplicate points
            }
            // Prepare to draw a solid line
            if (m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                m_isDrawing = true;
                m_drawStartPoint = e.Location;
                //m_lastPosition = new Point(0, 0);
            }
        }
        /*
         * If tool is selected, continue to draw or erase 3/10/15 9:01AM
         */
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                if (m_isDrawing)
                {
                    // Free hand draw any shape in any direction, drawing and saving occurs here
                    if (m_lastPosition != e.Location)
                    {
                        //set this position as the last positon
                        m_lastPosition = e.Location;
                        //store the position, width, colour and shape relation data
                        m_shapesStorage.NewShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                    }
                    transparentPanel.Refresh();
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                if (m_isErasing)
                {
                    // Remove any point within a certain distance of the mouse, saving occurs here
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
                    // Saving does not occur in the code below, only on MouseUp event
                    if (m_lastPosition != e.Location &&
                        e.Location.X < m_drawStartPoint.X)
                    {
                        m_lastPosition.X = e.Location.X;  // Restrict drawing direction
                        m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
                        
                        // Draw the line part of the arrow
                        Graphics g = this.transparentPanel.CreateGraphics();
                        Pen pen = new Pen(m_currentDrawColor);
                        g.DrawLine(pen, m_drawStartPoint, m_lastPosition);

                        // Draw the arrowhead part of the arrow
                        m_arrowRightSide.X = e.Location.X + 5;
                        m_arrowRightSide.Y = m_drawStartPoint.Y - 5;
                        g.DrawLine(pen, m_arrowRightSide, m_lastPosition);
                        m_arrowLeftSide.X = m_arrowRightSide.X;
                        m_arrowLeftSide.Y = m_drawStartPoint.Y + 5;
                        g.DrawLine(pen, m_lastPosition, m_arrowLeftSide);

                        pen.Dispose();
                        g.Dispose();

                        transparentPanel.Invalidate();
                        richTextBox.Invalidate();
                        backPanel.Invalidate();                    
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
        }

        /*
         * Reset values and redraw transparentPanel and richTexbox 3/10/15 9:49am
         */
        private void transparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                if (m_isDrawing)
                {
                    m_isDrawing = false;
                    mslog("MouseUp pencil lastPos = " + m_lastPosition);
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
                m_drawEndPoint = e.Location;

                // Draw and save only the necessary and valid points (2 points per line)
                if (m_isDrawing && (m_drawEndPoint.X < m_drawStartPoint.X))
                {
                    // Add points to draw and save the line part of the arrow
                    m_shapeNumber++;
                    for (int i = m_drawStartPoint.X; i >= m_drawEndPoint.X; i--)
                    {
                        m_lastPosition.X = i;
                        m_lastPosition.Y = m_drawStartPoint.Y;
                        m_shapesStorage.NewShape(m_lastPosition, m_currentPenWidth, 
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
                        m_shapesStorage.NewShape(m_arrowRightSide, m_currentPenWidth, 
                            m_currentDrawColor, m_shapeNumber);
                    }

                    m_shapeNumber++;
                    m_arrowLeftSide.X = m_drawEndPoint.X;
                    m_arrowLeftSide.Y = m_drawStartPoint.Y;
                    for (int i = 0; i < 5; i++)
                    {
                        m_arrowLeftSide.X++;
                        m_arrowLeftSide.Y++;
                        m_shapesStorage.NewShape(m_arrowLeftSide, m_currentPenWidth, 
                            m_currentDrawColor, m_shapeNumber);
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
            if( m_currentSelectedControl == e_SelectedControl.SOLID)
//WARNING saving needs to be implemented
            {
                if(m_isDrawing)
                {
                    m_isDrawing = false;
                }
            }
        }

        /*
         * 3/17/15 12:38pm
         */
        private void transparentPanel_MouseHover(object sender, EventArgs e)
        {
            if (m_currentSelectedControl == e_SelectedControl.TEXT)
            {
                transparentPanel.Cursor = Cursors.IBeam;
            }
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                transparentPanel.Cursor = Cursors.Cross;
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                transparentPanel.Cursor = Cursors.Hand;
            }
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                transparentPanel.Cursor = Cursors.Cross;
            }
            if (m_currentSelectedControl == e_SelectedControl.SOLID)
            {
                transparentPanel.Cursor = Cursors.Cross;
            }
        }
        
        /*
         * Paint and update the panel graphics 3/10/15 9:52am
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
        }
        #endregion
    }

    /*
     * 3/10/2015/ 9:13am
     */
    public class Shape
    {
        private Point m_pointLocation;          // position of the point
        private float m_lineWidth;              // width of the line
        private Color m_lineColor;              // color of the line
        private int m_shapeNumber;              // part of which shape it belongs to

        // Position of the point
        public Point PointLocation
        {
            get
            {
                return m_pointLocation;
            }
            set
            {
                m_pointLocation = value;
            }
        }

        // Width of the line
        public float LineWidth
        {
            get
            {
                return m_lineWidth;
            }
            set
            {
                m_lineWidth = value;
            }
        }

        // Color of the line
        public Color LineColor
        {
            get
            {
                return m_lineColor;
            }
            set
            {
                m_lineColor = value;
            }
        }

        // Part of which shape it belongs to
        public int ShapeNumber
        {
            get
            {
                return m_shapeNumber;
            }
            set
            {
                m_shapeNumber = value;
            }
        }

        // Constructor 
        public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            PointLocation = a_pointLocation;    // stores the line location
            LineWidth = a_lineWidth;            // stores the line width
            LineColor = a_lineColor;            // stores the line color
            ShapeNumber = a_shapeNumber;        // stores the shape number
        }
    }

    /*
     * 3/10/2015/ 9:13am
     */
    public class Shapes
    {
        private List<Shape> m_shapes;    //Stores all the shapes

        public Shapes()
        {
            m_shapes = new List<Shape>();
        }
        //Returns the number of shapes being stored.
        public int NumberOfShapes()
        {
            return m_shapes.Count;
        }
        //Add a shape to the database, recording its position, width, colour and shape relation information
        public void NewShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            m_shapes.Add(new Shape(a_pointLocation, a_lineWidth, a_lineColor, a_shapeNumber));
        }
        //returns a shape of the requested data.
        public Shape GetShape(int a_index)
        {
            return m_shapes[a_index];
        }
        //Removes any point data within a certain threshold of a point.
        public void RemoveShape(Point a_pointLocation, float a_threshold)
        {
            for (int i = 0; i < m_shapes.Count; i++)
            {
                //Finds if a point is within a certain distance of the point to remove.
                if ((Math.Abs(a_pointLocation.X - m_shapes[i].PointLocation.X) < a_threshold) &&
                    (Math.Abs(a_pointLocation.Y - m_shapes[i].PointLocation.Y) < a_threshold))
                {
                    //removes all data for that number
                    m_shapes.RemoveAt(i);

                    //goes through the rest of the data and adds an extra 1 to defined them as a seprate shape and shuffles on the effect.
                    for (int n = i; n < m_shapes.Count; n++)
                    {
                        m_shapes[n].ShapeNumber += 1;
                    }
                    //Go back a step so we dont miss a point.
                    i -= 1;
                }
            }
        }
    }
}
