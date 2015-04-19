using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/*
 *  TITLE:
 *      Shape
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to provide the capacity to draw a shape by using individual points and
 *      connect them all based on a shape number. This class is intended to work with ShapeContainer class,
 *      therefore, each shape number can have many points as well as custom line width and line color. The 
 *      reason for creating so many points is to accomodate erase functionality where the eraser can remove
 *      individual points and not just sections, especially when this shape class is used to draw rectangles
 *      and ellipses. All the code for this class was taken from an existing project on the web with pencil
 *      and eraser functionality and extended to work with other shapes like lines, ellipses and rectangles.
 *      The only chages to this class were made to member variables and arguments in methods by updating the
 *      naming conventions to match this application. Public member variables were converted to private and
 *      were assigned propeties.
 *      A link to the author and site is provided below:
 *      Geoff Samuel, May 23, 2011
 *      http://www.codeproject.com/Articles/198419/Painting-on-a-panel
 *      
 *  STRUCTURE:
 *      This class contains a custom constructor and member variables along with their properties.
 */

namespace My_Note
{
    public class Shape
    {
        private Point m_pointLocation;          // position of the point
        private float m_lineWidth;              // width of the line
        private Color m_lineColor;              // color of the line
        private int m_shapeNumber;              // part of which shape it belongs to

        // Custom constructor 
        public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            m_pointLocation = a_pointLocation;  // stores the line location
            m_lineWidth = a_lineWidth;          // stores the line width
            m_lineColor = a_lineColor;          // stores the line color
            m_shapeNumber = a_shapeNumber;      // stores the shape number
        }

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
    }
}
