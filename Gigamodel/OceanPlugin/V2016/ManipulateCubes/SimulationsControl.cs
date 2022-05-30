using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.UI;
using System.Diagnostics;

namespace ManipulateCubes
{
    public partial class SimulationsControl : UserControl
    {
        #region UI
        public SimulationsControl()
        {
            InitializeComponent();
            InitializeSelectors();
            ConnectEvents();

            miiImage.Image = PetrelImages.Eye;
            modelImage.Image = PetrelImages.Model;
            modelImage.BorderStyle = BorderStyle.None;

            pressureImage.Image = PetrelImages.Fluid;
            pressureImage.BorderStyle = BorderStyle.None;

            bConditionsImage.Image = PetrelImages.Boundary;
            bConditionsImage.BorderStyle = BorderStyle.None;
        }

        public ImageList ImagesList
        { get { if (imageList1 == null) imageList1 = new ImageList(); return imageList1; }
            set { imageList1 = value; }
        }

        private void ConnectEvents()
        {
            this.newEditSelector1.SelectionChanged += new System.EventHandler(this.newEditSelector1_SelectionChanged);
            this.saveButton.Click += new System.EventHandler(this.CreateOrEditModel);

            this.exportButton.Click += new System.EventHandler(this.ExportClicked);
            this.VisibleChanged += new System.EventHandler(this.SimulationsControl_VisibleChanged);
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
            newEditSelector1.ModelNames = gigamodel.SimulationNames.ToArray();

            materialSelector.Items.Clear();
            pressureSelector.Items.Clear();
            boundarySelector.Items.Clear();

            materialSelector.Items.AddRange(gigamodel.MaterialModelNames);
            pressureSelector.Items.AddRange(gigamodel.PressureModelNames);
            boundarySelector.Items.AddRange(gigamodel.BoundaryConditionsNames);

            if ((materialSelector.SelectedIndex < 0) && (materialSelector.Items.Count > 0))
                materialSelector.SelectedIndex = 0;
            if ((pressureSelector.SelectedIndex < 0) && (pressureSelector.Items.Count > 0))
                pressureSelector.SelectedIndex = 0;
            if ((boundarySelector.SelectedIndex < 0) && (boundarySelector.Items.Count > 0))
                boundarySelector.SelectedIndex = 0;

           

        }

        private void newEditSelector1_SelectionChanged(object sender, EventArgs e)
        {
            ModelSelectedToUi();
        }

        private void ModelSelectedToUi()
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();

            string currentModelSelected = newEditSelector1.SelectedName;

            if (gigaModel.SimulationsModel.FindModel(currentModelSelected))
            {
                ModelToUi(gigaModel.SimulationsModel.GetOrCreateModel(currentModelSelected));
            }
        }

        private void ModelToUi(SimulationModelItem model)
        {
            string matName = model.MaterialModelName;
            string pressureName = model.PressureModelName;
            string bcName = model.BoundaryConditionsModelName;
            materialSelector.SelectedIndex = Array.IndexOf(getComboBoxNames(materialSelector), matName);
            pressureSelector.SelectedIndex = Array.IndexOf(getComboBoxNames(pressureSelector), pressureName);
            boundarySelector.SelectedIndex = Array.IndexOf(getComboBoxNames(boundarySelector), bcName); ;
            descriptionText.Text = model.Description;
        }

        private string[] getComboBoxNames(ComboBox selectorCombobox)
        {
            List<string> l = new List<string>();
            foreach (object x in selectorCombobox.Items) l.Add(x.ToString());
            return l.ToArray();
        }

        private void SimulationsControl_VisibleChanged(object sender, EventArgs e)
        {
            InitializeSelectors();
        }
#endregion 

        #region logic 
        private void CreateOrEditModel(object sender, EventArgs e)
        {

            /*
             check  here the data is correct. We have selected mats, pressures, etc.
             */

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

        private void storeModel(string name)
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();

            string materialModelName = materialSelector.Text;
            string pressureModelName = pressureSelector.Text;
            string bcModelName = boundarySelector.Text;
            string description = descriptionText.Text;

            gigaModel.AppendOrEditSimulationModel(name, materialModelName, pressureModelName, bcModelName, description);

        }

        private void ExportClicked(object sender, EventArgs e)
        {
            //copy and paste the code developed outside. 
            //Careful to finish the edge load and correct i-j bug 

        }
        #endregion

        private void exportButton_Click(object sender, EventArgs e)
        {
            //ProgressDialog dlg = new ProgressDialog();
            //dlg.Show();

            //we will export the one selected in the top selector. 
            var selectedName = newEditSelector1.SelectedName;

            var gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            if (gigaModel == null)
            {
                //this is a software problem. Cannot create the gigamodel. 
                return;
            }
            if (!gigaModel.SimulationsModel.FindModel(selectedName))
            { //the user needs to save the model first. 
                return; 
            }

            SimulationModelItem simObject = gigaModel.SimulationsModel.GetOrCreateModel( selectedName );

            //these are the three cubes. Each has an internal droid that is set when they are exported/modified/create. 
            //If we find that droid in the App folder, it is because the last action performed was a save and then  
            //we dont need to export them again. 
            //MaterialsModelItem mat    = gigaModel.MaterialModels.GetOrCreateModel( simObject.MaterialModelName);
            //PressureModelItem pres    = gigaModel.PressureModels.GetOrCreateModel(simObject.PressureModelName);
            //BoundaryConditionsItem bc = gigaModel.BoundaryConditionsModels.GetOrCreateModel(simObject.BoundaryConditionsModelName);




            if (VisageTools.WriteDeck(selectedName) != null)
            {
                MessageBox.Show(" Visage deck exported","Data Exported",MessageBoxButtons.OK,MessageBoxIcon.Information); 
            }

      
          
        }//export 
    }
}
