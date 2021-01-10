using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_17
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inlezen bestand
            var FirstCube = File
                   .ReadAllLines("Input.txt")
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToList();
            // Deel 1
            Cube3D Positie3D = new Cube3D(FirstCube);
            //Positie3D.PrintCube();
            for (var i=1; i<=6; i++)
            {
                Positie3D = Positie3D.nextCube();
                Console.WriteLine($"Ronde: {i}");
            }
            var oplossing1 = Positie3D.CountCube();
            Console.WriteLine($"Er zijn {oplossing1} posities actief.");

            // Deel 2
            Cube4D Positie4D = new Cube4D(FirstCube);
            //Positie.PrintCube();
            for (var i = 1; i <= 6; i++)
            {
                Positie4D = Positie4D.nextCube();
                Console.WriteLine($"Ronde: {i}");
            }
            var oplossing2 = Positie4D.CountCube();
            Console.WriteLine($"Er zijn {oplossing2} posities actief.");
        }
    }



    public class Cube3D
    {
        public List<List<List<Point3D>>> cube { get; }
        public int DimX { get; set; }
        public int DimY { get; set; }
        public int DimZ { get; set; }
        public Cube3D(List<String> firstCube)
        {
            //var Cube = firstCube;
            char c;
            var first = new List<Point3D>();
            var second = new List<List<Point3D>>();
            cube = new List<List<List<Point3D>>>();
            DimX = firstCube.First().Length;
            DimY = firstCube.Count;
            DimZ = 1;
            for (var i = 0; i < DimY; i++)
            {
                for (var j = 0; j < DimX; j++)
                {
                    c = firstCube[i][j];
                    first.Add(new Point3D(0, i, j, c));
                }
                second.Add(first);
                first = new List<Point3D>();
            }
            cube.Add(second);    
        }

        public Cube3D(List<List<List<Point3D>>> allePunten)
        {
            cube = allePunten;
            
            DimZ = cube.Count();
            DimY = cube.Select(l => l.Count()).Max();
            DimX = cube.SelectMany(l => l.Select(m => m.Count)).Max();
        }
        public int CountCube()
        {
            var count = 0;
            for (var z = 0; z < DimZ; z++)
            {
                for (var y = 0; y < DimY; y++)
                {
                    for (var x = 0; x < DimX; x++)
                    {
                        if (cube[z][y][x].EnergyState == EnergyState.Active) count++;
                    }
                }
            }
            return count;
        }
        public void PrintCube()
        {
            Console.WriteLine();
            Console.WriteLine($"DimZ = {DimZ} DimY = {DimY} DimX = {DimX}");
            Console.WriteLine();
            for (var z = 0; z < DimZ; z++)
            {
                for (var y = 0; y < DimY; y++)
                {
                    for (var x = 0; x < DimX; x++)
                    {
                        Console.Write(HelpFuncties.ConvertToString(cube[z][y][x].EnergyState));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
           
        }
        public Cube3D nextCube()
        {
            var nextCube = new List<List<List<Point3D>>>();
            var first = new List<Point3D>();
            var second = new List<List<Point3D>>();
            EnergyState state;
            for (var z = -1; z <= DimZ; z++)
            {
                for (var y = -1; y <= DimY; y++)
                {
                    for (var x = -1; x <= DimX; x++)
                    {
                        if(z == -1 || y == -1 || x ==-1|| z == DimZ || y == DimY || x == DimX)
                        {
                            state = EnergyState.Inactive;
                        }
                        else
                        {
                            state = cube[z][y][x].EnergyState;
                        }
                        var check = actieveBuren(z,y,x);
                        if (state == EnergyState.Active )
                        {
                            //active
                            if (check == 3 || check == 2)
                            {
                                first.Add(new Point3D(z + 1, y + 1, x + 1, EnergyState.Active));
                            } else
                            {
                                first.Add(new Point3D(z + 1, y + 1, x + 1, EnergyState.Inactive));
                            }        
                        }
                        else
                        {
                            //inactive
                            if (check == 3)
                            {
                                first.Add(new Point3D(z + 1, y + 1, x + 1, EnergyState.Active));
                            }
                            else
                            {
                                first.Add(new Point3D(z + 1, y + 1, x + 1, EnergyState.Inactive));
                            }
                        }
                    }
                    second.Add(first);
                    first= new List<Point3D>();
                }
                nextCube.Add(second);
                second = new List<List<Point3D>>();
            }
            return new Cube3D(nextCube);
        }
        public int actieveBuren(int Z, int Y, int X)
        {
            var buren = 0;
            var XPos = X;
            var YPos = Y;
            var ZPos = Z;
            var Zmin = ZPos - 1 <= 0 ? 0 : ZPos - 1; 
            var Zmax = ZPos + 1 >= DimZ - 1 ? DimZ - 1: ZPos + 1; 
            var Ymin = YPos - 1 <= 0 ? 0 : YPos - 1;
            var Ymax = YPos + 1 >= DimY - 1? DimY - 1 : YPos + 1;
            var Xmin = XPos - 1 <= 0 ? 0 : XPos - 1;
            var Xmax = XPos + 1 >= DimX - 1? DimX - 1 : XPos + 1;
            for (var z = Zmin; z <= Zmax; z++)
            {
                for (var y = Ymin; y <= Ymax; y++)
                {
                    for (var x = Xmin; x <= Xmax; x++)
                    {
                        if (!(x == XPos && y == YPos && z == ZPos) && cube[z][y][x].EnergyState == EnergyState.Active)
                        { 
                                buren++;
                        }
                    }
                }
            }
            return buren;
        }
    }
    public class Cube4D
    {
        public List<List<List<List<Point4D>>>> cube { get; }
        public int DimX { get; set; }
        public int DimY { get; set; }
        public int DimZ { get; set; }
        public int DimW { get; set; }
        public Cube4D(List<String> firstCube)
        {
            //var Cube = firstCube;
            char c;
            var first = new List<Point4D>();
            var second = new List<List<Point4D>>();
            var third = new List<List<List<Point4D>>>();
            cube = new List<List<List<List<Point4D>>>>();
            
            DimX = firstCube.First().Length;
            DimY = firstCube.Count;
            DimZ = 1;
            DimW = 1;
            for (var h = 0; h < DimZ; h++)
            {
                for (var i = 0; i < DimY; i++)
                {
                    for (var j = 0; j < DimX; j++)
                    {
                        c = firstCube[i][j];
                        first.Add(new Point4D(0, h, i, j, c));
                    }
                    second.Add(first);
                    first = new List<Point4D>();
                }
                third.Add(second);
                second = new List<List<Point4D>>();
            }
            cube.Add(third);
        }

        public Cube4D(List<List<List<List<Point4D>>>> allePunten)
        {
            cube = allePunten;
            DimW = cube.Count();
            DimZ = cube.Select(l => l.Count()).Max();
            DimY = cube.SelectMany(l => l.Select(m => m.Count)).Max();
            // TODO DimX klopt niet
            DimX = cube.SelectMany(l => l.SelectMany(l => l.Select(m => m.Count))).Max();
        }
        public int CountCube()
        {
            var count = 0;
            for (var w = 0; w<DimW; w++)
            {
                for (var z = 0; z < DimZ; z++)
                {
                    for (var y = 0; y < DimY; y++)
                    {
                        for (var x = 0; x < DimX; x++)
                        {
                            if (cube[w][z][y][x].EnergyState == EnergyState.Active) count++;
                        }
                    }
                }
            }
            return count;
        }
        public void PrintCube()
        {
            Console.WriteLine();
            Console.WriteLine($"Dim W = {DimW} DimZ = {DimZ} DimY = {DimY} DimX = {DimX}");
            Console.WriteLine();
            for (var w = 0; w<DimW; w++)
            {
                for (var z = 0; z < DimZ; z++)
                {
                    for (var y = 0; y < DimY; y++)
                    {
                        for (var x = 0; x < DimX; x++)
                        {
                            Console.Write(HelpFuncties.ConvertToString(cube[w][z][y][x].EnergyState));
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public Cube4D nextCube()
        {
            var nextCube = new List<List<List<List<Point4D>>>>();
            var first = new List<Point4D>();
            var second = new List<List<Point4D>>();
            var third = new List<List<List<Point4D>>>();
            EnergyState state;
            for (var w = -1; w <= DimW; w++)
            {
                for (var z = -1; z <= DimZ; z++)
                {
                    for (var y = -1; y <= DimY; y++)
                    {
                        for (var x = -1; x <= DimX; x++)
                        {
                            if (w == -1 || z == -1 || y == -1 || x == -1 || w == DimW || z == DimZ || y == DimY || x == DimX)
                            {
                                state = EnergyState.Inactive;
                            }
                            else
                            {
                                state = cube[w][z][y][x].EnergyState;
                            }
                            var check = actieveBuren(w, z, y, x);
                            if (state == EnergyState.Active)
                            {
                                //active
                                if (check == 3 || check == 2)
                                {
                                    first.Add(new Point4D(w + 1, z + 1, y + 1, x + 1, EnergyState.Active));
                                }
                                else
                                {
                                    first.Add(new Point4D(w +1 , z + 1, y + 1, x + 1, EnergyState.Inactive));
                                }
                            }
                            else
                            {
                                //inactive
                                if (check == 3)
                                {
                                    first.Add(new Point4D(w + 1, z + 1, y + 1, x + 1, EnergyState.Active));
                                }
                                else
                                {
                                    first.Add(new Point4D(w +1 , z + 1, y + 1, x + 1, EnergyState.Inactive));
                                }
                            }
                        }
                        second.Add(first);
                        first = new List<Point4D>();
                    }
                    third.Add(second);
                    second = new List<List<Point4D>>();
                }
                nextCube.Add(third);
                third = new List<List<List<Point4D>>>();
            }
            return new Cube4D(nextCube);
        }

        //public int actieveBuren(Point ditPunt)
        public int actieveBuren(int W, int Z, int Y, int X)
        {
            var buren = 0;
            var XPos = X;
            var YPos = Y;
            var ZPos = Z;
            var WPos = W;
            var Wmin = WPos - 1 <= 0 ? 0 : WPos - 1;
            var Wmax = WPos + 1 >= DimW - 1 ? DimW - 1 : WPos + 1;
            var Zmin = ZPos - 1 <= 0 ? 0 : ZPos - 1;
            var Zmax = ZPos + 1 >= DimZ - 1 ? DimZ - 1 : ZPos + 1;
            var Ymin = YPos - 1 <= 0 ? 0 : YPos - 1;
            var Ymax = YPos + 1 >= DimY - 1 ? DimY - 1 : YPos + 1;
            var Xmin = XPos - 1 <= 0 ? 0 : XPos - 1;
            var Xmax = XPos + 1 >= DimX - 1 ? DimX - 1 : XPos + 1;

            for (var w = Wmin; w <= Wmax; w++)
            {
                for (var z = Zmin; z <= Zmax; z++)
                {
                    for (var y = Ymin; y <= Ymax; y++)
                    {
                        for (var x = Xmin; x <= Xmax; x++)
                        {
                            if (!(x == XPos && y == YPos && z == ZPos && w == WPos) && cube[w][z][y][x].EnergyState == EnergyState.Active)
                            {
                                buren++;
                            }
                        }
                    }
                }
            }
            return buren;
        }
    }
    public class Point3D
    {
        public int XPos { get; }
        public int YPos { get; }
        public int ZPos { get; }
        public EnergyState EnergyState { get; }
        public Point3D(int zpos, int ypos, int xpos, char c)
        {
            XPos = xpos;
            YPos = ypos;
            ZPos = zpos;
            EnergyState = HelpFuncties.Parse(c);
        }
        public Point3D(int Z, int Y, int X, EnergyState newState)
        {
            XPos = X;
            YPos = Y;
            ZPos = Z;
            EnergyState = newState;
        }
    }
    public class Point4D
    {
        public int XPos { get; }
        public int YPos { get; }
        public int ZPos { get; }
        public int WPos { get; }
        public EnergyState EnergyState { get; }
        public Point4D(int wpos, int zpos, int ypos, int xpos, char c)
        {
            XPos = xpos;
            YPos = ypos;
            ZPos = zpos;
            WPos = wpos;
            EnergyState = HelpFuncties.Parse(c);
        }
        public Point4D(int W, int Z, int Y, int X, EnergyState newState)
        {
            XPos = X;
            YPos = Y;
            ZPos = Z;
            WPos = W;
            EnergyState = newState;
        }
    }
    public static class HelpFuncties
    {
        public static EnergyState Parse(char c)
        {
            return c switch
            {
                '.' => EnergyState.Inactive,
                '#' => EnergyState.Active,
                _ => throw new ArgumentOutOfRangeException("c", c, null),
            };
        }

        public static string ConvertToString(EnergyState ch)
        {
            return ch switch
            {
                EnergyState.Inactive => ".",
                EnergyState.Active => "#",
                _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
            };
        }
    }
    public enum EnergyState
    {
        Active,
        Inactive,
    }
}
