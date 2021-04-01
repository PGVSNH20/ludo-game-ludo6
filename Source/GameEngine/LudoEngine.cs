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
        private LudoDbContext context;
        public List<Player> Players { get; set; }
        private Random random;
        private Player winner = null;

        public LudoEngine()
        {
            context = new LudoDbContext();
            random = new Random();
            Players = new List<Player>();
        }

        public void AddPlayer(string color, string name)
        {
            Player player = new Player() { Name = name };
            player.Pieces = new List<Piece>() {
                new Piece() { Color = color },
                new Piece() { Color = color },
                new Piece() { Color = color },
                new Piece() { Color = color }
            };
            Players.Add(player);
        }

        // Uses the parameters to move a piece a certain number of steps
        public void MovePiece(Piece piece, int moves)
        {
            piece.Position = piece.Position == 0 ? 1 : piece.Position += moves;
            Console.WriteLine($"Moved piece to position {piece.Position}!");
        }

        public List<Piece> GetPiecesInNest(Player player)
        {
            return player.Pieces.Where(p => p.Position == 0).ToList();
        }

        public List<Piece> GetPiecesInPlay(Player player)
        {
            return player.Pieces.Where(p => p.Position != 0).ToList();
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
                return context.Users.Where(u => u.Name == name).Single();
            } catch { return null; }
        }
    }
}
