namespace Clipboard
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public class DataList
    {
        private readonly BindingSource bindingSource = new BindingSource();

        private readonly BindingList<Entry> entries = new BindingList<Entry>();

        public DataList()
        {
            this.bindingSource.DataSource = this.entries;
        }

        public bool Add(Entry entry)
        {
            var temporary = new List<Entry>(this.entries);
            var index = temporary.FindIndex(e => e.Content.Equals(entry.Content));

            if (index == 0)
            {
                return false;
            }
            else if (index != -1)
            {
                temporary.RemoveAt(index);
            }

            temporary.Insert(0, entry);
            try
            {
                this.entries.RaiseListChangedEvents = false;
                this.entries.Clear();
                temporary.ForEach(e => this.entries.Add(e));
            }
            finally
            {
                this.entries.RaiseListChangedEvents = true;
                this.entries.ResetBindings();
            }

            return true;
        }

        public BindingSource GetBindingSource()
        {
            return this.bindingSource;
        }

        public void Load()
        {
            try
            {
                var text = System.IO.File.ReadAllText(@"content.json");
                var list = JsonConvert.DeserializeObject<List<Entry>>(
                    text);
                this.entries.RaiseListChangedEvents = false;
                this.entries.Clear();
                list.ForEach(e => this.entries.Add(e));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                this.entries.RaiseListChangedEvents = true;
                this.entries.ResetBindings();
            }
        }

        public void RemoveAt(int index)
        {
            this.entries.RemoveAt(index);
        }

        public void Save()
        {
            try
            {
                var list = new List<Entry>(this.entries);
                var text = JsonConvert.SerializeObject(
                    list,
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

        public void Swap(int indexA, int indexB)
        {
            this.entries.Swap(indexA, indexB);
        }
    }
}
