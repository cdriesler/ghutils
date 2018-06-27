using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;
using Grasshopper.Kernel;
using Rhino.DocObjects;

namespace RenameGeometry
{
    public class SetGeometryName : GH_Component
    {
        public SetGeometryName() : base("Set Geometry Name", "SetName", "Sets name of geometry.", "Params", "Util")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Geometry ID", "ID", "Input geometry id.", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name to assign.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Output geometry.", GH_ParamAccess.item);
            pManager.AddTextParameter("Geometry ID", "ID", "Used IDs.", GH_ParamAccess.item);
            pManager.AddTextParameter("Confirmation", "c", "Text explanation of operation.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string id = null;
            DA.GetData(0, ref id);

            Rhino.DocObjects.ObjRef geoRef = new ObjRef(new Guid(id));
            var geoObj = geoRef.Object();

            string name = null;
            DA.GetData(1, ref name);

            geoObj.Attributes.Name = name;
            geoObj.CommitChanges();

            string geoID = geoRef.ObjectId.ToString();

            DA.SetData(0, geoRef.Geometry());
            DA.SetData(1, geoID);

            //Confirmation routine.
            ObjRef confRef = new ObjRef(new Guid(geoID));

            string confName = confRef.Object().Name;
            string confGeoType = confRef.Geometry().ObjectType.ToString();

            string confString = confGeoType + " > " + confName + " [" + geoID.ToString() + "]";

            DA.SetData(2, confString);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            { return null; /* Properties.Resources.icon; */ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a00081ce-1e8a-42ef-bfe3-88f5f9508be0"); }
        }
    }
}
