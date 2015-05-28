using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      MyNoteStore
 *      
 *  DESCRIPTION:
 *      This class is used to represent the main data storage object for this application. Since it is used in only one class
 *      it does not need to be a singleton object. This class implements the 'ISerializable' interface, which allows this
 *      object to control its own serialization and deserialization. This class is marked with the 'SerializableAttribute'
 *      and is 'sealed' to prevent inheritance. Custom constructor is used in the deserialization process and a regular
 *      constructor is used for first time creation of this object in the application. GetObjectData() is used for serialization
 *      of this object.
 *      
 *  CODE STRUCTURE:
 *      This class organizes code in the following order: member variables, properties, regular methods, data peristence
 *      methods.
 */

namespace My_Note
{
    [Serializable()]
    sealed class MyNoteStore : ISerializable
    {
        private List<Subject> m_savedSubjects;      // Used to store 'Subject' objects
        private int m_autoSaveTimeInterval = 0;     // Remembers user preferred auto-save time interval

        public List<Subject> SavedSubjects
        {
            get
            {
                return this.m_savedSubjects;
            }
        }

        public int AutoSaveTimeInterval
        {
            get
            {
                return this.m_autoSaveTimeInterval;
            }
            set
            {
                m_autoSaveTimeInterval = value;
            }
        }

        /*
         * NAME
         *  NumberOfSubjects() - gets the number of subjects used in this object
         *  
         * SYNOPSIS
         *  public int NumberOfSubjects();
         *      
         * DESCRIPTION
         *  This method checks to see how many subjects in the container are being 'used' and returns the count. 
         *  
         * RETURNS
         *  A number of objects currently used in this object
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:32pm 5/21/2015
         */
        public int NumberOfSubjects()
        {
            int currentSubjectCount = 0;
            foreach (Subject s in m_savedSubjects)
            {
                if (s.SubjectTitle != "New Subject")
                {
                    currentSubjectCount++;
                }
            }
            return currentSubjectCount;
        } /* public int NumberOfSubjects() */

        /*
         * NAME
         *  MyNoteStore() - gets called to construct this object
         *  
         * SYNOPSIS
         *  public MyNoteStore();
         * 
         * DESCRIPTION
         *  This constructor gets called when an instance of this object is created and used during
         *  application execution. It creates five 'Subject' objects and places them in a container.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:07pm 5/19/2015
         */
        public MyNoteStore()
        {
            Subject subjectOne = new Subject();
            Subject subjectTwo = new Subject();
            Subject subjectThree = new Subject();
            Subject subjectFour = new Subject();
            Subject subjectFive = new Subject();
            m_savedSubjects = new List<Subject>();
            m_savedSubjects.Add(subjectOne);
            m_savedSubjects.Add(subjectTwo);
            m_savedSubjects.Add(subjectThree);
            m_savedSubjects.Add(subjectFour);
            m_savedSubjects.Add(subjectFive);
        } /* public MyNoteStore() */

        /*
         * NAME
         *  MyNoteStore() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public MyNoteStore(SerializationInfo a_info, StreamingContext a_context);
         *      a_info      -> provides data it has stored
         *      a_context   -> does nothing (required)
         *      
         * DESCRIPTION
         *  This constructor gets called when an instance of this object is to be deserialized.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:10pm 5/19/2015
         */
        public MyNoteStore(SerializationInfo a_info, StreamingContext a_context)
        {
            m_savedSubjects = (List<Subject>)a_info.GetValue("SavedSubjects", typeof(List<Subject>));
            m_autoSaveTimeInterval = (int)a_info.GetValue("AutoSaveTimeInterval", typeof(int));
        } /* public MyNoteStore(SerializationInfo a_info, StreamingContext a_context) */

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
         *  This method is used to serialize this object by saving storing member variable into SerializationInfo object.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  6:13pm 5/19/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("SavedSubjects", m_savedSubjects);
            a_info.AddValue("AutoSaveTimeInterval", m_autoSaveTimeInterval);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */
    }
}