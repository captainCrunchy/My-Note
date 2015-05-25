using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// This is data persistence, does not need to be a singleton


namespace My_Note
{
    [Serializable()]
    class MyNoteStore : ISerializable // MyNoteStore == ObjectToSerialize
    {
        // do these need to be created here? can they be created in the constructor
        // since they are not being used anywhere else?
        //private Subject subjectOne = new Subject();
        //private Subject subjectTwo = new Subject();
        //private Subject subjectThree = new Subject();
        //private Subject subjectFour = new Subject();
        //private Subject subjectFive = new Subject();
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
        /*
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
        }


        // Each notebook starts with five subjects
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