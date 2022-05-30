using Slb.Ocean.Petrel.DomainObject.Seismic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigamodel//.GigaModelUtils
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string s) : base()
        {
            Value = s;
        }

        public string Value { get; set; }
    }

    public class StringListEventArgs : EventArgs
    {

        public StringListEventArgs(List<string> s) : base()
        {
            Values = s;
        }
        public List<string> Values { get; set; }
    }

    public class ImportResultsEventArgs : EventArgs
    {
        public ImportResultsEventArgs(List<string> s) : base() { }

        public ImportResultsEventArgs(SeismicCube refCube, List<string> names, string folder, string caseName, int time) : base()
        {
            PropertyNames = names;
            FolderName = folder;
            CaseName = caseName;
            TimeStep = time;
            ReferenceCube = refCube;
        }


        public List<string> PropertyNames;
        public string FolderName;
        public string CaseName;
        public int TimeStep;
        public SeismicCube ReferenceCube; 

    }


    public class CreateEditArgs : EventArgs
    {

        public CreateEditArgs() : base() {;}

        public bool IsNew { get; set; }

        public object Object { get; set; }

        public string Name { get; set; }

    };
}
