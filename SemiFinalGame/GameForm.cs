using SemiFinalGame.Entities;
using SemiFinalGame.FileHandling;
using SemiFinalGame.Interfaces;
using SemiFinalGame.Movements;
using SemiFinalGame.Properties;
using SemiFinalGame.Systems;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace SemiFinalGame
{
    public partial class GameForm : Form
    {
        private List<Obstacle> obstacles = new List<Obstacle>();
        private List<Coin> coins = new List<Coin>();
        private List<Stone> stones = new List<Stone>(); // List to hold stones
        private int score = 0;
        private int coinsCollectedCount = 0;
        private Label scoreLabel;
        private Label levelLabel; // Logic Level Indicator

        private PictureBox playerSprite;
        private GameObject player;

        private HorizontalMovement horizontalMovement;
        private VerticalMovement verticalMovement;

        private bool gameEnded = false;

        // Level 3: Chaser Enemy
        private Enemy chaser;
        private PictureBox chaserSprite;


        private int initialFormWidth;
        private int initialFormHeight;
        private int lives = 3;
        private Label livesLabel;

        // Level System
        private int currentLevel;

        private Panel healthBarBackground;
        private Panel healthBarForeground;
        private Label HealthLabel;
        private int maxLives = 3;  // total lives
        private bool isInvincible = false; // to prevent multi-hit per frame

        // movement flags
        private bool moveLeft, moveRight, moveUp, moveDown;

        // Countdown Timer Fields
        private Label countdownLabel;
        private System.Windows.Forms.Timer startTimer;
        private int countdownValue = 3;

        // Physics Engine
        private PhysicsSystem physicsEngine = new PhysicsSystem();

        // Background Animation Fields
        private float backgroundX = 0;
        private float backgroundSpeed = 2.0f; // Positive for Left-to-Right
        private Image backgroundImage;
        private Image backgroundImageFlipped; //two images used for seamless looping
        private bool isCurrentTileFlipped = false;

        private void CreateCountdownLabel()
        {
            countdownLabel = new Label();
            countdownLabel.Text = "";
            countdownLabel.Font = new Font("Arial", 72, FontStyle.Bold);
            countdownLabel.ForeColor = Color.Red;
            countdownLabel.BackColor = Color.Transparent;
            countdownLabel.AutoSize = true;
            countdownLabel.TextAlign = ContentAlignment.MiddleCenter;//Aligns text centered inside the label

            // Center it (approximate, will be refined in StartCountdown or Update)
            countdownLabel.Location = new Point(this.ClientSize.Width / 2 - 50, this.ClientSize.Height / 2 - 50);
            //ClientSize → usable area of the form (excluding borders)
            this.Controls.Add(countdownLabel);//Adds the label to the form’s control collection

            //Without this, the label would not appear on screen
            countdownLabel.BringToFront();
            // Prevents it from being hidden behind player, enemies, or background
        }

        private void StartCountdown()
        {
            countdownValue = 3;
            countdownLabel.Text = countdownValue.ToString(); //Converts the integer 3 into text
            countdownLabel.Location = new Point((this.ClientSize.Width - countdownLabel.Width) / 2, (this.ClientSize.Height - countdownLabel.Height) / 2); // Recenter
            countdownLabel.Visible = true;

            startTimer = new System.Windows.Forms.Timer();
            startTimer.Interval = 1000; // 1 second
            //Timer ticks every 1000 milliseconds
            startTimer.Tick += StartTimer_Tick;
            startTimer.Start();
        }

        private void StartTimer_Tick(object sender, EventArgs e)
        //sender : The timer object that triggered the event
        //e : Event arguments(empty in this case).
        {
            countdownValue--;

            if (countdownValue > 0)
            {
                countdownLabel.Text = countdownValue.ToString();
            }
            else if (countdownValue == 0)
            {
                countdownLabel.Text = "GO!";
            }
            else
            {
                // Countdown finished
                startTimer.Stop(); //Stops the countdown timer so it won’t tick again.
                startTimer.Dispose(); //Frees up memory used by the timer.
                countdownLabel.Visible = false; //Hides the countdown label after it finishes.

                // START THE ACTUAL GAME
                gameTimer.Start(); //Starts the main game loop(movement, obstacles, coins, etc.)
            }

            // Keep centered
            countdownLabel.Location = new Point((this.ClientSize.Width - countdownLabel.Width) / 2, (this.ClientSize.Height - countdownLabel.Height) / 2);
        }

        private void CreatePlayer()
        {
            playerSprite = new PictureBox();
            playerSprite.Size = new Size(80, 80);   // Increased from 48,48
            playerSprite.Location = new Point(100, 300);

            //  IMAGE FROM RESOURCES
            playerSprite.Image = Properties.Resources.tile000;
            playerSprite.SizeMode = PictureBoxSizeMode.StretchImage;
            playerSprite.BackColor = Color.Transparent;

            this.Controls.Add(playerSprite);

            // Link GameObject with sprite 
            player = new GameObject();
            player.Position = new PointF(playerSprite.Left, playerSprite.Top);

            // --- ANIMATION SETUP ---
            var anims = new Dictionary<string, List<Image>>(); //This will allow walking animations for different directions.

            // Helper to load range
            List<Image> LoadFrames(string baseName, int start, int count)
            //LoadFrames  Loads a sequence of images for animation.
            // baseName → Prefix of resource name(like "tile").
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
            // here last parameter is 8 means that 8 number of frames are going to be to load
            anims["Down"] = LoadFrames("tile", 0, 8);   // 000-007
            anims["Up"] = LoadFrames("tile", 8, 8);     // 008-015
            anims["Left"] = LoadFrames("tile", 16, 8);  // 016-023
            anims["Right"] = LoadFrames("tile", 24, 8); // 024-031

            // Fallback if resources miss - ensure NO list is empty
            //If any images are missing, uses "Down" frames as fallback.

            //Prevents crashes when animation tries to display a missing frame.
            if (anims["Down"].Count == 0) anims["Down"].Add(Properties.Resources.tile000);
            if (anims["Up"].Count == 0) anims["Up"].AddRange(anims["Down"]); // Fallback to Down if Up missing
            if (anims["Left"].Count == 0) anims["Left"].AddRange(anims["Down"]); // Fallback
            if (anims["Right"].Count == 0) anims["Right"].AddRange(anims["Down"]); // Fallback

            if (player is Player p)
            {
                p.SetAnimation(anims, "Down");
            }
            else
            {
                //If not : converts GameObject into Player, keeps the position, and sets animation.
                // Re-create as Player if it was just GameObject
                var oldPos = player.Position;
                player = new Player();
                player.Position = oldPos;
                ((Player)player).SetAnimation(anims, "Down");
                //Player class is specialized for movement + animations, while GameObject is just a logical container.
            }
        }

        public GameForm(int level = 1)
        {
            this.currentLevel = level;

            InitializeComponent();//Sets up all the controls defined in the Designer (buttons, labels, etc.) before you add custom logic.
            this.Resize += GameForm_Resize;
            initialFormWidth = this.ClientSize.Width;
            initialFormHeight = this.ClientSize.Height;

            this.DoubleBuffered = true; // prevents flickering when redrawing graphics.
            this.KeyPreview = true; //allows the form to detect key presses even if a control (like a button) is focused.

            // Prevent runtime logic from running in the Designer
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            SetupLevelDifficulty(currentLevel);

            CreatePlayer();     // Add player first
            CreateScoreLabel(); // Add score label last
            CreateLevelLabel(); // Add level label
            SpawnCoins();       // Add coins next
            SetupObstacles();   // Add obstacles
            SetupStones();      // Add stones for Level 2
            SetupChaser();      // Add chaser for Level 3



            scoreLabel.BringToFront();

            CreateCountdownLabel(); // Init countdown label
            StartCountdown();       // Start the 3-2-1 sequence

            horizontalMovement = new HorizontalMovement(14f);
            verticalMovement = new VerticalMovement(14f);

            // Adjust player speed for Level 2
            if (currentLevel >= 2)
            {
                horizontalMovement = new HorizontalMovement(28f);
                verticalMovement = new VerticalMovement(28f);
            }
            //Sets game loop timer to tick every 20ms
            gameTimer.Interval = 20;
            gameTimer.Tick -= GameTimer_Tick; // safety
            gameTimer.Tick += GameTimer_Tick;//Adds GameTimer_Tick as the event handler for the game loop.

            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;

            CreateLivesLabel();
            CreateHealthBar();

            // Background Setup
            backgroundImage = Properties.Resources.background_still;

            // Create the flipped version for seamless tiling
            backgroundImageFlipped = (Image)backgroundImage.Clone();
            backgroundImageFlipped.RotateFlip(RotateFlipType.RotateNoneFlipX);

            this.BackgroundImage = null; // Disable default background rendering
            this.DoubleBuffered = true; // Ensure this is definitely on
            //The game timer is used to continuously update the game state and redraw the game,

            // Play Game Music
            SemiFinalGame.Sound.SoundManager.PlayMusic(Properties.Resources.GameFormsound);
        }

        private void SetupLevelDifficulty(int level)
        {
            if (level == 1)
            {
                backgroundSpeed = 2.0f;
                maxLives = 3;
            }
            else if (level == 2)
            {
                backgroundSpeed = 5.0f;
                maxLives = 7;   // MORE LIVES IN LEVEL 2
            }
            else
            {
                backgroundSpeed = 6.0f;
                maxLives = 8;
            }

            lives = maxLives; // IMPORTANT: sync lives with maxLives
        }

        private async void HandlePlayerHit()
        {
            if (isInvincible) return;
            isInvincible = true;

            // 1️⃣ Reduce lives
            lives--;
            livesLabel.Text = "Lives: " + lives;

            // 2️⃣ Update health
            float healthPercentage = (lives / (float)maxLives);
            player.Health = (int)(healthPercentage * 100);
            healthBarForeground.Width = (int)(healthPercentage * healthBarBackground.Width);

            //  Reset player position
            player.Position = new PointF(100, 300);
            playerSprite.Left = 100;
            playerSprite.Top = 300;
            //await means “wait here without freezing the game.
            await Task.Delay(500);
            isInvincible = false;

            if (lives <= 0)
                GameOver();
        }
        //An async method is a method that can pause and resume later
        //without stopping the game or freezing the UI.


        private void CreateHealthBar()
        {
            HealthLabel = new Label();
            HealthLabel.Text = "Health: ";
            // Background (gray)
            healthBarBackground = new Panel
            {

                Size = new Size(150, 20),
                BackColor = Color.Gray,
                Location = new Point(this.ClientSize.Width - 160, 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            this.Controls.Add(healthBarBackground);

            // Foreground (red)
            healthBarForeground = new Panel
            {
                Size = healthBarBackground.Size,
                BackColor = Color.Green,
                Location = healthBarBackground.Location,
                Anchor = AnchorStyles.Top | AnchorStyles.Right//This line locks the control to the top and right edges of the form, so its position stays fixed relative to those edges when the window is resized.
            };
            this.Controls.Add(healthBarForeground);
            healthBarForeground.BringToFront();
        }

        private void GameTimer_Tick(object sender, EventArgs e) //It is the main game loop that updates movement, animation, collisions, and rendering every frame.
        { //A timer provides continuous updates at fixed intervals to simulate real-time gameplay.
            if (gameEnded) return;

            // PLAYER MOVEMENT 
            string newDirection = null;//Stores player’s current movement direction
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

            // OBSTACLE UPDATE 
            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Update();

                // Only detect collision, DO NOT call GameOver here
                if (playerSprite.Bounds.IntersectsWith(obstacle.Sprite.Bounds))
                {
                    HandlePlayerHit();
                    break; // prevent multiple hits in same frame
                }
            }
            // COINS UPDATE
            UpdateCoins();
            //  STONES UPDATE (Level 2) 
            UpdateStones();
            // CHASER UPDATE (Level 3) 
            UpdateChaser();
            //  BACKGROUND ANIMATION 
            UpdateBackground();
            this.Invalidate(); // trigger OnPaint
        }
        private void UpdateBackground()
        {
            // Move Background Left to Right
            backgroundX += backgroundSpeed;

            // Reset based on FORM WIDTH (since we stretch the image to form width)
            if (backgroundX >= this.ClientSize.Width)
            {
                backgroundX = 0;
                // Toggle the tile type to alternate between Normal and Flipped
                isCurrentTileFlipped = !isCurrentTileFlipped;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (backgroundImage != null && backgroundImageFlipped != null)
            {
                // STRETCH Logic to fix "patches"
                int drawWidth = this.ClientSize.Width;
                int drawHeight = this.ClientSize.Height;

                // OPTIMIZATION: Use NearestNeighbor for performance and pixel-perfect look
                // HighQualityBicubic is too slow for large scrolling backgrounds
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.None; // or HighSpeed
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

                // Determine which image is "Current" (leaving right) and "Incoming" (entering from left)
                Image currentImage = isCurrentTileFlipped ? backgroundImageFlipped : backgroundImage;
                Image incomingImage = isCurrentTileFlipped ? backgroundImage : backgroundImageFlipped;

                // Convert float to int to prevent sub-pixel rendering gaps
                int x = (int)Math.Round(backgroundX);

                // 1. Draw current cycle (Moving from 0 to Width)
                // Draw slightly wider (+1) or ensure overlap logic
                e.Graphics.DrawImage(currentImage, x, 0, drawWidth, drawHeight);

                // 2. Draw incoming cycle (Moving from -Width to 0)
                if (x > 0)
                {
                    // Overlap by 1 pixel to remove the seam line
                    // Position: x - drawWidth + 1 (The +1 moves it 1 pixel to the right, creating overlap)
                    e.Graphics.DrawImage(incomingImage, x - drawWidth + 1, 0, drawWidth, drawHeight);
                }
            }
        }
        private void GameOver()
        {
            if (gameEnded) return;
            ResetMovementFlags();
            gameEnded = true;
            gameTimer.Stop();

            // Use custom GameOver form
            SemiFinalGame.Sound.SoundManager.StopMusic(); // Stop game music
            using (var gameOverForm = new GameOver(score, coinsCollectedCount, lives))
            {
                DialogResult result = gameOverForm.ShowDialog();

                if (result == DialogResult.Yes)
                    RestartGame();
                else
                    this.Close();
            }
            this.Focus();
            //File Handling (save in file)
            SaveData.SaveStats(this.currentLevel, this.score, this.coinsCollectedCount, this.lives);
        }




        private void RestartGame()
        {
            ResetMovementFlags();
            gameEnded = false;

            // Reset score
            score = 0;
            coinsCollectedCount = 0;
            scoreLabel.Text = "Score: 0";

            //  Reset lives and health 
            lives = maxLives;                      // Set lives to max
            livesLabel.Text = "Lives: " + lives;   // Update label

            player.Health = 100;                   // Reset player health
            livesLabel.Text = "Lives: " + lives;   // Update label

            // Update Level Label
            if (levelLabel != null) levelLabel.Text = "Level: " + currentLevel;

            player.Health = 100;                   // Reset player health

            // Reset player position
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

            // Re-setup Difficulty (in case it changed or for cleanlines  )
            SetupLevelDifficulty(currentLevel);

            // Spawn fresh game objects
            SpawnCoins();        // IMPORTANT
            SetupObstacles();    // IMPORTANT
            SetupStones();       // IMPORTANT
            SetupChaser();       // IMPORTANT



            gameTimer.Stop();
            gameTimer.Start();

            // Play Game Music (for Level 2, 3, or Restart)
            SemiFinalGame.Sound.SoundManager.PlayMusic(Properties.Resources.GameFormsound);

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

        private void CreateLevelLabel()
        {
            levelLabel = new Label();
            levelLabel.Text = "Level: " + currentLevel;
            levelLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            levelLabel.ForeColor = Color.Cyan;
            levelLabel.BackColor = Color.Transparent;
            levelLabel.AutoSize = true;
            //levelLabel.Location = new Point(this.ClientSize.Width - 120, 10);
            levelLabel.Location = new Point(
            this.ClientSize.Width - levelLabel.PreferredWidth - 10, 60);
            levelLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;


            this.Controls.Add(levelLabel);
            levelLabel.BringToFront();
        }

        private void SpawnCoins()
        {
            coins.Clear();

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            Random rnd = new Random();

            for (int i = 0; i < 20; i++)
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
                    coinsCollectedCount++; // Track count
                    scoreLabel.Text = "Score: " + score;

                    // Play Coin Sound
                    SemiFinalGame.Sound.SoundManager.PlaySoundEffect(Properties.Resources.CoinSound);

                    this.Controls.Remove(coin.Sprite);
                    coins.Remove(coin);
                }
            }

            // WIN CONDITION
            if (coins.Count == 0 && !gameEnded)
            {
                gameEnded = true;
                gameTimer.Stop();
                ShowWinMessage();
            }
        }


        private void ShowWinMessage()
        {
            // Save Stats on Level Win
            SemiFinalGame.FileHandling.SaveData.SaveStats(this.currentLevel, this.score, this.coinsCollectedCount, this.lives);

            // Use custom VictoryForm
            // Pass the current level so VictoryForm can decide to show "Next Level"
            SemiFinalGame.Sound.SoundManager.StopMusic(); // Stop game music
            using (var victoryForm = new VictoryForm(score, coinsCollectedCount, lives, currentLevel))
            {
                DialogResult result = victoryForm.ShowDialog();

                if (result == DialogResult.Yes) // Played clicked "Level 2" or "Restart"
                {
                    // VictoryForm sets a static property or we handle leveling up?
                    // Simpler: VictoryForm returns "Yes", but we need to know if it's NEXT LEVEL.
                    // Actually, if we want to change level, we might need to close this Generic Form and open a NEW one
                    // OR just update currentLevel and RestartGame().

                    if (victoryForm.GoToNextLevel)
                    {
                        currentLevel++;
                        RestartGame(); // Restart with new level difficulty
                    }
                    else
                    {
                        RestartGame(); // Just restart same level
                    }
                }
                else
                {
                    this.Close();
                }
            }
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

            int obstacleCount;
            if (currentLevel == 2)
                obstacleCount = 5; // 20 obstacles for Level 2
            else
                obstacleCount = 10; // Default Level 1

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
                // Random up/down speed based on Level
                int minSpeed, maxSpeed;

                if (currentLevel == 1)
                {
                    minSpeed = 3;
                    maxSpeed = 6;
                }
                else if (currentLevel == 2)
                {
                    minSpeed = 6;   //  faster than Level 1
                    maxSpeed = 9;
                }
                else
                {
                    minSpeed = 8;
                    maxSpeed = 12;
                }


                float speed = rnd.Next(minSpeed, maxSpeed);
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


        private void CreateLivesLabel()
        {
            livesLabel = new Label();
            livesLabel.Text = "Lives: 3";
            livesLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            livesLabel.ForeColor = Color.Red;
            livesLabel.BackColor = Color.Transparent;
            livesLabel.AutoSize = true;

            // Place near top-right initially
            livesLabel.Location = new Point(
                this.ClientSize.Width - livesLabel.PreferredWidth - 10,
                10
            );

            // THIS is the key line
            livesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.Controls.Add(livesLabel);
            livesLabel.BringToFront();
        }



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

            // Player
            float playerXRatio = player.Position.X / (float)initialFormWidth;
            float playerYRatio = player.Position.Y / (float)initialFormHeight;

            playerSprite.Left = (int)(playerXRatio * formWidth);
            playerSprite.Top = (int)(playerYRatio * formHeight);
            player.Position = new PointF(playerSprite.Left, playerSprite.Top);

            // Coins 
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
            //  Obstacles
            foreach (Obstacle obs in obstacles)
            {
                float obsXRatio = obs.Sprite.Left / (float)initialFormWidth;
                float obsYRatio = obs.Sprite.Top / (float)initialFormHeight;

                obs.Sprite.Left = (int)(obsXRatio * formWidth);
                obs.Sprite.Top = (int)(obsYRatio * formHeight);
                obs.Body.Position = new PointF(obs.Sprite.Left, obs.Sprite.Top);
                // Update vertical patrol bounds
                // Update vertical patrol bounds WITH LEVEL-BASED SPEED
                if (obs.Movement is VerticalPatrolMovement)
                {
                    float resizeSpeed;

                    if (currentLevel == 1)
                        resizeSpeed = 4f;
                    else if (currentLevel == 2)
                        resizeSpeed = 7f;   // medium speed
                    else
                        resizeSpeed = 10f;

                    obs.Movement = new VerticalPatrolMovement(
                        0, // top bound
                        formHeight - obs.Sprite.Height, // bottom bound
                        resizeSpeed
                    );
                }

            }
        }


        private void SetupStones()
        {
            // Clear existing stones
            foreach (Stone s in stones)
            {
                if (this.Controls.Contains(s.Sprite))
                    this.Controls.Remove(s.Sprite);
            }
            stones.Clear();

            // Only spawn stones in Level 2 or higher
            if (currentLevel < 2) return;

            int stoneCount = 10; // Number of stones
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;
            Random rnd = new Random();

            for (int i = 0; i < stoneCount; i++)
            {
                PictureBox stoneBox = new PictureBox();
                stoneBox.Size = new Size(48, 48);
                // Random starting position (some on screen, some above)
                int x = rnd.Next(50, formWidth - 50);
                int y = rnd.Next(-600, -50); // Start above the screen

                stoneBox.Location = new Point(x, y);
                stoneBox.Image = Properties.Resources.stone; // Assuming 'stone' resource exists
                stoneBox.SizeMode = PictureBoxSizeMode.StretchImage;
                stoneBox.BackColor = Color.Transparent;

                this.Controls.Add(stoneBox);

                // Movement: VerticalPatrolMovement but configured to just fall endlessly
                // We set bottomBound very high so it doesn't bounce back up automatically
                // We handle the "recycle" manually when it goes off screen
                float speed = rnd.Next(5, 12); // Random falling speed

                IMovement movement;
                if (currentLevel == 3)
                {
                    // In Level 3, we use real physics!
                    stoneBox.BackColor = Color.Transparent; // Fix transparency
                    movement = null; // PhysicsSystem will handle movement

                    Stone physicalStone = new Stone(stoneBox, null);
                    physicalStone.Body.HasPhysics = true;
                    // Reduced gravity range for better playability
                    physicalStone.Body.CustomGravity = 0.5f + (float)rnd.NextDouble() * 0.10f;
                    stones.Add(physicalStone);
                }
                else
                {
                    // Use existing VerticalPatrolMovement for Level 2
                    movement = new VerticalPatrolMovement(-1000, formHeight + 2000, speed);
                    stones.Add(new Stone(stoneBox, movement));
                }
            }
        }

        private void UpdateStones()
        {
            if (gameEnded) return;

            int formHeight = this.ClientSize.Height;
            int formWidth = this.ClientSize.Width;
            Random rnd = new Random();

            // Apply Physics to stones in Level 3
            if (currentLevel == 3 && stones.Count > 0)
            {
                var physicsObjects = stones.Select(s => s.Body).ToList();
                physicsEngine.Apply(physicsObjects);
            }

            foreach (Stone stone in stones)
            {
                if (currentLevel != 3)
                {
                    stone.Update(); // Uses standard Movement (VerticalPatrolMovement)
                }
                else
                {
                    // For Physics stones, we only sync the sprite to the body
                    stone.Sprite.Left = (int)stone.Body.Position.X;
                    stone.Sprite.Top = (int)stone.Body.Position.Y;
                }

                // Recycle: If stone goes below screen
                if (stone.Sprite.Top > formHeight)
                {
                    // Reset to top with new random X
                    int newX = rnd.Next(50, formWidth - 50);
                    stone.Body.Position = new PointF(newX, rnd.Next(-100, -10));
                    stone.Body.Velocity = PointF.Empty; // Reset momentum for physics stones
                    stone.Sprite.Top = (int)stone.Body.Position.Y;
                    stone.Sprite.Left = (int)stone.Body.Position.X;
                }

                // Collision with Player
                if (playerSprite.Bounds.IntersectsWith(stone.Sprite.Bounds))
                {
                    HandlePlayerHit();
                    // Optional: Reset this specific stone so it doesn't hit again immediately
                    stone.Body.Position = new PointF(stone.Body.Position.X, -100);
                }
            }
        }
        private void SetupChaser()
        {
            // Clean up existing chaser
            if (chaserSprite != null && this.Controls.Contains(chaserSprite))
            {
                this.Controls.Remove(chaserSprite);
                chaserSprite.Dispose();
                chaserSprite = null;
            }
            chaser = null;

            if (currentLevel < 3) return;

            // Create Sprite
            chaserSprite = new PictureBox();
            chaserSprite.Size = new Size(100, 100);
            chaserSprite.BackColor = Color.Transparent; 
            chaserSprite.Image = SemiFinalGame.Properties.Resources.rightside;
            chaserSprite.Location = new Point(this.ClientSize.Width - 100, 100); // Start top-right
            chaserSprite.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(chaserSprite);
            chaserSprite.BringToFront();

            // Create Entity
            chaser = new Enemy();
            chaser.Position = new PointF(chaserSprite.Left, chaserSprite.Top);
            chaser.Size = new SizeF(chaserSprite.Width, chaserSprite.Height);

            // Movement - using the existing class ChasePlayerMovement
            // Make sure player is created before this!
            if (player != null)
            {
                // Faster than stones, but escapable
                chaser.Movement = new ChasePlayerMovement(player, 3.5f);
            }
        }

        private void UpdateChaser()
        {
            if (gameEnded || chaser == null) return;

            // Update logical position
            chaser.Update(new GameTime { DeltaTime = 0.02f });

            // Sync Sprite
            chaserSprite.Left = (int)chaser.Position.X;
            chaserSprite.Top = (int)chaser.Position.Y;
            if (chaser.Sprite != null)
                chaserSprite.Image = chaser.Sprite;

            // Collision
            if (playerSprite.Bounds.IntersectsWith(chaserSprite.Bounds))
            {
                HandlePlayerHit();

                // Optional: Pushback or reset chaser to give player a chance
                chaser.Position = new PointF(this.ClientSize.Width - 50, chaser.Position.Y);
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
