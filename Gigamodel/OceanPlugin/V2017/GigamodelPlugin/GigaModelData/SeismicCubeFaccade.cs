using Gigamodel.GigaModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigamodel.Data
{

    //Pretty much everything we need to validate user's input and to store in the model. No data, light 
    public class SeismicCubeFaccade
    {
        public SeismicCubeFaccade(string name, string units, int[] isize, string droid)
        {
            Name = name;
            Units = units;
            Size = isize;
            DroidString = droid; 
        }

        public SeismicCubeFaccade()
        {
            Name = string.Empty;
            Units = string.Empty;
            Size = new int[3] { 0,0,0};
            DroidString = string.Empty;
        }

        public string Name { get; set; }

        public string Units { get; set; }

        public int[]  Size { get; set; }

        public string DroidString { get; set; }


        public SplitCubeFloats SplitCube { get; set; } 

    };
}
