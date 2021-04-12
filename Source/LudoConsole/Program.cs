using GameEngine;
using GameEngine.DataAccess;
using GameEngine.GameModels;
using GameEngine.Models;
using System;
using System.Collections.Generic;

namespace LudoConsole
{
    class Program
    {
        private static LudoDbContext dbContext;
        private static string[] commonZone = new string[41];
        private static string[] blueSafeZone = new string[5];
        private static string[] greenSafeZone = new string[5];
        private static string[] redSafeZone = new string[5];
        private static string[] yellowSafeZone = new string[5];
        private static List<string[]> safeZones = new List<string[]>() { blueSafeZone, greenSafeZone, redSafeZone, yellowSafeZone };

        static void Main(string[] args)
        {
            dbContext = new LudoDbContext();

            int choice;
            do
            {
                choice = ChooseFromMainMenu();

                switch (choice)
                {
                    case 1: // Create new game
                        var newGame = SetupNewGame();
                        Play(newGame);
                        break;
                    case 2: // Load game from database
                        string gameToLoad = AskForGameNameToLoad();
                        var game = LudoEngine.Load(gameToLoad, dbContext);
                        ShowOrPlayLoadedGame(game);
                        break;
                    case 3: // Show user statistics
                        var name = AskForUsername();
                        var user = LudoEngine.GetUserByName(name, dbContext);
                        ShowStatistics(user);
                        break;
                    case 4: // Show info about all games in database
                        ShowAllGames();
                        break;
                    default:
                        break;
                }
            }
            while (choice != 9);

        }

        private static void ShowOrPlayLoadedGame(LudoEngine game)
        {
            if (game != null)
                if (game.Winner == null)
                    Play(game);
                else
                    Console.WriteLine($"{game.Winner.Name} won this game!");
            else
                Console.WriteLine("Sorry, couldn't find game");
        }

        private static LudoEngine SetupNewGame()
        {
            string gameName = AskForNewGameName();

            LudoEngine game = new LudoEngine(dbContext, gameName);

            int numberOfPlayers = AskForNumberOfPlayers();
            AddPlayers(numberOfPlayers, game);

            return game;
        }

