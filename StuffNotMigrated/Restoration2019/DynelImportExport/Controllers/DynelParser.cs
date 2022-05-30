using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.Controllers
{

    public enum DynelParseMode
    {
        DynelOutputFiles,
        RomainOutputFiles,
        All

    }

    public class DynelParser
    {

        public static bool ValidFile(string fileName)
        {
            return true;

        }

        public static Dictionary<string, List<double>> ParseFile(string fileName, CommonData.Vector3 Traslation)
        {
            if (!ValidFile(fileName)) return null;


            Dictionary<string, List<double>> dataToReturn = new Dictionary<string, List<double>>();

            string[] dynelKeys;
            Dictionary<string, string> DynelToVisageNameChange = new Dictionary<string, string>()
            {
                { "eSxx","TOTSTRXX"},
                { "eSyy","TOTSTRYY"},  //first word, as exported byu dynel, second word ->we will call this variable with this name 
                { "eSzz","TOTSTRZZ"},
                { "eSyz","TOTSTRYZ"},
                { "eSxy","TOTSTRXY"},
                { "eSxz","TOTSTRZX"}


            };


            try
            {


                string line = "";
                bool ignored_lines = false;


                using (StreamReader sr = new StreamReader(fileName))
                {

                    line = sr.ReadLine();
                    bool keepLookingForHeader = true;

                    while (keepLookingForHeader)
                    {
                        if (!line.Contains("#"))
                            keepLookingForHeader = false;

                        line = sr.ReadLine();
                    }


                    dynelKeys = line.Replace("#", "").Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int n = 0; n < dynelKeys.Length; n++)
                    {
                        if (DynelToVisageNameChange.Keys.Contains(dynelKeys[n])) dynelKeys[n] = DynelToVisageNameChange[dynelKeys[n]];
                        dataToReturn.Add(dynelKeys[n], new List<double>());
                    }
                    string[] keys = dataToReturn.Keys.ToArray();

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        float tt;

                        bool missingvalues = values.Length != keys.Length;
                        bool nonparseable = values.Select(t => float.TryParse(t, out tt)).Any(w => w == false);


                        if ((!missingvalues) && (!nonparseable))
                        {
                            for (int n = 0; n < values.Length; n++)
                            {
                                float value = float.Parse(values[n]);
                                dataToReturn[keys[n/*+1*/]].Add(value);
                            }
                        }
                        else ignored_lines = true;

                    }//reader ALL mode       

                }//streamreader

                if (ignored_lines)
                {
                    Services.MessageService.ShowMessage("Some lines in the input file were ignored due to missing or invalid data", Services.MessageType.INFO);
                }

                List<double> x = dataToReturn["x"].Select(t => t - Traslation.X).ToList();
                List<double> y = dataToReturn["y"].Select(t => t - Traslation.Y).ToList();
                List<double> z = dataToReturn["z"].Select(t => t - Traslation.Z).ToList();

                dataToReturn["x"] = x;
                dataToReturn["y"] = y;
                dataToReturn["z"] = z;
            }


            catch (Exception e)
            {

                string what = e.ToString();
                dataToReturn = null;
            }

            return dataToReturn;
        }

    }
}
