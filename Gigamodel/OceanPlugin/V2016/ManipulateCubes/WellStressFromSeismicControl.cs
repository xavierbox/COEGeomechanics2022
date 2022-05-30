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
using Slb.Ocean.Petrel.DomainObject.Well;

namespace ManipulateCubes
{
    public partial class WellStressFromSeismicControl : UserControl
    {
        public WellStressFromSeismicControl()
        {
            InitializeComponent();
            minStrainTrackBar.Tag = minStrainTextBox;
            maxStrainTrackBar.Tag = maxStrainTextBox;
        }

        private void strainChangedTemporaly(object sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)(sender);
            if (bar == minStrainTrackBar)
                if (minStrainTrackBar.Value > maxStrainTrackBar.Value) maxStrainTrackBar.Value = minStrainTrackBar.Value;

            if (bar == maxStrainTrackBar)
                if (minStrainTrackBar.Value > maxStrainTrackBar.Value) minStrainTrackBar.Value = maxStrainTrackBar.Value;

            Label l = (Label)(minStrainTrackBar.Tag);
            l.Text = trackBarToStrain( minStrainTrackBar).ToString();
            l = (Label)(maxStrainTrackBar.Tag);
            l.Text = trackBarToStrain(maxStrainTrackBar).ToString();

        }

        private float trackBarToStrain(TrackBar tBar)
        {
            return tBar.Value / 50000.0f;
        }

        private void strainBarMouseRelease(object sender, MouseEventArgs e)
        {

        }

       
        private void saveButtonPressed(object sender, EventArgs e)
        {

        }

        private void seismicCubeDropped(object sender, EventArgs e)
        {
            Object dropped = ((DatumDropTarget)(sender)).Value;
            if (!(dropped is SeismicCube))
            {
                MessageBox.Show("Please drop a realized seismic cube", "Wrong data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          }

        private void wellDropped(object sender, EventArgs e)
        {
            Object dropped = ((DatumDropTarget)(sender)).Value;
            if (!(dropped is Borehole))
            if (!(dropped is BoreholeCollection))
                    MessageBox.Show("Please drop a well or a folder containing wells from the input tab", "Wrong data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }
    }
}
