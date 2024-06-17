using System;
using System.Collections;
using System.Collections.Generic;

namespace lab12dot7
{
    

    public class MyCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        private const int DefaultCapacity = 10;
        private MyKeyValuePair<TKey, TValue>?[] _items;
        private int _count;

        // Конструктор для создания пустой коллекции
        public MyCollection()
        {
            _items = new MyKeyValuePair<TKey, TValue>?[DefaultCapacity];
            _count = 0;
        }

        // Конструктор для создания коллекции из length элементов, сформированных с помощью ДСЧ
        public MyCollection(int length)
        {
            _items = new MyKeyValuePair<TKey, TValue>?[length];
            _count = 0;
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                TKey key = GetRandomKey();
                TValue value = default;
                Add(key, value);
            }
        }

        // Конструктор для создания коллекции, которая инициализируется элементами и емкостью коллекции
        public MyCollection(MyCollection<TKey, TValue> c)
        {
            _items = new MyKeyValuePair<TKey, TValue>?[c._items.Length];
            _count = c._count;
            foreach (var item in c)
            {
                Add(item.Key, item.Value);
            }
        }

        // Метод для генерации случайного ключа
        private TKey GetRandomKey()
        {
            Random rand = new Random();
            return (TKey)Convert.ChangeType(rand.Next().ToString(), typeof(TKey));
        }

        // Метод добавления элемента в коллекцию
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

        // Метод удаления элемента из коллекции по ключу
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

        // Метод получения значения по ключу
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetPrimaryIndex(key);
            int step = GetSecondaryIndex(key);
            int startIndex = index;

            while (_items[index] != null)
            {
                if (_items[index].Key.Equals(key))
                {
                    value = _items[index].Value;
                    return true;
                }
                index = (index + step) % _items.Length;
                if (index == startIndex)
                {
                    break;
                }
            }
            value = default;
            return false;
        }

        // Метод получения значения по ключу
        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                {
                    return value;
                }
                throw new KeyNotFoundException("Key not found");
            }
            set
            {
                Add(key, value);
            }
        }

        // Метод получения индекса для элемента по ключу
        private int GetPrimaryIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode() % _items.Length);
        }

        private int GetSecondaryIndex(TKey key)
        {
            int hash = key.GetHashCode();
            return Math.Abs((hash / _items.Length) % _items.Length) | 1; // гарантируем, что шаг не равен 0
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

        private void Resize()
        {
            var newSize = _items.Length * 2;
            var newItems = new MyKeyValuePair<TKey, TValue>?[newSize];
            foreach (var item in _items)
            {
                if (item != null)
                {
                    int index = GetPrimaryIndex(item.Key);
                    int step = GetSecondaryIndex(item.Key);
                    while (newItems[index] != null)
                    {
                        index = (index + step) % newSize;
                    }
                    newItems[index] = item;
                }
            }
            _items = newItems;
        }

        // Реализация интерфейса IEnumerable<KeyValuePair<TKey, TValue>>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    yield return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                }
            }
        }

        // Реализация интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Реализация интерфейса ICollection<KeyValuePair<TKey, TValue>>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _items = new MyKeyValuePair<TKey, TValue>?[DefaultCapacity];
            _count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item) && Remove(item.Key);
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        // Реализация интерфейса IDictionary<TKey, TValue>
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>();
                foreach (var item in _items)
                {
                    if (item != null)
                    {
                        keys.Add(item.Key);
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                foreach (var item in _items)
                {
                    if (item != null)
                    {
                        values.Add(item.Value);
                    }
                }
                return values;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        public bool Remove(TKey key, out TValue value)
        {
            if (TryGetValue(key, out value))
            {
                return Remove(key);
            }
            return false;
        }
    }
}
