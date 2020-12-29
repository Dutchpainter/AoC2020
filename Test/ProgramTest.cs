using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dag_01
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
            // deel 1 van de vraag oplossen
            // controleren of er een eersteGetal en een tweedeGetal zijn die samen 2020 opleveren
            foreach (var eersteGetal in getallen)
            {
                var benodigdGetal = 2020 - eersteGetal;
                var tweedeGetal = getallen
                    .Where(getal => getal == benodigdGetal)
                    .ToList();

                // als er een resultaat is de oplossing printen
                if (tweedeGetal.Any())
                {
                    Console.WriteLine("Oplossing deel 1:");
                    Console.WriteLine($"Eerste getal: {eersteGetal} Tweede getal: {tweedeGetal.Single()} Som: {eersteGetal + tweedeGetal.Single()} Product: {eersteGetal * tweedeGetal.Single()}");
                    Console.WriteLine();

                    // als er een waarde gevonden is:Break. Er worden minimaal 2 sets gevonden
                    break;
                }
            }
            // deel 2 van de vraag oplossen
            // controleren of er een eersteGetal, een tweedeGetal en een derdeGetal zijn die samen 2020 opleveren

            Console.WriteLine("Oplossing deel 2:");
            foreach (var eersteGetal in getallen)
            {
                var benodigdGetalEen = 2020 - eersteGetal;
                foreach (var tweedeGetal in getallen.Where(getal => (getal != eersteGetal)).ToList())
                {
                    var benodigdGetalTwee = benodigdGetalEen - tweedeGetal;
                    var derdeGetal = getallen
                        .Where(getal => (getal == benodigdGetalTwee && getal != eersteGetal && getal != tweedeGetal))
                        .ToList();

                    if (derdeGetal.Any())
                    {
                        Console.WriteLine($"Eerste getal: {eersteGetal} Tweede getal: {tweedeGetal} Derde getal: {derdeGetal.Single()} Som: {eersteGetal + tweedeGetal + derdeGetal.Single()} Product: {eersteGetal * tweedeGetal * derdeGetal.Single()}");
                        break;
                    }

                }
            }
            Console.WriteLine("Opdracht afgerond");
        }
    }
}