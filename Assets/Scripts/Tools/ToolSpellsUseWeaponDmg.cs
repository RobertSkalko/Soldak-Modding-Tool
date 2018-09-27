using System;
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
                if (obj.Name.Contains("Skill") && obj.Dict.ContainsKey("ProjMinDamage") && obj.Dict.ContainsKey("ProjMaxDamage")) {
                    Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>
                    {
                    { "ProjMinDamage", new List<string>() { "0" } },
                    { "ProjMaxDamage", new List<string>() { "0" } },
                    { "DamageMultAll", new List<string>() { "0" } },
                    { "ProjectileDamage", new List<string>() { "1" } },
                    };

                    Debug.Log(obj.GetTextRepresentation(Dict, Save.file.ModName, Modifiers.overrides));
                }
                else if (obj.Name.Contains("PerLevel") && obj.Dict.ContainsKey("ProjMinDamage") && obj.Dict.ContainsKey("ProjMaxDamage")) {
                }
            }
        }

        private float ConvertSpellDamageIntoWeaponDmgMultipliers(float AverageDmg)
        {
            float MultPerOneNumber = 0.01F;
            float AverageDmgCutOff = 30;

            if (AverageDmg > AverageDmgCutOff) {
                return 1 + (AverageDmg - AverageDmgCutOff) * MultPerOneNumber;
            }
            else {
                return -Math.Abs(MultPerOneNumber * (AverageDmg - AverageDmgCutOff) * 2.5F);
            }
        }
    }
}