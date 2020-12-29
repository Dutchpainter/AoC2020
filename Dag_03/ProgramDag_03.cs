using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dag_03
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var kaart = new List<string>();
           

            Console.WriteLine("Begin van opdracht 3");
            // Kaart inlezen
            while (true)
            {
                var regel = reader.ReadLine();

                if (string.IsNullOrEmpty(regel))
                    break;
                kaart.Add(regel);

            }

            // controleren hoeveel bomen er geraakt worden als er naar rechts en naar beneden
            // gesleed wordt.

            //var naarRechts = 3;
            //var naarBeneden = 1;

            var hellingen = new (int naarRechts, int naarBeneden)[]
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            // bomen op een helling tellen.
            Func<int, int, int> telBomen = (naarRechts, naarBeneden) =>
              {
                  var aantalBomen = 0;
                  var posX = 0;
                  var posY = 0;
                  var lijn = kaart[0];
                  var modPosX = 0;

                  while (posY < kaart.Count())
                  {
                      lijn = kaart[posY];
                      modPosX = posX % lijn.Length;

                      if (lijn[modPosX] == '#')
                      {
                          aantalBomen++;
                      }
                      posY += naarBeneden;
                      posX += naarRechts;
                  }
                  return aantalBomen;
              };

            var Bomen = hellingen.Select(helling => (helling.naarRechts, helling.naarBeneden, bomen: telBomen(helling.naarRechts, helling.naarBeneden))).ToList();

            // bomen op elke helling tellen.
            foreach(var Boom in Bomen)
            {
                Console.WriteLine($"Right {Boom.naarRechts} Down {Boom.naarBeneden} Bomen: {Boom.bomen}");
            }
            var oplossingDeel2 = Bomen.Aggregate(1ul, (i, tuple) => i * (ulong)tuple.bomen);
            Console.WriteLine($"Oplossing deel 2: {oplossingDeel2}");

        }
    }
}
