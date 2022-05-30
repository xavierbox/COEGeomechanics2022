using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Core;
using System.IO;
using System.Diagnostics;

namespace ManipulateCubes
{
    public partial class MaterialsModelsControl : UserControl
    {
        public event EventHandler< StringEventArgs> EditSelectionChangedEvent;
        public event EventHandler<EventArgs> NewClickedEvent;

        public MaterialsModelsControl()
        {
            InitializeComponent();
            ConnectEvents();
            UpdateSelector();
        }


        #region UI stuff
        private void ConnectEvents()
        {
            this.newEditSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
            this.newEditSelector.NewClicked       += new System.EventHandler(this.NewClicked);
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
                EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(newEditSelector.SelectedName));
            }    
        }


        //when the item in the combobox changed or a change from new/edit was made. 
        private void EditSelectionChanged(object sender, EventArgs e)
        {
            EditSelectionChangedEvent?.Invoke( this, new StringEventArgs(newEditSelector.SelectedName ));
        }


        public void DisplayModelItem(MaterialsModelItem model)
        {
            if (model != null)
            {
                newEditSelector.UpdateSelector(model.Name); //only changes selected index if the name is new 
                ymDragDrop.Value = model.YoungsModulus;
                prDragDrop.Value = model.PoissonsRatio;
                densDragDrop.Value = model.Density;
            }
            else
            {
                ClearCubes();
            }
        }

        void ClearCubes()
        {
            ymDragDrop.ClearComponent();
            prDragDrop.ClearComponent();
            densDragDrop.ClearComponent();
        }


 
        //private void ModelSelectedToUi()
        //{
        //    Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
        //    string currentModelSelected = newEditSelector.SelectedName;
        //    if (gigaModel.MaterialModels.FindModel(currentModelSelected))
        //    {
        //        DisplayModelItem(gigaModel.MaterialModels.GetOrCreateModel(currentModelSelected));
        //    }
        //}


        ////updating the control is basically just updating the selector on top
        //public void UpdateControl(object sender, EventArgs e)
        //{
        //    UpdateSelector();         
        //}
 
         
        public void UpdateSelector()
        {
            //dont delete the combobox or chaneg where it points to, just update whats needed. 
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            if (gigaModel == null) return;

            //the currently pointed name (if any) 
            string aux = newEditSelector.SelectedName;
            newEditSelector.ModelNames = gigaModel.MaterialModels.ModelNames;
            newEditSelector.UpdateSelector(aux);
            
        }

        //ImageList _imageList1;

        //public ImageList ImagesList
        //{
        //    get { if (_imageList1 == null) _imageList1 = new ImageList(); return _imageList1; }
        //    set { _imageList1 = value; }
        //}
 
      
        public bool IsDataSelectedOk(out string mesg)
        {

            mesg = string.Empty;
            if (((SeismicCube)(ymDragDrop.Value) == null) || ((SeismicCube)(prDragDrop.Value) == null) || ((SeismicCube)(densDragDrop.Value) == null))
            {
                mesg = "Missing input data. All seismic cubes must be selected";
                return false;
            }
            else if ((SelectedName == string.Empty) || (SelectedName == null))
            {
                mesg = "Need to provide a name for the new material model";
                return false;
            }
            else if ((IsSelectedAsNew) && (newEditSelector.ModelNames.Contains(newEditSelector.SelectedName)))
            {
                mesg = "The selected name for the new model already exists";
                return false;
            }
            else
            {
            }
            return true;
        }

        public bool IsSelectedAsNew { get { return newEditSelector.IsNewSelected; } }

        public string SelectedName { get { return newEditSelector.SelectedName; } }

        #endregion

        public MaterialSelection GetSelection()
        {
            return new MaterialSelection(
                  newEditSelector.SelectedName,
                  new List<KeyValuePair<Droid, string>>()
                   {
                    new KeyValuePair<Droid,string>( ((SeismicCube)(ymDragDrop.Value)).Droid,   "YOUNGSMOD" ),
                    new KeyValuePair<Droid,string>( ((SeismicCube)(prDragDrop.Value)).Droid,   "POISSONR" ),
                    new KeyValuePair<Droid,string>( ((SeismicCube)(densDragDrop.Value )).Droid, "DENSITY" )
                   });
        }




        /*
         *   private string[] ValidateSelection(bool IsNewSelected, string selectedName, string[] previousModelNames)
        {
            if (IsNewSelected)
            {

                if (selectedName == string.Empty)
                {
                    return new string[] { "Please provide a valid model name. Thats why we put the text box there!", "Incomplete data" };
                }
                if (previousModelNames.Contains(selectedName))
                {
                    return new string[] { "The model name is already in use. Either modify the pre-existing model or pick a new name", "Wrong input" };
                }
            }//new model

            return null;
        }

        private void CreateOrEditModel(object sender, EventArgs e)
        {
            if (((SeismicCube)(ymDragDrop.Value) == null) || ((SeismicCube)(prDragDrop.Value) == null) || ((SeismicCube)(densDragDrop.Value) == null))
            {
                MessageBox.Show("Please select all the property cubes", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isNew = newEditSelector1.IsNewSelected;
            string selectedName = newEditSelector1.SelectedName;
            string[] msg = ValidateSelection(isNew, selectedName, newEditSelector1.ModelNames);
            if (msg != null)
            {
                MessageBox.Show(msg[0], msg[1], MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                storeModel(selectedName);
                newEditSelector1.UpdateSelector(selectedName);
                MessageBox.Show("Model  " + selectedName + (isNew ? "was Successfully Created" : " was Edited. "), (isNew ? "Model Created" : "Model Modified"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       
        private bool storeModel(string name)
        {
            OceanUtilities.GetExistingOrCreateGigamodel().AppendOrEditMaterialsModel
               (
               string.Copy(name),
               new List<KeyValuePair<Droid, string>>()
                   {
                    new KeyValuePair<Droid,string>( ((SeismicCube)(ymDragDrop.Value)).Droid,   "YOUNGSMOD" ),
                    new KeyValuePair<Droid,string>( ((SeismicCube)(prDragDrop.Value)).Droid,   "POISSONR" ),
                    new KeyValuePair<Droid,string>( ((SeismicCube)(densDragDrop.Value )).Droid, "DENSITY" )
                   }
               );
            return true;
        }
        */

        


    }
}
