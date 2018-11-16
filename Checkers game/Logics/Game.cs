using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Game
    {
        public enum GameStatus { WIN, TIE, NONE }

        private static Player m_FirstPlayer;
        private static Player m_SecondPlayer;
        private static Player m_CurrentPlayer;
        private static int m_NumberOfPlayers;
        private static Board m_Board;
        private bool m_GameEnded = false;
        private static string m_LastMove;
        public GameStatus m_StatusOfTheGame;

        public static event EventHandler DoWhenNeedToEat;
        public static event EventHandler DoWhenUnvalidMove;

        public Game(string i_FirstPlayerName, string i_SecondPlayerName, int i_SizeOfBoard, int i_NumberOfPlayers)
        {
            m_StatusOfTheGame = GameStatus.NONE;
            m_FirstPlayer = new Player(i_FirstPlayerName, i_SizeOfBoard, 'X');
            m_SecondPlayer = new Player(i_SecondPlayerName, i_SizeOfBoard, 'O');
            m_CurrentPlayer = m_FirstPlayer;
            m_NumberOfPlayers = i_NumberOfPlayers;
            m_Board = new Board(i_SizeOfBoard);
        }

        public void RestartNewGame()
        {
            this.StatusOfTheGame = Game.GameStatus.NONE;
            GameEnded = false;
            FirstPlayer.JustCaptured = false;
            SecondPlayer.JustCaptured = false;
            Game.Board.initSquares(Game.Board.BoardAcceser, Game.Board.BoardSize);
            FirstPlayer.ToolsCounter = Player.initNumOfTools(m_Board.BoardSize);
            SecondPlayer.ToolsCounter = Player.initNumOfTools(m_Board.BoardSize);
            m_CurrentPlayer = FirstPlayer;
        }

        public static Player FirstPlayer
        {
            get { return m_FirstPlayer; }
        }

        public static Player SecondPlayer
        {
            get { return m_SecondPlayer; }
        }

        public static Player CurrentPlayer
        {
            get { return m_CurrentPlayer; }
        }

        public bool GameEnded
        {
            get { return m_GameEnded; }
            set { m_GameEnded = value; }
        }

        public static Board Board
        {
            get { return m_Board; }
        }

        public GameStatus StatusOfTheGame
        {
            get { return m_StatusOfTheGame; }
            set { m_StatusOfTheGame = value; }
        }

        public static bool CheckAvailableMove(string i_Move)
        {
            bool validMove = false;
            char fromCol = i_Move[0];
            char fromRow = i_Move[1];
            char toCol = i_Move[3];
            char toRow = i_Move[4];
            int indexFromCol = fromCol - 'A';
            int indexFromRow = fromRow - 'a';
            int indexToCol = toCol - 'A';
            int indexToRow = toRow - 'a';

            if (CurrentPlayer.JustCaptured == true)
            {
                validMove = false;
                bool captureWithSameTool = (i_Move.Substring(0, 2) == m_LastMove);
                bool ableToCapture = IsLeaglCaptureMove(indexToCol, indexToRow, indexFromCol, indexFromRow);

                if (captureWithSameTool && ableToCapture)
                {
                    validMove = true;
                }

                if (!validMove && CurrentPlayer.Name != "Computer")
                {
                    OnDoWhenUnvalidMove();
                }
            }

            else if (CurrentPlayer.IsPlayerAbleToCapture())
            {
                validMove = IsLeaglCaptureMove(indexToCol, indexToRow, indexFromCol, indexFromRow);

                if (!validMove && CurrentPlayer.Name != "Computer")
                {
                    OnDoWhenNeedToEat();
                }
            }

            else
            {
                if (IsLeaglRegularMove(indexToCol, indexToRow, indexFromCol, indexFromRow)
                || IsLeaglCaptureMove(indexToCol, indexToRow, indexFromCol, indexFromRow))
                {
                    validMove = true;
                }

                if (!validMove && CurrentPlayer.Name != "Computer")
                {
                    OnDoWhenUnvalidMove();
                }
            }

            return validMove;
        }
       
        //checks if the destination square is free
        private static bool checkFreeSquares(int i_ToCol, int i_ToRow)
        {
            bool validMove = true;

            if (m_Board.BoardAcceser[i_ToRow, i_ToCol] != null)
            {
                validMove = false;
            }

            return validMove;
        }

        //checks if the dest square is not out of bounds
        internal static bool NotOutOfBoundsChecker(int i_ToCol, int i_ToRow)
        {
            bool validMove = true;

            if ((i_ToCol >= m_Board.BoardSize) || (i_ToRow >= m_Board.BoardSize) || (i_ToRow < 0) || (i_ToCol < 0))
            {
                validMove = false;
            }

            return validMove;
        }

        //check whether the requested move is done from the correct player position
        public static bool CheckIfThisMySquares(int i_FromCol, int i_FromRow)
        {
            bool validMove = false;

            if (m_CurrentPlayer.Tools.ContainsKey(Tool.ConvertToStringPosition(i_FromRow, i_FromCol)))
            {
                validMove = true;
            }

            return validMove;
        }

        //checks if the destination square is in a legal place and free
        private static bool isLegalDest(int i_ToCol, int i_ToRow, int i_FromCol, int i_FromRow, bool checkForCapture)
        {
            bool validMove = false;
            Player prevPlayer = (m_CurrentPlayer.Name == m_FirstPlayer.Name) ? m_SecondPlayer : m_FirstPlayer;
            bool isOpponentToolBetween = prevPlayer.Tools.ContainsKey(Tool.ConvertToStringPosition(((i_ToRow + i_FromRow) / 2), (i_ToCol + i_FromCol) / 2));

            //the tool is a regular tool
            if (Board.BoardAcceser[i_FromRow, i_FromCol].TypeOfTool == "regular")
            {
                bool playerIsCapturingInTheRightDirection = (i_ToRow - i_FromRow == m_CurrentPlayer.MovingDirection * 2) && (Math.Abs(i_FromCol - i_ToCol) == 2);
                bool playerIsMovingInTheRightDirection = (i_ToRow - i_FromRow == m_CurrentPlayer.MovingDirection) && (Math.Abs(i_FromCol - i_ToCol) == 1);

                //if the next move is legal
                if ((playerIsMovingInTheRightDirection && !checkForCapture || playerIsCapturingInTheRightDirection && isOpponentToolBetween)
                    && checkFreeSquares(i_ToCol, i_ToRow))
                {
                    validMove = true;
                }
            }
            //the tool is a king
            else
            {
                bool playerIsCapturingInTheRightDirection = (Math.Abs(i_ToRow - i_FromRow) == 2) && (Math.Abs(i_FromCol - i_ToCol) == 2);
                bool playerIsMovingInTheRightDirection = (Math.Abs(i_ToRow - i_FromRow) == 1) && (Math.Abs(i_FromCol - i_ToCol) == 1);

                //if the next move is legal
                if (checkFreeSquares(i_ToCol, i_ToRow) && ((playerIsMovingInTheRightDirection && !checkForCapture)
                    || (playerIsCapturingInTheRightDirection && isOpponentToolBetween)))
                {
                    validMove = true;
                }
            }

            return validMove;
        }


        //checks if a capture move is legal
        internal static bool IsLeaglCaptureMove(int i_ToCol, int i_ToRow, int i_FromCol, int i_FromRow)
        {
            bool validMove = false;

            if (NotOutOfBoundsChecker(i_ToCol, i_ToRow)
                && CheckIfThisMySquares(i_FromCol, i_FromRow)
                && isLegalDest(i_ToCol, i_ToRow, i_FromCol, i_FromRow, true))
            {
                validMove = true;
            }

            return validMove;
        }

        internal static bool IsLeaglRegularMove(int i_ToCol, int i_ToRow, int i_FromCol, int i_FromRow)
        {
            bool validMove = false;

            if (NotOutOfBoundsChecker(i_ToCol, i_ToRow)
                && CheckIfThisMySquares(i_FromCol, i_FromRow)
                && isLegalDest(i_ToCol, i_ToRow, i_FromCol, i_FromRow, false))
            {
                validMove = true;
            }

            return validMove;
        }

        //checks if there is an option to capture another tool after one has been capture.
        private static bool checkIfNextCaptureIsLegal(int i_ToCol, int i_ToRow, int i_FromCol, int i_FromRow)
        {
            bool legalCapture = false;
            int columnOfFirstOptionToCapture = i_ToCol + 2;
            int rowOfOptionsToCapture = i_ToRow + (2 * m_CurrentPlayer.MovingDirection);
            int columnOfSecondOptionToCapture = i_ToCol - 2;
            int rowAvailbleForTheKing = i_ToRow + (-2 * m_CurrentPlayer.MovingDirection);

            //checks if the player can capture another tool
            if ((IsLeaglCaptureMove(columnOfFirstOptionToCapture, rowOfOptionsToCapture, i_ToCol, i_ToRow))
                || (IsLeaglCaptureMove(columnOfSecondOptionToCapture, rowOfOptionsToCapture, i_ToCol, i_ToRow)))
            {
                legalCapture = true;
            }
            //if the tool is king he can capture backwards
            if (Board.BoardAcceser[i_ToRow, i_ToCol].TypeOfTool == "king")
            {
                if ((IsLeaglCaptureMove(columnOfFirstOptionToCapture, rowAvailbleForTheKing, i_ToCol, i_ToRow))
                || (IsLeaglCaptureMove(columnOfSecondOptionToCapture, rowAvailbleForTheKing, i_ToCol, i_ToRow)))
                {
                    legalCapture = true;
                }
            }

            return legalCapture;
        }


        public void Move(string i_Move)
        {
            char fromCol = i_Move[0];
            char fromRow = i_Move[1];
            char toCol = i_Move[3];
            char toRow = i_Move[4];
            int indexFromCol = fromCol - 'A';
            int indexFromRow = fromRow - 'a';
            int indexToCol = toCol - 'A';
            int indexToRow = toRow - 'a';
            bool aimToCapture = Math.Abs(indexFromRow - indexToRow) == 2;
            //changing the board after a move
            Tool toolToMove = m_Board.BoardAcceser[indexFromRow, indexFromCol];
            m_Board.BoardAcceser[indexFromRow, indexFromCol] = null;
            m_Board.BoardAcceser[indexToRow, indexToCol] = toolToMove;
            toolToMove.SetPositions(indexToRow, indexToCol, Tool.ConvertToStringPosition(indexToRow, indexToCol));
            //modifies the dictionary
            CurrentPlayer.Tools.Remove(Tool.ConvertToStringPosition(indexFromRow, indexFromCol));
            CurrentPlayer.Tools.Add(Tool.ConvertToStringPosition(indexToRow, indexToCol), toolToMove);

            if (aimToCapture)
            {
                string opponentToolPosition = Tool.ConvertToStringPosition(((indexToRow + indexFromRow) / 2), (indexToCol + indexFromCol) / 2);
                m_Board.BoardAcceser[((indexToRow + indexFromRow) / 2), (indexToCol + indexFromCol) / 2] = null;
                Player prevPlayer = (CurrentPlayer.Name == FirstPlayer.Name) ? SecondPlayer : FirstPlayer;
                prevPlayer.Tools.Remove(opponentToolPosition);
            }
            //checks if the player can capture another tool
            if (aimToCapture
                && checkIfNextCaptureIsLegal(indexToCol, indexToRow, indexFromCol, indexFromRow))
            {
                m_LastMove = i_Move.Substring(3, 2);
                CurrentPlayer.JustCaptured = true;
            }

            else
            {
                CurrentPlayer.JustCaptured = false;
                if (m_CurrentPlayer.Equals(m_FirstPlayer))
                {
                    m_CurrentPlayer = m_SecondPlayer;
                }

                else
                {
                    m_CurrentPlayer = m_FirstPlayer;
                }

                CheckIfWinner();
                CurrentPlayer.JustCaptured = false;
            }
        }

        //move random move from the list of moves
        public void ComputerMove()
        {
            List<string> listOfMoves = Player.GetListOfLegalMoves();
            Random randomNum = new Random();
            int num = randomNum.Next(0, listOfMoves.Count);
            string computerMove = listOfMoves.ElementAt(num);
            Move(computerMove);
        }

        internal bool IsLoserPlayerWhenQuit(Player i_PlayerToCheck)
        {
            bool isLosing = false;
            if (i_PlayerToCheck == FirstPlayer)
            {
                isLosing = (FirstPlayer.Score <= SecondPlayer.Score) ? true : false;
            }

            if (i_PlayerToCheck == SecondPlayer)
            {
                isLosing = (SecondPlayer.Score <= FirstPlayer.Score) ? true : false;
            }

            return isLosing;
        }

        public void PlayerSurrender()
        {
            GameEnded = true;
            checkIfWinOrTie();
            if (StatusOfTheGame == GameStatus.WIN)
            {
                string winner = WhoWins();
                Console.WriteLine(winner + " has won !!");
            }
            if (StatusOfTheGame == GameStatus.TIE)
            {
                Console.WriteLine("It's a tie!");
            }
        }

        private void checkIfWinOrTie()
        {
            if (FirstPlayer.Score == SecondPlayer.Score)
            {
                StatusOfTheGame = GameStatus.TIE;
            }

            else
            {
                StatusOfTheGame = GameStatus.WIN;
            }
        }

        public string WhoWins()
        {
            string winner = "";
            int margin = Math.Abs(FirstPlayer.Score - SecondPlayer.Score);
            if (FirstPlayer.Score > SecondPlayer.Score)
            {
                winner = FirstPlayer.Name;
                FirstPlayer.TournamentScore += margin;
            }
            if (SecondPlayer.Score > FirstPlayer.Score)
            {
                winner = SecondPlayer.Name;
                SecondPlayer.TournamentScore += margin;
            }
            return winner;
        }

        internal void CheckIfWinner()
        {
            if (isLosing(CurrentPlayer) == true)
            {
                this.StatusOfTheGame = Game.GameStatus.WIN;
                GameEnded = true;
            }
        }

        private bool isLosing(Player i_PlayerToCheck)
        {
            bool losing = false;
            if ((i_PlayerToCheck.Score == 0) && !i_PlayerToCheck.IsPlayerAbleToMove())
            {
                losing = true;
            }
            return losing;
        }

        private void scoreUpdate(Player i_PlayerScoreToBeUpdated, int i_rowIndexOfToolToDelete, int i_colIndexOfToolToDelete)
        {
            switch (Board.BoardAcceser[i_rowIndexOfToolToDelete, i_colIndexOfToolToDelete].Symbol)
            {
                case 'X':
                    i_PlayerScoreToBeUpdated.Score--;
                    break;
                case 'O':
                    i_PlayerScoreToBeUpdated.Score--;
                    break;
                case 'K':
                    i_PlayerScoreToBeUpdated.Score -= 4;
                    break;
                case 'U':
                    i_PlayerScoreToBeUpdated.Score -= 4;
                    break;
            }
        }

        protected static void OnDoWhenNeedToEat()
        {
            if (DoWhenNeedToEat != null)
            {
                DoWhenNeedToEat(true, EventArgs.Empty);
            }
        }

        protected static void OnDoWhenUnvalidMove()
        {
            if (DoWhenNeedToEat != null)
            {
                DoWhenUnvalidMove(true, EventArgs.Empty);
            }
        }
    }
}