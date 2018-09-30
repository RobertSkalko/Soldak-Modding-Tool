using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolGetAnyObjectThatContains : ToolButton
    {
        public override string Name { get => "GetAnyObjectThatContains"; }
        public override string Description { get => ""; }

        protected override void Action()
        {
            var set = new HashSet<string>();

            string str = Save.File.InputCommand;

            foreach (var obj in FileManager.GetObjectsFromAllFilesInPath(Save.File.GamePath)) {
                if (obj.Name.Contains(str)
                    || (!string.IsNullOrEmpty(obj.ModdedName) && obj.ModdedName.Contains(str))
                    || (obj.Dict.Count > 0 && obj.Dict.Any(x => x.Key.Contains(str) || (x.Value.Count > 0 && x.Value.Any(y => y.Contains(str)))))) {
                    set.Add(obj.GetTextRepresentation(obj.Dict, obj.ModdedName, obj.Modifier));
                }
            }

            FileManager.SaveOutputToFile(string.Join("\n", set));
        }
    }
}