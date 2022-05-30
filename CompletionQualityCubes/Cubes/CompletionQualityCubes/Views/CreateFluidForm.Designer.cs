
namespace CompletionQualityCubes.Views
{
    partial class CreateFluidForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FluidNameControl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FluidViscosityControl = new Slb.Ocean.Petrel.UI.Controls.UnitTextBox();
            this.FluidDensityControl = new Slb.Ocean.Petrel.UI.Controls.UnitTextBox();
            this.DensityUnits = new System.Windows.Forms.Label();
            this.ViscosityUnits = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "Density";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Viscosity";
            // 
            // FluidNameControl
            // 
            this.FluidNameControl.Location = new System.Drawing.Point(86, 35);
            this.FluidNameControl.MinimumSize = new System.Drawing.Size(4, 20);
            this.FluidNameControl.Name = "FluidNameControl";
            this.FluidNameControl.Size = new System.Drawing.Size(120, 20);
            this.FluidNameControl.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Name";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(270, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 46;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(270, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 47;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FluidViscosityControl
            // 
            this.FluidViscosityControl.Location = new System.Drawing.Point(86, 63);
            this.FluidViscosityControl.Name = "FluidViscosityControl";
            this.FluidViscosityControl.Size = new System.Drawing.Size(92, 20);
            this.FluidViscosityControl.TabIndex = 48;
            // 
            // FluidDensityControl
            // 
            this.FluidDensityControl.Location = new System.Drawing.Point(86, 89);
            this.FluidDensityControl.Name = "FluidDensityControl";
            this.FluidDensityControl.Size = new System.Drawing.Size(92, 20);
            this.FluidDensityControl.TabIndex = 49;
            // 
            // DensityUnits
            // 
            this.DensityUnits.AutoSize = true;
            this.DensityUnits.Location = new System.Drawing.Point(185, 91);
            this.DensityUnits.Name = "DensityUnits";
            this.DensityUnits.Size = new System.Drawing.Size(29, 13);
            this.DensityUnits.TabIndex = 52;
            this.DensityUnits.Text = "units";
            // 
            // ViscosityUnits
            // 
            this.ViscosityUnits.AutoSize = true;
            this.ViscosityUnits.Location = new System.Drawing.Point(185, 67);
            this.ViscosityUnits.Name = "ViscosityUnits";
            this.ViscosityUnits.Size = new System.Drawing.Size(29, 13);
            this.ViscosityUnits.TabIndex = 51;
            this.ViscosityUnits.Text = "units";
            // 
            // CreateFluidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 141);
            this.Controls.Add(this.DensityUnits);
            this.Controls.Add(this.ViscosityUnits);
            this.Controls.Add(this.FluidDensityControl);
            this.Controls.Add(this.FluidViscosityControl);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FluidNameControl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Name = "CreateFluidForm";
            this.Text = "CreateFluidForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox FluidNameControl;
        private Slb.Ocean.Petrel.UI.Controls.UnitTextBox FluidViscosityControl;
        private Slb.Ocean.Petrel.UI.Controls.UnitTextBox FluidDensityControl;
        private System.Windows.Forms.Label DensityUnits;
        private System.Windows.Forms.Label ViscosityUnits;
    }
}