using lab12dot7;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeType { get; set; }
        public object ChangedItem { get; set; }

        public CollectionHandlerEventArgs(string changeType, object changedItem)
        {
            ChangeType = changeType;
            ChangedItem = changedItem;
        }
    }

    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

    public class MyObservableCollection<TKey, TValue> : MyCollection<TKey, TValue>
    {
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        protected virtual void OnCollectionCountChanged(string changeType, object changedItem)
        {
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(changeType, changedItem));
        }

        protected virtual void OnCollectionReferenceChanged(string changeType, object changedItem)
        {
            CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(changeType, changedItem));
        }

        public new void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }

            base.Add(key, value);
            OnCollectionCountChanged("Added", value);
        }

        public new bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                TValue value = base[key];
                bool removed = base.Remove(key);
                if (removed)
                {
                    OnCollectionCountChanged("Removed", value);
                }
                return removed;
            }
            return false;
        }

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                if (ContainsKey(key))
                {
                    TValue oldValue = base[key];
                    base[key] = value;
                    OnCollectionReferenceChanged("Replaced", value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }
    }
}
