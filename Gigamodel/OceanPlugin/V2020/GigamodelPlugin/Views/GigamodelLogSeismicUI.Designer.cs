using OceanControlsLib;

namespace Gigamodel
{
    partial class GigamodelLogSeismicUI
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
            Slb.Ocean.Petrel.UI.Controls.ComboBoxItem comboBoxItem1 = new Slb.Ocean.Petrel.UI.Controls.ComboBoxItem();
            Slb.Ocean.Petrel.UI.Controls.ComboBoxItem comboBoxItem2 = new Slb.Ocean.Petrel.UI.Controls.ComboBoxItem();
            Slb.Ocean.Petrel.UI.Controls.ComboBoxItem comboBoxItem3 = new Slb.Ocean.Petrel.UI.Controls.ComboBoxItem();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.dropTarget1 = new Slb.Ocean.Petrel.UI.DropTarget();
            this._seismicDragDrop = new DatumDropTarget();
            this._wellsDragDrop = new DatumDropTarget();
            this.comboBox1 = new Slb.Ocean.Petrel.UI.Controls.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "Well/well folder";
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(709, 30);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 111;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 13);
            this.label2.TabIndex = 140;
            this.label2.Text = "Seismic cube/seismic collection";
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(709, 88);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 141;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Controls.Add(this.dropTarget1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(310, 229);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(388, 36);
            this.flowLayoutPanel1.TabIndex = 142;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.Location = new System.Drawing.Point(310, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 28);
            this.cancelButton.TabIndex = 107;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveButton.Location = new System.Drawing.Point(229, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 28);
            this.saveButton.TabIndex = 108;
            this.saveButton.Text = "Create";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // dropTarget1
            // 
            this.dropTarget1.AllowDrop = true;
            this.dropTarget1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dropTarget1.Location = new System.Drawing.Point(197, 3);
            this.dropTarget1.Name = "dropTarget1";
            this.dropTarget1.Size = new System.Drawing.Size(26, 28);
            this.dropTarget1.TabIndex = 143;
            this.dropTarget1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropTarget1_DragDrop);
            // 
            // _seismicDragDrop
            // 
            this._seismicDragDrop.AcceptedTypes = null;
            this._seismicDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._seismicDragDrop.BackColor = System.Drawing.Color.White;
            this._seismicDragDrop.DisplayImage = null;
            this._seismicDragDrop.ErrorImage = null;
            this._seismicDragDrop.ImageList = null;
            this._seismicDragDrop.Location = new System.Drawing.Point(17, 88);
            this._seismicDragDrop.Name = "_seismicDragDrop";
            this._seismicDragDrop.PlaceHolder = "Drop a a seismic cube or a folder containing seismic cubes";
            this._seismicDragDrop.ReferenceName = null;
            this._seismicDragDrop.Size = new System.Drawing.Size(681, 30);
            this._seismicDragDrop.TabIndex = 139;
            this._seismicDragDrop.Value = null;
            // 
            // _wellsDragDrop
            // 
            this._wellsDragDrop.AcceptedTypes = null;
            this._wellsDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._wellsDragDrop.BackColor = System.Drawing.Color.White;
            this._wellsDragDrop.DisplayImage = null;
            this._wellsDragDrop.ErrorImage = null;
            this._wellsDragDrop.ImageList = null;
            this._wellsDragDrop.Location = new System.Drawing.Point(17, 30);
            this._wellsDragDrop.Name = "_wellsDragDrop";
            this._wellsDragDrop.PlaceHolder = "Dropa well or a collection of wells";
            this._wellsDragDrop.ReferenceName = null;
            this._wellsDragDrop.Size = new System.Drawing.Size(681, 30);
            this._wellsDragDrop.TabIndex = 109;
            this._wellsDragDrop.Value = null;
            // 
            // comboBox1
            // 
            comboBoxItem1.Text = "0.5 Seismic resolution";
            comboBoxItem2.Text = "0.5 m";
            comboBoxItem3.Text = "0.5 feet";
            this.comboBox1.Items.AddRange(new Slb.Ocean.Petrel.UI.Controls.ComboBoxItem[] {
            comboBoxItem1,
            comboBoxItem2,
            comboBoxItem3});
            this.comboBox1.Location = new System.Drawing.Point(20, 156);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(174, 22);
            this.comboBox1.TabIndex = 145;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 146;
            this.label4.Text = "Logs Resolution";
            // 
            // GigamodelLogSeismicUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._seismicDragDrop);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._wellsDragDrop);
            this.Name = "GigamodelLogSeismicUI";
            this.Size = new System.Drawing.Size(749, 286);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DatumDropTarget _wellsDragDrop;
        private System.Windows.Forms.Label label1;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private DatumDropTarget _seismicDragDrop;
        private System.Windows.Forms.Label label2;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private Slb.Ocean.Petrel.UI.DropTarget dropTarget1;
        private Slb.Ocean.Petrel.UI.Controls.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
    }
}
