using Gigamodel.GigaModelData;
using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{

    public class VisageTools
    {
        public static float[] GetEdgeLoad(Object Datum, bool offshore, float gapDensity, float seaWaterDensity, SeismicCube dens)
        {
            float[] values = new float[dens.NumSamplesIJK.I * dens.NumSamplesIJK.J];
            int counter = 0;
            float offshoreFlag = offshore == true ? 1.0f : 0.0f;

            for (int i = 0; i < dens.NumSamplesIJK.I; i++)
            {
                for (int j = 0; j < dens.NumSamplesIJK.J; j++)
                {
                    Point3 p = dens.PositionAtIndex(new IndexDouble3(i, j, 0));
                    if (p.Z > 0.0f) values[counter++] = 0.0f;
                    else
                    {
                        float distanceToDatum = SeismicTools.DistanceToSurface(p, Datum);
                        float overburden = distanceToDatum < 0.0f ? 0.0f : gapDensity * 10.0f * distanceToDatum;

                        //seaweight at datum depth 
                        var waterHeight = offshoreFlag * (p.Z + distanceToDatum);  //this is the sea level. pz is negative 
                        overburden += (waterHeight < 0.0f ? (float)(-1.0f * 10.0 * seaWaterDensity * waterHeight) : 0.0f);
                        counter += 1;
                    }
                }
            }

            return values;
        }

        public static string WriteBoundaryConditions(Gigamodel gigaModel, string simulationName, string caseFolder)//, ref List<string> filesToInclude)
        {
            SimulationModelItem simObject = gigaModel.SimulationsModel.GetOrCreateModel(simulationName);
            BoundaryConditionsItem bc = gigaModel.BoundaryConditionsModels.GetOrCreateModel(simObject.BoundaryConditionsModelName);
            SeismicCube densityCube = gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName).Density;

            //lets start with the edge load. 
            Object datumObject = DataManager.Resolve(bc.DatumDroid);
            float[] edgeLoads = GetEdgeLoad(datumObject, bc.Offshore, bc.GapDensity, bc.SeaWaterDensity, densityCube);

            //write the edgeLoads as a single one-chunk binary object. This function can be async. 
            string edgeLoadFileName = simulationName + ".edg";
            OceanUtilities.SaveToDrive(edgeLoads, caseFolder + "\\" + edgeLoadFileName);

            string configFileSection = "\n\nBOUNDARIES:\n";
            configFileSection += ("\tEDGELOAD: " + edgeLoadFileName + "\n");
            configFileSection += ("\tMINSTRAIN: " + bc.MinStrain);
            configFileSection += ("\tMAXSTRAIN: " + bc.MaxStrain);
            configFileSection += ("\tMINSTRAINANGLE: " + bc.MinStrainAngle);
            configFileSection += ("\tDISPLACEMENTS: " + simulationName + ".dis\n");
            return configFileSection;
        }

        public static string WriteMatFiles(MaterialsModelItem mat, string simulationName, string matsFolder)//, ref List<string> filesToInclude)
        {

            List<SeismicCube> cubes = new List<SeismicCube>() { mat.Density, mat.YoungsModulus, mat.PoissonsRatio };
            List<string> baseFileNames = new List<string>() { "DENSITY", "YOUNGSMOD", "POISSONR" };
            string suffix = ".mat";

            bool success = true;
            string configFileSection = "\nELASTIC_DATA:\n";
            for (int n = 0; n < cubes.Count(); n++)
            {
                //success &= OceanUtilities.saveCube(OceanUtilities.splitBinaryCube(cubes[n]), matsFolder + "\\" + baseFileNames[n] + suffix);
                SplitCubeFloats.Serialize(OceanUtilities.splitBinaryCube(cubes[n]), matsFolder + "\\" + baseFileNames[n] + suffix);
                configFileSection += ("\t" + baseFileNames[n] + ": " + baseFileNames[n] + suffix + "\n");
            }

            return success ? configFileSection : null;

        }//filesToInclude.Add(simulationName + ".mat ");

        public static string WritePressures(PressureModelItem pressures, string simulationName, string caseFolder)//, ref List<string> filesToInclude)
        {
            List<SeismicCube> cubes = pressures.Cubes;
            bool success = true;
            List<string> files = new List<string>();

            for (int n = 0; n < cubes.Count(); n++)
            {
                string baseFileName = (("PRESSURES") + n.ToString("D4") + ".ppr");
                files.Add(baseFileName);// += ((n > 0 ? "," : "") + baseFileName);
                //success &= OceanUtilities.saveCube(OceanUtilities.splitBinaryCube(cubes[n]), caseFolder + "\\" + baseFileName);

                SplitCubeFloats.Serialize(OceanUtilities.splitBinaryCube(cubes[n]), caseFolder + "\\" + baseFileName);
            }
            if (!success) return null;

            string configFileSection = "\nPRESSURES:\n";
            configFileSection += ("\tPRESSURESFILES: " + string.Join(",", files) + "\n");
            configFileSection += ("\tPRESSUREDATES: " + (string.Join(",", pressures.Dates.Select(t => Utils.DateToString(t)))) + "\n");

            return configFileSection;
        }

        public static bool WriteConfigFiles(string configSections, string caseFolder, string simulationName)//, ref List<string> filesToInclude)
        {
            //        /*
            //         * 
            //         * CASENAME: xxx 
            //         ELASTIC: 
            //               YOUNGSMOD: FILE
            //               POISSONRAT: FILE 
            //               DENSITY: FILE
            //         PRESSURES:
            //               PRESSUREDATES: DATE1,DATE2,DATE3,...
            //               PRESSURES: PRESSURE1,PRESSURE2,PRESSURE3,....

            //         BOUNDARIES:
            //               EDGELOAD: FILE 
            //               DISPLACEMENTS: FILE 

            //         MII:  FILE 

            //         CTL:  FILE 

            //         GRID:
            //              SPACING: X,Y,Z 
            //              ORIGIN:  X,Y,Z 
            //              CELLS:   NX,NY,NZ  


            //         */

            File.WriteAllText(caseFolder + "\\" + simulationName + ".cfg", configSections);
            return true;
        }

        public static Dictionary<string, string> CreateFolders(string simulationName)
        {
            string dataFolder = GigaModelDefinitions.DataExportFolder;
            System.IO.Directory.CreateDirectory(dataFolder);

            string simExportFolder = GigaModelDefinitions.SimExportFolder;
            string caseFolder = simExportFolder + "\\" + simulationName;  //Path.Combine;
            string preDeckFolder = caseFolder + "\\PreDeck\\";  //Path.Combine;
            string[] preDeckSubFolders = { "\\PRESSURES", "\\MATERIALS", "\\BOUNDARIES", "\\EXTRA" };

            System.IO.Directory.CreateDirectory(simExportFolder);
            System.IO.Directory.CreateDirectory(caseFolder);
            System.IO.Directory.CreateDirectory(preDeckFolder);
            foreach (string s in preDeckSubFolders)
                System.IO.Directory.CreateDirectory(preDeckFolder + s);

            return new Dictionary<string, string>()
            { { "ApplicationFolder", dataFolder }, {"CaseFolder", caseFolder}, {"PreDeckFolder", preDeckFolder},
              { "ExtraFolder",       preDeckFolder + "\\EXTRA" },
              { "MaterialsFolder",   preDeckFolder + "\\MATERIALS" },
              { "PressuresFolder",   preDeckFolder + "\\PRESSURES" },
              { "BoundariesFolder",  preDeckFolder + "\\BOUNDARIES" }
            };
        }

        public static List<string> WriteDeck(string simulationName)
        {
            //check that we actually have a model 
            var gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();
            if ((gigaModel == null) || (!gigaModel.SimulationsModel.FindModel(simulationName)))//this is a software problem. Cannot create the gigamodel. 
            {
                return null;
            }

            SimulationModelItem simObject = gigaModel.SimulationsModel.GetOrCreateModel(simulationName);
            GridDimensions dims = OceanUtilities.GridDimensionsFromSeismicCube(gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName).YoungsModulus);

            //create the output folder 
            Dictionary<string, string> caseFolder = CreateFolders(simulationName);
            List<string> theFilesToIncludeInTheMII = new List<string>();

            string configSections = ("Version: 1.0\n");
            configSections += ("Compatible Versions: 1.0\n\n");

            configSections += ("CASENAME: " + simulationName);

            configSections += dims.ToString();

            configSections += (WriteMatFiles(gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName), simulationName, caseFolder["MaterialsFolder"]));//, ref theFilesToIncludeInTheMII));

            configSections += (WritePressures(gigaModel.PressureModels.GetOrCreateModel(simObject.PressureModelName), simulationName, caseFolder["PressuresFolder"]));//, ref theFilesToIncludeInTheMII));

            configSections += (WriteBoundaryConditions(gigaModel, simulationName, caseFolder["BoundariesFolder"]));//, ref theFilesToIncludeInTheMII));

            WriteConfigFiles(configSections, caseFolder["CaseFolder"], simulationName);//, ref theFilesToIncludeInTheMII);



            //for importing results we will need a reference seismic cube. So, we just write a file with the droid of one that we then later read.
            var mat = gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName).Droids[0];
            File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".Droid", mat.ToString());

            //also for the sake of importing dates quickly in the ui, we also save the dates in a separate file.
            var dates = gigaModel.PressureModels.GetOrCreateModel(simObject.PressureModelName).Dates;
            File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".Dates", string.Join(",", dates.Select(t => Utils.DateToString(t))));


            //we use a default MII. then customized for dimensions, etc 
            MII mii = new MII("2018.2");
            mii.ModelName = simulationName;
            mii.GridDimensions = dims;
            mii.Dates = dates;

            List<string> fileContens = mii.GetFileContents();
            string vbtFileContents = "";
            for (int n = 0; n < fileContens.Count(); n++)
            {
                string suffix = simulationName + n.ToString("D4") + ".MII";
                File.WriteAllText(caseFolder["CaseFolder"] + "\\" + suffix, fileContens[n]);
                vbtFileContents += (suffix + "\n");
            }
            File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".vbt", vbtFileContents);

            fileContens.Add(vbtFileContents);
            return fileContens;


        }

    };


}
