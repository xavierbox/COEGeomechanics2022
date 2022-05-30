using FrachiteData.cs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace FrachiteData
{
    public partial class FrachiteRunner
    {
        public FrachiteRunner()
        {
            ;
        }

        public void ParseInputData( string binaryFile = "Arrays.bin", string optionsFile = null )//"Options.bin")
        {
            ParseBinaryData(Path.Combine(Folder, binaryFile));
            if (optionsFile != null)
                ParseFracHiteInternalOptions(Path.Combine(Folder, optionsFile));
            else
                InternalOptions = new FracHiteOptions(); //default
        }

        public bool RunSimulation( string id = "1" )
        {

            DateTime today = DateTime.Now;
            DateTime date2 = new DateTime(2022, 8, 1);
            int result = DateTime.Compare(today, date2);

            if (result > 0)
            {
                return false;
            }


                if (!CanRun) return false;

            var flag = BinaryData.GetArray("FRACHITEFLAG");
            var indices = Enumerable.Range(0, BinaryData.NumCells).Where(i => flag[i] >= 0.9999);
            Random rnd = new Random(11230759);

            LastResults = new List<FrachiteFileResults>();
            Errors = new List<string>();

            int cellsToRun = indices.Count();
            for (int i = 0; i < cellsToRun; i++)
            {
                int cell = indices.ElementAt(i);

                int[] ijk = GetCellIndices(cell);
                string frachiteInputFile = Path.Combine(Folder, ModelName + $"_{ijk[0]}_{ijk[1]}_{ijk[2]}_" + cell.ToString() + ".dat");

                //string frachiteInputFile = Path.Combine(Folder, ModelName + "Cell" + cell.ToString() + "Batch" + id + ".dat");

                string resultsFile = Path.Combine(Folder, Path.GetFileNameWithoutExtension(frachiteInputFile) + ".dbg");

                FrachiteInput input = GenerateInputforCell(cell, frachiteInputFile);
                if (input != null)
                {
                    if (RunFrachiteForInput(frachiteInputFile))
                    {
                        var i3 = GetCellIndices(cell);

                        FrachiteFileResults item = new FrachiteFileResults();
                        item.GetDataArraysFromResultsFile(resultsFile);
                        item.Indices = new FrachiteFileResults.Index3(GetCellIndices(cell));
                        item.CellIndex = cell;
                        item.ReservoirTops = input.GetReservoirTops();
                        item.PerforationTop = (float)input.TopPerforation;
                        item.PerforationBottom = (float)input.BottomPerforation;
                        item.ModelName = ModelName;
                        LastResults.Add(item);

                        //delete files but leave some as an example and for debugging.
                        if ((cellsToRun >= 100) && (rnd.Next(0, 100) > 2))
                            DeleteAssociatedFiles(Folder, Path.GetFileNameWithoutExtension(frachiteInputFile));//, new string[] { ".log", ".dat" });
                    }
                    else
                    {
                        Errors.Add($"Unable to run simulation for cell {cell.ToString()}");
                    }
                }
                else
                {
                    Errors.Add($"Unable to generate input files for cell  { cell.ToString() }");
                }

                if ((cellsToRun > 25) && (i % 25 == 0))
                {
                    using (StreamWriter writer = new StreamWriter(Path.Combine(Folder, "Progress.txt"), false))
                    {
                        writer.Write($"Progress: {(100.0 * i) / cellsToRun}");
                    }

                    int p = (int)((100.0 * i) / cellsToRun);
                    OnThresholdReached(new ThresholdReachedEventArgs(p));
                }
            }//cells to run

            OnThresholdReached(new ThresholdReachedEventArgs(100));
            return true;
        }//run simmulation

        public bool SerializeLastResults()
        {
            if (!CanSerialize) return false;

            try
            {
                //write the last errors if any ErrorModelName.txt
                var errText = String.Join("\n", Errors.ToArray());
                File.WriteAllText(ErrorFile, errText);

                //write a collection of results ResultsModelName.bin
                using (Stream stream = File.Open(DefaultSerializedResultsFile, FileMode.Create, FileAccess.Write))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, LastResults);
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                string why = e.ToString();
                return false;
            }

            return true;
        }

        public bool DeserializeResults( string file )
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                FileStream fsin = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                using (fsin)
                {
                    LastResults = (List<FrachiteFileResults>)bf.Deserialize(fsin);
                }

                return true;
            }
            catch (Exception ee)
            {
                string why = ee.ToString();
                LastResults = null;
                return false;
            }
        }

        #region configure

        public FracHiteOptions InternalOptions { get; set; } = new FracHiteOptions();

        public List<FrachiteFileResults> LastResults
        {
            get;
#if DEBUG
            set;
#else
            private set;
#endif
        } = null;

        public string Folder { get; set; } = string.Empty;

        public string ModelName { get; set; } = "DEFAULT";

        public List<string> Errors { get; set; } = new List<string>();

        private string ErrorFile { get => Path.Combine(Folder, ModelName + "ERROR.txt"); }

        public string DefaultSerializedResultsFile { get => Path.Combine(Folder, ModelName + "RESULTS.bin"); }

        #endregion configure

        #region private

        private bool CanSerialize { get => CanReadWriteInFolder(Folder); }

        private bool CanRun { get => (BinaryData != null && InternalOptions != null && CanReadWriteInFolder(Folder)); }

        private static bool CanReadWriteInFolder( string f )
        {
            return true;
        }

        public FrachiteSimulationBinaryData BinaryData { get; /*private*/ set; } = null;

        private int[] GetCellIndices( int elementIndex )//int nx, int ny, int nz) (int,int,int) and return (...) in c# 7
        {
            int Nx = BinaryData.Nx, Ny = BinaryData.Ny, Nz = BinaryData.Nz;

            int elementVerticalGap = Nx * Ny;
            //int element index is assumed to be as  = ii + jj * NumElementsIJK.I + kk * (NumElementsIJ);
            int elementK = (int)(elementIndex / (Nx * Ny));
            int elementJ = (int)((elementIndex - elementK * elementVerticalGap) / Nx);
            int elementI = elementIndex - elementJ * Nx - elementK * elementVerticalGap;

            return new int[] { elementI, elementJ, elementK };
        }

        private FrachiteSimulationBinaryData ParseBinaryData( string fileName )
        {
            try
            {
                Dictionary<string, float[]> arrays = new Dictionary<string, float[]>();
                int Nx, Ny, Nz;

                using (var filestream = File.Open(fileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(filestream))
                    {
                        float[] aux = new float[4] { 0f, 0f, 0f, 0f }; ;
                        var tempBufferSize = 4 * sizeof(float);
                        var tempBuffer = reader.ReadBytes(tempBufferSize);
                        Buffer.BlockCopy(tempBuffer, 0, aux, 0, tempBufferSize);   //nx,ny,nz

                        Nx = (int)(aux[0]);
                        Ny = (int)(aux[1]);
                        Nz = (int)(aux[2]);
                        int nArrays = (int)(aux[3]), nCells = Nz * Ny * Nx;

                        for (int i = 0; i < nArrays; i++)
                        {
                            string arrayName = reader.ReadString();
                            arrays.Add(arrayName, Enumerable.Repeat(0.0f, nCells).ToArray());
                        }

                        var names = arrays.Keys;
                        for (int i = 0; i < nArrays; i++)
                        {
                            tempBufferSize = nCells * sizeof(float);
                            tempBuffer = reader.ReadBytes(tempBufferSize);
                            Buffer.BlockCopy(tempBuffer, 0, arrays[names.ElementAt(i)], 0, tempBufferSize);
                        }

                        //writer.Write(nx); as float
                        //writer.Write(ny);
                        //writer.Write(nz);
                        //writer.Write(numberArrays);as float
                        //writer.Write(arraynames);
                    }
                }

                BinaryData = new FrachiteSimulationBinaryData()
                {
                    Arrays = arrays,
                    Nx = Nx,
                    Ny = Ny,
                    Nz = Nz
                };
            }
            catch
            {
                //MessageBox.Show("An error occured when reading the binary data");
                BinaryData = null;
                return BinaryData;
            }

            return BinaryData;
        }

        private FracHiteOptions ParseFracHiteInternalOptions( string fileName )
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FracHiteOptions));

                    InternalOptions = (FracHiteOptions)serializer.Deserialize(fs);
                }
            }
            catch
            {
                InternalOptions = null;
                return InternalOptions;
            }
            return InternalOptions;
        }

        private bool RunFrachiteForInput( string inputFile )
        {
            Process p = new Process();
            p.StartInfo.Arguments = (inputFile);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = FrachiteConfig.FrachiteExecutable;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputFile);// workingPath;

            bool crashed = false;
            p.Start();
            try
            {
                if (p.WaitForExit(30000))
                {
                }
                else
                {
                    crashed = true;
                    p.Kill();
                }
            }
            catch
            {
            }
            return !crashed;
        }

        private void DeleteAssociatedFiles( string folderName, string frachiteInputFile, string[] extensions = null )
        {
            var allFiles = Directory.GetFiles(folderName, $"*{frachiteInputFile}*");

            if (extensions == null)
            {
                Array.ForEach(allFiles, delegate ( string aFile ) { File.Delete(aFile); });
            }
            else
            {
                foreach (string extension in extensions)
                {
                    var selected = allFiles.Where(t => t.Contains(extension)).ToArray();
                    Array.ForEach(selected, delegate ( string aFile ) { File.Delete(aFile); });
                }
            }
        }

        private FrachiteInput GenerateInputforCell( int cell, string fileName )
        {
            int nCells = BinaryData.NumCells;
            int[] indices = GetCellIndices(cell);
            int ko = indices[2];

            Dictionary<string, float[]> arrays = BinaryData.Arrays;
            int Nx = BinaryData.Nx, Ny = BinaryData.Ny, Nz = BinaryData.Nz;

            //we will also generate a field of reservoir flag vs depth. It will be stored here. #

            //number of layers
            int minKLayer = Math.Max(1, ko - 100), maxKLayer = Math.Min(BinaryData.Nz - 2, ko + 100), totalLayers = maxKLayer - minKLayer;

            //perforation
            float perforationTopTVD = arrays["TVDTOP"][cell];
            float perforationBottomTVD = arrays["TVDBOTTOM"][cell];

            FrachiteInput inputs = new FrachiteInput();
            inputs.AllocMemory(totalLayers, includeReservoirFlag: true);
            inputs.TopPerforation = arrays["TVDTOP"][cell];
            inputs.BottomPerforation = arrays["TVDBOTTOM"][cell];

            int counter = 0;
            for (int k = maxKLayer; k > minKLayer; k--)
            {
                //ii + jj * NumElementsIJK.I + kk * (NumElementsIJ);
                int c = indices[0] + indices[1] * Nx + k * (Nx * Ny);
                inputs.TopTVD[counter] = arrays["TVDTOP"][c];
                inputs.Toughness[counter] = arrays["TOUGHNESS"][c];
                inputs.Stress[counter] = arrays["SHMIN"][c];
                inputs.YoungsModulus[counter] = arrays["YOUNGMOD"][c];
                inputs.PoissonsRatio[counter] = arrays["POISSONR"][c];
                inputs.ReservoirFlag[counter] = arrays["RESERVOIRFLAG"][c];
                counter += 1;
            }

            inputs.ToFile(fileName);

            return inputs;
        }

        protected virtual void OnThresholdReached( ThresholdReachedEventArgs e )
        {
            ThresholdReached?.Invoke(this, e);
        }

        #endregion private

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
    }
}