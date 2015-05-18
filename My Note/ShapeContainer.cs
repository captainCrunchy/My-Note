using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/*
 *  TITLE:
 *      ShapeContainer
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to store instances of the Shape class. Shapes in the container are related
 *      based on a shape number. An object of this class is intended to be used in a (someObject_Paint) event method
 *      by using a for-loop and connecting shapes based on their shape number. This class contains methods that
 *      accomodate the creation of shapes using points and removal of points upon erase execution by the user. Initial
 *      code for this class was taken from an existing project on the web with pencil and eraser functionality and
 *      extended to work with other shapes like lines, ellipses, and rectangles. The only changes to this class were
 *      made to member variables and arguments in methods by udpating the naming conventions to match this application.
 *      Public member variables were converted to private and were assigned properties.
 *      A link to the author and site is provided below:
 *      Geoff Samuel, May 23, 2011
 *      http://www.codeproject.com/Articles/198419/Painting-on-a-panel
 * 
 *  CODE STRUCTURE:
 *      This class has a basic layout with member variables and methods.
 */

namespace My_Note
{
    public class ShapeContainer
    {
        private List<Shape> m_shapes;           // stores all the shapes

        public ShapeContainer()
        {
            m_shapes = new List<Shape>();
        }

        // Returns the number of shapes being stored
        public int NumberOfShapes()
        {
            return m_shapes.Count;
        }

        // Add a shape to the database, recording its position, width, color and shape related information
        public void AddShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            m_shapes.Add(new Shape(a_pointLocation, a_lineWidth, a_lineColor, a_shapeNumber));
        }

        // Returns a shape of the requested data
        public Shape GetShape(int a_index)
        {
            return m_shapes[a_index];
        }

        // Removes any point data within a certain threshold of a point
        public void RemoveShape(Point a_pointLocation, float a_threshold)
        {
            for (int i = 0; i < m_shapes.Count; i++)
            {
                // Finds if a point is within a certain distance of the point to remove
                if ((Math.Abs(a_pointLocation.X - m_shapes[i].PointLocation.X) < a_threshold) &&
                    (Math.Abs(a_pointLocation.Y - m_shapes[i].PointLocation.Y) < a_threshold))
                {
                    // Removes all data for that number
                    m_shapes.RemoveAt(i);

                    // Goes through the rest of the data and adds an extra 1 to defined them as a seprate shape and shuffles on the effect
                    for (int n = i; n < m_shapes.Count; n++)
                    {
                        m_shapes[n].ShapeNumber += 1;
                    }
                    // Go back a step so we dont miss a point
                    i -= 1;
                }
            }
        }
    }
}
