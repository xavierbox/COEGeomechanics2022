using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Restoration.GFM
{
    class ExportDeck
    {

        static public bool ExporHorizonsFile(LithoData data, string FolderName, string ModelName, string basementName)
        {
            try
            {
                string fileName = Path.Combine(FolderName, ModelName + ".horizons");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    foreach (var l in data.Layers)
                        file.WriteLine(l.Layer);

                    file.WriteLine(basementName);
                }
            }
            catch
            {

                return false;
            }

            return true;
        }

        static public bool ExportSteps(LithoData data, string FolderName, string ModelName, string stepSuffix, string presentDayName)
        {
            try
            {
                string fileName = Path.Combine(FolderName, ModelName + ".steps");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
          //          file.WriteLine(presentDayName);
                    foreach (var l in data.Layers)
                        file.WriteLine(l.Layer + stepSuffix);
                }

            }
            catch
            {
                return false;
            }

            return true;
        }


        static public bool ExportDVTables(LithoData data, string FolderName, string ModelName)
        {
            List<string> lines = new List<string>();

            string header = "*DVTTABLES\n\t1\tNPOINTS\nmeanstrain E";

            try
            {
                int counter = 1;
                foreach (var l in data.Layers)
                {
                    lines.Add("--\t" + counter.ToString() + " table\t" + counter.ToString() + "  " + l.Layer);
                    lines.Add("-- E0\t" + l.YMInitial.ToString());
                    string droid = l.YoungsModulusHardeningFunctionDroid;
                    Function f = (Function)DataManager.Resolve(new Droid(droid));
                    foreach (var p in f.Points)
                        lines.Add(p.X.ToString() + "\t" + p.Y.ToString());
                    lines.Add(" ");

                    if (counter == 1)
                        header = header.Replace("NPOINTS", f.PointCount.ToString());

                    counter += 1;
                }

                lines.Insert(0, header);
                //write the file 
                string fileName = Path.Combine(FolderName, ModelName + "001_vol.dvt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    foreach (string line in lines)
                        file.WriteLine(line);
                }
            }

            catch
            {
                return false;
            }

            return true;
        }

        static public bool ExportLithofile(LithoData data, string FolderName, string ModelName)
        {
            try
            {
                string fileName = Path.Combine(FolderName, ModelName + ".litho");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {

                    file.WriteLine(data);
                }

            }
            catch
            {
                return false;
            }

            return true;

        }

        static public bool CopyTriangularSurfaceFilesToSimulationFolderAsMESH(List<string> filePaths, string FolderName, double rotation = 0.0, bool flipz = true)
        {

            try
            {
                if (filePaths.Count() < 1) return false;
                //if file is a .ts convert it to .msh using Assef code. 
                int count = 0;
                using (IProgress nestedProgressBar = PetrelLogger.NewProgress(0, 100, ProgressType.Cancelable, Cursors.WaitCursor))
                {

                    PetrelLogger.Status(new string[] { "Coverting file formats for VISAGE and copying data files to sim folder" });


                    foreach (string filePath in filePaths)
                    {
                        nestedProgressBar.ProgressStatus = Math.Min(100, Math.Max(0, (int)(100 * count / filePaths.Count())));

                        string extension = Path.GetExtension(filePath);
                        string mshFile = Path.Combine(FolderName, Path.GetFileNameWithoutExtension(filePath) + ".msh");

                        if (extension.ToLower() == ".ts")
                        {
                            ConvetGoCadToMsh(filePath, mshFile, rotation, flipz);
                        }
                        else
                        {
                            File.Copy(filePath, mshFile);
                        }

                        string vglFile = Path.Combine(FolderName, Path.GetFileNameWithoutExtension(filePath) + ".vgl");

                        using (System.IO.StreamWriter w = new System.IO.StreamWriter(vglFile, true))
                        {
                            w.WriteLine("*UNIT METRIC");
                            w.WriteLine("* AXIS RESERVOIR");
                            w.WriteLine("* GID " + Path.GetFileName(mshFile));
                        }

                        count += 1;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        static public bool ConvetGoCadToMsh(string gocadFile, string mshFile, double rotation = 0.0, bool flipz = true)
        {
            string[] lines = File.ReadAllLines(gocadFile, Encoding.UTF8);

            double rotationRadians = Math.PI * rotation / 180.0;


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mshFile, true))
            {

                double zsign = (flipz == true ? -1.0 : 1.0);
                bool applyRotation = (Math.Abs(rotation) > 0.001 ? true : false);

                string mshHeader = "MESH    dimension 3 ElemType Triangle  Nnode 3\nCoordinates ";

                int addOne = -1;
                file.WriteLine(mshHeader);

                int counter = 0;
                for (int n = 0; n < lines.Count(); n++)
                {
                    string line = lines[n];
                    if (line.Contains("VRTX"))
                    {

                        string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        line = (counter + 1).ToString() + " ";

                        double x = double.Parse(words[2]);
                        double y = double.Parse(words[3]);
                        double z = zsign * double.Parse(words[4]);

                        if (addOne == -1)
                        {
                            int firstIndex = int.Parse(words[1]);
                            addOne = firstIndex == 0 ? 1 : 0;
                        }

                        //rotation
                        if (applyRotation)
                        {
                            double r = Math.Sqrt(x * x + y * y);
                            double thetaZero = Math.Acos(x / r);

                            double finalAngle = thetaZero + rotationRadians;

                            x = r * Math.Cos(finalAngle);
                            y = r * Math.Sin(finalAngle);
                        }
                        line += (x + " " + y + " " + z);


                        //for (int k = 2; k < words.Count(); k++)
                        //line += ( ( k == words.Count()-1 ? "-" : "") +  words[k] + " ");


                        file.WriteLine(line.Trim());
                        counter += 1;
                    }
                }
                file.WriteLine("end coordinates");
                file.WriteLine();

                counter = 0;
                file.WriteLine("Elements");
                for (int n = 0; n < lines.Count(); n++)
                {
                    string line = lines[n];
                    if (line.Contains("TRGL"))
                    {
                        string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                        int index1 = int.Parse(words[1]) + addOne;
                        int index2 = int.Parse(words[2]) + addOne;
                        int index3 = int.Parse(words[3]) + addOne;

                        line = (counter + 1).ToString() + " " + index1 + " " + index2 + " " + index3;

                        //line = ((counter + 1).ToString() + " " + line.Replace("TRGL", "")).Trim();
                        file.WriteLine(line);
                        counter += 1;
                    }
                }
                file.WriteLine("end elements");
                file.WriteLine();

            }//writter 

            return true;

        }
    }//class

}//namespace 
