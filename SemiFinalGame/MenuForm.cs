using System;
using System.Drawing;
using System.Windows.Forms;

namespace SemiFinalGame
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Hide menu
            this.Hide();

            // Show Game Form
            using (var gameForm = new GameForm())
            {
                gameForm.ShowDialog();
            }

            // Show menu again when game closes
            this.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
             // Make full screen like game
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
