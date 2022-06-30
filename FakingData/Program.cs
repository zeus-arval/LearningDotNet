using Faker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakingData
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] persons = GeneratePeople(18).ToArray();

            Array.ForEach(persons, PrintInfo);
            Console.Read();
        }

        private static IEnumerable<Person> GeneratePeople(int count)
        {
            for(int i = 0; i < count; i++)
            {
                yield return new Person
                {
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Address = $"{Faker.Address.StreetAddress()}, {Faker.Address.Country()}",
                    PhoneNumber = Faker.Phone.Number(),
                };
            }
        }


        public static void PrintInfo(Person person)
        {
            Console.WriteLine($"[{person.Id}] Person {person.FirstName} {person.LastName} lives in {person.Address}. Tel: {person.PhoneNumber}");
        }
    }

    public class Person
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
