using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Dog> dogs = new List<Dog>();
            dogs.Add(new Dog("Fido"));
            dogs.Add(new Dog("Bob"));
            dogs.Add(new Dog("Adam"));
            dogs.Sort();
            foreach (Dog dog in dogs)
                Console.WriteLine(dog.Describe());
            Console.ReadKey();
        }
    }

    interface IAnimal
    {
        string Describe();

        string Name
        {
            get;
            set;
        }
    }

    class Dog : IAnimal, IComparable
    {
        private string name;

        public Dog(string name)
        {
            this.Name = name;
        }

        public string Describe()
        {
            return "Hello, I'm a dog and my name is " + this.Name;
        }

        public int CompareTo(object obj)
        {
            if (obj is IAnimal)
                return this.Name.CompareTo((obj as IAnimal).Name);
            return 0;
        }

        // of:
        // public string Name { get; set; }
        // dan is ook
        // private string name;
        // niet nodig

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }


}