        private static void ShowAllGames()
        {
            var allGames = LudoEngine.GetAllGames(dbContext);
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

        private static string AskForNewGameName()
        {
            bool gameExists = true;
            string input;
            do {
                Console.Write("What should the game be called? ");
                input = Console.ReadLine();
                gameExists = LudoEngine.GameExists(input, dbContext);
                if (gameExists)
                {
                    Console.Clear();
                    Console.WriteLine("Sorry, name already taken.");
                }                    
            } while (gameExists);

            return input;
        }

        private static void Play(LudoEngine game)
        {
            bool gameHasWinner = false;
            while (!gameHasWinner)
            {
                Console.Clear();
                DrawBoard(game);
                int moves = LetPlayerRollDice(game.CurrentPlayer, game);
                Console.WriteLine($"{game.CurrentPlayer.Name} got a {moves}!");
                Console.WriteLine();
                var moveablePiece = game.GetMoveablePieces(moves);
                if (moveablePiece.Count > 0)
                {
                    var pieceToMove = LetPlayerChoosePiece(moveablePiece, game);
                    if (pieceToMove != null)
                    {
                        bool moved = MovePiece(moves, pieceToMove, game);

                        while (!moved)
                        {
                            pieceToMove = LetPlayerChoosePiece(moveablePiece, game);
                            moved = MovePiece(moves, pieceToMove, game);
                        }
                    }
                }
                gameHasWinner = game.FindWinner();
                game.SwitchPlayer();
            }
            Console.WriteLine();
            Console.WriteLine($"{game.Winner.Name} won the game!");
        }

        private static void DrawPlayerSafeZone(string[] safeZone, List<IPiece> pieces)
        {
            for (int i = 1; i < safeZone.Length; i++)
            {
                foreach (var piece in pieces)
                {
                    if (piece.RelativePosition > 40 && piece.RelativePosition < 45)
                    {
                        var zonePosition = piece.RelativePosition - 40;
                        if (zonePosition == i)
                            safeZone[i] = piece.GetType().Name.Remove(1);
                    }
                }
            }
        }

        private static void FillZones()
        {
            foreach (var safeZone in safeZones)
            {
                for (int i = 1; i < safeZone.Length; i++)
                {
                    safeZone[i] = "O";
                }
            }
        }

        private static void DrawBoard(LudoEngine game)
        {
            int bluesInNest = 0;
            int greensInNest = 0;
            int redsInNest = 0;
            int yellowsInNest = 0;

            foreach (var player in game.Players)
            {
                if (player.Pieces[0].GetType() == typeof(BluePiece))
                    bluesInNest = game.PlayerPiecesInNest(player).Count;
                if (player.Pieces[0].GetType() == typeof(GreenPiece))
                    greensInNest = game.PlayerPiecesInNest(player).Count;
                if (player.Pieces[0].GetType() == typeof(RedPiece))
                    redsInNest = game.PlayerPiecesInNest(player).Count;
                if (player.Pieces[0].GetType() == typeof(YellowPiece))
                    yellowsInNest = game.PlayerPiecesInNest(player).Count;
            }

            FillZones();
            FillZonesWithPieces(game);

            Console.WriteLine($"          {commonZone[39]} {commonZone[40]} {commonZone[1]}");
            Console.WriteLine($"    Y:{yellowsInNest}   {commonZone[38]} {blueSafeZone[1]} {commonZone[2]}    B:{bluesInNest}");
            Console.WriteLine($"          {commonZone[37]} {blueSafeZone[2]} {commonZone[3]}");
            Console.WriteLine($"          {commonZone[36]} {blueSafeZone[3]} {commonZone[4]}");
            Console.WriteLine($"  {commonZone[31]} {commonZone[32]} {commonZone[33]} {commonZone[34]} {commonZone[35]} {blueSafeZone[4]} {commonZone[5]} {commonZone[6]} {commonZone[7]} {commonZone[8]} {commonZone[9]}");
            Console.WriteLine($"  {commonZone[30]} {yellowSafeZone[1]} {yellowSafeZone[2]} {yellowSafeZone[3]} {yellowSafeZone[4]}   {greenSafeZone[4]} {greenSafeZone[3]} {greenSafeZone[2]} {greenSafeZone[1]} {commonZone[10]}");
            Console.WriteLine($"  {commonZone[29]} {commonZone[28]} {commonZone[27]} {commonZone[26]} {commonZone[25]} {redSafeZone[4]} {commonZone[15]} {commonZone[14]} {commonZone[13]} {commonZone[12]} {commonZone[11]}");
            Console.WriteLine($"          {commonZone[24]} {redSafeZone[3]} {commonZone[16]}");
            Console.WriteLine($"          {commonZone[23]} {redSafeZone[2]} {commonZone[17]}");
            Console.WriteLine($"    R:{redsInNest}   {commonZone[22]} {redSafeZone[1]} {commonZone[18]}    G:{greensInNest}");
            Console.WriteLine($"          {commonZone[21]} {commonZone[20]} {commonZone[19]}");
        }

        private static void FillZonesWithPieces(LudoEngine game)
        {
            foreach (var player in game.Players)
            {
                if (player.Pieces[0].GetType() == typeof(BluePiece))
                {
                    DrawPlayerSafeZone(blueSafeZone, player.Pieces);
                }
                else if (player.Pieces[0].GetType() == typeof(GreenPiece))
                {
                    DrawPlayerSafeZone(greenSafeZone, player.Pieces);
                }
                else if (player.Pieces[0].GetType() == typeof(RedPiece))
                {
                    DrawPlayerSafeZone(redSafeZone, player.Pieces);
                }
                if (player.Pieces[0].GetType() == typeof(YellowPiece))
                {
                    DrawPlayerSafeZone(yellowSafeZone, player.Pieces);
                }
            }

            for (int i = 1; i < commonZone.Length; i++)
            {
                commonZone[i] = "O";
                foreach (var player in game.Players)
                {
                    foreach (var piece in player.Pieces)
                    {
                        if (piece.AbsoluteBoardPosition == i && !game.PieceIsInSafeZone(piece))
                            commonZone[i] = piece.GetType().Name.Remove(1);
                    }

                }
            }
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
                Console.WriteLine();
                Console.WriteLine($"You've won {(user.GamesWon == null ? "0" : $"{user.GamesWon}")} games and lost {(user.GamesLost == null ? "0" : $"{user.GamesLost}")}!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Couldn't find user");
            }               
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
                    Console.WriteLine($"{i}: Piece at position {moveablePieces[i].RelativePosition}/45");
                }

                var choiceIsNumber = int.TryParse(Console.ReadLine(), out choice);
                choice = choiceIsNumber ? choice : -1;

            } while (choice < 0 || choice > moveablePieces.Count - 1);

            return moveablePieces[choice];
        }

        private static int LetPlayerRollDice(User player, LudoEngine game)
        {
            string input;
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{player.Name} it's your turn. Enter 'r' to roll the dice.");
                input = Console.ReadLine();

            } while (input != "r");

            int moves = game.ThrowDice();
            return moves;
        }

        private static int ChooseFromMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine();
            Console.WriteLine("1: Start new game");
            Console.WriteLine("2: Load game");
            Console.WriteLine("3: Show user statistics");
            Console.WriteLine("4: Show history of all games");
            Console.WriteLine("9: Exit");
            int.TryParse(Console.ReadLine(), out int choice);
            Console.Clear();
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

        private static void AddPlayers(int numOfPlayers, LudoEngine game)
        { 
            for(int i = 0; i < numOfPlayers; i++)
            {
                var types = LudoEngine.GetPieceTypes();
                bool runLoop = true;
                do
                {
                    Console.Write($"Enter username for {types[i].Name.Replace("Piece", "")}: ");
                    var name = Console.ReadLine();

                    var user = LudoEngine.GetUserByName(name, dbContext);
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

        private static bool MovePiece(int moves, IPiece piece, LudoEngine game)
        {
            bool couldMove = game.MovePiece(piece, moves);

            if (couldMove)
            {
                if (game.PieceIsInGoal(piece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} entered goal with a piece!");

                var collidingPiece = game.CollidingPiece;
                if (collidingPiece != null && game.PieceIsEnemy(collidingPiece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} knocked away a {collidingPiece.GetType().Name.Replace("Piece", "")} piece!");
                if (collidingPiece == null && !game.PieceIsInGoal(piece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} moved a piece to position {piece.RelativePosition}/45");
            }
            else
            {
                Console.WriteLine("Sorry, can't do that");
            }
            
            return couldMove;
        }
    }

}
