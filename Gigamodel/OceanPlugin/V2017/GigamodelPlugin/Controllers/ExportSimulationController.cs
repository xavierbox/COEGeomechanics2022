using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigamodel.Data;
using Gigamodel.OceanUtils;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Gigamodel.VisageUtils;
using Gigamodel.Services;

namespace Gigamodel.Controllers
{
    class ExportSimulationController
    {
        GigaModelDataModel _model;
        GigaModelProcessUI _view;

        public ExportSimulationController(GigaModelProcessUI views, GigaModelDataModel model)
        {
            _model = model;
            _view = views;
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            //    _view.ExportSimulationRequestEvent += new System.EventHandler(this.ExportSimulationHandler);
            _view.ExportSimulationRequestEvent += new System.EventHandler<CreateEditArgs>(this.ExportSimulationHandler);

            _view.ExportSimulationCompressedRequestEvent += new System.EventHandler<CreateEditArgs>(this.ExportSimulationCompressedHandler);

        }

        private void ExportSimulationCompressedHandler(object sender, CreateEditArgs evt)
        {
            SimulationModelItem item = (SimulationModelItem)(evt.Object);
            if (item != null)
            {
                using (Slb.Ocean.Petrel.IProgress nestedProgressBar = Slb.Ocean.Petrel.PetrelLogger.NewProgress(0, 100))
                {
                   bool error;
                   List<string> s = VisageOceanSeismicDeckWritter.WriteDeckCompressedBinary(item, _model, true, out error, nestedProgressBar);
                    if (!error)
                    {
                        MessageService.ShowMessage("Simulation deck exported to folder:\n\n" + System.IO.Path.Combine(GigaModelDataDefinitions.SimExportFolder,item.Name));
                    }
                    else
                    {
                        MessageService.ShowError("An error occured while exporting the simulation deck. The Exception was:\n" + s[0]);
                    }
                }
            }
        }

        private void ExportSimulationHandler(object sender, CreateEditArgs evt)
        {

            SimulationModelItem item = (SimulationModelItem)(evt.Object);
            if (item != null)
            {
                bool error;
                List<string> s = VisageOceanSeismicDeckWritter.WriteDeck(item, _model, out error);
                if (!error)
                {
                    MessageService.ShowMessage("Simnulation deck exported");
                }
                else
                {
                    MessageService.ShowError("An error occured while exporting the simulation deck. The Exception was:\n" + s[0]);
                }
            }
                /*

                MaterialsModelItem mats = _model.MaterialModels.GetOrCreateModel(item.MaterialModelName);

                SeismicCubeFaccade YoungsModulus = mats.YoungsModulus;
                SeismicCubeFaccade PoissonsRatio = mats.PoissonsRatio;
                SeismicCubeFaccade Density = mats.Density;

                try
                {
                    YoungsModulus.SplitCube = SeismicCubesUtilities.splitBinaryCube(DataManager.Resolve(new Droid(YoungsModulus.DroidString)) as SeismicCube;
               
                }

                catch
                {

                }*/

                //    PressureModelItem  press = model.PressureModels.GetOrCreateModel(item.PressureModelName);



                //    BoundaryConditionsItem bc = model.BoundaryConditionsModels.GetOrCreateModel(item.BoundaryConditionsModelName);




            




        }


    }


}
