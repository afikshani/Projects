using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Player
    {
        private string m_Name;
        private Dictionary<string, Tool> m_Tools;
        private int m_MovingDirection;
        private int m_NumOfTools;
        private int m_Score;
        private bool m_JustCaptured;
        private char m_PlayerSymbol;
        private char m_KingSymbol;
        private int m_TournamentScore;

        internal Player(string i_Name, int i_BoardSize, char i_PlayerSymbol)
        {
            m_Tools = new Dictionary<string, Tool>();
            m_Name = i_Name;
            m_Score = 0;
            m_TournamentScore = 0;
            m_JustCaptured = false;
            m_NumOfTools = initNumOfTools(i_BoardSize);
            m_PlayerSymbol = i_PlayerSymbol;

            if (i_PlayerSymbol == 'O')
            {
                m_MovingDirection = 1;
                m_KingSymbol = 'U';
            }

            else
            {
                m_MovingDirection = -1;
                m_KingSymbol = 'K';
            }
        }

        internal static int initNumOfTools(int i_BoardSize)
        {
            int numOfTools = 0;
            switch (i_BoardSize)
            {
                case 6:
                    numOfTools = 6;
                    break;
                case 8:
                    numOfTools = 12;
                    break;
                case 10:
                    numOfTools = 20;
                    break;
            }

            return numOfTools;
        }

        public Dictionary<string, Tool> Tools
        {
            get { return m_Tools; }
            set { m_Tools = value; }
        }

        public char KingSymbol
        {
            get { return m_KingSymbol; }
            set { m_KingSymbol = value; }
        }

        public char PlayerSymbol
        {
            get { return m_PlayerSymbol; }
            set { m_PlayerSymbol = value; }
        }

        public bool JustCaptured
        {
            get { return m_JustCaptured; }
            set { m_JustCaptured = value; }
        }

        public int MovingDirection
        {
            get { return m_MovingDirection; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int Score
        {
            get
            {
                int score = 0;
                foreach (var item in this.Tools)
                {
                    score += item.Value.Value;
                }

                return score;
            }

            set { m_Score = value; }
        }

        public int TournamentScore
        {
            get { return m_TournamentScore; }
            set { m_TournamentScore = value; }
        }

        public int ToolsCounter
        {
            get { return m_NumOfTools; }
            set { m_NumOfTools = value; }
        }

        internal bool IsPlayerAbleToMove()
        {
            bool ableToMove = false;

            foreach (var item in Tools)
            {
                if (item.Value.IsAbleToMove() || item.Value.ToolIsAbleToCapture())
                {
                    ableToMove = true;
                }
            }
            Console.WriteLine(ableToMove);
            return ableToMove;
        }

        internal bool IsPlayerAbleToCapture()
        {
            bool ableToMove = false;

            foreach (var item in Tools)
            {
                if (item.Value.ToolIsAbleToCapture())
                {
                    ableToMove = true;
                }
            }

            return ableToMove;
        }
        //genarates a list of optional moves for the computer
        internal static List<string> GetListOfLegalMoves()
        {
            List<string> listOfLegalMoves = new List<string>();
            int rowOfTheOptionsToMoveForCapture;
            int rowAvailbleForTheKingForCapture;
            int colOfTheFirstOptionForCapture;
            int colOfTheSecondOptionForCapture;

            int rowOfTheOptionsToMove;
            int rowAvailbleForTheKing;
            int colOfTheFirstOption;
            int colOfTheSecondOption;

            //if player can capture it can only move to capture
            foreach (var item in Game.SecondPlayer.Tools)
            {
                if (Game.SecondPlayer.IsPlayerAbleToCapture())
                {
                    rowOfTheOptionsToMoveForCapture = item.Value.RowPosition + Game.CurrentPlayer.MovingDirection * 2;
                    rowAvailbleForTheKingForCapture = item.Value.RowPosition + Game.CurrentPlayer.MovingDirection * -2;
                    colOfTheFirstOptionForCapture = item.Value.ColPosition + 2;
                    colOfTheSecondOptionForCapture = item.Value.ColPosition - 2;
                    string firstOptionToMoveForCapture = item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMoveForCapture, colOfTheFirstOptionForCapture);
                    string secondOptionToMoveForCapture = item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMoveForCapture, colOfTheSecondOptionForCapture);
                    string kingFirstOptionToMoveForCapture = item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKingForCapture, colOfTheFirstOptionForCapture);
                    string kingSecondOptionToMoveForCapture = item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKingForCapture, colOfTheSecondOptionForCapture);
                    //adding the moves to the list
                    if (Game.CheckAvailableMove(firstOptionToMoveForCapture))
                    {
                        listOfLegalMoves.Add(item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMoveForCapture, colOfTheFirstOptionForCapture));
                    }

                    if (Game.CheckAvailableMove(secondOptionToMoveForCapture))
                    {
                        listOfLegalMoves.Add(item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMoveForCapture, colOfTheSecondOptionForCapture));
                    }

                    if (item.Value.TypeOfTool == "king")
                    {
                        if (Game.CheckAvailableMove(kingFirstOptionToMoveForCapture))
                        {
                            listOfLegalMoves.Add(item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKingForCapture, colOfTheFirstOptionForCapture));
                        }

                        if (Game.CheckAvailableMove(kingSecondOptionToMoveForCapture))
                        {
                            listOfLegalMoves.Add(item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKingForCapture, colOfTheSecondOptionForCapture));
                        }
                    }
                }

                else
                {
                    rowOfTheOptionsToMove = item.Value.RowPosition + Game.CurrentPlayer.MovingDirection; 
                    rowAvailbleForTheKing = item.Value.RowPosition + (-1 * Game.CurrentPlayer.MovingDirection);
                    colOfTheFirstOption = item.Value.ColPosition + 1;
                    colOfTheSecondOption = item.Value.ColPosition - 1;
                    string firstOptionToMove = item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMove, colOfTheFirstOption);
                    string secondOptionToMove = item.Key + '>' + Tool.ConvertToStringPosition(rowOfTheOptionsToMove, colOfTheSecondOption);
                    string kingFirstOptionToMove = item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKing, colOfTheFirstOption);
                    string kingSecondOptionToMove = item.Key + '>' + Tool.ConvertToStringPosition(rowAvailbleForTheKing, colOfTheSecondOption);

                    if (Game.CheckAvailableMove(firstOptionToMove))
                    {
                        listOfLegalMoves.Add(firstOptionToMove);
                    }

                    if (Game.CheckAvailableMove(secondOptionToMove))
                    {
                        listOfLegalMoves.Add(secondOptionToMove);
                    }

                    if (item.Value.TypeOfTool == "king")
                    {
                        if (Game.CheckAvailableMove(kingFirstOptionToMove))
                        {
                            listOfLegalMoves.Add(kingFirstOptionToMove);
                        }

                        if (Game.CheckAvailableMove(kingSecondOptionToMove))
                        {
                            listOfLegalMoves.Add(kingSecondOptionToMove);
                        }
                    }
                }
            }

            return listOfLegalMoves;
        }
    }
}
