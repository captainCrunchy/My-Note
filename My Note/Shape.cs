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
 *      Shape
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to provide the capacity to draw a shape by using individual points and connecting
 *      them based on a shape number. This class is intended to work with ShapeContainer class, therefore, each shape
 *      number can have many points as well as custom line width and line color. The reason for creating so many points
 *      is to accomodate erase functionality where the eraser can remove individual points and not just full sections,
 *      especially when this shape class is used to draw rectangles and ellipses. Initial code for this class was taken
 *      from an existing project on the web with pencil and eraser functionality and extended to work with other shapes
 *      like lines, ellipses, and rectangles. Changes to this class were made to member variables and arguments in methods
 *      by updating the naming conventions to match this application. Public member variables were converted to private and
 *      were assigned propeties. This class now implements 'ISerializable' interface, which allows this object to control
 *      its own serialization and deserialization. This class is marked with the 'SerializableAttribute' and is 'sealed' to
 *      prevent inheritance.
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
    sealed public class Shape : ISerializable
    {
        private Point m_pointLocation;          // Position of the point
        private float m_lineWidth;              // Width of the line
        private Color m_lineColor;              // Color of the line
        private int m_shapeNumber;              // Part of which shape it belongs to

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

        // Shape number within a container
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

        /*
         * NAME
         *  Shape() - gets called when creating a custom 'Shape' object
         *  
         * SYNOPSIS
         *  public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber);
         *      a_pointLocation     -> stores the line location
         *      a_lineWidth         -> stores the line width
         *      a_lineColor         -> stores the line color
         *      a_shapeNumber       -> stores the shape number
         *      
         * DESCRIPTION
         *  This constructor gets called when instances of this object are created by and are being added
         *  to the ShapeContainer object.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  10:12am 5/20/2015
         */
        public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            m_pointLocation = a_pointLocation;
            m_lineWidth = a_lineWidth;
            m_lineColor = a_lineColor;
            m_shapeNumber = a_shapeNumber;
        } /* public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber) */

        /*
         * NAME
         *  Shape() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public Shape(SerializationInfo a_info, StreamingContext a_context);
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
         *  9:37am 5/20/2015
         */
        public Shape(SerializationInfo a_info, StreamingContext a_context)
        {
            m_pointLocation = (Point)a_info.GetValue("PointLocation", typeof(Point));
            m_lineWidth = (float)a_info.GetValue("LineWidth", typeof(float));
            m_lineColor = (Color)a_info.GetValue("LineColor", typeof(Color));
            m_shapeNumber = (int)a_info.GetValue("ShapeNumber", typeof(int));
        } /* public Shape(SerializationInfo a_info, StreamingContext a_context) */

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
         *  9:21am 5/20/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("PointLocation", m_pointLocation);
            a_info.AddValue("LineWidth", m_lineWidth);
            a_info.AddValue("LineColor", m_lineColor);
            a_info.AddValue("ShapeNumber", m_shapeNumber);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */
    }
}