namespace ManipulateCubes
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
            this.SuspendLayout();
            // 
            // editButton
            // 
            this.editButton.AutoSize = true;
            this.editButton.Location = new System.Drawing.Point(15, 59);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(43, 17);
            this.editButton.TabIndex = 92;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
             // 
            // newName
            // 
            this.newName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newName.Location = new System.Drawing.Point(77, 36);
            this.newName.Name = "newName";
            this.newName.Size = new System.Drawing.Size(448, 20);
            this.newName.TabIndex = 90;
            // 
            // newButton
            // 
            this.newButton.AutoSize = true;
            this.newButton.Checked = true;
            this.newButton.Location = new System.Drawing.Point(15, 36);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(47, 17);
            this.newButton.TabIndex = 91;
            this.newButton.TabStop = true;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(86, 13);
            this.titleLabel.TabIndex = 87;
            this.titleLabel.Text = "Tectonics Model";
            // 
            // selector
            // 
            this.selector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selector.FormattingEnabled = true;
            this.selector.Location = new System.Drawing.Point(77, 62);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(448, 21);
            this.selector.TabIndex = 89;
            this.selector.Visible = false;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel5.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(94, 18);
            this.flowLayoutPanel5.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel5.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(431, 2);
            this.flowLayoutPanel5.TabIndex = 88;
            // 
            // NewEditSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.newName);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.selector);
            this.Controls.Add(this.flowLayoutPanel5);
            this.Name = "NewEditSelector";
            this.Size = new System.Drawing.Size(542, 95);
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
    }
}
