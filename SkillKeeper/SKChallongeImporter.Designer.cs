namespace SkillKeeper
{
    partial class SKChallongeImporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SKChallongeImporter));
            this.eventSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.importPlayerList = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sKLinkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.importPlayerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.importButton = new System.Windows.Forms.Button();
            this.eventDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // eventSelector
            // 
            this.eventSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eventSelector.FormattingEnabled = true;
            this.eventSelector.Location = new System.Drawing.Point(295, 15);
            this.eventSelector.Name = "eventSelector";
            this.eventSelector.Size = new System.Drawing.Size(389, 21);
            this.eventSelector.TabIndex = 0;
            this.eventSelector.SelectedIndexChanged += new System.EventHandler(this.eventSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(254, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Event";
            // 
            // importPlayerList
            // 
            this.importPlayerList.AllowUserToAddRows = false;
            this.importPlayerList.AllowUserToDeleteRows = false;
            this.importPlayerList.AutoGenerateColumns = false;
            this.importPlayerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.importPlayerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.sKLinkDataGridViewTextBoxColumn});
            this.importPlayerList.DataSource = this.importPlayerBindingSource;
            this.importPlayerList.Location = new System.Drawing.Point(15, 42);
            this.importPlayerList.Name = "importPlayerList";
            this.importPlayerList.Size = new System.Drawing.Size(669, 529);
            this.importPlayerList.TabIndex = 2;
            this.importPlayerList.CurrentCellDirtyStateChanged += new System.EventHandler(this.importPlayerList_CurrentCellDirtyStateChanged);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 58;
            // 
            // sKLinkDataGridViewTextBoxColumn
            // 
            this.sKLinkDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sKLinkDataGridViewTextBoxColumn.DataPropertyName = "SKLink";
            this.sKLinkDataGridViewTextBoxColumn.HeaderText = "Link to Player";
            this.sKLinkDataGridViewTextBoxColumn.Name = "sKLinkDataGridViewTextBoxColumn";
            this.sKLinkDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sKLinkDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sKLinkDataGridViewTextBoxColumn.Width = 94;
            // 
            // importPlayerBindingSource
            // 
            this.importPlayerBindingSource.DataSource = typeof(ImportPlayer);
            // 
            // importButton
            // 
            this.importButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.importButton.Location = new System.Drawing.Point(15, 577);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 3;
            this.importButton.Text = "Run Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // eventDatePicker
            // 
            this.eventDatePicker.Location = new System.Drawing.Point(48, 12);
            this.eventDatePicker.Name = "eventDatePicker";
            this.eventDatePicker.Size = new System.Drawing.Size(200, 20);
            this.eventDatePicker.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(96, 577);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SKChallongeImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 612);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.eventDatePicker);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.importPlayerList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eventSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SKChallongeImporter";
            this.Text = "SkillKeeper Challonge Importer";
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importPlayerBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox eventSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView importPlayerList;
        private System.Windows.Forms.BindingSource importPlayerBindingSource;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.DateTimePicker eventDatePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn sKLinkDataGridViewTextBoxColumn;
    }
}