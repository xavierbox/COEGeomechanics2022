using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
namespace OceanControlsLib
{
    public partial class DatumDropTarget : UserControl
    {
        public event EventHandler DataDropAccepted;

        public event EventHandler DataDropRejected;

        public event EventHandler ValueChanged;

        private Font _italic, _normal;
        private string _droid;
        private string _name;
        private Object _value = null;

        public DatumDropTarget()
        {
            InitializeComponent();
            _normal = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size, presentationBox1.Font.Style);
            _italic = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size * 0.8f, FontStyle.Italic);

            ConnectEvents();
            ClearComponent();
            PetrelPropertyName = string.Empty;
            PlaceHolder = "Drag-drop here...";
            AcceptedTypes = null;
            ImageList = null;
            DisplayImage = null;
            ErrorImage = null;
            StyleControl();
        }

        public void ClearComponent()
        {
            Value = null;
        }

        public Image DisplayImage
        {
            get { return presentationBox1.Image; }
            set { presentationBox1.Image = value; }
        }

        public Image ErrorImage
        {
            get; set;
        }

        public string DroidString
        {
            get { return _droid; }
            private set { _droid = value; }
        }

        public string PetrelPropertyName
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string ReferenceName
        {
            get; set;
        }

        public Object Valueold
        {
            get { return _value; }

            set
            {
                PetrelPropertyName = string.Empty;
                DroidString = string.Empty;
                _value = null;

                if (value == null)
                {
                    DisplayImage = null;
                    StyleControl();
                }
                else if (AcceptedTypes != null) //non-default configuration. Prefferred


                {
                    int counter = 0;
                    bool accepted = false;

                    foreach (Type t in AcceptedTypes)
                    {
                        if (value.GetType() == t)
                        {
                            var isItANamedObject = value.GetType().GetProperty("Name") != null ? true : false;

                            if (isItANamedObject)
                            {
                                PetrelPropertyName = value.GetType().GetProperty("Name").GetValue(value).ToString();
                                DroidString = value.GetType().GetProperty("Droid").GetValue(value).ToString();
                            }
                            else
                            {
                            }
                            _value = value;
                            if ((ImageList != null) && (ImageList.Images.Count > counter))
                                DisplayImage = ImageList.Images[counter];

                            accepted = true;
                            break;
                        }
                        counter += 1;
                    }

                    StyleControl();

                    if (accepted)
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    else
                    {
                        DisplayImage = ErrorImage;
                        DataDropRejected?.Invoke(this, new EventArgs());
                    }
                }
                else //(AcceptedTypes == null) //use default
                {
                    PetrelPropertyName = value.GetType().GetProperty("Name")?.GetValue(value).ToString() ?? "None";
                    DroidString = value.GetType().GetProperty("Droid").GetValue(value).ToString();
                    presentationBox1.Text = PetrelPropertyName;

                    //var isItANamedObject = value.GetType().GetProperty("Name") != null ? true : false;

                    //        if (isItANamedObject)
                    //        {
                    //            PetrelPropertyName = value.GetType().GetProperty("Name").GetValue(value).ToString();
                    //            DroidString = value.GetType().GetProperty("Droid").GetValue(value).ToString();
                    //        }




                    if (value is StructuredSurface)
                    {
                        _value = (StructuredSurface)(value);
                        presentationBox1.Image = PetrelImages.Surface;

                    }
                    else if (value is RegularHeightFieldSurface)
                    {
                        _value = (RegularHeightFieldSurface)(value);
                        presentationBox1.Image = PetrelImages.Surface;
                    }

                    else if (value is SeismicCube)
                    {
                        _value = (SeismicCube)(value);
                        presentationBox1.Image = PetrelImages.Seismic3D;
                    }

                    else if (value is SeismicCollection)
                    {
                        _value = (SeismicCollection)(value);
                        presentationBox1.Image = PetrelImages.Interpretation;
                    }

                    else if (value is Borehole)
                    {
                        _value = (Borehole)(value);
                        presentationBox1.Image = PetrelImages.WellBlue;
                    }

                    else if (value is BoreholeCollection)
                    {
                        BoreholeCollection s1 = (BoreholeCollection)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        presentationBox1.Image = PetrelImages.WellFolder;
                        //StyleControl();
                        //DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else
                    {
                        presentationBox1.Text = string.Empty;
                        _value = null;
                        dropTarget1.Tag = null;
                        StyleControl();
                        DataDropRejected?.Invoke(this, new EventArgs());
                    }


                }

                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        public List<Type> AcceptedTypes
        {
            get; set;
        }

        public ImageList ImageList
        {
            get { return imageList1; }
            set { imageList1 = value; }
        }

        public string PlaceHolder { get; set; }

        #region private

        private void ConnectEvents()
        {
            this.dropTarget1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropTarget1_DragDrop);
            this.presentationBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.presentationBox1_KeyUp);
        }

        public void StyleControl()
        {
            presentationBox1.Image = DisplayImage;

            if (_value == null)
            {
                presentationBox1.Text = string.Empty;
                presentationBox1.ForeColor = Color.Gray;
                presentationBox1.Font = _italic;
                presentationBox1.Text = PlaceHolder;

            }

            else
            {
                if ((_value.GetType() == typeof(float)) || (_value.GetType() == typeof(double)) || (_value.GetType() == typeof(int)))
                {
                    presentationBox1.Text = _value.ToString();

                }
                else
                {
                    presentationBox1.Text = PetrelPropertyName;
                }
                presentationBox1.ForeColor = Color.Black;
                presentationBox1.Font = _normal;
            }

            /*if (presentationBox1.Text == string.Empty)
            {
                presentationBox1.ForeColor = Color.Gray;
                presentationBox1.Font = _italic;
                presentationBox1.Text = PlaceHolder;
            }
            else
            {
                presentationBox1.ForeColor = Color.Black;
                presentationBox1.Font = _normal;
            }*/
        }

        private void presentationBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
               ClearComponent();
        }

        private void dropTarget1_DragDrop(object sender, DragEventArgs e)
        {
            /*SeismicCube sq1 = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;
            string droid = sq1.Droid.ToString();

            try
            {
                SeismicCube initialPressure = (SeismicCube)(Slb.Ocean.Core.DataManager.Resolve(new Slb.Ocean.Core.Droid(droid)));
            }
            catch (Exception except)
            {
                ;
            }*/

            Object s = e.Data.GetData(typeof(Object));
            Value = s;
        }


        public Object Value
        {
            get { return _value; }

            set
            {
                PetrelPropertyName = string.Empty;
                DroidString = string.Empty;
                //_value = null;
                bool accepted = false;
                DisplayImage = null;

                if (value == null)
                {
                    if (_value != null)
                    {
                        _value = value;
                        PetrelPropertyName = string.Empty; ;
                        DroidString = string.Empty; ;

                        StyleControl();
                        ValueChanged?.Invoke(this, new EventArgs());
                        DataDropRejected?.Invoke(this, new EventArgs());
                    }

                    return;
                }

                List<Type> _types;
                ImageList _images;
                if (AcceptedTypes == null)
                {
                    _types = new List<Type>()
                        {       typeof(StructuredSurface),
                                typeof(RegularHeightFieldSurface),
                                typeof(SeismicCube),
                                typeof(SeismicCollection),
                                typeof(Borehole),
                                typeof(BoreholeCollection),

                                typeof(Property),
                                typeof(Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection),


                        };

                    _images = new ImageList();
                    _images.Images.Add(PetrelImages.Surface);
                    _images.Images.Add(PetrelImages.Surface);
                    _images.Images.Add(PetrelImages.Seismic3D);
                    _images.Images.Add(PetrelImages.Interpretation);
                    _images.Images.Add(PetrelImages.WellBlue);
                    _images.Images.Add(PetrelImages.WellFolder);
                    _images.Images.Add(PetrelImages.Property);
                    _images.Images.Add(PetrelImages.Property);
                }
                else
                {
                    _types = AcceptedTypes;
                    _images = this.ImageList;
                }


                int counter = 0;
                foreach (Type t in _types)
                {
                    if (value.GetType() == t)
                    {
                        
                        _value = value;
                        


                        if ((_images != null) && (_images.Images.Count > counter))
                            DisplayImage = _images.Images[counter];

                        accepted = true;
                        break;
                    }
                    counter += 1;
                }


                if (accepted)
                {

                    PetrelPropertyName = value.GetType().GetProperty("Name")?.GetValue(value).ToString() ?? "None";
                    DroidString = value.GetType().GetProperty("Droid").GetValue(value).ToString();
                    StyleControl();

                    //this.presentationBox1.DomainObject = _value;
                    DataDropAccepted?.Invoke(this, new EventArgs());

                }
                else
                {
                    PetrelPropertyName = string.Empty;
                    DroidString = string.Empty;
                    DisplayImage = ErrorImage;
                    _value = null;
                    StyleControl();
                    DataDropRejected?.Invoke(this, new EventArgs());
                }


                ValueChanged?.Invoke(this, new EventArgs());
            }
        }


        #endregion private
    }

}
