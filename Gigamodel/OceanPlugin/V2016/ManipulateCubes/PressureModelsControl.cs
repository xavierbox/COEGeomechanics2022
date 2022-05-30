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

namespace ManipulateCubes
{
    public partial class PressureModelsControl : UserControl
    {
        DateTimePicker oDateTimePicker;
        int DATECOLUMNINGRID = 2;

        public PressureModelsControl()
        {
            InitializeComponent();


            oDateTimePicker = new DateTimePicker();
            oDateTimePicker.Visible = false;
            oDateTimePicker.Format = DateTimePickerFormat.Long;// Short;
            coupledPressuresGrid.Controls.Add(oDateTimePicker);

            ConnectEvents();

            updateSelector();

            newButton.Checked = selector.Items.Count <= 0 ? true : false;
            this.editButton.Checked = selector.Items.Count <= 0 ? false : true;

            initialPressureDragDrop.PlaceHolder = "Drop a realized pressure cube.";
            initialPressureDragDrop.PropertyName = "Initial Pressure";

        }

        #region UI stuff

        public ImageList ImagesList
        {
            get { if (imageList1 == null) imageList1 = new ImageList(); return imageList1; }
            set { imageList1 = value; }
        }

        private void ConnectEvents()
        {
            editButton.CheckedChanged += new System.EventHandler(this.checkedChanged);
            newButton.CheckedChanged += new System.EventHandler(this.checkedChanged);
            selector.SelectedIndexChanged += new System.EventHandler(this.selectionChanged);

            // An event attached to dateTimePicker Control which is fired when DateTimeControl is closed  
            oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
            oDateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);

