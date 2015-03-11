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
        // Drawing tools
        //private bool drawPencil;
        //private bool drawEraser;
        //private Color m_currentDrawColor;

        private bool Brush = true;                      //Uses either Brush or Eraser. Default is Brush
        private Shapes DrawingShapes = new Shapes();    //Stores all the drawing data
        private bool IsPainting = false;                //Is the mouse currently down (PAINTING)
        private bool IsEraseing = false;                 //Is the mouse currently down (ERASEING)
        private Point LastPos = new Point(0, 0);        //Last Position, used to cut down on repative data.
        private Color CurrentColour = Color.Black;      //Deafult Colour
        private float CurrentWidth = 1;                //Deafult Pen width
        private int ShapeNum = 0;                       //record the shapes so they can be drawn sepratley.
        private Point MouseLoc = new Point(0, 0);       //Record the mouse position
        private bool IsMouseing = false;                //Draw the mouse?
        //private bool IsMouseDown = false;

        #region richTextBoxMethods
        /*
         * refresh the transparentPanel on top
         * limit the number of characters entered and handle it properly
         * 3/10/15 8:36 am
         */
        private void richTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //transparentPanel.Update();  // Does nothing
            //transparentPanel.Refresh();  // Refreshes but not every time
            //transparentPanel.Invalidate();  // Works like a charm

            
        }
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
            //logTextBox.Text = "transparentPanel clicked";

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
            else
            {
                //logTextBox.Text = "transparentPanel clicked NOT in text mode";
            }
        }

        /*
         * If tool is selected, start drawing or erasing 3/10/15 9:01AM
         */
        private void transparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            //mslog("MouseDown");
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                //mslog("MouseDown PENCIL");
                //IsMouseDown = true;
                //logTextBox.Text = "panel mouse down";
                //If we're painting...
                if (Brush)
                {
                    //mslog("MouseDown Brush");
                    //set it to mouse down, illatrate the shape being drawn and reset the last position
                    IsPainting = true;
                    ShapeNum++;
                    LastPos = new Point(0, 0);
                }
                ////but if we're eraseing...
                //else
                //{
                //    logTextBox.Text = "panel mouse down and ERASING";
                //    IsEraseing = true;
                //}
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                //mslog("MouseDown ERASER");
                IsEraseing = true;
            }
        }
        /*
         * If tool is selected, continue to draw or erase 3/10/15 9:01AM
         */
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //mslog("MouseMove");
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                //mslog("MouseMove PENCIL");
                //logTextBox.Text = "panel mouse move";
                MouseLoc = e.Location;
                //PAINTING
                if (IsPainting)
                {
                    //mslog("MouseMove IsPainting");
                    //check its not at the same place it was last time, saves on recording more data.
                    if (LastPos != e.Location)
                    {
                        //set this position as the last positon
                        LastPos = e.Location;
                        //store the position, width, colour and shape relation data
                        DrawingShapes.NewShape(LastPos, CurrentWidth, CurrentColour, ShapeNum);
                    }
                }
                //if (IsEraseing)
                //{
                //    logTextBox.Text = "panel mouse move and ERASING";
                //    //Remove any point within a certain distance of the mouse
                //    DrawingShapes.RemoveShape(e.Location, 10);
                //}
                //refresh the panel so it will be forced to re-draw.
                transparentPanel.Refresh();
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                //mslog("MouseMove ERASER");
                //logTextBox.Text = "panel mouse move";
                MouseLoc = e.Location;
                ////PAINTING
                //if (IsPainting)
                //{
                //    logTextBox.Text = "panel mouse move and PAINTING";
                //    //check its not at the same place it was last time, saves on recording more data.
                //    if (LastPos != e.Location)
                //    {
                //        //set this position as the last positon
                //        LastPos = e.Location;
                //        //store the position, width, colour and shape relation data
                //        DrawingShapes.NewShape(LastPos, CurrentWidth, CurrentColour, ShapeNum);
                //    }
                //}
                if (IsEraseing)
                {
                    //mslog("MouseMove IsEraseing");
                    //Remove any point within a certain distance of the mouse
                    DrawingShapes.RemoveShape(e.Location, 10);

                    transparentPanel.Invalidate();
                    backPanel.Invalidate();
                    richTextBox.Invalidate();
                }
                //refresh the panel so it will be forced to re-draw.
                transparentPanel.Refresh();
            }
        }

        /*
         * Reset values and redraw transparentPanel and richTexbox 3/10/15 9:49am
         */
        private void transparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            //mslog("MouseUp");
            if (m_currentSelectedControl == e_SelectedControl.PENCIL)
            {
                //mslog("MouseUp, PENCIL");
                //IsMouseDown = false;
                if (IsPainting)
                {
                    //mslog("MouseUp, finished painting");
                    //Finished Painting.
                    IsPainting = false;
                }
                //if (IsEraseing)
                //{
                //    logTextBox.Text = "panel mouse up, finished erasing";
                //    //Finished Earsing.
                //    IsEraseing = false;
                //}
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
            }
            if (m_currentSelectedControl == e_SelectedControl.ERASER)
            {
                ////IsMouseDown = false;
                //if (IsPainting)
                //{
                //    logTextBox.Text = "panel mouse up, finished painting";
                //    //Finished Painting.
                //    IsPainting = false;
                //}
                //mslog("MouseUp, ERASER");
                if (IsEraseing)
                {
                    //mslog("MouseUp, finished erasing");
                    //Finished Earsing.
                    IsEraseing = false;
                }
                transparentPanel.Invalidate();
                richTextBox.Invalidate();
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
            //mslog("Paint");
            //logTextBox.Text = "panel paint";
            //Apply a smoothing mode to smooth out the line.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //DRAW THE LINES
            for (int i = 0; i < DrawingShapes.NumberOfShapes() - 1; i++)
            {
                //mslog("Paint DRAW THE LINES");
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
                //mslog("Paint isMouseing");
                //e.Graphics.DrawEllipse(new Pen(Color.Transparent, 0.5f), MouseLoc.X - (CurrentWidth / 2), MouseLoc.Y - (CurrentWidth / 2), CurrentWidth, CurrentWidth);
                e.Graphics.DrawEllipse(new Pen(Color.White, 0.5f), MouseLoc.X - (CurrentWidth / 2), MouseLoc.Y - (CurrentWidth / 2), CurrentWidth, CurrentWidth);
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
        public Point Location;          //position of the point
        public float Width;             //width of the line
        public Color Colour;            //colour of the line
        public int ShapeNumber;         //part of which shape it belongs to

        //CONSTRUCTOR
        public Shape(Point L, float W, Color C, int S)
        {
            Location = L;               //Stores the Location
            Width = W;                  //Stores the width
            Colour = C;                 //Stores the colour
            ShapeNumber = S;            //Stores the shape number
        }
    }

    /*
     * 3/10/2015/ 9:13am
     */
    public class Shapes
    {
        private List<Shape> _Shapes;    //Stores all the shapes

        public Shapes()
        {
            _Shapes = new List<Shape>();
        }
        //Returns the number of shapes being stored.
        public int NumberOfShapes()
        {
            return _Shapes.Count;
        }
        //Add a shape to the database, recording its position, width, colour and shape relation information
        public void NewShape(Point L, float W, Color C, int S)
        {
            _Shapes.Add(new Shape(L, W, C, S));
        }
        //returns a shape of the requested data.
        public Shape GetShape(int Index)
        {
            return _Shapes[Index];
        }
        //Removes any point data within a certain threshold of a point.
        public void RemoveShape(Point L, float threshold)
        {
            for (int i = 0; i < _Shapes.Count; i++)
            {
                //Finds if a point is within a certain distance of the point to remove.
                if ((Math.Abs(L.X - _Shapes[i].Location.X) < threshold) && (Math.Abs(L.Y - _Shapes[i].Location.Y) < threshold))
                {
                    //removes all data for that number
                    _Shapes.RemoveAt(i);

                    //goes through the rest of the data and adds an extra 1 to defined them as a seprate shape and shuffles on the effect.
                    for (int n = i; n < _Shapes.Count; n++)
                    {
                        _Shapes[n].ShapeNumber += 1;
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
