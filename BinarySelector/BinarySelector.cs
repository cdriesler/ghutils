using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySelector.Properties;
using Rhino;
using Grasshopper.Kernel;

namespace BinarySelector
{
    public class BinarySelector : GH_Component
    {
        public BinarySelector() : base("Binary Selector", "BinarySel", "Returns a series of binary numbers to use for selection.", "Sets", "Binary")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Number", "N", "Number of objects to generate list for.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Binary Output", "b", "List of numbers to use for selection.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int numberItems = 0;
            DA.GetData(0, ref numberItems);

            int totalCombinations = Convert.ToInt32(Math.Pow(2, numberItems));

            List<string> outputNumbers = new List<string>();

            for (int i = 0; i < totalCombinations; i++)
            {
                outputNumbers.Add(Convert.ToString(i, 2).PadLeft(numberItems, '0'));
            }

            DA.SetDataList(0, outputNumbers);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return null; /*Properties.Resources.icon;*/ }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("bd8ded4d-ba43-4410-bf78-691b7a9882ad"); }
        }
    }
}
