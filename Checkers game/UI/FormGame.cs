using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logics;
using UI.Properties;

namespace UI
{
    public partial class FormGame : Form
    {
        private static string m_Player1Name;
        private static string m_Player2Name;
        private static BoardButton m_ToolToMove;
        private static FormSettings m_FormSettings;
        private static Game m_Checkers;
        private static BoardUI m_BoardInterface;


        public FormGame()
        {
            this.Load += FormGame_OnLoad;
            InitializeComponent();
            Game.DoWhenNeedToEat += m_Checkers_DoWhenNeedToEat;
            Game.DoWhenUnvalidMove += m_Checkers_DoWhenUnvalidMove;
        }

        private void FormGame_OnLoad(object sender, EventArgs e)
        {
            m_FormSettings = new FormSettings();
            m_FormSettings.ShowDialog();
            m_Player1Name = m_FormSettings.Player1Name;
            m_Player2Name = m_FormSettings.Player2Name;
            if (m_FormSettings.RegisterationCompleted == true)
            {
                initGame(m_FormSettings.BoardSize);
            }

            else
            {
                Application.Exit();
            }
        }


        private void initGame(int i_SizeOfBoard)
        {
            //construct a new checkers game with compatible name of two players or player and computer
            int numberOfPlayers = m_FormSettings.NumberOfPlayers;
            m_Checkers = new Game(m_Player1Name, m_Player2Name, i_SizeOfBoard, numberOfPlayers);

            m_BoardInterface = new BoardUI(i_SizeOfBoard);
            Controls.Add(m_BoardInterface);

            player1Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player1Name, "\n", Game.FirstPlayer.Score, "\n", Game.FirstPlayer.TournamentScore);
            player2Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player2Name, "\n", Game.SecondPlayer.Score, "\n", Game.SecondPlayer.TournamentScore);
            player1Label.Left = m_BoardInterface.Left;
            player2Label.Left = m_BoardInterface.Right - player2Label.Width + 15;
            turnLabel.Text = m_Player1Name + "'s turn";
            Padding = new Padding(50, 30, 50, 50);
            turnLabel.Location = new Point((ClientSize.Width / 2 - turnLabel.Width / 2), 20);
            AutoSize = true;
            Top = 50;
        }


        private void m_Checkers_DoWhenNeedToEat(object sender, EventArgs e)
        {
            MessageBox.Show("You Must Eat!", "Move error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void m_Checkers_DoWhenUnvalidMove(object sender, EventArgs e)
        {
            MessageBox.Show("You have to commit a different move!", "Move error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public partial class BoardUI : Panel
        {

            private BoardButton[,] m_BoardGame;
            private int m_BoardSize;
            public BoardUI(int i_SizeOfBoard)
            {
                m_BoardSize = i_SizeOfBoard;
                initBoard(m_BoardSize);
                AutoSize = true;
            }

            private void initBoard(int i_SizeOfBoard)
            {
                m_BoardGame = new BoardButton[i_SizeOfBoard, i_SizeOfBoard];

                for (int i = 0; i < i_SizeOfBoard; i++)
                {
                    for (int j = 0; j < i_SizeOfBoard; j++)
                    {
                        m_BoardGame[i, j] = new BoardButton();
                        setBoardButton(m_BoardGame[i, j], i, j);

                        if ((i + j) % 2 == 0)
                        {
                            m_BoardGame[i, j].Enabled = false;
                            m_BoardGame[i, j].BackColor = Color.Black;
                        }

                        else
                        {
                            if (Game.Board.BoardAcceser[i, j] != null)
                            {
                                m_BoardGame[i, j] = new BoardButton(Game.Board.BoardAcceser[i, j]);
                            }

                            m_BoardGame[i, j].Click += new System.EventHandler(this.boardButton_OnClick);
                        }

                        Controls.Add(m_BoardGame[i, j]);
                    }
                }
            }

            internal static void setBoardButton(Button i_BoardButton, int i, int j)
            {
                i_BoardButton.Width = 40;
                i_BoardButton.Height = 40;
                i_BoardButton.Location = new Point(j * 40 + 10, i * 40 + 70);
                i_BoardButton.BackColor = Color.White;
                i_BoardButton.Name = Tool.ConvertToStringPosition(i, j);
            }

            private void boardButton_OnClick(object sender, EventArgs e)
            {
                BoardButton clickedButton = sender as BoardButton;
                string winner;

                if (checkIfAnotherPlayerSquareIsMarked())
                {

                    if (Game.CurrentPlayer.Tools.ContainsKey(clickedButton.Name))
                    {
                        clickedButton.BackColor = Color.White;
                    }

                    else
                    {
                        string playerWantedMove = m_ToolToMove.Name + ">" + clickedButton.Name;

                        if (Game.CheckAvailableMove(playerWantedMove))
                        {
                            m_Checkers.Move(playerWantedMove);
                            m_ToolToMove.BackColor = Color.White;
                            this.updateBoard();
                            while (Game.CurrentPlayer.Name == "Computer" && m_Checkers.GameEnded == false)
                            {
                                if ((m_FormSettings.NumberOfPlayers == 1) && (Game.CurrentPlayer.Name == "Computer"))
                                {
                                    m_Checkers.ComputerMove();
                                    this.updateBoard();
                                }
                                
                                if (m_FormSettings.NumberOfPlayers == 2)
                                {
                                    break;
                                }
                            }

                            turnLabel.Text = Game.CurrentPlayer.Name + "'s turn";
                            player1Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player1Name, "\n", Game.FirstPlayer.Score, "\n", Game.FirstPlayer.TournamentScore);
                            player2Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player2Name, "\n", Game.SecondPlayer.Score, "\n", Game.SecondPlayer.TournamentScore);

                            if (m_Checkers.StatusOfTheGame == Game.GameStatus.WIN)
                            {
                               winner = m_Checkers.WhoWins();
                               DialogResult dialogResult = MessageBox.Show(string.Format(@"{0} has won!{1}{2}", winner, "\n", "Another round?"), "Damka - Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (dialogResult == DialogResult.No)
                                {
                                    Application.Exit();
                                }

                                else if (dialogResult == DialogResult.Yes)
                                {
                                    foreach (BoardButton button in this.m_BoardGame)
                                    {
                                        this.Controls.Remove(button);
                                    }

                                    FormGame.m_Checkers.RestartNewGame();
                                    FormGame.m_BoardInterface.initBoard(Game.Board.BoardSize);
                                    turnLabel.Text = m_Player1Name + "'s turn";
                                    player1Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player1Name, "\n", Game.FirstPlayer.Score, "\n", Game.FirstPlayer.TournamentScore);
                                    player2Label.Text = string.Format(@"{0}{1}Game score: {2}{3}Tournament score: {4}", m_Player2Name, "\n", Game.SecondPlayer.Score, "\n", Game.SecondPlayer.TournamentScore);
                                }
                            }
                        }
                    }
                }

                else
                {
                    if (Game.CurrentPlayer.Tools.ContainsKey(clickedButton.Name))
                    {
                        if (clickedButton.BackColor == Color.White && Game.CheckIfThisMySquares(clickedButton.Tool.ColPosition, clickedButton.Tool.RowPosition))
                        {
                            clickedButton.BackColor = Color.LightBlue;
                            m_ToolToMove = clickedButton;
                        }
                    }
                }
            }

            private void updateBoard()
            {
                for (int i = 0; i < m_BoardSize; i++)
                {
                    for (int j = 0; j < m_BoardSize; j++)
                    {
                        if (Game.Board.BoardAcceser[i, j] != null)
                        {
                            m_BoardGame[i, j].Tool = Game.Board.BoardAcceser[i, j];
                            switch (m_BoardGame[i, j].Tool.Symbol)
                            {
                                case 'X':
                                    m_BoardGame[i, j].Image = new Bitmap(Resources.BlackSoldier, new Size(20, 20));
                                    break;
                                case 'K':
                                    m_BoardGame[i, j].Image = new Bitmap(Resources.blackking2, new Size(40, 40));
                                    break;
                                case 'U':
                                    m_BoardGame[i, j].Image = new Bitmap(Resources.redking2, new Size(20, 20));
                                    break;
                                case 'O':
                                    m_BoardGame[i, j].Image = new Bitmap(Resources.RedSoldier, new Size(20, 20));
                                    break;
                            } 
                        }

                        else
                        {
                            m_BoardGame[i, j].Image = null;
                        }
                    }
                }
            }


            private bool checkIfAnotherPlayerSquareIsMarked()
            {
                bool anotherSquareIsMarked = false;

                for (int i = 0; i < m_BoardSize; i++)
                {
                    for (int j = 0; j < m_BoardSize; j++)
                    {
                        if((m_BoardGame[i, j].BackColor == Color.LightBlue) &&
                            Game.CurrentPlayer.Tools.ContainsKey(m_BoardGame[i, j].Name))
                        {
                            anotherSquareIsMarked = true;
                        } 
                    }
                }

                return anotherSquareIsMarked;
            }
        }
    }
}
