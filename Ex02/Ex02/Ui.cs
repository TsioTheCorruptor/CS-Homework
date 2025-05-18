using System.Text;
using Logic;
using Ex02.ConsoleUtils;


namespace UeserInterface
{
    internal class Ui
    {
        private readonly string k_QuitCommand = "Q";
        private readonly string k_YesString = "Y";
        private readonly string k_NoString = "N";

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
            bool quitting = false;

            m_gameHistoryMatrix = m_engine.HistoryMatrix;

            printGuessTable();


            while (m_gameOngoing)
            {
                userGuess = promptUserForGuess();
                if (userGuess == k_QuitCommand) 
                {
                    m_playAnotherGame = false;
                    m_gameOngoing = false;
                    quitting = true;
                }
                else
                {
                    m_engine.GetGuessInfoAndUpdate(userGuess);
                    m_gameHistoryMatrix = m_engine.HistoryMatrix;
                    printGuessTable();
                    m_gameOngoing = m_engine.IsGameOnGoing;
                }

            }

            if (!quitting)
            {
                printGameResultScreen();
                promptUserForRetry();
            }

        }


        private void promptUserForNumberOfGuesses()
        {
            bool valid = false;
            Screen.Clear();
            Console.WriteLine("Enter number of guesses you want <{0}-{1}>: ", m_minGuessAmount, m_maxGuessAmount);
            valid = int.TryParse(Console.ReadLine(), out m_guessAmount);
            while (!valid || m_guessAmount < m_minGuessAmount || m_guessAmount > m_maxGuessAmount)
            {
                Screen.Clear();
                Console.WriteLine("Guess number  Must be between  {0} to {1}", m_minGuessAmount, m_maxGuessAmount);
                Console.WriteLine("Enter number of guesses you want <{0}-{1}>: ", m_minGuessAmount, m_maxGuessAmount);
                valid = int.TryParse(Console.ReadLine(), out m_guessAmount);
            }
        }

        private string promptUserForGuess()
        {
            bool valid = false;
            string? guess;
            char firstArrayChar = m_charArray[0];
            char lastArrayChar = m_charArray[m_letterArrayLength - 1];
            string lettersDisplay = string.Join(" ", m_charArray);

            Console.WriteLine("Please type your next guess <{0}> or '{1}' to quit:", lettersDisplay, k_QuitCommand);
            guess = Console.ReadLine();
            if (guess != null)
            {
                valid = isValidGuess(guess);
            }

            while (!valid)
            {
                Console.WriteLine("Invalid Guess!");
                Console.WriteLine("Please type your next guess <{0}> or '{1}' to quit:", lettersDisplay, k_QuitCommand);
                guess = Console.ReadLine();
                if (guess != null)
                {
                    valid = isValidGuess(guess);
                }
            }

            if (guess == null)
            {
                guess = k_QuitCommand;
            }
            return guess;
        }


        private void printGuessTable()
        {
            Screen.Clear();
            Console.WriteLine("Current board status:");

            int colWidth = m_guessLength * 2 + 1;
            string hSep = new string('=', colWidth);
            string divider =string.Format( "|{0}|{1}|",hSep,hSep);

            Console.WriteLine(string.Format("|{0}|{1}|","Pins:".PadRight(colWidth),"Result:".PadRight(colWidth)));
            Console.WriteLine(divider);

            string[,] matrix = m_engine.HistoryMatrix;
            int rows = matrix.GetLength(0);

            for (int row = 0; row < rows; row++)
            {
                string rawPins = matrix[row, 0];
                string rawResult = matrix[row, 1];

                string pinsCell = buildSpacedCell(rawPins, m_guessLength, colWidth);
                string resultCell = buildSpacedCell(rawResult, m_guessLength, colWidth);

                Console.WriteLine(string.Format("|{0}|{1}|",pinsCell,resultCell));
                Console.WriteLine(divider);
            }
            Console.WriteLine();
        }


        private bool isValidGuess(string i_guess)
        {
            if (i_guess == k_QuitCommand)
            {
                return true;
            }

            if (i_guess.Length != m_guessLength)
            {
                return false;
            }

            char firstChar = m_charArray[0];
            char lastChar = m_charArray[m_letterArrayLength - 1];

            bool inRange = true;
            foreach (char c in i_guess)
            {
                if (c < firstChar || c > lastChar)
                {
                    inRange = false;
                    break;
                }
            }
            bool allUnique = i_guess.Distinct().Count() == i_guess.Length;

            return inRange && allUnique;
        }

        private static string buildSpacedCell(string raw, int pegCount, int colWidth)
        {
            StringBuilder cell = new StringBuilder();
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
                Console.WriteLine("You won you guessesed in {0} guesses!", numOfGuesses);
            else
                Console.WriteLine("No more guesses allowed. You Lost!");
        }

        private void promptUserForRetry()
        {

            string? answer = null;
            while (answer != k_YesString && answer != k_NoString)
            {
                Console.WriteLine("Would you like to start a new game? <{0}/{1}>", k_YesString, k_NoString);
                answer = Console.ReadLine();
                if (answer == null)
                {
                    answer = k_NoString;
                }
            }
            if (answer == k_YesString)
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
