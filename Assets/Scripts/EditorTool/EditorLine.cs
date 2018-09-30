using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoldakModdingTool
{
    public class EditorLine : MonoBehaviour
    {
        public Text DefualtKeyObj;
        public Text DefualtValueObj;

        public string Name;

        public string DefaultKey;
        public string DefaultValue;

        private string LastKey = "";

        private string _key = "";
        public string Key { get => _key; set { _key = value; TryOverride(); LastKey = _key; } }

        private string _value = "";
        public string Value { get => _value; set { _value = value; TryOverride(); } }

        public void Init(string name, string key, string value)
        {
            this.Name = name;
            //this._key = key;
            // this._value = value;

            LastKey = key;

            DefaultKey = key;
            DefaultValue = value;

            DefualtKeyObj.text = key;
            DefualtValueObj.text = value;
        }

        public void TryOverride()
        {
            //Debug.Log(before + " " + after);
            if (string.IsNullOrWhiteSpace(Key) && string.IsNullOrWhiteSpace(Value)) {
                EditorGenerator.RemoveAKey(Name, LastKey);
            }
            else {
                var keyused = string.IsNullOrWhiteSpace(Key) ? DefaultKey : Key;
                var valueused = string.IsNullOrWhiteSpace(Value) ? DefaultValue : Value;

                EditorGenerator.TryAddOverride(Name, keyused, valueused);
                Debug.Log("trying to add override");
            }
        }
    }
}