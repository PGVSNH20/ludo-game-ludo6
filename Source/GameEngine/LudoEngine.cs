using GameEngine.DataAccess;
using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class LudoEngine
    {
        private LudoDbContext Context;
        public List<Player> Players { get; set; }
        private Random random;
        private Player winner = null;

        public LudoEngine()
        {
            Context = new LudoDbContext();
            random = new Random();
            Players = new List<Player>();
        }

        public void Create()
        {
            Console.Write("Enter number of players: ");
            int.TryParse(Console.ReadLine(), out int numOfPlayers);

            AddPlayers(numOfPlayers);
        }

        public void AddPlayers(int numOfPlayers)
        {
            if (numOfPlayers > 1 && numOfPlayers <= 4)
            {

                string[] colors = { "Red", "Blue", "Green", "Yellow" };
                for (int i = 0; i < numOfPlayers; i++)
                {
                    Console.Write($"Enter username for {colors[i]}: ");
                    var name = Console.ReadLine();

                    Player player = new Player() { Name = name };
                    player.Pieces = new List<Piece>() {
                        new Piece() { Color = colors[i] },
                        new Piece() { Color = colors[i] },
                        new Piece() { Color = colors[i] },
                        new Piece() { Color = colors[i] }
                    };
                    Players.Add(player);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Choose between 2 and 4.");
            }
        }

        public void Run()
        {
            while(winner == null)
            {
                foreach (var player in Players)
                {
                    Console.WriteLine($"{player.Name} it's your turn. Enter 'r' to roll the dice.");
                    var input = Console.ReadLine();
                    if (input == "r")
                    {
                        List<Piece> PiecesInNest = player.Pieces.Where(p => p.Position == 0).ToList();
                        List<Piece> PiecesInPlay = player.Pieces.Where(p => p.Position != 0).ToList();
                        int moves = ThrowDice();
                        Console.WriteLine($"You got a {moves}!");

                        if(moves == 6 && PiecesInNest.Count == 4)
                        {
                            player.Pieces[0].Position = 1;
                            Console.WriteLine("Moved your first piece out of your nest!");
                            continue;
                        }
                        if(moves == 6 && PiecesInNest.Count > 0)
                        {
                            Console.WriteLine("Which piece do you want to move?");
                            for(int i = 0; i < player.Pieces.Count; i++)
                            {
                                Console.WriteLine($"{i}: Piece at position {player.Pieces[i].Position}");
                            }
                            int choice = int.Parse(Console.ReadLine());
                            if (player.Pieces[choice].Position == 0)
                                player.Pieces[choice].Position++;
                            else
                                player.Pieces[choice].Position += moves;
                            Console.WriteLine($"Moved piece to position {player.Pieces[choice].Position}.");

                        }
                        else if (PiecesInPlay.Count > 0 && PiecesInPlay.Count < 2)
                        {
                            PiecesInPlay[0].Position += moves;
                            Console.WriteLine($"Moved piece to position { PiecesInPlay[0].Position }");
                        }
                        else if (PiecesInPlay.Count > 0)
                        {
                            Console.WriteLine("Which piece do you want to move?");
                            for (int i = 0; i < PiecesInPlay.Count; i++)
                            {
                                Console.WriteLine($"{i}: Piece at position {PiecesInPlay[i].Position}");
                            }
                            int choice = int.Parse(Console.ReadLine());
                            PiecesInPlay[choice].Position += moves;
                            Console.WriteLine($"Moved piece to position {PiecesInPlay[choice].Position}.");
                        }
                        if(PiecesInPlay.Count > 0)
                        {

                        }
                    }
                }
            }
        }

        public void Load()
        {
            
        }

        private int ThrowDice()
        {
            return random.Next(1, 7);
        }

        public void GetStatistics()
        {
            Console.Write("Username? ");
            var name = Console.ReadLine();

            var user = GetUserByName(name);
            if (user != null)
                Console.WriteLine($"You've won {user.GamesWon} games and lost {user.GamesLost}.");
            else
                Console.WriteLine("Couldn't find user");
        }

        public User GetUserByName(string name)
        {
            try {
                return Context.Users.Where(u => u.Name == name).Single();
            } catch { return null; }
        }
    }
}
