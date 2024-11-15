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

            var fontName = this.Font.Name;

            this.ListBox1 = new ListBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                DisplayMember = "Content",
                DataSource = this.dataList.GetBindingSource(),
                Font = new Font(fontName, 15),
                Location = new Point(0, 0),
                ClientSize = this.ClientSize,
                //// DrawMode = DrawMode.OwnerDrawFixed,
            };

            this.Controls.Add(this.ListBox1);
        }
    }
}
