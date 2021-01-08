using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_09
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            //using var file = new FileStream("Inputtest.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var getallen = new List<ulong>();
            var preamble = 25;
            var oplossing1 = 0ul;
            var oplossing2 = 0ul;

            Console.WriteLine("Opdracht 9");
            // inlezen getallen
            while (true)
            {
                var regel = reader.ReadLine();
                if (regel == null) break;
                getallen.Add(ulong.Parse(regel));
            }
            Console.WriteLine("De getallen zijn ingelezen.");
            //oplossing deel 1
            for (var i = preamble; i < getallen.Count(); i++)
            {
                var mix = getallen.GetRange(i-preamble, preamble)
                    .ToList()
                    .SelectMany(num => getallen
                        .GetRange(i-preamble, preamble)
                        .ToList(), (a, b) => a+b)
                    .ToHashSet();
                if (!mix.Contains(getallen[i])) oplossing1 = getallen[i];
            }
            Console.WriteLine($"Oplossing 1 is: {oplossing1}");
            // oplossing deel 2
            var rondes = 0;
            var som = 0ul;
            var range = new List<ulong>();
            while(som != oplossing1)
            {
                som = 0ul;
                range = getallen
                    .Skip(rondes)
                    .TakeWhile(item => (som += item) < oplossing1)
                    .ToList();
                rondes++;
            }
            oplossing2 = range.Max() + range.Min();
            Console.WriteLine($"Oplossing 2 is: {oplossing2}");
        }
    }
}
