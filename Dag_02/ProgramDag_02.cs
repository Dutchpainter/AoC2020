using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Dag_02
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            //Reguliere expressie voor inlezen waarden vanuit input
            var passwordsRegex = new Regex("(?'min'[0-9]+)-(?'max'[0-9]+) (?'zoekletter'[a-z]): (?'reeks'[a-z]+)", RegexOptions.Compiled);
            
            var oplossingDeel1 = 0;
            var oplossingDeel2 = 0;

            // Begin
            Console.WriteLine("Begin van het programma.");
            while (true)
             {
                var regel = reader.ReadLine();

                if (string.IsNullOrEmpty(regel))
                    break;

                var passwordsMatch = passwordsRegex.Match(regel);
                if (passwordsMatch.Success)
                {
                    // inlezen notes
                    var min = int.Parse(passwordsMatch.Groups["min"].Value);
                    var max = int.Parse(passwordsMatch.Groups["max"].Value);
                    var zoekletter = passwordsMatch.Groups["zoekletter"].Value;
                    var reeks = passwordsMatch.Groups["reeks"].Value;

                    // lijst maken met daarin elke gevonden goede letter.
                    var aantalGevonden = reeks
                        .Where(letter => letter == zoekletter.Single())
                        .ToList();

                    // controle of er het juiste aantal goede letters zijn gevonden
                    if (aantalGevonden.Count() >= min && aantalGevonden.Count() <= max)
                    {
                        oplossingDeel1++;
                    }

                    // controle of op exact 1 van beide posities een goede letter staat
                    if ((reeks[min - 1] == zoekletter.Single()) != (reeks[max - 1] == zoekletter.Single()))
                    {
                        oplossingDeel2++;
                    }
                }  
            }
            Console.WriteLine($"Oplossing deel1: {oplossingDeel1}");
            Console.WriteLine($"Oplossing deel2: {oplossingDeel2}");
        }
    }
}
