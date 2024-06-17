using System;
using System.Collections.Generic;

namespace lab12dot7
{
    public class MyKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public MyKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public class HashTable<TKey, TValue>
    {
        private const int DefaultCapacity = 10;
        private MyKeyValuePair<TKey, TValue>?[] _items;
        private int _count;

        public HashTable()
        {
            _items = new MyKeyValuePair<TKey, TValue>?[DefaultCapacity];
            _count = 0;
        }

        private int GetPrimaryIndex(TKey key, int length)
        {
            int hash = key.GetHashCode();
            return Math.Abs(hash % length);
        }

        private int GetSecondaryIndex(TKey key, int length)
        {
            int hash = key.GetHashCode();
            return Math.Abs((hash / length) % length) | 1; // гарантируем, что шаг не равен 0
        }

        private int GetPrimaryIndex(TKey key)
        {
            return GetPrimaryIndex(key, _items.Length);
        }

        private int GetSecondaryIndex(TKey key)
        {
            return GetSecondaryIndex(key, _items.Length);
        }

        private int GetInsertIndex(TKey key)
        {
            int index = GetPrimaryIndex(key);
            int step = GetSecondaryIndex(key);
            while (_items[index] != null && !_items[index].Key.Equals(key))
            {
                index = (index + step) % _items.Length;
            }
            return index;
        }

        public void Add(TKey key, TValue value)
        {
            if (_count >= _items.Length * 0.75)
            {
                Resize();
            }

            int index = GetInsertIndex(key);

            if (_items[index] == null)
            {
                _items[index] = new MyKeyValuePair<TKey, TValue>(key, value);
                _count++;
            }
            else
            {
                _items[index].Value = value;
            }
        }

        public TValue Find(TKey key)
        {
            int index = GetPrimaryIndex(key);
            int step = GetSecondaryIndex(key);
            int startIndex = index;
            while (_items[index] != null)
            {
                if (_items[index].Key.Equals(key))
                {
                    return _items[index].Value;
                }
                index = (index + step) % _items.Length;
                if (index == startIndex)
                {
                    break;
                }
            }
            throw new KeyNotFoundException("Key not found");
        }

        public bool Remove(TKey key)
        {
            int index = GetPrimaryIndex(key);
            int step = GetSecondaryIndex(key);
            int startIndex = index;

            while (_items[index] != null)
            {
                if (_items[index].Key.Equals(key))
                {
                    _items[index] = null;
                    _count--;

                    int nextIndex = (index + step) % _items.Length;
                    while (_items[nextIndex] != null)
                    {
                        var tempItem = _items[nextIndex];
                        _items[nextIndex] = null;
                        int newIndex = GetInsertIndex(tempItem.Key);
                        _items[newIndex] = tempItem;

                        nextIndex = (nextIndex + step) % _items.Length;
                    }
                    return true;
                }
                index = (index + step) % _items.Length;
                if (index == startIndex)
                {
                    break;
                }
            }
            return false;
        }

        private void Resize()
        {
            var newSize = _items.Length * 2;
            var newItems = new MyKeyValuePair<TKey, TValue>?[newSize];
            foreach (var item in _items)
            {
                if (item != null)
                {
                    int index = GetPrimaryIndex(item.Key, newSize);
                    int step = GetSecondaryIndex(item.Key, newSize);
                    while (newItems[index] != null)
                    {
                        index = (index + step) % newSize;
                    }
                    newItems[index] = item;
                }
            }
            _items = newItems;
        }

        public int GetCount()
        {
            return _count;
        }

        public void Print()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                if (item != null)
                {
                    int calculatedIndex = GetPrimaryIndex(item.Key);
                    Console.WriteLine($"Индекс: {i}, Ключ: {item.Key}, Значение: {item.Value}, Расчетный индекс: {calculatedIndex}");
                }
            }
            Console.WriteLine($"Количествои элементов {_count}, размер массива {_items.Length}");
        }

        
        public void Clear()
        {
            _items = new MyKeyValuePair<TKey, TValue>?[DefaultCapacity];
            _count = 0;
        }
    }
}