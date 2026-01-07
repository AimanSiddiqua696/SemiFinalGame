namespace SemiFinalGame
{
    partial class MenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            btnStart = new Button();
            btnExit = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.None;
            btnStart.BackColor = SystemColors.GradientInactiveCaption;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Century", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(94, 505);
            btnStart.Margin = new Padding(5, 6, 5, 6);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(304, 109);
            btnStart.TabIndex = 0;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.None;
            btnExit.BackColor = SystemColors.GradientInactiveCaption;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Century", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(527, 505);
            btnExit.Margin = new Padding(5, 6, 5, 6);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(297, 109);
            btnExit.TabIndex = 1;
            btnExit.Text = "EXIT";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Berlin Sans FB Demi", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.GradientInactiveCaption;
            label1.Location = new Point(100, 50);
            label1.Name = "label1";
            label1.Size = new Size(1453, 163);
            label1.TabIndex = 2;
            label1.Text = "EVASION PROTOCOL";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(927, 617);
            Controls.Add(label1);
            Controls.Add(btnExit);
            Controls.Add(btnStart);
            DoubleBuffered = true;
            Margin = new Padding(5, 6, 5, 6);
            Name = "MenuForm";
            Text = "MenuForm";
            Load += MenuForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private Label label1;
    }
}
