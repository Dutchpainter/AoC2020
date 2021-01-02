using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_07
{
    class Program
    {
		static void Main(string[] args)
		{
			using var file = new FileStream("Input.txt", FileMode.Open);
			//using var file = new FileStream("InputTest.txt", FileMode.Open);
			using var reader = new StreamReader(file);

			Console.WriteLine("Opdracht 7");
			var tassen = new Dictionary<string, List<(string naam, ulong aantal, bool isLeeg)>>();

			var tasNaamRegex = new Regex("(?'tasNaam'[a-z\\s]+)\\sbags\\scontain", RegexOptions.Compiled);
			var welInhoudRegex = new Regex("(?'inhoudAantal'[0-9]+)\\s(?'inhoudNaam'[a-z\\s]+)\\sbags*",
				RegexOptions.Compiled);
			var geenInhoudRegex = new Regex("(?'tasNaam'[a-z\\s]+)\\sbags\\scontain\\sno\\sother\\sbags\\.",
				RegexOptions.Compiled);

			var tasNaam = "";
			var inhoudNaam = "";
			ulong inhoudAantal = 0;
			//var isLeeg = false;

			// begin met laden dictionary
			while (true)
			{
				var regel = reader.ReadLine();
				if (regel == null) break;

				var tasMetInhoud = new List<(string naam, ulong aantal, bool isLeeg)>();
				//var tasNaam = "";
				//var inhoudNaam = "";
				//var inhoudAantal = 0;

				var match = geenInhoudRegex.Match(regel);
				if (match.Success)
				{
					// tas zonder inhoud
					tasNaam = match.Groups["tasNaam"].Value;
					tasMetInhoud.Add((tasNaam, 1, true));
					Console.WriteLine($"inhoud: {tasNaam}: geen inhoud");
				}
				else
				{
					match = tasNaamRegex.Match(regel);
					if (match.Success)
					{
						// tassen met inhoud
						tasNaam = match.Groups["tasNaam"].Value;
                        tasMetInhoud.Add((tasNaam, 1, true));
						match = welInhoudRegex.Match(regel);
						while (match.Success)
						{
							inhoudNaam = match.Groups["inhoudNaam"].Value;
							inhoudAantal = ulong.Parse(match.Groups["inhoudAantal"].Value);
							tasMetInhoud.Add((inhoudNaam, inhoudAantal, false));
							Console.WriteLine($"inhoud: {tasNaam}: {inhoudNaam} {inhoudAantal}");
							match = match.NextMatch();
						}
					}
				}
				tassen[tasNaam] = tasMetInhoud;
			}
			Console.WriteLine("Tassen ingelezen");
			Console.WriteLine("Volgende stap");

			// tassen doorrekenen en alle volle tassen vervangen tot alle tassen leeg zijn
			foreach (var tas in tassen)
			{
				var tasInhoud = tas.Value;
				//Console.WriteLine($"Tasnaam {tas.Key} inhoud is: ");	
				for (var i = 0; i < tasInhoud.Count(); i++)
				{
					//Console.WriteLine($"     tas inhoud: {tasInhoud[i].naam} tas aantal: {tasInhoud[i].aantal}  tas is leeg: {tasInhoud[i].isLeeg}");
					if (tasInhoud[i].isLeeg == false)
					// als tas leeg is niets doen
					{
						// tas is niet leeg
						inhoudAantal = tasInhoud[i].aantal;
						inhoudNaam = tasInhoud[i].naam;
						tasInhoud[i] = (inhoudNaam, inhoudAantal, true);
						var tasVervangen = tassen[inhoudNaam].ToList();
						for (var j = 1; j < tasVervangen.Count(); j++)
						{
							//tasVervangen[j] = (tasVervangen[j].naam, tasVervangen[j].aantal * inhoudAantal, tasVervangen[j].isLeeg);
							tasInhoud.Add((tasVervangen[j].naam, tasVervangen[j].aantal * inhoudAantal, tasVervangen[j].isLeeg));
						}
					}
				}
			}
			// reeksen uitgewerkt
			Console.WriteLine("Tassen uitgewerkt");
			// berekenen opdrachten
			var oplossingDeel1 = tassen
				.Values
				//.ToList()
				.Select(tas => tas
					.Select(tas => tas.naam)
					.Where(naam => naam == "shiny gold")
						.Count())
				.ToList()
				.Where(aantal => aantal >0)
				.Count()
				- 1;
			// 1 eraf trekken voor de tas zelf
			Console.WriteLine($"Opdracht 1, aantal tassen : {oplossingDeel1}");

			var oplossingDeel2 = tassen["shiny gold"]
				.Select(tasinhoud => tasinhoud.aantal)
				.ToList()
				.Aggregate(0ul, (a, b) => a + b)
				- 1;
			// 1 eraf trekken voor de tas zelf
			Console.WriteLine($"Opdracht 12, aantal tassen : {oplossingDeel2}");


		}
    }
}