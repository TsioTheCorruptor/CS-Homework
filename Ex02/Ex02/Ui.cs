using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Ex02.ConsoleUtils;


namespace UeserInterface
{
    internal class Ui
    {

        private Engine m_engine;
        private int m_guessAmount;

        public Ui(Engine i_engine) 
        {
            m_engine = i_engine;

        }

        public void Run ()
        {
            bool playAgain;
            do
            {
                promptUserForNumberOfGuesses();
                m_engine.ResetGame(m_guessAmount);
                int[] randomIndexes = m_engine.GetRandomObjectIndexes();
                m_correctAnswer = string.Join("", Array.ConvertAll(randomIndexes, i => ((char)(i + 'A')).ToString()));
                bool guessedCorrectly = false;

                for (int attempt = 0; attempt < m_guessAmount && !guessedCorrectly; attempt++)
                {
                    promptUserForGuess(attempt);
                    printGuessTable(attempt);
                    guessedCorrectly = m_engine.IsGuessCorrect(m_engine.m_historyMatrix[attempt, 0], m_correctAnswer);
                }

                printGameResultScreen(guessedCorrectly);
                playAgain = promptUserForRetry();

            } while (playAgain);
        }


        private void promptUserForNumberOfGuesses()
        {
            bool valid;
            do
            {
                Screen.Clear();
                Console.WriteLine("Enter number of guesses you want (4-10): ");
                valid = int.TryParse(Console.ReadLine(), out m_guessAmount);

            } while (!valid || m_guessAmount < 4 || m_guessAmount > 10);
        }

        private void promptUserForGuess()
        {
            bool valid;
            string guess;

            do
            {
                Screen.Clear();
                Console.WriteLine("Please type your next guess (4 letters between A-F):");
                guess = Console.ReadLine().ToUpper();
                valid = guess.Length == 4 && guess.All(c => c >= 'A' && c <= 'F');

            } while (!valid);

            int[] matchInfo = m_engine.GetGuessMatchingInfo(guess, m_correctAnswer);
            string pins = new string('V', matchInfo[0]) + new string('X', matchInfo[1]);
            pins = pins.PadRight(4);

            m_engine.m_historyMatrix[attempt, 0] = guess;
            m_engine.m_historyMatrix[attempt, 1] = pins;
        }

        private void printGuessTable()
        {
            Screen.Clear();
            Console.WriteLine("Current board status:");
            Console.WriteLine("|Pins:  |Result:|");

            for (int i = 0; i <= attempts; i++)
            {
                Console.WriteLine("|=======|=======|");
                Console.WriteLine($"| {m_engine.m_historyMatrix[i, 0]} | {m_engine.m_historyMatrix[i, 1]} |");
            }
            Console.WriteLine("|=======|=======|");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void printGuessTableLine()
        {

        }

        private void printGameResultScreen()
        {

            if (guessedCorrectly)
                Console.WriteLine($"You won in less than {m_guessAmount} guesses!");
            else
                Console.WriteLine($"You lost! Correct answer was {m_correctAnswer}");
        }

        private void promptUserForRetry()
        {
            Console.WriteLine("Would you like to start a new game? (Y/N)");
            string answer;
            do
            {
                answer = Console.ReadLine().ToUpper();
            } while (answer != "Y" && answer != "N");

            return answer == "Y";
        }

    }
}
