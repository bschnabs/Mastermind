using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using BitFactory.Logging;

namespace Mastermind
{
    class Program
    {
        static Random rng = new Random();

        static void Main(string[] args)
        {
            string theGuess = rng.Next(0, 9999).ToString();
            if(theGuess.Length < 4)
            {
                for (int x = 0; x < 5 - theGuess.Length; x++)
                    theGuess = "0" + theGuess;
            }

            FileLogger masterLogger = new FileLogger(Properties.Settings.Default.LoggingDir + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_hhmmss") + ".txt");

            int numTries = 0;
            int.TryParse(Properties.Settings.Default.NumberOfTries, out numTries);
            masterLogger.Log(LogSeverity.Info, "Mastermind - New Game Begin | Answer: " + theGuess + " | Number of tries: " + numTries);

            string tries = (numTries == 1) ? " try" : " tries";

            Console.WriteLine("Welcome to Mastermind! You have " + numTries + tries + " to guess a randomly determined four digit number!\n\nFor each guess submitted, the system will return a + for each character in your guess that matches both the number and position, and a - for a matching number, but not a matching position.\nGood luck!");
            
            do
            {
                Console.Write("\n\nPlease enter your next guess..." + numTries + " remaining...\n");
                string userGuess = Console.ReadLine();

                int userCheck;
                while(userGuess.Length != 4 || !int.TryParse(userGuess, out userCheck))
                {
                    Console.WriteLine("Your guess must be a 4 digit number...\nPlease enter another guess...");
                    userGuess = Console.ReadLine();
                }

                masterLogger.Log(LogSeverity.Info, "User guess: " + userGuess + " | Answer: " + theGuess);    
                userGuess = processGuess(userGuess, theGuess);
                Console.WriteLine("Guess result: " + userGuess);
                masterLogger.Log(LogSeverity.Info, "Guess Result: " + userGuess);

                if (userGuess == "++++")
                {
                    Console.WriteLine("\nYou solved it!\nEnter y to play again or enter to exit...");
                    if (Console.ReadLine() == "y")
                        Main(args);
                    else
                    {
                        numTries -= numTries;
                        return;
                    }
                }
                
                else if(numTries == 1)
                {
                    Console.WriteLine("You lost :( - the answer was " + theGuess + "\r\nPress y to play again or any other key to exit...");
                    if (Console.ReadLine() == "y")
                        Main(args);
                    else
                        return;
                }

                numTries--;
            } while (numTries > 0);
        }

        static string processGuess(string guess, string theGuess)
        {
            string result = "";
            int pos = 0;
            List<char> submitList = new List<char>();

            foreach(char x in guess)
            {
                if (x == theGuess[pos])
                {
                    result += "+";
                    submitList.Add(x);
                }
                
                pos++;
            }

            foreach (char x in guess)
            {
                if (theGuess.Contains(x) && !submitList.Contains(x))
                    result += "-";
            }

            return String.Concat(result.OrderBy(x => x));
        }
    }
}
