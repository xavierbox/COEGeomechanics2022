namespace Gigamodel
{
    partial class BigDataCalculatorUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
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
            this.propertyTable = new System.Windows.Forms.DataGridView();
            this.close = new System.Windows.Forms.Button();
            this.accept = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.historyTextBox = new System.Windows.Forms.RichTextBox();
            this.checkButton = new System.Windows.Forms.Button();
            this.movePropertyUp = new System.Windows.Forms.Button();
            this.moveProperyDown = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dropProperty = new Slb.Ocean.Petrel.UI.DropTarget();
            this.dropTarget2 = new Slb.Ocean.Petrel.UI.DropTarget();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteProperty = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.propertyTable)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyTable
            // 
            this.propertyTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propertyTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column1});
            this.propertyTable.Location = new System.Drawing.Point(40, 162);
            this.propertyTable.Name = "propertyTable";
            this.propertyTable.Size = new System.Drawing.Size(565, 184);
            this.propertyTable.TabIndex = 0;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(530, 358);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "button1";
            this.close.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            this.accept.Location = new System.Drawing.Point(449, 358);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 2;
            this.accept.Text = "button2";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 136);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(543, 20);
            this.textBox1.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // historyTextBox
            // 
            this.historyTextBox.Location = new System.Drawing.Point(8, 3);
            this.historyTextBox.Name = "historyTextBox";
            this.historyTextBox.Size = new System.Drawing.Size(599, 127);
            this.historyTextBox.TabIndex = 5;
            this.historyTextBox.Text = "";
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(557, 134);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(48, 23);
            this.checkButton.TabIndex = 6;
            this.checkButton.Text = "button3";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // movePropertyUp
            // 
            this.movePropertyUp.Location = new System.Drawing.Point(8, 191);
            this.movePropertyUp.Name = "movePropertyUp";
            this.movePropertyUp.Size = new System.Drawing.Size(26, 23);
            this.movePropertyUp.TabIndex = 7;
            this.movePropertyUp.Text = "button4";
            this.movePropertyUp.UseVisualStyleBackColor = true;
            // 
            // moveProperyDown
            // 
            this.moveProperyDown.Location = new System.Drawing.Point(8, 220);
            this.moveProperyDown.Name = "moveProperyDown";
            this.moveProperyDown.Size = new System.Drawing.Size(26, 23);
            this.moveProperyDown.TabIndex = 8;
            this.moveProperyDown.Text = "button5";
            this.moveProperyDown.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(247, 371);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(48, 23);
            this.button6.TabIndex = 9;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // dropProperty
            // 
            this.dropProperty.AllowDrop = true;
            this.dropProperty.Location = new System.Drawing.Point(8, 162);
            this.dropProperty.Name = "dropProperty";
            this.dropProperty.Size = new System.Drawing.Size(26, 23);
            this.dropProperty.TabIndex = 10;
            this.dropProperty.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropTarget1_DragDrop);
            // 
            // dropTarget2
            // 
            this.dropTarget2.AllowDrop = true;
            this.dropTarget2.Location = new System.Drawing.Point(162, 363);
            this.dropTarget2.Name = "dropTarget2";
            this.dropTarget2.Size = new System.Drawing.Size(54, 31);
            this.dropTarget2.TabIndex = 11;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 10F;
            this.Column2.HeaderText = "Variable";
            this.Column2.Name = "Column2";
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Property";
            this.Column1.Name = "Column1";
            // 
            // deleteProperty
            // 
            this.deleteProperty.Location = new System.Drawing.Point(8, 249);
            this.deleteProperty.Name = "deleteProperty";
            this.deleteProperty.Size = new System.Drawing.Size(26, 23);
            this.deleteProperty.TabIndex = 12;
            this.deleteProperty.Text = "button5";
            this.deleteProperty.UseVisualStyleBackColor = true;
            // 
            // BigDataCalculatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteProperty);
            this.Controls.Add(this.dropTarget2);
            this.Controls.Add(this.dropProperty);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.moveProperyDown);
            this.Controls.Add(this.movePropertyUp);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.historyTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.close);
            this.Controls.Add(this.propertyTable);
            this.Name = "BigDataCalculatorUI";
            this.Size = new System.Drawing.Size(616, 405);
            ((System.ComponentModel.ISupportInitialize)(this.propertyTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView propertyTable;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.RichTextBox historyTextBox;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button movePropertyUp;
        private System.Windows.Forms.Button moveProperyDown;
        private System.Windows.Forms.Button button6;
        private Slb.Ocean.Petrel.UI.DropTarget dropProperty;
        private Slb.Ocean.Petrel.UI.DropTarget dropTarget2;
        private System.Windows.Forms.Button deleteProperty;
    }
}
