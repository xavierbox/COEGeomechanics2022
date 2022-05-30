using System.Collections.Generic;
using System.Linq;

namespace TestWinformsStuffOutOfPetrel
{
    public class MIIBase
    {

        public string Version { get; set; }



    }



    public class AsteriskKey
    {

        public AsteriskKey(string name = "", string value = "", string preffix = "*")
        {
            Value = string.Empty;
            Preffix = "*";
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
                s += "#\n";

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


        public AsteriskKey SetOrReplaceAsterisk(string name, string value, Dictionary<string, string> hashes = null)
        {
            //search for all the commands in Commands, if found, remove it and then add a new one.
            AsteriskKey command;
            int index = Commands.FindIndex(t => t.Name == name);
            if (index >= 0) command = Commands[index];
            else command = new AsteriskKey(name, value);

            //copy the hashes. 
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
            if (index >= 0) Commands[index].SetOrReplaceHash(hashName, hashValue);
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

        public List<AsteriskKey> Commands = new List<AsteriskKey>();

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

        public MII(string ver) : base()
        {
            Version = ver;
            Commands.Add(new AsteriskKey("MODELNAME", "Default", "*"));
            Commands.Add(new AsteriskKey("NOECHO", "", "*"));
            Commands.Add(new AsteriskKey("ELASTIC", "", "*"));
            Commands.Add(new AsteriskKey("TIDY", "", "*"));
            Commands.Add(new AsteriskKey("GRAVITY", "0.0 1.0 0.0", "*"));
            Commands.Add(new AsteriskKey("INCREMENTS,S", "1     1", "*"));
            Commands.Add(new AsteriskKey("VISCOPLASTICMETHOD,S", "1.000000000E+000    1.000000000E-003", "*"));

            //this vary slightly between versions and sub-versions 
            Commands.Add(new AsteriskKey("STRUCTUREDGRID", "", "*"));
            Commands.Add(new AsteriskKey("SOLVER", "", "*"));
            Commands.Add(new AsteriskKey("HEADER", "", "*"));

            ConfigureStructuredGridSection();
            ConfigureSolverSection();

        }

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
                if (versionAsFloat >= 2018.2)
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
            hashes.Add("type", "3");
            hashes.Add("sdeflation ", "0");
            hashes.Add("vtolerance", "1.00000E-010");
            hashes.Add("serrortrap", "3");
            hashes.Add("sgpu", "0");
            hashes.Add("ngpu", "0");
            hashes.Add("device", "");
            hashes.Add("niter_stagnation", "5");
            SetOrReplaceAsterisk("SOLVER", "", hashes);
        }

        private void initAs2018V2()
        {


        }


        public override string ToString()
        {
            string s = string.Empty;


            return s;

        }

    }

}
