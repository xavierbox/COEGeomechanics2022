using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject.Seismic;

namespace ManipulateCubes
{
    public partial class BoundaryConditionsControl : UserControl
    {

        #region UI 

        public BoundaryConditionsControl()
        {
            InitializeComponent();


            maxStrainTrackBar.Maximum = 150;
            maxStrainTrackBar.Minimum = -150;
            minStrainTrackBar.Maximum = 150;
            minStrainTrackBar.Minimum = -150;


            InitializeSelectors();
            ConnectEvents();

            maxStrainTextBox.Text = "0.000007";
            minStrainTextBox.Text = "0.000005";
           // maxStrainTrackBar.Value = 

            minStrainTrackBar.Tag = minStrainTextBox;
            maxStrainTrackBar.Tag = maxStrainTextBox;

            AddLogicForPoroelasticCalculations();

        }

        private void ConnectEvents()
        {
            this.newEditSelector1.SelectionChanged += new System.EventHandler(this.NewEditSelector1_SelectionChanged);
            this.saveButton.Click += new System.EventHandler(this.CreateOrEditModel);
            this.VisibleChanged += new System.EventHandler(this.BoundaryConditionsControl_VisibleChanged);

            this.flowLayoutPanel3.Resize += new System.EventHandler(this.FlowLayoutPanel_Resize);

            this.minStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minStrainTrackBar_MouseUp);
            this.maxStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minStrainTrackBar_MouseUp);

            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);

            this.maxStrainTrackBar.ValueChanged += new System.EventHandler(this.trackbarChanged);
            this.minStrainTrackBar.ValueChanged += new System.EventHandler(this.trackbarChanged);

        }
        public void UpdateControl(object sender, EventArgs e)
        {
           
        }



        private void InitializeSelectors()
        {
            var gigamodel = OceanUtilities.GetExistingOrCreateGigamodel();
            if (gigamodel == null)
            {
                return;
            }
            newEditSelector1.ModelNames = gigamodel.BoundaryConditionsNames.ToArray();

            materialSelector.Items.Clear();
            pressureSelector.Items.Clear();
            materialSelector.Items.AddRange(gigamodel.MaterialModelNames);
            pressureSelector.Items.AddRange(gigamodel.PressureModelNames);

            if ((materialSelector.SelectedIndex < 0) && (materialSelector.Items.Count > 0))
                materialSelector.SelectedIndex = 0;
            if ((pressureSelector.SelectedIndex < 0) && (pressureSelector.Items.Count > 0))
                pressureSelector.SelectedIndex = 0;
        }

        private void NewEditSelector1_SelectionChanged(object sender, EventArgs e)
        {
            ModelSelectedToUi();
        }

        private void ModelSelectedToUi()
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            string currentModelSelected = newEditSelector1.SelectedName;
            if (gigaModel.SimulationsModel.FindModel(currentModelSelected))
            {
                ModelToUi(gigaModel.BoundaryConditionsModels.GetOrCreateModel(currentModelSelected));
            }
        }

        private float DensityFromUIFactor()
        {
            return (float)(PetrelUnitSystem.ConvertFromUI(PetrelProject.WellKnownTemplates.LogTypesGroup.Density, 1.0f));
        }

        private float DensityToUIFactor()
        {
            return (float)(PetrelUnitSystem.ConvertFromUI(PetrelProject.WellKnownTemplates.LogTypesGroup.Density, 1.0f));
        }



        private void ModelToUi(BoundaryConditionsItem model)
        {
            gapDensityUpDown.Value = (Decimal)(model.GapDensity * DensityToUIFactor());
            waterDensityUpDown.Value = (Decimal)(model.SeaWaterDensity * DensityToUIFactor());
            offshoreCheck.Checked = model.Offshore ? true : false;

            minStrainTrackBar.Value = StrainToTrackBarValue(model.MinStrain);
            maxStrainTrackBar.Value = StrainToTrackBarValue(model.MaxStrain);
            strainModeGradients.Checked = model.StrainModeGradients == true ? true : false;
            minStrainAngle.Value = (Decimal)model.MinStrainAngle;

            var obj = DataManager.Resolve(model.DatumDroid);
            if (obj is StructuredSurface)
            {
                datumDropTarget.Value = (StructuredSurface)(obj);
            }
            else if (obj is RegularHeightFieldSurface)
            {
                datumDropTarget.Value = (RegularHeightFieldSurface)(obj);
            }
            else {; }
        }

        private float TrackBarValueToStrain(int trackBarValue)
        {
            //var factor = strainModeGradients.Checked ? 0.022 : 10.00;
            //return (float)(factor * trackBarValue / 100000000.00);

            return trackBarValue / 10000000.00f;
        }

        private int StrainToTrackBarValue(double strainValue)
        {
            //double factor = strainModeGradients.Checked ? 0.022f : 10.00f;
            //return (int)(maxStrainValue * 1.0e8 / factor);
            return (int)(strainValue * 10000000.00f);
        }

        private void trackbarChanged(object sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)(sender);
            if (minStrainTrackBar.Value > maxStrainTrackBar.Value)
            {
                if (bar == minStrainTrackBar)
                    maxStrainTrackBar.Value = minStrainTrackBar.Value;
                else
                    minStrainTrackBar.Value = maxStrainTrackBar.Value;
            }

            TextBox l = (TextBox)(minStrainTrackBar.Tag);
            l.Text = TrackBarValueToStrain(minStrainTrackBar.Value).ToString();
            l = (TextBox)(maxStrainTrackBar.Tag);
            l.Text = TrackBarValueToStrain(maxStrainTrackBar.Value).ToString();


            // CalculateWellStress();
        }

        private void FlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            poroelasticPanel.Width = flowLayoutPanel1.Width;
            wellPanel.Width = flowLayoutPanel1.Width;
        }

        private void BoundaryConditionsControl_VisibleChanged(object sender, EventArgs e)
        {
            InitializeSelectors();
        }

        #endregion

        #region logic 

        private void CreateOrEditModel(object sender, EventArgs e)
        {
            string selectedName = newEditSelector1.SelectedName;
            if (newEditSelector1.IsNewSelected)
            {
                string[] previousModelNames = newEditSelector1.ModelNames.ToArray();
                if (selectedName == string.Empty)
                {
                    MessageBox.Show("Please provide a valid model name. Thats why we put the text box there!", "Incomplete data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (previousModelNames.Contains(selectedName))
                {
                    MessageBox.Show("The model name is already in use. Either modify the pre-existing model or pick a new name", "Wron input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    storeModel(selectedName);
                    newEditSelector1.UpdateSelector(selectedName);
                    MessageBox.Show("Model  " + selectedName + " was Successfully Created", "Model Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }//new model

            else
            {
                storeModel(selectedName);
                newEditSelector1.UpdateSelector(selectedName);
                MessageBox.Show("Model " + selectedName + " was Edited. ", "Model Modified", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private bool storeModel(string name)
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();

            float emin = TrackBarValueToStrain(minStrainTrackBar.Value);// / strainToUi;
            float emax = TrackBarValueToStrain(maxStrainTrackBar.Value);// / strainToUi;

            Object obj = datumDropTarget.Value;
            Droid datumDroid = null;

            if (obj is RegularHeightFieldSurface)
            {
                datumDroid = ((RegularHeightFieldSurface)(datumDropTarget.Value)).Droid;
            }
            else if (obj is StructuredSurface)
            {
                datumDroid = ((StructuredSurface)(datumDropTarget.Value)).Droid;
            }
            else
            {
            }
            float angle = (float)minStrainAngle.Value;

            Decimal densityFactor = (Decimal)(DensityFromUIFactor());//from UI unit to SI 
            gigaModel.AppendOrEditBoundaryConditionsModel(name, datumDroid, offshoreCheck.Checked, (float)(densityFactor * gapDensityUpDown.Value), (float)(densityFactor * waterDensityUpDown.Value), strainModeGradients.Checked, emin, emax, angle);

            return true;
        }

        #endregion

        #region this is due to the on-the-fly poroelastic calculation 

        private void AddLogicForPoroelasticCalculations()
        {
            //basically, any change of any datum would trigger the calculation if
            //the checkbox is checked and actually we have a well (s) 
            datumDropTarget.SelectionChanged += new System.EventHandler(SomethingChanged1);
            materialSelector.SelectedIndexChanged += new System.EventHandler(SomethingChanged1);
            pressureSelector.SelectedIndexChanged += new System.EventHandler(SomethingChanged1);
            waterDensityUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            gapDensityUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            calibrationWellDropTarget.SelectionChanged += new System.EventHandler(SomethingChanged1);
            biotsNumericUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            poroelasticCalculationCheckBox.CheckedChanged += new System.EventHandler(SomethingChanged1);
            offshoreCheck.CheckedChanged += new System.EventHandler(SomethingChanged1);
            offshoreCheck.CheckedChanged += new System.EventHandler(updateImage);
            minStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SomethingChanged2);
            maxStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SomethingChanged2);


        }
        private void updateImage(object sender, EventArgs e)
        {
            pictureBox1.Image = (offshoreCheck.Checked == true ? imageList1.Images[1] : imageList1.Images[0]);
        }
        private void SomethingChanged1(object sender, EventArgs e)
        {
            CalculateWellStress();
        }

        private void SomethingChanged2(object sender, MouseEventArgs e)
        {
            CalculateWellStress();
        }

        private bool IsAnyOfTheseGuysNull(List<SeismicCube> guys)
        {
            for (int n = 0; n < guys.Count; n++) if (guys[n] == null) return true;
            return false;
        }

        private bool IsDatumAndWellsProperlySelected()
        {
            bool itIs = true;
            if (poroelasticCalculationCheckBox.Checked == false)
                return false;

            Object obj = calibrationWellDropTarget.Value;
            if (!(obj is Borehole))
                if (!(obj is BoreholeCollection))
                {
                    MessageBox.Show("Please drop a well or a folder containing wells from the input tab", "Wrong data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itIs = false;
                }

            Object Datum = datumDropTarget.Value;
            if (!(Datum is StructuredSurface))
                if (!(Datum is RegularHeightFieldSurface))
                {
                    MessageBox.Show("Please drop a datum surface", "Wrong data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itIs = false;
                }
            return itIs;
        }

        private static List<Borehole> GetAllBoreholesInsideCollectionRecursive(BoreholeCollection c)
        {
            var ws = new List<Borehole>();
            ws.AddRange(c);
            foreach (var c2 in c.BoreholeCollections)
            {
                ws.AddRange(GetAllBoreholesInsideCollectionRecursive(c2));
            }

            return ws;
        }

        private void CalculateWellStress()
        {
            if (!poroelasticCalculationCheckBox.Checked) return;

            if (materialSelector.SelectedIndex < 0) return;
            if (pressureSelector.SelectedIndex < 0) return;
            if (!IsDatumAndWellsProperlySelected()) return;

            string materialModelName = materialSelector.Items[materialSelector.SelectedIndex].ToString();
            MaterialsModelItem mats = OceanUtilities.GetExistingOrCreateGigamodel().MaterialModels.GetOrCreateModel(materialModelName);
            SeismicCube ym = mats.YoungsModulus;
            SeismicCube pr = mats.PoissonsRatio;
            SeismicCube dens = mats.Density;

            string pressureModelName = pressureSelector.Items[pressureSelector.SelectedIndex].ToString();
            PressureModelItem pressure = OceanUtilities.GetExistingOrCreateGigamodel().PressureModels.GetOrCreateModel(pressureModelName);
            SeismicCube initialPressure = pressure.InitialPressure;

            if (IsAnyOfTheseGuysNull(new List<SeismicCube>() { ym, pr, dens, initialPressure }))
                return;

            Object w = calibrationWellDropTarget.Value;
            List<Borehole> wells = new List<Borehole>();
            if (w is Borehole) wells.Add((Borehole)(w));
            if (w is BoreholeCollection)
            {
                BoreholeCollection c = (BoreholeCollection)(w);
                wells.AddRange(BoreholeTools.GetAllBoreholesInsideCollectionRecursive(c));
            }

            float emin = TrackBarValueToStrain(this.minStrainTrackBar.Value);
            float emax = TrackBarValueToStrain(this.maxStrainTrackBar.Value);

            Decimal densFactor = (Decimal)DensityFromUIFactor();//from UI to SI 
            SeismicTools.WellPoroelasticStressFromSeismic(datumDropTarget.Value, offshoreCheck.Checked, (float)(densFactor * gapDensityUpDown.Value), (float)(densFactor * this.waterDensityUpDown.Value), ym, pr, dens, initialPressure, wells, emin, emax, 0.0f);
        }



        #endregion

        private void minStrainTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            //CalculateWellStress();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.minStrainAngle.Enabled = checkBox1.Checked;
            this.angleLabel.Enabled = checkBox1.Checked;
        }
    }
}
