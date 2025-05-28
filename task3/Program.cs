using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace task3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var dice = DiceParser.ParseDice(args);
                var game = new Game(dice);
                game.DetermineFirstMove();
                game.PlayRound();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Example usage: dotnet run 1,2,3,4,5,6 6,5,4,3,2,1 2,2,2,8,8,8");
            }
        }
    }
}