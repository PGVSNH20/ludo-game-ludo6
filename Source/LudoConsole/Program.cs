using GameEngine;
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
                                HandleMoves(moves, player, game);

                            }
                        }
                        break;
                    case 2: game.Load();
                        break;
                    case 3: game.GetStatistics();
                        break;
                    default:
                        break;
                }
            }
            while (choice != 9);

        }

        private static void HandleMoves(int moves, Player player, LudoEngine game)
        {
            List<Piece> PiecesInNest = game.GetPiecesInNest(player);
            List<Piece> PiecesInPlay = game.GetPiecesInPlay(player);

            if (moves == 6)
            {
                // If all piece are in the nest, automatically move one out
                if (PiecesInNest.Count == 4)
                {
                    game.MovePiece(player.Pieces[0], 1);
                }
                // If there are pieces both in the nest and in play, let the user choose one of all pieces and move it.
                else
                    game.MovePiece(LetPlayerChoosePiece(player.Pieces, game), moves);
            }
            else
            {
                // If there's only one piece in play, automatically move the piece
                if (PiecesInPlay.Count == 1)
                {
                    game.MovePiece(PiecesInPlay[0], moves);
                }
                // If there are multiple pieces in play, let the user choose between those pieces and move it.
                if (PiecesInPlay.Count > 1)
                {
                    game.MovePiece(LetPlayerChoosePiece(PiecesInPlay, game), moves);
                }
            }
        }

        private static Piece LetPlayerChoosePiece(List<Piece> pieces, LudoEngine game)
        {
            Console.WriteLine("Which piece do you want to move?");
            for (int i = 0; i < pieces.Count; i++)
            {
                Console.WriteLine($"{i}: Piece at position {pieces[i].Position}");
            }
            int choice = int.Parse(Console.ReadLine());
            return pieces[choice];
        }

        private static int LetPlayerRollDice(Player player, LudoEngine game)
        {
            Console.WriteLine($"{player.Name} it's your turn. Enter 'r' to roll the dice.");
            var input = Console.ReadLine();

            if (input == "r")
            {
                int moves = game.ThrowDice();
                Console.WriteLine($"You got a {moves}!");
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
                string[] colors = { "Red", "Blue", "Green", "Yellow" };
                Console.Write($"Enter username for {colors[i]}: ");
                var name = Console.ReadLine();

                game.AddPlayer(colors[i], name);

            }
            
        }
    }

}
