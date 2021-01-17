﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_20
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Inputtest.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var listOfTiles = new List<Tile>();
            var regel = reader.ReadLine();
            var gevondenWaardenZijkanten = new Dictionary<int, List<int>>();

            // Inlezen tiles
            while (regel != null)
            {
                var tile = new List<string>();
                for (var i = 0; i < 11; i++)
                {
                    tile.Add(regel);
                    regel = reader.ReadLine();
                }
                listOfTiles.Add(new Tile(tile));
                regel = reader.ReadLine();
            }
            Console.WriteLine("De tiles zijn ingelezen");
            // Uitwerking deel 1
            gevondenWaardenZijkanten = GetSideValues(listOfTiles);
            Console.WriteLine("Dictionary is gevuld");
            var hoeken = gevondenWaardenZijkanten
                .Where(x => x.Value.Count() <= 1)
                .Select(b => b.Value[0])
                .ToList()
                .GroupBy(i => i)
                .Where(x => x.Count() == 4)
                .Select(x => x.Key)
                .ToList();
            var oplossing1 = hoeken.Aggregate(1L, (current, item) => current * item);
            Console.WriteLine($"Oplossing 1: {oplossing1}");
            Console.WriteLine();

            // deel 2
            // variabelen nodig om afbeelding op te bouwen
            var afbeeldingOpdracht1 = new List<List<PlacedTile>>();
            var afbeeldingsRij = new List<PlacedTile>();
            var tilesToPlace = new Dictionary<int, Tile>();
            foreach (Tile tile in listOfTiles) tilesToPlace.Add(tile.TileNumber, tile);
            var dimAfbeelding = (int)Math.Sqrt(listOfTiles.Count);
            // linkerbovenhoek kiezen
            var gekozenHoek = hoeken[0];
            //Console.WriteLine($"Gekozen hoek: {gekozenHoek}");


            // orientatie gekozen hoek bepalen
            var zijdenGekozenHoek = gevondenWaardenZijkanten
                .Where(x => x.Value.Count() <= 1)
                .Select(x => new { x.Key, x.Value })
                .Where(x => x.Value[0] == gekozenHoek)
                .Select(x => x.Key)
                .ToList();
            // de orientatie is de zijde die eigenlijk boven zou moeten liggen
            Orientatie linksboven = tilesToPlace[gekozenHoek].DraaiHoek(zijdenGekozenHoek);
            // tiles in nieuwe lijst plaatsen
            var huidigeTile = tilesToPlace[gekozenHoek]; //1951
            var huidigeOrientatie = linksboven;


            // maak afbeelding
            int waardeNaarBeneden = 0;
            for (var j = 0; j < dimAfbeelding; j++)
            {
                for (var i = 0; i < dimAfbeelding; i++)
                {
                    var geplaatsteTile = new PlacedTile(huidigeTile, huidigeOrientatie);
                    //Console.WriteLine($"Ronde {i} Tilenummer: {geplaatsteTile.TileNumber} {huidigeOrientatie}");
                    //geplaatsteTile.PrintPlacedTile();
                    //Console.WriteLine();
                    afbeeldingsRij.Add(geplaatsteTile);
                    tilesToPlace.Remove(huidigeTile.TileNumber);
                    if (i == 0) waardeNaarBeneden = geplaatsteTile.SearchDown;
                    if (i < dimAfbeelding - 1)
                    {
                        huidigeTile = tilesToPlace[tilesToPlace.Where(x => x.Value.TileSides.Contains(geplaatsteTile.SearchRight)).Select(x => x.Value.TileNumber).ToList().First()];//tile die past
                        var error = tilesToPlace.Where(x => x.Value.TileSides.Contains(geplaatsteTile.SearchRight)).Select(x => x.Value.TileNumber).ToList().Count();//meerdere tiles
                        if (error > 1) Console.WriteLine("Ojeetje");
                        var toppositie = (Orientatie)huidigeTile.TileSides.IndexOf(huidigeTile.TileSides.Where(p => p == geplaatsteTile.SearchRight).FirstOrDefault());
                        switch (toppositie)
                        {
                            case Orientatie.Top:
                                toppositie = Orientatie.Right;
                                break;
                            case Orientatie.Right:
                                toppositie = Orientatie.Bottom;
                                break;
                            case Orientatie.Bottom:
                                toppositie = Orientatie.Left;
                                break;
                            case Orientatie.Left:
                                toppositie = Orientatie.Top;
                                break;
                            case Orientatie.TopReverse:
                                toppositie = Orientatie.LeftReverse;
                                break;
                            case Orientatie.RightReverse:
                                toppositie = Orientatie.TopReverse;
                                break;
                            case Orientatie.BottomReverse:
                                toppositie = Orientatie.RightReverse;
                                break;
                            case Orientatie.LeftReverse:
                                toppositie = Orientatie.BottomReverse;
                                break;
                            default:
                                Console.WriteLine("Dit is niet goed");
                                break;
                        }
                        huidigeOrientatie = (Orientatie)toppositie;
                    }
                    else
                    {
                        if (j < (dimAfbeelding - 1))
                        {
                            huidigeTile = tilesToPlace[tilesToPlace.Where(x => x.Value.TileSides.Contains(waardeNaarBeneden)).Select(x => x.Value.TileNumber).ToList().First()];//tile die past
                            var toppositie = huidigeTile.TileSides.IndexOf(huidigeTile.TileSides.Where(p => p == waardeNaarBeneden).FirstOrDefault());
                            huidigeOrientatie = (Orientatie)toppositie;
                        }
                    }
                    //Console.WriteLine();
                }
                afbeeldingOpdracht1.Add(afbeeldingsRij);
                afbeeldingsRij = new List<PlacedTile>();
            }

            var afbeeldingOpdracht2 = ConvertAfbeelding(afbeeldingOpdracht1);
            //PrintAfbeelding(afbeeldingOpdracht2, dimAfbeelding*8, dimAfbeelding*8);
            Console.WriteLine("De opdracht is klaar");

            char[,] seaMonster = new char[,]
            {{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0'},
             {'1', '0', '0', '0', '0', '1', '1', '0', '0', '0', '0', '1', '1', '0', '0', '0', '0', '1', '1', '1'},
             {'0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '0'}};

            char[,] seaMonster2 = new char[,]
            {{' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', ' '},
             {'1', ' ', ' ', ' ', ' ', '1', '1', ' ', ' ', ' ', ' ', '1', '1', ' ', ' ', ' ', ' ', '1', '1', '1'},
             {' ', '1', ' ', ' ', '1', ' ', ' ', '1', ' ', ' ', '1', ' ', ' ', '1', ' ', ' ', '1', ' ', ' ', ' '}};
            PrintAfbeelding(seaMonster2, 20, 3);





            char[,] ConvertAfbeelding(List<List<PlacedTile>> afbeelding)
            {
                // Nieuwe afbeelding met buitenste waardes eruit maken om deel 2 te berekenen
                char[,] convertedAfbeelding = new char[8 * (int)dimAfbeelding, 8 * (int)dimAfbeelding];
                for (var y = 0; y < dimAfbeelding; y++)
                {
                    for (var x = 0; x < dimAfbeelding; x++)
                    {
                        // zet waarde uit Afbeelding in afbeeldingOpdracht2
                        for (var y2 = 0; y2 < 8; y2++)
                        {
                            for (var x2 = 0; x2 < 8; x2++)
                            {
                                convertedAfbeelding[x2 + x * 8, y2 + y * 8] = afbeelding[y][x].PlacedTileLayout[x2 + 1, y2 + 1];
                            }
                        }
                    }
                }
                return convertedAfbeelding;
            }

            bool PrintAfbeelding(char[,] afbeelding, int dimX, int dimY)
            {
                for (var y = 0; y < dimY; y++)
                {
                    for (var x = 0; x < dimX; x++)
                    {
                        Console.Write(afbeelding[y, x]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                return true;
            }
            Dictionary<int, List<int>> GetSideValues(List<Tile> tiles)
            {
                var gevondenWaarden = new Dictionary<int, List<int>>();
                foreach (Tile tile in tiles)
                {
                    foreach (int side in tile.TileSides)
                    {
                        if (gevondenWaarden.ContainsKey(side))
                            gevondenWaarden[side].Add(tile.TileNumber);
                        else
                            gevondenWaarden.Add(side, new List<int>() { tile.TileNumber });
                    }
                }
                return gevondenWaarden;
            }
        }
    }


    public class Puzzle
    {

    }
    public class PlacedTile
    {
        public int TileNumber { get; }
        public char[,] PlacedTileLayout { get; }
        public int SearchDown { get; }
        public int SearchRight { get; }
        public PlacedTile(Tile tile, Orientatie orientatie)
        {
            TileNumber = tile.TileNumber;
            char[,] placedTileLayout = new char[10, 10];
            int searchDown;
            int searchRight;
            switch (orientatie)
            {
                case Orientatie.Top:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[y][x];
                        }
                    }
                    searchRight = tile.RightReverse;
                    searchDown = tile.BottomReverse;
                    break;
                case Orientatie.Right:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[x][9 - y];
                        }
                    }
                    searchRight = tile.BottomReverse;
                    searchDown = tile.LeftReverse;
                    break;
                case Orientatie.Bottom:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[9 - y][9 - x];
                        }
                    }
                    searchRight = tile.LeftReverse;
                    searchDown = tile.TopReverse;
                    break;
                case Orientatie.Left:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[9 - x][y];
                        }
                    }
                    searchRight = tile.TopReverse;
                    searchDown = tile.RightReverse;
                    break;
                case Orientatie.TopReverse:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[y][9 - x];
                        }
                    }
                    searchRight = tile.Left;
                    searchDown = tile.Bottom;
                    break;
                case Orientatie.RightReverse:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[9 - x][9 - y];
                        }
                    }
                    searchRight = tile.Top;
                    searchDown = tile.Left;
                    break;
                case Orientatie.BottomReverse:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[9 - y][x];
                        }
                    }
                    searchRight = tile.Right;
                    searchDown = tile.Top;
                    break;
                case Orientatie.LeftReverse:
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            placedTileLayout[x, y] = tile.TileLayout[x][y];
                        }
                    }
                    searchRight = tile.Bottom;
                    searchDown = tile.Right;
                    break;
                default:
                    Console.WriteLine("PlacedTile: Hier hoor je niet te komen");
                    searchDown = tile.BottomReverse;
                    searchRight = tile.RightReverse;
                    break;
            }
            PlacedTileLayout = placedTileLayout;
            SearchDown = searchDown;
            SearchRight = searchRight;
        }

        public bool PrintPlacedTile()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Console.Write($"{PlacedTileLayout[x, y]}");
                }
                Console.WriteLine();
            }
            return true;
        }
    }

    public class Tile
    {
        public int TileNumber { get; }
        public List<string> TileLayout { get; }
        public List<int> TileSides { get; }
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public int Left { get; }
        public int TopReverse { get; }
        public int RightReverse { get; }
        public int BottomReverse { get; }
        public int LeftReverse { get; }
        public Tile(List<string> regels)
        {
            TileNumber = int.Parse((regels.First().Substring(5, 4)));
            TileLayout = new List<string>();
            string row = "";
            string rowReverse = "";
            string columnFirst = "";
            string columnLast = "";
            string columnFirstReverse = "";
            string columnLastReverse = "";
            for (var i = 1; i < 11; i++) //regels
            {
                for (var j = 0; j < 10; j++) //kolommen
                {
                    if (regels[i][j] == '#')
                    {
                        row += "1";
                        rowReverse = "1" + rowReverse;
                    }
                    else
                    {
                        row += "0";
                        rowReverse = "0" + rowReverse;
                    }
                }
                TileLayout.Add(row);
                if (i == 1)
                {
                    Top = Convert.ToInt32(row, 2);
                    TopReverse = Convert.ToInt32(rowReverse, 2);
                }
                if (i == 10)
                {
                    BottomReverse = Convert.ToInt32(row, 2);
                    Bottom = Convert.ToInt32(rowReverse, 2);
                }
                columnFirst += row[0];
                columnLast += row[9];
                columnFirstReverse = row[0] + columnFirstReverse;
                columnLastReverse = row[9] + columnLastReverse;
                row = "";
                rowReverse = "";
            }
            Right = Convert.ToInt32(columnLast, 2);
            RightReverse = Convert.ToInt32(columnLastReverse, 2);
            LeftReverse = Convert.ToInt32(columnFirst, 2);
            Left = Convert.ToInt32(columnFirstReverse, 2);
            TileSides = new List<int>()
            {
                Top,
                Right,
                Bottom,
                Left,
                TopReverse,
                RightReverse,
                BottomReverse,
                LeftReverse
            };
        }
        public bool PrintTile()
        {
            for (var i = 0; i < TileLayout.Count; i++)
            {
                Console.WriteLine(TileLayout[i]);
            }
            return true;
        }
        public Orientatie DraaiHoek(List<int> zijden)
        {
            Orientatie bovenkant = Orientatie.Top;
            if (zijden.IndexOf(Top) != -1 && zijden.IndexOf(Right) != -1) bovenkant = Orientatie.Right;
            if (zijden.IndexOf(Top) != -1 && zijden.IndexOf(Left) != -1) bovenkant = Orientatie.Top;
            if (zijden.IndexOf(Bottom) != -1 && zijden.IndexOf(Right) != -1) bovenkant = Orientatie.Bottom;
            if (zijden.IndexOf(Bottom) != -1 && zijden.IndexOf(Left) != -1) bovenkant = Orientatie.Left;
            return bovenkant;
        }
    }
    public enum Orientatie
    {
        Top,
        Right,
        Bottom,
        Left,
        TopReverse,
        RightReverse,
        BottomReverse,
        LeftReverse
    }
}
