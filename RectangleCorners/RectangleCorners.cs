using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectangleCorners.Properties;
using Rhino;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RectangleCorners
{
    public class RectangleCorners : GH_Component
    {
        public RectangleCorners() : base("Rectangle Corners", "Corners", "Determines relative location of a rectangle's corner points.", "Curve", "Analysis")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Rectangle", "R", "Orthogonal rectangle to analyze.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Top Right", "A", "Top right corner point.", GH_ParamAccess.item);
            pManager.AddPointParameter("Top Left", "B", "Top left corner point.", GH_ParamAccess.item);
            pManager.AddPointParameter("Bottom Left", "C", "Bottom left corner point.", GH_ParamAccess.item);
            pManager.AddPointParameter("Bottom Right", "D", "Bottom right corner point.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Curve rectangle = null;
            DA.GetData(0, ref rectangle);

            bool closedBool = rectangle.IsClosed;
            int spans = rectangle.SpanCount;
            int degree = rectangle.Degree;

            List<Rhino.Geometry.Point2d> originalPoints = new List<Point2d>();

            List<double> xVals = new List<double>();
            List<double> yVals = new List<double>();

            if (closedBool != true || spans != 4 || degree != 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input curve is not a closed, linear quadrilateral.");

                return;
            }

            for (int i = 0; i < spans; i++)
            {
                Rhino.Geometry.Point3d cornerPoint = rectangle.PointAt(rectangle.SpanDomain(i).Min);
                Rhino.Geometry.Point2d flattenedCornerPoint = new Rhino.Geometry.Point2d(cornerPoint);
                originalPoints.Add(flattenedCornerPoint);

                xVals.Add(flattenedCornerPoint.X);
                yVals.Add(flattenedCornerPoint.Y);
            }

            Rhino.Geometry.LineCurve testCurveA = new Rhino.Geometry.LineCurve(originalPoints[0], originalPoints[2]);
            Rhino.Geometry.LineCurve testCurveB = new Rhino.Geometry.LineCurve(originalPoints[1], originalPoints[3]);

            if (testCurveA.GetLength() != testCurveB.GetLength())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input curve is not orthogonal, results are not guaranteed.");
            }

            Rhino.Geometry.Point2d topRightPoint = new Rhino.Geometry.Point2d(xVals.Max(), yVals.Max());
            DA.SetData(0, topRightPoint);

            Rhino.Geometry.Point2d topLeftPoint = new Rhino.Geometry.Point2d(xVals.Min(), yVals.Max());
            DA.SetData(1, topLeftPoint);

            Rhino.Geometry.Point2d bottomLeftPoint = new Rhino.Geometry.Point2d(xVals.Min(), yVals.Min());
            DA.SetData(2, bottomLeftPoint);

            Rhino.Geometry.Point2d bottomRightPoint = new Rhino.Geometry.Point2d(xVals.Max(), yVals.Min());
            DA.SetData(3, bottomRightPoint);

            //TODO: Check for rotation. No warnings will be raised if input is orthogonal rotated object, but corners will not be accurate.
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return null; /*Properties.Resources.icon;*/ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b5205ab5-8f90-4b3f-811e-27801e19d8b3"); }
        }
    }
}
