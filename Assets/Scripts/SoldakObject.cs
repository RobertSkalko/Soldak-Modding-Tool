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
        public Dictionary<string, List<string>> Dict = new Dictionary<string, List<string>>();

        public bool HasBase => Dict.ContainsKey("Base") && Dict["Base"].Count > 0 && !string.IsNullOrEmpty(Dict["Base"][0]);
        public string GetBase => Dict["Base"][0];

        public string CreateOverrideName(string ModName)
        {
            return ModName + Name;
        }

        public SoldakObject(string text)
        {
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
                    Debug.Log("Invalid modifier! " + modifier.ToString());
                }
            }
            else {
                Debug.Log("Invalid Name Lines!" + string.Join(" ", nameinfo));
            }
        }

        private string TrimWhiteSpaceAtStart(string s)
        {
            string final = s;
            for (var i = 0; i < s.Length; i++) {
                char c = s[i];
                if (char.IsWhiteSpace(c)) {
                    final = final.Substring(1);
                }
                else {
                    return final;
                }
            }
            return final;
        }

        private string TrimWhiteSpaceAtEnd(string s)
        {
            string final = s;
            for (var i = s.Length - 1; i > -1; i--) {
                char c = s[i];
                if (char.IsWhiteSpace(c)) {
                    final = final.Substring(0, final.Length - 1);
                }
                else {
                    return final;
                }
            }
            return final;
        }

        private string TrimWhiteSpaceAtBothEnds(string s)
        {
            return TrimWhiteSpaceAtEnd(TrimWhiteSpaceAtStart(s));
        }

        // this is probably performance issue
        private void SetupDBInfo(string txt)
        {
            foreach (string s in RemoveEmptyStrings(txt.Split('\n'))) {
                if (string.IsNullOrWhiteSpace(s)) {
                    continue;
                }
                string trimmed = TrimWhiteSpaceAtBothEnds(s);

                int SplitIndex = trimmed.IndexOf(c => char.IsWhiteSpace(c));

                string first = TrimWhiteSpaceAtBothEnds(trimmed.Substring(0, SplitIndex));
                string second = TrimWhiteSpaceAtBothEnds(trimmed.Substring(SplitIndex));

                List<string> KeyAndValue = new List<string>() { first, second };

                if (KeyAndValue.Count == 2) {
                    var value = new List<string>() { KeyAndValue[1] };
                    var key = KeyAndValue[0];
                    if (!Dict.ContainsKey(key)) {
                        Dict.Add(key, value);
                    }
                    else {
                        Dict[key].AddRange(value);
                    }
                }
                else {
                    Debug.Log("Data length isn't 2, there must be an entry and a value!");
                    KeyAndValue.ForEach(x => Debug.Log(x));
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