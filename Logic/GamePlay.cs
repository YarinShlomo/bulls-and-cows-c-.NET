using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class GamePlay
    {
        private readonly Random r_RandomLetterPicker;
        private readonly List<char> r_ChosenLetters;
        private readonly Board r_GameBoard;

        public List<char> ChosenLetters
        {
            get
            {
                return r_ChosenLetters;
            }
        }

        public List<Guess> GameBoard
        {
            get
            {
                return r_GameBoard.GameBoard;
            }
        }

        public GamePlay()
        {
            r_RandomLetterPicker = new Random();
            r_ChosenLetters = new List<char>(4);
            r_GameBoard = new Board();
        }

        public bool CheckWinningAndUpdateBoard(string io_UserGuesses)
        {
            int lettersInPlace = lettersInRightPlace(io_UserGuesses);
            int lettersExistNotInPlace = lettersNotInRightPlace(io_UserGuesses);
            r_GameBoard.UpdateRow(io_UserGuesses, lettersInPlace, lettersExistNotInPlace);
            return hasWon(io_UserGuesses);
        }

        public void SetNewGame(int io_NewSize)
        {
            r_GameBoard.ClearBoardAndResize(io_NewSize);
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
            r_ChosenLetters.Clear();
            for (int i = 0; i < 4; i++)
            {
                r_ChosenLetters.Add(getRandomCharacter(availableOption, r_RandomLetterPicker));
                availableOption.Remove(r_ChosenLetters[i]);
            }
        }

        private int lettersNotInRightPlace(string io_UserGuesses)
        {
            int rightAttempts = 0;
            int lettersFoundInRightPlace = lettersInRightPlace(io_UserGuesses);

            foreach(char letter in io_UserGuesses)
            {
                if(r_ChosenLetters.Contains(letter))
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
                if(i_UserGuesses[i] == r_ChosenLetters[i])
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
