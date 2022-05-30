
namespace CO2
{
    partial class CO2ProcessUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CO2ProcessUI));
            this._capillaryDragDrop = new OceanControlsLib.DatumDropTarget();
            this._porosityDragDrop = new OceanControlsLib.DatumDropTarget();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label19 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.CancelButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this._injectorsDragDrop = new OceanControlsLib.DatumDropTarget();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.presentationBox1 = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.toolTipHotspot3 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // _capillaryDragDrop
            // 
            this._capillaryDragDrop.AcceptedTypes = null;
            this._capillaryDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._capillaryDragDrop.BackColor = System.Drawing.Color.White;
            this._capillaryDragDrop.DisplayImage = null;
            this._capillaryDragDrop.ErrorImage = null;
            this._capillaryDragDrop.ImageList = null;
            this._capillaryDragDrop.Location = new System.Drawing.Point(33, 135);
            this._capillaryDragDrop.Name = "_capillaryDragDrop";
            this._capillaryDragDrop.PlaceHolder = "Drop here a realized seismic cube";
            this._capillaryDragDrop.ReferenceName = null;
            this._capillaryDragDrop.Size = new System.Drawing.Size(526, 30);
            this._capillaryDragDrop.TabIndex = 115;
            this._capillaryDragDrop.Value = null;
            // 
            // _porosityDragDrop
            // 
            this._porosityDragDrop.AcceptedTypes = null;
            this._porosityDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._porosityDragDrop.BackColor = System.Drawing.Color.White;
            this._porosityDragDrop.DisplayImage = null;
            this._porosityDragDrop.ErrorImage = null;
            this._porosityDragDrop.ImageList = null;
            this._porosityDragDrop.Location = new System.Drawing.Point(33, 190);
            this._porosityDragDrop.Name = "_porosityDragDrop";
            this._porosityDragDrop.PlaceHolder = "Drop here a realized seismic cube";
            this._porosityDragDrop.ReferenceName = null;
            this._porosityDragDrop.Size = new System.Drawing.Size(526, 30);
            this._porosityDragDrop.TabIndex = 114;
            this._porosityDragDrop.Value = null;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(123, 91);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(436, 2);
            this.flowLayoutPanel1.TabIndex = 116;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(30, 84);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 13);
            this.label19.TabIndex = 117;
            this.label19.Text = "Rock Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 118;
            this.label1.Text = "Capillary pressure";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 119;
            this.label2.Text = "Porosity";
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot1.Location = new System.Drawing.Point(571, 81);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 120;
            // 
            // Icons
            // 
            this.Icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons.ImageStream")));
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons.Images.SetKeyName(0, "TopTriangle.png");
            this.Icons.Images.SetKeyName(1, "BottomTriangle.png");
            this.Icons.Images.SetKeyName(2, "112_LeftArrowShort_Blue_16x16_72.png");
            this.Icons.Images.SetKeyName(3, "112_RightArrowShort_Blue_16x16_72.png");
            this.Icons.Images.SetKeyName(4, "delete.png");
            this.Icons.Images.SetKeyName(5, "DiffStress_32.bmp");
            this.Icons.Images.SetKeyName(6, "Exel.png");
            this.Icons.Images.SetKeyName(7, "FH_32.png");
            this.Icons.Images.SetKeyName(8, "HeimdallPlane.png");
            this.Icons.Images.SetKeyName(9, "Normalize_24.png");
            this.Icons.Images.SetKeyName(10, "oilwell.png");
            this.Icons.Images.SetKeyName(11, "PoroelasticCube_32.bmp");
            this.Icons.Images.SetKeyName(12, "QFactor_32.bmp");
            this.Icons.Images.SetKeyName(13, "QFactor_32.png");
            this.Icons.Images.SetKeyName(14, "QScreener24.png");
            this.Icons.Images.SetKeyName(15, "QScreener32.bmp");
            this.Icons.Images.SetKeyName(16, "reload.png");
            this.Icons.Images.SetKeyName(17, "Tectonics_32.bmp");
            this.Icons.Images.SetKeyName(18, "base_checkmark_32.png");
            this.Icons.Images.SetKeyName(19, "base_cog_32.png");
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CancelButton.ImageKey = "delete.png";
            this.CancelButton.ImageList = this.Icons;
            this.CancelButton.Location = new System.Drawing.Point(319, 509);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 123;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ApplyButton.ImageKey = "base_checkmark_32.png";
            this.ApplyButton.ImageList = this.Icons;
            this.ApplyButton.Location = new System.Drawing.Point(400, 509);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 122;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // RunButton
            // 
            this.RunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RunButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RunButton.ImageKey = "base_cog_32.png";
            this.RunButton.ImageList = this.Icons;
            this.RunButton.Location = new System.Drawing.Point(481, 509);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 121;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(563, 258);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 126;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(115, 268);
            this.flowLayoutPanel2.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(441, 2);
            this.flowLayoutPanel2.TabIndex = 124;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 125;
            this.label3.Text = "Injectors";
            // 
            // _injectorsDragDrop
            // 
            this._injectorsDragDrop.AcceptedTypes = null;
            this._injectorsDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._injectorsDragDrop.BackColor = System.Drawing.Color.White;
            this._injectorsDragDrop.DisplayImage = null;
            this._injectorsDragDrop.ErrorImage = null;
            this._injectorsDragDrop.ImageList = null;
            this._injectorsDragDrop.Location = new System.Drawing.Point(33, 284);
            this._injectorsDragDrop.Name = "_injectorsDragDrop";
            this._injectorsDragDrop.PlaceHolder = "Drop a well folder";
            this._injectorsDragDrop.ReferenceName = null;
            this._injectorsDragDrop.Size = new System.Drawing.Size(523, 30);
            this._injectorsDragDrop.TabIndex = 127;
            this._injectorsDragDrop.Value = null;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(123, 31);
            this.flowLayoutPanel3.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel3.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(436, 2);
            this.flowLayoutPanel3.TabIndex = 117;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 128;
            this.label4.Text = "Simulation name";
            // 
            // presentationBox1
            // 
            this.presentationBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presentationBox1.Location = new System.Drawing.Point(37, 42);
            this.presentationBox1.Name = "presentationBox1";
            this.presentationBox1.Size = new System.Drawing.Size(522, 22);
            this.presentationBox1.TabIndex = 129;
            // 
            // toolTipHotspot3
            // 
            this.toolTipHotspot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot3.Location = new System.Drawing.Point(571, 17);
            this.toolTipHotspot3.Name = "toolTipHotspot3";
            this.toolTipHotspot3.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot3.TabIndex = 131;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(37, 323);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(484, 145);
            this.dataGridView1.TabIndex = 132;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.ImageKey = "delete.png";
            this.button1.ImageList = this.Icons;
            this.button1.Location = new System.Drawing.Point(524, 323);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 27);
            this.button1.TabIndex = 133;
            this.button1.Text = "   ";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.ImageKey = "TopTriangle.png";
            this.button2.ImageList = this.Icons;
            this.button2.Location = new System.Drawing.Point(524, 352);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(32, 27);
            this.button2.TabIndex = 134;
            this.button2.Text = "   ";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.ImageKey = "BottomTriangle.png";
            this.button3.ImageList = this.Icons;
            this.button3.Location = new System.Drawing.Point(524, 381);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 27);
            this.button3.TabIndex = 135;
            this.button3.Text = "   ";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // CO2ProcessUI
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolTipHotspot3);
            this.Controls.Add(this.presentationBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this._injectorsDragDrop);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this._capillaryDragDrop);
            this.Controls.Add(this._porosityDragDrop);
            this.Name = "CO2ProcessUI";
            this.Size = new System.Drawing.Size(609, 551);
            this.Load += new System.EventHandler(this.CO2ProcessUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private OceanControlsLib.DatumDropTarget _capillaryDragDrop;
        private OceanControlsLib.DatumDropTarget _porosityDragDrop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private System.Windows.Forms.ImageList Icons;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button RunButton;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private OceanControlsLib.DatumDropTarget _injectorsDragDrop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox presentationBox1;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}
