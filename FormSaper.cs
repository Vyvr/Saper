using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    public partial class FormSaper : Form
    {
        private const int fieldSize = 30;
        SaperLogic mySaperGame;
        public FormSaper()
        {
            InitializeComponent();

            małaToolStripMenuItem_Click(null, null);
        }

        private void małaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mySaperGame = new SaperLogic(8, 8, 10);
            GenerateBoardView();
            RefreshBoardView();
        }

        private void dużaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mySaperGame = new SaperLogic(20, 15, 30);
            GenerateBoardView();
            RefreshBoardView();
        }

        private void RefreshBoardView()
        {
            foreach(Button b in panelBoard.Controls)
            {
                Point p = (Point)(b).Tag;
                if (mySaperGame.GetFieldUncovered(p.X, p.Y))
                {
                    b.BackColor = Color.White;
                    if (mySaperGame.GetFieldType(p.X, p.Y) == SaperLogic.FieldTypeEnum.Bomb)
                    {
                        //b.Text = "B";
                        b.BackgroundImage = Properties.Resources.BombPNg;
                    }
                    else if(mySaperGame.GetNeighbours(p.X, p.Y)>0)
                    {
                        b.Text = (mySaperGame.GetNeighbours(p.X, p.Y).ToString());
                    }
                } 
            }
        }

        private void GenerateBoardView()
        {
            panelBoard.Controls.Clear();
            for(int x=0; x<mySaperGame.Width; x++)
            {
                for(int y = 0; y < mySaperGame.Height; y++)
                {
                    Button b = new Button();
                    b.Size = new Size(fieldSize, fieldSize);
                    b.Location = new Point(x * fieldSize, y * fieldSize);

                    b.Click += ButtonField_Click;
                    b.Tag = new Point(x, y);
                    panelBoard.Controls.Add(b);
                }
            }
        }

        private void ButtonField_Click(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button b = sender as Button;
                Point p = (Point)(b).Tag;
                SaperLogic.GameState result = mySaperGame.Uncover(p.X, p.Y);
                RefreshBoardView();

                if(result == SaperLogic.GameState.Win)
                {
                    MessageBox.Show("Gratulacje, wygrałeś grę!");
                }
                else if(result == SaperLogic.GameState.Defeat)
                {
                    MessageBox.Show("KABOOM!");
                }
            }
            
        }
    }
}
