using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolOnHitOrKillReduce : ToolButton
    {
        public override string Name { get => "Reduce On Hit/Kill chances"; }
        public override string Description { get; }

        public List<string> OnHitKill = new List<string>() { "UseOnHitChance", "UseOnKillChance" };

        protected override void Action()
        {
            List<SoldakObject> objects = new List<SoldakObject>();

            foreach (var item in FileManager.GetObjectsFromAllFilesInPath(Save.Instance.GamePath)
                .ToList().GetDerivedFrom("BaseSkill")
                .Where(x => x.Name.Contains("SkillModification") && x.Modifier == Modifiers.none)) {
                int num = -1;

                for (var i = 0; i < OnHitKill.Count; i++) {
                    var str = OnHitKill[i];
                    if (item.Dict.ContainsKey(str)) {
                        num = i;
                    }
                }

                if (num > -1) {
                    var obj = SoldakObject.GenerateOverrideObject(item.Name);

                    float oldval = float.Parse(item.Dict[OnHitKill[num]][0]);

                    float newval;

                    if (OnHitKill[num] == "UseOnHitChance") {
                        newval = (float)(oldval / 2.5);
                    }
                    else {
                        newval = (float)(oldval / 1.5);
                    }

                    obj.Dict.Add(OnHitKill[num], new List<string>() { (oldval / 2).ToString() });

                    objects.Add(obj);
                }
            }

            FileManager.SaveOutputToFile(string.Join("\n", objects.ToStringList()));
        }
    }
}