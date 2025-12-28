using System;
using System.Drawing;
using System.Windows.Forms;
using SemiFinalGame.Entities;
using SemiFinalGame.Movements;

namespace SemiFinalGame
{
    public partial class GameForm : Form
    {
        private GameObject player;

        private HorizontalMovement horizontalMovement;
        private VerticalMovement verticalMovement;

        // Key states
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false;
        private bool moveDown = false;

        public GameForm()
        {
            InitializeComponent();

            // Initialize GameObject for player
            player = new GameObject();
            player.Position = new PointF(playerdown.Left, playerdown.Top);

            // Initialize movement objects with speed
            horizontalMovement = new HorizontalMovement(5f); // Horizontal speed
            verticalMovement = new VerticalMovement(5f);     // Vertical speed

            // Timer setup
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // Key events
            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) moveLeft = true;
            if (e.KeyCode == Keys.Right) moveRight = true;
            if (e.KeyCode == Keys.Up) moveUp = true;
            if (e.KeyCode == Keys.Down) moveDown = true;
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) moveLeft = false;
            if (e.KeyCode == Keys.Right) moveRight = false;
            if (e.KeyCode == Keys.Up) moveUp = false;
            if (e.KeyCode == Keys.Down) moveDown = false;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Horizontal movement
            if (moveLeft) horizontalMovement.MoveLeft(player);
            if (moveRight) horizontalMovement.MoveRight(player, this.ClientSize.Width - playerdown.Width);

            // Vertical movement
            if (moveUp) verticalMovement.MoveUp(player);
            if (moveDown) verticalMovement.MoveDown(player, this.ClientSize.Height - playerdown.Height);

            // Apply updated position to PictureBox
            playerdown.Left = (int)player.Position.X;
            playerdown.Top = (int)player.Position.Y;
        }
    }
}
