using System;
using System.Collections.Generic;

namespace Collections
{
    public class JournalEntry
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ItemData { get; set; }

        public JournalEntry(string collectionName, string changeType, string itemData)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ItemData = itemData;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, ChangeType: {ChangeType}, ItemData: {ItemData}";
        }
    }

    public class Journal
    {
        private List<JournalEntry> entries = new List<JournalEntry>();

        public List<JournalEntry> Entries
        {
            get { return entries; }
            set { entries = value; }
        }

        public void AddEntry(object source, CollectionHandlerEventArgs args, string collectionName)
        {
            string changeType = args.ChangeType;
            string itemData = args.ChangedItem.ToString();

            JournalEntry entry = new JournalEntry(collectionName, changeType, itemData);
            entries.Add(entry);
        }

        public void PrintJournal()
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry.ToString());
            }
        }
    }
}
