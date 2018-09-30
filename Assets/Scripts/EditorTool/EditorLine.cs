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

        private string _key;
        public string Key { get => _key; set { TryOverride(_key, value); _key = value; } }

        private string _value;
        public string Value { get => _value; set { TryOverride(_value, value); _value = value; } }

        public void Init(string name, string key, string value)
        {
            this.Name = name;
            this._key = key;
            this._value = value;

            DefualtKeyObj.text = key;
            DefualtValueObj.text = value;
        }

        public void TryOverride(string before, string after)
        {
            if (string.IsNullOrEmpty(after) && !string.IsNullOrEmpty(before)) {
                EditorGenerator.RemoveAKey(Key);
            }
            else {
                EditorGenerator.TryAddOverride(Name, Key, Value);
            }
        }
    }
}