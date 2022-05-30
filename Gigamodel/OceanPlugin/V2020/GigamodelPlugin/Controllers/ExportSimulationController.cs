using Gigamodel.Data;
using Gigamodel.GigaModelProcess;
using Gigamodel.Services;
using Gigamodel.VisageUtils;
using System.Collections.Generic;

namespace Gigamodel.Controllers
{
    internal class ExportSimulationController
    {
        private GigaModelDataModel _model;
        private GigaModelProcessUI _view;

        public ExportSimulationController( GigaModelProcessUI views, GigaModelDataModel model )
        {
            _model = model;
            _view = views;
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            _view.ExportSimulationRequestEvent += new System.EventHandler<CreateEditArgs>(this.ExportSimulationHandler);

            _view.ExportSimulationCompressedRequestEvent += new System.EventHandler<CreateEditArgs>(this.ExportSimulationCompressedHandler);
        }

        private void ExportSimulationCompressedHandler( object sender, CreateEditArgs evt )
        {
            SimulationModelItem item = (SimulationModelItem)(evt.Object);
            if (item != null)
            {
                Slb.Ocean.Petrel.PetrelLogger.Status(new string[] { "Exporting simulation data" });

                Dictionary<string, List<KeyValuePair<string, bool>>> options = null;

                GigaModelProcess.MIIConfigOptions dlg = new MIIConfigOptions();
                dlg.ShowDialog();
                options = dlg.GetOptions();

                bool error;
                List<string> s = VisageOceanSeismicDeckWritter.WriteDeckCompressedBinary(item, _model, true, out error, options);// nestedProgressBar);

                if (!error)
                {
                    MessageService.ShowMessage("Simulation deck exported to folder:\n\n" + System.IO.Path.Combine(GigaModelDataDefinitions.SimExportFolder, item.Name));
                }
                else
                {
                    MessageService.ShowError("An error occured while exporting the simulation deck. The Exception was:\n" + s[0]);
                }
                Slb.Ocean.Petrel.PetrelLogger.Status(new string[] { "" });
            }
        }

        private void ExportSimulationHandler( object sender, CreateEditArgs evt )
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
        }
    }
}