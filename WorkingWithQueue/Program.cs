using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithQueue
{
    class Program
    {
        const string STAR_WARS_LEGO = "Star Wars: Lego";
        const string CITY_LEGO = "City: Lego";

        static ConcurrentQueue<Item> queue;
        static void Main(string[] args)
        {
            queue = new ConcurrentQueue<Item>
            (
                new List<Item>
                {
                    new Item{ Name = "Batman toy", Price = 29.99m },
                    new Item{ Name = "Superman toy", Price = 35.99m },
                    new Item{ Name = STAR_WARS_LEGO, Price = 23.99m },
                    new Item{ Name = "Toy story toys", Price = 16.5m },
                    new Item{ Name = "Table game: Survavial", Price = 45.6m },
                }
            );

            Extensions.PrintAllElements(queue);

            queue = new ConcurrentQueue<Item>(GenerateList(queue));

            Extensions.PrintAllElements(queue);

            Console.Read();
        }

        private static IEnumerable<Item> GenerateList(ConcurrentQueue<Item> queue)
        {
            for (int i = 0; i < 5; i++)
            {
                queue.TryDequeue(out Item item);
                if (item.Name.Equals(STAR_WARS_LEGO))
                {
                    yield return new Item { Name = CITY_LEGO, Price = item.Price };
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
    internal class Item
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    internal static class Extensions 
    {
        public static void PrintAllElements(this IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("{0} - {1} costs {2} euros", item.Id, item.Name, item.Price);
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat('-', 70)));
        }
    }
}
