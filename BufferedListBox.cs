namespace Clipboard
{
    using System.Windows.Forms;

    public class BufferedListBox : ListBox
    {
        public BufferedListBox()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
    }
}