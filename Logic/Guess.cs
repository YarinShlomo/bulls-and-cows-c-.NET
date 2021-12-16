using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public struct Guess
    {
        public Guess(string i_Guess, int i_NumOfIncorrectPositionLetters, int i_NumOfRightPositionLetters)
        {
            GuessPins = i_Guess;
            NumOfIncorrectPositionLetters = i_NumOfIncorrectPositionLetters;
            NumOfRightPositionLetters = i_NumOfRightPositionLetters;
        }

        public string GuessPins { get; }

        public int NumOfIncorrectPositionLetters { get; }

        public int NumOfRightPositionLetters { get; }
    } 
}
