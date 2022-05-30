namespace ManipulateCubes
{
    partial class DatumDropTarget
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
            this.dropTarget1 = new Slb.Ocean.Petrel.UI.DropTarget();
            this.SuspendLayout();
            // 
            // presentationBox1
            // 
            this.presentationBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presentationBox1.Location = new System.Drawing.Point(27, 2);
            this.presentationBox1.MaximumSize = new System.Drawing.Size(1000, 50);
            this.presentationBox1.MinimumSize = new System.Drawing.Size(50, 25);
            this.presentationBox1.Name = "presentationBox1";
            this.presentationBox1.Size = new System.Drawing.Size(289, 25);
            this.presentationBox1.TabIndex = 3;
            // 
            // dropTarget1
            // 
            this.dropTarget1.AllowDrop = true;
            this.dropTarget1.Location = new System.Drawing.Point(0, 3);
            this.dropTarget1.Name = "dropTarget1";
            this.dropTarget1.Size = new System.Drawing.Size(25, 25);
            this.dropTarget1.TabIndex = 2;
            // 
            // DatumDropTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.presentationBox1);
            this.Controls.Add(this.dropTarget1);
            this.Name = "DatumDropTarget";
            this.Size = new System.Drawing.Size(316, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Slb.Ocean.Petrel.UI.Controls.PresentationBox presentationBox1;
        private Slb.Ocean.Petrel.UI.DropTarget dropTarget1;
    }
}
