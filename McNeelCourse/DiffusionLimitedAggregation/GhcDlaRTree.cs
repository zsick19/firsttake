using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace McNeelCourse.DiffusionLimitedAggregation
{
    public class GhcDiffusionLimitedAggregationRTree : GH_Component
    {
        private DlaSystemRTree dlaSystemRTree;

        public GhcDiffusionLimitedAggregationRTree()
            : base(
                "DiffusionLimitedAggregationRTree", 
                "DLA-RTree",
                "Diffusion Limited Aggregation using RTree",
                "McNeelCourse", 
                "DLA")
        {
        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Run", "Run", "Run", GH_ParamAccess.item, false);
            pManager.AddPointParameter("Initial Particles", "Initial Particles", "Initial Particles", GH_ParamAccess.list);
            pManager.AddLineParameter("Initial Branches", "Initial Branches", "Initial Branches", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Free Particle Count", "Free Particle Count", "Free Particle Count", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Subiterations", "Subiterations", "Subiterations", GH_ParamAccess.item, 10);
        }


        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Info", "Info", "Info", GH_ParamAccess.item);
            pManager.AddPointParameter("Particles", "Particles", "Particles", GH_ParamAccess.list);
            pManager.AddPointParameter("Free Particles", "Free Particles", "Free Particles", GH_ParamAccess.list);
            pManager.AddLineParameter("Branches", "Branches", "Branches", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool iReset = false;

            DA.GetData("Reset", ref iReset);

            if (iReset || dlaSystemRTree == null)
            {
                List<Point3d> iInitialParticles = new List<Point3d>();
                DA.GetDataList("Initial Particles", iInitialParticles);
                List<Line> iInitialBranches = new List<Line>();
                DA.GetDataList("Initial Branches", iInitialBranches);
                int iFreeParticleCount = 0;
                DA.GetData("Free Particle Count", ref iFreeParticleCount);
                dlaSystemRTree = new DlaSystemRTree(iInitialParticles, iInitialBranches, iFreeParticleCount);
                return;
            }

            int iSubiterations = 0;
            bool iUseParallel = false;

            DA.GetData("Subiterations", ref iSubiterations);

            for (int i = 0; i < iSubiterations; i++)
                dlaSystemRTree.Iterate();

            bool iRun = false;
            DA.GetData("Run", ref iRun);

            if (iRun)
                ExpireSolution(true);


            //================================================================
            // Output data using Grasshopper geometry data type wrapper
            //================================================================

            List<GH_Point> particles = new List<GH_Point>();
            foreach (Point3d particle in dlaSystemRTree.Particles) 
                particles.Add(new GH_Point(particle));
            DA.SetDataList("Particles", dlaSystemRTree.Particles);

            List<GH_Line> branches = new List<GH_Line>();
            foreach (Line line in dlaSystemRTree.Branches)
                branches.Add(new GH_Line(line));
            DA.SetDataList("Branches", branches);

            List<GH_Point> freeParticles = new List<GH_Point>();
            foreach (FreeParticle freeParticle in dlaSystemRTree.FreeParticles)
                freeParticles.Add(new GH_Point(freeParticle.Position));
            DA.SetDataList("Free Particles", freeParticles);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("{dedd68da-8e6d-4590-b239-a92662520386}"); }
        }
    }
}
