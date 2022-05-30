using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
using Slb.Ocean.Geometry;

namespace CompletionQualityCubes.Data
{

    public class Fluid
    {
        public Fluid(String name, double viscosity, double density)
        {
            Name = name;
            Density = density;
            Viscosity = viscosity;

        }
        public Fluid(Fluid other)
        {
            Name = other.Name;
            Density = other.Density;
            Viscosity = other.Viscosity;

        }

        public double Density { get; set; }         //kg/m3 

        public double Viscosity { get; set; }       //Pa/s          1mPa/s = 1 cP

        public String Name { get; set; }

        override public String ToString() { return Name; }
    }

    public class FluidFactory
    {
        public static List<Fluid> KnownFluids
        {
            get
            {
                return new List<Fluid>()
                {
                    new Fluid("Water",1.0016e-3f,1.0e3f),
                    new Fluid("Brine",1.1e-3f,1.1e3f),
                    new Fluid("Gel",1.2e-3f,1.2e3f)
                };
            }
        }

        public static Fluid Default => KnownFluids[0];
    };

    public class Proppant
    {
        public Proppant(string name, double permeability, double grain_density, double bulk_density)
        {
            Name = name;
            GrainDensity = grain_density;
            BulkDensity = bulk_density;
            Permeability = permeability;

        }


        public double GrainDensity { get; set; }   //kg/m3 

        public double BulkDensity { get; set; }    //kg/m3

        public double Permeability { get; set; }   //D

        public String Name { get; set; }

        override public String ToString() { return Name; }
    };

    public class ProppantFactory
    {
        public static List<Proppant> KnownProppants
        {
            get
            {
                return new List<Proppant>()
                {
                    new Proppant("Sand 40/70",550.0f,2.0e3f,1.0e3f),
                    new Proppant("Sand 70/30",340.0f,2.1e3f,1.0e3f),
                    new Proppant("Sand 70/10",220.0f,2.2e3f,1.1e3f)
                };
            }
        }

        public static Proppant Default => KnownProppants[0];
    };

    public class RazorFracture
    {
        public double[] AllTops;

        public CommonData.Vector3 StimulationLocation;

        public double PerforationLength { get; set; }

        public float[] MinStress;
        public float[] Stiffness;
        public float[] PoissonR;
        public float[] Toughness;
        public float[] Leakoff;




        //var RazorTops    = all_cell_tops.Take(indices.Count() - 1);
        //var RazorBottoms = all_cell_tops.Skip(1).Take(indices.Count() - 1);



        // "min_horizontal_stress[bar]": [ 400, 500, 400 ],
        //"youngs_modulus[GPa]": [ 3e+1, 3e+1, 3e+1 ],
        //"poissons_ratio": [ 0.25, 0.25, 0.25 ],
        //"fracture_toughness[kPa*m^0.5]": [ 1500, 1500, 1500 ],
        //"carter_leakoff_coefficient[m/s^0.5]": [ 2.361e-05, 2.361e-05, 2.361e-05 ]
    }

    public class Pumping
    {
        public double Rate { get; set; } = 3.0;        //m3/s

        public double Volume { get; set; } = 150;      //m3

    };

 
}
