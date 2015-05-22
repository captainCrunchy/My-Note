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
        private Page m_firstPage = new Page();
        private int m_currentPageNumber = 1;
        /*
         *  Think about how page numbers should be. Maybe I should have a
         *  current page for each subject? Possibly current page shown?
         *  Possibly total number of pages? Maybe when user returns they
         *  go to the page from where they left off (save current page)?
         */

        /*
         *  8:42am 5/21/2015
         */
        public Page getPageForPageNumber(int a_pageNumber)
        {
            int index = a_pageNumber - 1;
            Page retPage;
            if (a_pageNumber > m_pages.Count)
            {
                retPage = new Page();
                m_pages.Add(retPage);
            }
            else
            {
                retPage = m_pages[index];
            }
            return retPage;
        }

        public void savePageWithPageNumber(Page a_page, int a_pageNumber)
        {
            int index = a_pageNumber - 1;
            m_pages[index] = a_page;
        }
        
        public int TotalNumberOfPages
        {
            get
            {
                return m_pages.Count;
            }
        }

        public int CurrentPageNumber
        {
            get
            {
                return m_currentPageNumber;
            }
            set
            {
                m_currentPageNumber = value;
            }
        }

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
         *  Each subject starts with first page
         */
        public Subject()
        {
            m_pages.Add(m_firstPage);
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