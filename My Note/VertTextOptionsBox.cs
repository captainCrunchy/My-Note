using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 *  TITLE:
 *      VertTextOptionsBox : Form
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to accomodate the attributes of VertText box.
 * 
 *  STRUCTURE:
 *      None yet
 */

namespace My_Note
{
    public partial class VertTextOptionsBox : Form
    {
        private Color m_colorToBeSet;   // used to update color combo box to current color
        public VertTextOptionsBox()
        {
            InitializeComponent();
        }

        /*
         *  10:54am 5/14/15
         */
        private void VertTextOptionsBox_Load(object sender, EventArgs e)
        {
            FontColorComboBox.AddStandardColors();
            //FontColorComboBox.SelectedValue = Color.White;
            FontColorComboBox.SelectedValue = m_colorToBeSet;
        }

        /*  Takes arguments by reference from the VertText object, specifically upon the MouseUp
         *  event of the options button. The parameters are first used to update the values of the
         *  form that is about to be shown to indicate current values. When values are changed in
         *  one of the combo boxes or the rich text box, they are reflected and updated to the
         *  attributes of the vertical text box
         *  12:26 5/14/15
         */
        public void SetUIAttributes(ref Font a_font, ref String a_text, ref SolidBrush a_brush)
        {
            FontStyleComboBox.Text = a_font.Name;
            FontSizeComboBox.Text = Convert.ToString(a_font.Size);
            //FontColorComboBox.SelectedValue = a_brush.Color;
            m_colorToBeSet = a_brush.Color;
            FontColorComboBox.SelectedValue = m_colorToBeSet;
            TextRichTextBox.Text = a_text;
        }
    }
}
