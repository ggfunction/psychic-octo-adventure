namespace Clipboard
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Memorandum.Threading;
    using Memorandum.UI;
    using Newtonsoft.Json;

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

            var updateEntries = new Throttle<IDataObject>(
                (data) =>
            {
                var text = data.GetData(DataFormats.Text);
                var entry = new Entry()
                {
                    Content = text.ToString(),
                    LastModified = DateTime.Now,
                };

                var index = this.entries.FindIndex(e => e.Content.Equals(text));
                if (index == 0)
                {
                    return;
                }
                else if (index != -1)
                {
                    this.entries.RemoveAt(index);
                }

                this.entries.Insert(0, entry);
                this.ListBox1.Items.Clear();
                this.ListBox1.Items.AddRange(this.entries.ToArray());
            }, 100);

            this.clipboardListener = new ClipboardListener(this.Handle);
            this.clipboardListener.ClipboardUpdated += (sender, e) =>
            {
                if (NativeMethods.GetForegroundWindow() == this.Handle)
                {
                    return;
                }

                if (e.DataObject.GetDataPresent(DataFormats.Text))
                {
                    updateEntries.Push(e.DataObject);
                }
            };

            this.ListBox1.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.dragStart = new Point(e.X, e.Y);
                }
            };

            this.ListBox1.MouseMove += (sender, e) =>
            {
                if (this.dragStart.HasValue)
                {
                }
            };

            this.ListBox1.MouseUp += (sender, e) =>
            {
                if (this.dragStart.HasValue && e.Button == MouseButtons.Left)
                {
                    this.dragStart = null;
                }

                var index = this.ListBox1.IndexFromPoint(new Point(e.X, e.Y));
                if (index != ListBox.NoMatches)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        var entry = (Entry)this.ListBox1.Items[index];
                        Clipboard.SetText(entry.Content);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                    }
                }
            };

            this.ListBox1.DragOver += (sender, e) =>
            {
            };

            this.ListBox1.DragDrop += (sender, e) =>
            {
            };

            this.Shown += (s, e) => this.LoadEntries();
            this.FormClosed += (s, e) => this.SaveEntries();
        }

        public void LoadEntries()
        {
            try
            {
                var text = System.IO.File.ReadAllText(@"content.json");
                var list = JsonConvert.DeserializeObject<List<Entry>>(
                    text);
                this.entries.Clear();
                this.entries.AddRange(list);
                this.ListBox1.Items.AddRange(this.entries.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SaveEntries()
        {
            try
            {
                var text = JsonConvert.SerializeObject(
                    this.entries,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    });

                System.IO.File.WriteAllText(@"content.json", text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }
    }
}
