
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using System.IO;


namespace Gigamodel.Data
{
    internal class GigaModelDataDefinitions
    {
        public static string GigamodelStorageFolder
        {
            get
            {
                //Project proj = PetrelProject.PrimaryProject;
                var dir = PetrelProject.GetProjectInfo(DataManager.DataSourceManager).ProjectStorageDirectory;

                string storageFolder = System.IO.Path.Combine(System.IO.Path.Combine(dir.FullName, "Ocean"), "GigaModel");
                System.IO.Directory.CreateDirectory(storageFolder);

                return storageFolder;
            }
        }

        public static string GigamodelBinaryFile => Path.Combine(GigamodelStorageFolder, "gigamodel2.bin");

        public static string SimExportFolder => System.IO.Path.Combine(GigamodelStorageFolder, "Simulations\\");
    }
}