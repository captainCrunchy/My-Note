using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      ShapeContainer
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to store instances of the Shape class. 'Shapes' in the container are related based on a 
 *      shape number. An object of this class is intended to be used in a (someObject_Paint) event method by using a for-loop
 *      and connecting shapes based on their shape number. This class contains methods that accomodate the creation of shapes
 *      using points and removal of points upon erase execution by the user. Initial code for this class was taken from an
 *      existing project on the web with pencil and eraser functionality and extended to work with other shapes like lines,
 *      ellipses, and rectangles. The only changes to this class were made to member variables and arguments in methods by
 *      udpating the naming conventions to match this application. Public member variables were converted to private and were
 *      assigned properties. This class now implements 'ISerializable' interface, which allows this object to control its own
 *      serialization and deserialization. This class is marked with the 'SerializableAttribute' and is 'sealed' to prevent
 *      inheritance. 
 *      A link to the author and site is provided below:
 *      Geoff Samuel, May 23, 2011
 *      http://www.codeproject.com/Articles/198419/Painting-on-a-panel
 * 
 *  CODE STRUCTURE:
 *      This class maintains much of original code structure with the addition of data persistence methods at the end.
 */

namespace My_Note
{
    [Serializable()]
    sealed public class ShapeContainer : ISerializable
    {
        private List<Shape> m_shapes;           // stores all the shapes

        /*
         * NAME
         *  ShapeContainer() - default constructor
         *  
         * SYNOPSIS
         *  public ShapeContainer();
         *      
         * DESCRIPTION
         *  This constructor gets called when instances of this object are created.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Geoff Samuel
         *  
         * DATE
         *  5/23/2011
         */
        public ShapeContainer()
        {
            m_shapes = new List<Shape>();
        } /* public ShapeContainer() */

        /*
         * NAME
         *  NumberOfShapes() - gets the number of shapes stored
         *  
         * SYNOPSIS
         *  public int NumberOfShapes();
         *      
         * DESCRIPTION
         *  This constructor method gets the number of shapes this container stores.
         *  
         * RETURNS
         *  Returns the number of shapes being stored
         *  
         * AUTHOR
         *  Geoff Samuel
         *  
         * DATE
         *  5/23/2011
         */
        public int NumberOfShapes()
        {
            return m_shapes.Count;
        } /* public int NumberOfShapes() */

        /*
         * NAME
         *  AddShape() - adds a shape to this container
         *  
         * SYNOPSIS
         *  public void AddShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber);
         *      a_pointLocation     -> stores the line location
         *      a_lineWidth         -> stores the line width
         *      a_lineColor         -> stores the line color
         *      a_shapeNumber       -> stores the shape number
         *      
         * DESCRIPTION
         *  Add a shape to the database, recording its position, width, color and shape related information.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Geoff Samuel
         *  
         * DATE
         *  5/23/2011
         */
        public void AddShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            m_shapes.Add(new Shape(a_pointLocation, a_lineWidth, a_lineColor, a_shapeNumber));
        } /* public void AddShape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber) */

        /*
         * NAME
         *  GetShape() - gets a specific shape form container
         *  
         * SYNOPSIS
         *  public Shape GetShape(int a_index);
         *      a_index    -> index of the shape to be retrieved
         *      
         * DESCRIPTION
         *  Returns a shape of the requested data.
         *  
         * RETURNS
         *  Instance of a Shape object
         *  
         * AUTHOR
         *  Geoff Samuel
         *  
         * DATE
         *  5/23/2011
         */
        public Shape GetShape(int a_index)
        {
            return m_shapes[a_index];
        } /* public Shape GetShape(int a_index) */

        /*
         * NAME
         *  GetShape() - gets a specific shape form container
         *  
         * SYNOPSIS
         *  public Shape GetShape(int a_index);
         *      a_pointLocation     -> location of the point to be removed
         *      a_threshold         -> the proximity of shapes to location to be removed
         *      
         * DESCRIPTION
         *  Removes any point data within a certain threshold of a point.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Geoff Samuel
         *  
         * DATE
         *  5/23/2011
         */
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
        } /* public void RemoveShape(Point a_pointLocation, float a_threshold) */

        /*
         * NAME
         *  ShapeContainer() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public ShapeContainer(SerializationInfo a_info, StreamingContext a_context);
         *      a_info      -> provides data it has stored
         *      a_context   -> does nothing (required)
         *      
         * DESCRIPTION
         *  This constructor gets called when instances of this object are to be deserialized.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:33am 5/20/2015
         */
        public ShapeContainer(SerializationInfo a_info, StreamingContext a_context)
        {
            m_shapes = (List<Shape>)a_info.GetValue("Shapes", typeof(List<Shape>));
        } /* public ShapeContainer(SerializationInfo a_info, StreamingContext a_context) */

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
         *  This method is used to serialize this object by storing member variables into SerializationInfo object.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  9:18am 5/20/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("Shapes", m_shapes);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */
    }
}