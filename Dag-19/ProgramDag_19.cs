using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_19
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var nieuweRegels = new Dictionary<int, NieuweRegel>();
            var uitgewerkteRegels = new Dictionary<int, UitgewerkteRegel>();
            var messages = new List<String>();

            // Inlezen informatie
            var regel = reader.ReadLine();
            while(regel!= "")
            {
                ReadRules(regel);
                regel = reader.ReadLine();
            }
            Console.WriteLine("De regels zijn ingelezen");
            regel = reader.ReadLine();
            while(regel != null)
            {
                messages.Add(regel);
                regel = reader.ReadLine();
            }
            Console.WriteLine("De messages zijn ingelezen");
            //

            while (nieuweRegels.Count != 0)
            {
                foreach(var testregel in nieuweRegels)
                {
                   var uitgewerkteRegel = (testregel.Value).ParseRegel(uitgewerkteRegels);
                   if(uitgewerkteRegel != null)
                    {
                        uitgewerkteRegels.Add(testregel.Key, uitgewerkteRegel);
                        nieuweRegels.Remove(testregel.Key);
                    } 
                }
            }

          
            var regel0 = new HashSet<string>();
            foreach (var waarde in uitgewerkteRegels[0].Regels)
            {
                regel0.Add(waarde);
            }
            // check
            int oplossing1 = 0;
            foreach (var message in messages)
            {
                if (regel0.Contains(message)) oplossing1++;
            }
            Console.WriteLine($"Het antwoord op vraag 1 = {oplossing1}");

            var oplossing2 = BepaalTwee(42, 31);

            Console.WriteLine($"Het antwoord op vraag 2 = {oplossing2}");

            bool FormatValid(string format)
            {
                string allowableLetters = "\"ab";
                foreach (char c in format)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                        return false;
                }
                return true;
            }

            bool ReadRules(string regel)
            {
                // inlezen regels
                var index = regel.IndexOf(':');
                var regelNummer = int.Parse(regel.Substring(0, index));
                var regelOverig = regel.Substring(regel.IndexOf(':') + 2);
                if (FormatValid(regelOverig))
                {
                    uitgewerkteRegels.Add(regelNummer, new UitgewerkteRegel(regelNummer, regelOverig));
                }
                else
                {
                   nieuweRegels.Add(regelNummer,new NieuweRegel(regelNummer, regelOverig));
                }
                return true;
            }
            int BepaalTwee( int nummer42, int nummer31)
            {
                var messagesLeft = new List<string>(messages);
                var searchListBegin = uitgewerkteRegels[nummer42].Regels;
                var searchListEind = uitgewerkteRegels[nummer31].Regels;

                messagesLeft = messagesLeft.Where(m => searchListBegin.Any(r => m.StartsWith(r))).ToList();
                messagesLeft = messagesLeft.Where(m => searchListEind.Any(r => m.EndsWith(r))).ToList();
              
                var searchList = new List<string>(searchListBegin.SelectMany(first => searchListBegin, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).Distinct().ToList());
                //validatie
                searchList = searchList.Where(r => messages.Any(m => m.Contains(r))).ToList();
                messagesLeft = messagesLeft.Where(m => searchList.Any(r => m.Contains(r))).ToList();

                searchList = searchList.SelectMany(first => searchListEind, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).Distinct().ToList();
                //validatie
                searchList = searchList.Where(r => messages.Any(m => m.Contains(r))).ToList();
                messagesLeft = messagesLeft.Where(m => searchList.Any(r => m.Contains(r))).ToList();

                var totalSearchlist = new List<string>(searchList);
                var teller = messagesLeft.Count();
                Console.WriteLine($"Ronde 1: Teller regel 42: {teller}, messages over: {messagesLeft.Count}");
                while (teller > 0)
                {
                    searchList = searchListBegin.SelectMany(first => searchList, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).ToList();
                    //validatie
                    searchList = searchList.Where(r => messages.Any(m => m.Contains(r))).ToList();
                    messagesLeft = messagesLeft.Where(m => searchList.Any(r => m.Contains(r))).ToList();

                    searchList = searchList.SelectMany(first => searchListEind, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).ToList();
                    //validatie
                    searchList = searchList.Where(r => messages.Any(m => m.Contains(r))).ToList();
                    messagesLeft = messagesLeft.Where(m => searchList.Any(r => m.Contains(r))).ToList();

                    teller = messagesLeft.Count;
                    totalSearchlist.AddRange(searchList);
                    Console.WriteLine($"Teller regel 42: {teller}, messages over: {messagesLeft.Count}");
                }
                messagesLeft = new List<string>(messages.Where(m => totalSearchlist.Any(r => m.EndsWith(r))));
                Console.WriteLine($"Regel 42 op messages uitvoeren, messages over: {messagesLeft.Count}");

                searchList = totalSearchlist;
                teller = messagesLeft.Count();
                while (teller > 0)
                {
                    searchList = searchListBegin.SelectMany(first => searchList, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).ToList();

                    searchList = searchList.Where(r => messages.Any(m => m.EndsWith(r))).ToList();
                    messagesLeft = messagesLeft.Where(m => searchList.Any(r => m.EndsWith(r))).ToList();

                    teller = messagesLeft.Count;
                    totalSearchlist.AddRange(searchList);
                    Console.WriteLine($"Teller regel 31: {teller}, messages over: {messagesLeft.Count}");
                }
                messagesLeft = new List<string>(messages.Where(r => totalSearchlist.Any(f => r.EndsWith(f))));
                Console.WriteLine($"Teller regel 31  is klaar, nog {messagesLeft.Count} messages over");

                messagesLeft = new List<string>(messages.Where(m => totalSearchlist.Any(r => m == r)));
                return messagesLeft.Count();
            }
        }
    }

    public class NieuweRegel
    {
        public int Regelnummer { get; }
        public List<List<int>> Regels {get;}
        public NieuweRegel(int nummer, string regelOverig)
        {
            Regels = new List<List<int>>();
            Regelnummer = nummer;
            var regelEnOf = regelOverig.Split(" | ");
            for (var i = 0; i < regelEnOf.Count(); i++)
            {
                Regels.Add(regelEnOf[i].Split(' ').Select(int.Parse).ToList());
            } 
        }
        public UitgewerkteRegel ParseRegel(Dictionary<int, UitgewerkteRegel> uitgewerkteRegels)
        {
            var mixen = new List<List<string>>();
            if (KanUitgewerktWorden(uitgewerkteRegels))
            {
                var setMix = new List<List<string>>();
                for (var set = 0; set < Regels.Count; set++)
                {
                    if (Regels[set].Count() == 1)
                    {
                        var mix = new List<string>(uitgewerkteRegels[Regels[set][0]].Regels);
                        setMix.Add(mix);
                    }
                    else
                    {
                        var mix = new List<string>();
                        var setA = uitgewerkteRegels[Regels[set][0]].Regels;
                        for (var getal = 1; getal < Regels[set].Count(); getal++)
                        {
                            var setB = uitgewerkteRegels[Regels[set][getal]].Regels;
                            mix = setA.SelectMany(first => setB, (first, second) => new { first, second }).Select(x => String.Concat(x.first, x.second)).ToList();
                            setA = mix;
                        }
                        setMix.Add(mix);
                    }   
                }
                //  sets optellen
                var alleUitgewerkteregels = new List<string>();
                for (var m = 0; m< setMix.Count; m++)
                {
                    alleUitgewerkteregels.AddRange(setMix[m]);
                }
                return new UitgewerkteRegel(Regelnummer, alleUitgewerkteregels);
            }
            else
            {
                return null;
            }
        }
        bool KanUitgewerktWorden(Dictionary<int, UitgewerkteRegel> uitgewerkteRegels)
        {
            bool kanUitgewerktWorden = true;
            var uitTeWerken = Regels.SelectMany(y => y).Distinct().ToList();

            foreach (var key in uitTeWerken)
            {
                if (!uitgewerkteRegels.ContainsKey(key))
                { kanUitgewerktWorden = false; }
            }
            return kanUitgewerktWorden;
        }
    }
    public class UitgewerkteRegel
    {
        public int Regelnummer { get;  }
        public List<String> Regels { get; }
        public UitgewerkteRegel(int nummer, string regelOverig)
        {
            Regels = new List<string>();
            Regelnummer = nummer;
            Regels.Add(regelOverig.Substring(1, regelOverig.Length - 2));
        }
        public UitgewerkteRegel(int nummer, List<string>regels)
        {
            Regels = regels;
            Regelnummer = nummer;
        }
    }
}
