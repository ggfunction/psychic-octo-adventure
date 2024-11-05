namespace Clipboard
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Memorandum.UI;

    public partial class Form1 : Form
    {
        private readonly ClipboardListener clipboardListener;

        private readonly List<Entry> entries = new List<Entry>();

        private readonly List<Entry> pinnedEntries = new List<Entry>();

        private readonly object lockObject = new object();

        private Point? dragStart;

        public Form1()
        {
            this.InitializeComponent();

            this.clipboardListener = new ClipboardListener(this.Handle);
            this.clipboardListener.ClipboardUpdated += (sender, e) =>
            {
                if (NativeMethods.GetForegroundWindow() == this.Handle)
                {
                    return;
                }

                if (e.DataObject.GetDataPresent(DataFormats.Text))
                {
                    var text = e.DataObject.GetData(DataFormats.Text);
                    this.ListBox1.Items.Insert(0, new Entry()
                    {
                        Content = text.ToString(),
                        LastModified = DateTime.Now,
                    });
                }
            };

            this.ListBox1.MouseDown += (sender, e) =>
            {
                this.dragStart = new Point(e.X, e.Y);
            };

            this.ListBox1.MouseMove += (sender, e) =>
            {
                if (this.dragStart.HasValue)
                {
                }
            };

            this.ListBox1.MouseUp += (sender, e) =>
            {
                if (this.dragStart.HasValue)
                {
                    this.dragStart = null;
                }

                var index = this.ListBox1.IndexFromPoint(new Point(e.X, e.Y));
                if (index != ListBox.NoMatches)
                {
                    var entry = (Entry)this.ListBox1.Items[index];
                    Clipboard.SetText(entry.Content);
                }
            };

            this.ListBox1.DragOver += (sender, e) =>
            {
            };

            this.ListBox1.DragDrop += (sender, e) =>
            {
            };
        }

        private class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }
    }
}
