using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// class to help data persistence

namespace My_Note
{
    [Serializable()]
    class Page : ISerializable // Page == Owner
    {
        // I may not need to save this page number here because the subject
        // keeps track of pages as well.
        private int m_pageNumber = 1;
        private ShapeContainer m_shapeContainer = new ShapeContainer();
        private List<VerticalText> m_verticalTextList = new List<VerticalText>();
        private string m_pageText = "";
        public string PageText
        {
            get
            {
                return m_pageText;
            }
            set
            {
                m_pageText = value;
            }
        }

        public ShapeContainer ShapeContainer
        {
            get
            {
                return m_shapeContainer;
            }
            set
            {
                m_shapeContainer = value;
            }
        }

        public List<VerticalText> VerticalTextList
        {
            get
            {
                return m_verticalTextList;
            }
            set
            {
                m_verticalTextList = value;
            }
        }

        /*
         *  5:20pm 5/19/2015
         */
        public Page()
        {

        }
        
        /*
         *  6:27pm 5/19/2015 // used in deserializer
         */
        public Page(SerializationInfo a_info, StreamingContext a_context)
        {
            m_pageNumber = (int)a_info.GetValue("PageNumber", typeof(int));
            m_shapeContainer = (ShapeContainer)a_info.GetValue("ShapeContainer", typeof(ShapeContainer));
            m_verticalTextList = (List<VerticalText>)a_info.GetValue("VerticalTextList", typeof(List<VerticalText>));
            m_pageText = (string)a_info.GetValue("PageText", typeof(string));
        }

        /*
         *  6:31pm 5/19/2015
         *
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("PageNumber", m_pageNumber);
            a_info.AddValue("ShapeContainer", m_shapeContainer);
            a_info.AddValue("VerticalTextList", m_verticalTextList);
            a_info.AddValue("PageText", m_pageText);
        }
    }
}