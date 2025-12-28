namespace SemiFinalGame
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Label lblscore;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            tiles2 = new PictureBox();
            box2 = new PictureBox();
            box1 = new PictureBox();
            tiles1 = new PictureBox();
            lblhighscore = new Label();
            gameTimer = new System.Windows.Forms.Timer(components);
            playerdown = new PictureBox();
            lblscore = new Label();
            ((System.ComponentModel.ISupportInitialize)tiles2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)box2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)box1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tiles1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerdown).BeginInit();
            SuspendLayout();
            // 
            // tiles2
            // 
            tiles2.Image = Properties.Resources.platform_tilesblue;
            tiles2.Location = new Point(-1, 544);
            tiles2.Name = "tiles2";
            tiles2.Size = new Size(1014, 69);
            tiles2.SizeMode = PictureBoxSizeMode.StretchImage;
            tiles2.TabIndex = 1;
            tiles2.TabStop = false;
            // 
            // box2
            // 
            box2.Image = Properties.Resources.box;
            box2.Location = new Point(632, 393);
            box2.Name = "box2";
            box2.Size = new Size(76, 154);
            box2.TabIndex = 4;
            box2.TabStop = false;
            box2.Tag = "obstacle";
            // 
            // box1
            // 
            box1.Image = Properties.Resources.box;
            box1.Location = new Point(271, 64);
            box1.Name = "box1";
            box1.Size = new Size(77, 157);
            box1.TabIndex = 3;
            box1.TabStop = false;
            box1.Tag = "obstacle";
            // 
            // tiles1
            // 
            tiles1.Image = Properties.Resources.platform_tilesblue;
            tiles1.Location = new Point(-1, 0);
            tiles1.Name = "tiles1";
            tiles1.Size = new Size(1014, 75);
            tiles1.SizeMode = PictureBoxSizeMode.StretchImage;
            tiles1.TabIndex = 6;
            tiles1.TabStop = false;
            // 
            // lblhighscore
            // 
            lblhighscore.AutoSize = true;
            lblhighscore.BackColor = Color.Transparent;
            lblhighscore.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblhighscore.ForeColor = Color.White;
            lblhighscore.Location = new Point(-1, 549);
            lblhighscore.Name = "lblhighscore";
            lblhighscore.Size = new Size(230, 45);
            lblhighscore.TabIndex = 8;
            lblhighscore.Text = "High Score: 0";
            // 
            // gameTimer
            // 
            gameTimer.Enabled = true;
            gameTimer.Interval = 20;
            //gameTimer.Tick += GameTimerEvent;
            // 
            // lblscore
            // 
            lblscore.AutoSize = true;
            lblscore.BackColor = Color.Transparent;
            lblscore.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblscore.ForeColor = Color.White;
            lblscore.Location = new Point(23, 9);
            lblscore.Name = "lblscore";
            lblscore.Size = new Size(144, 45);
            lblscore.TabIndex = 7;
            lblscore.Text = "Score: 0";
            // 
            // playerdown
            // 
            playerdown.BackColor = Color.Transparent;
            playerdown.BackgroundImageLayout = ImageLayout.Stretch;
            playerdown.Image = Properties.Resources.run_down0;
            playerdown.Location = new Point(23, 230);
            playerdown.Name = "playerdown";
            playerdown.Size = new Size(73, 103);
            playerdown.SizeMode = PictureBoxSizeMode.StretchImage;
            playerdown.TabIndex = 5;
            playerdown.TabStop = false;
            playerdown.Tag = "obstacle";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(968, 603);
            Controls.Add(lblhighscore);
            Controls.Add(lblscore);
            Controls.Add(tiles1);
            Controls.Add(playerdown);
            Controls.Add(box2);
            Controls.Add(box1);
            Controls.Add(tiles2);
            Name = "GameForm";
            Text = "Game Form";
            //Load += GameForm_Load;
            //KeyUp += KeyIsUp;
            ((System.ComponentModel.ISupportInitialize)tiles2).EndInit();
            ((System.ComponentModel.ISupportInitialize)box2).EndInit();
            ((System.ComponentModel.ISupportInitialize)box1).EndInit();
            ((System.ComponentModel.ISupportInitialize)tiles1).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerdown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox tiles2;
        private PictureBox box2;
        private PictureBox box1;
        private PictureBox tiles1;
        private Label lblhighscore;
        private Label lblscore;
        private System.Windows.Forms.Timer gameTimer;
        private PictureBox playerdown;
    }
}
