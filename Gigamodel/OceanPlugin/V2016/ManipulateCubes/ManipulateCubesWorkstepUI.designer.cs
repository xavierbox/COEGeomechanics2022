namespace ManipulateCubes
{
    partial class ManipulateCubesWorkstepUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManipulateCubesWorkstepUI));
            this.gigaModelTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pressureModelsControl2 = new ManipulateCubes.PressureModelsControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.boundaryConditionsControl2 = new ManipulateCubes.BoundaryConditionsControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.simulationsControl1 = new ManipulateCubes.SimulationsControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.exportbutton = new System.Windows.Forms.Button();
            this.gigaModelTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // gigaModelTabControl
            // 
            this.gigaModelTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gigaModelTabControl.Controls.Add(this.tabPage1);
            this.gigaModelTabControl.Controls.Add(this.tabPage2);
            this.gigaModelTabControl.Controls.Add(this.tabPage3);
            this.gigaModelTabControl.Controls.Add(this.tabPage4);
            this.gigaModelTabControl.ImageList = this.imageList1;
            this.gigaModelTabControl.ItemSize = new System.Drawing.Size(117, 30);
            this.gigaModelTabControl.Location = new System.Drawing.Point(3, 3);
            this.gigaModelTabControl.Name = "gigaModelTabControl";
            this.gigaModelTabControl.SelectedIndex = 0;
            this.gigaModelTabControl.Size = new System.Drawing.Size(637, 650);
            this.gigaModelTabControl.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(629, 612);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mechanical Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pressureModelsControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(629, 612);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pressure Model";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pressureModelsControl2
            // 
            this.pressureModelsControl2.BackColor = System.Drawing.Color.White;
            this.pressureModelsControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pressureModelsControl2.Location = new System.Drawing.Point(0, 0);
            this.pressureModelsControl2.Name = "pressureModelsControl2";
            this.pressureModelsControl2.Size = new System.Drawing.Size(629, 612);
            this.pressureModelsControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.boundaryConditionsControl2);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(629, 612);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Boundary Conditions";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // boundaryConditionsControl2
            // 
            this.boundaryConditionsControl2.BackColor = System.Drawing.Color.White;
            this.boundaryConditionsControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boundaryConditionsControl2.Location = new System.Drawing.Point(0, 0);
            this.boundaryConditionsControl2.MinimumSize = new System.Drawing.Size(500, 575);
            this.boundaryConditionsControl2.Name = "boundaryConditionsControl2";
            this.boundaryConditionsControl2.Size = new System.Drawing.Size(629, 612);
            this.boundaryConditionsControl2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.simulationsControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(629, 612);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Simulation";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // simulationsControl1
            // 
            this.simulationsControl1.BackColor = System.Drawing.Color.White;
            this.simulationsControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.simulationsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulationsControl1.Location = new System.Drawing.Point(0, 0);
            this.simulationsControl1.Name = "simulationsControl1";
            this.simulationsControl1.Size = new System.Drawing.Size(629, 612);
            this.simulationsControl1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "112_Tick_Green.ico");
            this.imageList1.Images.SetKeyName(1, "Annotate_Default.ico");
            this.imageList1.Images.SetKeyName(2, "Annotate_info.ico");
            this.imageList1.Images.SetKeyName(3, "delete.ico");
            this.imageList1.Images.SetKeyName(4, "help.ico");
            this.imageList1.Images.SetKeyName(5, "warn.ico");
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList3.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ImageList = this.imageList1;
            this.saveButton.Location = new System.Drawing.Point(462, 670);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 24;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.ImageList = this.imageList1;
            this.cancelButton.Location = new System.Drawing.Point(541, 670);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 25;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // exportbutton
            // 
            this.exportbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportbutton.ImageList = this.imageList1;
            this.exportbutton.Location = new System.Drawing.Point(381, 670);
            this.exportbutton.Name = "exportbutton";
            this.exportbutton.Size = new System.Drawing.Size(75, 23);
            this.exportbutton.TabIndex = 26;
            this.exportbutton.Text = "Export";
            this.exportbutton.UseVisualStyleBackColor = true;
            this.exportbutton.Visible = false;
            // 
            // ManipulateCubesWorkstepUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.exportbutton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.gigaModelTabControl);
            this.Name = "ManipulateCubesWorkstepUI";
            this.Size = new System.Drawing.Size(643, 704);
            this.gigaModelTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl gigaModelTabControl;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private PressureModelsControl pressureModelsControl2;
        private BoundaryConditionsControl boundaryConditionsControl2;
        private SimulationsControl simulationsControl1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button exportbutton;
    }
}
