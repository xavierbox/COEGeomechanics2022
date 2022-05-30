using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gigamodel
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class GigamodelLogSeismicUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private GigamodelLogSeismic process;

        /// <summary>
        /// Initializes a new instance of the <see cref="GigamodelLogSeismicUI"/> class.
        /// </summary>
        public GigamodelLogSeismicUI( GigamodelLogSeismic process )
        {
            InitializeComponent();
            _wellsDragDrop.AcceptedTypes = new List<Type>() { typeof(Borehole), typeof(BoreholeCollection) };
            _wellsDragDrop.ImageList = new ImageList();
            _wellsDragDrop.ImageList.Images.Add(PetrelImages.Well);
            _wellsDragDrop.ImageList.Images.Add(PetrelImages.WellFolder);
            _wellsDragDrop.ReferenceName = "Well/Well folder";
            _wellsDragDrop.ErrorImage = PetrelImages.Cancel;
            _wellsDragDrop.PlaceHolder = "Please drop a well or a well folder";

            _seismicDragDrop.AcceptedTypes = new List<Type>() { typeof(Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube), typeof(Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection) };
            _seismicDragDrop.ImageList = new ImageList();
            _seismicDragDrop.ImageList.Images.Add(PetrelImages.Seismic3D);
            _seismicDragDrop.ImageList.Images.Add(PetrelImages.Interpretation);
            _seismicDragDrop.ReferenceName = "Seismic/seismic folder";
            _seismicDragDrop.ErrorImage = PetrelImages.Cancel;
            _seismicDragDrop.PlaceHolder = "Please drop a seismic cube or a folder containing seismic cubes";

            saveButton.Image = PetrelImages.Apply;
            cancelButton.Image = PetrelImages.Cancel;
            comboBox1.SelectedIndex = 0;

            this.process = process;
        }

        private void dropTarget1_DragDrop( object sender, DragEventArgs e )
        {
            //Object s = e.Data.GetData(typeof(Object));
            //Slb.Ocean.Petrel.DomainObject.Seismic.SeismicLine3D line = s as Slb.Ocean.Petrel.DomainObject.Seismic.SeismicLine3D;
            ;
            //var name = s.GetType();
            //var index = line.LineIndex;
            ;
        }

        private void saveButton_Click( object sender, EventArgs e )
        {
            var w = _wellsDragDrop.Value;
            var ss = _seismicDragDrop.Value;

            List<Borehole> wells = new List<Borehole>();
            if (w is Borehole)
                wells.Add(w as Borehole);
            else if (w is BoreholeCollection)
            {
                BoreholeCollection c = w as BoreholeCollection;
                foreach (Borehole borehole in c)
                    wells.Add(borehole);
            }
            else
            {
                return;
            }

            List<Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube> cubes = new List<Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube>();
            if (ss is Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube)
                cubes.Add(ss as Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube);
            else if (ss is Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection)
            {
                Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection c = ss as Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection;
                foreach (Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube cube in c.SeismicCubes)
                    cubes.Add(cube);
            }
            else { return; }

            double spacing = -1.0;
            if (comboBox1.SelectedIndex == 1)//0.5m
            {
                spacing = 0.5;
            }
            else if (comboBox1.SelectedIndex == 2)//0.5ft
            {
                spacing = 0.5 * 0.30428;
            }
            else
            {
                ;
            }

            using (Slb.Ocean.Petrel.IProgress p = Slb.Ocean.Petrel.PetrelLogger.NewProgress(0, 100))
            {
                for (int n = 0; n < wells.Count; n++)
                {
                    for (int k = 0; k < cubes.Count; k++)
                        Gigamodel.OceanUtils.SeismicCubesUtilities.SeismicToLog(cubes[k], wells[n], spacing);

                    p.ProgressStatus = Math.Min(100, (int)(100.0 * n / wells.Count));
                }
            }
        }

        private void cancelButton_Click( object sender, EventArgs e )
        {
            this.ParentForm.Close();
        }
    }
}