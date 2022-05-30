using CommonData;
using Restoration.GFM;
using Restoration.GFM.OceanUtils;
using Restoration.GFM.View;
using Slb.Ocean.Core;

//using Slb.Ocean.Petrel.UI;

using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class GFMGeometryUI : UserControl
    {
        private LithoTableControl lithoTableControl1;
        private LithoTableControl lithoTableControl2 = null;

        public event EventHandler CancelClicked;

        public event EventHandler<StringArgs> LoadModelClicked;

        public event EventHandler GenerateProject;

        public event EventHandler<SimulationEventArgs> GenerateSimulationModels;

        public event EventHandler Request1DSimulationList;

        public event EventHandler<Args<List<string>>> Request1DSimulationResults;

        #region Construction

        public GFMGeometryUI( GFMGeometry process )//, IGFMSerializer serial)
        {
            InitializeComponent();

            DateTime dateTime = new DateTime(2020, 12, 27);
            if (DateTime.Now > dateTime)
            {
                expiredLabel.Visible = true;
                tabControl2.Visible = false;
            }
            else
            {
                InitializeComponentExtras();

                ConnectEvents();

                UIFromProject(process.Model);
            }
        }

        private void InitializeComponentExtras()
        {
            launchAfterDeckControl.Visible = false;
            openProjectButton.Image = PetrelImages.Folder;
            openProjectButton.Text = string.Empty;
            openProjectButton.ImageAlign = ContentAlignment.MiddleCenter;
            radio1DSim.Checked = false;
            radio1DSim.Checked = true;

            goCadFolderButton.Image = PetrelImages.Folder;
            goCadFolderButton.Text = string.Empty;
            GeneratePropertyFilesButton.Image = PetrelImages.Apply;
            CancelButton.Image = PetrelImages.Cancel;

            coresControl.Value = Environment.ProcessorCount;
            coresControl.Maximum = Environment.ProcessorCount;
            coresControl.Minimum = 1;

            loadCustomLithoFileButton.Image = PetrelImages.Folder;
            loadCustomLithoFileButton.ImageAlign = ContentAlignment.MiddleRight;

            lithoTableControl1 = new LithoTableControl();
            lithoPanel.Controls.Add(lithoTableControl1);
            lithoTableControl1.Dock = DockStyle.Fill;
            lithoTableControl2 = new LithoTableControl();
            lithoTablePanel1DCharting.Controls.Add(lithoTableControl2);
            lithoTableControl2.Dock = DockStyle.Fill;

            lithoTableControl2.DisplayCompact = true;
            RestorationFolder = string.Empty;
            TriangulatedSurfaceFilesToCopy = new List<string>();
            simd1DResultsChart.Series.Clear();
            simd1DResultsChart.ChartAreas[0].AxisX.Interval = double.NaN;
            exportChart.Visible = true;

            splitContainer1.Panel1.Controls.Add(lithoTablePanel1DCharting);
            lithoTablePanel1DCharting.Dock = DockStyle.Fill;

            splitContainer1.Panel2.Controls.Add(simd1DResultsChart);
            simd1DResultsChart.Dock = DockStyle.Fill;

            CancelButton.Image = PetrelImages.Cancel;
        }

        private void ConnectEvents()
        {
            this.CancelButton.Click += ( sender, evt ) =>
            {
                CancelClicked?.Invoke(this, EventArgs.Empty);
                this.ParentForm.Close();
            };

            this.openProjectButton.Click += ( seder, evt ) =>
            {
                ModelName = string.Empty;
                string selectedPath = string.Empty;

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = System.IO.Path.Combine(ProjectTools.GetOceanFolder(), "GFM");// Environment.SpecialFolder.ApplicationData;
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        selectedPath = fbd.SelectedPath;
                    }
                }

                //the controller will load the model and then will ask the UI to display it.
                if (selectedPath != string.Empty)
                    LoadModelClicked?.Invoke(this, new StringArgs(selectedPath));
            };

            this.GeneratePropertyFilesButton.Click += ( sender, evt ) =>
            {// new System.EventHandler(this.Generate_Click);
                if (!isItOkToGenerate)
                {
                    MessageBox.Show("Cannot generate the " + (tabControl2.SelectedTab == GenerateTab ? "project." : "simulation files.") + "\nData is incomplete", "Incomplete data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (tabControl2.SelectedTab == GenerateTab)
                {
                    GenerateProject?.Invoke(this, EventArgs.Empty);
                }
                else if (tabControl2.SelectedTab == SimulationsTab)
                {
                    DialogResult result = MessageBox.Show("Launch simulations after generating the needed files ?\n\n    No: Only generate simulation files\n\n    Yes: Launch the simulations automatically\n", "Launch Simulation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    bool launchAfterDeck = result == DialogResult.Yes ? true : false;
                    bool inputIsWrong = false;
                    if (simPanel1D.Visible == true)
                    {
                        if (presentationBox1DSimulation.Tag != null)
                        {
                            string simBaseName = simBaseNameTextBox.Text;
                            Vector3[] xyz = ((PointSet)(presentationBox1DSimulation.Tag)).Points.Select(t => new Vector3(t.X, t.Y, 0.0)).ToArray();
                            SimulationEventArgs e = new SimulationEventArgs(simBaseName, 1, 100, xyz, launchAfterDeck/*launchAfterDeckControl.Checked*/, null);
                            GenerateSimulationModels?.Invoke(this, e);
                        }
                        else inputIsWrong = true;
                    }
                    else if (simPanel3D.Visible == true)
                    {
                        if (presentationBox3DSimulation.Tag != null)
                        {
                            string simBaseName = simBaseNameTextBox.Text;
                            Vector3[] xyz = (Vector3[])presentationBox3DSimulation.Tag;// ((PointSet)(presentationBox3DSimulation.Tag)).Points.Select(t => new Vector3(t.X, t.Y, 0.0)).ToArray();
                            SimulationEventArgs e = new SimulationEventArgs(simBaseName, 3, (int)(xResolutionControl.Value), xyz, launchAfterDeck/*launchAfterDeckControl.Checked*/, null);

                            e.Cores = (int)(coresControl.Value);
                            GenerateSimulationModels?.Invoke(this, e);
                        }
                        else inputIsWrong = true;
                    }
                    else {; }

                    if (inputIsWrong)
                    {
                        MessageBox.Show("Cannot process the input points (1D) or boundary (3D) for the simulation.", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {; }
            };

            this.referencialGridDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.referencialGridDrop_DragDrop);

            this.clearChartbutton.Click += ( sender, evt ) => { simd1DResultsChart.Series.Clear(); };

            this.dropPoints1DSimulation.DragDrop += ( sender, evt ) =>
            {
                if (simBaseNameTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Missing data. Please select a name for the 1D simulation(s) first", "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PointSet points = evt.Data.GetData(typeof(PointSet)) as PointSet;
                if (points != null)
                {
                    dropPoints1DSimulation.Tag = points;

                    Font blackFont = new Font(presentationBox1DSimulation.Font.FontFamily, presentationBox1DSimulation.Font.Size, FontStyle.Regular);
                    presentationBox1DSimulation.ForeColor = Color.Black;
                    presentationBox1DSimulation.Text = points.Name;

                    presentationBox1DSimulation.Image = PetrelImages.PointSet;
                    presentationBox1DSimulation.Tag = points;
                }
                else
                {
                    presentationBox1DSimulation.Tag = null;
                    dropPoints1DSimulation.Tag = null;
                    presentationBox1DSimulation.Image = null;

                    Font greyFont = new Font(presentationBox1DSimulation.Font.FontFamily, presentationBox1DSimulation.Font.Size, FontStyle.Italic);
                    presentationBox1DSimulation.ForeColor = Color.Gray;
                    presentationBox1DSimulation.Text = "Please drop a point set";
                }
            };

            this.dropPoints3DSimulation.DragDrop += ( sender, evt ) =>
            {
                if (simBaseNameTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Missing data. Please select a name for the 1D simulation(s) first", "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                Slb.Ocean.Petrel.DomainObject.Shapes.PolylineSet points = evt.Data.GetData(typeof(PolylineSet)) as PolylineSet;
                if (points != null)
                {
                    presentationBox3DSimulation.Tag = points.Points.Select(t => new Vector3(t.X, t.Y, t.Z)).ToArray();// sortPointsFor3DBoundary(points.Points.Select(t => new Vector3(t.X, t.Y, t.Z)).ToArray());
                    Font blackFont = new Font(presentationBox3DSimulation.Font.FontFamily, presentationBox3DSimulation.Font.Size, FontStyle.Regular);
                    presentationBox3DSimulation.ForeColor = Color.Black;
                    presentationBox3DSimulation.Text = points.Name;
                    presentationBox3DSimulation.Image = PetrelImages.Boundary;
                }
                else
                {
                    presentationBox3DSimulation.Tag = null;
                    presentationBox3DSimulation.Tag = null;
                    presentationBox3DSimulation.Image = null;

                    Font greyFont = new Font(presentationBox3DSimulation.Font.FontFamily, presentationBox3DSimulation.Font.Size, FontStyle.Italic);
                    presentationBox3DSimulation.ForeColor = Color.Gray;
                    presentationBox3DSimulation.Text = "Please drop a polygon";
                }
            };

            this.tabControl2.SelectedIndexChanged += ( sender, evt ) =>
            {
                if (tabControl2.SelectedTab == GenerateTab)
                {
                    GeneratePropertyFilesButton.Text = "Update Project";
                    GeneratePropertyFilesButton.Enabled = true;
                    clearButton.Enabled = true;
                }
                if (tabControl2.SelectedTab == SimulationsTab)
                {
                    GeneratePropertyFilesButton.Text = "Export";
                    GeneratePropertyFilesButton.Enabled = true;
                    clearButton.Enabled = false;
                }
                if (tabControl2.SelectedTab == Simulation1DResultsTab)
                {
                    GeneratePropertyFilesButton.Enabled = false;//.Text = "Update project";
                    clearButton.Enabled = false;
                }
            };

            this.lithoTableControl2.TableChanged += ( sender, evt ) => { saveEditedTableButton.Visible = true; };

            this.saveEditedTableButton.Click += ( sender, evt ) =>
            {
                if (MessageBox.Show("Do you want to update the project properties? ", "Update properties", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LithoData = lithoTableControl2.LithoData;
                    MessageBox.Show("Project properties updated.\nPlease remember to regenerate the project before running new simulations", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    saveEditedTableButton.Visible = false;
                }
            };
        }

        #endregion Construction

        #region properties

        public void ClearProject()
        {
            ModelName = string.Empty;
            LithoData = new LithoData();
            BaseName = string.Empty;
            TriangulatedSurfaceFilesToCopy.Clear();
            ReferenceGridDroid = string.Empty;
            RestorationFolder = string.Empty;
            simd1DResultsChart.Series.Clear();
        }

        public string ModelName
        {
            get => modelNameBox.Text;

            set
            {
                modelNameBox.Text = value;
                lithoTableControl1.ModelName = value;
            }
        }

        public LithoData LithoData
        {
            get => lithoTableControl1.LithoData;
            set => lithoTableControl1.LithoData = value;
        }

        public string BaseName
        {
            set => lithoTableControl1.BaseName = value;
            get => lithoTableControl1.BaseName;
        }

        public string ReferenceGridDroid
        {
            get => _referenceGridDroid;//.Text;

            set
            {
                _referenceGridDroid = string.Empty;
                Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid g = null;

                Droid d = Droid.TryParse(value);
                if (d != null)
                {
                    g = (Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid)DataManager.Resolve(d);
                    if (g != null)
                    {
                        _referenceGridDroid = value;
                        referenceGridPresentationBox.Text = g.Name;
                    }
                }
                referenceGridPresentationBox.Text = _referenceGridDroid != string.Empty ? g.Name : string.Empty;
                referenceGridPresentationBox.Image = _referenceGridDroid != string.Empty ? Slb.Ocean.Petrel.UI.PetrelImages.PillarGrid : null;
            }
        }

        private string _referenceGridDroid;

        public string RestorationFolder
        {
            get => gocadFolderBox.Text;
            set
            {
                gocadFolderBox.Text = value;
                gocadFolderBox.Image = gocadFolderBox.Text != string.Empty ? PetrelImages.Apply : null;
            }
        }

        public List<string> TriangulatedSurfaceFilesToCopy { get; set; } = new List<string>();

        #endregion properties

        #region user interaction

        private void sims1DColumnSelector_SelectedIndexChanged( object sender, EventArgs e )
        {
            ////////////////
            Dictionary<string, List<string>> data = (Dictionary<string, List<string>>)(sims1DCaseSelector.Tag);
            if (data == null)
            {
                if (lithoTableControl2 != null) lithoTableControl2.ClearTable();
                return;
            }

            string nameOfSimulationGroup = data.Keys.ElementAt(sims1DCaseSelector.SelectedIndex);
            var SimFoldersThere = Directory.GetDirectories(nameOfSimulationGroup, "*Col*");
            string nameOfColumn = SimFoldersThere.ElementAt(sims1DColumnSelector.SelectedIndex);

            //get the litho file if any. We need the split and the name to return the results.

            string[] lithoFile = Directory.GetFiles(nameOfColumn, "*.litho");
            if (lithoFile.Count() > 0)
            {
                //LithoData l = LithoData.FromVisageLithoFile(lithoFile[0]);
                lithoTableControl2.LithoData = LithoData.FromVisageLithoFile(lithoFile.ElementAt(0));
                lithoTableControl2.DisplayCompact = true;

                string[] horizonsFile = Directory.GetFiles(nameOfColumn, "*.horizons");
                if (horizonsFile.Count() > 0)
                {
                    string[] lines = File.ReadAllLines(horizonsFile.ElementAt(0));
                    BaseName = lines.ElementAt(lines.Count() - 1);
                }
            }
            else
            {
                MessageBox.Show("This simulation dit not run properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lithoTableControl2.ClearTable();
                return;
            }

            ////////////////////
        }

        private void RestorationFolderSelected( object sender, EventArgs e )
        {
            if ((LithoData == null) || (LithoData.Layers == null) || (LithoData.Layers.Count() < 1))
            {
                MessageBox.Show("Please select a model name first and then\ndrop a pillar grid for the present day.\nThen the triangulated surfaces can be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            TriangulatedSurfaceFilesToCopy = new List<string>();
            string path = string.Empty;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                path = folderDlg.SelectedPath;
            }
            else
            {
                RestorationFolder = string.Empty;
                return;
            }

            string error;
            ProcessRestorationDataFolder(path, out error);

            if (error != string.Empty)
            {
                MessageBox.Show(error, "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RestorationFolder = string.Empty;
            }
        }

        private void referencialGridDrop_DragDrop( object sender, DragEventArgs evt )
        {
            object dropped = evt.Data.GetData(typeof(object));
            ReferenceGridDroid = string.Empty;

            if (ModelName == string.Empty)
            {
                if (modelNameBox.Text != string.Empty)
                {
                    ModelName = modelNameBox.Text;
                }
                else
                {
                    MessageBox.Show("Please select a name for the model first", "Model name is missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (dropped == null)
            {
                MessageBox.Show("Please drop a valid pillar grid", "Wrong data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dropped as Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid != null)
            {
                Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid g = (Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid)(dropped);
                ReferenceGridDroid = g.Droid.ToString();
                processGridDrop(g, null);
            }
            else
            {
                ReferenceGridDroid = string.Empty;
                processGridDrop(null, null);
                MessageBox.Show("Please select a pillar grid", "Object is not a Valid pillar grid.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void processGridDrop( Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid g, Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection pc )
        {
            if (g == null)
            {
                return;
            }
            LithoData newLithoData = new LithoData();
            BaseName = string.Empty;

            List<string> names = g.Horizons.Select(t => FixHorizonName(t.Name)).ToList();
            names.RemoveAt(names.Count() - 1);

            //layers resolution from the grid
            List<int> layersResolution = new List<int>();
            var h = g.Horizons.ToArray();

            for (int n = 0; n < h.Count() - 1; n++)
                layersResolution.Add(h[n + 1].K - h[n].K);

            BaseName = FixHorizonName(h[h.Count() - 1].Name);

            int count = 0;
            List<LayerLithoData> data = new List<LayerLithoData>();
            foreach (string name in names)
            {
                LayerLithoData l = new LayerLithoData();
                l.Split = layersResolution[count++];
                l.Layer = name;
                data.Add(l);// LayerLithoData.Default);
            }

            newLithoData.Layers = data;

            foreach (var l in newLithoData.Layers)
            {
                List<KeyValuePair<double, double>> dvt = l.GetDVTTable();
                string droid = GFMOceanUtils.PublishFunction(ModelName, l.Layer + "_YM_dvt", dvt);
                l.YoungsModulusHardeningFunctionDroid = droid;
            }
            LithoData = newLithoData;
            MessageBox.Show("Stiffness vs strain tables created in the input tab inside the folder:\n\n" + ModelName, "Data created/changed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ReferenceGridDroid = g.Droid.ToString();

            RestorationFolder = string.Empty;
            TriangulatedSurfaceFilesToCopy = new List<string>();
        }

        private void SImDimensionRadio( object sender, EventArgs e )
        {
            RadioButton b = (RadioButton)(sender);
            if (b == radio1DSim) simPanel1D.Show(); else simPanel1D.Hide();
            if (b == radio3DSim) simPanel3D.Show(); else simPanel3D.Hide();
        }

        private void exportChart_Click( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Please select a .csv file";
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            string fileName = string.Empty;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
            }
            else
                return;

            fileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName)) + ".csv";

            fakeDataGridView.Columns.Clear();
            fakeDataGridView.Rows.Clear();

            //for each series add two columns
            //then export the table as a csv.

            int maxRows = 0;
            Array.ForEach(simd1DResultsChart.Series.ToArray(), delegate ( Series t )
            {
                //x values
                DataGridViewColumn c1 = new DataGridViewTextBoxColumn();

                c1.HeaderText = "Layer";// t.Name;
                c1.Name = "Column" + t.Name + "X";
                fakeDataGridView.Columns.Add(c1);
                maxRows = Math.Max(maxRows, t.Points.Count());

                //y values
                DataGridViewColumn c2 = new DataGridViewTextBoxColumn();
                c2.HeaderText = "Thickness"; //t.Name;
                c2.Name = "Column" + t.Name + "Y";
                fakeDataGridView.Columns.Add(c2);
                ;
            });

            for (int n = 0; n < maxRows; n++)
            {
                DataGridViewRow row = (DataGridViewRow)fakeDataGridView.Rows[0].Clone();
                for (int k = 0; k < fakeDataGridView.ColumnCount; k++)
                {
                    row.Cells[k].Value = string.Empty;
                }
                fakeDataGridView.Rows.Add(row);
            }

            //return;

            int columnCount = 0;
            Array.ForEach(simd1DResultsChart.Series.ToArray(), delegate ( Series s )
            {
                //var ytu = s.IsValueShownAsLabel;
                //var ddd = s.XValueType;

                //var ww = s.AxisLabel;
                //var ttt = s.XValueMember;
                //var yyu = simd1DResultsChart.ChartAreas[0].AxisX.MinorTickMark;// I.ValueToPosition(1);

                var pts = s.Points;

                var yth = pts.ElementAt(0).AxisLabel;

                var xValues = pts.Select(t => t.AxisLabel);//.ToString() );
                var yValues = pts.Select(t => (t.YValues.Count() > 0 ? t.YValues.ElementAt(0).ToString() : string.Empty));

                for (int k = 0; k < xValues.Count(); k++)
                {
                    fakeDataGridView.Rows[k].Cells[columnCount].Value = xValues.ElementAt(k);
                }
                for (int k = 0; k < yValues.Count(); k++)
                {
                    fakeDataGridView.Rows[k].Cells[columnCount + 1].Value = yValues.ElementAt(k);
                }
                columnCount = columnCount + 2;
                return;
            });

            try
            {
                //alright, now we can export it as csv;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    string csvColumns = string.Join(",", simd1DResultsChart.Series.Select(t => t.Name + " Layer" + "," + t.Name + " Thickness"));
                    file.WriteLine(csvColumns);

                    string line = "";
                    for (int nc = 0; nc < simd1DResultsChart.Series.Count; nc++)
                    {
                        line += (simd1DResultsChart.ChartAreas[0].AxisX.Title + ", " + simd1DResultsChart.ChartAreas[0].AxisY.Title + (nc != simd1DResultsChart.Series.Count() - 1 ? "," : ""));
                    }
                    ;
                    ;
                    ;

                    file.WriteLine(line);

                    for (int nr = 0; nr < fakeDataGridView.Rows.Count; nr++)
                    {
                        var r = fakeDataGridView.Rows[nr];
                        line = "";

                        for (int c = 0; c < fakeDataGridView.ColumnCount; c++)
                            line += (r.Cells[c].Value + (c == fakeDataGridView.ColumnCount - 1 ? "" : ","));
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception ee)
            {
                string why = ee.ToString();
                MessageBox.Show("Cannot open the selected file.\nCheck the file is not open and that you have write permissions", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult res = MessageBox.Show("File created.\nDo you want to open it?", "Results Exportedr", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(fileName);
                }
                catch
                {
                    MessageBox.Show("Cannot open the selected file.\nEither it was not created or the selected extension is not .csv", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //var excelApp = new Excel.Application();
                //excelApp.Visible = true;
                //excelApp.Workbooks.Open( fileName );

                //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //startInfo.UseShellExecute = false;
                ////startInfo.WorkingDirectory = targetDir.FullName;
                //startInfo.FileName = "excel.exe";
                //startInfo.Arguments = fileName;
                //System.Diagnostics.Process p = new System.Diagnostics.Process();
                //p.StartInfo = startInfo;

                //p.Start( );
            }

            //Series[] seriesArray = simd1DResultsChart.Series.ToArray();
            //fakeDataGridView.Columns.Add(new DataGridViewColumn());

            //foreach (Series s in simd1DResultsChart.Series)
            //{
            //    var pts = s.Points;
            //    if ((pts != null) && (pts.Count() > 0))
            //    {
            //        var xValues = pts.Select(t => t.XValue);
            //        var yValues = pts.Select(t => t.YValues.ElementAt(0));

            //        fakeDataGridView.Columns.Add(new DataGridViewColumn() );

            //    }
            //        //    maxXVlufes = Math.Max(maxXVlues, x);
            ////    totalPoints += pts.Count();
            //}

            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //saveFileDialog1.Filter = "csv files (*.csv)|*.txt|All files (*.*)|*.*";
            //saveFileDialog1.FilterIndex = 2;
            //saveFileDialog1.RestoreDirectory = true;

            //if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            //string fileName = saveFileDialog1.FileName;

            //int totalPoints = 0;
            //int maxXVlues = -1;
            //foreach (Series s in simd1DResultsChart.Series) {
            //    var pts = s.Points;
            //    var x = pts.Select(t => t).Count() ;
            //    maxXVlues = Math.Max(maxXVlues, x);
            //    totalPoints += pts.Count();
            //}

            //string[][] data = new string[2 * simd1DResultsChart.Series.Count()][];
            //int counter = 0;
            //foreach (Series s in simd1DResultsChart.Series)
            //{
            //    var pts = s.Points;
            //    data[counter++] = pts.Select(t => t.XValue.ToString()).ToArray();
            //    data[counter++] = pts.Select(t => t.YValues.ElementAt(0).ToString()).ToArray();
            //}

            //int pointNow = 0;
            //bool keepWritting = true;
            //while (keepWritting)
            //{
            //}

            //    columns.Add(x.ToList());
            //    columns.Add(yval.ToList());
            //    maxXVlues = Math.Max(maxXVlues, x.Count());
            //}

            //for (int n = 0; n < columns.Count(); n++)
            //{
            //}
        }

        private void reloadButton_Click( object sender, EventArgs e )
        {
            Request1DSimulationList?.Invoke(this, EventArgs.Empty);
        }

        private void addToChatButton_Click( object sender, EventArgs e )
        {
            Dictionary<string, List<string>> data = (Dictionary<string, List<string>>)(sims1DCaseSelector.Tag);
            if (data == null) return;
            string nameOfSimulation = data.Keys.ElementAt(sims1DCaseSelector.SelectedIndex);
            List<string> cols = data[nameOfSimulation];

            List<string> resultFolders = new List<string>();
            foreach (var item in sims1DColumnSelector.CheckedItems)
            {
                resultFolders.Add(Path.Combine(nameOfSimulation, item.ToString()));
            }

            Args<List<string>> evtArgs = new Args<List<string>>(resultFolders);
            Request1DSimulationResults?.Invoke(this, evtArgs);
        }

        private void sims1DCaseSelector_SelectedIndexChanged( object sender, EventArgs e )
        {
            sims1DColumnSelector.Items.Clear();
            Dictionary<string, List<string>> data = (Dictionary<string, List<string>>)(sims1DCaseSelector.Tag);
            if (data == null) return;

            string nameOfSimulation = data.Keys.ElementAt(sims1DCaseSelector.SelectedIndex);
            List<string> cols = data[nameOfSimulation];

            foreach (string s in cols)
                sims1DColumnSelector.Items.Add(Path.GetFileName(s));

            lithoTableControl2.BaseName = BaseName;

            if ((sims1DColumnSelector.SelectedIndex < 0) && (sims1DColumnSelector.Items.Count > 0))
                sims1DColumnSelector.SelectedIndex = 0;
            else
            {
                if (lithoTableControl2 != null) lithoTableControl2.ClearTable();
                return;
            }
        }

        private void loadCustomLithoFileButton_Click( object sender, EventArgs e )
        {
            if (ModelName == string.Empty)
            {
                MessageBox.Show("Please select a project name first", "Data missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (ReferenceGridDroid == string.Empty)
            {
                MessageBox.Show("Please drop a reference grid first", "Data missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LithoData l = LithoData.FromCustomFile(openFileDialog1.FileName);
                if (l == null)
                {
                    MessageBox.Show("Error parasing the selected file.\n", "Data error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (var li in l.Layers)
                    {
                        li.Layer = FixHorizonName(li.Layer);
                        List<KeyValuePair<double, double>> dvt = li.GetDVTTable();
                        string droid = GFMOceanUtils.PublishFunction(ModelName, li.Layer + "_YM_dvt", dvt);
                        li.YoungsModulusHardeningFunctionDroid = droid;
                    }
                    MessageBox.Show("Stiffness vs strain tables created in the input tab inside the folder:\n\n" + ModelName, "Data created/changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LithoData = l;
                    RestorationFolder = string.Empty;
                    TriangulatedSurfaceFilesToCopy = new List<string>();
                }
            }
        }

        #endregion user interaction

        #region Calculation and Validation

        private bool isItOkToGenerate
        {
            get
            {
                bool isOk = ModelName != string.Empty;
                isOk &= (LithoData.Layers.Count() >= 1);
                isOk &= (BaseName != string.Empty ? true : false);
                isOk &= (RestorationFolder != string.Empty);
                isOk &= (ReferenceGridDroid != string.Empty);

                if (!isOk)
                    return false;

                if (simPanel1D.Visible == true)
                {
                    if ((PointSet)presentationBox1DSimulation.Tag == null) return false;
                    if (simBaseNameTextBox.Text == string.Empty) return false;
                }

                if (simPanel3D.Visible == true)
                {
                    if ((Vector3[])presentationBox3DSimulation.Tag == null) return false;
                    Vector3[] vec = (Vector3[])presentationBox3DSimulation.Tag;
                    if (vec.Count() != 4)
                    {
                        MessageBox.Show("The boundary polygon in 3D must have 4 points", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (simBaseNameTextBox.Text == string.Empty) return false;
                }

                //this is a patch for a bug
                if (TriangulatedSurfaceFilesToCopy.Count() == 0)
                {
                    TriangulatedSurfaceFilesToCopy.AddRange(Directory.GetFiles(RestorationFolder));
                }

                return isOk;
            }
        }

        public void UIFromProject( GFMProject Project )
        {
            ModelName = Project.Name;
            LithoData = Project.LithoData;
            BaseName = Project.BaseName;

            ReferenceGridDroid = Project.GridDroid;
            RestorationFolder = Project.RestorationFolder;
            TriangulatedSurfaceFilesToCopy.Clear();

            foreach (var l in LithoData.Layers)
            {
                List<KeyValuePair<double, double>> dvt = l.GetDVTTable();
                string droid = GFMOceanUtils.PublishFunction(ModelName, l.Layer + "_YM_dvt", dvt);
                l.YoungsModulusHardeningFunctionDroid = droid;
            }
            LithoData = Project.LithoData;
            //MessageBox.Show("Stiffness vs strain tables created in the input tab inside the folder:\n\n" + ModelName, "Data created/changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string FixHorizonName( string s )
        {
            string[] keys = new string[] { "present", "day", "deposition", "step" };

            foreach (string forbiddenKey in keys)
            {
                string lower = s.ToLower();
                int index1 = lower.IndexOf(forbiddenKey);
                if (index1 >= 0)
                    s = s.Remove(index1, forbiddenKey.Length);
            }
            s = s.Trim(new char[] { '_', '-' });

            return s;
        }

        //public Dictionary<string, GFMDVTTable> GetDVTTablesFromUI()
        //{
        //    LithoData data = LithoData;

        //    Dictionary<string, GFMDVTTable> tables = new Dictionary<string, GFMDVTTable>();
        //    foreach (var l in data.Layers)
        //    {
        //        string droid = l.YoungsModulusHardeningFunctionDroid;
        //        Function f = null;
        //        try
        //        {
        //            f = (Function)DataManager.Resolve(new Droid(droid));
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Error. The function describing the dvt table was deleted from Petrel or cant be found");
        //            return null;
        //        }

        //        GFMDVTTable table = new GFMDVTTable(l.Layer);
        //        foreach (var p in f.Points)
        //            table.Data.Add(new Tuple<double, double>(p.X, p.Y));

        //        tables.Add(l.Layer, table);

        //    }

        //    return tables;

        //}

        private void ProcessRestorationDataFolder( string path, out string error )
        {
            error = string.Empty;
            if ((path == null) || (path == string.Empty)) return;

            bool applyPath = false;

            //gocadFolderBox.Text = path;
            //gocadFolderBox.Image = PetrelImages.Folder;

            //1- DO SOME VALIDATION HERE. WE NEED THAT ALL THE STEPS IN THE TABLE ARE IN SEPARATED FOLDERS
            //2- WE NEED ALL TO HAVE NAME + SUFFIX = _d   Deposited
            //3- we need that each contains the right number of horizons and the right names.
            //then we copy everything to the model folder. all the names, everything when generate is executed.

            //validation 1
            //required names
            string errors1 = string.Empty;
            List<string> stepNames = LithoData.Layers.Select(t => t.Layer + "Deposited").ToList();
            stepNames.Insert(0, "PresentDay");

            //get the subfolder names
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryInfo[] directories = directory.GetDirectories();
            string[] dirNames = directories.Select(t => t.Name).ToArray();

            foreach (string name in stepNames)
                if (!dirNames.Contains(name))
                {
                    errors1 += "     " + name + "\n";
                    //path create the required subfolders
                    if (applyPath) Directory.CreateDirectory(Path.Combine(path, name));
                }
            directories = directory.GetDirectories();

            //get all rubbysh files that Assef sent to me

            if (errors1 != string.Empty)
            {
                error = "Missing folders for geological steps:\n" + errors1;
                RestorationFolder = string.Empty;

                return;
            }

            //now check l;ayer by layer. The first folder should contain all names except
            string errors2 = string.Empty;
            //stepNames[n], dirNames[n], directories[n];

            int k = 1;
            List<string> missingFiles = new List<string>();
            stepNames = LithoData.Layers.Select(t => t.Layer).ToList();
            stepNames.Insert(0, "PresentDay");
            stepNames.Add(BaseName);// basementBox.Text);

            for (int n = 0; n < stepNames.Count() - 1; n++)
            {
                string stepName = stepNames[n];
                string suffix = stepName == "PresentDay" ? "" : "Deposited";
                DirectoryInfo dir = directories.Where(t => t.Name == stepName + suffix).ElementAt(0);
                string[] filesInFolder = dir.GetFiles().Select(t => t.Name).ToArray();

                //first make a list of the required files.
                List<string> requiredFiles = new List<string>();
                for (int kk = n + 1; kk < stepNames.Count(); kk++)  //(n+1) was k
                {
                    string fileName = stepName + suffix + "_" + stepNames[kk];
                    requiredFiles.Add(fileName);
                }

                if (suffix == "Deposited")
                {
                    requiredFiles.Insert(0, stepName + "Deposited_" + stepName);
                }

                //now we check whther those files exist as either .msh or .ts
                foreach (string baseName in requiredFiles)
                {
                    if (filesInFolder.Contains(baseName + ".msh"))
                    {
                        string fullFileName = Path.Combine(dir.FullName, baseName + ".msh");
                        TriangulatedSurfaceFilesToCopy.Add(fullFileName);
                    }
                    else if (filesInFolder.Contains(baseName + ".ts"))
                    {
                        string fullFileName = Path.Combine(dir.FullName, baseName + ".ts");
                        TriangulatedSurfaceFilesToCopy.Add(fullFileName);
                    }
                    else
                    {
                        missingFiles.Add(baseName + " (.ts ot .msh)");
                    }
                }
                k += 1;

                /*
                if (stepName == "PresentDay")
                {
                    string fileRequiredOption1 = "PresentDay_" + basementBox.Text + ".msh";
                    string fileRequiredOption2 = "PresentDay_" + basementBox.Text + ".ts";
                    if (filesInFolder.Contains(fileRequiredOption1))
                    {
                        fileRequiredOption1 = Path.Combine(dir.FullName, fileRequiredOption1);
                        TriangulatedSurfaceFilesTocopy.Add(fileRequiredOption1);
                    }
                    else if (filesInFolder.Contains(fileRequiredOption2))
                    {
                        fileRequiredOption2 = Path.Combine(dir.FullName, fileRequiredOption2);
                        TriangulatedSurfaceFilesTocopy.Add(fileRequiredOption2);
                    }
                    else
                    {
                        missingFiles.Add("PresentDay_" + basementBox.Text +  " (.ts ot .msh)");
                    }
                }

                for (int kk = k; kk < stepNames.Count(); kk++)
                {
                    string fileRequiredOption1 = stepName + suffix + "_" + stepNames[kk] + ".msh";
                    string fileRequiredOption2 = stepName + suffix + "_" + stepNames[kk] + ".ts";

                    if (filesInFolder.Contains(fileRequiredOption1))
                    {
                        fileRequiredOption1 = Path.Combine(dir.FullName, fileRequiredOption1);
                        TriangulatedSurfaceFilesTocopy.Add(fileRequiredOption1);
                    }
                    else if (filesInFolder.Contains(fileRequiredOption2))
                    {
                        fileRequiredOption2 = Path.Combine(dir.FullName, fileRequiredOption2);
                        TriangulatedSurfaceFilesTocopy.Add(fileRequiredOption2);
                    }
                    else
                    {
                        missingFiles.Add(stepName + suffix + "_" + stepNames[kk] + " (.ts ot .msh)");
                    }
                }*/
            }

            if (missingFiles.Count() > 0)
            {
                error = "Missing " + missingFiles.Count() + " files describing the horizons.\nThese are some examples:\n";

                for (int n = 0; n < Math.Min(5, missingFiles.Count()); n++)
                {
                    error += (missingFiles[n] + "\n");
                }
                RestorationFolder = string.Empty;

                return;
            }

            //gocadFolderBox.Text = path;
            //gocadFolderBox.Image = PetrelImages.Apply;
            RestorationFolder = path;

            MessageBox.Show("A total of " + TriangulatedSurfaceFilesToCopy.Count() + " files were selected ", "Data queued to be copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Update1DSimulationList( Dictionary<string, List<string>> simulationNameAndSteps )
        {
            sims1DCaseSelector.Tag = simulationNameAndSteps;
            int wasSelected = sims1DCaseSelector.SelectedIndex;

            sims1DCaseSelector.Items.Clear();
            foreach (string key in simulationNameAndSteps.Keys)
            {
                sims1DCaseSelector.Items.Add(Path.GetFileName(key));
            }
            try
            {
                if (wasSelected > 0)
                {
                    sims1DCaseSelector.SelectedIndex = wasSelected;
                }
                else
                    sims1DCaseSelector.SelectedIndex = sims1DCaseSelector.Items.Count > 0 ? 0 : -1;
            }
            catch
            {
                sims1DCaseSelector.SelectedIndex = 0;
            }
        }

        public void DisplayThicknessValues( string SeriesName, Dictionary<string, double> data )
        {
            Series s = null;
            var matchingSeries = simd1DResultsChart.Series.Where(t => t.Name == SeriesName);
            if (matchingSeries.Count() > 0)
            {
                s = matchingSeries.ElementAt(0);
                s.Enabled = false;
                s.Points.Clear();
            }
            else
            {
                s = new Series();
                s.Name = SeriesName;
                s.Enabled = false;
                simd1DResultsChart.Series.Add(s);
            }
            foreach (string layerName in data.Keys)
                s.Points.AddXY(layerName, data[layerName]);

            s.Enabled = true;
        }

        #endregion Calculation and Validation

        private void button2_Click( object sender, EventArgs e )
        {
            ChartConfigurationDialog dlg = new ChartConfigurationDialog();
            dlg.Chart = simd1DResultsChart;
            dlg.Show();
        }

        private void button4_Click( object sender, EventArgs e )
        {
            //panel2.MinimumSize = new Size(100, 5);

            //splitContainer1.Panel1.Controls.Add( lithoTablePanel1DCharting);
            //lithoTablePanel1DCharting.Dock = DockStyle.Fill;

            //return;
            splitContainer1.Panel1Collapsed = !(splitContainer1.Panel1Collapsed);
            lithoTablePanel1DCharting.Visible = !splitContainer1.Panel1Collapsed;

            splitContainer1.Width = splitContainer1.Width + 1;
            splitContainer1.Width = splitContainer1.Width - 1;

            //flowLayoutPanel1.SuspendLayout();

            //lithoTablePanel1DCharting.Visible = !lithoTablePanel1DCharting.Visible;

            //if (lithoTablePanel1DCharting.Visible = false)
            //{
            //    groupBox1.Height = flowLayoutPanel1.Height - 2;
            //    groupBox1.Width = flowLayoutPanel1.Width - 2;//.Fill;
            //    groupBox1.BackColor = Color.Orange;

            //    simd1DResultsChart.Dock = DockStyle.None;
            //    simd1DResultsChart.BackColor = Color.Green;
            //    simd1DResultsChart.Width = groupBox1.Width - 30;
            //    simd1DResultsChart.Height = groupBox1.Height - 5;

            //    //flowLayoutPanel1.ResumeLayout();

            //    flowLayoutPanel1.BackColor = Color.Red;
            //}
            //else
            //{
            //}
        }

        private void modelNameBox_TextChanged( object sender, EventArgs e )
        {
            modelNameLabel.Text = modelNameBox.Text;
        }

        private void ds_SplitContainer_Paint( object sender, PaintEventArgs e )
        {
            //splitContainer1.Paint();

            SplitContainer l_SplitContainer = sender as SplitContainer;

            if (l_SplitContainer != null)
            {
                Rectangle ll_ShrinkedSplitterRectangle = l_SplitContainer.SplitterRectangle;
                ll_ShrinkedSplitterRectangle.Offset(0, 1);
                ll_ShrinkedSplitterRectangle.Height = ll_ShrinkedSplitterRectangle.Height - 1;
                e.Graphics.FillRectangle(Brushes.Black, ll_ShrinkedSplitterRectangle);
            }
        }

        private void clearButton_Click( object sender, EventArgs e )
        {
            ClearProject();
        }
    }//class
}//namespace