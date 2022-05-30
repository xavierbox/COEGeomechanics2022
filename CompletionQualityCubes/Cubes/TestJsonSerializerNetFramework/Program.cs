using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json.Linq;
using static System.Text.Json.JsonElement;

namespace TestJsonSerializerNetFramework
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

        [JsonPropertyName("dfgdfgdfgVISCOSITY")]
        public float Viscosity { get; set; }

        public String Name { get; set; }

        [JsonPropertyName("VALUES")]
        public float[] values { get; set; } = new float[] { 1.2f, 1.3f };

        string[] SummaryWords { get; set; } = new[] { "Cool", "Windy", "Humid" };

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
                    new Fluid("Water",0.0006f,1.0f),
                    new Fluid("Brine",0.001f,1.1f),
                    new Fluid("Gel",0.020f,1.2f)
                };
            }
        }

        public static Fluid Default => KnownFluids[0];
    };




    public class Proppant
    {
        public Proppant(string name, float permeability, float grain_density, float bulk_dens)
        {
            Name = name;
            GrainDensity = grain_density;
            BulkDensity = bulk_dens;
            Permeability = permeability;
        }

        //[JsonPropertyName("grain_density[kg/m^3]")]
        public float GrainDensity { get; set; }

        //[JsonPropertyName("bulk_density[kg/m^3]")]
        public float BulkDensity { get; set; }

        //[JsonPropertyName("permeability[D]")]
        public float Permeability { get; set; }

        //[JsonPropertyName("mass[kg]")]
        public float Mass { get; set; }

        //[JsonIgnore]
        public String Name { get; set; }
    };



    public class ProppantFactory
    {
        public static List<Proppant> KnownProppants
        {
            get
            {
                return new List<Proppant>()
                {
                    new Proppant("Sand 40/70",550.0006f,2.0f,1.0f),
                    new Proppant("Sand 70/30",440.001f,2.1f,1.1f),
                    new Proppant("Sand 70/10",320.020f,2.2f,1.2f)
                };
            }
        }

        public static Proppant Default => KnownProppants[0];
    };

    public class RazorFracture
    {
        public float[] AllTops { get; set; }

        public float[] StimulationLocation { get; set; }


        [JsonPropertyName("min_horizontal_stress[bar]")]
        public float[] MinStress { get; set; }
        public float[] Stiffness { get; set; }
        public float[] PoissonR { get; set; }
        public float[] Toughness { get; set; }
        public float[] Leakoff { get; set; }




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
        public double Rate { get; set; } = 3.0;

        public double Seconds { get; set; } = 150;

    };

    public class RazorSolverSettings
    {

        double InitialFracRadius { get; set; } = 0.1;

        double TipIncrement { get; set; } = 0.5;

        int TimeSteps { get; set; } = 10;

        int OutputTimeStep { get; set; } = 60;

        double MaxTimeAfterShutin { get; set; } = 3600;

        double MaxComputationTimePerCase { get; set; } = 10;

        double StressShadowRadius { get; set; } = 10;

        double PhiK { get; set; } = 1;
        double PhiL { get; set; } = 0.3;
        double PhiE { get; set; } = 1;


    };

    public class RazorDeck
    {
        [JsonPropertyName("version")]
        public string Version => "2";

        public string Name { get; set; } = "Default";

        //[JsonPropertyName("zoneset")]
        public List<RazorFracture> Fractures { get; set; } = new List<RazorFracture>();

        public Fluid Fluid { get; set; } = FluidFactory.Default;


        [JsonPropertyName("proppant")]
        public Proppant Proppant { get; set; } = ProppantFactory.Default;

        public Pumping Pumping { get; set; } = new Pumping();

        public RazorSolverSettings SolverSettings { get; set; } = new RazorSolverSettings();

        public string ToJson()
        {
            //        var fluid = new
            //        {
            //            volume[m^3] = 152.3,
            //            mass_density[kg/m^3]=1000,
            //viscosity[cP]=119.8233563
            //        }; 
            //        var pumping = new
            //        {
            //            rate[m^3/min]=3.18,
            //fracturing_fluid=fluid
            //        };

            return "s";
        }

    }


    public class RazorFracResult
    {

        public class EOJSummary
        {

            [JsonPropertyName("time[s]")]
            public double Time { get; set; }

            [JsonPropertyName("height[top,bottom][m]")]
            public double[] Height { get; set; } //= new double[2] { 0.0, 0.0 };

            [JsonPropertyName("length[left,right][m]")]
            public double[] Length { get; set; } //= new double[2] { 0.0, 0.0 };

            [JsonPropertyName("width[m]")]
            public double Width { get; set; }

            [JsonPropertyName("net_pressure[Pa]")]
            public double NetPressure { get; set; }

            [JsonPropertyName("ISIP[Pa]")]
            public double ISIP { get; set; }

            [JsonPropertyName("fluid_efficiency[%]")]
            public double Efficiency { get; set; }

        }

        public class FinalSummary
        {
            [JsonPropertyName("time[s]")]
            public double Time { get; set; }

            [JsonPropertyName("growth_time[s]")]
            public double GrowthTime { get; set; }

            [JsonPropertyName("height[top,bottom][m]")]
            public double[] Height { get; set; } //= new double[2] { 0.0, 0.0 };

            [JsonPropertyName("length[left,right][m]")]
            public double[] Length { get; set; } //= new double[2] { 0.0, 0.0 };

            [JsonPropertyName("width[m]")]
            public double Width { get; set; }

            [JsonPropertyName("proppant_width_per_design[m]")]
            public double DesignProppantWidth { get; set; }

            [JsonPropertyName("proppant_width_pumped[m]")]
            public double PumpedProppantWidth { get; set; }

        }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("initiation_TVD[m]")]
        public double InitiationTVD { get; set; }

        public double TopTip => (InitiationTVD + Height[0]);

        public double BottomTip => (InitiationTVD + Height[1]);


        [JsonPropertyName("EOJ")]
        public RazorFracResult.EOJSummary EOJ { get; set; }// = new EOJSummary();


        [JsonPropertyName("final")]
        public RazorFracResult.FinalSummary Final { get; set; }// = new EOJSummary();

        public double Time => EOJ.Time;

        public double[] Height => EOJ.Height;

        public double[] Length => EOJ.Length;

        public double Width => EOJ.Width;

        public double NetPressure => EOJ.NetPressure;

        public double ISIP => EOJ.ISIP;

        public double Efficiency => EOJ.Efficiency;




    }


    public class RazorResults
    {

        [JsonPropertyName("simulation_results")]
        public List<RazorFracResult> FracResults { get; set; }

    };


    public static class RazorResultsParser
    {
        private static string ResultsKey = "simulation_results";

        public static List<RazorFracResult> ParseSummary(string FileName)
        {
            var jDoc = JsonDocument.Parse(File.ReadAllText(FileName));
            JsonElement myClass = jDoc.RootElement.GetProperty(ResultsKey);
            List<RazorFracResult> f2 = JsonSerializer.Deserialize<List<RazorFracResult>>(myClass.GetRawText());

            return f2;
        }

        //public static RazorFracResult ParseSummary(string FileName, int ChildIndex)
        //{
        //    var jDoc = JsonDocument.Parse(File.ReadAllText(FileName));
        //    JsonElement myClass = jDoc.RootElement.GetProperty(ResultsKey);
        //    var e = myClass.EnumerateArray().ToArray()[ChildIndex];
        //    return JsonSerializer.Deserialize<RazorFracResult>(e.GetRawText());
        //}

        public static List<RazorFracResult> ParseHistory(string FileName, int FractureIndex )
        {
            return null;
        }
    
    
    
    }

    class Program
    {
        static void Main(string[] args)
        {

            RazorDeck deck = new RazorDeck();

            RazorFracture f = new RazorFracture();
            f.AllTops = new float[] { 1.1f, 1.2f, 1.3f };
            f.StimulationLocation = new float[] { 1.1f, 1.2f, 1.3f };
            f.MinStress = new float[] { 1.1f, 1.2f, 1.3f };
            f.Stiffness = new float[] { 1.1f, 1.2f, 1.3f };
            f.PoissonR = new float[] { 1.1f, 1.2f, 1.3f };
            f.Toughness = new float[] { 1.1f, 1.2f, 1.3f };
            f.Leakoff = new float[] { 1.1f, 1.2f, 1.3f };

            deck.Fractures.Add(f);


            var options = new JsonSerializerOptions 
            { WriteIndented = true, IgnoreNullValues  = true};
            //string jsonString = JsonSerializer.Serialize(deck.Proppant, options);
            //Console.WriteLine(jsonString);


            //string jsonString = JsonSerializer.Serialize(new , options);
            //Console.WriteLine(jsonString);

            //string s = new RazorProppantSection(deck.Proppant, 123.321f).ToJson();
            //Console.WriteLine(s); 

            //option 1 
            string folder = "D:\\Projects\\Programming\\CompletionQualityCubes\\";
            string fileName = folder + "outputXX2.json";
            string jsonString = File.ReadAllText(fileName);
            var fracResults = JsonSerializer.Deserialize<RazorResults>(jsonString, options)?.FracResults;

            //option 2 
            List<RazorFracResult> f2 = RazorResultsParser.ParseSummary( fileName );


            ;

            //using newton soft ...
            //JObject googleSearch = JObject.Parse(jsonString);
            //IList<JToken> results = googleSearch["simulation_results"].Children().ToList();

            //IList<RazorFracResult> fracResults = new List<RazorFracResult>();
            //foreach (JToken result in results)
            //{
            //    // JToken.ToObject is a helper method that uses JsonSerializer internally
            //    RazorFracResult searchResult = result.ToObject<RazorFracResult>();
            //    fracResults.Add(searchResult);
            //}

            Console.WriteLine("count " + fracResults.Count());


            //RazorResults results = JsonSerializer.Deserialize<RazorResults>(jsonString, options);
            //RazorResults bsObj = JsonSerializer.Deserialize .DeserializeObject<RazorResults>(jsonString);


        }
    }

}
