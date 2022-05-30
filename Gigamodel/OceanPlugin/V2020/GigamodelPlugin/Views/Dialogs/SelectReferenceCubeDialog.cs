
using Slb.Ocean.Petrel.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Gigamodel.Dialogs
{
    public partial class SelectReferenceCubeDialog : Form
    {
        public SelectReferenceCubeDialog()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            button1.Image = PetrelImages.Warning;
            button1.ImageAlign = ContentAlignment.MiddleCenter;

            cancelbutton.Image = PetrelImages.Cancel;
            cancelbutton.ImageAlign = ContentAlignment.MiddleCenter;
            acceptButton.Image = PetrelImages.Apply;
            acceptButton.ImageAlign = ContentAlignment.MiddleCenter;

            /*This may have happened because a seismic cube used during the model build was deleted
            or because some files exported in the simulation folder are missing
            <b> To fix this problem please</ b >
            Drop here any seismic cube(from the material properties or pressures) used when the model was built
            or a cube with identical geometry to any of these.
            */
        }

        //this is a comment

        private string _droid;

        public string Droid { get { return _droid; } set { _droid = value; } }

        private void dropTarget1_DragDrop( object sender, DragEventArgs e )
        {
            Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube sq1 = e.Data.GetData(typeof(Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube)) as Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube;

            if (sq1 != null)
            {
                Droid = sq1.Droid.ToString();
                presentationBox1.Image = PetrelImages.Seismic3D;
                presentationBox1.Text = sq1.Name;
            }
            else
            {
                Droid = string.Empty;
                presentationBox1.Image = PetrelImages.Cancel;
                presentationBox1.Text = string.Empty;
            }
        }
    }
}