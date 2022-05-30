using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DroidString = System.String;

namespace Gigamodel.Data
{
    [Serializable]
    public abstract class ANamedItem
    {
        public ANamedItem()
        {
        }

        public string Name { get; set; } = "Default";

        public string Id { get; set; } = string.Empty;

        public abstract ANamedItem DeepCopy();

        public abstract void CopyFrom( ANamedItem input );
    };

    [Serializable]
    public class MaterialsModelItem : ANamedItem
    {
        public MaterialsModelItem()
        {
        }

        public void Clear()
        {
            namedCubes.Clear(); Id = string.Empty;
        }

        public void AddCube( SeismicCubeFaccade s )
        {
            int i = Names.IndexOf(s.Name);
            if (i >= 0) { namedCubes.RemoveAt(i); }
            namedCubes.Add(s);
        }

        public List<DroidString> Droids { get => namedCubes.Select(t => t.DroidString).ToList(); }

        public List<string> Names { get => namedCubes.Select(t => t.Name).ToList(); }

        public SeismicCubeFaccade YoungsModulus
        {
            get => FindByName("YOUNGSMOD");
            set => AddCube(value);
        }

        public SeismicCubeFaccade PoissonsRatio
        {
            get => FindByName("POISSONR");
            set => AddCube(value);
        }

        public SeismicCubeFaccade Density
        {
            get => FindByName("DENSITY");
            set => AddCube(value);
        }

        public SeismicCubeFaccade FindByName( string name )
        {
            int i = Names.IndexOf(name);
            return (i >= 0 ? namedCubes[i] : null);
        }

        public float TensileRatio { get; set; } = 1.0f;

        public override void CopyFrom( ANamedItem item )
        {
            Clear();
            MaterialsModelItem input = (MaterialsModelItem)item;
            namedCubes.AddRange(input.namedCubes);
            TensileRatio = input.TensileRatio;
        }

        public override ANamedItem DeepCopy()
        {
            MaterialsModelItem item = new MaterialsModelItem();
            item.CopyFrom(this);
            return item;
        }

        public List<SeismicCubeFaccade> Cubes { get => namedCubes; }

        protected List<SeismicCubeFaccade> namedCubes = new List<SeismicCubeFaccade>();
    };

    [Serializable]
    public class PressureModelItem : ANamedItem
    {
        public PressureModelItem()
        {
        }

        public SeismicCubeFaccade InitialPressure
        {
            get { return datedCubes[0].Key; }
        }

        public void Clear()
        {
            datedCubes.Clear();
        }

        public void AddCube( SeismicCubeFaccade s, DateTime t )
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

        public List<string> Names { get { return datedCubes.Select(t => t.Key.Name).ToList(); } }

        public float Offset { get; set; }

        public int Steps { get { return datedCubes.Count(); } }

        public List<SeismicCubeFaccade> Cubes
        {
            get
            {
                return datedCubes.Select(t => t.Key).ToList();
            }
        }

        public override void CopyFrom( ANamedItem item )
        {
            Clear();
            PressureModelItem input = (PressureModelItem)(item);
            foreach (KeyValuePair<SeismicCubeFaccade, DateTime> s in input.datedCubes)
            {
                datedCubes.Add(new KeyValuePair<SeismicCubeFaccade, DateTime>(s.Key, s.Value));
            }

            Offset = input.Offset;
            Gradient = input.Gradient;
        }

        public override ANamedItem DeepCopy()
        {
            PressureModelItem item = new PressureModelItem();
            item.CopyFrom(this);
            return item;
        }

        private List<KeyValuePair<SeismicCubeFaccade, DateTime>> datedCubes = new List<KeyValuePair<SeismicCubeFaccade, DateTime>>();
    };

    [Serializable]
    public class BoundaryConditionsItem : ANamedItem
    {
        public BoundaryConditionsItem() : base()
        {
        }

        public float MinStrain { get; set; } = 0.0f;

        public float MaxStrain { get; set; } = 0.0f;

        public float MaxStrainAngle { get; set; } = 0.0f;

        public bool Offshore { get; set; }

        public float GapDensity { get; set; }

        public float SeaWaterDensity { get; set; }

        public bool StrainModeGradients { get; set; }

        public DroidString DatumDroid { get; set; }

        public float DatumValue { get; set; }

        public override void CopyFrom( ANamedItem item )
        {
            BoundaryConditionsItem input = (BoundaryConditionsItem)(item);
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

        public override ANamedItem DeepCopy()
        {
            BoundaryConditionsItem item = new BoundaryConditionsItem();
            item.CopyFrom(this);
            return item;
        }
    };

