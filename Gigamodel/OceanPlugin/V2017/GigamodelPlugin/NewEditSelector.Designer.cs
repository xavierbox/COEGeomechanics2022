namespace Gigamodel
{
    partial class NewEditSelector
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
            this.editButton = new System.Windows.Forms.RadioButton();
            this.newName = new System.Windows.Forms.TextBox();
            this.newButton = new System.Windows.Forms.RadioButton();
            this.titleLabel = new System.Windows.Forms.Label();
            this.selector = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // editButton
            // 
            this.editButton.AutoSize = true;
            this.editButton.Location = new System.Drawing.Point(21, 60);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(43, 17);
            this.editButton.TabIndex = 98;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            // 
            // newName
            // 
            this.newName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newName.Location = new System.Drawing.Point(83, 37);
            this.newName.Name = "newName";
            this.newName.Size = new System.Drawing.Size(463, 20);
            this.newName.TabIndex = 96;
            // 
            // newButton
            // 
            this.newButton.AutoSize = true;
            this.newButton.Checked = true;
            this.newButton.Location = new System.Drawing.Point(21, 37);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(47, 17);
            this.newButton.TabIndex = 97;
            this.newButton.TabStop = true;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(6, 11);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(86, 13);
            this.titleLabel.TabIndex = 93;
            this.titleLabel.Text = "Tectonics Model";
            // 
            // selector
            // 
            this.selector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selector.FormattingEnabled = true;
            this.selector.Location = new System.Drawing.Point(83, 63);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(435, 21);
            this.selector.TabIndex = 95;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel5.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(100, 19);
            this.flowLayoutPanel5.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel5.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(456, 2);
            this.flowLayoutPanel5.TabIndex = 94;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Location = new System.Drawing.Point(523, 62);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(22, 23);
            this.deleteButton.TabIndex = 99;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButtonClicked);
            // 
            // NewEditSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.newName);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.selector);
            this.Controls.Add(this.flowLayoutPanel5);
            this.Name = "NewEditSelector";
            this.Size = new System.Drawing.Size(546, 95);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton editButton;
        private System.Windows.Forms.TextBox newName;
        private System.Windows.Forms.RadioButton newButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ComboBox selector;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Button deleteButton;
    }
}
