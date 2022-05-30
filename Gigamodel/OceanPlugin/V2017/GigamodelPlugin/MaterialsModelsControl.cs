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
using Gigamodel;
using Gigamodel.Data;
using Gigamodel.Services;
using Slb.Ocean.Core;

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
            this.masterSelector.DeleteClicked += (s, e) => 
            {
                if (!masterSelector.IsNewSelected)
                    DeleteModelEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));

            };
            
        }

        void ClearCubes()
        {
            _ymDragDrop.Value = null;
            _prDragDrop.Value = null;
            _densityDragDrop.Value = null;
        }

        public void DisplayModelItem(MaterialsModelItem model)
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
        private void EditSelectionChanged(object sender, EventArgs e)
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

        public bool IsSelectedAsNew { get { return masterSelector.IsNewSelected; }  set { masterSelector.IsNewSelected = true; } }

        private void NewClicked(object sender, EventArgs e) //clears the cubes 
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


                    //item.YoungsModulusDroid = ((SeismicCube)( _ymDragDrop.Value)).Droid.ToString();
                    //item.PoissonsRatioDroid = ((SeismicCube)( _prDragDrop.Value)).Droid.ToString();
                    //item.Density       = ((SeismicCube)( _densityDragDrop.Value)).Droid.ToString();

                }
                catch
                {
                    return null;
                }
            }
        }

        public void UpdateSelector(string[] names)
        {
            ////dont delete the combobox or chaneg where it points to, just update whats needed. 
            //Gigamodel gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            //if (gigaModel == null) return;

            ////the currently pointed name (if any) 
            string aux = masterSelector.SelectedName;
            masterSelector.ModelNames = names.ToList();

            if(names.Contains(aux))
            masterSelector.UpdateSelector(aux);


        }



        #endregion






    }


    //newer version, buggy 
    //public partial class MaterialsModelsControl : UserControl
    //{
    //    public event EventHandler DataDropAccepted;
    //    public event EventHandler DataDropRejected;
    //    public event EventHandler<StringEventArgs> EditSelectionChangedEvent;
    //    public event EventHandler<EventArgs> NewClickedOrUnClickedEvent;

    //    public MaterialsModelsControl()
    //    {
    //        InitializeComponent();
    //        ConnectEvents();

    //        List<Type> acceptedTypes = new List<Type>() { typeof(SeismicCube) };
    //        _ymDragDrop.AcceptedTypes = acceptedTypes;
    //        _prDragDrop.AcceptedTypes = acceptedTypes;
    //        _densityDragDrop.AcceptedTypes = acceptedTypes;

    //        ImageList images = new ImageList();
    //        images.Images.Add(PetrelImages.Seismic3D);
    //        _ymDragDrop.ImageList = images;
    //        _prDragDrop.ImageList = images;
    //        _densityDragDrop.ImageList = images;

    //        _ymDragDrop.ReferenceName = "YOUNGSMOD";
    //        _prDragDrop.ReferenceName = "POISSONR";
    //        _densityDragDrop.ReferenceName = "DENSITY";

    //        _ymDragDrop.ErrorImage = PetrelImages.Cancel;
    //        _prDragDrop.ErrorImage = PetrelImages.Cancel;
    //        _densityDragDrop.ErrorImage = PetrelImages.Cancel;

    //    }


    //    public List<string> ModelNames
    //    {
    //        get { return masterSelector.ModelNames; }
    //        set
    //        {
    //            masterSelector.ModelNames = value;
    //            masterSelector.IsNewSelected = false;
    //        } //doenst chnage selection if no new names added 
    //          //doest emit events if edit isnt selected
    //    }

    //    public CreateEditArgs UIToModel
    //    {
    //        get
    //        {

    //            try
    //            {
    //                MaterialsModelItem item = new MaterialsModelItem();
    //                item.Name = SelectedName;

    //                SeismicCube c = (SeismicCube)(_ymDragDrop.Value);
    //                item.YoungsModulus = new SeismicCubeFaccade("YOUNGSMOD", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

    //                c = (SeismicCube)(_prDragDrop.Value);
    //                item.PoissonsRatio = new SeismicCubeFaccade("POISSONR", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

    //                c = (SeismicCube)(_densityDragDrop.Value);
    //                item.Density = new SeismicCubeFaccade("DENSITY", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());

    //                item.TensileRatio = (float)(tensileStrengthRatio.Value);

    //                CreateEditArgs args = new CreateEditArgs();
    //                args.IsNew = IsSelectedAsNew;
    //                args.Object = item;
    //                args.Name = SelectedName;
    //                return args;


    //                //item.YoungsModulusDroid = ((SeismicCube)( _ymDragDrop.Value)).Droid.ToString();
    //                //item.PoissonsRatioDroid = ((SeismicCube)( _prDragDrop.Value)).Droid.ToString();
    //                //item.Density       = ((SeismicCube)( _densityDragDrop.Value)).Droid.ToString();

    //            }
    //            catch
    //            {
    //                return null;
    //            }
    //        }
    //    }

    //    public void ModelToUi(MaterialsModelItem model)
    //    {
    //        if (model != null)
    //        {
    //            masterSelector.AddIfMissingAndSelect(model.Name); //only changes selected index if the name is new 
    //            DisplayDetails(model);
    //        }
    //        else
    //        {
    //            masterSelector.IsNewSelected = true;
    //        }



    //        /*from older version 
    //        if (model != null)
    //        {
    //            if (SelectedName == model.Name) { DisplayDetails(model); }
    //            else {
    //                masterSelector.AddIfMissingAndSelect( model.Name );
    //            }
    //        }
    //        else
    //        {
    //            masterSelector.IsNewSelected = true;
    //        }
    //        */
    //    }

    //    private void DisplayDetails(MaterialsModelItem model)
    //    {
    //        try
    //        {
    //            _ymDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.YoungsModulus.DroidString)));
    //            _prDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.PoissonsRatio.DroidString)));
    //            _densityDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.Density.DroidString)));
    //        }
    //        catch
    //        {
    //            MessageService.ShowError("Unable to resolve data droids. Model corrupted");
    //            masterSelector.IsNewSelected = true;
    //        }
    //    }

    //    public void ClearControlDetails()
    //    {
    //        _ymDragDrop.Value = null;
    //        _prDragDrop.Value = null;
    //        _densityDragDrop.Value = null;
    //    }

    //    public bool IsSelectedAsNew { get { return masterSelector.IsNewSelected; } }

    //    public string SelectedName { get { return masterSelector.SelectedName; } }

    //    #region private 
    //    private void ConnectEvents()
    //    {
    //        this.masterSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
    //        this.masterSelector.NewClickedOrUnClicked += new System.EventHandler(this.NewClickedOrUnClicked);
    //    }

    //    private void datumDropped(object sender, EventArgs args)
    //    {

    //        DataDropAccepted?.Invoke(this, args);
    //    }

    //    private void datumRejected(object sender, EventArgs args)
    //    {
    //        MessageService.ShowError("Incorrect Data type");
    //        DataDropRejected?.Invoke(this, args);
    //    }

    //    private void EditSelectionChanged(object sender, EventArgs e)
    //    {
    //        EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
    //    }

    //    private void NewClickedOrUnClicked(object sender, EventArgs e) //clears the cubes 
    //    {
    //        ClearControlDetails();
    //        NewClickedOrUnClickedEvent?.Invoke(this, EventArgs.Empty);
    //    }

    //    #endregion
    //}





}//namespace 
