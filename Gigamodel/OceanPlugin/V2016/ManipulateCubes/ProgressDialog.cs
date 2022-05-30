using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManipulateCubes
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
              button1.Click += new System.EventHandler(Start);

        }

        public void Start(object sender, EventArgs e )
        {
            for (int n = 0; n < 50; n++)
            {
                System.Threading.Thread.Sleep(200);
                progressBar2.Value = Math.Min(100, progressBar2.Value + 5);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Start( sender, e );

            this.Close();
        }
    }
}
