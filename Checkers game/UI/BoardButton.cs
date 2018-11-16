using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logics;
using UI.Properties;

namespace UI
{
    internal class BoardButton : Button
    {
        private Tool m_Tool;

        public BoardButton() : base() { }

        public BoardButton(Tool i_Tool) : base()
        {
            m_Tool = i_Tool;

            if(m_Tool.Symbol == 'X')
            {
                Image = new Bitmap(Resources.BlackSoldier, new Size(20, 20));
            }

            if (m_Tool.Symbol == 'O')
            {
                Image = new Bitmap(Resources.RedSoldier, new Size(20, 20));
            }

            FormGame.BoardUI.setBoardButton(this, i_Tool.RowPosition, i_Tool.ColPosition);
        }

        public Tool Tool
        {
            get { return m_Tool; }
            set { m_Tool = value; }
        }
    }
}
