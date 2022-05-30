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

namespace ManipulateCubes
{
    public partial class SeismicDropTarget : UserControl
    {
        Font italic, normal;

        public SeismicDropTarget()
        {
            InitializeComponent();
            ConnectEvents();
            AdditionalInitialize();
            ClearComponent();


        }

        public string PlaceHolder { get; set; }

        public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube Value
        {
            get { return (Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube)(dropTarget1.Tag); }

            set
            {
                if (value == null)
                    ClearComponent();

                if (value is Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube)
                {
                    presentationBox1.Text = value == null ? string.Empty : ((SeismicCube)(value)).Name;

                }
                presentationBox1.Image = PetrelImages.Seismic3D_32;
                SetStyleTextBox(presentationBox1);
                dropTarget1.Tag = value;

            }
        }

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
            presentationBox1.Text = string.Empty;
            SetStyleTextBox(presentationBox1);
        }

        private void SetStyleTextBox(PresentationBox sender)
        {
            if (sender.Text == string.Empty)
            {
                sender.ForeColor = Color.Gray;
                sender.Font = italic;
                sender.Text = "*Please drop a realized seismic cube for " + PropertyName;
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
            SeismicCube singleCube = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;

            if (singleCube == null)
            {
                MessageBox.Show("Please drop a valid seismic cube", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Value = singleCube;
        }








    };
}

