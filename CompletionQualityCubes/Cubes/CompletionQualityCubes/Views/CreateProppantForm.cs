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
    public partial class CreateProppantForm : Form
    {
        public CreateProppantForm()
        {
            InitializeComponent();
            AcceptButton = this.FormAcceptButton;
            CancelButton = this.FormCancelButton;

            BulkDensityEntry.Template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            BulkDensityUnits.Text = PetrelUnitSystem.GetDisplayUnit(BulkDensityEntry.Template).DisplaySymbol;

            GrainDensityEntry.Template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            GrainDenityUnits.Text = PetrelUnitSystem.GetDisplayUnit(GrainDensityEntry.Template).DisplaySymbol;

            PermeabilityEntry.Template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            PermeabiliyUnits.Text = "D";


        }

        public string ProppantName => ProppantNameEntry.Text.Trim();

        public double GrainDensity
        =>PetrelUnitSystem.GetConverterFromUI(GrainDensityEntry.Template).Convert(GrainDensityEntry.Value);
        
        
        public double BulkDensity
        =>PetrelUnitSystem.GetConverterFromUI(BulkDensityEntry.Template).Convert(BulkDensityEntry.Value);
  
        public double Permeability => (float)this.PermeabilityEntry.Value;



        public double GrainDensityUI => (float)this.GrainDensityEntry.Value;

        public double BulkDensityUI => (float)this.BulkDensityEntry.Value;

        public double PermeabilityUI => (float)this.PermeabilityEntry.Value;


    }
}
