using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace QueueLearning
{
    class Program
    {
        static List<Customer> customers = new List<Customer>
        {
            new Customer{ FirstName = "Cailin", LastName = "Alford", StateOfResidence = "GA", Purchases = new List<string>{ "Protein", "Whey", "BCAA" }, MoneySpent = 45.99m },
            new Customer{ FirstName = "Theodore", LastName = "Brock", StateOfResidence = "AR", Purchases = new List<string>{ "1", "2", "3" }, MoneySpent = 15.0m },
            new Customer{ FirstName = "Adena", LastName = "Jenkins", StateOfResidence = "GA", Purchases = new List<string>{ }, MoneySpent = 19.2m},
            new Customer{ FirstName = "Jerry", LastName = "Gill", StateOfResidence = "MI", Purchases = new List<string>{ }, MoneySpent = 20.0m },
        };

        static List<Distributor> distributors = new List<Distributor>
        {
            new Distributor{ Name = "Scaboo", StateOfBusiness = "GA"},
            new Distributor{ Name = "Jabbersphere", StateOfBusiness = "TX"},
            new Distributor{ Name = "Yankijo", StateOfBusiness = "GA"},
            new Distributor{ Name = "Vidoo", StateOfBusiness = "AR"},
        };

        static void Main(string[] args)
        {
            var stateQuery =
                from cust in customers
                group cust by cust.StateOfResidence;

            var matchupQuery =
                from c in customers
                join d in distributors on c.StateOfResidence equals d.StateOfBusiness
                orderby c.StateOfResidence
                select new { CustomerName = c.LastName, DistName = d.Name, State = c.StateOfResidence };

            var matchupGroupedQuery =
                from c in customers
                join d in distributors on c.StateOfResidence equals d.StateOfBusiness into matches
                select new { CustomerName = c.LastName, DistributorName = matches.Select(dist => dist.Name)};

            foreach (var group in stateQuery)
            {
                Console.WriteLine(group.Key);
                foreach(var customer in group)
                {
                    Console.WriteLine("\t{0} {1}", customer.FirstName, customer.LastName);
                }    
            }
            Console.Clear();

            foreach(var match in matchupQuery)
            {
                Console.WriteLine("{0} {1} | {2}", match.CustomerName, match.DistName, match.State);
            }
            Console.Clear();

            foreach(var cd in matchupGroupedQuery)
            {
                Console.WriteLine("{0}", cd.CustomerName);
                foreach(var d in cd.DistributorName)
                {
                    Console.WriteLine("\t{0}", d);
                }
                Console.WriteLine();
            }

            Console.Read();
        }
    }

    public class Customer
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string StateOfResidence { get; set; }
        public decimal MoneySpent { get; set; }
        public List<string> Purchases { get; set; }
    }

    public class Distributor
    {
        public string Name { get; set; }
        public string StateOfBusiness { get; set; }
    }
}
