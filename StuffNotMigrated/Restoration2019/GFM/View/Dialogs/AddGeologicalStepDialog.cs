using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Restoration.GFM
{
    public partial class AddGeologicalStepDialog : Form 
    {
        public AddGeologicalStepDialog()
        {
            InitializeComponent();
            textBox1.Text = string.Empty;
            this.AcceptButton = button1;
            this.CancelButton = button2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // return DialogResult.OK;
            if (textBox1.Text != string.Empty)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid Name for the step");
                textBox1.Text = string.Empty;
                //this.DialogResult = DialogResult.Cancel;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;
            this.DialogResult = DialogResult.Cancel;
            // return DialogResult.Cancel;
        }

        public string NameEntered { get { return textBox1.Text;  }  }
    }
}
