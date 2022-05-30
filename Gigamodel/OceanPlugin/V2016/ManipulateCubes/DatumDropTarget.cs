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
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;

namespace ManipulateCubes
{
    public partial class DatumDropTarget : UserControl
    {
        public event EventHandler SelectionChanged;

        Font italic, normal;

        public DatumDropTarget()
        {
            InitializeComponent();
            ConnectEvents();
            AdditionalInitialize();
            ClearComponent();
            PropertyName = "Datum";
            PlaceHolder = "Drag-drop here...";
        }


        Object _value;
        public Object Value
        {
            get { return _value; }

            set
            {

                if (value is StructuredSurface)
                {
                    StructuredSurface s2 = (StructuredSurface)(value);
                    presentationBox1.Text = s2.Name;
                    dropTarget1.Tag = s2;
                    _value = s2;
                    presentationBox1.Image = PetrelImages.Surface;
                    presentationBox1.Text = s2.Name;
                }
                else if (value is RegularHeightFieldSurface)
                {
                    RegularHeightFieldSurface s1 = (RegularHeightFieldSurface)(value);
                    dropTarget1.Tag = s1;
                    presentationBox1.Text = s1.Name;
                    _value = s1;
                    presentationBox1.Image = PetrelImages.Surface;
                    presentationBox1.Text = s1.Name;
                    SetStyleTextBox(presentationBox1);
                }
                else if (value is SeismicCube)
                {
                    SeismicCube s1 = (SeismicCube)(value);

                   // var i = s1.NumSamplesIJK.I;
                   // var j = s1.NumSamplesIJK.J;
                   // var k = s1.NumSamplesIJK.K; 

                    dropTarget1.Tag = s1;
                    presentationBox1.Text = s1.Name;
                    _value = s1;
                    presentationBox1.Image = PetrelImages.Seismic3D;
                    SetStyleTextBox(presentationBox1);



                }
                else if (value is Borehole)
                {
                    Borehole s1 = (Borehole)(value);
                    dropTarget1.Tag = s1;
                    presentationBox1.Text = s1.Name;
                    _value = s1;
                    presentationBox1.Image = PetrelImages.WellBlue;
                    SetStyleTextBox(presentationBox1);
                }
                else if (value is Borehole)
                {
                    Borehole s1 = (Borehole)(value);
                    dropTarget1.Tag = s1;
                    presentationBox1.Text = s1.Name;
                    _value = s1;
                }
                else if (value is BoreholeCollection)
                {
                    BoreholeCollection s1 = (BoreholeCollection)(value);
                    dropTarget1.Tag = s1;
                    presentationBox1.Text = s1.Name;
                    _value = s1;
                    presentationBox1.Image = PetrelImages.WellFolder;
                    SetStyleTextBox(presentationBox1);
                }


                else
                {
                    presentationBox1.Text = string.Empty;
                    _value = null;
                    dropTarget1.Tag = null;
                    SetStyleTextBox(presentationBox1);
                }

           
                    SelectionChanged?.Invoke(this, new EventArgs());

          
            }


        }

        public string PlaceHolder { get; set; }

        public string PropertyName { get; set; }

        private void ConnectEvents()
        {
            this.dropTarget1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropTarget1_DragDrop);
            this.presentationBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.presentationBox1_KeyUp);

        }

        private void AdditionalInitialize()
        {

            normal = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size, presentationBox1.Font.Style);
            italic = new Font(presentationBox1.Font.FontFamily, presentationBox1.Font.Size * 0.8f, FontStyle.Italic);
        }

        public void ClearComponent()
        {
            dropTarget1.Tag = null;
            Value = null;
            presentationBox1.Text = string.Empty;
            SetStyleTextBox(presentationBox1);
        }

        private void SetStyleTextBox(PresentationBox sender)
        {
            if (sender.Text == string.Empty)
            {
                sender.ForeColor = Color.Gray;
                sender.Font = italic;
                sender.Text = PlaceHolder;
            }
            else
            {
                sender.ForeColor = Color.Black;
                sender.Font = normal;
            }
        }

        private void presentationBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
                ClearComponent();
        }

        private void dropTarget1_DragDrop(object sender, DragEventArgs e)
        {
            Object s = e.Data.GetData(typeof(Object));
            Value = s;
        }


    };
}

