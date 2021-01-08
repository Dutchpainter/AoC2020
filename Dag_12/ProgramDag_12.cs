using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_12
{
    class Program
    {
        static void Main(string[] args)
        {    
            var lines = File
                   .ReadAllLines("Input.txt")
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToList();

            var instructions = lines.Select(l => new Instructie(l)).ToList();

            // opgave deel 1
            var status = new Status(0, 0, BootRichting.East);
            foreach (var instruction in instructions)
            {
                instruction.PrintInstructie();
                status = instruction.BerekenNieuweStatus(status);
                status.PrintStatus();
            }
            var oplossing1 =  Math.Abs(status.BootPosEast) + Math.Abs(status.BootPosNorth);
            Console.WriteLine($"De Manhattan distance  van deel 1 is {oplossing1}");
            Console.WriteLine();

            // opgave deel 2
            status = new Status(0, 0, BootRichting.East, 10, 1);
            foreach (var instruction in instructions)
            {
                instruction.PrintInstructie();
                status = instruction.BerekenNieuweStatusDeel2(status);
                status.PrintStatus();
            }
            var oplossing2 = Math.Abs(status.BootPosEast) + Math.Abs(status.BootPosNorth);
            Console.WriteLine($"De Manhattan distance  van deel 2 is {oplossing2}");
            Console.WriteLine();
            Console.WriteLine("Opgave 12 klaar");
        }
    }

    class Instructie
    {
        // hier worden de instructies ingelezen
        public Status status { get;  }
        public Commando Commando { get; }
        public int Waarde { get; }
        public Instructie(string regel)
        {
            Commando = HelperFuncties.ParseCom(regel.First());
            Waarde = int.Parse(regel.Substring(1));
        }
        public void PrintInstructie()
        {
            Console.WriteLine($"Het gelezenc commando is: {Commando} {Waarde}.");
        }
        public Status BerekenNieuweStatus(Status oudeStatus)
        {
            var Eastpos = oudeStatus.BootPosEast;
            var Northpos = oudeStatus.BootPosNorth;
            var Richting = oudeStatus.BootRichting;
            switch (Commando)
            {
                case Commando.East:
                    Eastpos += Waarde;
                    break;
                case Commando.North:
                    Northpos += Waarde;
                    break;
                case Commando.West:
                    Eastpos -= Waarde;
                    break;
                case Commando.South:
                    Northpos -= Waarde;
                    break;
                case Commando.Left:
                    // linksom, stappen terug south-west-north-east
                    Richting = (BootRichting)(((int)Richting - (Waarde / 90) + 4) % 4);
                    break;
                case Commando.Right:
                    // rechtsom, stappen verder north-west-south-east
                    Richting = (BootRichting)(((int)Richting + (Waarde / 90) + 4) % 4);
                    break;
                case Commando.Forward:
                    switch (Richting)
                    {
                        case BootRichting.East:
                            Eastpos += Waarde;
                            break;
                        case BootRichting.North:
                            Northpos += Waarde;
                            break;
                        case BootRichting.West:
                            Eastpos -= Waarde;
                            break;
                        case BootRichting.South:
                            Northpos -= Waarde;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Unknown commando");
                    break;
            }
            return new Status(Eastpos, Northpos, Richting);
        }
        public Status BerekenNieuweStatusDeel2(Status oudeStatus)
        {
            var Eastpos = oudeStatus.BootPosEast;
            var Northpos = oudeStatus.BootPosNorth;
            var Richting = oudeStatus.BootRichting;
            var WPEastpos = oudeStatus.WaypointPosEast;
            var WPNorthpos = oudeStatus.WaypointPosNorth;
            switch (Commando)
            {
                case Commando.East:
                    WPEastpos += Waarde;
                    break;
                case Commando.North:
                    WPNorthpos += Waarde;
                    break;
                case Commando.West:
                    WPEastpos -= Waarde;
                    break;
                case Commando.South:
                    WPNorthpos -= Waarde;
                    break;
                case Commando.Left:
                    // linksom, stappen terug south-west-north-east
                    var left = Waarde / 90 % 4;
                    var help = 0;
                    switch (left)
                    {
                        case 1:
                            help = WPEastpos;
                            WPEastpos = -WPNorthpos;
                            WPNorthpos = help;
                            break;
                        case 2:
                            WPEastpos = -WPEastpos;
                            WPNorthpos =-WPNorthpos;
                            break;
                        case 3:
                            help = -WPEastpos;
                            WPEastpos = WPNorthpos;
                            WPNorthpos = help;
                            break;
                        case 4:
                            // niets doen.
                            break;
                        default:
                            Console.WriteLine("Hier gaat iets fout...");
                            break;
                    }
                    break;
                case Commando.Right:
                    // rechtsom, stappen verder north-west-south-east
                    var right = Waarde / 90 % 4;
                    switch (right)
                    {
                        case 1:
                            help = -WPEastpos;
                            WPEastpos = WPNorthpos;
                            WPNorthpos = help;
                            break;
                        case 2:
                            WPEastpos = -WPEastpos;
                            WPNorthpos = -WPNorthpos;
                            break;
                        case 3:
                            help = WPEastpos;
                            WPEastpos = -WPNorthpos;
                            WPNorthpos = help;
                            break;
                        case 4:
                            // niets doen.
                            break;
                        default:
                            Console.WriteLine("Hier gaat iets fout...");
                            break;
                    }

                    break;
                case Commando.Forward:
                    Eastpos += Waarde * WPEastpos;
                    Northpos += Waarde * WPNorthpos;
                    break;
                default:
                    Console.WriteLine("Unknown commando");
                    break;
            }
            return new Status(Eastpos, Northpos, Richting, WPEastpos, WPNorthpos);
        }
    }

    class Status
    {
        public int BootPosEast { get; }
        public int BootPosNorth { get; }
        public BootRichting BootRichting { get; }
        public int WaypointPosEast { get; }
        public int WaypointPosNorth { get; }
        public Status (int bootPosEast, int bootPosNorth, BootRichting bootRichting, int waypointPosEast = 0, int waypointPosNorth = 0)
        {
            BootPosEast = bootPosEast;
            BootPosNorth = bootPosNorth;
            BootRichting = bootRichting;
            WaypointPosEast = waypointPosEast;
            WaypointPosNorth = waypointPosNorth;
        }
        public void PrintStatus()
        {
            Console.WriteLine($"De boot bevindt zich nu op {(BootPosEast <= 0 ? "West":"East") }:" +
                $" {(BootPosEast < 0 ? Math.Abs(BootPosEast): BootPosEast)} " +
                $"{(BootPosNorth <= 0 ? "South":"North")}: " +
                $"{(BootPosNorth < 0 ? Math.Abs(BootPosNorth) : BootPosNorth)} " +
                $"en vaart richting {BootRichting}."
            );
        }
    }
    public static class HelperFuncties
    {
        public static BootRichting ParseBR(char c)
        {
            return c switch
            {
                'E' => BootRichting.East,
                'S' => BootRichting.South,
                'W' => BootRichting.West,
                'N' => BootRichting.North,
                _ => throw new ArgumentOutOfRangeException("c", c, null),
            };
        }
        public static Commando ParseCom(char c)
        {
            return c switch
            {
                'E' => Commando.East,
                'S' => Commando.South,
                'W' => Commando.West,
                'N' => Commando.North,
                'L' => Commando.Left,
                'R' => Commando.Right,
                'F' => Commando.Forward,
                _ => throw new ArgumentOutOfRangeException("c", c, null),
            };
        }
    }
    public enum BootRichting
    {
        East,
        South,
        West,
        North
    }
    public enum Commando
    {
        East,
        South,
        West,
        North,
        Left,
        Right,
        Forward
    }
}
