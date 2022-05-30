
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gigamodel.Data;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Core;

namespace Gigamodel.Controllers
{
    internal class ModelCreateDestroySvc
    {
        public static GigaModelDataModel GetOrCreateGigaModelForPetrelProject()
        {
            return FindGigaModelInPetrelTree() ?? (FindSerializedGigaModel() ?? CreateNewGigaModel()); //may be null 
        }

        public static GigaModelDataModel DeleteGigaModelForPetrelProject()
        {
            //enter in Petrel Project ptd folder. Search folder Gigamodel and delete it. 
            return null;
        }

        private static GigaModelDataModel FindSerializedGigaModel()
        {
            //Petrel primary project needs to be open. 
            //enter in Petrel Project ptd folder. Search folder Gigamodel. Grab the first Gigamodel.bin 
            return null;
        }

        private static GigamodelDomainObject FindGigaModelDomainObjectInPetrelTree()
        {
            GigamodelDomainObject gigaModelDomain = null;
            if (PetrelProject.IsPrimaryProjectOpen)
            {
                Project proj = PetrelProject.PrimaryProject;
                var extensions = proj.Extensions;
                foreach (object item in extensions)
                {
                    if (item is GigamodelDomainObject)
                        gigaModelDomain = (GigamodelDomainObject)(item);
                }
            }

            return gigaModelDomain;
        }

        private static GigaModelDataModel FindGigaModelInPetrelTree()
        {
            GigaModelDataModel model = null;
            GigamodelDomainObject gigaModelDomainObject = FindGigaModelDomainObjectInPetrelTree();
            if (gigaModelDomainObject != null)
            {
                model = gigaModelDomainObject.GigaModelDataModel;
            }

            return model;
        }

        private static void StoreGigaModelInPetrelTree(GigaModelDataModel m)
        {

            if (PetrelProject.IsPrimaryProjectOpen)
            {
                Project proj = PetrelProject.PrimaryProject;
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
            SerializeGigaModel(model);

            return model;
        }

        private static bool SerializeGigaModel(GigaModelDataModel m)
        {
            return false;
        }

    }
}
