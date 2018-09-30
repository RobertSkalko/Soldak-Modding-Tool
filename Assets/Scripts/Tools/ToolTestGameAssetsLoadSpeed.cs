using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolTestGameAssetsLoadSpeed : ToolButton
    {
        public override string Name { get => "Test Game Assets Load Speed"; }
        public override string Description { get; }

        protected override void Action()
        {
            FileManager.GetObjectsFromAllFilesInPath(Save.Instance.GamePath);
        }
    }
}