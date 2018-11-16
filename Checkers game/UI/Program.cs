using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace UI
{
    public class Program
    {
        public static void Main()
        {
            FormGame checkers = new FormGame();
            checkers.ShowDialog();
        }
    }
}
