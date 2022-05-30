namespace Restoration.GFM.View
{
    partial class Simulation1DProcessingControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 11D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(3D, 13D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(5D, 22D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(11D, 44D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(14D, 33D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(17D, 44D);
            this.ColumnCasesComboBox = new Slb.Ocean.Petrel.UI.Controls.ComboBox();
            this.selectedPointcontrol = new System.Windows.Forms.NumericUpDown();
            this.thicknessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelcounter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.selectedPointcontrol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessChart)).BeginInit();
            this.SuspendLayout();
            // 
            // ColumnCasesComboBox
            // 
            this.ColumnCasesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColumnCasesComboBox.Location = new System.Drawing.Point(72, 84);
            this.ColumnCasesComboBox.Name = "ColumnCasesComboBox";
            this.ColumnCasesComboBox.Size = new System.Drawing.Size(121, 22);
            this.ColumnCasesComboBox.TabIndex = 159;
            this.ColumnCasesComboBox.SelectedIndexChanged += new System.EventHandler(this.ColumnCasesComboBox_SelectedIndexChanged);
            // 
            // selectedPointcontrol
            // 
            this.selectedPointcontrol.Location = new System.Drawing.Point(247, 85);
            this.selectedPointcontrol.Name = "selectedPointcontrol";
            this.selectedPointcontrol.Size = new System.Drawing.Size(84, 20);
            this.selectedPointcontrol.TabIndex = 158;
            this.selectedPointcontrol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.selectedPointcontrol.ValueChanged += new System.EventHandler(this.selectedPointcontrol_ValueChanged);
            // 
            // thicknessChart
            // 
            this.thicknessChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.thicknessChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.thicknessChart.Legends.Add(legend1);
            this.thicknessChart.Location = new System.Drawing.Point(19, 112);
            this.thicknessChart.Name = "thicknessChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            this.thicknessChart.Series.Add(series1);
            this.thicknessChart.Size = new System.Drawing.Size(467, 372);
            this.thicknessChart.TabIndex = 157;
            this.thicknessChart.Text = "chart1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(199, 87);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 156;
            this.label14.Text = "Column";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(35, 87);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 160;
            this.label12.Text = "Case";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(235, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 13);
            this.label15.TabIndex = 161;
            this.label15.Text = "Layer Thickness";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(326, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 162;
            this.button1.Text = "Calculate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(348, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 163;
            this.label1.Text = "of";
            // 
            // labelcounter
            // 
            this.labelcounter.AutoSize = true;
            this.labelcounter.Location = new System.Drawing.Point(370, 87);
            this.labelcounter.Name = "labelcounter";
            this.labelcounter.Size = new System.Drawing.Size(35, 13);
            this.labelcounter.TabIndex = 164;
            this.labelcounter.Text = "label2";
            // 
            // Simulation1DProcessingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelcounter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ColumnCasesComboBox);
            this.Controls.Add(this.selectedPointcontrol);
            this.Controls.Add(this.thicknessChart);
            this.Controls.Add(this.label14);
            this.MinimumSize = new System.Drawing.Size(497, 500);
            this.Name = "Simulation1DProcessingControl";
            this.Size = new System.Drawing.Size(497, 509);
            this.Load += new System.EventHandler(this.Simulation1DProcessingControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.selectedPointcontrol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Slb.Ocean.Petrel.UI.Controls.ComboBox ColumnCasesComboBox;
        private System.Windows.Forms.NumericUpDown selectedPointcontrol;
        private System.Windows.Forms.DataVisualization.Charting.Chart thicknessChart;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelcounter;
    }
}
