using System;
using System.Collections.Generic;

namespace Logic
{
    internal class Engine
    {
        private string m_correctGuess = string.Empty;
        private int m_objectAmount;
        private int m_minGuessAmount;
        private int m_maxGuessAmount;
        private int m_currGuessAmount;
        private int m_answerSignTypeAmount;
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
            set { m_isGameWon = value; }
        }

        public bool IsGameOnGoing
        {
            get { return m_isGameOngoing; }
            set { m_isGameOngoing = value;}
        }

        public int MinGuessAmount
        {
            get => m_minGuessAmount;
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
            get => m_maxGuessAmount;
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

        public void ResetGame(int in_guessAmount)
        {
            IsGameOnGoing = true;
            IsGameWon = false;
            m_triesAmount = 0;
            m_currGuessAmount = in_guessAmount;
            ResetMatrix(in_guessAmount);
        }

        public void ResetMatrix(int in_guessAmount)
        {
            m_historyMatrix = new string[in_guessAmount, 2];
            for (int i = 0; i < in_guessAmount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m_historyMatrix[i, j] = string.Empty;
                }
            }
        }

        public void AddDataToMatrix(string in_guess, string in_result, int in_guessNumber)
        {
            if (m_historyMatrix == null)
            {
                throw new Exception("History matrix not instantiated");
            }

            m_historyMatrix[in_guessNumber, 0] = in_guess;
            m_historyMatrix[in_guessNumber, 1] = in_result;
        }

        public int[] GetRandomObjectIndexes()
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

        public bool IsGuessCorrect(string in_guess, string in_correctGuess)
        {
            bool o_isEqual = in_guess == in_correctGuess;
            return o_isEqual;
        }

        public int[] GetGuessInfoAndUpdate(string in_guess)
        {
            int[] o_resultArray = new int[2];
            bool isGuessCorrect = IsGuessCorrect(in_guess, m_correctGuess);
            AddDataToMatrix(in_guess, m_correctGuess,m_triesAmount);

            m_triesAmount++;
            if (m_triesAmount == m_currGuessAmount && !isGuessCorrect)
                IsGameOnGoing = false;
            
            

            if (isGuessCorrect)
            {
                IsGameWon = true; 
                IsGameOnGoing = false;
                o_resultArray[0] = m_guessLength;
                return o_resultArray;
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

            return o_resultArray;
        }
    }
}