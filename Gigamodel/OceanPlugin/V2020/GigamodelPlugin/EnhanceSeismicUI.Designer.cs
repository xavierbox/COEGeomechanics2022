namespace Gigamodel
{
    partial class EnhanceSeismicUI
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
            this.enhanceFactorControl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.seismicDragDrop = new Slb.Ocean.Petrel.UI.DropTarget();
            this.seismicPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cubicSizeControl = new System.Windows.Forms.NumericUpDown();
            this.removeSalt = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.enhanceFactorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cubicSizeControl)).BeginInit();
            this.SuspendLayout();
            // 
            // enhanceFactorControl
            // 
            this.enhanceFactorControl.DecimalPlaces = 2;
            this.enhanceFactorControl.Location = new System.Drawing.Point(23, 106);
            this.enhanceFactorControl.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.enhanceFactorControl.Name = "enhanceFactorControl";
            this.enhanceFactorControl.Size = new System.Drawing.Size(120, 20);
            this.enhanceFactorControl.TabIndex = 0;
            this.enhanceFactorControl.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enhance factor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Seismic cube";
            // 
            // seismicDragDrop
            // 
            this.seismicDragDrop.AllowDrop = true;
            this.seismicDragDrop.Location = new System.Drawing.Point(23, 44);
            this.seismicDragDrop.Name = "seismicDragDrop";
            this.seismicDragDrop.Size = new System.Drawing.Size(26, 23);
            this.seismicDragDrop.TabIndex = 3;
            this.seismicDragDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.seismicDragDrop_DragDrop);
            // 
            // seismicPresentationBox
            // 
            this.seismicPresentationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seismicPresentationBox.Location = new System.Drawing.Point(55, 44);
            this.seismicPresentationBox.Name = "seismicPresentationBox";
            this.seismicPresentationBox.Size = new System.Drawing.Size(483, 22);
            this.seismicPresentationBox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(449, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Cube size";
            // 
            // cubicSizeControl
            // 
            this.cubicSizeControl.DecimalPlaces = 2;
            this.cubicSizeControl.Location = new System.Drawing.Point(222, 103);
            this.cubicSizeControl.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.cubicSizeControl.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.cubicSizeControl.Name = "cubicSizeControl";
            this.cubicSizeControl.Size = new System.Drawing.Size(120, 20);
            this.cubicSizeControl.TabIndex = 6;
            this.cubicSizeControl.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // removeSalt
            // 
            this.removeSalt.AutoSize = true;
            this.removeSalt.Location = new System.Drawing.Point(23, 148);
            this.removeSalt.Name = "removeSalt";
            this.removeSalt.Size = new System.Drawing.Size(80, 17);
            this.removeSalt.TabIndex = 8;
            this.removeSalt.Text = "checkBox1";
            this.removeSalt.UseVisualStyleBackColor = true;
            // 
            // EnhanceSeismicUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.removeSalt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cubicSizeControl);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.seismicPresentationBox);
            this.Controls.Add(this.seismicDragDrop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enhanceFactorControl);
            this.Name = "EnhanceSeismicUI";
            this.Size = new System.Drawing.Size(560, 211);
            ((System.ComponentModel.ISupportInitialize)(this.enhanceFactorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cubicSizeControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown enhanceFactorControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Slb.Ocean.Petrel.UI.DropTarget seismicDragDrop;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox seismicPresentationBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown cubicSizeControl;
        private System.Windows.Forms.CheckBox removeSalt;
    }
}
