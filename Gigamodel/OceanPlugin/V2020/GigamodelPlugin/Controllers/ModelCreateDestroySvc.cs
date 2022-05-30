using Gigamodel.Data;

using Slb.Ocean.Core;

using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.Basics;
using Slb.Ocean.Petrel.DomainObject;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gigamodel.Controllers
{
    internal class ModelCreateDestroySvc
    {
        public static GigaModelDataModel GetOrCreateGigaModelForPetrelProject()
        {
            return FindGigaModelInPetrelTree() ?? (FindSerializedGigaModel() ?? CreateNewGigaModel()); //
        }

        public static GigaModelDataModel FindSerializedGigaModel()
        {
            GigaModelDataModel mp = null;
            //Petrel primary project needs to be open.
            //enter in Petrel Project ptd folder. Search folder Gigamodel. Grab the first Gigamodel.bin
            if (PetrelProject.IsPrimaryProjectOpen)
            {
                try
                {
                    string fileName = GigaModelDataDefinitions.GigamodelBinaryFile;// "gigamodel2.bin");

                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter bformatter = new BinaryFormatter();
                        mp = (GigaModelDataModel)bformatter.Deserialize(stream);
                        stream.Close();
                        StoreGigaModelInPetrelTree(mp);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return mp;
        }

        private static GigamodelDomainObject FindGigaModelDomainObjectInPetrelTree()
        {
            GigamodelDomainObject gigaModelDomain = null;
            if (PetrelProject.IsPrimaryProjectOpen)
            {
                foreach (object item in PetrelProject.PrimaryProject.Extensions)
                {
                    if (item is GigamodelDomainObject)
                    {
                        gigaModelDomain = (GigamodelDomainObject)(item);
                        break;
                    }
                }
            }

            return gigaModelDomain;
        }

        public static GigaModelDataModel FindGigaModelInPetrelTree()
        {
            GigamodelDomainObject gigaModelDomainObject = FindGigaModelDomainObjectInPetrelTree();
            return gigaModelDomainObject?.GigaModelDataModel;
        }

        private static void StoreGigaModelInPetrelTree( GigaModelDataModel m )
        {
            if (PetrelProject.IsPrimaryProjectOpen)
            {
                Project proj = PetrelProject.PrimaryProject;
                IProjectInfo i = PetrelProject.GetProjectInfo(DataManager.DataSourceManager);

                GigamodelDomainObject d = FindGigaModelDomainObjectInPetrelTree();
                using (ITransaction txn = DataManager.NewTransaction())
                {
                    txn.Lock(proj);
                    if (d == null)
                    {
                        d = new GigamodelDomainObject();
                        proj.Extensions.Add(d);
                    }
                    d.GigaModelDataModel = m;
                    txn.Commit();
                }
            }
        }

        private static GigaModelDataModel CreateNewGigaModel()
        {
            GigaModelDataModel model = new GigaModelDataModel();
            StoreGigaModelInPetrelTree(model);
            return model;
        }

        public static bool SerializeGigaModel( GigaModelDataModel m )
        {
            if (PetrelProject.IsPrimaryProjectOpen)
            {
                try
                {
                    string fileName = GigaModelDataDefinitions.GigamodelBinaryFile;// "gigamodel2.bin");
                    using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, m);
                        stream.Close();
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}