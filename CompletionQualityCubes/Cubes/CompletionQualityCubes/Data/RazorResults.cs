using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace CompletionQualityCubes.Data
{
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

        public static List<RazorFracResult> ParseHistory(string FileName, int FractureIndex)
        {
            return null;
        }

        public static void ProcessAndCompressResults(string FileName)
        { 
        }
    }

}
