using CommonData;
using System;

namespace Restoration.Model
{
    public class Orientation
    {
        public Orientation( double idip = 0.0, double idipAzimuth = 0.0 )
        {
            Dip = idip;
            DipAzimuth = idipAzimuth;
        }

        public double Dip { get; set; }

        public double DipAzimuth { get; set; }

        public Vector3 Normal
        {
            get
            {
                double delta = Dip;
                double alpha = DipAzimuth;

                double nx = Math.Sin(delta) * Math.Sin(alpha);
                double ny = Math.Sin(delta) * Math.Cos(alpha);
                double nz = Math.Cos(delta);

                //double nz = Math.Cos(Dip);
                //double ny = Math.Cos(DipAzimuth);
                //double nx = Math.Sin(DipAzimuth);
                alpha = Math.Sqrt(nx * nx + ny * ny + nz * nz);
                return new Vector3(nx / alpha, ny / alpha, nz / alpha);
            }
        }
    };
}