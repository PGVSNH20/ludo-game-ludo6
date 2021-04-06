using GameEngine.DataAccess;
using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameEngine
{
    public class LudoEngine
    {
        private LudoDbContext context;
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        private Random random;

        public LudoEngine()
        {
            context = new LudoDbContext();
            random = new Random();
            Players = new List<Player>();
        }

        public void AddPlayer(Type pieceType, string name)
        {
            Player player = new Player() { Name = name };
            player.Pieces = new List<IPiece>() {
                (IPiece)Activator.CreateInstance(pieceType),
                (IPiece)Activator.CreateInstance(pieceType),
                (IPiece)Activator.CreateInstance(pieceType),
                (IPiece)Activator.CreateInstance(pieceType)
            };
            Players.Add(player);

            if (Players.Count == 1)
                CurrentPlayer = Players[0];
        }

        public List<Type> GetPieceTypes()
        {
            var type = typeof(IPiece);
            Assembly IPieceAssembly = type.Assembly;
            return IPieceAssembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToList();

        }

        public bool CheckIfEnteringGoal(IPiece piece, int moves)
        {
            int nextPosition = piece.Position + moves;

            if (nextPosition >= piece.EndPosition) return true;
            return false;
        }

        public IPiece FindCollidingPiece(int position)
        {
            IPiece collidingPiece = null;

            foreach(var player in Players)
            {
                collidingPiece = player.Pieces.Find(p => p.Position == position); 
            }
            return collidingPiece;
        }

        public bool PieceIsEnemy(IPiece piece)
        {
            return piece.GetType() != CurrentPlayer.Pieces[0].GetType();
        }

        public List<IPiece> GetMoveablePieces(int moves)
        {
            List<IPiece> moveablePieces = new List<IPiece>();

            foreach(var piece in CurrentPlayer.Pieces)
            {
                if (moves == 6)
                {
                    moveablePieces.Add(piece);
                }
                else
                {
                    if (PieceIsInPlay(piece))
                        moveablePieces.Add(piece);
                }
            }

            return moveablePieces;
        }

        // Uses the parameters to move a piece a certain number of steps
        public bool MovePiece(IPiece piece, int moves)
        {
            if (CheckIfEnteringGoal(piece, moves))
            {
                piece.Position = piece.EndPosition;
            }

            var collidingPiece = FindCollidingPiece(piece.Position + moves);

            if (collidingPiece != null && !PieceIsEnemy(collidingPiece))
                return false;

            if (collidingPiece != null && PieceIsEnemy(collidingPiece))
            {
                collidingPiece.Position = 0;
            }

            if (collidingPiece == null)
            {
                if (piece.Position == 0 && moves == 6)
                    piece.Position = piece.StartPosition;
                else
                    piece.Position += moves;
            }

            return true;
            
        }

        public bool PieceIsInPlay(IPiece piece)
        {
            return piece.Position > 0 && piece.Position < piece.EndPosition;
        }

        public List<IPiece> GetPiecesInNest(Player player)
        {
            return player.Pieces.Where(p => p.Position == 0).ToList();
        }

        public List<IPiece> GetPiecesInPlay(Player player)
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
