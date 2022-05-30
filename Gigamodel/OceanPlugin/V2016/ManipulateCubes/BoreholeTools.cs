using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{


    public class BoreholeTools
    {
        public static WellLog GetOrCreateLog(string intendedName, Borehole well, Template templateInput, string subFolder = null)
        {

            if (!PetrelProject.IsPrimaryProjectOpen)
            {
                return null;
            }
            var template = Template.NullObject;
            if (templateInput == null)
            {
                template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            }
            else
            {
                template = templateInput;
            }

            var wellRoot = WellRoot.Get(PetrelProject.PrimaryProject);
            var foundLogSameName = false;
            var rootFolder = wellRoot.LogVersionCollection;
            WellLogVersion newVersion = null;

            if (rootFolder.GetUniqueName(intendedName) != intendedName)
            {
                var allVersions = wellRoot.AllWellLogVersions;
                foreach (var logVersion in allVersions)
                {
                    if (logVersion.Name == intendedName && (template == Template.NullObject || logVersion.Template == template))
                    {
                        foundLogSameName = true;
                        newVersion = logVersion;
                        break;
                    }
                }
            }

            if (!foundLogSameName)
            {
                using (var trans = DataManager.NewTransaction())
                {
                    var rootLvColl = wellRoot.LogVersionCollection;
                    trans.Lock(rootLvColl);
                    newVersion = rootLvColl.CreateWellLogVersion(intendedName, template);
                    trans.Commit();
                }
            } /*
            else
            {
                foreach (WellLogVersion wellLogVersion in wellRoot.AllWellLogVersions)
                {
                    if (wellLogVersion.Name == intendedName) { newVersion = wellLogVersion; break; }
                }
            }*/

            var wellLogToReturn = WellLog.NullObject;
            if (!well.Logs.CanCreateWellLog(newVersion))
            {
                wellLogToReturn = well.Logs.GetWellLog(newVersion);
            }
            else
            {
                using (var trans = DataManager.NewTransaction())
                {
                    trans.Lock(well);
                    wellLogToReturn = well.Logs.CreateWellLog(newVersion);
                    trans.Commit();
                }
            }
            return wellLogToReturn;
        } //getorcreateLog   

        public static void ReplaceLog(Borehole well, WellLog log, List<float> values, List<float> mds, bool sort = true)
        {
            if (values.Count != mds.Count)
            {
                return;
            }
            if (values.Count == 0)
            {
                return;
            }
            if (!log.IsWritable)
            {
                return;
            }

            var valuesCorrected = new List<WellLogSample>();

            for (var i = 0; i < values.Count; i++)
            {
                var sample = new WellLogSample(mds[i], (float)values[i]);
                valuesCorrected.Add(sample);
            }

            ////////
            //   PetrelLogger.InfoOutputWindow("Log = " + log.Name);
            //for (int k = 0; k < 10; k++)
            //PetrelLogger.InfoOutputWindow("++Sorted samples berofe sorting are " + valuesCorrected[k].MD);

            if (sort)
            {
                sortSamples(valuesCorrected);
            }
            // for (int k = 0; k < valuesCorrected.Count; k++)
            //     PetrelLogger.InfoOutputWindow(log.Name + " --Sorted samples after sorting are " + valuesCorrected[k].MD);

            var somethingIsWrong = false;
            try
            {
                for (var k = 0; k < valuesCorrected.Count - 1; k++)
                {
                    if (valuesCorrected[k + 1].MD < valuesCorrected[k].MD)
                    {
                        PetrelLogger.InfoOutputWindow("wrong k =" + k + " " + valuesCorrected[k].MD + " " + valuesCorrected[k + 1].MD);
                    }

                    if (double.IsNaN(valuesCorrected[k].MD))
                    {
                        somethingIsWrong = true;
                    }
                    //PetrelLogger.InfoOutputWindow("Nan in k =" + k + " = "); ;

                    if (double.IsInfinity(valuesCorrected[k].MD))
                    {
                        somethingIsWrong = true;
                    }
                    // PetrelLogger.InfoOutputWindow("inf Nan in k =" + k + " = "); ;


                    if (double.IsNaN(valuesCorrected[k].Value))
                    {
                        somethingIsWrong = true;
                    }
                    // PetrelLogger.InfoOutputWindow("Nan in k =" + k + " = "); ;

                    if (double.IsInfinity(valuesCorrected[k].Value))
                    {
                        somethingIsWrong = true;
                    }
                    // PetrelLogger.InfoOutputWindow("inf Nan in k =" + k + " = "); ;
                }
                if (somethingIsWrong)
                {
                    PetrelLogger.InfoOutputWindow("Something is wrong for log " + log.Name);
                }
                if (!somethingIsWrong)
                {
                    using (var trans = DataManager.NewTransaction())
                    {
                        //  PetrelLogger.InfoOutputWindow("Entering in the transaction for " + log.Name);
                        trans.Lock(log);
                        log.Samples = valuesCorrected;
                        //log.Samples.SetSamples(0.0, valuesCorrected);
                        //   PetrelLogger.InfoOutputWindow("Committing the transaction");

                        trans.Commit();
                    }
                    //   PetrelLogger.InfoOutputWindow("end of the transaction");
                }
            } //try

            catch (Exception e)
            {
                PetrelLogger.InfoOutputWindow("Failure here [1] \n" + e.ToString());
            }


            //PetrelLogger.InfoBox("Leaving routine here 1 ");
        } //replace Log  


        private static void sortSamples(List<WellLogSample> x)
        {
            for (var passes = 0; passes < x.Count - 1; passes++)
            {
                for (var j = 0; j < x.Count - passes - 1; j++)
                {
                    var comparison = 1; // = DateTime.Compare( x[j].Date, x[j+1].Date);
                    double z1 = x[j].MD, z2 = x[j + 1].MD;
                    if (z1 < z2)
                    {
                        comparison = 0;
                    }

                    if (comparison > 0)
                    {
                        var hold = x[j];
                        x[j] = x[j + 1];
                        x[j + 1] = hold;
                    }
                }
            } //Bubble Sorting finished    
        }

        public static Point3 getPositionAtWellMD(Borehole b, double md)
        {
            double x = b.Transform(Domain.MD, md, Domain.X); // Access by MD, no point in specifying limit.
            double y = b.Transform(Domain.MD, md, Domain.Y);
            double z = b.Transform(Domain.MD, md, Domain.ELEVATION_DEPTH);
            Point3 p = new Point3(x, y, z);
            return p;
        }

        public static double getDipAtWellMD(Borehole b, double md)
        {
            double value = 180 / 3.1415926535 * b.Transform(Domain.MD, md, Domain.INCLINATION);
            Console.WriteLine("Returning dip as a floas : " + value);

            return  /*0.5*3.1415926535-*/b.Transform(Domain.MD, md, Domain.INCLINATION);
        }

        public static double getDirAtWellMD(Borehole b, double md)
        {
            Console.WriteLine("Returning dir as a floas : " + -b.Transform(Domain.MD, md, Domain.AZIMUTH_GN) * 180 / 3.14);
            return b.Transform(Domain.MD, md, Domain.AZIMUTH_GN);
        }

        public static List<Point3> getTrajectoryPoints(Borehole w)
        {
            List<Point3> pts = new List<Point3>();
            Trajectory trajectory = w.Trajectory;

            var records = w.Trajectory.TrajectoryPolylineRecords;
            foreach (TrajectoryPolylineRecord record in records)
            {
                Point3 point = getPositionAtWellMD(w, record.MD);
                //Point3 point2 = record.Point;
                pts.Add(point);
            }
            return pts;

        }

        public static List<double> getTrajectoryMDS(Borehole w)
        {
            return w.Trajectory.TrajectoryPolylineRecords.Select(t => t.MD).ToList();
        }




        public static List<Borehole> GetAllBoreholesInsideCollectionRecursive(BoreholeCollection c)
        {
            var ws = new List<Borehole>();
            foreach (Borehole b in c)
            {
                ws.Add(b);
            }
            //ws.AddRange(c.);
            foreach (var c2 in c.BoreholeCollections)
            {
                ws.AddRange(GetAllBoreholesInsideCollectionRecursive(c2));
            }

            return ws;
        }


    };


    public class DistanceCalculationMethods
    {
        public DistanceCalculationMethods()
        {
        }
        public float DistanceToRegularHeightFieldSurface(Point3 p, RegularHeightFieldSurface surface)
        {
            var s = surface;// as RegularHeightFieldSurface;
            var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
            return i2 != null ? (float)(s[(int)i2.I, (int)i2.J] - p.Z) : float.NaN;
        }

        //SIGNED distance, zsurface - z point. If negative, the point is above the surface. 
        public float DistanceToStructuredSurface(Point3 p, StructuredSurface surface)
        {
            var s = surface;// as StructuredSurface;
            var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
            return i2 != null ? (float)(s.PositionAtIndex(i2).Z - p.Z) : float.NaN;
        }

    };

    public class SeismicTools
    {
        public static List<IndexDouble3> GetNonRepeatedTraceIndicesForPoints(SeismicCube cube, List<Point3> pts)
        {
            List<IndexDouble3> traces = new List<IndexDouble3>();
            Dictionary<IndexDouble3, Point3> pointsIndexMap = new Dictionary<IndexDouble3, Point3>();
            foreach (Point3 p in pts)
            {
                IndexDouble3 doubleIndex3 = cube.IndexAtPosition(p);
                //Index2 i2 = new Index2((int)doubleIndex3.I, (int)doubleIndex3.J);
                if (!pointsIndexMap.Keys.Contains(doubleIndex3))
                {
                    traces.Add(doubleIndex3);
                }

            }
            return traces;
        }
        public static List<IndexDouble3> GetTraceIndicesForPoints(SeismicCube cube, List<Point3> pts)
        {
            List<IndexDouble3> traces = new List<IndexDouble3>();
            Dictionary<IndexDouble3, Point3> pointsIndexMap = new Dictionary<IndexDouble3, Point3>();
            foreach (Point3 p in pts)
            {
                IndexDouble3 doubleIndex3 = cube.IndexAtPosition(p);
                traces.Add(doubleIndex3);

            }
            return traces;
        }

        public static Dictionary<Index2, float> CalculateDistanceToDatum(Object Datum, List<Index2> traces, SeismicCube cube)
        {
            Dictionary<Index2, float> distance = new Dictionary<Index2, float>();


            return distance;
        }

        public static float DistanceToRegularHeightFieldSurface(Point3 p, object surface)
        {
            var s = surface as RegularHeightFieldSurface;
            var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
            return i2 != null ? (float)(s[(int)i2.I, (int)i2.J] - p.Z) : float.NaN;
        }

        //SIGNED distance, zsurface - z point. If negative, the point is above the surface. 
        public static float DistanceToStructuredSurface(Point3 p, object surface)
        {
            var s = surface as StructuredSurface;
            var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
            return i2 != null ? (float)(s.PositionAtIndex(i2).Z - p.Z) : float.NaN;
        }
        public static float DistanceToSurface(Point3 p, object surface)
        {
            if (surface == null)
            {
                return (float)(-p.Z);
            }
            else if (surface is StructuredSurface) return SeismicTools.DistanceToStructuredSurface(p, surface);
            else return SeismicTools.DistanceToRegularHeightFieldSurface(p, surface);
        }

        public static bool WellPoroelasticStressFromSeismic(Object datum, bool offshore, float gapDensity, float seaWaterDensity, SeismicCube ym, SeismicCube pr, SeismicCube dens, SeismicCube press, List<Borehole> wells, float emin, float emax, float offset, string name = "PoroelasticIsotropic")
        {
            float offshoreFlag = (offshore == true ? 1.0f : 0.0f);
            float CellWidthZ = (float)Math.Abs(ym.PositionAtIndex(new IndexDouble3(0, 0, 0)).Z - ym.PositionAtIndex(new IndexDouble3(0, 0, 1)).Z);

            foreach (Borehole w in wells)
            {
                List<Point3> thisWellPoints = BoreholeTools.getTrajectoryPoints(w);
                Index2 oldTrace2D = null;
                Point3 topTracePos = null;

                float[] verticalStressAlongTrace = new float[ym.NumSamplesIJK.K];
                float[] traceDensityValues = new float[ym.NumSamplesIJK.K];
                float[] traceYMValues = new float[ym.NumSamplesIJK.K];
                float[] tracePRValues = new float[ym.NumSamplesIJK.K];
                float[] tracePPValues = new float[press.NumSamplesIJK.K];
                float[] traceBiotValues = Enumerable.Repeat(1.0f, ym.NumSamplesIJK.K).ToArray();
                float overburden = 0.0f;
                List<float> svLogValues = new List<float>(),
                            shminLogValues = new List<float>(),
                            shmaxLogValues = new List<float>(),
                            stressRegime = new List<float>();

                foreach (Point3 p in thisWellPoints)
                {
                    IndexDouble3 trace3D = ym.IndexAtPosition(p); // pointMapToTrace3D[p];
                    Index2 trace2D = new Index2((int)trace3D.I, (int)trace3D.J);

                    if (trace2D != oldTrace2D) //dont repeat the heavy calculations for vertical wells where all the points lie on the same trace. 
                    {
                        oldTrace2D = trace2D;
                        topTracePos = dens.PositionAtIndex(new IndexDouble3(trace2D.I, trace2D.J, 0));
                        //overburden
                        if (topTracePos.Z < 0.0f)
                        {

                            var distanceToDatum = SeismicTools.DistanceToStructuredSurface(topTracePos, datum);
                            overburden = distanceToDatum < 0.0f ? 0.0f : gapDensity * 10.0f * distanceToDatum;
                            //seaweight at datum depth 
                            var waterHeight = offshoreFlag * (p.Z + distanceToDatum);  //this is the sea level. pz is negative 
                            overburden += (waterHeight < 0.0f ? (float)(-1.0f * 10.0 * seaWaterDensity * waterHeight) : 0.0f);
                        }
                        else
                            overburden = 0.0f;

                        //this is the slow part. The next trace-getting lines
                        dens.GetTraceData(trace2D.I, trace2D.J, traceDensityValues);
                        ym.GetTraceData(trace2D.I, trace2D.J, traceYMValues);
                        pr.GetTraceData(trace2D.I, trace2D.J, tracePRValues);
                        press.GetTraceData(trace2D.I, trace2D.J, tracePPValues);

                        //integrate the density from 0 to NumSamples.K (all) and store the values along the trace in the vertical stress. 
                        verticalStressAlongTrace[0] = overburden;
                        for (int k = 1; k < ym.NumSamplesIJK.K; k++)
                            verticalStressAlongTrace[k] = verticalStressAlongTrace[k - 1] + (float)(10.0 * CellWidthZ * traceDensityValues[k - 1]);
                    }

                    //ISOTROPIC 
                    //we have the vertical stress all along the trace. We need Sv,Sh, SH at the point location. 
                    int kk = (int)(trace3D.K);
                    float sv = verticalStressAlongTrace[(int)(kk)];
                    var nu = tracePRValues[kk];
                    var a = traceYMValues[kk] / (1.0f - nu * nu);
                    var b = a * nu;
                    var lithostatic = (nu / (1.0f - nu)) * (sv - traceBiotValues[kk] * tracePPValues[kk]) + traceBiotValues[kk] * tracePPValues[kk];
                    float shmin = lithostatic + (10.0f * emin * a + 10.0f * emax * b);
                    float shmax = lithostatic + (10.0f * emin * b + 10.0f * emax * a);
                    //stress regime 

                    stressRegime.Add(sv >= shmax ? 1 : sv < shmin ? 3 : 2);
                    svLogValues.Add(sv);
                    shminLogValues.Add(shmin);
                    shmaxLogValues.Add(shmax);

                }// points 

                List<float> mds = w.Trajectory.TrajectoryPolylineRecords.Select(item => (float)(item.MD)).ToList();
                var t = PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal;
                var l1 = BoreholeTools.GetOrCreateLog("SVertical", w, t, name);
                var l2 = BoreholeTools.GetOrCreateLog("Shmin", w, t, name);
                var l3 = BoreholeTools.GetOrCreateLog("Shmax", w, t, name);
                BoreholeTools.ReplaceLog(w, l1, svLogValues, mds, false);
                BoreholeTools.ReplaceLog(w, l2, shminLogValues, mds, false);
                BoreholeTools.ReplaceLog(w, l3, shmaxLogValues, mds, false);

            } //borehole 


            return true;
        }





        //public static bool aWellPoroelasticStressFromSeismic(Object datum, bool offshore, float gapDensity, float seaWaterDensity, SeismicCube ym, SeismicCube pr, SeismicCube dens, SeismicCube press, List<Borehole> wells, float emin, float emax, float offset)
        //{
        //    //first lets get the overburden stress acting on the very top of the traces. 
        //    //lets do them all at once, so we need all the traces that are intersected by any of the wells.
        //    List<Point3> pts = new List<Point3>();
        //    List<IndexDouble3> traces = new List<IndexDouble3>();
        //    int counter = 0;

        //    foreach (Borehole w in wells)
        //    {
        //        List<Point3> thisWellPoints = BoreholeTools.getTrajectoryPoints(w);
        //        List<IndexDouble3> tracesThisWell = SeismicTools.GetTraceIndicesForPoints(ym, thisWellPoints);

        //        //the non-repeated points and traces are added to the lists  
        //        pts.AddRange(thisWellPoints.Where(t => !pts.Any(i => i == t)));
        //        traces.AddRange(tracesThisWell.Where(t => !traces.Any(i => i == t)));
        //    }

        //    //now for each trace we will calculate the overburden due to solid.water above the seismic to the datum 
        //    float[] overburden = new float[traces.Count];
        //    float offshoreFlag = (offshore == true ? 1.0f : 0.0f);

        //    counter = 0;
        //    foreach (IndexDouble3 trace in traces)
        //    {
        //        Point3 p = ym.PositionAtIndex(new IndexDouble3(trace.I, trace.J, 0));
        //        if (p.Z > 0.0f) continue;

        //        float distanceToDatum = SeismicTools.DistanceToStructuredSurface(p, datum);
        //        overburden[counter] = distanceToDatum < 0.0f ? 0.0f : gapDensity * 10.0f * distanceToDatum;

        //        //seaweight at datum depth 
        //        var waterHeight = offshoreFlag * (p.Z + distanceToDatum);  //this is the sea level. pz is negative 
        //        overburden[counter] += (waterHeight < 0.0f ? (float)(-1.0f * 10.0 * seaWaterDensity * waterHeight) : 0.0f);
        //        counter += 1;
        //    }

        //    //now for each trace we will compute the vertical stress...
        //    counter = 0;
        //    float[][] verticalStress = new float[traces.Count()][];
        //    float[] traceDensity = new float[dens.NumSamplesIJK.K];
        //    float CellWidthZ = (float)Math.Abs(ym.PositionAtIndex(new IndexDouble3(0, 0, 0)).Z - ym.PositionAtIndex(new IndexDouble3(0, 0, 1)).Z);
        //    foreach (IndexDouble3 trace in traces)
        //    {
        //        float topWeight = overburden[counter];
        //        verticalStress[counter] = new float[ym.NumSamplesIJK.K];

        //        verticalStress[counter][0] = topWeight;
        //        dens.GetTraceData((int)(trace.I), (int)(trace.J), traceDensity);
        //        for (int k = 1; k < ym.NumSamplesIJK.K; k++)
        //        {
        //            verticalStress[counter][k] = verticalStress[counter][k - 1] + (float)(10.0 * CellWidthZ * traceDensity[k - 1]);
        //        }

        //        counter += 1;
        //    }






        //    return true;
        //}

    };



};