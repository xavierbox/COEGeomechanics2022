using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Basics;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Property = Slb.Ocean.Petrel.DomainObject.PillarGrid.Property;

namespace COEGeomechanicsPlugin
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class FracSimulatorUI : UserControl
    {

        private FracSimulator process;
        private Property _filter = null;
        private PropertyCollection _collection = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="FracSimulatorUI"/> class.
        /// </summary>
        public FracSimulatorUI( FracSimulator process )
        {
            InitializeComponent();

            InitializeView(); 
            InitializeEvents();

            this.process = process;
        }

        private void InitializeView() 
        {
        }

        private void InitializeEvents()
        {

            filterDrop.DragDrop += ( s, e ) =>
            {
                FilterProperty = e.Data.GetData(typeof(Property)) as Property;
            };
            filterBox.KeyUp += ( s, e ) =>
            {
                if (e.KeyCode == Keys.Delete)
                    FilterProperty = null;
            };

            collectionDrop.DragDrop += ( s, e ) =>
            {
                PropertyCollection = e.Data.GetData(typeof(PropertyCollection)) as PropertyCollection;
            };
            collectionDrop.KeyUp += ( s, e ) =>
            {
                if (e.KeyCode == Keys.Delete)
                    PropertyCollection = null;
            };

        }




        PropertyCollection PropertyCollection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                collectionBox.Text = _filter == null ? string.Empty : _collection.Name;
                collectionBox.Image = _filter == null ? null : PetrelImages.PillarGrid;
            }
        }


        Property FilterProperty
        {
            get{return _filter;}
            set
            {
                _filter = value;
                filterBox.Text  = _filter == null ? string.Empty : _filter.Name;
                filterBox.Image = _filter == null ? null : PetrelImages.Property;
            }
        }

        private void button5_Click( object sender, EventArgs e )
        {

        }
    }
}
