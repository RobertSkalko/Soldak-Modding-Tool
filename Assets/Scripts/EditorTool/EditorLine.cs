using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SoldakModdingTool
{
    public class EditorLine : MonoBehaviour
    {
        public string Name;

        private string _key;
        public string Key { get => _key; set { _key = value; OnValueChanged.Invoke(); } }

        private string _value;
        public string Value { get => _value; set { _value = value; OnValueChanged.Invoke(); } }

        public UnityEvent OnValueChanged;

        public void Start()
        {
            OnValueChanged = new UnityEvent();

            OnValueChanged.AddListener(AddOverride);
        }

        public void AddOverride()
        {
            EditorGenerator.TryAddOverride(Name, Key, Value);

            Debug.Log("test working");
        }
    }
}