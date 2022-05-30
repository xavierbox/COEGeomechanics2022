using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManipulateCubes
{
    class DeckExportedManaged
    {

        public DeckExportedManaged()
        {
        }

        static bool CanExport(string simulationName)
        {
            return true;
        }

        static string GetOrCreateExportFolder(string simulationName)
        {

            return "D:\\Projects\\CurrentProjects\\ProgrammingProjects\\GigamodelVisage\\OceanPlugin\\V2016\\ExportsSimulations\\";

        }

        static void ExportDeck(string simulationName)
        {
            SimulationModelItem model;
            PressureModelItem pressures;
            MaterialsModelItem mats;
            BoundaryConditionsItem bcs;
            string message, folder; 
            if (!PreExportDeck(simulationName, out message, out folder, out model, out pressures, out mats, out bcs))
            {
                MessageBox.Show(message);
                return;
            }

            var w1 = model;
            var w2 = pressures;
            var w3 = mats;
            var w4 = bcs;

            //export async for speed. Divide cubes by 2 export compressed binary 
            ExportMaterials(mats, folder, simulationName);

            //this is done simulataneously with the previous one 
            ExportPressures(pressures, folder, simulationName);

            //this is done simulataneously with the previous one 
            ExportBoundaryConditions(bcs, folder, simulationName);

            //Export config File for processing in the server 
        }



        static bool PreExportDeck(string simulationName, out string message, out string folder, out SimulationModelItem model, out PressureModelItem pressures, out MaterialsModelItem mats, out BoundaryConditionsItem bcs)
        {
            folder = GetOrCreateExportFolder(simulationName);

            //get the simulation object that contains only strings so far. 
            var gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            message = string.Empty;  model = null; pressures = null; mats = null;bcs = null;

            if (!gigaModel.SimulationsModel.FindModel(simulationName))
            {
                return false;
            }
            try
            {
                model = gigaModel.SimulationsModel.GetOrCreateModel(simulationName);
                pressures = gigaModel.PressureModels.GetOrCreateModel(model.PressureModelName);
                mats = gigaModel.MaterialModels.GetOrCreateModel(model.MaterialModelName);
                bcs = gigaModel.BoundaryConditionsModels.GetOrCreateModel(model.BoundaryConditionsModelName);
            }
            catch (Exception e)
            {
                message = e.ToString();
            }

            //seems to be fine...lets continue with the export; 


            return true;
        }//


        static bool ExportMaterials(MaterialsModelItem mats, string folder, string simulationName)
        {


            return true;
        }
        static bool ExportPressures(PressureModelItem press, string folder, string simulationName)
        {


            return true;
        }
        static bool ExportBoundaryConditions(BoundaryConditionsItem bcs, string folder, string simulationName)
        {


            return true;
        }
        //static bool ExportConfigfiles(BoundaryConditionsItem bcs, string folder, string simulationName)
        //{


        //    return true;
        //}




    }
}
