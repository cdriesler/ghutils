using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenameGeometry.Properties;

using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace RenameGeometry
{
    public class BakeWithName : GH_Component
    {
        public BakeWithName() : base("Bake With Name", "BakeName", "Bakes geometry into rhino document with given name.", "Params", "Util")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometry", "G", "Geometry to bake.", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name for geometry.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Bake", ">", "Set to True to bake.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output Geometry ID", "ID", "ID of new geometry created.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo g = null;
            DA.GetData<IGH_GeometricGoo>(0, ref g);

            string geoType = g.TypeName;

            string name = null;
            DA.GetData(1, ref name);

            bool activate = false;
            DA.GetData(2, ref activate);

            Rhino.DocObjects.ObjectAttributes atts = new Rhino.DocObjects.ObjectAttributes();
            string newID = null;
            atts.Name = name;

            if (activate)
            {
                GH_Point p = g as GH_Point;
                if (p != null)
                {
                    GeometryBase geo = GH_Convert.ToGeometryBase(p);
                    newID = RhinoDoc.ActiveDoc.Objects.Add(geo, atts).ToString();
                }
                GH_Curve c = g as GH_Curve;
                if (c != null)
                {
                    GeometryBase geo = GH_Convert.ToGeometryBase(c);
                    newID = RhinoDoc.ActiveDoc.Objects.Add(geo, atts).ToString();
                }
            }

            DA.SetData(0, newID);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.BakeName; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("854d9035-1c2f-4677-b5ea-ff68373828ca"); }
        }
    }
}


