using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineSDLBS.Properties;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace LineSDLBS
{
    public class LineSDLBS : GH_Component
    {
        public LineSDLBS() : base("Line SDL Both Sides", "LineSDLBS", "Does the same thing as Line SDL, but in both directions.", "Curve", "Primitive")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Start Point", "S", "Starting point of line.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "D", "Forward direction of line. (Points towards end point.)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "L", "Length of line extension from start point, or, half the length of the final line.", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "L", "Linear output curve.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Point3d startPoint = new Rhino.Geometry.Point3d();
            DA.GetData(0, ref startPoint);

            Rhino.Geometry.Vector3d directionVector = new Rhino.Geometry.Vector3d();
            DA.GetData(1, ref directionVector);

            double lineDistance = 0;
            DA.GetData(2, ref lineDistance);

            double startingVectorLength = directionVector.Length;
            double scalingFactor = lineDistance / startingVectorLength;

            Rhino.Geometry.Vector3d newDirectionVector = directionVector * scalingFactor;

            double deltaX = newDirectionVector.X;
            double deltaY = newDirectionVector.Y;

            double baseX = startPoint.X;
            double baseY = startPoint.Y;

            Rhino.Geometry.Point2d outputCurveEndPoint = new Rhino.Geometry.Point2d(baseX + deltaX, baseY + deltaY);
            Rhino.Geometry.Point2d outputCurveStartPoint = new Rhino.Geometry.Point2d(baseX - deltaX, baseY - deltaY);

            Rhino.Geometry.Curve outputCurve = new Rhino.Geometry.LineCurve(outputCurveStartPoint, outputCurveEndPoint) as Rhino.Geometry.Curve;

            DA.SetData(0, outputCurve);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            { return null; /* Properties.Resources.icon; */ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a8ba9f68-ffcf-4186-a23b-b7ff4ce8620f"); }
        }
    }
}