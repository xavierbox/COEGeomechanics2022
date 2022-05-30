using CompletionQualityCubes.Data;
using CompletionQualityCubes.Services;
using CompletionQualityCubes.Views;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Text.Json;
using System.Text.Json.Serialization;
using OceanControlsLib;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.Selection;
using Slb.Ocean.Petrel;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using System.IO;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Units;
using Slb.Ocean.Petrel.UI.Controls;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CompletionQualityCubes
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class CompletionQualitySimulation : UserControl
    {

        //known proppant and fluids are in metric units
        List<Fluid> KnownFluids = FluidFactory.KnownFluids;

        List<Proppant> KnownProppants = ProppantFactory.KnownProppants;

        List<DatumDropTarget> PropertyDrops;

        public CompletionQualitySimulation(CompletionQualityCubesProcess process)
        {
            InitializeComponent();

            SetDefaultValues();

            PropertyDrops = new List<DatumDropTarget>()
            {
            YmDrop,PrDrop,ToughnessDrop,SminDrop,LeakOffDrop,ReservoirIndexDrop
            };

            List<Type> AcceptedTypes = new List<Type>() { typeof(Property), typeof(SeismicCube) };
            PropertyDrops.ForEach(t => t.AcceptedTypes = AcceptedTypes);
            PropertyDrops.ForEach(t => t.ErrorImage = PetrelImages.Cancel);

            ImageList images2 = new ImageList();
            images2.Images.Add(PetrelImages.Property);
            images2.Images.Add(PetrelImages.Seismic3D);
            PropertyDrops.ForEach(t => t.ImageList = images2);


            PopulateFluids();

            PopulateProppants();

            StyleControl();

            ConnectEvents();


            SimulatorSelector.SelectedIndex = 0;
            tabControl1.TabPages[1].Hide();

            FluidSelector.SelectedIndex = 0;
            ProppantSelector.SelectedIndex = 0;

            this.process = process;
        }


        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private CompletionQualityCubesProcess process;

        double ConvertFromUIToMetric(UnitTextBox EntryControl)
        {
            var coverter = PetrelUnitSystem.GetConverterFromUI(EntryControl.Template);
            return coverter.Convert(EntryControl.Value);
        }

        private void SetDefaultValues()
        {
            RazorSolverSettingsButton.Tag = new RazorSolverSettingsSection();

            FluidDensityControl2.Template = PetrelProject.WellKnownTemplates.LogTypesGroup.Density;
            FluidViscosityControl2.Template = PetrelProject.WellKnownTemplates.PetrophysicalGroup.ViscosityWater;
            FluidVolumeControl2.Template = PetrelProject.WellKnownTemplates.StimulationGroup.CleanFluid;
            FluidInjectionRateControl2.Template = PetrelProject.WellKnownTemplates.StimulationGroup.CleanFluidRate;
            FLuidDensityUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidDensityControl2.Template).DisplaySymbol;
            FLuidViscosityUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidViscosityControl2.Template).DisplaySymbol;
            FluidVolumeUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidVolumeControl2.Template).DisplaySymbol;
            FluidInjectionRateUnits.Text = PetrelUnitSystem.GetDisplayUnit(FluidInjectionRateControl2.Template).DisplaySymbol;


            ProppantGrainDensity2.Template = PetrelProject.WellKnownTemplates.LogTypesGroup.Density;
            ProppantBulkDensity2.Template = PetrelProject.WellKnownTemplates.LogTypesGroup.Density;

            ProppantMass2.Template = PetrelProject.WellKnownTemplates.StimulationGroup.Proppant;

            ProppantPermeability2.Template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;// .Ge.PetrophysicalGroup.Permeability;
            ProppantPermeabilityUnits.Text = "D";// PetrelUnitSystem.GetDisplayUnit(ProppantPermeability2.Template).DisplaySymbol;
            //ProppantPermeabilityUnits.Text = PetrelUnitSystem.GetDisplayUnit(ProppantPermeability2.Template).DisplaySymbol;



            ProppantBulkDensityUnits.Text = PetrelUnitSystem.GetDisplayUnit(ProppantBulkDensity2.Template).DisplaySymbol;
            ProppantGrainDensityUnits.Text = PetrelUnitSystem.GetDisplayUnit(ProppantBulkDensity2.Template).DisplaySymbol;
            ProppantMassUnits.Text = PetrelUnitSystem.GetDisplayUnit(ProppantMass2.Template).DisplaySymbol;


            //set in razor units, the code aboce will display everythin in Petrel units automatically 
            ProppantMass2.Value = 5000.00;          //kg
            FluidInjectionRateControl2.Value = 0.1; //m3/s
            FluidVolumeControl2.Value = 150.0;      //m3
        }


        public void PopulateProppants()
        {
            ProppantSelector.Items.Clear();
            ProppantSelector.Items.AddRange(KnownProppants.ToArray());
        }

        public void PopulateFluids()
        {

            FluidSelector.Items.Clear();
            FluidSelector.Items.AddRange(KnownFluids.ToArray());//.Select(t => t.Name).ToArray()); 
        }

        public void StyleControl()
        {//
            //Width = 740;
            //Height = 940;


            ImageList images2 = new ImageList();
            images2.Images.Add(PetrelImages.Folder);
            ReservoirIndexDrop.ImageList = images2;
            ReservoirIndexDrop.ReferenceName = "Property folder";
            ReservoirIndexDrop.ErrorImage = PetrelImages.Cancel;
            ReservoirIndexDrop.PlaceHolder = "Please drop a property folder from a grid";
            ReservoirIndexDrop.StyleControl();

            //RazorSolverSettingsButton.Tag = false;

        }

        private void ConnectEvents()
        {
            CancelButton.Click += (s, e) =>
            {
                this.Parent.Dispose();
            };

            ApplyButton.Click += new System.EventHandler(ApplyButton_Click);


            ReservoirIndexDrop.ValueChanged += (sender, obj) =>
            {
                Console.WriteLine("Dropped property folder");

            };

            RazorSolverSettingsButton.Click += (s, e) =>
            {

                RazorSolverSettingsButton.Image = (bool)RazorSolverSettingsButton.Tag == true
                        ? Icons.Images["TopTriangle.png"]
                        : Icons.Images["BottomTriangle.png"];
                //RazorSolverSettingsButton.Tag = !((bool)RazorSolverSettingsButton.Tag);

            };

            this.FluidSelector.SelectedIndexChanged += (s, e) =>
            {
                var item = (Fluid)FluidSelector.SelectedItem;

                //metric units are read but these are converted to UI units automatically by the control
                FluidDensityControl2.Value = item.Density;
                FluidViscosityControl2.Value = item.Viscosity;


            };

            this.ProppantSelector.SelectedIndexChanged += (s, e) =>
            {
                var item = (Proppant)ProppantSelector.SelectedItem;

                //metric units are read but these are converted to UI units automatically by the control
                ProppantGrainDensity2.Value = item.GrainDensity;
                ProppantBulkDensity2.Value = item.BulkDensity;
                ProppantPermeability2.Value = item.Permeability;
            };


            this.SimulatorSelector.SelectedIndexChanged += (s, e) =>
            {
                bool razor = SimulatorSelector.SelectedIndex == 0;
                //FracHiteInputPanel.Visible = !razor;
                //RazorInputPanel.Visible = razor;

                if (razor)
                {
                    tabControl1.TabPages[0].Show();
                    tabControl1.TabPages[1].Hide();
                    RazorSolverSettingsButton.Show();
                }
                else//if (razor)
                {
                    tabControl1.TabPages[1].Show();
                    tabControl1.TabPages[0].Hide();
                    RazorSolverSettingsButton.Hide();
                }

            };

            this.CreateFluidButton.Click += (s, e) =>
            {

                //the form returns the properties of the fluid in metric units 
                CreateFluidForm form = new CreateFluidForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    String FluidName = form.FluidName.Trim();

                    var OldNames = KnownFluids.Select(t => t.Name);
                    if (OldNames.Contains(FluidName))
                    {
                        MessageService.ShowMessage("Fluid name must be unique", MessageType.INFO);
                    }

                    else
                    {
                        Fluid f = new Fluid(FluidName, form.FluidViscosity, form.FluidDensity);
                        KnownFluids.Add(f);
                        PopulateFluids();
                        FluidSelector.SelectedItem = f;
                    }
                }

            };

            this.CreateProppantButton.Click += (s, e) =>
            {
                //the form returns the properties of the proppant in metric units 
                CreateProppantForm form = new CreateProppantForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    String ProppantName = form.ProppantName;

                    var OldNames = KnownProppants.Select(t => t.Name);
                    if (OldNames.Contains(ProppantName))
                    {
                        MessageService.ShowMessage("Proppant name must be unique", MessageType.INFO);
                    }

                    else
                    {
                        Proppant f = new Proppant(form.ProppantName, form.Permeability, form.GrainDensity, form.BulkDensity);
                        KnownProppants.Add(f);
                        PopulateProppants();
                        ProppantSelector.SelectedItem = f;
                    }
                }

            };

            foreach (var p in PropertyDrops)
            {
                p.ValueChanged += (s, e) =>
                {
                    if (!ValidateCubes(out string error))
                    {
                        klayerPanel.Enabled = false;
                    }
                    else
                    {
                        if (!klayerPanel.Enabled)
                        {

                            var prop1 = YmDrop.Value;

                            int size = (prop1 is Property ? ((Property)(prop1)).Grid.NumCellsIJK.K :
                                       (prop1 is SeismicCube ? ((SeismicCube)prop1).NumSamplesIJK.K : 1));
                            KLayerTrackBar.Minimum = 1;
                            KLayerTrackBar.Maximum = size - 1;
                            klayerPanel.Enabled = true;
                            KLayerTrackBar.Value = (decimal)(1 + 0.5 * (size - 1 - 1));

                            //numericUpDown7.Maximum = size - 1;
                            //numericUpDown7.Minimum = 1;
                        }
                    }
                };
            }





        }

        bool ValidateCubes(out string msg)
        {
            msg = "All good";
            if (!PropertyDrops.TrueForAll(t => t.Value != null))
            {
                msg = "Some inputs are missing";
            }

            //grid
            if (PropertyDrops.TrueForAll(t => t.Value is Property))
            {
                Grid g = ((Property)PropertyDrops[0].Value).Grid;
                if (!PropertyDrops.TrueForAll(t => ((Property)t.Value).Grid == g))
                {
                    msg = "All properties must belong to the same grid";
                }
                else return true;
            }
            //seismic
            else if (PropertyDrops.TrueForAll(t => t.Value is SeismicCube))
            {

                Index3 Size = ((SeismicCube)PropertyDrops[0].Value).NumSamplesIJK;
                if (!PropertyDrops.TrueForAll(t => ((SeismicCube)t.Value).NumSamplesIJK == Size))
                {
                    msg = "All seismic cubes must have the same resolution";
                }
                else return true;
            }
            msg = "All inputs must be either grid properties or seismic cubes";
            return false;

        }

        bool ValidateName(out string msg)
        {
            msg = " ";

            if (AnalysisName.Text == "")
            {
                msg = "Please enter a name for the analysis";
                return false;
            }

            return true;
        }



        int Gap { get => 20; }

        object Stiffness { get => YmDrop.Value; }
        object PoissonR { get => PrDrop.Value; }
        object Toughness { get => ToughnessDrop.Value; }
        object MinStress { get => SminDrop.Value; }
        object Leakoff { get => LeakOffDrop.Value; }
        object Flag { get => ReservoirIndexDrop.Value; }

        object Filter { get => filterDrop.Value; }

        private void button3_Click(object sender, EventArgs e)
        {
            /*SelectionService service = PetrelSystem.SelectionService;
            IEnumerable<object> selection = service.Selection;

            IImageInfoFactory ImageFactory = CoreSystem.GetService<IImageInfoFactory>(selection.First());
            ImageInfo ImageInfo = ImageFactory.GetImageInfo( selection.First() );
            var image = ImageInfo.TypeImage;
            props[0].DisplayImage = image;
            props[0].ErrorImage = image;
        
            props[0].StyleControl();
            */
            if (ValidateCubes(out string msg))
            {
                if (PropertyDrops[0].Value is Property)
                {
                    GenerateRazorDeckFromGrid();
                }
                else// if (PropertyDrops[0].Value is SeismicCube)
                {
                    MessageService.ShowMessage("Not implemented yet", MessageType.INFO);
                }


            }

            else
            {
                MessageService.ShowMessage("Not all inputs are given", MessageType.ERROR);
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ProppantMass2.Template = PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal;//.PetrelMeasurement;// StimulationGroup.Proppant.PetrelMeasurement;
            ProppantMass2.Value = 1000000.0;




            /*
            InjectionRateInput.Template = PetrelProject.WellKnownTemplates.StimulationGroup.CleanFluidRate;



            FluidVolumeInput.Template = PetrelProject.WellKnownTemplates.StimulationGroup.CleanFluid;

            
            InjectionRateInput.TextFormat = "F1";
            FluidVolumeInput.TextFormat = "F1";


            ProppantMassInput.TextFormat = "F1";
            ProppantMassInput.Template = PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal;// StimulationGroup.Proppant;
            ProppantMassInput.UnitMeasurement = PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal.PetrelMeasurement;// StimulationGroup.Proppant.PetrelMeasurement;
            ProppantMassInput.Value = 100.0;

            var StressFromUi = PetrelUnitSystem.GetConverterFromUI(PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            var StressToUi  = PetrelUnitSystem.GetConverterToUI(PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);

            double stressPa  = StressFromUi.Convert(100.0);
            double stressBar = StressToUi.Convert( stressPa);
            

            var r = ProppantMassInput.UnitMeasurement;
            var iunit = PetrelUnitSystem.GetDisplayUnit(ProppantMassInput.Template);
            var base_unit = iunit.BaseMeasurement;

            var val = ProppantMassInput.Value;
            */

            RazorDesign design = new RazorDesign();



            design.NumberCases = 321;

            design.RazorPumpingSection = new RazorPumpingSection(
            (Proppant)ProppantSelector.SelectedItem,
            (Fluid)FluidSelector.SelectedItem,
            new Pumping()
            {
                Rate = (double)FluidInjectionRateControl2.Value,
                Volume = (double)FluidVolumeControl2.Value
            },
            (double)ProppantMass2.Value);

            var options = new JsonSerializerOptions { WriteIndented = true, };
            string json = JsonSerializer.Serialize(design, options);

            Console.WriteLine(json);
            //File.WriteAllText("Design.txt", json);

            Console.WriteLine();
            RazorInput input = new RazorInput();
            input.Design = design;
            json = JsonSerializer.Serialize(input, options);
            Console.WriteLine(json);


            //File.WriteAllText("Input.txt", json);


        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (!ValidateName(out string error1))
            {
                MessageService.ShowMessage(error1, MessageType.ERROR);
                errorProvider1.SetError(AnalysisName, error1);
                return;
            }

            if (!ValidateCubes(out string error2))
            {
                errorProvider1.SetError(PropertiesGroupControl, error2);
                MessageService.ShowMessage(error2, MessageType.ERROR);
                return;
            }

            if (PropertyDrops[0].Value is Property)
            {
                GenerateRazorDeckFromGrid();
            }

            else// if (PropertyDrops[0].Value is SeismicCube)
            {
                MessageService.ShowMessage("Not implemented yet", MessageType.INFO);
            }


        }

        private void GenerateRazorDeckFromGrid()
        {
            //Note, Razor works in metric units for everything except:
            //1. Viscosity tha needs to be in cP, which is metric x 1000.00 
            //2. Rate that needs to be ib m3/min, i.e. metric * 60.0             
                

            //get grid and grid size from the 1dt property. All are the same 
            List<Property> selected = PropertyDrops.Select(t => (Property)(t.Value)).ToList();
            Grid Grid = selected[0].Grid;
            Index3 NumCellsIJK = Grid.NumCellsIJK;
            int NumCellsK = Grid.NumCellsIJK.K;

            int kcell = (int)KLayerTrackBar.Value;
            int KMin = Math.Max(1, kcell - Gap);            //inclusive 
            int KMax = Math.Min(NumCellsK - 2, kcell + Gap);//inclusive

            //indices k of the column 
            IEnumerable<int> indices = Enumerable.Range(KMin, KMax - KMin + 1 + 1);


            Property PropertyFilter = filterDrop.Value != null ? (Property)filterDrop.Value : null;

            Func<Index3, Property, bool> p1 = (Index3 cell, Property p) => p[cell] >= 1;
            Func<Index3, Property, bool> CheckCell = PropertyFilter != null ? p1 : (Index3 cell, Property p) => true;


            int totalChecks = NumCellsIJK.I * NumCellsIJK.J,checksDone = -1;
            IProgress ProgressBar = PetrelLogger.NewProgress(0, totalChecks, ProgressType.Cancelable, System.Windows.Forms.Cursors.Cross);

            int NumFractures = 0;

            List<RazorFracture> Fractures = new List<RazorFracture>();


            for (int j = 0; j < NumCellsIJK.J; j++)
            {
                if (ProgressBar.IsCanceled) break;

                for (int i = 0; i < NumCellsIJK.I; i++)
                {
                    if (ProgressBar.IsCanceled) break;

                    checksDone += 1;
                    ProgressBar.ProgressStatus = checksDone;

                    Index3 ThisCellIndex = new Index3(i, j, kcell);

                    if (CheckCell(ThisCellIndex, PropertyFilter))
                    {
                        var tops = Grid.GetCellCorners(ThisCellIndex, CellCornerSet.Top).Select(t => t.Z);
                        var bottoms = Grid.GetCellCorners(ThisCellIndex, CellCornerSet.Base).Select(t => t.Z);
                        var estimatedHeight = Math.Abs(tops.Sum() - bottoms.Sum()) / tops.Count();
                        

                        //this one has one more 
                        IEnumerable<double> all_cell_tops = indices.Select(t => (Grid.GetCellCenter(new Index3(i, j, t - 1)).Z + Grid.GetCellCenter(new Index3(i, j, t)).Z) * 0.5);

                        RazorFracture f = new RazorFracture();
                        f.AllTops = all_cell_tops.ToArray();
                        f.PerforationLength = estimatedHeight;

                        Point3 pt = Grid.GetCellCenter(ThisCellIndex);
                        f.StimulationLocation = new CommonData.Vector3(pt.X, pt.Y, Math.Abs(pt.Z));

                        List<Index3> I3Indices = indices.Select(t => new Index3(i, j, t)).ToList();

                        List<float> Values = ProjectTools.GetPropertyValuesAtIndices((Property)Stiffness, I3Indices);
                        f.Stiffness = Values.ToArray();

                        Values = ProjectTools.GetPropertyValuesAtIndices((Property)MinStress, I3Indices);
                        f.MinStress = Values.ToArray();

                        Values = ProjectTools.GetPropertyValuesAtIndices((Property)PoissonR, I3Indices);
                        f.PoissonR = Values.ToArray();

                        Values = ProjectTools.GetPropertyValuesAtIndices((Property)Toughness, I3Indices);
                        f.Toughness = Values.ToArray();

                        Values = ProjectTools.GetPropertyValuesAtIndices((Property)Leakoff, I3Indices);
                        f.Leakoff = Values.ToArray();

                        Fractures.Add(f);
                        NumFractures += 1;
                    }
                } //i 


            }//j

            ProgressBar.Dispose();


            Console.WriteLine("Rate in UI" + (double)FluidInjectionRateControl2.Value);
            Debug.WriteLine("Rate converted From UI" + ConvertFromUIToMetric(FluidInjectionRateControl2));


            Fluid SelectedFluid = new Fluid( (Fluid)FluidSelector.SelectedItem);
            SelectedFluid.Viscosity = ConvertFromUIToMetric(FluidViscosityControl2)*1000.00;  //xx->Pa/s -> mPa/s

            Proppant SelectedProppant = (Proppant)ProppantSelector.SelectedItem;

            Pumping SelectedPumping = new Pumping()
            {
                Rate = ConvertFromUIToMetric(FluidInjectionRateControl2) * 60.0, //xx->m3/s - >m3/min 
                Volume = ConvertFromUIToMetric(FluidVolumeControl2)
            };

            RazorDesign design = new RazorDesign()
            {
                NumberCases = NumFractures,

                RazorPumpingSection = new RazorPumpingSection(SelectedProppant, SelectedFluid, SelectedPumping,
                ConvertFromUIToMetric(ProppantMass2)),
                RazorSolverSettings = (RazorSolverSettingsSection)(RazorSolverSettingsButton.Tag),
                RazorPlacementSection = new RazorPlacementSection(Fractures)
            };

            RazorDeck deck = new RazorDeck()
            {
                Name = AnalysisName.Text,
                Input = new RazorInput() { Design = design }
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(deck, options);
            Debug.WriteLine(json);

            using (FileStream createStream = File.Create(@"D:\test.json"))
            {
                JsonSerializer.Serialize(createStream, deck, options);
            }

            //string json = JsonSerializer.Serialize(design, options);
            //Console.WriteLine(json);
            //File.WriteAllText("Design.txt", json);


        }

        private void button7_Click(object sender, EventArgs e)
        {
            IUnitServiceSettings uss = CoreSystem.GetService<IUnitServiceSettings>();
            IUnitService unitService = CoreSystem.GetService<IUnitService>();

            

            IUnitCatalog OSDDCatalog = unitService.GetCatalog("OSDD");
            
            IUnitMeasurement P = OSDDCatalog.GetUnitMeasurement("Pressure");
            var pressure_units = P.Units;
            var w = P.BaseMeasurement;

           
                foreach (IUnit unit1 in pressure_units)
                {

                if (!Regex.IsMatch(unit1.DisplaySymbol, @"^\d+"))

                    foreach (IUnit unit2 in pressure_units)
                    {

                    if ((unit2 != unit1) && (!Regex.IsMatch(unit2.DisplaySymbol, @"^\d+")))
                        {
                        IUnitConverter conv = OSDDCatalog.GetConverter(unit1, unit2);

                        double v1 = conv.Convert(1);
                        double v2 = conv.Convert(10);
                        double v3 = conv.Convert(100);

                        double m = (v2 - v1) / (10 - 1);
                        double b = v1 - (v2 - v1) / (10 - 1);

                        bool error = !(Math.Abs(v3 - (m * 100 + b)) < 1.0e-5 * v3);

                        double value = m * 100 + b;

                        //Debug.WriteLine("m = " + m.ToString() + " b=" + b.ToString() + " error "+ error.ToString() );

                        //simple converter, no offset 
                        if (!error)
                        {
                            Debug.WriteLine("");
                            Debug.WriteLine(unit1.Symbol + " -> " + unit2.Symbol);
                            Debug.WriteLine("m = " + m.ToString() + " b=" + b.ToString() + " error " + error.ToString());

                        }
                        else
                        {
                                Debug.WriteLine("******");
                                Debug.WriteLine(unit1.Symbol + " -> " + unit2.Symbol);
                                Debug.WriteLine("m = " + m.ToString() + " b=" + b.ToString() + " error " + error.ToString());

                                ;
                            }

                    }

                }
            }

           
            return; 

            Dictionary<string, string> RazorUnits = new Dictionary<string, string>()
            { { "viscosity", "cP" },
              { "density", "kg/m3"}
            };

            IUnit ViscosityUI_Unit = PetrelUnitSystem.GetDisplayUnit(FluidDensityControl2.Template);//.DisplaySymbol
            string toUnitSymbol = RazorUnits["density"];

            IUnit RazorUnit = ViscosityUI_Unit.BaseMeasurement.Units.Where(t => t.DisplaySymbol == toUnitSymbol).ElementAt(0);


            
            RazorUnit = uss.CurrentCatalog.GetUnit(RazorUnits["density"]);

            IUnitConverter converter = uss.CurrentCatalog.GetConverter(ViscosityUI_Unit, RazorUnit);

            var RazoeValue = converter.Convert(1.0);
            ;
            ;


        }


    }

}//namespace 

