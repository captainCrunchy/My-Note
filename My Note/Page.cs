using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      Page
 *      
 *  DESCRIPTION:
 *      This class is used as the underlying container for each page that's displayed in the user interface. It's member
 *      variables hold the actual data that is shown to the user, which gets saved to and loaded from disk. This class
 *      implements the 'ISerializable' interface, which allows this object to control its own serialization and 
 *      deserialization. This class is marked with the 'SerializableAttribute' and is 'sealed' to prevent inheritance.
 *      Custom constructor is used in the deserialization process and regular construtor is used for temporary uses of
 *      this object in the application. GetObjectData() is used for serialization of this object.
 *      
 *  CODE STRUCTURE:
 *      - Member variables
 *      - Properties
 *      - Constructors
 *      - Data persistence and serialization methods
 */

namespace My_Note
{
    [Serializable()]
    sealed class Page : ISerializable
    {
        private ShapeContainer m_shapeContainer = new ShapeContainer();             // Used to store 'Shape' objects
        private List<VerticalText> m_verticalTextList = new List<VerticalText>();   // Used to store 'VerticalText' objects
        private string m_rtfTextCode = "";                                          // Used to store rich text formatting

        // Stores 'Shape(s)'
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

        // Stores 'Vertical Text(s)'
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

        // Remembers rich text formats
        public string RTFTextCode
        {
            get
            {
                return m_rtfTextCode;
            }
            set
            {
                m_rtfTextCode = value;
            }
        }

        /*
         * NAME
         *  Page() - gets called to construct this object
         *  
         * SYNOPSIS
         *  public Page();
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
         *  5:20pm 5/19/2015
         */
        public Page()
        {

        } /* public Page() */

        /*
         * NAME
         *  Page() - gets called to deserialize this object
         *  
         * SYNOPSIS
         *  public Page(SerializationInfo a_info, StreamingContext a_context);
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
         *  6:27pm 5/19/2015
         */
        public Page(SerializationInfo a_info, StreamingContext a_context)
        {
            m_shapeContainer = (ShapeContainer)a_info.GetValue("ShapeContainer", typeof(ShapeContainer));
            m_verticalTextList = (List<VerticalText>)a_info.GetValue("VerticalTextList", typeof(List<VerticalText>));
            m_rtfTextCode = (string)a_info.GetValue("RTFTextCode", typeof(string));
        } /* public Page(SerializationInfo a_info, StreamingContext a_context) */

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
         *  6:31pm 5/19/2015
         */
        public void GetObjectData(SerializationInfo a_info, StreamingContext a_context)
        {
            a_info.AddValue("ShapeContainer", m_shapeContainer);
            a_info.AddValue("VerticalTextList", m_verticalTextList);
            a_info.AddValue("RTFTextCode", m_rtfTextCode);
        } /* public void GetObjectData(SerializationInfo a_info, StreamingContext a_context) */
    }
}
