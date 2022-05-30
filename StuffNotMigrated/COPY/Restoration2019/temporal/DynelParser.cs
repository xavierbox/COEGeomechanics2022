using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractureDrivers.Controllers
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

        public static Dictionary<string, List<double>> ParseFile(string fileName, out string error)
        {
            error = string.Empty;
            Dictionary<string, List<double> > data = new Dictionary<string,List<double>>();

            string[] dynelKeys;


            try
            {

                if (ValidFile(fileName))
                {
                    string line = "";

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

                        dynelKeys = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int n = 1; n < dynelKeys.Length; n++)
                        data.Add(dynelKeys[n], new List<double>());
                        string[] keys = data.Keys.ToArray();


                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int n = 0; n < values.Length; n++)
                            {
                                float value;
                                if (float.TryParse(values[n], out value))
                                data[keys[n]].Add(value);
                                

                                else
                                {
                                    error = "There is a wrong value in line " + line;
                                    return data;
                                }
                            }
                        }//reader ALL mode       

                    }//streamreader


                }
                else
                    error = "This is not a dynel file";

            }

            catch (Exception e)
            {

                string what = e.ToString();
            }


            return data;
        }

    }
}
