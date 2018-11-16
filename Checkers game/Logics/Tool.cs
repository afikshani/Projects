using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
     public class Tool
    {
        private string m_StringPosition;
        private int m_RowPosition;
        private int m_ColPosition;
        private string m_type;
        private char m_Symbol;
        private int m_value;

        public Tool(int i_RowPosition, int i_ColPosition, char i_Symbol)
        {
            m_RowPosition = i_RowPosition;
            m_ColPosition = i_ColPosition;
            m_StringPosition = ConvertToStringPosition(i_RowPosition, i_ColPosition);
            m_Symbol = i_Symbol;
            m_value = 1;
            m_type = "regular";
        }

        //set the getters and setters for tool
        public int ColPosition
        {
            get { return m_ColPosition; }
            set { m_ColPosition = value; }
        }

        public int RowPosition
        {
            get { return m_RowPosition; }
            set { m_RowPosition = value; }
        }

        public string StringPosition
        {
            get { return m_StringPosition; }
            set { m_StringPosition = value; }
        }

        public string TypeOfTool
        {
            get { return m_type; }
        }

        public char Symbol
        {
            get { return m_Symbol; }
            set
            {
                m_Symbol = value;
                switch (value)
                {
                    case 'X':
                        m_type = "regular";
                        m_value = 1;
                        break;
                    case 'O':
                        m_type = "regular";
                        m_value = 1;
                        break;
                    case 'K':
                        m_type = "king";
                        m_value = 4;
                        break;
                    case 'U':
                        m_type = "king";
                        m_value = 4;
                        break;
                }
            }
        }

        public int Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        // constructor internal method to convert the int position into string format
        public static string ConvertToStringPosition(int i_PositionRowToConvert, int i_PositionColToConvert)
        {
            StringBuilder stringToReturn = new StringBuilder("");
            char rowAsString = Convert.ToChar(i_PositionRowToConvert + 'a');
            char columnAsString = Convert.ToChar(i_PositionColToConvert + 'A');

            stringToReturn.Append(columnAsString);
            stringToReturn.Append(rowAsString);

            return stringToReturn.ToString();
        }

        //if the player can move regular move
        internal bool IsAbleToMove()
        {
            bool isAbleToMove = false;
            int rowOfTheOptionsToMove = RowPosition + Game.CurrentPlayer.MovingDirection;
            int rowAvailbleForTheKing = RowPosition + (-1 * Game.CurrentPlayer.MovingDirection);
            int colOfTheFirstOption = ColPosition + 1;
            int colOfTheSecondOption = ColPosition - 1;
            string firstOptionToMove = StringPosition + '>' + ConvertToStringPosition(rowOfTheOptionsToMove, colOfTheFirstOption);
            string secondOptionToMove = StringPosition + '>' + ConvertToStringPosition(rowOfTheOptionsToMove, colOfTheSecondOption);
            string kingFirstOptionToMove = StringPosition + '>' + ConvertToStringPosition(rowAvailbleForTheKing, colOfTheFirstOption);
            string kingSecondtOptionToMove = StringPosition + '>' + ConvertToStringPosition(rowAvailbleForTheKing, colOfTheSecondOption);

            if (Game.CheckAvailableMove(firstOptionToMove) || Game.CheckAvailableMove(secondOptionToMove))
            {
                isAbleToMove = true;
            }

            if (TypeOfTool == "king")
            {
                if (Game.CheckAvailableMove(kingFirstOptionToMove) || Game.CheckAvailableMove(kingSecondtOptionToMove))
                {
                    isAbleToMove = true;
                }
            }

            return isAbleToMove;
        }

        internal bool ToolIsAbleToCapture()
        {
            bool isAbleToMove = false;
            int rowOfTheOptionsToMove = RowPosition + Game.CurrentPlayer.MovingDirection * 2;
            int rowAvailbleForTheKing = RowPosition + Game.CurrentPlayer.MovingDirection * -2;
            int colOfTheFirstOption = ColPosition + 2;
            int colOfTheSecondOption = ColPosition - 2;

            if (Game.IsLeaglCaptureMove(colOfTheFirstOption, rowOfTheOptionsToMove, ColPosition, RowPosition)
                || Game.IsLeaglCaptureMove(colOfTheSecondOption, rowOfTheOptionsToMove, ColPosition, RowPosition))
            {
                isAbleToMove = true;
            }

            if (TypeOfTool == "king")
            {
                if (Game.IsLeaglCaptureMove(colOfTheFirstOption, rowAvailbleForTheKing, ColPosition, RowPosition)
                   || Game.IsLeaglCaptureMove(colOfTheSecondOption, rowAvailbleForTheKing, ColPosition, RowPosition))
                {
                    isAbleToMove = true;
                }
            }

            return isAbleToMove;
        }

        internal void SetPositions(int i_NewRowPosition, int i_NewColPosition, string i_NewStringPosition)
        {
            RowPosition = i_NewRowPosition;
            ColPosition = i_NewColPosition;
            StringPosition = i_NewStringPosition;

            //checks if the player reached the end of the board and replace the tool to a king
            if (Game.CurrentPlayer.MovingDirection == 1)
            {
                if (RowPosition == Game.Board.BoardSize - 1)
                {
                    Symbol = Game.CurrentPlayer.KingSymbol;
                }
            }
            else
            {
                if (RowPosition == 0)
                {
                    Symbol = Game.CurrentPlayer.KingSymbol;
                }
            }
        }
    }
}