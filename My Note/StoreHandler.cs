using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 *  TITLE:
 *      StoreHandler
 *      
 *  DESCRIPTION:
 *      This class is used to write the current data persistence object to disk. It uses a save path provided by the caller
 *      when saving to or restoring from a disk. It is designed to work with MyNoteStore object to serialize it when saving
 *      to disk, or deserialize it when restoring from disk.
 *      
 *  CODE STRUCTURE:
 *      - Constructors
 *      - Regular methods
 */

namespace My_Note
{
    class StoreHandler
    {
        /*
         * NAME
         *  StoreHandler() - gets called to construct this object
         *  
         * SYNOPSIS
         *  public StoreHandler();
         *      
         * DESCRIPTION
         *  This constructor gets called when an instance of this object is created and used in
         *  the application.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:37pm 5/19/2015
         */
        public StoreHandler()
        {

        } /* public StoreHandler() */

        /*
         * NAME
         *  SaveMyNoteStore() - saves data to disk
         *  
         * SYNOPSIS
         *  public void SaveMyNoteStore(string a_fileName, MyNoteStore a_myNoteStore)
         *      a_fileName      -> the path to a file on disk
         *      a_myNoteStore   -> the object to be serialized and written to disk
         *      
         * DESCRIPTION
         *  This method serializes and saves an instance of MyNoteStore object to disk to a specified
         *  path. The file is saved in binary format.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:47pm 5/19/2015
         */
        public void SaveMyNoteStore(string a_fileName, MyNoteStore a_myNoteStore)
        {
            Stream saveStream = File.Open(a_fileName, FileMode.Create);
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(saveStream, a_myNoteStore);
            saveStream.Close();
        } /* public void SaveMyNoteStore(string a_fileName, MyNoteStore a_myNoteStore) */

        /*
         * NAME
         *  OpenMyNoteStore() - restores data from disk
         *  
         * SYNOPSIS
         *  public MyNoteStore OpenMyNoteStore(string a_fileName);
         *      a_fileName  -> a path used to get object on disk
         *      
         * DESCRIPTION
         *  This method uses a given path to locate a binary file on disk and deserialize it.
         *  
         * RETURNS
         *  MyNoteStore object containing saved data
         *  
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  5:53pm 5/19/2015
         */
        public MyNoteStore OpenMyNoteStore(string a_fileName)
        {
            MyNoteStore retStore;
            Stream openStream = File.Open(a_fileName, FileMode.Open);
            BinaryFormatter binFormatter = new BinaryFormatter();
            retStore = (MyNoteStore)binFormatter.Deserialize(openStream);
            openStream.Close();
            return retStore;
        } /* public MyNoteStore OpenMyNoteStore(string a_fileName) */
    }
}
