using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
       


        /*
        *
        */
        public bool Active { get; set; } 

        public void DeepCopy(LayerLithoData data)
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

        public double InitPor { get; set; }

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

        public string YoungsModulusHardeningFunctionDroid { get; set; }

        public double YMInitial { get { return YMFinal * 0.2; } }

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
                              "Tensile",
                              "Lamda",
                              "Kappa",
                              "e0",
                              "Pc0",
                              "Stiffness Coefficient",
                              "Stiffness Function"

                       };
            }
        }

        //horrorific.....FIX THIS 

        public void SetValue(string visiblePropertyName, string value)
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
                if (visiblePropertyName == "Tensile") { Tensile = double.Parse(value); return; }
                if (visiblePropertyName == "Lamda") { Lamda = double.Parse(value); return; }
                if (visiblePropertyName == "Kappa") { Kappa = double.Parse(value); return; }
                if (visiblePropertyName == "e0") { e0 = double.Parse(value); return; }
                if (visiblePropertyName == "Pc0") { Pc0 = double.Parse(value); return; }
                if (visiblePropertyName == "Stiffness Coefficient") { YMLambda = double.Parse(value); return; }
                //if (visiblePropertyName == "Stiffness Function") {  = value; return; }
            }
            catch {
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
                    string functionName = obj != null ? ((Function)obj).Name : "No set ";

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
                              functionName
                    };







                }

                return x;
            }
        }


        #endregion


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

        #endregion




    }

    [Serializable]
    public class LithoData
    {

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

        public LithoData(List<LayerLithoData> x)// Dictionary<string, LayerLithoData> x)
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

        public void DeepCopy(LithoData l)
        {
            Layers = new List<LayerLithoData>();
            foreach (LayerLithoData data in l.Layers)
            {
                LayerLithoData x = new LayerLithoData();
                x.DeepCopy(data);
                Layers.Add(data);
            }
        }


        //public List< LayerLithoData> GeologicalLayers { get; set; }
        //public Dictionary<string, LayerLithoData> GeologicalLayers { get; set; }
    }
}
