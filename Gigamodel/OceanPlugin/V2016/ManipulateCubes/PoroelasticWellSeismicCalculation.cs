using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject.Well;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{
    class PoroelasticWellSeismicCalculation
    {
        public PoroelasticWellSeismicCalculation()
        {;
        }


        public List<Index2> getTracesIntersectedByWells(SeismicCube ym, List<Borehole> wells)
        {
            List<Index2> l = new List<Index2>();
            foreach (Borehole w in wells)
            {
                Trajectory trajectory = w.Trajectory;//.TrajectoryCollection.WorkingTrajectory;

                var records = trajectory.TrajectoryPolylineRecords;

                foreach (TrajectoryPolylineRecord record in records)
                {
                    Point3  point = record.Point;
                    IndexDouble3 index = ym.IndexAtPosition(point);


                }

                
            }

            return l; 
        }





    }
}
