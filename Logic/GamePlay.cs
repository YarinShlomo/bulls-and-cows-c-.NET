using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class GamePlay
    {
        private readonly Random r_RandomLetterPicker = new Random();
        private List<char> m_ChosenLetters = new List<char>(4);
        private Board m_GameBoard;

        public List<char> ChosenLetters
        {
            get
            {
                return m_ChosenLetters;
            }
        }

        public List<Guess> GameBoard
        {
            get
            {
                return m_GameBoard.GameBoard;
            }
        }

        public GamePlay()
        {
            setGame();
        }

        public bool CheckWinningAndUpdateBoard(string io_UserGuesses)
        {
            int lettersInPlace = lettersInRightPlace(io_UserGuesses);
            int lettersExistNotInPlace = lettersNotInRightPlace(io_UserGuesses);
            m_GameBoard.UpdateRow(io_UserGuesses, lettersInPlace, lettersExistNotInPlace);
            return hasWon(io_UserGuesses);
        }

        public void SetNewGame()
        {
            m_GameBoard = new Board();
            setGame();
        }

        private char getRandomCharacter(List<char> i_AvailableOptions, Random i_RandomGenerator)
        {
            int index = i_RandomGenerator.Next(i_AvailableOptions.Count);
            return i_AvailableOptions[index];
        }

        private void setGame()
        {
            List<char> availableOption = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            m_ChosenLetters.Clear();
            for (int i = 0; i < 4; i++)
            {
                m_ChosenLetters.Add(getRandomCharacter(availableOption, r_RandomLetterPicker));
                availableOption.Remove(m_ChosenLetters[i]);
            }
        }

        private int lettersNotInRightPlace(string io_UserGuesses)
        {
            int rightAttempts = 0;
            int lettersFoundInRightPlace = lettersInRightPlace(io_UserGuesses);

            foreach(char letter in io_UserGuesses)
            {
                if(m_ChosenLetters.Contains(letter))
                {
                    rightAttempts++;
                }
            }

            return rightAttempts - lettersFoundInRightPlace;
        }

        private int lettersInRightPlace(string i_UserGuesses)
        {
            int rightGuesses = 0;

            for(int i = 0; i < 4; i++)
            {
                if(i_UserGuesses[i] == m_ChosenLetters[i])
                {
                    rightGuesses++;
                }
            }

            return rightGuesses;
        }

        private bool hasWon(string io_UserGuesses)
        {
            return lettersInRightPlace(io_UserGuesses) == 4;
        }
    }
}
