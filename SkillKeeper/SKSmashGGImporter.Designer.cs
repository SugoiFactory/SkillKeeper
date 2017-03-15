namespace SkillKeeper
{
    partial class SKSmashGGImporter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SKSmashGGImporter));
            this.importPlayerList = new System.Windows.Forms.DataGridView();
            this.importPlayerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.importButton = new System.Windows.Forms.Button();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sKLinkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // importPlayerList
            // 
            this.importPlayerList.AllowUserToAddRows = false;
            this.importPlayerList.AllowUserToDeleteRows = false;
            this.importPlayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importPlayerList.AutoGenerateColumns = false;
            this.importPlayerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.importPlayerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.sKLinkDataGridViewTextBoxColumn});
            this.importPlayerList.DataSource = this.importPlayerBindingSource;
            this.importPlayerList.Location = new System.Drawing.Point(12, 12);
            this.importPlayerList.Name = "importPlayerList";
            this.importPlayerList.Size = new System.Drawing.Size(493, 326);
            this.importPlayerList.TabIndex = 0;
            this.importPlayerList.CurrentCellDirtyStateChanged += new System.EventHandler(this.importPlayerList_CurrentCellDirtyStateChanged);
            // 
            // importPlayerBindingSource
            // 
            this.importPlayerBindingSource.DataSource = typeof(ImportPlayer);
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Location = new System.Drawing.Point(430, 364);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 1;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // sKLinkDataGridViewTextBoxColumn
            // 
            this.sKLinkDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sKLinkDataGridViewTextBoxColumn.DataPropertyName = "SKLink";
            this.sKLinkDataGridViewTextBoxColumn.HeaderText = "Link to Player";
            this.sKLinkDataGridViewTextBoxColumn.Name = "sKLinkDataGridViewTextBoxColumn";
            this.sKLinkDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sKLinkDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sKLinkDataGridViewTextBoxColumn.Width = 96;
            // 
            // SKSmashGGImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 399);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.importPlayerList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SKSmashGGImporter";
            this.Text = "Double Check Player Names!";
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView importPlayerList;
        private System.Windows.Forms.BindingSource importPlayerBindingSource;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn sKLinkDataGridViewTextBoxColumn;
    }
}