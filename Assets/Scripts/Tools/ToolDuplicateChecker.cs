using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolDuplicateChecker : ToolButton
    {
        public override string Name { get => "Duplicate Checker"; }
        public override string Description { get; }

        protected override void Action()
        {
            var list = new List<string>();

            foreach (var obj in FileManager.GetObjectsFromAllFilesInPath(Save.File.FilesToEditPath, false, true)) {
                list.Add(obj.Name);
            }

            var duplicates = list.GroupBy(x => x)
                       .Where(group => group.Count() > 1)
                       .Select(group => group.Key);

            FileManager.SaveOutputToFile(string.Join("\n", duplicates.ToArray()));
        }
    }
}