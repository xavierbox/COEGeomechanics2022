using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Restoration.OceanUtils;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using System.IO;
using System.Collections.Generic;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using System.Linq;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject;

namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class ImportPointStressUI : UserControl
    {

        public event EventHandler<StringEventArgs> DynelResultsFileSelected;
        public event EventHandler<DynelDatatoProcessEvent> ImportStressesFromDynelToGridClicked;


        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private ImportPointStress process;
        Font greyFont;//= new gridPresentationBox.Font;
        Font blackFont;// = new gridPresentationBox.Font;



        /// <summary>
        /// Initializes a new instance of the <see cref="ImportPointStressUI"/> class.
        /// </summary>
        public ImportPointStressUI(ImportPointStress process)
        {
            InitializeComponent();

            this.process = process;

            cancelButton.Image = PetrelImages.Cancel;
            ImportStressFromDynelButton.Image = PetrelImages.Apply;
            xValue.DecimalPlaces = 2;
            yValue.DecimalPlaces = 2;
            zValue.DecimalPlaces = 2;
            xValue.Maximum = (decimal)1.0e8;
            yValue.Maximum = (decimal)1.0e8;
            zValue.Maximum = (decimal)1.0e8;
            xValue.Minimum = (decimal)(-1.0e8);
            yValue.Minimum = (decimal)(-1.0e8);
            zValue.Minimum = (decimal)(-1.0e8);
            xValue.Value = (decimal)(-398256.00);
            yValue.Value = (decimal)(-7521520.00);
            zValue.Value = (decimal)(0.00);
            greyFont = new Font(dynelResultsFileTextBoxPpresentationBox.Font.FontFamily, dynelResultsFileTextBoxPpresentationBox.Font.Size, FontStyle.Italic);
            blackFont = new Font(dynelResultsFileTextBoxPpresentationBox.Font.FontFamily, dynelResultsFileTextBoxPpresentationBox.Font.Size, FontStyle.Regular);

            InitializeFileSelectionPanel();
            InitializeGridControl();
            ConnectUIEvents();

            traslateFromStructuralModelCheckbox.Checked = true;

        }

        private void ConnectUIEvents()
        {
            this.cancelButton.Click += (sender, evt) =>
            {
                this.ParentForm.Close();
            };

            this.dropTarget2.DragDrop += (sender, evt) =>
            {
                Grid = evt.Data.GetData(typeof(Grid)) as Grid; ;
            };// new System.Windows.Forms.DragEventHandler(this.dropGrid_DragDrop);


            this.selectDynelFilebutton.Click += (sender, evt) =>
            {
                SelectInputFile();

            };

            this.ImportStressFromDynelButton.Click += (sender, evt) =>
            {
                if (Grid == null)
                {
                    //Restoration.Services.MessageService.ShowMessage("Please select a grid to map the results to", Restoration.Services.MessageType.ERROR);
                    //return;

                    string msg = "Results will not be mapped to a grid.\nA Point set will still be generated";
                    Restoration.Services.MessageService.ShowMessage(msg, Restoration.Services.MessageType.ERROR);
                    
                }


                DynelDatatoProcessEvent dataEvent = new DynelDatatoProcessEvent();
                dataEvent.DataFileName = dynelResultsFileTextBoxPpresentationBox.Text;
                dataEvent.Traslation = Traslation;
                ImportStressesFromDynelToGridClicked(this, dataEvent);

            };

            this.dynelResultsFileTextBoxPpresentationBox.Click += (sender, evt) =>
            {
                SelectInputFile();
            };


        }

        private void SelectInputFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Point Stress Data|*.xyz";
            openFileDialog1.Title = "Select a Restoration Results File";
            openFileDialog1.Multiselect = false;

            // Show the Dialog.          
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                DynelResultsFileSelected?.Invoke(this, new StringEventArgs(openFileDialog1.FileName));
        }

        Grid Grid
        {
            get
            {
                Grid g = (Grid)(gridPresentationBox.Tag);
                return g;
            }
            set
            {
                Grid g = value as Grid;

                if ((g == null) || (g == Grid.NullObject))
                {
                    InitializeGridControl();
                }
                else
                {
                    gridPresentationBox.ForeColor = Color.Black;
                    gridPresentationBox.Font = blackFont;
                    gridPresentationBox.Tag = g;
                    gridPresentationBox.Text = g.Name;
                    gridPresentationBox.Image = PetrelImages.Model;
                }
            }
        }

        string ModelName
        {
            get
            {
                return modelNamePresentationBox.Text;
            }
            set
            {
                modelNamePresentationBox.Text = value;
            }
        }

        public CommonData.Vector3 Traslation
        {
            get { return new CommonData.Vector3((double)xValue.Value, (double)yValue.Value, (zcheckBox.Checked==true? 1.0 : 0.0 )*(double)zValue.Value); }

            set
            {
                xValue.Value = (decimal)value.X;
                yValue.Value = (decimal)value.Y;
                zValue.Value = (decimal)value.Z;

            }
        }






        private void InitializeFileSelectionPanel()
        {
            dynelResultsFileTextBoxPpresentationBox.ForeColor = Color.Gray;
            dynelResultsFileTextBoxPpresentationBox.Font = greyFont;
            dynelResultsFileTextBoxPpresentationBox.Text = "Please select a restoration results file";
            dynelResultsFileTextBoxPpresentationBox.Image = null;
        }

        private void InitializeGridControl()
        {
            gridPresentationBox.ForeColor = Color.Gray;
            gridPresentationBox.Font = greyFont;
            gridPresentationBox.Tag = null;
            gridPresentationBox.Text = "Please select a grid to map the results to";
            gridPresentationBox.Image = null;
        }



        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }







        #region display controller results

        public void AcceptOrRejectSelectedDynelfile(bool accept, string fileName, string error = "")
        {
            if (accept)
            {
                dynelResultsFileTextBoxPpresentationBox.ForeColor = Color.Black;
                dynelResultsFileTextBoxPpresentationBox.Font = blackFont;
                dynelResultsFileTextBoxPpresentationBox.Text = fileName;
                dynelResultsFileTextBoxPpresentationBox.Image = PetrelImages.Folder;

                modelNamePresentationBox.Text = Path.GetFileNameWithoutExtension(fileName);
            }
            else
            {

                MessageBox.Show(error == "" ? error : "Invalid file. Please drop a Dynel results file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InitializeFileSelectionPanel();
            }
        }
        #endregion

        #region map to grid 
        public void DisplayStress(Dictionary<string, List<double>> pointData)
        {
            
            PointSet stressMappedToPoints = MapStressToPointsWithAttributes(pointData);

            PropertyCollection stressMappedToGrid = MapStressToGrid(pointData);

        }

        public PropertyCollection MapStressToGrid(Dictionary<string, List<double>> stressPointData)
        {
            if (Grid == null) return null;

            //get the data 
            float[] sxx = stressPointData["TOTSTRXX"].Select(t => (float)(t)).ToArray();
            float[] syy = stressPointData["TOTSTRYY"].Select(t => (float)(t)).ToArray();
            float[] szz = stressPointData["TOTSTRZZ"].Select(t => (float)(t)).ToArray();
            float[] sxy = stressPointData["TOTSTRXY"].Select(t => (float)(t)).ToArray();
            float[] syz = stressPointData["TOTSTRYZ"].Select(t => (float)(t)).ToArray();
            float[] szx = stressPointData["TOTSTRZX"].Select(t => (float)(t)).ToArray();
            float[] x = stressPointData["x"].Select(t => (float)(t)).ToArray();
            float[] y = stressPointData["y"].Select(t => (float)(t)).ToArray();
            float[] z = stressPointData["z"].Select(t => (float)(t)).ToArray();
            float[][] data = new float[9][] { sxx, syy, szz, sxy, syz, szx, x, y, z };
            Dictionary<string, float[]> names = new Dictionary<string, float[]>()
            {
                { "TOTSTRXX",sxx },
                { "TOTSTRYY",syy},
                { "TOTSTRZZ",szz },
                { "TOTSTRXY",sxy },
                { "TOTSTRYZ",syz },
                { "TOTSTRZX",szx}
            };




            int count = x.Length;

            PropertyCollection col = null;

            using (IProgress progress = PetrelLogger.NewProgress(0, 100))
            {
                progress.SetProgressText("Preparing data to copy...");

                progress.ProgressStatus = 0;

                Dictionary<Index3, float> invCounter = new Dictionary<Index3, float>();
                Dictionary<Index3, float> indexedValues = new Dictionary<Index3, float>();
                Index3[] indices = new Index3[count];

                int pointsInCOunt = 0;
                for (int n = 0; n < count; n++)
                {

                    Index3 i3 = Grid.GetCellAtPoint(new Point3(x[n], y[n], z[n]));
                    indices[n] = null;
                    if (i3 != null)
                    {
                        if (!indexedValues.Keys.Contains(i3)) indexedValues.Add(i3, 0.0f);
                        if (!invCounter.Keys.Contains(i3)) invCounter.Add(i3, 0.0f);
                        invCounter[i3] = invCounter[i3] + 1.0f;
                        indices[n] = i3;
                        pointsInCOunt++;
                    }

                }


                //bool createPointsAndLeave = false;
                if (pointsInCOunt < (int)(0.5 * count))
                {
          
                    if (pointsInCOunt < 1)
                    {
                        MessageBox.Show("The xyz points in the file do not lie in the grid.\nMapping aborted\nA point set matching the locations in the file was created as a guide.", "Error during Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        progress.ProgressStatus = 0;
                        return null;
                    }
                    else
                    {
                        var answer = MessageBox.Show("Less than 50% of the data in the file lies inside the grid.\nDo you want to continue", "Unexpected Import Condition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (answer != DialogResult.Yes)
                        {
                            MessageBox.Show("Process aborted.\nThe point set:\n\n" + ModelName  + " Locations Data" + "\n\nmatching the locations in the file was created as a guide.", "Unexpected Import Condition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            progress.ProgressStatus = 0;
                            return null;
                        }
                    }

                }

                //Collection colx = ProjectTools.GetOrCreateCollectionByName(ModelName + " Locations Data");
                //List<Point3> lst = new List<Point3>();
                //for (int n = 0; n < x.Count(); n++)
                //    lst.Add(new Point3(x[n], y[n], z[n]));
                //ProjectTools.CreatePointSet(ref colx, lst);
                
                

               progress.SetProgressText("Mapping properties");


                col = ProjectTools.GetOrCreatePropertyCollection(Grid, ModelName);

                int counter = 0;
                foreach (string name in names.Keys)
                {
                    float[] arrayData = names[name];
                    float[] avgValues = Enumerable.Repeat(0.0f, count).ToArray();

                    for (int k = 0; k < indexedValues.Keys.Count; k++) indexedValues[indexedValues.Keys.ElementAt(k)] = 0.0f;


                    try
                    {
                        for (int k = 0; k < count; k++)
                        {
                            if (indices[k] != null)
                                indexedValues[indices[k]] += (arrayData[k]/ Math.Max(1, invCounter[indices[k]]));
                        }
                    }

                    catch (Exception w)
                    {
                        var msg = w.Message;

                        ;
                        ;

                    }



                    Property p = ProjectTools.GetOrCreateProperty(col, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
                    ProjectTools.SetValue(ref p, float.NaN);
                    ProjectTools.SetValues(ref p, indexedValues.Keys.ToList(), indexedValues.Values.ToList());

                    counter += 1;
                    progress.ProgressStatus = (int)(100.0 * counter / names.Keys.Count());//100 *(int)( ((double)yyy / (double)dfn.FracturePatchCount));

                }
            }//progress bar

            return col;

        } //map stress to grid 






        public PointSet MapStressToPointsWithAttributes(Dictionary<string, List<double>> stressPointData)
        {
            //get the data 
            double[] sxx = stressPointData["TOTSTRXX"].ToArray();//;//.Select(t => (float)(t)).ToArray();
            double[] syy = stressPointData["TOTSTRYY"].ToArray();//;//.Select(t => (float)(t)).ToArray();
            double[] szz = stressPointData["TOTSTRZZ"].ToArray();//;//.Select(t => (float)(t)).ToArray();
            double[] sxy = stressPointData["TOTSTRXY"].ToArray();//;//.Select(t => (float)(t)).ToArray();
            double[] syz = stressPointData["TOTSTRYZ"].ToArray();//;//.Select(t => (float)(t)).ToArray();
            double[] szx = stressPointData["TOTSTRZX"].ToArray();//.Select(t => (float)(t)).ToArray();
            double[] x = stressPointData["x"].ToArray();//Select(t => (float)(t)).ToArray();
            double[] y = stressPointData["y"].ToArray();//Select(t => (float)(t)).ToArray();
            double[] z = stressPointData["z"].ToArray();//Select(t => (float)(t)).ToArray();
            double[][] data = new double[9][] { sxx, syy, szz, sxy, syz, szx, x, y, z };
            Dictionary<string, double[]> names = new Dictionary<string, double[]>()
            {
                { "TOTSTRXX",sxx },
                { "TOTSTRYY",syy},
                { "TOTSTRZZ",szz },
                { "TOTSTRXY",sxy },
                { "TOTSTRYZ",syz },
                { "TOTSTRZX",szx}
            };

            int count = x.Length;

            List<Point3> coords = new List<Point3>();
            for (int n = 0; n < count; n++) coords.Add(new Point3(x[n], y[n], z[n]));

            Collection col = ProjectTools.GetOrCreateCollectionByName(ModelName);
            PointSet pts = ProjectTools.GetOrCreatePointSet(ref col, coords, ModelName);


            using (IProgress progress = PetrelLogger.NewProgress(0, 100))
            {
                progress.SetProgressText("Creating point data in the input tab.");
                progress.ProgressStatus = 0;


                int counter = 0;
                foreach (string name in names.Keys)
                {
                    Template t = name.Contains("TOTSTR") ? PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal : PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
                    PointProperty p = ProjectTools.GetOrCreatePointProperty(pts, name+"PTS", t);
                    ProjectTools.SetPropertyValues(p, names[name]);

                    counter += 1;
                    progress.ProgressStatus = (int)(100.0 * counter / names.Keys.Count());//100 *(int)( ((double)yyy / (double)dfn.FracturePatchCount));

                }
            }

            return pts;
        }






        #endregion

        #region drag-drops 



        #endregion

        private void coordinateTraslationCheckBoxChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)(sender);
            if (r != null)
            {
                structuralModelDrop.Enabled = false;
                structuralModelPresentationbox.Enabled = false;
                exportedFilesForTraslationButton.Enabled = false;
                exportedfilesPresentationBox.Enabled = false;
                xValue.Enabled = false;
                yValue.Enabled = false;
                zValue.Enabled = false;
            }
            else { return; }

            if (r == traslateFromStructuralModelCheckbox)
            {
                structuralModelDrop.Enabled = true;
                structuralModelPresentationbox.Enabled = true;

                xValue.Enabled = false;
                yValue.Enabled = false;
                zValue.Enabled = false;
            }
            else if (r == traslateFromFilesCheckbox)
            {
                exportedFilesForTraslationButton.Enabled = true;
                exportedfilesPresentationBox.Enabled = true;
            }
            else if (r == traslateManualcheckbox)
            {
                xValue.Enabled = true;
                yValue.Enabled = true;
                zValue.Enabled = true;
            }
        }

        private void structuralModelDrop_DragDrop(object sender, DragEventArgs evt)
        {

            object dropped = evt.Data.GetData(typeof(object));
            if (dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel != null)
            {
                Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel model = (Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel)(dropped);
                Traslation = -1.0 * GetReferecePointFromStructuralModel(model);

                structuralModelPresentationbox.Text = model.Name;
                structuralModelPresentationbox.Image = PetrelImages.Model;
                structuralModelPresentationbox.Tag = model;
          
            }

        }

        private CommonData.Vector3 GetReferecePointFromStructuralModel(Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel model)
        {
            if ((model == null) || (model.HorizonCollection.Horizons.Count < 1))
            {
                return null;
            }
            var pts = model.HorizonCollection.Horizons.ElementAt(0).IndexedTriangleMesh.Vertices;

            var avgX = pts.Average(t => t.X);
            var avgY = pts.Average(t => t.Y);
            var avgZ = pts.Average(t => t.Z);


            return new CommonData.Vector3(avgX, avgY,  avgZ);
        }


        private void exportedFilesForTraslationButton_Click(object sender, EventArgs e)
        {

        }
    };


}
