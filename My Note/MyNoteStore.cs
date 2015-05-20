using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// This is data persistence


namespace My_Note
{
    [Serializable()]
    class MyNoteStore : ISerializable // MyNoteStore == ObjectToSerialize
    {
        private List<Subject> m_savedSubjects; // Subjects == Cars

        /*
         *  6:05pm 5/19/2015
         */
        public List<Subject> SavedSubjects
        {
            get
            {
                return this.m_savedSubjects;
            }
            set
            {
                m_savedSubjects = value;
            }
        }

        public MyNoteStore()
        {

        }

        /*
         *  6:10pm 5/19/2015
         */
        public MyNoteStore(SerializationInfo a_info, StreamingContext a_context)
        {
            m_savedSubjects = (List<Subject>)a_info.GetValue("SavedSubjects", typeof(List<Subject>));
        }

        /*
         *  6:13pm 5/19/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("SavedSubjects", m_savedSubjects);
        }
    }
}