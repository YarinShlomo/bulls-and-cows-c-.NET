using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Board
    {

        private List<Guess> m_GameBoard;

        public Board()
        {
            m_GameBoard = new List<Guess>();
        }

        public List<Guess> GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public void UpdateRow(string io_UserInput, int io_Successes, int io_Tries)
        {
            m_GameBoard.Add(new Guess(io_UserInput, io_Tries, io_Successes));
        }

        public void ClearBoardAndResize(int i_NewSize)
        {
            m_GameBoard.Clear();
            m_GameBoard.Capacity = i_NewSize;
        }
    }
}
