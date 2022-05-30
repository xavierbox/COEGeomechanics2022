namespace Gigamodel
{
    partial class PressureModelsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PressureModelsControl));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label19 = new System.Windows.Forms.Label();
            this._initialPressureDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.offsetTrackBar = new Slb.Ocean.Petrel.UI.Controls.TrackBar();
            this.gradientTrackBar = new Slb.Ocean.Petrel.UI.Controls.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._coupledPressuresGrid = new System.Windows.Forms.DataGridView();
            this.ImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.FieldColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pressureCollectionCheckbox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dropPressureCollection = new Slb.Ocean.Petrel.UI.DropTarget();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.downButton = new System.Windows.Forms.Button();
            this.upButtonj = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this._initialPressureDragDrop = new Gigamodel.DatumDropTarget();
            this.masterSelector = new Gigamodel.NewEditSelector();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._coupledPressuresGrid)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(107, 105);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(462, 2);
            this.flowLayoutPanel1.TabIndex = 119;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(14, 98);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 13);
            this.label19.TabIndex = 120;
            this.label19.Text = "Initial Conditions";
            // 
            // _initialPressureDate
            // 
            this._initialPressureDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._initialPressureDate.Location = new System.Drawing.Point(353, 126);
            this._initialPressureDate.Name = "_initialPressureDate";
            this._initialPressureDate.Size = new System.Drawing.Size(213, 20);
            this._initialPressureDate.TabIndex = 121;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(482, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 129;
            this.label7.Text = "psi";
            // 
            // offsetTrackBar
            // 
            this.offsetTrackBar.Location = new System.Drawing.Point(376, 171);
            this.offsetTrackBar.Maximum = 200000;
            this.offsetTrackBar.MaximumSize = new System.Drawing.Size(850, 25);
            this.offsetTrackBar.MinimumSize = new System.Drawing.Size(80, 25);
            this.offsetTrackBar.Name = "offsetTrackBar";
            this.offsetTrackBar.Size = new System.Drawing.Size(185, 45);
            this.offsetTrackBar.TabIndex = 128;
            this.offsetTrackBar.Value = 50;
            // 
            // gradientTrackBar
            // 
            this.gradientTrackBar.Location = new System.Drawing.Point(173, 171);
            this.gradientTrackBar.Maximum = 200;
            this.gradientTrackBar.MaximumSize = new System.Drawing.Size(850, 25);
            this.gradientTrackBar.MinimumSize = new System.Drawing.Size(80, 25);
            this.gradientTrackBar.Name = "gradientTrackBar";
            this.gradientTrackBar.Size = new System.Drawing.Size(185, 45);
            this.gradientTrackBar.TabIndex = 127;
            this.gradientTrackBar.Value = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(269, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 126;
            this.label6.Text = "psi";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 125;
            this.label5.Text = "Gradient";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(441, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 124;
            this.label4.Text = "Offset";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 123;
            this.label3.Text = "Pressure for undefined cells";
            // 
            // _coupledPressuresGrid
            // 
            this._coupledPressuresGrid.AllowUserToAddRows = false;
            this._coupledPressuresGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._coupledPressuresGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._coupledPressuresGrid.BackgroundColor = System.Drawing.Color.White;
            this._coupledPressuresGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this._coupledPressuresGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._coupledPressuresGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImageColumn,
            this.FieldColumn,
            this.TimeColumn});
            this._coupledPressuresGrid.GridColor = System.Drawing.SystemColors.Control;
            this._coupledPressuresGrid.Location = new System.Drawing.Point(2, 33);
            this._coupledPressuresGrid.Margin = new System.Windows.Forms.Padding(2);
            this._coupledPressuresGrid.MultiSelect = false;
            this._coupledPressuresGrid.Name = "_coupledPressuresGrid";
            this._coupledPressuresGrid.ReadOnly = true;
            this._coupledPressuresGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this._coupledPressuresGrid.RowHeadersVisible = false;
            this._coupledPressuresGrid.RowTemplate.Height = 24;
            this._coupledPressuresGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._coupledPressuresGrid.Size = new System.Drawing.Size(500, 200);
            this._coupledPressuresGrid.TabIndex = 130;
            this._coupledPressuresGrid.Visible = false;
            // 
            // ImageColumn
            // 
            this.ImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ImageColumn.FillWeight = 15F;
            this.ImageColumn.HeaderText = "      ";
            this.ImageColumn.Name = "ImageColumn";
            this.ImageColumn.ReadOnly = true;
            this.ImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FieldColumn
            // 
            this.FieldColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldColumn.HeaderText = "field";
            this.FieldColumn.Name = "FieldColumn";
            this.FieldColumn.ReadOnly = true;
            // 
            // TimeColumn
            // 
            this.TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.ReadOnly = true;
            // 
            // pressureCollectionCheckbox
            // 
            this.pressureCollectionCheckbox.AutoSize = true;
            this.pressureCollectionCheckbox.Checked = true;
            this.pressureCollectionCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pressureCollectionCheckbox.Location = new System.Drawing.Point(17, 205);
            this.pressureCollectionCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.pressureCollectionCheckbox.Name = "pressureCollectionCheckbox";
            this.pressureCollectionCheckbox.Size = new System.Drawing.Size(119, 17);
            this.pressureCollectionCheckbox.TabIndex = 131;
            this.pressureCollectionCheckbox.Text = "Use Coupled Model";
            this.pressureCollectionCheckbox.UseVisualStyleBackColor = true;
            this.pressureCollectionCheckbox.CheckedChanged += new System.EventHandler(this.pressureCollectionCheckbox_CheckedChanged);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(141, 211);
            this.flowLayoutPanel3.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel3.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(424, 2);
            this.flowLayoutPanel3.TabIndex = 138;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Error_red.png");
            this.imageList1.Images.SetKeyName(1, "112_UpArrowLong_Blue_16x16_72.png");
            this.imageList1.Images.SetKeyName(2, "112_DownArrowShort_Blue_16x16_72.png");
            // 
            // dropPressureCollection
            // 
            this.dropPressureCollection.AllowDrop = true;
            this.dropPressureCollection.Location = new System.Drawing.Point(26, 227);
            this.dropPressureCollection.MinimumSize = new System.Drawing.Size(25, 25);
            this.dropPressureCollection.Name = "dropPressureCollection";
            this.dropPressureCollection.Size = new System.Drawing.Size(28, 29);
            this.dropPressureCollection.TabIndex = 139;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.Controls.Add(this.richTextBox1);
            this.flowLayoutPanel2.Controls.Add(this._coupledPressuresGrid);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(57, 227);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(508, 284);
            this.flowLayoutPanel2.TabIndex = 140;
            this.flowLayoutPanel2.Resize += new System.EventHandler(this.flowLayoutPanel2_Resize);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(499, 25);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "    Drop a collection or single pressure cubes for coupled simulations";
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(572, 202);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 137;
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.ImageIndex = 2;
            this.downButton.ImageList = this.imageList1;
            this.downButton.Location = new System.Drawing.Point(26, 340);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(28, 29);
            this.downButton.TabIndex = 135;
            this.downButton.Text = " ";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.movePressureDown);
            // 
            // upButtonj
            // 
            this.upButtonj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButtonj.ImageIndex = 1;
            this.upButtonj.ImageList = this.imageList1;
            this.upButtonj.Location = new System.Drawing.Point(26, 302);
            this.upButtonj.Name = "upButtonj";
            this.upButtonj.Size = new System.Drawing.Size(28, 29);
            this.upButtonj.TabIndex = 134;
            this.upButtonj.Text = " ";
            this.upButtonj.UseVisualStyleBackColor = true;
            this.upButtonj.Click += new System.EventHandler(this.movePressureUp);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.ImageIndex = 0;
            this.deleteButton.ImageList = this.imageList1;
            this.deleteButton.Location = new System.Drawing.Point(26, 376);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(28, 29);
            this.deleteButton.TabIndex = 133;
            this.deleteButton.Text = " ";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deletePressure);
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(572, 98);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 132;
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(572, 14);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 117;
            // 
            // _initialPressureDragDrop
            // 
            this._initialPressureDragDrop.AcceptedTypes = null;
            this._initialPressureDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._initialPressureDragDrop.BackColor = System.Drawing.Color.White;
            this._initialPressureDragDrop.DisplayImage = null;
            this._initialPressureDragDrop.ErrorImage = null;
            this._initialPressureDragDrop.ImageList = null;
            this._initialPressureDragDrop.Location = new System.Drawing.Point(26, 122);
            this._initialPressureDragDrop.Name = "_initialPressureDragDrop";
            this._initialPressureDragDrop.PlaceHolder = "Drag-drop here...";
            this._initialPressureDragDrop.ReferenceName = null;
            this._initialPressureDragDrop.Size = new System.Drawing.Size(319, 30);
            this._initialPressureDragDrop.TabIndex = 122;
            this._initialPressureDragDrop.Value = null;
            // 
            // masterSelector
            // 
            this.masterSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterSelector.DeleteImage = null;
            this.masterSelector.IsNewSelected = true;
            this.masterSelector.Location = new System.Drawing.Point(3, 0);
            this.masterSelector.ModelNames = ((System.Collections.Generic.List<string>)(resources.GetObject("masterSelector.ModelNames")));
            this.masterSelector.Name = "masterSelector";
            this.masterSelector.Size = new System.Drawing.Size(564, 95);
            this.masterSelector.TabIndex = 118;
            this.masterSelector.Title = "Pressure Model";
            // 
            // PressureModelsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.dropPressureCollection);
            this.Controls.Add(this.pressureCollectionCheckbox);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButtonj);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.offsetTrackBar);
            this.Controls.Add(this.gradientTrackBar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._initialPressureDragDrop);
            this.Controls.Add(this._initialPressureDate);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.masterSelector);
            this.Controls.Add(this.toolTipHotspot3);
            this.Name = "PressureModelsControl";
            this.Size = new System.Drawing.Size(604, 552);
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._coupledPressuresGrid)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NewEditSelector masterSelector;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker _initialPressureDate;
        private DatumDropTarget _initialPressureDragDrop;
        private System.Windows.Forms.Label label7;
        private Slb.Ocean.Petrel.UI.Controls.TrackBar offsetTrackBar;
        private Slb.Ocean.Petrel.UI.Controls.TrackBar gradientTrackBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView _coupledPressuresGrid;
        private System.Windows.Forms.DataGridViewImageColumn ImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.CheckBox pressureCollectionCheckbox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButtonj;
        private System.Windows.Forms.Button deleteButton;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private Slb.Ocean.Petrel.UI.DropTarget dropPressureCollection;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
