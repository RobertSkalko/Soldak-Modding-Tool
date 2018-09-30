using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SoldakModdingTool
{
    public class EditorGenerator : MonoBehaviour
    {
        public GameObject LineObjectPrefab;

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

        public void GenerateModFile()
        {
            string s = "";

            OverridenObjects.Values.ToList().ForEach(x => s += x.GetTextRepresentation());

            FileManager.SaveOutputToFile(s);
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
                OnFilterUpdated.AddListener(OnFilterUpdate);
            }
            if (OverridenObjects == null || OverridenObjects.Count == 0) {
                OverridenObjects = new Dictionary<string, SoldakObject>();
            }

            if (AllObjects == null || AllObjects.Count == 0) {
                AllObjects = new Dictionary<string, SoldakObject>();

                FileManager.GetObjectsFromAllFilesInPath(Save.Instance.GamePath).ToList().Where(x => x.Modifier == Modifiers.none).ToList().ForEach(x => AllObjects.Add(x.Name, x));
            }
        }

        private void OnFilterUpdate()
        {
            var objects = AllObjects;

            if (!string.IsNullOrEmpty(Save.Instance.EditorDatas.NameContains)) {
                objects = objects.Where(x => x.Value.Name.Contains(Save.Instance.EditorDatas.NameContains)).ToDictionary(v => v.Key, v => v.Value);
                Debug.Log("Objects after name filtering :" + objects.Count);
            }
            if (!string.IsNullOrEmpty(Save.Instance.EditorDatas.AnyPartContains)) {
                objects = objects.Where(x => x.Value.GetTextRepresentation().Contains(Save.Instance.EditorDatas.AnyPartContains)).ToDictionary(v => v.Key, v => v.Value);
                Debug.Log("Objects after anypart filtering :" + objects.Count);
            }
            if (!string.IsNullOrEmpty(Save.Instance.EditorDatas.IsDerivedOf)) {
                objects = objects.GetDerivedFrom(Save.Instance.EditorDatas.IsDerivedOf);
                Debug.Log("Objects after derived filtering :" + objects.Count);
            }

            int MaxObjectCount = 50;

            if (objects.Count > MaxObjectCount) {
                Debug.Log("too many objects :" + objects.Count + " Please filter until there's less than " + MaxObjectCount + " objects");
            }
            else if (objects.Count == 0) {
                Debug.Log("No Objects Match");
            }
            else {
                UpdateView(objects);
            }
        }

        private void UpdateView(Dictionary<string, SoldakObject> dict)
        {
            foreach (Transform kid in this.transform) {
                Destroy(kid.gameObject);
            }

            foreach (var item in dict) {
                foreach (var line in item.Value.Dict) {
                    var obj = Instantiate(LineObjectPrefab, transform);
                    var comp = obj.GetComponent<EditorLine>();
                    foreach (var str in line.Value) {
                        comp.Init(item.Key, line.Key, str);
                    }
                }
            }
        }
    }
}