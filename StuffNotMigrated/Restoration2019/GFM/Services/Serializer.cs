using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.GFM.Services
{
    interface IGFMSerializer
    {
        void SeriealizeModel(GFMProject p, string fileName);

        void PersistModel(GFMProject model);

         GFMProject Deserialize(string file);
    };


    internal class GFMSerializer : IGFMSerializer
    {
         public  void PersistModel(GFMProject model)
        {
            string fileName = GetPersistentModelFile();
            if (fileName != string.Empty)
                SeriealizeModel(model, fileName);

        }

        private static string GetPersistentModelFile()
        {
            string fileName = string.Empty;

            if (PetrelProject.IsPrimaryProjectOpen)
            {
                Project proj = PetrelProject.PrimaryProject;
                var info = PetrelProject.GetProjectInfo(DataManager.DataSourceManager);
                var dir = info.ProjectStorageDirectory;
                var file = info.ProjectFile;

                try
                {
                    string storageFolder = System.IO.Path.Combine(System.IO.Path.Combine(dir.FullName, "Ocean"), "GFM");
                    System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(storageFolder);
                    fileName = Path.Combine(storageFolder, "gfm.bin");

                }
                catch (Exception exx)
                {
                    string why = exx.ToString();
                    return string.Empty;
                }
            }
            return fileName;
        }

        public  GFMProject Deserialize(string file)
        {
            BinaryFormatter bf = new BinaryFormatter();
            GFMProject m = new GFMProject();
            FileStream fsin = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
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

         public  void SeriealizeModel(GFMProject p, string fileName)
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

        public static GFMProject GetPersistedModel()
        {
            string file = GetPersistentModelFile();

            if (file != string.Empty)
            {
                return new GFMProject();
            }
            else
            {
                return new GFMProject();
            }
        }
    };

}
