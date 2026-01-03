using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemiFinalGame
{
    public partial class VictoryForm : Form
    {
        public VictoryForm(int score, int coins, int lives)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            lblScore.Text = score.ToString();
            lblCoins.Text = coins.ToString();
            lblLives.Text = lives.ToString();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Align all numbers to the same X coordinate, slightly to the right of the center
            int labelX = centerX + 20; 

            // Adjust Y coordinates to match the lines on the background image
            // "FINAL SCORE" line
            if (lblScore != null) lblScore.Location = new Point(labelX, centerY - 25);
            if (lblScoreTitle != null) lblScoreTitle.Location = new Point(labelX - lblScoreTitle.Width - 10, centerY - 25);
            
            // "COINS COLLECTED" line
            if (lblCoins != null) lblCoins.Location = new Point(labelX, centerY + 25);
            if (lblCoinsTitle != null) lblCoinsTitle.Location = new Point(labelX - lblCoinsTitle.Width - 10, centerY + 25);
            
            // "LIVES REMAINING" line
            if (lblLives != null) lblLives.Location = new Point(labelX, centerY + 75);
            if (lblLivesTitle != null) lblLivesTitle.Location = new Point(labelX - lblLivesTitle.Width - 10, centerY + 75);

            if (btnPlayAgain != null && btnExit != null)
            {
                int gap = 20;
                int totalWidth = btnPlayAgain.Width + btnExit.Width + gap;
                int startX = (this.ClientSize.Width - totalWidth) / 2;
                int y = this.ClientSize.Height - btnPlayAgain.Height - 50; 

                btnPlayAgain.Location = new Point(startX, y);
                btnExit.Location = new Point(startX + btnPlayAgain.Width + gap, y);
            }
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
