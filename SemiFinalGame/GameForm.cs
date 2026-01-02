using System;
using System.Drawing;
using System.Windows.Forms;
using SemiFinalGame.Entities;
using SemiFinalGame.Movements;


namespace SemiFinalGame
{
    public partial class GameForm : Form
    {
        private List<Obstacle> obstacles = new List<Obstacle>();
        private List<Coin> coins = new List<Coin>();
        private int score = 0;
        private Label scoreLabel;

        private PictureBox playerSprite;
        private GameObject player;

        private HorizontalMovement horizontalMovement;
        private VerticalMovement verticalMovement;
        private bool gameEnded = false;
        private int initialFormWidth;
        private int initialFormHeight;



        // movement flags
        private bool moveLeft, moveRight, moveUp, moveDown;

        private void CreatePlayer()
        {
            playerSprite = new PictureBox();
            playerSprite.Size = new Size(64, 64);   // Increased from 48,48
            playerSprite.Location = new Point(100, 300);

            //  IMAGE FROM RESOURCES
            playerSprite.Image = Properties.Resources.run_down0;
            playerSprite.SizeMode = PictureBoxSizeMode.StretchImage;
            playerSprite.BackColor = Color.Transparent;

            this.Controls.Add(playerSprite);

            // Link GameObject with sprite
            player = new GameObject();
            player.Position = new PointF(playerSprite.Left, playerSprite.Top);

            // --- ANIMATION SETUP ---
            var anims = new Dictionary<string, List<Image>>();

            // Helper to load range
            List<Image> LoadFrames(string baseName, int start, int count)
            {
                var frames = new List<Image>();
                for (int i = 0; i < count; i++)
                {
                    // Assuming resource names like tile000, tile001, etc.
                    string resName = $"tile{(start + i).ToString("D3")}";
                    var img = (Image)Properties.Resources.ResourceManager.GetObject(resName);
                    if (img != null) frames.Add(img);
                }
                return frames;
            }

            // Mapping based on tile indices
            // Mapping based on tile indices (Corrected based on visual feedback)
            anims["Down"] = LoadFrames("tile", 0, 8);   // 000-007
            anims["Up"] = LoadFrames("tile", 8, 8);     // 008-015
            anims["Left"] = LoadFrames("tile", 16, 8);  // 016-023
            anims["Right"] = LoadFrames("tile", 24, 8); // 024-031

            // Fallback if resources miss - ensure NO list is empty
            if (anims["Down"].Count == 0) anims["Down"].Add(Properties.Resources.run_down0);
            if (anims["Up"].Count == 0) anims["Up"].AddRange(anims["Down"]); // Fallback to Down if Up missing
            if (anims["Left"].Count == 0) anims["Left"].AddRange(anims["Down"]); // Fallback
            if (anims["Right"].Count == 0) anims["Right"].AddRange(anims["Down"]); // Fallback

            if (player is Player p)
            {
                 p.SetAnimation(anims, "Down");
            }
            else
            {
                // Re-create as Player if it was just GameObject
                var oldPos = player.Position;
                player = new Player(); 
                player.Position = oldPos;
                ((Player)player).SetAnimation(anims, "Down");
            }
        }

        public GameForm()
        {
            InitializeComponent();
            this.Resize += GameForm_Resize;
            initialFormWidth = this.ClientSize.Width;
            initialFormHeight = this.ClientSize.Height;



            this.DoubleBuffered = true;
            this.KeyPreview = true;

            // Prevent runtime logic from running in the Designer
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            CreatePlayer();     // Add player first
            CreateScoreLabel(); // Add score label last
            SpawnCoins();       // Add coins next
            SetupObstacles();   // Add obstacles

            scoreLabel.BringToFront();

            horizontalMovement = new HorizontalMovement(8f);
            verticalMovement = new VerticalMovement(8f);

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;
        }


        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (gameEnded) return;

            // ================= PLAYER MOVEMENT =================
            string newDirection = null;

            if (moveLeft)
            {
                horizontalMovement.MoveLeft(player);
                newDirection = "Left";
            }

            if (moveRight)
            {
                horizontalMovement.MoveRight(player, this.ClientSize.Width - playerSprite.Width);
                newDirection = "Right";
            }

            if (moveUp)
            {
                verticalMovement.MoveUp(player);
                newDirection = "Up";
            }

