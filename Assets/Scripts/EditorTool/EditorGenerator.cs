using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorGenerator : MonoBehaviour
    {
        private static Dictionary<string, SoldakObject> Dict;

        public static void TryAddOverride(string name, string key, string value)
        {
            if (IfIsAnEdit(name, key, value)) {
            }
        }

        private static bool IfIsAnEdit(string name, string key, string value)
        {
            var obj = Dict[name];
            if (!obj.Dict.ContainsKey(key) || !obj.Dict.ContainsValue(new List<string>() { value })) {
                return true;
            }
            if (obj.Dict[key].Contains(value)) {
                return true;
            }

            return false;
        }

        public void Start()
        {
            if (Dict == null || Dict.Count == 0) {
                Dict = new Dictionary<string, SoldakObject>();

                FileManager.GetObjectsFromAllFilesInPath(Save.file.GamePath).ToList().Where(x => x.Modifier == Modifiers.none).ToList().ForEach(x => Dict.Add(x.Name, x));
            }
        }
    }
}