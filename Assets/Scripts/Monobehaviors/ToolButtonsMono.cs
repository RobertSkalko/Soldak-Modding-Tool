using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoldakModdingTool
{
    public class ToolButtonsMono : MonoBehaviour
    {
        public GameObject ButtonPrefab;

        public void Start()
        {
            foreach (var button in Main.Buttons) {
                GameObject obj = Instantiate(ButtonPrefab, this.transform);

                obj.GetComponent<Button>().onClick.AddListener(() => button.Action());

                obj.GetComponentInChildren<Text>().text = button.Name;
            }
        }
    }
}