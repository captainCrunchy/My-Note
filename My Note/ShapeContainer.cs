using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace My_Note
{
    /*
     * 3/10/2015/ 9:13am
     */
    public class ShapeContainer
    {
        private List<Shape> m_shapes;    //Stores all the shapes

        public ShapeContainer()
        {
            m_shapes = new List<Shape>();
        }
        //Returns the number of shapes being stored.
        public int NumberOfShapes()
        {
            return m_shapes.Count;
        }
        //Add a shape to the database, recording its position, width, colour and shape relation information
        public void AddShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
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