            if (moveDown)
            {
                verticalMovement.MoveDown(player, this.ClientSize.Height - playerSprite.Height);
                newDirection = "Down";
            }

            // Update Animation
            if (player is Player p)
            {
                if (newDirection != null)
                {
                    p.ChangeDirection(newDirection);
                    p.Update(new GameTime { DeltaTime = 0.02f }); // approx for 20ms interval
                }
                
                if (p.Sprite != null)
                    playerSprite.Image = p.Sprite;
            }

            // Apply position to sprite
            playerSprite.Left = (int)player.Position.X;
            playerSprite.Top = (int)player.Position.Y;

            // ================= OBSTACLE UPDATE =================
            bool hitObstacle = false;   // FLAG

            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Update();

                // Only detect collision, DO NOT call GameOver here
                if (playerSprite.Bounds.IntersectsWith(obstacle.Sprite.Bounds))
                {
                    hitObstacle = true;
                }
            }

            // ================= COINS UPDATE =================
            UpdateCoins();

            // ================= GAME OVER AFTER LOOP =================
            if (hitObstacle)
            {
                GameOver();
            }
        }



        private void GameOver()
        {
            if (gameEnded) return;
            ResetMovementFlags();
            gameEnded = true;
            gameTimer.Stop();

            DialogResult result = MessageBox.Show(
                " GAME OVER!\nDo you want to play again?",
                "Gravity Run",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
                RestartGame();
            else
                this.Close();
            this.Focus();
        }


        private void RestartGame()
        {
            ResetMovementFlags();
            // Reset flags
            gameEnded = false;
            gameEnded = false;

            // Reset score
            score = 0;
            scoreLabel.Text = "Score: 0";

            // Reset player
            player.Position = new PointF(100, 300);
            playerSprite.Left = 100;
            playerSprite.Top = 300;

            // Remove OLD coins from form
            foreach (Coin coin in coins)
            {
                if (this.Controls.Contains(coin.Sprite))
                    this.Controls.Remove(coin.Sprite);
            }
            coins.Clear();

            // Remove OLD obstacles
            foreach (Obstacle obs in obstacles)
            {
                if (this.Controls.Contains(obs.Sprite))
                    this.Controls.Remove(obs.Sprite);
            }
            obstacles.Clear();

            // Spawn fresh game objects
            SpawnCoins();        // IMPORTANT
            SetupObstacles();    // IMPORTANT

            gameTimer.Start();
            this.Focus();
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
        private void CreateScoreLabel()
        {
            scoreLabel = new Label();
            scoreLabel.Text = "Score: 0";
            scoreLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            scoreLabel.ForeColor = Color.Black;
            scoreLabel.BackColor = Color.Transparent; // optional
            scoreLabel.AutoSize = true;
            scoreLabel.Location = new Point(10, 10);

            this.Controls.Add(scoreLabel);
            scoreLabel.BringToFront(); // make sure it's on top of all PictureBoxes
        }

        private void SpawnCoins()
        {
            coins.Clear();

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                // Random position but spread across the form
                int x = rnd.Next(50, formWidth - 50);
                int y = rnd.Next(50, formHeight - 50);

                Image coinImage;
                int coinValue;

                // Alternate silver and gold
                if (i % 2 == 0)
                {
                    coinImage = Properties.Resources.giphy; // Silver coin
                    coinValue = 2;
                }
                else
                {
                    coinImage = Properties.Resources.giphy1; // Gold coin
                    coinValue = 5;
                }

                // Horizontal bounds for patrol ±50 pixels from start X
                float leftBound = Math.Max(0, x - 50);
                float rightBound = Math.Min(formWidth - 32, x + 50); // 32 = coin width

        Coin coin = new Coin(coinImage, new Point(x, y), coinValue, new Size(48, 48), leftBound, rightBound, formWidth, formHeight);

                this.Controls.Add(coin.Sprite);
                coins.Add(coin);
            }

            scoreLabel.BringToFront(); // Ensure score is on top
        }




        private void UpdateCoins()
        {
            if (gameEnded) return;

            foreach (Coin coin in coins.ToList())
            {
                coin.Movement.Move(coin.Body, null);

                coin.Sprite.Left = (int)coin.Body.Position.X;
                coin.Sprite.Top = (int)coin.Body.Position.Y;

                if (playerSprite.Bounds.IntersectsWith(coin.Sprite.Bounds))
                {
                    score += coin.Value;
                    scoreLabel.Text = "Score: " + score;

                    this.Controls.Remove(coin.Sprite);
                    coins.Remove(coin);
                }
            }

            // ✅ WIN CONDITION
            if (coins.Count == 0 && !gameEnded)
            {
                gameEnded = true;
                gameTimer.Stop();
                ShowWinMessage();
            }
        }
        private void ShowWinMessage()
        {
            DialogResult result = MessageBox.Show(
                "🎉 YOU WON!\nDo you want to play again?",
                "Victory",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.Yes)
                RestartGame();
            else
                this.Close();
        }
        private void ResetMovementFlags()
        {
            moveLeft = false;
            moveRight = false;
            moveUp = false;
            moveDown = false;
        }





        private void SetupObstacles()
        {
            obstacles.Clear();

            int obstacleCount = 10; // 🔥 increase from 5 to 10
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            Random rnd = new Random();

            // Even horizontal spacing
            int spacing = formWidth / (obstacleCount + 1);

            for (int i = 0; i < obstacleCount; i++)
            {
                PictureBox box = new PictureBox();
                box.Size = new Size(80, 30); // Increased from 60,20

                int x = spacing * (i + 1);
                int y = rnd.Next(0, formHeight - box.Height);

                box.Location = new Point(x, y);
                box.Image = Properties.Resources.box;
                box.SizeMode = PictureBoxSizeMode.StretchImage;
                box.BackColor = Color.Transparent;

                this.Controls.Add(box);

                // Vertical patrol bounds
                float topBound = 0;
                float bottomBound = formHeight - box.Height;

                // Random up/down speed
                float speed = rnd.Next(2, 4);
                if (rnd.Next(2) == 0)
                    speed = -speed;

                obstacles.Add(new Obstacle(
                    box,
                    new VerticalPatrolMovement(topBound, bottomBound, speed)
                ));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;   // optional
        }

        //private int initialFormWidth;
        //private int initialFormHeight;

        private void GameForm_Resize(object sender, EventArgs e)
        {
            if (gameEnded) return;

            // Store initial form size on first call
            if (initialFormWidth == 0 || initialFormHeight == 0)
            {
                initialFormWidth = this.ClientSize.Width;
                initialFormHeight = this.ClientSize.Height;
                return;
            }

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // ====== Player ======
            float playerXRatio = player.Position.X / (float)initialFormWidth;
            float playerYRatio = player.Position.Y / (float)initialFormHeight;

            playerSprite.Left = (int)(playerXRatio * formWidth);
            playerSprite.Top = (int)(playerYRatio * formHeight);
            player.Position = new PointF(playerSprite.Left, playerSprite.Top);

            // ====== Coins ======
            foreach (Coin coin in coins)
            {
                float coinXRatio = coin.Body.Position.X / (float)initialFormWidth;
                float coinYRatio = coin.Body.Position.Y / (float)initialFormHeight;

                coin.Sprite.Left = (int)(coinXRatio * formWidth);
                coin.Sprite.Top = (int)(coinYRatio * formHeight);
                coin.Body.Position = new PointF(coin.Sprite.Left, coin.Sprite.Top);

                // Update horizontal patrol bounds
                coin.Movement = new HorizontalPatrolMovement(
                    Math.Max(0, coin.Sprite.Left - 50),
                    Math.Min(formWidth - coin.Sprite.Width, coin.Sprite.Left + 50)
                );
            }

            // ====== Obstacles ======
            foreach (Obstacle obs in obstacles)
            {
                float obsXRatio = obs.Sprite.Left / (float)initialFormWidth;
                float obsYRatio = obs.Sprite.Top / (float)initialFormHeight;

                obs.Sprite.Left = (int)(obsXRatio * formWidth);
                obs.Sprite.Top = (int)(obsYRatio * formHeight);
                obs.Body.Position = new PointF(obs.Sprite.Left, obs.Sprite.Top);

                // Update vertical patrol bounds
                if (obs.Movement is VerticalPatrolMovement)
                {
                    obs.Movement = new VerticalPatrolMovement(
                        0, // top bound
                        formHeight - obs.Sprite.Height, // bottom bound
                        2f // default speed
                    );
                }
            }
        }

    }
}

