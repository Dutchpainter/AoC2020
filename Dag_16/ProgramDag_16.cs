using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Op16_2
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            // inlezen notes
            // inlezen own ticket
            // inlezen nearby tickets en controle invalid tickets

            var notesRegex = new Regex("(?'note'[a-z\\s]+):\\s(?'start1'[0-9]+)-(?'end1'[0-9]+)\\sor\\s(?'start2'[0-9]+)-(?'end2'[0-9]+)", RegexOptions.Compiled);
            var notesList = new List<Note>();
            while (true)
            {
                var line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                var notesMatch = notesRegex.Match(line);
                if (notesMatch.Success)
                {
                    // inlezen notes
                    var note = notesMatch.Groups["note"].Value;
                    var start1 = int.Parse(notesMatch.Groups["start1"].Value);
                    var end1 = int.Parse(notesMatch.Groups["end1"].Value);
                    var start2 = int.Parse(notesMatch.Groups["start2"].Value);
                    var end2 = int.Parse(notesMatch.Groups["end2"].Value);

                    var noteObject = new Note
                    {
                        NoteName = note,
                        Range = new List<Range>
                        {
                            new Range
                            {
                                Start = start1, End = end1
                            },
                            new Range
                            {
                                Start = start2, End = end2
                            }
                        }
                    };
                    notesList.Add(noteObject);
                }
            }
            Console.WriteLine("Notes ingelezen");
            reader.ReadLine();
            var myTicket = reader.ReadLine().Split(",").Select(int.Parse).ToList();
            Console.WriteLine("Eigen ticket ingelezen");
            reader.ReadLine(); reader.ReadLine();
            // maak een lijst van tickets nearby

            var nearbyTickets = new List<List<int>>();
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                var nearbyTicket = line.Split(",").Select(int.Parse).ToList();
                nearbyTickets.Add(nearbyTicket);
            }
            Console.WriteLine("Nearby Tickets ingelezen");

            // opdracht 1 berekenen
            // optellen invalid waardes uit alle nearby tickets
            var opdracht1 = nearbyTickets
                .SelectMany(ticket => ticket)
                .Where(getal => !notesList
                    .Any(note => note.IsValid(getal)))
                .Sum();

            Console.WriteLine($"Opdracht 1: {opdracht1}");

            // opdracht 2 berekenen
            var validTickets = nearbyTickets
                .Where(ticket => ticket.All(getal => notesList.Any(note => note.IsValid(getal))))
                .Append(myTicket)
                .ToList();

            Console.WriteLine("Lijst met geldige tickets gemaakt, incl eigen ticket");

            // Nu voor ranges uit elke note kijken voor welk veld van elk ticket deze geldig is.
            // Hier komt een lijst met velden uit.

            var validColumnsPerNote = notesList.Select(note => Enumerable.Range(0, myTicket.Count)
                        .Select(col => validTickets
                            .All(ticket => note.IsValid(ticket[col])))
                        .ToList())
                    .ToList();

            Console.WriteLine("Mapping gemaakt van geldige ticket posities per note");

            // Kijken welke kolom maar 1 "true" bevat. Dan moet dit de positie van de bijbehorende note zijn

            var mapping = new Dictionary<int, int>();

            while (mapping.Count
                != notesList.Count)
            {
                var validNotes = validColumnsPerNote
                    .Select((val, idx) => (val, idx))
                    .Where(cols => cols.val.Count(col => col == true) == 1);
                foreach (var validNote in validNotes)
                {
                    var noteIdx = validNote.idx;
                    var columnIdx = validNote.val.IndexOf(true);
                    mapping[noteIdx] = columnIdx;
                    foreach (var columnPerNote in validColumnsPerNote)
                    {
                        columnPerNote[columnIdx] = false;
                    }
                }
            }

            Console.WriteLine("Mapping gemaakt van posities en notes");

            // alle waarden van velden uit eigen ticket waarvan de naam begint met Departure optellen.
            var opdracht2 = notesList
                .Select((val, idx) => (val, idx))
                .Where(noteAndIndex => noteAndIndex.val.NoteName.StartsWith("departure"))
                .Select(noteAndIndex => mapping[noteAndIndex.idx])
                .Select(col => myTicket[col])
                .Aggregate(1ul, (a, b) => (a * (ulong)b));

            Console.WriteLine($"De uitkomst van opdracht 2: {opdracht2}");

        }

    }

    class Note
    {
        public string NoteName { get; set; }
        public List<Range> Range { get; set; }
        public bool IsValid(int getal)
        {
            return Range.Any(range => range.IsInRange(getal));
        }

    }
    class Range
    {
        public int Start { get; set; }
        public int End { get; set; }
        public bool IsInRange(int getal)
        {
            return getal >= Start && getal <= End;
        }
    }

}
