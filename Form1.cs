namespace Clipboard
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Memorandum.Threading;
    using Memorandum.UI;

    public partial class Form1 : Form
    {
        private readonly ClipboardListener clipboardListener;

        private readonly DataList dataList = new DataList();

        private readonly object lockObject = new object();

        private Point? dragStart;

        public Form1()
        {
            this.InitializeComponent();

            var updateEntries = new Throttle<IDataObject>(
                (data) =>
            {
                var text = data.GetData(DataFormats.Text);
                var entry = new Entry()
                {
                    Content = text.ToString(),
                    LastModified = DateTime.Now,
                };

                if (this.dataList.Add(entry))
                {
                    if (this.ListBox1.SelectedIndex != ListBox.NoMatches)
                    {
                        this.ListBox1.SelectedIndex = 0;
                    }
                }
            }, 100);

            this.clipboardListener = new ClipboardListener(this.Handle);
            this.clipboardListener.ClipboardUpdated += (s, e) =>
            {
                if (NativeMethods.GetForegroundWindow() == this.Handle)
                {
                    return;
                }

                this.ListBox1.SelectedIndex = ListBox.NoMatches;

                if (e.DataObject.GetDataPresent(DataFormats.Text))
                {
                    updateEntries.Push(e.DataObject);
                }
            };

            this.ListBox1.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.dragStart = new Point(e.X, e.Y);
                }
            };

            this.ListBox1.MouseMove += (s, e) =>
            {
                if (this.dragStart.HasValue)
                {
                }
            };

            this.ListBox1.MouseUp += (s, e) =>
            {
                if (this.dragStart.HasValue && e.Button == MouseButtons.Left)
                {
                    this.dragStart = null;
                }

                var index = this.ListBox1.IndexFromPoint(new Point(e.X, e.Y));
                if (index != ListBox.NoMatches)
                {
                    var entry = (Entry)this.ListBox1.Items[index];
                    if (e.Button == MouseButtons.Left)
                    {
                        try
                        {
                            Clipboard.SetText(entry.Content);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        entry.Pinned = !entry.Pinned;
                        this.ListBox1.Invalidate();
                    }
                }
            };

            this.ListBox1.DragOver += (s, e) =>
            {
            };

            this.ListBox1.DragDrop += (s, e) =>
            {
            };

            this.ListBox1.DrawItem += (s, e) =>
            {
                if (e.Index == ListBox.NoMatches)
                {
                    return;
                }

                var item = (Entry)this.ListBox1.Items[e.Index];
                var content = item.Content.ToString();
                var font = item.Pinned ? new Font(this.ListBox1.Font, FontStyle.Bold) : this.ListBox1.Font;
                var textColor = SystemBrushes.ControlText;

                e.DrawBackground();

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    textColor = SystemBrushes.HighlightText;
                }

                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    e.DrawFocusRectangle();
                }

                e.Graphics.DrawString(content, font, textColor, e.Bounds);
            };

            this.ListBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                {
                    var index = this.ListBox1.SelectedIndex;
                    if (index != ListBox.NoMatches)
                    {
                        this.dataList.RemoveAt(index);
                    }
                }
            };

            this.Shown += (s, e) => this.dataList.Load();
            this.FormClosed += (s, e) => this.dataList.Save();
        }

        private class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }
    }
}
