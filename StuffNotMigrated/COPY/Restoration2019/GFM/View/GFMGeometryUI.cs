using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using System.Linq;
using System.Collections.Generic;
using Restoration.GFM;
using System.Data;
using Restoration.OceanUtils;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Analysis;
using System.IO;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.UI.Controls;
//using Slb.Ocean.Petrel.UI;

using System.Speech.Synthesis;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CommonData;
using Restoration.GFM.Services;

namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class GFMGeometryUI : UserControl
    {
        public event EventHandler CancelClicked;
        public event EventHandler<StringArgs> LoadModelClicked;

        public event EventHandler GenerateProject;
        public event EventHandler<SimulationEventArgs> Generate1DModels;

        public event EventHandler Request1DSimulationList;
        public event EventHandler Request1DSimulationResults;

        IGFMSerializer Serializer { get; set; }

        //private GFMProject Project { get; set; }

        public string ModelName
        {
            get
            {
                return modelNameBox.Text;/// return Project.Name;//.Text;
            }
            set
            {
                modelNameBox.Text = value;
                //Project.Name = value;
            }
        }

        public LithoData LithoData { get { return _lithoData; } set { _lithoData = value; PopulateTable(); } }// { return Project.LithoData; } set { Project.LithoData = value; PopulateTable(); } }
        LithoData _lithoData = new LithoData();


        public string BaseName
        {
            set
            {
                basementBox.Text = value;
                basementBox.Image = basementBox.Text != string.Empty ? PetrelImages.Horizon : null;
            }

            get
            {
                return basementBox.Text;// return Project.BaseName;
            }
        }

        public string ReferenceGridDroid
        {
            get
            {
                return _referenceGridDroid;//.Text;
            }
            set
            {
                _referenceGridDroid = string.Empty;

                Grid g = null;
                Droid d = Droid.TryParse(value);
                if (d != null)
                {
                    g = (Grid)DataManager.Resolve(d);
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
        string _referenceGridDroid;

        public string RestorationFolder
        {
            get
            {
                return gocadFolderBox.Text;
            }

            set
            {
                gocadFolderBox.Text = value;
                gocadFolderBox.Image = gocadFolderBox.Text != string.Empty ? PetrelImages.Apply : null;

            }
        }


        public GFMGeometryUI(GFMGeometry process)//, IGFMSerializer serial)
        {
            InitializeComponent();
            InitializeComponentExtras();

            //this.Process = process;
            //Serializer = serial;
            //Project = process.Model;


            ConnectEvents();
            UIFromProject(process.Model);
        }

        private void InitializeComponentExtras()
        {
            rotationLabel.Visible = true;
            rotationControl.Visible = true;
            openProjectButton.Image = PetrelImages.Folder;
            openProjectButton.Text = string.Empty;
            openProjectButton.ImageAlign = ContentAlignment.MiddleCenter;
            radio1DSim.Checked = false;
            radio1DSim.Checked = true;
            this.newProjectRadio.Checked = true;
            rotationControl.DecimalPlaces = 4;
            loadLithoFromFileButton.Image = PetrelImages.Folder;
            loadLithoFromFileButton.Text = string.Empty;
            goCadFolderButton.Image = PetrelImages.Folder;
            goCadFolderButton.Text = string.Empty;
            lithoTableMoveDown.Image = PetrelImages.DownArrow;
            lithoTableMoveUp.Image = PetrelImages.UpArrow;
            lithoTableDelete.Image = PetrelImages.Cancel;
            GeneratePropertyFilesButton.Image = PetrelImages.Apply;
            CancelButton.Image = PetrelImages.Cancel;
        }

        public List<string> TriangulatedSurfaceFilesToCopy { get; set; } = new List<string>();


        private void ConnectEvents()
        {

            this.simulation1DProcessingControl1.Request1DSimulationResults += (sender, evt) =>
            {

                int column = simulation1DProcessingControl1.SelectedColumn;
                string caseName = simulation1DProcessingControl1.SelectedCase;

                Request1DSimulationResults?.Invoke(this, EventArgs.Empty);




            };


            this.CancelButton.Click += (sender, evt) =>
            {
                CancelClicked?.Invoke(this, EventArgs.Empty);
                this.ParentForm.Close();
            };

            this.openProjectButton.Click += (seder, evt) =>
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

            this.GeneratePropertyFilesButton.Click += (sender, evt) =>
            {// new System.EventHandler(this.Generate_Click);

                if (tabControl2.SelectedTab == GenerateTab)
                {
                    if (isItOkToGenerate)
                        GenerateProject?.Invoke(this, EventArgs.Empty);
                    else
                        MessageBox.Show("Cannot generate the project. Data is incomplete");
                }

                else if (tabControl2.SelectedTab == SimulationsTab)
                {
                    if (isItOkToGenerate)
                    {
                        if (simPanel1D.Visible == true)
                        {
                            if (presentationBox1DSimulation.Tag != null)
                            {
                                string simBaseName = simBaseNameTextBox.Text;
                                Vector3[] xyz = ((PointSet)(presentationBox1DSimulation.Tag)).Points.Select(t => new Vector3(t.X, t.Y, 0.0)).ToArray();

                                SimulationEventArgs e = new SimulationEventArgs(simBaseName, 1, xyz);
                                Generate1DModels?.Invoke(this, e);
                            }
                        }

                        Request1DSimulationList?.Invoke(this, EventArgs.Empty);


                    }
                    else
                        MessageBox.Show("Cannot generate the simulation. Data is incomplete");
                }
                else {; }

            };

            this.loadProjectRadio.CheckedChanged += new System.EventHandler(this.NewLoadProjectChanged);
            this.newProjectRadio.CheckedChanged += new System.EventHandler(this.NewLoadProjectChanged);
            // this.loadLithoFromFileButton.Click += new System.EventHandler(this.loadLithoFromFileButton_Click);

            this.addErosionalStep.Click += new System.EventHandler(this.lithoTableMoveUp_Click);
            this.referencialGridDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.referencialGridDrop_DragDrop);
            this.dropPoints1DSimulation.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropPoints1DSimulation_DragDrop);

            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_TabIndexChanged);

        }

        private bool isItOkToGenerate
        {
            get
            {
                bool isOk = ModelName != string.Empty;
                isOk &= (LithoData.Layers.Count() >= 1);
                isOk &= (BaseName != string.Empty ? true : false);
                isOk &= (RestorationFolder != string.Empty);
                isOk &= (ReferenceGridDroid != string.Empty);


                if (simPanel1D.Visible == true)
                {
                    if ((PointSet)presentationBox1DSimulation.Tag == null) return false;
                    if (simBaseNameTextBox.Text == string.Empty) return false;



                }

                return isOk;
            }
        }

        public void UIFromProject(GFMProject Project)
        {
            ModelName = Project.Name;
            LithoData = Project.LithoData;
            PopulateTable();
            BaseName = Project.BaseName;

            radioButton2.Checked = true;
            ReferenceGridDroid = Project.GridDroid;
            RestorationFolder = Project.RestorationFolder;
            TriangulatedSurfaceFilesToCopy.Clear();
            // string error;
            // ProcessRestorationDataFolder(Project.RestorationFolder, out error);

            // if (error != string.Empty)
            // {
            //     MessageBox.Show("There are errrors in the Restoration data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }

        }


        private string PublishFunction(string name, List<KeyValuePair<double, double>> functionData)
        {

            string droid = "";

            Collection col = ProjectTools.GetOrCreateCollectionByName(ModelName);

            Function f = ProjectTools.CreateOrReplaceFunction(col, name, functionData);

            droid = f.Droid.ToString();
            return droid;
        }


        private void PopulateTable()//LithoData data)
        {
            LithoData data = LithoData;

            LithoTable.Rows.Clear();
            LithoTable.Columns.Clear();

            foreach (string name in LayerLithoData.VisiblePropertyNames)
                LithoTable.Columns.Add(name, name);

            foreach (LayerLithoData row in data.Layers)
            {
                string[] values = row.VisiblePropertyValues;
                LithoTable.Rows.Add(values);

            }

            for (int n = 0; n < LithoTable.RowCount; n++)
            {
                LithoTable.Rows[n].Cells[0].Style.BackColor = ProjectTools.GetRandomColor(n); ;

            }



        }


        private string FixHorizonName(string s)
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


        private void GeologicalStepsSourceChanged(object sender, EventArgs e)
        {
            Panel[] panels = { this.optionsPanel1, this.optionsPanel2 };
            RadioButton[] buttons = { this.radioButton1, this.radioButton2 };

            for (int i = 0; i < 2; i++)
            {
                if (sender as RadioButton == buttons[i])
                {
                    panels[i].Enabled = true;
                }
                else
                    panels[i].Enabled = false;


            }
        }

        public Dictionary<string, GFMDVTTable> GetDVTTablesFromUI()
        {

            LithoData data = LithoData;

            Dictionary<string, GFMDVTTable> tables = new Dictionary<string, GFMDVTTable>();
            foreach (var l in data.Layers)
            {

                string droid = l.YoungsModulusHardeningFunctionDroid;

                Function f = null;
                try
                {
                    f = (Function)DataManager.Resolve(new Droid(droid));
                }
                catch
                {
                    MessageBox.Show("Error. The function describing the dvt table was deleted from Petrel or cant be found");
                    return null;
                }

                GFMDVTTable table = new GFMDVTTable(l.Layer);
                foreach (var p in f.Points)
                    table.Data.Add(new Tuple<double, double>(p.X, p.Y));

                tables.Add(l.Layer, table);

            }

            return tables;

        }


        private GFMProject loadProjectFromFolder(string folderName)
        {

            return new GFMProject();
        }

        private void goCadFolderButton_Click(object sender, EventArgs e)
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
                MessageBox.Show(error, "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RestorationFolder = string.Empty;
            }
        }


        private void ProcessRestorationDataFolder(string path, out string error)
        {

            error = string.Empty;
            if (path == null) return;
            if (path == string.Empty) return;
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
                error = "Missing Folders for Geological Steps:\n" + errors1;
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
            stepNames.Add(basementBox.Text);

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

        private void cancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SImDimensionRadio(object sender, EventArgs e)
        {
            RadioButton b = (RadioButton)(sender);

            if (b == radio1DSim) simPanel1D.Show(); else simPanel1D.Hide();
            if (b == radio2DSim) simPanel2D.Show(); else simPanel2D.Hide();
            if (b == radio3DSim) simPanel3D.Show(); else simPanel3D.Hide();

        }

        private void dropPoints1DSimulation_DragDrop(object sender, DragEventArgs e)
        {
            PointSet points = e.Data.GetData(typeof(PointSet)) as PointSet;
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
        }


        private void GenerateDVTFunctions(List<LayerLithoData> data)
        {
            foreach (LayerLithoData l in data)
            {
                List<KeyValuePair<double, double>> function = GenerateYMFunctionData(l.YMInitial, l.YMFinal, l.YMLambda);
                string droid = PublishFunction(l.Layer + "_YM_dvt", function);
                l.YoungsModulusHardeningFunctionDroid = droid;
            }
        }

        private void GenerateDVTFunction(LayerLithoData l)
        {
            List<KeyValuePair<double, double>> function = GenerateYMFunctionData(l.YMInitial, l.YMFinal, l.YMLambda);
            string droid = PublishFunction(l.Layer + "_YM_dvt", function);
            l.YoungsModulusHardeningFunctionDroid = droid;
        }

        public static List<KeyValuePair<double, double>> GenerateYMFunctionData(double Ymo, double Ymf, double YmLambda, int nPoints = 50, double minStrain = -0.5, double maxStrain = 0.0)
        {
            List<KeyValuePair<double, double>> data = new List<KeyValuePair<double, double>>();
            minStrain = 0.0;
            maxStrain = 0.5;///3.00; 
            double strain = 0.0;// minStrain;
            double deltaStrain = Math.Abs((maxStrain - minStrain) / nPoints);
            int NNPoints = 1 + (int)((maxStrain - minStrain) / deltaStrain);


            double factor = YmLambda * (Ymf - Ymo) / (0.99 * Ymf - Ymo) - YmLambda;

            for (int n = 0; n < NNPoints; n++)
            {
                double ym = Ymo + (Ymf - Ymo) * (strain / (factor + strain));
                data.Add(new KeyValuePair<double, double>(-strain / 3.0, ym));
                strain += deltaStrain;
            }

            //invert the table so Petrel is happy with x increasing values 
            data.Reverse();



            return data;

        }


        private void NewLoadProjectChanged(object sender, EventArgs evt)
        {
            RadioButton b = (RadioButton)(sender);
            openProjectButton.Visible = b == newProjectRadio ? false : true;
            labelName.Visible = !(openProjectButton.Visible);
        }

        private void lithoTableMoveUp_Click(object sender, EventArgs e)
        {
            Button b = (Button)(sender);

            if (b == lithoTableMoveUp)
            {
                MessageBox.Show("Not implemeted yet....Wait!!");
            }
            else if (b == lithoTableMoveDown)
            {
                MessageBox.Show("Not implemeted yet....Wait!!");
            }
            else if (b == lithoTableDelete)
            {

                var rows = LithoTable.SelectedRows;
                if (rows.Count >= 1)
                {
                    int row = rows[0].Index;
                    LithoData.Layers.RemoveAt(row);
                    LithoTable.Rows.RemoveAt(row);
                }


            }
            else if (b == addErosionalStep)
            {
                AddGeologicalStepDialog dlg = new AddGeologicalStepDialog();
                DialogResult r = dlg.ShowDialog();
                string entered = dlg.NameEntered;

                if (entered != string.Empty)
                {
                    LayerLithoData l = new LayerLithoData();
                    l.Layer = dlg.NameEntered;
                    LithoData.Layers.Add(l);
                    GenerateDVTFunction(l);
                    PopulateTable();
                }
            }
        }


        private void referencialGridDrop_DragDrop(object sender, DragEventArgs evt)
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


            if (dropped as Grid != null)
            {
                Grid g = (Grid)(dropped);
                ReferenceGridDroid = g.Droid.ToString();


                processGridDrop(g, null);
            }
            //else if (dropped as Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection != null)
            //{
            //    Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection c = (Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection)(dropped);
            //    referenceGridPresentationBox.Text = c.Grid.Name;
            //    referenceGridPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.PillarGrid;
            //    processGridDrop(c.Grid, c);
            //}
            else
            {
                ReferenceGridDroid = string.Empty;
                //referenceGridPresentationBox.Text = string.Empty;
                //referenceGridPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.Cancel;

                processGridDrop(null, null);
                MessageBox.Show("Please Select a Pillar Grid", "Object is not a Valid Pillar Grid.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void processGridDrop(Grid g, Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection pc)
        {
            LithoData = new LithoData();
            BaseName = string.Empty;

            if (g != null)
            {

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


                LithoData.Layers = data;
                GenerateDVTFunctions(LithoData.Layers);
                MessageBox.Show("Stiffness vs strain tables created in folder:\n\n" + ModelName + "\n\nIn the Input tab", "Data created/changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            PopulateTable();
            //Project.GridDroid = g.Droid.ToString();// as string;

            ReferenceGridDroid = g.Droid.ToString();


            ;
            //comboBox1.Items.Clear();
            //int indexSelected = -1;
            //foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection c in g.PropertyCollection.PropertyCollections)
            //{
            //    ComboBoxItem it = new ComboBoxItem();
            //    it.Text = c.Name;
            //    it.Image = PetrelImages.Property;
            //    it.Value = c;
            //    comboBox1.Items.Add(it);

            //    if (c == pc) indexSelected = comboBox1.Items.Count() - 1;
            //}
            //ComboBoxItem item = new ComboBoxItem();
            //item.Text = "Default";
            //item.Image = PetrelImages.Property;
            //item.Value = null;
            //comboBox1.Items.Add(item);
            //if (indexSelected == -1)
            //    indexSelected = comboBox1.Items.Count() - 1;



            //comboBox1.SelectedIndex = -1;
            //comboBox1.SelectedIndex = indexSelected;







            //int lastRow = LithoTable.Rows.Count-1;
            //Rectangle r = LithoTable.GetCellDisplayRectangle(0, 0, false);
            //LithoTable.Height = (2 + LithoTable.Rows.Count) * r.Height;
            //LithoTable.Width = 500;
            //Size s = LithoTable.ClientSize;
            //s.Height = r.Y + r.Height;
            //LithoTable.ClientSize = s;




            //try
            //{
            //    Slb.Ocean.Geometry.Point3 p1 = g.GetCellCenter(new Slb.Ocean.Basics.Index3(11, 21, 11));
            //    Slb.Ocean.Geometry.Point3 p2 = g.GetCellCenter(new Slb.Ocean.Basics.Index3(1, 0, 0));
            //    Slb.Ocean.Geometry.Point3 p3 = g.GetCellCenter(new Slb.Ocean.Basics.Index3(0, 1, 0));

            //    decimal I = (decimal)(p3 - p1).Norm / g.NumCellsIJK.I;
            //    decimal J = (decimal)(p3 - p1).Norm / g.NumCellsIJK.J;
            //    I = Math.Min(xResolutionControl.Maximum, Math.Max(xResolutionControl.Minimum, I));
            //    J = Math.Min(xResolutionControl.Maximum, Math.Max(xResolutionControl.Minimum, J));
            //    decimal minOfThem = Math.Min(I, J);
            //    xResolutionControl.Value = minOfThem;


            //}
            //catch
            //{
            //    double lx = g.BoundingBox.Length / g.NumPillarsIJ.I;
            //    double ly = g.BoundingBox.Width / g.NumPillarsIJ.J;
            //    double minOfThem = Math.Min(lx, ly);
            //    xResolutionControl.Value = (decimal)(minOfThem);


            //}



        }

        private void LithoTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            ;
            ;
            string[] visiblePropertyNames = LayerLithoData.VisiblePropertyNames;
            string propertyChanged = visiblePropertyNames[column];
            string newValue = (string)(LithoTable.Rows[row].Cells[column].Value);

            LithoData.Layers[row].SetValue(propertyChanged, newValue);
            //PopulateTable();


            string[] values = LithoData.Layers[row].VisiblePropertyValues;
            LithoTable.Rows[row].SetValues(values);


            ;
        }






        private void tabControl2_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab != tabPage1) return;


            //get 1D simulation cases and populate combobox
            string projectName = ModelName;

            Request1DSimulationList?.Invoke(this, new StringArgs(ModelName));

            //set the limits to the point index for this simulation

            //get the point index FGRID results file 
            //parse the file and plot thickness
        }



        public void Update1DSimulationList(Dictionary<string, int> simulationNameAndSteps)
        {

            simulation1DProcessingControl1.Update1DSimulationList( simulationNameAndSteps );


        }

        private void simulation1DProcessingControl1_Load(object sender, EventArgs e)
        {
            ;
            ;
            ;
            ;
            ;

        }
    }



}

