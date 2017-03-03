namespace SkillKeeper
{
    partial class SKSmashGGEventSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SKSmashGGEventSelector));
            this.eventSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bracketSelector = new System.Windows.Forms.ComboBox();
            this.importFullBracket = new System.Windows.Forms.CheckBox();
            this.phaseSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // eventSelector
            // 
            this.eventSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventSelector.FormattingEnabled = true;
            this.eventSelector.Location = new System.Drawing.Point(12, 31);
            this.eventSelector.Name = "eventSelector";
            this.eventSelector.Size = new System.Drawing.Size(383, 21);
            this.eventSelector.TabIndex = 0;
            this.eventSelector.SelectedIndexChanged += new System.EventHandler(this.eventSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Event";
            // 
            // bracketSelector
            // 
            this.bracketSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bracketSelector.Enabled = false;
            this.bracketSelector.FormattingEnabled = true;
            this.bracketSelector.Location = new System.Drawing.Point(12, 152);
            this.bracketSelector.Name = "bracketSelector";
            this.bracketSelector.Size = new System.Drawing.Size(383, 21);
            this.bracketSelector.TabIndex = 2;
            // 
            // importFullBracket
            // 
            this.importFullBracket.AutoSize = true;
            this.importFullBracket.Checked = true;
            this.importFullBracket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.importFullBracket.Location = new System.Drawing.Point(12, 59);
            this.importFullBracket.Name = "importFullBracket";
            this.importFullBracket.Size = new System.Drawing.Size(130, 17);
            this.importFullBracket.TabIndex = 3;
            this.importFullBracket.Text = "Import entire Bracket?";
            this.importFullBracket.UseVisualStyleBackColor = true;
            this.importFullBracket.CheckedChanged += new System.EventHandler(this.importFullBracket_CheckedChanged);
            // 
            // phaseSelector
            // 
            this.phaseSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phaseSelector.Enabled = false;
            this.phaseSelector.FormattingEnabled = true;
            this.phaseSelector.Location = new System.Drawing.Point(12, 106);
            this.phaseSelector.Name = "phaseSelector";
            this.phaseSelector.Size = new System.Drawing.Size(383, 21);
            this.phaseSelector.TabIndex = 4;
            this.phaseSelector.SelectedIndexChanged += new System.EventHandler(this.phaseSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Phase";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Groups";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(318, 203);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 7;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // SKSmashGGEventSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 238);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.phaseSelector);
            this.Controls.Add(this.importFullBracket);
            this.Controls.Add(this.bracketSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eventSelector);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SKSmashGGEventSelector";
            this.Text = "Select an Event";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox eventSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox bracketSelector;
        private System.Windows.Forms.CheckBox importFullBracket;
        private System.Windows.Forms.ComboBox phaseSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button submitButton;
    }
}