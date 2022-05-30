using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Restoration.GFM.View
{
    public partial class ChartConfigurationDialog : Form
    {
        public ChartConfigurationDialog()
        {
            InitializeComponent();
        }

        public Object Chart
        {
            get { return propertyGrid1.SelectedObject;
                }
            set { propertyGrid1.SelectedObject = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            this.Close();
        }
    
    }
}
