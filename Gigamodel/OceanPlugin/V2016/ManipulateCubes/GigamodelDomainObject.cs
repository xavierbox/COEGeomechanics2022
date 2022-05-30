using Slb.Ocean.Core;
using Slb.Ocean.Petrel.Basics;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Slb.Ocean.Petrel.UI;
using System.Drawing;

namespace ManipulateCubes
{
    public class GigamodelDomainObject : /*IExtensions, */IImageInfoSource, INameInfoSource
    {
        private Bitmap _bitmap = PetrelImages.Model_32;
        private DefaultImageInfo _image;
        private NameInfo _nameInfo = null;
        private List<Object> l;

        public GigamodelDomainObject()
        {
            _image = null;
            Name = "Gigamodel";
            l = new List<object>();
        }

        public string Name { get; set; }

        public override string ToString() { return Name; }

        #region ImagEInfo interface 
        public ImageInfo ImageInfo
        {
            get { if (_image == null) _image = new DefaultImageInfo(_bitmap); return _image; }
        }
        #endregion 

        #region NameInfo interface 
        public NameInfo NameInfo
        {
            get
            {
                if (_nameInfo == null)
                    _nameInfo = new DefaultNameInfo("GigaModel", " 3D Seismic Giga Model", "GigaModel");
                return _nameInfo;
            }
        }

        #endregion 

        #region IExtensions interface
        public void Add(object obj)
        {
            ;// throw new NotImplementedException();
        }
        public int Count
        {
            get
            {
                return 1;
            }
        }

        public bool Contains(object obj)
        {
            return false;// throw new NotImplementedException();
        }

        public bool Remove(object obj)
        {
            return true;// throw new NotImplementedException();
        }

        // //       public IEnumerator<object> GetEnumerator()
        //        {
        //            return l.GetEnumerator();
        //        }
        ////
        //        IEnumerator IEnumerable.GetEnumerator()
        //        {
        //            return l.GetEnumerator();
        //        }
        ////
        #endregion

    };

    public class INamedItem
    {
        public INamedItem()
        {
            Name = "Default";
        }

        public string Name { get; set; }

        public virtual void Clear() {; }

    };

    public class MaterialsModelItem : INamedItem
    {
        public MaterialsModelItem()
        {
            namedCubes = new List<KeyValuePair<Droid, string>>();
            Id = string.Empty;
        }

        public string Id { get; set; }

        public override void Clear() { namedCubes.Clear(); }

        public void AddCube(Droid d, string s)
        {
            namedCubes.Add(new KeyValuePair<Droid, string>(d, s));
        }

        public List<KeyValuePair<Droid, string>> DroidNames
        {
            get { return namedCubes; }
        }

        public List<Droid> Droids { get { return namedCubes.Select(t => t.Key).ToList(); } }


        public List<string> Names { get { return namedCubes.Select(t => t.Value).ToList(); } }

        public SeismicCube GetSeismicCubeByName(string name)
        {
            //"YOUNGSMOD" "POISSONR" "DENSITY"
            int index = Names.IndexOf(name);
            if (index < 0) return null;

            return (SeismicCube)(DataManager.Resolve(Droids[index]));
        }

        public SeismicCube YoungsModulus
        {
            get
            {
                return GetSeismicCubeByName("YOUNGSMOD");
            }
        }

        public SeismicCube PoissonsRatio
        {          //"YOUNGSMOD" "POISSONR" "DENSITY"
            get
            {
                return GetSeismicCubeByName("POISSONR");
            }
        }

        public SeismicCube Density
        {          //"YOUNGSMOD" "POISSONR" "DENSITY"
            get
            {
                return GetSeismicCubeByName("DENSITY");
            }
        }


        public string Droid { get; set; }

        List<KeyValuePair<Droid, string>> namedCubes;
    };

    public class PressureModelItem : INamedItem
    {
        public PressureModelItem()
        {
            datedCubes = new List<KeyValuePair<Droid, DateTime>>();
            Id = string.Empty;
        }

        public string Id { get; set; }
        public override void Clear() { datedCubes.Clear(); datedCubes.Clear(); }

        public void AddCube(Droid d, DateTime date)
        {
            //namedCubes.Add(new KeyValuePair<Droid, string>(d, s));
            datedCubes.Add(new KeyValuePair<Droid, DateTime>(d, date));
        }

