using System;
using System.Drawing;
using System.Windows.Forms;


//using Restoration.OceanUtils;
using System.Collections.Generic;

using System.Linq;
 
 
using System.Timers;
 
using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.UI;
using Restoration.Model;
using Slb.Ocean.Data.Hosting;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;
using Restoration.Services;

namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the _process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class DRestorationProcessUI : UserControl
    {

        public event EventHandler<FracturePredictionFromStressEvent> FracturePredictionFromStressClicked;
        public event EventHandler<ModelToWellsComparisonEvent> CompareModelAndWellDataClicked;
        Dictionary<string, float[]> _dynelFile;
        private DRestorationProcess _process;

        Font greyFont;//= new gridPresentationBox.Font;
        Font blackFont;// = new gridPresentationBox.Font;

        List<Borehole> connectedWells;
        List<Borehole> changedWells;
        System.Timers.Timer eventTimer;

        #region initialization

        public DRestorationProcessUI(DRestorationProcess process)
        {
            _dynelFile = new Dictionary<string, float[]>();
            _process = process;

            var rr = this.Parent;


            InitializeComponent();
            dfnSetsView.BackgroundColor = Color.White;


            

            //this.BackColor = Color.OldLace;
            this.ResumeLayout();
            var ss = this.Size;
            this.SuspendLayout();
            this.ResumeLayout();

            this.Dock = DockStyle.Fill;
            // this.MaximumSize = new Size(750, 650);
            //  this.MinimumSize = new Size(750, 650);
            //this.Size = new Size(700, 450);
            var w = flowLayoutPanel6.Size;
            //this.ParentForm.Size = new Size(755, 700);

            optionsControl_Click(null, EventArgs.Empty);

            fracturePredictionButton.Image = PetrelImages.Apply;
            cancelButton.Image = PetrelImages.Close;
            fracturePredictionButton.ImageAlign = ContentAlignment.MiddleRight;
            cancelButton.ImageAlign = ContentAlignment.MiddleRight;

            executeWellCalculationsButton.Image = PetrelImages.Apply;
            executeWellCalculationsButton.ImageAlign = ContentAlignment.MiddleRight;


            //  greyFont = new Font(dynelResultsFileTextBoxPpresentationBox.Font.FontFamily, dynelResultsFileTextBoxPpresentationBox.Font.Size, FontStyle.Italic);
            //  blackFont = new Font(dynelResultsFileTextBoxPpresentationBox.Font.FontFamily, dynelResultsFileTextBoxPpresentationBox.Font.Size, FontStyle.Regular);


            //log-dfn analysis 
            Patches = new List<FracturePatch>();
            eventTimer = new System.Timers.Timer(3000);
            eventTimer.Elapsed += OnTimedEvent;
            eventTimer.AutoReset = false;
            eventTimer.Enabled = true;
            connectedWells = new List<Borehole>();
            changedWells = new List<Borehole>();


            InitializeStressProcessingControls();
            InitializeWellComparisonPanel();
            InitializeLogDFNPanel();


            DateTime today = DateTime.Today;
            DateTime expiration = new DateTime(2022, 6, 15);
            int result = DateTime.Compare(today, expiration);
            if (result>0)
            {
                ExpiredLabel.Visible = true;
            }
            else {
                ExpiredLabel.Visible = false;
                ConnectCalculationEvents();
            }
            cancelButton.Click += (sender, evt) => { this.ParentForm.Close(); };

        }

        private void InitializeWellComparisonPanel()
        {
            wellLogVersionPresentationBox.Font = greyFont;
            wellLogVersionPresentationBox.ForeColor = Color.Gray;
            wellLogVersionPresentationBox.Text = "Drop a point-data with the observed fractures in wells ";

            dipControl.Tag = null;
            wellLogVersionPresentationBox.Tag = null;
        }

        private void InitializeStressProcessingControls()
        {
            StressPresentationBox.ForeColor = Color.Gray;
            StressPresentationBox.Font = greyFont;
            StressPresentationBox.Text = "Drop Grid Property Collection (TOTSTRXX,TOTSTRYY,...,TOTSTRYZ)";
            StressPresentationBox.Image = null;
            StressPresentationBox.Tag = null;
        }

        private void InitializeComparisonControls()
        {
            modelPredictedOrientationPresentationBox.ForeColor = Color.Gray;
            modelPredictedOrientationPresentationBox.Font = greyFont;
            modelPredictedOrientationPresentationBox.Text = "Drop Grid Property Collection (Dip_Type, Dip Azimuth_Type, etc...)";
            modelPredictedOrientationPresentationBox.Image = null;
            modelPredictedOrientationPresentationBox.Tag = null;

            modelPredictedOrientationsGrid.Visible = false;
            modelPredictedOrientationsGrid.Rows.Clear();
            //modelPredictedOrientationsGrid.BorderStyle = BorderStyle.FixedSingle;
            comboBox1.Items.Clear();

        }

        private void InitializeLogDFNPanel(bool topPart = true, bool bottomPart = true)
        {
            if (topPart)
            {
                dfnPresentationBox.Font = greyFont;
                dfnPresentationBox.ForeColor = Color.Gray;
                dfnPresentationBox.Text = "Drop a Discrete Fracture Network Model (DFN)";
                dfnPresentationBox.Tag = null;
            }
            if (bottomPart)
            {
                wellsPresentationBox.Font = greyFont;
                wellsPresentationBox.ForeColor = Color.Gray;
                wellsPresentationBox.Text = "Drop a well or a wells folder to intersect the DFN ";
                wellsPresentationBox.Tag = null;


                horizonPresentationbox.Font = greyFont;
                horizonPresentationbox.ForeColor = Color.Gray;
                horizonPresentationbox.Text = "Drop a regular height field surface ";
                horizonPresentationbox.Tag = null;


            }
        }


        #endregion

        #region access


        //Grid Grid
        //{
        //    get
        //    {
        //        return null;
        //       // Grid g = (Grid)(gridPresentationBox.Tag);
        //       // return g;
        //    }
        //    set
        //    {
        //        Grid g = value as Grid;
        //       // gridPresentationBox.Tag = g;
        //        if ((g == null) || (g == Grid.NullObject))
        //        {
        //            ;// Initialize3DGridPanel();
        //        }
        //        else
        //        {
        //         //   gridPresentationBox.ForeColor = Color.Black;
        //         //   gridPresentationBox.Font = blackFont;
        //         //   gridPresentationBox.Tag = g;
        //        //    gridPresentationBox.Text = g.Layer;
        //        //    gridPresentationBox.Image = PetrelImages.Model;
        //        }
        //    }
        //}




        #endregion


        #region KeyEvents Triggering Calculations 
        float[] GetEquivalentPlasticStrainScalarFromTensor(Dictionary<string, float[]> tensor)
        {
            int count = tensor[tensor.Keys.ElementAt(0)].Length;
            float[] ret = Enumerable.Repeat(float.NaN, count).ToArray();

            //List<string> keys = tensor.Keys;

            float[] xxVals = tensor[tensor.Keys.ElementAt(0)];
            float[] yyVals = tensor[tensor.Keys.ElementAt(1)];
            float[] zzVals = tensor[tensor.Keys.ElementAt(2)];
            float[] xyVals = tensor[tensor.Keys.ElementAt(3)];
            float[] yzVals = tensor[tensor.Keys.ElementAt(4)];
            float[] zxVals = tensor[tensor.Keys.ElementAt(5)];

            //var exx = xxVals.Select( (t,i) => ) blah..bl;ah..
            for (int n = 0; n < count; n++)
            {
                float xx = xxVals[n];
                float yy = yyVals[n];
                float zz = zzVals[n];
                float xy = xyVals[n];
                float yz = yzVals[n];
                float zx = zxVals[n];

                bool areNotOk = ((float.IsNaN(xx)) || (float.IsNaN(yy)) || (float.IsNaN(zz)));

                if (areNotOk)
                {
                    ret[n] = float.NaN;
                }
                else
                {
                    double exx = (2.0 * xx - yy - zz) / 3.0;
                    double eyy = (-xx + 2.0 * yy - zz) / 3.0;
                    double ezz = (-xx - yy + 2.0 * zz) / 3.0;

                    double aux = Math.Sqrt(1.5 * (exx * exx + eyy * eyy + ezz * ezz) + 0.75 * (xy * xy + yz * yz + zx * zx));
                    ret[n] = (float)(0.6666666666 * aux);
                }

            }


            return ret;

        }

        private void ConnectCalculationEvents()
        {
            #region predict fractures from stress in grid or points 
            this.fracturePredictionButton.Click += (sender, evt) =>
            {
                if (StressPresentationBox.Tag is PropertyCollection)
                {

                    PropertyCollection col = StressPresentationBox.Tag as PropertyCollection;
                    Dictionary<string, float[]> stressNames = GetStressFromGridPropertyFolder(col);
                    if ((stressNames == null) || (stressNames.Keys.Count() != 6))
                    {
                        MessageBox.Show("Was not possible to read stress data.\nPlease be sure to have properties with the Total Stress Template and the following names:\nTOTSTRXX\nTOTSTRYY\nTOTSTRZZ\nTOTSTRXY\nTOTSTRYZ\nTOTSTRZX", "Data missing", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        MessageBox.Show("Process interrupted");

                        return;
                    }

                    //do we have a friction angle?  
                    bool copyFrictionwhenRotating = false;
                    float[] friction = ProjectTools.GetGridPropertyValues(col, "FANG", PetrelProject.WellKnownTemplates.GeomechanicGroup.FrictionAngle);
                    if (friction != null)
                    {
                        stressNames["FANG"] = friction;
                        copyFrictionwhenRotating = true;
                    }

                    ////Assef addition in last minute. According to him, it shouldnt be affected by rotations 
                    bool copystrainsWhenRotating = false;
                    Dictionary<string, float[]> strainNames = GetTensorFromPropertyFolder(col, "PLSTRN", PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);
                    if (strainNames != null)
                    {
                        float[] eqPlasticStrain = GetEquivalentPlasticStrainScalarFromTensor(strainNames);
                        Property p2 = ProjectTools.GetOrCreateProperty(col, "Equivalent Plastic Strain", PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);
                        ProjectTools.SetValues(p2, eqPlasticStrain.ToList());
                        stressNames["Equivalent Plastic Strain"] = eqPlasticStrain;
                        copystrainsWhenRotating = true;
                    }



                    //do we need to rotate ? 
                    if (rotateStressCheckBox.Checked)
                    {
                        float angle = (float)rotationAngleControl.Value;
                        PropertyCollection rotatedCollection = RotateStress(col, angle);

                        StressPresentationBox.Tag = rotatedCollection;
                        StressPresentationBox.Text = rotatedCollection.Name;
                        stressNames = GetStressFromGridPropertyFolder(rotatedCollection);

                        if (copyFrictionwhenRotating)
                        {
                            stressNames["FANG"] = ProjectTools.GetGridPropertyValues(col, "FANG", PetrelProject.WellKnownTemplates.GeomechanicGroup.FrictionAngle);
                            Property p1 = ProjectTools.GetOrCreateProperty(rotatedCollection, "FANG", PetrelProject.WellKnownTemplates.GeomechanicGroup.FrictionAngle);
                            ProjectTools.SetValues(p1, stressNames["FANG"].ToList());
                        }


                        if (copystrainsWhenRotating)
                        {
                            stressNames["Equivalent Plastic Strain"] = ProjectTools.GetGridPropertyValues(col, "Equivalent Plastic Strain", PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);
                            Property p1 = ProjectTools.GetOrCreateProperty(rotatedCollection, "Equivalent Plastic Strain", PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);
                            ProjectTools.SetValues(p1, stressNames["Equivalent Plastic Strain"].ToList());
                        }


                        col = rotatedCollection;

                    }





                    //emit an event that the controller will process. If successfully, here will be received an instruction to display the results. 
                    CommonData.Vector3[] locations = ProjectTools.GetCellCenters(col.Grid);
                    FracturePredictionFromStressEvent args = new FracturePredictionFromStressEvent(stressNames, (float)(frictionAngleControl.Value), locations, applyFilteringOutlayersCheckbox.Checked);
                    FracturePredictionFromStressClicked?.Invoke(this, args);
                }


                else if (StressPresentationBox.Tag is PointSet)
                {
                    PointSet pts = StressPresentationBox.Tag as PointSet;
                    MessageService.ShowMessage("Not implemented. Predict fractures from points with attributes. [1]");
                }

                else
                {
                    MessageService.ShowError("Please select the Stress Source. Either a property collection in a pilar grid or a point set ");
                    return;
                }



            };



            #endregion


            #region well-data-comparison

            this.executeWellCalculationsButton.Click += (sender, evt) =>
         {
             if (CompareModelAndWellDataClicked != null)
             {
                 //stored in the UI 
                 List<KeyValuePair<int, Tuple<Property, Property>>> x = modelPredictedOrientationsGrid.Tag as List<KeyValuePair<int, Tuple<Property, Property>>>;
                 if (x == null)
                     return;


                 //lets create a fracture model with the predicted data. 
                 FractureModel model = new FractureModel();
                 List<Index3> indices = new List<Index3>();
                 CommonData.Vector3[] cellCenters = null;

                 PropertyCollection propCollection = null;

                 int rowCounter = 0;
                 foreach (var item in x)
                 {
                     if ((bool)(modelPredictedOrientationsGrid.Rows[rowCounter++].Cells[0].Value) == true)
                     {
                         var code = item.Key;
                         var fracCodeForDebug = (FractureType)(code);

                         var orientations = item.Value;

                         Property dip = orientations.Item1;
                         Property dipAzimuth = orientations.Item2;

                         if (propCollection == null) propCollection = dip.PropertyCollection;


                         List<float> dips = new List<float>();
                         List<float> dipAzimuths = new List<float>();


                         //Monzurul's patch 
                         //lets only do the work in a range of indices with a selected range of K values in [K1, K2] 
                         //the rest are flagged as invalid.  
                         int kmin = (int)kminControl.Value - 1;
                         int kmax = (int)kmaxControl.Value - 1;
                         ProjectTools.GetValidPropertyValues(dip, ref dips, ref indices, kmin, kmax);




                         //if (indices.Count() < 1)  //first get only the valid indices and do this once. 
                         //{
                         //ProjectTools.GetValidPropertyValues(dip, ref dips, ref indices);
                         cellCenters = ProjectTools.GetCellCentersAtIndices(dip.Grid, indices);
                         //}
                         //else
                         //{
                         //    dips = ProjectTools.GetPropertyValuesAtIndices(dip, indices);
                         //}
                         dipAzimuths = ProjectTools.GetPropertyValuesAtIndices(dipAzimuth, indices);

                         List<Fracture> fracs = new List<Fracture>();
                         for (int n = 0; n < cellCenters.Length; n++)
                         {
                             Fracture f = new Fracture();
                             f.Type = (FractureType)code;
                             f.Location = cellCenters[n];
                             f.Orientation = new Model.Orientation(dips[n], dipAzimuths[n]);
                             fracs.Add(f);
                             //we dont need the intensity or anything else....so we dont bother. 

                             //Type = f.Type;
                             //Id = f.Id;
                             //Set = f.Set;
                             //Group = f.Group;
                             //Observed = f.Observed;
                             //Location = f.Location;
                             //Orientation = f.Orientation;
                             //Intensity = f.Intensity;
                         }
                         model.Fractures.AddRange(fracs);

                     }//item was selected in the grid 
                 }

                 //done, we have a model and this is a data object that the controller understands. IT should be able to compare it with another model. 


                 //pass the data to the controller as a fracture model (Ocean free) 
                 ModelToWellsComparisonEvent args = new ModelToWellsComparisonEvent();
                 args.ObservedData = GetWellObservedData();
                 args.ObservedData.Name = propCollection.Name;

                 //   args.WellSearchRadius = (double)(searchRadius.Value);
                 //  args.UseCustomSearchRadius = !(automaticSearchRadius.Checked);
                 args.PredictedModel = model;












                 CompareModelAndWellDataClicked(this, args);



             }
         };

            #endregion


            #region logdfn


            #endregion

            //RotateStress(PropertyCollection col)
            //#region file selected
            //this.selectDynelFilebutton.Click += (sender, evt) =>
            //{
            //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //    openFileDialog1.Filter = "Dynel|*.xyz";
            //    openFileDialog1.Title = "Select a Dynel Results File";
            //    openFileDialog1.Multiselect = false;

            //    // Show the Dialog.          
            //    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        DynelResultsFileSelected?.Invoke(this, new StringEventArgs(openFileDialog1.FileName));

            //};
            //#endregion

            //#region map stress to grid and points 
            //this.ImportStressFromDynelButton.Click += (sender, evt) =>
            //{
            //    DynelDatatoProcessEvent dataEvent = new DynelDatatoProcessEvent();
            //    dataEvent.DataFileName = dynelResultsFileTextBoxPpresentationBox.Text;
            //    dataEvent.Traslation = Traslation;
            //    ImportStressesFromDynelToGridClicked(this, dataEvent);
            //};
            //#endregion
            //Dictionary<string, float[]> names = new Dictionary<string, float[]>()
            //    {
            //    { "TOTSTRXX",null},
            //    { "TOTSTRYY",null},
            //    { "TOTSTRZZ",null },
            //    { "TOTSTRXY",null },
            //    { "TOTSTRYZ",null },
            //    { "TOTSTRZX",null}
            //    };

            //CommonData.Vector3[] locations = null;
            //if (StressPresentationBox.Tag is PropertyCollection)
            //{
            //    //it is a grid. Lets pass to the controller (calculator) all the center points and the stress tensor.
            //    PropertyCollection col = (PropertyCollection)(StressPresentationBox.Tag);
            //    Index3 numCellsIJK = col.Grid.NumCellsIJK;
            //    int nCells = numCellsIJK.I * numCellsIJK.J * numCellsIJK.K;
            //    locations = ProjectTools.GetCellCenters(col.Grid);

            //    //get cell centers. NOT USED when the results are mapped into a grid 
            //    //get the properties if we have them in the collection. Otherwise, something is wrong. 
            //    for (int i = 0; i < names.Keys.Count(); i++)
            //    {
            //        string name = names.Keys.ElementAt(i);
            //        names[name] = ProjectTools.GetGridPropertyValues(col, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);

            //        //where I have stress, there must be a cell center. Lets check that var notNullCellCenters = names[names.Keys.ElementAt(0)].Where((t, ii) => !double.IsNaN(locations[ii].X))  ;
            //        if (names[name] == null)
            //        {
            //            MessageService.ShowError("Property " + name + " is missing or it doesnt have the expected template");
            //            break;
            //        }
            //    }

            //    //do we have a friction angle?  
            //    float[] friction = ProjectTools.GetGridPropertyValues(col, "FANG", PetrelProject.WellKnownTemplates.GeomechanicGroup.FrictionAngle);
            //    if (friction != null)
            //    {
            //        names["FANG"] = friction;
            //    }




            //}







        }

        #endregion


        #region Drad-Drops 

        //private void propertyCollectionDrop_DragDrop(object sender, DragEventArgs e)
        //{
        //    Object obj = e.Data.GetData(typeof(Object)) as Object; ;
        //    StressDataDropped(obj);
        //}

        //private void dropGrid_DragDrop(object sender, DragEventArgs e)
        //{
        //    Grid = e.Data.GetData(typeof(Grid)) as Grid; ;
        //}

        private void modelPredictedOrientationsDropControl_DragDrop(object sender, DragEventArgs e)
        {
            Object obj = e.Data.GetData(typeof(Object)) as Object; ;
            ModelOrientationsDropped(obj);
        }


        private void StressDataDropped(object obj)
        {
            if (obj is PropertyCollection)    //grid properties 
            {
                StressPresentationBox.ForeColor = Color.Black;
                StressPresentationBox.Font = blackFont;
                PropertyCollection c = obj as PropertyCollection;
                StressPresentationBox.Text = c.Name;
                StressPresentationBox.Image = PetrelImages.Property;
                StressPresentationBox.Tag = c;
                PropertyCollection col = (PropertyCollection)(StressPresentationBox.Tag);



                float[] friction = ProjectTools.GetGridPropertyValues(col, "FANG", PetrelProject.WellKnownTemplates.GeomechanicGroup.FrictionAngle);
                float[] pressures = ProjectTools.GetGridPropertyValues(col, "PRESSURE", PetrelProject.WellKnownTemplates.PetrophysicalGroup.Pressure);

                //do we have a friction angle?  
                fangProperty.Visible = friction != null ? true : false;
                frictionAngleControl.Visible = friction != null ? false : true;


                if (friction != null)
                {
                    fangProperty.Image = PetrelImages.Property;
                    fangProperty.Text = "FANG";
                }





            }
            else if (obj is PointSet)  //points with stress attribute 
            {
                StressPresentationBox.ForeColor = Color.Black;
                StressPresentationBox.Font = blackFont;
                PointSet c = obj as PointSet;
                StressPresentationBox.Text = c.Name;
                StressPresentationBox.Image = PetrelImages.PointSet;
                StressPresentationBox.Tag = c;
            }
            else
            {
                InitializeStressProcessingControls();
            }
        }

        private void ModelOrientationsDroppedOLD(object obj)
        {
            if (obj is PropertyCollection)    //grid properties 
            {
                modelPredictedOrientationPresentationBox.ForeColor = Color.Black;
                modelPredictedOrientationPresentationBox.Font = blackFont;
                PropertyCollection c = obj as PropertyCollection;
                modelPredictedOrientationPresentationBox.Text = c.Name;
                modelPredictedOrientationPresentationBox.Image = PetrelImages.Property;
                modelPredictedOrientationPresentationBox.Tag = c;

                //in this version, the name of the properties is fixed. The user must guarantee that there are properties with the required names. 
                //here we just check that at least one for comparison is available 

                modelPredictedOrientationsGrid.Rows.Clear();
                modelPredictedOrientationsGrid.AllowUserToAddRows = false;

                List<KeyValuePair<int, Tuple<Property, Property>>> dataToStore = new List<KeyValuePair<int, Tuple<Property, Property>>>();



                string[] namesFractureTypesKnown = Enum.GetNames(typeof(FractureType));
                foreach (string name in namesFractureTypesKnown)
                {
                    var code = (int)(Enum.Parse(typeof(FractureType), name));

                    if (name.ToLower().Contains("unknown"))
                    {
                        ;
                    }

                    else if (name.ToLower() != "shear")
                    {
                        var dips = c.Properties.Where(t => t.Name.ToLower() == ("dip" + name).ToLower());
                        var dipAzimuths = c.Properties.Where(t => t.Name.ToLower() == ("dipazimuth" + name).ToLower());

                        var tr = dips.Count();

                        if ((dips.Count() > 0) && (dipAzimuths.Count() >= 0))
                        {
                            int i = modelPredictedOrientationsGrid.Rows.Add("", name, code.ToString());
                            modelPredictedOrientationsGrid.Rows[i].Cells[0].Value = true;

                            KeyValuePair<int, Tuple<Property, Property>> props = new KeyValuePair<int, Tuple<Property, Property>>
                            ((int)(code), new Tuple<Property, Property>(dips.ElementAt(0), dipAzimuths.ElementAt(0)));
                            modelPredictedOrientationsGrid.Rows[i].Tag = props;

                            dataToStore.Add(props);
                        }
                    }

                    else
                    {
                        var dips = c.Properties.Where(t => t.Name.ToLower() == ("dip" + name + "1").ToLower());
                        var dipAzimuths = c.Properties.Where(t => t.Name.ToLower() == ("dipazimuth" + name + "1").ToLower());
                        if ((dips.Count() > 0) && (dipAzimuths.Count() >= 0))
                        {
                            int i = modelPredictedOrientationsGrid.Rows.Add("", name + "1", code.ToString());
                            modelPredictedOrientationsGrid.Rows[i].Cells[0].Value = true;

                            KeyValuePair<int, Tuple<Property, Property>> props = new KeyValuePair<int, Tuple<Property, Property>>
                            ((int)(code), new Tuple<Property, Property>(dips.ElementAt(0), dipAzimuths.ElementAt(0)));
                            modelPredictedOrientationsGrid.Rows[i].Tag = props;

                            dataToStore.Add(props);
                        }

                        var dips2 = c.Properties.Where(t => t.Name.ToLower() == ("dip" + name + "2").ToLower());
                        var dipAzimuths2 = c.Properties.Where(t => t.Name.ToLower() == ("dipazimuth" + name + "2").ToLower());
                        if ((dips2.Count() > 0) && (dipAzimuths2.Count() >= 0))
                        {
                            int i = modelPredictedOrientationsGrid.Rows.Add("", name + "2", code.ToString());
                            modelPredictedOrientationsGrid.Rows[i].Cells[0].Value = true;

                            KeyValuePair<int, Tuple<Property, Property>> props = new KeyValuePair<int, Tuple<Property, Property>>
                            ((int)(code), new Tuple<Property, Property>(dips2.ElementAt(0), dipAzimuths2.ElementAt(0)));
                            modelPredictedOrientationsGrid.Rows[i].Tag = props;
                            dataToStore.Add(props);
                        }

                    }


                }


                modelPredictedOrientationsGrid.Tag = dataToStore;

            }


            else if (obj is PointSet)  //points with stress attribute 
            {

                MessageService.ShowMessage("Not Implemented yet. Comparison with PointSets from PointLogData. [2]");
                InitializeComparisonControls();
                //StressPresentationBox.ForeColor = Color.Black;
                //StressPresentationBox.Font = blackFont;
                //PointSet c = obj as PointSet;
                //StressPresentationBox.Text = c.Layer;
                //StressPresentationBox.Image = PetrelImages.PointSet;
                //StressPresentationBox.Tag = c;
            }
            else
            {
                InitializeComparisonControls();
            }
        }

        private void ModelOrientationsDropped(object obj)
        {
            if (obj is PropertyCollection)    //grid properties 
            {
                modelPredictedOrientationPresentationBox.ForeColor = Color.Black;
                modelPredictedOrientationPresentationBox.Font = blackFont;
                PropertyCollection c = obj as PropertyCollection;
                modelPredictedOrientationPresentationBox.Text = c.Name;
                modelPredictedOrientationPresentationBox.Image = PetrelImages.Property;
                modelPredictedOrientationPresentationBox.Tag = c;

                //in this version, the name of the properties is fixed. The user must guarantee that there are properties with the required names. 
                //here we just check that at least one for comparison is available 
                modelPredictedOrientationsGrid.Rows.Clear();
                modelPredictedOrientationsGrid.AllowUserToAddRows = false;

                List<KeyValuePair<int, Tuple<Property, Property>>> dataToStore = new List<KeyValuePair<int, Tuple<Property, Property>>>();



                string[] namesFractureTypesKnown = Enum.GetNames(typeof(FractureType));
                foreach (string name in namesFractureTypesKnown)
                {
                    var code = (int)(Enum.Parse(typeof(FractureType), name));

                    if ((!name.ToLower().Contains("unknown")) && (code < (int)FractureType.Shear3))
                    {
                        var dips = c.Properties.Where(t => t.Name.ToLower() == ("dip" + name).ToLower());
                        var dipAzimuths = c.Properties.Where(t => t.Name.ToLower() == ("dipazimuth" + name).ToLower());

                        var tr = dips.Count();

                        if ((dips.Count() > 0) && (dipAzimuths.Count() >= 0))
                        {
                            int i = modelPredictedOrientationsGrid.Rows.Add("", name, code.ToString());
                            modelPredictedOrientationsGrid.Rows[i].Cells[0].Value = true;

                            KeyValuePair<int, Tuple<Property, Property>> props = new KeyValuePair<int, Tuple<Property, Property>>
                            ((int)(code), new Tuple<Property, Property>(dips.ElementAt(0), dipAzimuths.ElementAt(0)));
                            modelPredictedOrientationsGrid.Rows[i].Tag = props;

                            dataToStore.Add(props);
                        }
                    }

                }//known names 
                modelPredictedOrientationsGrid.Tag = dataToStore;

                kminControl.Minimum = 1;
                kminControl.Maximum = c.Grid.NumCellsIJK.K + 1;
                kminControl.Value = 1;

                kmaxControl.Minimum = 2;
                kmaxControl.Maximum = c.Grid.NumCellsIJK.K + 1;
                kmaxControl.Value = c.Grid.NumCellsIJK.K + 1;


                //////////////////////////
                //now we will also allow the user to do a comparison per zone
                comboBox1.Items.Clear();
                var zones = c.Grid.Zones;
                foreach (var zone in c.Grid.Zones)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Text = zone.Name;
                    item.Image = PetrelImages.Zone;
                    item.Value = zone;
                    comboBox1.Items.Add(item);
                }
                ComboBoxItem lastItem = new ComboBoxItem();
                lastItem.Text = "All Layers";
                lastItem.Image = PetrelImages.Zone;
                lastItem.Value = "All";
                comboBox1.Items.Add(lastItem);
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                comboBox1.DropDownStyle = Slb.Ocean.Petrel.UI.Controls.ComboBoxStyle.DropDownList;
                ///////////////////////

                modelPredictedOrientationsGrid.Visible = true;

                modelPredictedOrientationsGrid.BorderStyle = BorderStyle.None;
            }//property collection



            else if (obj is PointSet)  //points with stress attribute 
            {

                MessageService.ShowMessage("Not Implemented yet. Comparison with PointSets from PointLogData. [2]");
                InitializeComparisonControls();
                //StressPresentationBox.ForeColor = Color.Black;
                //StressPresentationBox.Font = blackFont;
                //PointSet c = obj as PointSet;
                //StressPresentationBox.Text = c.Layer;
                //StressPresentationBox.Image = PetrelImages.PointSet;
                //StressPresentationBox.Tag = c;
            }
            else
            {
                InitializeComparisonControls();
            }
        }

        private void dropWellPointDataForComparison_DragDrop(object sender, DragEventArgs e)
        {
            PointWellLogVersion pointWellLogVersion = e.Data.GetData(typeof(PointWellLogVersion)) as PointWellLogVersion;
            dipControl.Items.Clear();
            fractureTypeControl.Items.Clear();
            azimuthControl.Items.Clear();

            wellLogVersionPresentationBox.Text = "";

            dipControl.Tag = null;

            if (pointWellLogVersion == null)
            {
                InitializeWellComparisonPanel();
                return;
            }

            wellLogVersionPresentationBox.ForeColor = Color.Black;
            wellLogVersionPresentationBox.Font = blackFont;
            wellLogVersionPresentationBox.Text = pointWellLogVersion.Name;
            wellLogVersionPresentationBox.Image = PetrelImages.PointSet;


            foreach (WellPointProperty p in pointWellLogVersion.PropertyCollection.Properties)
            {
                ComboBoxItem item1 = new ComboBoxItem(p);
                item1.Text = p.Name;
                dipControl.Items.Add(item1);

                ComboBoxItem item2 = new ComboBoxItem(p);
                item2.Text = p.Name;

                azimuthControl.Items.Add(item2);

                ComboBoxItem item3 = new ComboBoxItem(p);
                item3.Text = p.Name;
                fractureTypeControl.Items.Add(item3);

                ComboBoxItem item4 = new ComboBoxItem(p);
                item4.Text = p.Name;
                mdControl.Items.Add(item4);
            }

            var names = pointWellLogVersion.PropertyCollection.Properties.Select(t => t.Name.ToLower()).ToList();
            if (names.IndexOf("dip angle") >= 0) dipControl.SelectedIndex = names.IndexOf("dip angle");
            if (names.IndexOf("dip azimuth") >= 0) azimuthControl.SelectedIndex = names.IndexOf("dip azimuth");
            if (names.IndexOf("fracturetype") >= 0) fractureTypeControl.SelectedIndex = names.IndexOf("fracturetype");
            if (names.IndexOf("fracture type") >= 0) fractureTypeControl.SelectedIndex = names.IndexOf("fracture type");


            if (names.IndexOf("md") >= 0) mdControl.SelectedIndex = names.IndexOf("md");
            else if (names.IndexOf("depth") >= 0) mdControl.SelectedIndex = names.IndexOf("depth");
            else
            {; }

            if ((dipControl.SelectedIndex < 0) && (dipControl.Items.Count > 0)) dipControl.SelectedIndex = 0;
            if ((azimuthControl.SelectedIndex < 0) && (azimuthControl.Items.Count > 0)) azimuthControl.SelectedIndex = 0;
            if ((fractureTypeControl.SelectedIndex < 0) && (fractureTypeControl.Items.Count > 0)) fractureTypeControl.SelectedIndex = 0;
            if ((mdControl.SelectedIndex < 0) && (mdControl.Items.Count > 0)) mdControl.SelectedIndex = 0;

            dipControl.Tag = pointWellLogVersion.PropertyCollection.Properties.ToArray();
            wellLogVersionPresentationBox.Tag = pointWellLogVersion;
        }

        WellFractureDataModel GetWellObservedData()
        {
            WellFractureDataModel toReturn = null;

            //this is the data stored by the user 
            PointWellLogVersion pointWellLogVersion = wellLogVersionPresentationBox.Tag as PointWellLogVersion;
            WellPointProperty[] properties = (dipControl.Tag as IEnumerable<WellPointProperty>).ToArray();

            WellPointProperty mds = properties[mdControl.SelectedIndex];
            WellPointProperty dip = properties[dipControl.SelectedIndex];
            WellPointProperty dipAzimuth = properties[azimuthControl.SelectedIndex];
            WellPointProperty fractureType = properties[fractureTypeControl.SelectedIndex];
            if ((dip == null) || (dipAzimuth == null) || (fractureType == null) || (mds == null))
            {
                Restoration.Services.MessageService.ShowError("Need Dip angle, Dip azimuth, MD and a Fracture Type (as a generic continuous template) to continue");
                return toReturn;
            }


            //We need to crate the following data <Dictionary<string = wellName, List<Fracture> observedFractures >
            //or, event better: List< Fracture > and Fracture.Well = wellName; Location = global location
            //run thorugh all the wells and find those that have the pointWellLogVersion;
            List<Borehole> wells = BoreholeTools.GetBoreholesWithPointLog(pointWellLogVersion);
            if (wells.Count < 1) return toReturn;


            List<string> names = new List<string>();

            toReturn = new WellFractureDataModel();
            foreach (Borehole well in wells)
            {
                //GetPointWellLog(Borehole w, PointWellLogVersion wellLogVersion)
                PointWellLog log = BoreholeTools.GetPointWellLog(well, pointWellLogVersion);
                var countSamples = log.SortedPointWellLogSamples.Count(); //only in this well
                var sortedSamples = log.SortedPointWellLogSamples;

                foreach (var sample in sortedSamples)
                {
                    int index = sample.NativeIndex;

                    double md, dipValue = 0.0, azimuthValue = 0.0;
                    float fTypeValue = 0.0f;

                    if (dip.DataType == typeof(double))
                    {
                        dipValue = log.GetPropertyAccessAtIndex(index).GetPropertyValue<double>(dip);
                    }
                    if (dip.DataType == typeof(float))
                    {
                        dipValue = (double)log.GetPropertyAccessAtIndex(index).GetPropertyValue<float>(dip);
                    }


                    if (dipAzimuth.DataType == typeof(double))
                    {
                        azimuthValue = log.GetPropertyAccessAtIndex(index).GetPropertyValue<double>(dipAzimuth);
                    }
                    if (dipAzimuth.DataType == typeof(float))
                    {
                        azimuthValue = (double)log.GetPropertyAccessAtIndex(index).GetPropertyValue<float>(dipAzimuth);
                    }





                    fTypeValue = log.GetPropertyAccessAtIndex(index).GetPropertyValue<float>(fractureType);

                    md = log.GetPropertyAccessAtIndex(index).GetPropertyValue<double>(mds);
                    var Location = BoreholeTools.getPositionAtWellMD(well, md);

                    Fracture f = new Fracture();
                    f.Orientation = new Restoration.Model.Orientation(dipValue, azimuthValue);
                    f.Type = (FractureType)(fTypeValue);
                    f.Location = new CommonData.Vector3(Location.X, Location.Y, Location.Z);
                    f.Id = index;
                    f.Observed = true;

                    toReturn.Fractures.Add(f);

                    if (!toReturn.WellFractures.Keys.Contains(well.Name))
                        toReturn.WellFractures.Add(well.Name, new List<Fracture>());
                    toReturn.WellFractures[well.Name].Add(f);



                }

                names = toReturn.WellFractures.Keys.ToList();



                //foreach (PointWellLog log in pointWellLogs)
                //{
                //    if (log.WellLogVersion.Droid == pointWellLogVersion.Droid) //this guy has the selected data in the PointWellLog  = log 
                //    {                                                          //we should be able to get the observed fractures 


                //        var countSamples = log.SortedPointWellLogSamples.Count(); //only in this well
                //        var sortedSamples = log.SortedPointWellLogSamples;
                //        foreach (var sample in sortedSamples)
                //        {
                //            int index = sample.NativeIndex;
                //            double dipValue     = log.GetPropertyAccessAtIndex(index).GetPropertyValue<double>(dip);
                //            double azimuthValue = log.GetPropertyAccessAtIndex(index).GetPropertyValue<double>(dipAzimuth);
                //            float  fTypeValue   = log.GetPropertyAccessAtIndex(index).GetPropertyValue<float>(fractureType);
                //            ;
                //        }
                //    }
                //}//logs

            }//wells 

            return toReturn;
        }


        private void wellDropControl_DragDrop(object sender, DragEventArgs e)
        {

            Borehole well = e.Data.GetData(typeof(Borehole)) as Borehole;
            BoreholeCollection wells = e.Data.GetData(typeof(BoreholeCollection)) as BoreholeCollection;
            DisconnectWells();
            List<Borehole> wellList = new List<Borehole>();

            //wellsPresentationBox.Tag = null;
            if (well != null)
            {
                wellsPresentationBox.Image = PetrelImages.Well;
                wellsPresentationBox.Text = well.Name;
                wellList.Add(well);
            }
            else if (wells != null)
            {
                wellsPresentationBox.Image = PetrelImages.WellFolder;
                wellsPresentationBox.Text = wells.Name;
                foreach (Borehole w in wells)
                    wellList.Add(w);
            }

            else
            {
                InitializeLogDFNPanel(false, true);
                return;
            }


            Font f = new Font(wellsPresentationBox.Font.FontFamily, wellsPresentationBox.Font.Size, FontStyle.Regular);//.Font;
            wellsPresentationBox.Font = f;
            wellsPresentationBox.ForeColor = Color.Black;
            wellsPresentationBox.Tag = wellList;
            ConnectWells(wellList);


            dfnIntersectedName.Text = "DFNIntersect" + wellsPresentationBox.Text;


        }

        List<FracturePatch> _patches;
        List<FracturePatch> Patches
        {

            get
            {
                return _patches;
            }
            set
            {
                _patches = value;
            }
        }
        private void dfnDropControl_DragDrop(object sender, DragEventArgs e)
        {
            FractureNetwork dfn = e.Data.GetData(typeof(FractureNetwork)) as FractureNetwork;
            if (dfn != null)
            {
                Font f = new Font(dfnPresentationBox.Font.FontFamily, dfnPresentationBox.Font.Size, FontStyle.Regular);//.Font;
                dfnPresentationBox.Font = f;
                dfnPresentationBox.Tag = dfn;
                dfnPresentationBox.Image = PetrelImages.FaultPatches;
                dfnPresentationBox.ForeColor = Color.Black;
                dfnPresentationBox.Text = dfn.Name;

                Patches = DFNTools.GetPatchesArray(dfn).ToList();

                //sets 
                dfnSetsView.Rows.Clear();
                foreach (FractureSet set in dfn.FractureSets)
                {
                    int setIndex = set.Value;
                    int i = dfnSetsView.Rows.Add(true, setIndex.ToString());
                    dfnSetsView.Rows[i].Cells[0].Value = true;
                    dfnSetsView.Rows[i].Cells[0].Tag = setIndex;
                    dfnSetsView.Rows[i].Cells[1].Tag = setIndex;
                }


            }
            else
            {
                InitializeLogDFNPanel(true, false);
            }
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            eventTimer.Stop();

            if (WellMonitor == false)
                PetrelLogger.InfoOutputWindow("Received a timer event, but will not do any math ");

            else
            {
                PetrelLogger.InfoOutputWindow("Received a timer event, and will recalculated everything in the changed wells. Total wells  " + changedWells.Count().ToString());




                List<Borehole> wells = SelectedWells;
                FractureNetwork dfn = DFN;
                // Dictionary<Borehole, List<Tuple<FracturePatch, double>>> wellFracsIntersections = IntersectFracturesAndWells(wells, dfn);
                ////<well, List<Patch, md> > 

                // DisplayInPetrelWellIntersectedFractures(wellFracsIntersections);






                ;
            }

            //UpdateAnalysis();
        }



        #endregion


        #region DisplayControllerResults


        public void DisplayFractureModel(FractureModel model)
        {
            if (model == null) return;

            //display the results in a grid
            //if the results come from a grid, then fracure id =1 is for cell = 1, fracture id = 2, for cell 2, .... 
            if (StressPresentationBox.Tag is PropertyCollection)
            {
                using (IProgress p = PetrelLogger.NewProgress(0, 100))
                {
                    //IProgress p = PetrelLogger.NewProgress(0, 100);
                    p.SetProgressText("Creating Fracture Orientations and Intensities in the Grid");
                    PropertyCollection col = StressPresentationBox.Tag as PropertyCollection;
                    Grid g = col.Grid;


                    int counter = 0;
                    int total = Enum.GetNames(typeof(FractureType)).Count();
                    foreach (string name in Enum.GetNames(typeof(FractureType)))
                    {

                        ///try to get fracs of a specific type listed in the enum 
                        var fracs = model.Fractures.Where(t => t.Type == (FractureType)(Enum.Parse(typeof(FractureType), name)));

                        FractureType code = (FractureType)Enum.Parse(typeof(FractureType), name);


                        //only go ahead if there are any 
                        if ((code != FractureType.Shear3) && (fracs.Count() > 0))
                        {
                            //assume that the frac id is equivaklent to the cell counter or the point counter. Get the index3 or each id cell 
                            List<Index3> indices3 = fracs.Select(t =>
                            {
                                int n = t.Id;
                                int cij = g.NumCellsIJK.I * g.NumCellsIJK.J;
                                int klayer = (int)(n / cij);
                                int cellIJ = n - klayer * cij;
                                int cj = cellIJ / g.NumCellsIJK.I;
                                int ci = n - cj * g.NumCellsIJK.I - klayer * cij;

                                return new Index3(ci, cj, klayer);
                            }).ToList();


                            //select properties
                            var intensities = fracs.Select(t => (float)t.Intensity);
                            var cellIndices = fracs.Select(t => t.Id);
                            var dip = fracs.Select(t => (float)(t.Orientation.Dip));// * Math.PI / 180.0));
                            var dipAzimuth = fracs.Select(t => (float)(t.Orientation.DipAzimuth));// * Math.PI / 180.0));


                            //creqate grid properties
                            Property intensityProperties = ProjectTools.GetOrCreateProperty(col, "Intensity" + name, PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);
                            Property dipProperties = ProjectTools.GetOrCreateProperty(col, "Dip" + name, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAngle);
                            Property dipAzimuthProperties = ProjectTools.GetOrCreateProperty(col, "DipAzimuth" + name, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAzimuth);

                            //set all to undefined 
                            ProjectTools.SetValue(ref intensityProperties, float.NaN);
                            ProjectTools.SetValue(ref dipProperties, float.NaN);
                            ProjectTools.SetValue(ref dipAzimuthProperties, float.NaN);

                            //replace the values only in the indices where we have data 
                            ProjectTools.SetValues(ref intensityProperties, indices3, intensities.ToList());
                            ProjectTools.SetValues(ref dipProperties, indices3, dip.ToList());
                            ProjectTools.SetValues(ref dipAzimuthProperties, indices3, dipAzimuth.ToList());


                        }//count > 0 


                        counter += 1;
                        int progress = (int)(1 + 100.0 * counter / total);
                        p.ProgressStatus = (int)(progress);


                        if (counter == total) p.ProgressStatus = 101;

                    }




                }
            }//it is a grid 



            //display the results as point with attributes
            else if (StressPresentationBox.Tag is PointSet)
            {
            }

            else
            {
                ;
            }



        }

        public void DisplayWellComparison(WellFractureDataModel data)
        {

            //we will use these to create histograms
            List<float> allTypeMatchedCosts = new List<float>();
            List<float> allSmallestCosts = new List<float>();

            try
            {

                //patch addded Assef in the last minute
                //is there a plastic strain in the grid ?
                PropertyCollection col = modelPredictedOrientationPresentationBox.Tag as PropertyCollection;
                Property plasticStrain = ProjectTools.GetProperty(col, "Equivalent Plastic Strain", PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);




                int counter = 0; int total = data.WellFractures.Keys.Count();
                using (IProgress p = PetrelLogger.NewProgress(0, 100))
                {
                    p.ProgressStatus = 0;
                    p.SetProgressText("Creating comparison data as Point Logs ");


                    //foreach well, create a point set for each fracture. 
                    foreach (string wellName in data.WellFractures.Keys)
                    {
                        Borehole well = BoreholeTools.findWellByName(wellName);
                        PointWellLog xx = BoreholeTools.GetOrCreatePointWellLog(well, "ComparisonModelObservations_" + data.Name);

                        var observedFracs = data.WellFractures[wellName];
                        foreach (Fracture f in observedFracs)
                            if (f.Extension == null)  //there are observed fractures that do not have a cost function. 
                            {
                                f.Extension = new FractureCostFunction(); //create a dummy if there isnt one. Makes everything easier 
                            }

                        //Monzurul's patch
                        //only display comparison of those in that have extension !=null 
                        observedFracs = data.WellFractures[wellName].Where(t => t.Extension != null).ToList();


                        var point3Locations = observedFracs.Select(t => new Point3(t.Location.X, t.Location.Y, t.Location.Z));
                        List<double> mds = point3Locations.Select(t => (double)BoreholeTools.GetWellMDAtPosition(well, t)).ToList();

                        using (ITransaction trans = DataManager.NewTransaction())
                        {
                            trans.Lock(xx);
                            trans.Lock(xx.WellLogVersion);

                            xx.PointWellLogSamples.ToList().Clear();// = null;
                            trans.Commit();
                        }


                        BoreholeTools.SetPointWellLogMDs(xx, mds);
                        {
                            IEnumerable<float> dip = observedFracs.Select(t => (float)t.Orientation.Dip);
                            IEnumerable<float> dipAzimuth = observedFracs.Select(t => (float)t.Orientation.DipAzimuth);

                            var dips = dip.ToList();
                            var azims = dipAzimuth.ToList();

                            BoreholeTools.SetPointWellLogAttribute(xx, "Dip angle", dips, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAngle);
                            BoreholeTools.SetPointWellLogAttribute(xx, "Dip azimuth", azims, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAzimuth);
                        }
                        {
                            IEnumerable<FractureCostFunction> costFunctions = observedFracs.Select(t => (FractureCostFunction)(t.Extension));
                            IEnumerable<float> typeMatchedCost = costFunctions.Select(t => t.MatchingTypeCost);


                            var typeMatchedCostList = typeMatchedCost.ToList();
                            BoreholeTools.SetPointWellLogAttribute(xx, "CostMatchedType", typeMatchedCostList, PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);

                            allTypeMatchedCosts.AddRange(typeMatchedCost);

                            PetrelLogger.InfoOutputWindow("Doing the cost function thingy for well " + wellName);

                            IEnumerable<Tuple<float, int>> filteredValidBestFits = costFunctions.Where(t => t.SmallestCostAndType != null).Select(t => t.SmallestCostAndType);
                            IEnumerable<float> smallestCost = filteredValidBestFits.Select(t => t.Item1);
                            IEnumerable<float> corrspondingType = filteredValidBestFits.Select(t => (float)t.Item2);

                            allSmallestCosts.AddRange(smallestCost);

                            BoreholeTools.SetPointWellLogAttribute(xx, "LowestCost ", smallestCost.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);
                            BoreholeTools.SetPointWellLogAttribute(xx, "LowestCostType", corrspondingType.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);
                            PetrelLogger.InfoOutputWindow("!!!Done the cost function thingy for well " + wellName);
                            //display the rest of the types match
                        }



                        var cc = 0;
                        {
                            IEnumerable<float> fTypes = observedFracs.Select(t => 1.0f * (int)t.Type);
                            BoreholeTools.SetPointWellLogAttribute(xx, "FractureType", fTypes.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);

                            cc = fTypes.Count();
                        }
                        {
                            IEnumerable<float> x = observedFracs.Select(t => (float)t.Location.X);
                            BoreholeTools.SetPointWellLogAttribute(xx, "X", x.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);

                            IEnumerable<float> y = observedFracs.Select(t => (float)t.Location.Y);
                            BoreholeTools.SetPointWellLogAttribute(xx, "Y", x.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);


                        }



                        if (plasticStrain != null)
                        {
                            Index3 i3;

                            try
                            {
                                Grid g = col.Grid;
                                List<float> strains = new List<float>();
                                foreach (var location in point3Locations)
                                {
                                    i3 = g.GetCellAtPoint(location);
                                    if (i3 != null)
                                    {
                                        float value = plasticStrain[i3];
                                        strains.Add(value);
                                    }
                                    else
                                        strains.Add(float.NaN);
                                }
                                BoreholeTools.SetPointWellLogAttribute(xx, "Equivalent Plastic Strain", strains.ToList(), PetrelProject.WellKnownTemplates.GeomechanicGroup.Strain);
                                cc = strains.Count();

                            }
                            catch (Exception ee)
                            {
                                var ww = ee.ToString();
                                PetrelLogger.InfoOutputWindow("The program crashed in the visualization. This is the error " + ww);
                            }

                        }

                        int progress = (int)(1 + 100 * (++counter) / total);
                        p.ProgressStatus = (int)(progress);//100 *(int)( ((double)yyy / (double)dfn.FracturePatchCount));

                    }//wells done

                }
                //BoreholeTools.SetPointWellLogAttribute(xx, "Dip angle", dips, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAngle);
                //BoreholeTools.SetPointWellLogAttribute(xx, "Dip azimuth", azimuths, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAzimuth);






                //var observedFracs = data.WellFractures[wellName];
                //var point3Locations = observedFracs.Select(t => new Point3(t.Location.X, t.Location.Y, t.Location.Z));
                //List<double> mds = point3Locations.Select(t => (double)BoreholeTools.GetWellMDAtPosition(well, t)).ToList();

                //IEnumerable<float> dip = observedFracs.Select(t => (float)t.Orientation.Dip);
                //IEnumerable<float> dipAzimuth = observedFracs.Select(t => (float)t.Orientation.DipAzimuth);

                //DothingWithWell(well, mds, dip.ToList(), dipAzimuth.ToList());

                //BoreholeTools.SetFracturePointData(well, "ThisName", mds, dip.ToList(), dipAzimuth.ToList());



                //Borehole well = BoreholeTools.findWellByName(wellName);
                //PointWellLog log = BoreholeTools.GetOrCreatePointWellLog(well, "ModelComparison_" + data.Layer);

                //var observedFracs = data.WellFractures[wellName];
                //foreach (Fracture f in observedFracs)
                //    if (f.Extension == null)  //there are observed fractures that do not have a cost function. 
                //    {
                //        f.Extension = new FractureCostFunction(); //create a dummy if there isnt one. Makes everything easier 
                //    }
                //IEnumerable<FractureCostFunction> costFunctions = observedFracs.Select(t => (FractureCostFunction)(t.Extension));



                //var point3Locations = observedFracs.Select(t => new Point3(t.Location.X, t.Location.Y, t.Location.Z));
                //List<float> mds = point3Locations.Select(t => BoreholeTools.GetWellMDAtPosition(well, t)).ToList();


                ////create the type-matched attribute 
                //IEnumerable<float> typeMatchedCost = costFunctions.Select(t => t.MatchingTypeCost);
                ////BoreholeTools.SetPointWellLogMDs(log,  mds );
                ////BoreholeTools.SetPointWellLogAttribute(log, "CostMatchedType",  typeMatchedCost.ToList(), PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);

                ////now the dip and dip-azimuth. these are already in the observed fractures and are compulsory for the stereonet plotting 
                //IEnumerable<float> dip = observedFracs.Select(t => (float)t.Orientation.Dip);
                //IEnumerable<float> dipAzimuth = observedFracs.Select(t => (float)t.Orientation.DipAzimuth);

                //BoreholeTools.SetFracturePointData(well, "ThisName", mds, dip.ToList(), dipAzimuth.ToList());



                //BoreholeTools.SetPointWellLogAttribute(log, "Dip angle",  typeMatchedCost.ToList(), PetrelProject.WellKnownTemplates.GeometricalGroup.DipAngle);
                //BoreholeTools.SetPointWellLogAttribute(log, "Dip azimuth", typeMatchedCost.ToList(), PetrelProject.WellKnownTemplates.GeometricalGroup.DipAzimuth);


                //PointWellLog aux = BoreholeTools.GetOrCreatePointWellLog(well, "ModelComparison_" + data.Layer);

                //;
                //;
                //;



            }

            catch (Exception e)
            {
                var txt = e.ToString();
                PetrelLogger.InfoOutputWindow("The program crashed in displaying the point well log data ");
            }



        }

        #region map to grid 
        //public void DisplayStress(Dictionary<string, List<double>> stressPointData)
        //{


        //    PropertyCollection stressMappedToGrid = checkBox2.Checked ? MapStressToGrid(stressPointData) : null;
        //    PointSet stressMappedToPoints = MapStressToPointsWithAttributes(stressPointData);


        //    if (stressMappedToGrid != null)
        //    {
        //        StressDataDropped(stressMappedToGrid);
        //        //ModelOrientationsDropped(stressMappedToGrid);
        //    }
        //    else if (stressMappedToPoints != null)
        //    {
        //        StressDataDropped(stressMappedToPoints);
        //        InitializeComparisonControls();
        //    }
        //    else
        //    {
        //        InitializeStressProcessingControls();
        //        InitializeComparisonControls();
        //    }

        //}

        //public PropertyCollection MapStressToGrid(Dictionary<string, List<double>> stressPointData)
        //{
        //    if (Grid == null) return null;

        //    //get the data 
        //    float[] sxx = stressPointData["TOTSTRXX"].Select(t => (float)(t)).ToArray();
        //    float[] syy = stressPointData["TOTSTRYY"].Select(t => (float)(t)).ToArray();
        //    float[] szz = stressPointData["TOTSTRZZ"].Select(t => (float)(t)).ToArray();
        //    float[] sxy = stressPointData["TOTSTRXY"].Select(t => (float)(t)).ToArray();
        //    float[] syz = stressPointData["TOTSTRYZ"].Select(t => (float)(t)).ToArray();
        //    float[] szx = stressPointData["TOTSTRZX"].Select(t => (float)(t)).ToArray();
        //    float[] x = stressPointData["x"].Select(t => (float)(t)).ToArray();
        //    float[] y = stressPointData["y"].Select(t => (float)(t)).ToArray();
        //    float[] z = stressPointData["z"].Select(t => (float)(t)).ToArray();
        //    float[][] data = new float[9][] { sxx, syy, szz, sxy, syz, szx, x, y, z };
        //    Dictionary<string, float[]> names = new Dictionary<string, float[]>()
        //    {
        //        { "TOTSTRXX",sxx },
        //        { "TOTSTRYY",syy},
        //        { "TOTSTRZZ",szz },
        //        { "TOTSTRXY",sxy },
        //        { "TOTSTRYZ",syz },
        //        { "TOTSTRZX",szx}
        //    };

        //    int count = x.Length;

        //    Dictionary<Index3, float> invCounter = new Dictionary<Index3, float>();
        //    Dictionary<Index3, float> indexedValues = new Dictionary<Index3, float>();
        //    Index3[] indices = new Index3[count];
        //    for (int n = 0; n < count; n++)
        //    {

        //        Index3 i3 = Grid.GetCellAtPoint(new Point3(x[n], y[n], z[n]));
        //        if (!indexedValues.Keys.Contains(i3)) indexedValues.Add(i3, 0.0f);
        //        if (!invCounter.Keys.Contains(i3)) invCounter.Add(i3, 0.0f);
        //        invCounter[i3] = invCounter[i3] + 1.0f;
        //        indices[n] = i3;
        //    }


        //    PropertyCollection col = ProjectTools.GetOrCreatePropertyCollection(Grid, ModelName);

        //    foreach (string name in names.Keys)
        //    {
        //        float[] arrayData = names[name];
        //        float[] avgValues = Enumerable.Repeat(0.0f, count).ToArray();

        //        for (int k = 0; k < indexedValues.Keys.Count; k++) indexedValues[indexedValues.Keys.ElementAt(k)] = 0.0f;

        //        for (int k = 0; k < count; k++)
        //            indexedValues[indices[k]] += (arrayData[k] / invCounter[indices[k]]);

        //        Property p = ProjectTools.GetOrCreateProperty(col, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
        //        ProjectTools.SetValue(ref p, float.NaN);
        //        ProjectTools.SetValues(ref p, indexedValues.Keys.ToList(), indexedValues.Values.ToList());
        //    }


        //    return col;

        //} //map stress to grid 

        //public PointSet MapStressToPointsWithAttributes(Dictionary<string, List<double>> stressPointData)
        //{
        //    //get the data 
        //    double[] sxx = stressPointData["TOTSTRXX"].ToArray();//;//.Select(t => (float)(t)).ToArray();
        //    double[] syy = stressPointData["TOTSTRYY"].ToArray();//;//.Select(t => (float)(t)).ToArray();
        //    double[] szz = stressPointData["TOTSTRZZ"].ToArray();//;//.Select(t => (float)(t)).ToArray();
        //    double[] sxy = stressPointData["TOTSTRXY"].ToArray();//;//.Select(t => (float)(t)).ToArray();
        //    double[] syz = stressPointData["TOTSTRYZ"].ToArray();//;//.Select(t => (float)(t)).ToArray();
        //    double[] szx = stressPointData["TOTSTRZX"].ToArray();//.Select(t => (float)(t)).ToArray();
        //    double[] x = stressPointData["x"].ToArray();//Select(t => (float)(t)).ToArray();
        //    double[] y = stressPointData["y"].ToArray();//Select(t => (float)(t)).ToArray();
        //    double[] z = stressPointData["z"].ToArray();//Select(t => (float)(t)).ToArray();
        //    double[][] data = new double[9][] { sxx, syy, szz, sxy, syz, szx, x, y, z };
        //    Dictionary<string, double[]> names = new Dictionary<string, double[]>()
        //    {
        //        { "TOTSTRXX",sxx },
        //        { "TOTSTRYY",syy},
        //        { "TOTSTRZZ",szz },
        //        { "TOTSTRXY",sxy },
        //        { "TOTSTRYZ",syz },
        //        { "TOTSTRZX",szx}
        //    };

        //    int count = x.Length;

        //    List<Point3> coords = new List<Point3>();
        //    for (int n = 0; n < count; n++) coords.Add(new Point3(x[n], y[n], z[n]));

        //    Collection col = ProjectTools.GetOrCreateCollectionByName(ModelName);
        //    PointSet pts = ProjectTools.GetOrCreatePointSet(ref col, coords, ModelName);

        //    foreach (string name in names.Keys)
        //    {
        //        PointProperty p = ProjectTools.GetOrCreatePointProperty(pts, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
        //        var values = names[name];
        //        ProjectTools.SetPropertyValues(p, values);
        //    }


        //    return pts;
        //}



        #endregion

        #endregion


        private PropertyCollection RotateStress(PropertyCollection col, float angleDegrees)
        {

            Dictionary<string, float[]> names = GetStressFromGridPropertyFolder(col);


            float angle = angleDegrees;

            float a1 = (float)Math.Cos(Math.PI * angle / 180.0);
            float a2 = -(float)Math.Sin(Math.PI * angle / 180.0);

            float b1 = -a2;// (float)Math.Cos(Math.PI * angle / 180.0);
            float b2 = a1; //(float)Math.Sin(Math.PI * angle / 180.0);


            var sxx = names["TOTSTRXX"];
            var syy = names["TOTSTRYY"];
            var szz = names["TOTSTRZZ"];
            var sxy = names["TOTSTRXY"];
            var syz = names["TOTSTRYZ"];
            var szx = names["TOTSTRZX"];


            for (int n = 0; n < sxx.Count(); n++)
            {
                float sxxprime = sxx[n] * a1 + sxy[n] * a2;

                float sxyprime = sxy[n] * a1 + syy[n] * a2;

                float szxprime = szx[n] * a1 + syz[n] * a2;

                float syyprime = sxy[n] * b1 + syy[n] * b2;

                float syzprime = szx[n] * b1 + syz[n] * b2;

                sxx[n] = sxxprime;
                syy[n] = syyprime;
                sxy[n] = sxyprime;
                syz[n] = syzprime;
                szx[n] = szxprime;
            }


            //create  new properties. 
            PropertyCollection rotatedCollection = ProjectTools.GetOrCreatePropertyCollection(col.Grid, col.Name + "Rotated");

            Property p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRXX", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, sxx.Select(t => t).ToList());

            p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRYY", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, syy.Select(t => t).ToList());

            p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRZZ", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, szz.Select(t => t).ToList());


            p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRXY", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, sxy.Select(t => t).ToList());

            p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRYZ", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, syz.Select(t => t).ToList());

            p = ProjectTools.GetOrCreateProperty(rotatedCollection, "TOTSTRZX", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            ProjectTools.SetValues(p, szx.Select(t => t).ToList());


            return rotatedCollection;
        }


        Dictionary<string, float[]> GetStressFromGridPropertyFolder(PropertyCollection col)
        {
            Dictionary<string, float[]> names = new Dictionary<string, float[]>()
                    {
                    { "TOTSTRXX",null},
                    { "TOTSTRYY",null},
                    { "TOTSTRZZ",null },
                    { "TOTSTRXY",null },
                    { "TOTSTRYZ",null },
                    { "TOTSTRZX",null}
                    };

            //get the properties if we have them in the collection. Otherwise, something is wrong. 
            for (int i = 0; i < names.Keys.Count(); i++)
            {
                string name = names.Keys.ElementAt(i);
                names[name] = ProjectTools.GetGridPropertyValues(col, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);

                //where I have stress, there must be a cell center. Lets check that var notNullCellCenters = names[names.Keys.ElementAt(0)].Where((t, ii) => !double.IsNaN(locations[ii].X))  ;
                if (names[name] == null)
                {
                    // MessageService.ShowError("Property " + name + " is missing or it doesnt have the expected template");
                    return null;
                }
            }


            return names;
        }

        Dictionary<string, float[]> GetTensorFromPropertyFolder(PropertyCollection col, string tensorName, Template t)
        {
            Dictionary<string, float[]> names = new Dictionary<string, float[]>()
                    {
                    { tensorName+"XX",null},
                    { tensorName+"YY",null},
                    { tensorName+"ZZ",null },
                    { tensorName+"XY",null },
                    { tensorName+"YZ",null },
                    { tensorName+"ZX",null}
                    };


            var anames = col.Properties.Select(rt => rt.Name);



            //get the properties if we have them in the collection. Otherwise, something is wrong. 
            for (int i = 0; i < names.Keys.Count(); i++)
            {
                string name = names.Keys.ElementAt(i);
                //names[name] 
                float[] values = ProjectTools.GetGridPropertyValues(col, name, t);


                if (values == null)
                    return null;

                else
                    names[name] = values;

            }


            return names;
        }




        private void optionsControl_Click(object sender, EventArgs e)
        {
            if (optionsPanel.Visible)
            {
                optionsPanel.Visible = false;
                optionsControl.Image = imageList2.Images[0];
            }
            else
            {
                optionsPanel.Visible = true;
                optionsControl.Image = imageList2.Images[1];
            }
        }

        List<Borehole> SelectedWells
        {
            get
            {
                return (wellsPresentationBox.Tag != null ? wellsPresentationBox.Tag as List<Borehole> : null);
            }
        }
        RegularHeightFieldSurface SelectedHorizon
        {
            get
            {
                return (horizonPresentationbox.Tag != null ? horizonPresentationbox.Tag as RegularHeightFieldSurface : null);
            }
        }

        FractureNetwork DFN
        {
            get
            {
                return (dfnPresentationBox.Tag != null ? dfnPresentationBox.Tag as FractureNetwork : null);
            }
        }


        private void logDFNButton_Click(object sender, EventArgs e)
        {
            if (DFN == null) return;

            if(SelectedWells!=null)
            ComputeAndDisplayDFNWellIntersection();

            if(SelectedHorizon != null )
            ComputeAndDisplayDFNHorizonIntersection();

        }

        private void ComputeAndDisplayDFNHorizonIntersection()
        {
            RegularHeightFieldSurface s = horizonPresentationbox.Tag as RegularHeightFieldSurface;
            FractureNetwork dfn = DFN;

            if (s == null) return;
            List<FracturePatch> fracs = Patches.Where(t => DFNTools.IntersectSurface(t, s) == true).ToList();

            string name = horizonIntersectName.Text; 
            FractureNetwork intersected = DFNTools.GetOrCreateFractureNetwork(name);
            DFNTools.DeleteFractures(intersected);
            DFNTools.CopyPatches(fracs, intersected);


        }

        private void ComputeAndDisplayDFNWellIntersection()
        {
            FractureNetwork dfn = DFN;


            //get all the wells selected
       
            List<Borehole> wells = SelectedWells;
            

            //added to please Mozurul. 
            //get from the table the selected patches (by index of patch) and fuilter out from allPatches those with a different set index.
            List<int> selectedSets = new List<int>();
            for (int n = 0; n < dfnSetsView.Rows.Count; n++)
            {
                var row = dfnSetsView.Rows[n];
                if ((bool)(row.Cells[0].Value) == true) //selected by checkbox 
                    selectedSets.Add((int)(row.Cells[0].Tag)); //created when dropped the dfn 
            }


            Dictionary<Borehole, List<Tuple<FracturePatch, double>>> wellFracsIntersections = IntersectFracturesAndWells(wells, dfn, selectedSets);
            //<well, List<Patch, md> > 
            string name = dfnIntersectedName.Text.Trim();
            DisplayInPetrelWellIntersectedFractures(wellFracsIntersections, name);
        }

        void DisplayInPetrelWellIntersectedFractures(Dictionary<Borehole, List<Tuple<FracturePatch, double>>> intersections, string name = "")
        {


            if (name == "") name = "Intersected" + dfnPresentationBox.Text + wellsPresentationBox.Text;

            //first, create an intersected DFN if the user selected that option,
            //if (generateWellIntersectedCheckBox.Checked)
            {
                List<FracturePatch> fracs = new List<FracturePatch>();

                foreach (Borehole w in intersections.Keys)
                {
                    var aux = intersections[w].Select(t => t.Item1);
                    foreach (FracturePatch f in aux) if (!fracs.Contains(f)) fracs.Add(f);
                }

                FractureNetwork intersected = DFNTools.GetOrCreateFractureNetwork(name);
                DFNTools.DeleteFractures(intersected);
                DFNTools.CopyPatches(fracs, intersected);
            }

            if (generateWellPointDataOfOrientationsCheckBox.Checked == true)
            {
                foreach (Borehole w in intersections.Keys)
                {
                    var aux = intersections[w];
                    List<double> mds = aux.Select(t => t.Item2).ToList();
                    var patches = aux.Select(t => t.Item1);
                    List<float> dips = patches.Select(t => (float)t.Dip.Radians).ToList();
                    List<float> azimuths = patches.Select(t => (float)t.DipAzimuth.Radians).ToList();
                    BoreholeTools.SetFracturePointData(w, name, mds, dips, azimuths);
                }

            }



        }


        Dictionary<Borehole, List<Tuple<FracturePatch, double>>> IntersectFracturesAndWells(List<Borehole> wells, FractureNetwork dfn, List<int> setIndicesSelected = null)
        {
            List<Tuple<FracturePatch, double>> toReturn = new List<Tuple<FracturePatch, double>>();

            Dictionary<Borehole, List<Tuple<FracturePatch, double>>> toReturn2 = new Dictionary<Borehole, List<Tuple<FracturePatch, double>>>();

            //List<Borehole> wells = new List<Borehole>();
            //Borehole well = wellsPresentationBox.Tag as Borehole;
            //BoreholeCollection wellCollection = wellsPresentationBox.Tag as BoreholeCollection;

            //if (well != null) wells.Add(well);
            //else if (wellCollection != null) wells.AddRange(BoreholeTools.GetAllWells(wellCollection));
            //else return toReturn2;

            if ((wells == null) || (wells.Count() < 1) || (dfn == null) || (dfn == FractureNetwork.NullObject))
                return toReturn2;

            List<FracturePatch> allPatches = null;
            if (setIndicesSelected == null)
                allPatches = Patches;
            else
            {

                //added to please Mozurul. 
                allPatches = Patches.Where(t => setIndicesSelected.IndexOf(t.FractureSetValue) != -1).ToList();
            }
            if (allPatches.Count < 1)
                return toReturn2;


            double maxFractureLength = Math.Sqrt(Patches.Select(t => Slb.Ocean.Geometry.Vector3.Dot(t.MajorAxis, t.MajorAxis)).Max());
            //double minFractureLength = Math.Sqrt(dfn.FracturePatches.Select(t => Slb.Ocean.Geometry.Vector3.Dot(t.MajorAxis, t.MajorAxis)).Min());



            double minMD = 0.0;
            double maxMD = 100000.00;

            List<Point3> inters = new List<Point3>();

            //well trayecrory points 
            foreach (Borehole w in wells)
            {
                toReturn2.Add(w, new List<Tuple<FracturePatch, double>>());

                maxMD = w.MDRange.Max;
                List<Point3> pts = w.Trajectory.TrajectoryPolylineRecords.Where(t => ((t.MD > minMD) && (t.MD < maxMD))).Select(t => t.Point).ToList();    //this are the points along the well that we will consider 
                pts.Add(BoreholeTools.getPositionAtWellMD(w, maxMD * 1.01));

                //double mo = minMD;
                //pts.Clear();
                //double delta = Math.Min(minFractureLength, (maxMD - minMD) / 10000.00);

                //while (mo < maxMD)
                //{
                //    pts.Add(BoreholeTools.getPositionAtWellMD(w, mo));
                //    mo += delta; 
                // }

                //pts.Clear();
                //pts.Add(BoreholeTools.getPositionAtWellMD(w, w.MDRange.Min));
                //pts.Add(BoreholeTools.getPositionAtWellMD(w, w.MDRange.Max));


                double xmin = -maxFractureLength + pts.Select(t => t.X).Min();
                double xmax = maxFractureLength + pts.Select(t => t.X).Max();
                double ymin = -maxFractureLength + pts.Select(t => t.Y).Min();
                double ymax = maxFractureLength + pts.Select(t => t.Y).Max();
                double zmin = -maxFractureLength + pts.Select(t => t.Z).Min();
                double zmax = maxFractureLength + pts.Select(t => t.Z).Max();
                double[] limits = new double[6] { xmin, xmax, ymin, ymax, zmin, zmax };

                //this is a preliminary of the fracs that might intersect the well.  They are within a box around the well limitted by minMD, maxMD box 
                List<FracturePatch> patches = allPatches.Where(t => ProjectTools.IsInside3DBox(t.Center, limits)).Where(t => ProjectTools.IsInside2DBox(t.Center, limits)).ToList();
                CommonData.Vector3[] patchCenters = allPatches.Select(t => new CommonData.Vector3(t.Center.X, t.Center.Y, t.Center.Z)).ToArray();

                //create a dfn to show in Petrel just for debugging. 
                // FractureNetwork intersected = DFNTools.GetOrCreateFractureNetwork("Intersected1");
                // DFNTools.DeleteFractures(intersected);
                //DFNTools.CopyPatches(patches, intersected);


                //Slb.Ocean.Petrel.DomainObject.Collection col = ProjectTools.GetOrCreateCollectionByName("test1");
                //ProjectTools.CreatePointSet( ref col, pts, "TrayectoryPoints");


                for (int i = 0; i < pts.Count - 1; i++)
                {

                    limits = new double[6]
                    {
                            Math.Min( pts[i].X, pts[i+1].X) - 0.5*maxFractureLength,
                            Math.Max( pts[i].X, pts[i+1].X)+0.5*maxFractureLength,

                            Math.Min( pts[i].Y, pts[i+1].Y)-0.5*maxFractureLength,
                            Math.Max( pts[i].Y, pts[i+1].Y)+0.5*maxFractureLength,

                            Math.Min( pts[i].Z, pts[i+1].Z)-0.5*maxFractureLength,
                            Math.Max( pts[i].Z, pts[i+1].Z)+0.5*maxFractureLength

                    };
                    List<FracturePatch> patches2 = patches.Where(t => ProjectTools.IsInside3DBox(t.Center, limits)).ToList();

                    foreach (FracturePatch f in patches2)
                    {
                        Point3 center = f.Center;
                        Direction3 normal = new Direction3(Slb.Ocean.Geometry.Vector3.Cross(f.MinorAxis.ToNormalized(), f.MajorAxis.ToNormalized()).ToNormalized());
                        Plane3 plane = new Plane3(f.Center, normal);

                        //Slb.Ocean.Geometry.Vector3 dr = new Slb.Ocean.Geometry.Vector3(f.Center - pts[i]);
                        //Slb.Ocean.Geometry.Vector3 r12 = new Slb.Ocean.Geometry.Vector3(pts[i + 1] - pts[i]);
                        //double l1 = Slb.Ocean.Geometry.Vector3.Dot(r12, r12);
                        //double l3 = Slb.Ocean.Geometry.Vector3.Dot(dr,dr);

                        if (true)//l3 - l1 * l1 < 0.25 * Slb.Ocean.Geometry.Vector3.Dot(f.MajorAxis, f.MajorAxis)) //closeEnough = true; 
                        {
                            Segment3 segment = new Segment3(pts[i], pts[i + 1]);
                            Line3 line = new Line3(pts[i], pts[1 + i]);

                            //This method can sometimes return Point3.Null even in case of intersecting plane
                            //     and line, use Slb.Ocean.Geometry.Plane3Extensions.Intersect(Slb.Ocean.Geometry.Plane3,Slb.Ocean.Geometry.Line3,System.Double)
                            //     instead. Calls the Slb.Ocean.Geometry.Plane3Extensions.Intersect(Slb.Ocean.Geometry.Plane3,Slb.Ocean.Geometry.Line3,System.Double)
                            //     method with epsilon=0.0.
                            Point3 interSection = Slb.Ocean.Geometry.Plane3Extensions.Intersect(plane, line, 0.00001);
                            if ((interSection != Point3.Null) && (DFNTools.isPointInFracturePatch(interSection, f)) && (BoreholeTools.IsPointInSegment(interSection, pts[i], pts[i + 1])))
                            {
                                //inters.Add(interSection);
                                double md = BoreholeTools.GetWellMDAtPosition(w, interSection);
                                Tuple<FracturePatch, double> item = new Tuple<FracturePatch, double>(f, md);
                                toReturn.Add(item); //break;

                                toReturn2[w].Add(item);
                            }
                        }
                    }//patches 
                }//points  





            }//wells 



            //    //old code, a bit inefficient....
            //    foreach (FracturePatch f in patches)
            //    {

            //        Point3 center = f.Center;
            //        Direction3 normal = new Direction3(Slb.Ocean.Geometry.Vector3.Cross(f.MinorAxis.ToNormalized(), f.MajorAxis.ToNormalized()).ToNormalized());
            //        Plane3 plane = new Plane3(f.Center, normal);



            //            Slb.Ocean.Geometry.Vector3 dr = new Slb.Ocean.Geometry.Vector3(f.Center - pts[i]);
            //            Slb.Ocean.Geometry.Vector3 r12 = new Slb.Ocean.Geometry.Vector3(pts[i + 1] - pts[i]);
            //            double l1 = Slb.Ocean.Geometry.Vector3.Dot(dr, r12);
            //            double l3 = dr.X * dr.X + dr.Y * dr.Y + dr.Z * dr.Z;

            //            if ( true )//l3 - l1 * l1 < 0.25 * Slb.Ocean.Geometry.Vector3.Dot(f.MajorAxis, f.MajorAxis)) //closeEnough = true; 
            //            {
            //                Segment3 segment = new Segment3(pts[i], pts[i+1]);
            //                Line3 line = new Line3( pts [i], pts[1+i] );

            //               // Point3 interSection = Plane3Extensions.Intersect(plane, segment);
            //                Point3 interSection = Plane3Extensions.Intersect(plane, line);

            //                if ((interSection != Point3.Null)&& (DFNTools.isPointInFracturePatch(interSection, f) ))
            //                {

            //                    inters.Add(interSection); 

            //                    double md = BoreholeTools.GetWellMDAtPosition(well, interSection);
            //                    Tuple<FracturePatch, double> item = new Tuple<FracturePatch, double>(f, md);
            //                    toReturn.Add(item); //break;
            //                }
            //                else
            //                {
            //                    ;
            //                }
            //            }
            //        }
            //    }//patches 

            //}//wells 


            //publish as debug 
            //create a dfn to show in Petrel just for debugging. 


            //            Slb.Ocean.Petrel.DomainObject.Collection col2 = ProjectTools.GetOrCreateCollectionByName("test1");
            //            ProjectTools.CreatePointSet(ref col2, inters, "INTER");


            //var points = well 
            ////well trajectory poin ts 
            //Slb.Ocean.Geometry.Polyline3 line = well.Trajectory.TrajectoryPolyline as Slb.Ocean.Geometry.Polyline3;

            //var trajectory = well.Trajectory;
            //var records = trajectory.TrajectoryPolylineRecords;
            //foreach (var record in records)
            //{
            //    var azimuth = record.Azimuth;
            //    Slb.Ocean.Geometry.Point3 point = record.Point; 

            //    ;
            //}

            ////List<Point3> pts = new List<Point3>();
            ////IEnumerator<Point3> it = line.GetEnumerator();
            ////while (it.MoveNext()) { pts.Add(it.Current); }
            ////return pts;


            // return toReturn;


            return toReturn2;
        }


        /// <summary>
        /// ////////
        /// 
        /// 
        /// </summary>
        /// <returns></returns>



        private bool SwitchWellMonitorOnOff()
        {
            WellMonitor = !(WellMonitor);
            return WellMonitor;
        }

        private bool WellMonitor
        {
            get
            {
                if ((logDFNButton.Tag == null) || (logDFNButton.Tag.ToString().ToLower() == "false") || ((bool)(logDFNButton.Tag) == false)) return false;

                else
                    return true;
            }

            set
            {

                logDFNButton.Tag = value;

            }

        }


        private void ConnectWells(List<Borehole> wells)
        {
            if (connectedWells == null) connectedWells = new List<Borehole>();
            foreach (Borehole w in wells)
            {
                w.Changed += new System.EventHandler<DomainObjectChangeEventArgs>(this.wellChanged);

                w.Trajectory.Changed += new System.EventHandler<DomainObjectChangeEventArgs>(this.trayectoryChanged);
                connectedWells.Add(w);
            }
            PetrelLogger.InfoOutputWindow("Wells Connected");
        }

        private void DisconnectWells()
        {
            if ((connectedWells == null) || (connectedWells.Count < 1)) return;

            for (int n = 0; n < connectedWells.Count; n++) connectedWells[n].Changed -= wellChanged;

            for (int n = 0; n < connectedWells.Count; n++) connectedWells[n].Trajectory.Changed -= trayectoryChanged;


            connectedWells.Clear();
            changedWells.Clear();
            PetrelLogger.InfoOutputWindow("Wells disconnected");
        }

        private void wellChanged(object sender, DomainObjectChangeEventArgs e)
        {
            //Borehole w = (Borehole)(sender);
            //if ((changedWells.Count() < 1) || (changedWells[changedWells.Count() - 1] != w))
            //    changedWells.Add(w);
            //eventTimer.Start();

        }
        private void trayectoryChanged(object sender, DomainObjectChangeEventArgs e)
        {
            //Trajectory t = sender as Trajectory;
            //PetrelLogger.InfoOutputWindow("Recalculating all !!!");

            //Borehole w = null;
            //foreach (Borehole well in connectedWells)
            //{
            //    if (well.Trajectory == t)
            //    {
            //        PetrelLogger.InfoOutputWindow("Or better, jmust do well " + well.Layer );
            //    }
            //}


            ////      Borehole w = t.B (Borehole)(sender);
            ////      if ((changedWells.Count() < 1) || (changedWells[changedWells.Count() - 1] != w))
            ////          changedWells.Add(w);
            ////      eventTimer.Start();

        }







        private void stressDropControl_DragDrop(object sender, DragEventArgs e)
        {
            Object obj = e.Data.GetData(typeof(Object)) as Object; ;
            StressDataDropped(obj);
        }

        private void zoneFilterButton_Click(object sender, EventArgs e)
        {

        }

        private void wellsPresentationBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                InitializeLogDFNPanel(false, true);
            }
        }


        private void dfnPresentationBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                InitializeLogDFNPanel(true, false);
            }
        }

        private void horizonDropcontrol_DragDrop(object sender, DragEventArgs e)
        {


            RegularHeightFieldSurface s = e.Data.GetData(typeof(RegularHeightFieldSurface)) as RegularHeightFieldSurface;

            //wellsPresentationBox.Tag = null;
            if (s != null)
            {
                horizonPresentationbox.Image = PetrelImages.Surface;
                horizonPresentationbox.Text = s.Name;
            }
            else
            {
                horizonPresentationbox.Image = null;;
                horizonPresentationbox.Text = string.Empty;
                return;
            }
            Font f = new Font(horizonPresentationbox.Font.FontFamily, horizonPresentationbox.Font.Size, FontStyle.Regular);//.Font;
            horizonPresentationbox.Font = f;
            horizonPresentationbox.ForeColor = Color.Black;
            horizonPresentationbox.Tag = s;
        }

       
    }//class
}//namespace



