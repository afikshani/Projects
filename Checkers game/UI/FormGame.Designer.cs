namespace UI
{
    partial class FormGame
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
            player1Label = new System.Windows.Forms.Label();
            player2Label = new System.Windows.Forms.Label();
            turnLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // player1Label
            // 
            player1Label.AutoSize = true;
            player1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            player1Label.Location = new System.Drawing.Point(123, 38);
            player1Label.Name = "player1Label";
            player1Label.Size = new System.Drawing.Size(0, 31);
            player1Label.TabIndex = 0;
            // 
            // player2Label
            // 
            player2Label.AutoSize = true;
            player2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            player2Label.Location = new System.Drawing.Point(480, 38);
            player2Label.Name = "player2Label";
            player2Label.Size = new System.Drawing.Size(0, 31);
            player2Label.TabIndex = 1;
            // 
            // turnLabel
            // 
            turnLabel.AutoSize = true;
            turnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            turnLabel.Location = new System.Drawing.Point(0, 0);
            turnLabel.Name = "turnLabel";
            turnLabel.Size = new System.Drawing.Size(0, 37);
            turnLabel.TabIndex = 2;
            // 
            // FormGame
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(748, 660);
            Controls.Add(turnLabel);
            Controls.Add(player2Label);
            Controls.Add(player1Label);
            Name = "FormGame";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Damka";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private static System.Windows.Forms.Label player1Label;
        private static System.Windows.Forms.Label player2Label;
        private static System.Windows.Forms.Label turnLabel;
    }
}