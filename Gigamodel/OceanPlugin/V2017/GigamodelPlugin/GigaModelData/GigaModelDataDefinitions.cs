using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigamodel.Data
{
    internal class GigaModelDataDefinitions
    {
        //public static string DataExportFolder = @"C:\AppData\GigaModel\";

        public static string DataExportFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\GigaModel");

        public static string SimExportFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\GigaModel\\Simulations");//@"C:\AppData\GigaModel\Simulations\";

    }
}
