using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Logic
{
    
        internal class Engine
        {
        private int m_objectAmount;
        private int m_minGuessAmount;
        private int m_maxGuessAmount;
        private int m_currGuessAmount;
        private int m_answerSignTypeAmount;
        private  int MinGuessAmount 
            {
            get { 
                return m_minGuessAmount; 
                }
            set 
            {
                if (value < m_minGuessAmount)
                {
                    throw new Exception(string.Format("guess amount is lower then min amount({0})", m_minGuessAmount));
                }
                else { m_minGuessAmount = value; 
                }
            } 
        } 
            private  int MaxGuessAmount
        {
            get { 
                return m_maxGuessAmount; 
                }
            set
            {
                if (value < m_maxGuessAmount)
                {
                    throw new Exception(string.Format("guess amount is higher then max amount({0})", m_maxGuessAmount));
                }
                else
                {
                    m_maxGuessAmount = value;
                }
            }
        }
        private  int m_guessLength;
           
            private string[,] m_historyMatrix;

            public Engine(int in_guessLength, int in_maxGuessAmount,int in_minGuessAmount,int in_possibleObjectAmount)
            {
                m_objectAmount = in_possibleObjectAmount;
                m_maxGuessAmount=in_maxGuessAmount;
                m_minGuessAmount= in_minGuessAmount;
                MinGuessAmount = in_minGuessAmount;
                m_guessLength = in_guessLength;
                MaxGuessAmount = in_maxGuessAmount;
               
                m_historyMatrix = new string[m_maxGuessAmount, 2];
            }
        public void ResetGame(int in_guessAmount)
        {
            m_currGuessAmount = in_guessAmount;
            
        }
        public int[] GetRandomObjectIndexes()
        {
            int randomValue;
            int[] randomList=new int[m_guessLength];
            Random rand=new Random();
            for(int i = 0;i< m_guessLength;i++)
            {
             randomValue = rand.Next(m_objectAmount);
                randomList[i] = randomValue;
            }
            foreach(int i in randomList)
                Console.WriteLine(i);
            return randomList;
            
        }
        public bool IsGuessCorrect(string in_guess,string in_correctGuess)
        {
            bool isequal;
            if(in_guess==in_correctGuess)
               isequal= true;
            else isequal= false;
            return isequal;
        }
        public int[] GetGuessMatchingInfo(string in_guess, string in_correctGuess)
        {
            int[] o_returnArray= new int[2];

            if (IsGuessCorrect(in_guess, in_correctGuess))
                o_returnArray[0] = m_guessLength;
            else
            {
                for(int i = 0; i < m_guessLength;i++)
                {
                    if (in_guess[i] == in_correctGuess[i])
                        o_returnArray[0]++;
                    else
                    {
                        if (in_correctGuess.Contains(in_guess[i]))
                            o_returnArray[1]++;
                    }
                }
            }
            return o_returnArray;    
        }
        }
    }
