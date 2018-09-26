using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolSpellsUseWeaponDmg : ToolButton
    {
        public override string Name { get => "Spell Damage Into Weapon Damage"; }
        public override string Description { get => "Put your mod zip file in the specified folder and it will generate spell overrides that use weapon damage in proportion to how much spell dmg it had "; }

        public override void Action()
        {
            var objects = Main.GetObjectsFromAllFilesInPath(Paths.FilesToEditPath);

            foreach (var obj in objects) {
                if (obj.Dict.ContainsKey("ProjMinDamage")) {
                    Debug.Log(obj.Dict["ProjMinDamage"]);
                }
            }
        }
    }
}