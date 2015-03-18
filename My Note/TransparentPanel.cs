using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// LOOK back at the log and add the insert date of this class
namespace My_Note
{
    class TransparentPanel : Panel
    {
        public struct lineSpaceForFont
        {
            public const float MICROSOFT_SANS_SERIF = 20;
        }
        private float thisWidth;
        private float thisHeight;
        private float lineBegin;
        private float lineEnd;
        private float nextLine;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            thisWidth = this.Size.Width;
            thisHeight = this.Size.Height;
            lineBegin = 5;
            lineEnd = thisWidth - 10;
            nextLine = 55;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // This draws the shapes before the lines below
            Graphics g = e.Graphics;
            Pen bluePen = new Pen(Color.Blue);
            for (int i = 0; i < 30; i++)
            {
                e.Graphics.DrawLine(bluePen, lineBegin, nextLine, lineEnd, nextLine);
                nextLine += lineSpaceForFont.MICROSOFT_SANS_SERIF;
            }

            Pen redPen = new Pen(Color.Red);
            e.Graphics.DrawLine(redPen, 35, 5, 35, thisHeight - 5);
        }
    }
}