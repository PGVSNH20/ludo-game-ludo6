using GameEngine.DataAccess;
using GameEngine.DbModels;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;
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
        public Game Game { get; set; }

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

        private LudoEngine(LudoDbContext dbContext)
        {
            context = dbContext;
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
            Game = game;

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

        public static List<Type> GetPieceTypes()
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
            GamePosition gamePosition = context.GamePositions
                .Where(gp => gp.Position == piece.Position && gp.User == CurrentPlayer && gp.Game == Game).First();

            var collidingPiece = FindCollidingPiece(piece.Position + moves);

            if (collidingPiece != null && !PieceIsEnemy(collidingPiece))
                return false;

            if (collidingPiece != null && PieceIsEnemy(collidingPiece))
            {
                GamePosition enemyPosition = context.GamePositions
                    .Where(gp => gp.Position == collidingPiece.Position && gp.Game == Game).Single();

                gamePosition.Position += moves;
                enemyPosition.Position = 0;
                context.GamePositions.Update(gamePosition);
                context.GamePositions.Update(enemyPosition);

                collidingPiece.Position = 0;
                piece.Position += moves;
            }

            if (collidingPiece == null)
            {
                if (piece.Position == 0 && moves == 6)
                {
                    gamePosition.Position = piece.StartPosition;
                    context.GamePositions.Update(gamePosition);

                    piece.Position = piece.StartPosition;
                }
                else
                {
                    gamePosition.Position += moves;
                    context.GamePositions.Update(gamePosition);

                    piece.Position += moves;
                }
            }

            if (PieceIsInGoal(piece))
            {
                gamePosition.Position = 100;
                context.GamePositions.Update(gamePosition);

                piece.Position = 100;
            }

            return true;
            
        }

        public bool PieceIsInPlay(IPiece piece)
        {
            return piece.Position > 0 && piece.Position < piece.EndPosition;
        }

        public bool PieceIsInNest(IPiece piece)
        {
            return piece.Position == 0;
        }

        public static LudoEngine Load(string name, LudoDbContext context)
        {
            try
            {
                // Find the game in the database using name
                var gameEntity = context.Games.Include(g => g.NextToRollDice).Include(g => g.Winner).Where(g => g.Name == name).Single();

                // Instantiate a new game and set the needed properties to values from the gameEntity
                LudoEngine game = new LudoEngine(context);
                game.GameName = gameEntity.Name;
                game.CurrentPlayer = gameEntity.NextToRollDice;
                game.Game = gameEntity;
                game.Winner = gameEntity.Winner;

                // Get all the players in the game
                var gameMemberEntities = context.GameMembers.Include(gm => gm.Piece).Where(gm => gm.GameId == gameEntity.GameId).ToList();

                // Setup a player object for each member in the game
                foreach(var gameMemberEntity in gameMemberEntities)
                {
                    var pieceEntity = context.Pieces.Where(p => p.PieceId == gameMemberEntity.Piece.PieceId).Single();
                    var pieceType = GetPieceTypeFromColor(pieceEntity.Color);
                    var user = context.Users.Where(u => u.UserId == gameMemberEntity.UserId).Single();
                    user.Pieces = new List<IPiece>();

                    // Get all the player piece positions
                    var gamePositions = context.GamePositions.Where(gp => gp.Game == gameEntity && gp.User == user).ToList();

                    // For each piece that the player has in the game, create a piece object and add it to the player Pieces-list
                    foreach(var gamePosition in gamePositions)
                    {
                        IPiece piece = (IPiece)Activator.CreateInstance(pieceType);
                        piece.Position = gamePosition.Position;
                        user.Pieces.Add(piece);
                    }

                    // Player setup is done, therefore add the player to the Players-list.
                    game.Players.Add(user);
                    
                }

                return game;


            }catch
            {
                return null;

            }


        }

        private static Type GetPieceTypeFromColor(string color)
        {
            var types = GetPieceTypes();
            var type = types.Where(t => t.Name.ToLower().Contains(color.ToLower())).Single();
            return type;
        }

        public int ThrowDice()
        {
            return random.Next(1, 7);
        }

        public User GetUserByName(string name)
        {
            try {
                return context.Users.Where(u => u.Name.ToLower() == name.ToLower()).Single();
            } catch { return null; }
        }

        public void SwitchPlayer()
        {
            int currentPlayerIndex = Players.FindIndex(pl => pl.Name == CurrentPlayer.Name);
            CurrentPlayer = Players[(currentPlayerIndex + 1) >= Players.Count ? 0 : currentPlayerIndex + 1];
            Game.NextToRollDice = CurrentPlayer;
            context.Games.Update(Game);
            // SaveChanges needed?
            context.SaveChanges();
        }

        public bool HasWinner()
        {
            var winner = Players.Find(pl => pl.Pieces.TrueForAll(p => p.Position >= p.EndPosition));
            Winner = winner;
            if (Winner != null)
            {
                Game.Winner = Winner;
                Game.Active = false;
                context.Games.Update(Game);
                // SaveChanges needed?
                context.SaveChanges();
            }
            return winner != null;
        }

        public static bool GameExists(string name, LudoDbContext context)
        {
            if (name == "") return true;
            try
            {
                Game game = context.Games.Where(g => g.Name.ToLower() == name.ToLower()).Single();
                return game != null;
            }
            catch { return false; }
        }
    }
}
