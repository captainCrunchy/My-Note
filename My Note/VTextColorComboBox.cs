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
 *      VTextColorComboBox : ComboBox
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to provide the application with a custom ComboBox tool in the toolbox.
 *      An instance of this class/control is used in VertTextOptionsForm to allow the user to set the color
 *      of the rotatable text. Initial code for this class was taken from a website and slightly modified
 *      to accomodate the needs of this application. The modifications that were made to this class include
 *      the removal of unnecessary features. Due to the simplicity of this class and effective coding standards
 *      by the original author, further modifications were not necessary.
 *      A link to the author and site is provided below:
 *      Jonathan Wood, February 11, 2011 
 *      http://www.blackbeltcoder.com/Articles/controls/creating-a-color-picker-with-an-owner-draw-combobox
 *      
 *  CODE STRUCTURE:
 *      - Properties
 *      - Constructors
 *      - Regular methods
 */

namespace My_Note
{
    public partial class VTextColorComboBox : ComboBox
    {
        /// <summary>
        /// Data for each color in the list
        /// </summary>
        public class ColorInfo
        {
            public string Text { get; set; }
            public Color Color { get; set; }

            public ColorInfo(string text, Color color)
            {
                Text = text;
                Color = color;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item.
        /// </summary>
        public new ColorInfo SelectedItem
        {
            get
            {
                return (ColorInfo)base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the value of the selected item, or sets the selection to
        /// the item with the specified value.
        /// </summary>
        public new Color SelectedValue
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Color;
                return Color.Black; // changed from white
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((ColorInfo)Items[i]).Color == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /*
         * NAME
         *  VTextColorComboBox() - default constructor
         *  
         * SYNOPSIS
         *  public VTextColorComboBox();
         * 
         * DESCRIPTION
         *  This is the default consructor which initializes several member variables.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Jonathan Wood
         *  
         * DATE
         *  February 11, 2011
         */
        public VTextColorComboBox()
        {
            InitializeComponent();
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;
        } /* VTextColorComboBox() */

        /*
         * NAME
         *  AddStandardColors() - adds colors to combo box
         *  
         * SYNOPSIS
         *  public void AddStandardColors();
         * 
         * DESCRIPTION
         *  Populate control with standard colors.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Jonathan Wood
         *  
         * DATE
         *  February 11, 2011
         */
        public void AddStandardColors()
        {
            Items.Clear();
            Items.Add(new ColorInfo("Black", Color.Black));
            Items.Add(new ColorInfo("Gray", Color.Gray));
            Items.Add(new ColorInfo("Red", Color.Red));
            Items.Add(new ColorInfo("Orange", Color.Orange));
            Items.Add(new ColorInfo("Yellow", Color.Yellow));
            Items.Add(new ColorInfo("White", Color.White));
            Items.Add(new ColorInfo("Green", Color.Green));
            Items.Add(new ColorInfo("Teal", Color.Teal));
            Items.Add(new ColorInfo("Blue", Color.Blue));
            Items.Add(new ColorInfo("Purple", Color.Purple));
        } /* public void AddStandardColors() */

        /*
         * NAME
         *  OnDrawItem() draws list item
         *  
         * SYNOPSIS
         *  protected void OnDrawItem(object sender, DrawItemEventArgs e);
         *      sender  -> does nothing
         *      e       -> provides data for the drawing of colored boxes
         * 
         * DESCRIPTION
         *  Draw list item.
         *  
         * RETURNS
         *  Nothing
         *  
         * AUTHOR
         *  Jonathan Wood
         *  
         * DATE
         *  February 11, 2011
         */
        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                // Get this color
                ColorInfo color = (ColorInfo)Items[e.Index];

                // Fill background
                e.DrawBackground();

                // Draw color box
                Rectangle rect = new Rectangle();
                rect.X = e.Bounds.X + 2;
                rect.Y = e.Bounds.Y + 2;
                rect.Width = 18;
                rect.Height = e.Bounds.Height - 5;
                e.Graphics.FillRectangle(new SolidBrush(color.Color), rect);
                e.Graphics.DrawRectangle(SystemPens.WindowText, rect);

                // Write color name
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                    brush = SystemBrushes.HighlightText;
                else
                    brush = SystemBrushes.WindowText;
                e.Graphics.DrawString(color.Text, Font, brush,
                    e.Bounds.X + rect.X + rect.Width + 2,
                    e.Bounds.Y + ((e.Bounds.Height - Font.Height) / 2));

                // Draw the focus rectangle if appropriate
                if ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None)
                    e.DrawFocusRectangle();
            }
        } /* protected void OnDrawItem(object sender, DrawItemEventArgs e) */
    }
}
