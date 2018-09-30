using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class InputButtons : MonoBehaviour
    {
        public void SetGamePathName(string s)
        {
            Save.Instance.GamePath = s;
        }

        public void SetFilesToEditPathName(string s)
        {
            Save.Instance.FilesToEditPath = s;
        }

        public void SetModName(string s)
        {
            Save.Instance.ModName = s;
        }

        public void SetOutputPath(string s)
        {
            Save.Instance.OutputPath = s;
        }

        public void SetInputCommand(string s)
        {
            Save.Instance.InputCommand = s;
        }
    }
}