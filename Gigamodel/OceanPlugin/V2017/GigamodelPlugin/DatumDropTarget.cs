using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject.Well;

namespace Gigamodel
{
    public partial class DatumDropTarget : UserControl
    {
        public event EventHandler DataDropAccepted;
        public event EventHandler DataDropRejected;
        public event EventHandler ValueChanged;

        Font _italic, _normal;
        private string _droid;
        private string _name;
        private Object _value;

        public DatumDropTarget()
        {
            InitializeComponent();
            _normal = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size, presentationBox1.Font.Style);
            _italic = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size * 0.8f, FontStyle.Italic);

            connectEvents();
            ClearComponent();
            PetrelPropertyName = string.Empty;
            PlaceHolder = "Drag-drop here...";
            AcceptedTypes = null;
            ImageList = null;
            DisplayImage = null;
            ErrorImage = null;
            setStyleTextBox();
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
            get;set;
        }


        public Object Value
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
                    setStyleTextBox();
                }

                else if (AcceptedTypes != null) //non-default configuration. Prefferred 
                {
                    int counter = 0;
                    bool accepted = false;

                    foreach (Type t in AcceptedTypes)
                    {
                        if (value.GetType() == t)
                        {
                            PetrelPropertyName = value.GetType().GetProperty("Name").GetValue(value).ToString();
                            DroidString = value.GetType().GetProperty("Droid").GetValue(value).ToString();
                            _value = value;
                            if ((ImageList != null) && (ImageList.Images.Count > counter))
                            DisplayImage = ImageList.Images[counter];

                            accepted = true;
                            break;
                        }
                        counter += 1;
                    }

                    setStyleTextBox();

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

                    if (value is StructuredSurface)
                    {
                        StructuredSurface s2 = (StructuredSurface)(value);
                        presentationBox1.Text = s2.Name;
                        dropTarget1.Tag = s2;
                        _value = s2;
                        presentationBox1.Image = PetrelImages.Surface;
                        presentationBox1.Text = s2.Name;
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else if (value is RegularHeightFieldSurface)
                    {
                        RegularHeightFieldSurface s1 = (RegularHeightFieldSurface)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        presentationBox1.Image = PetrelImages.Surface;
                        presentationBox1.Text = s1.Name;
                        setStyleTextBox();
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else if (value is SeismicCube)
                    {
                        SeismicCube s1 = (SeismicCube)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        presentationBox1.Image = PetrelImages.Seismic3D;
                        setStyleTextBox();
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else if (value is Borehole)
                    {
                        Borehole s1 = (Borehole)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        presentationBox1.Image = PetrelImages.WellBlue;
                        setStyleTextBox();
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else if (value is Borehole)
                    {
                        Borehole s1 = (Borehole)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }
                    else if (value is BoreholeCollection)
                    {
                        BoreholeCollection s1 = (BoreholeCollection)(value);
                        dropTarget1.Tag = s1;
                        presentationBox1.Text = s1.Name;
                        _value = s1;
                        presentationBox1.Image = PetrelImages.WellFolder;
                        setStyleTextBox();
                        DataDropAccepted?.Invoke(this, new EventArgs());
                    }


                    else
                    {
                        presentationBox1.Text = string.Empty;
                        _value = null;
                        dropTarget1.Tag = null;
                        setStyleTextBox();
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
        private void connectEvents()
        {
            this.dropTarget1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropTarget1_DragDrop);
            this.presentationBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.presentationBox1_KeyUp);

        }

        private void setStyleTextBox()
        {
            
            presentationBox1.Image = DisplayImage;
            presentationBox1.Text = PetrelPropertyName;

            if (presentationBox1.Text == string.Empty)
            {
                presentationBox1.ForeColor = Color.Gray;
                presentationBox1.Font = _italic;
                presentationBox1.Text = PlaceHolder;
            }
            else
            {
                presentationBox1.ForeColor = Color.Black;
                presentationBox1.Font = _normal;
            }



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
        #endregion
    }
}
