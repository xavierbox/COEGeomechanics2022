using Slb.Ocean.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restoration.GFM
{
    [Serializable]
    public class LayerLithoData
    {
        // InitPor Athy      Split    Tensile Lamda   Kappa e0  HvEcc TanSlope    Pc0_fac E_fac   DamAlpha Pc0 Smp_dam_stn Dam_factor  Mx_dam_stn grv_thk_mod

        public LayerLithoData()
        {
            Layer = "NoSet";
            Active = true;
            InitPor = 0.5;
            Athy = 0.41;
            Density = 2300;
            Poisson = 0.3;
            YMFinal = 5.0e6;
            Split = 3;
            Cohesion = 5000;
            FrAngle = 40.0;
            Tensile = 500.0;
            Lamda = 0.27;
            Kappa = 8.6E-02;
            e0 = 0.95;
            Pc0 = 9500;
            YMLambda = 0.3;
            ERatio = 0.1;
        }

        /*
        *
        */
        public bool Active { get; set; }

        public void DeepCopy( LayerLithoData data )
        {
            Layer = data.Layer;
            InitPor = data.InitPor;
            Athy = data.Athy;
            Density = data.Density;
            Poisson = data.Poisson;
            YMFinal = data.YMFinal;
            Split = data.Split;
            Cohesion = data.Cohesion;
            FrAngle = data.FrAngle;
            Tensile = data.Tensile;

            Lamda = data.Lamda;
            Kappa = data.Kappa;
            e0 = data.e0;
            Pc0 = data.Pc0;
            YMLambda = data.YMLambda;
            YoungsModulusHardeningFunctionDroid = data.YoungsModulusHardeningFunctionDroid;
            //YMInitial = data.YMInitial;
        }

        #region visible/editable

        public string Layer { get; set; }

        public double InitPor { get; set; } = 0.5;

        public double Athy { get; set; }

        public double Density { get; set; }

        public double Poisson { get; set; }

        public double YMFinal { get; set; }

        public int Split { get; set; }

        public double Cohesion { get; set; }

        public double FrAngle { get; set; }

        public double Tensile { get; set; }

        public double Lamda { get; set; }

        public double Kappa { get; set; }

        public double e0 { get; set; }

        public double Pc0 { get; set; }

        public double YMLambda { get; set; }

        public double YMInitial { get { return YMFinal * ERatio; } }

        public double ERatio { get; set; }

        public string YoungsModulusHardeningFunctionDroid { get; set; }

        public static string[] VisiblePropertyNames
        {
            get
            {
                return new string[]
                {
                              "Layer",
                              "Initial Porosity",
                              "Athy's Coefficient",
                              "Density",
                              "Poisson",
                              "Youngs Modulus",
                              "Split",
                              "Cohesion",
                              "Friction Angle",
                              "Tensile Strength",
                              "Lamda",
                              "Kappa",
                              "e0",
                              "Pc0",
                              "Hardening Rate",
                              "Initial-Final Young's Modulus"//,
                              //"Stiffness Function"
                       };
            }
        }

        //horrorific.....FIX THIS

        public void SetValue( string visiblePropertyName, string value )
        {
            try
            {
                if (visiblePropertyName == "Layer") { Layer = value; return; }
                if (visiblePropertyName == "Initial Porosity") { InitPor = double.Parse(value); return; }
                if (visiblePropertyName == "Athy's Coefficient") { Athy = double.Parse(value); return; }
                if (visiblePropertyName == "Density") { Density = double.Parse(value); return; }
                if (visiblePropertyName == "Poisson") { Poisson = double.Parse(value); return; }
                if (visiblePropertyName == "Youngs Modulus") { YMFinal = double.Parse(value); return; }
                if (visiblePropertyName == "Split") { Split = int.Parse(value); return; }
                if (visiblePropertyName == "Cohesion") { Cohesion = double.Parse(value); return; }
                if (visiblePropertyName == "Friction Angle") { FrAngle = double.Parse(value); return; }
                if (visiblePropertyName == "Tensile Strength") { Tensile = double.Parse(value); return; }
                if (visiblePropertyName == "Lamda") { Lamda = double.Parse(value); return; }
                if (visiblePropertyName == "Kappa") { Kappa = double.Parse(value); return; }
                if (visiblePropertyName == "e0") { e0 = double.Parse(value); return; }
                if (visiblePropertyName == "Pc0") { Pc0 = double.Parse(value); return; }
                if (visiblePropertyName == "Hardening Rate") { YMLambda = double.Parse(value); return; }
                if (visiblePropertyName == "Initial-Final Young's Modulus") { ERatio = double.Parse(value); return; }

                //if (visiblePropertyName == "Stiffness Function") {  = value; return; }
            }
            catch
            {
            }
        }

        public string[] VisiblePropertyValues
        {
            get
            {
                string[] x = null;
                object obj = null;
                try { obj = DataManager.Resolve(new Droid(YoungsModulusHardeningFunctionDroid)); }
                catch { obj = null; }
                finally
                {
                    //string functionName = obj != null ? ((Function)obj).Name : "No set ";

                    x = new string[]
                    {
                              Layer.ToString(),
                              InitPor.ToString(),
                              Athy.ToString(),
                              Density.ToString(),
                              Poisson.ToString(),
                              YMFinal.ToString(),
                              Split.ToString(),
                              Cohesion.ToString(),
                              FrAngle.ToString(),
                              Tensile.ToString(),
                              Lamda.ToString(),
                              Kappa.ToString(),
                              e0.ToString(),
                              Pc0.ToString(),
                              YMLambda.ToString(),
                              ERatio.ToString()//,
                              //functionName
                    };
                }

                return x;
            }
        }

        #endregion visible/editable

        #region stupid properties needed for the script but that actually do nothing

        public double HvEcc { get { return 1.0; } }

        public double TanSlope { get { return 1.0; } }
        public double Pc0_fac { get { return 1.0; } }
        public double E_fac { get { return 1.0; } }
        public double DamAlpha { get { return 1.0; } }
        public double Smp_dam_stn { get { return 1.0; } }
        public double Dam_factor { get { return 1.0; } }
        public double Mx_dam_stn { get { return 1.0; } }
        public double grv_thk_mod { get { return 1.0; } }

        #endregion stupid properties needed for the script but that actually do nothing

        public List<KeyValuePair<double, double>> GetDVTTable()//double Ymo, double Ymf, double YmLambda, int nPoints = 50, double minStrain = -0.5, double maxStrain = 0.0)
        {
            List<KeyValuePair<double, double>> data = new List<KeyValuePair<double, double>>();
            int nPoints = 50;
            double strain = 0.0, minStrain = 0.0, maxStrain = 0.5, deltaStrain = Math.Abs((maxStrain - minStrain) / nPoints);

            int NNPoints = 1 + (int)((maxStrain - minStrain) / deltaStrain);
            double factor = YMLambda * (YMFinal - YMInitial) / (0.99 * YMFinal - YMInitial) - YMLambda;
            for (int n = 0; n < NNPoints; n++)
            {
                double ym = YMInitial + (YMFinal - YMInitial) * (strain / (factor + strain));
                data.Add(new KeyValuePair<double, double>(-strain / 3.0, ym));
                strain += deltaStrain;
            }

            //invert the table so Petrel is happy with x increasing values
            data.Reverse();
            return data;
        }
    }

    [Serializable]
    public class LithoData
    {
        public static LithoData FromVisageLithoFile( string fileName )
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);//first line is a header

                List<LayerLithoData> layers = new List<LayerLithoData>();
                for (int nn = 2; nn < lines.Count(); nn++)
                {
                    string[] words = lines[nn].Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    string aux = "";
                    int n = 0;
                    LayerLithoData l = new LayerLithoData();
                    l.Layer = (words[n++]);
                    l.InitPor = double.Parse(words[n++]);
                    l.Athy = double.Parse(words[n++]);
                    l.Density = double.Parse(words[n++]);
                    l.Poisson = double.Parse(words[n++]);
                    l.YMFinal = double.Parse(words[n++]);
                    l.Split = int.Parse(words[n++]);
                    l.Cohesion = double.Parse(words[n++]);
                    l.FrAngle = double.Parse(words[n++]);

                    l.Tensile = double.Parse(words[n++]);
                    l.Lamda = double.Parse(words[n++]);
                    l.Kappa = double.Parse(words[n++]);
                    l.e0 = double.Parse(words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);

                    aux = (words[n++]);
                    l.Pc0 = double.Parse(words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);
                    aux = (words[n++]);

                    layers.Add(l);
                }

                return new LithoData(layers);
            }
            catch
            {
                return null;
            }
        }

        public static LithoData FromCustomFile( string fileName )
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);//first line is a header

                List<LayerLithoData> layers = new List<LayerLithoData>();
                for (int nn = 1; nn < lines.Count(); nn++) //we start from line 1 since line 0 is a header
                {
                    string[] words = lines[nn].Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int n = 0;
                    LayerLithoData l = new LayerLithoData();
                    l.Layer = (words[n++]);
                    l.InitPor = double.Parse(words[n++]);
                    l.Athy = double.Parse(words[n++]);
                    l.Density = double.Parse(words[n++]);
                    l.Poisson = double.Parse(words[n++]);
                    l.YMFinal = double.Parse(words[n++]);
                    l.Split = int.Parse(words[n++]);
                    l.Cohesion = double.Parse(words[n++]);
                    l.FrAngle = double.Parse(words[n++]);

                    l.Tensile = double.Parse(words[n++]);
                    l.Lamda = double.Parse(words[n++]);
                    l.Kappa = double.Parse(words[n++]);
                    l.e0 = double.Parse(words[n++]);

                    l.Pc0 = double.Parse(words[n++]);
                    l.YMLambda = double.Parse(words[n++]);
                    l.ERatio = double.Parse(words[n++]);

                    layers.Add(l);
                }

                return new LithoData(layers);
            }
            catch
            {
                return null;
            }
        }

        override public string ToString()
        {
            string s = "Version:  4    \nLayer InitPor Athy Density Poisson Youngs  Split Cohesion    FrAngle Tensile Lamda Kappa   e0 HvEcc   TanSlope Pc0_fac E_fac  DamAlpha    Pc0 Smp_dam_stn Dam_factor Mx_dam_stn  grv_thk_mod\n";

            foreach (LayerLithoData l in Layers)
            {
                s += (l.Layer.ToString() + '\t');
                s += (l.InitPor.ToString() + '\t');
                s += (l.Athy.ToString() + '\t');
                s += (l.Density.ToString() + '\t');
                s += (l.Poisson.ToString() + '\t');
                s += (l.YMFinal.ToString() + '\t');
                s += (l.Split.ToString() + '\t');
                s += (l.Cohesion.ToString() + '\t');
                s += (l.FrAngle.ToString() + '\t');

                s += (l.Tensile.ToString() + '\t');
                s += (l.Lamda.ToString() + '\t');
                s += (l.Kappa.ToString() + '\t');
                s += (l.e0.ToString() + '\t');

                s += (l.HvEcc.ToString() + '\t');
                s += (l.TanSlope.ToString() + '\t');
                s += (l.Pc0_fac.ToString() + '\t');
                s += (l.E_fac.ToString() + '\t');

                s += (l.DamAlpha.ToString() + '\t');
                s += (l.Pc0.ToString() + '\t');
                s += (l.Smp_dam_stn.ToString() + '\t');
                s += (l.Dam_factor.ToString() + '\t');
                s += (l.Mx_dam_stn.ToString() + '\t');
                s += (l.grv_thk_mod.ToString() + '\t');

                if (l != Layers[Layers.Count() - 1])
                    s += '\n';
            }

            return s;
        }

        public LithoData( List<LayerLithoData> x )// Dictionary<string, LayerLithoData> x)
        {
            Layers = x;
        }

        public LithoData()
        {
            Layers = new List<LayerLithoData>();// Dictionary<string, LayerLithoData>();
        }

        public List<LayerLithoData> Layers
        {
            get; set;
        }

        public void DeepCopy( LithoData l )
        {
            Layers = new List<LayerLithoData>();
            foreach (LayerLithoData data in l.Layers)
            {
                LayerLithoData x = new LayerLithoData();
                x.DeepCopy(data);
                Layers.Add(data);
            }
        }
    }
}