using Gigamodel.Data;
using Gigamodel.GigaModelData;
using Gigamodel.OceanUtils;
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

namespace Gigamodel.VisageUtils
{


    public class VisageOceanSeismicDeckWritter
    {
        public static Dictionary<string, string> CreateFolders(string simulationName, bool useStreams = true)
        {
            string dataFolder = GigaModelDataDefinitions.DataExportFolder;
            System.IO.Directory.CreateDirectory(dataFolder);

            string simExportFolder = GigaModelDataDefinitions.SimExportFolder;
            string caseFolder = simExportFolder + "\\" + simulationName;  //Path.Combine;

            string preDeckFolder = "";// caseFolder + "\\PreDeck\\";  //Path.Combine;
            string[] preDeckSubFolders = null;//

            System.IO.Directory.CreateDirectory(simExportFolder);
            System.IO.Directory.CreateDirectory(caseFolder);

            if (!useStreams)
            {
                preDeckFolder = caseFolder + "\\PreDeck\\";  //Path.Combine;
                preDeckSubFolders = new string[] { "\\PRESSURES", "\\MATERIALS", "\\BOUNDARIES", "\\EXTRA" };
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
            else
            {
                return new Dictionary<string, string>()
            { { "ApplicationFolder", dataFolder+ "\\" },
              { "CaseFolder", caseFolder+ "\\"},
              { "PreDeckFolder", caseFolder+ "\\"},
              { "ExtraFolder",       caseFolder + "\\" },
              { "MaterialsFolder",   caseFolder + "\\" },
              { "PressuresFolder",   caseFolder + "\\" },
              { "BoundariesFolder",  caseFolder + "\\" }
            };

            }

        }

        public static List<string> WriteDeck(string simulationName)
        {


            return null;
        }


        public static string WriteMatFiles(MaterialsModelItem mat, string simulationName, string matsFolder)//, ref List<string> filesToInclude)
        {

            List<SeismicCube> cubes = new List<SeismicCube>()
            {

                (SeismicCube)(DataManager.Resolve(new Droid(mat.Density.DroidString))),
                (SeismicCube)(DataManager.Resolve(new Droid(mat.YoungsModulus.DroidString))),
                (SeismicCube)(DataManager.Resolve(new Droid(mat.PoissonsRatio.DroidString)))
            };


            float[] scallingToVisageUnits = new float[] { (1.0f - 0.2f) * 10.0f, 0.001f, 1.0f };
            List<string> baseFileNames = new List<string>() { "DENSITY", "YOUNGSMOD", "POISSONR" };
            string suffix = ".mat";

            bool success = true;
            string configFileSection = "\nELASTIC_DATA:\n";
            for (int n = 0; n < cubes.Count(); n++)
            {
                SplitCubeFloats c = SeismicCubesUtilities.splitBinaryCube(cubes[n], scallingToVisageUnits[n]);
                success &= (SplitCubeFloats.Serialize(c, matsFolder + "\\" + baseFileNames[n] + suffix, true));
                configFileSection += ("\t" + baseFileNames[n] + ": " + baseFileNames[n] + suffix + "\n");
            }

            return success ? configFileSection : null;


        }//filesToInclude.Add(simulationName + ".mat ");

        public static string WritePressures(PressureModelItem pressures, string simulationName, string caseFolder)//, ref List<string> filesToInclude)
        {

            List<string> droids = pressures.Droids;
            List<SeismicCube> cubes = new List<SeismicCube>();// pressures.Cubes;

            foreach (string d in droids)
                cubes.Add((SeismicCube)(DataManager.Resolve(new Droid(d))));

            bool success = true;
            List<string> files = new List<string>();

            float scallingToVisageUnits = 0.001f; //Pa to KPa 
            for (int n = 0; n < cubes.Count(); n++)
            {
                string baseFileName = (("PRESSURES") + n.ToString("D4") + ".ppr");
                files.Add(baseFileName);// += ((n > 0 ? "," : "") + baseFileName);
                                        //success &= OceanUtilities.saveCube(OceanUtilities.splitBinaryCube(cubes[n]), caseFolder + "\\" + baseFileName);

                SplitCubeFloats c = SeismicCubesUtilities.splitBinaryCube(cubes[n], scallingToVisageUnits);
                success &= SplitCubeFloats.Serialize(c, caseFolder + "\\" + baseFileName, true);
            }
            if (!success) return null;

            string configFileSection = "\nPRESSURES:\n";
            configFileSection += ("\tPRESSURESFILES: " + string.Join(",", files) + "\n");
            configFileSection += ("\tPRESSUREDATES: " + (string.Join(",", pressures.Dates.Select(t => Utils.DateToString(t)))) + "\n");

            return configFileSection;
        }


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
                        float distanceToDatum = SeismicCubesUtilities.DistanceToSurface(p, Datum);
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




        public static async void SaveToDrive(float[] cube, string fileName)
        {
            await Task.Run(() => slowWriteFloats(cube, fileName));
            Console.WriteLine("Writting done");



        }
        public static void slowWriteFloats(float[] cube, string fileName)
        {
            var tempBuffer = new byte[cube.Count() * sizeof(float)];
            Buffer.BlockCopy(cube, 0, tempBuffer, 0, cube.Count() * sizeof(float));
            using (BinaryWriter writter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writter.Write(tempBuffer);
            }
        }


        public static string WriteBoundaryConditions(BoundaryConditionsItem bcItem, MaterialsModelItem matItem, string simulationName, string caseFolder, SeismicCube referenceCube)//, ref List<string> filesToInclude)
        {
            string configSections = string.Empty;

            configSections += WriteDisplacementsFile(bcItem, matItem, simulationName, caseFolder, referenceCube);//, ref List<string> filesToInclude)

            configSections += WriteEdgeLoadsFile(bcItem, matItem, simulationName, caseFolder);//, ref List<string> filesToInclude)

            return configSections;





            //// SimulationModelItem simObject = gigaModel.SimulationsModel.GetOrCreateModel(simulationName);
            //BoundaryConditionsItem bc = bcItem;// gigaModel.BoundaryConditionsModels.GetOrCreateModel(simObject.BoundaryConditionsModelName);
            //SeismicCube densityCube = (SeismicCube)(DataManager.Resolve(new Droid(matItem.Density.DroidString)));

            ////lets start with the edge load. 
            //Droid droid = Droid.TryParse(bc.DatumDroid); 
            //Object datumObject = droid == Droid.Empty ? null : DataManager.Resolve( droid  );
            //float[] edgeLoads = GetEdgeLoad(datumObject, bc.Offshore, bc.GapDensity, bc.SeaWaterDensity, densityCube);                     

            ////write the edgeLoads as a single one-chunk binary object. This function can be async. 
            //string edgeLoadFileName = simulationName + ".edg";
            //SaveToDrive(edgeLoads, caseFolder + "\\" + edgeLoadFileName);

            ////the displacement file. This code was first written in c++ and now migrated here. 
            ////it may have redundant instructions that in the c++ code in the server were needed but not here. 







            //string configFileSection = "\n\nBOUNDARIES:\n";
            //configFileSection += ("\tEDGELOAD: " + edgeLoadFileName + "\n");
            //configFileSection += ("\tMINSTRAIN: " + bc.MinStrain);
            //configFileSection += ("\tMAXSTRAIN: " + bc.MaxStrain);
            //configFileSection += ("\tMAXSTRAINANGLE: " + bc.MaxStrainAngle);
            //configFileSection += ("\tDISPLACEMENTS: " + simulationName + ".dis\n");
            //return configFileSection;
        }

        public static string WriteEdgeLoadsFile(BoundaryConditionsItem bcItem, MaterialsModelItem matItem, string simulationName, string caseFolder)//, ref List<string> filesToInclude)
        {
            // SimulationModelItem simObject = gigaModel.SimulationsModel.GetOrCreateModel(simulationName);
            BoundaryConditionsItem bc = bcItem;// gigaModel.BoundaryConditionsModels.GetOrCreateModel(simObject.BoundaryConditionsModelName);
            SeismicCube densityCube = (SeismicCube)(DataManager.Resolve(new Droid(matItem.Density.DroidString)));

            //lets start with the edge load. 
            Droid droid = Droid.TryParse(bc.DatumDroid);
            Object datumObject = droid == Droid.Empty ? null : DataManager.Resolve(droid);
            float[] edgeLoads = GetEdgeLoad(datumObject, bc.Offshore, bc.GapDensity, bc.SeaWaterDensity, densityCube);

            //write the edgeLoads as a single one-chunk binary object. This function can be async. 
            string edgeLoadFileName = simulationName + ".edg";
            SaveToDrive(edgeLoads, caseFolder + "\\" + edgeLoadFileName);

            string configFileSection = "\n\nBOUNDARIES:\n";
            configFileSection += ("\tEDGELOAD: " + edgeLoadFileName + "\n");
            return configFileSection;
        }


        public static string WriteDisplacementsFile(BoundaryConditionsItem bcItem, MaterialsModelItem matItem, string simulationName, string caseFolder, SeismicCube referenceCube)//, ref List<string> filesToInclude)
        {

            GridDimensions dims = SeismicCubesUtilities.GridDimensionsFromSeismicCube(referenceCube);
            //path should be inside the dims
            Point3 po = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
            Point3 p1 = referenceCube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
            Point3 p2 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 1.0, 0.0));
            Point3 p3 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 1.0));

            var vI = p1 - po;
            var vJ = p2 - po;
            var vK = p3 - po;

            double mod = Math.Sqrt(vI.X * vI.X + vI.Y * vI.Y + vI.Z * vI.Z);
            double[] directionIncreaseI = { vI.X / mod, vI.Y / mod, vI.Z / mod };

            mod = Math.Sqrt(vJ.X * vJ.X + vJ.Y * vJ.Y + vJ.Z * vJ.Z);
            double[] directionIncreaseJ = { vJ.X / mod, vJ.Y / mod, vJ.Z / mod };

            mod = Math.Sqrt(vK.X * vK.X + vK.Y * vK.Y + vK.Z * vK.Z);
            double[] directionIncreaseK = { vK.X / mod, vK.Y / mod, vK.Z / mod };


            //the displacement file. This code was first written in c++ and now migrated here. 
            //it may have redundant instructions that in the c++ code in the server were needed but not here. 
            //so....who is pointing North ? 
            double maxStrain = bcItem.MaxStrain;
            double minStrain = bcItem.MinStrain;
            double maxStrainAngle = bcItem.MaxStrainAngle;

            double IProjectedNorth = 0.0;
            double JProjectedNorth = 0.0;
            double[] north = { 0.0, 1.0, 0.0 };
            double[] length = { dims.Spacing[0] * (dims.Cells[0]), dims.Spacing[1] * (dims.Cells[1]) };

            double signI = 1.0, signJ = 1.0;
            double displacementI;int visageDirectionForDisplacementI;
            double displacementJ;int visageDirectionForDisplacementJ;


            for (int d = 0; d < 3; d++)
            {
                IProjectedNorth += Math.Abs((directionIncreaseI[d] * north[d]));
                JProjectedNorth += Math.Abs((directionIncreaseJ[d] * north[d]));
            }
            if (IProjectedNorth >= JProjectedNorth) //then I is pointing north. It shoould get the max strain if angle is zero and min strain if 90. This is the FIVESIZSEVEN Example 
            {
                //strain
                displacementI = 0.5 * length[0] * (Math.Abs(maxStrainAngle) < 0.001 ? maxStrain : minStrain);  //the ui only allows angles of either 0 or 90 degrees for now. 
                displacementJ = 0.5 * length[1] * (Math.Abs(maxStrainAngle) < 0.001 ? minStrain : maxStrain);

                //visage direction. I know the displacements along North will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirI = 3 and sign = -1.0; 
                visageDirectionForDisplacementI = 3;

                //the other direction is Petrel x, which is x in visage. So:
                visageDirectionForDisplacementJ = 1;

                signI = -1.0;
            }
            else
            {
                //pretty much the same, but we change I and J 
                //now J is pointing North. So, if the angle is small, the max strain is poiting north, i.e. J
                //strain
                displacementJ = 0.5 * length[1] * (Math.Abs(maxStrainAngle) < 0.001 ? maxStrain : minStrain);
                displacementI = 0.5 * length[0] * (Math.Abs(maxStrainAngle) < 0.001 ? minStrain : maxStrain);

                //visage direction. I know the displacements along north will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirJ = 3 and sign = -1.0; 
                visageDirectionForDisplacementJ = 3;

                //the other direction is Petrel x, which is x in visage. So:
                visageDirectionForDisplacementI = 1;

                signJ = -1.0;

            }



            string fileName = Path.Combine(caseFolder, simulationName + ".dis");
            using (System.IO.StreamWriter ofs = new System.IO.StreamWriter(fileName))
            {
                ofs.WriteLine("*DISPLACEMENTS, N");
                int[] nodesDims = { 1 + dims.Cells[0], 1 + dims.Cells[1], 1 + dims.Cells[2] }; //nodes 
                int dirK = 2; 

                int i, j, counter = 0;
                for (i = 0; i < nodesDims[0]; i++)  //	j = 0;
                    for (j = 0; j < nodesDims[1]; j++)  //	j = 0;
                        for (int k = 0; k < nodesDims[2]; k++)  //	j = 0;	
                        {
                            if (i == 0)
                                writeElementalDisplacement( ofs, counter + 1, visageDirectionForDisplacementI, displacementI * signI);
                            else if (i == nodesDims[0] - 1)
                                writeElementalDisplacement( ofs, counter + 1, visageDirectionForDisplacementI, -displacementI * signI);
                            else {; }


                            if (j == 0)
                                writeElementalDisplacement( ofs, counter + 1, visageDirectionForDisplacementJ, displacementJ * signJ);

                            else if (j == nodesDims[1] - 1)
                                writeElementalDisplacement( ofs, counter + 1, visageDirectionForDisplacementJ, -displacementJ * signJ);
                            else {; }


                            if (k == 0)
                            {;//	writeElementalDisplacement(ofs, counter + 1, dirK, -33.0);
                            }
                            else if (k == nodesDims[2] - 1)
                            {
                                writeElementalDisplacement(ofs, counter + 1, dirK, 0.0);
                            }
                            else {; }
                            counter++;
                        }

            }

            string configFileSection = dims.ToString();
            configFileSection += "\n";
            configFileSection += ("DirectionI: " + directionIncreaseI[0] + " " + directionIncreaseI[1] + " " + directionIncreaseI[2] + "\n");
            configFileSection += ("DirectionJ: " + directionIncreaseJ[0] + " " + directionIncreaseJ[1] + " " + directionIncreaseJ[2] + "\n");
            configFileSection += ("DirectionK: " + directionIncreaseK[0] + " " + directionIncreaseK[1] + " " + directionIncreaseK[2] + "\n");
            configFileSection += "\n";
            configFileSection += ("\tMINSTRAIN: " + bcItem.MinStrain);
            configFileSection += ("\tMAXSTRAIN: " + bcItem.MaxStrain);
            configFileSection += ("\tMAXSTRAINANGLE: " + bcItem.MaxStrainAngle);
            configFileSection += ("\tDISPLACEMENTS: " + simulationName + ".dis\n");
            configFileSection += "\n";


            return configFileSection;



            /*
            
            double* strain = new double[2] { data.minStrain, data.maxStrain };
            double* spacing = &(data.spacing[0]);
    
            double length[2] = { spacing[0] * (data.cells[0]), spacing[1] * (data.cells[1]) };






            //so, the MAXSTRAINANGLE IS MEASURED IN RESPECT TO NORTH. WHICH VECTOR POINTS NORTH HERE? I OR J 
            //THATS THE DIRECTION OF THE MAXIMUM APPLIED STRAIN. So there are several potential scenarios. 
            //FOR US, NORTH IS ALWAYS (0,1,0)
            double displacementI, visageDirectionForDisplacementI;
            double displacementJ, visageDirectionForDisplacementJ;
            int dirI = 3;
            int dirJ = 1;
            int dirK = 2;

            //so....who is pointing North ? 
            double IProjectedNorth = 0.0;
            double JProjectedNorth = 0.0;
            double north[] = { 0.0, 1.0, 0.0 };
            double signI = 1.0, signJ = 1.0;
            for (int d = 0; d < DIMS; d++)
            {
                IProjectedNorth += fabs((data.directionIncreaseI[d] * north[d]));
                JProjectedNorth += fabs((data.directionIncreaseJ[d] * north[d]));
            }
            if (IProjectedNorth >= JProjectedNorth) //then I is pointing north. It shoould get the max strain if angle is zero and min strain if 90. This is the FIVESIZSEVEN Example 
            {
                //strain
                displacementI = 0.5 * length[0] * (fabs(data.maxStrainAngle) < 0.001 ? maxStrain : minStrain);  //the ui only allows angles of either 0 or 90 degrees for now. 
                displacementJ = 0.5 * length[1] * (fabs(data.maxStrainAngle) < 0.001 ? minStrain : maxStrain);

                //visage direction. 
                //I know the displacements along North will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirI = 3 and sign = -1.0; 
                visageDirectionForDisplacementI = 3;

                //the other direction is Petrel x, which is x in visage. So:
                visageDirectionForDisplacementJ = 1;

                signI = -1.0;
            }
            else
            {
                //pretty much the same, but we change I and J 
                //now J is pointing North. So, if the angle is small, the max strain is poiting north, i.e. J
                //strain
                displacementJ = 0.5 * length[1] * (fabs(data.maxStrainAngle) < 0.001 ? maxStrain : minStrain);
                displacementI = 0.5 * length[0] * (fabs(data.maxStrainAngle) < 0.001 ? minStrain : maxStrain);

                //visage direction. 
                //I know the displacements along north will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirJ = 3 and sign = -1.0; 
                visageDirectionForDisplacementJ = 3;

                //the other direction is Petrel x, which is x in visage. So:
                visageDirectionForDisplacementI = 1;

                signJ = -1.0;
            }

            */





        }

        static void writeElementalDisplacement( System.IO.StreamWriter file, int element, int direction, double displacement )
        {
            file.WriteLine(element + "   " + direction + "   " + displacement);
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


        public static List<string> WriteDeck(SimulationModelItem item, GigaModelDataModel model, out bool error)
        {
            error = false;
            try
            {
                //create the output folder 
                Dictionary<string, string> caseFolder = CreateFolders(item.Name);

                string simulationName = item.Name;

                string referenceDroid = model.MaterialModels.GetOrCreateModel(item.MaterialModelName).Droids[0];
                SeismicCube referenceCube = (SeismicCube)(DataManager.Resolve(new Droid(referenceDroid)));

                //get grid dims. Needed to write the deck. use ym as a reference cube 
                GridDimensions dims = SeismicCubesUtilities.GridDimensionsFromSeismicCube(referenceCube);
                string configSections = ("Version: 1.0\n");
                configSections += ("Compatible Versions: 1.0\n\n");
                configSections += ("CASENAME: " + item.Name);
                configSections += dims.ToString();



                //path should be inside the dims
                Point3 po = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
                Point3 p1 = referenceCube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
                Point3 p2 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 1.0, 0.0));
                Point3 p3 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 1.0));

                var vI = p1 - po;
                var vJ = p2 - po;
                var vK = p3 - po;

                double mod = Math.Sqrt(vI.X * vI.X + vI.Y * vI.Y + vI.Z * vI.Z);
                double[] dirI = { vI.X / mod, vI.Y / mod, vI.Z / mod };

                mod = Math.Sqrt(vJ.X * vJ.X + vJ.Y * vJ.Y + vJ.Z * vJ.Z);
                double[] dirJ = { vJ.X / mod, vJ.Y / mod, vJ.Z / mod };

                mod = Math.Sqrt(vK.X * vK.X + vK.Y * vK.Y + vK.Z * vK.Z);
                double[] dirK = { vK.X / mod, vK.Y / mod, vK.Z / mod };


                configSections += ("DirectionI: " + dirI[0] + " " + dirI[1] + " " + dirI[2] + "\n");
                configSections += ("DirectionJ: " + dirJ[0] + " " + dirJ[1] + " " + dirJ[2] + "\n");
                configSections += ("DirectionK: " + dirK[0] + " " + dirK[1] + " " + dirK[2] + "\n");





                MaterialsModelItem mats = model.MaterialModels.GetOrCreateModel(item.MaterialModelName);
                configSections += (WriteMatFiles(mats, simulationName, caseFolder["MaterialsFolder"]));

                PressureModelItem pressures = model.PressureModels.GetOrCreateModel(item.PressureModelName);
                configSections += (WritePressures(pressures, simulationName, caseFolder["PressuresFolder"]));//, ref theFilesToIncludeInTheMII));

                BoundaryConditionsItem bcItem = model.BoundaryConditionsModels.GetOrCreateModel(item.BoundaryConditionsModelName);
                configSections += (WriteBoundaryConditions(bcItem, mats, simulationName, caseFolder["BoundariesFolder"], referenceCube));//, ref theFilesToIncludeInTheMII));


                WriteConfigFiles(configSections, caseFolder["CaseFolder"], simulationName);//, ref theFilesToIncludeInTheMII);

                //for importing results we will need a reference seismic cube. So, we just write a file with the droid of one that we then later read.
                File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".Droid", referenceDroid);

                //also for the sake of importing dates quickly in the ui, we also save the dates in a separate file.
                var dates = pressures.Dates;
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

            catch (Exception e)
            {
                error = true;
                return new List<string>() { e.ToString() };
            }


        }


        public static List<string> WriteDeckCompressedBinary(SimulationModelItem item, GigaModelDataModel model, bool UseStreams, out bool error, Slb.Ocean.Petrel.IProgress bar = null)
        {
            error = false;
            int progress = 0; 
            if (bar != null) bar.ProgressStatus = 0;
            try
            {
                //create the output folder 
                string simulationName = item.Name;
                Dictionary<string, string> caseFolder = CreateFolders(simulationName, UseStreams);
                string referenceDroid = model.MaterialModels.GetOrCreateModel(item.MaterialModelName).Droids[0];
                SeismicCube referenceCube = (SeismicCube)(DataManager.Resolve(new Droid(referenceDroid)));

                //get grid dims. Needed to write the deck. use ym as a reference cube 
                GridDimensions dims = SeismicCubesUtilities.GridDimensionsFromSeismicCube(referenceCube);
                string configSections = ("Version: 2.0\n");
                configSections += ("Compatible Versions: 2.0\n\n");
                configSections += ("CASENAME: " + item.Name);


                ////path should be inside the dims
                //Point3 po = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
                //Point3 p1 = referenceCube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
                //Point3 p2 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 1.0, 0.0));
                //Point3 p3 = referenceCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 1.0));

                //var vI = p1 - po;
                //var vJ = p2 - po;
                //var vK = p3 - po;

                //double mod = Math.Sqrt(vI.X * vI.X + vI.Y * vI.Y + vI.Z * vI.Z);
                //double[] dirI = { vI.X / mod, vI.Y / mod, vI.Z / mod };

                //mod = Math.Sqrt(vJ.X * vJ.X + vJ.Y * vJ.Y + vJ.Z * vJ.Z);
                //double[] dirJ = { vJ.X / mod, vJ.Y / mod, vJ.Z / mod };

                //mod = Math.Sqrt(vK.X * vK.X + vK.Y * vK.Y + vK.Z * vK.Z);
                //double[] dirK = { vK.X / mod, vK.Y / mod, vK.Z / mod };


                //configSections += ("DirectionI: " + dirI[0] + " " + dirI[1] + " " + dirI[2] + "\n");
                //configSections += ("DirectionJ: " + dirJ[0] + " " + dirJ[1] + " " + dirJ[2] + "\n");
                //configSections += ("DirectionK: " + dirK[0] + " " + dirK[1] + " " + dirK[2] + "\n");          

                MaterialsModelItem mats = model.MaterialModels.GetOrCreateModel(item.MaterialModelName);
                PressureModelItem pressures = model.PressureModels.GetOrCreateModel(item.PressureModelName);
                BoundaryConditionsItem bcItem = model.BoundaryConditionsModels.GetOrCreateModel(item.BoundaryConditionsModelName);

                int totalCubes = pressures.Names.Count() + mats.Cubes.Count();


                configSections += (WriteBoundaryConditions(bcItem, mats, simulationName, caseFolder["BoundariesFolder"], referenceCube));//, ref theFilesToIncludeInTheMII));
                bar.ProgressStatus = 10; 

                           configSections += (WriteMatFiles(mats, simulationName, caseFolder["MaterialsFolder"]));
                if (bar != null) bar.ProgressStatus = 40;// Math.Min(100, (int)(mats.Cubes.Count() * 100.0 / totalCubes));

                configSections += (WritePressures(pressures, simulationName, caseFolder["PressuresFolder"]));//, ref theFilesToIncludeInTheMII));
                if (bar != null) bar.ProgressStatus = 90;


                WriteConfigFiles(configSections, caseFolder["CaseFolder"], simulationName);//, ref theFilesToIncludeInTheMII);

                //for importing results we will need a reference seismic cube. So, we just write a file with the droid of one that we then later read.
                File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".Droid", referenceDroid);

                //also for the sake of importing dates quickly in the ui, we also save the dates in a separate file.
                var dates = pressures.Dates;
                File.WriteAllText(caseFolder["CaseFolder"] + "\\" + simulationName + ".Dates", string.Join(",", dates.Select(t => Utils.DateToString(t))));

                //we use a default MII. then customized for dimensions, etc 
                MII mii = new MII("2018.2");
                mii.ModelName = simulationName;
                mii.GridDimensions = dims;
                mii.Dates = dates;
                mii.StreamKeywordEnabled = UseStreams;


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

                bar.ProgressStatus = 100; 

                return fileContens;
            }

            catch (Exception e)
            {
                error = true;
                return new List<string>() { e.ToString() };
            }
        }



    };




}
