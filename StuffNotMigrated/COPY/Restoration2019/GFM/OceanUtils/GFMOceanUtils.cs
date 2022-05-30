using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.GFM.OceanUtils
{
    class GFMOceanUtils
    {
        public static string PublishFunction(string ModelName, string name, List<KeyValuePair<double, double>> functionData)
        {

            string droid = "";

            Collection col = ProjectTools.GetOrCreateCollectionByName(ModelName);

            Function f = ProjectTools.CreateOrReplaceFunction(col, name, functionData);

            droid = f.Droid.ToString();
            return droid;
        }


        public static void GenerateDVTFunctions(string ModelName, List<LayerLithoData> data)
        {
            foreach (LayerLithoData l in data)
            {
                List<KeyValuePair<double, double>> function = GenerateYMFunctionData(l.YMInitial, l.YMFinal, l.YMLambda);
                string droid = GFMOceanUtils.PublishFunction( ModelName, l.Layer + "_YM_dvt", function);
                l.YoungsModulusHardeningFunctionDroid = droid;
            }
        }

        public static void GenerateDVTFunction(string ModelName, LayerLithoData l)
        {
            List<KeyValuePair<double, double>> function = GenerateYMFunctionData(l.YMInitial, l.YMFinal, l.YMLambda);
            string droid = GFMOceanUtils.PublishFunction( ModelName, l.Layer + "_YM_dvt", function);
            l.YoungsModulusHardeningFunctionDroid = droid;
        }

        public static List<KeyValuePair<double, double>> GenerateYMFunctionData(double Ymo, double Ymf, double YmLambda, int nPoints = 50, double minStrain = -0.5, double maxStrain = 0.0)
        {
            List<KeyValuePair<double, double>> data = new List<KeyValuePair<double, double>>();
            minStrain = 0.0;
            maxStrain = 0.5;///3.00; 
            double strain = 0.0;// minStrain;
            double deltaStrain = Math.Abs((maxStrain - minStrain) / nPoints);
            int NNPoints = 1 + (int)((maxStrain - minStrain) / deltaStrain);


            double factor = YmLambda * (Ymf - Ymo) / (0.99 * Ymf - Ymo) - YmLambda;

            for (int n = 0; n < NNPoints; n++)
            {
                double ym = Ymo + (Ymf - Ymo) * (strain / (factor + strain));
                data.Add(new KeyValuePair<double, double>(-strain / 3.0, ym));
                strain += deltaStrain;
            }

            //invert the table so Petrel is happy with x increasing values 
            data.Reverse();



            return data;

        }

    }
}
