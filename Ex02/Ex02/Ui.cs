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
        private int m_guessAmount = 0;
        private int m_maxGuessAmount;
        private int m_minGuessAmount;
        private int m_guessLength;
        private int m_letterArrayLength;
        private char[] m_charArray;
        private string[,]? m_gameHistoryMatrix = null;
        private bool m_playAnotherGame;
        private bool m_gameOngoing = false;


        public Ui(Engine i_engine, int i_minGuessNum, int i_maxGuessNum, int i_guessLength, char[] i_valid_chars) 
        {
            m_engine = i_engine;
            m_maxGuessAmount = i_maxGuessNum;
            m_minGuessAmount = i_minGuessNum;
            m_guessLength = i_guessLength;
            m_letterArrayLength = i_valid_chars.Length;

        }

        public void Run ()
        {
            bool playAgain;
            while (m_playAnotherGame)
            {
                initializeGame();
                mainGamplayLoop();
            }
            do
            {



            } while (playAgain);
        }

        private void initializeGame()
        {
            
            while (!m_gameOngoing)
            {
                promptUserForNumberOfGuesses();
                try
                {
                    m_engine.ResetGame(m_guessAmount, m_charArray);
                }
                catch
                {
                    Console.WriteLine("Error: ");
                }

                //m_gameOngoing = engine.isGameOngoing()

            }

        }

        private void mainGamplayLoop()
        {
            bool guessedCorrectly = false;
            bool gameOver = false;
            bool didUserWin = false;
            string userGuess;
            //m_gameHistoryMatrix = engine.get table
            //printGuessTable();


            while (m_gameOngoing)
            {

                userGuess = promptUserForGuess();
                // m_gameHistoryMatrix = m_engine.GetGuessMatchingInfo(userGuess);
                // printGuessTable();
                // m_gameOngoing = engine.isGameOngoing()
            }

            //didUserWin = engine.didUserWin()
            printGameResultScreen(didUserWin);

             promptUserForRetry();
        }


        private void promptUserForNumberOfGuesses()
        {
            bool valid = false;
            Screen.Clear();
            Console.WriteLine("Enter number of guesses you want ({0}-{1}): ", m_minGuessAmount, m_maxGuessAmount);
            valid = int.TryParse(Console.ReadLine(), out m_guessAmount);
            while (!valid || m_guessAmount < m_minGuessAmount || m_guessAmount > m_maxGuessAmount)
            {
                Console.WriteLine("Guess number  must be between  {0}-{1}", m_minGuessAmount, m_maxGuessAmount);
                Screen.Clear();
                Console.WriteLine("Enter number of guesses you want ({0}-{1}): ", m_minGuessAmount, m_maxGuessAmount);
                valid = int.TryParse(Console.ReadLine(), out m_guessAmount);
            }
        }

        private string promptUserForGuess()
        {
            bool valid;
            string guess;
            char firstArrayChar = m_charArray[0];
            char lastArrayChar = m_charArray[m_letterArrayLength - 1];

            Screen.Clear();
            Console.WriteLine("Please type your next guess ({0} letters between {1}-{2}):", m_guessLength, firstArrayChar, lastArrayChar);
            guess = Console.ReadLine().ToUpper();
            valid = guess.Length == 4 && guess.All(c => c >= firstArrayChar && c <= lastArrayChar);

            while (!valid)
            {
                Console.WriteLine("Invalid Guess!");
                Screen.Clear();
                Console.WriteLine("Please type your next guess ({0} letters between {1}-{2}):", m_guessLength, firstArrayChar, lastArrayChar);
                guess = Console.ReadLine().ToUpper();
                valid = guess.Length == 4 && guess.All(c => c >= firstArrayChar && c <= lastArrayChar);
            } 
            return guess;
        }

        private void printGuessTable()
        {
            //todo: print according to the matrix
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

        private void printGameResultScreen( bool i_userWonGame)
        {
            int numOfGuesses;
            //numOfGuesses = engine.getNumOfGuesses
            if (i_userWonGame)
                Console.WriteLine($"You won you guessesd in {numOfGuesses} guesses!");
            else
                Console.WriteLine($"You lost!");
        }

        private void promptUserForRetry()
        {

            string answer = "";
            while (answer != "Y" && answer != "N")
            {
                Console.WriteLine("Would you like to start a new game? (Y/N)");
                answer = Console.ReadLine().ToUpper();
            }
            if (answer == "Y")
            {
                m_playAnotherGame = true;
            }
            else
            {
                m_playAnotherGame = false;
            }
        }

    }
}
