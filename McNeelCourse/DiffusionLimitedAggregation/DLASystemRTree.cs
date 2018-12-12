using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace McNeelCourse.DiffusionLimitedAggregation
{
    public class DlaSystemRTree
    {
        public List<Point3d> Particles;
        public List<Line> Branches;
        public List<FreeParticle> FreeParticles;

        private double particleRadius = 0.3;
        private double environmentBoundary;
        private double clusterReach;
        private RTree rTree;

        public DlaSystemRTree(List<Point3d> initialParticles, List<Line> initialBranches, int freeParticleCount)
        {
            Particles = new List<Point3d>();
            Particles.AddRange(initialParticles);

            Branches = new List<Line>();
            Branches.AddRange(initialBranches);

            clusterReach = 0.0;
            foreach (Point3d particle in Particles)
            {
                double d = particle.DistanceTo(Point3d.Origin);
                if (d > clusterReach)
                    clusterReach = d;
            }

            UpdateEnvironmentBoundary();

            FreeParticles = new List<FreeParticle>();

            for (int i = 0; i < freeParticleCount; i++)
                FreeParticles.Add(new FreeParticle(environmentBoundary, particleRadius));

            rTree = new RTree();

            for (int i = 0; i < Particles.Count; i++)
            {
                rTree.Insert(Particles[i], i);
            }
        }


        private void UpdateEnvironmentBoundary()
        {
            environmentBoundary = clusterReach * 2.0;
            if (environmentBoundary < particleRadius * 20.0)
                environmentBoundary = particleRadius * 20.0;
        }


        public void Iterate()
        {
            for (int i = 0; i < FreeParticles.Count; i++)
            {
                FreeParticle freeParticle = FreeParticles[i];

                // Apply random force to the particle
                Vector3d randomForce = Util.RandomUnitVector() * particleRadius;
                freeParticle.Velocity += randomForce;
                if (freeParticle.Velocity.Length > particleRadius)
                    freeParticle.Velocity *= particleRadius / freeParticle.Velocity.Length;
                freeParticle.Position += freeParticle.Velocity;

                // If the particle wanders out of range, respawn it
                if (freeParticle.Position.DistanceTo(Point3d.Origin) > environmentBoundary ||
                    freeParticle.Position.Z < 0.0)
                {
                    FreeParticles[i] = new FreeParticle(environmentBoundary, particleRadius);
                    continue;
                }

                // Otherwise check if the free particle collide with the cluster
                Point3d collidedParticle = Point3d.Unset;
                EventHandler<RTreeEventArgs> rTreeCallback = (sender, args) =>
                {
                    collidedParticle = Particles[args.Id];
                    args.Cancel = true;
                };

                rTree.Search(
                    new Sphere(freeParticle.Position, 2.0 * particleRadius),
                    rTreeCallback);

                // If a collision has indeed been found
                if (collidedParticle.IsValid)
                {
                    Particles.Add(freeParticle.Position);
                    Branches.Add(new Line(freeParticle.Position, collidedParticle));
                    FreeParticles[i] = new FreeParticle(environmentBoundary, particleRadius);

                    double distanceToOrigin = freeParticle.Position.DistanceTo(Point3d.Origin);
                    if (clusterReach < distanceToOrigin) clusterReach = distanceToOrigin;

                    rTree.Insert(freeParticle.Position, Particles.Count - 1);
                }
            }

            UpdateEnvironmentBoundary();
        }
    }
}
