using Gigamodel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigamodel.VisageUtils
{
    public class MIIBase
    {
        public string Version { get; set; }
    }



    public class AsteriskKey
    {

        public AsteriskKey(string name = "", string value = "", string preffix = "*", Dictionary<string, string> hashes = null)
        {
            Value = value;
            Preffix = preffix;
            Name = name;
            if (hashes != null) hashedValues = hashes;
            else
                hashedValues = new Dictionary<string, string>();
        }

        public string Preffix { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            if (Name == string.Empty) return string.Empty;


            string s = Preffix + Name + "\n";
            if (Value != string.Empty)            //the values printed below the *xxxx
            {
                s += Value;
                s += "\n";
            }
            if (hashedValues.Count() > 0)
            {
                foreach (string key in hashedValues.Keys)
                {
                    s += ("#" + key + "\t" + hashedValues[key] + "\n");
                }
                s += "#end\n";

            }

            return s;
        }

        public Dictionary<string, string> hashedValues;

        public void SetOrReplaceHash(string name, string value)
        {
            if (hashedValues.ContainsKey(name)) hashedValues[name] = value;
            else hashedValues.Add(name, value);
        }

        public void DeleteHashedKey(string name)
        {
            if (hashedValues.ContainsKey(name)) hashedValues.Remove(name);

        }

    }

    public class MII : MIIBase
    {
        private string _modelName;
        private bool _elastic;


        public MII(string ver, string modelName = "Default") : base()
        {
            Version = ver;
            StreamKeywordEnabled = true;

            _dates = new List<DateTime>();

            Commands.Add(new AsteriskKey("MODELNAME", modelName, "*"));
            Commands.Add(new AsteriskKey("NOECHO", "", "*", new Dictionary<string, string>() { { "smaterials", "1" }, { "scgm", "-100" } }));

            Commands.Add(new AsteriskKey("ELASTIC", "", "*"));
            Commands.Add(new AsteriskKey("TIDY", "", "*"));
            Commands.Add(new AsteriskKey("HEADER", "", "*"));

            Commands.Add(new AsteriskKey("GRAVITY", "0.0 1.0 0.0", "*"));
            Commands.Add(new AsteriskKey("INCREMENTS,S", "1     1", "*"));
            Commands.Add(new AsteriskKey("VISCOPLASTICMETHOD,S", "1.000000000E+000    1.000000000E-003", "*"));

            //this vary slightly between versions and sub-versions 
            Commands.Add(new AsteriskKey("STRUCTUREDGRID", "", "*"));
            Commands.Add(new AsteriskKey("SOLVER", "", "*"));

            ConfigureStructuredGridSection();
            ConfigureSolverSection();
            ConfigureOutputSection();
            ConfigureRestartSection();
            ConfigureHeaderSection();
        }

        public AsteriskKey SetOrReplaceAsterisk(string name, string value, Dictionary<string, string> hashes = null)
        {
            //search for all the commands in Commands, if found, remove it and then add a new one.
            AsteriskKey command;
            int index = Commands.FindIndex(t => t.Name == name);
            if (index >= 0) command = Commands[index];
            else { command = new AsteriskKey(name, value); Commands.Add(command); }

            //copy the hashes. 
            command.Value = value;
            command.hashedValues = hashes != null ? hashes : new Dictionary<string, string>();

            return command;
        }

        public void DeleteAsterisk(string name)
        {
            int index = Commands.FindIndex(t => t.Name == name);
            if (index >= 0) Commands.RemoveAt(index);
        }

        public void SetOrReplaceHash(string commandName, string hashName, string hashValue)
        {
            int index = Commands.FindIndex(t => t.Name == commandName);
            if (index >= 0) Commands[index].SetOrReplaceHash(hashName.Trim(), hashValue);
            else
            {
                AsteriskKey command = SetOrReplaceAsterisk(commandName, "");
                command.SetOrReplaceHash(hashName, hashValue);
            }
        }

        public string ModelName
        {
            get { return _modelName; }
            set
            {
                _modelName = value;
                SetOrReplaceAsterisk("MODELNAME", _modelName);
            }
        }

        public bool Elastic
        {
            get { return _elastic; }
            set
            {
                _elastic = value;
                int index = Commands.FindIndex(t => t.Name == "ELASTIC");
                if ((index >= 0) && (!_elastic)) Commands.RemoveAt(index);
                if ((index < 0) && (_elastic)) SetOrReplaceAsterisk("ELASTIC", "");
            }
        }

        private List<AsteriskKey> Commands = new List<AsteriskKey>();

        private void ConfigureStructuredGridSection()
        {
            float versionAsFloat;
            float.TryParse(Version, out versionAsFloat);
            if (versionAsFloat >= 2018.0)
            {
                Dictionary<string, string> gridHashes = new Dictionary<string, string>();
                gridHashes.Add("idimension", "0");
                gridHashes.Add("jdimension", "0");
                gridHashes.Add("kdimension", "0");
                gridHashes.Add("isize", "0");
                gridHashes.Add("jsize", "0");
                gridHashes.Add("ksize", "0");
                SetOrReplaceAsterisk("STRUCTUREDGRID", "", gridHashes);

                if (versionAsFloat >= 2018.199)
                {
                    gridHashes.Add("squadratic ", "0");
                    gridHashes.Add("ordering  ", "kji");
                    gridHashes.Add("sflip", "1");
                }
            }

            else { }


        }

        private void ConfigureSolverSection()
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            hashes.Add("type", "7");
            hashes.Add("sdeflation ", "0");
            hashes.Add("vtolerance", "1.00000E-08");
            hashes.Add("serrortrap", "3");
            //hashes.Add("sgpu", "0");
            //hashes.Add("ngpu", "0");
            hashes.Add("device", "");
            hashes.Add("niter_stagnation", "5");
            SetOrReplaceAsterisk("SOLVER", "", hashes);
        }

        private void ConfigureOutputSection()
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            // hashes.Add("append_model", "   ");
            hashes.Add("petrel_units", "metric");
            hashes.Add("material_data", "1");
            hashes.Add("petrel", "1");
            hashes.Add("ele_pressures", "1");
            hashes.Add("ele_strain", "1");
            hashes.Add("ele_stresses", "1");
            hashes.Add("ele_total_stresses", "1");

            hashes.Add("ele_yield_values", "1");
            hashes.Add("ele_failure_mode", "1");

            hashes.Add("ele_fault_disps", "0");
            hashes.Add("ele_fault_strain", "0");
            hashes.Add("ele_fracture_disps", "0");
            hashes.Add("ele_fracture_strain", "0");

            hashes.Add("unify_faults", "1");
            hashes.Add("unify_fractures", "1");

            hashes.Add("nodal_total_disps", "1");
            hashes.Add("petrel_nodal", "0");

            hashes.Add("ele_creep_strains", "0");
            hashes.Add("ele_dvt_variation", "0");

            hashes.Add("permeability_update", "0");
            SetOrReplaceAsterisk("RESULTS", "", hashes);





            /*
PETE's suggestion 
*RESULTS
#petrel_units                metric
#material_data                    1
#petrel                           1
#ele_pressures                    1
#ele_strain                       1
#ele_stresses                     1
#ele_total_stresses               1
#ele_yield_values                 1
#ele_failure_mode                 1

#ele_fault_strains                0
#ele_fault_disps                  0
#ele_fracture_strain              0
#ele_fracture_disps               0

#unify_faults                     1
#unify_fractures                  1

#nodal_total_disps                1
#petrel_nodal                     0
#end   

             fs.WriteLine(string.Format("{2}#{0,-20}{1,14}", "material_data",  
              fs.WriteLine(string.Format("{2}#{0,-20}{1,14}", "fbm_data", 1, comment)); 
             */
        }

        private void ConfigureRestartSection(int stepNumber = 0)
        {
            //0000.ctl 
            Dictionary<string, string> rhashes = new Dictionary<string, string>();
            if (stepNumber == 0)
            {
                rhashes.Add("Swriterestart", "1");
                rhashes.Add("Nwrite_number", "0");
                rhashes.Add("Suse_hdf5", "0");
                SetOrReplaceAsterisk("RESTART", "", rhashes);
            }


            /*ST.ctl*/
            if (stepNumber > 0)
            {
                rhashes.Add("Swriterestart", "1");
                rhashes.Add("Nwrite_number", "1");
                rhashes.Add("Sreadrestart", "1");
                rhashes.Add("Nread_number", "0");
                rhashes.Add("Saccumulatedisp", "0");
                rhashes.Add("Suse_hdf5", "0");
                SetOrReplaceAsterisk("RESTART", "", rhashes);
            }
        }

        private void ConfigureHeaderSection()
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            hashes.Add("module", "static");
            hashes.Add("solution", "1");
            hashes.Add("analysis", "3-D");
            hashes.Add("sautoplastictime", "5");
            hashes.Add("vp_timestep_factor", "0.5");
            hashes.Add("sconvergencemethod", "0");
            hashes.Add("Sanisotropic", "0");
            hashes.Add("Slocalanisotropy", "0");
            hashes.Add("Sjoint_anisotropy", "1");
            hashes.Add("Smpc", "0");
            hashes.Add("Nmpc", "0");
            hashes.Add("Sporepressures", "4");
            hashes.Add("numMeshBlocks", "1");

            hashes.Add("Nelements", "xxxxxxxxxxxxx");
            hashes.Add("Nnodes", "xxxxxxxxxxxxxxx");
            hashes.Add("Nmaterials", "xxxxxxxxxxxxx");
            hashes.Add("Nconstraints", "xxxxxxxxxxxx");
            hashes.Add("gaussrulebricks", "2");
            hashes.Add("Niterations", "99999");

            hashes.Add("Sdisplacements", "1");
            hashes.Add("Ndisplacements", "36");

            hashes.Add("Nsub_increments", "1");
            hashes.Add("Nfaults", "0");
            hashes.Add("Sfaults", "0");
            hashes.Add("Ndfn", "0");
            hashes.Add("Sdfn", "0");
            hashes.Add("Nmax_ele_per_dfn", "0");
            hashes.Add("Nmax_ele_per_fault", "0");

            hashes.Add("vcreep_tolerance", "1.000000E+000");

            hashes.Add("Sedgeload", "0");
            hashes.Add("Stemperatures", "0");
            hashes.Add("Sgravity", "3");
            hashes.Add("Saturation", "0");
            hashes.Add("Sconnect", "1");
            hashes.Add("Nconnect", "0");
            hashes.Add("Npointload", "0");
            hashes.Add("Screep", "0");
            hashes.Add("Saccelerator", "0");
            hashes.Add("sperformance_type", "2");

            //hashes.Add("--Nporepressures", "8");
            SetOrReplaceAsterisk("HEADER", "", hashes);
        }

        public List<string> Includes
        {
            get; set;
        }

        List<DateTime> _dates;

        public List<DateTime> Dates
        {
            get; set;

            //get { return _dates; }
            //set
            //{
            //    _dates = value;
            //    string values = _dates.Count().ToString() + "\n";
            //    foreach (DateTime t in _dates)
            //    values += ( Utils.DateToString(t) + "\n");
            //    SetOrReplaceAsterisk("PRINTDATES", values  /* no haseshed*/);
            //}
        }


        public bool StreamKeywordEnabled { get; set; }

        private new string ToString()
        {
            string s = ("--Generated[ Exported by: Gigamodel plugin 2018 ]\n\n\n");

            foreach (AsteriskKey key in Commands)
                s += (key.ToString() + "\n");

            foreach (string file in Includes)
                s += ("INCLUDE  " + file + "\n");

            s += "\n\n*END";

            return s;
        }

        private string GetCommandsAsString()
        {
            string s = ("--Generated[ Exported by: Gigamodel plugin 2018 ]\n\n\n");

            foreach (AsteriskKey key in Commands)
            s += (key.ToString() + "\n");

            return s; 
        }

        private string GetIncludesAsString()
        {
            string s = string.Empty;
            foreach (string file in Includes)
                s += ("INCLUDE  " + file + "\n");

            return s; 
        }


        GridDimensions _dims;
        public GridDimensions GridDimensions
        {
            get { return _dims; }
            set
            {
                _dims = value;
                int nCells = _dims.nCells;
                int nNodes = _dims.nNodes;
                SetOrReplaceHash("HEADER", "Nelements", nCells.ToString());
                SetOrReplaceHash("HEADER", "Nnodes", nNodes.ToString());
                SetOrReplaceHash("HEADER", "Nmaterials", nCells.ToString());


                int boundaryNodes = (1 + _dims.Cells[2]) * (1 + _dims.Cells[1]) * 2 +
                                    (1 + _dims.Cells[2]) * (1 + _dims.Cells[0]) * 2 +
                                    (1 + _dims.Cells[1]) * (1 + _dims.Cells[0]) * 1; //base 


                SetOrReplaceHash("HEADER", "Ndisplacements  ", boundaryNodes.ToString());
                SetOrReplaceHash("HEADER", "Nconstraints  ", "0");

                SetOrReplaceHash("STRUCTUREDGRID", "idimension ", _dims.Cells[0].ToString());
                SetOrReplaceHash("STRUCTUREDGRID", "jdimension ", _dims.Cells[1].ToString());
                SetOrReplaceHash("STRUCTUREDGRID", "kdimension ", _dims.Cells[2].ToString());

                SetOrReplaceHash("STRUCTUREDGRID", "isize  ", _dims.Size[0].ToString());
                SetOrReplaceHash("STRUCTUREDGRID", "jsize  ", _dims.Size[1].ToString());
                SetOrReplaceHash("STRUCTUREDGRID", "ksize  ", _dims.Size[2].ToString());

            }
        }

        public List<string> GetFileContentsOLD()
        {
            List<string> files = new List<string>();
            GridDimensions dims = GridDimensions;

            //this is the step 0000. Pretty much a default
            Commands.Add(new AsteriskKey("PRINTDATES", "1\n" + Utils.DateToString(Dates[0])));
            Includes = new List<string>()
            {
                ModelName+".mat",
                ModelName+".dis",
                ModelName+".edg",
                "PRESSURES0000.ppr"
            };

            files.Add(ToString());

            if (Dates.Count() > 1)
            {
                //now the coupled steps 0001, 0002, .....
                string loadStepValue = (Dates.Count() - 1).ToString() + "\n";
                for (int n = 1; n < Dates.Count; n++)
                    loadStepValue += ("PRESSURES" + n.ToString("D4") + ".ppr\n");
                Commands.Add(new AsteriskKey("LOADSTEP", loadStepValue));

                string printDatesValue = "\n" + (Dates.Count() - 1).ToString() + "\n";
                for (int n = 0; n < Dates.Count; n++)
                    printDatesValue += (Utils.DateToString(Dates[n]) + "\n");
                Commands.Add(new AsteriskKey("PRINTDATES", printDatesValue));

                Includes = new List<string>()
                    {
                    ModelName+".mat",
                    ModelName+".dis"
                     };

                files.Add(ToString());
            }

            return files;
        }

        public List<string> GetFileContents()
        {
            List<string> files = new List<string>();
            GridDimensions dims = GridDimensions;

            //this is the step 0000. Pretty much a default
            Commands.Add(new AsteriskKey("PRINTDATES", "1\n" + Utils.DateToString(Dates[0])));

            if (!StreamKeywordEnabled)
            {
                Includes = new List<string>()
                    {
                        ModelName+".mat",
                        ModelName+".dis",
                       // ModelName+".edg",
                        "PRESSURES0000.ppr"
                    };         
            }


            else
             {

                Includes = new List<string>()
                    {
                        ModelName+".dis",
                        //ModelName+".edg",
                    };

                //SetOrReplaceAsterisk(string name, string value, Dictionary<string, string> hashes = null)

                Commands.Add(new AsteriskKey("POREPRESSURE,stream",   "include PRESSURES0000.ppr", "*"));
                Commands.Add(new AsteriskKey("YOUNGS_MODULUS,stream", "include YOUNGSMOD.mat", "*"));
                Commands.Add(new AsteriskKey("POISSONS_RATIO,stream", "include POISSONR.mat",  "*"));
                Commands.Add(new AsteriskKey("SOLID_UNIT_WEIGHT,stream", "include DENSITY.mat", "*"));

            }

            string s = GetCommandsAsString() + "\n";
            s += GetIncludesAsString();
            s += "\n\n\n*END";
            files.Add(s);



            //if (Dates.Count() > 1)
            //{
            //    //now the coupled steps 0001, 0002, .....
            //    string loadStepValue = (Dates.Count() - 1).ToString() + "\n";
            //    for (int n = 1; n < Dates.Count; n++)
            //        loadStepValue += ("PRESSURES" + n.ToString("D4") + ".ppr\n");
            //    Commands.Add(new AsteriskKey("LOADSTEP", loadStepValue));

            //    string printDatesValue = "\n" + (Dates.Count() - 1).ToString() + "\n";
            //    for (int n = 0; n < Dates.Count; n++)
            //        printDatesValue += (Utils.DateToString(Dates[n]) + "\n");
            //    Commands.Add(new AsteriskKey("PRINTDATES", printDatesValue));

            //    Includes = new List<string>()
            //        {
            //        ModelName+".mat",
            //        ModelName+".dis"
            //         };

            //    files.Add(ToString());
            //}

            return files;
        }

        public List<string> GetCompressedFileContents()
        {
            List<string> files = new List<string>();

            return files;
        }



        public List<string> GetCommands()
        {
            List<string> files = new List<string>();

            return files;
        }

        public List<string> GetIncludes()
        {
            List<string> files = new List<string>();


            return files;
        }





    }

}
