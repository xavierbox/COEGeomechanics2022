namespace Gigamodel
{
    partial class ImportResultsControl
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.titleLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTipHotspot2 = new Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot(this.components);
            this.caseNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.simFolderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectSimulationFolder = new System.Windows.Forms.Button();
            this.resultsTree = new System.Windows.Forms.TreeView();
            this.casesGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeStepsComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.casesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 11);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(93, 13);
            this.titleLabel.TabIndex = 131;
            this.titleLabel.Text = "Simulation Results";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel5.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(110, 18);
            this.flowLayoutPanel5.MaximumSize = new System.Drawing.Size(2000, 2);
            this.flowLayoutPanel5.MinimumSize = new System.Drawing.Size(20, 2);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(450, 2);
            this.flowLayoutPanel5.TabIndex = 132;
            // 
            // toolTipHotspot2
            // 
            this.toolTipHotspot2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolTipHotspot2.Location = new System.Drawing.Point(565, 14);
            this.toolTipHotspot2.Name = "toolTipHotspot2";
            this.toolTipHotspot2.Size = new System.Drawing.Size(20, 20);
            this.toolTipHotspot2.TabIndex = 130;
            this.toolTipHotspot2.Click += new System.EventHandler(this.toolTipHotspot2_Click);
            // 
            // caseNameTextBox
            // 
            this.caseNameTextBox.Enabled = false;
            this.caseNameTextBox.Location = new System.Drawing.Point(67, 79);
            this.caseNameTextBox.Name = "caseNameTextBox";
            this.caseNameTextBox.Size = new System.Drawing.Size(209, 20);
            this.caseNameTextBox.TabIndex = 128;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 127;
            this.label2.Text = "Cases";
            // 
            // simFolderTextBox
            // 
            this.simFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simFolderTextBox.Enabled = false;
            this.simFolderTextBox.Location = new System.Drawing.Point(67, 53);
            this.simFolderTextBox.Name = "simFolderTextBox";
            this.simFolderTextBox.Size = new System.Drawing.Size(490, 20);
            this.simFolderTextBox.TabIndex = 126;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 125;
            this.label1.Text = "Simulation Folder";
            // 
            // selectSimulationFolder
            // 
            //this.selectSimulationFolder.Image = global::Gigamodel.Properties.Resources.Folder_16x16;
            this.selectSimulationFolder.Location = new System.Drawing.Point(518, 553);
            this.selectSimulationFolder.Name = "selectSimulationFolder";
            this.selectSimulationFolder.Size = new System.Drawing.Size(39, 22);
            this.selectSimulationFolder.TabIndex = 124;
            this.selectSimulationFolder.UseVisualStyleBackColor = true;
            this.selectSimulationFolder.Visible = false;
            // 
            // resultsTree
            // 
            this.resultsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultsTree.CheckBoxes = true;
            this.resultsTree.Location = new System.Drawing.Point(282, 104);
            this.resultsTree.Name = "resultsTree";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Node4";
            treeNode5.Name = "Node0";
            treeNode5.Text = "Node0";
            this.resultsTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.resultsTree.Size = new System.Drawing.Size(275, 443);
            this.resultsTree.TabIndex = 139;
            // 
            // casesGrid
            // 
            this.casesGrid.AllowUserToAddRows = false;
            this.casesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.casesGrid.BackgroundColor = System.Drawing.Color.White;
            this.casesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.casesGrid.ColumnHeadersVisible = false;
            this.casesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.casesGrid.GridColor = System.Drawing.Color.White;
            this.casesGrid.Location = new System.Drawing.Point(36, 105);
            this.casesGrid.MultiSelect = false;
            this.casesGrid.Name = "casesGrid";
            this.casesGrid.ReadOnly = true;
            this.casesGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.casesGrid.RowHeadersVisible = false;
            this.casesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.casesGrid.Size = new System.Drawing.Size(240, 442);
            this.casesGrid.TabIndex = 141;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 20F;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 80F;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // timeStepsComboBox
            // 
            this.timeStepsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeStepsComboBox.FormattingEnabled = true;
            this.timeStepsComboBox.Location = new System.Drawing.Point(282, 78);
            this.timeStepsComboBox.Name = "timeStepsComboBox";
            this.timeStepsComboBox.Size = new System.Drawing.Size(275, 21);
            this.timeStepsComboBox.TabIndex = 142;
            // 
            // ImportResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeStepsComboBox);
            this.Controls.Add(this.simFolderTextBox);
            this.Controls.Add(this.selectSimulationFolder);
            this.Controls.Add(this.casesGrid);
            this.Controls.Add(this.resultsTree);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.flowLayoutPanel5);
            this.Controls.Add(this.toolTipHotspot2);
            this.Controls.Add(this.caseNameTextBox);
            this.Controls.Add(this.label2);
            this.Name = "ImportResultsControl";
            this.Size = new System.Drawing.Size(600, 600);
            ((System.ComponentModel.ISupportInitialize)(this.casesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private Slb.Ocean.Petrel.UI.Controls.ToolTipHotspot toolTipHotspot2;
        private System.Windows.Forms.TextBox caseNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox simFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectSimulationFolder;
        private System.Windows.Forms.TreeView resultsTree;
        private System.Windows.Forms.DataGridView casesGrid;
        private System.Windows.Forms.ComboBox timeStepsComboBox;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}
