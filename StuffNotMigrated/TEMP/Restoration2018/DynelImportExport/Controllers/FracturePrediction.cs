using CommonData;
using Restoration.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restoration.Controllers
{
    public class FracturePredictor
    {
        public FracturePredictor()
        {
        }

        private static void GetEigenValues( double[] stress ) //sxx. syx, szx,  sxy, syy, szy   szz   //as in petrel
        {
        }

        public static List<KeyValuePair<double, Vector3>> EigenVectors( object p1, object p2, object p3, object p4, object p5, object p6 )
        {
            throw new NotImplementedException();
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
        private static List<KeyValuePair<double, Vector3>> EigenVectors( double[] a )
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

            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[0], new Vector3(eigen_vec[0], eigen_vec[1], eigen_vec[2])));
            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[1], new Vector3(eigen_vec[3], eigen_vec[4], eigen_vec[5])));
            keyValuePairs.Add(new KeyValuePair<double, Vector3>(eigen_val[2], new Vector3(eigen_vec[6], eigen_vec[7], eigen_vec[8])));

            return keyValuePairs;
        }

        private static List<KeyValuePair<double, Vector3>> EigenVectors( double sxx, double sxy, double syy, double sxz, double syz, double szz )
        {
            return EigenVectors(new double[] { sxx, sxy, syy, sxz, syz, szz });
        }

        private static Orientation getDipDipAzimuthOrientation( Vector3 normal )
        {
            double dip = 0.0;
            double dipAzimuth = 0.0;
            double deg = 180 / Math.PI;

            double l;
            // normalize along normalx and normaly
            l = Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y /*+ normal.Z*normal.Z*/);// Math.Sqrt(normal[0] * normal[0] + normal[1] * normal[1]);

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

                dipAzimuth = Math.Asin(nx) * deg;

                if (ny < 0) { dipAzimuth = 180 - dipAzimuth; }
                if (dipAzimuth < 0) { dipAzimuth = 180 * 2 + dipAzimuth; }
                if (normal.Z >= 0)
                {
                    dip = Math.Acos(normal.Z) * deg;
                }
                else
                {
                    dip = Math.Acos(-normal.Z) * deg;
                }
            }

            //convert to radians
            dip *= (Math.PI / 180.0);
            dipAzimuth *= (Math.PI / 180.0);

            return new Orientation(dip, dipAzimuth);
        }

        public static Matrix3x3 rotationShearFracture( double fangle )
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

        /// <returns></returns>

        public static Orientation getRandShearPlane( Orientation frac1, Orientation frac2 )
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

        public static void getNormalize( List<Fracture> fracModel, FractureType type )
        {
            var joints = fracModel.Where(t => t.Type == type);
            var jointsIntensity = joints.Select(t => t.Intensity);
            var jointMaxIntensity = jointsIntensity.Max();
            var jointsMinIntensity = jointsIntensity.Min();

            foreach (var j in joints) j.Intensity = 3.0;
        }

        public static FractureModel PredictFracturesFromStress( Dictionary<string, float[]> data, double fangle = 30.0, Vector3[] centers = null, bool filterOutlayers = true )
        {
            float[] sxx = data["TOTSTRXX"];

            if ((sxx == null) || (sxx.Count() < 1))
                return null;

            Vector3[] locations = centers == null ? new Vector3[sxx.Count()] : centers;

            float[] syy = data["TOTSTRYY"];
            float[] szz = data["TOTSTRZZ"];
            float[] syz = data["TOTSTRXY"];
            float[] sxy = data["TOTSTRYZ"];
            float[] sxz = data["TOTSTRZX"];

            float[] friction = null;
            if (data.Keys.Contains("FANG"))
                friction = data["FANG"];

            float[] plasticStrain = Enumerable.Repeat(float.NaN, sxx.Count()).ToArray();
            if (data.Keys.Contains("Equivalent Plastic Strain"))
                plasticStrain = data["Equivalent Plastic Strain"];

            Random rand = new Random();
            List<Fracture> fracs = new List<Fracture>();
            List<Fracture> joints = new List<Fracture>();
            List<Fracture> stylolite = new List<Fracture>();
            List<Fracture> shear = new List<Fracture>();

            int oksCounter = 0;

            for (int i = 0; i < sxx.Count(); i++)
            {
                bool areAllOk = !(float.IsNaN(sxx[i]) || float.IsNaN(syy[i]) || float.IsNaN(szz[i]) || float.IsNaN(sxy[i]) || float.IsNaN(syz[i]) || float.IsNaN(sxz[i]));

                if (areAllOk)
                {
                    oksCounter += 1;
                    float ssxx = sxx[i], ssxy = sxy[i], ssyy = syy[i], ssxz = sxz[i], ssyz = syz[i], sszz = szz[i];

                    List<KeyValuePair<double, Vector3>> result = FracturePredictor.EigenVectors(ssxx, ssxy, ssyy, ssxz, ssyz, sszz);
                    List<double> values = result.Select(t => t.Key).ToList();
                    Vector3[] vectors = result.Select(t => t.Value).ToArray(); //x1, y1 ,z1 .. x2,y2,z2 .....

                    Vector3 center = centers[i];

                    int valuesMinIndex = values.IndexOf(values.Min()), valuesMaxIndex = values.IndexOf(values.Max());
                    if (valuesMinIndex == valuesMaxIndex) { valuesMaxIndex = 0; valuesMinIndex = 1; }

                    Vector3 minEigenVector = vectors[valuesMinIndex];//
                    Vector3 maxEigenVector = vectors[valuesMaxIndex];//

                    //this is a tensile joint -minimum = intensity
                    Fracture f1 = new Fracture();
                    f1.Type = FractureType.Joint;
                    f1.Orientation = getDipDipAzimuthOrientation(minEigenVector);//  new Orientation((float)minStressDip, (float)minStressDipAzimuth);
                    f1.Intensity = -1.0 * values[valuesMinIndex];
                    f1.Id = i;
                    f1.Location = locations[i];
                    f1.EquivalentPlasticStrain = plasticStrain[i];
                    joints.Add(f1);

                    //this is a styolite maximum = intensity
                    Fracture f2 = new Fracture(f1);
                    f2.Type = FractureType.Styolite;
                    f2.Orientation = getDipDipAzimuthOrientation(maxEigenVector);//  new Orientation((float)minStressDip, (float)minStressDipAzimuth);
                    f2.Intensity = values[valuesMaxIndex];
                    f2.Location = locations[i];
                    f1.EquivalentPlasticStrain = plasticStrain[i];
                    stylolite.Add(f2);

                    // this is the shear
                    Matrix3x3 stressTensor = new Matrix3x3(new double[,]
                                            {   { vectors[0].X,vectors[1].X,vectors[2].X},
                                            { vectors[0].Y,vectors[1].Y,vectors[2].Y},
                                            { vectors[0].Z,vectors[1].Z,vectors[2].Z}
                                            });

                    double sign = 1.0;
                    FractureType[] shearTypes = new FractureType[3] { FractureType.Shear1, FractureType.Shear2, FractureType.Shear3 };

                    for (int nshear = 0; nshear < 2; nshear++)
                    {
                        double fangleRad = (friction != null) ? friction[i] : fangle * Math.PI / 180;

                        double coefFang = Math.Tan(fangleRad);
                        Matrix3x3 rotateShearFracture = stressTensor * rotationShearFracture((sign * fangleRad));

                        Fracture f3 = new Fracture();
                        f3.Type = shearTypes[nshear];
                        f3.Orientation = getDipDipAzimuthOrientation(new Vector3(rotateShearFracture[0, 2], rotateShearFracture[1, 2], rotateShearFracture[2, 2]));
                        f3.Id = i;
                        // Maximum Coulomb shear stress = ((s1-s3/2)*sqrt(1*coeffric^2)-coeffric(s1+s3/2))
                        f3.Intensity = ((values[valuesMaxIndex] - values[valuesMinIndex]) / 2 * Math.Sqrt(1 + coefFang * coefFang)) - ((values[valuesMaxIndex] + values[valuesMinIndex]) / 2);
                        //fracs.Add(f3);

                        f3.Location = locations[i];
                        shear.Add(f3);
                        f3.EquivalentPlasticStrain = plasticStrain[i];
                        sign -= (2.0);
                    }

                    //now we pick one at random
                    int index = shear.Count() - 1 - rand.Next(0, 2);
                    Fracture randomShear = new Fracture(shear[index]);
                    randomShear.Type = FractureType.Shear3;
                    randomShear.EquivalentPlasticStrain = plasticStrain[i];
                    shear.Add(randomShear);// new Fracture(shear[index]));
                }//areok
            }//i

            FractureModel model = new FractureModel();
            model.Fractures.AddRange(normalizedIntensity(joints, filterOutlayers));
            model.Fractures.AddRange(normalizedIntensity(stylolite, filterOutlayers));
            model.Fractures.AddRange(normalizedIntensity(shear, filterOutlayers));

            var shearFracs = model.Fractures.Where(t => t.Type == FractureType.Shear);

            return model;
        }

        private static List<Fracture> Translate( Vector3 dir, List<Fracture> fracs )
        {
            //return null;

            return fracs.Select(t =>
            {
                t.Location = t.Location - dir;
                return t;
            }).ToList();
        }

        private static List<Fracture> normalizedIntensity( List<Fracture> fracs, bool filterOutlayers = true )
        {
            var intensities = fracs.Select(t => t.Intensity).OrderBy(x => x).ToList();
            var max = intensities.Max();
            var min = intensities.Min();

            //bool isEven = Math.Abs(1.0f * intensities.Count() % 2.0f) < 0.0000001f;
            //var median = isEven == true ? 0.5 * (intensities[intensities.Count()/2 - 1] + intensities[ (intensities.Count() + 2/ 2) - 1]) : intensities[ intensities.Count() / 2];
            var q1 = intensities[(int)(intensities.Count() * 0.25) - 1];
            var q3 = intensities[(int)(intensities.Count() * 0.75) - 1];
            var IQR = q3 - q1;
            var minOutlier = q1 - 1.5 * IQR;
            var maxOutlier = q3 + 1.5 * IQR;

            List<Fracture> filteredOutliers = fracs;

            if (filterOutlayers)
            {
                filteredOutliers = fracs.Where(t => t.Intensity >= minOutlier && t.Intensity <= maxOutlier).ToList();
                max = filteredOutliers.Select(t => t.Intensity).Max();
                min = filteredOutliers.Select(t => t.Intensity).Min();
            }

            var N = 1.0 / (max - min);
            List<Fracture> xxx = filteredOutliers.Select(t =>
              {
                  t.Intensity = (t.Intensity - min) * N;

                  return t;
              }).ToList();

            return xxx;
        }
    };
}//bnamespace
