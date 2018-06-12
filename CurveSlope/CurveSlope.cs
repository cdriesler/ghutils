using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurveSlope.Properties;
using Rhino;
using Grasshopper.Kernel;

namespace CurveSlope
{
    public class CurveSlope : GH_Component
    {
        public CurveSlope() : base("Curve Slope", "CSlope", "Returns slope of 2D curve.", "Curve", "Analysis")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curve to measure.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Slope", "d", "Slope of curve.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Proportion", "P","General proportion. 0 = Horizontal (dY = 0) 1 = Landscape (dX > dY) 2 = Sqaure (dX = dY) 3 = Portrait (dY > dX) 4 = Vertical (dX = 0)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Curve curveToMeasure = null;
            DA.GetData(0, ref curveToMeasure);

            Rhino.Geometry.Point3d startPoint = curveToMeasure.PointAtStart;
            Rhino.Geometry.Point3d endPoint = curveToMeasure.PointAtEnd;

            double dY = endPoint.Y - startPoint.Y;
            double dX = endPoint.X - startPoint.X;

            if (dX == 0)
            {
                DA.SetData(0, null);
                DA.SetData(1, 4);
            }
            else
            {
                double slope = dY / dX;

                DA.SetData(0, slope);

                if (dY == 0)
                {
                    DA.SetData(1, 0);
                }
                else if (dX > dY)
                {
                    DA.SetData(1, 1);
                }
                else if (dX == dY)
                {
                    DA.SetData(1, 2);
                }
                else if (dY > dX)
                {
                    DA.SetData(1, 3);
                }
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.icon; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("142a50c8-810c-476f-86f9-06be0893a429"); }
        }
    }
}
