using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class Game
    {
        private readonly List<Dice> _dice;
        private bool _userFirstMove;

        public Game(List<Dice> dice)
        {
            _dice = dice;
        }

        public void DetermineFirstMove()
        {
            Console.WriteLine("Let's determine who makes the first move.");
            Console.WriteLine("I selected a random value in the range 0..1");

            var random = new FairRandomGenerator(0, 2);
            Console.WriteLine($"(HMAC={random.Hmac})");
            Console.WriteLine("Try to guess my selection.");

            while (true)
            {
                Console.WriteLine("0 - 0");
                Console.WriteLine("1 - 1");
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Your selection: ");

                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "X")
                {
                    Environment.Exit(0);
                }
                else if (input == "?")
                {
                    Console.WriteLine(ProbabilityCalculator.GenerateProbabilityTable(_dice));
                    continue;
                }

                if (int.TryParse(input, out int userChoice) && (userChoice == 0 || userChoice == 1))
                {
                    int result = FairRandomGenerator.CalculateFairResult(random.ComputerNumber, userChoice, 2);
                    Console.WriteLine($"My selection: {random.ComputerNumber} (KEY={random.Key}).");
                    Console.WriteLine($"The fair number generation result is {random.ComputerNumber} + {userChoice} = {result} (mod 2).");

                    _userFirstMove = result == 0;
                    Console.WriteLine($"{(result == 0 ? "You" : "I")} make the first move.");
                    break;
                }

                Console.WriteLine("Invalid input. Please enter 0, 1, X or ?.");
            }
        }

        public void PlayRound()
        {
            Dice userDice = null;
            Dice computerDice = null;

            if (_userFirstMove)
            {
                userDice = SelectDice("Choose your dice:");
                computerDice = _dice.First(d => d != userDice);
                Console.WriteLine($"I choose the {computerDice} dice.");
            }
            else
            {
                computerDice = _dice[FairRandomGenerator.GetSecureRandomInRange(0, _dice.Count)];
                Console.WriteLine($"I make the first move and choose the {computerDice} dice.");
                userDice = SelectDice("Choose your dice:", computerDice);
            }

            if (_userFirstMove)
            {
                int userRoll = PerformRoll($"It's time for your roll.", userDice);
                int computerRoll = PerformRoll($"It's time for my roll.", computerDice);
                DetermineWinner(userRoll, computerRoll, true);
            }
            else
            {
                int computerRoll = PerformRoll($"It's time for my roll.", computerDice);
                int userRoll = PerformRoll($"It's time for your roll.", userDice);
                DetermineWinner(userRoll, computerRoll, false);
            }
        }

        private Dice SelectDice(string prompt, Dice excludedDice = null)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                for (int i = 0; i < _dice.Count; i++)
                {
                    if (_dice[i] == excludedDice) continue;
                    Console.WriteLine($"{i} - {_dice[i]}");
                }
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Your selection: ");

                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "X")
                {
                    Environment.Exit(0);
                }
                else if (input == "?")
                {
                    Console.WriteLine(ProbabilityCalculator.GenerateProbabilityTable(_dice));
                    continue;
                }

                if (int.TryParse(input, out int selection) && selection >= 0 && selection < _dice.Count)
                {
                    var selectedDice = _dice[selection];
                    if (excludedDice != null && selectedDice == excludedDice)
                    {
                        Console.WriteLine("You cannot select the same dice as the first player.");
                        continue;
                    }
                    Console.WriteLine($"You choose the {selectedDice} dice.");
                    return selectedDice;
                }

                Console.WriteLine("Invalid input. Please enter a valid dice number, X or ?.");
            }
        }

        private int PerformRoll(string message, Dice dice)
        {
            Console.WriteLine(message);
            var random = new FairRandomGenerator(0, dice.Faces.Length);
            Console.WriteLine($"I selected a random value in the range 0..{dice.Faces.Length - 1}");
            Console.WriteLine($"(HMAC={random.Hmac})");
            Console.WriteLine($"Add your number modulo {dice.Faces.Length}.");

            for (int i = 0; i < dice.Faces.Length; i++)
            {
                Console.WriteLine($"{i} - {i}");
            }
            Console.WriteLine("X - exit");
            Console.WriteLine("? - help");

            while (true)
            {
                Console.Write("Your selection: ");
                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "X")
                {
                    Environment.Exit(0);
                }
                else if (input == "?")
                {
                    Console.WriteLine(ProbabilityCalculator.GenerateProbabilityTable(_dice));
                    continue;
                }

                if (int.TryParse(input, out int userChoice) && userChoice >= 0 && userChoice < dice.Faces.Length)
                {
                    int result = FairRandomGenerator.CalculateFairResult(random.ComputerNumber, userChoice, dice.Faces.Length);
                    Console.WriteLine($"My number is {random.ComputerNumber} (KEY={random.Key}).");
                    Console.WriteLine($"The fair number generation result is {random.ComputerNumber} + {userChoice} = {result} (mod {dice.Faces.Length}).");

                    int rollResult = dice.Roll(result);
                    Console.WriteLine($"{(dice == _dice[0] ? "Your" : "My")} roll result is {rollResult}.");
                    return rollResult;
                }

                Console.WriteLine($"Invalid input. Please enter a number between 0 and {dice.Faces.Length - 1}, X or ?.");
            }
        }

        private void DetermineWinner(int roll1, int roll2, bool userRolledFirst)
        {
            if (roll1 > roll2)
            {
                Console.WriteLine($"{(userRolledFirst ? "You" : "I")} win ({roll1} > {roll2})!");
            }
            else if (roll2 > roll1)
            {
                Console.WriteLine($"{(userRolledFirst ? "I" : "You")} win ({roll2} > {roll1})!");
            }
            else
            {
                Console.WriteLine($"It's a tie ({roll1} = {roll2})!");
            }
        }
    }
}