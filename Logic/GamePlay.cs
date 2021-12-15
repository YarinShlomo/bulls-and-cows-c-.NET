using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class GamePlay
    {
        private readonly Random m_randomLetterPicker = new Random();
        private List<char> m_chosenLetters = new List<char>(4);

        public List<char> chosenLetters
        {
            get
            {
                return m_chosenLetters;
            }
        }

        public GamePlay()
        {
            setGame();
        }

        private char getRandomCharacter(List<char> i_availableOptions, Random i_randomGenerator)
        {
            int index = i_randomGenerator.Next(i_availableOptions.Count);
            return i_availableOptions[index];
        }

        private void setGame()
        {
            List<char> availableOption = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            for (int i = 0; i < 4; i++)
            {
                m_chosenLetters.Add(getRandomCharacter(availableOption, m_randomLetterPicker));
                availableOption.Remove(m_chosenLetters[i]);
            }
        }

        public int LettersNotInRightPlace(string i_userGuesses)
        {
            int rightAttempts = 0;
            int lettersFoundInRightPlace = LettersInRightPlace(i_userGuesses);

            foreach(char letter in i_userGuesses)
            {
                if(m_chosenLetters.Contains(letter))
                {
                    rightAttempts++;
                }
            }

            return rightAttempts - lettersFoundInRightPlace;
        }

        public int LettersInRightPlace(string i_userGuesses)
        {
            int rightGuesses = 0;

            for(int i = 0; i < 4; i++)
            {
                if(i_userGuesses[i] == m_chosenLetters[i])
                {
                    rightGuesses++;
                }
            }

            return rightGuesses;
        }

        public bool HasWon(string i_userGuesses)
        {
            bool win = false;

            if(LettersInRightPlace(i_userGuesses) == 4)
            {
                win = true;    
            }

            return win;
            // return LettersInRightPlace(i_userGuesses) == 4;
        }

        public bool BoardFull(string[,] i_GameBoard) // NOT IN USE
        {
            bool isFull = false;

            for (int i = 0; i < i_GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < i_GameBoard.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(i_GameBoard[i, j]))
                    {
                        isFull = false;
                        break;
                    }

                    isFull = true;
                }

                if (!isFull)
                {
                    break;
                }
            }

            return isFull;
        }
    }
}