    [Serializable]
    public class GigaModelCollection<T> : IEnumerable<T> where T : ANamedItem, new()
        //a colllection represents a group of pressures, a group of material models, a group of boundary conditions, etc...
    {
        protected List<T> _models;

        protected GigaModelCollection()
        {
            _models = new List<T>();
        }

        public T GetOrCreateModel( string name )
        {
            T item = null;

            if (!_models.Any(t => t.Name == name))  //not there, create one
            {
                item = new T();
                item.Name = name;
            }
            else
            {
                item = (_models.Where(t => t.Name == name).Select(t => t)).ToArray()[0];
            }

            return item;
        }

        public T AppendOrEditModel( T input )
        {
            T item = GetOrCreateModel(input.Name);
            item.CopyFrom(input);

            if (!FindModel(input.Name))
            {
                _models.Add(item);
            }

            item.Id = Guid.NewGuid().ToString("N");
            return item;
        }

        public string[] ModelNames
        {
            get { return _models.Select(s => s.Name).ToArray(); }
        }

        public bool FindModel( string name ) => _models.Any(s => s.Name == name);

        public void Clear()
        {
            _models.Clear();
        }

        public int Size { get { return _models.Count; } }

        public bool DeleteModelbyName( string name )
        {
            List<string> names = _models.Select(t => t.Name).ToList();
            int index = names.IndexOf(name);
            if (index < 0) return false;

            _models.RemoveAt(index);

            return true;
        }

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return new ItemEnumerator<T>(_models);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public class ItemEnumerator<T> : IEnumerator<T>
        {
            public List<T> _items = null;
            private int _pos = -1;

            public ItemEnumerator( List<T> items )
            {
                _items = items;
            }

            public T Current => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        };

        #endregion IEnumerable
    };

    [Serializable]
    public class MaterialCollection : GigaModelCollection<MaterialsModelItem>
    {
        public MaterialCollection() : base()
        {
        }
    };

    [Serializable]
    public class PressuresCollection : GigaModelCollection<PressureModelItem>
    {
        public PressuresCollection() : base()
        {
        }
    };

    [Serializable]
    public class BoundaryConditionsCollection : GigaModelCollection<BoundaryConditionsItem>
    {
        public BoundaryConditionsCollection() : base()
        {
        }

        public bool AppendOrEditModel( string name, DroidString Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle )
        {
            bool wasThere = FindModel(name);
            BoundaryConditionsItem item = GetOrCreateModel(name);
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
                _models.Add(item);

            return true; //we are always returning true but some checking needs to be done and return false if something went wrong.
        }
    };

    [Serializable]
    public class SimulationModelItem : ANamedItem
    {
        public SimulationModelItem()
        {
        }

        public string MaterialModelName { get; set; }

        public string PressureModelName { get; set; }

        public string BoundaryConditionsModelName { get; set; }

        public string Description { get; set; }

        public void Clear()
        {
            MaterialModelName = string.Empty; 
            PressureModelName = string.Empty; 
            BoundaryConditionsModelName = string.Empty; 
            Description = string.Empty;
        }

        public MaterialsModelItem MaterialsModelItem { get; set; }

        public PressureModelItem PressureModelItem { get; set; }

        public List<DateTime> Dates { get => PressureModelItem?.Dates; }

        public override void CopyFrom( ANamedItem item )
        {
            Clear();
            SimulationModelItem input = (SimulationModelItem)item;
            MaterialModelName = input.MaterialModelName;
            PressureModelName = input.PressureModelName;
            BoundaryConditionsModelName = input.BoundaryConditionsModelName;
            Description = input.Description;
            Name = input.Name;
        }

        public override ANamedItem DeepCopy()
        {
            SimulationModelItem item = new SimulationModelItem();
            item.CopyFrom(this);
            return item;
        }
    };

    [Serializable]
    public class SimulationCollection : GigaModelCollection<SimulationModelItem>
    {
        public SimulationCollection() : base()
        {
            //_models = new List<SimulationModelItem>();
        }

        public bool AppendOrEditModel( string name, string matsModelName, string pressureModelName, string bConditionsModelName, string description )
        {
            bool wasThere = FindModel(name);
            SimulationModelItem item = GetOrCreateModel(name);
            item.Clear();

            item.MaterialModelName = matsModelName;
            item.PressureModelName = pressureModelName;
            item.BoundaryConditionsModelName = bConditionsModelName;
            item.Description = description;

            if (!wasThere)
                _models.Add(item);

            item.Id = Guid.NewGuid().ToString("N");
            return true; //we are always returning true but some checking needs to be done and return false if something went wrong.
        }

        //new public SimulationModelItem GetOrCreateModel(string name)
        //{
        //    return base.GetOrCreateModel(name);
        //}

        //public SimulationModelItem AppendOrEditModel(SimulationModelItem input)
        //{
        //    bool wasThere = FindModel(input.Name);

        //    SimulationModelItem item = GetOrCreateModel(input.Name);
        //    item.CopyFrom(input);

        //    if (!wasThere) _models.Add(item);

        //    item.Id = Guid.NewGuid().ToString("N");
        //    return item;

        //}
    };

    [Serializable]
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

        private Guid _uid;

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

        public string[] PressureModelNames => pressuresCollection.ModelNames;

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

        public void AppendOrEditBoundaryConditionsModel( string name, DroidString Datum, bool Offshore, float gapDensity, float waterDensity, bool strainGraientMode, float emin, float emax, float angle )
        {
            bcCollection.AppendOrEditModel(name, Datum, Offshore, gapDensity, waterDensity, strainGraientMode, emin, emax, angle);
        }

        public void AppendOrEditSimulationModel( string name, string matModelName, string pressureModelName, string bcModelName, string description )
        {
            simsCollection.AppendOrEditModel(name, matModelName, pressureModelName, bcModelName, description);
        }
    }
}