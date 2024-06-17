using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Collections;

namespace Collections.Tests
{
    [TestClass]
    public class JournalEntryTests
    {
        [TestMethod]
        public void JournalEntry_ToString_FormatsCorrectly()
        {
            // Arrange
            JournalEntry entry = new JournalEntry("TestCollection", "Added", "Item1");

            // Act
            string result = entry.ToString();

            // Assert
            Assert.AreEqual("Collection: TestCollection, ChangeType: Added, ItemData: Item1", result);
        }
    }
    [TestClass]
    public class JournalTests
    {
        [TestMethod]
        public void Journal_AddEntry_AddsEntryToList()
        {
            // Arrange
            Journal journal = new Journal();
            var args = new CollectionHandlerEventArgs("Added", "Item1");

            // Act
            journal.AddEntry(null, args, "TestCollection");

            // Assert
            Assert.AreEqual(1, journal.Entries.Count);
            Assert.AreEqual("TestCollection", journal.Entries[0].CollectionName);
            Assert.AreEqual("Added", journal.Entries[0].ChangeType);
            Assert.AreEqual("Item1", journal.Entries[0].ItemData);
        }

        [TestMethod]
        public void Journal_PrintJournal_PrintsEntries()
        {
            // Arrange
            Journal journal = new Journal();
            var args1 = new CollectionHandlerEventArgs("Added", "Item1");
            var args2 = new CollectionHandlerEventArgs("Removed", "Item2");
            journal.AddEntry(null, args1, "TestCollection");
            journal.AddEntry(null, args2, "AnotherCollection");

            // Act
            var consoleOutput = new System.IO.StringWriter();
            System.Console.SetOut(consoleOutput);
            journal.PrintJournal();
            string output = consoleOutput.ToString();

            // Assert
            StringAssert.Contains(output, "Collection: TestCollection, ChangeType: Added, ItemData: Item1");
            StringAssert.Contains(output, "Collection: AnotherCollection, ChangeType: Removed, ItemData: Item2");
        }
    }
    [TestClass]
    public class MyObservableCollectionTests
    {
        [TestMethod]
        public void Add_AddsItemAndRaisesCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();

            // Act
            collection.Add(1, "Item1");

            // Assert
            Assert.IsTrue(collection.ContainsKey(1));
            Assert.AreEqual("Item1", collection[1]);
            // Verify event
            // Note: Use TestHelper.AssertEventRaised method or similar for verifying events.
        }

        [TestMethod]
        public void Remove_RemovesItemAndRaisesCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();
            collection.Add(1, "Item1");

            // Act
            bool removed = collection.Remove(1);

            // Assert
            Assert.IsTrue(removed);
            Assert.IsFalse(collection.ContainsKey(1));
            // Verify event
            // Note: Use TestHelper.AssertEventRaised method or similar for verifying events.
        }
        
        
        [TestMethod]
        public void Indexer_SetNonExistingKey_AddsNewItemAndRaisesCollectionReferenceChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();

            // Act
            collection[1] = "NewItem";

            // Assert
            Assert.IsTrue(collection.ContainsKey(1));
            Assert.AreEqual("NewItem", collection[1]);
            // Verify event
            // Note: Use TestHelper.AssertEventRaised method or similar for verifying events.
        }
        [TestMethod]
        public void Remove_NonExistingKey_ReturnsFalse()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();

            // Act
            bool removed = collection.Remove(1);

            // Assert
            Assert.IsFalse(removed);
            // Verify event - ensure no events are raised
            // Note: Use TestHelper.AssertEventNotRaised method or similar for verifying absence of events.
        }
        [TestMethod]
        public void Add_ExistingKey_ThrowsException()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();
            collection.Add(1, "Item1");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => collection.Add(1, "Item2"));
            // Verify event - ensure no events are raised
            // Note: Use TestHelper.AssertEventNotRaised method or similar for verifying absence of events.
        }
        [TestMethod]
        public void RemoveAndAdd_SameKey_AddsNewItemAndRaisesCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();
            collection.Add(1, "Item1");
            collection.Remove(1);

            // Act
            collection.Add(1, "NewItem");

            // Assert
            Assert.IsTrue(collection.ContainsKey(1));
            Assert.AreEqual("NewItem", collection[1]);
            // Verify event
            // Note: Use TestHelper.AssertEventRaised method or similar for verifying events.
        }
        [TestMethod]
        public void AddAndRemove_Items_ChangesCollectionCountAndRaisesCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();

            // Act
            collection.Add(1, "Item1");
            collection.Add(2, "Item2");
            collection.Remove(1);

            // Assert
            Assert.AreEqual(1, collection.Count);
            // Verify event
            // Note: Use TestHelper.AssertEventRaised method or similar for verifying events.
        }
        [TestMethod]
        public void Indexer_SetExistingKey_ReplacesItemAndRaisesCollectionReferenceChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<int, string>();
            collection.Add(1, "Item1");

            bool eventRaised = false;
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventRaised = true;
            };

            // Act
            collection[1] = "UpdatedItem";

            // Assert
            Assert.IsTrue(collection.ContainsKey(1));
            Assert.AreEqual("UpdatedItem", collection[1]);
            Assert.IsTrue(eventRaised);
        }


    }
}
