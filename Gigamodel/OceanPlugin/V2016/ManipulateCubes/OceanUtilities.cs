using Gigamodel.GigaModelData;
using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{
    internal class OceanUtilities
    {
 
        private void getTraceDifference(int i, int j, float[] floatsSingleTrace, SeismicCube cube, float[] result)
        {
            cube.GetTraceData(i, j, result);
            for (int k = 0; k < result.Count(); k++) result[k] -= floatsSingleTrace[k];
        }

        public static SplitSeismicCube splitBinaryCubeOLD(SeismicCube cube)
        {
            //first, lets export the P0 cube as a binary. i,j,k and divide the writting process 
            float[] floatsSingleTrace = new float[cube.NumSamplesIJK.K];

            int I1 = (int)(cube.NumSamplesIJK.I / 4);  //  241/4   = 60
   
            SplitSeismicCube ret = new SplitSeismicCube();
            for (int chunk = 0; chunk < 4; chunk++)
            {
                int size = sizeof(float) * cube.NumSamplesIJK.K * cube.NumSamplesIJK.J * (chunk == 3 ? cube.NumSamplesIJK.I - 3 * I1 : I1);
                byte [] tempBuffer1 = new byte[size];

                //byte [] tempBuffer1 = chunk == 3 ? new byte[size2 * sizeof(float)] : new byte[size1 * sizeof(float)];
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

                ret.part[chunk] = tempBuffer1;

            }

            return ret;
        }

        public static SplitCubeFloats splitBinaryCube(SeismicCube cube)
        {
            //memory allocation
            SplitCubeFloats splitCube = new SplitCubeFloats( cube.NumSamplesIJK.I*cube.NumSamplesIJK.J, cube.NumSamplesIJK.K );

            //copy the data from seismic to cube. 
            var p1 = cube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
            var p2 = cube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));
            bool flipIJ = Math.Abs(p1.X - p2.X) < Math.Abs( p1.Y - p2.Y );   // if Dir I points towrds y 
            float[] floatsSingleTrace = new float[cube.NumSamplesIJK.K];

            int traceCounter = 0;
            if (flipIJ)                                                    
            {
                splitCube.Ordering = "KJI";                     //i varies fastest, the dir most aligned with K and then North 
                for (int j = 0; j < cube.NumSamplesIJK.J; j++)
                {
                    for (int i = 0; i < cube.NumSamplesIJK.I; i++)
                    {
                        cube.GetTraceData(i, j, floatsSingleTrace);
                        splitCube.SetTrace(traceCounter++, floatsSingleTrace);
                    }
                }
            }
            else
            {
                splitCube.Ordering = "KIJ";                    //j varies fastest, the dir most aligned with K and then North 
                for (int i = 0; i < cube.NumSamplesIJK.I; i++)
                {
                    for (int j = 0; j < cube.NumSamplesIJK.J; j++)
                    {
                        cube.GetTraceData(i, j, floatsSingleTrace);
                        splitCube.SetTrace(traceCounter++, floatsSingleTrace);

                    }
                }
            }

            return splitCube;
        }


        public static async void SaveToDrive(SplitSeismicCube cube, string file)
        {
            await Task.Run(() => slowWrite(cube, file));
            Console.WriteLine("Writting done");
        }

        public static void slowWrite(SplitSeismicCube cube, string file)
        {
            string file1 = file + "_PartI";
            using (BinaryWriter writter = new BinaryWriter(File.Open(file1, FileMode.Create)))
            {
                writter.Write(cube.part[0]);
            }

            string file2 = file + "_PartII";
            using (BinaryWriter writter = new BinaryWriter(File.Open(file2, FileMode.Create)))
            {
                writter.Write(cube.part[1]);
            }
        }

        public static async void SaveToDrive(byte[] bytes, string file)
        {
            await Task.Run(() => slowWrite(bytes, file));
            Console.WriteLine("Writting done");

        }

        public static async void SaveToDrive(float[] cube, string fileName)
        {
            await Task.Run(() => slowWriteFloats(cube, fileName));
            Console.WriteLine("Writting done");

        }

        public static void slowWriteFloats(float[] cube, string fileName)
        {
            var tempBuffer = new byte[cube.Count() * sizeof(float)];
            Buffer.BlockCopy(cube, 0, tempBuffer, 0, cube.Count() * sizeof(float));

            using (BinaryWriter writter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writter.Write(tempBuffer);
            }
        }

        public static void slowWrite(byte[] bytes, string file)
        {
            using (BinaryWriter writter = new BinaryWriter(File.Open(file, FileMode.Create)))
            {
                writter.Write(bytes);
            }
        }

        static public Gigamodel GetExistingOrCreateGigamodel()
        {
            //first, is there already a gigamodel ? 
            Gigamodel gigaModel = null;
            bool found = false;

            if (PetrelProject.IsPrimaryProjectOpen)
            {



                Project proj = PetrelProject.PrimaryProject;
                var extensions = proj.Extensions;
                foreach (object item in extensions)
                {
                    if (item is Gigamodel) { found = true; gigaModel = (Gigamodel)(item); }
                }

                if (!found)
                {
                    using (ITransaction txn = DataManager.NewTransaction())
                    {
                        txn.Lock(proj);
                        gigaModel = new Gigamodel();
                        proj.Extensions.Add(gigaModel);
                        //proj.ModelsExtensions.Add(new GigamodelDomainObject());
                        txn.Commit();
                    }
                }

                return gigaModel;
            }

            else
            {
                return null;
            }
        }

        public static void slowWriteBytes(byte[] cube, string file)
        {
            using (BinaryWriter writter = new BinaryWriter(File.Open(file, FileMode.Create)))
            {
                writter.Write(cube);
            }
        }

        public static bool saveCube(SplitSeismicCube splitcube, string fileName)
        {
            using (BinaryWriter writter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int n = 0; n < splitcube.part.Count(); n++)
                    writter.Write(splitcube.part[n]);
                writter.Close();
            }

            return true;
        }

   


        public static GridDimensions GridDimensionsFromSeismicCube(SeismicCube cube)
        {
            GridDimensions dims = new GridDimensions();

            Index3 numIJK = cube.NumSamplesIJK;// gigaModel.MaterialModels.GetOrCreateModel(simObject.MaterialModelName).YoungsModulus.NumSamplesIJK;

            //SEISMIC DIRECTION i 
            SeismicCube refCube = cube;
             
            Point3 po = refCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 0.0));
            Point3 p1 = refCube.PositionAtIndex(new IndexDouble3(1.0, 0.0, 0.0));

            var dx = Math.Abs(p1.X - po.X);
            var dy = Math.Abs(p1.Y - po.Y);


            double flipIJ = dy > dx ? 1.0 : 0.0; 
            double spacingI = Math.Sqrt( (p1.X - po.X)* (p1.X - po.X)+(p1.Y - po.Y)* (p1.Y - po.Y)+ (p1.Z - po.Z)*(p1.Z - po.Z) ) ;

            //SEISMIC DIRECTION j 
            p1 = refCube.PositionAtIndex(new IndexDouble3(0.0, 1.0, 0.0));
            double spacingJ = Math.Sqrt((p1.X - po.X) * (p1.X - po.X) + (p1.Y - po.Y) * (p1.Y - po.Y) + (p1.Z - po.Z) * (p1.Z - po.Z));


            //SEISMIC DIRECTION k 
            p1 = refCube.PositionAtIndex(new IndexDouble3(0.0, 0.0, 1.0));
            double spacingK = Math.Sqrt((p1.X - po.X) * (p1.X - po.X) + (p1.Y - po.Y) * (p1.Y - po.Y) + (p1.Z - po.Z) * (p1.Z - po.Z));


            dims.Spacing = new double[3] { spacingI* (1.0 - flipIJ) + spacingJ*flipIJ ,
                                           spacingJ* (1.0 - flipIJ) + spacingI*flipIJ ,
                                           spacingK };

            dims.Cells = new int[3] { flipIJ  > 0.0 ? numIJK.J : numIJK.I,
                                      flipIJ  > 0.0 ? numIJK.I : numIJK.J,
                                      numIJK.K };

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

    };


    internal class SplitSeismicCube
    {
        public byte[][] part;

        public int parts { get { return 4; } }

        public string Name { get; set; }

        public SplitSeismicCube()
        {
            part = new byte[4][];

        }

    };



 
    

    


}
