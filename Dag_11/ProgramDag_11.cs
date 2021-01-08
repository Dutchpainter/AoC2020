using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_11
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inlezen bestand
            var OriginalHal = File
                   .ReadAllLines("Input.txt")
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToList();

            Stoelen eerstePositie = new Stoelen(OriginalHal);
            eerstePositie.PrintStoelen();

            // opdracht 1
            var i = 0;
            var situatie = eerstePositie;
            do
            {
                situatie = situatie.nieuweSituatie();
                Console.WriteLine($"opdracht 1 ronde: {i}");
                i++;
            }
            while (situatie.IsVeranderd);

            var opdracht1 = situatie.stoelen.Where(stoel => stoel.StoelState == StoelState.Bezet).Count();
            Console.WriteLine($"Opdracht 1: {opdracht1}");

            // opdracht 2
            var j = 0;
            var situatie2 = eerstePositie;
            do
            {
                situatie2 = situatie2.nieuwereSituatie();
                Console.WriteLine($"opdracht 2 ronde: {j}");
                //situatie.PrintStoelen();
                j++;
            }
            while (situatie2.IsVeranderd);

            var opdracht2 = situatie2.stoelen.Where(stoel => stoel.StoelState == StoelState.Bezet).Count();
            Console.WriteLine($"Opdracht 2:  {opdracht2}");
        }
    }

    public class Stoelen
    {
        public List<Stoel> stoelen { get;  }
        public bool IsVeranderd { get; }
        public Stoelen(List<String> hal)
        {
            var Hal = hal;
            char c;
            IsVeranderd = true;
            stoelen = new List<Stoel>();
            for (var i = 0; i < Hal.Count; i++)
            {
                for (var j = 0; j < Hal.First().Length; j++)
                {
                    c = Hal[i][j];
                    stoelen.Add(new Stoel(j, i, c));
                }
            }
        }

        public Stoelen(IEnumerable<Stoel> alleStoelen, bool isVeranderd)
        {
            IsVeranderd = isVeranderd;
            stoelen = alleStoelen.ToList();
        }

        public bool PrintStoelen()
        {
            Console.WriteLine();
            var breedte = stoelen.Select(stoel => stoel.XPos).Max();
            var hoogte = stoelen.Select(stoel => stoel.YPos).Max();
            for (var y = 0; y <= hoogte; y++)
            {
                for (var x = 0; x <= breedte; x++)
                {
                    var stoel = stoelen.Where(stoel => stoel.XPos == x && stoel.YPos == y);
                    Console.Write(HelperFuncties.ConvertToString(stoel.Single().StoelState));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return true;
        }

        public int VindVerreBuren(Stoel dezestoel)
        {
            // Vind de buren voor een enkele stoel
            var Buren = new List<Stoel>();
            var xpos = dezestoel.XPos;
            var ypos = dezestoel.YPos;
            var breedte = stoelen.Select(stoel => stoel.XPos).Max();
            var hoogte = stoelen.Select(stoel => stoel.YPos).Max();
            // kijken welke stoelen je nodig hebt en in lijst zetten
            // stoel links
            var i = 1;
            while(xpos - i >= 0 && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos - i && stoel.YPos == ypos && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer )).FirstOrDefault();
                if (buurStoel != null)
                {
                    if(!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    } else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            // stoel rechts
            i = 1;
            while (xpos + i <= breedte && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos + i && stoel.YPos == ypos && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            //  stoel boven
            i = 1;
            while (ypos - i >= 0 && i!=0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos && stoel.YPos == ypos - i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            //  stoel beneden
            i = 1;
            while (ypos + i <= hoogte && i!=0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos && stoel.YPos == ypos + i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            // stoel linksboven
            i = 1;
            while (xpos - i >= 0 &&  ypos - i >= 0 && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos - i && stoel.YPos == ypos - i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            // stoel rechtsboven
            i = 1;
            while (xpos + i <= breedte && ypos - i >= 0 && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos + i && stoel.YPos == ypos - i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            //  stoel linksonder
            i = 1;
            while (xpos - i >= 0 && ypos + i <= hoogte && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos - i && stoel.YPos == ypos + i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }
            //  stoel rechtsonder
            i = 1;
            while (xpos + i <= breedte && ypos + i <= hoogte && i != 0)
            {
                var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos + i && stoel.YPos == ypos + i && (stoel.StoelState == StoelState.Bezet || stoel.StoelState == StoelState.Vrij || stoel.StoelState == StoelState.Vloer)).FirstOrDefault();
                if (buurStoel != null)
                {
                    if (!(buurStoel.StoelState == StoelState.Vloer))
                    {
                        Buren.Add(buurStoel);
                        i = 0;
                    }
                    else { i++; }
                }
                else
                {
                    i = 0;
                }
            }

            return (Buren
                .Where(buur => buur.StoelState == StoelState.Bezet)
                .Count());
        }


        public int VindBuren(Stoel dezestoel)
        {
            // Vind de buren voor een enkele stoel
            var Buren = new List<Stoel>();
            var xpos = dezestoel.XPos;
            var ypos = dezestoel.YPos;
            // kijken welke stoelen je nodig hebt en in lijst zetten
            for (var y = -1; y <= 1; y++)
            {
                for (var x = -1; x <= 1; x++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        var buurStoel = stoelen.Where(stoel => stoel.XPos == xpos + x && stoel.YPos == ypos + y).FirstOrDefault();
                        if (buurStoel != null) Buren.Add(buurStoel);
                    }
                }
            }
            return (Buren
                .Where(buur => buur.StoelState == StoelState.Bezet)
                .Count());
        }

        public Stoelen nieuwereSituatie()
        {
            var NieuweSituatie = new List<Stoel>();
            Stoel nieuweStoel;
            var isVeranderd = false;
            var vorigeStoelen = stoelen.ToList();

            foreach (Stoel dezeStoel in vorigeStoelen)
            {
                if (dezeStoel.StoelState == StoelState.Vrij)
                {
                    if (VindVerreBuren(dezeStoel) == 0)
                    {
                        // stoel wordt bezet
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Bezet);
                        NieuweSituatie.Add(nieuweStoel);
                        isVeranderd = true;
                    }
                    else
                    {
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Vrij);
                        NieuweSituatie.Add(nieuweStoel);
                    }
                }
                if (dezeStoel.StoelState == StoelState.Bezet)
                {
                    if (VindVerreBuren(dezeStoel) >= 5)
                    {
                        // stoel wordt leeg
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Vrij);
                        NieuweSituatie.Add(nieuweStoel);
                        isVeranderd = true;
                    }
                    else
                    {
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Bezet);
                        NieuweSituatie.Add(nieuweStoel);
                    }
                }
                if (dezeStoel.StoelState == StoelState.Vloer)
                {
                    nieuweStoel = new Stoel(dezeStoel, StoelState.Vloer);
                    NieuweSituatie.Add(nieuweStoel);
                }
            }
            return new Stoelen(NieuweSituatie, isVeranderd);
        }

        public Stoelen nieuweSituatie()
        {
            var NieuweSituatie = new List<Stoel>();
            Stoel nieuweStoel;
            var isVeranderd = false;
            var vorigeStoelen = stoelen.ToList();

            foreach (Stoel dezeStoel in vorigeStoelen)
            {
                if (dezeStoel.StoelState == StoelState.Vrij)
                {
                    if (VindBuren(dezeStoel) == 0)
                    {
                        // stoel wordt bezet
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Bezet);
                        NieuweSituatie.Add(nieuweStoel);
                        isVeranderd = true;
                    }
                    else
                    {
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Vrij);
                        NieuweSituatie.Add(nieuweStoel);       
                    }
                }
                if (dezeStoel.StoelState == StoelState.Bezet)
                {
                    if (VindBuren(dezeStoel) >= 4)
                    {
                        // stoel wordt leeg
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Vrij);
                        NieuweSituatie.Add(nieuweStoel);
                        isVeranderd = true;
                    }
                    else
                    {
                        nieuweStoel = new Stoel(dezeStoel, StoelState.Bezet);
                        NieuweSituatie.Add(nieuweStoel);
                    }              
                }
                if (dezeStoel.StoelState == StoelState.Vloer)
                {
                    nieuweStoel = new Stoel(dezeStoel, StoelState.Vloer);
                    NieuweSituatie.Add(nieuweStoel);
                }
            }
            return new Stoelen(NieuweSituatie, isVeranderd);
        }
    }




   public class Stoel
    {
        public int XPos { get; }
        public int YPos { get;  }
        public StoelState StoelState { get; }
        public Stoel(int xpos, int ypos, char c)
        {
            XPos = xpos;
            YPos = ypos;
            StoelState = HelperFuncties.Parse(c);
        }
        public Stoel(Stoel oldStoel, StoelState newState)
        {
            XPos = oldStoel.XPos;
            YPos = oldStoel.YPos;
            StoelState = newState;
        }
    }

    public enum StoelState
    {
        Vloer,
        Bezet,
        Vrij
    }

    public static class HelperFuncties
    {
        public static StoelState Parse(char c)
        {
            return c switch
            {
                '.' => StoelState.Vloer,
                '#' => StoelState.Bezet,
                'L' => StoelState.Vrij,
                _ => throw new ArgumentOutOfRangeException("c", c, null),
            };
        }

        public static string ConvertToString(StoelState ch)
        {
            return ch switch
            {
                StoelState.Vloer => ".",
                StoelState.Bezet => "#",
                StoelState.Vrij => "L",
                _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
            };
        }
    }
}