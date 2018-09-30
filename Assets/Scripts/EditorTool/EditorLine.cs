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
        public string Key { get => _key; set { _key = value; OnValueChanged.Invoke(); } }

        private string _value;
        public string Value { get => _value; set { _value = value; OnValueChanged.Invoke(); } }

        public UnityEvent OnValueChanged;

        public void Init(string name, string key, string value)
        {
            OnValueChanged = new UnityEvent();

            this.Name = name;
            this.Key = key;
            this.Value = value;

            DefualtKeyObj.text = key;
            DefualtValueObj.text = value;
        }

        public void Start()
        {
            OnValueChanged.AddListener(AddOverride);
        }

        public void AddOverride()
        {
            EditorGenerator.TryAddOverride(Name, Key, Value);
        }
    }
}