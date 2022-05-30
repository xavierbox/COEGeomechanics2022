using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Basics;
using System.Linq;
using Slb.Ocean.Petrel;
using System.Collections.Generic;

namespace Gigamodel
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class EnhanceSeismicUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private EnhanceSeismic process;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhanceSeismicUI"/> class.
        /// </summary>
        public EnhanceSeismicUI(EnhanceSeismic process)
        {
            InitializeComponent();

            this.process = process;
        }

        private void seismicDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            SeismicCube s = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;

            seismicPresentationBox.Tag = null;
            if (s == null) return;


            seismicPresentationBox.Tag = s;
            seismicPresentationBox.Text = s.Name;
            seismicPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.Seismic3D;




        }

        private void EnhanceSubCube(SeismicCube original, SeismicCube s, int blockSize, Index3 imin, Index3 imax, float enhanceFactor)
        {
            if (s == null) return;
            if (original == null) return;

            float[,,] values = null;
            double avg = 0.0f;
            long count = 0;

            double weight, weightSum = 0.0;
            //SeismicCube saltConcentration = CreateSeismicCubeFromReference(original, original.Name + "_SaltConcentration");


            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(original);
                ISubCube subCube = original.GetSubCubeReadOnly(imin, imax-1);

                values = subCube.ToArray();
                avg = 0.0;
                weightSum = 0.0;
                count = 0;
                double maxVal = float.MinValue;
                double minVal = float.MaxValue;
                double test = 0.0f; 
                foreach (float v in values)
                {
                    if ((!float.IsNaN(v)) && (v > 0.00015) && (v < 0.456f))
                    {


                        minVal = Math.Min(minVal, v);
                        maxVal = Math.Max(maxVal, v);


                        //weight = (float)(0.5 * (1 - Math.Tanh(2.0 * (v - 8.5))));
                        weight = 1.0;// (float)(0.5  * (1  + (Math.Tanh(15  * (v - 0.15 )))));
                        //weight =  Math.Min( 1.0f, (float)(0.5 * (1 + (Math.Tanh(12 * (v - 0.22))))));

                          test = avg / weightSum; 
                        avg += (v*weight);
                        count += 1;
                        weightSum = weightSum + weight;
                    }
                }
                ;
                ;
                avg /= weightSum;// count;
                //avg *= weightSum;

                //avg /= count;

                trans.Commit();
            }

            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(s);
                //trans.Lock(saltConcentration);
                ;
                ISubCube subCube1 = original.GetSubCubeReadOnly(imin, imax-1);
                ISubCube subCube2 = s.GetSubCubeReadWrite(imin, imax-1);
                //ISubCube saltCube = saltConcentration.GetSubCubeReadWrite( new Index3(0,0,0), saltConcentration.NumSamplesIJK - 1 );

                //now enhance 
                for (int ii = subCube1.MinIJK.I; ii <= subCube1.MaxIJK.I; ii++)
                    for (int jj = subCube1.MinIJK.J; jj <= subCube1.MaxIJK.J; jj++)
                        for (int kk = subCube1.MinIJK.K; kk <= subCube1.MaxIJK.K; kk++)
                        {
                            Index3 i3 = new Index3(ii, jj, kk);
                            float value = subCube1[i3];
                            double delta = value - avg;
                            if ((!float.IsNaN(value)) && (value > 0.0000015) && (value < 0.456f))
                            {
                                //weight = (float)(0.5 * (1 - Math.Tanh(2.0 * (value - 8.5))));
                                weight = 1;// (float)(0.5 * (1 + (Math.Tanh(15 * (value - 0.15)))));
                                //weight =  (float)( 0.5 * (1 + (Math.Tanh(12 * (value - 0.22)))) );

                                double newValue = /*3.0f + */value + weight*enhanceFactor * (delta);
                                subCube2[i3] = (float)newValue;

                                //float concentration = -(weight - 1 );

                                //saltCube[i3] = concentration;
                            }
                            else
                                subCube2[i3] =  (float)avg;
                        }

                trans.Commit();
            }

        }

        public SeismicCube CreateSeismicCubeFromReference(SeismicCube s, string name, SeismicCollection col = null)
        {
            SeismicCube retCube = null;

            if (col == null) col = s.SeismicCollection;
            if (col.CanCreateSeismicCube(s))
            {

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


        private void RemoveSaltArtifacts(SeismicCube original, SeismicCube s, Index3 imin, Index3 imax, float threshold = 2.1f)
        {
            //the idea is that if in a little subcube there ia a nan or a value smaller than a threshold (salt) then all the little subcube will be set to NAN
            if (s == null) return;
            if (original == null) return;

            float[,,] values = null;
            bool setToSalt = false;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(original);
                ISubCube subCube = original.GetSubCubeReadOnly(imin, imax);

                values = subCube.ToArray();

                foreach (int v in values)
                {
                    if ((float.IsNaN(v)) || (v < threshold))
                    {
                        setToSalt = true;
                        break;
                    }
                }
                trans.Commit();
            }


            if (setToSalt == true)
            {

                using (ITransaction trans = DataManager.NewTransaction())
                {
                    trans.Lock(s);
                    ISubCube subCube1 = s.GetSubCubeReadWrite(imin, imax);

                    //now enhance 
                    for (int ii = subCube1.MinIJK.I; ii < subCube1.MaxIJK.I; ii++)
                        for (int jj = subCube1.MinIJK.J; jj < subCube1.MaxIJK.J; jj++)
                            for (int kk = subCube1.MinIJK.K; kk < subCube1.MaxIJK.K; kk++)
                            {
                                Index3 i3 = new Index3(ii, jj, kk);
                                subCube1[i3] = float.NaN;
                            }

                    trans.Commit();
                }

            }


        }


        List<Tuple<Index3, Index3>> GetSubcubeIndices(SeismicCube s, int[] blockSize)
        {
            List<Tuple<Index3, Index3>> l = null;
            using (ITransaction trans = DataManager.NewTransaction())
            {

                trans.Lock(s);
                ISubCube subCube = s.GetSubCubeReadOnly(new Index3(0, 0, 0), new Index3(s.NumSamplesIJK - new Index3(1, 1, 1)));
                l = GetSubcubeIndices(subCube, blockSize);

                trans.Commit();
            }

            return l;
        }

        List<Tuple<Index3, Index3>> GetSubcubeIndices(ISubCube s, int[] blockSize)
        {
            List<Tuple<Index3, Index3>> indices = new List<Tuple<Index3, Index3>>();
            Index3 numSamples = s.MaxIJK - s.MinIJK;

            for (int i = s.MinIJK.I; i < 1 + numSamples.I / blockSize[0]; i++)
                for (int j = s.MinIJK.J; j < 1 + numSamples.J / blockSize[1]; j++)
                    for (int k = s.MinIJK.K; k < 1 + numSamples.K / blockSize[2]; k++)
                    {
                        Index3 imin = new Index3(Math.Min(i * blockSize[0], numSamples.I - 1),
                                            Math.Min(j * blockSize[1], numSamples.J - 1), Math.Min(k * blockSize[2], numSamples.K - 1));

                        Index3 imax = new Index3(Math.Min(imin.I + blockSize[0], numSamples.I - 1),
                        Math.Min(imin.J + blockSize[1], numSamples.J - 1), Math.Min(imin.K + blockSize[2], numSamples.K - 1));

                        bool isOk = imax.I > imin.I && imax.J > imin.J && imax.K > imin.K && (imax.I < numSamples.I && imax.J < numSamples.J && imax.K < numSamples.K);

                        if (isOk)
                        {
                            indices.Add(new Tuple<Index3, Index3>(imin, imax));
                        }
                    }

            return indices;
        }

        private List<Index3> FlagSalt(SeismicCube original, float threshold1 = 2.1f, float threshold2 = 7.7f)
        {
            List<Index3> saltIndices = new List<Index3>();

            if (original == null) return saltIndices;

            int[] blockSize = new int[] { 100, 100, 500 };
            List<Tuple<Index3, Index3>> allBigCudes = GetSubcubeIndices(original, blockSize);

            int counter = 0;

            //create a salt-flag cube 
            SeismicCube saltCube = CreateSeismicCubeFromReference(original, original.Name + "_salt");

            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(original);
                trans.Lock(saltCube);

                ISubCube saltSubCube = saltCube.GetSubCubeReadWrite(new Index3(0, 0, 0), new Index3(saltCube.NumSamplesIJK - new Index3(1, 1, 1)));
                foreach (var cube in allBigCudes)
                {
                    ISubCube subCube = original.GetSubCubeReadOnly(cube.Item1, cube.Item2);
                    float[,,] values = subCube.ToArray();
                    bool hasSalt = false;
                    foreach (var value in values)
                    {
                        if ((float.IsNaN(value)) || (value < 2.1f))
                        {
                            hasSalt = true;
                            break;
                        }
                    }

                    if (hasSalt)
                    {
                        for (int i = subCube.MinIJK.I; i < subCube.MaxIJK.I; i++)
                            for (int j = subCube.MinIJK.J; j < subCube.MaxIJK.J; j++)
                                for (int k = subCube.MinIJK.K; k < subCube.MaxIJK.K; k++)
                                {
                                    float value = subCube[new Index3(i, j, k)];
                                    if (float.IsNaN(value) || value < threshold1)
                                    {
                                        //saltIndices.Add(new Index3(i, j, k));
                                        saltSubCube[new Index3(i, j, k)] = float.NaN;
                                    }
                                    else if (value > threshold2)
                                    {
                                        saltIndices.Add(new Index3(i, j, k));
                                        //saltSubCube[new Index3(i, j, k)] = 999.99f;
                                    }
                                    else {; }
                                }
                    }


                    counter += 1;
                }
                trans.Commit();


            }







            return saltIndices;
        }





        private void button1_Click(object sender, EventArgs e)
        {

            int[] blockSize = new int[] { 650, 650, 10 };

            SeismicCube original = seismicPresentationBox.Tag as SeismicCube;
            if (original == null) return;

            SeismicCube s = CreateSeismicCubeFromReference(original, original.Name + "_enhanced");

            Index3 numSamples = s.NumSamplesIJK;

            List<Tuple<Index3, Index3>> tuples = new List<Tuple<Index3, Index3>>();
            float enhanceFactor = (float)this.enhanceFactorControl.Value;

            int maxIndexI  = 1 + numSamples.I / blockSize[0];
            int maxIndexJ = 1 + numSamples.J / blockSize[1];
            int maxIndexK = 1 + numSamples.K / blockSize[2];
   

            for (int i = 0; i < maxIndexI; i++)
                for (int j = 0; j < maxIndexJ; j++)
                    for (int k = 0; k < maxIndexK; k++)
                    {
                        Index3 imin = new Index3(Math.Min(i * blockSize[0], numSamples.I),
                        Math.Min(j * blockSize[1], numSamples.J ), Math.Min(k * blockSize[2], numSamples.K));

                        Index3 imax = new Index3(Math.Min(imin.I + blockSize[0], numSamples.I ),
                        Math.Min(imin.J + blockSize[1], numSamples.J), Math.Min(imin.K + blockSize[2], numSamples.K ));

                        bool isOk = imax.I > imin.I && imax.J > imin.J && imax.K > imin.K && (imax.I <= numSamples.I && imax.J <= numSamples.J && imax.K <= numSamples.K);

                        if (isOk)
                        {
                            tuples.Add( new Tuple<Index3, Index3>( imin,imax) );

                            EnhanceSubCube(original, s, 9999999, imin, imax, enhanceFactor);
                            //Console.WriteLine(imin + " " + imax);
                            //RemoveSaltArtifacts(original, s, imin, imax, 2.1f);

                        }


                    };

            ;


        }


        private void button2_Click(object sender, EventArgs e)
        {
            SeismicCube original = seismicPresentationBox.Tag as SeismicCube;
            if (original == null) return;

            //step 1:
            //get all the indices that are Nan or smaller than a threshold
            List<Index3> saltIndices = FlagSalt(original, 2.1f, 7.7f);

            //in the list we have the high valued that were found in big blocks where salt was also found. 


            ////create a salt-flag cube 
            //SeismicCube saltCube = CreateSeismicCubeFromReference(original, original.Name + "_salt");
            //using (ITransaction trans = DataManager.NewTransaction())
            //{
            //    trans.Lock(saltCube);
            //    ISubCube subCube = saltCube.GetSubCubeReadWrite(new Index3(0, 0, 0), new Index3(saltCube.NumSamplesIJK - new Index3(1, 1, 1)));
            //    int counter = 0;
            //    foreach (Index3 i3 in saltIndices)
            //    {
            //        subCube[i3] = float.NaN;
            //        counter += 1;
            //    }

            //    trans.Commit();
            //}

            //float threshold2 = 7.0f;

            ////now we scan all the original cube and search for the high values. Those next to a salt, become salt too. 
            //using (ITransaction trans = DataManager.NewTransaction())
            //{
            //    trans.Lock(saltCube);
            //    ISubCube originalCube = original.GetSubCubeReadOnly(new Index3(0, 0, 0), new Index3(original.NumSamplesIJK - new Index3(1, 1, 1)));
            //    ISubCube subCube = saltCube.GetSubCubeReadWrite(new Index3(0, 0, 0), new Index3(saltCube.NumSamplesIJK - new Index3(1, 1, 1)));

            //    int counter = 0;
            //    foreach (Index3 i3 in saltIndices)
            //    {
            //        subCube[i3] = float.NaN;
            //        counter += 1;
            //    }

            //    trans.Commit();
            //}



            /*
                                 int i = i3.I, j = i3.J, k = i3.K;


                    ////now, get a small sub-volume around this cell and flag also as salt everything above a theshold
                    int imin = Math.Max(subCube.MinIJK.I, i - 10), imax = Math.Min(i + 10, subCube.MaxIJK.I - 1);
                    int jmin = Math.Max(subCube.MinIJK.J, j - 10), jmax = Math.Min(j + 10, subCube.MaxIJK.J - 1);
                    int kmin = Math.Max(subCube.MinIJK.K, k - 50), kmax = Math.Min(k + 50, subCube.MaxIJK.K - 1);

                    for (int ii = imin; ii <= imax; ii++)
                        for (int jj = jmin; jj <= jmax; jj++)
                            for (int kk = kmin; kk <= kmax; kk++)
                            {
                                Index3 index = new Index3(ii, jj, kk);
                                float value2 = originalCube[new Index3(ii, jj, kk)];
                                if ((value2 > threshold2) && (value2 != float.NaN) )
                                    subCube[index ]= float.NaN;
                            }

             */

            return;



            /*

            int[] blockSize = new int[] { 100, 100, 500 };



            SeismicCube s = CreateSeismicCubeFromReference(original, original.Name + "_copy");
            Index3 numSamples = s.NumSamplesIJK;

            float enhanceFactor = (float)this.enhanceFactorControl.Value;

            for (int i = 0; i < 1 + numSamples.I / blockSize[0]; i++)
                for (int j = 0; j < 1 + numSamples.J / blockSize[1]; j++)
                    for (int k = 0; k < 1 + numSamples.K / blockSize[2]; k++)
                    {
                        Index3 imin = new Index3(Math.Min(i * blockSize[0], numSamples.I - 1),
                        Math.Min(j * blockSize[1], numSamples.J - 1), Math.Min(k * blockSize[2], numSamples.K - 1));

                        Index3 imax = new Index3(Math.Min(imin.I + blockSize[0], numSamples.I - 1),
                        Math.Min(imin.J + blockSize[1], numSamples.J - 1), Math.Min(imin.K + blockSize[2], numSamples.K - 1));

                        bool isOk = imax.I > imin.I && imax.J > imin.J && imax.K > imin.K && (imax.I < numSamples.I && imax.J < numSamples.J && imax.K < numSamples.K);

                        if (isOk)
                        {
                            //EnhanceSubCube(original, s, blockSize, imin, imax, enhanceFactor);

                            RemoveSaltArtifacts(original, s, imin, imax, 2.1f);

                        }



                    }

            */
        }

    }
}
