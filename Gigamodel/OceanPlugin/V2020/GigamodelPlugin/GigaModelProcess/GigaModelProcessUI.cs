using Gigamodel.Data;
using Slb.Ocean.Petrel.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gigamodel
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class GigaModelProcessUI : UserControl
    {   //private GigaModelBuildProcess process;
        //public event EventHandler SaveClicked;

        public event EventHandler CancelClicked;

        //create/edit items in the different controls
        public event EventHandler<ImportResultsEventArgs> ImportRequestEvent;

        //import export passed event to the controllers.
        public event EventHandler<CreateEditArgs> ExportSimulationRequestEvent;

        public event EventHandler<CreateEditArgs> ExportSimulationCompressedRequestEvent;

        public event EventHandler<CreateEditArgs> CreateOrEditEvent;

        public event EventHandler TabChanged;

        public MaterialsModelsControl materialsControl;
        public PressureModelsControl pressuresControl;
        public BoundaryConditionsControl boundaryConditionsControl;
        public SimulationsControl simulationControl;

        public ImportResultsControl resultsControl;

        public GigaModelProcessUI()//GigaModelProcess process)
        {
            InitializeComponent();

            setImagery();

            setTabsControls();

            ConnectEvents();

            buttonExportImport.Visible = false;
            buttonExportImport.Enabled = !(simulationControl.Dirty);

            tabControl.SelectedIndex = 0;

            System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer();
            string systemUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string userName = systemUserName.Substring(systemUserName.IndexOf('\\') + 1);

            //synth.SetOutputToDefaultAudioDevice();
            //synth.Speak("Hello " + userName);
            //synth.Speak("Welcome to Giga Model Builder Version 1.0");
        }

        public int TabIndexSelected
        {
            get { return tabControl.SelectedIndex; }
            set
            {
                tabControl.SelectedIndex = value;
                TabChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public TabPage TabPageSelected
        {
            get { return tabControl.TabPages[tabControl.SelectedIndex]; }
        }

        private void setImagery()
        {
            this.BackColor = Color.White;

            imageList1.Images.Clear();
            imageList1.Images.AddRange(new Bitmap[] {
                                       PetrelImages.Property,
                                       PetrelImages.Fluid,
                                       PetrelImages.Boundary,
                                       PetrelImages.Models,
                                       PetrelImages.Apply,
                                       PetrelImages.Cancel,
                                       PetrelImages.Help,
                                       PetrelImages.Info,
                                       PetrelImages.Pie,
                                       PetrelImages.Case
            });
            imageList1.ImageSize = new Size(16, 16);
            this.tabControl.ImageList = imageList1;
            this.materialsTab.ImageIndex = 0;
            this.pressuresTab.ImageIndex = 1;
            this.boundariesTab.ImageIndex = 2;
            this.simulationsTab.ImageIndex = 3;
            this.resultsTab.ImageIndex = 8;

            saveButton.ImageList = imageList1;
            saveButton.ImageAlign = ContentAlignment.MiddleLeft;

            cancelButton.ImageList = imageList1;
            cancelButton.ImageAlign = ContentAlignment.MiddleLeft;

            buttonExportImport.ImageList = imageList1;
            buttonExportImport.ImageAlign = ContentAlignment.MiddleLeft;

            saveButton.ImageIndex = 4;
            cancelButton.ImageIndex = 5;
        }

        private void setTabsControls()
        {
            materialsControl = new MaterialsModelsControl();
            materialsTab.Controls.Add(materialsControl);
            materialsControl.Dock = DockStyle.Fill;

            pressuresControl = new PressureModelsControl();
            pressuresTab.Controls.Add(pressuresControl);
            pressuresControl.Dock = DockStyle.Fill;

            boundaryConditionsControl = new BoundaryConditionsControl();
            boundariesTab.Controls.Add(boundaryConditionsControl);
            boundaryConditionsControl.Dock = DockStyle.Fill;

            simulationControl = new SimulationsControl();
            simulationsTab.Controls.Add(simulationControl);
            simulationControl.Dock = DockStyle.Fill;

            resultsControl = new ImportResultsControl();
            resultsTab.Controls.Add(resultsControl);
            resultsControl.Dock = DockStyle.Fill;
        }

        private void ConnectEvents()
        {
            this.simulationControl.DirtyChanged += ( sender, evt ) =>
            {
                buttonExportImport.Enabled = !(simulationControl.Dirty);
            };

            this.tabControl.SelectedIndexChanged += ( sender, evt ) =>
            {
                buttonExportImport.Visible = (tabControl.SelectedTab == resultsTab) || (tabControl.SelectedTab == simulationsTab);

                if (tabControl.SelectedTab == resultsTab) buttonExportImport.Enabled = true;
                else if (tabControl.SelectedTab == simulationsTab) buttonExportImport.Enabled = !(simulationControl.Dirty);

                buttonExportImport.Text = tabControl.SelectedTab == resultsTab ? "Import" : "Export";
                buttonExportImport.ImageIndex = tabControl.SelectedTab == resultsTab ? 8 : 9;
            };

            this.buttonExportImport.Click += ( s, e ) =>
            {
                if (TabPageSelected == simulationsTab)
                {
                    ExportButtonClicked(this.resultsControl, e);

                    //ExportSimulationRequestEvent?.Invoke(this, simulationControl.UIToModel);
                }
                else if (TabPageSelected == resultsTab)
                {
                    ImportButtonClicked(this.resultsControl, e);
                }
                else
                {
                    ;
                }
            };

            this.cancelButton.Click += ( s, e ) =>
            {
                //var dlg = MessageBox.Show("Save pending changes?", "Save Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (dlg == DialogResult.Yes)
                //{
                CancelClicked?.Invoke(this, EventArgs.Empty);
                //}
                this.ParentForm.Close();
            };

            this.saveButton.Click += ( s, e ) =>
            {
                if (tabControl.SelectedTab == materialsTab)
                {
                    CreateOrEditEvent?.Invoke(materialsControl, materialsControl.UIToModel);
                }
                else if (tabControl.SelectedTab == pressuresTab)
                {
                    CreateOrEditEvent?.Invoke(pressuresControl, pressuresControl.UIToModel);
                }
                else if (tabControl.SelectedTab == boundariesTab)
                {
                    CreateOrEditEvent?.Invoke(boundaryConditionsControl, boundaryConditionsControl.UIToModel);
                }
                else if (tabControl.SelectedTab == simulationsTab)
                {
                    CreateOrEditEvent?.Invoke(simulationControl, simulationControl.UIToModel);
                }
                else
                {
                    ;
                }
            };
        }

        private void ImportButtonClicked( object sender, EventArgs e )
        {
            ImportRequestEvent?.Invoke(resultsControl, new ImportResultsEventArgs(resultsControl.ReferenceCube,
                           resultsControl.SelectedPropertiesForImport,
                           resultsControl.FolderName,
                           resultsControl.CaseName,
                           resultsControl.TimeStep));
        }

        public void ClearResultsControl()
        {
            this.resultsControl.ClearControl();
        }

        private void ExportButtonClicked( object sender, EventArgs e )
        {
            CreateEditArgs args = simulationControl.UIToModel;
            SimulationModelItem sim = (SimulationModelItem)(args.Object);

            //var answer = MessageBox.Show("Do you want to use the new compressed binary export (fastest)?", "Export options", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (answer != DialogResult.Yes)
            //{
            //    MessageBox.Show("Well...you should because thats the only active option now.", "Export options", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //ExportSimulationRequestEvent?.Invoke(this, args);
            //else

            ExportSimulationCompressedRequestEvent?.Invoke(this, args);
        }

        private void button1_Click( object sender, EventArgs e )
        {
        }
    }
}