using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using Gigamodel.VisageUtils;
using Gigamodel.OceanUtils;
using Slb.Ocean.Petrel.UI.Controls;
using Gigamodel.Data;
using Slb.Ocean.Petrel;

using GeoTemplateImages = Slb.Ocean.Petrel.PetrelProject.WellKnownTemplates.GeomechanicGroup;
using Templates = Slb.Ocean.Petrel.PetrelProject.WellKnownTemplates;
using Slb.Ocean.Core;

namespace Gigamodel
{
    public partial class ImportResultsControl : UserControl
    {
        public event EventHandler<StringEventArgs> ResultsFolderSelected;
        public event EventHandler ListCasesRequestEvent;


        public ImportResultsControl()
        {
            InitializeComponent();
            resultsTree.BorderStyle = BorderStyle.None;
            resultsTree.Nodes.Clear();
            casesGrid.BorderStyle = BorderStyle.None;
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            this.selectSimulationFolder.Click += new System.EventHandler(this.selectSimulationFolder_Click);
            this.casesGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.casesGrid_CellClick);

            this.VisibleChanged += (s, e) =>
            {
                ListCasesRequestEvent?.Invoke(this, EventArgs.Empty);
            };


        }

        public List<string> SelectedPropertiesForImport
        {
            get
            {
                List<string> names = new List<string>();
                foreach (TreeNode rootNode in resultsTree.Nodes)
                {
                    foreach (TreeNode n in rootNode.Nodes)
                        if (n.Checked) names.Add(n.Text);
                }
                return names;
            }
        }

        public string CaseName
        {
            get { return caseNameTextBox.Text; }
            set { caseNameTextBox.Text = value; }
        }

        public string FolderName
        {
            get { return simFolderTextBox.Text; }
            set { simFolderTextBox.Text = value; }
        }

        /* public int TimeStep
         {
             get { return Convert.ToInt32(timeStepNumericControl.Value); }
             set
             {
                 if (value > TimeSteps) TimeSteps = value;
                 timeStepNumericControl.Value = value;
             }
         }*/

        /*public int TimeSteps
        {
            get { return Convert.ToInt32(timeStepNumericControl.Maximum); }
            set
            {
                if (value < TimeStep) TimeStep = value;
                timeStepNumericControl.Maximum = value;
            }
        }*/

        public void ClearControl()
        {
           // FolderName = string.Empty;
           // CaseName = string.Empty;

            ReferenceDroid = string.Empty;

            //  TimeStep = 0;
            // TimeSteps = 0;

            //ListOfCases = new List<string>(); 

            this.timeStepsComboBox.Items.Clear();
            resultsTree.Nodes.Clear();
            //label5.Visible = false;
        }

        public SeismicCube ReferenceCube
        {
            get { try { return DataManager.Resolve(new Droid(ReferenceDroid)) as SeismicCube; } catch { return null; } }
        }

        public List<string> ListOfCases
        {
            get
            {
                List<string> l = new List<string>();
                foreach (DataGridViewRow r in casesGrid.Rows)
                    l.Add(r.Cells[1].Value.ToString());

                return l;

            }
            set
            {
                casesGrid.Rows.Clear();

                Bitmap original = PetrelImages.Folder_32;
                Bitmap resized = new Bitmap(original, new Size(original.Width / 2, original.Height / 2));


                foreach (string s in value)
                {
                    casesGrid.Rows.Add(resized, s);
                }

                /*
                List<string> l = new List<string>();
                foreach (DataGridViewRow r in casesGrid.Rows)
                l.Add(r.Cells[0].Value.ToString());

                foreach (string s in value)
                {
                    if (!l.Contains(s))
                    {
                        casesGrid.Rows.Add(s);
                    }
                }
                */
            }//set

        }

        public string ReferenceDroid { get; set; }

        public void DisplayAvailableResultsForCase(string[] names, int tSteps, string caseName, string droidAsString)
        {
            List<string> lnames = names.ToList();
            resultsTree.Nodes.Clear();
          
            string[] groupPreffixes = { "ROCKDIS", "EFFSTR", "TOTSTR", "PRESS", "STRAIN" };
            string[] groupsName = { "DISPLACEMENT", "EFF. STRESS", "TOT. STRESS", "PRESSURE", "STRAIN" };

            ImageList images = new ImageList();
            images.Images.AddRange(new Image[] {
            PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.RockDisplacement.TemplateType),
            PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressEffective.TemplateType),
            PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.StressTotal.TemplateType),
            PetrelSystem.TemplateService.GetTemplateTypeImage( Templates.PetrophysicalGroup.Pressure.TemplateType),
            PetrelSystem.TemplateService.GetTemplateTypeImage( GeoTemplateImages.Strain.TemplateType),
            PetrelSystem.TemplateService.GetTemplateTypeImage( Templates.MiscellaneousGroup.General.TemplateType)

            });


            resultsTree.ImageList = images;
            resultsTree.AfterCheck += (s, e) =>
            {
                if ((e.Action != TreeViewAction.Unknown) && (e.Node.Nodes.Count > 0))
                    foreach (TreeNode t in e.Node.Nodes) t.Checked = e.Node.Checked;
            };


            for (int n = 0; n < groupPreffixes.Count(); n++)
            {
                var children = names.Where(t => t.Contains(groupPreffixes[n]));
                if (children.Count() > 0)
                {
                    TreeNode gnode = new TreeNode(groupsName[n], n, n);
                    gnode.Tag = null;

                    //gnode.ImageIndex = n;
                    resultsTree.Nodes.Add(gnode);
                    foreach (string s in children)
                    {
                        TreeNode c = new TreeNode(s);
                        c.Tag = s;
                        c.Checked = false;
                        c.ImageKey = null;
                        c.ImageIndex = 999;
                        gnode.Nodes.Add(c);
                        lnames.Remove(s);
                    }
                }
            }
            TreeNode node = new TreeNode("Misc Dynamic", 5, 5);
            node.Tag = null;
            resultsTree.Nodes.Add(node);
            foreach (string s in lnames)
            {
                TreeNode c = new TreeNode(s);
                c.Checked = false;
                c.Tag = s;
                node.Nodes.Add(c);
            }

            //  TimeSteps = tSteps;
            //   TimeStep = 0;


            timeStepsComboBox.Items.Clear();
            for (int n = 0; n < tSteps ; n++)
            {
                timeStepsComboBox.Items.Add("Time " + n.ToString());
            }
            if(tSteps >0)
            timeStepsComboBox.SelectedIndex = 0;

            CaseName = caseName;
            ReferenceDroid = droidAsString;
            //label5.Visible = resultsTree.Nodes?.Count > 0;
        }

        public int TimeSteps
        {
            get
            {
                return timeStepsComboBox.Items.Count;
            } 
        }


        public int TimeStep
        {
            get
            {
                return timeStepsComboBox.SelectedIndex;
            }
        }



        private void selectSimulationFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = false;
                dlg.SelectedPath = GigaModelDataDefinitions.SimExportFolder;

                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
                {
                    FolderName = dlg.SelectedPath;
                    ResultsFolderSelected?.Invoke(this, new StringEventArgs(FolderName));
                }
            }

        }

        private void toolTipHotspot2_Click(object sender, EventArgs e)
        {
          //  panel1.Visible = !(panel1.Visible);
        }

        private void casesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (casesGrid.Rows.Count < 1) return;

            string caseName = casesGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            FolderName = GigaModelDataDefinitions.SimExportFolder + "\\" + caseName + "\\";
            ResultsFolderSelected?.Invoke(this, new StringEventArgs(FolderName));
        }
    }
}
