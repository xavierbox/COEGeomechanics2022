using Gigamodel.Data;
using Gigamodel.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gigamodel.Controllers
{
    internal class InputValidationController
    {
        private GigaModelProcessUI ui;
        private GigaModelDataModel model;

        //the object to edit or create is the object reference inside evt

        public SimulationModelItem SaveSimulation( CreateEditArgs evt )
        {
            if (!ValidateArgsNonNull(evt)) return null;

            Object item = null;
            if (((evt.IsNew) && (ValidateNameIsNew(evt.Name, model.SimulationNames))) || (!evt.IsNew))
            {
                item = ValidateAndCreateSimulation((SimulationModelItem)evt.Object);
                if (item != null)
                {
                    return (SimulationModelItem)item;
                }
            }

            return null;
        }

        private void CreateOrEdit( object sender, CreateEditArgs evt )
        {
            if (!ValidateArgsNonNull(evt)) return;

            Object item = null;

            //create/edit and validate. Note that each model is different and requires different validations, etc.
            if (evt.Object as MaterialsModelItem != null)
            {
                if (((evt.IsNew) && (ValidateNameIsNew(evt.Name, model.MaterialModelNames))) || (!evt.IsNew))
                {
                    item = ValidateAndCreateMaterial((MaterialsModelItem)evt.Object);
                    if (item != null)
                    {
                        ui.materialsControl.DisplayModelItem((MaterialsModelItem)(item));
                        MessageService.ShowMessage(evt.IsNew ? "Material Model was Created" : "Material Model was Updated");
                    }
                }
            }//material
            else if (evt.Object as PressureModelItem != null)
            {
                if (((evt.IsNew) && (ValidateNameIsNew(evt.Name, model.PressureModelNames))) || (!evt.IsNew))
                {
                    item = ValidateAndCreatePressure((PressureModelItem)(evt.Object));
                    if (item != null)
                    {
                        ui.pressuresControl.DisplayModelItem((PressureModelItem)(item));
                        MessageService.ShowMessage(evt.IsNew ? "Pressure Model was Created" : "Pressure Model was Updated");
                    }
                }
            }//pressure
            else if (evt.Object as BoundaryConditionsItem != null)
            {
                if (((evt.IsNew) && (ValidateNameIsNew(evt.Name, model.BoundaryConditionsNames))) || (!evt.IsNew))
                {
                    item = ValidateAndCreateTectonics((BoundaryConditionsItem)(evt.Object));
                    if (item != null)
                    {
                        ui.boundaryConditionsControl.DisplayModelItem((BoundaryConditionsItem)(item));
                        MessageService.ShowMessage(evt.IsNew ? "Tectonic Model was Created" : "Tectonic Model was Updated");
                    }
                }
            }//bc
            else if (evt.Object as SimulationModelItem != null)
            {
                item = SaveSimulation(evt);
                if (item != null)
                {
                    ui.simulationControl.DisplayModelItem((SimulationModelItem)(item));
                    MessageService.ShowMessage(evt.IsNew ? "Simulation Setup was Created" : "Simulation Setup was Updated");
                }
                else
                    ui.simulationControl.Dirty = true;
            }//bc
            else
            {
                MessageService.ShowMessage("Not Implemented yet [CreateOrEdit Controller]", MessageType.INFO);
            }
        }

        private void Connect()
        {
            ui.CancelClicked += new System.EventHandler(this.cancelClicked);

            ui.TabChanged += new System.EventHandler(this.tabChanged);

            ui.materialsControl.EditSelectionChangedEvent += ( s, e ) => { ui.materialsControl.DisplayModelItem(model.MaterialModels.GetOrCreateModel(e.Value)); };

            ui.materialsControl.DeleteModelEvent += ( s, e ) =>
            {
                int index = model.MaterialModelNames.ToList().IndexOf(e.Value);
                if (index < 0) return;
                if (model.MaterialModels.DeleteModelbyName(e.Value))
                {
                    ui.materialsControl.UpdateSelector(model.MaterialModelNames);
                    List<string> names = model.MaterialModelNames.ToList();
                    if (names.Count < 1) ui.materialsControl.IsSelectedAsNew = true;
                    else
                        ui.materialsControl.DisplayModelItem(model.MaterialModels.GetOrCreateModel(names[Math.Max(0, index - 1)]));
                }
            };

            ui.pressuresControl.EditSelectionChangedEvent += ( s, e ) => { ui.pressuresControl.DisplayModelItem(model.PressureModels.GetOrCreateModel(e.Value)); };

            ui.pressuresControl.DeleteModelEvent += ( s, e ) =>
            {
                int index = model.PressureModelNames.ToList().IndexOf(e.Value);
                if (index < 0) return;
                if (model.PressureModels.DeleteModelbyName(e.Value))
                {
                    ui.pressuresControl.UpdateSelector(model.PressureModelNames);
                    List<string> names = model.PressureModelNames.ToList();
                    if (names.Count < 1) ui.pressuresControl.IsSelectedAsNew = true; //(null);
                    else
                        ui.pressuresControl.DisplayModelItem(model.PressureModels.GetOrCreateModel(names[Math.Max(0, index - 1)]));
                }
            };

            ui.boundaryConditionsControl.EditSelectionChangedEvent += ( s, e ) => { ui.boundaryConditionsControl.DisplayModelItem(model.BoundaryConditionsModels.GetOrCreateModel(e.Value)); };

            ui.boundaryConditionsControl.VisibilitychangedEvent += ( s, e ) =>
            {
                ui.boundaryConditionsControl.updatePoroelasticSelectors(model.MaterialModelNames.ToList(), model.PressureModelNames.ToList());
            };

            ui.boundaryConditionsControl.DeleteModelEvent += ( s, e ) =>
            {
                int index = model.BoundaryConditionsNames.ToList().IndexOf(e.Value);
                if (index < 0) return;

                if (model.BoundaryConditionsModels.DeleteModelbyName(e.Value))
                {
                    ui.boundaryConditionsControl.UpdateSelector(model.BoundaryConditionsNames);
                    List<string> names = model.BoundaryConditionsNames.ToList();
                    if (names.Count < 1) ui.boundaryConditionsControl.IsSelectedAsNew = true; //(null);
                    else
                        ui.boundaryConditionsControl.DisplayModelItem(model.BoundaryConditionsModels.GetOrCreateModel(names[Math.Max(0, index - 1)]));
                }
            };

            ui.boundaryConditionsControl.MaterialAndPressureRequestEvent += ( s, e ) =>
                {
                    if (e.Values.Count != 2)
                    {
                        ui.boundaryConditionsControl.MaterialSelected = null;
                        ui.boundaryConditionsControl.PressureSelected = null;

                        return;
                    }
                    ui.boundaryConditionsControl.MaterialSelected = model.MaterialModels.FindModel(e.Values[0]) == true ? model.MaterialModels.GetOrCreateModel(e.Values[0]) : null;
                    ui.boundaryConditionsControl.PressureSelected = model.PressureModels.FindModel(e.Values[1]) == true ? model.PressureModels.GetOrCreateModel(e.Values[1]) : null;
                };

            ui.simulationControl.VisibilitychangedEvent += ( s, e ) =>
            {
                ui.simulationControl.UpdateSelectors(model.MaterialModelNames.ToList(), model.PressureModelNames.ToList(), model.BoundaryConditionsNames.ToList());
            };

            ui.simulationControl.DeleteModelEvent += ( s, e ) =>
            {
                //  model.SimulationsModel.Clear();
                //  ui.simulationControl.IsSelectedAsNew = true;
                // ui.simulationControl.UpdateSelector(model.SimulationNames);

                int index = model.SimulationNames.ToList().IndexOf(e.Value);
                if (index < 0) return;
                if (model.SimulationsModel.DeleteModelbyName(e.Value))
                {
                    ui.simulationControl.UpdateSelector(model.SimulationNames);
                    List<string> names = model.SimulationNames.ToList();
                    if (names.Count < 1) ui.simulationControl.IsSelectedAsNew = true; //(null);
                    else
                        ui.simulationControl.DisplayModelItem(model.SimulationsModel.GetOrCreateModel(names[Math.Max(0, index - 1)]));
                }
            };

            ui.simulationControl.EditSelectionChangedEvent += ( s, e ) => { ui.simulationControl.DisplayModelItem(model.SimulationsModel.GetOrCreateModel(e.Value)); };

            ui.CreateOrEditEvent += new System.EventHandler<CreateEditArgs>(this.CreateOrEdit);
        }

        private void cancelClicked( object sender, EventArgs evt )
        {
            ;//tell the view to close itself. Ask  first to save pending changes if any.
        }

        public InputValidationController( GigaModelProcessUI views, GigaModelDataModel data )
        {
            ui = views;
            model = data;

            ui.materialsControl.UpdateSelector(model.MaterialModelNames);

            ui.pressuresControl.UpdateSelector(model.PressureModelNames);

            ui.boundaryConditionsControl.UpdateSelector(model.BoundaryConditionsNames);

            ui.simulationControl.UpdateSelector(model.SimulationNames);
            ui.simulationControl.UpdateSelectors(model.MaterialModelNames.ToList(), model.PressureModelNames.ToList(), model.BoundaryConditionsNames.ToList());

            Connect();
        }

        private void tabChanged( object sender, EventArgs evt )
        {
            ; //update the view if needed.
        }

        private bool ValidateNameIsNew( string name, string[] names )
        {
            if (names.Contains(name))
            {
                MessageService.ShowMessage("The selected name is already used.");
                return false;
            }
            return true;
        }

        private bool ValidateCubesAreSimilarSize( List<SeismicCubeFaccade> cubes )
        {
            if (cubes.Count() < 1) return false;

            int[] size = cubes[0].Size;

            for (int n = 1; n < cubes.Count(); n++)
                for (int d = 0; d < size.Length; d++)
                    if (cubes[n].Size[d] != size[d])
                    {
                        MessageService.ShowMessage("Please select seismic cubes with the same geometry");
                        return false;
                    }
            return true;
        }

        private bool ValidateDatesAscending( List<DateTime> dates )
        {
            DateTime t1 = dates[0];
            for (int n = 1; n < dates.Count(); n++)
            {
                DateTime t2 = dates[n];
                if (t2.CompareTo(t1) > 0) //t2 is later than t1
                {
                    t1 = t2;
                }
                else
                {
                    MessageService.ShowMessage("Dates must be in increasing order");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateArgsNonNull( CreateEditArgs evt )
        {
            if ((evt == null) || (evt.Object == null) || (evt.Name == null) || (evt.Name == string.Empty))
            {
                MessageService.ShowMessage("Incomplete or unacceptable data entered or model corrupted.");

                return false;
            }

            return true;
        }

        public SimulationModelItem ValidateAndCreateSimulation( SimulationModelItem m )
        {
            //List<SeismicCubeFaccade> cubes = new List<SeismicCubeFaccade>();
            //validate cubes are the same size, the naming, etc....

            List<SeismicCubeFaccade> cubes = new List<SeismicCubeFaccade>();

            cubes.AddRange(model.MaterialModels.GetOrCreateModel(m.MaterialModelName).Cubes);
            cubes.AddRange(model.PressureModels.GetOrCreateModel(m.PressureModelName).Cubes);

            //cubes.AddRange(m.MaterialsModelItem.Cubes);
            //cubes.AddRange(m.PressureModelItem.Cubes);

            if (ValidateCubesAreSimilarSize(cubes))
            {
                return model.SimulationsModel.AppendOrEditModel(m);
            }

            return null;
        }

        public MaterialsModelItem ValidateAndCreateMaterial( MaterialsModelItem m )
        {
            if (ValidateCubesAreSimilarSize(m.Cubes))
            {
                return model.MaterialModels.AppendOrEditModel(m);
            }

            return null;
        }

        public PressureModelItem ValidateAndCreatePressure( PressureModelItem m )
        {
            if ((ValidateDatesAscending(m.DatedDroids.Select(t => t.Value).ToList())) &&
                (ValidateCubesAreSimilarSize(m.DatedDroids.Select(t => t.Key).ToList())))
            {
                return model.PressureModels.AppendOrEditModel(m);
            }

            return null;
        }

        public BoundaryConditionsItem ValidateAndCreateTectonics( BoundaryConditionsItem m )
        {
            /*
             if ((ValidateDatesAscending(m.DatedDroids.Select(t => t.Value).ToList())) &&
                (ValidateCubesAreSimilarSize(m.DatedDroids.Select(t => t.Key).ToList())))
            {
                return model.PressureModels.AppendOrEditModel(m);
            }*/

            return model.BoundaryConditionsModels.AppendOrEditModel(m); ;
        }

        /*
        SimulationFolderSelectedForImport( string folder )
        {
        //find the xFiles width the same name.
        //determine the number of time steps
        //get the keywords of the time-step = 0;
        //return to the view for display: Number of time steps, selectedTimeStep = 0, Array of keywords, case name

        //if the selection is wrong, send a message to the message service and tell the view to clear itself
        }

        SimulationResultsSelectedForImport( List<string> propertyNames, currentTimeStep, caseName, folderName  )
        {
        }

        */

        private bool ValidateSimulationExport( CreateEditArgs item )
        {
            bool returnValue = true;

            return returnValue;
        }

        private void ExportSimulation( object sender, CreateEditArgs evt )
        {
            SimulationModelItem item = SaveSimulation(evt);

            //if (item != null)
            //{
            //        MaterialsModelItem mats  = model.MaterialModels.GetOrCreateModel( item.MaterialModelName );

            //        SeismicCubeFaccade YoungsModulus = mats.YoungsModulus;
            //        SeismicCubeFaccade PoissonsRatio = mats.PoissonsRatio;
            //        SeismicCubeFaccade Density       = mats.Density;

            //        YoungsModulus.SplitCube = SeismicCubesUtilities.splitBinaryCube(SeismicCube cube);

            //    //    PressureModelItem  press = model.PressureModels.GetOrCreateModel( item.PressureModelName );

            //    //    BoundaryConditionsItem bc = model.BoundaryConditionsModels.GetOrCreateModel(item.BoundaryConditionsModelName);

            //    //    VisageTools.WriteDeck( item );
            //    //    return;
            //}
        }
    }
}