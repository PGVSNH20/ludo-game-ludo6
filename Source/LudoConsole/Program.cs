using GameEngine;
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
            LudoEngine game = new LudoEngine();
            Player winner = null;

            do
            {
                choice = ChooseFromMainMenu();

                switch (choice)
                {
                    case 1:
                        int numberOfPlayers = AskForNumberOfPlayers();
                        AddPlayers(numberOfPlayers, game);
                        while(winner == null)
                        {
                            foreach (var player in game.Players)
                            {
                                int moves = LetPlayerRollDice(player, game);
                                var moveablePiece = game.GetMoveablePieces(moves);
                                if (moveablePiece.Count > 0)
                                {
                                    var pieceToMove = LetPlayerChoosePiece(moveablePiece, game);
                                    MovePiece(moves, pieceToMove, game);
                                }

                            }
                        }
                        break;
                    case 2: game.Load();
                        break;
                    case 3:
                        var name = AskForUsername();
                        var user = game.GetUserByName(name);
                        if (user != null)
                        {
                            ShowStatistics(user);
                        }
                        else
                            Console.WriteLine("Couldn't find user");
                        break;
                    default:
                        break;
                }
            }
            while (choice != 9);

        }

        private static string AskForUsername()
        {
            Console.Write("Username? ");
            var name = Console.ReadLine();
            return name;
        }

        private static void ShowStatistics(User user)
        {
            Console.WriteLine($"You've won {user.GamesWon} games and lost {user.GamesLost}!");
        }

        private static IPiece LetPlayerChoosePiece(List<IPiece> moveablePieces, LudoEngine game)
        {
            Console.WriteLine("Which piece do you want to move?");
            for (int i = 0; i < moveablePieces.Count; i++)
            {
                Console.WriteLine($"{i}: Piece at position {moveablePieces[i].Position}");
            }
            int choice = int.Parse(Console.ReadLine());
            return moveablePieces[choice];
        }

        private static int LetPlayerRollDice(Player player, LudoEngine game)
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

        private static void AddPlayers(int numOfPlayers, LudoEngine game)
        { 
            for(int i = 0; i < numOfPlayers; i++)
            {
                var types = game.GetPieceTypes();

                Console.Write($"Enter username for {types[i].Name.Replace("Piece", "")}: ");
                var name = Console.ReadLine();

                game.AddPlayer(types[i], name);

            }
            
        }

        private static void MovePiece(int moves, IPiece piece, LudoEngine game)
        {
            bool couldMove = game.MovePiece(piece, moves);

            if (couldMove)
            {
                if (game.CheckIfEnteringGoal(piece, moves))
                    Console.WriteLine($"{game.CurrentPlayer.Name} entered goal with a piece!");

                var collidingPiece = game.FindCollidingPiece(piece.Position + moves);
                if (collidingPiece != null && game.PieceIsEnemy(collidingPiece))
                    Console.WriteLine($"{game.CurrentPlayer.Name} knocked an enemy piece!");
                if (collidingPiece == null)
                    Console.WriteLine($"{game.CurrentPlayer.Name} moved a piece to position {piece.Position}");
            }
            else
            {
                Console.WriteLine("Sorry, can't do that");
            }
        }
    }

}
