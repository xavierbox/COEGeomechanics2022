using Gigamodel.GigaModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DroidString = System.String;

namespace Gigamodel.Data
{
    public class INamedItem
    {
        public INamedItem(string name = "Default")
        {
            Name = name;
        }

        public string Name { get; set; }

        public virtual void Clear() {; }

    };

    //public class MaterialsModelItemOLD : INamedItem
    //{
    //    public MaterialsModelItem()
    //    {
    //        namedCubes = new List<KeyValuePair<DroidString, string>>();
    //        Id = string.Empty;
    //    }

    //    public string Id { get; set; }

    //    public override void Clear() { namedCubes.Clear(); }

    //    public void AddCube(DroidString droidString, string name)
    //    {
    //        int i = Names.IndexOf(name);
    //        if (i >= 0) { namedCubes.RemoveAt(i); }
    //        namedCubes.Add(new KeyValuePair<DroidString, string>(droidString, name));
    //    }

    //    public List<KeyValuePair<DroidString, string>> DroidNames
    //    {
    //        get { return namedCubes; }
    //    }

    //    public List<DroidString> Droids { get { return namedCubes.Select(t => t.Key).ToList(); } }

    //    public List<string> Names { get { return namedCubes.Select(t => t.Value).ToList(); } }

    //    public string GetDroidStringByName(string name)
    //    {
    //        //"YOUNGSMOD" "POISSONR" "DENSITY"
    //        int index = Names.IndexOf(name);
    //        if (index < 0) return null;

    //        return  Droids[index];
    //    }

    //    public string YoungsModulusDroid
    //    {
    //        get
    //        {
    //            int i = Names.IndexOf("YOUNGSMOD");
    //            return (i >= 0 ? namedCubes[i].Key : null); 
    //        }
    //        set
    //        {
    //            AddCube(value, "YOUNGSMOD");  
    //        }

    //    }

    //    public string PoissonsRatioDroid
    //    {
    //        get
    //        {
    //            int i = Names.IndexOf("POISSONR");
    //            return (i >= 0 ? namedCubes[i].Key : null);
    //        }
    //        set
    //        {
    //            AddCube(value, "POISSONR");
    //        }
    //    }

    //    public string DensityDroid
    //    {
    //        get
    //        {
    //            int i = Names.IndexOf("DENSITY");
    //            return (i >= 0 ? namedCubes[i].Key : null);
    //        }
    //        set
    //        {
    //            AddCube(value, "DENSITY");
    //        }
    //    }

    //    //public string DroidString { get; set; }

    //    List<KeyValuePair<DroidString, string>> namedCubes;
    //};

    public class MaterialsModelItem : INamedItem
    {
        public MaterialsModelItem()
        {
            namedCubes = new List<SeismicCubeFaccade>();
            Id = string.Empty;
            TensileRatio = 1.0f;
        }

        public string Id { get; set; }

        public override void Clear() { namedCubes.Clear(); Id = string.Empty; }

        public void AddCube(SeismicCubeFaccade s)
        {
            int i = Names.IndexOf(s.Name);
            if (i >= 0) { namedCubes.RemoveAt(i); }
            namedCubes.Add(s);
        }

        //public List<KeyValuePair<DroidString, string>> DroidNames
        //{
        //    get { return namedCubes.Values; }
        //}

        public List<DroidString> Droids { get { return namedCubes.Select(t => t.DroidString).ToList(); } }

        public List<string> Names { get { return namedCubes.Select(t => t.Name).ToList(); } }


  

        public SeismicCubeFaccade YoungsModulus
        {
            get
            {
                int i = Names.IndexOf("YOUNGSMOD");
                return (i >= 0 ? namedCubes[i] : null);
            }
            set
            {
                AddCube(value);
            }

        }

        public SeismicCubeFaccade PoissonsRatio
        {
            get
            {
                int i = Names.IndexOf("POISSONR");
                return (i >= 0 ? namedCubes[i] : null);
            }
            set
            {
                AddCube(value);
            }
        }

