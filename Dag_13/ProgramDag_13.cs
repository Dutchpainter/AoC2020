using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_13
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var timestamp = long.Parse(reader.ReadLine());
            var line = reader.ReadLine();
            var busLines = line.Split(",")
                .Select((element, index) => (valid: long.TryParse(element, out var waarde), busline: waarde, index: (long) index))
                .Where(waarde => waarde.valid)
                .Select(waarde => (waarde.busline, waarde.index))
                .ToList();
     
            // Deel 1
            var results = new List<(long timeToWait, long busline)>();
            foreach (var waarde in busLines)
            {
                var timeToWait = waarde.busline - timestamp % waarde.busline;
                results.Add((timeToWait, waarde.busline));
            }
            results.Sort();
            var oplossing1 = results.First().timeToWait * results.First().busline;
            Console.WriteLine($"De oplossing van deel 1 is:{oplossing1}");

            // Deel 2
            var factor = busLines[0].busline;
            var rest = busLines[0].index;
            var stamp = factor - rest;
            long modulo;
            long controle;
            for (var i = 1; i < busLines.Count; i++)
            {
                do
                {
                    stamp += factor;
                    modulo = stamp % busLines[i].busline;
                    controle = (busLines[i].busline - (busLines[i].index == 0 ? busLines[i].busline : busLines[i].index));
                    while (controle < 0)
                    {
                        controle += busLines[i].busline;
                    }
                }
                while (modulo != controle);
                factor *= busLines[i].busline;
                Console.WriteLine($"Ronde : {i} Timestamp: {stamp} Factor :{factor}");
            }
            Console.WriteLine($"De oplossing van deel 2 is:{stamp}");
        }
    }
}
