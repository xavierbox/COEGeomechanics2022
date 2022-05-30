using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Petrel.UI;
using Restoration.GFM.Model;

namespace Restoration.GFM.View
{
    public partial class Simulation1DProcessingControl : UserControl
    {

        public event EventHandler Request1DSimulationResults;

        public Simulation1DProcessingControl()
        {
            InitializeComponent();
        }

        public string SelectedCase
        {
            get
            {
                int index =  ColumnCasesComboBox.SelectedIndex;
                if (index < 0)
                    return string.Empty;

                return ColumnCasesComboBox.SelectedItem.Text;
            }
        }

        public int SelectedColumn
        {
            get
            {
                return (int)selectedPointcontrol.Value;
            }
        }

        public void Update1DSimulationList(Dictionary<string, int> simulationNameAndSteps)
        {
            ColumnCasesComboBox.Items.Clear();
            ColumnCasesComboBox.Tag = simulationNameAndSteps;
            if (simulationNameAndSteps == null)
            {
                selectedPointcontrol.Visible = false;
                return;
            }
            selectedPointcontrol.Visible = true;
            thicknessChart.Visible = true;

            var selectedItem = ColumnCasesComboBox.SelectedItem;
            int selectedPoint = (int)selectedPointcontrol.Value;


            ComboBoxItem last = null; 
            foreach (string sim in simulationNameAndSteps.Keys)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Image = PetrelImages.Case;
                item.Text = sim;
                item.Value = simulationNameAndSteps[sim];
                ColumnCasesComboBox.Items.Add(item);
                last = item;
            }

            ColumnCasesComboBox.SelectedItem = last;
             

        }

        private void ColumnCasesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            var i = ColumnCasesComboBox.SelectedIndex;
            Dictionary<string, int> Data  = (Dictionary<string, int>)(ColumnCasesComboBox.Tag);
            if (Data == null ) return;

            int maxSteps = (int)(ColumnCasesComboBox.SelectedValue);
            selectedPointcontrol.Maximum = maxSteps > 0 ? maxSteps + 1 : 0;
            selectedPointcontrol.Minimum = maxSteps > 0 ? 1 : 0;

            selectedPointcontrol.Value = Math.Min(maxSteps, selectedPointcontrol.Value);

     
            labelcounter.Text = selectedPointcontrol.Maximum.ToString();

        }

        private void selectedPointcontrol_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Request1DSimulationResults?.Invoke(this, EventArgs.Empty);

        }

        public void Display1DResults(SimulationResults1D results)
        {

        }

        private void Simulation1DProcessingControl_Load(object sender, EventArgs e)
        {

        }
    }
}
