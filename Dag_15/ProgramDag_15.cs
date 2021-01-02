using System;
using System.Collections.Generic;
using System.Linq;


namespace Dag_15
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var ctr = 0;
            var startingNumbers = "16,1,0,18,12,14,19"
            // var startingNumbers = "0,3,6"
                .Split(',')
                .Select(el => int.Parse(el))
                .ToList();
            var lastSpoken = 0;
            var previousSpoken = 0;
            var rounds = 2020;
            var data = new Dictionary<int, int>();

            do
            {
                // Console.WriteLine($"startingnummer: {startingNumbers[ctr]}");

                data[startingNumbers[ctr]] = ctr + 1;
                lastSpoken = startingNumbers[ctr];
                //Console.WriteLine($"Round: {ctr+1} : Genoemde waarde: {lastSpoken} : {data[startingNumbers[ctr]]}");
                lastSpoken = startingNumbers[ctr];
                ctr++;
            } while (ctr < startingNumbers.Count - 1);
            lastSpoken = startingNumbers[ctr];
            ctr++;
            // Console.WriteLine("Inlezen eerste waarden is klaar");
            // Console.WriteLine($"Laatst genoemde waarde = {lastSpoken}");
            do
            {
                // controleren of lastspoken al in dict zit, waardes aanpassen
                if (data.ContainsKey(lastSpoken))
                {
                    // is al langsgekomen
                    var vorige = data[lastSpoken];
                    //var bereken = ctr - 1 - data[lastSpoken].voorlaatste;

                    data[lastSpoken] = ctr;
                    previousSpoken = lastSpoken;
                    lastSpoken = ctr - vorige;
                }
                else
                {
                    // Key bestaan nog niet, toevoegen
                    data[lastSpoken] = ctr;
                    previousSpoken = lastSpoken;
                    lastSpoken = 0;
                }
                // Console.WriteLine($"Round: {ctr} Lastspoken: {lastSpoken} Lastdata {previousSpoken}: {data[previousSpoken]}");
                ctr++;
            } while (ctr < rounds);
            Console.WriteLine($"Waarde op beurt {ctr} gesproken  = {lastSpoken}");
        }
    }
}
