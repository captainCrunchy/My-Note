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
    class StoreHandler // StoreHandler == Serializer
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
        public void SaveMyNoteStore(string a_fileName, MyNoteStore a_myNoteStore)
        {
            Stream saveStream = File.Open(a_fileName, FileMode.Create);
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(saveStream, a_myNoteStore);
            saveStream.Close();
        }

        /*
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
        }
    }
}

/*
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {   // Save based on subject?
            string textToSave = richTextBox.Text;
            ShapeContainer shapesToSave = m_shapesStorage;
            List<VerticalText> verticalTextListToSave = m_verticalTextList;

            Page pageToSave = new Page();
            pageToSave.PageText = textToSave;
            pageToSave.ShapeContainer = shapesToSave;
            pageToSave.VerticalTextList = verticalTextListToSave;
            
            Subject subjectToSave = new Subject();
            subjectToSave.Pages.Add(pageToSave);
            
            MyNoteStore myNoteToSave = new MyNoteStore();
            myNoteToSave.SavedSubjects.Add(subjectToSave);
            
            StoreHandler storeHandler = new StoreHandler();
            storeHandler.SaveMyNoteStore("savedNotes.txt", myNoteToSave);
        }
        private void restoreButton_Click(object sender, EventArgs e)
        {
            MyNoteStore myNoteToRestore = new MyNoteStore();
            StoreHandler storeHandler = new StoreHandler();
            myNoteToRestore = storeHandler.OpenMyNoteStore("savedNotes.txt");
            
            // Restore based on subject?
            Subject subjectToRestore = myNoteToRestore.SavedSubjects[0];
            
            Page pageToRestore = subjectToRestore.Pages[0];
            richTextBox.Text = pageToRestore.PageText;
            m_shapesStorage = pageToRestore.ShapeContainer;
            m_verticalTextList = pageToRestore.VerticalTextList;

            // quick fix
            transparentPanel.Controls.Clear();
            foreach (VerticalText v in m_verticalTextList)
            {
                v.setButtonProperties();
                transparentPanel.Controls.Add(v.MoveButton);
                transparentPanel.Controls.Add(v.OptionsButton);
                transparentPanel.Controls.Add(v.DeleteButton);
                transparentPanel.Controls.Add(v.RotateButton);
            } */