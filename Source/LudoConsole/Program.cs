using GameEngine;
using GameEngine.DataAccess;
using GameEngine.Models;
using System;
using System.Collections.Generic;

namespace LudoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            LudoDbContext dbContext = new LudoDbContext();
            LudoEngine game;

            do
            {
                choice = ChooseFromMainMenu();

                switch (choice)
                {
                    case 1:
                        var newGame = SetupNewGame(dbContext);
                        Play(newGame);
                        break;
                    case 2:
                        string gameToLoad = AskForGameNameToLoad();
                        game = LudoEngine.Load(gameToLoad, dbContext);
                        LoadGame(game);
                        break;
                    case 3:
                        var name = AskForUsername();
                        var user = LudoEngine.GetUserByName(name, dbContext);
                        ShowStatistics(user);
                        break;
                    case 4:
                        ShowAllGames(dbContext);
                        break;
                    default:
                        break;
                }
            }
            while (choice != 9);

        }

        private static void LoadGame(LudoEngine game)
        {
            if (game != null)
                if (game.Winner == null)
                    Play(game);
                else
                    Console.WriteLine($"{game.Winner.Name} won this game!");
            else
                Console.WriteLine("Sorry, couldn't find game");
        }

        private static LudoEngine SetupNewGame(LudoDbContext dbContext)
        {
            string gameName = AskForNewGameName(dbContext);

            LudoEngine game = new LudoEngine(dbContext, gameName);

            int numberOfPlayers = AskForNumberOfPlayers();
            AddPlayers(numberOfPlayers, game, dbContext);

            return game;
        }

        private static void ShowAllGames(LudoDbContext context)
        {
            var allGames = LudoEngine.GetAllGames(context);
            foreach (var game in allGames)
            {
                Console.WriteLine($"{game.Name} - Winner: {game.Winner.Name}\n");
            }
        }

        private static string AskForGameNameToLoad()
        {
            string gameToLoad;
            do
            {
                Console.Write("What's the name of the game to load? ");
                gameToLoad = Console.ReadLine();

            } while (gameToLoad.Length <= 0);
            
            return gameToLoad;
        }

        private static string AskForNewGameName(LudoDbContext context)
        {
            bool gameExists = true;
            string input;
            do {
                Console.Write("What should the game be called? ");
                input = Console.ReadLine();
                gameExists = LudoEngine.GameExists(input, context);
                if (gameExists)
                    Console.WriteLine("Sorry, name already taken.");
            } while (gameExists);

            return input;
        }

        private static void Play(LudoEngine game)
        {
            bool gameHasWinner = false;
            while (!gameHasWinner)
            {
                int moves = LetPlayerRollDice(game.CurrentPlayer, game);
                var moveablePiece = game.GetMoveablePieces(moves);
                if (moveablePiece.Count > 0)
                {
                    var pieceToMove = LetPlayerChoosePiece(moveablePiece, game);
                    if (pieceToMove != null)
                        MovePiece(moves, pieceToMove, game);
                }

                gameHasWinner = game.HasWinner();

                game.SwitchPlayer();


            }

            Console.WriteLine($"{game.Winner.Name} won the game!");
        }

        private static string AskForUsername()
        {
            Console.Write("Username? ");
            var name = Console.ReadLine();
            return name;
        }

        private static void ShowStatistics(User user)
        {
            if (user != null)
            {
                Console.WriteLine($"You've won {(user.GamesWon == null ? "0" : $"{user.GamesWon}")} games and lost {(user.GamesLost == null ? "0" : $"{user.GamesLost}")}!");
            }
            else
                Console.WriteLine("Couldn't find user");
        }

        private static IPiece LetPlayerChoosePiece(List<IPiece> moveablePieces, LudoEngine game)
        {
            if (moveablePieces.Count == 0)
                return null;

            int choice;
            do
            {
                Console.WriteLine("Which piece do you want to move?");
                for (int i = 0; i < moveablePieces.Count; i++)
                {
                    Console.WriteLine($"{i}: Piece at position {moveablePieces[i].Position}");
                }

                var choiceIsNumber = int.TryParse(Console.ReadLine(), out choice);
                choice = choiceIsNumber ? choice : -1;

            } while (choice < 0 || choice > moveablePieces.Count - 1);

            return moveablePieces[choice];
        }

        private static int LetPlayerRollDice(User player, LudoEngine game)
        {
            Console.WriteLine($"{player.Name} it's your turn. Enter 'r' to roll the dice.");
            var input = Console.ReadLine();

            if (input == "r")
            {
                int moves = game.ThrowDice();
                Console.WriteLine($"{player.Name} got a {moves}!");
                return moves;
            }
            return 0;
        }

        private static int ChooseFromMainMenu()
        {
            Console.WriteLine("What do you want do?");
            Console.WriteLine("1: Start new game");
            Console.WriteLine("2: Load game");
            Console.WriteLine("3: Show user statistics");
            Console.WriteLine("4: Show history of all games");
            Console.WriteLine("9: Exit");
            int.TryParse(Console.ReadLine(), out int choice);
            return choice;
        }

        private static int AskForNumberOfPlayers()
        {
            int numOfPlayers = 0;
            do
            {
                Console.Write("Enter number of players (between 2 and 4): ");
                int.TryParse(Console.ReadLine(), out numOfPlayers);

            } while (numOfPlayers > 4 || numOfPlayers < 2);

            return numOfPlayers;
        }

        private static void AddPlayers(int numOfPlayers, LudoEngine game, LudoDbContext context)
        { 
            for(int i = 0; i < numOfPlayers; i++)
            {
                var types = LudoEngine.GetPieceTypes();
                bool runLoop = true;
                do
                {

                    Console.Write($"Enter username for {types[i].Name.Replace("Piece", "")}: ");
                    var name = Console.ReadLine();

                    var user = LudoEngine.GetUserByName(name, context);
                    if (user == null)
                    {
                        game.AddPlayer(types[i], name);
                        runLoop = false;
                    }

                    else
                    {
                        Console.WriteLine($"{user.Name} already exists, do you want to use another name? (y/n)");
                        var answer = Console.ReadLine();
                        if (answer.ToLower() == "y")
                            runLoop = true;
                        else
                        {
                            game.AddPlayer(types[i], user.Name);
                            runLoop = false;
                        }
                    }

                } while (runLoop);

            }

            game.SaveGame();
            
        }

        private static void MovePiece(int moves, IPiece piece, LudoEngine game)
        {
            bool couldMove = game.MovePiece(piece, moves);

            if (couldMove)
            {
                if (game.PieceIsInGoal(piece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} entered goal with a piece!");

                var collidingPiece = game.FindCollidingPiece(piece.Position + moves);
                if (collidingPiece != null && game.PieceIsEnemy(collidingPiece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} knocked away a {collidingPiece.GetType().Name.Replace("Piece", "")} piece!");
                if (collidingPiece == null && !game.PieceIsInGoal(piece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} moved a piece to position {piece.Position}");
            }
            else
            {
                Console.WriteLine("Sorry, can't do that");
            }
        }
    }

}
