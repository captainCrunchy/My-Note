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
 *      HelpForm : Form
 *      
 *  DESCRIPTION:
 *      This class represents the 'Help' Form in the application that visually provides descriptions and definitions of
 *      user interface elements. It is accessed by expanding the 'Help' menu and selecting 'My Note Help' menu item.
 *      
 *  CODE STRUCTURE:
 *      - Constructors
 *      - Event handlers
 */

namespace My_Note
{
    public partial class HelpForm : Form
    {
        /*
         * NAME
         *  HelpForm() - default constructor
         * 
         * SYNOPSIS
         *  public HelpForm();
         *      
         * DESCRIPTION
         *  This is the default constructor and initializes required components.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:02am 5/31/2015
         */
        public HelpForm()
        {
            InitializeComponent();
        } /* public HelpForm() */

        /*
         * NAME
         *  HelpOKButton_Click() - closes this form
         * 
         * SYNOPSIS
         *  private void HelpOKButton_Click(object sender, EventArgs e);
         *      sender  -> does nothing
         *      e       -> does nothing
         *      
         * DESCRIPTION
         *  This is the default constructor and initializes required variables.
         * 
         * RETURNS
         *  Nothing
         * 
         * AUTHOR
         *  Murat Zazi
         *  
         * DATE
         *  8:19am 5/31/2015
         */
        private void HelpOKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        } /* private void HelpOKButton_Click(object sender, EventArgs e) */
    }
}
