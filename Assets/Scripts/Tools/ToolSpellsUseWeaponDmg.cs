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

            foreach (var obj in Main.GetObjectsFromAllFilesInPath(Save.file.FilesToEditPath)) {
                if (obj.Dict.ContainsKey("ProjMinDamage") && obj.Dict.ContainsKey("ProjMaxDamage") && float.Parse(obj.Dict["ProjMaxDamage"][0]) > 0) {
                    float AverageDmg = (float.Parse(obj.Dict["ProjMinDamage"][0]) + float.Parse(obj.Dict["ProjMaxDamage"][0])) / 2F;

                    if (!obj.Dict["Base"][0].Contains("PerLevel")) { // if its a skill (should be)
                        float DmgMult = ConvertSpellDamageIntoWeaponDmgMultipliers(AverageDmg);

                        Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>
                        {
                        { "ProjMinDamage", new List<string>() { "0" } },
                        { "ProjMaxDamage", new List<string>() { "0" } },
                        { "DamageMultAll", new List<string>() {  DmgMult.ToString() } },
                        { "ProjectileDamage", new List<string>() { "1" } },
                        };

                        list.Add(obj.GetTextRepresentation(Dict, Save.file.ModName, Modifiers.overrides));
                    }
                    else if (obj.Dict["Base"][0].Contains("PerLevel")) { // if per level
                        float DmgMult = GetPerLevelMult(AverageDmg);

                        Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>
                        {
                        { "ProjMinDamage", new List<string>() { "0" } },
                        { "ProjMaxDamage", new List<string>() { "0" } },
                        { "DamageMultAll", new List<string>() {  DmgMult.ToString() } },
                        };

                        list.Add(obj.GetTextRepresentation(Dict, Save.file.ModName, Modifiers.overrides));
                    }
                }
            }

            Main.SaveOutputToFile(string.Join("\n", list.ToArray()));
            //Debug.Log(string.Join("\n", list.ToArray()));
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
                return 1 + (AverageDmg - AverageDmgCutOff) * MultPerOneNumber;
            }
            else {
                return -Math.Abs(MultPerOneNumber * (AverageDmg - AverageDmgCutOff) * 2.5F);
            }
        }
    }
}