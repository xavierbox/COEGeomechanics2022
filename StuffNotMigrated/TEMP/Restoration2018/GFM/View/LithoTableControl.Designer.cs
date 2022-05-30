namespace Restoration
{
    partial class LithoTableControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LithoTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basementBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.Basement = new System.Windows.Forms.Label();
            this.lithoTableMoveUp = new System.Windows.Forms.Button();
            this.lithoTableDelete = new System.Windows.Forms.Button();
            this.lithoTableMoveDown = new System.Windows.Forms.Button();
            this.addErosionalStep = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LithoTable)).BeginInit();
            this.SuspendLayout();
            // 
            // LithoTable
            // 
            this.LithoTable.AllowUserToAddRows = false;
            this.LithoTable.AllowUserToDeleteRows = false;
            this.LithoTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LithoTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LithoTable.BackgroundColor = System.Drawing.Color.White;
            this.LithoTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LithoTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LithoTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column8});
            this.LithoTable.Location = new System.Drawing.Point(3, 28);
            this.LithoTable.MinimumSize = new System.Drawing.Size(533, 150);
            this.LithoTable.Name = "LithoTable";
            this.LithoTable.RowHeadersVisible = false;
            this.LithoTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LithoTable.Size = new System.Drawing.Size(671, 194);
            this.LithoTable.TabIndex = 45;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 200F;
            this.Column1.HeaderText = "Step";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Eo";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Ef";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "ELambda";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "Sub-layers";
            this.Column5.Name = "Column5";
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column8.FillWeight = 200F;
            this.Column8.HeaderText = "Function";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // basementBox
            // 
            this.basementBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.basementBox.Location = new System.Drawing.Point(60, 228);
            this.basementBox.Name = "basementBox";
            this.basementBox.Size = new System.Drawing.Size(616, 22);
            this.basementBox.TabIndex = 51;
            // 
            // Basement
            // 
            this.Basement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Basement.AutoSize = true;
            this.Basement.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Basement.Location = new System.Drawing.Point(0, 233);
            this.Basement.Name = "Basement";
            this.Basement.Size = new System.Drawing.Size(54, 13);
            this.Basement.TabIndex = 50;
            this.Basement.Text = "Basement";
            // 
            // lithoTableMoveUp
            // 
            this.lithoTableMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lithoTableMoveUp.Location = new System.Drawing.Point(596, 1);
            this.lithoTableMoveUp.Name = "lithoTableMoveUp";
            this.lithoTableMoveUp.Size = new System.Drawing.Size(26, 26);
            this.lithoTableMoveUp.TabIndex = 46;
            this.lithoTableMoveUp.UseVisualStyleBackColor = true;
            // 
            // lithoTableDelete
            // 
            this.lithoTableDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lithoTableDelete.Location = new System.Drawing.Point(623, 1);
            this.lithoTableDelete.Name = "lithoTableDelete";
            this.lithoTableDelete.Size = new System.Drawing.Size(26, 26);
            this.lithoTableDelete.TabIndex = 48;
            this.lithoTableDelete.UseVisualStyleBackColor = true;
            // 
            // lithoTableMoveDown
            // 
            this.lithoTableMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lithoTableMoveDown.Location = new System.Drawing.Point(569, 1);
            this.lithoTableMoveDown.Name = "lithoTableMoveDown";
            this.lithoTableMoveDown.Size = new System.Drawing.Size(26, 26);
            this.lithoTableMoveDown.TabIndex = 47;
            this.lithoTableMoveDown.UseVisualStyleBackColor = true;
            // 
            // addErosionalStep
            // 
            this.addErosionalStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addErosionalStep.Image = global::Restoration.Properties.Resources.plus16;
            this.addErosionalStep.Location = new System.Drawing.Point(650, 1);
            this.addErosionalStep.Name = "addErosionalStep";
            this.addErosionalStep.Size = new System.Drawing.Size(26, 26);
            this.addErosionalStep.TabIndex = 49;
            this.addErosionalStep.UseVisualStyleBackColor = true;
            // 
            // LithoTableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.LithoTable);
            this.Controls.Add(this.basementBox);
            this.Controls.Add(this.Basement);
            this.Controls.Add(this.lithoTableMoveUp);
            this.Controls.Add(this.addErosionalStep);
            this.Controls.Add(this.lithoTableDelete);
            this.Controls.Add(this.lithoTableMoveDown);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LithoTableControl";
            this.Size = new System.Drawing.Size(679, 263);
            this.Load += new System.EventHandler(this.LithoTableControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LithoTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView LithoTable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox basementBox;
        private System.Windows.Forms.Label Basement;
        private System.Windows.Forms.Button lithoTableMoveUp;
        private System.Windows.Forms.Button addErosionalStep;
        private System.Windows.Forms.Button lithoTableDelete;
        private System.Windows.Forms.Button lithoTableMoveDown;
    }
}
