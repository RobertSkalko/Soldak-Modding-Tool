using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class SoldakObject
    {
        public string ModdedName;
        public Modifiers Modifier;
        public string Name;
        public Dictionary<string, string> Dict = new Dictionary<string, string>();
        public string CreateOverrideName => "Mod" + Name;

        public SoldakObject(string text)
        {
            text = Main.RemoveComments(text);

            SetupNameInfo(text);

            text = RemoveBracketsAndAnythingBeforeThem(text);

            SetupDBInfo(text);
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
                    Debug.LogError("Invalid modifier!");
                }
            }
            else {
                Debug.LogError("Invalid Name Lines!");
            }
        }

        private void SetupDBInfo(string txt)
        {
            foreach (string s in RemoveEmptyStrings(txt.Split('\n'))) {
                List<string> data = RemoveEmptyStrings(s.Split());

                if (string.IsNullOrWhiteSpace(s)) {
                    continue;
                }

                if (data.Count == 2) {
                    Dict.Add(data[0], data[1]);
                }
                else {
                    Debug.LogError("Data length isn't 2, there must be an entry and a value!");
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

        public string RemoveComments(string file)
        {
            List<string> lines = file.Split('\n').ToList();

            lines.ForEach(x => x.Remove(file.IndexOf("//")));

            string final = "";

            lines.ForEach(x => final += x + "\n");

            return final;
        }

        public string GetTextRepresentation()
        {
            string text = "";

            // Name portion
            if (ModdedName.Length != 0 && Modifier != Modifiers.none) {
                text += CreateOverrideName + " " + Modifier.ToString() + " " + Name;
            }
            else {
                text += Name;
            }
            // end

            text += "\n{\n";

            foreach (var item in Dict) {
                text += "\n" + item.Key + " " + item.Value;
            }

            text += "\n}\n";

            return text;
        }
    }
}