using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            // lijst voor alle getallen
            var getallen = new List<int>();

            // inlezen getallen
            while (true)
            {
                var regel = reader.ReadLine();
                if (string.IsNullOrEmpty(regel))
                    break;

                getallen.Add(int.Parse(regel));

            }
     
            Console.WriteLine("Opdracht afgerond");
        }
    }
}