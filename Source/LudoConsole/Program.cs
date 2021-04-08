﻿using GameEngine;
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
                        string gameName = AskForGameName(dbContext);
                        game = new LudoEngine(dbContext, gameName);
                        int numberOfPlayers = AskForNumberOfPlayers();
                        AddPlayers(numberOfPlayers, game);
                        Play(game);
                        break;
                    case 2:
                        game = new LudoEngine(dbContext, "");
                        game.Load("");
                        break;
                    case 3:
                        game = new LudoEngine(dbContext, "");
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

        private static string AskForGameName(LudoDbContext context)
        {
            string input;
            do {
                Console.Write("What should the game be called? ");
                input = Console.ReadLine();
            } while (LudoEngine.GameExists(input, context));

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
            Console.WriteLine($"You've won {user.GamesWon} games and lost {user.GamesLost}!");
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
                bool runLoop = true;
                do
                {

                    Console.Write($"Enter username for {types[i].Name.Replace("Piece", "")}: ");
                    var name = Console.ReadLine();

                    var user = game.GetUserByName(name);
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
                            runLoop = false;
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
