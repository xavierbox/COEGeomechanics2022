using OceanControlsLib;

namespace Gigamodel
{
    partial class MaterialsModelsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialsModelsControl));
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tensileStrengthRatio = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.prLabel = new System.Windows.Forms.Label();
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.masterSelector = new Gigamodel.NewEditSelector();
            this._ymDragDrop = new DatumDropTarget();
            this.frictionDragtDrop = new DatumDropTarget();
            this.ucsDragDrop = new DatumDropTarget();
            this._densityDragDrop = new DatumDropTarget();
            this._prDragDrop = new DatumDropTarget();
            ((System.ComponentModel.ISupportInitialize)(this.tensileStrengthRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(109, 307);
            this.flowLayoutPanel2.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(450, 2);
            this.flowLayoutPanel2.TabIndex = 99;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(101, 109);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(458, 2);
            this.flowLayoutPanel1.TabIndex = 98;
            // 
            // tensileStrengthRatio
            // 
            this.tensileStrengthRatio.DecimalPlaces = 1;
            this.tensileStrengthRatio.Location = new System.Drawing.Point(34, 469);
            this.tensileStrengthRatio.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.tensileStrengthRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.tensileStrengthRatio.Name = "tensileStrengthRatio";
            this.tensileStrengthRatio.Size = new System.Drawing.Size(120, 20);
            this.tensileStrengthRatio.TabIndex = 105;
            this.tensileStrengthRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tensileStrengthRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 444);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 13);
            this.label2.TabIndex = 103;
            this.label2.Text = "Tensile to Compressional Strength Ratio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 330);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 101;
            this.label3.Text = "Compressional Strength (UCS)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 104;
            this.label4.Text = "Strength Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 382);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 102;
            this.label5.Text = "Friction Angle";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 238);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(334, 13);
            this.label11.TabIndex = 97;
            this.label11.Text = "Density (Solid Unit Weight  = Density x (1-Porosity) and Porosiry = 0.2)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 132);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 95;
            this.label1.Text = "Young\'s Modulus";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 102);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 13);
            this.label19.TabIndex = 100;
            this.label19.Text = "Elastic Properties";
            // 
            // prLabel
            // 
            this.prLabel.AutoSize = true;
            this.prLabel.Location = new System.Drawing.Point(25, 184);
            this.prLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.prLabel.Name = "prLabel";
            this.prLabel.Size = new System.Drawing.Size(79, 13);
            this.prLabel.TabIndex = 96;
            this.prLabel.Text = "Poisson\'s Ratio";
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(565, 14);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 115;
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(565, 300);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 112;
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(565, 102);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 111;
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
            this.masterSelector.TabIndex = 116;
            this.masterSelector.Title = "Property Model";
            // 
            // _ymDragDrop
            // 
            this._ymDragDrop.AcceptedTypes = null;
            this._ymDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ymDragDrop.BackColor = System.Drawing.Color.White;
            this._ymDragDrop.DisplayImage = null;
            this._ymDragDrop.ErrorImage = null;
            this._ymDragDrop.ImageList = null;
            this._ymDragDrop.Location = new System.Drawing.Point(34, 151);
            this._ymDragDrop.Name = "_ymDragDrop";
            this._ymDragDrop.PlaceHolder = "Drop here a realized seismic cube";
            this._ymDragDrop.ReferenceName = null;
            this._ymDragDrop.Size = new System.Drawing.Size(525, 30);
            this._ymDragDrop.TabIndex = 113;
            this._ymDragDrop.Value = null;
            // 
            // frictionDragtDrop
            // 
            this.frictionDragtDrop.AcceptedTypes = null;
            this.frictionDragtDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frictionDragtDrop.BackColor = System.Drawing.Color.White;
            this.frictionDragtDrop.DisplayImage = null;
            this.frictionDragtDrop.ErrorImage = null;
            this.frictionDragtDrop.ImageList = null;
            this.frictionDragtDrop.Location = new System.Drawing.Point(34, 398);
            this.frictionDragtDrop.Name = "frictionDragtDrop";
            this.frictionDragtDrop.PlaceHolder = "Drag-drop here...";
            this.frictionDragtDrop.ReferenceName = null;
            this.frictionDragtDrop.Size = new System.Drawing.Size(525, 30);
            this.frictionDragtDrop.TabIndex = 110;
            this.frictionDragtDrop.Value = null;
            // 
            // ucsDragDrop
            // 
            this.ucsDragDrop.AcceptedTypes = null;
            this.ucsDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucsDragDrop.BackColor = System.Drawing.Color.White;
            this.ucsDragDrop.DisplayImage = null;
            this.ucsDragDrop.ErrorImage = null;
            this.ucsDragDrop.ImageList = null;
            this.ucsDragDrop.Location = new System.Drawing.Point(34, 346);
            this.ucsDragDrop.Name = "ucsDragDrop";
            this.ucsDragDrop.PlaceHolder = "Drag-drop here...";
            this.ucsDragDrop.ReferenceName = null;
            this.ucsDragDrop.Size = new System.Drawing.Size(525, 30);
            this.ucsDragDrop.TabIndex = 109;
            this.ucsDragDrop.Value = null;
            // 
            // _densityDragDrop
            // 
            this._densityDragDrop.AcceptedTypes = null;
            this._densityDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._densityDragDrop.BackColor = System.Drawing.Color.White;
            this._densityDragDrop.DisplayImage = null;
            this._densityDragDrop.ErrorImage = null;
            this._densityDragDrop.ImageList = null;
            this._densityDragDrop.Location = new System.Drawing.Point(34, 254);
            this._densityDragDrop.Name = "_densityDragDrop";
            this._densityDragDrop.PlaceHolder = "Drop here a realized seismic cube";
            this._densityDragDrop.ReferenceName = null;
            this._densityDragDrop.Size = new System.Drawing.Size(525, 30);
            this._densityDragDrop.TabIndex = 108;
            this._densityDragDrop.Value = null;
            // 
            // _prDragDrop
            // 
            this._prDragDrop.AcceptedTypes = null;
            this._prDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._prDragDrop.BackColor = System.Drawing.Color.White;
            this._prDragDrop.DisplayImage = null;
            this._prDragDrop.ErrorImage = null;
            this._prDragDrop.ImageList = null;
            this._prDragDrop.Location = new System.Drawing.Point(34, 200);
            this._prDragDrop.Name = "_prDragDrop";
            this._prDragDrop.PlaceHolder = "Drop here a realized seismic cube";
            this._prDragDrop.ReferenceName = null;
            this._prDragDrop.Size = new System.Drawing.Size(525, 30);
            this._prDragDrop.TabIndex = 107;
            this._prDragDrop.Value = null;
            // 
            // MaterialsModelsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.masterSelector);
            this.Controls.Add(this.toolTipHotspot3);
            this.Controls.Add(this._ymDragDrop);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.frictionDragtDrop);
            this.Controls.Add(this.ucsDragDrop);
            this.Controls.Add(this._densityDragDrop);
            this.Controls.Add(this._prDragDrop);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tensileStrengthRatio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.prLabel);
            this.Name = "MaterialsModelsControl";
            this.Size = new System.Drawing.Size(600, 600);
            ((System.ComponentModel.ISupportInitialize)(this.tensileStrengthRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown tensileStrengthRatio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label prLabel;
        private DatumDropTarget _prDragDrop;
        private DatumDropTarget _densityDragDrop;
        private DatumDropTarget ucsDragDrop;
        private DatumDropTarget frictionDragtDrop;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private DatumDropTarget _ymDragDrop;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private NewEditSelector masterSelector;
    }
}
