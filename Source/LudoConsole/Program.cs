using GameEngine;
using System;

namespace LudoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            LudoEngine game = new LudoEngine();
            do
            {
                Console.WriteLine("What do you want do?");
                Console.WriteLine("1: Start new game");
                Console.WriteLine("2: Load game");
                Console.WriteLine("3: Show user statistics");
                Console.WriteLine("9: Exit");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1: game.Create();
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
    }
}