        public List<KeyValuePair<Droid, DateTime>> DroidDates
        {
            get { return datedCubes; }
        }

        public float Gradient { get; set; }

        public float Offset { get; set; }

        public int Steps { get { return datedCubes.Count(); } }


        public bool isCoupled { get { return (datedCubes.Count() > 1); } }

        public float PressureScaling { get; set; }

        public float DepthScaling { get; set; }

        public List<Droid> Droids { get { return datedCubes.Select(t => t.Key).ToList(); } }

        public List<DateTime> Dates { get { return datedCubes.Select(t => t.Value).ToList(); } }

        public SeismicCube InitialPressure
        {
            get
            {
                return (SeismicCube)(DataManager.Resolve(Droids[0]));

            }
        }

        public List<SeismicCube> Cubes
        {
            get
            {
                List<SeismicCube> l = new List<SeismicCube>();

                try
                {
                    foreach (Droid d in Droids)
                    {

                        l.Add((SeismicCube)(DataManager.Resolve(d)));
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                return l;
            }
        }
        //List<KeyValuePair<Droid, string>> namedCubes;
        List<KeyValuePair<Droid, DateTime>> datedCubes;

    };

    public class BoundaryConditionsItem : INamedItem
    {

        public BoundaryConditionsItem() : base()
        {
            Name = "DefaultBoundaryConditions";
            MinStrain = 0.0f;
            MaxStrain = 0.0f;
            MinStrainAngle = 0.0f;
            Id = string.Empty;
        }

        public float MinStrain { get; set; }

        public float MaxStrain { get; set; }

        public float MinStrainAngle { get; set; }

        public bool Offshore { get; set; }

        public float GapDensity { get; set; }

        public float SeaWaterDensity { get; set; }

        public bool StrainModeGradients { get; set; }

        public Droid DatumDroid { get; set; }

        public float DatumValue { get; set; }

        public string Id { get; set; }
    };

    public class GigaModelCollection<T> where T : INamedItem, new() //a colllection represents a group of pressures, a group of material models, a group of boundary conditions, etc...
    {

        protected List<T> models;

        protected GigaModelCollection()
        {
            models = new List<T>();
        }

        protected T GetOrCreateModel(string name)
        {
            T item = null;
            //not there, create one 
            if (!models.Any(t => t.Name == name))
            {
                item = new T();
                item.Name = name;
            }
            else
            {
                item = (models.Where(t => t.Name == name).Select(t => t)).ToArray()[0];
            }

            return item;
        }

        public string[] ModelNames
        {
            get { return models.Select(s => s.Name).ToArray(); }
        }

        public bool FindModel(string name)
        {
            return models.Any(s => s.Name == name);

        }

        public void Clear() { models.Clear(); }

        public int Size { get { return models.Count; } }
    };


    public class MaterialCollection : GigaModelCollection<MaterialsModelItem>
    {
        public MaterialCollection() : base()
        {
        }

        public MaterialsModelItem AppendOrEditModel(string name, List<KeyValuePair<Droid, string>> cubeDroids)
        {
            bool wasThere = FindModel(name);
            MaterialsModelItem item = GetOrCreateModel(name);
            item.Clear();

            //create a clone of the list.
            foreach (KeyValuePair<Droid, string> step in cubeDroids)
                item.AddCube(step.Key, step.Value);
            if (!wasThere)
                models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return item; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }

        public List<KeyValuePair<Droid, string>> CubeDroids(string modelName)
        {
            if (!FindModel(modelName)) return new List<KeyValuePair<Droid, string>>();
            else
            {
                MaterialsModelItem item = GetOrCreateModel(modelName);
                return item.DroidNames;

            }
        }

        new public MaterialsModelItem GetOrCreateModel(string name)
        {
            return base.GetOrCreateModel(name);
        }
    };

    public class PressuresCollection : GigaModelCollection<PressureModelItem>
    {
        public PressuresCollection() : base()
        {
        }

        public bool AppendOrEditModel(string name, List<KeyValuePair<Droid, DateTime>> cubeDroids, float gradient, float offset, /*float scaling,*/ float depthScaling)
        {
            bool wasThere = FindModel(name);
            PressureModelItem item = GetOrCreateModel(name);
            item.Clear();

            //create a clone of the list.
            foreach (KeyValuePair<Droid, DateTime> step in cubeDroids)
                item.AddCube(step.Key, step.Value);

            item.Gradient = gradient;
            item.Offset = offset;
            //item.PressureScaling = scaling;
            item.DepthScaling = depthScaling;
            if (!wasThere)
                models.Add(item);
            item.Id = item.Id = Guid.NewGuid().ToString("N");

            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }

        new public PressureModelItem GetOrCreateModel(string name)
        {
            return base.GetOrCreateModel(name);
        }
    };

    public class BoundaryConditionsCollection : GigaModelCollection<BoundaryConditionsItem>
    {
        public BoundaryConditionsCollection() : base()
        {
        }

        public bool AppendOrEditModel(string name, Droid Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle)
        {

            bool wasThere = FindModel(name);
            BoundaryConditionsItem item = GetOrCreateModel(name);
            item.Clear();

            item.DatumDroid = Datum;
            item.Offshore = Offshore;
            item.GapDensity = gapDensity;
            item.SeaWaterDensity = waterDensity;
            item.StrainModeGradients = strainGraientMode;
            item.MinStrain = emin;
            item.MaxStrain = emax;
            item.MinStrainAngle = angle;
            item.Id = Guid.NewGuid().ToString("N");

            if (!wasThere)
                models.Add(item);

            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }

        new public BoundaryConditionsItem GetOrCreateModel(string name)
        {
            return base.GetOrCreateModel(name);
        }
    };

    public class SimulationModelItem : INamedItem
    {
        public SimulationModelItem()
        {
            Id = string.Empty;
        }

        public string Id { get; set; }

        public string MaterialModelName { get; set; }

        public string PressureModelName { get; set; }

        public string BoundaryConditionsModelName { get; set; }

        public string Description { get; set; }

        public override void Clear() { MaterialModelName = string.Empty; PressureModelName = string.Empty; BoundaryConditionsModelName = string.Empty; }

        public MaterialsModelItem MaterialsModelItem { get; set; }

        public PressureModelItem PressureModelItem { get; set; }

        public List<DateTime> Dates { get { return PressureModelItem?.Dates; } }

    };

    public class SimulationCollection : GigaModelCollection<SimulationModelItem>
    {
        public SimulationCollection() : base()
        {
            models = new List<SimulationModelItem>();
        }

        public bool AppendOrEditModel(string name, string matsModelName, string pressureModelName, string bConditionsModelName, string description)
        {
            bool wasThere = FindModel(name);
            SimulationModelItem item = GetOrCreateModel(name);
            item.Clear();

            item.MaterialModelName = matsModelName;
            item.PressureModelName = pressureModelName;
            item.BoundaryConditionsModelName = bConditionsModelName;
            item.Description = description;

            if (!wasThere)
                models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }

        new public SimulationModelItem GetOrCreateModel(string name)
        {
            return base.GetOrCreateModel(name);
        }

    };

    public class Gigamodel : GigamodelDomainObject
    {

        private BoundaryConditionsCollection bcCollection;

        private PressuresCollection pressuresCollection;

        private MaterialCollection materialsCollection;

        private SimulationCollection simsCollection;

        public Gigamodel() : base()
        {
            bcCollection = new BoundaryConditionsCollection();
            pressuresCollection = new PressuresCollection();
            materialsCollection = new MaterialCollection();
            simsCollection = new SimulationCollection();
        }

        public string[] PressureModelNames { get { return pressuresCollection.ModelNames; } }

        public string[] MaterialModelNames { get { return materialsCollection.ModelNames; } }

        public string[] BoundaryConditionsNames { get { return bcCollection.ModelNames; } }

        public string[] SimulationNames { get { if (simsCollection == null) simsCollection = new SimulationCollection(); return simsCollection.ModelNames; } }

        public PressuresCollection PressureModels { get { return pressuresCollection; } }

        public MaterialCollection MaterialModels { get { return materialsCollection; } }

        public BoundaryConditionsCollection BoundaryConditionsModels { get { return bcCollection; } }

        public SimulationCollection SimulationsModel { get { return simsCollection; } }



        public bool StoreModel(PressureSelection item)
        {
            //AppendOrEditMaterialsModel(item.SelectedName, item.NamedDroids);
            //return pressuresCollection.AppendOrEditModel(item.SelectedName, item.NamedDroids);
            return false;
        }
        public void AppendOrEditPressureModel(string name, List<KeyValuePair<Droid, DateTime>> cubeDroids, float grad, float offset, /*float scaling,*/ float depthScaling)
        {
            pressuresCollection.AppendOrEditModel(name, cubeDroids, grad, offset, /*scaling,*/ depthScaling);
        }



        public MaterialsModelItem StoreModel(MaterialSelection item)
        {
            //AppendOrEditMaterialsModel(item.SelectedName, item.NamedDroids);
            return materialsCollection.AppendOrEditModel(item.SelectedName, item.NamedDroids);
        }
        public void AppendOrEditMaterialsModel(string name, List<KeyValuePair<Droid, string>> cubeDroids)
        {
            materialsCollection.AppendOrEditModel(name, cubeDroids);
        }

        public void AppendOrEditBoundaryConditionsModel(string name, Droid Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle)
        {
            bcCollection.AppendOrEditModel(name, Datum, Offshore, gapDensity, waterDensity, strainGraientMode, emin, emax, angle);
        }

        public void AppendOrEditSimulationModel(string name, string matModelName, string pressureModelName, string bcModelName, string description)
        {
            simsCollection.AppendOrEditModel(name, matModelName, pressureModelName, bcModelName, description);
        }

    }





    /*

    //this should be a singleton 
    public class Gigamodel : GigamodelDomainObject
    {
        public Gigamodel() : base()
        {
            bcCollection = new BoundaryConditions();
            pressuresCollection = new PressureCollection();
            materialsCollection = new MaterialCollection();
        }

        public PressureCollection PressureModels { get { return pressuresCollection; } }

        public MaterialCollection MaterialModels { get { return materialsCollection; } }

        public BoundaryConditions BoundaryConditions { get { return bcCollection; } }

        public string[] PressureModelNames { get { return pressuresCollection.ModelNames; } }

        public string[] MaterialModelNames { get { return materialsCollection.ModelNames; } }

        public void AppendOrEditPressureModel(string name, List<KeyValuePair<Droid, DateTime>> cubeDroids)
        {
            pressuresCollection.AppendOrEditModel(name, cubeDroids);
        }

        public void AppendOrEditMaterialsModel(string name, List<KeyValuePair<Droid, string>> cubeDroids)
        {
            materialsCollection.AppendOrEditModel(name, cubeDroids);
        }





        private BoundaryConditions bcCollection;

        private PressureCollection pressuresCollection;

        private MaterialCollection materialsCollection;

    }


    public class PressureCollection
    {
        public PressureCollection()
        {
            models = new List<PressureModelItem>();

        }

        public string[] ModelNames
        {
            get { return models.Select(t => t.Name).ToArray(); }
        }

        public bool FindModel(string name)
        {
            return models.Any(t => t.Name == name);
        }

        private PressureModelItem GetOrCreateModel(string name)
        {
            PressureModelItem item = null;
            //not there, create one 
            if (!models.Any(t => t.Name == name))
            {
                item = new PressureModelItem();
                item.Name = name;
            }
            else
            {
                item = (models.Where(t => t.Name == name).Select(t => t)).ToArray()[0];
            }

            return item;
        }

        public bool AppendOrEditModel(string name, List<KeyValuePair<Droid, DateTime>> cubeDroids)
        {
            PressureModelItem item = GetOrCreateModel(name);
            item.Clear();

            //create a clone of the list.
            foreach (KeyValuePair<Droid, DateTime> step in cubeDroids)
                item.pressures.Add(new KeyValuePair<Droid, DateTime>(step.Key, step.Value));
            //to do: item.sortPressures();
            //to do: if the cube is deleted, the pressure item becomes invalid. Needs to be deleted.  


            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }




        //this is used by the UI to populate combo-pboxes or lists, tables, etc. 


        private List<PressureModelItem> models;

        public class PressureModelItem
        {
            public PressureModelItem()
            {
                pressures = new List<KeyValuePair<Droid, DateTime>>();
            }

            public string Name { get; set; }

            public int Steps { get { return pressures.Count(); } }

            public List<KeyValuePair<Droid, DateTime>> pressures;

            public void Clear() { pressures.Clear(); }

            public bool isAlreadyInDisk { get; set; }

            public float UndefinedCellsOffset { get; set; }

            public float UndefinedCellsGradient { get; set; }

        }


    };

    public class StrainBoundaryConditionsItem //: BoundaryConditionsItem
    {
        public float MinStrain { get; set; }
        public float MaxStrain { get; set; }
        public float MaxStrainOrientation { get; set; }

        public string Name { get; set; }

    }

    public class BoundaryConditions
    {
        public BoundaryConditions()
        {
            items = new List<StrainBoundaryConditionsItem>();


        }

        public string[] ModelNames
        {
            get { return items.Select(t => t.Name).ToArray(); }
        }

        public bool AppendOrEditModel(string name, StrainBoundaryConditionsItem itemData) //create a clone
        {
            return true;
        }

        public bool FindModel(string name)
        {
            return items.Any(t => t.Name == name);
        }

        private StrainBoundaryConditionsItem GetOrCreateModel(string name)
        {
            StrainBoundaryConditionsItem item = null;
            //not there, create one 
            if (!items.Any(t => t.Name == name))
            {
                item = new StrainBoundaryConditionsItem();
                item.Name = name;
            }
            else
            {
                item = (items.Where(t => t.Name == name).Select(t => t)).ToArray()[0];
            }

            return item;
        }


        List<StrainBoundaryConditionsItem> items;

    }



    public class INamedItem
    {
        string _name;
        public string Name { get { return _name; } set { _name = value; } }

    };

    public class GigaModelCollection<T> where T : INamedItem //a colllection represents a group of pressures, a group of material models, a group of boundary conditions, etc...
    {

        private List<T> models;

        public GigaModelCollection()
        {
            models = new List<T>();
        }

        public string[] ModelNames
        {
            get { return models.Select(s => s.Name).ToArray(); }
        }

        public bool FindModel(string name)
        {
            return models.Any(s => s.Name == name);

        }


    };



    public class MaterialCollection
    {
        public MaterialCollection()
        {
            models = new List<MaterialsModelItem>();
        }

        public string[] ModelNames
        {
            get { return models.Select(t => t.Name).ToArray(); }
        }

        public bool FindModel(string name)
        {
            return models.Any(t => t.Name == name);
        }

        private MaterialsModelItem GetOrCreateModel(string name)
        {
            MaterialsModelItem item = null;
            //not there, create one 
            if (!models.Any(t => t.Name == name))
            {
                item = new MaterialsModelItem();
                item.Name = name;
            }
            else
            {
                item = (models.Where(t => t.Name == name).Select(t => t)).ToArray()[0];
            }

            return item;
        }


        public bool AppendOrEditModel(string name, List<KeyValuePair<Droid, string>> cubeDroids)
        {
            bool wasThere = FindModel(name);
            MaterialsModelItem item = GetOrCreateModel(name);
            item.Clear();

            //create a clone of the list.
            foreach (KeyValuePair<Droid, string> step in cubeDroids)
                item.Add(step.Key, step.Value);
            if (!wasThere)
                models.Add(item);


            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }


        public List<KeyValuePair<Droid, string>> CubeDroids(string modelName)
        {
            if (!FindModel(modelName)) return null;

            MaterialsModelItem item = GetOrCreateModel(modelName);
            return item.DroidCubeNames;


        }




        //this is used by the UI to populate combo-pboxes or lists, tables, etc. 
        private List<MaterialsModelItem> models;// = new List<MaterialsModelItem>();

        public class MaterialsModelItem
        {
            public MaterialsModelItem()
            {
                mats = new List<KeyValuePair<Droid, string>>();
            }

            public string Name { get; set; }

            public void Clear() { mats.Clear(); }


            public void Add(Droid d, string s)
            {
                mats.Add(new KeyValuePair<Droid, string>(d, s));
            }

            public List<KeyValuePair<Droid, string>> DroidCubeNames
            {
                get { return mats; }
            }

            List<KeyValuePair<Droid, string>> mats;


        };

    };


*/
};

