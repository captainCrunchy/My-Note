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
        private Shapes m_shapesStorage = new Shapes();      // Storage of all the drawing data
        private bool m_isDrawing = false;                   // Is the mouse currently down (DRAWING)
        private bool m_isErasing = false;                   // Is the mouse currently down (ERASING)
        private Point m_lastPosition = new Point(0, 0);     // Last Position, used to cut down on repetative data
        private Color m_currentDrawColor = Color.Black;     // Current drawing color
        private float m_currentPenWidth = 1;                // Current pen width
        private int m_shapeNumber = 0;                      // Record the shape numbers so they can be drawn separately
        private Point m_mouseLoc = new Point(0, 0);         // Current mouse position
        private Point m_drawStartPoint;                     // Start point of arrow or line
        private Point m_drawEndPoint;                       // End point of arrow or line
        private bool IsMouseing = false;                //Draw the mouse?

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
            mslog("MouseDown");
            // Prepare to draw using pencil
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                m_isDrawing = true;
                m_shapeNumber++;
                m_lastPosition = new Point(0, 0);
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
// WARNING!!!!!!!!!!!!!!!!! incrementing shape
                m_shapeNumber++;
                m_lastPosition = new Point(0, 0);

                //mslog("start point = " + m_drawStartPoint);
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
                    // Free hand draw any shape in any direction
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
                    // Remove any point within a certain distance of the mouse
                    m_shapesStorage.RemoveShape(e.Location, 10);

                    transparentPanel.Invalidate();
                    backPanel.Invalidate();
                    richTextBox.Invalidate();
                    transparentPanel.Refresh();
                }
            }
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                if (m_isDrawing)
                {
                    // Draw an arrow pointing West
                    if (m_lastPosition != e.Location &&
                        e.Location.X < m_drawStartPoint.X)
                    {
                        m_lastPosition.X = e.Location.X;  // Restrict drawing direction
                        m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
                        //m_shapesStorage.NewShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                        //mslog("drawing arrow");
                        
                        
                        Graphics g = this.transparentPanel.CreateGraphics();
                        Pen pen = new Pen(m_currentDrawColor);
                        g.DrawLine(pen, m_drawStartPoint, m_lastPosition);
                        //g.Dispose();
                        transparentPanel.Invalidate();
                        richTextBox.Invalidate();
                        backPanel.Invalidate();
                    }
                    //transparentPanel.Refresh();
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
                if (m_isDrawing)
                {
                    // Draw and save only from beginning to end points
                    m_drawEndPoint = e.Location;
                    for (int i = m_drawEndPoint.X; i <= m_drawStartPoint.X; i++)
                    {
                        m_lastPosition.X = i;  // Draw the X points
                        m_lastPosition.Y = m_drawStartPoint.Y;  // Restrict vertical movement
                        m_shapesStorage.NewShape(m_lastPosition, m_currentPenWidth, m_currentDrawColor, m_shapeNumber);
                        //mslog("drew shapes, m_shapeNumber = " + m_shapeNumber);
                    }
                    m_isDrawing = false;
                }
                //mslog("end point = " + m_drawEndPoint);
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
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
            if (m_currentSelectedControl == e_SelectedControl.WARROW)
            {
                transparentPanel.Cursor = Cursors.Cross;
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                transparentPanel.Cursor = Cursors.Hand;
            }
        }
        /*
             * 3/11/15 8:19am.
             */
        private void transparentPanel_MouseEnter(object sender, EventArgs e)
        {
            //mslog("MouseEnter");

            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                //Hide the mouse cursor and tell the re-drawing function to draw the mouse
                //Cursor.Hide();
                //IsMouseing = true;
                //mslog("MouseEnter PENCIL");
            }
        }
        /*
         * 3/10/15 9:54am
         */
        private void transparentPanel_MouseLeave(object sender, EventArgs e)
        {
            //mslog("MouseLeave");
            //show the mouse, tell the re-drawing function to stop drawing it and force the panel to re-draw.
            //Cursor.Show();
            //IsMouseing = false;
            //transparentPanel.Refresh();
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
                //mslog("Paint DRAW THE LINES");
                Shape T = m_shapesStorage.GetShape(i);
                Shape T1 = m_shapesStorage.GetShape(i + 1);
                //make sure shape the two ajoining shape numbers are part of the same shape
                if (T.ShapeNumber == T1.ShapeNumber)
                {
                    //create a new pen with its width and colour
                    Pen p = new Pen(T.LineColor, T.LineWidth);
                    p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    //draw a line between the two ajoining points
                    e.Graphics.DrawLine(p, T.PointLocation, T1.PointLocation);
                    //get rid of the pen when finished
                    p.Dispose();
                }
            }
            //If mouse is on the panel, draw the mouse
            //if (IsMouseing && IsMouseDown)
            if (IsMouseing)
            {
                //mslog("Paint isMouseing");
                //e.Graphics.DrawEllipse(new Pen(Color.Transparent, 0.5f), MouseLoc.X - (CurrentWidth / 2), MouseLoc.Y - (CurrentWidth / 2), CurrentWidth, CurrentWidth);
                e.Graphics.DrawEllipse(new Pen(Color.White, 0.5f), m_mouseLoc.X - (m_currentPenWidth / 2), m_mouseLoc.Y - (m_currentPenWidth / 2), m_currentPenWidth, m_currentPenWidth);
                transparentPanel.Invalidate();
            }
            /*
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                //logTextBox.Text = "panel paint";
                //Apply a smoothing mode to smooth out the line.
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //DRAW THE LINES
                for (int i = 0; i < DrawingShapes.NumberOfShapes() - 1; i++)
                {
                    Shape T = DrawingShapes.GetShape(i);
                    Shape T1 = DrawingShapes.GetShape(i + 1);
                    //make sure shape the two ajoining shape numbers are part of the same shape
                    if (T.ShapeNumber == T1.ShapeNumber)
                    {
                        //create a new pen with its width and colour
                        Pen p = new Pen(T.Colour, T.Width);
                        p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        //draw a line between the two ajoining points
                        e.Graphics.DrawLine(p, T.Location, T1.Location);
                        //get rid of the pen when finished
                        p.Dispose();
                    }
                }
                //If mouse is on the panel, draw the mouse
                //if (IsMouseing && IsMouseDown)
                if (IsMouseing)
                {
                    //e.Graphics.DrawEllipse(new Pen(Color.Transparent, 0.5f), MouseLoc.X - (CurrentWidth / 2), MouseLoc.Y - (CurrentWidth / 2), CurrentWidth, CurrentWidth);
                    e.Graphics.DrawEllipse(new Pen(Color.White, 0.5f), MouseLoc.X - (CurrentWidth / 2), MouseLoc.Y - (CurrentWidth / 2), CurrentWidth, CurrentWidth);
                    transparentPanel.Invalidate();
                }
            }*/
        }
        /*
         * 3/10/15 9:53am
         */

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

/* notes
 * 
 * Microsoft Sans Serif 12pt - line height = 20; max size = 1866 (using lorem ipsum)
 * 
 */