        public SeismicCubeFaccade Density
        {
            get
            {
                int i = Names.IndexOf("DENSITY");
                return (i >= 0 ? namedCubes[i] : null);
            }
            set
            {
                AddCube(value);
            }
        }

        public float TensileRatio { get; set; }

        public void ShallowCopyFrom(MaterialsModelItem input)
        {
            Clear();
            foreach (SeismicCubeFaccade s in input.namedCubes)
            {
                namedCubes.Add(s);
            }

            TensileRatio = input.TensileRatio;

        }

        public List<SeismicCubeFaccade> Cubes
        {
            get { return namedCubes; }
        }


        List<SeismicCubeFaccade> namedCubes;

    };


    public class PressureModelItem : INamedItem
    {
        public PressureModelItem()
        {
            datedCubes = new List<KeyValuePair<SeismicCubeFaccade, DateTime>>();
            Id = string.Empty;
        }

        public string Id { get; set; }

        public SeismicCubeFaccade InitialPressure
        {
            get { return datedCubes[0].Key; }
        }


        public override void Clear() { datedCubes.Clear(); datedCubes.Clear(); }


        //public void AddCube(DroidString d, DateTime date)
        //{
        //    //namedCubes.Add(new KeyValuePair<Droid, string>(d, s));
        //    datedCubes.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(d, date));
        //}
        public void AddCube(SeismicCubeFaccade s, DateTime t)
        {
            int i = Names.IndexOf(s.Name);
            if (i >= 0) { datedCubes.RemoveAt(i); }
            datedCubes.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(s, t));
        }

        public List<DateTime> Dates { get { return datedCubes.Select(t => t.Value).ToList(); } }

        public List<DroidString> Droids { get { return datedCubes.Select(t => t.Key.DroidString).ToList(); } }

        public List<KeyValuePair<SeismicCubeFaccade, DateTime>> DatedDroids
        {
            get { return datedCubes; }
        }

        public float Gradient { get; set; }

        public bool isCoupled { get { return (datedCubes.Count() > 1); } }

        public List<string> Names { get { return datedCubes.Select(t => t.Key.Name).ToList(); } }

        public float Offset { get; set; }

        public int Steps { get { return datedCubes.Count(); } }


        public void ShallowCopyFrom(PressureModelItem input)
        {
            Clear();
            foreach (KeyValuePair<SeismicCubeFaccade, DateTime> s in input.datedCubes)
            {
                datedCubes.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(s.Key, s.Value));
            }

