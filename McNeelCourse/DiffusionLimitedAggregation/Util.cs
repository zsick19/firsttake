using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace McNeelCourse.DiffusionLimitedAggregation
{
    public static class Util
    {
        private static Random random = new Random();

        public static Vector3d RandomUnitVector()
        {
            lock (random)
            {
                double theta = 2.0 * Math.PI * random.NextDouble();
                double phi = Math.Acos(2.0 * random.NextDouble() - 1.0);
                return new Vector3d(Math.Sin(phi) * Math.Cos(theta), Math.Sin(phi) * Math.Sin(theta), Math.Cos(phi));
            }   
        }


        public static Vector3d RandomUnitVectorUpperHemisphere()
        {
            lock (random)
            {
                double theta = 2.0 * Math.PI * random.NextDouble();
                double phi = Math.Acos(2.0 * random.NextDouble() - 1.0);
                return new Vector3d(Math.Sin(phi) * Math.Cos(theta), Math.Sin(phi) * Math.Sin(theta),
                    Math.Abs(Math.Cos(phi)));
            }
        }


        public static Vector3d RandomUnitVectorCosineDistribution()
        {
            double temp = random.NextDouble();
            double r = Math.Sqrt(temp);
            double theta = 2.0 * Math.PI * random.NextDouble();
            return new Vector3d(
                r * Math.Cos(theta),
                r * Math.Sin(theta),
                Math.Sqrt(1.0 - temp));
        }
    }
}
