using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Board
    {
        public Board()
        {
            GameBoard = new List<Guess>(10);
        }

        public List<Guess> GameBoard { get; }

        public void UpdateRow(string io_UserInput, int io_Successes, int io_Tries)
        {
            GameBoard.Add(new Guess(io_UserInput, io_Tries, io_Successes));
        }

        public void ClearBoard() // Currently not in use.
        {
            GameBoard.Clear();
        }
    }
}
