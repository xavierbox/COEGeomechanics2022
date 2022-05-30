using CommonData;
using Restoration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string s) : base()
        {
            Value = s;
        }

        public string Value { get; set; }
    }

    public class DynelDataAndWellsEventArgs : EventArgs
    {
        public DynelDataAndWellsEventArgs() : base()
        {

        }

        public Dictionary<string, List<Fracture>> ObservedData { get; set; }




    };

    public class SimulationEventArgs : EventArgs
    {
        public SimulationEventArgs(string name = "Default", int dims = 1, int resol = 100,  Vector3[] boundaryPts = null, bool launch = false, object data = null)
        {
            Data = data;
            Dims = dims;
            Resolution = resol;
            BoundaryPoints = boundaryPts;
            Name = name;
            LaunchVisageAfterDeck = launch;
            Cores = 1; 
        }
        public string Name { get; set; }


        public int Dims { get; set; }

        public int Resolution { get; set; } 
      
        public Vector3[] BoundaryPoints
        {
            get;set; 
        }
        public bool LaunchVisageAfterDeck { get; set; }

        public object Data { get; set; }

        public int Cores { get; set; }

    }




    public class DataEvent<T> : EventArgs
    {
        public DataEvent() : base()
        {
            Data = default(T);// null;
        }

        public DataEvent(T inputData) : base()
        {
            Data = inputData;// null;
        }

        public T Data { get; set; }

        // public Dictionary<string, List<Fracture>> ObservedData { get; set; }

    };

    public class ModelToWellsComparisonEvent : EventArgs
    {

        public ModelToWellsComparisonEvent() : base()
        {
            ObservedData = null;
            WellSearchRadius = -1.0;
            UseCustomSearchRadius = false;
        }

        public double WellSearchRadius { get; set; }


        public bool UseCustomSearchRadius { get; set; }


        public WellFractureDataModel ObservedData { get; set; }

        public FractureModel PredictedModel { get; set; } 



    };


    public class DynelDatatoProcessEvent : EventArgs
    {
        public DynelDatatoProcessEvent() : base()
        {
            //ObservedData = new Dictionary<string, List<Fracture>>();
            //WellSearchRadius = -1.0;
        }

        // public Dictionary<string, List<Fracture>> ObservedData { get; set; }

        public string DataFileName { get; set; }

        public Vector3 Traslation { get; set; }

        //public double WellSearchRadius { get; set; }

        //public double SearchRadius { get; set; }

        //public bool UseCustomSearchRadius { get; set; } 

    };


    class FracturePredictionFromStressEvent
    {

        public FracturePredictionFromStressEvent(Dictionary<string, float[]> Stress, double frictionAngle = 30.0, Vector3[] positions = null, bool applyFilteringOfOutlayers = true )
        {
            StressTensor = Stress;
            Locations = positions;
            FrictionAngle = frictionAngle;
            ApplyRotation = false;
            ApplyFiltering = applyFilteringOfOutlayers;
        }

        public Vector3[] Locations { get; set; }

        public double FrictionAngle { get; set; }

        public Dictionary<string, float[]> StressTensor { get; set; }

        public bool ApplyFiltering { get; set;}

        //public float RotationAngle { get; set; }
        public bool ApplyRotation { get; set;  }
    };





}
