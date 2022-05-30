using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{
    public class GridDimensions
    {
        public int nNodes { get { return (1 + Cells[0]) * (1 + Cells[1]) * (1 + Cells[2]); } }

        public int nCells { get { return Cells[0] * Cells[1] * Cells[2]; } }

        public double[] Spacing { get; set; }

        public double[] Origin { get; set; }

        public int[] Cells { get; set; }

        public double[] Size { get; set; }

        public override string ToString()
        {
            string gridConfigFileSection = "\nGRID:\n";
            gridConfigFileSection += ("\tCELLS: " + Cells[0] + " " + Cells[1] + " " + Cells[2] + "\n");
            gridConfigFileSection += ("\tSPACING: " + Spacing[0] + " " + Spacing[1] + " " + Spacing[2] + "\n");
            gridConfigFileSection += ("\tORIGIN: " + Origin[0] + " " + Origin[1] + " " + Origin[2] + "\n");

            gridConfigFileSection += "\n\n";
            return gridConfigFileSection;
        }
    }

    public class Utils
    {
        public static string DateToString( DateTime t )
        {
            return t.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
        }

        static public Stopwatch StartStopWatch()
        {
            Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            return stopWatch;
        }

        static public string GetElapsedTimeText(Stopwatch w)
        {
            TimeSpan ts = w.Elapsed;
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        }


    }
}
