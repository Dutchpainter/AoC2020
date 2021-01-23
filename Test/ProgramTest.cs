using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var allTiles = File
                  .ReadAllLines("Inputtest.txt")
                  .Where(s => !string.IsNullOrWhiteSpace(s))
                  .ToList();
            
            Console.WriteLine("De opdracht is klaar");

        }
    }
   
}
