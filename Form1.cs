namespace Clipboard
{
    using System;
    using System.Windows.Forms;
    using Memorandum.UI;

    public partial class Form1 : Form
    {
        private readonly ClipboardListener clipboardListener;

        public Form1()
        {
            this.InitializeComponent();

            this.clipboardListener = new ClipboardListener(this.Handle);
            this.clipboardListener.ClipboardUpdated += (sender, e) =>
            {
                if (e.DataObject.GetDataPresent(DataFormats.Text))
                {
                    var text = e.DataObject.GetData(DataFormats.Text);
                    this.ListBox1.Items.Insert(0, text);
                }
            };

            this.ListBox1.Click += (sender, e) =>
            {
                var index = (sender as ListBox).SelectedIndex;
                if (index != -1)
                {
                    if (NativeMethods.GetForegroundWindow() != this.Handle)
                    {
                        Clipboard.SetText((string)(sender as ListBox).Items[index]);
                    }
                }
            };
        }

        private class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }
    }
}
