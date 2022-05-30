using CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.GFM.Model
{
    class SimulationBoundary
    {
        public static string Simulation1DFileDescription(Vector3 center, double edgeSize = 50.0)
        {
            double hEdgeSize = 0.5 * edgeSize;

            Vector3 p1 = center + new Vector3(-hEdgeSize, -hEdgeSize, 0.0);
            Vector3 p2 = center + new Vector3(hEdgeSize, -hEdgeSize, 0.0);
            Vector3 p3 = center + new Vector3(hEdgeSize, hEdgeSize, 0.0);
            Vector3 p4 = center + new Vector3(-hEdgeSize, hEdgeSize, 0.0);
            Vector3 [] pts = new Vector3[]{ p1, p2, p3, p4 };

string x = @"################################################################
Analysis:
1D------ > 1
Sector-- > 2
3D------ > 3
Model Boundary:
X1 Y1
X2 Y2
X3 Y3
X4 Y4
################################################################
1
";



            //string x = @"######################################################################\n";
            //x += "Analysis:\n";
            //x += "1D------ > 1\n";
            //x += "Sector-- > 2\n";
            //x += "3D------ > 3\n";
            //x += "Model Boundary:\n";
            //x += "X1 Y1\n";
            //x += "X2 Y2\n";
            //x += "X3 Y3\n";
            //x += "X4 Y4\n";
            //x += "######################################################################\n";
            //x += "1\n";

            
            foreach (Vector3 p in pts)
                x += (p.X + "  " + p.Y + ( p != pts[pts.Count()-1]? "\n" : "" ));
            return x;
        }

    };

    class Simulation3DBoundary
    {
        Simulation3DBoundary(Vector3[] pts) : base()
        {
            Points = pts;
        }

        public Vector3[] Points { get; set; } 
       

        public new string ToString()
        {
            string x = "######################################################################\n";
            x += "Analysis:\n";
            x += "1D------ > 1\n";
            x += "Sector-- > 2\n";
            x += "3D------ > 3\n";
            x += "Model Boundary:\n";
            x += "X1 Y1\n";
            x += "X2 Y2\n";
            x += "X3 Y3\n";
            x += "X4 Y4\n";
            x += "######################################################################\n";
            x += "3\n";

            Vector3[] pts = Points;
            foreach (Vector3 p in pts)
                x += (p.X + "  " + p.Y + "\n");
            return x;
        }

    };


}
