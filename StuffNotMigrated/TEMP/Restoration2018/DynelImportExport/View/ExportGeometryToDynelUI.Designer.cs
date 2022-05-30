namespace Restoration
{
    partial class ExportGeometryToDynelUI
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
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dropFaults = new Slb.Ocean.Petrel.UI.DropTarget();
            this.faultsPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.objectsGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.traslationLabel = new System.Windows.Forms.Label();
            this.XcheckBox = new System.Windows.Forms.CheckBox();
            this.ycheckBox = new System.Windows.Forms.CheckBox();
            this.zcheckBox = new System.Windows.Forms.CheckBox();
            this.xTraslationControl = new System.Windows.Forms.NumericUpDown();
            this.yTraslationControl = new System.Windows.Forms.NumericUpDown();
            this.zTraslationControl = new System.Windows.Forms.NumericUpDown();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.selectExportFolderButton = new System.Windows.Forms.Button();
            this.folderPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.exportFolderLabel = new System.Windows.Forms.Label();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.dropObjectsLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xTraslationControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yTraslationControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zTraslationControl)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.objectsGrid);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 104);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(498, 196);
            this.flowLayoutPanel1.TabIndex = 30;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dropFaults);
            this.panel1.Controls.Add(this.faultsPresentationBox);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.MinimumSize = new System.Drawing.Size(470, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 28);
            this.panel1.TabIndex = 22;
            // 
            // dropFaults
            // 
            this.dropFaults.AllowDrop = true;
            this.dropFaults.Location = new System.Drawing.Point(3, 0);
            this.dropFaults.Name = "dropFaults";
            this.dropFaults.Size = new System.Drawing.Size(23, 23);
            this.dropFaults.TabIndex = 0;
            // 
            // faultsPresentationBox
            // 
            this.faultsPresentationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.faultsPresentationBox.Location = new System.Drawing.Point(32, 0);
            this.faultsPresentationBox.Name = "faultsPresentationBox";
            this.faultsPresentationBox.Size = new System.Drawing.Size(435, 22);
            this.faultsPresentationBox.TabIndex = 3;
            this.faultsPresentationBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.faultsPresentationBox_KeyDown);
            // 
            // objectsGrid
            // 
            this.objectsGrid.AllowUserToAddRows = false;
            this.objectsGrid.AllowUserToDeleteRows = false;
            this.objectsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectsGrid.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.objectsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.objectsGrid.CausesValidation = false;
            this.objectsGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.objectsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objectsGrid.ColumnHeadersVisible = false;
            this.objectsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.objectsGrid.Location = new System.Drawing.Point(3, 37);
            this.objectsGrid.MinimumSize = new System.Drawing.Size(470, 50);
            this.objectsGrid.Name = "objectsGrid";
            this.objectsGrid.RowHeadersVisible = false;
            this.objectsGrid.Size = new System.Drawing.Size(470, 67);
            this.objectsGrid.TabIndex = 5;
            this.objectsGrid.Visible = false;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 30F;
            this.Column1.HeaderText = "Image";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Layer";
            this.Column2.Name = "Column2";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.traslationLabel);
            this.panel2.Controls.Add(this.XcheckBox);
            this.panel2.Controls.Add(this.ycheckBox);
            this.panel2.Controls.Add(this.zcheckBox);
            this.panel2.Controls.Add(this.xTraslationControl);
            this.panel2.Controls.Add(this.yTraslationControl);
            this.panel2.Controls.Add(this.zTraslationControl);
            this.panel2.Controls.Add(this.toolTipHotspot2);
            this.panel2.Location = new System.Drawing.Point(3, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(470, 75);
            this.panel2.TabIndex = 23;
            // 
            // traslationLabel
            // 
            this.traslationLabel.AutoSize = true;
            this.traslationLabel.Location = new System.Drawing.Point(3, 7);
            this.traslationLabel.Name = "traslationLabel";
            this.traslationLabel.Size = new System.Drawing.Size(82, 13);
            this.traslationLabel.TabIndex = 9;
            this.traslationLabel.Text = "Apply Traslation";
            // 
            // XcheckBox
            // 
            this.XcheckBox.AutoSize = true;
            this.XcheckBox.Checked = true;
            this.XcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.XcheckBox.Location = new System.Drawing.Point(111, 7);
            this.XcheckBox.Name = "XcheckBox";
            this.XcheckBox.Size = new System.Drawing.Size(33, 17);
            this.XcheckBox.TabIndex = 10;
            this.XcheckBox.Text = "X";
            this.XcheckBox.UseVisualStyleBackColor = true;
            // 
            // ycheckBox
            // 
            this.ycheckBox.AutoSize = true;
            this.ycheckBox.Checked = true;
            this.ycheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ycheckBox.Location = new System.Drawing.Point(203, 7);
            this.ycheckBox.Name = "ycheckBox";
            this.ycheckBox.Size = new System.Drawing.Size(33, 17);
            this.ycheckBox.TabIndex = 11;
            this.ycheckBox.Text = "Y";
            this.ycheckBox.UseVisualStyleBackColor = true;
            // 
            // zcheckBox
            // 
            this.zcheckBox.AutoSize = true;
            this.zcheckBox.Location = new System.Drawing.Point(289, 7);
            this.zcheckBox.Name = "zcheckBox";
            this.zcheckBox.Size = new System.Drawing.Size(33, 17);
            this.zcheckBox.TabIndex = 12;
            this.zcheckBox.Text = "Z";
            this.zcheckBox.UseVisualStyleBackColor = true;
            // 
            // xTraslationControl
            // 
            this.xTraslationControl.DecimalPlaces = 4;
            this.xTraslationControl.Location = new System.Drawing.Point(85, 28);
            this.xTraslationControl.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.xTraslationControl.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.xTraslationControl.Name = "xTraslationControl";
            this.xTraslationControl.Size = new System.Drawing.Size(83, 20);
            this.xTraslationControl.TabIndex = 13;
            this.xTraslationControl.ValueChanged += new System.EventHandler(this.xTraslationControl_ValueChanged);
            this.xTraslationControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.xTraslationControl_KeyDown);
            // 
            // yTraslationControl
            // 
            this.yTraslationControl.DecimalPlaces = 4;
            this.yTraslationControl.Location = new System.Drawing.Point(174, 28);
            this.yTraslationControl.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.yTraslationControl.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.yTraslationControl.Name = "yTraslationControl";
            this.yTraslationControl.Size = new System.Drawing.Size(83, 20);
            this.yTraslationControl.TabIndex = 14;
            this.yTraslationControl.ValueChanged += new System.EventHandler(this.xTraslationControl_ValueChanged);
            // 
            // zTraslationControl
            // 
            this.zTraslationControl.DecimalPlaces = 4;
            this.zTraslationControl.Location = new System.Drawing.Point(263, 28);
            this.zTraslationControl.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.zTraslationControl.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.zTraslationControl.Name = "zTraslationControl";
            this.zTraslationControl.Size = new System.Drawing.Size(83, 20);
            this.zTraslationControl.TabIndex = 15;
            this.zTraslationControl.ValueChanged += new System.EventHandler(this.xTraslationControl_ValueChanged);
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(423, 28);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 16;
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(510, 107);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 29;
            // 
            // selectExportFolderButton
            // 
            this.selectExportFolderButton.Image = global::Restoration.Properties.Resources.Folder_16x16;
            this.selectExportFolderButton.Location = new System.Drawing.Point(12, 50);
            this.selectExportFolderButton.Name = "selectExportFolderButton";
            this.selectExportFolderButton.Size = new System.Drawing.Size(23, 22);
            this.selectExportFolderButton.TabIndex = 28;
            this.selectExportFolderButton.UseVisualStyleBackColor = true;
            // 
            // folderPresentationBox
            // 
            this.folderPresentationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderPresentationBox.Location = new System.Drawing.Point(41, 50);
            this.folderPresentationBox.Name = "folderPresentationBox";
            this.folderPresentationBox.Size = new System.Drawing.Size(452, 22);
            this.folderPresentationBox.TabIndex = 27;
            this.folderPresentationBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.folderPresentationBox_KeyDown);
            // 
            // exportFolderLabel
            // 
            this.exportFolderLabel.AutoSize = true;
            this.exportFolderLabel.Location = new System.Drawing.Point(3, 31);
            this.exportFolderLabel.Name = "exportFolderLabel";
            this.exportFolderLabel.Size = new System.Drawing.Size(102, 13);
            this.exportFolderLabel.TabIndex = 26;
            this.exportFolderLabel.Text = "Select Export Folder";
            // 
            // cancelbutton
            // 
            this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelbutton.Location = new System.Drawing.Point(348, 365);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.cancelbutton.TabIndex = 25;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exportButton.Location = new System.Drawing.Point(429, 365);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 24;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            // 
            // dropObjectsLabel
            // 
            this.dropObjectsLabel.AutoSize = true;
            this.dropObjectsLabel.Location = new System.Drawing.Point(3, 85);
            this.dropObjectsLabel.Name = "dropObjectsLabel";
            this.dropObjectsLabel.Size = new System.Drawing.Size(364, 13);
            this.dropObjectsLabel.TabIndex = 23;
            this.dropObjectsLabel.Text = "Drop Objects to Export (Structural Model, Final Faults or Horizons Collection)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 13);
            this.label4.TabIndex = 431;
            this.label4.Text = "Export Geometry to Restoration";
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(510, 52);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 432;
            // 
            // ExportGeometryToDynelUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.toolTipHotspot3);
            this.Controls.Add(this.selectExportFolderButton);
            this.Controls.Add(this.folderPresentationBox);
            this.Controls.Add(this.exportFolderLabel);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.dropObjectsLabel);
            this.MinimumSize = new System.Drawing.Size(540, 415);
            this.Name = "ExportGeometryToDynelUI";
            this.Size = new System.Drawing.Size(540, 415);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xTraslationControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yTraslationControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zTraslationControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private Slb.Ocean.Petrel.UI.DropTarget dropFaults;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox faultsPresentationBox;
        private System.Windows.Forms.DataGridView objectsGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label traslationLabel;
        private System.Windows.Forms.CheckBox XcheckBox;
        private System.Windows.Forms.CheckBox ycheckBox;
        private System.Windows.Forms.CheckBox zcheckBox;
        private System.Windows.Forms.NumericUpDown xTraslationControl;
        private System.Windows.Forms.NumericUpDown yTraslationControl;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private System.Windows.Forms.NumericUpDown zTraslationControl;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private System.Windows.Forms.Button selectExportFolderButton;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox folderPresentationBox;
        private System.Windows.Forms.Label exportFolderLabel;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label dropObjectsLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label4;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
    }
}
