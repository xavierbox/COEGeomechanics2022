using Restoration.GFM;
using Restoration.GFM.Model;
using Restoration.GFM.Services;
using Restoration.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Restoration.Controllers
{
    internal class GFMController
    {
        private GFMGeometryUI _ui;
        private GFMProject _model;
        private IGFMSerializer _serializer;

        //public event EventHandler<SimulationEventArgs> GenerateSimulationModels;

        public GFMController( GFMGeometryUI gui, GFMProject model, IGFMSerializer SerializerI )
        {
            _ui = gui;
            _model = model;
            _serializer = SerializerI;

            CreateApplicationLogic();
        }

        private void CreateApplicationLogic()
        {
            _ui.CancelClicked += ( sender, evt ) =>
            {
                UpdateModelFromUI();
                _serializer.PersistModel(_model);
            };

            _ui.GenerateProject += ( sender, evt ) =>
            {
                UpdateModelFromUI();

                string modelName = _model.Name;
                string projectFolder = Path.Combine(GetGFMFolder(), modelName);
                string uiRestorationFolder = _ui.RestorationFolder;
                Dictionary<string, DirectoryInfo> dirs = GenerateProjectFolders(projectFolder);
                if (dirs == null)
                {
                    MessageService.ShowMessage("Cannot create project folders.\nPlease check the name given to the folder and permissions to write in the .ptd folder", MessageType.ERROR);
                    return;
                }

                string prjRestorationFolderTarget = dirs["Restoration"].FullName;
                CopyNewRestorationDataToProjectFolder(_ui.TriangulatedSurfaceFilesToCopy, prjRestorationFolderTarget);
                string error;
                GenerateLithoFiles(projectFolder, out error);
                if (error != string.Empty)
                {
                    MessageService.ShowMessage(error, MessageType.ERROR);
                    return;
                }

                _model.RestorationFolder = prjRestorationFolderTarget;
                _serializer.SeriealizeModel(_model, Path.Combine(projectFolder, modelName + ".prj"));

                _ui.UIFromProject(_model);
                System.Windows.Forms.MessageBox.Show("Project files generated and stored in the project folder", "Project created/updated", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); ;

                //MessageBox.Show("Project generated", MessageType.INFO);
            };


            _ui.GenerateSimulationModels += ( sender, evt ) =>
            {
                //generate project folders if they werent generated before.
                Dictionary<string, DirectoryInfo> dirs = GenerateProjectFolders(Path.Combine(GetGFMFolder(), _model.Name));
                if (dirs == null)
                {
                    MessageService.ShowMessage("Cannot Generate Project Folders\nDo you have Write Permissions?\nPlease check the project name", MessageType.ERROR);
                    return;
                }

                var w = ProjectTools.GetInstallationFolder();

                //copy the exec files to the Restoration folder
                Array.ForEach(Directory.GetFiles(ProjectTools.GetInstallationFolder(), "*.exe"), delegate ( string execFile )
                {
                    string targetFile = Path.Combine(dirs["Restoration"].FullName, Path.GetFileName(execFile));
                    File.Copy(execFile, targetFile, true);
                });

                CommonData.Vector3[] boundaryPoints = (CommonData.Vector3[])(evt.BoundaryPoints);
                if (evt.Dims == 1)
                {
                    //each point in the boundary is actually a new simulation. Col001, Col002, ....
                    int counter = 0;
                    foreach (CommonData.Vector3 pt in boundaryPoints)
                    {
                        string simulationGroupName = evt.Name;
                        string simPreffixName = "Col" + (1 + counter).ToString().PadLeft(3, '0');          //in 1D resolution = 100 always
                        GenerateSimulationDeck(dirs, 1, 1, new CommonData.Vector3[] { pt }, simulationGroupName, simPreffixName, evt.Resolution, evt.LaunchVisageAfterDeck); //pass an array of 1 single point.
                        counter += 1;
                    }
                }
                else if (evt.Dims == 2)
                {
                    ;
                }
                else if (evt.Dims == 3)
                {
                    string simulationGroupName = evt.Name;
                    string simPreffixName = "Vol" + (0).ToString().PadLeft(3, '0');          //in 1D resolution = 100 always
                    GenerateSimulationDeck(dirs, 3, evt.Cores, boundaryPoints, simulationGroupName, simPreffixName, evt.Resolution, evt.LaunchVisageAfterDeck); //pass an array of 1 single point.
                }
            };

            _ui.LoadModelClicked += ( sender, evt ) =>
            {
                string modelFolder = evt.Value;
                string[] prjFile = Directory.GetFiles(modelFolder).Where(t => Path.GetExtension(t) == ".prj").ToArray();

                if (prjFile.Count() < 1) return;

                GFMProject p = _serializer.Deserialize(prjFile[0]);
                _model.DeppCopy(p);
                _ui.UIFromProject(_model);
            };

            _ui.Request1DSimulationList += ( sender, evt ) =>
            {
                DirectoryInfo sims1D = GetProjectFolders(Path.Combine(GetGFMFolder(), _model.Name))["1D"];
                Dictionary<string, List<string>> toReturn = new Dictionary<string, List<string>>();
                if (sims1D.Exists == false)
                {
                    MessageService.ShowMessage("1D Simulations have not been created yet.", MessageType.INFO);
                }
                else
                {
                    string[] simulationNames = Directory.GetDirectories(sims1D.FullName).ToArray(); ;
                    foreach (string simulation in simulationNames)
                        toReturn.Add(simulation, Directory.GetDirectories(simulation).ToList());
                    _ui.Update1DSimulationList(toReturn);
                }
            };

            _ui.Request1DSimulationResults += ( sender, evt ) =>
            {
                List<string> folderNames1DSimulations = (List<string>)(evt.Value);
                foreach (string folderName in folderNames1DSimulations)
                {
                    //get the litho file if any. We need the split and the name to return the results.
                    string[] lithoFile = Directory.GetFiles(folderName, "*.litho");
                    if (lithoFile.Count() < 1) continue;
                    LithoData l = LithoData.FromVisageLithoFile(lithoFile[0]);

                    string[] fgridFiles = GetFirstAndLastFGridFiles(folderName, "*_WARP_*.FGRID");
                    if (fgridFiles == null) continue;

                    int layerStart = 0;
                    Dictionary<string, double> thicknessValues = new Dictionary<string, double>();
                    List<double> thickness = GetCellThicknessFromWRAPFGridFile(fgridFiles[1]);
                    foreach (LayerLithoData layer in l.Layers)
                    {
                        string name = layer.Layer;
                        int split = layer.Split;
                        double sum = 0.0;
                        for (int k = layerStart; k < layerStart + split; k++) sum += thickness.ElementAt(k);
                        layerStart += split;

                        thicknessValues.Add(name, sum);
                    }
                    var parentFolder = Directory.GetParent(folderName).Name;
                    var seriesName = parentFolder + "[" + Path.GetFileName(folderName) + "]";
                    _ui.DisplayThicknessValues(seriesName, thicknessValues);

                    //now the present day
                    var grdFiles = GetFirstAndLastFGridFiles(folderName, "*.GRD");
                    if (grdFiles == null) continue;

                    string presetDayGrid = grdFiles.ElementAt(grdFiles.Count() - 1);
                    thickness = GetCellThicknessFromGRDFile(presetDayGrid);
                    Dictionary<string, double> presentDayThickness = new Dictionary<string, double>();
                    layerStart = 0;
                    foreach (LayerLithoData layer in l.Layers)
                    {
                        string name = layer.Layer;
                        int split = layer.Split;
                        double sum = 0.0;
                        for (int k = layerStart; k < layerStart + split; k++) sum += thickness.ElementAt(k);
                        layerStart += split;

                        presentDayThickness.Add(name, sum);
                    }
                    seriesName = parentFolder + "[" + Path.GetFileName(folderName) + "] Present";
                    _ui.DisplayThicknessValues(seriesName, presentDayThickness);
                }
            };
        }

        private void GenerateSimulationDeck( Dictionary<string, DirectoryInfo> dirs, int cores, int dims, CommonData.Vector3[] boundaryPoints, string simulationGroupName, string simulationPreffixName, int resolution, bool launchSimulation = false )
        {
            //make a list of all the files that were there before any new file was created. We need this because the fortran exec
            //creates a bunch of files, we cant control their names whatsoever and it is just better to copy everything just written to the sim folder.
            //for that, we need to track what files were there before.

            DirectoryInfo targetDir = dirs["Restoration"];
            var originalFiles = Directory.GetFiles(targetDir.FullName);

            //create boundary file.
            using (StreamWriter file = new StreamWriter(Path.Combine(targetDir.FullName, "ModelBoundary.txt")))
            {
                //if boundary contains only one point, then it is a 1D simulation, otherwise, it is a 3D sim. This is handled inside
                file.Write(SimulationBoundary.AsStringDescription(boundaryPoints));
            }

            //copy litho, horizons, steps, etc... from main to the restoration
            try
            {
                //string tmpName = "Col" + simulationCounter.ToString().PadLeft(3, '0'); ;
                string[] extensions = { ".litho", ".steps", ".horizons", "001_vol.dvt" };
                foreach (string s in extensions)
                {
                    string litho = Path.Combine(dirs["Main"].FullName, _model.Name + s);
                    string target = Path.Combine(targetDir.FullName, simulationPreffixName + s);
                    File.Copy(litho, target, true);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Cannot generate simulation.\nProject files such as .litho, or .steps cannot be found.\nDid you generate the project first?", "Missing data", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            #region launch the process

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(targetDir.FullName, "GFALauncher.bat")))
            {
                file.WriteLine("GFA.exe -PQDEOLY " + simulationPreffixName + " " + resolution);
            }

            //launch deck generation
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = targetDir.FullName;
            startInfo.FileName = "GFALauncher.bat";
            startInfo.Arguments = "";// "-PQDEOLY " + simulationPreffixName + " " + resolution + " ";
            Process p = new Process();
            p.StartInfo = startInfo;

            bool gfaFailed = false;
            try
            {
                p.Start();
                p.WaitForExit();

                var exitCode = p.ExitCode;
                gfaFailed = exitCode != 0 ? true : false;

                if (gfaFailed)
                {
                    CleanRestorationFolder(dirs["Restoration"]);
                    throw (new Exception("GFA returned a code different from zero. It may have failed"));
                }
            }
            catch (Exception e)
            {
                string why = e.ToString(); //file not found ?
                System.Windows.MessageBox.Show("Cannot generate the simulation files.\nGFA not available or crashed?", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            #endregion launch the process

            #region CrazyThingWithDVT

            //the exec that Assef created generates miis with a keyword for dvt tables. However, we dont know what the name of the file in the mii is defined.
            //so we get one file, figure out what is the name of the dvt file expected and then write our dvt table with that name

            //this is in the project main folder. dirs[Main]
            string currentDVTFileName = Path.Combine(dirs["Main"].FullName, _model.Name + "001_vol.dvt");//simulationPreffixName + "001_vol.dvt";
            Array.ForEach(Directory.GetFiles(targetDir.FullName, "*.mii"), delegate ( string miiFileName )
            {
                string[] lines = System.IO.File.ReadAllLines(miiFileName).Where(t => t.Contains(".dvt")).ToArray();
                if (lines.Count() > 0)
                {
                    var words = lines.ElementAt(0).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(t => t.Contains(".dvt"));
                    if (words.Count() > 0)
                    {
                        string dvtFileNameInMII = words.ElementAt(0);
                        //create a dvt file with that name but the contents of the current one
                        string dvtTarget = Path.Combine(targetDir.FullName, dvtFileNameInMII);
                        File.Copy(currentDVTFileName, dvtTarget, true);
                    }
                }
            });

            #endregion CrazyThingWithDVT

            #region Copy all the new files to the 1D sim folder

            string[] newFiles = Directory.GetFiles(targetDir.FullName).Where(t => ((originalFiles.Contains(t) == false) || (t.Contains("Boundary")))).ToArray();

            //now we copy any new file to the final simulation folder. The simulation folder is:
            DirectoryInfo info = Directory.CreateDirectory(Path.Combine(dirs[dims == 1 ? "1D" : "3D"].FullName, simulationGroupName, simulationPreffixName));

            //delete whatever was there before
            Array.ForEach(Directory.GetFiles(info.FullName), delegate ( string path ) { File.Delete(path); });
            Array.ForEach(Directory.GetDirectories(info.FullName), delegate ( string path ) { Directory.Delete(path); });

            //move the newly created files there
            Array.ForEach(newFiles, delegate ( string file ) { File.Move(file, Path.Combine(info.FullName, Path.GetFileName(file))); });

            //try
            //{
            //    //copy all the .exe and all the .dll files to the sim folder
            //    //Array.ForEach(Directory.GetFiles(dirs["Restoration"].FullName, "*.exe"), delegate (string file) { File.Copy(file, Path.Combine(info.FullName, Path.GetFileName(file)), true); });
            //    //Array.ForEach(Directory.GetFiles(dirs["Restoration"].FullName, "*.dll"), delegate (string file) { File.Copy(file, Path.Combine(info.FullName, Path.GetFileName(file)), true); });
            //}
            //catch
            //{
            //    ;
            //}
            CleanRestorationFolder(dirs["Restoration"]);

            #endregion Copy all the new files to the 1D sim folder

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(info.FullName, "batch.vbt")))
            {
                foreach (var miiFile in Directory.GetFiles(info.FullName, "*a0*.mii"))
                    file.WriteLine(Path.GetFileName(miiFile));
            }

            #region launch simulation if needed and if possible

            if ((launchSimulation) && (gfaFailed == false))
            {
                ProcessStartInfo startInfo2 = new ProcessStartInfo();
                startInfo2.UseShellExecute = true;
                startInfo2.WorkingDirectory = info.FullName;
                startInfo2.FileName = "eclrun";// "eclrun" + " --np ="+cores+" -e ./visage_ilmpi.exe visage batch.vbt";// --np=" + cores+" -e ./visage_ilmpi.exe visage batch.vbt";// "eclrun -e ./visage_ilmpi.exe visage --np="+cores+"  batch.vbt";
                startInfo2.Arguments = " --np=" + cores + " visage batch.vbt";// --np="+cores+" - e ./visage_ilmpi.exe visage batch.vbt";// "eclrun - e./ visage_ilmpi.exe visage--np = "+cores+"  batch.vbt";

                Process p2 = new Process();
                p2.StartInfo = startInfo2;
                try
                {
                    p2.Start();
                }
                catch (Exception ee)
                {
                    string why = ee.ToString();
                    System.Windows.MessageBox.Show("Cannot launch the simulation.\nVISAGE not available ?", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
            }
        }

        #endregion launch simulation if needed and if possible

        private void CleanRestorationFolder( DirectoryInfo folder )
        {
            if ((folder == null) || (!folder.Exists)) return;

            //delete from the restoration folder anything that is not a .msh, .vgl, .ts or exe
            string[] allowedExtensionsToRemain = new string[] { ".msh", ".ts", ".exe", ".dll", ".vgl", "txt" };
            Array.ForEach(Directory.GetFiles(folder.FullName), delegate ( string file )
          {
              if (!allowedExtensionsToRemain.Contains(Path.GetExtension(file)))
              {
                  File.Delete(file);
              }
          });
        }

        private void myProcess_Exited( object sender, EventArgs e )
        {
            //Process myProcess = (Process)(e.Value);
            //if (myProcess.ExitCode != 0)
            //{
            //    MessageService.ShowMessage("The process did not finish successfully", MessageType.ERROR);
            //}
            //Console.WriteLine(
            //    $"Exit time    : {myProcess.ExitTime}\n" +
            //    $"Exit code    : {myProcess.ExitCode}\n" ;
        }

        //var miiFiles = Directory.GetFiles(dirs["Restoration"].FullName).Where(t => t.Contains(".mii"));
        //if (miiFiles.Count() > 0)
        //{
        //    foreach (string file in miiFiles)//.ElementAt(0);
        //    {
        //        string[] lines = System.IO.File.ReadAllLines(file).Where(t => t.Contains(".dvt")).ToArray();
        //        if (lines.Count() > 0)
        //        {
        //            var words = lines.ElementAt(0).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(t => t.Contains(".dvt"));
        //            if (words.Count() > 0)
        //            {
        //                currentDVTFileName = words.ElementAt(0);
        //                //override GFA.exe tables
        //                string dvtTarget = Path.Combine( targetDir.FullName, currentDVTFileName);
        //                File.Copy(Path.Combine(dirs["Main"].FullName, Model.Name + "001_vol.dvt"), dvtTarget, true);
        //            }
        //        }
        //    }
        //}

        /*
        int counter = 1;
        foreach (CommonData.Vector3 columnCenter in boundaryPoints)
        {
            //this is because naming doesnt help and we need to move all the new files.

            bool COPYTOFLDER = true;
            if (COPYTOFLDER)
            {
                string simName = evt.Name;
                string[] newFiles = Directory.GetFiles(dirs["Restoration"].FullName).Where(t => ((originalFiles.Contains(t) == false) || (t.Contains("Boundary")))).ToArray();

                //now we copy any new file to the final simulation folder. The simulation folder is:
                DirectoryInfo info = Directory.CreateDirectory(Path.Combine(dirs["1D"].FullName, simName, tmpName));

                //delete whatever was there before
                Array.ForEach(Directory.GetFiles(info.FullName), delegate (string path) { File.Delete(path); });
                Array.ForEach(Directory.GetDirectories(info.FullName), delegate (string path) { Directory.Delete(path); });

                //move the newly created files there
                Array.ForEach(newFiles, delegate (string file) { File.Move(file, Path.Combine(info.FullName, Path.GetFileName(file))); });

                ////foreach (FileInfo file in info.GetFiles()) file.Delete();
                ////foreach (DirectoryInfo dir in info.GetDirectories()) dir.Delete(true);

                ////foreach (string newFile in newFiles)
                ////    File.Move(newFile, Path.Combine(info.FullName, Path.GetFileName(newFile));

                //foreach (string newFile in newFiles)
                //File.Copy(newFile, Path.Combine(info.FullName, Path.GetFileName(newFile)), true);

                //delete the newly created files in the restortion folder.
                //foreach (string newFile in newFiles) File.Delete(newFile);
            }

            counter++;
        }
        */

        private List<double> GetCellThicknessFromGRDFile( string fileName )
        {
            List<double> thickness = new List<double>();

            List<string> lines = File.ReadAllLines(fileName).ToList();
            int ZCORNLINE = lines.IndexOf("ZCORN");
            if (ZCORNLINE >= 0)
            {
                for (int n = 1 + ZCORNLINE; n < lines.Count(); n++)
                {
                    double result;
                    if (double.TryParse(lines[n].Trim(), out result))
                        thickness.Add(result);
                }
            }

            var every4 = thickness.Where(( x, i ) => (1 + i) % 4 == 0);

            List<double> intervalThickness = new List<double>();

            for (int n = 0; n < every4.Count(); n += 2)
                intervalThickness.Add(every4.ElementAt(n + 1) - every4.ElementAt(n));

            return intervalThickness;
        }

        private List<double> GetCellThicknessFromWRAPFGridFile( string fileName )
        {
            List<string> l = new List<string>();
            string[] lines = File.ReadAllLines(fileName);
            for (int n = 0; n < lines.Count(); n++)
            {
                if (lines[n].Contains("CORNERS"))
                {
                    int k = n + 1;
                    while ((k < lines.Count()) && (!lines[k].Contains("COORDS")))
                        l.Add(lines[k++].Trim());//.Replace("  "," " ).Replace(" ",","));
                    n = k + 1;
                }
            }

            var values = Regex.Replace(string.Join(" ", l.ToArray()), @"\s+", " ").Split(' ').Select(t => double.Parse(t.Trim()));   // .Join( )
            var every3 = values.Where(( x, i ) => (1 + i) % 3 == 0);
            var every4 = every3.Where(( x, i ) => (1 + i) % 4 == 0);

            List<double> thickNess = new List<double>();
            for (int n = 0; n < every4.Count(); n += 2)
                thickNess.Add(every4.ElementAt(n + 1) - every4.ElementAt(n));

            return thickNess;
        }

        private string[] GetFirstAndLastFGridFiles( string folder, string wildcard = "*WARP*.FGRID" )
        {
            var fgridFiles = Directory.GetFiles(folder, wildcard);//.Select(t=> Path.GetFileName( t ));
            if ((fgridFiles.Count() < 2)) return null;

            Regex digitsOnly = new Regex(@"[^\d]");
            var numericFileNames = fgridFiles.Select(t => Path.GetFileName(t));//.Select(w => w.IndexOf("0"));//.Select( ww => digitsOnly.Replace(ww, "") ) ;//.Select( t => t.Substring(  t  )) ;

            var w2 = numericFileNames.Select(t => t.Substring(t.IndexOf("0"), 5)).Select(t => digitsOnly.Replace(t, ""));

            List<int> numericValues = w2.Select(t => int.Parse(t)).ToList();

            int firstFileIndex = numericValues.IndexOf(numericValues.Min());
            int lastFileIndex = numericValues.IndexOf(numericValues.Max());
            string firstFileName = fgridFiles.ElementAt(firstFileIndex);
            string lastFileName = fgridFiles.ElementAt(lastFileIndex);

            string[] toReturn = new string[] { firstFileName, lastFileName };

            return toReturn;
        }

        public List<SimulationResults1D> GetAllAvailable1DResultsForModel()
        {
            string modelPath = Path.Combine(GetGFMFolder(), _model.Name);
            Dictionary<string, DirectoryInfo> dirs = GetProjectFolders(modelPath);
            DirectoryInfo FolderSims1DInfo = dirs["1D"];

            return new List<SimulationResults1D>();
        }

        public SimulationResults1D GetAllAvailable1DResultsForSimulation( string simulationFolder )
        {
            string modelPath = Path.Combine(GetGFMFolder(), _model.Name);
            Dictionary<string, DirectoryInfo> dirs = GetProjectFolders(modelPath);
            DirectoryInfo FolderSims1DInfo = dirs["1D"];

            return new SimulationResults1D();
        }

        public string GetGFMFolder()
        {
            string storageFolder = string.Empty;

            if (PetrelProject.IsPrimaryProjectOpen)
            {
                Project proj = PetrelProject.PrimaryProject;
                var info = PetrelProject.GetProjectInfo(DataManager.DataSourceManager);
                var dir = info.ProjectStorageDirectory;
                var file = info.ProjectFile;

                try
                {
                    storageFolder = System.IO.Path.Combine(ProjectTools.GetOceanFolder(), "GFM");
                    System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(storageFolder);
                }
                catch (Exception exx)
                {
                    string why = exx.ToString();
                    storageFolder = string.Empty;
                }
            }

            return storageFolder;
        }

        private Dictionary<string, DirectoryInfo> GenerateProjectFolders( string modelPath )
        {
            try
            {
                DirectoryInfo main = Directory.CreateDirectory(modelPath);
                string simsPath = Path.Combine(modelPath, "Simulations");
                DirectoryInfo sims = Directory.CreateDirectory(simsPath);
                DirectoryInfo sims1D = Directory.CreateDirectory(Path.Combine(simsPath, "1D"));
                DirectoryInfo sims2D = Directory.CreateDirectory(Path.Combine(simsPath, "2D"));
                DirectoryInfo sims3D = Directory.CreateDirectory(Path.Combine(simsPath, "3D"));

                string triangulatedSurfacesFolder = Path.Combine(modelPath, "RestorationSurfaces");
                DirectoryInfo restoration = Directory.CreateDirectory(triangulatedSurfacesFolder);

                Dictionary<string, DirectoryInfo> folders = new Dictionary<string, DirectoryInfo>()
                    {
                    { "Main", main},
                      { "Simulations", sims},
                        { "1D", sims1D},
                          { "2D", sims2D},
                            { "3D", sims3D},
                                { "Restoration", restoration}
                    };

                return folders;
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, DirectoryInfo> GetProjectFolders( string modelPath )
        {
            DirectoryInfo main = new DirectoryInfo(modelPath);
            string simsPath = Path.Combine(modelPath, "Simulations");
            DirectoryInfo sims = new DirectoryInfo(simsPath);
            DirectoryInfo sims1D = new DirectoryInfo(Path.Combine(simsPath, "1D"));
            DirectoryInfo sims2D = new DirectoryInfo(Path.Combine(simsPath, "2D"));
            DirectoryInfo sims3D = new DirectoryInfo(Path.Combine(simsPath, "3D"));

            DirectoryInfo crazy = new DirectoryInfo(Path.Combine(simsPath, "dfgdfgdfg3D"));

            string triangulatedSurfacesFolder = Path.Combine(modelPath, "RestorationSurfaces");
            DirectoryInfo restoration = new DirectoryInfo(triangulatedSurfacesFolder);

            Dictionary<string, DirectoryInfo> folders = new Dictionary<string, DirectoryInfo>()
            {
                { "Main", main},
                  { "Simulations", sims},
                    { "1D", sims1D},
                      { "2D", sims2D},
                        { "3D", sims3D},
                            { "Restoration", restoration},
                             { "crazy", crazy}
            };

            return folders;
        }

        private void UpdateModelFromUI()
        {
            _model.BaseName = _ui.BaseName;
            _model.Name = _ui.ModelName;
            _model.LithoData = _ui.LithoData;
            //Model.DVTTables = UI.GetDVTTablesFromUI();
            _model.GridDroid = _ui.ReferenceGridDroid;
            _model.RestorationFolder = _ui.RestorationFolder;
        }

        private void CopyNewRestorationDataToProjectFolder( List<string> files, string folder )
        {
            //we will copy only the files that arent already there.
            //lets make a list of the files that are already there.
            string[] filesThere = Directory.GetFiles(folder);

            int counter = 0;

            using (IProgress p = PetrelLogger.NewProgress(0, 100))
            {
                foreach (string file in files)
                {
                    p.ProgressStatus = (int)(100.0 * (counter++) / files.Count());

                    if (!filesThere.Contains(file))
                    {
                        string extension = Path.GetExtension(file);
                        string mshFile = Path.Combine(folder, Path.GetFileNameWithoutExtension(file) + ".msh");

                        if (extension.ToLower() == ".ts")
                        {
                            ExportDeck.ConvetGoCadToMsh(file, mshFile, 0.0);// rotation, flipz);
                        }
                        else if (extension.ToLower() == ".msh")
                        {
                            File.Copy(file, mshFile, true);
                            string vglFile = Path.Combine(folder, Path.GetFileNameWithoutExtension(file) + ".vgl");
                            using (System.IO.StreamWriter w = new System.IO.StreamWriter(vglFile))
                            {
                                w.WriteLine("*UNIT METRIC");
                                w.WriteLine("* AXIS RESERVOIR");
                                w.WriteLine("* GID " + Path.GetFileName(mshFile));
                            }
                        }
                    }//if
                }//files
            }
        }

        private void GenerateLithoFiles( string path, out string error )
        {
            error = string.Empty;
            LithoData l = _model.LithoData;
            string ModelName = _model.Name;

            if (ExportDeck.ExportDVTables(l, path, ModelName) == false)
            {
                error = "Error writting the dvt tables";
            }
            if (!ExportDeck.ExportSteps(l, path, ModelName, "Deposited", "PresentDay"))
            {
                error = "Error writting the steps file";
            }
            if (ExportDeck.ExporHorizonsFile(l, path, ModelName, _model.BaseName) == false)
            {
                error = "Error writting the horizons file";
            }
            if (ExportDeck.ExportLithofile(l, path, ModelName) == false)
            {
                error = "Error writting the litho file";
            }
        }

        public void CalculateThickness1D( string projectNamem, string simName, int column )
        {
        }
    }
}