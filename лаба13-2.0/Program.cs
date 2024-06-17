using System;

using Collections;
using лаба10;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection1 = new MyObservableCollection<string, MusicalInstrument>();
            var collection2 = new MyObservableCollection<string, MusicalInstrument>();

            var journal1 = new Journal();
            var journal2 = new Journal();

            // Подписка журнала 1 на события CollectionCountChanged и CollectionReferenceChanged для первой коллекции
            collection1.CollectionCountChanged += (source, args) => journal1.AddEntry(source, args, "Первая коллекция");
            collection1.CollectionReferenceChanged += (source, args) => journal1.AddEntry(source, args, "Первая коллекция");

            // Подписка журнала 2 только на события CollectionReferenceChanged для обеих коллекций
            collection1.CollectionReferenceChanged += (source, args) => journal2.AddEntry(source, args, "Первая коллекция");
            collection2.CollectionReferenceChanged += (source, args) => journal2.AddEntry(source, args, "Вторая коллекция");

            

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить элемент в первую коллекцию");
                Console.WriteLine("2. Добавить элемент во вторую коллекцию");
                Console.WriteLine("3. Удалить элемент из первой коллекции");
                Console.WriteLine("4. Удалить элемент из второй коллекции");
                Console.WriteLine("5. Изменить элемент в первой коллекции");
                Console.WriteLine("6. Изменить элемент во второй коллекции");
                Console.WriteLine("7. Показать журнал 1");
                Console.WriteLine("8. Показать журнал 2");
                Console.WriteLine("9. Показать первую коллекцию");
                Console.WriteLine("10. Показать вторую коллекцию");
                Console.WriteLine("0. Выход");

                int choice = InputInt(0, 10);
                if (choice == 0) break;

                switch (choice)
                {
                    case 1:
                        AddElementToCollection(collection1);
                        break;
                    case 2:
                        AddElementToCollection(collection2);
                        break;
                    case 3:
                        RemoveElementFromCollection(collection1);
                        break;
                    case 4:
                        RemoveElementFromCollection(collection2);
                        break;
                    case 5:
                        ChangeElementInCollection(collection1);
                        break;
                    case 6:
                        ChangeElementInCollection(collection2);
                        break;
                    case 7:
                        Console.WriteLine("Журнал 1:");
                        journal1.PrintJournal();
                        break;
                    case 8:
                        Console.WriteLine("Журнал 2:");
                        journal2.PrintJournal();
                        break;
                    case 9:
                        Console.WriteLine("Первая коллекция:");
                        DisplayCollection(collection1);
                        break;
                    case 10:
                        Console.WriteLine("Вторая коллекция:");
                        DisplayCollection(collection2);
                        break;
                }
            }
        }

        static void AddElementToCollection(MyObservableCollection<string, MusicalInstrument> collection)
        {
            Console.WriteLine("Выберите тип инструмента для добавления:");
            Console.WriteLine("1. Музыкальный инструент");
            Console.WriteLine("2. Электрогитара");
            Console.WriteLine("3. Пианино");

            int choice = InputInt(1, 3);
            MusicalInstrument instrument = choice switch
            {
                1 => new MusicalInstrument(),
                2 => new ElectricGuitar(),
                3 => new Piano(),
                _ => null
            };

            if (instrument != null)
            {
                instrument.Init();
                collection.Add(instrument.Name, instrument);
                Console.WriteLine("Элемент добавлен.");
            }
        }

        static void RemoveElementFromCollection(MyObservableCollection<string, MusicalInstrument> collection)
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("Коллекция пуста. Удаление невозможно.");
                return;
            }

            Console.WriteLine("Введите имя инструмента для удаления:");
            string name = Console.ReadLine();
            bool removed = collection.Remove(name);
            if (removed)
            {
                Console.WriteLine("Элемент удален.");
            }
            else
            {
                Console.WriteLine("Элемент не найден.");
            }
        }

        static void ChangeElementInCollection(MyObservableCollection<string, MusicalInstrument> collection)
        {
            Console.WriteLine("Введите имя инструмента для изменения:");
            string name = Console.ReadLine();
            if (collection.ContainsKey(name))
            {
                Console.WriteLine("Выберите тип нового инструмента:");
                Console.WriteLine("1. Музыкальный инструент");
                Console.WriteLine("2. Электрогитара");
                Console.WriteLine("3. Пианино");

                int choice = InputInt(1, 3);
                MusicalInstrument newInstrument = choice switch
                {
                    1 => new MusicalInstrument(),
                    2 => new ElectricGuitar(),
                    3 => new Piano(),
                    _ => null
                };

                if (newInstrument != null)
                {
                    newInstrument.Init();
                    collection[name] = newInstrument;
                    Console.WriteLine("Элемент изменен.");
                }
            }
            else
            {
                Console.WriteLine("Элемент не найден.");
            }
        }

        static void DisplayCollection(MyObservableCollection<string, MusicalInstrument> collection)
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("Коллекция пуста.");
            }
            else
            {
                foreach (var item in collection)
                {
                    Console.WriteLine(item.Value.ToString());
                }
            }
        }

        static int InputInt(int min, int max)
        {
            int number;
            bool inputCheck;
            do
            {
                Console.Write("Ввод: ");
                inputCheck = int.TryParse(Console.ReadLine(), out number) && number >= min && number <= max;
                if (!inputCheck) Console.WriteLine($"Ошибка ввода! Введите целое число в пределах от {min} до {max} (включительно)");
            } while (!inputCheck);
            return number;
        }
    }
}
