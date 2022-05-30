using Gigamodel.Data;
using Gigamodel.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
/*
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gigamodel
{
    public partial class PressureModelsControl : UserControl
    {
        public event EventHandler<StringEventArgs> EditSelectionChangedEvent;

        public event EventHandler<EventArgs> NewClickedEvent;

        public event EventHandler<StringEventArgs> DeleteModelEvent;

        private DateTimePicker _oDateTimePicker;
        private int DATECOLUMNINGRID = 2;

        public PressureModelsControl()
        {
            InitializeComponent();
            _coupledPressuresGrid.Visible = true;
            InitializeTimeDateEditor();
            InitializeCustomDragDrops();
            ConnectEvents();
            masterSelector.DeleteImage = PetrelImages.Close;
        }

        private void ConnectEvents()
        {
            this.masterSelector.DeleteClicked += ( s, e ) =>
            {
                if (!masterSelector.IsNewSelected)
                    DeleteModelEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
            };

            this.masterSelector.SelectionChanged += new System.EventHandler(this.EditSelectionChanged);
            this.masterSelector.NewClicked += new System.EventHandler(this.NewClicked);

            this._coupledPressuresGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.coupledPressuresDoubleClick);
            dropPressureCollection.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropPressuresCollection);

            // An event attached to dateTimePicker Control which is fired when DateTimeControl is closed
            _oDateTimePicker.CloseUp += ( s, e ) =>
            {
                DateTimePicker sender = (DateTimePicker)(s);
                sender.Visible = false;
            };

            _oDateTimePicker.TextChanged += ( s, e ) =>
            {
                // Saving the 'Selected Date on Calendar' into DataGridView current cell  modify the view
                DateTimePicker sender = (DateTimePicker)(s);
                _coupledPressuresGrid.CurrentCell.Value = sender.Text.ToString();
                DataGridViewCell cell = _coupledPressuresGrid.CurrentCell;

                //modifyt the stored data in the list
                int rowIndex = _coupledPressuresGrid.CurrentCell.RowIndex;
                List<KeyValuePair<SeismicCube, DateTime>> storedValues = (List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag);
                SeismicCube storedcube = storedValues[rowIndex].Key;
                storedValues[rowIndex] = new KeyValuePair<SeismicCube, DateTime>(storedcube, DateTime.Parse(sender.Text.ToString()));
                //this line shouldnt be needed.
                _coupledPressuresGrid.Tag = storedValues;//
            };
        }

        private void ClearCubes()
        {
            _initialPressureDragDrop.Value = null;
            _coupledPressuresGrid.Rows.Clear();
            _coupledPressuresGrid.Tag = null;
        }

        public void DisplayModelItem( PressureModelItem model )
        {
            if (model != null)
            {
                masterSelector.UpdateSelector(model.Name); //only changes selected index if the name is new
                try
                {
                    _initialPressureDate.Value = model.Dates[0];
                    _initialPressureDragDrop.Value = (SeismicCube)(DataManager.Resolve(new Droid(model.InitialPressure.DroidString)));

                    List<KeyValuePair<SeismicCube, DateTime>> datedCubes = new List<KeyValuePair<SeismicCube, DateTime>>();

                    for (int n = 1; n < model.DatedDroids.Count(); n++)
                    {
                        SeismicCube s = (SeismicCube)(DataManager.Resolve(new Droid(model.DatedDroids[n].Key.DroidString)));
                        datedCubes.Add(new KeyValuePair<SeismicCube, DateTime>(s, model.DatedDroids[n].Value));
                    }

                    populateCoupledPressures(datedCubes);
                    gradientTrackBar.Value = (int)(model.Gradient);
                    offsetTrackBar.Value = (int)(model.Offset);
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

        private void populateCoupledPressures( List<KeyValuePair<SeismicCube, DateTime>> datedCubes )
        {
            _coupledPressuresGrid.Rows.Clear();
            _coupledPressuresGrid.Tag = null;
            for (int n = 0; n < datedCubes.Count(); n++)
            {
                _coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, datedCubes[n].Key.Name, datedCubes[n].Value.ToString());
            }
            _coupledPressuresGrid.Tag = datedCubes;

            richTextBox1.Visible = false;
            _coupledPressuresGrid.Visible = true;
        }

        private void coupledPressuresDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            if (e.ColumnIndex == DATECOLUMNINGRID)
            {
                // It returns the retangular area that represents the Display area for a cell
                Rectangle oRectangle = _coupledPressuresGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                //Setting area for DateTimePicker Control
                _oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
                // Setting Location
                _oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
                // Now make it visible
                _oDateTimePicker.Visible = true;
            }
        }

        private void dropPressuresCollection( object sender, DragEventArgs e )
        {
            ////dropped a single cube or a cube collection, if a collection, clear the grid.
            //we will store this list as the table Tag.
            List<KeyValuePair<SeismicCube, DateTime>> list = new List<KeyValuePair<SeismicCube, DateTime>>();

            SeismicCollection cubes = e.Data.GetData(typeof(SeismicCollection)) as SeismicCollection;
            if (cubes != null)  //it is a collection
            {
                //this is the initial and its date
                SeismicCube initialPressure = (SeismicCube)(_initialPressureDragDrop.Value);
                DateTime initialTime = this._initialPressureDate.Value;

                int count = 1;
                foreach (SeismicCube cube in cubes.SeismicCubes)
                {
                    if (cube != initialPressure)
                    {
                        DateTime t = initialTime.AddYears(count++);
                        //_coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, cube.Name, t.ToString());
                        list.Add(new KeyValuePair<SeismicCube, DateTime>(cube, t));
                    }
                }

                //store the collection as a list of cubes
                //_coupledPressuresGrid.Tag = list;
                pressureCollectionCheckbox.Checked = true;
                populateCoupledPressures(list);
                return;
            }

            SeismicCube singlecube = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;

            if (singlecube != null)
            {
                //this is the initial and its date
                SeismicCube initialPressure = (SeismicCube)(_initialPressureDragDrop.Value);      /////////////////////////
                DateTime initialTime = this._initialPressureDate.Value;

                if ((List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag) != null)
                {
                    list = (List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag);
                }

                if (singlecube != initialPressure)
                {
                    DateTime t = initialTime.AddYears(_coupledPressuresGrid.Rows.Count + 1);
                    //this._coupledPressuresGrid.Rows.Add(PetrelImages.Seismic3D, singlecube.Name, t.ToString());
                    list.Add(new KeyValuePair<SeismicCube, DateTime>(singlecube, t));
                    //_coupledPressuresGrid.Tag = list;
                }
                else
                {
                    MessageBox.Show("Please drop a pressure cube different from the initial pressure one", "Wrong Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                pressureCollectionCheckbox.Checked = true;
                populateCoupledPressures(list);
            }
            else
            {
                MessageBox.Show("Please drop a valid seismic cube", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //when the item in the combobox changed or a change from new/edit was made.
        private void EditSelectionChanged( object sender, EventArgs e )
        {
            EditSelectionChangedEvent?.Invoke(this, new StringEventArgs(masterSelector.SelectedName));
        }

        private void InitializeTimeDateEditor()
        {
            _oDateTimePicker = new DateTimePicker();
            _oDateTimePicker.Visible = false;
            _oDateTimePicker.Format = DateTimePickerFormat.Long;// Short;
            _coupledPressuresGrid.Controls.Add(_oDateTimePicker);
        }

        private void InitializeCustomDragDrops()
        {
            _initialPressureDragDrop.AcceptedTypes = new List<Type>() { typeof(SeismicCube) };
            ImageList images = new ImageList();
            images.Images.Add(PetrelImages.Seismic3D);

            _initialPressureDragDrop.ImageList = images;
            _initialPressureDragDrop.ReferenceName = "Initial Pressure";
            _initialPressureDragDrop.ErrorImage = PetrelImages.Cancel;
            _initialPressureDragDrop.PlaceHolder = "Please drop a realized seismic pressure cube";
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

        //returns a CreateEditArgs. This is the same object returned by all the tabs. Internally it has a reference to an object.
        //the object in this tab is a PressureItem. Also has a name and a flag indicating whether the user wanted to create or edit the model.
        public CreateEditArgs UIToModel
        {
            get
            {
                try
                {
                    PressureModelItem item = new PressureModelItem();
                    item.Name = SelectedName;

                    //initial time step
                    SeismicCube initialPressure = (SeismicCube)(_initialPressureDragDrop.Value);
                    SeismicCubeFaccade faccade = new SeismicCubeFaccade("PRESSURE", "xx", new int[] { initialPressure.NumSamplesIJK.I, initialPressure.NumSamplesIJK.J, initialPressure.NumSamplesIJK.K }, initialPressure.Droid.ToString());
                    item.DatedDroids.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(faccade, _initialPressureDate.Value));

                    //coupled steps, if any
                    List<KeyValuePair<SeismicCube, DateTime>> datedCubes = (List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag);
                    if ((pressureCollectionCheckbox.Checked) && (datedCubes != null))
                    {
                        for (int i = 0; i < datedCubes.Count(); i++)
                        {
                            SeismicCube c = datedCubes[i].Key;
                            DateTime t = datedCubes[i].Value;
                            faccade = new SeismicCubeFaccade("PRESSURE", "xx", new int[] { c.NumSamplesIJK.I, c.NumSamplesIJK.J, c.NumSamplesIJK.K }, c.Droid.ToString());
                            item.DatedDroids.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(faccade, t));
                        }
                    }

                    //others
                    item.Gradient = gradientTrackBar.Value;
                    item.Offset = offsetTrackBar.Value;

                    CreateEditArgs args = new CreateEditArgs();
                    args.Name = SelectedName;
                    args.Object = item;
                    args.IsNew = IsSelectedAsNew;

                    return args;
                }
                catch
                {
                    return null;
                }
            }
        }//

        public void UpdateSelector( string[] names )
        {
            string aux = masterSelector.SelectedName;
            masterSelector.ModelNames = names.ToList();

            if (names.Contains(aux))
                masterSelector.UpdateSelector(aux);
        }

        private void movePressureUp( object sender, EventArgs e )
        {
            movePressureUpDown(true);
        }

        private void movePressureUpDown( bool up )
        {
            List<KeyValuePair<SeismicCube, DateTime>> data = (List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag);
            if (data == null) return;
            var rowsSelected = _coupledPressuresGrid.SelectedRows;
            if ((rowsSelected != null) && (rowsSelected.Count > 0))
            {
                int rowIndex = _coupledPressuresGrid.SelectedRows[0].Index;
                KeyValuePair<SeismicCube, DateTime> item = data[rowIndex];

                bool condition1 = (up) && (rowIndex > 0);
                bool condition2 = (!up) && (rowIndex < _coupledPressuresGrid.Rows.Count - 1);

                if ((condition1) || (condition2))
                {
                    int indexChange = up == true ? -1 : 1;
                    data.RemoveAt(rowIndex);
                    data.Insert(rowIndex + indexChange, item);
                    // _coupledPressuresGrid.Tag = data;
                    populateCoupledPressures(data);
                    _coupledPressuresGrid.Rows[rowIndex + indexChange].Selected = true;
                }
            }
        }

        private void movePressureDown( object sender, EventArgs e )
        {
            movePressureUpDown(false);
        }

        private void deletePressure( object sender, EventArgs e )
        {
            List<KeyValuePair<SeismicCube, DateTime>> data = (List<KeyValuePair<SeismicCube, DateTime>>)(_coupledPressuresGrid.Tag);
            if (data == null) return;

            var rowsSelected = _coupledPressuresGrid.SelectedRows;
            if ((rowsSelected != null) && (rowsSelected.Count > 0))
            {
                var rowIndex = _coupledPressuresGrid.SelectedRows[0].Index;

                data.RemoveAt(rowIndex);
                _coupledPressuresGrid.Rows.RemoveAt(rowIndex);
                //populateCoupledPressures( data );
            }
        }

        private void pressureCollectionCheckbox_CheckedChanged( object sender, EventArgs e )
        {
            _coupledPressuresGrid.Enabled = pressureCollectionCheckbox.Checked;
        }

        private void flowLayoutPanel2_Resize( object sender, EventArgs e )
        {
            richTextBox1.Width = flowLayoutPanel2.Width - 2;
            _coupledPressuresGrid.Width = flowLayoutPanel2.Width - 2;
        }
    }
}