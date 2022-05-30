namespace Gigamodel
{
    partial class GigaModelProcessUI
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
            this.buttonExportImport = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.materialsTab = new System.Windows.Forms.TabPage();
            this.pressuresTab = new System.Windows.Forms.TabPage();
            this.boundariesTab = new System.Windows.Forms.TabPage();
            this.simulationsTab = new System.Windows.Forms.TabPage();
            this.resultsTab = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.deleteButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExportImport
            // 
            this.buttonExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExportImport.Location = new System.Drawing.Point(148, 3);
            this.buttonExportImport.Name = "buttonExportImport";
            this.buttonExportImport.Size = new System.Drawing.Size(75, 28);
            this.buttonExportImport.TabIndex = 109;
            this.buttonExportImport.Text = "Export";
            this.buttonExportImport.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveButton.Location = new System.Drawing.Point(229, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 28);
            this.saveButton.TabIndex = 108;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
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
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.materialsTab);
            this.tabControl.Controls.Add(this.pressuresTab);
            this.tabControl.Controls.Add(this.boundariesTab);
            this.tabControl.Controls.Add(this.simulationsTab);
            this.tabControl.Controls.Add(this.resultsTab);
            this.tabControl.ItemSize = new System.Drawing.Size(117, 30);
            this.tabControl.Location = new System.Drawing.Point(6, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(575, 600);
            this.tabControl.TabIndex = 110;
            // 
            // materialsTab
            // 
            this.materialsTab.Location = new System.Drawing.Point(4, 34);
            this.materialsTab.Name = "materialsTab";
            this.materialsTab.Size = new System.Drawing.Size(567, 562);
            this.materialsTab.TabIndex = 0;
            this.materialsTab.Text = "Mechanical Properties";
            this.materialsTab.UseVisualStyleBackColor = true;
            // 
            // pressuresTab
            // 
            this.pressuresTab.Location = new System.Drawing.Point(4, 34);
            this.pressuresTab.Name = "pressuresTab";
            this.pressuresTab.Size = new System.Drawing.Size(567, 562);
            this.pressuresTab.TabIndex = 1;
            this.pressuresTab.Text = "Pressure Model";
            this.pressuresTab.UseVisualStyleBackColor = true;
            // 
            // boundariesTab
            // 
            this.boundariesTab.Location = new System.Drawing.Point(4, 34);
            this.boundariesTab.Name = "boundariesTab";
            this.boundariesTab.Size = new System.Drawing.Size(567, 562);
            this.boundariesTab.TabIndex = 2;
            this.boundariesTab.Text = "Boundary Conditions";
            this.boundariesTab.UseVisualStyleBackColor = true;
            // 
            // simulationsTab
            // 
            this.simulationsTab.Location = new System.Drawing.Point(4, 34);
            this.simulationsTab.Name = "simulationsTab";
            this.simulationsTab.Size = new System.Drawing.Size(567, 562);
            this.simulationsTab.TabIndex = 3;
            this.simulationsTab.Text = "Simulation";
            this.simulationsTab.UseVisualStyleBackColor = true;
            // 
            // resultsTab
            // 
            this.resultsTab.Location = new System.Drawing.Point(4, 34);
            this.resultsTab.Name = "resultsTab";
            this.resultsTab.Size = new System.Drawing.Size(567, 562);
            this.resultsTab.TabIndex = 4;
            this.resultsTab.Text = "Import Results";
            this.resultsTab.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Controls.Add(this.buttonExportImport);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(186, 609);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(388, 36);
            this.flowLayoutPanel1.TabIndex = 113;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(67, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 28);
            this.deleteButton.TabIndex = 114;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 612);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 114;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GigaModelProcessUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabControl);
            this.Name = "GigaModelProcessUI";
            this.Size = new System.Drawing.Size(581, 650);
            this.tabControl.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExportImport;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage materialsTab;
        private System.Windows.Forms.TabPage pressuresTab;
        private System.Windows.Forms.TabPage boundariesTab;
        private System.Windows.Forms.TabPage simulationsTab;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage resultsTab;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button button1;
    }
}
