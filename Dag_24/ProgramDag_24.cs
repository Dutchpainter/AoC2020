using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_24
{
    class Program
    {
        static void Main(string[] args)
        {
            var allTiles = File
                  .ReadAllLines("Input.txt")
                  .Where(s => !string.IsNullOrWhiteSpace(s))
                  .ToList();
            Console.WriteLine("De tegels zijn ingelezen");
            var alleTegels = new Tiles(allTiles);
            Console.WriteLine($"Het aantal zwarte tegels is: {alleTegels.BlackTiles}");
            var rondes = 100;
            for (var i = 0; i < rondes; i++)
            {
                alleTegels = alleTegels.NewSituation();
            }
            Console.WriteLine($"Het aantal zwarte tegels in opdracht 2 is: {alleTegels.BlackTiles}");
            Console.WriteLine("De opdracht is klaar");
        }
    }
    public class Tiles
    {
        public Dictionary<(int East, int North), TileColor> ListOfTiles { get; }
        public int BlackTiles { get;}
        public int WhiteTiles { get;}
        public Tiles(List<string> tiles)
        {
            BlackTiles = 0;
            WhiteTiles = 0;
            ListOfTiles = new Dictionary<(int, int), TileColor>();
            foreach (var tile in tiles)
            {
                var newCoordinate = MakeCoordinate(tile);
                if (ListOfTiles.ContainsKey(newCoordinate) )
                {
                    if (ListOfTiles[newCoordinate] == TileColor.Black)
                    {
                        ListOfTiles[newCoordinate] = TileColor.White;
                        BlackTiles--;
                        WhiteTiles++;
                    }
                    else
                    {
                        ListOfTiles[newCoordinate] = TileColor.Black;
                        WhiteTiles--;
                        BlackTiles++;
                    }
                }
                else
                {
                    ListOfTiles.Add(newCoordinate, TileColor.Black);
                    BlackTiles++;
                }
            }
        }

        public Tiles(Dictionary<(int East, int North), TileColor> tiles, int blackTiles, int whiteTiles)
        {
            BlackTiles = blackTiles;
            WhiteTiles = whiteTiles;
            ListOfTiles = new Dictionary<(int, int), TileColor>(tiles);
        }

        public bool PrintListOfTiles()
        {
            foreach (var tile in ListOfTiles)
            {
                Console.WriteLine($"Coordinate:{tile.Key}, TileColor: {tile.Value}");
            }
            return true;
        }

        public (int, int) MakeCoordinate(string tile)
        {
            var listOfDirections = new List<Directions>(MakeListOfDirections(tile));
            var east = 0;
            var north = 0;
            foreach (var direction in listOfDirections)
            {
                switch (direction)
                {
                    case Directions.East:
                        east += 2;
                        break;
                    case Directions.SouthEast:
                        east++;
                        north--;
                        break;
                    case Directions.SouthWest:
                        east--;
                        north--;
                        break;
                    case Directions.West:
                        east -= 2;
                        break;
                    case Directions.NorthWest:
                        east--;
                        north++;
                        break;
                    case Directions.NorthEast:
                        east++;
                        north++;
                        break;
                    default:
                        Console.WriteLine("Hier gaat iets niet goed....");
                        break;
                }
            }
            return (east, north);
        }

        public List<Directions> MakeListOfDirections(string directions)
        {
            var listOfDirections = new List<Directions>();
            while (directions.Length > 1)
            {
                var directionsFirst = directions[0];
                var directionsSecond = directions[1];
                switch (directionsFirst)
                {
                    case 's':
                        if (directionsSecond == 'e')
                        {
                            listOfDirections.Add(Directions.SouthEast);
                        }
                        else
                        {
                            listOfDirections.Add(Directions.SouthWest);
                        }
                        directions = directions.Substring(2);
                        break;
                    case 'n':
                        if (directionsSecond == 'e')
                        {
                            listOfDirections.Add(Directions.NorthEast);
                        }
                        else
                        {
                            listOfDirections.Add(Directions.NorthWest);
                        }
                        directions = directions.Substring(2);
                        break;
                    case 'e':
                        listOfDirections.Add(Directions.East);
                        directions = directions.Substring(1);
                        break;
                    case 'w':
                        listOfDirections.Add(Directions.West);
                        directions = directions.Substring(1);
                        break;
                    default:
                        Console.WriteLine("Er is iets niet goed gegaan");
                        break;
                }
                if (directions.Length == 1)
                {
                    if (directions[0] == 'e')
                    {
                        listOfDirections.Add(Directions.East);
                    }
                    else
                    {
                        listOfDirections.Add(Directions.West);
                    }
                }
            }
            return listOfDirections;
        }

        public Tiles NewSituation()
        {
            var blackTiles = 0;
            var whiteTiles = 0;
            var biggerListOfTiles = new Dictionary<(int,int), TileColor>(EnlargeRoom());
            var listOfTiles = new Dictionary<(int,int), TileColor>();
            foreach (var tile in biggerListOfTiles)
            {
                var blackTilesAdjecent = 0;
                var whiteTilesAdjacent = 0;
                var tilesAdjecent = TilesAdjacent(tile.Key);
                foreach(var coordinate in tilesAdjecent)
                {
                    if (biggerListOfTiles.ContainsKey(coordinate))
                    {
                        if (biggerListOfTiles[coordinate] == TileColor.Black)
                        {
                            blackTilesAdjecent++;  
                        }
                        else
                        {
                            whiteTilesAdjacent++; 
                        }
                    }
                    else
                    {
                        whiteTilesAdjacent++;
                    }
                }
                if (tile.Value == TileColor.Black)
                {
                    if (blackTilesAdjecent == 0 || blackTilesAdjecent > 2)
                    {
                        listOfTiles.Add(tile.Key, TileColor.White);
                        whiteTiles++;
                        
                    }
                    else
                    {
                        listOfTiles.Add(tile.Key, tile.Value);
                        blackTiles++;
                    } 
                }
                else
                {
                    if (blackTilesAdjecent == 2)
                    {
                        listOfTiles.Add(tile.Key, TileColor.Black);
                        blackTiles++;
                       
                    }
                    else
                    {
                        listOfTiles.Add(tile.Key, TileColor.White);
                        whiteTiles++;
                    }
                }
            }
            return new Tiles(listOfTiles, blackTiles, whiteTiles);
        }

        Dictionary<(int East, int North), TileColor> EnlargeRoom()
        {
            var enlargedRoom = new Dictionary<(int, int), TileColor>(ListOfTiles);
            foreach (var tile in ListOfTiles)
            {
                var tilesAdjecent = TilesAdjacent(tile.Key);
                foreach(var coordinate in tilesAdjecent)
                if (!ListOfTiles.ContainsKey(coordinate) && !enlargedRoom.ContainsKey(coordinate))
                {
                        enlargedRoom.Add(coordinate, TileColor.White);
                }
            }
            return enlargedRoom;
        }

        List<(int, int)> TilesAdjacent((int east, int north )tile)
        {
            var tilesAdjacent = new List<(int, int)>();
            var tileCoordinateNorth = tile.north;
            var tileCoordinateEast = tile.east;
            tilesAdjacent.Add((tileCoordinateEast - 1, tileCoordinateNorth + 1));
            tilesAdjacent.Add((tileCoordinateEast + 1, tileCoordinateNorth + 1));
            tilesAdjacent.Add((tileCoordinateEast + 2, tileCoordinateNorth ));
            tilesAdjacent.Add((tileCoordinateEast + 1, tileCoordinateNorth - 1));
            tilesAdjacent.Add((tileCoordinateEast - 1, tileCoordinateNorth - 1));
            tilesAdjacent.Add((tileCoordinateEast - 2, tileCoordinateNorth ));
            return tilesAdjacent;
        }
    }

    
    public enum Directions
    {
        East,
        SouthEast,
        SouthWest,
        West,
        NorthWest,
        NorthEast,
    }
    public enum TileColor
    {
        Black,
        White
    }
}
