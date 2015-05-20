using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// class to help data persistence

namespace My_Note
{
    [Serializable()]
    class Page : ISerializable // Page == Owner
    {
        private int m_pageNumber = 1;
        private ShapeContainer m_shapeContainer = new ShapeContainer();
        private List<VerticalText> m_verticalTextList = new List<VerticalText>();
        private string m_pageText = "";

        /*
         *  5:20pm 5/19/2015
         */
        public Page()
        {

        }

        /*
         *  6:27pm 5/19/2015
         */
        public Page(SerializationInfo a_info, StreamingContext ctx)
        {
            m_pageNumber = (int)a_info.GetValue("PageNumber", typeof(int));
            m_pageText = (string)a_info.GetValue("PageText", typeof(string));
        }

        /*
         *  6:31pm 5/19/2015
         *
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("PageNumber", m_pageNumber);
            a_info.AddValue("PageText", m_pageText);
        }
    }
}