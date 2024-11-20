namespace Clipboard
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected ListBox ListBox1 { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = Application.ProductName;
            this.MaximizeBox = false;

            var font = new Font(this.Font.Name, 15);

            this.ListBox1 = new BufferedListBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                ClientSize = this.ClientSize,
                DataSource = this.dataList.GetBindingSource(),
                DisplayMember = "Content",
                DrawMode = DrawMode.OwnerDrawFixed,
                Font = font,
                ItemHeight = font.Height,
                Location = new Point(0, 0),
            };

            this.Controls.Add(this.ListBox1);
        }
    }
}
