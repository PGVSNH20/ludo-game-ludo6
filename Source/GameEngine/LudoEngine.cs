using GameEngine.DataAccess;
using GameEngine.DbModels;
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
        public List<User> Players { get; set; }
        public User CurrentPlayer { get; set; }
        public User Winner { get; set; }
        public string GameName { get; set; }

        private Random random;

        public LudoEngine(LudoDbContext dbContext, string gameName)
        {
            context = dbContext;
            if (!GameExists(gameName, context))
                GameName = gameName;
            else
                throw new ArgumentException("Make sure to check the game name is available before instantiating.");
            random = new Random();
            Players = new List<User>();
        }

        public void AddPlayer(Type pieceType, string name)
        {
            User player;
            var user = GetUserByName(name);

            if (user == null)
            {
                player = new User() { Name = name };
                context.Users.Add(player);
                context.SaveChanges();
            }
            else
                player = user;

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

        public void SaveGame()
        {
            var game = new Game() { Name = GameName, Active = true, NextToRollDice = CurrentPlayer };
            context.Games.Add(game);
            int gameId = game.GameId;

            foreach(var player in Players)
            {

                var typeOfPiece = player.Pieces[0].Color;
                var pieceEntity = context.Pieces.Where(p => p.Color.ToLower() == typeOfPiece.ToLower()).Single();

                context.GameMembers.Add(
                    new GameMember() { Game = game, GameId = game.GameId, User = player, UserId = player.UserId, Piece = pieceEntity
                });

                foreach(var piece in player.Pieces)
                {
                    
                    context.GamePositions.Add(new GamePosition { Game = game, Position = 0, User = player  });
                }

            }

            context.SaveChanges();
        }


        private bool UserExistsInDatabase(string name)
        {
            try
            {
                var user = context.Users.Where(u => u.Name == name).Single();
                return true;
            } catch
            {
                return false;
            }
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

        public bool PieceIsInGoal(IPiece piece)
        {
            return piece.Position >= piece.EndPosition;
        }

        public IPiece FindCollidingPiece(int position)
        {
            IPiece collidingPiece = null;

            foreach(var player in Players)
            {
                collidingPiece = player.Pieces.Find(p => p.Position == position && p.EndPosition == p.Position); 
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
                    if (PieceIsInPlay(piece) || PieceIsInNest(piece))
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

            if (PieceIsInGoal(piece))
            {
                piece.Position = piece.EndPosition;
            }

            UpdateGame();

            return true;
            
        }

        private void UpdateGame()
        {

        }

        public bool PieceIsInPlay(IPiece piece)
        {
            return piece.Position > 0 && piece.Position < piece.EndPosition;
        }

        public bool PieceIsInNest(IPiece piece)
        {
            return piece.Position == 0;
        }

        public List<IPiece> GetPiecesInNest(Player player)
        {
            return player.Pieces.Where(p => p.Position == 0).ToList();
        }

        public List<IPiece> GetPiecesInPlay(Player player)
        {
            return player.Pieces.Where(p => p.Position != 0).ToList();
        }

        public void Load(string name)
        {
            // Find game using name

            // Get players in game

            // For every player, add pieces at correct positions
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

        public void SwitchPlayer()
        {
            int currentPlayerIndex = Players.FindIndex(pl => pl.Name == CurrentPlayer.Name);
            CurrentPlayer = Players[(currentPlayerIndex + 1) >= Players.Count ? 0 : currentPlayerIndex + 1];
        }

        public bool HasWinner()
        {
            var winner = Players.Find(pl => pl.Pieces.TrueForAll(p => p.Position >= p.EndPosition));
            Winner = winner;
            return winner == null ? false : true;
        }

        public static bool GameExists(string name, LudoDbContext context)
        {
            if (name == "") return true;
            try
            {
                var game = context.Games.Where(g => g.Name == name).Single();
                return game == null ? true : false;
            }
            catch { return false; }
        }
    }
}
