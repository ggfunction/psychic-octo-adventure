namespace Clipboard
{
    using System;

    public class Entry
    {
        public string Content { get; set; }

        public int Count { get; set; }

        public DateTime LastModified { get; set; }

        public bool Pinned { get; set; }
    }
}