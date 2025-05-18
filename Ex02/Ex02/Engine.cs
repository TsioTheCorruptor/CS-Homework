using System;
using System.Collections.Generic;

namespace Logic
{
    internal class Engine
    {
        private string m_correctGuess = string.Empty;
        private static readonly char  m_correctAnswerMask='#';
        private int m_objectAmount;
        private int m_minGuessAmount;
        private int m_maxGuessAmount;
        private int m_currGuessAmount;
        
        private string[,]? m_historyMatrix = null;
        private int m_guessLength;
        private int m_triesAmount;
        private bool m_isGameOngoing;
        private bool m_isGameWon;

        public string[,] HistoryMatrix
        {
            get { if (m_historyMatrix != null)
                    return m_historyMatrix;
                else throw new Exception("non initialized matrix");
                }
        }
        public int TriesAmount
        {
            get { return m_triesAmount; }
        }
        public bool IsGameWon
        {
            get { return m_isGameWon; }
            
        }

        public bool IsGameOnGoing
        {
            get { return m_isGameOngoing; }
            
        }

        public int MinGuessAmount
        {
            get { return m_minGuessAmount; }
            set
            {
                if (value < m_minGuessAmount)
                {
                    throw new Exception(string.Format("guess amount is lower then min amount({0})", m_minGuessAmount));
                }
                m_minGuessAmount = value;
            }
        }

        public int MaxGuessAmount
        {
            get { return m_maxGuessAmount; }
            set
            {
                if (value < m_maxGuessAmount)
                {
                    throw new Exception(string.Format("guess amount is higher then max amount({0})", m_maxGuessAmount));
                }
                m_maxGuessAmount = value;
            }
        }

        public Engine(int in_guessLength, int in_maxGuessAmount, int in_minGuessAmount, int in_possibleObjectAmount, char[] in_possibleObjectArray)
        {
            m_guessLength = in_guessLength;
            m_objectAmount = in_possibleObjectAmount;
            MaxGuessAmount = in_maxGuessAmount;
            MinGuessAmount = in_minGuessAmount;

            ResetMatrix(in_maxGuessAmount);
        }

        public void ResetGame(int in_guessAmount, char[] in_objectArray )
        {
            m_isGameOngoing = true;
            m_isGameWon = false;
            m_triesAmount = 0;
            m_currGuessAmount = in_guessAmount;
            m_correctGuess=string.Empty;
            ResetMatrix(in_guessAmount+1);
            CreateCorrectGuess(GetRandomObjectIndexes(),in_objectArray);

        }

        private void ResetMatrix(int in_guessAmount)
        {
            m_historyMatrix = new string[in_guessAmount, 2];
            for (int i = 0; i < in_guessAmount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m_historyMatrix[i, j] = string.Empty;
                }
            }
             
            string maskString = new string(m_correctAnswerMask, m_guessLength);
            m_historyMatrix[0,0] = maskString;
            

        }

        private void AddDataToMatrix(string in_guess, string in_result, int in_guessNumber)
        {
            if (m_historyMatrix == null)
            {
                throw new Exception("History matrix not instantiated");
            }

            m_historyMatrix[in_guessNumber, 0] = in_guess;
            m_historyMatrix[in_guessNumber, 1] = in_result;
        }

        private int[] GetRandomObjectIndexes()
        {
            if (m_guessLength > m_objectAmount)
            {
                throw new ArgumentException("Guess length cannot exceed object amount.");
            }

            int[] o_randomIndexes = new int[m_guessLength];
            List<int> m_duplicatesHelperList = new List<int>();

            for (int i = 0; i < m_objectAmount; i++)
            {
                m_duplicatesHelperList.Add(i);
            }

            Random m_rand = new Random();

            for (int i = 0; i < m_guessLength; i++)
            {
                int m_randomIndex = m_rand.Next(m_duplicatesHelperList.Count);
                o_randomIndexes[i] = m_duplicatesHelperList[m_randomIndex];
                m_duplicatesHelperList.RemoveAt(m_randomIndex);
            }

            return o_randomIndexes;
        }
        private void CreateCorrectGuess(int[] in_indexes, char[] in_guessCharacters)
        {
            int currentIndex;
            for(int i = 0;i<m_guessLength;i++)
            {
                currentIndex = in_indexes[i];
                m_correctGuess = m_correctGuess + in_guessCharacters[currentIndex];
            }
            
        }

        private bool IsGuessCorrect(string in_guess, string in_correctGuess)
        {
            bool o_isEqual = in_guess == in_correctGuess;
            return o_isEqual;
        }

        public void GetGuessInfoAndUpdate(string in_guess)
        {
           
            int[] o_resultArray = new int[2];
            bool isGuessCorrect = IsGuessCorrect(in_guess, m_correctGuess);
           

            
            if (m_triesAmount == m_currGuessAmount-1 && !isGuessCorrect)
            {
                m_isGameOngoing = false;
                AddDataToMatrix(m_correctGuess, "", 0);

            }
                
            
            

            if (isGuessCorrect)
            {
                m_isGameWon = true; 
                m_isGameOngoing = false;
                o_resultArray[0] = m_guessLength;
                AddDataToMatrix(m_correctGuess, "", 0);
                
                
            }
            

            for (int i = 0; i < m_guessLength; i++)
            {
                if (in_guess[i] == m_correctGuess[i])
                {
                    o_resultArray[0]++;
                }
                else if (m_correctGuess.Contains(in_guess[i]))
                {
                    o_resultArray[1]++;
                }
            }
            AddDataToMatrix(in_guess, CreateGuessResultString(o_resultArray), m_triesAmount+1);
            m_triesAmount ++;
            
        }
        private string CreateGuessResultString(int[] in_resultArr)
    {
        string stringToReturn="";
            for (int i = 0; i < in_resultArr[0];i++)
                stringToReturn = stringToReturn + "V";
            for (int y =0 ; y< in_resultArr[1]; y++)
                stringToReturn = stringToReturn + "X";


        return stringToReturn;

    }
    }
    
}