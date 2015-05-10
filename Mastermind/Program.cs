using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace Mastermind
{
    class Program
    {
        static Random rng = new Random();
        static string theGuess = rng.Next(1000, 9999).ToString();

        static void Main(string[] args)
        {
            int numTries = 0;
            int.TryParse(Properties.Settings.Default.NumberOfTries, out numTries);

            string tries = (numTries == 1) ? " try" : " tries";

            Console.WriteLine("Welcome to Mastermind! You have " + args[0] + tries + " to guess a randomly determined number!\n\nFor each guess submitted, the system will return a + for each character in your guess that matches both the number and position, and a - for a matching number, but not a matching position.\nGood luck!");
            
            do
            {
                Console.Write("\n\nPlease enter your next guess...");
                string userGuess = Console.ReadLine();
                userGuess = processGuess(userGuess);
                Console.WriteLine("Guess result: " + userGuess);

                if (userGuess == "++++")
                {
                    Console.WriteLine("You solved it!\r\nPlease press enter to exit...");
                    Console.ReadLine();
                    return;
                }
                
                else if(numTries == 1)
                {
                    Console.WriteLine("You lost :(\r\nPress y to play again or any other key to exit...");
                    if (Console.ReadLine() == "y")
                        Main(args);
                    else
                        return;
                }

                numTries--;
            } while (numTries > 0);
        }

        static string processGuess(string guess)
        {
            string result = "";
            char character = char.MinValue;
            int pos = 0;

            foreach(char x in guess)
            {
                if (x == theGuess[pos] && x != character)
                    result += "+";
                else if (theGuess.Contains(x) && x != character)
                    result += "-";

                character = x;
                pos++;
            }

            return result;
        }
    }
}