            Offset = input.Offset;
            Gradient = input.Gradient;
        }


        //public float PressureScaling { get; set; }

        //public float DepthScaling { get; set; }
        List<KeyValuePair<SeismicCubeFaccade, DateTime>> datedCubes;



    };



    public class BoundaryConditionsItem : INamedItem
    {


        public BoundaryConditionsItem() : base()
        {
            Name = "DefaultBoundaryConditions";
            MinStrain = 0.0f;
            MaxStrain = 0.0f;
            MaxStrainAngle = 0.0f;
            Id = string.Empty;
        }

        public float MinStrain { get; set; }

        public float MaxStrain { get; set; }

        public float MaxStrainAngle { get; set; }

        public bool Offshore { get; set; }

        public float GapDensity { get; set; }

        public float SeaWaterDensity { get; set; }

        public bool StrainModeGradients { get; set; }

        public DroidString DatumDroid { get; set; }

        public float DatumValue { get; set; }

        public string Id { get; set; }


        public void ShallowCopyFrom(BoundaryConditionsItem input)
        {


            MinStrain = input.MinStrain;
            MaxStrain = input.MaxStrain;
            MaxStrainAngle = input.MaxStrainAngle;
            Offshore = input.Offshore;
            GapDensity = input.GapDensity;
            SeaWaterDensity = input.SeaWaterDensity;
            StrainModeGradients = input.StrainModeGradients;
            DatumDroid = input.DatumDroid;
            DatumValue = input.DatumValue;
            Name = input.Name;



        }

    };

    public class GigaModelCollection<T> where T : INamedItem, new() //a colllection represents a group of pressures, a group of material models, a group of boundary conditions, etc...
    {

        protected List<T> models;

        protected GigaModelCollection()
        {
            models = new List<T>();
        }

        public T GetOrCreateModel(string name)
        {
            T item = null;

            if (!models.Any(t => t.Name == name))  //not there, create one 
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


        public bool DeleteModelbyName(string name)
        {
            List<string> names = models.Select(t => t.Name).ToList();
            int index = names.IndexOf(name);
            if (index < 0) return false;

            models.RemoveAt( index );

            return true;

        }

    };


    public class MaterialCollection : GigaModelCollection<MaterialsModelItem>
    {
        public MaterialCollection() : base()
        {
        }

        public MaterialsModelItem AppendOrEditModel(MaterialsModelItem input)
        {
            bool wasThere = FindModel(input.Name);
            MaterialsModelItem item = GetOrCreateModel(input.Name);
            item.ShallowCopyFrom(input);

            if (!wasThere)
                models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return item;
        }

        //public MaterialsModelItem AppendOrEditModel(string name, List<KeyValuePair<DroidString, string>> cubeDroids)
        //{
        //    bool wasThere = FindModel(name);
        //    MaterialsModelItem item = GetOrCreateModel(name);
        //    item.Clear();

        //    //create a clone of the list.
        //    foreach (KeyValuePair<DroidString, string> step in cubeDroids)
        //        item.AddCube(step.Key, step.Value);
        //    if (!wasThere)
        //        models.Add(item);

        //    item.Id = Guid.NewGuid().ToString("N");
        //    return item; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        //}

        //public List<KeyValuePair<DroidString, string>> CubeDroids(string modelName)
        //{
        //    if (!FindModel(modelName)) return new List<KeyValuePair<DroidString, string>>();
        //    else
        //    {
        //        MaterialsModelItem item = GetOrCreateModel(modelName);
        //        return item.DroidNames;

        //    }
        //}

        //new public MaterialsModelItem GetOrCreateModel(string name)
        //{
        //    return base.GetOrCreateModel(name);
        //}
    };

    public class PressuresCollection : GigaModelCollection<PressureModelItem>
    {
        public PressuresCollection() : base()
        {
        }

        public PressureModelItem AppendOrEditModel(PressureModelItem input)
        {
            bool wasThere = FindModel(input.Name);
            PressureModelItem item = GetOrCreateModel(input.Name);
            item.ShallowCopyFrom(input);

            if (!wasThere) models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return item;
        }


        //public bool AppendOrEditModel(string name, List<KeyValuePair<SeismicCubeFaccade, DateTime>> cubeDroids, float gradient, float offset, /*float scaling,*/ float depthScaling)
        //{
        //    bool wasThere = FindModel(name);
        //    PressureModelItem item = GetOrCreateModel(name);
        //    item.Clear();

        //    //create a clone of the list.
        //    foreach (KeyValuePair<SeismicCubeFaccade, DateTime> step in cubeDroids)
        //        item.AddCube(step.Key, step.Value);

        //    item.Gradient = gradient;
        //    item.Offset = offset;
        //    //item.PressureScaling = scaling;
        //    //item.DepthScaling = depthScaling;
        //    if (!wasThere)
        //        models.Add(item);
        //    item.Id = item.Id = Guid.NewGuid().ToString("N");

        //    return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        //}

        //new public PressureModelItem GetOrCreateModel(string name)
        //{
        //    return base.GetOrCreateModel(name);
        //}
    };

    public class BoundaryConditionsCollection : GigaModelCollection<BoundaryConditionsItem>
    {
        public BoundaryConditionsCollection() : base()
        {
        }

        public bool AppendOrEditModel(string name, DroidString Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle)
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
            item.MaxStrainAngle = angle;
            item.Id = Guid.NewGuid().ToString("N");

            if (!wasThere)
                models.Add(item);

            return true; //we are always returning true but some checking needs to be done and return false if something went wrong. 
        }

        public BoundaryConditionsItem AppendOrEditModel(BoundaryConditionsItem input)
        {


            bool wasThere = FindModel(input.Name);

            BoundaryConditionsItem item = GetOrCreateModel(input.Name);
            item.ShallowCopyFrom(input);

            if (!wasThere) models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return item;

        }






        //new public BoundaryConditionsItem GetOrCreateModel(string name)
        //{
        //    return base.GetOrCreateModel(name);
        //}
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

        public override void Clear() { MaterialModelName = string.Empty; PressureModelName = string.Empty; BoundaryConditionsModelName = string.Empty; Description = string.Empty; }

        public MaterialsModelItem MaterialsModelItem { get; set; }

        public PressureModelItem PressureModelItem { get; set; }

        public List<DateTime> Dates { get { return PressureModelItem?.Dates; } }

        public void ShallowCopyFrom(SimulationModelItem input)
        {
            Clear();
            MaterialModelName = input.MaterialModelName;
            PressureModelName = input.PressureModelName;
            BoundaryConditionsModelName = input.BoundaryConditionsModelName;
            Description = input.Description;
            Name = input.Name;

        }


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


        public SimulationModelItem AppendOrEditModel(SimulationModelItem input)
        {


            bool wasThere = FindModel(input.Name);

            SimulationModelItem item = GetOrCreateModel(input.Name);
            item.ShallowCopyFrom(input);

            if (!wasThere) models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return item;

        }

    };

    public class GigaModelDataModel //: GigamodelDomainObject
    {

        private BoundaryConditionsCollection bcCollection;

        private PressuresCollection pressuresCollection;

        private MaterialCollection materialsCollection;

        private SimulationCollection simsCollection;

        public GigaModelDataModel() //: base()
        {
            bcCollection = new BoundaryConditionsCollection();
            pressuresCollection = new PressuresCollection();
            materialsCollection = new MaterialCollection();
            simsCollection = new SimulationCollection();
            _uid = System.Guid.NewGuid();
        }

        Guid _uid;
        public Guid Guid
        {
            get
            {
                return _uid;
            }
            private set
            {
                _uid = value;
            }
        }

        public string[] PressureModelNames { get { return pressuresCollection.ModelNames; } }

        public string[] MaterialModelNames { get { return materialsCollection.ModelNames; } }

        public string[] BoundaryConditionsNames { get { return bcCollection.ModelNames; } }

        public string[] SimulationNames { get { if (simsCollection == null) simsCollection = new SimulationCollection(); return simsCollection.ModelNames; } }

        public PressuresCollection PressureModels { get { return pressuresCollection; } }

        public MaterialCollection MaterialModels { get { return materialsCollection; } }

        public BoundaryConditionsCollection BoundaryConditionsModels { get { return bcCollection; } }

        public SimulationCollection SimulationsModel { get { return simsCollection; } }

        //public void AppendOrEditPressureModel(string name, List<KeyValuePair<DroidString, DateTime>> cubeDroids, float grad, float offset, /*float scaling,*/ float depthScaling)
        //{
        //    pressuresCollection.AppendOrEditModel(name, cubeDroids, grad, offset, /*scaling,*/ depthScaling);
        //}





        public void AppendOrEditBoundaryConditionsModel(string name, DroidString Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle)
        {
            bcCollection.AppendOrEditModel(name, Datum, Offshore, gapDensity, waterDensity, strainGraientMode, emin, emax, angle);
        }

        public void AppendOrEditSimulationModel(string name, string matModelName, string pressureModelName, string bcModelName, string description)
        {
            simsCollection.AppendOrEditModel(name, matModelName, pressureModelName, bcModelName, description);
        }

    }



}
