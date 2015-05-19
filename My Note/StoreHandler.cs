using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace My_Note
{
    class StoreHandler // storeHandler == serializer
    {
        /*
         *  5:37pm 5/19/2015
         */ 
        public StoreHandler()
        {

        }

        /*
         *  5:47pm 5/19/2015
         */ 
        public void SaveMyNoteStore (string a_fileName, MyNoteStore a_myNoteStore)
        {
            Stream saveStream = File.Open(a_fileName, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(saveStream, a_myNoteStore);
            saveStream.Close();
        }

        /*
         *  5:53pm 5/19/2015
         */ 
        public MyNoteStore OpenMyNoteStore (string a_fileName, MyNoteStore a_myNoteStore)
        {
            MyNoteStore retStore;
            Stream openStream = File.Open(a_fileName, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            retStore = (MyNoteStore)bFormatter.Deserialize(openStream);
            openStream.Close();
            return retStore;
        }
    }
}
