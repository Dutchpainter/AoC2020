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
                   .ReadAllLines("Input.txt")
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToList();

            // Deel 1
            var antwoorden = new List<long>();
            foreach (var vraag in huiswerk)
            {
                var antwoord = new Som(vraag).Berekening();
                antwoorden.Add(antwoord);
            }
            var oplossing1 = antwoorden.Aggregate(0L, (a, b) => (a + b));
            Console.WriteLine($"Oplossing deel 1 : {oplossing1}");

            // deel 2
            var antwoorden2 = new List<long>();
            foreach (var vraag in huiswerk)
            {
                var antwoord = new Som(vraag).Berekening2();
                antwoorden2.Add(antwoord);
            }
            var oplossing2 = antwoorden2.Aggregate(0L, (a, b) => (a + b));
            Console.WriteLine($"Oplossing deel 2 : {oplossing2}");
        }
    }

    public class Som
    {
        public string Input { get; }
        public Som(string input)
        {
            Input = input.Replace(" ", "");
        }
        public (string reststring, long waarde) findHook(string todoString)
        {
            int countStart = 1;
            int countEnd = 0;
            var indexEnd = 1;
            
            while (countStart != countEnd)
            {
                if (todoString[indexEnd] == ')') countEnd++;
                if (todoString[indexEnd] == '(') countStart++;
                indexEnd++;
            }
            var nextSom = todoString.Substring(1, indexEnd - 2);
            var beginwaarde = new Som(nextSom).Berekening2();
            todoString = todoString.Substring(indexEnd);
            return (todoString, beginwaarde);
        }
        public long Berekening2()
        {
            long Antwoord = 0;
            var todoString = Input;
            long beginwaarde;
            char bewerking;
            char bewerking2;
            long argument;
            long argument2;
            // beginwaarde:
            if (todoString[0] == '(')
            {
                beginwaarde = findHook(todoString).waarde;
                todoString = findHook(todoString).reststring;
            }
            else
            {
                beginwaarde = long.Parse(todoString[0].ToString());
                todoString = todoString.Substring(1);
            }
            // rest van de som
            while (todoString.Length != 0)
            {
                // bewerking
                bewerking = todoString[0];
                todoString = todoString.Substring(1);
                // argument
                if (todoString[0] == '(')
                {
                    argument = findHook(todoString).waarde;
                    todoString = findHook(todoString).reststring;
                }
                else
                {
                    argument = long.Parse(todoString[0].ToString());
                    todoString = todoString.Substring(1);
                }
                // controle
                
                if(bewerking == '*' && todoString.Length != 0 && todoString[0] == '+')
                {
                    while (todoString.Length != 0 && todoString[0] == '+')
                    {
                        bewerking2 = todoString[0];
                        todoString = todoString.Substring(1);
                        //argument 2 ophalen
                        if (todoString[0] == '(')
                        {
                            argument2 = findHook(todoString).waarde;
                            todoString = findHook(todoString).reststring;
                        }
                        else
                        {
                            argument2 = long.Parse(todoString[0].ToString());
                            todoString = todoString.Substring(1);
                        }
                        argument = Bewerking(argument2, argument, bewerking2);   
                    }
                    Antwoord = Bewerking(argument, beginwaarde, bewerking);
                    beginwaarde = Antwoord;
                }
                else
                {
                    Antwoord = Bewerking(argument, beginwaarde, bewerking);
                    beginwaarde = Antwoord;
                }
            
            }
            return Antwoord;
        }
        public long Berekening()
        {
            long Antwoord = 0;
            var todoString = Input;
            long beginwaarde;
            char bewerking;
            long argument;

            // bepaal beginwaarde:
            if (todoString[0] == '(')
            {
                int countStart = 1;
                int countEnd = 0;
                var indexEnd = 1;
                while (countStart != countEnd)
                {
                    if (todoString[indexEnd] == ')') countEnd++;
                    if (todoString[indexEnd] == '(') countStart++;
                    indexEnd++;
                }
                var nextSom = todoString.Substring(1, indexEnd - 2);
                beginwaarde = new Som(nextSom).Berekening();
                todoString = todoString.Substring(indexEnd);
            }
            else
            {
                beginwaarde = long.Parse(todoString[0].ToString());
                todoString = todoString.Substring(1);
            }
            // rest van de som
            while (todoString.Length != 0)
            {
                bewerking = todoString[0];
                todoString = todoString.Substring(1);
                if (todoString[0] == '(')
                {
                    int countStart = 1;
                    int countEnd = 0;
                    var indexEnd = 1;
                    while (countStart != countEnd)
                    {
                        if (todoString[indexEnd] == ')') countEnd++;
                        if (todoString[indexEnd] == '(') countStart++;
                        indexEnd++;
                    }
                    var nextSom = todoString.Substring(1, indexEnd - 2);
                    argument = new Som(nextSom).Berekening();
                    todoString = todoString.Substring(indexEnd);
                }
                else
                {
                    argument = long.Parse(todoString[0].ToString());
                    todoString = todoString.Substring(1);
                }
                Antwoord = Bewerking(argument, beginwaarde, bewerking);
                beginwaarde = Antwoord;
            }
            return Antwoord;
        }

        private long Bewerking(long argument, long beginwaarde, char bewerking)
        {
            var antwoord = 0L;
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
