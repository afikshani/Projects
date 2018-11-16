using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FormSettings : Form
    {
        private bool m_RegisterationCompleted = false;
        private int m_BoardSize = 0;
        private string m_Player1Name = "Player 1";
        private string m_Player2Name = "Computer";
        private int m_NumOfPlayers = 1;

        public FormSettings()
        {
            InitializeComponent();
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public string Player1Name
        {
            get { return m_Player1Name; }
        }

        public string Player2Name
        {
            get { return m_Player2Name; }
        }

        public int NumberOfPlayers
        {
            get { return m_NumOfPlayers; }
        }

        public bool RegisterationCompleted
        {
            get { return m_RegisterationCompleted; }
        }


        private void boardSizeButton_OnChecked(object sender, EventArgs e)
        {
            if (sixOnSix.Checked)
            {
                m_BoardSize = 6;
            }

            if (eightOnEight.Checked)
            {
                m_BoardSize = 8;
            }

            if (tenOnTen.Checked)
            {
                m_BoardSize = 10;
            }
        }

        private void isPlayerTwoPlays_OnClick(object sender, EventArgs e)
        {
            if (isPlayerTwoPlays.Checked == true)
            {
                textBoxPlayer2.Enabled = true;
                m_NumOfPlayers = 2;
            }

            if (isPlayerTwoPlays.Checked == false)
            {
                textBoxPlayer2.Enabled = false;
                m_NumOfPlayers = 1;
            }
        }

        private void playerTextBox_TextChanged(object sender, EventArgs e)
        {
            m_Player1Name = textBoxPlayer1.Text;
            if (isPlayerTwoPlays.Checked)
            {
                m_Player2Name = textBoxPlayer2.Text;
            }
        }

        private void doneButton_OnClick(object sender, EventArgs e)
        {
            m_RegisterationCompleted = true;
            Close();
        }
    }
}
