using System.Text;
using Logic;
using Ex02.ConsoleUtils;


namespace UeserInterface
{
    internal class Ui
    {
        private readonly string k_QuitCommand = "Q";

        private Engine m_engine;
        private int m_guessAmount = 0;
        private int m_maxGuessAmount;
        private int m_minGuessAmount;
        private int m_guessLength;
        private int m_letterArrayLength;
        private char[] m_charArray;
        private string[,]? m_gameHistoryMatrix = null;
        private bool m_playAnotherGame = true;
        private bool m_gameOngoing = false;


        public Ui(Engine i_engine, int i_guessLength, int i_minGuessNum, int i_maxGuessNum, char[] i_valid_chars) 
        {
            m_engine = i_engine;
            m_maxGuessAmount = i_maxGuessNum;
            m_minGuessAmount = i_minGuessNum;
            m_guessLength = i_guessLength;
            m_charArray = i_valid_chars;
            m_letterArrayLength = i_valid_chars.Length;

        }

        public void Run ()
        {
            while (m_playAnotherGame)
            {
                initializeGame();
                mainGamplayLoop();
            }

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
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}",ex.Message);
                }
                m_gameOngoing = m_engine.IsGameOnGoing;
            }

        }

        private void mainGamplayLoop()
        {
            string userGuess;
            m_gameHistoryMatrix = m_engine.HistoryMatrix;
            printGuessTable();


            while (m_gameOngoing)
            {
                userGuess = promptUserForGuess();
                if (userGuess == k_QuitCommand) 
                {
                    m_playAnotherGame = false;
                    m_gameOngoing = false;
                    return;
                }
                m_engine.GetGuessInfoAndUpdate(userGuess);
                m_gameHistoryMatrix = m_engine.HistoryMatrix;
                printGuessTable();
                m_gameOngoing = m_engine.IsGameOnGoing;
            }

            printGameResultScreen();
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
                Screen.Clear();
                Console.WriteLine("Guess number  Must be between  {0}-{1}", m_minGuessAmount, m_maxGuessAmount);
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
            string lettersDisplay = string.Join(" ", m_charArray);

            Console.WriteLine("Please type your next guess <{0}> or '{1}' to quit:", lettersDisplay, k_QuitCommand);
            guess = Console.ReadLine().ToUpper();

            valid = (guess.Length == m_guessLength && guess.All(c => c >= firstArrayChar && c <= lastArrayChar))|| guess == k_QuitCommand;

            while (!valid)
            {
                Console.WriteLine("Invalid Guess!");
                Console.WriteLine("Please type your next guess <{0}> or '{1}' to quit:", lettersDisplay, k_QuitCommand);
                guess = Console.ReadLine().ToUpper();

                valid = (guess.Length == m_guessLength && guess.All(c => c >= firstArrayChar && c <= lastArrayChar)) || guess == k_QuitCommand;
            } 
            return guess;
        }


        private void printGuessTable()
        {
            Screen.Clear();
            Console.WriteLine("Current board status:");

            // +2 spaces (one on each side) compared to the old formula
            int colWidth = m_guessLength * 2 + 1;
            string hSep = new string('=', colWidth);
            string divider = $"|{hSep}|{hSep}|";

            Console.WriteLine($"|{"Pins:".PadRight(colWidth)}|{"Result:".PadRight(colWidth)}|");
            Console.WriteLine(divider);

            string[,] matrix = m_engine.HistoryMatrix;
            int rows = matrix.GetLength(0);

            for (int row = 0; row < rows; row++)
            {
                string rawPins = matrix[row, 0];
                string rawResult = matrix[row, 1];

                string pinsCell = buildSpacedCell(rawPins, m_guessLength, colWidth);
                string resultCell = buildSpacedCell(rawResult, m_guessLength, colWidth);

                Console.WriteLine($"|{pinsCell}|{resultCell}|");
                Console.WriteLine(divider);
            }
            Console.WriteLine();
        }

        private static string buildSpacedCell(string raw, int pegCount, int colWidth)
        {
            var cell = new StringBuilder();
            cell.Append(' ');

            for (int i = 0; i < pegCount; i++)
            {
                char cellChar = (i < raw.Length) ? raw[i] : ' ';
                cell.Append(cellChar);

                if (i < pegCount - 1)
                    cell.Append(' ');
            }

            cell.Append(' '); 

            return cell.ToString().PadRight(colWidth);
        }
        private void printGameResultScreen()
        {
            int numOfGuesses = m_engine.TriesAmount;
            bool userWon = m_engine.IsGameWon;
            if (userWon)
                Console.WriteLine("You won you guessesd in {0} guesses!", numOfGuesses);
            else
                Console.WriteLine("You lost!");
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
