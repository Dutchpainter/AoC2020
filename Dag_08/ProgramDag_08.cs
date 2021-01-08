using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_08
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var inputRegex = new Regex("(?'opcode'[a-z]{3})\\s(?'waarde'[+\\-][0-9]+)", RegexOptions.Compiled);
            var commandos = new List<(string opcode, int waarde)>();

            // start opdracht 8
            Console.WriteLine("Opdracht 8");
            var opcode = "";
            var waarde = 0;

            while (true)
            {
                var regel = reader.ReadLine();
                if (regel == null) break;
                var match = inputRegex.Match(regel);
                if (match.Success)
                {
                    opcode = match.Groups["opcode"].Value;
                    waarde = int.Parse(match.Groups["waarde"].Value);
                    commandos.Add((opcode, waarde));
                }
            }
            // waardes ingelezen
            Console.WriteLine("De waardes zijn ingelezen");

            // berekening
            var isTweedeMaal = new HashSet<int>();
            var positie = 0;
            var resultaat = 0;
            var oplossing1 = 0;
            var positieVeranderd = 0;

            while (positie < commandos.Count)
                {
                isTweedeMaal.Clear();
                positie = 0;
                resultaat = 0;
                while (!isTweedeMaal.Contains(positie) && (positie < commandos.Count))
                {
                    isTweedeMaal.Add(positie);
                    switch (commandos[positie].opcode)
                    {
                        case "nop":
                            if (positie == positieVeranderd)
                            {
                                // voor jmp uit en ga naar positie + waarde
                                positie += commandos[positie].waarde;
                                break;
                            }
                            else
                            {
                                // ga naar volgende positie
                                positie++;
                                break;
                            }
                        case "acc":
                            // tel waarde op bij resultaat en ga naar volgende positie 
                            resultaat += commandos[positie].waarde;
                            positie++;
                            break;
                        case "jmp":
                            if (positie == positieVeranderd)
                            {
                                // voer nop uit, ga naar volgende positie 
                                positie++;
                                break;
                            }
                            else
                            {
                                // ga naar positie + waarde
                                positie += commandos[positie].waarde;
                                break;
                            }
                        default:
                            //hier hoor je niet te komen
                            Console.WriteLine("Er gaat iets niet goed ;)");
                            break;
                    }
                }
                // eerste oplossing is oplossing vraag 1
                if (positieVeranderd == 0) { oplossing1 = resultaat; };
                // test volgende waarden
                positieVeranderd++;
                if (commandos[positieVeranderd].opcode == "acc" ) { positieVeranderd++; };
            }
            // de juiste positie voor NOP/JMP gevonden
            Console.WriteLine($"De oplossing voor deel 1: {oplossing1}");
            Console.WriteLine($"De oplossing voor deel 2: {resultaat}");
        }
    }
}
