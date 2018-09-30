using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoldakModdingTool
{
    public class EditorObject : MonoBehaviour
    {
        public List<EditorLine> LineObjects;
        public string Name;
        public Text NameObj;

        public void Init(string name, Dictionary<string, List<string>> dict, GameObject prefab)
        {
            LineObjects = new List<EditorLine>();
            this.Name = name;
            NameObj.text = name;

            foreach (var line in dict) {
                var obj = Instantiate(prefab, transform);
                var comp = obj.GetComponent<EditorLine>();
                foreach (var str in line.Value) {
                    comp.Init(name, line.Key, str);
                }
                LineObjects.Add(comp);
            }
        }
    }
}