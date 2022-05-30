using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.GFM
{
    class GFMDataDefinitions
    {

        //public static string DataExportFolder = @"C:\AppData\GigaModel\";

        public static string DataExportFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)/*"C:\\"*/, "AssefGFM\\");
                //return System.IO.Path.Combine("C:\\", "GigaModel\\");

            }
        }
        public static string SimExportFolder
        {
            get
            {
                return System.IO.Path.Combine(DataExportFolder, "Simulations\\");//@"C:\AppData\GigaModel\Simulations\";
            }
        }// = System.IO.Path.Combine(DataExportFolder, "Simulations\\");//@"C:\AppData\GigaModel\Simulations\";


    }

    [Serializable]
    class GFMProject
    {
        public GFMProject()
        {
            Name = string.Empty;
            BaseName = string.Empty;
            LithoData = new LithoData();
            DVTTables = new Dictionary<string, GFMDVTTable>();
            GridDroid = string.Empty;
            RestorationFolder = string.Empty;
        }

        public void DeppCopy(GFMProject p)
        {

            Name = p.Name;
            BaseName = p.BaseName;
            LithoData.DeepCopy(p.LithoData);

            GridDroid = p.GridDroid;
            RestorationFolder = p.RestorationFolder;


        }

        public string RestorationFolder { get; set; }

        public string GridDroid { get; set; }

        public string Name { get; set; }

        public string BaseName { get; set; }

        public LithoData LithoData { get; set; }

        public Dictionary<string, GFMDVTTable> DVTTables { get; set; }

    };




    [Serializable]
    class GFMDVTTable
    {
        public GFMDVTTable(string name, List<Tuple<double, double>> values = null)
        {
            Name = name;
            Data = values != null ? values : new List<Tuple<double, double>>();
        }

        public List<Tuple<double, double>> Data;

        public string Name { get; set; }

        public void DeepCopy(GFMDVTTable table)
        {
            Data = new List<Tuple<double, double>>();
            Name = table.Name;

            foreach (Tuple<double, double> item in table.Data)
            {
                Data.Add(new Tuple<double, double>(item.Item1, item.Item2));
            }
        }

    }

}



