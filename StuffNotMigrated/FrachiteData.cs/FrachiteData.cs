using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FrachiteData
{
    [Serializable]
    public class FracHiteSimulationSettings
    {
        public FracHiteSimulationSettings()
        {
            RunInTheCloud = false;
            NumberOfCores = 4;
            Folder = "No set";
            ModelName = "Default";
        }

        public XElement ToXElement()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { OmitXmlDeclaration = true });
            x.Serialize(writer, this);
            return System.Xml.Linq.XElement.Parse(sww.ToString());
        }

        private bool _cld;

        public bool RunInTheCloud
        {
            get { return _cld; }
            set { _cld = value; }
        }

        public bool RunLocal
        {
            get { return !RunInTheCloud; }
            set { RunInTheCloud = !value; }
        }

        private int _nc;

        public int NumberOfCores
        {
            get { return _nc; }
            set { _nc = value; }
        }

        public string Folder { get; set; }

        public string ModelName { get; set; }

        public void CopyFrom( FracHiteSimulationSettings s )
        {
            RunInTheCloud = s.RunInTheCloud;
            NumberOfCores = s.NumberOfCores;
            Folder = s.Folder;
            ModelName = s.ModelName;
        }
    };

    [Serializable]
    public class FracHiteOptions
    {
        public FracHiteOptions()
        {
        }

        public void CopyFrom( FracHiteOptions s )
        {
            MaximumHeightGrowthDownwards = s.MaximumHeightGrowthDownwards;
            MaximumHeightGrowthUpwards = s.MaximumHeightGrowthUpwards;
            MaximumPressure = s.MaximumPressure;
            Convergency = s.Convergency;
            Hydrostatic = s.Hydrostatic;
            FluidDensity = s.FluidDensity;
        }

        public float MaximumHeightGrowthUpwards { get; set; } = 200;

        public float MaximumHeightGrowthDownwards { get; set; } = 200;

        public float MaximumPressure { get; set; } = 1.0E10f;

        public int Convergency { get; set; } = 0;

        public int Hydrostatic { get; set; } = 1;

        public float FluidDensity { get; set; } = 1000.0f;

        public string ToXMLString()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true
                };
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww, settings);
                x.Serialize(writer, this);
                var xml = System.Xml.Linq.XDocument.Parse(sww.ToString());
                return xml.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public override string ToString() => $"{MaximumPressure} {MaximumHeightGrowthUpwards} {MaximumHeightGrowthDownwards} {Convergency} {Hydrostatic} {FluidDensity}    Max pressure allowed(Pa).Max upward height growth(m).Max downward height growth(m). 0 = Equil.height,1 = modulus layer. 0 = non - hydrostatic,1 = hydrostatic.Fluid density(kg / m ^ 3";
    }

    [Serializable]
    public class FrachiteSimulationBinaryData
    {
        [XmlIgnoreAttribute]
        public Dictionary<string, float[]> Arrays { get; set; } = new Dictionary<string, float[]>();

        public int Nx { get; set; }

        public int Ny { get; set; }

        public int Nz { get; set; }

        public int NumCells => (Nx * Ny * Nz);

        public float[] GetArray( string name ) => (Arrays.Keys.Contains(name) ? Arrays[name] : null);

        public string ToXMLString()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true
                };
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww, settings);
                x.Serialize(writer, this);
                var xml = System.Xml.Linq.XDocument.Parse(sww.ToString());
                return xml.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string[] RequiredPropertyNamesFromPetrel => new string[] { "TVDTOP", "TVDBOTTOM", "TOUGHNESS", "SHMIN", "YOUNGMOD", "POISSONR", "RESERVOIRFLAG", "FRACHITEFLAG" };
    }

    public static class FrachiteConfig
    {
        public static string[] RequiredPropertyNamesFromPetrel => new string[] { "TVDTOP", "TVDBOTTOM", "TOUGHNESS", "SHMIN", "YOUNGMOD", "POISSONR", "RESERVOIRFLAG", "FRACHITEFLAG" };

        public static string ApplicationFolder
        {
            get
            {
                string s = "D:\\AppData\\";// Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FracHite\\";
                Directory.CreateDirectory(s);
                return s;// @"D:\\Projects\\Projects2017\\Q\\FracHite\FracHite\\FracHiteApplication";
            }
        }

        //D:\\AppData\\FrachiteExecutable\\FracHITEklee.exe

        public static string FrachiteExecutable
        {
            get => ApplicationFolder + "FrachiteExecutable\\FracHITEklee.exe";
        }

        public static string FrachiteLauncher
        {
            get => ApplicationFolder + "FrachiteExecutable\\RunFrachiteIndependent.exe";
        }
    }

    [Serializable]
    public class FrachiteFileResults
    {
        [Serializable]
        public class Index3
        {
            public Index3()
            {
                I = -1; J = -1; K = -1;
            }

            public Index3( int[] input )
            {
                I = input[0]; J = input[1]; K = input[2];
            }

            public int I = -1, J = -1, K = -1;
        }

        public Dictionary<string, List<float>> Arrays { get; set; }

        public int CellIndex { get; set; }

        public Index3 Indices { get; set; } = new Index3();

        public float PerforationTop { get; set; }

        public float PerforationBottom { get; set; }

        public string ModelName { get; set; }

        public Dictionary<int, float[]> ReservoirTops { get; set; } = new Dictionary<int, float[]>();

        public bool GetDataArraysFromResultsFile( string fileName )
        {
            Arrays = new Dictionary<string, List<float>>();

            //read headers, this is the line just before a line that hass all numbers, wee look for the keyword PRESSURE.
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
            {
                int count = 0;
                string line = file.ReadLine();
                while ((count++ < 10) && (!string.IsNullOrEmpty(line)) && (!line.ToLower().Contains("pressure")))
                {
                    line = file.ReadLine();
                }
                if (count >= 10 || string.IsNullOrEmpty(line))
                    return false; //no headers found

                Regex trimmer = new Regex(@"\s\s+");
                char[] spkittes = new char[] { ' ', '\t' };
                string[] keywords = trimmer.Replace(line.Trim(), " ").Split(spkittes, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in keywords)
                    Arrays.Add(s, new List<float>());

                line = file.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();
                    string[] values = trimmer.Replace(line, " ").Split(spkittes, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < values.Length; i++)
                    {
                        float.TryParse(values[i], out float result);
                        Arrays[keywords[i]].Add(result);
                    }

                    line = file.ReadLine().Trim();
                }

                file.Close();
            }

            return true;
        }
    }

    [Serializable]
    public class FrachiteInput
    {
        public double TopPerforation { get; set; }

        public double BottomPerforation { get; set; }

        public bool ToFile( string file )
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(file))
                {
                    outputFile.WriteLine(Options.ToString());

                    outputFile.WriteLine($"{TopTVD.Length}");

                    outputFile.WriteLine($"{TopPerforation}  {BottomPerforation}");

                    float[][] data = new float[6][] { TopTVD, Toughness, Stress, YoungsModulus, PoissonsRatio, ReservoirFlag };
                    int nArrays = (ReservoirFlag != null ? 6 : 5), nRows = TopTVD.Length;
                    for (int n = 0; n < nRows; n++)
                    {
                        string line = "";
                        for (int k = 0; k < nArrays; k++) line = line + $"{data[k][n]}  "; //very inefficient.
                        outputFile.WriteLine(line);
                    }

                    outputFile.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FromFile( string file )
        {
            return false;
        }

        public float[] TopTVD { get; set; }
        public float[] Toughness { get; set; }
        public float[] Stress { get; set; }
        public float[] YoungsModulus { get; set; }
        public float[] PoissonsRatio { get; set; }
        public float[] ReservoirFlag { get; set; } = null;

        public void AllocMemory( int nLayers, bool includeReservoirFlag = true )
        {
            TopTVD = new float[nLayers];
            Toughness = new float[nLayers];
            Stress = new float[nLayers];
            YoungsModulus = new float[nLayers];
            PoissonsRatio = new float[nLayers];
            if (includeReservoirFlag)
                ReservoirFlag = new float[nLayers];
            else
                ReservoirFlag = null;
        }

        public Dictionary<int, float[]> GetReservoirTops()
        {
            Dictionary<int, float[]> toReturn = new Dictionary<int, float[]>();

            Dictionary<int, List<float>> temp = new Dictionary<int, List<float>>();
            for (int n = TopTVD.Length - 1; n >= 0; n--)
            {
                if (!temp.Keys.Contains((int)ReservoirFlag[n]))
                {
                    toReturn.Add((int)(ReservoirFlag[n]), new float[2]);
                    temp.Add((int)ReservoirFlag[n], new List<float>());
                }
                temp[(int)ReservoirFlag[n]].Add(TopTVD[n]);
            }

            foreach (var key in temp.Keys)
            {
                var values = temp[key];
                toReturn[key][0] = values.Min();
                toReturn[key][1] = values.Max();
            }

            return toReturn;
        }

        public bool Verify()
        {
            //check array sizes
            if (TopTVD.Length < 10) return false;
            if (ReservoirFlag != null)
            {
                if (ReservoirFlag.Length != TopTVD.Length) return false;

                if (TopTVD.Any(t => float.IsNaN(t))) return false;

                if (ReservoirFlag.Any(t => float.IsNaN(t))) return false;
            }
            float[][] data = new float[4][] { Toughness, Stress, YoungsModulus, PoissonsRatio };
            foreach (var arr in data)
            {
                if (arr.Length != TopTVD.Length) return false;
                if (arr.Any(t => float.IsNaN(t))) return false;
                if (arr.Any(t => t < 0.1f)) return false;
            }

            //perforations
            if (TopPerforation > BottomPerforation) return false;

            for (int n = 1; n < TopTVD.Length; n++) if (TopTVD[n] < TopTVD[n - 1]) return false;

            return true;
        }

        public FracHiteOptions Options { get; set; } = new FracHiteOptions();
    }

    /*
    public GFMProject Deserialize(string file)
    {
        BinaryFormatter bf = new BinaryFormatter();
        GFMProject m = new GFMProject();

        try
        {
            FileStream fsin = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
            using (fsin)
            {
                m = (GFMProject)bf.Deserialize(fsin);
            }

            return m;
        }
        catch (Exception ee)
        {
            string why = ee.ToString();
            return m;
        }
    }

    public void SeriealizeModel(GFMProject p, string fileName)
    {
        if (fileName != string.Empty)
        {
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, p);
                    stream.Close();
                }
            }
            catch
            {
                ;
            }
        }
    }
    */
}

///