﻿using FractureDrivers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractureDrivers.Controllers
{
    public class FracturePredictor
    {
        Random _rand;

        public FracturePredictor()
        {
            _rand = new Random();
        }



        static void GetEigenValues(double[] stress) //sxx. syx, szx,  sxy, syy, szy   szz   //as in petrel 
        {


        }

        /// <summary>
        /// \brief Computes the eigen values and eigen vectors* of a semi definite symmetric matrix
        /// \note IMPORANT: Eigen -values and -vectors are ordered from the highest to the lowest
        /// From Igeoss.
        /// Example:
        /// \code
        /// double matrix[6] ;
        /// double eigen_vectors[9] ;
        /// double eigen_values[3] ;
        /// MatrixUtils::symmetric_eigen(matrix, 3, eigen_vectors, eigen_values) ;
        /// Vector3d v1(eigen_vectors[0], eigen_vectors[1], eigen_vectors[2]) ;
        /// Vector3d v2(eigen_vectors[3], eigen_vectors[4], eigen_vectors[5]) ;
        /// Vector3d v3(eigen_vectors[6], eigen_vectors[7], eigen_vectors[8]) ;
        /// \endcode
        /// </summary>
        static /*EigenValuePairs */List<KeyValuePair<double, Vector3>>  EigenVectors(double[] a)
        {

            List<KeyValuePair<double, Vector3>> keyValuePairs = new List<KeyValuePair<double, Vector3>>();

            const double EPS = 0.00001;
            int MAX_ITER = 100;
            int n = 3;
            double[] v = new double[36];
            int[] index = new int[3];
            double[] eigen_val = new double[3];
            double[] eigen_vec = new double[9];
            double a_norm, a_normEPS, thr, thr_nn;
            int nb_iter = 0;
            int jj;
            int k, ik, lm, mq, lq, ll, mm, imv, im, iq, ilv, il, nn;
            double a_ij, a_lm, a_ll, a_mm, a_im, a_il;
            double a_lm_2;
            double v_ilv, v_imv;
            double x;
            double sinx, sinx_2, cosx, cosx_2, sincos;
            double delta;


            nn = (n * (n + 1)) / 2;


            //define indentity matrix 
            int ij = 0;
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == j)
                    {
                        v[ij++] = 1.0;
                    }
                    else
                    {
                        v[ij++] = 0.0;
                    }
                }
            }

            ij = 1;
            a_norm = 0.0;

            for (int i = 1; i <= n; ++i)
            {
                for (int j = 1; j <= i; ++j)
                {
                    if (i != j)
                    {
                        a_ij = a[ij - 1];
                        a_norm += a_ij * a_ij;
                    }
                    ++ij;
                }
            }

            if (a_norm != 0.0)
            {
                a_normEPS = a_norm * EPS;
                thr = a_norm;

                while (thr > a_normEPS && nb_iter < MAX_ITER)
                {
                    ++nb_iter;
                    thr_nn = thr / nn;

                    for (int l = 1; l < n; ++l)
                    {
                        for (int m = l + 1; m <= n; ++m)
                        {
                            lq = (l * l - l) / 2;
                            mq = (m * m - m) / 2;
                            lm = l + mq;
                            a_lm = a[lm - 1];
                            a_lm_2 = a_lm * a_lm;
                            if (a_lm_2 < thr_nn)
                                continue;

                            ll = l + lq;
                            mm = m + mq;
                            a_ll = a[ll - 1];
                            a_mm = a[mm - 1];
                            delta = a_ll - a_mm;

                            double xcpi = Math.Cos(Math.PI / 4);
                            double xspi = Math.Sin(Math.PI / 4);

                            if (delta == 0.0)
                            {
                                x = Math.PI / 4;
                                sinx = xspi;
                                cosx = xcpi;
                            }
                            else
                            {
                                x = -Math.Atan((a_lm + a_lm) / delta) / 2.0;
                                sinx = Math.Sin(x);
                                cosx = Math.Cos(x);
                            }

                            sinx_2 = sinx * sinx;
                            cosx_2 = cosx * cosx;
                            sincos = sinx * cosx;
                            ilv = n * (l - 1);
                            imv = n * (m - 1);

                            for (int i = 1; i <= n; ++i)
                            {
                                if ((i != l) && (i != m))
                                {
                                    iq = (i * i - i) / 2;

                                    if (i < m)
                                    {
                                        im = i + mq;
                                    }
                                    else
                                    {
                                        im = m + iq;
                                    }
                                    a_im = a[im - 1];

                                    if (i < l)
                                    {
                                        il = i + lq;
                                    }
                                    else
                                    {
                                        il = l + iq;
                                    }
                                    a_il = a[il - 1];
                                    a[il - 1] = a_il * cosx - a_im * sinx;
                                    a[im - 1] = a_il * sinx + a_im * cosx;
                                }

                                ++ilv;
                                ++imv;

                                v_ilv = v[ilv - 1];
                                v_imv = v[imv - 1];

                                v[ilv - 1] = cosx * v_ilv - sinx * v_imv;
                                v[imv - 1] = sinx * v_ilv + cosx * v_imv;
                            }

                            x = a_lm * sincos;
                            x += x;
                            a[ll - 1] = a_ll * cosx_2 + a_mm * sinx_2 - x;
                            a[mm - 1] = a_ll * sinx_2 + a_mm * cosx_2 + x;
                            a[lm - 1] = 0.0;
                            thr = Math.Abs(thr - a_lm_2);
                        }
                    }
                }
            }

            for (int i = 0; i < n; ++i)
            {
                k = i + (i * (i + 1)) / 2;
                eigen_val[i] = a[k];
                index[i] = i;
            }

            for (int i = 0; i < (n - 1); ++i)
            {
                x = eigen_val[i];
                k = i;
                for (int j = i + 1; j < n; ++j)
                {
                    if (x < eigen_val[j])
                    {
                        k = j;
                        x = eigen_val[j];
                    }
                }

                eigen_val[k] = eigen_val[i];
                eigen_val[i] = x;
                jj = index[k];
                index[k] = index[i];
                index[i] = jj;
            }

            ij = 0;

            for (k = 0; k < n; ++k)
            {
                ik = index[k] * n;
                for (int i = 0; i < n; ++i)
                {
                    eigen_vec[ij++] = v[ik++];
                }
            }


            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[0], new Vector3( eigen_vec[0], eigen_vec[1], eigen_vec[2])));
            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[1], new Vector3( eigen_vec[3], eigen_vec[4], eigen_vec[5])));
            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[2], new Vector3( eigen_vec[6], eigen_vec[7], eigen_vec[8])));

            return keyValuePairs;

            /*
            EigenValuePairs results = new EigenValuePairs();
            results.values = eigen_val;
            results.vectors = eigen_vec;

            return results;
            */
            //return new EigenResults(eigen_val, eigen_vec);
        }


        static List<KeyValuePair<double, Vector3>> EigenVectors(double sxx, double sxy, double syy, double sxz, double syz, double szz)
        {
            return EigenVectors(new double[] { sxx, sxy, syy, sxz, syz, szz });
        }

        static void getDipDipAzimuth(Vector3 normal, ref double dip, ref double dipAzimuth)
        {
            dip = 0.0;
            dipAzimuth = 0.0;

            FracturePredictor.getDipDipAzimuth(normal.ToArray(), ref dip, ref dipAzimuth);

        }



        static void getDipDipAzimuth(double[] normal, ref double dip, ref double dipAzimuth)
        {

            dip = 0.0;
            dipAzimuth = 0.0;

            double l;
            // normalize along normalx and normaly
            l = Math.Sqrt(normal[0] * normal[0] + normal[1] * normal[1]);

            if (l != 0) //FIX THIS 
            {
                double nx, ny;
                nx = normal[0] / l;
                ny = normal[1] / l;

                if (normal[2] < 0)
                {
                    nx = -nx;
                    ny = -ny;
                }

                dipAzimuth = Math.Asin(nx);

                if (ny < 0) { dipAzimuth = Math.PI - dipAzimuth; }
                if (dipAzimuth < 0) { dipAzimuth = Math.PI * 2 + dipAzimuth; }
                if (normal[2] >= 0)
                {
                    dip = Math.Acos(normal[2]);
                }
                else
                {
                    dip = Math.Acos(-normal[2]);
                }
            }
        }

        static  Orientation getDipDipAzimuthOrientation( Vector3 normal )
        {
            double dip = 0.0;
            double dipAzimuth = 0.0;

            double l;
            // normalize along normalx and normaly
            l = normal.Length;// Math.Sqrt(normal[0] * normal[0] + normal[1] * normal[1]);

            if (l != 0) //FIX THIS 
            {
                double nx, ny;
                nx = normal.X / l;
                ny = normal.Y / l;

                if (normal.Z < 0)
                {
                    nx = -nx;
                    ny = -ny;
                }

                dipAzimuth = Math.Asin(nx);

                if (ny < 0) { dipAzimuth = Math.PI - dipAzimuth; }
                if (dipAzimuth < 0) { dipAzimuth = Math.PI * 2 + dipAzimuth; }
                if (normal.Z >= 0)
                {
                    dip = Math.Acos(normal.Z);
                }
                else
                {
                    dip = Math.Acos(-normal.Z);
                }
            }

            return new Orientation( dip, dipAzimuth);
        }



        public static Matrix3x3 rotationShearFracture(double fangle)
        {
            return new Matrix3x3(new double[,]
                                                    {
                                                {   //Row 0
                                                    Math.Cos(fangle), 0, Math.Sin(fangle)
                                                },
                                                {
                                                    //Row 1
                                                    0, 1, 0
                                                },
                                                {
                                                    //Row 2
                                                    -Math.Sin(fangle), 0, Math.Cos(fangle)
                                                }
                                                    });
        }


         static double DegsToRadian(double degs)
        {
            return degs * 180 / Math.PI;
        }



        /// <returns></returns>
 

        public static Orientation getRandShearPlane(Orientation frac1, Orientation frac2)
        {


            Orientation randShearPlane;
            int numbRand = 0;// _rand.Next(0, 1);
            if (numbRand == 1)
            {
                randShearPlane = new Orientation(frac1.DipAzimuth, frac1.Dip);
            }
            else
            {

                randShearPlane = new Orientation(frac2.DipAzimuth, frac2.Dip);
            }

            return randShearPlane;

        }




        public static FractureModel PredictFractures(Dictionary<string, List<double>> data, double fangle = 30.0 )
        {
            Random rand = new Random();

            List<int> rands = new List<int>();
            for (int n = 0; n < 100; n++) rands.Add( rand.Next(0,1));



            List<double> sxx = data["eSxx"];
            List<double> syy = data["eSyy"];
            List<double> szz = data["eSzz"];

            List<double> syz = data["eSyz"];
            List<double> sxy = data["eSxy"];
            List<double> sxz = data["eSxz"];

            List<double> x = data["x"], y = data["y"], z = data["z"];

            List<Fracture> fracs = new List<Fracture>();
            for (int i = 0; i < sxx.Count(); i++)
            {

                List<KeyValuePair<double, Vector3>> result = FracturePredictor.EigenVectors(sxx[i], sxy[i], syy[i], sxz[i], syz[i], szz[i]);
                double[] values = result.Select(t => t.Key).ToArray();
                Vector3[] vectors = result.Select(t=>t.Value).ToArray(); //x1, y1 ,z1 .. x2,y2,z2 ..... 

                int valuesMinIndex = values.ToList().IndexOf( values.Min() ), valuesMaxIndex = values.ToList().IndexOf(values.Max());
                if (valuesMinIndex == valuesMaxIndex) { valuesMaxIndex = 0; valuesMinIndex = 1; }

                Vector3 minEigenVector = vectors[ valuesMinIndex ];// new double[] { vectors[3 * valuesMinIndex], vectors[3 * valuesMinIndex + 1], vectors[valuesMinIndex + 2] };
                Vector3 maxEigenVector = vectors[ valuesMaxIndex ];// new double[] { vectors[3 * valuesMaxIndex], vectors[3 * valuesMaxIndex + 1], vectors[valuesMaxIndex + 2] };

//this is a tensile joint -minimum = intensity 
                Fracture f1 = new Fracture();
                f1.Type = FractureType.Joint;
                f1.Orientation = getDipDipAzimuthOrientation(minEigenVector);//  new Orientation((float)minStressDip, (float)minStressDipAzimuth);
                f1.Intensity = values[valuesMinIndex];
                f1.Id = i;
                f1.Location = new Vector3(x[i], y[i], z[i]);

//this is a styolite maximum = intensity 
                Fracture f2 = new Fracture( f1 );
                f2.Type = FractureType.Styolite;
                f2.Orientation = getDipDipAzimuthOrientation(maxEigenVector);//  new Orientation((float)minStressDip, (float)minStressDipAzimuth);
                f2.Intensity = values[valuesMaxIndex];

                fracs.Add(f1);
                fracs.Add(f2);

// this is the shear 
                Matrix3x3 stressTensor = new Matrix3x3(new double[,]
                                        {   { vectors[0].X,vectors[1].X,vectors[2].X},
                                            { vectors[0].Y,vectors[1].Y,vectors[2].Y},
                                            { vectors[0].Z,vectors[1].Z,vectors[2].Z}
                                        });

                double sign = 1.0; 
                for (int nshear = 0; nshear <2; nshear++)
                {
                    Matrix3x3 rotateShearFracture = stressTensor * rotationShearFracture(DegsToRadian(sign*fangle));
                    Fracture f3 = new Fracture();
                    f3.Type = FractureType.Shear;
                    f3.Orientation = getDipDipAzimuthOrientation(new Vector3(rotateShearFracture[0, 2], rotateShearFracture[1, 2], rotateShearFracture[2, 2]) );
                    f3.Id = i;
                    f3.Location = new Vector3(x[i], y[i], z[i]);
                    fracs.Add(f3);
                    sign -= (2.0);
                }

                //now we pick one at random 
                int index = fracs.Count() - 1 - rand.Next(0, 1);
                fracs.Add( new Fracture( fracs[index] )) ;
               
            }//i

            FractureModel model = new FractureModel();
            model.Fractures = fracs;
            return model;
        }



    
        

        //public static void GetWellIntersectedFractures(string wellName, List<Vector3D> wellLocations, FractureModel fracs)
        //{

        //}


    }
}
