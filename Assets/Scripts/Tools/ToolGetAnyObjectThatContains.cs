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
            var set = new HashSet<SoldakObject>();

            string str = Save.file.InputCommand;

            foreach (var obj in FileManager.GetObjectsFromAllFilesInPath(Save.file.GamePath)) {
                if (obj.Name.Contains(str) || obj.ModdedName.Contains(str) || (obj.Dict.Count > 0 && obj.Dict.Any(x => x.Key.Contains(str) || x.Value.Contains(str)))) {
                    set.Add(obj);
                }
            }
        }
    }
}