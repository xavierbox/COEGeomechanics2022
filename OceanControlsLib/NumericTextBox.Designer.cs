
namespace OceanControlsLib
{
    partial class NumericTextBox
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
            this.presentationBox1 = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.UnitsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // presentationBox1
            // 
            this.presentationBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.presentationBox1.Location = new System.Drawing.Point(2, 1);
            this.presentationBox1.Name = "presentationBox1";
            this.presentationBox1.Size = new System.Drawing.Size(81, 22);
            this.presentationBox1.TabIndex = 115;
            // 
            // UnitsLabel
            // 
            this.UnitsLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.UnitsLabel.AutoSize = true;
            this.UnitsLabel.Location = new System.Drawing.Point(88, 5);
            this.UnitsLabel.Name = "UnitsLabel";
            this.UnitsLabel.Size = new System.Drawing.Size(35, 13);
            this.UnitsLabel.TabIndex = 116;
            this.UnitsLabel.Text = "label1";
            // 
            // NumericTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UnitsLabel);
            this.Controls.Add(this.presentationBox1);
            this.Name = "NumericTextBox";
            this.Size = new System.Drawing.Size(128, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Slb.Ocean.Petrel.UI.Controls.PresentationBox presentationBox1;
        private System.Windows.Forms.Label UnitsLabel;
    }
}
