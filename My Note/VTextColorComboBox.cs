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
 *      TransparentPanel : Panel
 *      
 *  DESCRIPTION:
 *      The purpose of this class is to provide the application with a combo box tool in the toolbox. An
 *      instance of this class/control is used in the options box which will assist the vertical text box
 *      options form in allowing the user to set the color of the rotatable text. Initial code for this
 *      class was taken from a website and slightly modified to accomodate the needs of this application.
 *      Modifications include an update to the number of colors available and the addition of this comment
 *      block. Due to the simplicity of this class and the original author's professionalism towards effective
 *      coding standards, no additional modifications are needed.
 *      A link to the author and site is provided below:
 *      Jonathan Wood, February 11, 2011 
 *      http://www.blackbeltcoder.com/Articles/controls/creating-a-color-picker-with-an-owner-draw-combobox
 *      
 *  STRUCTURE:
 *      This class contains basic (self-explanatory) properties and methods
 */


/* TODO: remove selected text property. 'refresh' comments above, make sure
        that this class is loosely coupled and easily reusable
 */

namespace My_Note
{
    public partial class VTextColorComboBox : ComboBox
    {
        // Data for each color in the list
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

    public VTextColorComboBox()
    {
        InitializeComponent();

        //DropDownStyle = ComboBoxStyle.DropDownList;
        DrawMode = DrawMode.OwnerDrawFixed;
        DrawItem += OnDrawItem;
    }

    // Populate control with standard colors
    public void AddStandardColors()
    {
        Items.Clear();
        Items.Add(new ColorInfo("Black", Color.Black));
        Items.Add(new ColorInfo("Blue", Color.Blue));
        Items.Add(new ColorInfo("Lime", Color.Lime));
        Items.Add(new ColorInfo("Cyan", Color.Cyan));
        Items.Add(new ColorInfo("Red", Color.Red));
        Items.Add(new ColorInfo("Fuchsia", Color.Fuchsia));
        Items.Add(new ColorInfo("Yellow", Color.Yellow));
        Items.Add(new ColorInfo("White", Color.White));
        Items.Add(new ColorInfo("Navy", Color.Navy));
        Items.Add(new ColorInfo("Green", Color.Green));
        Items.Add(new ColorInfo("Teal", Color.Teal));
        Items.Add(new ColorInfo("Maroon", Color.Maroon));
        Items.Add(new ColorInfo("Purple", Color.Purple));
        Items.Add(new ColorInfo("Olive", Color.Olive));
        Items.Add(new ColorInfo("Gray", Color.Gray));
    }

    // Draw list item
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
    /// Gets the text of the selected item, or sets the selection to
    /// the item with the specified text.
    /// </summary>
    public new string SelectedText
    {
        get
        {
            if (SelectedIndex >= 0)
                return SelectedItem.Text;
            return String.Empty;
        }
        set
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (((ColorInfo)Items[i]).Text == value)
                {
                    SelectedIndex = i;
                    break;
                }
            }
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
            return Color.White;
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

    }
}
