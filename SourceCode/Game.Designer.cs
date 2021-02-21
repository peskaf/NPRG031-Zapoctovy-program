namespace MazeRunner
{
    partial class MazeRunner
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MazeRunner));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GameOver = new System.Windows.Forms.Label();
            this.NewGame = new System.Windows.Forms.Label();
            this.Exit = new System.Windows.Forms.Label();
            this.Menu = new System.Windows.Forms.PictureBox();
            this.Victory = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Menu)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GameOver
            // 
            this.GameOver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameOver.AutoSize = true;
            this.GameOver.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GameOver.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GameOver.ForeColor = System.Drawing.Color.Red;
            this.GameOver.Location = new System.Drawing.Point(320, 306);
            this.GameOver.Name = "GameOver";
            this.GameOver.Size = new System.Drawing.Size(543, 117);
            this.GameOver.TabIndex = 0;
            this.GameOver.Text = "GAME OVER";
            this.GameOver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NewGame
            // 
            this.NewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewGame.AutoSize = true;
            this.NewGame.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.NewGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NewGame.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NewGame.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.NewGame.Location = new System.Drawing.Point(334, 155);
            this.NewGame.Name = "NewGame";
            this.NewGame.Size = new System.Drawing.Size(519, 117);
            this.NewGame.TabIndex = 1;
            this.NewGame.Text = "NEW GAME";
            this.NewGame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NewGame.Click += new System.EventHandler(this.NewGame_Click);
            // 
            // Exit
            // 
            this.Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Exit.AutoSize = true;
            this.Exit.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Exit.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Exit.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Exit.Location = new System.Drawing.Point(476, 451);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(224, 117);
            this.Exit.TabIndex = 2;
            this.Exit.Text = "EXIT";
            this.Exit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Menu.Location = new System.Drawing.Point(290, 118);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(602, 498);
            this.Menu.TabIndex = 3;
            this.Menu.TabStop = false;
            // 
            // Victory
            // 
            this.Victory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Victory.AutoSize = true;
            this.Victory.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Victory.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Victory.ForeColor = System.Drawing.Color.Lime;
            this.Victory.Location = new System.Drawing.Point(381, 306);
            this.Victory.Name = "Victory";
            this.Victory.Size = new System.Drawing.Size(427, 117);
            this.Victory.TabIndex = 4;
            this.Victory.Text = "VICTORY!";
            this.Victory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MazeRunner
            // 
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.Victory);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.NewGame);
            this.Controls.Add(this.GameOver);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 800);
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "MazeRunner";
            this.Text = "MazeRunner";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MazeRunner_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MazeRunner_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.Menu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label GameOver;
        private System.Windows.Forms.Label NewGame;
        private System.Windows.Forms.Label Exit;
        private System.Windows.Forms.PictureBox Menu;
        private System.Windows.Forms.Label Victory;
    }
}

