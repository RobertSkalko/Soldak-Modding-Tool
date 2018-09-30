using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SoldakModdingTool
{
    public class EditorGenerator : MonoBehaviour
    {
        private static Dictionary<string, SoldakObject> OverridenObjects;
        private static Dictionary<string, SoldakObject> AllObjects;

        public static UnityEvent OnFilterUpdated;

        public static void TryAddOverride(string name, string key, string value)
        {
            if (IfIsAnEdit(name, key, value)) {
                if (OverridenObjects.ContainsKey(name)) {
                    OverridenObjects[name].Dict[key] = new List<string>() { value };
                }
                else {
                    var obj = SoldakObject.GenerateOverrideObject(name);

                    obj.Dict[key] = new List<string>() { value };

                    OverridenObjects.Add(name, obj);
                }
            }
        }

        private static bool IfIsAnEdit(string name, string key, string value)
        {
            var obj = AllObjects[name];
            if (!obj.Dict.ContainsKey(key) || !obj.Dict[key].SequenceEqual(new List<string>() { value })) {
                return true;
            }

            return false;
        }

        public void Start()
        {
            if (OnFilterUpdated == null) {
                OnFilterUpdated = new UnityEvent();
                OnFilterUpdated.AddListener(OnUpdate);
            }
            if (OverridenObjects == null || OverridenObjects.Count == 0) {
                OverridenObjects = new Dictionary<string, SoldakObject>();
            }

            if (AllObjects == null || AllObjects.Count == 0) {
                AllObjects = new Dictionary<string, SoldakObject>();

                FileManager.GetObjectsFromAllFilesInPath(Save.file.GamePath).ToList().Where(x => x.Modifier == Modifiers.none).ToList().ForEach(x => AllObjects.Add(x.Name, x));
            }
        }

        public void OnUpdate()
        {
            var objects = AllObjects;

            if (!string.IsNullOrEmpty(Save.file.EditorDatas.NameContains)) {
                objects = (Dictionary<string, SoldakObject>)AllObjects.Where(x => x.Value.Name.Contains(Save.file.EditorDatas.NameContains));
            }
            if (!string.IsNullOrEmpty(Save.file.EditorDatas.AnyPartContains)) {
                objects = (Dictionary<string, SoldakObject>)AllObjects.Where(x => x.Value.GetTextRepresentation().Contains(Save.file.EditorDatas.AnyPartContains));
            }
            if (!string.IsNullOrEmpty(Save.file.EditorDatas.IsDerivedOf)) {
                objects = AllObjects.GetDerivedFrom(Save.file.EditorDatas.IsDerivedOf);
            }

            if (objects.Count < 50) {
                Debug.Log("filtered correctly probably");
            }
        }
    }
}