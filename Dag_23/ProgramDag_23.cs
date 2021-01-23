using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_23
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzleInput = "739862541";  
            //var puzzleInput = "389125467";  // testinput 67384529 at 100 moves. 149245887792
            var vraag1 = new CrabCups(puzzleInput);
            vraag1.MoveCups(100);
            vraag1.PrintQueue();
            var numberOfCups = 1000000;
            var Rounds = 10000000;
            var vraag2 = new CrabCups(puzzleInput, numberOfCups);
            vraag2.MoveCups(Rounds);
            //vraag2.PrintQueue();
            vraag2.CalculateResult2();
        }

        public class CrabCups
        {
            public List<int> Cups { get; set; }
            public int CurrentCup { get; set; }
            public int MoveNumber { get; set; }
            public int CupSize { get; }
            public CrabCups(string cups, int cupsize = 9)
            {
                MoveNumber = 0;
                CupSize = cupsize;
                Cups = new List<int>(cups.Select(c => int.Parse(c.ToString())).ToList());
                CurrentCup = Cups.First();
                for (var i = 9; i < cupsize; i++)
                {
                    Cups.Add(i + 1);
                }        
            }
            public bool PrintQueue()
            { 
                Console.Write("Het antwoord op vraag 1:");
                var startPrint = Cups.IndexOf(1);
                Console.WriteLine($"Index of 1 {startPrint}");
                for (var i = 1; i < CupSize; i++)
                {

                    Console.Write(Cups[(startPrint + i) % CupSize]);
                }
                Console.WriteLine();
                return true;
            }
            public bool CalculateResult2()
            {
                var positionOfOne = Cups.IndexOf(1) ;
                var firstResult = Cups[positionOfOne + 1] ;
                var secondResult = Cups[positionOfOne + 2];
                long Resultaat2 = (long)firstResult * (long)secondResult;
                Console.WriteLine($"Het antwoord op vraag 2: {firstResult} * {secondResult} = {Resultaat2}");
                return true;
            }
            int GetDestinationCup(List<int> threeMovingCups)
            {
                var destinationCup = ((CurrentCup + CupSize-2) % CupSize) +1;
                while(threeMovingCups.Contains(destinationCup))
                {
                    destinationCup = ((destinationCup + CupSize-2) % CupSize) +1;
                }
                return destinationCup;
            }
            bool MoveCups()
            {
                CurrentCup = Cups[0];
                var threeMovingCups = new List<int>(Cups.GetRange(1, 3));
                var destinationCup = GetDestinationCup(threeMovingCups);
                Cups.RemoveRange(0, 4);
                var indexDestinationCup = Cups.IndexOf(destinationCup);
                Cups.InsertRange(indexDestinationCup+1,threeMovingCups);
                Cups.Add(CurrentCup);
                if(MoveNumber % 10000 == 0)
                {
                    Console.WriteLine($"Round: {MoveNumber}");
                }
                MoveNumber++;
                return true;
            }
            public bool MoveCups(int times = 1)
            {
                for (var i = 0;i< times; i++)
                {
                    MoveCups();
                }
                return true;
            }
        }
    }
}
