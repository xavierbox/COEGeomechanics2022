using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gigamodel.Data;
using Gigamodel.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel;
using Gigamodel.OceanUtils;

namespace Gigamodel
{
    public partial class BoundaryConditionsControl : UserControl
    {
        public event EventHandler<StringEventArgs> EditSelectionChangedEvent;
        public event EventHandler<EventArgs> NewClickedEvent;
        public event EventHandler<EventArgs> VisibilitychangedEvent;
        public event EventHandler<StringListEventArgs> MaterialAndPressureRequestEvent;
        public event EventHandler<StringEventArgs> DeleteModelEvent;

        public MaterialsModelItem MaterialSelected { get; set; }

        public PressureModelItem PressureSelected { get; set; }


        public BoundaryConditionsControl()
        {
            InitializeComponent();
            this.pictureBox1.Image = Properties.Resources.OnshoreDatum;// ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));

            InitializeCustomDragDrops();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            var rr = pictureBox1.Size;

            updateImage(null, EventArgs.Empty);
            masterSelector.DeleteImage = PetrelImages.Close;

            maxStrainTrackBar.Maximum = 150;
            maxStrainTrackBar.Minimum = -150;
            minStrainTrackBar.Maximum = 150;
            minStrainTrackBar.Minimum = -150;


            // InitializeSelectors();
            ConnectEvents();

            maxStrainTextBox.Text = "0.000007";
            minStrainTextBox.Text = "0.000005";


            minStrainTrackBar.Tag = minStrainTextBox;
            maxStrainTrackBar.Tag = maxStrainTextBox;

            AddLogicForPoroelasticCalculations();

        }


        void InitializeCustomDragDrops()
        {
            datumDropTarget.AcceptedTypes = new List<Type>() { typeof(RegularHeightFieldSurface), typeof(StructuredSurface) };
            ImageList images = new ImageList();
            images.Images.Add(PetrelImages.SurfaceBlue);
            images.Images.Add(PetrelImages.Surface);
            datumDropTarget.ImageList = images;
            datumDropTarget.ReferenceName = "Datum zero pressure";
            datumDropTarget.ErrorImage = PetrelImages.Cancel;
            datumDropTarget.PlaceHolder = "Please drop a structured/regular field surface. Default=0";


            calibrationWellDropTarget.AcceptedTypes = new List<Type>() { typeof(Borehole), typeof(BoreholeCollection) };
            ImageList images2 = new ImageList();
            images2.Images.Add(PetrelImages.Well);
            images2.Images.Add(PetrelImages.WellFolder);
            calibrationWellDropTarget.ImageList = images2;
            calibrationWellDropTarget.ReferenceName = "Well/Well folder";
            calibrationWellDropTarget.ErrorImage = PetrelImages.Cancel;
            calibrationWellDropTarget.PlaceHolder = "Please drop a well or a well folder";

        }


        public void UpdateSelector(string[] names)
        {
            string aux = masterSelector.SelectedName;
            masterSelector.ModelNames = names.ToList();

            if (names.Contains(aux))
                masterSelector.UpdateSelector(aux);
        }

        public void updateImage(object sender, EventArgs e)
        {
            pictureBox1.Image = offshoreCheck.Checked ?  Properties.Resources.OffshoreDatum : Properties.Resources.OnshoreDatum;// ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
   
        }

        private void AddLogicForPoroelasticCalculations()
        {
            //basically, any change of any datum would trigger the calculation if
            //the checkbox is checked and actually we have a well (s) 

            datumDropTarget.ValueChanged += new System.EventHandler(SomethingChanged1);

            materialSelector.SelectedIndexChanged += new System.EventHandler(SomethingChanged1);
            pressureSelector.SelectedIndexChanged += new System.EventHandler(SomethingChanged1);

            waterDensityUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            gapDensityUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            calibrationWellDropTarget.ValueChanged += new System.EventHandler(SomethingChanged1);
            biotsNumericUpDown.ValueChanged += new System.EventHandler(SomethingChanged1);
            poroelasticCalculationCheckBox.CheckedChanged += new System.EventHandler(SomethingChanged1);
            offshoreCheck.CheckedChanged += new System.EventHandler(SomethingChanged1);
            offshoreCheck.CheckedChanged += new System.EventHandler(updateImage);
            minStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SomethingChanged2);
            maxStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SomethingChanged2);


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
            List<Borehole> ws = new List<Borehole>();
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


