using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_22
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);
            var regel = reader.ReadLine();
            var set1 = new List<string>();
            while (regel != "")
            {
                set1.Add(regel);
                regel = reader.ReadLine();
            }
            regel = reader.ReadLine();
            var set2 = new List<string>();
            while (regel != null)
            {
                set2.Add(regel);
                regel = reader.ReadLine();
            }
            Console.WriteLine("De sets zijn ingelezen");
            var newGame = new Game(new Player(set1), new Player(set2));
            var score = newGame.PlayGame();
            Console.WriteLine($"De score van opdracht 1 = {score}");

            var newRecursiveGame = new Game(new Player(set1), new Player(set2));
            var winnaar = newRecursiveGame.PlayRecursiveGame();
            Console.WriteLine($"winnaar is bekend: {winnaar}");
            
            var score2 = newRecursiveGame.CountScore(winnaar);
            Console.WriteLine($"De score van opdracht 2 = {score2}");
            Console.WriteLine("De opdracht is klaar");
        }
    }

   
    public class Game
    {
        public Player Player1 { get; }
        public Player Player2 { get; }
        public int Score { get; }
        public Game(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
        public static string DeckAsString(List<int> deck)
        {
            return deck.Aggregate("", (a, x) => a + x);
        }
        public string PlayRecursiveGame()
        {
            string gameWinner;
            var seen = new HashSet<(string, string)>();
            var deckPlayer1 = DeckAsString(Player1.Deck.ToList());
            var deckPlayer2 = DeckAsString(Player2.Deck.ToList());
            (string, string) DecksAsTuple() => (deckPlayer1, deckPlayer2);
            while (true)
            {
                var deckTuple = DecksAsTuple();
                if (seen.Contains(deckTuple))
                {
                    gameWinner = Player1.Name;
                    return gameWinner;
                }
                seen.Add(deckTuple);
                string rondeWinner;
                if ((Player1.Deck.Count > Player1.Deck.First()) && (Player2.Deck.Count > Player2.Deck.First()))
                {
                    var newPlayer1Deck = Player1.Deck.Skip(1).Take(Player1.Deck.First()).ToList();
                    var newPlayer2Deck = Player2.Deck.Skip(1).Take(Player2.Deck.First()).ToList();
                    var game = new Game(new Player(Player1.Name, newPlayer1Deck), new Player(Player2.Name, newPlayer2Deck));
                    rondeWinner = game.PlayRecursiveGame();
                }
                else
                {
                    if (Player1.Deck.First() > Player2.Deck.First())
                    {
                        rondeWinner = Player1.Name;
                    }
                    else
                    {
                        rondeWinner = Player2.Name;
                    }
                }
                SwapCards(rondeWinner);
                deckPlayer1 = DeckAsString(Player1.Deck.ToList());
                deckPlayer2 = DeckAsString(Player2.Deck.ToList());
                
                if (!Player2.Deck.Any())
                {
                    gameWinner = Player1.Name;
                    return gameWinner;
                }
                if (!Player1.Deck.Any())
                {
                    gameWinner = Player2.Name;
                    return gameWinner;
                }
            }
        }
        
        public bool SwapCards(string winner)
        {
            if (winner == Player1.Name)
            {
                // Player one gets the cards
                Player1.Deck.Add(Player1.Deck.First());
                Player1.Deck.Add(Player2.Deck.First());
                Player1.Deck.Remove(Player1.Deck.First());
                Player2.Deck.Remove(Player2.Deck.First());
            }
            else
            {
                // Player two gets the cards
                Player2.Deck.Add(Player2.Deck.First());
                Player2.Deck.Add(Player1.Deck.First());
                Player1.Deck.Remove(Player1.Deck.First());
                Player2.Deck.Remove(Player2.Deck.First());
            }
            return true;
        }
        public int CountScore(string player)
        {
            var score = 0;
            Player winner;    
            if (player == Player1.Name)
            {
                winner = Player1;
            }
            else
            {
                winner = Player2;
            }
            for (var c = 1; c <= winner.Deck.Count; c++)
            {
                score += (c) * winner.Deck[winner.Deck.Count - c];
                //score += (c) * winner.Deck[^c];
            }
            return score;
        }
        public int PlayGame()
        {
            string winner;
            while (Player1.Deck.Count > 0 && Player2.Deck.Count > 0)
            {
                if (Player1.Deck.First() > Player2.Deck.First())
                {
                    winner = Player1.Name;
                }
                else
                {
                    winner = Player2.Name;
                }
                SwapCards(winner);
            }
            if (Player1.Deck.Count() == 0)
            {
                winner = Player2.Name;
            }
            else
            {
                winner = Player1.Name;
            }
            var score = CountScore(winner);
            return score;
        }
    }
    public class Player
    {
        public List<int> Deck { get; set; }
        public string Name { get; }
        public Player(List<string> input)
        {
            var name = input[0].Substring(0, input[0].Count() - 1);
            Name = name;
            Deck = new List<int>();
            for (var i = 1; i < input.Count; i++)
            {
                Deck.Add(int.Parse(input[i]));
            }
        }
        public Player(string name, List<int> deck)
        {  
            Name = name;
            Deck = new List<int>(deck);
        }
    }
}
