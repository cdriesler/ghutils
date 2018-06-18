using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbersToDomain.Properties;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace NumbersToDomain
{
    public class NumbersToDomain : GH_Component
    {
        public NumbersToDomain() : base("Numbers to Domain", "NumToDomain", "Returns the extremes of a set of numbers as a Grasshopper domain.", "Maths", "Domain")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Numbers", "N", "Numbers to parse.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntervalParameter("Domain", "D", "Domain of input values.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> numbersToParse = new List<double>();
            DA.GetDataList(0, numbersToParse);

            Interval outputDomain = new Interval(numbersToParse.Min(), numbersToParse.Max());

            DA.SetData(0, outputDomain);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            { return null; /* Properties.Resources.icon; */ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a195c842-b1ac-4f87-8f59-7cc861ee35e4"); }
        }
    }
}