            try
            {
                SeismicCube ym = (SeismicCube)((SeismicCube)(DataManager.Resolve(new Droid(MaterialSelected.YoungsModulus.DroidString))));
                SeismicCube pr = (SeismicCube)((SeismicCube)(DataManager.Resolve(new Droid(MaterialSelected.PoissonsRatio.DroidString))));
                SeismicCube dens = (SeismicCube)((SeismicCube)(DataManager.Resolve(new Droid(MaterialSelected.Density.DroidString))));
                SeismicCube initialPressure = (SeismicCube)((SeismicCube)(DataManager.Resolve(new Droid(PressureSelected.InitialPressure.DroidString))));


                if (IsAnyOfTheseGuysNull(new List<SeismicCube>() { ym, pr, dens, initialPressure }))
                {
                    MessageService.ShowError("Some cubes are null, missing or corrupt. Well stress calculation is not possible");
                    return;
                }

                List<SeismicCube> cubesToCheck = new List<SeismicCube>() { ym, pr, dens, initialPressure };
                for (int i = 1; i < cubesToCheck.Count(); i++)
                {
                    if (cubesToCheck[i].NumSamplesIJK != cubesToCheck[0].NumSamplesIJK)
                    {
                        MessageService.ShowError("Property cubes and pressure cubes must all have the same size and geometry. Well stress calculation is not possible");
                        return;
                    }
                }


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
                SeismicCubesUtilities.WellPoroelasticStressFromSeismic(datumDropTarget.Value, offshoreCheck.Checked, (float)(densFactor * gapDensityUpDown.Value), (float)(densFactor * this.waterDensityUpDown.Value), ym, pr, dens, initialPressure, wells, emin, emax, 0.0f);
            }

