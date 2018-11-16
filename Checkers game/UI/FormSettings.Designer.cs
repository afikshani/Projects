namespace UI
{
    partial class FormSettings
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
            this.sizeOfBoard = new System.Windows.Forms.Label();
            this.sixOnSix = new System.Windows.Forms.RadioButton();
            this.tenOnTen = new System.Windows.Forms.RadioButton();
            this.eightOnEight = new System.Windows.Forms.RadioButton();
            this.Player1 = new System.Windows.Forms.Label();
            this.Players = new System.Windows.Forms.Label();
            this.isPlayerTwoPlays = new System.Windows.Forms.CheckBox();
            this.textBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.textBoxPlayer2 = new System.Windows.Forms.TextBox();
            this.DoneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sizeOfBoard
            // 
            this.sizeOfBoard.AutoSize = true;
            this.sizeOfBoard.Location = new System.Drawing.Point(21, 18);
            this.sizeOfBoard.Name = "sizeOfBoard";
            this.sizeOfBoard.Size = new System.Drawing.Size(123, 25);
            this.sizeOfBoard.TabIndex = 0;
            this.sizeOfBoard.Text = "Board Size:";
            // 
            // sixOnSix
            // 
            this.sixOnSix.AutoSize = true;
            this.sixOnSix.Location = new System.Drawing.Point(62, 60);
            this.sixOnSix.Name = "sixOnSix";
            this.sixOnSix.Size = new System.Drawing.Size(90, 29);
            this.sixOnSix.TabIndex = 1;
            this.sixOnSix.TabStop = true;
            this.sixOnSix.Text = "6 x 6";
            this.sixOnSix.UseVisualStyleBackColor = true;
            this.sixOnSix.CheckedChanged += new System.EventHandler(this.boardSizeButton_OnChecked);
            // 
            // tenOnTen
            // 
            this.tenOnTen.AutoSize = true;
            this.tenOnTen.Location = new System.Drawing.Point(285, 60);
            this.tenOnTen.Name = "tenOnTen";
            this.tenOnTen.Size = new System.Drawing.Size(114, 29);
            this.tenOnTen.TabIndex = 3;
            this.tenOnTen.TabStop = true;
            this.tenOnTen.Text = "10 x 10";
            this.tenOnTen.UseVisualStyleBackColor = true;
            this.tenOnTen.CheckedChanged += new System.EventHandler(this.boardSizeButton_OnChecked);
            // 
            // eightOnEight
            // 
            this.eightOnEight.AutoSize = true;
            this.eightOnEight.Location = new System.Drawing.Point(176, 60);
            this.eightOnEight.Name = "eightOnEight";
            this.eightOnEight.Size = new System.Drawing.Size(90, 29);
            this.eightOnEight.TabIndex = 2;
            this.eightOnEight.TabStop = true;
            this.eightOnEight.Text = "8 x 8";
            this.eightOnEight.UseVisualStyleBackColor = true;
            this.eightOnEight.CheckedChanged += new System.EventHandler(this.boardSizeButton_OnChecked);
            // 
            // Player1
            // 
            this.Player1.AutoSize = true;
            this.Player1.Location = new System.Drawing.Point(39, 153);
            this.Player1.Name = "Player1";
            this.Player1.Size = new System.Drawing.Size(97, 25);
            this.Player1.TabIndex = 5;
            this.Player1.Text = "Player 1:";
            // 
            // Players
            // 
            this.Players.AutoSize = true;
            this.Players.Location = new System.Drawing.Point(21, 112);
            this.Players.Name = "Players";
            this.Players.Size = new System.Drawing.Size(90, 25);
            this.Players.TabIndex = 6;
            this.Players.Text = "Players:";
            // 
            // isPlayerTwoPlays
            // 
            this.isPlayerTwoPlays.AutoSize = true;
            this.isPlayerTwoPlays.Location = new System.Drawing.Point(44, 208);
            this.isPlayerTwoPlays.Name = "isPlayerTwoPlays";
            this.isPlayerTwoPlays.Size = new System.Drawing.Size(129, 29);
            this.isPlayerTwoPlays.TabIndex = 5;
            this.isPlayerTwoPlays.Text = "Player 2:";
            this.isPlayerTwoPlays.UseVisualStyleBackColor = true;
            this.isPlayerTwoPlays.CheckedChanged += new System.EventHandler(this.isPlayerTwoPlays_OnClick);
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(205, 148);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(216, 31);
            this.textBoxPlayer1.TabIndex = 4;
            this.textBoxPlayer1.TextChanged += new System.EventHandler(this.playerTextBox_TextChanged);
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Enabled = false;
            this.textBoxPlayer2.Location = new System.Drawing.Point(205, 207);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(216, 31);
            this.textBoxPlayer2.TabIndex = 9;
            this.textBoxPlayer2.Text = "[Computer]";
            this.textBoxPlayer2.TextChanged += new System.EventHandler(this.playerTextBox_TextChanged);
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(267, 268);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(145, 48);
            this.DoneButton.TabIndex = 6;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.doneButton_OnClick);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 342);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.textBoxPlayer1);
            this.Controls.Add(this.isPlayerTwoPlays);
            this.Controls.Add(this.Players);
            this.Controls.Add(this.Player1);
            this.Controls.Add(this.eightOnEight);
            this.Controls.Add(this.tenOnTen);
            this.Controls.Add(this.sixOnSix);
            this.Controls.Add(this.sizeOfBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(468, 413);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(468, 413);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sizeOfBoard;
        private System.Windows.Forms.RadioButton sixOnSix;
        private System.Windows.Forms.RadioButton tenOnTen;
        private System.Windows.Forms.RadioButton eightOnEight;
        private System.Windows.Forms.Label Player1;
        private System.Windows.Forms.Label Players;
        private System.Windows.Forms.CheckBox isPlayerTwoPlays;
        private System.Windows.Forms.TextBox textBoxPlayer1;
        private System.Windows.Forms.TextBox textBoxPlayer2;
        private System.Windows.Forms.Button DoneButton;
    }
}