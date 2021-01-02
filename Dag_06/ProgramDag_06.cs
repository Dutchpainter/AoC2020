using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_06
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            
            var groepen = new List<List<string>>();
            Console.WriteLine("groepen inlezen");
            // regels per group inlezen
            string regel;
            while (true)
            {
                var groep = new List<string>();
                regel = reader.ReadLine();
                while (true)
                {
                    if (string.IsNullOrEmpty(regel))
                        break;
                    groep.Add(regel);
                    regel = reader.ReadLine();
                }
                groepen.Add(groep);
                if (regel == null) break;
            }
            Console.WriteLine("groepen ingelezen");

            // berekenen deel 1: de som van alle vragen die met ja beantwoord zijn per groep
            var oplossingDeel1 = groepen
                .Select(groep => groep
                    .Aggregate((i, j) => i + j)
                    .Distinct()
                    .ToList()
                    .Count())
                .Aggregate(0,(a, b) => (a + b));
            Console.WriteLine($"De oplossing voor deel 1: {oplossingDeel1}");


            //berekenen deel 1 optie 2:
            var oplossingDeel12 = groepen
                .Select(groep => groep
                    .Aggregate(string.Empty.AsEnumerable(), (a, b) => a.Union(b))
                    .ToList())
                .ToList()
                .Aggregate(0, (a, b) => a + b.Count);
            Console.WriteLine($"De oplossing voor deel 2: {oplossingDeel12}");


            //berekenen deel 2:
            var resultaat = new List<int>();
            foreach (var groep in groepen)
            {
                var tussenstap = groep[0].ToList();
                //Console.WriteLine($"eerste waarde: {both}");
                for (var i = 1; i <= groep.Count - 1; i++)
                {
                    // doe iets
                    tussenstap = tussenstap.Intersect(groep[i]).ToList();
                }
                resultaat.Add(tussenstap.ToList().Count()); 
            }
            var oplossingDeel2 = resultaat.Aggregate(0,(a, b) => (a + b));
            Console.WriteLine($"De oplossing voor deel 2: {oplossingDeel2}");

            //berekenen deel 2 optie 2:
            var oplossingDeel22 = groepen
                .Select(groep => groep
                    .Aggregate("abcdefghijklmnopqrstuvwxyz".AsEnumerable(), (a, b) => a.Intersect(b))
                    .ToList())
                .ToList()
                .Aggregate(0, (a, b) => a + b.Count);
            Console.WriteLine($"De oplossing voor deel 2: {oplossingDeel22}");
        }
    }
}
