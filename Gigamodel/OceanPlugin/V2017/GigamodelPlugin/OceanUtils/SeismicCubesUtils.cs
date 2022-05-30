using Gigamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;

using Slb.Ocean.Petrel.DomainObject.Seismic;
using Gigamodel.Data;
using Gigamodel.GigaModelData;
using Gigamodel.VisageUtils;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Shapes;

namespace Gigamodel.OceanUtils
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
                if (double.IsNaN(values[i]))
                {
                    ;
                }
                else if (double.IsInfinity(values[i]))
                {
                    ;
                }
                else if (double.IsNaN(mds[i]))
                {
                    ;
                }
                else if (double.IsInfinity(mds[i]))
                {
                    ;
                }
                else
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

            try
            {
                for (var k = 0; k < valuesCorrected.Count - 1; k++)
                {
                    if (valuesCorrected[k + 1].MD < valuesCorrected[k].MD)
                    {
                        PetrelLogger.InfoOutputWindow("wrong k =" + k + " " + valuesCorrected[k].MD + " " + valuesCorrected[k + 1].MD);
                    }
                }
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


        public static void getTrajectoryPoints(Borehole w, List<Point3> pts, List<double> mds)
        {
            pts.Clear();// = new List<Point3>();
            mds.Clear();// = new List<double>();

            double minMd = w.MDRange.Min, maxMd = w.MDRange.Max;

            double md = minMd;
            while (md < maxMd)
            {
                pts.Add(getPositionAtWellMD(w, md));
                mds.Add(md);
                md += 1 * 0.30428;  //1 foot 
            }

        }



        public static List<Point3> getTrajectoryPoints(Borehole w)
        {
            List<Point3> pts = new List<Point3>();

            double minMd = w.MDRange.Min, maxMd = w.MDRange.Max;



            double md = minMd;
            while (md < maxMd)
            {
                pts.Add(getPositionAtWellMD(w, md));
                md += 0.30428;  //1 foot 
            }


            /*
            Trajectory trajectory = w.Trajectory;
            var records = w.Trajectory.TrajectoryPolylineRecords;
            foreach (TrajectoryPolylineRecord record in records)
            {
                Point3 point = getPositionAtWellMD(w, record.MD);
                //Point3 point2 = record.Point;
                pts.Add(point);
            }
            */


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

    internal class SeismicCubesUtilities
    {



        public static SplitCubeFloats splitBinaryCube(SeismicCube cube, float scalingFactor = 1.0f )
        {
            //memory allocation
            SplitCubeFloats splitCube = new SplitCubeFloats(cube.NumSamplesIJK.I * cube.NumSamplesIJK.J, cube.NumSamplesIJK.K);

            //copy the data from seismic to cube. 
            var p1 = cube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
            var p2 = cube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
            bool flipIJ = Math.Abs(p1.X - p2.X) < Math.Abs(p1.Y - p2.Y);   // if Dir I points towrds y 
            float[] floatsSingleTrace = new float[cube.NumSamplesIJK.K];

            flipIJ = true;
            int traceCounter = 0;


            if (flipIJ)
            {
                splitCube.Ordering = "KJI";                     //i varies fastest, the dir most aligned with K and then North 
                for (int i = 0; i < cube.NumSamplesIJK.I; i++)
                {

                    for (int j = 0; j < cube.NumSamplesIJK.J; j++)
                    {


                        cube.GetTraceData(i, j, floatsSingleTrace);

                        float[] scaled = floatsSingleTrace.Select(t => t * scalingFactor).ToArray();


                        splitCube.SetTrace(traceCounter++, scaled);// floatsSingleTrace);




                    }

                }

            }

            //else
            //{
            //    splitCube.Ordering = "KIJ";                    //j varies fastest, the dir most aligned with K and then North 
            //    for (int i = 0; i < cube.NumSamplesIJK.I; i++)
            //    {
            //        for (int j = 0; j < cube.NumSamplesIJK.J; j++)
            //        {
            //            cube.GetTraceData(i, j, floatsSingleTrace);
            //            splitCube.SetTrace(traceCounter++, floatsSingleTrace);

            //        }
            //    }
            //}

            return splitCube;
        }




        public static GridDimensions GridDimensionsFromSeismicCube(SeismicCube cube)
        {
            GridDimensions dims = new GridDimensions();

            Index3 numIJK = cube.NumSamplesIJK;// gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName).YoungsModulus.NumSamplesIJK;

            //SEISMIC DIRECTION i 
            SeismicCube refCube = cube;

            double sI = refCube.SampleSpacingIJK.X;
            double sJ = refCube.SampleSpacingIJK.Y;
            double sK = refCube.SampleSpacingIJK.Z;
            dims.Spacing = new double[3] { refCube.SampleSpacingIJK.X, refCube.SampleSpacingIJK.Y, refCube.SampleSpacingIJK.Z };

            Point3 po = refCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
            /*
            Point3 p1 = refCube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));


            var dx = Math.Abs(p1.X - po.X);
            var dy = Math.Abs(p1.Y - po.Y);


            double flipIJ = 0.0;// dy > dx ? 1.0 : 0.0;
            double spacingI = Math.Sqrt((p1.X - po.X) * (p1.X - po.X) + (p1.Y - po.Y) * (p1.Y - po.Y) + (p1.Z - po.Z) * (p1.Z - po.Z));

            //SEISMIC DIRECTION j 
            p1 = refCube.PositionAtIndex(new IndexDouble3(0.0, 1.0, 0.0));
            double spacingJ = Math.Sqrt((p1.X - po.X) * (p1.X - po.X) + (p1.Y - po.Y) * (p1.Y - po.Y) + (p1.Z - po.Z) * (p1.Z - po.Z));


            //SEISMIC DIRECTION k 
            p1 = refCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 1.0));
            double spacingK = Math.Sqrt((p1.X - po.X) * (p1.X - po.X) + (p1.Y - po.Y) * (p1.Y - po.Y) + (p1.Z - po.Z) * (p1.Z - po.Z));


            dims.Spacing = new double[3] { spacingI* (1.0 - flipIJ) + spacingJ*flipIJ ,
                                           spacingJ* (1.0 - flipIJ) + spacingI*flipIJ ,
                                           spacingK };
            */



            /*
            dims.Cells = new int[3] { flipIJ  > 0.0 ? numIJK.J : numIJK.I,
                                      flipIJ  > 0.0 ? numIJK.I : numIJK.J,
                                      numIJK.K };
            */
            dims.Cells = new int[3] { numIJK.I, numIJK.J, numIJK.K };

            dims.Size = new double[3] { dims.Spacing[0] * (dims.Cells[0]), dims.Spacing[1] * (dims.Cells[1]), dims.Spacing[2] * (dims.Cells[2]) };

            dims.Origin = new double[3] { po.X - 0.5 * dims.Spacing[0], po.Y - 0.5 * dims.Spacing[1], po.Z + dims.Spacing[2] * (po.Z < 0.0 ? +0.5 : -0.5) };


            /*
           double[] Spacing = new double[3] { Math.Abs(p1.X - po.X), Math.Abs(p1.Y - po.Y), Math.Abs(p1.Z - po.Z) };

            //clone 
            dims.Spacing = new double[3] {  Spacing[0], Spacing[1], Spacing[2]};

            dims.Cells = new int[3] { numIJK.I, numIJK.J, numIJK.K };
            
            dims.Size = new double[3] { Spacing[0] * (0 + numIJK.I), Spacing[1] * (0 + numIJK.J), Spacing[2] * (0 + numIJK.K), };

            dims.Origin = new double[3] { po.X - 0.5 * Spacing[0], po.Y - 0.5 * Spacing[1], po.Z + Spacing[2] * (po.Z < 0.0 ? +0.5 : -0.5) };

            p1 = cube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
            bool flipIJ = Math.Abs(p1.X - po.X) < Math.Abs(p1.Y - po.Y);   // if Dir I points towrds y 
            if (flipIJ)
            {
                var itemp = dims.Cells[0]; dims.Cells[0] = dims.Cells[1]; dims.Cells[1] = itemp;
                double temp = dims.Spacing[0]; dims.Spacing[0] = dims.Spacing[1]; dims.Spacing[1] = temp;
                       temp = dims.Size[0]; dims.Size[0] = dims.Size[1]; dims.Size[1] = temp;
                       temp = dims.Origin[0]; dims.Origin[0] = dims.Origin[1]; dims.Origin[1] = temp;
            }


            */
            return dims;
        }







        public static SplitSeismicCubeBytes SplitBinaryCube(SeismicCube cube)
        {
            //traces are loaded from Ocean API, one by one in this array
            float[] floatsSingleTrace = new float[cube.NumSamplesIJK.K];

            //the cubes are too big, we need to split them to overcome the 2GB array-size limit 
            int numberOfChunks = 4; //should be fine up to 8GB data = 2GB floats = 2GB seismic-cube size
            int I1 = (int)(cube.NumSamplesIJK.I / numberOfChunks);  //  241/4   = 60

            //this is an storage class for the byte data 
            SplitSeismicCubeBytes byteChunks = new SplitSeismicCubeBytes(numberOfChunks);

            //fill the byte-chunks one by one 
            for (int chunk = 0; chunk < numberOfChunks; chunk++)
            {
                int size = sizeof(float) * cube.NumSamplesIJK.K * cube.NumSamplesIJK.J * (chunk == 3 ? cube.NumSamplesIJK.I - 3 * I1 : I1);
                byte[] tempBuffer1 = new byte[size];

                int imax = chunk == 3 ? cube.NumSamplesIJK.I : chunk * I1 + I1;
                int offset = 0;

                for (int i = chunk * I1; i < imax; i++)//j
                {
                    for (int j = 0; j < cube.NumSamplesIJK.J; j++)//i [0,119]
                    {
                        cube.GetTraceData(i, j, floatsSingleTrace);
                        Buffer.BlockCopy(floatsSingleTrace, 0, tempBuffer1, offset * sizeof(float), cube.NumSamplesIJK.K * sizeof(float));
                        offset += cube.NumSamplesIJK.K;
                    }
                }

                //them copy then in the storage class 
                byteChunks[chunk] = tempBuffer1;
            }

            //return all the parts 
            return byteChunks;



        }

        public static SeismicCube CreateSeismicCubeFromReference(SeismicCube s, string name, Template t = null, SeismicCollection col = null)
        {
            SeismicCube retCube = null;

            if(col == null )col = s.SeismicCollection;
            if (col.CanCreateSeismicCube(s))
            {
                Template aux = (t == null ? s.Template : t);
                using (ITransaction txn = DataManager.NewTransaction())
                {
                    txn.Lock(col);
                    retCube = col.CreateSeismicCube(s, PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);//  aux);
                    txn.Commit();
                }

                using (ITransaction txn = DataManager.NewTransaction())
                {
                    txn.Lock(retCube);
                    retCube.Name = name;
                    txn.Commit();
                }

            }

            return retCube;
        }


        public static List<SeismicCube> XFileToSeismicCubes(SeismicCube refCube, List<KeywordDescription> propList, string eclipseFileName, List<Template> templates)
        {
            //from the xfile to here. Only one object and one allocation. 
            SplitCubeFloats data = new SplitCubeFloats(refCube.NumSamplesIJK.I * refCube.NumSamplesIJK.J, refCube.NumSamplesIJK.K);

            List<SeismicCube> cubes = new List<SeismicCube>();
            int counter = 0;



            var parentCollectionOfReferenceCube = refCube.SeismicCollection;
          
            int timeStep = int.Parse(System.Text.RegularExpressions.Regex.Replace(System.IO.Path.GetExtension(eclipseFileName), "[^0-9]", ""));
            var simulationName = System.IO.Path.GetFileNameWithoutExtension(eclipseFileName) + "["+timeStep+"]";

            SeismicCollection resultsCollection = SeismicCubesUtilities.GetOrCreateSeismicSubCollection(simulationName, parentCollectionOfReferenceCube);

            foreach (KeywordDescription propDes in propList)
            {
                SeismicCube c = SeismicCubesUtilities.CreateSeismicCubeFromReference(refCube, propDes.Name, templates[counter], resultsCollection);// PetrelProject.WellKnownTemplates.MiscellaneousGroup.General);
                // templates[counter]); //we will put the data here
                if (c != null)
                {
                    if (propDes.Size == c.NumSamplesIJK.I * c.NumSamplesIJK.J * c.NumSamplesIJK.K)
                    {
                        float dataScallingFactor = 1.0f;

                        //if the remplace is pressure, the eclipse files are in bar. We read bar, but ocean expects Pa so we scale here
                        if (templates[counter].TemplateType == PetrelProject.WellKnownTemplates.GeomechanicGroup.StressEffective.TemplateType)
                        {
                            dataScallingFactor = 100000.00f; //bar to Pa 
                        }
                        else if (templates[counter].TemplateType == PetrelProject.WellKnownTemplates.PetrophysicalGroup.Pressure.TemplateType)
                        {
                            dataScallingFactor = 100000.00f; //bar to Pa 
                        }

                        else if (templates[counter].TemplateType == PetrelProject.WellKnownTemplates.GeomechanicGroup.RockDisplacement.TemplateType)
                        {
                            dataScallingFactor = 1.00f; //m to m  
                        }
                        else
                        {
                            dataScallingFactor = 1.00f; //m to m  
                        }
                        EclipseReader.LoadFloatsAsSplitCube(propDes, eclipseFileName, ref data);

                        // for (int k = 0; k < data.NumberChunks; k++)
                        //{
                        //    float[] a1 = data[k];
                        //    float maxi = a1.Max();
                        //    float mini = a1.Min();
                        //}
                        float[] a1 = data[0];
                        float maxi = a1.Max();
                        float mini = a1.Min();


                        SeismicCubesUtilities.CopyToSeismicCube(data, dataScallingFactor, ref c);
                        cubes.Add(c);

                        for (int i = 0; i < data.NumberChunks; i++)
                        {
                            float [] values = data.GetPart(i);
                            var max = values.Max() ;
                            var min = values.Min();

                        }

                        ;

                    }
                }

                /* using (ITransaction txn = DataManager.NewTransaction())
                 {
                     txn.Lock(c);
                     c.Template = templates[counter];
                     counter += 1;
                     txn.Commit();
                 }*/

            }
            return cubes;
        }

        public static SeismicCollection GetOrCreateSeismicSubCollection(string name, SeismicCollection parent)
        {
            foreach (SeismicCollection c in parent.SeismicCollections)
                if (c.Name == name) return c;

            SeismicCollection col = null;
            using (ITransaction txn = DataManager.NewTransaction())
            {
                txn.Lock(parent);
                col = parent.CreateSeismicCollection(name);
                txn.Commit();
            }

            return col;
        }



        public static void CopyToSeismicCube(SplitCubeFloats data, float dataScallingFactor, ref SeismicCube c)
        {
            //both need to have the same size, number of traces and trace length. 
            float[] srcData = new float[data.TraceLength];

            float max = -999990.0f;
            float min = 9999990.0f;
            int minCount = 1000000;
            int maxCount = -100000;

            using (ITransaction txn = DataManager.NewTransaction())
            {
                txn.Lock(c);

                int count = 0;
                for (int i = 0; i < c.NumSamplesIJK.I; i++)
                {


                    for (int j = 0; j < c.NumSamplesIJK.J; j++)
                    {








                        data.GetTrace(count, ref srcData);
                        ITrace targetTrace = c.GetTrace(i, j);


                        if (srcData.Count() > maxCount) maxCount = srcData.Count();
                        if (srcData.Count() < minCount) minCount = srcData.Count();


                        for (int k = 0; k < srcData.Count(); k++)
                        {
                            targetTrace[k] = dataScallingFactor * srcData[k];
                            if (srcData[k] > max)
                            {
                                max = srcData[k];
                            }
                            if (srcData[k] < min)
                            {
                                min = srcData[k];
                            }
                        }
                        count += 1;
                    }
                }

                ;

                txn.Commit();
            }






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
            else if (surface is StructuredSurface) return SeismicCubesUtilities.DistanceToStructuredSurface(p, surface);
            else if (surface is RegularHeightFieldSurface) return SeismicCubesUtilities.DistanceToRegularHeightFieldSurface(p, surface);

            else return (float)-p.Z;
        }

        public static bool WellPoroelasticStressFromSeismic(Object datum, bool offshore, float gapDensity, float seaWaterDensity, SeismicCube ym, SeismicCube pr, SeismicCube dens, SeismicCube press, List<Borehole> wells, float emin, float emax, float offset, string name = "PoroelasticIsotropic")
        {
            float offshoreFlag = (offshore == true ? 1.0f : 0.0f);
            float CellWidthZ = (float)Math.Abs(ym.PositionAtIndex(new IndexDouble3(0, 0, 0)).Z - ym.PositionAtIndex(new IndexDouble3(0, 0, 1)).Z);


            float gravity = 10.0f;

            foreach (Borehole w in wells)
            {
                List<Point3> thisWellPoints = new List<Point3>();
                List<double> mds = new List<double>();
                BoreholeTools.getTrajectoryPoints(w, thisWellPoints, mds);

                Index2 oldTrace2D = null;
                Point3 topTracePos = null;

                string xx = "";

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
                int count = 0; 
                foreach (Point3 p in thisWellPoints)
                {
                    IndexDouble3 trace3D = ym.IndexAtPosition(p); // pointMapToTrace3D[p];
                    Index2 trace2D = new Index2((int)trace3D.I, (int)trace3D.J);


                    topTracePos = dens.PositionAtIndex(new IndexDouble3(trace3D.I, trace3D.J, 0.0 * trace3D.K));
                    //overburden
                    if (topTracePos.Z < 0.0f)
                    {

                        var distanceToDatum = SeismicCubesUtilities.DistanceToSurface(topTracePos, datum);
                        overburden = distanceToDatum < 0.0f ? 0.0f : gapDensity * gravity * distanceToDatum;
                        //seaweight at datum depth 
                        var waterHeight = offshoreFlag * (p.Z + distanceToDatum);  //this is the sea level. pz is negative 
                        overburden += (waterHeight < 0.0f ? (float)(-1.0f * 10.0 * seaWaterDensity * waterHeight) : 0.0f);
                    }
                    else
                        overburden = 0.0f;




                    //integrate the density from 0 to NumSamples.K (all) and store the values along the trace in the vertical stress. 
                    //verticalStressAlongTrace[0] = overburden + (float)(gravity * CellWidthZ * traceDensityValues[0]);                              //weight at the bottom of sample k = 0 ;

                    //for (int k = 1; k < ym.NumSamplesIJK.K; k++)
                    //verticalStressAlongTrace[k] = verticalStressAlongTrace[k - 1] + (float)(gravity * CellWidthZ * (traceDensityValues[k]));    //weight at the bottom of sample k = 1,2,.... ;


                    //ISOTROPIC 
                    //we have the vertical stress all along the trace. We need Sv,Sh, SH at the point location. 


                    if ((trace3D.K >= 0) && ( trace3D.I > 0) && (trace3D.J > 0) && (trace3D.I < ym.NumSamplesIJK.I) && (trace3D.J < ym.NumSamplesIJK.J) && (trace3D.K < ym.NumSamplesIJK.K))
                    {

                        //this is the slow part. The next trace-getting lines
                        dens.GetTraceData(trace2D.I, trace2D.J, traceDensityValues);
                        ym.GetTraceData(trace2D.I, trace2D.J, traceYMValues);
                        pr.GetTraceData(trace2D.I, trace2D.J, tracePRValues);
                        press.GetTraceData(trace2D.I, trace2D.J, tracePPValues);

                        float sv = 0.0f;
                        int kk = (int)(trace3D.K);

                       

                        float partOfCell = (float)(Math.Abs(trace3D.K - (int)(trace3D.K)));
                        float pp = tracePPValues[kk];// * partOfCell;


                        sv = overburden + (float)(gravity * CellWidthZ * partOfCell * traceDensityValues[kk]);
                        for (int kprime = 0; kprime <= kk - 1; kprime++)
                        {
                            sv += (float)(gravity * CellWidthZ * (traceDensityValues[kprime]));

                        }
                        

                        var nu = tracePRValues[kk];
                        var a = traceYMValues[kk] / (1.0f - nu * nu);
                        var b = a * nu;
                        var lithostatic = (nu / (1.0f - nu)) * (sv - traceBiotValues[kk] *pp) + traceBiotValues[kk] * pp;
                        float shmin = lithostatic + (1.0f * emin * a + 1.0f * emax * b);
                        float shmax = lithostatic + (1.0f * emin * b + 1.0f * emax * a);
                        //stress regime 

                        stressRegime.Add(sv >= shmax ? 1 : sv < shmin ? 3 : 2);
                        svLogValues.Add(sv);
                        shminLogValues.Add(shmin);
                        shmaxLogValues.Add(shmax);
                        

                    }
                    else
                    {
                        stressRegime.Add(float.NaN);
                        svLogValues.Add(float.NaN);
                        shminLogValues.Add(float.NaN);
                        shmaxLogValues.Add(float.NaN);
                    }
                    if (p == thisWellPoints[thisWellPoints.Count - 2])
                    {
                        ;
                    }

                    if (p == thisWellPoints[thisWellPoints.Count - 1])
                    {
                        ;
                    }

                    if (stressRegime.Count == 79)
                    {
                        ;
                    }

                }// points 

                //List<float> mds = w.Trajectory.TrajectoryPolylineRecords.Select(item => (float)(item.MD)).ToList();
                var t = PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal;
                var l1 = BoreholeTools.GetOrCreateLog("SVertical", w, t, name);
                var l2 = BoreholeTools.GetOrCreateLog("Shmin", w, t, name);
                var l3 = BoreholeTools.GetOrCreateLog("Shmax", w, t, name);

                t = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
                var l4 = BoreholeTools.GetOrCreateLog("ShRatio", w, t, name);
                var l5 = BoreholeTools.GetOrCreateLog("Regime", w, t, name);


                List<float> mdsFloats = mds.Select(x => (float)(x)).ToList();
                BoreholeTools.ReplaceLog(w, l1, svLogValues, mdsFloats, false);
                BoreholeTools.ReplaceLog(w, l2, shminLogValues, mdsFloats, false);
                BoreholeTools.ReplaceLog(w, l3, shmaxLogValues, mdsFloats, false);



                for (int i = 0; i < shmaxLogValues.Count; i++) shmaxLogValues[i] /= shminLogValues[i];
                BoreholeTools.ReplaceLog(w, l4, shmaxLogValues, mdsFloats, false);
                BoreholeTools.ReplaceLog(w, l5, stressRegime, mdsFloats, false);


            } //borehole 


            return true;
        }

    };






}
