using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolOverrideNameSetter : ToolButton
    {
        public override string Name { get => "Override Name Setter"; }
        public override string Description { get => "If you don't want to transform *Name* into *ModNameName overrides Name manually* then use this tool!"; }

        protected override void Action()
        {
            var list = new List<string>();

            foreach (var obj in FileManager.GetObjectsFromAllFilesInPath(Save.file.FilesToEditPath)) {
                list.Add(obj.GetTextRepresentation(obj.Dict, Save.file.ModName, Modifiers.overrides));
            }
            FileManager.SaveOutputToFile(string.Join("\n", list.ToArray()));
        }
    }
}