            catch
            {
                ;
            }

        }


        private float DensityFromUIFactor()
        {
            return (float)(PetrelUnitSystem.ConvertFromUI(PetrelProject.WellKnownTemplates.LogTypesGroup.Density, 1.0f));
        }


        private void SomethingChanged1(object sender, EventArgs e)
        {
            if (!poroelasticCalculationCheckBox.Checked) return;
            List<string> names = new List<string>() { materialSelector.Text, pressureSelector.Text };
            MaterialAndPressureRequestEvent?.Invoke(this, new StringListEventArgs(names));
            CalculateWellStress();

        }

        private void SomethingChanged2(object sender, MouseEventArgs e)
        {
            SomethingChanged1(sender, EventArgs.Empty);
        }



        private void ConnectEvents()
        {
            this.masterSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
            this.masterSelector.NewClicked += new System.EventHandler(this.NewClicked);
            this.masterSelector.DeleteClicked += (s, e) =>
            {
                if(!masterSelector.IsNewSelected)
                DeleteModelEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));

            };

            this.flowLayoutPanel3.Resize += new System.EventHandler(this.FlowLayoutPanel_Resize);

            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);

            this.maxStrainTrackBar.ValueChanged += new System.EventHandler(this.trackbarChanged);
            this.minStrainTrackBar.ValueChanged += new System.EventHandler(this.trackbarChanged);

            this.offshoreCheck.CheckedChanged += new System.EventHandler(this.updateImage);

            this.VisibleChanged += new System.EventHandler(this.BoundaryConditionsControl_VisibleChanged);


            /* this.newEditSelector1.SelectionChanged += new System.EventHandler(this.NewEditSelector1_SelectionChanged);
             this.saveButton.Click += new System.EventHandler(this.CreateOrEditModel);
             this.VisibleChanged += new System.EventHandler(this.BoundaryConditionsControl_VisibleChanged);

            
             this.minStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minStrainTrackBar_MouseUp);
             this.maxStrainTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minStrainTrackBar_MouseUp);

           
                   */

        }

        public bool IsSelectedAsNew { get { return masterSelector.IsNewSelected; } set { masterSelector.IsNewSelected = true; } }

        void ClearCubes()
        {
        }

        public string SelectedName { get { return masterSelector.SelectedName; } }



        private void EditSelectionChanged(object sender, EventArgs e)
        {
            EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
        }

        private void NewClicked(object sender, EventArgs e) //clears the cubes 
        {
            if (IsSelectedAsNew)
            {
                ClearCubes();

                NewClickedEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
            }
        }




        private void FlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            poroelasticPanel.Width = flowLayoutPanel1.Width;
            wellPanel.Width = flowLayoutPanel1.Width;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.minStrainAngle.Enabled = checkBox1.Checked;
            this.angleLabel.Enabled = checkBox1.Checked;
        }

        private float TrackBarValueToStrain(int trackBarValue)
        {
            //var factor = strainModeGradients.Checked ? 0.022 : 10.00;
            //return (float)(factor * trackBarValue / 100000000.00);

            return trackBarValue / 50000.00f;
        }

        private int StrainToTrackBarValue(double strainValue)
        {
            //double factor = strainModeGradients.Checked ? 0.022f : 10.00f;
            //return (int)(maxStrainValue * 1.0e8 / factor);
            return (int)(strainValue * 50000.00f);
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

        public void DisplayModelItem(BoundaryConditionsItem model)
        {
            if (model != null)
            {
                masterSelector.UpdateSelector(model.Name); //only changes selected index if the name is new 
                try
                {
                    string datumDroid = model.DatumDroid;

                    if ((datumDroid == string.Empty) || (datumDroid == null))
                    {
                        datumDropTarget.Value = 0;
                    }
                    else
                    {
                        var obj = DataManager.Resolve(new Droid(datumDroid));
                        if (obj is StructuredSurface)
                        {
                            datumDropTarget.Value = (StructuredSurface)(obj);
                        }
                        else if (obj is RegularHeightFieldSurface)
                        {
                            datumDropTarget.Value = (RegularHeightFieldSurface)(obj);
                        }
                        else
                        {
                            datumDropTarget.Value = null;
                        }
                    }


                    minStrainTrackBar.Value = StrainToTrackBarValue(model.MinStrain);
                    maxStrainTrackBar.Value = StrainToTrackBarValue(model.MaxStrain);
                    offshoreCheck.Checked = model.Offshore;
                    gapDensityUpDown.Value = (decimal)model.GapDensity;
                    waterDensityUpDown.Value = (decimal)model.SeaWaterDensity;
                    minStrainAngle.Value = (decimal)model.MaxStrainAngle;
                }
                catch
                {
                    MessageService.ShowError("Unable to resolve data droids. Model corrupted");
                    masterSelector.IsNewSelected = true;
                }
            }
            else
            {
                ClearCubes();
            }
        }

        public CreateEditArgs UIToModel
        {
            get
            {
                /////////////////////
                try
                {
                    ///////////////
                    BoundaryConditionsItem item = new BoundaryConditionsItem();
                    item.Name = SelectedName;
                    var obj = datumDropTarget.Value;
                    if (obj is StructuredSurface)
                    {
                        item.DatumDroid = ((StructuredSurface)(obj)).Droid.ToString();

                    }
                    else if (obj is RegularHeightFieldSurface)
                    {
                        item.DatumDroid = ((RegularHeightFieldSurface)(obj)).Droid.ToString();

                    }
                    else
                    {
                        item.DatumDroid = string.Empty;
                        item.DatumValue = 0.0f;
                    }

                    item.GapDensity = (float)gapDensityUpDown.Value;
                    item.Offshore = offshoreCheck.Checked;
                    item.SeaWaterDensity = (float)(waterDensityUpDown.Value);
                    item.MinStrain = TrackBarValueToStrain(minStrainTrackBar.Value);
                    item.MaxStrain = TrackBarValueToStrain(maxStrainTrackBar.Value);
                    item.MaxStrainAngle = (float)minStrainAngle.Value;
                    item.StrainModeGradients = false;

                    CreateEditArgs args = new CreateEditArgs();
                    args.IsNew = IsSelectedAsNew;
                    args.Object = item;
                    args.Name = SelectedName;

                    return args;

                }
                catch
                {
                    return null;
                }




            }
        }

        private void BoundaryConditionsControl_VisibleChanged(object sender, EventArgs e)
        {

            VisibilitychangedEvent?.Invoke(this, EventArgs.Empty);

        }


        public void updatePoroelasticSelectors(List<string> properties, List<string> pressures)
        {

            bool prevStateWellCalc = poroelasticCalculationCheckBox.Checked;
            poroelasticCalculationCheckBox.Checked = false;

            string text1 = materialSelector.Text;
            string text2 = pressureSelector.Text;



            List<string> props = new List<string>();
            foreach (string s in materialSelector.Items)
            {
                if (properties.Contains(s)) props.Add(s);
            }
            foreach (string s in properties)
                if (!props.Contains(s)) props.Add(s);
            materialSelector.Items.Clear();
            materialSelector.Items.AddRange(props.ToArray());



            List<string> press = new List<string>();
            foreach (string s in pressureSelector.Items)
            {
                if (pressures.Contains(s)) press.Add(s);
            }
            foreach (string s in pressures)
                if (!press.Contains(s)) press.Add(s);
            pressureSelector.Items.Clear();
            pressureSelector.Items.AddRange(press.ToArray());


            int index1 = props.IndexOf(text1);
            int index2 = press.IndexOf(text2);

            try
            {
                materialSelector.SelectedIndex = index1 >= 0 ? index1 : props.Count > 0 ? 0 : -1;
                pressureSelector.SelectedIndex = index1 >= 0 ? index1 : press.Count > 0 ? 0 : -1;
            }
            catch {
            }

        }

    }
}
