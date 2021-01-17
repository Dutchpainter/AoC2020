using System;
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
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var listOfTiles = new List<Tile>();
            var regel = reader.ReadLine();

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
            var dezePuzzel = new Puzzle(listOfTiles);
            var oplossing1 = dezePuzzel.Hoeken.Aggregate(1L, (current, item) => current * item);
            Console.WriteLine($"Oplossing 1: {oplossing1}");
            Console.WriteLine();
            //dezePuzzel.PrintAfbeelding();
            char[,] seaMonster = new char[,]
            {{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0'},
             {'1', '0', '0', '0', '0', '1', '1', '0', '0', '0', '0', '1', '1', '0', '0', '0', '0', '1', '1', '1'},
             {'0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '1', '0', '0', '0'}};
            var deMonsters = new SeaMonsters(seaMonster);
            var oplossing2 = dezePuzzel.TelMonsters(deMonsters);
            var aantalMonsters = oplossing2[0];
            var roughness = oplossing2[1];
            Console.WriteLine($"Opdracht 2 aantal monsters: {aantalMonsters}");
            Console.WriteLine($"Opdracht 2 roughness: {roughness}");
            Console.WriteLine("De opdracht is klaar");


        }
    }
    public class SeaMonster
    {
        public List<(int XValue, int YValue)> Definition { get; }
        public int XMax { get; }
        public int YMax { get; }
        public SeaMonster(List<(int, int)> monster)
        {
            Definition = new List<(int, int)>(monster);
            XMax = Definition.Select(m => m.XValue).Max() + 1;
            YMax = Definition.Select(m => m.YValue).Max() + 1;
        }
    }

    public class SeaMonsters
    {
        public List<SeaMonster> Monsters { get; }
        public SeaMonsters(char[,] monster)
        {
            //maak verschillende monsters aan, met behulp van array
            var maxY = monster.GetLength(1); // Kolommen
            var maxX = monster.GetLength(0); // Rijen
            Monsters = new List<SeaMonster> ();
            var monster0 = new List<(int x, int y)>();
            var monster1 = new List<(int x, int y)>();
            var monster2 = new List<(int x, int y)>();
            var monster3 = new List<(int x, int y)>();
            var monster4 = new List<(int x, int y)>();
            var monster5 = new List<(int x, int y)>();
            var monster6 = new List<(int x, int y)>();
            var monster7 = new List<(int x, int y)>();
            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    if( monster[x,y] == '1')
                    {
                        monster0.Add((x, y));
                        monster1.Add((maxY - y - 1, x));
                        monster2.Add((maxX - x - 1, maxY - y - 1));
                        monster3.Add((y, maxX - x - 1));
                        monster4.Add((y, x));
                        monster5.Add((maxX - x - 1, y));
                        monster6.Add((maxY - y - 1, maxX - x - 1));
                        monster7.Add((x, maxY - y - 1));
                    }
                }
            }
            Monsters.Add(new SeaMonster(monster0));
            Monsters.Add(new SeaMonster(monster1));
            Monsters.Add(new SeaMonster(monster2));
            Monsters.Add(new SeaMonster(monster3));
            Monsters.Add(new SeaMonster(monster4));
            Monsters.Add(new SeaMonster(monster5));
            Monsters.Add(new SeaMonster(monster6));
            Monsters.Add(new SeaMonster(monster7));

        }
        bool printMonster()
        {
            // print het monster
            return true;
        }

    }
    public class Puzzle
    {
        public char[,] AfbeeldingsArray { get; }
        public int OnesInArray { get; }
        public List<List<PlacedTile>> PuzzledTiles { get; }
        public List<int> Hoeken { get; }
        public int DimAfbeelding { get;}
        Dictionary<int, List<int>> GevondenWaardenZijkanten { get; }
        public List<Tile> ListOfTiles { get; }
        public Puzzle(List<Tile> listOfTiles)
        {
            ListOfTiles = new List<Tile>(listOfTiles);
            DimAfbeelding = (int)Math.Sqrt(ListOfTiles.Count);
            GevondenWaardenZijkanten = new Dictionary<int, List<int>>(GetSideValues());
            Hoeken = new List<int> (zoekHoeken());
            PuzzledTiles = MaakAfbeelding();
            AfbeeldingsArray = ConvertAfbeelding();
            OnesInArray = countOnes();
        }
        int countOnes()
        {
            int ones = 0;
            for (int y = 0; y < (DimAfbeelding*8); y++)
            {
                for (int x = 0; x < (DimAfbeelding * 8); x++)
                {
                    if (AfbeeldingsArray[x,y] == '1') ones++;
                }
            }
            return ones;
        }
        List<int> zoekHoeken()
        {
            // Uitwerking deel 1
            //var gevondenWaardenZijkanten = new Dictionary<int, List<int>>();
            //GevondenWaardenZijkanten = GetSideValues();
            Console.WriteLine("Dictionary is gevuld");
            var hoeken = GevondenWaardenZijkanten
                .Where(x => x.Value.Count() <= 1)
                .Select(b => b.Value[0])
                .ToList()
                .GroupBy(i => i)
                .Where(x => x.Count() == 4)
                .Select(x => x.Key)
                .ToList();
            return hoeken;
        }
        Dictionary<int, List<int>> GetSideValues()
        {
            var gevondenWaarden = new Dictionary<int, List<int>>();
            foreach (Tile tile in ListOfTiles)
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
        List<List<PlacedTile>> MaakAfbeelding()
        {
            var afbeeldingOpdracht1 = new List<List<PlacedTile>>();
            var tilesToPlace = new Dictionary<int, Tile>();
            foreach (Tile tile in ListOfTiles) tilesToPlace.Add(tile.TileNumber, tile);
            var gekozenHoek = Hoeken[0];
            var zijdenGekozenHoek = GevondenWaardenZijkanten
                .Where(x => x.Value.Count() <= 1)
                .Select(x => new { x.Key, x.Value })
                .Where(x => x.Value[0] == gekozenHoek)
                .Select(x => x.Key)
                .ToList();
            Orientatie linksboven = tilesToPlace[gekozenHoek].DraaiHoek(zijdenGekozenHoek);
            var huidigeTile = tilesToPlace[gekozenHoek]; 
            var huidigeOrientatie = linksboven;
            int waardeNaarBeneden = 0;
            for (var j = 0; j < DimAfbeelding; j++)
            {
                var afbeeldingsRij = new List<PlacedTile>();
                for (var i = 0; i < DimAfbeelding; i++)
                {
                    var geplaatsteTile = new PlacedTile(huidigeTile, huidigeOrientatie);
                    afbeeldingsRij.Add(geplaatsteTile);
                    tilesToPlace.Remove(huidigeTile.TileNumber);
                    if (i == 0) waardeNaarBeneden = geplaatsteTile.SearchDown;
                    if (i < DimAfbeelding - 1)
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
                        if (j < (DimAfbeelding - 1))
                        {
                            huidigeTile = tilesToPlace[tilesToPlace.Where(x => x.Value.TileSides.Contains(waardeNaarBeneden)).Select(x => x.Value.TileNumber).ToList().First()];//tile die past
                            var toppositie = huidigeTile.TileSides.IndexOf(huidigeTile.TileSides.Where(p => p == waardeNaarBeneden).FirstOrDefault());
                            huidigeOrientatie = (Orientatie)toppositie;
                        }
                    }
                }
                afbeeldingOpdracht1.Add(afbeeldingsRij);
            }
            return afbeeldingOpdracht1;
        }
        char[,] ConvertAfbeelding()
        {
            char[,] convertedAfbeelding = new char[8 * DimAfbeelding, 8 * DimAfbeelding];
            for (var y = 0; y < DimAfbeelding; y++)
            {
                for (var x = 0; x < DimAfbeelding; x++)
                {
                    for (var y2 = 0; y2 < 8; y2++)
                    {
                        for (var x2 = 0; x2 < 8; x2++)
                        {
                            convertedAfbeelding[x2 + x * 8, y2 + y * 8] = PuzzledTiles[y][x].PlacedTileLayout[x2 + 1, y2 + 1];
                        }
                    }
                }
            }
            return convertedAfbeelding;
        }
        public bool PrintAfbeelding()
        {
            for (var y = 0; y < DimAfbeelding*8; y++)
            {
                for (var x = 0; x < DimAfbeelding*8; x++)
                {
                    Console.Write(AfbeeldingsArray[y, x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return true;
        }
        public List<int> TelMonsters(SeaMonsters seaMonsters)
        {
            var max = DimAfbeelding * 8;
            var oplossing2 = new List<int>();
            foreach (SeaMonster monster in seaMonsters.Monsters)
            {
                var aantalMonsters = 0;
                for (var y = 0; y < (max - monster.YMax); y++)
                {
                    for (var x = 0; x < (max - monster.XMax); x++)
                    {
                        var aantalPunten = 0;
                        foreach (var point in monster.Definition)
                        {
                            if (AfbeeldingsArray[x + point.XValue, y + point.YValue] == '1') aantalPunten++;
                        }
                        if (aantalPunten == 15) aantalMonsters++;
                    }
                }
                if (aantalMonsters > 0)
                {
                    oplossing2.Add(aantalMonsters);
                    oplossing2.Add(OnesInArray - aantalMonsters * 15);
                } 
            }
            return oplossing2;
        }
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
            char[,]  placedTileLayout = new char[10,10];
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
                    for (var y = 0; y<10; y++)
                    {
                        for (var x = 0; x<10; x++)
                        {
                            placedTileLayout[x,y] = tile.TileLayout[9-x][y];
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
                            placedTileLayout[x, y] = tile.TileLayout[y][9-x];
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
                for (var x = 0; x< 10; x++)
                {
                    Console.Write($"{PlacedTileLayout[x,y]}");
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
                    TopReverse = Convert.ToInt32(rowReverse,2);
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
        public Orientatie DraaiHoek(List <int> zijden)
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
