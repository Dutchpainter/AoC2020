using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            Console.WriteLine("Hello World!");
        }
    }
}
