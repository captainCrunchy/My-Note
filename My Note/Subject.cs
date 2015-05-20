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
    class Subject : ISerializable// Subject == Car
    {
        private string m_subjectTitle = "";
        private List<Page> m_pages = new List<Page>();

        /*
         *  6:00pm 5/19/2015
         */
        public Subject()
        {

        }

        /*
         *  6:19pm 5/19/2015
         */
        public Subject(SerializationInfo a_info, StreamingContext a_context)
        {
            m_subjectTitle = (string)a_info.GetValue("SubjectTitle", typeof(string));
            m_pages = (List<Page>)a_info.GetValue("Pages", typeof(List<Page>));
        }

        /*
         *  6:33pm 5/19/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("SubjectTitle", m_subjectTitle);
            a_info.AddValue("Pages", m_pages);
        }
    }
}