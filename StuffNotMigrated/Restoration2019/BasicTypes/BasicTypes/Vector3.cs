using System;
using System.Collections.Generic;

namespace CommonData
{
    public class Vector3
    {
        public Vector3()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public Vector3( double x, double y, double z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3( double all )
        {
            X = all;
            Y = all;
            Z = all;
        }

        public static Vector3 operator +( Vector3 a, Vector3 b )
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -( Vector3 a, Vector3 b )
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static double operator *( Vector3 a, Vector3 b )
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        public static Vector3 operator *( float a, Vector3 b )
        {
            return new Vector3(a * b.X, a * b.Y, a * b.Z);
        }

        public static Vector3 operator *( double a, Vector3 b )
        {
            return new Vector3(a * b.X, a * b.Y, a * b.Z);
        }

        public Vector3( Vector3 A, Vector3 B )
        {
            X = B.X - A.X;
            Y = B.Y - A.Y;
            Z = B.Z - A.Z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public void normalize()
        {
            double l = Math.Sqrt(X * X + Y * Y + Z * Z);
            X /= l;
            Y /= l;
            Z /= l;
        }

        public double dotProduct( Vector3 B )
        {
            double dotscalar = X * B.X + Y * B.Y + Z * B.Z;
            return dotscalar;
        }

        public Vector3 Sub( Vector3 v1 )
        {
            return new Vector3(X - v1.X, Y - v1.Y, Z - v1.Z);
        }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vector3 crossProduct( Vector3 other )
        {
            return new Vector3(Y * other.Z - Z * other.Y, -(X * other.Z - Z * other.X), X * other.Y - Y * other.X);
        }

        public Vector3 averageVec( List<Vector3> vecList )
        {
            double meanX = 0;
            double meanY = 0;
            double meanZ = 0;
            double lenghtList = vecList.Count;

            foreach (var vector in vecList)
            {
                meanX += vector.X;
                meanY += vector.Y;
                meanZ += vector.Z;
            }

            Vector3 r = new Vector3(meanX, meanY, meanZ);
            r.normalize();
            return r;
        }

        public Vector3 averageHarmonicVec( List<Vector3> vecList )
        {
            double meanX = 0;
            double meanY = 0;
            double meanZ = 0;
            double lenghtList = vecList.Count;

            foreach (var vector in vecList)
            {
                meanX += 1 / vector.X;
                meanY += 1 / vector.Y;
                meanZ += 1 / vector.Z;
            }

            Vector3 r = new Vector3(lenghtList / meanX, lenghtList / meanY, lenghtList / meanZ);

            return r;
        }

        public double[] ToArray()
        {
            return new double[] { X, Y, Z };
        }
    }
}