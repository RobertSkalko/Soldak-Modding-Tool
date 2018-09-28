using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolGetAllDerived : ToolButton
    {
        public override string Name { get => "Get Derived"; }
        public override string Description { get; }

        protected override void Action()
        {
            string file = FileManager.GetObjectsFromAllFilesInPath(Save.file.GamePath, true).ToList().GetDerivedObjectsOf(Save.file.InputCommand).JoinIntoString();

            FileManager.SaveOutputToFile(file);
        }
    }
}