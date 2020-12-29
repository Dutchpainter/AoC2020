using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_04
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var paspoorten = new List<Paspoort>();

            // regels combineren tot er 1 regel per paspoort 
            string laatsteRegel;
            while (true)
            {
                string regel = "";
                while (true)
                {
                    laatsteRegel = reader.ReadLine();
                    if (string.IsNullOrEmpty(laatsteRegel))
                        break;
                    regel = regel + " " + laatsteRegel;
                }
                var nieuwPaspoort = new Paspoort(regel);
                paspoorten.Add(nieuwPaspoort);
                if (laatsteRegel == null) break;
            }
            // de paspoorten zijn ingelezen
            Console.WriteLine("De paspoorten zijn ingelezen");
            // nu moet ik uit de lijst met objecten de aantallen etc halen, maar eerst de lijst goed.
            var LijstOplossingDeel1 = paspoorten
                .Where(paspoort => paspoort.paspoortIsValid());
            var oplossingDeel1 = LijstOplossingDeel1.Count();
            Console.WriteLine($"Oplossing deel 1: {oplossingDeel1}");
            // nu de objecten selecteren uit de vorige lijst waarbij ook nog eens alle lossen velden valid zijn
            var oplossingDeel2 = LijstOplossingDeel1
                .Where(paspoort =>(paspoort.velden
                    .Where(velden => velden.veldIsValid())
                    .Count()) == 7)
                .Count();
            Console.WriteLine($"Oplossing deel 2: {oplossingDeel2}");
        }
    }

    class Paspoort
    {
        public List<Veld> velden { get; set; }
        public Paspoort(string regel)
        {
            velden = new List<Veld>();
            var regex = new Regex("(?'sleutel'[a-z]{3}):(?'waarde'[a-z0-9#]+)", RegexOptions.Compiled);
            var match = regex.Match(regel);
            while (match.Success)
            {
                var nieuweSleutel = match.Groups["sleutel"].Value;
                var nieuweWaarde = match.Groups["waarde"].Value;
                // maak object veld aan
                if (nieuweSleutel != "cid")
                {
                    var nieuwVeld = new Veld(nieuweSleutel, nieuweWaarde);
                    velden.Add(nieuwVeld);
                }
                match = match.NextMatch();
            }        
            //Console.WriteLine("Nieuw Paspoort");
        }
        public bool paspoortIsValid()
        {
            // controleren met aantal velden valid 
            return (velden.Count == 7);
        }
    }
    class Veld
    {
        public string Sleutel { get; set; }
        public string Waarde { get; set; }
        public Veld (string sleutel, string waarde)
        {
            Sleutel = sleutel;
            Waarde = waarde;
        }
        public bool veldIsValid ()
        {
            // controle veld met caseSwitch en regel
            var caseSwitch = Sleutel;
            var isvalid = false;
            switch (caseSwitch)
            {
                case "byr":
                    //byr(Birth Year) - four digits; at least 1920 and at most 2002.
                    if (int.Parse(Waarde) >= 1920 && int.Parse(Waarde) <=2002)
                        isvalid = true;
                    break;
                case "iyr":
                    //iyr(Issue Year) - four digits; at least 2010 and at most 2020.
                    if (int.Parse(Waarde) >= 2010 && int.Parse(Waarde) <= 2020)
                        isvalid = true;
                    break;
                case "eyr":
                    //eyr(Expiration Year) - four digits; at least 2020 and at most 2030.
                    //controle nog toevoegen
                    if (int.Parse(Waarde) >= 2020 && int.Parse(Waarde) <= 2030)
                        isvalid = true;
                    break;
                case "hgt":
                    //hgt(Height) - a number followed by either cm or in:
                    //If cm, the number must be at least 150 and at most 193.
                    //If in, the number must be at least 59 and at most 76.
                    if (Waarde.EndsWith("cm"))
                    {
                        var hoogteStr = Waarde.Substring(0, Waarde.Length - 2);
                        isvalid = int.TryParse(hoogteStr, out var hoogte) &&  hoogte>= 150 && hoogte <= 193;
                    }
                    if (Waarde.EndsWith("in"))
                    {
                        var hoogteStr = Waarde.Substring(0, Waarde.Length - 2);
                        isvalid = int.TryParse(hoogteStr, out var hoogte) && hoogte >= 59 && hoogte <= 76;
                    }
                    break;
                case "hcl":
                    //hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                    if (Waarde.StartsWith("#"))
                    {
                        var haarkleur = Waarde.Substring(1);
                        isvalid = ((Waarde.Substring(1).Length == 6) && (Waarde.IndexOfAny("0123456789abcdef".ToCharArray()) != -1 ));
                    }
                    break;
                case "ecl":
                    //ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                    var optionList = new List<string>
                        { "amb","blu","brn","gry","grn","hzl","oth" };
                    if (optionList.Any(Waarde.Contains))
                        isvalid = true;
                    break;
                case "pid":
                    //pid(Passport ID) - a nine - digit number, including leading zeroes
                    if ((Waarde.Length == 9) && ( Waarde.All(char.IsDigit)))
                        isvalid = true;
                    break;
                case "cid":
                    //cid(Country ID) - ignored, missing or not
                    isvalid = true;
                    break;
                default:
                    //hier hoor je niet te komen
                    isvalid = false;
                    break;
            }
            return isvalid;
        }
    }
}


