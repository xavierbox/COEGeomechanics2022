using Slb.Ocean.Petrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompletionQualityCubes.Views
{
    public partial class CreateFluidForm : Form
    {
        public CreateFluidForm()
        {
            InitializeComponent();

            FluidViscosityControl.Template = PetrelProject.WellKnownTemplates.PetrophysicalGroup.ViscosityWater; FluidViscosityControl.Text = PetrelUnitSystem.GetDisplayUnit(FluidViscosityControl.Template).DisplaySymbol;
            ViscosityUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidViscosityControl.Template).DisplaySymbol;

            FluidDensityControl.Template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            DensityUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidDensityControl.Template).DisplaySymbol;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public String FluidName { get { return FluidNameControl.Text; } }
        public double FluidViscosity 
            =>PetrelUnitSystem.GetConverterFromUI(FluidViscosityControl.Template).Convert(FluidViscosityControl.Value);


        public double FluidDensity
            => PetrelUnitSystem.GetConverterFromUI(FluidViscosityControl.Template).Convert(FluidViscosityControl.Value);


        public double FluidViscosityUI =>FluidViscosityControl.Value; 

        public double FluidDensityUI => FluidDensityControl.Value; 


    }
}
