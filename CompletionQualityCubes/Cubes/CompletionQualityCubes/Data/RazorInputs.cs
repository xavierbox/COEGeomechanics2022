using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompletionQualityCubes.Data
{

    public class RazorProppantSection
    {
        public RazorProppantSection(Proppant proppant, double mass)
        {
            Mass = mass;
            GrainDensity = proppant.GrainDensity;
            BulkDensity = proppant.BulkDensity;
            Permeability = proppant.Permeability;
        }

        [JsonPropertyName("mass[kg]")]
        public double Mass { get; set; }

        [JsonPropertyName("grain_density[kg/m^3]")]
        public double GrainDensity { get; set; }

        [JsonPropertyName("bulk_density[kg/m^3]")]
        public double BulkDensity { get; set; }

        [JsonPropertyName("permeability[D]")]
        public double Permeability { get; set; }

        public string ToJson()
        {
            string s = "proppant:";
            var options = new JsonSerializerOptions { WriteIndented = true, };
            string jsonString = JsonSerializer.Serialize(this, options);
            return s + jsonString;
        }

    };

    public class RazorFluidSection
    {
        public RazorFluidSection(Fluid fluid, double volume)
        {
            Volume = volume;
            Density = fluid.Density;
            Viscosity = fluid.Viscosity;
        }

        [JsonPropertyName("volume[m^3]")]
        public double Volume { get; set; }

        [JsonPropertyName("mass_density[kg/m^3]")]
        public double Density { get; set; }

        [JsonPropertyName("viscosity[cP]")]
        public double Viscosity { get; set; }
    };

    public class RazorPumpingSection
    {
        public RazorPumpingSection(Proppant p, Fluid f, Pumping pump, double proppant_mass)
        {
            Rate = pump.Rate;
            FluidSection = new RazorFluidSection(f, pump.Volume);
            ProppantSection = new RazorProppantSection(p, proppant_mass);

        }

        [JsonPropertyName("rate[m^3/min]")]
        public double Rate { get; set; }

        [JsonPropertyName("fracturing_fluid")]
        public RazorFluidSection FluidSection { get; set; }

        [JsonPropertyName("proppant")]
        public RazorProppantSection ProppantSection { get; set; }

    }

    public class RazorSolverFittingParameters
    {
        [JsonPropertyName("phi_k")]
        public double PhiK { get; set; } = 1;

        [JsonPropertyName("phi_L")]
        public double PhiL { get; set; } = 0.3;

        [JsonPropertyName("phi_e")]
        public double PhiE { get; set; } = 1;
    };

    public class RazorSolverSettingsSection
    {
        [JsonPropertyName("initial_frac_radius[m]")]
        public double InitialFracRadius { get; set; } = 0.1;

        [JsonPropertyName("tip_increment[m]")]
        public double TipIncrement { get; set; } = 0.5;

        [JsonPropertyName("time_step[s]")]
        public int TimeStep { get; set; } = 10;

        [JsonPropertyName("output_time_step[s]")]
        public int OutputTimeStep { get; set; } = 60;

        [JsonPropertyName("max_time_after_shutin[s]")]
        public double MaxTimeAfterShutin { get; set; } = 3600;

        [JsonPropertyName("max_computation_time_per_case[s]")]
        public double MaxComputationTimePerCase { get; set; } = 10;

        [JsonPropertyName("stress_shadow_radius")]
        public double StressShadowRadius { get; set; } = 0;

        [JsonPropertyName("fitting_parameters")]
        public RazorSolverFittingParameters RazorSolverFittingParameters { get; set; }
            = new RazorSolverFittingParameters();
    };


    public class RazorPlacementSection
    {
        public class RazorPerforationIntervals
        {
            public RazorPerforationIntervals(List<RazorFracture> Fracs)
            {
                Fractures = Fracs;
            }

            [JsonIgnore]
            public List<RazorFracture> Fractures = null;

            [JsonIgnore]
            public List<double> PerforationLengths = null;

            [JsonPropertyName("x[m]")]
            public double[] X => Fractures.Select(t => t.StimulationLocation.X).ToArray();

            [JsonPropertyName("y[m]")]
            public double[] Y => Fractures.Select(t => t.StimulationLocation.Y).ToArray();

            [JsonPropertyName("top_TVD[m]")]
            public double[] TopTVD => Fractures.Select(t => t.StimulationLocation.Z + 0.5 * t.PerforationLength).ToArray();

            [JsonPropertyName("bot_TVD[m]")]
            public double[] BottomTVD => Fractures.Select(t => t.StimulationLocation.Z - 0.5 * t.PerforationLength).ToArray();

        };

        public class Zoneset
        {
            /*
            "top_TVD[m]": [ 1710, 1850, 1890 ],
            "bottom_TVD[m]": [ 1850, 1890, 2030 ],
            "min_horizontal_stress[bar]": [ 400, 500, 400 ],
            "youngs_modulus[GPa]": [ 3e+1, 3e+1, 3e+1 ],
            "poissons_ratio": [ 0.25, 0.25, 0.25 ],
            "fracture_toughness[kPa*m^0.5]": [ 1500, 1500, 1500 ],
            "carter_leakoff_coefficient[m/s^0.5]": [ 2.361e-05, 2.361e-05, 2.361e-05 ]
             */

        }

        public RazorPlacementSection(List<RazorFracture> Fracs)
        {
            Fractures = Fracs;

            Intervals = new RazorPerforationIntervals(Fracs);
        }

        List<RazorFracture> Fractures = null;


        [JsonPropertyName("perforation_interval")]
        public RazorPerforationIntervals Intervals { get; }

        //[JsonPropertyName("zoneset")]
        //Zoneset Zones;

    };


    public class RazorDesign
    {
        //public string Version = "2";

        [JsonPropertyName("number_of_cases")]
        public int NumberCases { get; set; }


        [JsonPropertyName("pumping")]
        public RazorPumpingSection RazorPumpingSection { get; set; }

        [JsonPropertyName("solver_settings")]
        public RazorSolverSettingsSection RazorSolverSettings { get; set; }

        [JsonPropertyName("placement")]
        public RazorPlacementSection RazorPlacementSection { get; set; }

    };

    public class RazorInput
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "2";

        [JsonPropertyName("design")]
        public RazorDesign Design { get; set; }

    };

    public class RazorDeck
    {
        [JsonIgnore]
        public string Name { get; set; }

        [JsonPropertyName("razor_input")]
        public RazorInput Input { get; set; }
    };



}
