using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolGetAllDerived : ToolButton
    {
        public override string Name { get => "Get Derived"; }
        public override string Description { get; }

        protected override void Action()
        {
            string file = Main.GetObjectsFromAllFilesInPath(Save.file.FilesToEditPath).GetDerivedObjectsOf("BaseTown").JoinIntoString();

            Main.SaveOutputToFile(file);
        }
    }
}