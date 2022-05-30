
namespace JsonTests
{
    partial class LevelTrackBar
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
            this.MinLabel = new System.Windows.Forms.Label();
            this.MaxLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.ValueSelector = new System.Windows.Forms.NumericUpDown();
            this.ValueLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // MinLabel
            // 
            this.MinLabel.AutoSize = true;
            this.MinLabel.Location = new System.Drawing.Point(12, 11);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Size = new System.Drawing.Size(35, 13);
            this.MinLabel.TabIndex = 0;
            this.MinLabel.Text = "label1";
            // 
            // MaxLabel
            // 
            this.MaxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxLabel.AutoSize = true;
            this.MaxLabel.Location = new System.Drawing.Point(456, 11);
            this.MaxLabel.Name = "MaxLabel";
            this.MaxLabel.Size = new System.Drawing.Size(35, 13);
            this.MaxLabel.TabIndex = 1;
            this.MaxLabel.Text = "label2";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.BackColor = System.Drawing.Color.White;
            this.trackBar1.Location = new System.Drawing.Point(13, 27);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(478, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // ValueSelector
            // 
            this.ValueSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueSelector.Location = new System.Drawing.Point(497, 27);
            this.ValueSelector.Name = "ValueSelector";
            this.ValueSelector.Size = new System.Drawing.Size(82, 20);
            this.ValueSelector.TabIndex = 3;
            this.ValueSelector.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ValueSelector.ValueChanged += new System.EventHandler(this.ValueSelector_ValueChanged);
            this.ValueSelector.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueSelector_KeyUp);
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(12, 59);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(35, 13);
            this.ValueLabel.TabIndex = 4;
            this.ValueLabel.Text = "label3";
            // 
            // LevelTrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.ValueSelector);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.MaxLabel);
            this.Controls.Add(this.MinLabel);
            this.Name = "LevelTrackBar";
            this.Size = new System.Drawing.Size(599, 85);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MinLabel;
        private System.Windows.Forms.Label MaxLabel;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.NumericUpDown ValueSelector;
        private System.Windows.Forms.Label ValueLabel;
    }
}
