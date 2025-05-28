using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class DiceParser
    {
        public static List<Dice> ParseDice(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("At least three dice must be provided. Example: 1,2,3,4,5,6 6,5,4,3,2,1 6,5,4,3,2,1");
            }

            var dice = new List<Dice>();
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    var faces = args[i].Split(',').Select(int.Parse).ToArray();
                    if (faces.Length < 1)
                    {
                        throw new ArgumentException($"Dice must have at least one face. Problem with dice {i + 1}");
                    }
                    dice.Add(new Dice(i, faces));
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"All values must be integers. Problem with dice {i + 1}");
                }
            }

            return dice;
        }
    }
}
