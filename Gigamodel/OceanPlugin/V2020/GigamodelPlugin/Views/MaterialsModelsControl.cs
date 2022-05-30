using Gigamodel.Data;
using Gigamodel.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gigamodel
{
    public partial class MaterialsModelsControl : UserControl
    {
        public event EventHandler<StringEventArgs> EditSelectionChangedEvent;

        public event EventHandler<EventArgs> NewClickedEvent;

        public event EventHandler<StringEventArgs> DeleteModelEvent;

        public MaterialsModelsControl()
        {
            InitializeComponent();

            this.tensileStrengthRatio.Maximum = (decimal)1.0;
            this.tensileStrengthRatio.DecimalPlaces = 2;
            this.tensileStrengthRatio.Minimum = (decimal)0.05;
            this.tensileStrengthRatio.Value = (decimal)0.1;
            this.tensileStrengthRatio.Increment = (decimal)0.1;

            masterSelector.DeleteImage = PetrelImages.Close;

            InitializeCustomDragDrops();
            ConnectEvents();
            //UpdateSelector();
        }

        #region UI stuff

        private void ConnectEvents()
        {
            this.masterSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
            this.masterSelector.NewClicked += new System.EventHandler(this.NewClicked);
            this.masterSelector.DeleteClicked += ( s, e ) =>
            {
                if (!masterSelector.IsNewSelected)
                    DeleteModelEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
            };
        }

        private void ClearCubes()
        {
            _ymDragDrop.Value = null;
            _prDragDrop.Value = null;
            _densityDragDrop.Value = null;
        }

        public void DisplayModelItem( MaterialsModelItem model )
        {
            if (model != null)
            {
                masterSelector.UpdateSelector(model.Name); //only changes selected index if the name is new
                try
                {
                    _ymDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.YoungsModulus.DroidString)));
                    _prDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.PoissonsRatio.DroidString)));
                    _densityDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.Density.DroidString)));

                    tensileStrengthRatio.Value = (decimal)(model.TensileRatio);
                }
                catch
                {
                    MessageService.ShowError("Unable to resolve data droids. Model corrupted");
                    masterSelector.IsNewSelected = true;
                }
            }
            else
            {
                ClearCubes();
            }
        }

        //when the item in the combobox changed or a change from new/edit was made.
        private void EditSelectionChanged( object sender, EventArgs e )
        {
            EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
        }

        private void InitializeCustomDragDrops()
        {
            List<Type> acceptedTypes = new List<Type>() { typeof(SeismicCube) };
            _ymDragDrop.AcceptedTypes = acceptedTypes;
            _prDragDrop.AcceptedTypes = acceptedTypes;
            _densityDragDrop.AcceptedTypes = acceptedTypes;

            ImageList images = new ImageList();
            images.Images.Add(PetrelImages.Seismic3D);
            _ymDragDrop.ImageList = images;
            _prDragDrop.ImageList = images;
            _densityDragDrop.ImageList = images;

            _ymDragDrop.ReferenceName = "YOUNGSMOD";
            _prDragDrop.ReferenceName = "POISSONR";
            _densityDragDrop.ReferenceName = "DENSITY";

            _ymDragDrop.PlaceHolder = "Please drop a realized seismic cube for the Youngs Modulus";
            _prDragDrop.PlaceHolder = "Please drop a realized seismic cube for the Poisson's ratio";
            _densityDragDrop.PlaceHolder = "Please drop a realized seismic cube for the Density";

            _ymDragDrop.ErrorImage = PetrelImages.Cancel;
            _prDragDrop.ErrorImage = PetrelImages.Cancel;
            _densityDragDrop.ErrorImage = PetrelImages.Cancel;
        }

        public bool IsSelectedAsNew { get { return masterSelector.IsNewSelected; } set { masterSelector.IsNewSelected = true; } }

        private void NewClicked( object sender, EventArgs e ) //clears the cubes
        {
            if (IsSelectedAsNew)
            {
                ClearCubes();

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
                    MaterialsModelItem item = new MaterialsModelItem();
                    item.Name = SelectedName;

                    SeismicCube c = (SeismicCube)(_ymDragDrop.Value);
                    item.YoungsModulus = new SeismicCubeFaccade("YOUNGSMOD", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

                    c = (SeismicCube)(_prDragDrop.Value);
                    item.PoissonsRatio = new SeismicCubeFaccade("POISSONR", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

                    c = (SeismicCube)(_densityDragDrop.Value);
                    item.Density = new SeismicCubeFaccade("DENSITY", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

                    item.TensileRatio = (float)(tensileStrengthRatio.Value);

                    CreateEditArgs args = new CreateEditArgs();
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

        public void UpdateSelector( string[] names )
        {
            ////the currently pointed name (if any)
            string aux = masterSelector.SelectedName;
            masterSelector.ModelNames = names.ToList();

            if (names.Contains(aux))
                masterSelector.UpdateSelector(aux);
        }

        #endregion UI stuff
    }
}//namespace