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
            var redStart = 1;
            var greenStart = 11;
            var yellowStart = 21;
            var blueStart = 31;

            if (piece.Position == 0)
            {
                switch (piece.Color)
                {
                    case "Red": piece.Position = redStart;
                        break;
                    case "Green": piece.Position = greenStart;
                        break;
                    case "Yellow": piece.Position = yellowStart;
                        break;
                    case "Blue": piece.Position = blueStart;
                        break;
                    default:
                        break;
                }
                piece.MovesCount = 1;
            }
            else
            {
                if (piece.Position + moves <= 40)
                    piece.Position += moves;
                else
                {
                    piece.Position = (piece.Position + moves) - 40;
                }
                piece.MovesCount += moves;
            }

            Console.WriteLine($"Movescount: {piece.MovesCount}");
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

        public User GetUserByName(string name)
        {
            try {
                return context.Users.Where(u => u.Name == name).Single();
            } catch { return null; }
        }
    }
}
