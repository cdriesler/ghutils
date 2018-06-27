using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NumbersToDomain.Properties;

using Grasshopper.Kernel;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace RenameGeometry
{
    public class GetGeometryName : GH_Component
    {
        public GetGeometryName() : base("Get Geometry Name", "GetName", "Returns name of geometry. Can be empty.", "Params", "Util")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Geometry ID", "ID", "Input geometry id.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Geometry's name.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string incomingID = null;
            DA.GetData(0, ref incomingID);

            Rhino.DocObjects.ObjRef geoRef = new ObjRef(new Guid(incomingID));

            string ObjectName = geoRef.Object().Name;

            DA.SetData(0, ObjectName);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            { return null; /* Properties.Resources.icon; */ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("94e5301b-28f6-4746-86d0-4b18878e4d0e"); }
        }
    }
}
