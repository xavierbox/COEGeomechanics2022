namespace ManipulateCubes
{
    partial class WellStressFromSeismicControl
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
            this.label14 = new System.Windows.Forms.Label();
            this.wellDropTarget = new ManipulateCubes.DatumDropTarget();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.densityDropTarget = new ManipulateCubes.DatumDropTarget();
            this.label15 = new System.Windows.Forms.Label();
            this.youngModulusDropTarget = new ManipulateCubes.DatumDropTarget();
            this.poissonDropTarget = new ManipulateCubes.DatumDropTarget();
            this.maxStrainTrackBar = new System.Windows.Forms.TrackBar();
            this.minStrainTrackBar = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.minStrainTextBox = new System.Windows.Forms.TextBox();
            this.maxStrainTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pressureDropTarget = new ManipulateCubes.DatumDropTarget();
            this.label12 = new System.Windows.Forms.Label();
            this.minStrainAngle = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.maxStrainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(0, 3);
            this.label14.Margin = new System.Windows.Forms.Padding(9, 8, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 261;
            this.label14.Text = "Calibration Well";
            // 
            // wellDropTarget
            // 
            this.wellDropTarget.Location = new System.Drawing.Point(3, 19);
            this.wellDropTarget.Name = "wellDropTarget";
            this.wellDropTarget.PlaceHolder = "Drag here a well or well folder";
            this.wellDropTarget.PropertyName = "Well";
            this.wellDropTarget.Size = new System.Drawing.Size(281, 29);
            this.wellDropTarget.TabIndex = 262;
            this.wellDropTarget.Value = null;
            this.wellDropTarget.SelectionChanged += new System.EventHandler(this.wellDropped);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 169);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 13);
            this.label17.TabIndex = 268;
            this.label17.Text = "Poisson\'s ratio";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(3, 113);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 13);
            this.label16.TabIndex = 267;
            this.label16.Text = "Young\'s Modulus";
            // 
            // densityDropTarget
            // 
            this.densityDropTarget.Location = new System.Drawing.Point(3, 73);
            this.densityDropTarget.Name = "densityDropTarget";
            this.densityDropTarget.PlaceHolder = "Drag-drop here...";
            this.densityDropTarget.PropertyName = "Datum";
            this.densityDropTarget.Size = new System.Drawing.Size(281, 29);
            this.densityDropTarget.TabIndex = 263;
            this.densityDropTarget.Value = null;
            this.densityDropTarget.SelectionChanged += new System.EventHandler(this.seismicCubeDropped);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(3, 60);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 13);
            this.label15.TabIndex = 266;
            this.label15.Text = "Density";
            // 
            // youngModulusDropTarget
            // 
            this.youngModulusDropTarget.Location = new System.Drawing.Point(3, 126);
            this.youngModulusDropTarget.Name = "youngModulusDropTarget";
            this.youngModulusDropTarget.PlaceHolder = "Drag-drop here...";
            this.youngModulusDropTarget.PropertyName = "Datum";
            this.youngModulusDropTarget.Size = new System.Drawing.Size(281, 29);
            this.youngModulusDropTarget.TabIndex = 264;
            this.youngModulusDropTarget.Value = null;
            this.youngModulusDropTarget.SelectionChanged += new System.EventHandler(this.seismicCubeDropped);
            // 
            // poissonDropTarget
            // 
            this.poissonDropTarget.Location = new System.Drawing.Point(3, 180);
            this.poissonDropTarget.Name = "poissonDropTarget";
            this.poissonDropTarget.PlaceHolder = "Drag-drop here...";
            this.poissonDropTarget.PropertyName = "Datum";
            this.poissonDropTarget.Size = new System.Drawing.Size(281, 29);
            this.poissonDropTarget.TabIndex = 265;
            this.poissonDropTarget.Value = null;
            this.poissonDropTarget.SelectionChanged += new System.EventHandler(this.seismicCubeDropped);
            // 
            // maxStrainTrackBar
            // 
            this.maxStrainTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStrainTrackBar.AutoSize = false;
            this.maxStrainTrackBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.maxStrainTrackBar.LargeChange = 1;
            this.maxStrainTrackBar.Location = new System.Drawing.Point(317, 104);
            this.maxStrainTrackBar.Margin = new System.Windows.Forms.Padding(2);
            this.maxStrainTrackBar.Maximum = 500;
            this.maxStrainTrackBar.MaximumSize = new System.Drawing.Size(1221, 240);
            this.maxStrainTrackBar.Minimum = -50;
            this.maxStrainTrackBar.Name = "maxStrainTrackBar";
            this.maxStrainTrackBar.Size = new System.Drawing.Size(176, 20);
            this.maxStrainTrackBar.TabIndex = 272;
            this.maxStrainTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.maxStrainTrackBar.ValueChanged += new System.EventHandler(this.strainChangedTemporaly);
            this.maxStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.strainBarMouseRelease);
            // 
            // minStrainTrackBar
            // 
            this.minStrainTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.minStrainTrackBar.AutoSize = false;
            this.minStrainTrackBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.minStrainTrackBar.LargeChange = 1;
            this.minStrainTrackBar.Location = new System.Drawing.Point(317, 74);
            this.minStrainTrackBar.Margin = new System.Windows.Forms.Padding(2);
            this.minStrainTrackBar.Maximum = 500;
            this.minStrainTrackBar.MaximumSize = new System.Drawing.Size(1221, 240);
            this.minStrainTrackBar.Minimum = -50;
            this.minStrainTrackBar.Name = "minStrainTrackBar";
            this.minStrainTrackBar.Size = new System.Drawing.Size(176, 20);
            this.minStrainTrackBar.TabIndex = 271;
            this.minStrainTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.minStrainTrackBar.ValueChanged += new System.EventHandler(this.strainChangedTemporaly);
            this.minStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.strainBarMouseRelease);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(321, 91);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 270;
            this.label9.Text = "Max";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(321, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 269;
            this.label5.Text = "Min ";
            // 
            // minStrainTextBox
            // 
            this.minStrainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minStrainTextBox.BackColor = System.Drawing.Color.White;
            this.minStrainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.minStrainTextBox.Location = new System.Drawing.Point(497, 76);
            this.minStrainTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.minStrainTextBox.MaximumSize = new System.Drawing.Size(80, 25);
            this.minStrainTextBox.MinimumSize = new System.Drawing.Size(50, 0);
            this.minStrainTextBox.Name = "minStrainTextBox";
            this.minStrainTextBox.Size = new System.Drawing.Size(50, 13);
            this.minStrainTextBox.TabIndex = 274;
            this.minStrainTextBox.Text = "0.0080";
            // 
            // maxStrainTextBox
            // 
            this.maxStrainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStrainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maxStrainTextBox.Location = new System.Drawing.Point(497, 107);
            this.maxStrainTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxStrainTextBox.MaximumSize = new System.Drawing.Size(80, 25);
            this.maxStrainTextBox.MinimumSize = new System.Drawing.Size(50, 0);
            this.maxStrainTextBox.Name = "maxStrainTextBox";
            this.maxStrainTextBox.Size = new System.Drawing.Size(50, 13);
            this.maxStrainTextBox.TabIndex = 273;
            this.maxStrainTextBox.Text = "0.0015";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 224);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 276;
            this.label1.Text = "Pressure";
            // 
            // pressureDropTarget
            // 
            this.pressureDropTarget.Location = new System.Drawing.Point(3, 237);
            this.pressureDropTarget.Name = "pressureDropTarget";
            this.pressureDropTarget.PlaceHolder = "Drag-drop here...";
            this.pressureDropTarget.PropertyName = "Datum";
            this.pressureDropTarget.Size = new System.Drawing.Size(281, 29);
            this.pressureDropTarget.TabIndex = 275;
            this.pressureDropTarget.Value = null;
            this.pressureDropTarget.SelectionChanged += new System.EventHandler(this.seismicCubeDropped);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(360, 152);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(164, 13);
            this.label12.TabIndex = 279;
            this.label12.Text = "Min Principal Strain Azimuth (deg)";
            // 
            // minStrainAngle
            // 
            this.minStrainAngle.DecimalPlaces = 1;
            this.minStrainAngle.Location = new System.Drawing.Point(373, 180);
            this.minStrainAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.minStrainAngle.Name = "minStrainAngle";
            this.minStrainAngle.Size = new System.Drawing.Size(120, 20);
            this.minStrainAngle.TabIndex = 280;
            this.minStrainAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(383, 19);
            this.label11.Margin = new System.Windows.Forms.Padding(9, 8, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 13);
            this.label11.TabIndex = 277;
            this.label11.Text = "Poro-Elastic Parameters";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(403, 40);
            this.label13.Margin = new System.Windows.Forms.Padding(9, 8, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 278;
            this.label13.Text = "Horizontal Strains";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(454, 255);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 281;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.saveButtonPressed);
            // 
            // WellStressFromSeismicControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.minStrainAngle);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pressureDropTarget);
            this.Controls.Add(this.maxStrainTrackBar);
            this.Controls.Add(this.minStrainTrackBar);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.minStrainTextBox);
            this.Controls.Add(this.maxStrainTextBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.wellDropTarget);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.densityDropTarget);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.youngModulusDropTarget);
            this.Controls.Add(this.poissonDropTarget);
            this.Name = "WellStressFromSeismicControl";
            this.Size = new System.Drawing.Size(570, 300);
            ((System.ComponentModel.ISupportInitialize)(this.maxStrainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minStrainAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label14;
        private DatumDropTarget wellDropTarget;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private DatumDropTarget densityDropTarget;
        private System.Windows.Forms.Label label15;
        private DatumDropTarget youngModulusDropTarget;
        private DatumDropTarget poissonDropTarget;
        private System.Windows.Forms.TrackBar maxStrainTrackBar;
        private System.Windows.Forms.TrackBar minStrainTrackBar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minStrainTextBox;
        private System.Windows.Forms.TextBox maxStrainTextBox;
        private System.Windows.Forms.Label label1;
        private DatumDropTarget pressureDropTarget;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown minStrainAngle;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button1;
    }
}
