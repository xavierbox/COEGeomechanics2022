using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonTests
{
    //public class WeatherForecastWithPropertyNameAttribute
    //{
    //    public DateTimeOffset Date { get; set; }
    //    public int TemperatureCelsius { get; set; }

    //    [JsonPropertyName("Wind")]
    //    public int WindSpeed { get; set; }
    //}


    public partial class Form1 : Form
    {
        RazorSimulationInput _RazorInput = new RazorSimulationInput();
        public Form1()
        {
            InitializeComponent();
            _RazorInput.Name = "Hello";

            _RazorInput.YoungModulus = new float[] { 1.1f, 1.2f, 1.3f };

            _RazorInput.Fluid = FluidFactory.KnownFluids()[0];

            _RazorInput.Proppant = ProppantFactory.KnownProppants()[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int NumCellsIJK = 49;
            //int kcell = 45;
            //int kgap = 5;
            //int kmin = Math.Max(1, kcell - kgap);
            //int kmax = Math.Min(NumCellsIJK - 2, kcell + kgap);

            //IEnumerable<int> indices = Enumerable.Range(kmin, kmax - kmin + 1 + 1);

            //var all_tops = indices.Select(t => TVD);
            //var tops   all_tops.Take(indices.Count() - 1);
            //var bottoms = all_tops.Skip(1).Take(indices.Count() - 1);
            //ym = indices.Select(t => p.Accessor(new Index3(i, J, t)))

            ////convert to RazorZone
            ////Convert to json
            ////add to deck string 



            //var x = 0;





            //string jsonString = JsonSerializer.Serialize(_RazorInput);

            //Console.WriteLine(jsonString);


            //var apple = new { Item = "apples", Price = 1.35 };
            //var v = new { Amount = 108, Message = "Hello" };


            //jsonString = JsonSerializer.Serialize(v);
            //Console.WriteLine(jsonString);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //;
            //class something  
            ///{
            ////[JsonPropertyName("Wind")]
            ////float wind = 123.12f;
            //}


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }


        //         "number_of_cases":1000,
        //"pumping":{
        //             "rate[m^3/min]":3.18,
        //	"fracturing_fluid":{
        //                 "volume[m^3]":152.3,
        //		"mass_density[kg/m^3]":1000,
        //		"viscosity[cP]":119.8233563

        //             },
        //	"proppant":{
        //                 "mass[kg]":50000,
        //		"grain_density[kg/m^3]":2660,
        //		"bulk_density[kg/m^3]":1480,
        //		"permeability[D]":548

        //             }
        //         },
        //         ////////
        ///



    }
}