using Slb.Ocean.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{
    public class ControlSelection
    {
        public ControlSelection( bool newItem, string name )
        {
            SelectedAsNew = newItem;
            SelectedName = name;
        }

        public bool SelectedAsNew { get; set; }

        public string SelectedName { get; set; }

    }

    public class MaterialSelection 
    {
        public MaterialSelection(string name,  List<KeyValuePair<Droid, string>> namedDroids)// : base(newItem, name )
        {
            NamedDroids = namedDroids;
            SelectedName = name; 
        }
        public string SelectedName { get; set; }

        public List<KeyValuePair<Droid, string>> NamedDroids{ get; set; }
    }


    public class PressureSelection
    {
        public PressureSelection(string name )//, List<KeyValuePair<Droid, string>> namedDroids)// : base(newItem, name )
        {
            //NamedDroids = namedDroids;
            SelectedName = name;
        }
        public string SelectedName { get; set; }

    }

}

