using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CO2
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class CO2ProcessUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private CO2Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="CO2ProcessUI"/> class.
        /// </summary>
        public CO2ProcessUI(CO2Process process)
        {
            InitializeComponent();
            InitializeCustomDragDrops();

            ConnectEvents();

            this.process = process;
        }


        private void InitializeCustomDragDrops()
        {
            List<Type> acceptedTypes = new List<Type>() { typeof(SeismicCube) };
            _capillaryDragDrop.AcceptedTypes = acceptedTypes;
            _porosityDragDrop.AcceptedTypes = acceptedTypes;
        
            ImageList images = new ImageList();
            images.Images.Add(PetrelImages.Seismic3D);

            _capillaryDragDrop.ImageList = images;
            _porosityDragDrop.ImageList = images;

            _capillaryDragDrop.ReferenceName = "CAPILLARY";
            _porosityDragDrop.ReferenceName = "POROSITY";

            _capillaryDragDrop.PlaceHolder = "Please drop a realized seismic cube for Capllary Pressure";
            _porosityDragDrop.PlaceHolder = "Please drop a realized seismic cube for Porosity";

            _capillaryDragDrop.ErrorImage = PetrelImages.Cancel;
            _porosityDragDrop.ErrorImage = PetrelImages.Cancel;

            _capillaryDragDrop.StyleControl();
            _porosityDragDrop.StyleControl();

            ImageList images2 = new ImageList();
            images2.Images.Add(PetrelImages.WellFolder);
            images2.Images.Add(PetrelImages.PointSet);
            List<Type> acceptedTypes2 = new List<Type>() { typeof(BoreholeCollection), typeof(PointSet) };
            _injectorsDragDrop.PlaceHolder = "Please drop a well folder or a point set";
            _porosityDragDrop.ReferenceName = "INJECTORS";
            _injectorsDragDrop.StyleControl();
            _capillaryDragDrop.ImageList = images2;
        }


        private void ConnectEvents() 
        {
            this.CancelButton.Click += (sender, evt) => {
                this.Parent.Dispose();
            }; 
            
        }

        private void ClearCubes()
        {
            _capillaryDragDrop.Value = null;
            _porosityDragDrop.Value = null;
           
        }

        private void CO2ProcessUI_Load(object sender, EventArgs e)
        {

        }
    }
}