            dropPressureCollection.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropPressuresCollection);
        }
        public void UpdateControl(object sender, EventArgs e)
        {

        }


        string[] getComboBoxNames(System.Windows.Forms.ComboBox c)
        {
            List<string> l = new List<string>();
            foreach (object x in c.Items) l.Add(x.ToString());
            return l.ToArray();
        }
        private void selectionChanged(object sender, EventArgs e)
        {
            //get all referent to the model and display it in the UI. Here, we only have like 3 seismic cubes so it should be simple.
            ModelSelectedToUi();
        }

        private void updateSelector()
        {
            //dont delete the combobox or chaneg where it points to, just update whats needed. 
            int index = -1;

            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            if (gigaModel == null) return;

            foreach (string s in gigaModel.PressureModels.ModelNames)
            {
                if (selector.Items.Contains(s)) {; }
                else
                {
                    selector.Items.Add(s);
                    index = selector.Items.Count - 1;
                }

            }
            if (index >= 0)
                selector.SelectedIndex = index; // = materialsNameTextBox.Text;

        }

        private void checkedChanged(object sender, EventArgs e)
        {
            newPressureName.Visible = newButton.Checked ? true : false;
            newPressureName.Text = string.Empty;
            selector.Visible = !(newPressureName.Visible);
            updateSelector();

            if (newButton.Checked)
            {
                initialPressureDragDrop.Value = null;//.ClearComponent();//   initialPressurePresentationBox.ClearComponent();
                coupledPressuresGrid.Tag = null;
                coupledPressuresGrid.Rows.Clear();
            }
            else
            {
            }
        }


        void oDateTimePicker_CloseUp(object osender, EventArgs e)
        {
            DateTimePicker sender = (DateTimePicker)(osender);
            // Hiding the control after use   
            sender.Visible = false;
        }

        private void dateTimePicker_OnTextChange(object osender, EventArgs e)
        {
            // Saving the 'Selected Date on Calendar' into DataGridView current cell  
            //modify the view 
            DateTimePicker sender = (DateTimePicker)(osender);
            coupledPressuresGrid.CurrentCell.Value = sender.Text.ToString();
            DataGridViewCell cell = coupledPressuresGrid.CurrentCell;

            //modifyt the stored data in the list 
            int rowIndex = coupledPressuresGrid.CurrentCell.RowIndex;
            List<KeyValuePair<SeismicCube, DateTime>> storedValues = (List<KeyValuePair<SeismicCube, DateTime>>)(coupledPressuresGrid.Tag);
            SeismicCube storedcube = storedValues[rowIndex].Key;
            storedValues[rowIndex] = new KeyValuePair<SeismicCube, DateTime>(storedcube, DateTime.Parse(sender.Text.ToString()));

            //this line shouldnt be needed. 
            coupledPressuresGrid.Tag = storedValues;//
        }

        private void coupledPressuresDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DATECOLUMNINGRID)
            {
                // It returns the retangular area that represents the Display area for a cell  
                Rectangle oRectangle = coupledPressuresGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                //Setting area for DateTimePicker Control  
                oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
                // Setting Location  
                oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
                // Now make it visible  
                oDateTimePicker.Visible = true;
            }

        }

        private void dropPressuresCollection(object sender, DragEventArgs e)
        {
            ////dropped a single cube or a cube collection, if a collection, clear the grid.  
            //we will store this list as the table Tag. 
            List<KeyValuePair<SeismicCube, DateTime>> list = new List<KeyValuePair<SeismicCube, DateTime>>();

            SeismicCollection cubes = e.Data.GetData(typeof(SeismicCollection)) as SeismicCollection;
            if (cubes != null)  //it is a collection 
            {
                //this is the initial and its date 
                SeismicCube initialPressure = (SeismicCube)(initialPressureDragDrop.Value);
                DateTime initialTime = this.initialPressureDate.Value;

                int count = 1;
                foreach (SeismicCube cube in cubes.SeismicCubes)
                {
                    if (cube != initialPressure)
                    {
                        DateTime t = initialTime.AddYears(count++);
                        coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, cube.Name, t.ToString());
                        list.Add(new KeyValuePair<SeismicCube, DateTime>(cube, t));

                    }
                }

                //store the collection as a list of cubes 
                coupledPressuresGrid.Tag = list;
                pressureCollectionCheckbox.Checked = true;

                return;

            }

            SeismicCube singlecube = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;

            if (singlecube != null)
            {
                //this is the initial and its date 
                SeismicCube initialPressure = (SeismicCube)(initialPressureDragDrop.Value);      /////////////////////////
                DateTime initialTime = this.initialPressureDate.Value;

                if (singlecube != initialPressure)
                {
                    DateTime t = initialTime.AddYears(coupledPressuresGrid.Rows.Count + 1);
                    this.coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, singlecube.Name, t.ToString());
                    list.Add(new KeyValuePair<SeismicCube, DateTime>(singlecube, t));
                    coupledPressuresGrid.Tag = list;
                }
                else
                {
                    MessageBox.Show("Please drop a pressure cube different from the initial pressure one", "Wrong Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                pressureCollectionCheckbox.Checked = true;
            }
            else
            {
                MessageBox.Show("Please drop a valid seismic cube", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pressureCollectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            coupledPressuresGrid.Enabled = pressureCollectionCheckbox.Checked;
        }


        #endregion

        #region logic 

        void ModelToUi(PressureModelItem model)
        {
            this.offsetTrackBar.Value = UIPressureToTrackbar(model.Offset);
            this.gradientTrackBar.Value = (int)(model.Gradient);

            List<KeyValuePair<Droid, DateTime>> datedcubes = model.DroidDates;
            initialPressureDate.Value = (datedcubes[0].Value);
            SeismicCube initialPressure = (SeismicCube)(DataManager.Resolve(datedcubes[0].Key));
            initialPressureDragDrop.Value = initialPressure;

            List<KeyValuePair<SeismicCube, DateTime>> list = new List<KeyValuePair<SeismicCube, DateTime>>();
            coupledPressuresGrid.Rows.Clear();
            for (int n = 1; n < datedcubes.Count(); n++)
            {
                DateTime t = datedcubes[n].Value;
                SeismicCube pressure = (SeismicCube)(DataManager.Resolve(datedcubes[n].Key));
                coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, pressure.Name, t.ToString());
                list.Add(new KeyValuePair<SeismicCube, DateTime>(pressure, t));
            }

            coupledPressuresGrid.Tag = list;
        }

        void ModelSelectedToUi()
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();

            string currentModelSelected = selector.SelectedItem.ToString();

            if (gigaModel.PressureModels.FindModel(currentModelSelected))
                ModelToUi(gigaModel.PressureModels.GetOrCreateModel(currentModelSelected));
        }



        //float PressureScaling
        //{
        //    get { return 1.0f; }
        //}

        float DepthScaling
        {
            get { return 1.0f; }
        }

        //if the trackbar is 1234 then the pressure s 1.234, a factor of 1000 ius applied 
        int UIPressureToTrackbar(float uiPressure)
        {
            return (int)(1000 * uiPressure);
        }

        float TrackbarToUIPressure(int trackbarValue)
        {
            return trackbarValue / 1000.00f;
        }


        private void CreateOrEditModel(object sender, EventArgs e)
        {

            var value = initialPressureDragDrop.Value;
            if ((SeismicCube)(value) == null)
            {
                MessageBox.Show("Please select all the property cubes", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newButton.Checked)
            {
                string[] names = getComboBoxNames(selector);
                if (newPressureName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please provide a valid model name. Thats why we put the text box there!", "Incomplete data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (names.Contains(newPressureName.Text.Trim()))
                {
                    MessageBox.Show("The model name is already in use. Either modify the pre-existing model or pick a new name", "Wron input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    storeModel(newPressureName.Text.Trim());
                    updateSelector();
                    selector.SelectedIndex = selector.Items.Count - 1;
                    newButton.Checked = false;
                    editButton.Checked = true;
                    MessageBox.Show("Model  " + newPressureName.Text + " was Successfully Created", "Model Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }//new model 
            else
            {
                storeModel(selector.SelectedItem.ToString());
                updateSelector();
                MessageBox.Show("Model " + selector.SelectedItem.ToString() + " was Edited. ", "Model Modified", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }


        private bool storeModel(string name)
        {
            Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            float offset = TrackbarToUIPressure(offsetTrackBar.Value);
            float gradient = gradientTrackBar.Value;

            SeismicCube cube = (SeismicCube)(initialPressureDragDrop.Value);                      //////////////////////
            List<KeyValuePair<Droid, DateTime>> datedDroids = new List<KeyValuePair<Droid, DateTime>>();
            datedDroids.Add(new KeyValuePair<Droid, DateTime>(cube.Droid, initialPressureDate.Value));

            List<KeyValuePair<SeismicCube, DateTime>> cubes = (List<KeyValuePair<SeismicCube, DateTime>>)(coupledPressuresGrid.Tag);
            if (cubes != null)
            {
                for (int i = 0; i < cubes.Count(); i++)
                {
                    SeismicCube s = cubes[i].Key;
                    DateTime t = DateTime.Parse(coupledPressuresGrid.Rows[i].Cells[DATECOLUMNINGRID].Value.ToString());
                    datedDroids.Add(new KeyValuePair<Droid, DateTime>(s.Droid, t));
                }
            }
            gigaModel.AppendOrEditPressureModel(name, datedDroids, gradient, offset, /*PressureScaling,*/ DepthScaling);
            return true;
        }

        #endregion

    }
}


