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
    public class Shape
    {
        /*
         * NAME
         *  function() - performs something
         * 
         * SYNOPSIS
         *  void function(argument);
         *      argument -> does nothing
         *      
         * DESCRIPTION
         *  Does something
         * 
         * RETURNS
         *  Something
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  time day
         */
        private Point m_pointLocation;          // position of the point
        private float m_lineWidth;              // width of the line
        private Color m_lineColor;              // color of the line
        private int m_shapeNumber;              // part of which shape it belongs to

        // Constructor 
        public Shape(Point a_pointLocation, float a_lineWidth, Color a_lineColor, int a_shapeNumber)
        {
            PointLocation = a_pointLocation;    // stores the line location
            LineWidth = a_lineWidth;            // stores the line width
            LineColor = a_lineColor;            // stores the line color
            ShapeNumber = a_shapeNumber;        // stores the shape number
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
