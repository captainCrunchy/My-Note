using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      Subject
 *      
 *  DESCRIPTION:
 *      This class is used to represent a 'Subject' in the notebook. It has a container of 'Page' objects, a title, and page
 *      number variable to remember last page used. This class implements the 'ISerializable' interface, which allows this
 *      object to control its own serialization and deserialization. This class is marked with the 'SerializableAttribute'
 *      and is 'sealed' to prevent inheritance. Custom constructor is used in the deserialization process and a regular
 *      constructor is used for temporary uses of this object in the application. GetObjectData() is used for serialization
 *      of this object.
 *      
 *  CODE STRUCTURE:
 *      This class organizes code in the following order: member variables, properties, regular methods, data peristence
 *      methods.
 */

namespace My_Note
{
    [Serializable()]
    sealed class Subject : ISerializable
    {
        private string m_subjectTitle = "New Subject";      // Used to hold the title for this subject
        private List<Page> m_pages = new List<Page>();      // Used to store 'Page' objects
        private int m_currentPageNumber = 1;                // Used to remember last page used in this 'Subject'

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

        /*
         * NAME
         *  getPageForPageNumber() - looks up and returns a 'Page' object
         *  
         * SYNOPSIS
         *  public Page getPageForPageNumber(int a_pageNumber);
         *      a_pageNumber    -> used as an index to look up an existing page
         *      
         * DESCRIPTION
         *  This method looks for a page in the container of 'Page' objects based on a given index. If the
         *  index number is beyond the number of pages in the container, then a new page is created, added
         *  to the container, and returned to the caller.
         *  
         * RETURNS
         *  Returns either a new 'blank' Page object or an existing one
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
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
        } /* public Page getPageForPageNumber(int a_pageNumber) */

        /*
         * NAME
         *  savePageWithPageNumber() - saves a page in the 'Page' objects container
         *  
         * SYNOPSIS
         *  public void savePageWithPageNumber(Page a_page, int a_pageNumber)
         *      a_page          -> the page to be saved
         *      a_pageNumber    -> helps index the page location
         *      
         * DESCRIPTION
         *  This method saves a 'Page' object into the container at the location based on the
         *  given page number.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:47am 5/21/2015
         */
        public void savePageWithPageNumber(Page a_page, int a_pageNumber)
        {
            int index = a_pageNumber - 1;
            m_pages[index] = a_page;
        } /* public void savePageWithPageNumber(Page a_page, int a_pageNumber) */
        
        /*
         * NAME
         *  Subject() - gets called to construct this object
         *  
         * SYNOPSIS
         *  public Subject();
         * 
         * DESCRIPTION
         *  This constructor gets called when instances of this object are used during normal
         *  application execution.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:00pm 5/19/2015
         */
        public Subject()
        {

        } /* public Subject() */

        /*
         * NAME
         *  Subject() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public Subject(SerializationInfo a_info, StreamingContext a_context);
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
         *  6:19pm 5/19/2015
         */
        public Subject(SerializationInfo a_info, StreamingContext a_context)
        {
            m_subjectTitle = (string)a_info.GetValue("SubjectTitle", typeof(string));
            m_pages = (List<Page>)a_info.GetValue("Pages", typeof(List<Page>));
            m_currentPageNumber = (int)a_info.GetValue("CurrentPage", typeof(int));
        } /* public Subject(SerializationInfo a_info, StreamingContext a_context) */

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
         *  6:33pm 5/19/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("SubjectTitle", m_subjectTitle);
            a_info.AddValue("Pages", m_pages);
            a_info.AddValue("CurrentPage", m_currentPageNumber);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */
    }
}