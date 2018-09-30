using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolSpellsUseWeaponDmg : ToolButton
    {
        public override string Name { get => "Spell Damage Into Weapon Damage"; }
        public override string Description { get => "Put your mod zip file in the specified folder and it will generate spell overrides that use weapon damage in proportion to how much spell dmg it had "; }

        protected override void Action()
        {
            var list = new List<string>();

            var min = new string[] { "ProjRadiusMinDamage", "ProjMinDamage", "MinDamage" };
            var max = new string[] { "ProjRadiusMaxDamage", "ProjMaxDamage", "MaxDamage" };

            int number;

            foreach (var obj in GetDerived.GetDerivedFrom(FileManager.GetObjectsFromAllFilesInPath(Save.Instance.GamePath, true).ToList(), "BaseSkill")) {
                number = -1;

                for (var i = 0; i < min.Length; i++) {
                    if (obj.Dict.Any(x => x.Key.Equals(min[i]))) {
                        number = i;
                    }
                }

                if (number > -1) {
                    float AverageDmg = (float.Parse(obj.Dict[min[number]][0]) + float.Parse(obj.Dict[max[number]][0])) / 2F;

                    if (!obj.Dict["Base"][0].Contains("PerLevel")) { // if its a skill (should be)
                        float DmgMult = ConvertSpellDamageIntoWeaponDmgMultipliers(AverageDmg);

                        Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>
                        {
                        { min[number], new List<string>() { "0" } },
                        { max[number], new List<string>() { "0" } },
                        { "DamageMultAll", new List<string>() {  DmgMult.ToString() } },
                        { "ProjectileDamage", new List<string>() { "1" } },
                        };

                        list.Add(obj.GetTextRepresentation(Dict, Save.Instance.ModName, Modifiers.overrides));
                    }
                    else if (obj.Dict["Base"][0].Contains("PerLevel")) { // if per level
                        float DmgMult = GetPerLevelMult(AverageDmg);

                        Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>
                        {
                        { min[number], new List<string>() { "0" } },
                        { max[number], new List<string>() { "0" } },
                        { "DamageMultAll", new List<string>() {  DmgMult.ToString() } },
                        };

                        list.Add(obj.GetTextRepresentation(Dict, Save.Instance.ModName, Modifiers.overrides));
                    }
                }
            }

            FileManager.SaveOutputToFile(string.Join("\n", list.ToArray()));
        }

        private float GetPerLevelMult(float AverageDmg)
        {
            float MultPerOneNumber = 0.01F;

            return AverageDmg * MultPerOneNumber;
        }

        private float ConvertSpellDamageIntoWeaponDmgMultipliers(float AverageDmg)
        {
            float MultPerOneNumber = 0.01F;
            float AverageDmgCutOff = 30;

            if (AverageDmg > AverageDmgCutOff) {
                return (AverageDmg - AverageDmgCutOff) * MultPerOneNumber;
            }
            else {
                return -Math.Abs(MultPerOneNumber * (AverageDmg - AverageDmgCutOff) * 2.5F);
            }
        }
    }
}