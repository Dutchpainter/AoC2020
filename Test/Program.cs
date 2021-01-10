using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_18
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inlezen bestand
            var huiswerk = File
                   .ReadAllLines("Inputtest.txt")
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToList();
            // Deel 1

            var antwoorden = new List<int>();
            
            Console.WriteLine($"Oplossing deel 1 : Nog doen ");
        }
    }

    public class Som
    {
        public string Input { get; }
        public Som(string input)
        {
            // maak string zonder spaties
            Input = input.Replace(" ", "");
        }
        public int Berekening()
        {
            int countStart = 0;
            int countEnd = 0;



            return 0;
        }

        public int Bewerking(int argument, int beginwaarde, char bewerking)
        {
            var antwoord = 0;
            switch (bewerking)
            {
                case '*':
                    antwoord = beginwaarde * argument;
                    break;
                case '+':
                    antwoord = beginwaarde + argument;
                    break;
            }
            return antwoord;
        }
    }
}
