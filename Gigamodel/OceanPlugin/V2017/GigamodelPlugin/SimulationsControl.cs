using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gigamodel.Data;
using Slb.Ocean.Petrel.UI;

namespace Gigamodel
{
    public partial class SimulationsControl : UserControl
    {
        public event EventHandler<EventArgs> VisibilitychangedEvent;
        public event EventHandler<StringEventArgs> EditSelectionChangedEvent;
        public event EventHandler<EventArgs> NewClickedEvent;
        public event EventHandler<StringEventArgs> DeleteModelEvent;

        public event EventHandler<EventArgs> DirtyChanged;



        bool _dirty; 
        public bool Dirty
        {
            get {
                return _dirty;
            }
            set {
                _dirty = value;
                warning.Visible = _dirty; 
                DirtyChanged?.Invoke(this, EventArgs.Empty); 
            }
        }


        public SimulationsControl()
        {
            InitializeComponent();
            _dirty = true; 
            warning.Visible = _dirty;

            masterSelector.DeleteImage = PetrelImages.Close;

            modelImage.Image = PetrelImages.Property;//.Model;
            modelImage.BorderStyle = BorderStyle.None;
            pressureImage.Image = PetrelImages.Fluid;
            pressureImage.BorderStyle = BorderStyle.None;
            bConditionsImage.Image = PetrelImages.Boundary;
            bConditionsImage.BorderStyle = BorderStyle.None;
            comboBox1.SelectedIndex = 0;

            pictureBox1.Image = Properties.Resources.BlueCloud;//.


            ConnectEvents();
        }

        private void ConnectEvents()
        {
            this.masterSelector.DeleteClicked += (s, e) =>
            {
                if (!masterSelector.IsNewSelected)
                    DeleteModelEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));

            };

            this.VisibleChanged += new System.EventHandler(this.SimulationsControl_VisibleChanged);
            this.masterSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
            this.masterSelector.NewClicked += new System.EventHandler(this.NewClicked);
            this.descriptionText.TextChanged += new System.EventHandler(this.somethingChanged);

            this.materialSelector.SelectedIndexChanged += new System.EventHandler(this.somethingChanged);
            this.pressureSelector.SelectedIndexChanged += new System.EventHandler(this.somethingChanged);
            this.boundarySelector.SelectedIndexChanged += new System.EventHandler(this.somethingChanged);
            this.descriptionText.TextChanged += new System.EventHandler(this.somethingChanged);

        }


        private void SimulationsControl_VisibleChanged(object sender, EventArgs e)
        {

            VisibilitychangedEvent?.Invoke(this, EventArgs.Empty);

        }

        public void DisplayModelItem(SimulationModelItem model)
        {
            if (model != null)
            {
                masterSelector.UpdateSelector(model.Name); //only changes selected index if the name is new 
                try
                {
                    materialSelector.SelectedIndex = materialSelector.Items.IndexOf(model.MaterialModelName);
                    pressureSelector.SelectedIndex = pressureSelector.Items.IndexOf(model.PressureModelName);
                    boundarySelector.SelectedIndex = boundarySelector.Items.IndexOf(model.BoundaryConditionsModelName);

                    descriptionText.Text = model.Description;
                    Dirty = false;

                }
                catch
                {
                    Dirty = true;
                }

            }
            else {
                Dirty = true;
            }
        }

        public void UpdateSelector(string[] names)
        {
            string aux = masterSelector.SelectedName;
            masterSelector.ModelNames = names.ToList();

            if (names.Contains(aux))
                masterSelector.UpdateSelector(aux);
        }


        public void UpdateSelectors(List<string> mats, List<string> press, List<string> bcs)
        {

            materialSelector.Items.Clear();
            pressureSelector.Items.Clear();
            boundarySelector.Items.Clear();

            foreach (string s in mats) if (materialSelector.Items.IndexOf(s) < 0) materialSelector.Items.Add(s);
            foreach (string s in press) if (pressureSelector.Items.IndexOf(s) < 0) pressureSelector.Items.Add(s);
            foreach (string s in bcs) if (boundarySelector.Items.IndexOf(s) < 0) boundarySelector.Items.Add(s);

            foreach (string s in materialSelector.Items) if (mats.IndexOf(s) < 0) materialSelector.Items.Remove(s);
            foreach (string s in pressureSelector.Items) if (press.IndexOf(s) < 0) pressureSelector.Items.Remove(s);
            foreach (string s in boundarySelector.Items) if (bcs.IndexOf(s) < 0) boundarySelector.Items.Remove(s);



            if (masterSelector.IsNewSelected == true)
            {
                if (materialSelector.Items.Count > 0) materialSelector.SelectedIndex = 0;
                if (pressureSelector.Items.Count > 0) pressureSelector.SelectedIndex = 0;
                if (boundarySelector.Items.Count > 0) boundarySelector.SelectedIndex = 0;
            }
            else
            {
                EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
            }


        }

        private void EditSelectionChanged(object sender, EventArgs e)
        {
            EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));

        }

        public bool IsSelectedAsNew { get { return masterSelector.IsNewSelected; } set { masterSelector.IsNewSelected = true; } }

        void ClearCubes()
        {
        }

        private void NewClicked(object sender, EventArgs e) //clears the cubes 
        {
            if (IsSelectedAsNew)
            {
                ClearCubes();

                Dirty = true; 
                NewClickedEvent?.Invoke(this, EventArgs.Empty);
                

            }
            else
            {
                EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
               
            }
        }

        public string SelectedName { get { return masterSelector.SelectedName; } }


        public CreateEditArgs UIToModel
        {
            get
            {
                try
                {
                    CreateEditArgs args = new CreateEditArgs();
                    SimulationModelItem item = new SimulationModelItem();
                    item.Name = SelectedName;
                    item.MaterialModelName = materialSelector.SelectedItem.ToString();
                    item.PressureModelName = pressureSelector.SelectedItem.ToString();
                    item.BoundaryConditionsModelName = boundarySelector.SelectedItem.ToString();
                    item.Description = descriptionText.Text;

                    if (item.MaterialModelName == string.Empty) return null;
                    if (item.PressureModelName == string.Empty) return null;
                    if (item.BoundaryConditionsModelName == string.Empty) return null;



                    args.IsNew = IsSelectedAsNew;
                    args.Object = item;
                    args.Name = SelectedName;

                    return args;

                }
                catch
                {
                    return null;
                }
            }
        }



        private void somethingChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox1.SelectedIndex == 0)
            //{
            //    panel1.Enabled = false;
              
            //}
            //else
            //    panel1.Enabled = true;

            panel1.Enabled = comboBox1.SelectedIndex != 0;
            pictureBox1.Visible = comboBox1.SelectedIndex != 0;
        }
    }

}
       