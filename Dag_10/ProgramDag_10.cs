using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_10
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var adapters = new List<int>();           

            Console.WriteLine("Opdracht 10");
            while (true)
            {
                var regel = reader.ReadLine();
                if (regel == null) break;
                adapters.Add(int.Parse(regel));

            }
            Console.WriteLine("De input is gelezen");

            // begin en eindpunt adapters toevoegen en lijst sorteren
            var allAdapters =  new List<int>(adapters);
            allAdapters.Add(allAdapters.Max() + 3);
            allAdapters.Sort();
            Console.WriteLine("Opdracht 1");

            var compared = new List<int>();
            var first = 0;
            foreach (var adapter in allAdapters)
            {
                compared.Add(adapter - first);
                first = adapter;
            }
            var oplossing1 = compared
                .Where(jolts => jolts == 1)
                .Count()
                * compared
                .Where(jolts => jolts == 3)
                .Count();
            Console.WriteLine($"Opdracht 1: {oplossing1}");

            // Opdracht 2
            Console.WriteLine("Opdracht 2");

            var possibleAdapters = new List<(int Jolts, long Mogelijkheden)> { (0, 1) };   
            foreach(var adapter in allAdapters)
            {
                // verwijder alle mogelijkheden waarvan het verschil met de vorige adapter meer dan 3 jolts is
                possibleAdapters.RemoveAll(tup => adapter - tup.Jolts > 3);
                // het aantal mogelijkheden voor deze adapter is de som van alle voorgaande mogelijkheden
                var mogelijkheden = possibleAdapters.Select(tup => tup.Mogelijkheden).Sum();
                // voeg de nieuwe mogelijkheid toe aan de lijst
                possibleAdapters.Add((adapter, mogelijkheden));
            }
            var oplossing2 = possibleAdapters.Last().Mogelijkheden;
            Console.WriteLine($"Opdracht 2: {oplossing2}");

            // lijst maken met verschil
            // tel aantal 1-n op rij.
            // vermenigvuldigfactor = 2^(n-1) tot n-3, daarna 2^(n-1)-1 ( geldt voor n=4
            // er komen geen verschillen van 2 voor in de reeks Het is of 1 of 3
            var teller = 0L;
            oplossing2 = 1L;
            foreach (var waarde in compared)
            {   
                if (waarde == 1)
                {
                    teller++;
                }
                else //waarde = 3
                {
                    switch (teller)
                    {
                        case 4:
                            oplossing2 = oplossing2 * ((long)Math.Pow(2, teller - 1) - 1);
                            teller = 0;
                            break;
                        case 0:
                            //do nothing
                            break;
                        default:
                            oplossing2 = oplossing2 * (long)Math.Pow(2, teller - 1);
                            teller = 0;
                            break;
                    }
                }
            }
            Console.WriteLine($"Opdracht 2 versie 2: {oplossing2}");
        }
    }
}

