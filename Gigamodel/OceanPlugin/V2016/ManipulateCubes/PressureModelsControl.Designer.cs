namespace ManipulateCubes
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
            this.dropPressureCollection = new Slb.Ocean.Petrel.UI.DropTarget();
            this.coupledPressuresGrid = new System.Windows.Forms.DataGridView();
            this.ImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.FieldColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pressureCollectionCheckbox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.offsetTrackBar = new Slb.Ocean.Petrel.UI.Controls.TrackBar();
            this.gradientTrackBar = new Slb.Ocean.Petrel.UI.Controls.TrackBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.downButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.upButtonj = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.initialPressureDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selector = new System.Windows.Forms.ComboBox();
            this.editButton = new System.Windows.Forms.RadioButton();
            this.newPressureName = new System.Windows.Forms.TextBox();
            this.newButton = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.initialPressureDragDrop = new ManipulateCubes.SeismicDropTarget();
            ((System.ComponentModel.ISupportInitialize)(this.coupledPressuresGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // dropPressureCollection
            // 
            this.dropPressureCollection.AllowDrop = true;
            this.dropPressureCollection.Location = new System.Drawing.Point(8, 232);
            this.dropPressureCollection.Name = "dropPressureCollection";
            this.dropPressureCollection.Size = new System.Drawing.Size(28, 30);
            this.dropPressureCollection.TabIndex = 48;
            // 
            // coupledPressuresGrid
            // 
            this.coupledPressuresGrid.AllowUserToAddRows = false;
            this.coupledPressuresGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.coupledPressuresGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.coupledPressuresGrid.BackgroundColor = System.Drawing.Color.White;
            this.coupledPressuresGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.coupledPressuresGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.coupledPressuresGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImageColumn,
            this.FieldColumn,
            this.TimeColumn});
            this.coupledPressuresGrid.GridColor = System.Drawing.SystemColors.Control;
            this.coupledPressuresGrid.Location = new System.Drawing.Point(43, 232);
            this.coupledPressuresGrid.Margin = new System.Windows.Forms.Padding(2);
            this.coupledPressuresGrid.MultiSelect = false;
            this.coupledPressuresGrid.Name = "coupledPressuresGrid";
            this.coupledPressuresGrid.ReadOnly = true;
            this.coupledPressuresGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.coupledPressuresGrid.RowHeadersVisible = false;
            this.coupledPressuresGrid.RowTemplate.Height = 24;
            this.coupledPressuresGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.coupledPressuresGrid.Size = new System.Drawing.Size(523, 271);
            this.coupledPressuresGrid.TabIndex = 46;
            this.coupledPressuresGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.coupledPressuresDoubleClick);
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
            this.pressureCollectionCheckbox.Location = new System.Drawing.Point(8, 198);
            this.pressureCollectionCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.pressureCollectionCheckbox.Name = "pressureCollectionCheckbox";
            this.pressureCollectionCheckbox.Size = new System.Drawing.Size(119, 17);
            this.pressureCollectionCheckbox.TabIndex = 50;
            this.pressureCollectionCheckbox.Text = "Use Coupled Model";
            this.pressureCollectionCheckbox.UseVisualStyleBackColor = true;
            this.pressureCollectionCheckbox.CheckedChanged += new System.EventHandler(this.pressureCollectionCheckbox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(480, 570);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 68;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(402, 570);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 67;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.CreateOrEditModel);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(111, 204);
            this.flowLayoutPanel3.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel3.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(455, 2);
            this.flowLayoutPanel3.TabIndex = 66;
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(575, 194);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 65;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(466, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "psi";
            // 
            // offsetTrackBar
            // 
            this.offsetTrackBar.Location = new System.Drawing.Point(380, 159);
            this.offsetTrackBar.Maximum = 200000;
            this.offsetTrackBar.MaximumSize = new System.Drawing.Size(250, 25);
            this.offsetTrackBar.MinimumSize = new System.Drawing.Size(80, 25);
            this.offsetTrackBar.Name = "offsetTrackBar";
            this.offsetTrackBar.Size = new System.Drawing.Size(80, 45);
            this.offsetTrackBar.TabIndex = 63;
            this.offsetTrackBar.Value = 50;
            // 
            // gradientTrackBar
            // 
            this.gradientTrackBar.Location = new System.Drawing.Point(210, 159);
            this.gradientTrackBar.Maximum = 200;
            this.gradientTrackBar.MaximumSize = new System.Drawing.Size(250, 25);
            this.gradientTrackBar.MinimumSize = new System.Drawing.Size(80, 25);
            this.gradientTrackBar.Name = "gradientTrackBar";
            this.gradientTrackBar.Size = new System.Drawing.Size(80, 45);
            this.gradientTrackBar.TabIndex = 62;
            this.gradientTrackBar.Value = 50;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(81, 13);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(485, 2);
            this.flowLayoutPanel1.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(296, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "psi";
            // 
            // downButton
            // 
            this.downButton.ImageKey = "112_DownArrowShort_Blue_24x24_72.png";
            this.downButton.ImageList = this.imageList1;
            this.downButton.Location = new System.Drawing.Point(8, 306);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(28, 29);
            this.downButton.TabIndex = 60;
            this.downButton.Text = " ";
            this.downButton.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "112_DownArrowShort_Blue_24x24_72.png");
            this.imageList1.Images.SetKeyName(1, "112_UpArrowShort_Blue_24x24_72.png");
            this.imageList1.Images.SetKeyName(2, "delete.png");
            // 
            // upButtonj
            // 
            this.upButtonj.ImageKey = "112_UpArrowShort_Blue_24x24_72.png";
            this.upButtonj.ImageList = this.imageList1;
            this.upButtonj.Location = new System.Drawing.Point(8, 268);
            this.upButtonj.Name = "upButtonj";
            this.upButtonj.Size = new System.Drawing.Size(28, 29);
            this.upButtonj.TabIndex = 59;
            this.upButtonj.Text = " ";
            this.upButtonj.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.ImageKey = "delete.png";
            this.deleteButton.ImageList = this.imageList1;
            this.deleteButton.Location = new System.Drawing.Point(8, 342);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(28, 29);
            this.deleteButton.TabIndex = 58;
            this.deleteButton.Text = " ";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(575, 88);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 57;
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(575, 3);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 122);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 54;
            this.label10.Text = "Initial Pressure";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(86, 94);
            this.flowLayoutPanel2.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(480, 2);
            this.flowLayoutPanel2.TabIndex = 52;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 51;
            this.label8.Text = "Initial Conditions";
            // 
            // initialPressureDate
            // 
            this.initialPressureDate.Location = new System.Drawing.Point(327, 117);
            this.initialPressureDate.Name = "initialPressureDate";
            this.initialPressureDate.Size = new System.Drawing.Size(237, 20);
            this.initialPressureDate.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "Gradient";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Offset";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Pressure for undefined cells";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Pressure model";
            // 
            // selector
            // 
            this.selector.FormattingEnabled = true;
            this.selector.Location = new System.Drawing.Point(83, 57);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(481, 21);
            this.selector.TabIndex = 41;
            this.selector.Visible = false;
            // 
            // editButton
            // 
            this.editButton.AutoSize = true;
            this.editButton.Location = new System.Drawing.Point(21, 57);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(43, 17);
            this.editButton.TabIndex = 84;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            // 
            // newPressureName
            // 
            this.newPressureName.Location = new System.Drawing.Point(83, 31);
            this.newPressureName.Name = "newPressureName";
            this.newPressureName.Size = new System.Drawing.Size(481, 20);
            this.newPressureName.TabIndex = 82;
            // 
            // newButton
            // 
            this.newButton.AutoSize = true;
            this.newButton.Checked = true;
            this.newButton.Location = new System.Drawing.Point(21, 34);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(47, 17);
            this.newButton.TabIndex = 83;
            this.newButton.TabStop = true;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(31, 618);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(241, 17);
            this.checkBox1.TabIndex = 86;
            this.checkBox1.Text = "offset and grad dont export with the right units";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // initialPressureDragDrop
            // 
            this.initialPressureDragDrop.BackColor = System.Drawing.Color.White;
            this.initialPressureDragDrop.Location = new System.Drawing.Point(92, 112);
            this.initialPressureDragDrop.Name = "initialPressureDragDrop";
            this.initialPressureDragDrop.PlaceHolder = null;
            this.initialPressureDragDrop.PropertyName = null;
            this.initialPressureDragDrop.Size = new System.Drawing.Size(229, 30);
            this.initialPressureDragDrop.TabIndex = 88;
            this.initialPressureDragDrop.Value = null;
            // 
            // PressureModelsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.initialPressureDragDrop);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.newPressureName);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.dropPressureCollection);
            this.Controls.Add(this.coupledPressuresGrid);
            this.Controls.Add(this.pressureCollectionCheckbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.toolTipHotspot3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.offsetTrackBar);
            this.Controls.Add(this.gradientTrackBar);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButtonj);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.initialPressureDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.selector);
            this.Name = "PressureModelsControl";
            this.Size = new System.Drawing.Size(602, 650);
            ((System.ComponentModel.ISupportInitialize)(this.coupledPressuresGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Slb.Ocean.Petrel.UI.DropTarget dropPressureCollection;
        private System.Windows.Forms.DataGridView coupledPressuresGrid;
        private System.Windows.Forms.CheckBox pressureCollectionCheckbox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private System.Windows.Forms.Label label7;
        private Slb.Ocean.Petrel.UI.Controls.TrackBar offsetTrackBar;
        private Slb.Ocean.Petrel.UI.Controls.TrackBar gradientTrackBar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButtonj;
        private System.Windows.Forms.Button deleteButton;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker initialPressureDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selector;
        private System.Windows.Forms.RadioButton editButton;
        private System.Windows.Forms.TextBox newPressureName;
        private System.Windows.Forms.RadioButton newButton;
        private System.Windows.Forms.DataGridViewImageColumn ImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        //private DatumDropTarget initialPressurePresentationBox;
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.ImageList imageList1;
        private SeismicDropTarget initialPressureDragDrop;
        //        private SeismicDropTarget initialPressurePresentationBox;
    }
}
