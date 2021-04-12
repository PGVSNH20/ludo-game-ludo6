using GameEngine.DataAccess;
using GameEngine.DbModels;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameEngine
{
    public class LudoEngine
    {
        public List<User> Players { get; private set; }
        public User CurrentPlayer { get; private set; }
        public User Winner { get; private set; }
        public bool Collided { get; private set; } = false;
        public IPiece CollidingPiece { get; private set; } = null;

        private Game game;
        private string gameName;
        private LudoDbContext context;
        private Random random;

        public LudoEngine(LudoDbContext dbContext, string gameName)
        {
            context = dbContext;
            if (!GameExists(gameName, context))
                this.gameName = gameName;
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
            var user = GetUserByName(name, context);

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
            var game = new Game() { Name = gameName, Active = true, NextToRollDice = CurrentPlayer };
            context.Games.Add(game);
            this.game = game;

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

        public static List<Type> GetPieceTypes()
        {
            var type = typeof(IPiece);
            Assembly IPieceAssembly = type.Assembly;
            return IPieceAssembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToList();

        }

        public List<IPiece> GetAllPiecesOfType(Type pieceType)
        {
            List<IPiece> pieces = new List<IPiece>();
            foreach (var player in Players)
            {
                if (player.Pieces[0].GetType() == pieceType)
                    pieces = player.Pieces;
            }
            return pieces;
        }

        public bool PieceIsInGoal(IPiece piece)
        {
            return piece.Position >= piece.EndPosition;
        }

        public bool PieceIsInSafeZone(IPiece piece)
        {
            return (piece.Position < piece.EndPosition) && (piece.Position > (piece.EndPosition - 5));
        }

        public List<IPiece> PlayerPiecesInNest(User player)
        {
            return player.Pieces.Where(p => PieceIsInNest(p)).ToList();
        }

        public IPiece FindCollidingPiece(int position)
        {
            IPiece collidingPiece = null;
            int nextPosition = position;
            if (position > 40)
                nextPosition = position - 40;

            foreach(var player in Players)
            {
                collidingPiece = player.Pieces.Find(p => p.AbsoluteBoardPosition == nextPosition && !PieceIsInSafeZone(p));
                if (collidingPiece != null)
                    break;
            }
            if (collidingPiece != null)
            {
                Collided = true;
                CollidingPiece = collidingPiece;
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

        public bool MovePiece(IPiece piece, int moves)
        {
            Collided = false;
            CollidingPiece = null;

            GamePosition gamePosition = context.GamePositions
                .Where(gp => gp.Position == piece.Position && gp.User == CurrentPlayer && gp.Game == game).First();

            IPiece collidingPiece;
            if (piece.AbsoluteBoardPosition == 0)
                collidingPiece = FindCollidingPiece(piece.StartPosition);
            else
                collidingPiece = FindCollidingPiece(piece.AbsoluteBoardPosition + moves);

            if (collidingPiece != null && !PieceIsEnemy(collidingPiece))
                return false;

            // If the user is colliding with an enemy piece
            if (collidingPiece != null && PieceIsEnemy(collidingPiece))
            {
                Collide(gamePosition, piece, collidingPiece, moves);
            }
            // If the user isn't colliding
            if (collidingPiece == null)
            {
                if (piece.Position == 0 && moves == 6)
                    MoveOutOfNest(gamePosition, piece);
                else
                    JustMove(gamePosition, piece, moves);
            }

            if (PieceIsInGoal(piece))
                MoveToGoal(gamePosition, piece);

            context.SaveChanges();

            return true;
            
        }

        private void Collide(GamePosition gamePosition, IPiece piece, IPiece collidingPiece, int moves)
        {
            GamePosition enemyPosition = context.GamePositions
                    .Where(gp => gp.Position == collidingPiece.Position && gp.Game == game).Single();

            enemyPosition.Position = 0;
            collidingPiece.Position = 0;

            if (piece.Position == 0)
            {
                piece.Position = piece.StartPosition;
                gamePosition.Position = piece.StartPosition;
            }
            else
            {
                piece.Position += moves;
                gamePosition.Position += moves;
            }
            context.GamePositions.Update(gamePosition);
            context.GamePositions.Update(enemyPosition);

        }

        private void MoveToGoal(GamePosition gamePosition, IPiece piece)
        {
            gamePosition.Position = 100;
            context.GamePositions.Update(gamePosition);

            piece.Position = 100;
        }

        private void JustMove(GamePosition gamePosition, IPiece piece, int moves)
        {
            if (piece.Position == 0)
            {
                gamePosition.Position = piece.StartPosition;
                piece.Position = piece.StartPosition;
            }
            else
            {
                gamePosition.Position += moves;
                piece.Position += moves;

            }
            context.GamePositions.Update(gamePosition);

        }

        private void MoveOutOfNest(GamePosition gamePosition, IPiece piece)
        {
            gamePosition.Position = piece.StartPosition;
            context.GamePositions.Update(gamePosition);

            piece.Position = piece.StartPosition;
        }

        private bool PieceIsInPlay(IPiece piece)
        {
            return piece.Position > 0 && piece.Position < piece.EndPosition;
        }

        private bool PieceIsInNest(IPiece piece)
        {
            return piece.Position == 0;
        }

        private static LudoEngine CreateGameFromEntity(Game gameEntity, LudoDbContext context)
        {
            // Instantiate a new game and set the needed properties to values from the gameEntity
            LudoEngine game = new LudoEngine(context);
            game.gameName = gameEntity.Name;
            game.CurrentPlayer = gameEntity.NextToRollDice;
            game.game = gameEntity;
            game.Winner = gameEntity.Winner;
            return game;
        }

        private static User CreatePlayerFromGameMember(GameMember gameMemberEntity, Type pieceType, Game gameEntity, LudoDbContext context)
        {
            var user = context.Users.Where(u => u.UserId == gameMemberEntity.UserId).Single();
            user.Pieces = new List<IPiece>();

            // Get all the player piece positions
            var gamePositions = context.GamePositions.Where(gp => gp.Game == gameEntity && gp.User == user).ToList();

            // For each piece that the player has in the game, create a piece object and add it to the player Pieces-list
            foreach (var gamePosition in gamePositions)
            {
                IPiece piece = (IPiece)Activator.CreateInstance(pieceType);
                piece.Position = gamePosition.Position;
                user.Pieces.Add(piece);
            }

            return user;
        }

        public static LudoEngine Load(string name, LudoDbContext context)
        {
            try
            {
                // Find the game in the database using name
                var gameEntity = context.Games.Include(g => g.NextToRollDice).Include(g => g.Winner).Where(g => g.Name == name).Single();

                LudoEngine game = CreateGameFromEntity(gameEntity, context);

                // Get all the players in the game
                var gameMemberEntities = context.GameMembers.Include(gm => gm.Piece).Where(gm => gm.GameId == gameEntity.GameId).ToList();

                // Setup a player object for each member in the game
                foreach(var gameMemberEntity in gameMemberEntities)
                {
                    var pieceEntity = context.Pieces.Where(p => p.PieceId == gameMemberEntity.Piece.PieceId).Single();
                    var pieceType = GetPieceTypeFromColor(pieceEntity.Color);

                    var user = CreatePlayerFromGameMember(gameMemberEntity, pieceType, gameEntity, context);
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

        public static User GetUserByName(string name, LudoDbContext context)
        {
            try {
                return context.Users.Where(u => u.Name.ToLower() == name.ToLower()).Single();
            } catch { return null; }
        }

        public void SwitchPlayer()
        {
            int currentPlayerIndex = Players.FindIndex(pl => pl.Name == CurrentPlayer.Name);
            CurrentPlayer = Players[(currentPlayerIndex + 1) >= Players.Count ? 0 : currentPlayerIndex + 1];
            game.NextToRollDice = CurrentPlayer;
            context.Games.Update(game);
            context.SaveChanges();
        }

        public bool FindWinner()
        {
            var winner = Players.Find(pl => pl.Pieces.TrueForAll(p => p.Position >= p.EndPosition));
            Winner = winner;
            if (Winner != null)
            {
                game.Winner = Winner;
                game.Active = false;
                context.Games.Update(game);
                // SaveChanges needed?

                Winner.GamesWon = Winner.GamesWon == null ?  1 : Winner.GamesWon++;
                context.Users.Update(Winner);

                foreach(var player in Players)
                {
                    if (player.Name != Winner.Name)
                    {
                        player.GamesLost = player.GamesLost == null ? 1 : player.GamesLost++;
                        context.Users.Update(player);
                    }
                }

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

        public static List<Game> GetAllGames(LudoDbContext context)
        {
            return context.Games.Where(g => !g.Active).Include(g => g.Winner).OrderBy(g => g.GameId).ToList();
        }
    }
}
