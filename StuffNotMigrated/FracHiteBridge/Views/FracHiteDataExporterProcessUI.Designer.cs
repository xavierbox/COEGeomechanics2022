namespace FracHiteBridge
{
    partial class FracHiteDataExporterProcessUI
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
            this.toolTipHotspot1 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this._folderButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.folderPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this._propsDropPresentationBox = new OceanControls.DropPresentationBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // toolTipHotspot1
            // 
            this.toolTipHotspot1.Location = new System.Drawing.Point(590, 18);
            this.toolTipHotspot1.Name = "toolTipHotspot1";
            this.toolTipHotspot1.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot1.TabIndex = 2;
            // 
            // _cancelButton
            // 
            this._cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._cancelButton.Location = new System.Drawing.Point(480, 157);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(101, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._acceptButton.Location = new System.Drawing.Point(373, 157);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(101, 23);
            this._acceptButton.TabIndex = 4;
            this._acceptButton.Text = "Accept";
            this._acceptButton.UseVisualStyleBackColor = true;
            // 
            // _folderButton
            // 
            this._folderButton.Location = new System.Drawing.Point(6, 45);
            this._folderButton.Name = "_folderButton";
            this._folderButton.Size = new System.Drawing.Size(25, 23);
            this._folderButton.TabIndex = 5;
            this._folderButton.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // folderPresentationBox
            // 
            this.folderPresentationBox.Location = new System.Drawing.Point(36, 45);
            this.folderPresentationBox.Name = "folderPresentationBox";
            this.folderPresentationBox.Size = new System.Drawing.Size(549, 22);
            this.folderPresentationBox.TabIndex = 8;
            // 
            // _propsDropPresentationBox
            // 
            this._propsDropPresentationBox.AcceptedTypes = null;
            this._propsDropPresentationBox.BackColor = System.Drawing.Color.White;
            this._propsDropPresentationBox.DisplayImage = null;
            this._propsDropPresentationBox.ErrorImage = null;
            this._propsDropPresentationBox.ImageList = null;
            this._propsDropPresentationBox.Location = new System.Drawing.Point(6, 6);
            this._propsDropPresentationBox.Name = "_propsDropPresentationBox";
            this._propsDropPresentationBox.PlaceHolder = "Drop a property folder from the grid";
            this._propsDropPresentationBox.ReferenceName = null;
            this._propsDropPresentationBox.Size = new System.Drawing.Size(581, 33);
            this._propsDropPresentationBox.TabIndex = 1;
            this._propsDropPresentationBox.Value = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // FracHiteDataExporterProcessUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.folderPresentationBox);
            this.Controls.Add(this._folderButton);
            this.Controls.Add(this._acceptButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this.toolTipHotspot1);
            this.Controls.Add(this._propsDropPresentationBox);
            this.Name = "FracHiteDataExporterProcessUI";
            this.Size = new System.Drawing.Size(621, 201);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OceanControls.DropPresentationBox _propsDropPresentationBox;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _folderButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox folderPresentationBox;
        private System.Windows.Forms.Label label1;
    }
}
