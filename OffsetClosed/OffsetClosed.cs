using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OffsetClosed.Properties;
using Rhino;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace OffsetClosed
{
    public class OffsetClosed : GH_Component
    {
        public OffsetClosed() : base("Offset Closed", "Offset Closed", "Offsets a curve and returns the closed shape formed by the old and new profiles.", "Curve", "Util")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curve to offset.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "D", "Distance to offset curve.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Both Sides", "B", "Set to true to offset both sides.", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Region", "R", "Region formed by offset operation.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool centeredBool = false;
            DA.GetData(2, ref centeredBool);

            double distance = 0;
            DA.GetData(1, ref distance);

            Rhino.Geometry.Curve curveToOffset = null;
            DA.GetData(0, ref curveToOffset);

            Rhino.Geometry.Point3d firstStartPoint = curveToOffset.PointAtStart;
            Rhino.Geometry.Point3d firstEndPoint = curveToOffset.PointAtEnd;

            Rhino.Geometry.Plane defaultPlane = Plane.WorldXY;

            Rhino.Geometry.Curve[] offsetCurves = curveToOffset.Offset(defaultPlane, distance, 0.1, CurveOffsetCornerStyle.Sharp);
            Rhino.Geometry.Curve[] mirroredOffsetCurves = curveToOffset.Offset(defaultPlane, distance * -1, 0.1, CurveOffsetCornerStyle.Sharp);

            if (offsetCurves.Length > 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve is not clean.");

                return;
            }

            if (centeredBool == true && mirroredOffsetCurves.Length > 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve is not clean.");

                return;
            }

            Rhino.Geometry.Curve offsetCurve = offsetCurves[0];
            Rhino.Geometry.Point3d newStartPoint = offsetCurve.PointAtStart;
            Rhino.Geometry.Point3d newEndPoint = offsetCurve.PointAtEnd;

            Rhino.Geometry.Curve mirroredOffsetCurve = mirroredOffsetCurves[0];
            Rhino.Geometry.Point3d mirroredNewStartPoint = mirroredOffsetCurve.PointAtStart;
            Rhino.Geometry.Point3d mirroredNewEndPoint = mirroredOffsetCurve.PointAtEnd;

            Rhino.Geometry.Curve startCapCurve = null;
            Rhino.Geometry.Curve endCapCurve = null;

            List<Rhino.Geometry.Curve> regionCurvePieces = new List<Curve>();

            if (centeredBool == false)
            {
                startCapCurve = new Rhino.Geometry.LineCurve(firstStartPoint, newStartPoint) as Rhino.Geometry.Curve;
                endCapCurve = new Rhino.Geometry.LineCurve(firstEndPoint, newEndPoint) as Rhino.Geometry.Curve;

                regionCurvePieces.Add(curveToOffset);
            }
            else if (centeredBool == true)
            {
                startCapCurve = new Rhino.Geometry.LineCurve(mirroredNewStartPoint, newStartPoint) as Rhino.Geometry.Curve;
                endCapCurve = new Rhino.Geometry.LineCurve(mirroredNewEndPoint, newEndPoint) as Rhino.Geometry.Curve;

                regionCurvePieces.Add(mirroredOffsetCurve);
            }

            regionCurvePieces.Add(startCapCurve);
            regionCurvePieces.Add(offsetCurve);
            regionCurvePieces.Add(endCapCurve);

            Rhino.Geometry.Curve[] regionOffset = Curve.JoinCurves(regionCurvePieces);

            if (regionOffset.Length > 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Join was unsuccessful.");

                return;
            }

            DA.SetData(0, regionOffset[0]);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return null; /*Properties.Resources.icon;*/ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("69b993d2-2c5f-40af-b616-9b68d509f922"); }
        }
    }
}
