using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Board
    {
        private string[] m_GameBoard;
        private int m_currentRound = 0;

        public Board(int i_Size)
        {
            m_GameBoard = new string[i_Size];
        }

        public string[] GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        ////public int numberOfRounds
        ////{
        ////    get 
        ////    {
        ////        return m_numberOfRounds;
        ////    }
        ////    set
        ////    {
        ////        m_numberOfRounds = value;
        ////    }
        ////}

        public void UpdateRow(string i_userInput, int i_Succeses, int i_Tries)
        {
            StringBuilder currentRow = new StringBuilder();
            currentRow.Append(i_userInput);
            currentRow.AppendFormat("{0}{1}", i_Succeses, i_Tries);
            m_GameBoard[m_currentRound++] = currentRow.ToString(); 
        }

        public void ClearBoard() // Currently not in use.
        {
            for (int i = 0; i < m_GameBoard.GetLength(0); i++)
            {
                m_GameBoard[i] = null;
            }

            m_currentRound = 0;
        }
    }
}
