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

        // Let's the user choose between the given list of pieces and returns that object
        private Piece ChoosePiece(List<Piece> pieces)
        {
            Console.WriteLine("Which piece do you want to move?");
            for (int i = 0; i < pieces.Count; i++)
            {
                Console.WriteLine($"{i}: Piece at position {pieces[i].Position}");
            }
            int choice = int.Parse(Console.ReadLine());
            return pieces[choice];
        }

        // Uses the parameters to move a piece a certain number of steps
        private void MovePiece(Piece piece, int moves)
        {
            piece.Position = piece.Position == 0 ? 1 : piece.Position += moves;
            Console.WriteLine($"Moved piece to position {piece.Position}!");
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

                        if (moves == 6)
                        {
                            // If all pieces are in the nest, automatically move one out
                            if (PiecesInNest.Count == 4)
                                MovePiece(player.Pieces[0], 1);
                            // If there are pieces both in the nest and in play, let the user choose one of all pieces and move it.
                            else
                                MovePiece(ChoosePiece(player.Pieces), moves);

                            continue;
                        }

                        // The code below only executes if 'moves' is not equal to 6

                        // If there's only one piece in play, automatically move the piece
                        if (PiecesInPlay.Count == 1)
                        {
                            MovePiece(PiecesInPlay[0], moves);
                            continue;
                        }
                        // If there are multiple pieces in play, let the user choose between those pieces and move it.
                        if (PiecesInPlay.Count > 1)
                            MovePiece(ChoosePiece(PiecesInPlay), moves);
                    }
                }
            }
        }

        public void Load()
        {
            
        }

        public int ThrowDice()
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
