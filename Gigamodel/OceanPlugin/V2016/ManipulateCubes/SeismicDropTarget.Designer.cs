namespace ManipulateCubes
{
    partial class SeismicDropTarget
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
            this.dropTarget1 = new Slb.Ocean.Petrel.UI.DropTarget();
            this.presentationBox1 = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.SuspendLayout();
            // 
            // dropTarget1
            // 
            this.dropTarget1.AllowDrop = true;
            this.dropTarget1.Location = new System.Drawing.Point(0, 3);
            this.dropTarget1.Name = "dropTarget1";
            this.dropTarget1.Size = new System.Drawing.Size(25, 25);
            this.dropTarget1.TabIndex = 0;
            // 
            // presentationBox1
            // 
            this.presentationBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presentationBox1.Location = new System.Drawing.Point(29, 4);
            this.presentationBox1.Name = "presentationBox1";
            this.presentationBox1.Size = new System.Drawing.Size(356, 22);
            this.presentationBox1.TabIndex = 1;
             // 
            // SeismicDropTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.presentationBox1);
            this.Controls.Add(this.dropTarget1);
            this.Name = "SeismicDropTarget";
            this.Size = new System.Drawing.Size(385, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Slb.Ocean.Petrel.UI.DropTarget dropTarget1;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox presentationBox1;
    }
}
