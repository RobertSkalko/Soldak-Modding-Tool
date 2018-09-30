using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class SoldakObject : IEquatable<SoldakObject>, IComparable<SoldakObject>
    {
        public bool Equals(SoldakObject other)
        {
            return this.Name == other.Name && this.Modifier == Modifiers.none && other.Modifier == Modifiers.none;
        }

        public int CompareTo(SoldakObject other)
        {
            return FilePath.CompareTo(other.FilePath) == 1 ? 1 : 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Modifier.GetHashCode();
        }

        public string FilePath;
        public string ModdedName = "";
        public Modifiers Modifier = Modifiers.none;
        public string Name = "";
        public Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>();

        public bool HasBase => Dict.ContainsKey("Base") && Dict["Base"].Count > 0 && !string.IsNullOrEmpty(Dict["Base"][0]);
        public string GetBase => Dict["Base"][0];

        public string CreateOverrideName(string ModName)
        {
            return ModName + Name;
        }

        public static SoldakObject GenerateOverrideObject(string name)
        {
            SoldakObject newobj = new SoldakObject
            {
                Name = name
            };
            newobj.ModdedName = Save.Instance.ModName + name;
            newobj.Modifier = Modifiers.overrides;

            return newobj;
        }

        public SoldakObject(string text, string filepath)
        {
            this.FilePath = filepath;

            SetupNameInfo(text);

            text = RemoveBracketsAndAnythingBeforeThem(text);

            SetupDBInfo(text);
        }

        public SoldakObject()
        {
        }

        private string GetBeforeBracket(string txt)
        {
            return txt.Substring(0, txt.IndexOf('{'));
        }

        private List<string> RemoveEmptyStrings(string[] strings)
        {
            return strings.ToList().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        private void SetupNameInfo(string txt)
        {
            List<string> nameinfo = RemoveEmptyStrings(GetBeforeBracket(txt).Split());

            if (nameinfo.Count == 1) {
                Name = nameinfo[0];
            }
            else if (nameinfo.Count == 3) {
                Name = nameinfo[2];
                ModdedName = nameinfo[0];

                string modifier = nameinfo[1];
                if (modifier == Modifiers.overrides.ToString()) {
                    Modifier = Modifiers.overrides;
                }
                else if (modifier == Modifiers.addsTo.ToString()) {
                    Modifier = Modifiers.addsTo;
                }
                else {
                    Debug.Log("Invalid modifier! " + modifier.ToString());
                }
            }
            else {
                Debug.Log("Invalid Name Lines!" + string.Join(" ", nameinfo));
            }
        }

        private string TrimWhiteSpaceAtBothEnds(string s)
        {
            return s.TrimEnd().TrimStart();
        }

        private void SetupDBInfo(string txt)
        {
            foreach (string s in RemoveEmptyStrings(txt.Split('\n'))) {
                if (string.IsNullOrWhiteSpace(s)) {
                    continue;
                }
                string trimmed = TrimWhiteSpaceAtBothEnds(s);

                int SplitIndex = trimmed.IndexOf(c => char.IsWhiteSpace(c));

                string left = trimmed.Substring(0, SplitIndex).TrimEnd();
                var right = new List<string>() { trimmed.Substring(SplitIndex).TrimStart() };

                if (!Dict.ContainsKey(left)) {
                    Dict.Add(left, right);
                }
                else {
                    Dict[left].AddRange(right);
                }
            }
        }

        private string RemoveBracketsAndAnythingBeforeThem(string txt)
        {
            txt = txt.Substring(txt.IndexOf("{"));

            txt = txt.Replace("{", "");
            txt = txt.Replace("}", "");

            return txt;
        }

        public string GetTextRepresentation()
        {
            return GetTextRepresentation(Dict, Save.Instance.ModName, Modifier);
        }

        public string GetTextRepresentation(Dictionary<string, List<string>> OverridenValues, string modName, Modifiers modifier)
        {
            string text = "";

            // Name portion
            if (modName.Length != 0 && modifier != Modifiers.none) {
                text += CreateOverrideName(modName) + " " + modifier.ToString() + " " + Name;
            }
            else {
                text += Name;
            }
            // end

            text += "\n{\n";

            foreach (var item in OverridenValues) {
                if (item.Value.Count == 0) {
                    Debug.Log("Value can't be empty!!");
                }
                else if (item.Value.Count == 1) {
                    text += "\n\t" + item.Key + " " + item.Value[0];
                }
                else {
                    foreach (string s in item.Value) {
                        text += "\n\t" + item.Key + " " + s;
                    }
                }
            }

            text += "\n}\n";

            return text;
        }
    }
}