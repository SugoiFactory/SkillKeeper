namespace SkillKeeper
{
    partial class SKChallongeLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SKChallongeLoader));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.subDomainBox = new System.Windows.Forms.TextBox();
            this.apiKeyBox = new System.Windows.Forms.TextBox();
            this.authButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tournamentNameBox = new System.Windows.Forms.TextBox();
            this.tournamentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "API Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Subdomain";
            // 
            // subDomainBox
            // 
            this.subDomainBox.Location = new System.Drawing.Point(78, 44);
            this.subDomainBox.Name = "subDomainBox";
            this.subDomainBox.Size = new System.Drawing.Size(415, 20);
            this.subDomainBox.TabIndex = 2;
            // 
            // apiKeyBox
            // 
            this.apiKeyBox.Location = new System.Drawing.Point(78, 18);
            this.apiKeyBox.Name = "apiKeyBox";
            this.apiKeyBox.Size = new System.Drawing.Size(415, 20);
            this.apiKeyBox.TabIndex = 1;
            this.apiKeyBox.TextChanged += new System.EventHandler(this.apiKeyBox_TextChanged);
            // 
            // authButton
            // 
            this.authButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.authButton.Enabled = false;
            this.authButton.Location = new System.Drawing.Point(321, 96);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(83, 23);
            this.authButton.TabIndex = 4;
            this.authButton.Text = "Authenticate";
            this.authButton.UseVisualStyleBackColor = true;
            this.authButton.Click += new System.EventHandler(this.authButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(410, 96);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(83, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tournamentNameBox
            // 
            this.tournamentNameBox.Location = new System.Drawing.Point(78, 70);
            this.tournamentNameBox.Name = "tournamentNameBox";
            this.tournamentNameBox.Size = new System.Drawing.Size(415, 20);
            this.tournamentNameBox.TabIndex = 3;
            // 
            // tournamentLabel
            // 
            this.tournamentLabel.AutoSize = true;
            this.tournamentLabel.Location = new System.Drawing.Point(12, 73);
            this.tournamentLabel.Name = "tournamentLabel";
            this.tournamentLabel.Size = new System.Drawing.Size(64, 13);
            this.tournamentLabel.TabIndex = 6;
            this.tournamentLabel.Text = "Tournament";
            // 
            // SKChallongeLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 134);
            this.Controls.Add(this.tournamentLabel);
            this.Controls.Add(this.tournamentNameBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.authButton);
            this.Controls.Add(this.apiKeyBox);
            this.Controls.Add(this.subDomainBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SKChallongeLoader";
            this.Text = "Import Challonge Event";
            this.Load += new System.EventHandler(this.SKChallongeLoader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox subDomainBox;
        private System.Windows.Forms.TextBox apiKeyBox;
        private System.Windows.Forms.Button authButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox tournamentNameBox;
        private System.Windows.Forms.Label tournamentLabel;
    }
}