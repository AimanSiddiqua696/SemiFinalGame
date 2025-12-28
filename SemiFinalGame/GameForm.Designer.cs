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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            tiles2 = new PictureBox();
            tiles1 = new PictureBox();
            box1 = new PictureBox();
            box2 = new PictureBox();
            playerdown = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)tiles2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tiles1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)box1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)box2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerdown).BeginInit();
            SuspendLayout();
            // 
            // tiles2
            // 
            tiles2.Image = Properties.Resources.platform_tilesblue;
            tiles2.Location = new Point(-1, 544);
            tiles2.Name = "tiles2";
            tiles2.Size = new Size(677, 69);
            tiles2.SizeMode = PictureBoxSizeMode.StretchImage;
            tiles2.TabIndex = 1;
            tiles2.TabStop = false;
            // 
            // tiles1
            // 
            tiles1.Image = Properties.Resources.platform_tilesblue;
            tiles1.Location = new Point(-1, 3);
            tiles1.Name = "tiles1";
            tiles1.Size = new Size(677, 67);
            tiles1.SizeMode = PictureBoxSizeMode.StretchImage;
            tiles1.TabIndex = 2;
            tiles1.TabStop = false;
            // 
            // box1
            // 
            box1.Image = Properties.Resources.box;
            box1.Location = new Point(271, 64);
            box1.Name = "box1";
            box1.Size = new Size(77, 157);
            box1.TabIndex = 3;
            box1.TabStop = false;
            // 
            // box2
            // 
            box2.Image = Properties.Resources.box;
            box2.Location = new Point(426, 395);
            box2.Name = "box2";
            box2.Size = new Size(76, 154);
            box2.TabIndex = 4;
            box2.TabStop = false;
            // 
            // playerdown
            // 
            playerdown.BackColor = Color.Transparent;
            playerdown.Image = Properties.Resources.run_down0;
            playerdown.Location = new Point(23, 199);
            playerdown.Name = "playerdown";
            playerdown.Size = new Size(127, 134);
            playerdown.TabIndex = 5;
            playerdown.TabStop = false;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(650, 603);
            Controls.Add(playerdown);
            Controls.Add(box2);
            Controls.Add(box1);
            Controls.Add(tiles1);
            Controls.Add(tiles2);
            Name = "GameForm";
            Text = "Game Form";
            Load += GameForm_Load;
            ((System.ComponentModel.ISupportInitialize)tiles2).EndInit();
            ((System.ComponentModel.ISupportInitialize)tiles1).EndInit();
            ((System.ComponentModel.ISupportInitialize)box1).EndInit();
            ((System.ComponentModel.ISupportInitialize)box2).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerdown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox tiles2;
        private PictureBox tiles1;
        private PictureBox box1;
        private PictureBox box2;
        private PictureBox playerdown;
    }
}
