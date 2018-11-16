using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Board
    {
        private Tool[,] m_Board;
        private int m_SizeOfBoard;

        internal Board(int i_Size)
        {
            m_SizeOfBoard = i_Size;
            m_Board = new Tool[i_Size, i_Size];
            initSquares(m_Board, m_SizeOfBoard);
        }

        public Tool[,] BoardAcceser
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public int BoardSize
        {
            get { return m_SizeOfBoard; }
            set { m_SizeOfBoard = value; }
        }
        
        //insert the players tools to the board
        internal void initSquares(Tool[,] io_BoardToInit, int io_SizeOfBoard)
        {
            Game.FirstPlayer.Tools.Clear();
            Game.SecondPlayer.Tools.Clear();
            Tool toolOfTheFirstPlayer;
            Tool toolOfTheSecondPlayer;
            for (int i = 0; i < (io_SizeOfBoard / 2) - 1; i++)
            {
                for (int j = 0; j < io_SizeOfBoard; j++)
                {
                    io_BoardToInit[io_SizeOfBoard / 2, j] = null;
                    io_BoardToInit[(io_SizeOfBoard / 2) - 1, j] = null;

                    if ((i + j) % 2 == 1)
                    {
                        //adding new tool to the first player dictionary and adding it to the board 

                        toolOfTheFirstPlayer = new Tool(io_SizeOfBoard - 1 - i, io_SizeOfBoard - 1 - j, 'X');
                        Game.FirstPlayer.Tools.Add(toolOfTheFirstPlayer.StringPosition, toolOfTheFirstPlayer);
                        io_BoardToInit[io_SizeOfBoard - 1 - i, io_SizeOfBoard - 1 - j] = toolOfTheFirstPlayer;
                        //adding new tool to the second player dictionary and adding it to the board

                        toolOfTheSecondPlayer = new Tool(i, j, 'O');
                        Game.SecondPlayer.Tools.Add(toolOfTheSecondPlayer.StringPosition, toolOfTheSecondPlayer);
                        io_BoardToInit[i, j] = toolOfTheSecondPlayer;
                    }

                    else
                    {
                        io_BoardToInit[i, j] = null;
                    }
                }
            }
        }
    }
}
