namespace NFLPredictions
{
    partial class TeamDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeamDetails));
            this.TeamGrid = new System.Windows.Forms.DataGridView();
            this.Week = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Opponent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeamName = new System.Windows.Forms.Label();
            this.SaveChanges = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TeamGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // TeamGrid
            // 
            this.TeamGrid.AllowUserToAddRows = false;
            this.TeamGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TeamGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Week,
            this.Opponent,
            this.Location});
            this.TeamGrid.Location = new System.Drawing.Point(12, 54);
            this.TeamGrid.Name = "TeamGrid";
            this.TeamGrid.ReadOnly = true;
            this.TeamGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TeamGrid.Size = new System.Drawing.Size(317, 378);
            this.TeamGrid.TabIndex = 0;
            this.TeamGrid.SelectionChanged += new System.EventHandler(this.TeamGrid_SelectionChanged);
            // 
            // Week
            // 
            this.Week.HeaderText = "Week";
            this.Week.Name = "Week";
            this.Week.ReadOnly = true;
            this.Week.Width = 50;
            // 
            // Opponent
            // 
            this.Opponent.HeaderText = "Opponent";
            this.Opponent.Name = "Opponent";
            this.Opponent.ReadOnly = true;
            this.Opponent.Width = 150;
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            this.Location.Width = 75;
            // 
            // TeamName
            // 
            this.TeamName.AutoSize = true;
            this.TeamName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamName.Location = new System.Drawing.Point(12, 15);
            this.TeamName.Name = "TeamName";
            this.TeamName.Size = new System.Drawing.Size(124, 24);
            this.TeamName.TabIndex = 1;
            this.TeamName.Text = "Team Name";
            // 
            // SaveChanges
            // 
            this.SaveChanges.Location = new System.Drawing.Point(335, 387);
            this.SaveChanges.Name = "SaveChanges";
            this.SaveChanges.Size = new System.Drawing.Size(130, 45);
            this.SaveChanges.TabIndex = 2;
            this.SaveChanges.Text = "Save Changes";
            this.SaveChanges.UseVisualStyleBackColor = true;
            this.SaveChanges.Click += new System.EventHandler(this.SaveChanges_Click);
            // 
            // Record
            // 
            this.Record.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Record.Location = new System.Drawing.Point(249, 15);
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(80, 24);
            this.Record.TabIndex = 3;
            this.Record.Text = "Record";
            this.Record.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TeamDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 444);
            this.Controls.Add(this.Record);
            this.Controls.Add(this.SaveChanges);
            this.Controls.Add(this.TeamName);
            this.Controls.Add(this.TeamGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TeamDetails";
            this.Text = "Team Details";
            this.Load += new System.EventHandler(this.TeamDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TeamGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView TeamGrid;
        private System.Windows.Forms.Label TeamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Week;
        private System.Windows.Forms.DataGridViewTextBoxColumn Opponent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.Button SaveChanges;
        private System.Windows.Forms.Label Record;
    }
}