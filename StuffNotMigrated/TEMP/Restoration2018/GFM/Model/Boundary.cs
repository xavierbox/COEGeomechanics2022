using CommonData;
using System.Linq;

namespace Restoration.GFM.Model
{
    internal class SimulationBoundary
    {
        public static string AsStringDescription( Vector3[] pts )
        {
            Vector3[] BoundaryFourPoints = null;
            int dims;
            if (pts.Count() == 1)      //it is a 1D simulation.
            {
                dims = 1;
                BoundaryFourPoints = GetPolygonPointsAroundCenterOPoint(pts.ElementAt(0));
            }
            else if (pts.Count() == 4) //it is a 3D simulation
            {
                dims = 3;
                BoundaryFourPoints = sortPointsFor3DBoundary(pts);
            }
            else
            {
                return string.Empty;
            }

            string description = GetHeaderForBoundaryDescription(dims);// pts.Count() == 1 ? 1 : 3);

            foreach (Vector3 p in BoundaryFourPoints)
                description += (p.X + "  " + p.Y + (p != pts[pts.Count() - 1] ? "\n" : ""));
            return description;
        }

        private static Vector3[] GetPolygonPointsAroundCenterOPoint( Vector3 center, double edgeSize = 100 )
        {
            double hEdgeSize = 0.5 * edgeSize;
            Vector3 p1 = center + new Vector3(-hEdgeSize, -hEdgeSize, 0.0);
            Vector3 p2 = center + new Vector3(hEdgeSize, -hEdgeSize, 0.0);
            Vector3 p3 = center + new Vector3(hEdgeSize, hEdgeSize, 0.0);
            Vector3 p4 = center + new Vector3(-hEdgeSize, hEdgeSize, 0.0);
            Vector3[] pts = new Vector3[] { p1, p2, p3, p4 };
            return pts;
        }

        private static string GetHeaderForBoundaryDescription( int dims )
        {
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
DIMSGOESHERE
";
            return x.Replace("DIMSGOESHERE", dims.ToString());
        }

        //private static string Simulation1DBoundaryDescription(Vector3 center, double edgeSize = 100.0)
        //{
        //    Vector3[] pts = GetPolygonPointsAroundCenterOPoint(center, edgeSize);

        //    string descriptionText = GetHeaderForBoundaryDescription(1);
        //    foreach (Vector3 p in pts)
        //    descriptionText += (p.X + "  " + p.Y + ( p != pts[pts.Count()-1]? "\n" : "" ));
        //    return descriptionText;
        //}

        public static Vector3[] sortPointsFor3DBoundary( Vector3[] vecs )
        {
            Vector3[] toReturn = new Vector3[] { vecs[0], vecs[1], vecs[2], vecs[3] };
            Vector3 axis1 = (vecs[1] - vecs[0]); axis1.normalize();
            Vector3 axis2 = (vecs[2] - vecs[1]); axis2.normalize();

            //the cross product must have positive z otherwise, they arent in counter-clock wise order and we need to invert them.
            double zCross = axis1.crossProduct(axis2).Z;
            if (zCross < 0.0)
            {
                Vector3 tm = new Vector3(vecs[3].X, vecs[3].Y, vecs[3].Z);
                vecs[3].X = vecs[1].X;
                vecs[3].Y = vecs[1].Y;
                vecs[3].Z = vecs[1].Z;

                vecs[1].X = tm.X;
                vecs[1].Y = tm.Y;
                vecs[1].Z = tm.Z;
            }

            return toReturn;
        }
    };

    internal class Simulation3DBoundary
    {
        private Simulation3DBoundary( Vector3[] pts ) : base()
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