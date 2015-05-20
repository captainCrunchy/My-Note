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
        private string m_subjectTitle = "New Subject";
        private List<Page> m_pages = new List<Page>();

        public string SubjectTitle
        {
            get
            {
                return m_subjectTitle;
            }
            set
            {
                m_subjectTitle = value;
            }
        }

        // consider rebuilding this, get based on page number
        public List<Page> Pages
        {
            get
            {
                return m_pages;
            }
            set
            {
                m_pages = value;
            }
        }

        /*
         *  8:10am 5/20/2015
         */
        public void AddNewPage(Page a_page)
        {
            m_pages.Add(a_page);
        }

        /*
         *  6:00pm 5/19/2015
         */
        public Subject()
        {

        }

        /*
         *  6:19pm 5/19/2015 // used in deserializer
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
/* Code below is working code when 1 page would save successfully. I did this before implementing
 * features to add new pages.



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

        public string SubjectTitle
        {
            get
            {
                return m_subjectTitle;
            }
            set
            {
                m_subjectTitle = value;
            }
        }

        // consider rebuilding this, get based on page number
        public List<Page> Pages
        {
            get
            {
                return m_pages;
            }
            set
            {
                m_pages = value;
            }
        }

        /*
         *  8:10am 5/20/2015
         *
        public void AddNewPage(Page a_page)
        {
            m_pages.Add(a_page);
        }

        /*
         *  6:00pm 5/19/2015
         *
        public Subject()
        {

        }

        /*
         *  6:19pm 5/19/2015 // used in deserializer
         *
        public Subject(SerializationInfo a_info, StreamingContext a_context)
        {
            m_subjectTitle = (string)a_info.GetValue("SubjectTitle", typeof(string));
            m_pages = (List<Page>)a_info.GetValue("Pages", typeof(List<Page>));
        }

        /*
         *  6:33pm 5/19/2015
         *
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("SubjectTitle", m_subjectTitle);
            a_info.AddValue("Pages", m_pages);
        }
    }
} */