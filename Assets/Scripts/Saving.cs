using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class Saving : MonoBehaviour
    {
        private void Start()
        {
            Save.TryLoadStateFromFile();
        }

        private void OnApplicationQuit()
        {
            Save.SaveStateToFile();
        }
    }
}