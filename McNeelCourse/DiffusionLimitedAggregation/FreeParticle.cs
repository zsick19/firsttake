using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace McNeelCourse.DiffusionLimitedAggregation
{
    public class FreeParticle
    {
        public Point3d Position;
        public Vector3d Velocity;

        public FreeParticle(double environmentBoundary, double particleRadius)
        {
            Position = Point3d.Origin + Util.RandomUnitVectorUpperHemisphere() * environmentBoundary;
            Velocity = Util.RandomUnitVector() * particleRadius;
        }
    }
}
