using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonTests
{
    public class Fluid
    {

        public Fluid(String name, float viscosity, float density)
        {
            Name = name;
            Density = density;
            Viscosity = viscosity;

        }
        public float Density { get; set; }

        public float Viscosity { get; set; }

        public String Name { get; set; }

        override public String ToString() { return Name; }
    };

    public class FluidFactory
    {
        public static List<Fluid> KnownFluids()
        {
            return new List<Fluid>()
                {
                    new Fluid("Water",0.0006f,1.0f),
                    new Fluid("Brine",0.001f,1.1f),
                    new Fluid("Gel",0.020f,1.2f)
                };
        }
    };

    public class Proppant
    {
        public Proppant(string name, float permeability, float density)
        {
            Name = name;
            Density = density;
            Permeability = permeability;

        }
        public float Density { get; set; }

        public float Permeability { get; set; }

        public String Name { get; set; }

        override public String ToString() { return Name; }
    };

    public class ProppantFactory
    {
        public static List<Proppant> KnownProppants()
        {
            return new List<Proppant>()
                {
                    new Proppant("Sand 40/70",0.0006f,2.0f),
                    new Proppant("Sand 70/30",0.001f,2.1f),
                    new Proppant("Sand 70/10",0.020f,2.2f)
                };
        }
    };


    public class RazorSimulationInput
    {
        public Fluid Fluid { get; set; } = null;

        public Proppant Proppant { get; set; } = null;

        public string Name { get; set; } = "Default";


        float[] _ym = null, _pr = null, _tough = null;

        public float[] YoungModulus
        {
            get { return _ym; }
            set { _ym = value; }
        }

        //public PoissonsRatio { get;set; }
        //public Toughness     { get; set; }



        public string ToJson()
        {

            return "";
        }
    };


    public class RazorResults 
    {
        simulation_results
        public List<>
    
    }


}
