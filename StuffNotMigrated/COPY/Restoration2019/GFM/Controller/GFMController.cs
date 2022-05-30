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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.Controllers
{
    class GFMController
    {
        GFMGeometryUI UI;
        GFMProject Model;
        IGFMSerializer Serializer;

        public GFMController(GFMGeometryUI gui, GFMProject model, IGFMSerializer SerializerI)
        {
            UI = gui;
            Model = model;
            Serializer = SerializerI;

            CreateApplicationLogic();
        }


        private void CreateApplicationLogic()
        {
            UI.CancelClicked += (sender, evt) =>
            {
                UpdateModelFromUI();
                Serializer.PersistModel(Model);

            };

            UI.GenerateProject += (sender, evt) =>
            {
                UpdateModelFromUI();


                string modelName = Model.Name;
                string projectFolder = Path.Combine(GetGFMFolder(), modelName);
                string uiRestorationFolder = UI.RestorationFolder;

                string error;
                Dictionary<string, DirectoryInfo> dirs = GenerateProjectFolders(projectFolder, out error);
                string prjRestorationFolderTarget = dirs["Restoration"].FullName;

                //MessageService.ShowMessage("The Project wuill be Generated in the Background.\nYou will be notified when done.\nYou can keep working.", MessageType.INFO);

                // var t = Task.Run(() =>
                //{
                CopyNewRestorationDataToProjectFolder(UI.TriangulatedSurfaceFilesToCopy, prjRestorationFolderTarget);
                //});


                GenerateLithoFiles(projectFolder, out error);

                Model.RestorationFolder = prjRestorationFolderTarget;
                Serializer.SeriealizeModel(Model, Path.Combine(projectFolder, modelName + ".prj"));

                UI.UIFromProject(Model);
                //  UI.RestorationFolder =  ( Model.RestorationFolder );
                // UI.TriangulatedSurfaceFilesToCopy.Clear();//t.Wait();

                MessageService.ShowMessage("Project generated", MessageType.INFO);
            };

            UI.Generate1DModels += (sender, evt) =>
            {
                CommonData.Vector3[] xyz = (CommonData.Vector3[])(evt.Data);
                string simName = evt.Name;
                string error, modelPath = Path.Combine(GetGFMFolder(), Model.Name);
                Dictionary<string, DirectoryInfo> dirs = GenerateProjectFolders(modelPath, out error);

                //copy the exec file to the Restoration folder 
                //File.Copy(Path.Combine(dirs["Main"].FullName, "GFA.exe"), Path.Combine(dirs["Restoration"].FullName,"GFA.exe"), true);

                File.Copy(Path.Combine(ProjectTools.GetOceanFolder(), "GFA.exe"), Path.Combine(dirs["Restoration"].FullName, "GFA.exe"), true);

                //make a list of all the files that were there before any new file was created. 
                //this is because naming doesnt help and we need to move all the new files. 


                int counter = 1;
                foreach (CommonData.Vector3 v in xyz)
                {
                    var originalFiles = Directory.GetFiles(dirs["Restoration"].FullName);

                    string fileName = "ModelBoundary.txt";
                    string filePath = Path.Combine(dirs["Restoration"].FullName, fileName);
                    using (StreamWriter file = new StreamWriter(filePath))
                    {
                        file.Write(GFM.Model.SimulationBoundary.Simulation1DFileDescription(v, 100.0));
                    }
                    string tmpName = string.Empty;
                    bool isinError = false;
                    try
                    {
                        //copy litho, horizons, steps, etc... from main to the restoration 
                        tmpName = "Col" + counter.ToString().PadLeft(3, '0'); ;
                        string[] extensions = { ".litho", ".steps", ".horizons", "001_vol.dvt" };
                        foreach (string s in extensions)
                        {
                            string litho = Path.Combine(dirs["Main"].FullName, Model.Name + s);
                            string target = Path.Combine(dirs["Restoration"].FullName, tmpName + s);
                            File.Copy(litho, target, true);

                        }
                    }
                    catch
                    {
                        isinError = true;
                    }
                    ;
                    ;
                    ;


                    if (isinError)
                    {
                        System.Windows.MessageBox.Show("Cannot generate simulation. Did you generate the project first?");
                        return;
                    }

                    bool RUNpROCESS = true;

                    if (RUNpROCESS)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.UseShellExecute = true;
                        startInfo.WorkingDirectory = dirs["Restoration"].FullName;
                        startInfo.FileName = "GFA.exe";
                        startInfo.Arguments = "-PQDEOLY " + tmpName + " 100 ";
                        Process p = new Process();
                        p.StartInfo = startInfo;
                        bool started = p.Start();


                        var id = p.Id;
                        //var fft = p.ExitCode;

                        p.WaitForExit();
                        System.Threading.Thread.Sleep(250);
                    }

                    //the exec that Assef created generates miis with a keyword for dvt tables. However, we dont know what the name of the file in the mii is defined. 
                    //so we get one file, figure out what is the name of the dvt file expected and then write our dvt table with that name
                    string dvtFileName = tmpName + "001_vol.dvt";
                    var miiFiles = Directory.GetFiles(dirs["Restoration"].FullName).Where(t => t.Contains(".mii"));
                    if (miiFiles.Count() > 0)
                    {
                        foreach (string file in miiFiles)//.ElementAt(0);
                        {
                            string[] lines = System.IO.File.ReadAllLines(file).Where(t => t.Contains(".dvt")).ToArray();
                            if (lines.Count() > 0)
                            {
                                var words = lines.ElementAt(0).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(t => t.Contains(".dvt"));
                                if (words.Count() > 0)
                                {
                                    dvtFileName = words.ElementAt(0);
                                    //override GFA.exe tables 
                                    string dvtTarget = Path.Combine(dirs["Restoration"].FullName, dvtFileName);
                                    File.Copy(Path.Combine(dirs["Main"].FullName, Model.Name + "001_vol.dvt"), dvtTarget, true);

                                }
                            }


                        }
                    }
                    else
                    {
                        ;
                    }


                    bool COPYTOFLDER = true;

                    System.Threading.Thread.Sleep(250);

                    if (COPYTOFLDER)
                    {
                        var newFiles = Directory.GetFiles(dirs["Restoration"].FullName).Where(t => ((originalFiles.Contains(t) == false) || (t.Contains("Boundary"))));



                        List<string> s = new List<string>();
                        foreach (string ff in newFiles)
                        {
                            if (ff.Contains("mii")) s.Add(Path.GetFileName(ff));
                        }

                        ;
                        //now we copy any new file to the final simulation folder 
                        //the simulation folder is 
                        DirectoryInfo info = Directory.CreateDirectory(Path.Combine(dirs["1D"].FullName, simName, tmpName));
                        System.Threading.Thread.Sleep(250);

                        foreach (FileInfo file in info.GetFiles()) file.Delete();
                        foreach (DirectoryInfo dir in info.GetDirectories()) dir.Delete(true);
                        System.Threading.Thread.Sleep(250);


                        foreach (string newFile in newFiles)
                            File.Copy(newFile, Path.Combine(info.FullName, Path.GetFileName(newFile)), true);
                        System.Threading.Thread.Sleep(250);
                        foreach (string newFile in newFiles)
                            File.Delete(newFile);
                        System.Threading.Thread.Sleep(250);
                    }

                    counter++;



                    //execute the exec file

                    //copy everything that was generated to the Simulations\1D\basename\ColCounter
                    //copy the exec fie to the same folder 
                    ;
                }
            };

            UI.LoadModelClicked += (sender, evt) =>
            {
                string modelFolder = evt.Value;
                string[] prjFile = Directory.GetFiles(modelFolder).Where(t => Path.GetExtension(t) == ".prj").ToArray();

                if (prjFile.Count() < 1) return;

                GFMProject p = Serializer.Deserialize(prjFile[0]);
                Model.DeppCopy(p);
                UI.UIFromProject(Model);

            };

            UI.Request1DSimulationList += (sender, evt) =>
            {
                string projectFile = Path.Combine( GetGFMFolder(), Model.Name, Model.Name+".prj" );
                GFMProject model = Serializer.Deserialize(projectFile);

                Dictionary<string, DirectoryInfo> dirs = GetProjectFolders(Path.Combine(GetGFMFolder(), Model.Name) );
                DirectoryInfo sims1D = dirs["1D"];

                Dictionary<string, int> toReturn = new Dictionary<string, int>();

                if (sims1D.Exists == false)
                {
                    MessageService.ShowMessage("1D Simulations have not been created yet.", MessageType.INFO);
                   
                }
                else
                {
                    string [] simulationNames = Directory.GetDirectories(sims1D.FullName).ToArray(); ;

                    foreach(string simulationName in simulationNames)
                    {
                        int nPoints =  Directory.GetDirectories(simulationName).Count();
                        toReturn.Add( Path.GetFileName(simulationName), nPoints);
                    }

                }

                ;
                UI.Update1DSimulationList( toReturn );
              
                ;


            };


            UI.Request1DSimulationResults += (sender, evt) =>
            {

                GetAllAvailable1DResultsForModel();

               ;
                ;
                ;
                ;
                ;

            };


        }


        public List<SimulationResults1D> GetAllAvailable1DResultsForModel()
        {
            string modelPath = Path.Combine(GetGFMFolder(), Model.Name);
            Dictionary<string, DirectoryInfo> dirs = GetProjectFolders(modelPath);
            DirectoryInfo FolderSims1DInfo = dirs["1D"];

            return new List<SimulationResults1D>();
        }
        public SimulationResults1D GetAllAvailable1DResultsForSimulation( string simulationFolder )
        {
            string  modelPath = Path.Combine(GetGFMFolder(), Model.Name);
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

        private Dictionary<string, DirectoryInfo> GenerateProjectFolders(string modelPath, out string error)
        {
            error = string.Empty;

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

        Dictionary<string, DirectoryInfo> GetProjectFolders(string modelPath)
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
            Model.BaseName = UI.BaseName;
            Model.Name = UI.ModelName;
            Model.LithoData = UI.LithoData;
            Model.DVTTables = UI.GetDVTTablesFromUI();
            Model.GridDroid = UI.ReferenceGridDroid;
            Model.RestorationFolder = UI.RestorationFolder;
        }

        private void CopyNewRestorationDataToProjectFolder(List<string> files, string folder)
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
                        else
                        {
                            File.Copy(file, mshFile);
                        }

                        string vglFile = Path.Combine(folder, Path.GetFileNameWithoutExtension(file) + ".vgl");

                        using (System.IO.StreamWriter w = new System.IO.StreamWriter(vglFile, true))
                        {
                            w.WriteLine("*UNIT METRIC");
                            w.WriteLine("* AXIS RESERVOIR");
                            w.WriteLine("* GID " + Path.GetFileName(mshFile));
                        }
                    }//if
                }//files

            }

        }

        private void GenerateLithoFiles(string path, out string error)
        {
            error = string.Empty;
            LithoData l = Model.LithoData;
            string ModelName = Model.Name;


            if (ExportDeck.ExportDVTables(l, path, ModelName) == false)
            {
                error = "Error writting the dvt tables";
            }
            if (!ExportDeck.ExportSteps(l, path, ModelName, "Deposited", "PresentDay"))
            {
                error = "Error writting the steps file";
            }
            if (ExportDeck.ExporHorizonsFile(l, path, ModelName, Model.BaseName) == false)
            {
                error = "Error writting the horizons file";
            }
            if (ExportDeck.ExportLithofile(l, path, ModelName) == false)
            {
                error = "Error writting the litho file";
            }
            //path = dirs["Restoration"].FullName;
            // if (ExportDeck.CopyTriangularSurfaceFilesToSimulationFolderAsMESH(TriangulatedSurfaceFilesToCopy, path, (double)rotationControl.Value, true) == false)
            // {
            //    error = true;
            //    MessageBox.Show("Error writting the  Mesh (.msh or .ts) files", "Error on export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }

        }



        public void CalculateThickness1D(string projectNamem, string simName, int column)
        {

        }

    }
}
