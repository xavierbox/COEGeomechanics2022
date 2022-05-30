using OceanControlsLib;

namespace Gigamodel
{
    partial class BoundaryConditionsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoundaryConditionsControl));
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.poroelasticPanel = new System.Windows.Forms.Panel();
            this.maxStrainControl = new System.Windows.Forms.NumericUpDown();
            this.angleLabel = new System.Windows.Forms.Label();
            this.minStrainControl = new System.Windows.Forms.NumericUpDown();
            this.minStrainAngle = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.minStrainTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.maxStrainTextBox = new System.Windows.Forms.TextBox();
            this.poroelasticCalculationCheckBox = new System.Windows.Forms.CheckBox();
            this.wellPanel = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.calibrationWellDropTarget = new DatumDropTarget();
            this.pressureSelector = new System.Windows.Forms.ComboBox();
            this.materialSelector = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.biotsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.gapDensityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.waterdensitypanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.waterDensityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.offshoreCheck = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.masterSelector = new Gigamodel.NewEditSelector();
            this.datumDropTarget = new DatumDropTarget();
            this.flowLayoutPanel3.SuspendLayout();
            this.poroelasticPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxStrainControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainAngle)).BeginInit();
            this.wellPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.biotsNumericUpDown)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gapDensityUpDown)).BeginInit();
            this.waterdensitypanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waterDensityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.Controls.Add(this.poroelasticPanel);
            this.flowLayoutPanel3.Controls.Add(this.poroelasticCalculationCheckBox);
            this.flowLayoutPanel3.Controls.Add(this.wellPanel);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(18, 283);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(571, 299);
            this.flowLayoutPanel3.TabIndex = 263;
            this.flowLayoutPanel3.WrapContents = false;
            // 
            // poroelasticPanel
            // 
            this.poroelasticPanel.Controls.Add(this.maxStrainControl);
            this.poroelasticPanel.Controls.Add(this.angleLabel);
            this.poroelasticPanel.Controls.Add(this.minStrainControl);
            this.poroelasticPanel.Controls.Add(this.minStrainAngle);
            this.poroelasticPanel.Controls.Add(this.label11);
            this.poroelasticPanel.Controls.Add(this.label9);
            this.poroelasticPanel.Controls.Add(this.label5);
            this.poroelasticPanel.Controls.Add(this.minStrainTextBox);
            this.poroelasticPanel.Controls.Add(this.label13);
            this.poroelasticPanel.Controls.Add(this.maxStrainTextBox);
            this.poroelasticPanel.Location = new System.Drawing.Point(3, 3);
            this.poroelasticPanel.Name = "poroelasticPanel";
            this.poroelasticPanel.Size = new System.Drawing.Size(557, 143);
            this.poroelasticPanel.TabIndex = 251;
            // 
            // maxStrainControl
            // 
            this.maxStrainControl.DecimalPlaces = 6;
            this.maxStrainControl.Increment = new decimal(new int[] {
            10,
            0,
            0,
            393216});
            this.maxStrainControl.Location = new System.Drawing.Point(64, 78);
            this.maxStrainControl.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.maxStrainControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147221504});
            this.maxStrainControl.Name = "maxStrainControl";
            this.maxStrainControl.Size = new System.Drawing.Size(111, 20);
            this.maxStrainControl.TabIndex = 268;
            this.maxStrainControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maxStrainControl.Value = new decimal(new int[] {
            25,
            0,
            0,
            327680});
            // 
            // angleLabel
            // 
            this.angleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.angleLabel.AutoSize = true;
            this.angleLabel.Location = new System.Drawing.Point(298, 29);
            this.angleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.angleLabel.Name = "angleLabel";
            this.angleLabel.Size = new System.Drawing.Size(167, 13);
            this.angleLabel.TabIndex = 239;
            this.angleLabel.Text = "Max Principal Strain Azimuth (deg)";
            // 
            // minStrainControl
            // 
            this.minStrainControl.DecimalPlaces = 6;
            this.minStrainControl.Increment = new decimal(new int[] {
            10,
            0,
            0,
            393216});
            this.minStrainControl.Location = new System.Drawing.Point(64, 52);
            this.minStrainControl.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.minStrainControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147221504});
            this.minStrainControl.Name = "minStrainControl";
            this.minStrainControl.Size = new System.Drawing.Size(111, 20);
            this.minStrainControl.TabIndex = 267;
            this.minStrainControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minStrainControl.Value = new decimal(new int[] {
            12,
            0,
            0,
            327680});
            // 
            // minStrainAngle
            // 
            this.minStrainAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minStrainAngle.DecimalPlaces = 1;
            this.minStrainAngle.Increment = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.minStrainAngle.Location = new System.Drawing.Point(330, 57);
            this.minStrainAngle.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.minStrainAngle.Name = "minStrainAngle";
            this.minStrainAngle.Size = new System.Drawing.Size(94, 20);
            this.minStrainAngle.TabIndex = 251;
            this.minStrainAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 8);
            this.label11.Margin = new System.Windows.Forms.Padding(9, 8, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 13);
            this.label11.TabIndex = 234;
            this.label11.Text = "Poro-Elastic Parameters";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 80);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 229;
            this.label9.Text = "Max";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 59);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 227;
            this.label5.Text = "Min ";
            // 
            // minStrainTextBox
            // 
            this.minStrainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minStrainTextBox.BackColor = System.Drawing.Color.White;
            this.minStrainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.minStrainTextBox.Location = new System.Drawing.Point(469, 102);
            this.minStrainTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.minStrainTextBox.MaximumSize = new System.Drawing.Size(80, 25);
            this.minStrainTextBox.MinimumSize = new System.Drawing.Size(50, 0);
            this.minStrainTextBox.Name = "minStrainTextBox";
            this.minStrainTextBox.Size = new System.Drawing.Size(50, 13);
            this.minStrainTextBox.TabIndex = 233;
            this.minStrainTextBox.Text = "0.0080";
            this.minStrainTextBox.Visible = false;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(33, 29);
            this.label13.Margin = new System.Windows.Forms.Padding(9, 8, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 236;
            this.label13.Text = "Horizontal Strains";
            // 
            // maxStrainTextBox
            // 
            this.maxStrainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStrainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maxStrainTextBox.Location = new System.Drawing.Point(469, 119);
            this.maxStrainTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxStrainTextBox.MaximumSize = new System.Drawing.Size(80, 25);
            this.maxStrainTextBox.MinimumSize = new System.Drawing.Size(50, 0);
            this.maxStrainTextBox.Name = "maxStrainTextBox";
            this.maxStrainTextBox.Size = new System.Drawing.Size(50, 13);
            this.maxStrainTextBox.TabIndex = 232;
            this.maxStrainTextBox.Text = "0.0015";
            this.maxStrainTextBox.Visible = false;
            // 
            // poroelasticCalculationCheckBox
            // 
            this.poroelasticCalculationCheckBox.AutoSize = true;
            this.poroelasticCalculationCheckBox.Location = new System.Drawing.Point(3, 152);
            this.poroelasticCalculationCheckBox.Name = "poroelasticCalculationCheckBox";
            this.poroelasticCalculationCheckBox.Size = new System.Drawing.Size(259, 17);
            this.poroelasticCalculationCheckBox.TabIndex = 0;
            this.poroelasticCalculationCheckBox.Text = "Use well and mechanical properties for calibration";
            this.poroelasticCalculationCheckBox.UseVisualStyleBackColor = true;
            // 
            // wellPanel
            // 
            this.wellPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wellPanel.Controls.Add(this.checkBox1);
            this.wellPanel.Controls.Add(this.calibrationWellDropTarget);
            this.wellPanel.Controls.Add(this.pressureSelector);
            this.wellPanel.Controls.Add(this.materialSelector);
            this.wellPanel.Controls.Add(this.label10);
            this.wellPanel.Controls.Add(this.biotsNumericUpDown);
            this.wellPanel.Location = new System.Drawing.Point(3, 175);
            this.wellPanel.Name = "wellPanel";
            this.wellPanel.Size = new System.Drawing.Size(557, 100);
            this.wellPanel.TabIndex = 262;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 66);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(146, 17);
            this.checkBox1.TabIndex = 252;
            this.checkBox1.Text = "Compute full stress tensor";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // calibrationWellDropTarget
            // 
            this.calibrationWellDropTarget.AcceptedTypes = null;
            this.calibrationWellDropTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calibrationWellDropTarget.BackColor = System.Drawing.Color.White;
            this.calibrationWellDropTarget.DisplayImage = null;
            this.calibrationWellDropTarget.ErrorImage = null;
            this.calibrationWellDropTarget.ImageList = null;
            this.calibrationWellDropTarget.Location = new System.Drawing.Point(3, 3);
            this.calibrationWellDropTarget.Name = "calibrationWellDropTarget";
            this.calibrationWellDropTarget.PlaceHolder = "Drag-drop here...";
            this.calibrationWellDropTarget.ReferenceName = null;
            this.calibrationWellDropTarget.Size = new System.Drawing.Size(282, 30);
            this.calibrationWellDropTarget.TabIndex = 266;
            this.calibrationWellDropTarget.Value = null;
            // 
            // pressureSelector
            // 
            this.pressureSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pressureSelector.FormattingEnabled = true;
            this.pressureSelector.Location = new System.Drawing.Point(430, 8);
            this.pressureSelector.Name = "pressureSelector";
            this.pressureSelector.Size = new System.Drawing.Size(124, 21);
            this.pressureSelector.TabIndex = 245;
            // 
            // materialSelector
            // 
            this.materialSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.materialSelector.FormattingEnabled = true;
            this.materialSelector.Location = new System.Drawing.Point(291, 8);
            this.materialSelector.Name = "materialSelector";
            this.materialSelector.Size = new System.Drawing.Size(133, 21);
            this.materialSelector.TabIndex = 244;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 43);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 228;
            this.label10.Text = "Biot\'s Coefficient";
            this.label10.Visible = false;
            // 
            // biotsNumericUpDown
            // 
            this.biotsNumericUpDown.DecimalPlaces = 1;
            this.biotsNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.biotsNumericUpDown.Location = new System.Drawing.Point(91, 41);
            this.biotsNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.biotsNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.biotsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.biotsNumericUpDown.Name = "biotsNumericUpDown";
            this.biotsNumericUpDown.Size = new System.Drawing.Size(133, 20);
            this.biotsNumericUpDown.TabIndex = 237;
            this.biotsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.biotsNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.biotsNumericUpDown.Visible = false;
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(565, 257);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 262;
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(565, 101);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 261;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.gapDensityUpDown);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(18, 177);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.MaximumSize = new System.Drawing.Size(1278, 65);
            this.panel3.MinimumSize = new System.Drawing.Size(210, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(309, 22);
            this.panel3.TabIndex = 260;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 132;
            this.label6.Text = "Avg. Gap Density";
            // 
            // gapDensityUpDown
            // 
            this.gapDensityUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gapDensityUpDown.DecimalPlaces = 1;
            this.gapDensityUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.gapDensityUpDown.Location = new System.Drawing.Point(113, 1);
            this.gapDensityUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.gapDensityUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.gapDensityUpDown.Name = "gapDensityUpDown";
            this.gapDensityUpDown.Size = new System.Drawing.Size(94, 20);
            this.gapDensityUpDown.TabIndex = 151;
            this.gapDensityUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gapDensityUpDown.Value = new decimal(new int[] {
            27,
            0,
            0,
            65536});
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(255, 3);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 133;
            this.label7.Text = "gr/cm3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 127);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 257;
            this.label2.Text = "Datum";
            // 
            // waterdensitypanel
            // 
            this.waterdensitypanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waterdensitypanel.Controls.Add(this.label4);
            this.waterdensitypanel.Controls.Add(this.waterDensityUpDown);
            this.waterdensitypanel.Controls.Add(this.label3);
            this.waterdensitypanel.Location = new System.Drawing.Point(19, 226);
            this.waterdensitypanel.Margin = new System.Windows.Forms.Padding(2);
            this.waterdensitypanel.MaximumSize = new System.Drawing.Size(1278, 65);
            this.waterdensitypanel.MinimumSize = new System.Drawing.Size(210, 0);
            this.waterdensitypanel.Name = "waterdensitypanel";
            this.waterdensitypanel.Size = new System.Drawing.Size(309, 22);
            this.waterdensitypanel.TabIndex = 259;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 152;
            this.label4.Text = "Sea Water Density";
            // 
            // waterDensityUpDown
            // 
            this.waterDensityUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waterDensityUpDown.DecimalPlaces = 1;
            this.waterDensityUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.waterDensityUpDown.Location = new System.Drawing.Point(113, 2);
            this.waterDensityUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.waterDensityUpDown.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.waterDensityUpDown.Name = "waterDensityUpDown";
            this.waterDensityUpDown.Size = new System.Drawing.Size(94, 20);
            this.waterDensityUpDown.TabIndex = 152;
            this.waterDensityUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.waterDensityUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 136;
            this.label3.Text = "gr/cm3";
            // 
            // offshoreCheck
            // 
            this.offshoreCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offshoreCheck.AutoSize = true;
            this.offshoreCheck.Location = new System.Drawing.Point(21, 208);
            this.offshoreCheck.Margin = new System.Windows.Forms.Padding(11, 2, 2, 2);
            this.offshoreCheck.Name = "offshoreCheck";
            this.offshoreCheck.Size = new System.Drawing.Size(66, 17);
            this.offshoreCheck.TabIndex = 258;
            this.offshoreCheck.Text = "Offshore";
            this.offshoreCheck.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(83, 268);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(469, 2);
            this.flowLayoutPanel1.TabIndex = 256;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 255;
            this.label1.Text = "Sideburden";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(85, 107);
            this.flowLayoutPanel2.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(466, 2);
            this.flowLayoutPanel2.TabIndex = 254;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 253;
            this.label8.Text = "Overburden";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(349, 127);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 266;
            this.pictureBox1.TabStop = false;
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(565, 14);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 268;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "PoroElasticDefinitions.png");
            this.imageList1.Images.SetKeyName(1, "PoroElasticDefinitions2.png");
            // 
            // masterSelector
            // 
            this.masterSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterSelector.DeleteImage = null;
            this.masterSelector.IsNewSelected = true;
            this.masterSelector.Location = new System.Drawing.Point(0, 0);
            this.masterSelector.ModelNames = ((System.Collections.Generic.List<string>)(resources.GetObject("masterSelector.ModelNames")));
            this.masterSelector.Name = "masterSelector";
            this.masterSelector.Size = new System.Drawing.Size(557, 95);
            this.masterSelector.TabIndex = 267;
            this.masterSelector.Title = "Tectonics Model";
            // 
            // datumDropTarget
            // 
            this.datumDropTarget.AcceptedTypes = null;
            this.datumDropTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datumDropTarget.BackColor = System.Drawing.Color.White;
            this.datumDropTarget.DisplayImage = null;
            this.datumDropTarget.ErrorImage = null;
            this.datumDropTarget.ImageList = null;
            this.datumDropTarget.Location = new System.Drawing.Point(20, 142);
            this.datumDropTarget.Name = "datumDropTarget";
            this.datumDropTarget.PlaceHolder = "Drag-drop  a datum surface here. Zero is assumed by default";
            this.datumDropTarget.ReferenceName = null;
            this.datumDropTarget.Size = new System.Drawing.Size(299, 30);
            this.datumDropTarget.TabIndex = 265;
            this.datumDropTarget.Value = null;
            // 
            // BoundaryConditionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolTipHotspot3);
            this.Controls.Add(this.masterSelector);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.datumDropTarget);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.waterdensitypanel);
            this.Controls.Add(this.offshoreCheck);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.label8);
            this.Name = "BoundaryConditionsControl";
            this.Size = new System.Drawing.Size(600, 600);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.poroelasticPanel.ResumeLayout(false);
            this.poroelasticPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxStrainControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainAngle)).EndInit();
            this.wellPanel.ResumeLayout(false);
            this.wellPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.biotsNumericUpDown)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gapDensityUpDown)).EndInit();
            this.waterdensitypanel.ResumeLayout(false);
            this.waterdensitypanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waterDensityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.CheckBox poroelasticCalculationCheckBox;
        private System.Windows.Forms.Panel wellPanel;
        private System.Windows.Forms.ComboBox pressureSelector;
        private System.Windows.Forms.ComboBox materialSelector;
        private System.Windows.Forms.Panel poroelasticPanel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label angleLabel;
        private System.Windows.Forms.NumericUpDown minStrainAngle;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minStrainTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox maxStrainTextBox;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown gapDensityUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel waterdensitypanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown waterDensityUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox offshoreCheck;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label8;
        private DatumDropTarget calibrationWellDropTarget;
        private DatumDropTarget datumDropTarget;
        private System.Windows.Forms.PictureBox pictureBox1;
        private NewEditSelector masterSelector;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown biotsNumericUpDown;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.NumericUpDown maxStrainControl;
        private System.Windows.Forms.NumericUpDown minStrainControl;
    }
}
