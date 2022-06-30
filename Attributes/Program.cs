using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    class Program
    {
        static void Main(string[] args)
        {
            Person tom = new Person("Tom", 37);
            Person mark = new Person("Mark", 15);

            Validate(tom);
            Validate(mark);


            Console.Read();
        }

        static void Validate(Person person)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(person);
            if (!Validator.TryValidateObject(person, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            else
                Console.WriteLine("User's validation succeeded");
            Console.WriteLine();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        
        [AgeValidation]
        public int Age { get; set; }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AgeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int age && age < 18)
            {
                ErrorMessage = "Age must be higher than 18";
                return false;
            }
            return true;
        }
    }
}
