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
    partial class ImportPointStressProcessUI : UserControl
    {

        public event EventHandler<StringEventArgs> DynelResultsFileSelected;
        public event EventHandler<DynelDatatoProcessEvent> ImportStressesFromDynelToGridClicked;


        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private ImportPointStressProcess process;
        Font greyFont;//= new gridPresentationBox.Font;
        Font blackFont;// = new gridPresentationBox.Font;

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


        /// <summary>
        /// Initializes a new instance of the <see cref="ImportPointStressUI"/> class.
        /// </summary>
        public ImportPointStressProcessUI(ImportPointStressProcess process)
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
            ConnectCalculationEvents();


        }

        private void ConnectUIEvents()
        {
            this.dropTarget2.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropGrid_DragDrop);
        }

        private void InitializeFileSelectionPanel()
        {
            dynelResultsFileTextBoxPpresentationBox.ForeColor = Color.Gray;
            dynelResultsFileTextBoxPpresentationBox.Font = greyFont;
            dynelResultsFileTextBoxPpresentationBox.Text = "Please select a dynel results file";
            dynelResultsFileTextBoxPpresentationBox.Image = null;
        }

        private void InitializeGridControl()
        {
            gridPresentationBox.ForeColor = Color.Gray;
            gridPresentationBox.Font = greyFont;
            gridPresentationBox.Tag = null;
            gridPresentationBox.Text = "Please select a grid";
            gridPresentationBox.Image = null;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        public CommonData.Vector3 Traslation
        {
            get { return new CommonData.Vector3((double)xValue.Value, (double)yValue.Value, (double)zValue.Value); }
        }


        #region KeyEvents Triggering Calculations 
        private void ConnectCalculationEvents()
        {

            this.selectDynelFilebutton.Click += (sender, evt) =>
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Dynel|*.xyz";
                openFileDialog1.Title = "Select a Dynel Results File";
                openFileDialog1.Multiselect = false;

                // Show the Dialog.          
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    DynelResultsFileSelected?.Invoke(this, new StringEventArgs(openFileDialog1.FileName));

            };

            this.ImportStressFromDynelButton.Click += (sender, evt) =>
            {
                DynelDatatoProcessEvent dataEvent = new DynelDatatoProcessEvent();
                dataEvent.DataFileName = dynelResultsFileTextBoxPpresentationBox.Text;
                dataEvent.Traslation = Traslation;
                ImportStressesFromDynelToGridClicked(this, dataEvent);
            };
        }

        #endregion





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
        public void DisplayStress(Dictionary<string, List<double>> stressPointData)
        {
            PropertyCollection stressMappedToGrid = MapStressToGrid(stressPointData);
            PointSet stressMappedToPoints = MapStressToPointsWithAttributes(stressPointData);
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
                for (int n = 0; n < count; n++)
                {

                    Index3 i3 = Grid.GetCellAtPoint(new Point3(x[n], y[n], z[n]));
                    if (!indexedValues.Keys.Contains(i3)) indexedValues.Add(i3, 0.0f);
                    if (!invCounter.Keys.Contains(i3)) invCounter.Add(i3, 0.0f);
                    invCounter[i3] = invCounter[i3] + 1.0f;
                    indices[n] = i3;
                }

                progress.SetProgressText("Mapping properties");


                col = ProjectTools.GetOrCreatePropertyCollection(Grid, ModelName);

                int counter = 0;
                foreach (string name in names.Keys)
                {
                    float[] arrayData = names[name];
                    float[] avgValues = Enumerable.Repeat(0.0f, count).ToArray();

                    for (int k = 0; k < indexedValues.Keys.Count; k++) indexedValues[indexedValues.Keys.ElementAt(k)] = 0.0f;

                    for (int k = 0; k < count; k++)
                        indexedValues[indices[k]] += (arrayData[k] / invCounter[indices[k]]);

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
                    PointProperty p = ProjectTools.GetOrCreatePointProperty(pts, name, PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
                    var values = names[name];
                    ProjectTools.SetPropertyValues(p, values);


                    counter += 1;
                    progress.ProgressStatus = (int)(100.0 * counter / names.Keys.Count());//100 *(int)( ((double)yyy / (double)dfn.FracturePatchCount));

                }
            }

            return pts;
        }




        #endregion

        #region drag-drops 

        private void dropGrid_DragDrop(object sender, DragEventArgs e)
        {
            Grid = e.Data.GetData(typeof(Grid)) as Grid; ;
        }



        #endregion

   
    };

}
