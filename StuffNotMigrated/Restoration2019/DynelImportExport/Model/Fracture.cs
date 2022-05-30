using CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.Model
{
    public enum FractureType
    {

        Joint = 0,
        Shear = 2,
        Styolite = 4,
        Shear1 =  3,
        Shear2 =  4,
        Shear3 =  5,
        Unknown = 1000
    }

    public class Fracture
    {
        public Fracture()
        {
            Type = FractureType.Unknown;
            Id = -1;// string.Empty;
            Set = 0;
            Group = 0;
            Observed = false;
            EquivalentPlasticStrain = float.NaN;
        }

        public Fracture(Fracture f)
        {
            Type = f.Type;
            Id = f.Id;
            Set = f.Set;
            Group = f.Group;
            Observed = f.Observed;
            Location = f.Location;
            Orientation = f.Orientation;
            Intensity = f.Intensity;
            EquivalentPlasticStrain = f.EquivalentPlasticStrain;
        }

        public FractureType Type { get; set; }

        public Orientation Orientation { get; set; }

        public int Set { get; set; }

        public Vector3 Location { get; set; }

        public int Id { get; set; }

        public int Group { get; set; }

        public bool Observed { get; set; }

        public double Intensity
        {
            get; set;
        }

        public Object Extension { get; set; }


        public double EquivalentPlasticStrain { get; set; }


    };

    public class FractureModel
    {



        public FractureModel()
        {
            _fracs = new List<Fracture>();
            Name = "FractureOrientationsModel";  
        }

        public string Name
        {
            get;set;
        }

        public void Clear()
        {
            _fracs.Clear();
        }


        //returns the non-repeated locations of all the fractures. 
        //if there is more than one fracture linked to a point, the point is returned only once. 
        //the code assumes that all the fracs for a single location are listed contiguous in the model. 
        public Vector3[] Locations
        {
            get
            {
                Vector3 lastOne = null;
                List<Vector3> locs = new List<Vector3>();
                foreach (Fracture f in _fracs)
                {
                    Vector3 loc = f.Location;
                    if (loc != lastOne)
                    {
                        lastOne = loc;
                        locs.Add(loc);
                    }
                }

                return locs.ToArray();
            }
        }

        public Dictionary<Vector3, List<Fracture>> FracturesAtLocations
        {
            get
            {
                Dictionary<Vector3, List<Fracture>> toReturn = new Dictionary<Vector3, List<Fracture>>();
                foreach (Fracture f in _fracs)
                {
                    Vector3 loc = f.Location;
                    if (!toReturn.Keys.Contains(loc))
                    toReturn.Add(loc, new List<Fracture>());
                    toReturn[loc].Add(f);
                }

                return toReturn;

            }
        }

        public List<Fracture> Fractures { get { return _fracs; } set { _fracs = value; } }


        private List<Fracture> _fracs;

    }




    public class WellFractureDataModel : FractureModel
    {
        public WellFractureDataModel() : base()
        {
            WellFractures = new Dictionary<string, List<Fracture>>();
        }

        public Dictionary<string, List<Fracture>> WellFractures { get; set; }  //well name fracture list 



    };

    public class FractureCostFunction
    {
        public FractureCostFunction()
        {
            Costs = new Dictionary<int, float>();
        }

        public Dictionary<int, float> Costs { get;set; }

        public void AddOrReplaceCostFunction( int FractureTypeCode, float cost )
        {

            Costs[FractureTypeCode] = cost;
        }

        public float GetCostForType(int type)
        {
            if (Costs.Keys.Contains(type)) return Costs[type];
            else return float.NaN;// float.MaxValue;
        }

        public int ReferenceType { get; set; }



        public float MatchingTypeCost
        {
            get
            {
                return GetCostForType(ReferenceType);
            }
        }


        public Tuple<float,int> SmallestCostAndType
        {
            get
            {
                if (Costs.Keys.Count < 1) return null;

                int correspondingType = -1;
                float smallest = float.MaxValue;
                foreach (var type in Costs.Keys)
                {
                    float cost = Costs[type];
                    if (cost < smallest) { smallest = cost; correspondingType = type; } 
                }

                return new Tuple<float, int>(smallest, correspondingType);
            }
        }



    }
}

