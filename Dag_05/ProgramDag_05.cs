using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_05
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            Console.WriteLine("Hello World!");
            var plaatsen = new List<(int, string)>();
            var stoelplek = "";
            
            while (true)
            {
                //inlezen van de regels
                var regel = reader.ReadLine();
                if (string.IsNullOrEmpty(regel))
                {
                    break;
                }
                stoelplek = regel.Replace("B", "1");
                stoelplek = stoelplek.Replace("F","0");
                stoelplek = stoelplek.Replace("R", "1");
                stoelplek = stoelplek.Replace("L", "0");
                //Console.WriteLine($"Vervang B door 0: {regel} => {stoelplek}");    
                plaatsen.Add((Convert.ToInt32(stoelplek, 2), regel));
            };
            plaatsen.Sort();
            var oplossing1 = plaatsen.Last();
            Console.WriteLine($"De oplossing voor deel 1 is: {oplossing1}");
            
            var teller = plaatsen.First().Item1;
            var oplossing2 = 0;
            foreach(var plaats in plaatsen){
                if (plaats.Item1 == teller)
                {
                    teller++;
                }
                oplossing2 = teller;  
            }
            Console.WriteLine($"De oplossing voor deel 2 is: {oplossing2}");
        }
    }
}