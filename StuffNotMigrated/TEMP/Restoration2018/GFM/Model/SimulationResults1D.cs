using System.Collections.Generic;
using System.Linq;

namespace Restoration.GFM.Model
{
    public class SimulationResults1D
    {
        public SimulationResults1D()
        {
            SimulationName = "";
            Step = -1;
            LayerNames = new List<string>();
            DepositionalThickness = new List<double>();
            CalculatedThickness = new List<double>();
        }

        public string SimulationName { get; set; }

        public int Step { get; set; }

        public List<string> LayerNames { get; set; }

        public List<double> DepositionalThickness { get; set; }

        public List<double> CalculatedThickness { get; set; }

        private int nLayers
        { get { return LayerNames.Count(); } }
    }
}