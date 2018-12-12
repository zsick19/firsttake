using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace McNeelCourse.DiffusionLimitedAggregation
{
    public class DlaSystem
    {
        public List<Point3d> Particles;
        public List<Line> Branches;
        public List<FreeParticle> FreeParticles;

        private double particleRadius = 0.3;
        private double environmentBoundary;
        private double clusterReach;

       
    }
}
