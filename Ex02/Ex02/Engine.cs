using System;
using System.Collections.Generic;

namespace Logic
{
    internal class Engine
    {
        private string _correctGuess = string.Empty;
        private int _objectAmount;
        private int _minGuessAmount;
        private int _maxGuessAmount;
        private int _currGuessAmount;
        private int _answerSignTypeAmount;
        private string[,]? _historyMatrix = null;
        private int _guessLength;

        public int MinGuessAmount
        {
            get => _minGuessAmount;
            set
            {
                if (value < 1)
                {
                    throw new Exception($"Guess amount is lower than min amount (1)");
                }
                _minGuessAmount = value;
            }
        }

        public int MaxGuessAmount
        {
            get => _maxGuessAmount;
            set
            {
                if (value < 1)
                {
                    throw new Exception($"Guess amount is higher than max amount (1)");
                }
                _maxGuessAmount = value;
            }
        }

        public Engine(int guessLength, int maxGuessAmount, int minGuessAmount, int possibleObjectAmount, char[] possibleObjectArray)
        {
            _guessLength = guessLength;
            _objectAmount = possibleObjectAmount;
            MaxGuessAmount = maxGuessAmount;
            MinGuessAmount = minGuessAmount;

            ResetMatrix(maxGuessAmount);
        }

        public void ResetGame(int guessAmount)
        {
            _currGuessAmount = guessAmount;
            ResetMatrix(guessAmount);
        }

        public void ResetMatrix(int guessAmount)
        {
            _historyMatrix = new string[guessAmount, 2];
            for (int i = 0; i < guessAmount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _historyMatrix[i, j] = string.Empty;
                }
            }
        }

        public void AddDataToMatrix(string guess, string result, int guessNumber)
        {
            if (_historyMatrix == null)
            {
                throw new Exception("History matrix not instantiated");
            }

            _historyMatrix[guessNumber, 0] = guess;
            _historyMatrix[guessNumber, 1] = result;
        }

        public int[] GetRandomObjectIndexes()
        {
            if (_guessLength > _objectAmount)
            {
                throw new ArgumentException("Guess length cannot exceed object amount.");
            }

            int[] randomIndexes = new int[_guessLength];
            List<int> availableIndexes = new List<int>();

            for (int i = 0; i < _objectAmount; i++)
            {
                availableIndexes.Add(i);
            }

            Random rand = new Random();

            for (int i = 0; i < _guessLength; i++)
            {
                int randomIndex = rand.Next(availableIndexes.Count);
                randomIndexes[i] = availableIndexes[randomIndex];
                availableIndexes.RemoveAt(randomIndex);
            }

            return randomIndexes;
        }

        public bool IsGuessCorrect(string guess, string correctGuess)
        {
            return guess == correctGuess;
        }

        public int[] GetGuessMatchingInfo(string guess, string correctGuess)
        {
            int[] result = new int[2];

            if (IsGuessCorrect(guess, correctGuess))
            {
                result[0] = _guessLength;
                return result;
            }

            for (int i = 0; i < _guessLength; i++)
            {
                if (guess[i] == correctGuess[i])
                {
                    result[0]++;
                }
                else if (correctGuess.Contains(guess[i]))
                {
                    result[1]++;
                }
            }

            return result;
        }
    }
}